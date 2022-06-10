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

		public static void PrefixOperation(string Line)
        {
			int PrefixIndex = SBPH.GetPrefixIndex(Line);
			string[] lineData = Line.Substring(Line.IndexOf(':') + 1).Split(new string[] {":"}, StringSplitOptions.RemoveEmptyEntries);
			string[] Elements = { "", "", "", "" };
			for(int i = 0; i < lineData.Length; i++)
            {
				Elements[i] = lineData[i];
            }

			switch (PrefixIndex)
			{
				case (int)EPrefix.startBlock:
					SBL.startBlock = Line.Substring(Line.IndexOf(':') + 1);
					break;

				case (int)EPrefix.title:
					SBL.title = Elements[0];
					break;

				case (int)EPrefix.story:
                    if(SBM.Menu.ContainsKey("S")){
						if (Elements[1] != "")
						{
							SBM.concatMenuChoice("S", Elements[0], Elements[1], Elements[2]);
						}
                        else
                        {
							SBM.concatMenuChoice("S", Elements[0]);
                        }
                    }
                    else
                    {
						SBM.addMenuChoice("S", Elements[0], Elements[1], Elements[2]);
					}
					break;

				case (int)EPrefix.loadBlock:
						SBM.addMenuChoice(Elements[0], Elements[1], Elements[2], Elements[3]);
					break;

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

				case (int)EPrefix.add:
                    SBL.intDict[Elements[0]] = (SBL.intDict[Elements[0]].Item1 + Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2);
					break;

				case (int)EPrefix.subtract:
					SBL.intDict[Elements[0]] = (SBL.intDict[Elements[0]].Item1 - Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2);
					break;

				case (int)EPrefix.multiply:
					SBL.intDict[Elements[0]] = (SBL.intDict[Elements[0]].Item1 * Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2);
					break;

				case (int)EPrefix.divide:
					SBL.intDict[Elements[0]] = ((SBL.intDict[Elements[0]].Item1 / Int32.Parse(Elements[1])), SBL.intDict[Elements[0]].Item2);
					break;

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

				case (int)EPrefix.lessThan:
					if (SBL.intDict[Elements[0]].Item1 < Int32.Parse(Elements[1]))
					{
						SBM.addMenuChoice(command: Elements[2],text: Elements[3]);
					}
					break;

				case (int)EPrefix.lessOrEqual:
					if (SBL.intDict[Elements[0]].Item1 <= Int32.Parse(Elements[1]))
					{
						SBM.addMenuChoice(command: Elements[2], text: Elements[3]);
					}
					break;

				case (int)EPrefix.equal:
					if (SBL.intDict.ContainsKey(Elements[0]) && SBL.intDict[Elements[0]] == (Int32.Parse(Elements[1]), SBL.intDict[Elements[0]].Item2))
					{
						SBM.addMenuChoice(command: Elements[2], text: Elements[3]);
					}
					else if (SBL.stringDict.ContainsKey(Elements[0]) && SBL.stringDict[Elements[0]] == (Elements[1], SBL.stringDict[Elements[0]].Item2))
					{
						SBM.addMenuChoice(command: Elements[2], text: Elements[3]);
					}
						break;

				case (int)EPrefix.greaterOrEqual:
					if (SBL.intDict[Elements[0]].Item1 >= Int32.Parse(Elements[1]))
					{
						SBM.addMenuChoice(command: Elements[2], text: Elements[3]);
					}
					break;

				case (int)EPrefix.greaterThan:
					if (SBL.intDict[Elements[0]].Item1 > Int32.Parse(Elements[1]))
					{
						SBM.addMenuChoice(command: Elements[2], text: Elements[3]);
					}
					break;

                case (int)EPrefix.inventoryAdd:
                    SBIH.InventoryAdd(Elements[0], Int32.Parse(Elements[1]));
					break;
				case (int)EPrefix.inventorySubtract:
                    SBIH.InventoryRemove(Elements[0], Int32.Parse(Elements[1]));
					break;

				case (int)EPrefix.inputVisible:
					Console.Clear();
					Console.WriteLine(Elements[1]+"\n");
					inputOperation(Elements[0], true);
					SBM.goBack();
					break;

				case (int)EPrefix.inputHidden:
					Console.Clear();
					Console.WriteLine(Elements[1] + "\n");
					inputOperation(Elements[0], false);
					SBM.goBack();
					break;

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

				case (int)EPrefix.makeVisible:
					SBL.makeVisible(Elements[0]);
					break;

				case (int)EPrefix.makeHidden:
					SBL.makeHidden(Elements[0]);
					break;

				case (int)EPrefix.toggleVisibility:
					SBL.toggleVisibility(Elements[0]);
					break;

				default:
					break;
					
			}
        }

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

	}
}