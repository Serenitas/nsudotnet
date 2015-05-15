using System;
using Enigma;

namespace Strekalova.Nsudotnet.Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Отсутствуют аргументы");
                return;
            }
            try
            {
                switch (args[0])
                {
                    case "encrypt":
                        if (args.Length < 4)
                        {
                            Console.WriteLine("В качестве аргументов необходимо указать имя входного файла, тип шифрования и имя выходного файла");
                            return;
                        }
                        var encryptor = new Encryptor();
                        encryptor.Encrypt(args[1], args[3], args[2]);
                        break;
                    case "decrypt":
                        if (args.Length < 5)
                        {
                            Console.WriteLine("В качестве аргументов необходимо указать имя входного файла, тип шифрованияЮ имя файла-ключа и имя выходного файла");
                            return;
                        }
                        var decryptor = new Decryptor();
                        decryptor.Decrypt(args[1], args[4], args[3], args[2]);
                        break;
                    default:
                        Console.WriteLine("Первым аргументом должно быть encrypt или decrypt");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
