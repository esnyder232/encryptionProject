using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryptionProject
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("=== starting encryption program ===");

            Dictionary<string, string> cipher = null;
            Dictionary<string, string> reverseCipher = null;

            string key = "abcdefghijklmnopqrstuvwxyz ";
            //string value = "zebrascdfghijklmnopqtuvwxy ";

            string keyWord = "zebras";
            string value = createValues(keyWord);

            cipher = createCipher(key, value);
            reverseCipher = createCipher(value, key);

            string message = "hello world this is a secret message";

            string cipherText = encode(message, cipher);
            string decryptedMessage = encode(cipherText, reverseCipher);

            Console.WriteLine("Original message: " + message);
            Console.WriteLine("Encrypted message: " + cipherText);
            Console.WriteLine("Decoded message: " + decryptedMessage);
            Console.WriteLine("=== ending encryption program ===");
            Console.ReadLine();
        }

        static string encode(string message, Dictionary<string, string> cipher)
        {
            string ret = "";

            List<string> plainText = new List<string>();

            message = message.Replace(".", "").Replace(" ", "");

            for (int i = 0; i < message.Length; i++)
            {
                string plainChar = cipher[message[i].ToString()];
                plainText.Add(plainChar);
            }

            string encyptedMessage = String.Join("", plainText.ToArray());

            int groupCount = 5;
            for (int i = encyptedMessage.Length / groupCount; i >= 0; i--)
            {
                encyptedMessage = encyptedMessage.Insert(i * groupCount, " ");
            }

            return encyptedMessage;
        }

        static string decode(string message, Dictionary<string, string> cipher)
        {
            string ret = "";

            return ret;
        }

        static Dictionary<string, string> createCipher(string keys, string values)
        {
            Dictionary<string, string> cipher = new Dictionary<string, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                cipher.Add(keys[i].ToString(), values[i].ToString());
            }
            return cipher;
        }

        static string createValues(string keyword)
        {
            string ret = "";

            string alphabet = "abcdefghijklmnopqrstuvwxyz ";
            string remains = alphabet;

            for (int i = 0; i < keyword.Length; i++)
            {
                remains = remains.Replace(keyword[i].ToString(), "");
            }

            ret = keyword + remains;

            return ret;
        }
    }
}
