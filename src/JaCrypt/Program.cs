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


            crypto.Encrypt(new FileStream(args[0], FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None), 
                new FileStream(args[1], FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None),
                ASCIIEncoding.ASCII.GetBytes(Console.ReadLine()));


           // File.WriteAllBytes(args[1], crypto.Encrypt(File.ReadAllBytes(args[0]), ASCIIEncoding.ASCII.GetBytes(Console.ReadLine())));
        }
    }
}

