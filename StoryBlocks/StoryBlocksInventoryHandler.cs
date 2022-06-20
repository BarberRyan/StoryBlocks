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
			if (SBL.Inventory.ContainsKey(name))
            {
                SBL.Inventory[name] = SBL.Inventory[name] + count;
			}
            else
            {
				SBL.Inventory.Add(name, count);
			}
        }

		//Removes item from the inventory or subtracts from the quantity of that item.
		//name: item name
		//count: quantity to subtract
		public static void InventoryRemove(string name, int count)
        {
			if (SBL.Inventory.ContainsKey(name))
            {
				int qty = SBL.Inventory[name];
				if (qty > count)
                {
					SBL.Inventory[name] = qty - count;
                }
				else if (qty <= count)
                {
					SBL.Inventory.Remove(name);
                }
            }
        }
	
		//Returns item name and quantity from the inventory.
		//name: name of item to check
		public static (string, int) GetItemInfo(string name)
        {
			if (SBL.Inventory.ContainsKey(name))
            {
				return (name, SBL.Inventory[name]);
            }
			return (name, 0);
        } 

		//draws the inventory box and items to go inside.
		public static void DrawInventory()
        {
			if (SBL.Inventory.Count() > 0)
			{
				Console.WriteLine("╔═══════════╗");
				Console.WriteLine("║ INVENTORY ║");
				string inventoryInfo = "";
				foreach (var item in SBL.Inventory)
				{
					(string, int) thisItem = GetItemInfo(item.Key);
					inventoryInfo += ($"║ { thisItem.Item1} : {thisItem.Item2} ");
					if(SBL.Inventory.Last().Key == item.Key)
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
							if(SBL.Inventory.Count == 1)
                            {
								TL[i] = '╣';
                            }
                            else
                            {
								TL[i] = '╬';
							}
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