using System;
using System.Text;
using SBL = StoryBlocks.SBLib;

namespace StoryBlocks
{
	public class SBInventoryHandler
	{
		public SBInventoryHandler()
		{
		}

		public static void InventoryAdd(string name, int count)
        {
			if (SBL.inventory.ContainsKey(name))
            {
                SBL.inventory[name] = SBL.inventory[name] + count;
			}
            else
            {
				SBL.inventory.Add(name, count);
			}
        }
		public static void InventoryRemove(string name, int count)
        {
			if (SBL.inventory.ContainsKey(name))
            {
				int item = SBL.inventory[name];
				if (item > count)
                {
					SBL.inventory[name] = item - count;
                }
				else if (item <= count)
                {
					SBL.inventory.Remove(name);
                }
            }
        }
	
		public static (string, int) GetItemInfo(string name)
        {
			if (SBL.inventory.ContainsKey(name))
            {
				return (name, SBL.inventory[name]);
            }
			return (name, 0);
        } 

		public static void DrawInventory()
        {
			if (SBL.inventory.Count() > 0)
			{
				Console.WriteLine("╔═══════════╗");
				Console.WriteLine("║ INVENTORY ║");
				string inventoryInfo = "";
				foreach (var item in SBL.inventory)
				{
					(string, int) thisItem = GetItemInfo(item.Key);
					inventoryInfo += ("║    " + thisItem.Item1 + " : " + thisItem.Item2 + "    ");
				}
				if(inventoryInfo.Length < 11)
                {
					inventoryInfo = inventoryInfo.PadRight(13, ' ');
                }
				string topLine = "╠═══════════╩".PadRight(inventoryInfo.Length, '═') + "╗";
				var TL = new StringBuilder(topLine);

				for (int i = 1; i < inventoryInfo.Length - 1; i++)
				{
					if (inventoryInfo[i] == '║')
					{ 
						if(i == 12)
                        {
							TL[i] = '╬';
						}
                        else
                        {
							TL[i] = '╦';
						}
						

					}
				}

				Console.WriteLine(TL.ToString());
				Console.WriteLine(inventoryInfo + "║");

				string bottomLine = "╚═".PadRight(inventoryInfo.Length, '═') + "╝";
				var BL = new StringBuilder(bottomLine);
				
				for(int i = 1; i < inventoryInfo.Length - 1; i++)
                {
                    if (inventoryInfo[i] == '║')
                    {
						BL[i] = '╩';

					}
                }
				Console.WriteLine(BL.ToString());
			}
		}
	}
}