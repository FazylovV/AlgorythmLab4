using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;

namespace ExtSort
{
    public class ExternalSort
    {
        public string FileInput { get; set; } = "data.csv";
        private ulong iterations = 1, segments;
        private static int column = 1;
        private int Delay { get; set; }

        public ExternalSort(int delay)
        {
            Delay = delay;
        }

        public void Sort()
        {
            string[] input = Input().Split(" ");
            File.Delete("data.csv");
            File.Copy(input[0], "data.csv");
            column = int.Parse(input[1]);
            while (true)
            {
                Console.WriteLine($"Длины сегментов равны {iterations}");
                SplitToFiles();

                if (segments == 1)
                {
                    File.Delete("../../../sortedData.csv");
                    File.Copy("data.csv", "../../../sortedData.csv");
                    Console.WriteLine("Сортировка завершена. Для продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
                }
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
                MergePairs();
                Console.WriteLine("\r\n\r\n\r\n\r\n\r\n");
            }
        }
        private static string Input()
        {
            Console.Write("Введите имя файла: ");
            string fileName = Console.ReadLine();
            while (!File.Exists($"../../../{fileName}"))
            {
                Console.Write("Файла не существует. Введите имя файла: ");
                fileName = Console.ReadLine();
            }

            StreamReader reader = new StreamReader($"../../../{fileName}");
            int maxColumn = reader.ReadLine().Split(";").Length;
            reader.Close();
            Console.Write("Введите номер столбца: ");
            string input = Console.ReadLine();
            int col;
            while (!int.TryParse(input, out col) || col < 0 || col >= maxColumn)
            {
                Console.Write("Неверный ввод. Введите номер столбца: ");
                input = Console.ReadLine();
            }
            return $"../../../{fileName} {col}";
        }

        private void SplitToFiles()
        {
            segments = 1;
            StreamReader br = new (File.Open(FileInput, FileMode.Open));
            StreamWriter writerA = new (File.Create("a.csv"));
            StreamWriter writerB = new (File.Create("b.csv"));
            ulong counter = 0;
            bool flag = true;
            while (!br.EndOfStream)
            {
                if (counter == iterations)
                {
                    if (flag)
                    {
                        Console.WriteLine($"В файл a.csv записан сегмент длиной {iterations}. " +
                            $"Следующий сегмент будет записан в файл b.csv");
                        Thread.Sleep(Delay);
                    }
                    else
                    {

                        Console.WriteLine($"В файл b.csv записан сегмент длиной {iterations}. " +
                            $"Следующий сегмент будет записан в файл a.csv");
                        Thread.Sleep(Delay);
                    }

                    flag = !flag;
                    counter = 0;
                    segments++;
                }

                if (flag)
                {
                    string line = br.ReadLine();
                    writerA.WriteLine(line);
                    counter++;
                    Console.WriteLine($"В файл a.csv записана строка:\r\n{line}\r\n");
                    Thread.Sleep(Delay);
                }
                else
                {
                    string line = br.ReadLine();
                    writerB.WriteLine(line);
                    counter++;
                    Console.WriteLine($"В файл b.csv записана строка:\r\n{line}\r\n");
                    Thread.Sleep(Delay);
                }
            }
            Console.WriteLine($"Исходный файл разделен.");
            Thread.Sleep(Delay);
            br.Close();
            writerA.Close();
            writerB.Close();
        }

        private void MergePairs()
        {
            StreamReader readerA = new StreamReader(File.Open("a.csv", FileMode.Open));
            StreamReader readerB = new StreamReader(File.Open("b.csv", FileMode.Open));
            StreamWriter bw = new StreamWriter(File.Create(FileInput));
            ulong counterA = iterations, counterB = iterations;
            string elementA = "", elementB = "";
            bool pickedA = false, pickedB = false, endA = false, endB = false;

            while (true)
            {
                if (endA && endB)
                {
                    Console.WriteLine($"Все строки обоих файлов переписаны в исходный. Слияние завершено.");
                    Thread.Sleep(Delay);
                    break;
                }

                if (counterA == 0 && counterB == 0)
                {
                    Console.WriteLine($"Сегменты длиной обоих файлов слились в 1 сегмент исходного файл.\r\n");
                    Thread.Sleep(Delay);
                    counterA = iterations;
                    counterB = iterations;
                }

                if (!readerA.EndOfStream)
                {
                    if (counterA > 0)
                    {
                        if (!pickedA)
                        {
                            elementA = readerA.ReadLine();
                            pickedA = true;
                            Console.WriteLine($"Из файла a.csv считана строка\r\n{elementA}");
                            Thread.Sleep(Delay);
                        }
                    }
                }
                else
                {
                    if (!endA)
                    {
                        Console.WriteLine($"Конец файла a.csv");
                        Thread.Sleep(Delay);
                    }
                    endA = true;
                }

                if (!readerB.EndOfStream)
                {
                    if (counterB > 0)
                    {
                        if (!pickedB)
                        {
                            elementB = readerB.ReadLine();
                            pickedB = true;
                            Console.WriteLine($"Из файла b.csv считана строка\r\n{elementB}");
                            Thread.Sleep(Delay);
                        }
                    }
                }
                else
                {
                    if (!endB)
                    {
                        Console.WriteLine($"Конец файла b.csv]\r\n");
                        Thread.Sleep(Delay);
                    }
                    endB = true;
                }

                if (endA && endB && pickedA == false && pickedB == false)
                {
                    Console.WriteLine($"\r\nВсе файлы закончились, нет ни одного считанного элемента из файлов. Слияние завершено.");
                    Thread.Sleep(Delay);
                    break;
                }
                if (pickedA)
                {
                    if (pickedB)
                    {
                        Console.WriteLine($"\r\nСравниваемые строки:");
                        Console.WriteLine($"файл a.csv: {elementA}");
                        Console.WriteLine($"файл b.csv: {elementB}");
                        Thread.Sleep(Delay);

                        if (Compare(elementA, elementB))
                        {
                            bw.WriteLine(elementA);
                            counterA--;
                            pickedA = false;
                            Console.WriteLine($"\r\nВ исходный файл записана строка:\r\n{elementA}\r\n" +
                                $"В текущем сегменте файла a.csv осталось {counterA} строк\r\n");
                        }
                        else
                        {
                            bw.WriteLine(elementB);
                            counterB--;
                            pickedB = false;
                            Console.WriteLine($"\r\nВ исходный файл записана строка:\r\n{elementB}\r\n" +
                                $"В текущем сегменте файла b.csv осталось {counterB} строк\r\n");
                        }
                    }
                    else
                    {
                        bw.WriteLine(elementA);
                        counterA--;
                        pickedA = false;

                        Console.WriteLine($"\r\nСтроку из файла a.csv не с чем сравнивать.");
                        Console.WriteLine($"В исходный файл записана строка:\r\n{elementA}\r\n" +
                            $"В текущем сегменте файла a.csv осталось {counterA} строк\r\n");
                        Thread.Sleep(Delay);
                    }
                }
                else if (pickedB)
                {
                    bw.WriteLine(elementB);
                    counterB--;
                    pickedB = false;

                    Console.WriteLine($"\r\nСтроку из файла b.csv не с чем сравнивать.");
                    Console.WriteLine($"В исходный файл записана строка:\r\n{elementB}\r\n" +
                        $"В текущем сегменте файла b.csv осталось {counterB} строк\r\n");
                    Thread.Sleep(Delay);
                }
            }


            Console.WriteLine($"Длина сегментов удваивается.\r\n");
            Thread.Sleep(Delay);
            iterations *= 2;

            bw.Close();
            readerA.Close();
            readerB.Close();
        }

        private static bool Compare(string firstLine, string secondLine)
        {
            if (double.TryParse(firstLine.Split(';')[column], out double fDouble))
            {
                double sDouble = double.Parse(secondLine.Split(';')[column]);
                return fDouble < sDouble;
            }

            if (DateTime.TryParse(firstLine.Split(';')[column], out DateTime fDate))
            {
                DateTime sDate = DateTime.Parse(secondLine.Split(';')[column]);
                return fDate < sDate;
            }

            return string.Compare(firstLine.Split(';')[column], secondLine.Split(';')[column]) < 0;
        }
    }
}