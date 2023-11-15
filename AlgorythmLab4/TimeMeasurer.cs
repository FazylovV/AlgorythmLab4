using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    public class TimeMeasurer
    {
        public void Start(int wordsCount)
        {
            string path = "../../../../Example/tests.txt";
            List<string> words = new() {"Cлова:ВремяМСПузырёк:ВремяМСАБС" };
            long bubbleTime = BubbleTimer(wordsCount);
            long abcTime =  ABCTimer(wordsCount);
            words.Add($"{wordsCount}:{bubbleTime}:{abcTime}");
            File.WriteAllLines(path, words);
        }

        public long BubbleTimer(int wordsCount) 
        {
            Stopwatch bubbleTimer = new Stopwatch();
            bubbleTimer.Start();
            var lemmer = new Lemmer();
            var contedWords = lemmer.WordsWithCount(File.ReadAllText("../../../../Example/WarAndPeace.txt"), wordsCount);
            List<string> sortedList = BubbleSort.Sort(contedWords.Keys.ToList());
            bubbleTimer.Stop();
            return bubbleTimer.ElapsedMilliseconds;
        }

        public long ABCTimer(int wordsCount)
        {
            Stopwatch ABCTimer = new Stopwatch();
            ABCTimer.Start();
            var lemmer = new Lemmer();
            var contedWords = lemmer.WordsWithCount(File.ReadAllText("../../../../Example/WarAndPeace.txt"), wordsCount);
            string[] sortedList = ABCSortAlgorithm.ABCSort(contedWords.Keys.ToArray<string>());
            ABCTimer.Stop();
            return ABCTimer.ElapsedMilliseconds;
        }
    }
}
