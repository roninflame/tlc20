using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolloScripts
{
    [System.Serializable]
    public class StatSystem
    {
        public enum DamageType
        {
            Energy,
            Physical,
            Fire,
            Cold,
            Electric
        }
        [System.Serializable]
        public class Stats
        {
            //Integer for simplicity, may switch to float later on. For now everything is integer
            public int health;
            public int strength;
            public int defense;
            public int agility;

            //use an array indexed by the DamageType enum for easy extensibility
            public int[] elementalProtection = new int[Enum.GetValues(typeof(DamageType)).Length];
            public int[] elementalBoosts = new int[Enum.GetValues(typeof(DamageType)).Length];

            public void Copy(Stats other)
            {
                health = other.health;
                strength = other.strength;
                defense = other.defense;
                agility = other.agility;

                Array.Copy(other.elementalProtection, elementalProtection, other.elementalProtection.Length);
                Array.Copy(other.elementalBoosts, elementalBoosts, other.elementalBoosts.Length);
            }

            /// <summary>
            /// Will modify that Stat by the given StatModifier (see StatModifier documentation for how to use them)
            /// </summary>
            /// <param name="modifier"></param>
            public void Modify(StatModifier modifier)
            {
                //bit convoluted, but allow to reuse the normal int stat system for percentage change
                if (modifier.ModifierMode == StatModifier.Mode.Percentage)
                {
                    health += Mathf.FloorToInt(health * (modifier.Stats.health / 100.0f));
                    strength += Mathf.FloorToInt(strength * (modifier.Stats.strength / 100.0f));
                    defense += Mathf.FloorToInt(defense * (modifier.Stats.defense / 100.0f));
                    agility += Mathf.FloorToInt(agility * (modifier.Stats.agility / 100.0f));

                    for (int i = 0; i < elementalProtection.Length; ++i)
                        elementalProtection[i] += Mathf.FloorToInt(elementalProtection[i] * (modifier.Stats.elementalProtection[i] / 100.0f));

                    for (int i = 0; i < elementalBoosts.Length; ++i)
                        elementalBoosts[i] += Mathf.FloorToInt(elementalBoosts[i] * (modifier.Stats.elementalBoosts[i] / 100.0f));
                }
                else
                {
                    health += modifier.Stats.health;
                    strength += modifier.Stats.strength;
                    defense += modifier.Stats.defense;
                    agility += modifier.Stats.agility;

                    for (int i = 0; i < elementalProtection.Length; ++i)
                        elementalProtection[i] += modifier.Stats.elementalProtection[i];

                    for (int i = 0; i < elementalBoosts.Length; ++i)
                        elementalBoosts[i] += modifier.Stats.elementalBoosts[i];
                }
            }
            [System.Serializable]
            public class StatModifier
            {
                /// <summary>
                /// The mode of the modifier : Percentage will divide the value by 100 to get a percentage, absolute use the
                /// value as is.
                /// </summary>
                public enum Mode
                {
                    Percentage,
                    Absolute
                }

                public Mode ModifierMode = Mode.Absolute;
                public Stats Stats = new Stats();
            }

            /// <summary>
            /// This is a special StatModifier, that gets added to the TimedStatModifier stack, that will be automatically
            /// removed when its timer reaches 0. Contains a StatModifier that controls the actual modification.
            /// </summary>
            [System.Serializable]
            public class TimedStatModifier
            {
                public string Id;
                public StatModifier Modifier;

                public Sprite EffectSprite;

                public float Duration;
                public float Timer;

                public void Reset()
                {
                    Timer = Duration;
                }
            }

            public Stats baseStats;
            public Stats stats { get; set; } = new Stats();
            public int CurrentHealth { get; private set; }
            //public List<BaseElementalEffect> ElementalEffects => m_ElementalEffects;
            //public List<TimedStatModifier> TimedModifierStack => m_TimedModifierStack;


        }
    }

}
