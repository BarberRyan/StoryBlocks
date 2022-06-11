using System;
using SBIH = StoryBlocks.SBInventoryHandler;
using SBL = StoryBlocks.SBLib;
using SBM = StoryBlocks.SBMenu;
using SBPH = StoryBlocks.SBPrefixHandler;
using EPrefix = StoryBlocks.SBPrefixHandler.EPrefix;

namespace StoryBlocks
{
	public class SBEventHandler
	{
		public SBEventHandler()
		{
		}

		//takes input string from line reader, finds the prefix index (from StoryBlocksPrefixHandler.EPrefix),
		//and provides logic for each prefix. Elements can be made larger to accommodate a larger number of arguments.
		//line: string provided by StreamReader

		public static void PrefixOperation(string Line)
        {
			int PrefixIndex = SBPH.GetPrefixIndex(Line);
			string subPrefix = SBPH.GetSubPrefix(Line);
			string[] lineData = Line.Substring(Line.IndexOf(':') + 1).Split(new string[] {":"}, StringSplitOptions.RemoveEmptyEntries);
			string[] Elements = new string[20];
			Array.Fill(Elements, "");
			for(int i = 0; i < lineData.Length; i++)
            {
				Elements[i] = lineData[i];
            }

			switch (PrefixIndex)
			{
				//indicates first block to load, should be set in CONFIG block ("1:[BLOCK NAME]").
				case (int)EPrefix.startBlock:
					SBL.startBlock = Line.Substring(Line.IndexOf(':') + 1);
					break;

				//indicates the window title, should be set in CONFIG block ("T:[TITLE]").
				case (int)EPrefix.title:
					SBL.title = Elements[0];
					break;

				//builds the story (the text above a menu) for a menu. Each line starting with "S:" will be added.
				//currently, a color argument will apply to the entire block ("S:[TEXT]").
				case (int)EPrefix.story:
                    if(SBM.Menu.ContainsKey("S")){
						if (Elements[1] != "")
						{
							SBM.concatMenuElement("S", Elements[0], Elements[1], Elements[2]);
						}
                        else
                        {
							SBM.concatMenuElement("S", Elements[0]);
                        }
                    }
                    else
                    {
						SBM.addMenuElement("S", Elements[0], Elements[1], Elements[2]);
					}
					break;

				//creates a menu choice that loads a new block (">:[BLOCK TO LOAD]").
				case (int)EPrefix.loadBlock:
						SBM.addMenuElement(Elements[0], Elements[1], Elements[2], Elements[3]);
					break;

				//creates or updates an integer variable that appears in the status section of the screen ("!:[NAME]:[START VALUE]").
				case (int)EPrefix.visibleInt:
					if (!SBL.intDict.ContainsKey(Elements[0]))
					{
						SBL.intDict.Add(Elements[0], (Int32.Parse(Elements[1]), true));
					}
					else
					{
						SBL.intDict[Elements[0]] = (Int32.Parse(Elements[1]), true);
					}
					break;

				//creates or updates an integer variable that does not appear in the status section of the screen ("XI:[NAME]:[START VALUE]")
				case (int)EPrefix.hiddenInt:
					if (!SBL.intDict.ContainsKey(Elements[0]))
					{
						SBL.intDict.Add(Elements[0], (Int32.Parse(Elements[1]), false));
					}
					else
					{
						SBL.intDict[Elements[0]] = (Int32.Parse(Elements[1]), false);
					}
					break;

				//adds a specified integer value to specified integer variable ("+:[NAME]:[AMOUNT TO ADD]")
				case (int)EPrefix.add:
                    SBL.intDict[Elements[0]] = (SBL.intDict[Elements[0]].Item1 + Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2);
					break;

				//subtracts a specified integer value from specified integer variable ("-:[NAME]:[AMOUNT TO SUBTRACT]").
				case (int)EPrefix.subtract:
					SBL.intDict[Elements[0]] = (SBL.intDict[Elements[0]].Item1 - Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2);
					break;

				//multiplies a specified integer value to specified integer variable ("*:").
				case (int)EPrefix.multiply:
					SBL.intDict[Elements[0]] = (SBL.intDict[Elements[0]].Item1 * Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2);
					break;

				//divides a specified integer variable by specified integer and ignores remainder ("/:").
				case (int)EPrefix.divide:
					SBL.intDict[Elements[0]] = ((SBL.intDict[Elements[0]].Item1 / Int32.Parse(Elements[1])), SBL.intDict[Elements[0]].Item2);
					break;


				//creates or updates a string variable that appears in the status section of the screen ("!S:").
				case (int)EPrefix.visibleStr:
					if (!SBL.stringDict.ContainsKey(Elements[0]))
					{
						SBL.stringDict.Add(Elements[0], (Elements[1], true));
					}
					else
					{
						SBL.stringDict[Elements[0]] = (Elements[1], true);
					}
					break;

				//creates or updates a string variable that does not appear in the status section of the screen ("XS:").
				case (int)EPrefix.hiddenStr:
					if (!SBL.stringDict.ContainsKey(Elements[0]))
					{
						SBL.stringDict.Add(Elements[0], (Elements[1], false));
					}
					else
					{
						SBL.stringDict[Elements[0]] = (Elements[1], false);
					}
					break;

				//tests if the specified integer variable is less than the specified integer value ("?<:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				case (int)EPrefix.lessThan:
					if (SBL.intDict[Elements[0]].Item1 < Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					else if (SBL.inventory.ContainsKey(Elements[0]) && SBL.inventory[Elements[0]] < Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					break;

				//tests if the specified integer variable is less than or equal to the specified integer value ("?<=:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				case (int)EPrefix.lessOrEqual:
					if (SBL.intDict[Elements[0]].Item1 <= Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					else if (SBL.inventory.ContainsKey(Elements[0]) && SBL.inventory[Elements[0]] <= Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					break;

				//tests if the specified variable is equal to the specified value ("?=:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				//Can be used on int or string variables.
				case (int)EPrefix.equal:
					if (SBL.intDict.ContainsKey(Elements[0]) && SBL.intDict[Elements[0]] == (Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					else if (SBL.stringDict.ContainsKey(Elements[0]) && SBL.stringDict[Elements[0]] == (Elements[1], SBL.stringDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					else if (SBL.inventory.ContainsKey(Elements[0]) && SBL.inventory[Elements[0]] == Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					break;
				//tests if the specified variable is NOT equal to the specified value ("?!=:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				//Can be used on int or string variables.
				case (int)EPrefix.notEqual:
					if (SBL.intDict.ContainsKey(Elements[0]) && SBL.intDict[Elements[0]] != (Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					else if (SBL.stringDict.ContainsKey(Elements[0]) && SBL.stringDict[Elements[0]] != (Elements[1], SBL.stringDict[Elements[0]].Item2))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					else if (SBL.inventory.ContainsKey(Elements[0]) && SBL.inventory[Elements[0]] != Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					break;

				//tests if the specified integer variable is greater than the specified integer value ("?>:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				case (int)EPrefix.greaterOrEqual:
                    if (SBL.intDict.ContainsKey(Elements[0]))
                    {
						if (SBL.intDict[Elements[0]].Item1 >= Int32.Parse(Elements[1]))
						{
							PrefixOperation(SBPH.GetNextPrefix(Line));
						}
                    }
					if (SBL.inventory.ContainsKey(Elements[0]))
                    {
						if (SBL.inventory[Elements[0]] >= Int32.Parse(Elements[1]))
						{
							PrefixOperation(SBPH.GetNextPrefix(Line));
						}
                    }
					
					break;
				//tests if the specified integer variable is greater than or equal to than the specified integer value ("?>=:[NAME]:[TEST VALUE]:[BLOCK TO LOAD]:[CHOICE TEXT]").
				case (int)EPrefix.greaterThan:
					if (SBL.intDict.ContainsKey(Elements[0]) && SBL.intDict[Elements[0]].Item1 > Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					else if (SBL.inventory.ContainsKey(Elements[0]) && SBL.inventory[Elements[0]] > Int32.Parse(Elements[1]))
					{
						PrefixOperation(SBPH.GetNextPrefix(Line));
					}
					break;

				//Adds an item to the inventory, or adds the specified quantity to the inventory ("I+:[NAME]:[QUANTITY]").
                case (int)EPrefix.inventoryAdd:
                    SBIH.InventoryAdd(Elements[0], Int32.Parse(Elements[1]));
					break;

				//subtracts quantity of item from inventory or removes it if 0 are left ("I-:[NAME]:[QUANTITY]").
				case (int)EPrefix.inventorySubtract:
                    SBIH.InventoryRemove(Elements[0], Int32.Parse(Elements[1]));
					break;

				//provides an input option for visible variables ("!=:[NAME]:[PROMPT]").
				case (int)EPrefix.inputVisible:
					Console.Clear();
					Console.ForegroundColor = SBM.getColor(Elements[2], true);
					Console.BackgroundColor = SBM.getColor(Elements[3], false);
					Console.WriteLine(Elements[1]+"\n");
					inputOperation(Elements[0], true);
					SBM.goBack();
					break;

				//provides an input option for hidden variables ("X=:[NAME]:[PROMPT]").
				case (int)EPrefix.inputHidden:
					Console.Clear();
					Console.ForegroundColor = SBM.getColor(Elements[2], true);
					Console.BackgroundColor = SBM.getColor(Elements[3], false);
					Console.WriteLine(Elements[1] + "\n");
					inputOperation(Elements[0], false);
					SBM.goBack();
					break;

				//immediately loads a new block when triggered (">>:[BLOCK TO LOAD]").
				//Used for making blocks for item functions, changing stats, etc.
				case (int)EPrefix.immediateJump:
                    if (Elements[0] == "BACK")
                    {
						SBM.goBack();
					}
                    else
                    {
						SBL.LoadBlock(Elements[0]);
                    }
					
					break;

				//Makes a variable visible ("!!:[NAME]").
				case (int)EPrefix.makeVisible:
					SBL.makeVisible(Elements[0]);
					break;

				//Makes a variable hidden ("XX:[NAME]").
				case (int)EPrefix.makeHidden:
					SBL.makeHidden(Elements[0]);
					break;

				//Toggles variable visibility ("!X:[NAME]").
				case (int)EPrefix.toggleVisibility:
					SBL.toggleVisibility(Elements[0]);
					break;

				default:
					break;
					
			}
        }

		//Handles manual user input.
		//key: name of the variable to update/add
		//visible: boolean value to set visibility (true = visible, false = hidden).
		//If the input provided is able to be parsed as an integer, it is saved as an integer variable, otherwise it is a string.

		public static void inputOperation(string key, bool visible)
        {
            string? inputString = Console.ReadLine();
			
			if(inputString == null)
            {
				inputString = "";
            }

			if(Int32.TryParse(inputString, out int inputInt))
            {
				if (SBL.intDict.ContainsKey(key))
				{
					SBL.intDict[key] = (inputInt, SBL.intDict[key].Item2);
				}
                else
                {
					SBL.intDict.Add(key, (inputInt, visible));
                }
			}
            else
            {
				if (SBL.stringDict.ContainsKey(key))
				{
					SBL.stringDict[key] = (inputString, SBL.stringDict[key].Item2);
				}
				else
				{
					SBL.stringDict.Add(key, (inputString, visible));
				}
			}
        }

		public static string replaceVariable(string input)
        {
			int startIndex;
			string varName;
			string newValue = "";
			for(int i = 0; i < input.Length; i++)
            {
                if (input[i] == '@')
                {
					startIndex = i + 1;
					for(int j = i + 1; j < input.Length; j++)
                    {
						if (input[j] == '@')
                        {
							newValue = "";
							varName = input.Substring(startIndex, (j - startIndex));
                            if (SBL.stringDict.ContainsKey(varName))
							{
								newValue = SBL.stringDict[varName].Item1;
                            }
							else if (SBL.intDict.ContainsKey(varName))
							{
								newValue = SBL.intDict[varName].Item1.ToString();
							}
							else if (SBL.inventory.ContainsKey(varName))
                            {
								newValue = SBL.inventory[varName].ToString();
                            }
                            else
                            {
								newValue = "null";
                            }
							i = j + 1;
							string oldValue = "@" + varName + "@";
							input = input.Replace(oldValue, newValue);
							break;
						}
                    }
                }
            }
			return input;
        }

	}
}