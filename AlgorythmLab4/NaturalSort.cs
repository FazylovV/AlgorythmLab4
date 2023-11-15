using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ExtSort
{
    public class NaturalSort
    {
        public string FileInput { get; set; } = "data.csv";
        private static int column;
        private readonly List<int> segmentsLength = new();
        private int Delay {  get; set; }

        public NaturalSort(int delay)
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
                segmentsLength.Clear();
                SplitToFiles();
                if (segmentsLength.Count == 1)
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
            StreamReader sr = new StreamReader(File.Open(FileInput, FileMode.Open));
            StreamWriter writerA = new StreamWriter(File.Create("a.csv"));
            StreamWriter writerB = new StreamWriter(File.Create("b.csv"));
            int counter = 0;
            bool flag = true;
            var firstStr = sr.ReadLine();
            var secondStr = sr.ReadLine();
            while (true)
            {
                bool tempFlag = flag;

                if (secondStr is not null)
                {
                    Console.WriteLine($"Из исходного файла считано 2 идущих подряд элемента:" +
                        $"\r\n{firstStr}" +
                        $"\r\n{secondStr}\r\n");
                    Thread.Sleep(Delay);
                    if (Compare(firstStr, secondStr))
                    {
                        Console.WriteLine($"Значение в 1 строке меньше 2. длина сегмента для файла a.csv увеличивается на 1.");
                        Thread.Sleep(Delay);
                        counter++;
                    }
                    else
                    {
                        tempFlag = !tempFlag;
                        segmentsLength.Add(counter + 1);
                        Console.WriteLine($"Значение в 1 строке больше 2, конец сегмента, его длина {counter + 1}");
                        Thread.Sleep(Delay);
                        counter = 0;
                    }
                }
                else
                {
                    if (firstStr != null)
                    {
                        Console.WriteLine($"считана единственная строка из исходного файла.\r\n{firstStr}");
                        Thread.Sleep(Delay);
                    }
                }

                if (firstStr == null)
                {
                    Console.WriteLine($"не считано никаих строк. конец исходного файла.");
                    Thread.Sleep(Delay);
                    break;
                }

                if (flag)
                {
                    Console.WriteLine($"в файл a.csv записана строка\r\n{firstStr}\r\n");
                    writerA.WriteLine(firstStr);
                }
                else
                {
                    Console.WriteLine($"в файл b.csv записана строка\r\n{firstStr}\r\n");
                    writerB.WriteLine(firstStr);
                }

                firstStr = secondStr;
                secondStr = sr.ReadLine();
                flag = tempFlag;
            }
            segmentsLength.Add(counter + 1);
            Console.WriteLine($"длина последнего добавленного сегмента {counter + 1}");
            Thread.Sleep(Delay);

            sr.Close();
            writerA.Close();
            writerB.Close();
        }

        private void MergePairs()
        {
            StreamReader readerA = new StreamReader(File.Open("a.csv", FileMode.Open));
            StreamReader readerB = new StreamReader(File.Open("b.csv", FileMode.Open));
            StreamWriter bw = new StreamWriter(File.Create(FileInput));
            int segmentNumber = 0;
            int counterA = segmentsLength[segmentNumber];
            segmentNumber++;
            int counterB = segmentsLength[segmentNumber];
            segmentNumber++;
            string elementA = "", elementB = "";
            bool pickedA = false, pickedB = false, endA = false, endB = false;

            while (true)
            {
                if (endA && endB)
                {
                    break;
                }

                if (counterA == 0 && counterB == 0)
                {
                    Console.WriteLine($"Сегменты из обоих файлов слились в 1 сегмент исходного файла.");
                    Thread.Sleep(Delay);
                    if (segmentsLength.Count - 1 >= segmentNumber)
                    {
                        counterA = segmentsLength[segmentNumber];
                        segmentNumber++; Console.WriteLine($"длина следующего сегмента из файла a.csv равна {counterA}\r\n");
                        Thread.Sleep(Delay);
                    }
                    if (segmentsLength.Count - 1 >= segmentNumber)
                    {
                        counterB = segmentsLength[segmentNumber];
                        segmentNumber++;
                        segmentNumber++; Console.WriteLine($"длина следующего сегмента из файла b.csv равна {counterB}\r\n");
                        Thread.Sleep(Delay);
                    }
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
                        Console.WriteLine($"Конец файла b.csv");
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
                            Thread.Sleep(Delay);
                        }
                        else
                        {
                            bw.WriteLine(elementB);
                            counterB--;
                            pickedB = false;
                            Console.WriteLine($"\r\nВ исходный файл записана строка:\r\n{elementB}\r\n" +
                                $"В текущем сегменте файла b.csv осталось {counterB} строк\r\n");
                            Thread.Sleep(Delay);
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

            bw.Close();
            readerA.Close();
            readerB.Close();
        }

        private static bool Compare(string firstLine, string secondLine)
        {
            if (double.TryParse(firstLine.Split(';')[column], out double fDouble))
            {
                double sDouble = double.Parse(secondLine.Split(';')[column]);
                return fDouble <= sDouble;
            }

            if (DateTime.TryParse(firstLine.Split(';')[column], out DateTime fDate))
            {
                DateTime sDate = DateTime.Parse(secondLine.Split(';')[column]);
                return fDate <= sDate;
            }

            return string.Compare(firstLine.Split(';')[column], secondLine) <= 0;
        }
    }
}