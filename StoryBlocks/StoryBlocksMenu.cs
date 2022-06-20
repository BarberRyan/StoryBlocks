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

namespace StoryBlocks
{
	public static class SBMenu
	{
		//Dictionary to hold items for the menu ([BLOCK TO LOAD ON USE], [DISPLAY TEXT]).
		public static Dictionary<string, string> Menu = new();
		
		//Initializing integer varaibles.
		static int activeOption = 1;
		static int fileCount;
	
		//Initializing string variables
		static string menuName = "";
		static List<string> menuData = new List<string>();

		static SBMenu()
		{
		}
		

		public static void addMenuElement(string command, string text = "")
        {
			if(command == "S" && Menu.ContainsKey("S"))
            {
				Menu["S"] = $"{Menu["S"]}\n{text}";
            }
            else
            {
				Menu.Add(command, text);
            }
        }

		//Adds a new line of text to a menu option (currently only used for "S:" lines).
		//command: Key to update in Menu dictionary
		//text: Text to add to the option on a new line
		public static void concatMenuElement(string command, string text)
        {
            Menu[command] = (Menu[command] + "\n" + text);
        }

		//Adds a new line of text to a menu option and changes the color options (currently only used for "S:" lines).
		//command: Key to update in Menu dictionary
		//text: Text to add to the option on a new line
		//textColor: ConsoleColor value for the foreground color
		//bkgColo: ConsoleColor value for the background color
		public static void concatMenuElement(string command, string text, string textColor, string bkgColor)
        {
			Menu[command] = (Menu[command] + "\n" + text);
		}

		
		//Processes the block data from the StoryBlocks dictionary to populate data needed for the menu screen.
		//BlockName: name of the block to process
		public static void CreateMenu(string BlockName)
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
				SBErrorHandler.ThrowError((int)EErrorCode.noBlock, BlockName);
				SB.LoadStory(SBL.loadedStory);
			}
			
			//Loops through each line to find prefixes and act on them.
			foreach (string menuItem in menuData)
			{
				SBEH.PrefixOperation(menuItem);
            }

			//Draws the inventory if there is anything in it, and if not on the main menu or about screens.
			//inventory use functions are not implemented yet.
			if(SBL.Inventory.Count > 0)
            {
				if(BlockName != "MAIN MENU" && BlockName != "ABOUT")
                {
					addMenuElement("INVENTORY", "-Inventory-");
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
				SBL.StartStory();
            }

			//Block load command that loads the main menu (and clears variables).
			else if(menuCommand == "MAIN MENU")
			{
				SBL.LoadConfig();
				SBL.LoadBlock(menuCommand);
			}

			//Block load command that loads the last visited block in blockHistory.
			else if(menuCommand == "BACK")
			{
				goBack();
			}

			//Block load command that completely reloads the story as if it was just opened. 
			else if(menuCommand == "RELOAD")
            {
				Console.Clear();
				Console.CursorVisible = false;
				SB.LoadStory(SBL.loadedStory);
            }
			//Loads the block provided in the line.
            else
            {
				SBL.LoadBlock(menuCommand);
            }
		}

		//Finds the last block visited in blockHistory and goes to it, removing the current block from the history.
		public static void goBack()
        {
			int lastIndex = SBL.BlockHistory.Count - 1;
			string lastBlock = "MAIN MENU";
			if(lastIndex >= 1)
            {
				lastBlock = SBL.BlockHistory[lastIndex - 1];
			}
			if(SBL.BlockHistory.Count() > 0)
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
			
			SBL.LoadBlock(lastBlock);
		}

		public static void FileMenu()
		{
			Console.Title = "StoryBlocks";
			menuName = "FILE MENU";
			Console.Clear();
			Menu.Clear();
			IEnumerable<string> files = System.IO.Directory.EnumerateFiles(System.IO.Directory.GetCurrentDirectory() + @"\Stories");
			fileCount = files.Count();
			if (fileCount > 1)
			{
				addMenuElement("S", "            {██╗     ███████╗██████╗}[cyan]\n" +
									"            {╚██╗ ██╗██╔════╝██╔══██╗}[cyan]\n" +
									"            { ╚██╗╚═╝███████╗██████╔╝}[cyan]\n" +
									"            { ██╔╝██╗╚════██║██╔══██╗}[cyan]\n" +
									"            {██╔╝ ╚═╝███████║██████╔╝}[cyan]\n" +
									"            {╚═╝     ╚══════╝╚═════╝}[cyan]\n" +
									"Welcome to StoryBlocks! Please choose a story to load!");


				foreach (var item in files)
				{
					string name = "";
					string? desc = "";
					if (item.EndsWith(".txt"))
                    {
						name = item.Substring(item.IndexOf(@"Stories\") + 8).Replace(".txt", "");

						if (name == "default")
						{
							continue;
						}
						desc = getDescription(item);
						addMenuElement(name, $"{name} : {desc}");
					}
				}
				addMenuElement("BLANK", "BLANK");
				addMenuElement("RELOAD", "Reload the story list");
				addMenuElement("EXIT", "Exit the program");
				
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

		static string getDescription(string filePath)
        {
			string? line;
			StreamReader reader = File.OpenText(filePath);
			while((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("D:"))
                {
					reader.Close();
					return line.Substring(2);
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
                SBTextHandler.PrintText(SBEH.replaceVariable(Menu["S"]) + "\n\n");
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
					SBTH.PrintText(SBEH.replaceVariable($">> {Menu.ElementAt(i).Value} <<"));
				}
				else
				{
					SBTH.PrintText(SBEH.replaceVariable($"   {Menu.ElementAt(i).Value}   "));
				}

				Console.ResetColor();
			}

			//Prints visible variables to the screen.
			if (!(menuName == "MAIN MENU") && !(menuName == "ABOUT") && !(menuName == "FILE MENU"))
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
						SBL.LoadBlock("MAIN MENU");
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
