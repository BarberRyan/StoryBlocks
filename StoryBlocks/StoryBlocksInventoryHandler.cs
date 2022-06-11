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

		//Adds item to the inventory or adds to the quantity of that item.
		//name: item name
		//count: quantity to add
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

		//Removes item from the inventory or subtracts from the quantity of that item.
		//name: item name
		//count: quantity to subtract
		public static void InventoryRemove(string name, int count)
        {
			if (SBL.inventory.ContainsKey(name))
            {
				int qty = SBL.inventory[name];
				if (qty > count)
                {
					SBL.inventory[name] = qty - count;
                }
				else if (qty <= count)
                {
					SBL.inventory.Remove(name);
                }
            }
        }
	
		//Returns item name and quantity from the inventory.
		//name: name of item to check
		public static (string, int) GetItemInfo(string name)
        {
			if (SBL.inventory.ContainsKey(name))
            {
				return (name, SBL.inventory[name]);
            }
			return (name, 0);
        } 

		//draws the inventory box and items to go inside.
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
					inventoryInfo += ($"║ { thisItem.Item1} : {thisItem.Item2} ");
					if(SBL.inventory.Last().Key == item.Key)
                    {
						inventoryInfo += "║";

					}
				}
				if(inventoryInfo.Length < 11)
                {
					inventoryInfo = inventoryInfo.PadRight(13, ' ');
                }
				string topLine = "╠═══════════╩".PadRight(inventoryInfo.Length - 1, '═') + "╗";
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
				Console.WriteLine(inventoryInfo);

				string bottomLine = "╚".PadRight(inventoryInfo.Length - 1, '═') + "╝";
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