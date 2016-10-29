using System;
using System.Drawing;
using System.IO;

using JaCrypt.Cryptography;

namespace JaCrypt
{
    public class JaCryptConfig
    {
        public string KeyFile { get; set; }
        public string InputFile { get; set; }
        public string OutputFile { get; set; }

        public EncryptionMode EncryptionMode { get; set; }

        public JaCryptConfig()
        {
            KeyFile = string.Empty;
            InputFile = string.Empty;
            OutputFile = string.Empty;
            EncryptionMode = EncryptionMode.FromFile;
        }

        public void ExecuteFromConfig()
        {
            resolveOutputFile();

            JaCrypto crypto = new JaCrypto();
            if (EncryptionMode == EncryptionMode.FromFile)
                crypto.Encrypt(File.ReadAllBytes(KeyFile), new FileStream(InputFile, FileMode.Open, FileAccess.Read, FileShare.Read), new FileStream(OutputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite));
            else
                crypto.Encrypt(File.ReadAllBytes(KeyFile), (Bitmap)Bitmap.FromFile(InputFile)).Save(OutputFile);
        }

        private void resolveOutputFile()
        {
            if (OutputFile == string.Empty)
            {
                if (EncryptionMode == EncryptionMode.FromFile)
                {
                    if (Path.GetExtension(InputFile) != ".jc")
                        OutputFile = string.Format("{0}.jc", InputFile);
                    else
                        OutputFile = Path.GetFileNameWithoutExtension(InputFile);
                }
                else
                {
                    if (!Path.GetFileNameWithoutExtension(InputFile).EndsWith(".jc"))
                        OutputFile = string.Format("{0}.jc{1}", Path.GetFileNameWithoutExtension(InputFile), Path.GetExtension(InputFile));
                    else
                        OutputFile = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(InputFile)), Path.GetExtension(InputFile));
                }
            }
        }
    }

    public enum EncryptionMode
    {
        FromFile,
        FromImage
    }
}

