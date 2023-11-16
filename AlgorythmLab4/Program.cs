using Alg4;
using AlgorythmLab4.Task1;
using WordCounter;

class Program
{
    static void Main()
    {
        //TimeMeasurer tm = new TimeMeasurer();
        //tm.Start(5000);
        /*InputOutput io = new InputOutput();
        io.ConsoleOut(io.ReadFile());*/


        ABCSortAlgorithm.ABCSort(File.ReadAllLines("../../../test.csv"), 0);
        Console.CursorVisible = false;
        List<MenuItem> menuItems = new List<MenuItem>()
            {
                new MenuItem("Прямое слияние", "ExternalSort.Sort"),
                new MenuItem("Естественное слияние", "NaturalSort.Sort"),
                new MenuItem("Многопутевое слияние", "MultipathSort.Sort"),
                new MenuItem("Bubble Sort", "InnerSorts.BubbleSort"),
                new MenuItem("Exchange Sort", "InnerSorts.ExchangeSort"),
                new MenuItem("Quick Sort", "InnerSorts.QuickSort"),
                new MenuItem("Merge Sort", "InnerSorts.MergeSort"),
                new MenuItem("Лексико-графическая сортировка", "InputOutput.ConsoleOut"),
                new MenuItem("exit", "exit")
            };
        Menu menu = new Menu(menuItems);
        MenuActions.MoveThrough(menu);
    }
}