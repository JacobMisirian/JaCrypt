using System;

namespace JaCrypt.Cryptography
{
    public class Prng
    {
        public const uint INITIAL_A = 0x1234;
        public const uint INITIAL_B = 0xABCDE;
        public const uint INITIAL_C = 0x141592;
        public const uint INITIAL_D = 0x3183098;

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
            c ^= b | seed ^ (d ^ source);
            d ^= a ^ source | seed;
            seed += source;
            return (byte)((a | b) ^ (c ^ d));
        }
    }
}

