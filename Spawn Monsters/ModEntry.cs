using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Spawn_Monsters.Monsters;
using Spawn_Monsters.MonsterSpawning;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Spawn_Monsters
{
    /// <summary>Represents the mod entry point.</summary>
    public class ModEntry : Mod
    {

        /*********
		** Properties
		*********/
        /// <summary>The mod configuration from the player.</summary>
        public ModConfig config;

        /*********
        ** Public methods
        *********/
        public override void Entry(IModHelper helper) {
            helper.ConsoleCommands.Add("monster_spawn", "Spawns a Monster.", SpawnEntity);
            helper.ConsoleCommands.Add("monster_list", "Shows a lists of all monsters available to spawn.", MonsterList);
            helper.ConsoleCommands.Add("monster_menu", "Shows a menu for spawning monsters", MonsterMenu);
            helper.ConsoleCommands.Add("farmer_position", "Prints the Farmer's current position", FarmerPosition);

            config = helper.ReadConfig<ModConfig>();

            Spawner.GetInstance().RegisterMonitor(Monitor);

            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.Saving += OnSaveCreating;
        }

        public void OnSaveCreating(object sender, SavingEventArgs e) {
            Spawner.GetInstance().KillEverything();
        }

        /*********
        ** Input Methods
        *********/
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e) {
            if (!Context.IsPlayerFree) {
                return;
            }

            if (e.Button == config.MenuKey) {
                Game1.activeClickableMenu = new MonsterMenu.MonsterMenu();
            }
        }

        /*********
        ** Command Methods
        *********/
        public void SpawnEntity(string command, string[] args) {


            if (args.Length == 0 || args[0].Equals("help")) {
                Monitor.Log($"Usage: monster_spawn \"Monster Name\" [posX] [posY] [amount]" +
                    $"\n\nUses Farmer's coordinates if none or '{config.FarmerPositionCharacter}' was given." +
                    $"\n\nExample: monster_spawn \"Green Slime\" 32 23 4" +
                    $"\nspawns four Green Slimes at coordinates 32|23" +
                    $"\nuse monster_list for a list of available monster names.", LogLevel.Info);
                return;
            }

            //We need a world to spawn monsters in, duh
            if (Context.IsWorldReady) {

                if (args.Length == 0) {
                    Monitor.Log("You need to provide at least a Monster name!", LogLevel.Info);
                    return;
                }
                MonsterData.Monster m = MonsterData.ForName(args[0]);
                Vector2 location = Game1.player.getTileLocation();

                if (m.Equals((MonsterData.Monster)(-1))) {
                    Monitor.Log($"There is no Monster with the name {args[0]}", LogLevel.Info);
                    return;
                }


                int amount = 1;

                try {
                    //Determine X tile
                    if (args.Length >= 2) {
                        location.X = int.Parse(args[1].Replace(config.FarmerPositionCharacter.ToString(), location.X.ToString()));
                    }

                    //Determine Y tile
                    if (args.Length >= 3) {
                        location.Y = int.Parse(args[2].Replace(config.FarmerPositionCharacter.ToString(), location.Y.ToString()));
                    }

                    if (args.Length >= 4) {
                        amount = int.Parse(args[3]);
                    }

                } catch (Exception e) {
                    Console.Error.WriteLine(e.Message);
                    Monitor.Log("Invalid Arguments! Type \"monster_spawn help\" for usage help.", LogLevel.Info);
                    return;
                }

                if (m != MonsterData.Monster.Duggy && m != MonsterData.Monster.WildernessGolem) {
                    location.X *= Game1.tileSize;
                    location.Y *= Game1.tileSize;
                }

                Spawner.GetInstance().SpawnMonster(m, location, amount);

            } else { Monitor.Log("Load a save first!"); }
        }

        public void MonsterList(string command, string[] args)
        {
            var monstersDictionary = new Dictionary<string, string[]>() {
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

            Monitor.Log(
                "Monsters available to spawn:\n\n" +

                String.Join(
                    "\n\n",
                    monstersDictionary.Select(kvp =>
                        $"{kvp.Key}:\n\t" +
                        $"{String.Join("\n\t", kvp.Value)}"
                    )
                ) +

                "\n\nUse these names with 'monster_spawn'.\n" +
                "Keep in mind that some monsters don't work properly outside of the farm and the mines!\n"
            , LogLevel.Info);
        }

        public void MonsterMenu(string command, string[] args) {
            if (Context.IsWorldReady) {
                Game1.activeClickableMenu = new MonsterMenu.MonsterMenu();
            } else {
                Monitor.Log("Load a save first!", LogLevel.Info);
            }
        }

        public void FarmerPosition(string command, string[] args) {
            Monitor.Log("The Farmer's coordinates are: " + Game1.player.getTileLocation(), LogLevel.Info);
        }
    }
}
