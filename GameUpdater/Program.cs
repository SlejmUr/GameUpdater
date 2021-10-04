using System;
using System.IO;
using System.Linq;

namespace GameUpdater
{
    class Program
    {
        public static readonly string extractPath = Directory.GetCurrentDirectory().ToString();
        static void Main(string[] args)
        {
            Console.WriteLine("GameUpdater\n");
            Console.WriteLine("Please place this file and any GameUpdater/Updates folder/file to game directory\nPress any key to continue");
            Console.ReadLine();

            if (Directory.Exists(extractPath + "\\Updates"))
            {
                ReadINFOS();
                DelAll();
                CopyAll();
            }
            else
            {
                Console.WriteLine("Updates folder not there, Failed!\nPlease create it!");
            }
            Console.WriteLine("Press anything to exit");
            Console.ReadLine();
        }

        static void CopyAll()
        {
            Console.WriteLine("\nCopy Update files\n");
            string[] fileArray = Directory.GetFiles(extractPath + "\\Updates", "*.*", SearchOption.AllDirectories);
            foreach (string file in fileArray)
            {
                Console.WriteLine("File: " + file);
                string filename = file.Replace(extractPath + "\\Updates\\", "");
                Console.WriteLine("Filename: " + filename);
                string lastname = file.Split("\\").Last();
                string filedir = file.Replace("\\Updates", "");
                if (!filename.Contains("\\"))
                {
                    filedir = filedir.Replace("\\" + filename, "");
                    lastname = "";
                }
                else
                {
                    filedir = filedir.Replace("\\" + lastname, "");
                }
                Console.WriteLine("FileDir: " + filedir);


                if (Directory.Exists(filedir) == false && lastname != "")
                {
                    Directory.CreateDirectory(filedir);
                }
                try
                {
                    Console.WriteLine("File Copy successful");
                    File.Copy(file, extractPath + "\\" + filename, true);
                }
                catch (IOException copyError)
                {
                    WriteToFile("Something failed!\n" + copyError.Message, "GameUpdater.ERROR.txt");
                    Console.WriteLine(copyError.Message);
                }
            }
        }

        static void DelAll()
        {
            if (File.Exists(extractPath + "\\GameUpdater.Delete.txt"))
            {
                Console.WriteLine("\nDelete Update files\n");
                String line;
                try
                {
                    StreamReader sr = new StreamReader(extractPath + "\\GameUpdater.Delete.txt");
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.Contains("#"))
                        {
                            //Console.WriteLine("\tComment, Skipping");
                        }
                        else
                        {
                            Console.WriteLine(line);
                            if (line.Contains("."))
                            {
                                if (File.Exists(extractPath + "\\" + line))
                                { File.Delete(extractPath + "\\" + line); }

                            }
                            else
                            {
                                if (Directory.Exists(extractPath + "\\" + line))
                                { Directory.Delete(extractPath + "\\" + line, true); }

                            }
                        }
                        line = sr.ReadLine();
                    }
                    sr.Close();
                    Console.WriteLine("Press anything to continue");
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    WriteToFile("Something failed!\n" + e.Message, "GameUpdater.ERROR.txt");
                    Console.WriteLine("Exception: " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("No need for delete any files!");
            }
        }

        public static void ReadINFOS()
        {
            if (File.Exists(extractPath + "\\GameUpdater.INFOS.txt"))
            {
                String line;
                StreamReader sr = new StreamReader(extractPath + "\\GameUpdater.INFOS.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    if (!line.StartsWith("###")) //Check if Commented
                    {
                        Console.WriteLine(line);
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
                Console.WriteLine("Press anything to continue");
                Console.ReadLine();
            }
        }

        public static void WriteToFile(string msg,string filename)
        {
            FileInfo logFileInfo = new FileInfo(filename);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(filename, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(DateTime.Now + " | " + msg);
                }
            }
        }

        string InfosFile = @"
### COMMENT | Game infos there
###This line is not gonna go to cmd
### 
		GAME INFOS
Game Name = 
###
### Game Name
###
### Example = Tom Clancy's Rainbow Six® Siege

Game Release Date =
###
### Released Game date
###
### Example = 1 Dec, 2015

Game Version = 
###
### Game Version (Steam Build if cant see)
###
### Example = 1 Dec, 2015

Game Size = 
###
### Game Size
###
### Example = 52.80GiB

Steam Build Version = 
###
### Check steamdb for this info
###
### Example = 7396897

Steam Branch Name = (PUBLIC)
###
### Check steamdb for this info
###
### Example = Public / tu_rotation_03
### If Public branch, dont need to specified

Steam Depots = 
###
### Check steamdb for this info
###
### Example = 359551(Content) | 377237(Sku WW) | 377236(Russian)

Other Version(s) = 
###
### EGS or Uplay version
###
### Example = 33108691

Platform = 
###
### Platform from ripped (Steam,Uplay, EGS, Origin, etc)
###
### Example = Steam

RIP Date = 
###
### Date when the game ripped/downloaded
###
### Example = 24 Sept 2021

Protection = 
###
### Protection what game using
###
### Example = Steam , BattlEye , Uplay (R2)

Removed = 
###
### Specify if you removed something
###
### Example = BattlEye , Videos folder 
### Or Check Removed.txt if you removed more things

Added = 
###
### Specify if you added something
###
### Example = Uninstaller
";
    }
}
