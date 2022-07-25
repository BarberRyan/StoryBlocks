using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using SB = StoryBlocks.StoryBlocksMain;
using SBEH = StoryBlocks.SBEventHandler;
using SBL = StoryBlocks.SBLib;
using SBPH = StoryBlocks.SBPrefixHandler;
using SBERR = StoryBlocks.SBErrorHandler;
using EErrorCode = StoryBlocks.SBErrorHandler.EErrorCode;
using SBTH = StoryBlocks.SBTextHandler;
using SBFH = StoryBlocks.SBFileHandler;

namespace StoryBlocks
{
	public static class SBMenu
	{
		//Dictionary to hold items for the menu ([BLOCK TO LOAD ON USE], [DISPLAY TEXT]).
		static readonly Dictionary<string, string> Menu = new();
		
		//Initializing integer varaibles.
		static int activeOption = 1;
		static int fileCount;
	
		//Initializing string variables
		static string menuName = "";
		static List<string> menuData = new();

		static SBMenu()
		{
		}
		

		public static void AddMenuElement(string command, string text = "")
        {
			if(command == "S" && Menu.ContainsKey("S"))
            {
				Menu["S"] = $"{Menu["S"]}\n{text}";
            }
            else
            {
                if (!Menu.ContainsKey(command))
                {
				Menu.Add(command, text);
                }
            }
        }

		public static void CreateDialog(string dialogName, string? title = null, int xCoord = 0, int yCoord = 0, string textColor = "WHITE", string bkgColor = "BLACK")
        {
			
        } 


		//Processes the block data from the StoryBlocks dictionary to populate data needed for the menu screen.
		//BlockName: name of the block to process
		public static void CreateMenu(string BlockName, bool flag = true)
		{
			menuName = BlockName;
			Menu.Clear();
			
			//Splits the block data into individual lines to be added to "menuData" for processing.
			//If the block doesn't exist, show an error.
            if (SBL.StoryBlocks.ContainsKey(BlockName))
            {
				menuData = SBL.StoryBlocks[BlockName].Split("\n").ToList<string>();
			}
            else
            {
				SBErrorHandler.ThrowError((int)EErrorCode.noBlock, 0, BlockName);
				SB.LoadStory(SBFH.loadedStory);
			}
			
			//Loops through each line to find prefixes and act on them.
			foreach (string menuItem in menuData)
			{
				SBEH.PrefixOperation(menuItem, flag);
            }

			//Draws the inventory if there is anything in it, and if not on the main menu or about screens.
			//inventory use functions are not implemented yet.
			if(SBL.Inventory.Count > 0)
            {
				if(BlockName != "MAIN MENU" && BlockName != "ABOUT" && BlockName != "INVENTORY")
                {
					AddMenuElement("INVENTORY", "-Inventory-");
				}
            }

			string menuCommand = RunMenu();

			//Block load command that exits the app.
			if (menuCommand == "EXIT")
            {
				if(menuName == "MAIN MENU" && fileCount > 1)
                {
					FileMenu();
				}
				else if(menuName == "MAIN MENU" && fileCount == 1)
                {
					Environment.Exit(0);
                }
				
            }

			//Block load command that starts the first block of the story.
			else if(menuCommand == "START")
            {
				SBFH.StartStory();
            }

			//Block load command that loads the main menu (and clears variables).
			else if(menuCommand == "MAIN MENU")
			{
				SBFH.LoadConfig();
				SBFH.LoadBlock(menuCommand);
			}

			//Block load command that loads the last visited block in blockHistory.
			else if(menuCommand == "BACK")
			{
				GoBack();
			}

			//Block load command that completely reloads the story as if it was just opened. 
			else if(menuCommand == "RELOAD")
            {
				Console.Clear();
				Console.CursorVisible = false;
				SB.LoadStory(SBFH.loadedStory);
            }
			//Loads the block provided in the line.
            
			else if(menuCommand == "INVENTORY")
            {
				InventoryMenu();
            }

			else
            {
				SBFH.LoadBlock(menuCommand);
            }
		}

		//Finds the last block visited in blockHistory and goes to it, removing the current block from the history.
		public static void GoBack()
        {
			int lastIndex = SBL.BlockHistory.Count - 1;
			string lastBlock = "MAIN MENU";
			if(lastIndex >= 1)
            {
				lastBlock = SBL.BlockHistory[lastIndex - 1];
			}
			if(SBL.BlockHistory.Count > 0)
            {
				SBL.BlockHistory.RemoveAt(lastIndex);
			}
			
			if(lastIndex >= 1)
            {
				SBL.BlockHistory.RemoveAt(lastIndex - 1);
			}
            else
            {
				SBL.BlockHistory.Clear();
            }

			SBFH.LoadBlock(lastBlock, false);
		}

