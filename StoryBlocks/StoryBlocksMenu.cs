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
		public static Dictionary<string, string> Menu = new Dictionary<string, string>();
		static int activeOption = 1;
		static string menuName = "";
		static string[] menuData = {""};
		static SBMenu()
		{
		}
		public static void CreateMenu(string BlockName)
		{
			Menu.Clear();
			menuName = BlockName;
			
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
					Menu.Add("INVENTORY", "-Inventory-");
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
			string lastBlock = SBL.blockHistory[lastIndex - 1];
			SBL.blockHistory.RemoveAt(lastIndex);
			SBL.blockHistory.RemoveAt(lastIndex - 1);
			SBL.LoadBlock(lastBlock);
		}

		public static string GetMenuStory(string blockName)
		{
			string[] menuData = SBL.storyBlocks[blockName].Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string menuItem in menuData)
			{
				if (menuItem.StartsWith("S:"))
				{
					string[] lineData = menuItem.Split(new string[] { ":" }, StringSplitOptions.None);
					return lineData[1];
				}
			}
			return "NO STORY FOUND";
		}
		public static void DrawMenu()
		{
			if (Menu.ContainsKey("S"))
			{
				Console.WriteLine(Menu["S"] + "\n\n");
			}
			for (int i = 1; i < Menu.Count; i++)
			{
				if (activeOption == i)
				{
					Console.WriteLine(">> " + Menu.ElementAt(i).Value + " <<");
				}
				else
				{
					Console.WriteLine("   " + Menu.ElementAt(i).Value + "   ");
				}
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