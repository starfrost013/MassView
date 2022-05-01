
namespace MassView
{
    public class CommandLine
    {
        public string InFolder { get; set; }

        public string OutFile { get; set; }

        public static CommandLine Parse(string[] args)
        {
            CommandLine cLine = new CommandLine();
            
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
                            cLine.InFolder = args[i + 1];
                            continue;
                        case "-outfile":
                        case "-o":
                            cLine.OutFile = args[i + 1];    
                            continue;
                    }
                }
            }

            if (cLine.InFolder == null)
            {
                return null;
            }
            else
            {
                return cLine;
            }
        }

        public static void ShowHelp()
        {
            PrintVersion();
            Console.WriteLine("massview -infolder [folder] -outfile [file]\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Required parameters:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-infolder: Folder to gather PE files from\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Optional parameters:\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-outfile: File to dump DateTimeStamp / ImageFileSize to (by default: will write to console)");
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