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

        //Dictionary to hold string variables ([VARIABLE NAME], ([VARIABLE VALUE], [VISIBILITY FLAG])).
        public static Dictionary<string, (string, bool)> StringDict = new();

        //Dictionary to hold integer variables ([VARIABLE NAME], ([VARIABLE VALUE], [VISIBILITY FLAG])).
        public static Dictionary<string, (int, bool)> IntDict = new();
        
        //Dictionary to hold inventory items ([ITEM NAME], [QUANTITY]).
        public static Dictionary<string, int> Inventory = new();

        //Dictionary to hold global conditionals ([ENTIRE LINE], (VARIABLE 1, OPERATION, VARIABLE 2, TRIGGERED FLAG)).
        public static Dictionary<string, (string, string, string, bool)> GlobalConditional = new();

        //List that stores the order of visited blocks, used for the "BACK" block loading argument ([BLOCK NAME]).
        public static List<string> BlockHistory = new();

        //boolean flag to determine if the current line falls within a block.
        static bool BlockStarted;


        //default values for string variables.
        static string blockName = "";
        static string blockData = "";
        public static string title = "Untitled Story";
        public static string startBlock = "";
        public static string loadedStory = "";

        public static Dictionary<string, string> StoryBlocks { get => storyBlocks; set => storyBlocks = value; }

        static SBLib()
        {
        }

        //steps through the story file line by line to determine where each block is and what it contains.
        //filePath: file path of story file
        public static void CreateBlocks(string filePath)
        {
            loadedStory = filePath;
            string? line;

           
            StreamReader reader = File.OpenText(filePath);

            while ((line = reader.ReadLine()) != null)
            {
                if (line.EndsWith("::") && !BlockStarted)
                {
                    BlockStarted = true;
                    blockName = line.TrimEnd(':');
                }
                else if (line.StartsWith("::") && BlockStarted)
                {
                    BlockStarted = false;
                    StoryBlocks.Add(blockName, blockData);
                    blockData = "";
                }
                else if (line.EndsWith("::") && line.IndexOf("::") > 0 && BlockStarted)
                {
                    SBERR.ThrowError((int)EErrorCode.unfinishedBlock, blockName);
                }
                else
                {
                    if (!line.StartsWith("//"))
                    {
                        blockData += (line + "\n");
                    }
                    
                }
            }
            reader.Close();
        }

        //clears dictionaries for variables, inventory, and block history.
        public static void ClearDicts()
        {
            StringDict.Clear();
            IntDict.Clear();
            Inventory.Clear();
            BlockHistory.Clear();
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

        //Loads and parses the "CONFIG" block.
        public static void LoadConfig()
        {
            ClearDicts();
            Inventory.Clear();
            string[] configData = StoryBlocks["CONFIG"].Split(new string[] { "\n" }, StringSplitOptions.None);
            foreach (string config in configData)
            {
                SBEH.PrefixOperation(config);
            }
        }

        //Loads the specified block to the screen.
        //BlockName: name of the block to load
        public static void LoadBlock(string BlockName)
        {
            Console.Clear();
            BlockHistory.Add(BlockName);
            SBM.CreateMenu(BlockName);
        }

        //Loads the story block specified in the CONFIG block for the beginning of the story.
        public static void StartStory()
        {
            LoadBlock(startBlock);
        }
    }
}