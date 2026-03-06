using System;
using System.Collections.Generic;

namespace Exercise06
{
    public class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public Character(string name, int level, int health, int maxHealth)
        {
            Name = name;
            Level = level;
            Health = health;
            MaxHealth = maxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
            Console.WriteLine($"{Name} takes {damage} damage. Health: {Health}/{MaxHealth}");
        }

        public void Heal(int amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
            Console.WriteLine($"{Name} heals {amount} points. Health: {Health}/{MaxHealth}");
        }

        public virtual void LevelUp()
        {
            Level++;
            MaxHealth += 10;
            Health = MaxHealth;  // Fully restore health on level up
            Console.WriteLine($"{Name} Levels up! New level: {Level}. MaxHealth: {MaxHealth}");
        }
    }

    public class PlayableCharacter : Character
    {
        public int Experience { get; set; }
        public int ExperienceToNextLevel { get; set; }

        public PlayableCharacter(string name, int level, int health) : base(name, level, health, 100)
        {
            Experience = 0;
            ExperienceToNextLevel = 100;
        }

        public void GainExperience(int exp)
        {
            Experience += exp;
            Console.WriteLine($"{Name} gains {exp} Experience points.");
            while (Experience >= ExperienceToNextLevel) // Level up multiple times if needed
            {
                LevelUp();
                Experience -= ExperienceToNextLevel;
            }
        }
    }

    public class Warrior : PlayableCharacter
    {
        public int Strength { get; set; }
        public int Armor { get; set; }

        public Warrior(string name) : base(name, 1, 100)
        {
            Strength = 10;
            Armor = 5;
        }

        public void Attack(Character target)
        {
            int damage = Strength;
            Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage.");
            target.TakeDamage(damage);
        }

        public void Defend()
        {
            Armor += 5;
            Console.WriteLine($"{Name} defends. Armor: {Armor}");
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Strength += 5;
            Armor += 3;
            Console.WriteLine($"{Name} levels up as Warrior! Strength: {Strength}, Armor: {Armor}");
        }
    }

    public class Mage : PlayableCharacter
    {
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public int SpellPower { get; set; }

        public Mage(string name) : base(name, 1, 100)
        {
            Mana = 50;
            MaxMana = 50;
            SpellPower = 15;
        }

        public void CastSpell(Character target)
        {
            if (Mana >= 10)
            {
                Mana -= 10;
                int damage = SpellPower;
                Console.WriteLine($"{Name} casts a spell on {target.Name} for {damage} damage.");
                target.TakeDamage(damage);
            }
            else
            {
                Console.WriteLine($"{Name} doesn't have enough mana to cast a spell!");
            }
        }

        public void RestoreMana(int amount)
        {
            Mana += amount;
            if (Mana > MaxMana) Mana = MaxMana;
            Console.WriteLine($"{Name} restores {amount} mana. Mana: {Mana}/{MaxMana}");
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Mana += 20;
            SpellPower += 5;
            Console.WriteLine($"{Name} levels up as Mage! Mana: {Mana}, SpellPower: {SpellPower}");
        }
    }

    public class Paladin : Warrior
    {
        public int Faith { get; set; }

        public Paladin(string name) : base(name)
        {
            Faith = 5;
        }

        public void HolyStrike(Character target)
        {
            int damage = Strength + Faith;
            Console.WriteLine($"{Name} performs a Holy Strike on {target.Name} for {damage} damage.");
            target.TakeDamage(damage);
        }

        public void Heal()
        {
            int healingAmount = Faith * 5;
            Heal(healingAmount);
            Console.WriteLine($"{Name} uses Faith to heal {healingAmount} health.");
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Faith += 3; // Faith increases on level up
            Console.WriteLine($"{Name} levels up as Paladin! Faith: {Faith}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Create 3 characters: Warrior, Mage, and Paladin
            var warrior = new Warrior("Thor");
            var mage = new Mage("Gandalf");
            var paladin = new Paladin("Arthur");

            List<Character> characters = new List<Character> { warrior, mage, paladin };

            Console.WriteLine("Character stats: ");
            foreach (var character in characters)
            {
                Console.WriteLine($"{character.Name}: Level {character.Level}, Health {character.Health}/{character.MaxHealth}");
            }

            // Simulate battle and experience gain
            warrior.Attack(mage);  // Warrior attacks Mage
            mage.CastSpell(warrior);  // Mage attacks Warrior
            paladin.HolyStrike(mage);  // Paladin attacks Mage

            // Gain experience after battle
            warrior.GainExperience(150);
            mage.GainExperience(80);
            paladin.GainExperience(120);

            // Display updated stats after battle and experience gain
            Console.WriteLine("\nUpdated Character Stats:");
            foreach (var character in characters)
            {
                Console.WriteLine($"{character.Name}: Level {character.Level}, Health {character.Health}/{character.MaxHealth}");
            }
        }
    }
}