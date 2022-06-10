using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using SB = StoryBlocks.StoryBlocksMain;
using SBEH = StoryBlocks.SBEventHandler;
using SBL = StoryBlocks.SBLib;
using SBPH = StoryBlocks.SBPrefixHandler;
namespace StoryBlocks
{
	public static class SBMenu
	{
		public static Dictionary<string, (string, ConsoleColor, ConsoleColor)> Menu = new Dictionary<string, (string, ConsoleColor, ConsoleColor)>();
		static int activeOption = 1;
		static string menuName = "";
		static string[] menuData = {""};
		static SBMenu()
		{
		}
		
		public static ConsoleColor getColor(string input, bool text)
        {
			switch (input.ToUpper())
            {
				case "BLACK":
					return ConsoleColor.Black;

				case "BLUE":
					return ConsoleColor.Blue;

				case "CYAN":
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

		public static void addMenuChoice(string command, string text, string textColor = "WHITE", string bkgColor = "BLACK")
        {
			Menu.Add(command, (text, getColor(textColor, true), getColor(bkgColor, false)));
        }

		public static void concatMenuChoice(string command, string text)
        {
            Menu[command] = (Menu[command].Item1 + "\n" + text, Menu[command].Item2, Menu[command].Item3);
        }
        public static void concatMenuChoice(string command, string text, string textColor, string bkgColor)
        {
			Menu[command] = (Menu[command].Item1 + "\n" + text, getColor(textColor, true), getColor(bkgColor, false));
		}

		public static void CreateMenu(string BlockName)
		{
			menuName = BlockName;
			Menu.Clear();
			
            if (SBL.storyBlocks.ContainsKey(BlockName))
            {
				menuData = SBL.storyBlocks[BlockName].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			}
            else
            {
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("ERROR! Block by the name of \"" + BlockName + "\" does not exist!\nPress any button to return to the main menu.");
				Console.ReadKey(true);
				Console.ResetColor();
				SBL.LoadBlock("MAIN MENU");
            }
			
			foreach (string menuItem in menuData)
			{
				SBEH.PrefixOperation(menuItem);
            }

			if(SBL.inventory.Count > 0)
            {
				if(BlockName != "MAIN MENU" && BlockName != "ABOUT")
                {
					addMenuChoice(command: "INVENTORY", text: "-Inventory-");
				}
            }

			string menuCommand = RunMenu();

			if (menuCommand == "EXIT")
            {
				Environment.Exit(0);
            }
			else if(menuCommand == "START")
            {
				SBL.StartStory();
            }
			else if(menuCommand == "MAIN MENU")
			{
				SBL.LoadConfig();
				SBL.LoadBlock(menuCommand);
			}
			else if(menuCommand == "BACK")
			{
				goBack();
			}
			else if(menuCommand == "RELOAD")
            {
				SB.LoadStory(SBL.loadedStory);
            }
			else if(menuCommand == "INPUT")
            {
				SBL.blockHistory.Add("INPUT");
				Console.Clear();

				goBack();
            }
            else
            {
				SBL.LoadBlock(menuCommand);
            }
		}

		public static void goBack()
        {
			int lastIndex = SBL.blockHistory.Count - 1;
			string lastBlock = "MAIN MENU";
			if(lastIndex >= 1)
            {
				lastBlock = SBL.blockHistory[lastIndex - 1];
			}
			
			SBL.blockHistory.RemoveAt(lastIndex);
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

		public static string GetTitle(string blockName)
		{
			string[] menuData = SBL.storyBlocks[blockName].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string menuItem in menuData)
			{
				if (menuItem.StartsWith("S:"))
				{
					string[] lineData = menuItem.Split(new string[] { ":" }, StringSplitOptions.None);
                    if (lineData[1].Length == 0)
                    {
						return "NO TITLE FOUND";
                    }
					return lineData[1];
				}
			}
			return "NO TITLE FOUND";
		}
		public static void DrawMenu()
		{
			if (Menu.ContainsKey("S"))
			{
				Console.ForegroundColor = Menu["S"].Item2;
				Console.BackgroundColor = Menu["S"].Item3;
				Console.WriteLine(Menu["S"].Item1 + "\n\n");
				Console.ResetColor();
			}
			for (int i = 1; i < Menu.Count; i++)
			{
				Console.ForegroundColor = Menu.ElementAt(i).Value.Item2;
				Console.BackgroundColor = Menu.ElementAt(i).Value.Item3;

				if (activeOption == i)
				{
					Console.WriteLine(">> " + Menu.ElementAt(i).Value.Item1 + " <<");
				}
				else
				{
					Console.WriteLine("   " + Menu.ElementAt(i).Value.Item1 + "   ");
				}

				Console.ResetColor();
			}
            if (!(menuName == "MAIN MENU") && !(menuName == "ABOUT"))
            {
				Console.WriteLine("\n\n");
				foreach (var item in SBL.stringDict)
				{
                    if (item.Value.Item2)
                    {
						Console.WriteLine(item.Key + ":" + item.Value.Item1);
					}
				}
				foreach (var item in SBL.intDict)
				{
                    if (item.Value.Item2)
                    {
						Console.WriteLine(item.Key + ":" + item.Value.Item1);
					}
				}
				SBInventoryHandler.DrawInventory();
			}
		}

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

                }
			} while (pressedKey != ConsoleKey.Enter);
			if (Menu.Count - 1 >= activeOption)
			{
				return Menu.ElementAt(activeOption).Key;
			}
            else
            {
				return "EXIT";
            }
		}
	}
}