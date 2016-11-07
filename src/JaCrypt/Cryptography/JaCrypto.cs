using System;
using System.Drawing;
using System.IO;

namespace JaCrypt.Cryptography
{
    public class JaCrypto
    {
        private Prng prng;

        
        public Bitmap Encrypt(byte[] key, Bitmap image)
        {
            byte[] data = new byte[(image.Width * image.Height) * 3];
            int k = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    data[k++] = pixel.R;
                    data[k++] = pixel.G;
                    data[k++] = pixel.B;
                }
            }
            byte[] encrypted = Encrypt(key, data);

            Bitmap result = new Bitmap(image.Width, image.Height);
            k = 0;
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    result.SetPixel(x, y, Color.FromArgb(encrypted[k++], encrypted[k++], encrypted[k++]));

            return result;
        }

        public byte[] Encrypt(byte[] key, byte[] data)
        {
            uint seed = 0;
            prng = new Prng((uint)key.Length);
            for (int i = 0; i < key.Length; i++)
                seed += prng.NextByte((byte)(key[i] * i ^ seed));
            prng = new Prng(seed);

            byte[] result = new byte[data.Length];

            for (int i = 0; i < result.Length; i++)
            {
                byte b = prng.NextByte(key[i % key.Length]);
                key[i % key.Length] += b;
                result[i] = (byte)(0xFF - (b + data[i]));
            }

            return result;
        }
        public Stream Encrypt(byte[] key, Stream data, Stream output)
        {
            uint seed = 0;
            prng = new Prng((uint)key.Length);
            for (int i = 0; i < key.Length; i++)
                seed += prng.NextByte((byte)(key[i] * i ^ seed));
            prng = new Prng(seed);

            while (data.Position < data.Length)
            {
                byte b = prng.NextByte(key[data.Position % key.Length]);
                key[data.Position % key.Length] += b;
                output.WriteByte((byte)(0xFF - (b + data.ReadByte())));
                output.Flush();
            }
            return output;
        }
    }
}

