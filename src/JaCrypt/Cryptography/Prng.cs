using System;

namespace JaCrypt.Cryptography
{
    public class Prng
    {
        public const uint INITIAL_A = 0x28BD;
        public const uint INITIAL_B = 0x1E347;
        public const uint INITIAL_C = 0x12D7A5;
        public const uint INITIAL_D = 0x75BCEBB;

        private uint seed;

        private uint a;
        private uint b;
        private uint c;
        private uint d;

        public Prng(uint seed, uint a = INITIAL_A, uint b = INITIAL_B, uint c = INITIAL_C, uint d = INITIAL_D)
        {
            this.seed = seed;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public byte NextByte(byte source)
        {
            a ^= (source ^ seed) | b;
            b ^= (c ^ d) ^ seed ^ source;
            c ^= b | seed ^ (d ^ a);
            d ^= a ^ source | seed;
            seed++;
            /*Console.WriteLine("a : {0}", (byte)a);
            Console.WriteLine("b : {0}", (byte)b);
            Console.WriteLine("c : {0}", (byte)c);
            Console.WriteLine("d : {0}", (byte)d);
            */
            return (byte)((a | b) ^ (c ^ d));
        }
    }
}

