using System;
using SBIH = StoryBlocks.SBInventoryHandler;
using SBL = StoryBlocks.SBLib;
using SBM = StoryBlocks.SBMenu;
using SBPH = StoryBlocks.SBPrefixHandler;
using EPrefix = StoryBlocks.SBPrefixHandler.EPrefix;
using SBTH = StoryBlocks.SBTextHandler;
using SBFH = StoryBlocks.SBFileHandler;

namespace StoryBlocks
{
	public class SBEventHandler
	{
		public SBEventHandler()
		{
		}

		/// <summary>
		/// takes input string from line reader, finds the prefix index (from StoryBlocksPrefixHandler.EPrefix),
		/// and provides logic for each prefix. Elements can be made larger to accommodate a larger number of arguments.
		/// </summary>
		/// <param name="Line"></param>
		/// <param name="flag"></param>
		/// <returns></returns>

		public static bool PrefixOperation(string Line, bool flag = true)
        {
			int PrefixIndex = SBPH.GetPrefixIndex(Line);
			List<string> lineData = Line[(Line.IndexOf(':') + 1)..].Split(':').ToList<string>();
			string[] Elements = new string[20];
			Array.Fill(Elements, "");
			for(int i = 0; i < lineData.Count; i++)
            {
				Elements[i] = ReplaceVariable(lineData[i]);
            }
			switch (PrefixIndex)
			{
				//indicates first block to load, should be set in CONFIG block ("1:[BLOCK NAME]").
				case (int)EPrefix.startBlock:
					SBFH.startBlock = Line[(Line.IndexOf(':') + 1)..];
					return true;

				//indicates the window title, should be set in CONFIG block ("T:[TITLE]").
				case (int)EPrefix.title:
					SBFH.title = Elements[0];
					return true;

				//builds the story (the text above a menu) for a menu. Each line starting with "S:" will be added.
				case (int)EPrefix.story:
					SBM.AddMenuElement("S", Elements[0]);
					return true;

				//sets output options for non-console applications
				case (int)EPrefix.outputOption:
					SBL.OutputOption = Elements[0].ToUpper();
					return true;

				//creates a menu choice that loads a new block (">:[BLOCK TO LOAD]").
				case (int)EPrefix.menuChoice:
                    if (Elements[0] == "BLANK")
                    {
						Random rand = new();
						Elements[1] = "BLANK";
						Elements[0] = "BLANK" + rand.Next().ToString();
                    }
						SBM.AddMenuElement(Elements[0], Elements[1]);
					return true;

				//creates or updates an integer variable that appears in the status section of the screen ("!:[NAME]:[START VALUE]").
				case (int)EPrefix.visibleInt:
					if (flag)
					{
						if (!SBL.IntDict.ContainsKey(Elements[0]))
						{
							SBL.IntDict.Add(Elements[0], (Int32.Parse(Elements[1]), true));
						}
						else
						{
							SBL.IntDict[Elements[0]] = (Int32.Parse(Elements[1]), true);
						}
					}
					return true;

				//creates or updates an integer variable that does not appear in the status section of the screen ("XI:[NAME]:[START VALUE]")
				case (int)EPrefix.hiddenInt:
					if (flag)
					{
						if (!SBL.IntDict.ContainsKey(Elements[0]))
						{
							SBL.IntDict.Add(Elements[0], (Int32.Parse(Elements[1]), false));
						}
						else
						{
							SBL.IntDict[Elements[0]] = (Int32.Parse(Elements[1]), false);
						}
					}
					return true;

				//adds a specified integer value to specified integer variable ("+:[NAME]:[AMOUNT TO ADD]")
				case (int)EPrefix.add:
					if (flag)
					{
						SBL.IntDict[Elements[0]] = (SBL.IntDict[Elements[0]].Item1 + Int32.Parse(ReplaceVariable(Elements[1])), SBL.IntDict[Elements[0]].Item2);
					}
					return true;

				//subtracts a specified integer value from specified integer variable ("-:[NAME]:[AMOUNT TO SUBTRACT]").
				case (int)EPrefix.subtract:
					if (flag)
					{
						SBL.IntDict[Elements[0]] = (SBL.IntDict[Elements[0]].Item1 - Int32.Parse(ReplaceVariable(Elements[1])), SBL.IntDict[Elements[0]].Item2);
					}
					return true;

				//multiplies a specified integer value to specified integer variable ("*:").
				case (int)EPrefix.multiply:
					if (flag)
					{
						SBL.IntDict[Elements[0]] = (SBL.IntDict[Elements[0]].Item1 * Int32.Parse(ReplaceVariable(Elements[1])), SBL.IntDict[Elements[0]].Item2);
					}
					return true;

				//divides a specified integer variable by specified integer and ignores remainder ("/:").
				case (int)EPrefix.divide:
					if (flag)
					{
						SBL.IntDict[Elements[0]] = ((SBL.IntDict[Elements[0]].Item1 / Int32.Parse(ReplaceVariable(Elements[1]))), SBL.IntDict[Elements[0]].Item2);
					}
					return true;

				//creates or updates a string variable that appears in the status section of the screen ("!S:").
				case (int)EPrefix.visibleStr:
					if (flag)
					{
						if (!SBL.StringDict.ContainsKey(Elements[0]))
						{
							SBL.StringDict.Add(Elements[0], (Elements[1], true));
						}
						else
						{
							SBL.StringDict[Elements[0]] = (Elements[1], true);
						}
					}
					return true;

				//creates or updates a string variable that does not appear in the status section of the screen ("XS:").
				case (int)EPrefix.hiddenStr:
					if (flag)
					{
						if (!SBL.StringDict.ContainsKey(Elements[0]))
						{
							SBL.StringDict.Add(Elements[0], (Elements[1], false));
						}
						else
						{
							SBL.StringDict[Elements[0]] = (Elements[1], false);
						}
					}
					return true;

				//tests if the specified integer variable is less than the specified integer value ("?<:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				case (int)EPrefix.lessThan:
					if (SBL.IntDict[Elements[0]].Item1 < Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					else if (SBL.Inventory.ContainsKey(Elements[0]) && SBL.Inventory[Elements[0]] < Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					return false;

				//tests if the specified integer variable is less than or equal to the specified integer value ("?<=:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				case (int)EPrefix.lessOrEqual:
					if (SBL.IntDict[Elements[0]].Item1 <= Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					else if (SBL.Inventory.ContainsKey(Elements[0]) && SBL.Inventory[Elements[0]] <= Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					return false;

				//tests if the specified variable is equal to the specified value ("?=:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				//Can be used on int or string variables.
				case (int)EPrefix.equal:
					if (SBL.IntDict.ContainsKey(Elements[0]) && SBL.IntDict[Elements[0]] == (Int32.Parse(Elements[1]), SBL.IntDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					else if (SBL.StringDict.ContainsKey(Elements[0]) && SBL.StringDict[Elements[0]] == (Elements[1], SBL.StringDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					else if (SBL.Inventory.ContainsKey(Elements[0]) && SBL.Inventory[Elements[0]] == Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					return false;

				//tests if the specified variable is NOT equal to the specified value ("?!=:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				//Can be used on int or string variables.
				case (int)EPrefix.notEqual:
					if (SBL.IntDict.ContainsKey(Elements[0]) && SBL.IntDict[Elements[0]] != (Int32.Parse(Elements[1]), SBL.IntDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					else if (SBL.StringDict.ContainsKey(Elements[0]) && SBL.StringDict[Elements[0]] != (Elements[1], SBL.StringDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					else if (SBL.Inventory.ContainsKey(Elements[0]) && SBL.Inventory[Elements[0]] != Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					return false;

				//tests if the specified integer variable is greater than the specified integer value ("?>:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				case (int)EPrefix.greaterOrEqual:
                    if (SBL.IntDict.ContainsKey(Elements[0]))
                    {
						if (SBL.IntDict[Elements[0]].Item1 >= Int32.Parse(Elements[1]))
						{
							PrefixOperation(SBPH.GetNextPrefix(Line));
							return true;
						}
                    }
					if (SBL.Inventory.ContainsKey(Elements[0]))
                    {
						if (SBL.Inventory[Elements[0]] >= Int32.Parse(Elements[1]))
						{
							PrefixOperation(SBPH.GetNextPrefix(Line));
							return true;
						}
                    }
					return false;

				//tests if the specified integer variable is greater than or equal to than the specified integer value ("?>=:[NAME]:[TEST VALUE]").
				case (int)EPrefix.greaterThan:
					if (SBL.IntDict.ContainsKey(Elements[0]) && SBL.IntDict[Elements[0]].Item1 > Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					else if (SBL.Inventory.ContainsKey(Elements[0]) && SBL.Inventory[Elements[0]] > Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
						return true;
					}
					return false;

				case (int)EPrefix.GlobalLessThan:
				case (int)EPrefix.GlobalLessOrEqual:
				case (int)EPrefix.GlobalEqual:
				case (int)EPrefix.GlobalNotEqual:
				case (int)EPrefix.GlobalGreaterOrEqual:
				case (int)EPrefix.GlobalGreaterThan:
                    if (!SBL.GlobalConditional.ContainsKey(Line))
                    {
						SBL.GlobalConditional.Add(Line, false);
                    }
					return true;

				//Adds an item to the inventory, or adds the specified quantity to the inventory ("I+:[NAME]:[QUANTITY]").
				case (int)EPrefix.inventoryAdd:
                    if (flag)
                    {
						SBIH.InventoryAdd(Elements[0], Int32.Parse(Elements[1]));
                    }
					return true;

				//subtracts quantity of item from inventory or removes it if 0 are left ("I-:[NAME]:[QUANTITY]").
				case (int)EPrefix.inventorySubtract:
                    if (flag)
                    {
						SBIH.InventoryRemove(Elements[0], Int32.Parse(Elements[1]));
                    }
					return true;

				//provides an input option for visible variables ("!=:[NAME]:[PROMPT]").
				case (int)EPrefix.inputVisible:
					Console.WriteLine(Elements[1] + "\n");
					InputOperation(Elements[0], true);
					SBM.GoBack();
					return true;

				//provides an input option for hidden variables ("X=:[NAME]:[PROMPT]").
				case (int)EPrefix.inputHidden:
					Console.WriteLine(Elements[1] + "\n");
					InputOperation(Elements[0], false);
					SBM.GoBack();
					return true;

				//immediately loads a new block when triggered (">>:[BLOCK TO LOAD]").
				//Used for making blocks for item functions, changing stats, etc.
				case (int)EPrefix.immediateJump:
                    if (Elements[0] == "BACK")
                    {
						SBM.GoBack();
					}
                    else
                    {
						SBFH.LoadBlock(Elements[0]);
                    }
					return true;

				//Makes a variable visible ("!!:[NAME]").
				case (int)EPrefix.makeVisible:
					SBL.MakeVisible(Elements[0]);
					return true;

				//Makes a variable hidden ("XX:[NAME]").
				case (int)EPrefix.makeHidden:
					SBL.MakeHidden(Elements[0]);
					return true;

				//Toggles variable visibility ("!X:[NAME]").
				case (int)EPrefix.toggleVisibility:
					SBL.ToggleVisibility(Elements[0]);
					return true;

				default:
					return true;

			}
        }

		//Handles manual user input.
		//key: name of the variable to update/add
		//visible: boolean value to set visibility (true = visible, false = hidden).
		//If the input provided is able to be parsed as an integer, it is saved as an integer variable, otherwise it is a string.

		public static void InputOperation(string key, bool visible)
        {
            string? inputString = Console.ReadLine();
			
			if(inputString == null)
            {
				inputString = "";
            }

			if(Int32.TryParse(inputString, out int inputInt))
            {
				if (SBL.IntDict.ContainsKey(key))
				{
					SBL.IntDict[key] = (inputInt, SBL.IntDict[key].Item2);
				}
                else
                {
					SBL.IntDict.Add(key, (inputInt, visible));
                }
			}
            else
            {
				if (SBL.StringDict.ContainsKey(key))
				{
					SBL.StringDict[key] = (inputString, SBL.StringDict[key].Item2);
				}
				else
				{
					SBL.StringDict.Add(key, (inputString, visible));
				}
			}
        }

		public static void InputOperation(string key, bool visible, string prompt, int xCord = 0, int yCord = 0)
		{
			SBBoxBuilder.DrawBox(text: prompt, xCoord: xCord, yCoord: yCord);
		}

		public static string ReplaceVariable(string input)
		{
			int startIndex;
			string varName;
			string newValue;
			for(int i = 0; i < input.Length; i++)
			{
				if (input[i] == '@')
				{
					startIndex = i + 1;
					for(int j = startIndex; j < input.Length; j++)
					{
                        if (input[j] == '@' && input[j-1] != '/')
                        {
							input = input.Replace(input.Substring(j), ReplaceVariable(input.Substring(j)));
                        }

						if (input[j] == '@' && input[j - 1] == '/')
						{
							varName = input[startIndex..(j - 1)];
							if (SBL.StringDict.ContainsKey(varName))
							{
								newValue = SBL.StringDict[varName].Item1;
							}
							else if (SBL.IntDict.ContainsKey(varName))
							{
								newValue = SBL.IntDict[varName].Item1.ToString();
							}
							else if (SBL.Inventory.ContainsKey(varName))
							{
								newValue = SBL.Inventory[varName].ToString();
							}
							else if (varName.Contains("-"))
                            {
								int lowerBound;
								int upperBound;
								Random rnd = new Random(); ;

                                Int32.TryParse(varName[..(varName.IndexOf('-') + 1)], out lowerBound);
								Int32.TryParse(varName[(varName.IndexOf('-') + 1)..], out upperBound);
								newValue = rnd.Next(lowerBound, upperBound).ToString();
							}
							else
							{
								newValue = varName;
							}
							i = j + 1;
							string oldValue = "@" + varName + "/@";
							input = input.Replace(oldValue, newValue);
							break;
						}
					}
				}
			}
			return input;
		}

		public static void CheckGlobalConditionals()
        {
			foreach(var item in SBL.GlobalConditional)
            {
                if (!item.Value && PrefixOperation(item.Key.Substring(1)))
                {
                    SBL.GlobalConditional[item.Key] = true;
					PrefixOperation(item.Key.Substring(1));
                }
            }
        }

	}
}