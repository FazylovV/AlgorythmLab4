
using AlgorythmLab4;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExtSort
{
    public class MultipathSort
    {
        public string FileInput { get; set; } = "data.csv";
        private static int PathCount { get; set; }
        private static int column { get; set; }
        private static readonly List<int> segmentsLength = new();
        private int Delay { get; set; }

        public MultipathSort(int delay)
        {
            Delay = delay;
        }

        public void Sort()
        {
            string[] input = Input().Split(" ");
            File.Delete("data.csv");
            File.Copy(input[0], "data.csv");
            column = int.Parse(input[1]);
            PathCount = int.Parse(input[2]);
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

            Console.Write("Введите количество путей: ");
            string inputPaths = Console.ReadLine();
            int paths;
            while (!int.TryParse(inputPaths, out paths) || paths <= 1)
            {
                Console.Write("Неверный ввод. Введите количество путей: ");
                inputPaths = Console.ReadLine();
            }
            return $"../../../{fileName} {col} {paths}";
        }

        private void SplitToFiles()
        {
            StreamReader sr = new(File.Open(FileInput, FileMode.Open));
            FileWriter[] writers = new FileWriter[PathCount];
            for (int i = 0; i < PathCount; i++)
            {
                writers[i] = new FileWriter($"a{i}.csv");
            }

            int counter = 0;
            int flag = 0;
            var firstStr = sr.ReadLine();
            var secondStr = sr.ReadLine();
            while (true)
            {
                int tempFlag = flag;

                if (secondStr is not null)
                {
                    Console.WriteLine($"Из исходного файла считано 2 идущих подряд элемента:" +
                        $"\r\n{firstStr}" +
                        $"\r\n{secondStr}\r\n");
                    Thread.Sleep(Delay);
                    if (Compare(firstStr, secondStr))
                    {
                        Console.WriteLine($"Значение в 1 строке меньше 2. длина сегмента для файла {writers[flag].FileName} увеличивается на 1.");
                        Thread.Sleep(Delay);
                        counter++;
                    }
                    else
                    {
                        SwitchFlag(ref tempFlag);
                        segmentsLength.Add(counter + 1);
                        Console.WriteLine($"Значение в 1 строке больше 2, конец сегмента, его длина {counter + 1}");
                        Thread.Sleep(Delay);
                        counter = 0;
                    }
                }
                else
                {
                    if(firstStr != null)
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

                writers[flag].Writer.WriteLine(firstStr);
                Console.WriteLine($"в файл {writers[flag].FileName} записана строка\r\n{firstStr}\r\n");
                Thread.Sleep(Delay);

                firstStr = secondStr;
                secondStr = sr.ReadLine();
                flag = tempFlag;
            }
            segmentsLength.Add(counter + 1);
            Console.WriteLine($"длина последнего добавленного сегмента {counter + 1}");
            Thread.Sleep(Delay);

            sr.Close();
            foreach(FileWriter sw in writers)
            {
                sw.Writer.Close();
            }
        }

        private static void SwitchFlag(ref int flag)
        {
            if (flag == PathCount - 1)
            {
                flag = 0;
            }
            else
            {
                flag++;
            }
        }

        private void MergePairs()
        {
            StreamWriter bw = new(File.Create(FileInput));
            int segmentNumber = 0;
            Segment[] segments = new Segment[PathCount];
            for (int i = 0; i < PathCount; i++)
            {
                segments[i] = new Segment(GetSegmentLength(ref segmentNumber), $"a{i}.csv", false, false, "", column);
            }

            while (true)
            {
                bool needForCount = true;
                foreach (Segment segment in segments)
                {
                    needForCount &= segment.Counter == 0;
                }
                if (needForCount)
                {
                    Console.WriteLine($"Сегменты из всех файлов слились в 1 сегмент исходного файла.");
                    Thread.Sleep(Delay);
                    foreach (Segment segment in segments)
                    {
                        segment.Counter = GetSegmentLength(ref segmentNumber);
                        Console.WriteLine($"длина следующего сегмента из файла {segment.FilePath} равна {segment.Counter}\r\n");
                        Thread.Sleep(Delay);
                    }
                }

                foreach (Segment segment in segments)
                {
                    if (!segment.Reader.EndOfStream)
                    {
                        if (segment.Counter > 0)
                        {
                            if (!segment.Picked)
                            {
                                segment.Value = segment.Reader.ReadLine();
                                segment.Picked = true;
                                Console.WriteLine($"Из файла {segment.FilePath} считана строка\r\n{segment.Value}");
                                Thread.Sleep(Delay);
                            }
                        }
                    }
                    else
                    {
                        if(!segment.End)
                        {
                            Console.WriteLine($"Конец файла {segment.FilePath}");
                            Thread.Sleep(Delay);
                        }
                        segment.End = true;
                    }
                }

                bool end = true;
                foreach (Segment segment in segments)
                {
                    end &= segment.End && segment.Picked == false;
                }
                if (end)
                {
                    Console.WriteLine($"\r\nВсе файлы закончились, нет ни одного считанного элемента из файлов. Слияние завершено.");
                    Thread.Sleep(Delay);
                    break;
                }

                Segment temp = null;
                foreach (Segment segment in segments)
                {
                    if (segment.Picked)
                    {
                        temp = segment;
                        break;
                    }
                }

                Console.WriteLine($"\r\nСравниваемые строки:");
                foreach (Segment segment in segments)
                {
                    if (segment.Picked)
                    {
                        Console.WriteLine($"файл {segment.FilePath}: {segment.Value}");
                        if (temp.Compare(segment))
                        {
                            temp = segment;
                        }
                    }
                }
                Thread.Sleep(Delay);

                bw.WriteLine(temp.Value);
                temp.Picked = false;
                temp.Counter--;
                Console.WriteLine($"\r\nВ исходный файл записана строка:\r\n{temp.Value}\r\n" +
                    $"В текущем сегменте файла {temp.FilePath} осталось {temp.Counter} строк\r\n");
                Thread.Sleep(Delay);
            }

            bw.Close();
            foreach (Segment segment in segments)
            {
                segment.Reader.Close();
            }
        }

        private static int GetSegmentLength(ref int segmentNumber)
        {
            if (segmentsLength.Count - 1 >= segmentNumber)
            {
                int temp = segmentsLength[segmentNumber];
                segmentNumber++;
                return temp;
            }

            return 0;
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