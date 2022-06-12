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
namespace StoryBlocks
{
	public static class SBMenu
	{
		//Dictionary to hold items for the menu ([BLOCK TO LOAD ON USE], ([ITEM NAME], [FOREGROUND COLOR], [BACKGROUND COLOR)).
		public static Dictionary<string, (string, ConsoleColor, ConsoleColor)> Menu = new Dictionary<string, (string, ConsoleColor, ConsoleColor)>();
		
		//Initializing integer varaibles.
		static int activeOption = 1;
		static int fileCount;
	
		//Initializing string variables
		static string menuName = "";
		static string[] menuData = {""};
		static SBMenu()
		{
		}
		//Takes in a string and returns a corresponding ConsoleColor.
		//input: string argument for color provided at the end of a line (":[COLOR NAME]")
		//text: true = foreground color, false = background color.
		public static ConsoleColor getColor(string input, bool text)
        {
			switch (input.ToUpper())
            {
				case "BLACK":
					return ConsoleColor.Black;

				case "BLUE":
					return ConsoleColor.Blue;

				case "CYAN":
				case "LIGHT BLUE":
				case "LIGHTBLUE":
					return ConsoleColor.Cyan;

				case "DARK BLUE":
				case "DARKBLUE":
					return ConsoleColor.DarkBlue;

				case "DARK CYAN":
				case "DARKCYAN":
					return ConsoleColor.DarkCyan;

				case "DARK GREY":
				case "DARKGREY":
				case "DARK GRAY":
				case "DARKGRAY":
					return ConsoleColor.DarkGray;


				case "DARK GREEN":
				case "DARKGREEN":
					return ConsoleColor.DarkGreen;

				case "DARK MAGENTA":
				case "DARKMAGENTA":
				case "PURPLE":
					return ConsoleColor.DarkMagenta;

				case "DARK RED":
				case "DARKRED":
					return ConsoleColor.DarkRed;

				case "DARK YELLOW":
				case "DARKTYELLOW":
				case "GOLD":
					return ConsoleColor.DarkYellow;

				case "GRAY":
				case "GREY":
					return ConsoleColor.Gray;

				case "GREEN":
					return ConsoleColor.Green;

				case "MAGENTA":
				case "PINK":
					return ConsoleColor.Magenta;

				case "RED":
					return ConsoleColor.Red;

				case "WHITE":
					return ConsoleColor.White;

				case "YELLOW":
					return ConsoleColor.Yellow;

				default:
                    if (text)
                    {
						return ConsoleColor.White;
                    }
                    else
                    {
						return ConsoleColor.Black;
                    }
			}
        }

		public static void addMenuElement(string command, string text, string textColor = "WHITE", string bkgColor = "BLACK")
        {
			Menu.Add(command, (text, getColor(textColor, true), getColor(bkgColor, false)));
        }

		//Adds a new line of text to a menu option (currently only used for "S:" lines).
		//command: Key to update in Menu dictionary
		//text: Text to add to the option on a new line
		public static void concatMenuElement(string command, string text)
        {
            Menu[command] = (Menu[command].Item1 + "\n" + text, Menu[command].Item2, Menu[command].Item3);
        }

		//Adds a new line of text to a menu option and changes the color options (currently only used for "S:" lines).
		//command: Key to update in Menu dictionary
		//text: Text to add to the option on a new line
		//textColor: ConsoleColor value for the foreground color
		//bkgColo: ConsoleColor value for the background color
		public static void concatMenuElement(string command, string text, string textColor, string bkgColor)
        {
			Menu[command] = (Menu[command].Item1 + "\n" + text, getColor(textColor, true), getColor(bkgColor, false));
		}

		
		//Processes the block data from the StoryBlocks dictionary to populate data needed for the menu screen.
		//BlockName: name of the block to process
		public static void CreateMenu(string BlockName)
		{
			menuName = BlockName;
			Menu.Clear();
			
			//Splits the block data into individual lines to be added to "menuData" for processing.
			//If the block doesn't exist, show an error.
            if (SBL.storyBlocks.ContainsKey(BlockName))
            {
				menuData = SBL.storyBlocks[BlockName].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
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
			if(SBL.inventory.Count > 0)
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
			int lastIndex = SBL.blockHistory.Count - 1;
			string lastBlock = "MAIN MENU";
			if(lastIndex >= 1)
            {
				lastBlock = SBL.blockHistory[lastIndex - 1];
			}
			if(SBL.blockHistory.Count() > 0)
            {
				SBL.blockHistory.RemoveAt(lastIndex);
			}
			
			if(lastIndex >= 1)
            {
				SBL.blockHistory.RemoveAt(lastIndex - 1);
			}
            else
            {
				SBL.blockHistory.Clear();
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
				addMenuElement("S", "            ██╗     ███████╗██████╗\n" +
									"            ╚██╗ ██╗██╔════╝██╔══██╗\n" +
									"             ╚██╗╚═╝███████╗██████╔╝\n" +
									"             ██╔╝██╗╚════██║██╔══██╗\n" +
									"            ██╔╝ ╚═╝███████║██████╔╝\n" +
									"            ╚═╝     ╚══════╝╚═════╝\n" +
									"Welcome to StoryBlocks! Please choose a story to load!", "CYAN");


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
				addMenuElement("EXIT", "Exit the program", "RED");
				
				string storyName = RunMenu();
				
				if(storyName == "EXIT")
                {
					Environment.Exit(0);
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
				Console.ForegroundColor = Menu["S"].Item2;
				Console.BackgroundColor = Menu["S"].Item3;
				Console.WriteLine(SBEH.replaceVariable(Menu["S"].Item1) + "\n\n");
				Console.ResetColor();
			}
			//iterates through each choice line to provide menu choices, and updates cursor position.
			for (int i = 1; i < Menu.Count; i++)
			{
				Console.ForegroundColor = Menu.ElementAt(i).Value.Item2;
				Console.BackgroundColor = Menu.ElementAt(i).Value.Item3;

				if (activeOption == i)
				{
					Console.WriteLine(SBEH.replaceVariable($">> {Menu.ElementAt(i).Value.Item1} <<"));
				}
				else
				{
					Console.WriteLine(SBEH.replaceVariable($"   {Menu.ElementAt(i).Value.Item1}   "));
				}

				Console.ResetColor();
			}

			//Prints visible variables to the screen.
            if (!(menuName == "MAIN MENU") && !(menuName == "ABOUT"))
            {
				Console.WriteLine("\n\n");
				foreach (var item in SBL.stringDict)
				{
                    if (item.Value.Item2)
                    {
						Console.WriteLine($"{item.Key}:{item.Value.Item1}");
					}
				}
				foreach (var item in SBL.intDict)
				{
                    if (item.Value.Item2)
                    {
						Console.WriteLine($"{item.Key}:{item.Value.Item1}");
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
						activeOption += 1;
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
						activeOption -= 1;
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
