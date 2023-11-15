using ExtSort;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace Alg4
{
    public class MenuActions
    {
        public static void MoveThrough(Menu menu)
        {
            while (true)
            {
                ConsoleHelper.ClearScreen();
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
                        }
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