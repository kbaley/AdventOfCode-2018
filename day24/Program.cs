using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("./input.txt").Skip(1);
            var boost = 34;

            var units = new List<Unit>();
            var immuneCrew = true;
            foreach (var input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input) || input.StartsWith("Infection")) {
                    immuneCrew = false;
                } else {
                    var unit = new Unit(input, immuneCrew);
                    if (immuneCrew) unit.Damage += boost;
                    units.Add(unit);
                }
            }

            var i = 1;
            while ( true ) {
                System.Console.WriteLine("Round " + i++);
                // if (i == 3) break;
                units.ForEach(u => { u.Defender = null; u.Attacker = null; });
                var attackers = units.OrderByDescending(u => u.EffectivePower).ThenByDescending(u => u.Initiative); 
                foreach (var item in attackers)
                {
                    var enemy = units
                        .Where(u => u.Type != item.Type 
                            && !u.Immunities.Contains(item.AttackType)
                            && u.Attacker == null)
                        .OrderByDescending(u => u.Weaknesses.Contains(item.AttackType) ? 1 : 0)
                        .ThenByDescending(u => u.EffectivePower)
                        .ThenByDescending(u => u.Initiative)
                        .FirstOrDefault();
                    if (enemy != null) {
                        item.Defender = enemy;
                        enemy.Attacker = item;
                    }
                    System.Console.WriteLine(item);
                }

                attackers = units.OrderByDescending(u => u.Initiative);
                foreach (var item in attackers)
                {
                    if (item.Defender != null) {
                        var hp = item.EffectivePower;
                        if (item.Defender.Weaknesses.Contains(item.AttackType)) hp *= 2;
                        var unitsKilled = hp / item.Defender.HP;
                        item.Defender.NumberOfUnits -= unitsKilled;
                    }
                }
                units.RemoveAll(u => u.NumberOfUnits <= 0);
                if (units.All(u => u.Type == UnitType.IMMUNE) || units.All(u => u.Type == UnitType.INFECTION)) break;
            }


            if (units.All(u => u.Type == UnitType.IMMUNE)) {
                System.Console.WriteLine("Immune won with: " + units.Sum(u => u.NumberOfUnits));
            }

            if (units.All(u => u.Type == UnitType.INFECTION)) {
                System.Console.WriteLine("Infection won with: " + units.Sum(u => u.NumberOfUnits));
            }

            Console.WriteLine("Done");
        }
    }

    public class Unit {
        public int HP { get; set; }
        public List<AttackType> Immunities {get; set; }
        public List<AttackType> Weaknesses { get; set; }
        public int Damage { get; set; }
        public AttackType AttackType { get; set; }
        public int NumberOfUnits { get; set; }
        public int Initiative { get; set; }
        public UnitType Type { get; set; }
        public Unit Defender { get; set; }
        public Unit Attacker { get; set; }

        public int EffectivePower { get { return NumberOfUnits * Damage; } }

        public override string ToString() {
            var result = $"Unit {Type} with {NumberOfUnits} units and initiative {Initiative} ";
            if (Defender == null) {
                result += "will attack no one";
            } else {
                result += $"will attack unit {Defender.Type} with {Defender.NumberOfUnits} and initiative {Defender.Initiative}";
            }

            return result;
        }


        public Unit(string input, bool isImmune) {
            Type = isImmune ? UnitType.IMMUNE : UnitType.INFECTION;
            var regex = new Regex(@"(\d+) units each with (\d+) hit points (\([^)]*\) )?with an attack that does (\d+) (\w+) damage at initiative (\d+)");
            var groups = regex.Match(input).Groups;
            NumberOfUnits = int.Parse(groups[1].Value);
            HP = int.Parse(groups[2].Value);
            Damage = int.Parse(groups[4].Value);
            AttackType = (AttackType)Enum.Parse(typeof(AttackType), groups[5].Value.ToUpper());
            Initiative = int.Parse(groups[6].Value);

            Immunities = new List<AttackType>();
            Weaknesses = new List<AttackType>();            
            var modifiers = groups[3].Value.Replace("(", "").Replace(")", "").Trim();
            if (!string.IsNullOrWhiteSpace(modifiers)) {
                var pieces = modifiers.Split("; ");
                var immunities = "";
                var weaknesses = "";
                if (pieces[0].StartsWith("immune to")) {
                    immunities = pieces[0].Substring("immune to ".Length);
                    if (pieces.Length > 1) {
                        weaknesses = pieces[1].Substring("weak to".Length);
                    }
                } else {
                     weaknesses = pieces[0].Substring("weak to".Length);
                    if (pieces.Length > 1) {
                        immunities = pieces[1].Substring("immune to ".Length);
                    }
                }
                foreach (var item in immunities.Split(", "))
                {
                    if (item != string.Empty)
                        Immunities.Add((AttackType)Enum.Parse(typeof(AttackType), item.ToUpper()));
                }
                foreach (var item in weaknesses.Split(", "))
                {
                    if (item != string.Empty)
                        Weaknesses.Add((AttackType)Enum.Parse(typeof(AttackType), item.ToUpper()));
                }
            }
        }
    }

    public enum AttackType {
        BLUDGEONING,
        FIRE,
        SLASHING,
        RADIATION,
        COLD
    }

    public enum UnitType {
        IMMUNE,
        INFECTION
    }
}
