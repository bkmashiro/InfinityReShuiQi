﻿namespace InfinityReShuiQi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cracker = new Cracker();
            cracker.CrackKeyA();
            //TODO
        }
    }

    class Cracker
    {
        readonly Logger logger = new($"./infinityWater-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log");

        public bool CrackKeyA()
        {
            byte[] curKey = new byte[6];
            byte[] serialNo = new byte[4];
            byte[] data = new byte[48];
            byte controlWord = IC.BLOCK0_EN | IC.BLOCK1_EN | IC.BLOCK2_EN | IC.EXTERNKEY;
            byte sectorNo = 10;
            byte authMode = 1;

            // a sector has 6B
            // enumerate every possible combination.
            for (long i = 0x00_00_00_00_00_00; i <= 0xFF_FF_FF_FF_FF_FF; i++)
            {
                byte i0 = (byte)((i >> 050) & 0xFF);
                byte i1 = (byte)((i >> 040) & 0xFF);
                byte i2 = (byte)((i >> 030) & 0xFF);
                byte i3 = (byte)((i >> 020) & 0xFF);
                byte i4 = (byte)((i >> 010) & 0xFF);
                byte i5 = (byte)((i >> 000) & 0xFF);

                curKey[0] = i0; curKey[1] = i1; curKey[2] = i2; curKey[3] = i3; curKey[4] = i4; curKey[5] = i5;

                byte status = IC.piccreadex(controlWord, serialNo, sectorNo, authMode, curKey, data);
                _ = logger.LogAsync($"Key {i:X} status {status}");
                switch (status)
                {
                    case 0:
                        Console.WriteLine($"Crack success! KeyA: {i:X}");
                        _ = logger.LogAsync($"*** Key {i:X} Success!");
                        return true;
                    case 8:
                        Console.WriteLine($"Card not found, halt at ${i:X}");
                        break;
                    case 11:
                        Console.WriteLine($"Cannot load key, halt at ${i:X}");
                        break;
                    case 12:
                        // This is normal case, this key is wrong.
                        continue;
                    default:
                        Console.WriteLine($"Unknown error, halt at ${i:X}");
                        break;
                }
            }

            return false;
        }
    }
}