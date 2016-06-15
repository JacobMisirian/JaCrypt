using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JaCrypt
{
    /// <summary>
    /// JaCrypter.
    /// </summary>
    public class JaCrypter
    {
        /// <summary>
        /// Decrypt the specified image and key.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="key">Key.</param>
        public Bitmap Decrypt(Bitmap image, byte[] key)
        {
            return Encrypt(image, key);
        }
        /// <summary>
        /// Decrypt the specified data and key.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="key">Key.</param>
        public byte[] Decrypt(byte[] data, byte[] key)
        {
            return Encrypt(data, key);
        }
        /// <summary>
        /// Decrypt the specified source, dest and key.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="dest">Destination.</param>
        /// <param name="key">Key.</param>
        public void Decrypt(Stream source, Stream dest, byte[] key)
        {
            Encrypt(source, dest, key);
        }
        /// <summary>
        /// Encrypt the specified image and key.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="key">Key.</param>
        public Bitmap Encrypt(Bitmap image, byte[] key)
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
            byte[] encrypted = Encrypt(data, key);

            Bitmap result = new Bitmap(image.Width, image.Height);
            k = 0;
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    result.SetPixel(x, y, Color.FromArgb(encrypted[k++], encrypted[k++], encrypted[k++]));
            
            return result;
        }
        /// <summary>
        /// Encrypt the specified data and key.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="key">Key.</param>
        public byte[] Encrypt(byte[] data, byte[] key)
        {
            init(key);

            byte[] result = new byte[data.Length];

            int keyPos = 0;
            for (int i = 0; i < result.Length; i++)
            {
                long b = nextByte(keyPos < key.Length ? key[keyPos] = (byte)nextByte(key[keyPos++]) : key[keyPos = 0]) + data[i];
                if (b == 0xFF)
                    b = 0;
                result[i] = (byte)(0xFF - b);
            }
            return result;
        }
        /// <summary>
        /// Encrypt the specified source, dest and key.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="dest">Destination.</param>
        /// <param name="key">Key.</param>
        public void Encrypt(Stream source, Stream dest, byte[] key)
        {
            init(key);

            int keyPos = 0;
            while (source.Position < source.Length)
            {
                long b = nextByte(keyPos < key.Length ? key[keyPos] = (byte)nextByte(key[keyPos++]) : key[keyPos = 0]) + source.ReadByte();
                if (b == 0xFF)
                    b = 0;
                dest.WriteByte((byte)(0xFF - b));
            }
        }

        private void init(byte[] key)
        {
            a = 0x6B87;
            b = 0x7F43;
            c = 0xA4Ad;
            d = (uint)key.Length;
            x = 0;
            accum = 0;

            for (int i = 0; i < key.Length; i++)
                x ^= key[i];
        }

        private uint a;
        private uint b;
        private uint c;
        private uint d;
        private uint x;
        private uint accum;

        private uint nextByte(byte k)
        {
            accum |= k;
            a = rotateLeft(a, x) ^ k;
            b = (k ^ a) - x * accum;
            c = (a + accum) & x;
            d ^= x - k ^ accum;
            x ^= d + accum;

            return ((a * c) + b - x * d ^ k) * accum;
        }

        private uint rotateLeft(uint b, uint bits)
        {
            return (uint)(((byte)b << (byte)bits) | ((byte)b >> 32 - (byte)bits));
        }
    }
}