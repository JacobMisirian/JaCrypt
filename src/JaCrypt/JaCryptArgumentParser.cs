using System;
using System.IO;

namespace JaCrypt
{
    public class JaCryptArgumentParser
    {
        private string[] args;
        private int position;

        public JaCryptConfig Parse(string[] args)
        {
            if (args.Length <= 0)
                displayHelp();

            this.args = args;

            JaCryptConfig config = new JaCryptConfig();

            for (position = 0; position < args.Length; position++)
            {
                switch (args[position])
                {
                    case "-f":
                    case "--file":
                        config.InputFile = expectData("file");
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-i":
                    case "--image":
                        config.InputFile = expectData("image file");
                        config.EncryptionMode = EncryptionMode.FromImage;
                        break;
                    case "-k":
                    case "--key":
                        config.KeyFile = expectData("key file");
                        break;
                    case "-o":
                    case "--output":
                        config.OutputFile = expectData("output file");
                        break;
                    default:
                        die(string.Format("Unexpected data or flag {0}!", args[position]));
                        break;
                }
            }

            if (config.InputFile == string.Empty)
                die("No input file specified!");
            if (config.KeyFile == string.Empty)
                die("No key file specified!");
            if (!File.Exists(config.InputFile))
                die("Input file does not exist!");
            if (!File.Exists(config.KeyFile))
                die("Key file does not exist!");

            return config;
        }

        private void displayHelp()
        {
            Console.WriteLine("-h --help                      Displays this help and exits.");
            Console.WriteLine("\nUsage:");
            Console.WriteLine("JaCrypt.exe [KEY] [SOURCE] [OUTPUT]");
            Console.WriteLine("\n[KEY]:");
            Console.WriteLine("-k --key [FILE]                Specifies the file to use as the key.");
            Console.WriteLine("\n[SOURCE]:");
            Console.WriteLine("-f --file [FILE]               Specifies the file to encrypt.");
            Console.WriteLine("-i --image [FILE]              Specifies the bitmap file to encrypt.");
            Console.WriteLine("\n[OUTPUT]:");
            Console.WriteLine("-o [FILE]                      Specifies the output file. Default [INPUT].jc. Must specify if using input string.");
            die(string.Empty);
        }

        private string expectData(string type)
        {
            if (args[++position].StartsWith("-"))
                die(string.Format("Unexpected flag {0}, expected {1}!", args[position], type));
            return args[position];
        }

        private void die(string msg)
        {
            Console.WriteLine(msg);
            Environment.Exit(0);
        }
    }
}

