using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIM
{
    public static class Cipherer
    {
        public static string Cipher(string RawData, int Key)
        {
            StringBuilder CipheredText = new StringBuilder();

            byte[] RawBytes = UnicodeEncoding.UTF8.GetBytes(RawData);

            int keyLen = Key.ToString().Length;
            int keyPos = 0;

            foreach (byte rawByte in RawBytes)
            {
                keyPos = keyPos == keyLen ? 0 : keyPos;
                int cipheredByte = rawByte + int.Parse(Key.ToString()[keyPos++].ToString());
                CipheredText.Append(cipheredByte.ToString());
                CipheredText.Append("-");
            }
            return CipheredText.ToString(); //ciphered value
        }

        public static string DeCipher(string EncryptedData, int Key)
        {
            char[] seperator = { '-' };
            string[] splitted = EncryptedData.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            List<byte> BYTES = new List<byte>();

            int keyLen = Key.ToString().Length;
            int keyPos = 0;

            foreach (string s in splitted)
            {
                keyPos = keyPos == keyLen ? 0 : keyPos;
                int x = int.Parse(s) - int.Parse(Key.ToString()[keyPos++].ToString());
                BYTES.Add(byte.Parse(x.ToString()));
            }

            byte[] ENDBYTES = new byte[BYTES.Count];
            int counter = 0;
            foreach (byte item in BYTES)
            {
                ENDBYTES[counter++] = item;
            }
            return UTF8Encoding.UTF8.GetString(ENDBYTES);// deciphered data
        }
    }
}
