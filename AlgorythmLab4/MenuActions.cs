using ExtSort;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using AlgorythmLab4.Logger;
using AlgorythmLab4.Task1;
using WordCounter;

namespace Alg4
{
    public class MenuActions
    {
        public static void MoveThrough(Menu menu)
        {
            while (true)
            {
                //ConsoleHelper.ClearScreen();
                Console.Clear();
                ShowTheMenu(menu);
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                switch (pressedKey.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (menu.SelectedItemIndex == menu.Items.Count - 1)
                            menu.SelectedItemIndex = 0;
                        else
                            menu.SelectedItemIndex++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (menu.SelectedItemIndex == 0)
                            menu.SelectedItemIndex = menu.Items.Count - 1;
                        else
                            menu.SelectedItemIndex--;
                        break;
                    case ConsoleKey.Enter:
                        switch (menu.Items[menu.SelectedItemIndex].Caption)
                        {
                            case "exit":
                                Environment.Exit(0);
                                break;
                            case "Прямое слияние":
                                new ExternalSort(InputDelay()).Sort();
                                break;
                            case "Естественное слияние":
                                new NaturalSort(InputDelay()).Sort();
                                break;
                            case "Многопутевое слияние":
                                new MultipathSort(InputDelay()).Sort();
                                break;
                            case "Bubble Sort":
                                InnerSorts.SortLogger = Logger.GetLogger(1, InputDelay());
                                InputArray().BubbleSort();
                                Console.ReadKey();
                                break;
                            case "Exchange Sort":
                                InnerSorts.SortLogger = Logger.GetLogger(1, InputDelay());
                                InputArray().ExchangeSort();
                                Console.ReadKey();
                                break;
                            case "Quick Sort":
                                InnerSorts.SortLogger = Logger.GetLogger(1, InputDelay());
                                InputArray().QuickSort();
                                Console.ReadKey();
                                break;
                            case "Merge Sort":
                                InnerSorts.SortLogger = Logger.GetLogger(1, InputDelay());
                                InputArray().MergeSort();
                                Console.ReadKey();
                                break;
                            case "Лексико-графическая сортировка":
                                InputOutput io = new InputOutput();
                                io.ConsoleOut(io.ReadFile());
                                break;
                        }
                        ConsoleHelper.ClearScreen();
                        break;
                }
            }
        }

        private static int InputDelay()
        {
            Console.Write("Введите задержку в мс:");
            string input = Console.ReadLine();
            int delay;
            while(!int.TryParse(input, out delay) || delay < 0)
            {
                Console.Write("Неверный ввод. Введите задержку в мс:");
                input = Console.ReadLine();
            }

            return delay;
        }

        public static List<int> InputArray()
        {
            Console.WriteLine("Введите числа через пробел, например: \"324 87 126\"");
            Console.Write("Ввод:");
            string str = Console.ReadLine();
            
            string[] nums = str != null ? str.Split(" ") : Array.Empty<string>();
            List<int> list = new List<int>();

            foreach (var num in nums)
            {
                list.Add(int.Parse(num));
            }

            return list;
        }

        private static void ShowTheMenu(Menu menu)
        {
            if (menu.Items.Count > 0)
            {
                for (int i = 0; i < menu.Items.Count; i++)
                {
                    if (i == menu.SelectedItemIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(menu.Items[i].Caption);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(menu.Items[i].Caption);
                    }
                }
            }
        }
    }
}