using System;

namespace Strekalova.Nsudotnet.NumberGuesser
{
    class Program
    {
        private static readonly string[] Phrases = { "Знаешь, {0}, угадывальщик из тебя так себе.", "{0}, хватит дурью маяться, иди пиши код.", "Сегодня явно не твой день, {0}.",
                                               "{0}, мне кажется, что удача от тебя отвернулась.", "{0}, серьезно? Ты же опять не угадал!",
                                               "Даже мой кот уже догадался, {0}, какое здесь было число."};

        private const string DefaultName = "Странник";

        static void Main(string[] args)
        {
            Console.WriteLine("Приветствую тебя, странник! Как называть тебя?");
            var s = Console.ReadLine();
            var username = (string.IsNullOrEmpty(s)) ? DefaultName : s;

            var rand = new Random();
            var number = rand.Next(0, 101);

            Console.WriteLine("{0}, я хочу сыграть с тобой в игру. Угадай число от 0 до 100.", username);

            var failsCount = 0;
            var isGuessed = false;
            var tryings = new int[1000];
            var startTime = DateTime.Now;
            var guessTime = startTime.TimeOfDay;

            while (!isGuessed)
            {
                int currentValue;
                s = Console.ReadLine();
                if (s == "q")
                {
                    Console.WriteLine("Не хочешь угадывать - твоё право :/");
                    Console.ReadKey();
                    return;
                }

                try
                {
                    currentValue = int.Parse(s);
                }
                catch (Exception)
                {
                    Console.WriteLine("{0}, ты должен ввести целое число.", username);
                    continue;
                }

                if (currentValue != number)
                {
                    tryings[failsCount] = currentValue;
                    failsCount++;
                    Console.WriteLine(currentValue > number ? "Многовато будет." : "Мало, побольше бы.");
                    if (failsCount % 4 == 0)
                        Console.WriteLine(Phrases[rand.Next(0, Phrases.Length)], username);
                }
                else
                {
                    isGuessed = true;
                    guessTime = DateTime.Now.Subtract(startTime);
                }
            }

            Console.WriteLine("Поздравляю! Ты угадал число c {0} попытки! Это заняло у тебя {1} секунд!", failsCount + 1, guessTime.Seconds);
            for (var i = 0; i < failsCount; i++)
            {
                Console.WriteLine(tryings[i] > number ? "{0} было больше, чем нужно" : "{0} было слишком мало",
                    tryings[i]);
            }
            Console.ReadKey();
        }
    }
}