		public static void FileMenu()
		{
			Console.Title = "StoryBlocks";
			menuName = "FILE MENU";
			Console.Clear();
			SBL.ClearDicts();
			Menu.Clear();
			IEnumerable<string> files;
			files = System.IO.Directory.EnumerateFiles(System.IO.Directory.GetCurrentDirectory() + @"\Stories");
			fileCount = files.Count();
			if (fileCount > 1)
			{
				AddMenuElement("S", "            {██╗     ███████╗██████╗}[cyan]\n" +
									"            {╚██╗ ██╗██╔════╝██╔══██╗}[cyan]\n" +
									"            { ╚██╗╚═╝███████╗██████╔╝}[cyan]\n" +
									"            { ██╔╝██╗╚════██║██╔══██╗}[cyan]\n" +
									"            {██╔╝ ╚═╝███████║██████╔╝}[cyan]\n" +
									"            {╚═╝     ╚══════╝╚═════╝}[cyan]\n" +
									"Welcome to StoryBlocks! Please choose a story to load!");


				foreach (var item in files)
				{
					string name;
					string? desc;
					if (item.EndsWith(".txt"))
                    {
						name = item[(item.IndexOf(@"Stories\") + 8)..].Replace(".txt", "");

						if (name == "default")
						{
							continue;
						}
						desc = GetStoryDescription(item);
						AddMenuElement(name, $"{name} : {desc}");
					}
				}
				AddMenuElement("BLANK", "BLANK");
				AddMenuElement("RELOAD", "Reload the story list");
				AddMenuElement("EXIT", "Exit the program");
				
				string storyName = RunMenu();
				
				if(storyName == "EXIT")
                {
					Environment.Exit(0);
                }
				if(storyName == "RELOAD")
                {
					FileMenu();
                }

				SB.LoadStory(System.IO.Directory.GetCurrentDirectory() + @$"\Stories\{storyName}.txt");
			}
            else
            {
				SB.LoadStory(System.IO.Directory.GetCurrentDirectory() + @"\Stories\default.txt");
            }
		}

		public static void InventoryMenu()
        {
			menuName = "INVENTORY";
			Console.Clear();
			Menu.Clear();
			SBL.BlockHistory.Add("INVENTORY");
			AddMenuElement("S", "INVENTORY");
			
			foreach (var item in SBL.Inventory)
			{
				AddMenuElement(item.Key, $"{item.Key} : {item.Value}");
            }

			AddMenuElement("BLANK", "BLANK");
			AddMenuElement("BACK", "Go back");

			string itemSelected = RunMenu();

			if(itemSelected == "BACK")
            {
				GoBack();
            }


        }

		static string GetStoryDescription(string filePath)
        {
			string? line;
			StreamReader reader = File.OpenText(filePath);
			while((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("D:"))
                {
					reader.Close();
					return line[2..];
                }
            }
			reader.Close();
			return "DESCRIPTION NOT FOUND";
		}

		//Takes the data in the Menu dictionary and uses it to draw the current menu on the screen.
		public static void DrawMenu()
		{
			//Draw story (text at the top of the menu).
			if (Menu.ContainsKey("S"))
			{
				Console.ResetColor();
                SBTextHandler.PrintText(SBEH.ReplaceVariable(Menu["S"]) + "\n\n");
			}
			//iterates through each choice line to provide menu choices, and updates cursor position.
			for (int i = 1; i < Menu.Count; i++)
			{
                if(Menu.ElementAt(i).Value == "BLANK")
                {
					Console.WriteLine();
					continue;
                }

				if (activeOption == i)
				{
					SBTH.PrintText(SBEH.ReplaceVariable($">> {Menu.ElementAt(i).Value} <<"));
				}
				else
				{
					SBTH.PrintText(SBEH.ReplaceVariable($"   {Menu.ElementAt(i).Value}   "));
				}

				Console.ResetColor();
			}

			//Prints visible variables to the screen.
			if (!(menuName == "MAIN MENU") && !(menuName == "ABOUT") && !(menuName == "FILE MENU") && !(menuName == "INVENTORY"))
            {
				Console.WriteLine("\n\n");
				foreach (var item in SBL.StringDict)
				{
                    if (item.Value.Item2)
                    {
						SBTH.PrintText($"{item.Key}:{item.Value.Item1}");
					}
				}
				foreach (var item in SBL.IntDict)
				{
                    if (item.Value.Item2)
                    {
						SBTH.PrintText($"{item.Key}:{item.Value.Item1}");
					}
				}
				//Draws the inventory screen at the bottom of the menu.
				SBInventoryHandler.DrawInventory();
			}
		}
		

		//handles keyboard input for choice selection, and escape to exit the program.
		//To Do: Move to event handler class, add more input options.
		public static string RunMenu()
        {
			SBEH.CheckGlobalConditionals();
			activeOption = 1;
			ConsoleKey pressedKey;
			do
			{
				Console.SetCursorPosition(0, 0);
				DrawMenu();
				pressedKey = Console.ReadKey(true).Key;
				if(pressedKey == ConsoleKey.DownArrow)
                {
					if(activeOption < Menu.Count - 1)
                    {
						activeOption++;
						while (Menu.ElementAt(activeOption).Value == "BLANK")
                        {
							activeOption++;
                        }
					}
					else if (activeOption == Menu.Count - 1)
					{
						activeOption = 1;
					}

				}
				else if (pressedKey == ConsoleKey.UpArrow)
				{
					if (activeOption > 1)
					{
						activeOption--;
						while (Menu.ElementAt(activeOption).Value == "BLANK")
						{
							activeOption--;
						}
					}
					else if (activeOption == 1)
                    {
						activeOption = Menu.Count - 1;
                    }
				}
				else if (pressedKey == ConsoleKey.Escape)
                {
                    if(menuName == "MAIN MENU" && fileCount > 1)
                    {
						FileMenu();
                    }
					else if(menuName == "MAIN MENU" && fileCount == 1)
					{
						Environment.Exit(0);
                    }
					else if(menuName == "FILE MENU")
                    {
						Environment.Exit(0);
                    }
                    else
                    {
						SBFH.LoadBlock("MAIN MENU");
                    }
                }
			} while (pressedKey != ConsoleKey.Enter);
			if (Menu.Count - 1 >= activeOption)
			{
				return Menu.ElementAt(activeOption).Key;
			}
            else
            {
				return "MAIN MENU";
            }
		}
	}
}
