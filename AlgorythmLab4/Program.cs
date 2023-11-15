using Alg4;
using WordCounter;

class Program
{
    static void Main()
    {
        //TimeMeasurer tm = new TimeMeasurer();
        //tm.Start(5000);
        /*InputOutput io = new InputOutput();
        io.ConsoleOut(io.ReadFile());*/

        Console.CursorVisible = false;
        List<MenuItem> menuItems = new List<MenuItem>()
            {
                new MenuItem("Прямое слияние", "ExternalSort.Sort"),
                new MenuItem("Естественное слияние", "NaturalSort.Sort"),
                new MenuItem("Многопутевое слияние", "MultipathSort.Sort"),
                new MenuItem("Exit", "exit")
            };
        Menu menu = new Menu(menuItems);
        MenuActions.MoveThrough(menu);
    }
}