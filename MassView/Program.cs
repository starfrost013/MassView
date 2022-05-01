// See https://aka.ms/new-console-template for more information
using MassView;
using System.IO;

CommandLine cl = CommandLine.Parse(args);

if (cl != null)
{
    try
    {
        Run(cl);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: \n\n{ex}");
        Environment.Exit(1);
    }
}
else
{
    CommandLine.ShowHelp();
    Environment.Exit(0);
}

#region bad code you should probably never write

void Run(CommandLine cl)
{
    if (!Directory.Exists(cl.InFolder))
    {
        Console.WriteLine($"The folder {cl.InFolder}() does not exist!");
        CommandLine.ShowHelp();
        Environment.Exit(0);
    }
    else
    {
        StreamWriter bw = null;

        if (cl.OutFile != null)
        {
            bw = new StreamWriter(new FileStream(cl.OutFile, FileMode.OpenOrCreate));
            bw.WriteLine("FileName,TimeDateStamp,ISO8601,SizeOfImage");
        }

        string[] dirFiles = Directory.GetFiles(cl.InFolder);

        foreach (string fileName in dirFiles)
        {
            // list of file extensions (all of these are PEs)
            if (fileName.Contains(".exe")
                || fileName.Contains(".dll")
                || fileName.Contains(".sys")
                || fileName.Contains(".ocx")
                || fileName.Contains(".rll")
                || fileName.Contains(".cpl")
                || fileName.Contains(".scr")
                || fileName.Contains(".winmd"))
            {
                using (BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open)))
                {
                    // read e_lfanew
                    br.BaseStream.Seek(0x3C, SeekOrigin.Begin);

                    uint e_lfanew = br.ReadUInt32();

                    // check for pe header 

                    if (e_lfanew < br.BaseStream.Length - 4)
                    {
                        br.BaseStream.Seek(e_lfanew, SeekOrigin.Begin);

                        byte[] peMagic = br.ReadBytes(2);

                        // skip MZ/NE files in 32bit 
                        if (peMagic[0] == 0x50
                            && peMagic[1] == 0x45)
                        {
                            br.BaseStream.Seek(e_lfanew + 0x08, SeekOrigin.Begin);

                            int timeDateStamp = br.ReadInt32();
                            DateTime date = new DateTime(1970, 1, 1, 1, 1, 1).AddSeconds(timeDateStamp);
                            string dateIso = date.ToString("yyyy-MM-dd HH:mm:ss");

                            // we don't need to distinguish between PE32 (x86) and PE32+ (x86-64) because it just happens to line up where we need it

                            br.BaseStream.Seek(e_lfanew + 0x50, SeekOrigin.Begin);

                            uint sizeOfImage = br.ReadUInt32();
                            string sizeOfImageHex = sizeOfImage.ToString("x");

                            Console.WriteLine($"{fileName}: {dateIso}: {sizeOfImageHex}");
                            
                            if (cl.OutFile != null) bw.WriteLine($"{fileName},{timeDateStamp},{dateIso},{sizeOfImageHex}");

                        }
                    }
                }
            }
        }

        if (cl.OutFile != null) bw.Close();

        if (cl.OutFile != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Done! Written to: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(cl.OutFile);
        }

    }
}

#endregion