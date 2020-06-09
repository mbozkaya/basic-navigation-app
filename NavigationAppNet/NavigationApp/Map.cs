using System;
using System.Collections.Generic;
using System.Linq;

namespace NavigationApp
{
    public class Map
    {
        public char[,] MapArray { get; set; }
        public char[] Item { get; set; } = new char[] { '#', 'H', 'S', 'P' };
        public int[] MapLocation { get; set; }

        public Map(int size)
        {
            Random random = new Random();
            MapArray = new char[size, size];
            MapLocation = new int[2] { size / 2 + 1, size / 2 + 1 };

            for (int x = 0; x < MapArray.GetLength(0); x++)
            {
                for (int y = 0; y < MapArray.GetLength(1); y++)
                {
                    MapArray[x, y] = Item[random.Next(0, 100) < 90 ? 0 : random.Next(0, Item.Length - 1)];
                }
            }
        }

        public void Print(string message = "")
        {
            Console.Clear();
            Console.WriteLine();

            for (int x = 0; x < MapArray.GetLength(0) + 1; x++)
            {
                Console.WriteLine();
                for (int y = 0; y < MapArray.GetLength(1) + 1; y++)
                {
                    string writing = string.Empty;

                    if (x == 0 && y == 0)
                    {
                        writing = $"X|Y";
                    }
                    else if (x == 0)
                    {
                        writing = $"{y - 1}";
                    }
                    else
                    {
                        if (y == 0)
                        {
                            writing = $"{x - 1}";
                        }
                        else
                        {
                            if (x == MapLocation[0] && y == MapLocation[1])
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            writing = $"{MapArray[x - 1, y - 1]}";
                        }

                    }

                    while (writing.Length < 3)
                    {
                        writing += " ";
                    }

                    Console.Write($"{writing}");
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

        }

        public void Go()
        {
            bool isExit = true;

            while (isExit)
            {
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("- - Use Only Direction Keys");
                Console.WriteLine("- - Go Back to Menu Press ESC");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++");

                ConsoleKeyInfo consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.X || consoleKey.Key == ConsoleKey.Escape)
                {
                    isExit = false;
                }
                else if (consoleKey.Key == ConsoleKey.UpArrow)
                {
                    Move(-1, 0);
                }
                else if (consoleKey.Key == ConsoleKey.LeftArrow)
                {
                    Move(0, -1);
                }
                else if (consoleKey.Key == ConsoleKey.RightArrow)
                {
                    Move(0, 1);
                }
                else if (consoleKey.Key == ConsoleKey.DownArrow)
                {
                    Move(1, 0);
                }
                else
                {
                    Print("Please enter valid key");
                }
            }


        }

        private void CurrentLocation()
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine();

            switch (MapArray[MapLocation[0] - 1, MapLocation[1] - 1])
            {
                case '#':
                    Console.WriteLine("Current Location is Default Place");
                    break;
                case 'H':
                    Console.WriteLine("Current Location is Hospital");
                    break;
                case 'S':
                    Console.WriteLine("Current Location is School");
                    break;
                case 'P':
                    Console.WriteLine("Current Location is Police Station");
                    break;
                default:
                    Console.WriteLine("Current Location is Unknown");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("-------------------------------------");
        }

        public void Find()
        {
            Dictionary<double, int[]> distances = new Dictionary<double, int[]>();

            for (int x = 0; x < MapArray.GetLength(0); x++)
            {
                for (int y = 0; y < MapArray.GetLength(1); y++)
                {
                    char building = MapArray[x, y];

                    if (building != '#')
                    {
                        double euclidianDistance = CalculateEuclidian(x, y);

                        distances.TryAdd(euclidianDistance, new int[] { x, y });
                    }
                }
            }

            Console.WriteLine("-------------------------------------");
            Console.WriteLine();

            if (distances.Keys.Count > 0)
            {
                IOrderedEnumerable<KeyValuePair<double, int[]>> orderedDistance = distances.OrderBy(o => o.Key);
                KeyValuePair<double, int[]> closestDistance = orderedDistance.FirstOrDefault();

                Console.WriteLine($"Nearest Building is :{MapArray[closestDistance.Value[0], closestDistance.Value[1]]}");
                Console.WriteLine($"Location X:{closestDistance.Value[0]} Location Y:{closestDistance.Value[1]}");
                Console.WriteLine($"Distance : {Math.Round(closestDistance.Key, 2)}");


            }
            else
            {
                Console.WriteLine("Nearest Building wasn't Found.");
            }

            Console.WriteLine();
            Console.WriteLine("-------------------------------------");

            Console.ReadKey();
        }

        public void Info()
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine();

            CurrentLocation();
            Console.WriteLine("# Default Place");
            Console.WriteLine("H Hospital");
            Console.WriteLine("S School");
            Console.WriteLine("P Police Station");


            Console.WriteLine();
            Console.WriteLine("-------------------------------------");
            Console.ReadKey();

        }

        private double CalculateEuclidian(int x, int y)
        {
            return Math.Sqrt(((x - (MapLocation[0] - 1)) * (x - (MapLocation[0] - 1))) + ((y - (MapLocation[1] - 1)) * (y - (MapLocation[1] - 1))));
        }

        private void Move(int x, int y)
        {
            int newX = MapLocation[0] + x;
            int newY = MapLocation[1] + y;

            string errorMessage = string.Empty;

            if (newX < 1 || newX >= MapArray.GetLength(0) + 1 || newY < 1 || newY >= MapArray.GetLength(1) + 1)
            {
                errorMessage = "You don't go to outside map";
            }
            else
            {
                MapLocation[0] = newX;
                MapLocation[1] = newY;
            }

            Print(errorMessage);
        }
    }
}
