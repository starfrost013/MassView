
namespace MassView
{
    public class CommandLine
    {
        public string InFolder { get; set; }

        public string OutFile { get; set; }

        public bool Quiet { get; set; }
        public static CommandLine Parse(string[] args)
        {
            CommandLine cmdLine = new CommandLine();
            
            for (int i = 0; i < args.Length; i++) 
            {
                string arg = args[i];

                arg = arg.Trim();
                arg = arg.ToLower();

                if (args.Length - i > 0)
                {
                    switch (arg)
                    {
                        case "-infolder":
                        case "-i":
                            cmdLine.InFolder = args[i + 1];
                            continue;
                        case "-outfile":
                        case "-o":
                            cmdLine.OutFile = args[i + 1];    
                            continue;
                        case "-quiet":
                        case "-q":
                            cmdLine.Quiet = true;
                            continue;
                    }
                }
            }

            if (cmdLine.InFolder == null)
            {
                return null;
            }
            else
            {
                return cmdLine;
            }
        }

        public static void ShowHelp()
        {
            PrintVersion();
            Console.WriteLine("massview -infolder [folder] -outfile [file]\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Required parameters:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-infolder [-i]: The folder to gather PE files from.\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Optional parameters:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-outfile [-o]: File to dump DateTimeStamp / ImageFileSize to (by default: will write to console).");
            Console.WriteLine("-quiet [-q]: Suppress non-essential console output.");
            Console.WriteLine("This file will be in CSV format.");
        }

        public static void PrintVersion()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("massview ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{MVVersion.MASSVIEW_VERSION_EXTENDED_STRING}");
            Console.WriteLine("massdumps all PE imagesize and timedatestamps in a folder\n");
        }
    }
}