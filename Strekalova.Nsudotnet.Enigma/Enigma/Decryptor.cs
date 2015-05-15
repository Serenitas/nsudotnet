using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Enigma
{
    public class Decryptor
    {
        private SymmetricAlgorithm _algorithm;

        public void Decrypt(string inputFileName, string outputFileName, string keyFileName, string algorithmName)
        {
            switch (algorithmName)
            {
                case "aes":
                    _algorithm = new AesCryptoServiceProvider();
                    break;
                case "des":
                    _algorithm = new DESCryptoServiceProvider();
                    break;
                case "rc2":
                    _algorithm = new RC2CryptoServiceProvider();
                    break;
                case "rijndael":
                    _algorithm = new RijndaelManaged();
                    break;
                default:
                    throw new Exception("Неизвестный алгоритм шифрования");
            }

            byte[] key, IV;

            using (var fin = new FileStream(keyFileName, FileMode.Open))
            {
                using (var sw = new StreamReader(fin))
                {
                    key = Convert.FromBase64String(sw.ReadLine());
                    IV = Convert.FromBase64String(sw.ReadLine());
                }
            }

            using (var fin = new FileStream(inputFileName, FileMode.Open))
            {
                using (var fout = new FileStream(outputFileName, FileMode.Create))
                {
                    using (var cs = new CryptoStream(fin, _algorithm.CreateDecryptor(key, IV), CryptoStreamMode.Read))
                    {
                        cs.CopyTo(fout);
                    }
                }
            }
        }
    }
}
