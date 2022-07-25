using System;
using SBM = StoryBlocks.SBMenu;
using SBERR = StoryBlocks.SBErrorHandler;
using EErrorCode = StoryBlocks.SBErrorHandler.EErrorCode;
using SBEH = StoryBlocks.SBEventHandler;
namespace StoryBlocks
{
    public static class SBLib
    {
        //Dictionary to hold all story blocks ([BLOCK NAME], [BLOCK DATA]).
        private static Dictionary<string, string> storyBlocks = new();
        public static Dictionary<string, string> StoryBlocks { get => storyBlocks; set => storyBlocks = value; }

        //Dictionary to hold string variables ([VARIABLE NAME], ([VARIABLE VALUE], [VISIBILITY FLAG])).
        public static Dictionary<string, (string, bool)> StringDict = new();

        //Dictionary to hold integer variables ([VARIABLE NAME], ([VARIABLE VALUE], [VISIBILITY FLAG])).
        public static Dictionary<string, (int, bool)> IntDict = new();
        
        //Dictionary to hold inventory items ([ITEM NAME], [QUANTITY]).
        public static Dictionary<string, int> Inventory = new();

        //Dictionary to hold global conditionals ([ENTIRE LINE], [TRIGGERED FLAG]).
        public static Dictionary<string, bool> GlobalConditional = new();

        //List that stores the order of visited blocks, used for the "BACK" block loading argument ([BLOCK NAME]).
        public static List<string> BlockHistory = new();

        public static string OutputOption = "CONSOLE";


        static SBLib()
        {
        }

        //clears dictionaries for variables, inventory, and block history.
        public static void ClearDicts()
        {
            StringDict.Clear();
            IntDict.Clear();
            Inventory.Clear();
            BlockHistory.Clear();
        }

        public static string GetDictValue(string key)
        {
            if (IntDict.ContainsKey(key))
            {
                return IntDict[key].Item1.ToString();
            }

            if (StringDict.ContainsKey(key))
            {
                return StringDict[key].Item1;
            }

            if (Inventory.ContainsKey(key))
            {
                return Inventory[key].ToString();
            }

            return key;
        }

        //finds variable name in the dictionaries and toggles the visibility flag.
        //name: name of variable to toggle
        public static void ToggleVisibility(string name)
        {
            if (IntDict.ContainsKey(name))
            {
                int value = IntDict[name].Item1;

                if (IntDict[name].Item2 == true)
                {
                    IntDict[name] = (value, false);
                }
                else if (IntDict[name].Item2 == false)
                {
                    IntDict[name] = (value, true);
                }
            }
            
            else if (StringDict.ContainsKey(name))
            {
                string value = StringDict[name].Item1;

                if (StringDict[name].Item2 == true)
                {
                    StringDict[name] = (value, false);
                }
                else if (StringDict[name].Item2 == false)
                {
                    StringDict[name] = (value, true);
                }
            }
        }

        //Sets visibility flag of a variable to true (visible).
        //name: name of variable to make visible
        public static void MakeVisible(string name)
        {
            if (IntDict.ContainsKey(name))
            {
                IntDict[name] = (IntDict[name].Item1, true);
            }
            if (StringDict.ContainsKey(name))
            {
                StringDict[name] = (StringDict[name].Item1, true);
            }
        }

        //Sets visibility flag of a variable to false (hidden).
        //name: name of variable to make hidden
        public static void MakeHidden(string name)
        {
            if (IntDict.ContainsKey(name))
            {
                IntDict[name] = (IntDict[name].Item1, false);
            }
            if (StringDict.ContainsKey(name))
            {
                StringDict[name] = (StringDict[name].Item1, false);
            }
        }
    }
}