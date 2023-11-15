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
                SplitToFiles();
                if (segments == 1)
                {
                    File.Delete("../../../sortedData.csv");
                    File.Copy("data.csv", "../../../sortedData.csv");
                    break;
                }
                MergePairs();
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
                    flag = !flag;
                    counter = 0;
                    segments++;
                }

                if (flag)
                {
                    writerA.WriteLine(br.ReadLine());
                    counter++;
                }
                else
                {
                    writerB.WriteLine(br.ReadLine());
                    counter++;
                }
            }
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
                    break;
                }

                if (counterA == 0 && counterB == 0)
                {
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
                        }
                    }
                }
                else
                {
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
                        }
                    }
                }
                else
                {
                    endB = true;
                }

                if (endA && endB && pickedA == false && pickedB == false)
                {
                    break;
                }
                if (pickedA)
                {
                    if (pickedB)
                    {
                        if (Compare(elementA, elementB))
                        {
                            bw.WriteLine(elementA);
                            counterA--;
                            pickedA = false;
                        }
                        else
                        {
                            bw.WriteLine(elementB);
                            counterB--;
                            pickedB = false;
                        }
                    }
                    else
                    {
                        bw.WriteLine(elementA);
                        counterA--;
                        pickedA = false;
                    }
                }
                else if (pickedB)
                {
                    bw.WriteLine(elementB);
                    counterB--;
                    pickedB = false;
                }

            }

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