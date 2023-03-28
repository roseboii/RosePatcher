using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string basePath = @"C:\Program Files\ROSE Online\";
        string fileName = "trose.exe";
        string patchedFileName = "trose_patched.exe";

        byte[] searchBytes = new byte[] { 0xFF, 0x15, 0x4B, 0xFB, 0x0F, 0x01 };
        byte[] replaceBytes = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };

        string filePath = Path.Combine(basePath, fileName);
        string patchedFilePath = Path.Combine(basePath, patchedFileName);

        byte[] fileBytes = File.ReadAllBytes(filePath);

        int matches = 0;
        for (int i = 0; i < fileBytes.Length - searchBytes.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < searchBytes.Length; j++)
            {
                if (fileBytes[i + j] != searchBytes[j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                matches++;
                Buffer.BlockCopy(replaceBytes, 0, fileBytes, i, replaceBytes.Length);
            }
        }

        if (matches > 0)
        {
            File.WriteAllBytes(patchedFilePath, fileBytes);
            Console.WriteLine("Patched {0} instances of the pattern.", matches);
        }
        else
        {
            Console.WriteLine("Pattern not found in file.");
        }
    }
}