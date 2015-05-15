using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Enigma
{
    public class Encryptor
    {
        private SymmetricAlgorithm _algorithm;
        private const string KeyFileNameEnd = ".key.txt";

        public void Encrypt(string inputFileName, string outputFileName, string algorithmName)
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

            _algorithm.GenerateIV();
            _algorithm.GenerateKey();

            var key = Convert.ToBase64String(_algorithm.Key);
            var IV = Convert.ToBase64String(_algorithm.IV);

            var keyFileName = string.Concat(inputFileName.Contains('.') ? inputFileName.Substring(0, inputFileName.LastIndexOf('.')) : inputFileName, KeyFileNameEnd);

            using (var fout = new FileStream(keyFileName, FileMode.Create))
            {
                using (var sw = new StreamWriter(fout))
                {
                    sw.WriteLine(key);
                    sw.WriteLine(IV);
                }
            }

            using (var fin = new FileStream(inputFileName, FileMode.Open))
            {
                using (var fout = new FileStream(outputFileName, FileMode.Create))
                {
                    using (var cs = new CryptoStream(fout, _algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        fin.CopyTo(cs);
                    }
                }
            }
        }
    }
}
