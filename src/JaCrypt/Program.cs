using System;
using System.IO;
using System.Text;

namespace JaCrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            JaCrypter crypto = new JaCrypter();

            File.WriteAllBytes(args[1], crypto.Encrypt(File.ReadAllBytes(args[0]), ASCIIEncoding.ASCII.GetBytes(Console.ReadLine())));
        }
    }
}

