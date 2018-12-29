using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace encryptionProject
{
    class MyStreamWriter : StreamWriter
    {        
        public MyStreamWriter(Stream stream) 
            : base(stream)
        {}

        public override void WriteLine(string msg)
        {
            DateTime dt = DateTime.Now;
            base.WriteLine(dt + " - " + msg);
        }

    }


    class Program
    {
        static void Main(string[] args)
        {            
            StreamWriter sw = null;
            TextWriter oldOut = Console.Out;
            FileStream fs = null;

            try 
            {
                //fs = new FileStream("./log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                //sw = new StreamWriter(fs);


                //Console.SetOut(sw);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught when redirecting console.WriteLine to text file: " + e.Message);
            }
            
            Console.WriteLine("=== starting encryption program ===");

            //substitution cipher
            string message = "flee at once we are discovered";
            string keyword = "zebras";            

            string cipherText = encodeSubstitution(message, keyword);
            string decryptedMessage = decodeSubstitution(cipherText, keyword);

            Console.WriteLine("Original message: " + message);
            Console.WriteLine("Encrypted message (with substitution): " + cipherText);
            Console.WriteLine("Decoded message: (with substitution): " + decryptedMessage);

            Console.WriteLine("==========================================================================");

            //transposition cipher            
            message = "we are discovered flee at once";
            keyword = "zebras";
            string keyword2 = "stripe";

            cipherText = encodeTransposition(message, keyword);
            string cipherText2 = encodeTransposition(cipherText, keyword2);

            string decryptedMessage2 = decodeTransposition(cipherText2, keyword2);
            decryptedMessage = decodeTransposition(decryptedMessage2, keyword);


            Console.WriteLine("Original message: " + message);
            Console.WriteLine("Encrypted message (with transposition): " + cipherText);
            Console.WriteLine("Encrypted message (with transposition): " + cipherText2);
            Console.WriteLine("Decoded message: (with transposition):  " + decryptedMessage2);
            Console.WriteLine("Decoded message: (with transposition):  " + decryptedMessage);


            Console.WriteLine("=== ending encryption program ===");
            
            //sw.Close();
            //Console.SetOut(oldOut);            
            Console.WriteLine("done");
            Console.ReadLine();

        }
               
        static string encodeSubstitution(string message, string keyword)
        {
            string encyptedMessage = "";
            
            Dictionary<string, string> cipherDict = createCipherDictionary(keyword);

            List<string> plainText = new List<string>();

            message = message.Replace(".", "").Replace(" ", "");

            for (int i = 0; i < message.Length; i++)
            {
                string plainChar = cipherDict[message[i].ToString()];
                plainText.Add(plainChar);
            }

            encyptedMessage = String.Join("", plainText.ToArray());

            int groupCount = 5;
            for (int i = encyptedMessage.Length / groupCount; i >= 0; i--)
            {
                encyptedMessage = encyptedMessage.Insert(i * groupCount, " ");
            }

            return encyptedMessage;
        }

        static string decodeSubstitution(string cipherText, string keyword)
        {
            string ret = "";

            Dictionary<string, string> cipherDict = createCipherDictionary(keyword, "decode");

            List<string> plainText = new List<string>();

            for (int i = 0; i < cipherText.Length; i++)
            {
                string plainChar = cipherDict[cipherText[i].ToString()];
                plainText.Add(plainChar);
            }

            ret = String.Join("", plainText.ToArray());

            return ret;
        }



        static Dictionary<string, string> createCipherDictionary(string keyword, string type = "encode")
        {
            Dictionary<string, string> cipher = new Dictionary<string, string>();

            string keys = "abcdefghijklmnopqrstuvwxyz ";
            string values = createKey(keyword);

            if(type == "decode")
            {
                string temp = keys;
                keys = values;
                values = temp;
            }
            
            for (int i = 0; i < keys.Length; i++)
            {
                cipher.Add(keys[i].ToString(), values[i].ToString());
            }
            return cipher;
        }

        static string createKey(string keyword)
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

        static string encodeTransposition(string message, string keyword)
        {
            string encyptedMessage = "";

            message = message.Replace(" ", "");

            int colLength = keyword.Length;

            string[] cols = new string[colLength];
            string[] colsFinal = new string[colLength];

            //add padding to message
            int paddingLength = colLength - (message.Length % colLength);

            if (paddingLength < colLength)
            {
                for (int i = 0; i < paddingLength; i++)
                {
                    message += "X";
                }
            }

            for(int i = 0; i < message.Length; i++)
            {
                int j = i % colLength;
                cols[j] += message[i];
            }

            //find ordering of keyword
            Tuple<string, int>[] keywordOrder = new Tuple<string, int>[colLength];
            
            for(int i = 0; i < colLength; i++)
            {
                keywordOrder[i] = new Tuple<string, int>(keyword[i].ToString(), i);
            }

            List<Tuple<string, int>> sorted = keywordOrder
                .Select((x, i) => new Tuple<string, int>(x.Item1, i))
                .OrderBy(x => x.Item1)
                .ToList();

            List<int> sortedIndex = sorted.Select(x => x.Item2).ToList();

            //reorder cols to ordering of keyword
            for(int i = 0; i < colLength; i++)
            {
                colsFinal[i] = cols[sortedIndex[i]];
            }

            encyptedMessage = String.Join("", colsFinal.ToArray());

            int groupCount = 5;
            for (int i = encyptedMessage.Length / groupCount; i >= 0; i--)
            {
                encyptedMessage = encyptedMessage.Insert(i * groupCount, " ");
            }

            return encyptedMessage;
        }

        static string decodeTransposition(string cipherText, string keyword)
        {
            string plainText = "";

            cipherText = cipherText.Replace(" ", "");

            int colLength = keyword.Length;
            int rowLength = cipherText.Length / colLength;

            string[] cols = new string[colLength];
            string[] colsFinal = new string[colLength];

            int cipherTextIndex = 0; 
            for(int i = 0; i < colLength; i++)
            {
                for(int j = 0; j < rowLength; j++)
                {
                    cols[i] += cipherText[cipherTextIndex];
                    cipherTextIndex++;
                }
            }

            //find ordering of keyword
            Tuple<string, int>[] keywordOrder = new Tuple<string, int>[colLength];

            for (int i = 0; i < colLength; i++)
            {
                keywordOrder[i] = new Tuple<string, int>(keyword[i].ToString(), i);
            }

            List<Tuple<string, int>> sorted = keywordOrder
                .Select((x, i) => new Tuple<string, int>(x.Item1, i))
                .OrderBy(x => x.Item1)
                .ToList();

            List<int> sortedIndex = sorted.Select(x => x.Item2).ToList();

            //reorder cols to ordering of keyword
            for (int i = 0; i < colLength; i++)
            {
                int colFinalIndex = sortedIndex[i];
                int colIndex = i;
                colsFinal[colFinalIndex] = cols[i];
            }

            //probably a more graceful way to do all this...but whatever it works :)
            int plainTextCount = 0;
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colsFinal.Length; j++)
                {
                    plainText += colsFinal[j][i];
                    plainTextCount++;
                }
            }

            int groupCount = 5;
            for (int i = plainText.Length / groupCount; i >= 0; i--)
            {
                plainText = plainText.Insert(i * groupCount, " ");
            }


            return plainText;
        }

    }
}
