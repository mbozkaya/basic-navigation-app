using System;

namespace NavigationApp
{
    public class Program
    {
        public static bool isMenuShow { get; set; } = true;
        public static bool isMapSizeRight { get; set; } = true;
        public static string message { get; set; } = string.Empty;
        public static int mapSize { get; set; } = 20;
        private static Map map { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to The Navigation System v1.0.0");
            Console.WriteLine();
            Console.WriteLine();

            TakeArraySize();
            map = new Map(mapSize);


            while (isMenuShow)
            {
                Menu();

                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine(message);
                }
            }

        }

        public static void Menu()
        {
            Console.Clear();
            map.Print();
            
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("1 - Print Map");
            Console.WriteLine("2 - Find Nearest Building");
            Console.WriteLine("3 - Go");
            Console.WriteLine("4 - Info");
            Console.WriteLine("5 - Close The Navigation System");

            switch (Console.ReadLine())
            {
                case "1":
                    TakeArraySize();
                    map = new Map(mapSize);
                    break;
                case "2":
                    map.Find();
                    break;
                case "3":
                    map.Go();
                    break;
                case "4":
                    map.Info();
                    break;
                case "5":
                    isMenuShow = false;

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Good Bye");

                    Console.ReadKey();

                    break;
                default:
                    message = "Plese press valid key";
                    break;
            }
        }

        public static void TakeArraySize()
        {
            do
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine();
                    Console.WriteLine(message);
                    Console.WriteLine();
                }

                Console.WriteLine("Please enter map size:");

                string mapSizeKey = Console.ReadLine();

                double numericValue = 0;

                if (double.TryParse(mapSizeKey, out numericValue))
                {
                    mapSize = (int)numericValue;
                    isMapSizeRight = false;
                }
                else
                {
                    message = "Please enter only numeric character";
                }

            }
            while (isMapSizeRight);
        }
    }

}
