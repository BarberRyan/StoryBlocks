using System;
using SBM = StoryBlocks.SBMenu;
using SBCH = StoryBlocks.SBConditionalHandler;
using SBEH = StoryBlocks.SBEventHandler;
namespace StoryBlocks
{
    public static class SBLib
    {
        public static Dictionary<string, string> storyBlocks = new Dictionary<string, string>();
        public static Dictionary<string, (string, bool)> stringDict = new Dictionary<string, (string, bool)>();
        public static Dictionary<string, (int, bool)> intDict = new Dictionary<string, (int, bool)>();
        public static Dictionary<string, int> inventory = new Dictionary<string, int>();
        public static List<string> blockHistory = new List<string>();
        static bool blockStarted;
        static string line = "";
        static string blockName = "";
        static string blockData = "";
        public static string startBlock = "";
        public static string loadedStory = "";

        static SBLib()
        {
        }

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

        public static void ClearDicts()
        {
            stringDict.Clear();
            intDict.Clear();
            inventory.Clear();
            blockHistory.Clear();
        }

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
            
            if (stringDict.ContainsKey(name))
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

        public static void LoadConfig()
        {
            ClearDicts();
            inventory.Clear();
            SBCH.clearConds();
            string[] configData = storyBlocks["CONFIG"].Split(new string[] { "\n" }, StringSplitOptions.None);
            foreach (string config in configData)
            {
                SBEH.PrefixOperation(config);
            }
        }

        public static void LoadBlock(string BlockName)
        {
            Console.Clear();
            blockHistory.Add(BlockName);
            SBM.CreateMenu(BlockName);
        }

        public static void StartStory()
        {
            LoadBlock(startBlock);
        }
    }
}