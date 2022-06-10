using System;
using SBM = StoryBlocks.SBMenu;
using SBCH = StoryBlocks.SBConditionalHandler;
using SBEH = StoryBlocks.SBEventHandler;
namespace StoryBlocks
{
    public static class SBLib
    {
        //Dictionary to hold all story blocks ([BLOCK NAME], [BLOCK DATA]).
        public static Dictionary<string, string> storyBlocks = new Dictionary<string, string>();
        
        //Dictionary to hold string variables ([VARIABLE NAME], ([VARIABLE VALUE], [VISIBILITY FLAG])).
        public static Dictionary<string, (string, bool)> stringDict = new Dictionary<string, (string, bool)>();

        //Dictionary to hold integer variables ([VARIABLE NAME], ([VARIABLE VALUE], [VISIBILITY FLAG])).
        public static Dictionary<string, (int, bool)> intDict = new Dictionary<string, (int, bool)>();
        
        //Dictionary to hold inventory items ([ITEM NAME], [QUANTITY]).
        public static Dictionary<string, int> inventory = new Dictionary<string, int>();
        
        //List that stores the order of visited blocks, used for the "BACK" block loading argument ([BLOCK NAME]).
        public static List<string> blockHistory = new List<string>();
        
        //boolean flag to determine if the current line falls within a block.
        static bool blockStarted;

        //default values for string variables.
        static string blockName = "";
        static string blockData = "";
        public static string title = "Untitled Story";
        public static string startBlock = "";
        public static string loadedStory = "";

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
                if (line.EndsWith("::") && !blockStarted)
                {
                    blockStarted = true;
                    blockName = line.TrimEnd(':');
                }
                else if (line.StartsWith("::") && blockStarted)
                {
                    blockStarted = false;
                    storyBlocks.Add(blockName, blockData);
                    blockData = "";
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
            stringDict.Clear();
            intDict.Clear();
            inventory.Clear();
            blockHistory.Clear();
        }

        //finds variable name in the dictionaries and toggles the visibility flag.
        //name: name of variable to toggle
        public static void toggleVisibility(string name)
        {
            if (intDict.ContainsKey(name))
            {
                int value = intDict[name].Item1;

                if (intDict[name].Item2 == true)
                {
                    intDict[name] = (value, false);
                }
                else if (intDict[name].Item2 == false)
                {
                    intDict[name] = (value, true);
                }
            }
            
            else if (stringDict.ContainsKey(name))
            {
                string value = stringDict[name].Item1;

                if (stringDict[name].Item2 == true)
                {
                    stringDict[name] = (value, false);
                }
                else if (stringDict[name].Item2 == false)
                {
                    stringDict[name] = (value, true);
                }
            }
        }

        //Sets visibility flag of a variable to true (visible).
        //name: name of variable to make visible
        public static void makeVisible(string name)
        {
            if (intDict.ContainsKey(name))
            {
                intDict[name] = (intDict[name].Item1, true);
            }
            if (stringDict.ContainsKey(name))
            {
                stringDict[name] = (stringDict[name].Item1, true);
            }
        }

        //Sets visibility flag of a variable to false (hidden).
        //name: name of variable to make hidden
        public static void makeHidden(string name)
        {
            if (intDict.ContainsKey(name))
            {
                intDict[name] = (intDict[name].Item1, false);
            }
            if (stringDict.ContainsKey(name))
            {
                stringDict[name] = (stringDict[name].Item1, false);
            }
        }

        //Loads and parses the "CONFIG" block.
        public static void LoadConfig()
        {
            ClearDicts();
            inventory.Clear();
            string[] configData = storyBlocks["CONFIG"].Split(new string[] { "\n" }, StringSplitOptions.None);
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
            blockHistory.Add(BlockName);
            SBM.CreateMenu(BlockName);
        }

        //Loads the story block specified in the CONFIG block for the beginning of the story.
        public static void StartStory()
        {
            LoadBlock(startBlock);
        }
    }
}