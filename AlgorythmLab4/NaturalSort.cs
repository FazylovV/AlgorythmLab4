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
                    if (Compare(firstStr, secondStr))
                    {
                        counter++;
                    }
                    else
                    {
                        tempFlag = !tempFlag;
                        segmentsLength.Add(counter + 1);
                        counter = 0;
                    }
                }

                if (firstStr == null)
                {
                    break;
                }

                if (flag)
                {
                    writerA.WriteLine(firstStr);
                }
                else
                {
                    writerB.WriteLine(firstStr);
                }

                firstStr = secondStr;
                secondStr = sr.ReadLine();
                flag = tempFlag;
            }
            segmentsLength.Add(counter + 1);

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
                    if (segmentsLength.Count - 1 >= segmentNumber)
                    {
                        counterA = segmentsLength[segmentNumber];
                        segmentNumber++;
                    }
                    if (segmentsLength.Count - 1 >= segmentNumber)
                    {
                        counterB = segmentsLength[segmentNumber];
                        segmentNumber++;
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