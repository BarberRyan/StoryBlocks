using System;
using SBL = StoryBlocks.SBLib;
using SBM = StoryBlocks.SBMenu;
using SBEH = StoryBlocks.SBEventHandler;
using SBERR = StoryBlocks.SBErrorHandler;
using EErrorCode = StoryBlocks.SBErrorHandler.EErrorCode;


namespace StoryBlocks
{
	public class SBFileHandler
	{
		public SBFileHandler()
		{
		}

        static string blockName = "";
        static string blockData = "";
        public static string title = "Untitled Story";
        public static string startBlock = "";
        public static string loadedStory = "";


        public static void CreateBlocks(string filePath)
        {
            SBL.StoryBlocks.Clear();
            loadedStory = filePath;
            string? line;
            bool blockStarted = false;
            int lineNumber = 0;
            int blockLine = 0;


            StreamReader reader = File.OpenText(filePath);

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;

                if (line.EndsWith("::") && !blockStarted)
                {
                    blockStarted = true;
                    blockLine = lineNumber;
                    blockName = line.TrimEnd(':');
                }
                else if (line.StartsWith("::") && blockStarted)
                {
                    blockStarted = false;
                    SBL.StoryBlocks.Add(blockName, blockData);
                    blockData = "";
                }
                else if (line.EndsWith("::") && line.IndexOf("::") > 0 && blockStarted)
                {
                    SBERR.ThrowError((int)EErrorCode.unfinishedBlock, blockLine, blockName);
                    reader.Close();
                    SBM.FileMenu();
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


        //Loads and parses the "CONFIG" block.
        public static void LoadConfig()
        {
            SBL.ClearDicts();
            string[] configData = SBL.StoryBlocks["CONFIG"].Split(new string[] { "\n" }, StringSplitOptions.None);
            foreach (string config in configData)
            {
                SBEH.PrefixOperation(config, true);
            }
        }

        //Loads the specified block to the screen.
        //BlockName: name of the block to load
        public static void LoadBlock(string BlockName, bool flag = true)
        {
            Console.Clear();
            SBL.BlockHistory.Add(BlockName);
            SBM.CreateMenu(BlockName, flag);
        }

        //Loads the story block specified in the CONFIG block for the beginning of the story.
        public static void StartStory()
        {
            LoadBlock(startBlock);
        }

    }
}