using System;
using System.Collections.Generic;
using System.Linq;

namespace Spawn_Monsters.Monsters
{
    public class MonsterList
    {
        private Dictionary<string, string[]> monstersDictionary = new Dictionary<string, string[]>() {
                {"Slimes",
                new string[] {
                    "Green Slime",
                    "Frost Jelly",
                    "Red Sludge",
                    "Purple Sludge",
                    "Yellow Slime",
                    "Black Slime",
                    "Gray Sludge",
                    "BigSlime"
                }},
                {"Bats",
                new string[] {
                    "Bat",
                    "Frost Bat",
                    "Lava Bat",
                    "Iridium Bat"
                }},
                {"Bugs",
                new string[] {
                    "Bug",
                    "Armored Bug"
                }},
                {"Flies",
                new string[] {
                    "Cave Fly",
                    "Grub",
                    "Mutant Fly",
                    "Mutant Grub"
                }},
                {"Ghosts",
                new string[] {
                    "Ghosts",
                    "Carbon Ghost",
                    "PutridGhost"
                }},
                {"Crabs",
                new string[] {
                    "Rock Crab",
                    "Lava Crab",
                    "Iridium Crab"
                }},
                {"Golems",
                new string[] {
                    "Rock Golem",
                    "Wilderness Golem"
                }},
                {"Serpents",
                new string[] {
                    "RoyalSerpent",
                    "Serpent"
                }},
                {"Shadows",
                new string[] {
                    "Shadow Brute",
                    "Shadow Shaman",
                }},
                {"Magma Sprites",
                new string[] {
                    "MagmaSprite",
                    "MagmaSparker"
                }},
                {"Squids",
                new string[] {
                    "BlueSquid",
                    "Squid Kid"
                }},
                {"Skeletons",
                new string[] {
                    "Skeleton",
                    "SkeletonMage"
                }},
                {"Other",
                new string[] {
                    "Cursed Doll",
                    "Duggy",
                    "Dust Sprite",
                    "DwarvishSentry",
                    "Haunted Skull",
                    "Hot Head",
                    "Lava Lurk",
                    "Metal Head",
                    "Mummy",
                    "Pepper Rex",
                    "Shooter",
                    "Spider"
                }}
            };

        public override string ToString()
        {
            return String.Join(
               "\n\n",
               monstersDictionary.Select(kvp =>
                   $"{kvp.Key}:\n\t" +
                   $"{String.Join("\n\t", kvp.Value)}"
               )
            );
        }
    }
}
