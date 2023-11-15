using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    public class InputOutput
    {
        private string path = "../../../../Example/text.txt";
        private Lemmer? lemmer;
        public string[] ReadFile () 
        {
            lemmer = new Lemmer(File.ReadAllText(path));
            return ABCSortAlgorithm.ABCSort(lemmer.countedWords.Keys.ToArray<string>());
        }

        public void  ConsoleOut(string[] sortedWords)
        {
            foreach (string word in sortedWords)
            {
                Console.WriteLine($"{word} x{lemmer.countedWords[word]}");
            }
        }
    }
}
