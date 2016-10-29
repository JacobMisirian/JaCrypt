using System;

namespace JaCrypt
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new JaCryptArgumentParser().Parse(args).ExecuteFromConfig();
        }
    }
}