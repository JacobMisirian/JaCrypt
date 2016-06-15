using System;
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
        private class Cypher
        {
            public uint Position { get; set; }

            public Cypher()
            {
                Position = 0;
            }

            public uint Increment(int i)
            {
                return Position += (uint)i;
            }
        }

        private Cypher cypher;
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
        /// Encrypt the specified data and key.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="key">Key.</param>
        public byte[] Encrypt(byte[] data, byte[] key)
        {
            cypher = new Cypher();
            init(key);

            byte[] result = new byte[data.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = (byte)(0xFF - ((nextByte() + data[i]) % 0xFF));

            return result;
        }

        private void init(byte[] key)
        {
            a = 0x6B87;
            b = 0x7F43;
            c = 0xA4Ad;
            d = 0xDC3F;
            x = 0;

            for (int i = 0; i < key.Length; i++)
            {
                x ^= key[i];
                cypher.Increment(key[i]);
            }
        }

        private uint a;
        private uint b;
        private uint c;
        private uint d;
        private uint x;

        private uint nextByte()
        {
            a = rotateLeft(a, x);
            b = (b ^ a) - x;
            c = (a + b) & x;
            d ^= x - b;
            x ^= d;

            return ((a * c) + b - x * d ^ x);
        }

        private uint rotateLeft(uint b, uint bits)
        {
            return (uint)(((byte)b << (byte)bits) | ((byte)b >> 32 - (byte)bits));
        }
    }
}