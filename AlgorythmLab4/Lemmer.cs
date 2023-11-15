using PorterStemmer.Stemmers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    public class Lemmer
    {
        private IrregularVerbs dictionary = new();
        public Dictionary<string, int> countedWords = new();

        public Lemmer() { }
        public Lemmer(string text) { countedWords = WordsWithCount(text); }
        public Dictionary<string, int> WordsWithCount(string text)
        {
            Dictionary<string, int> words = new();
            string[] splittedText = text.Split(new char[] { '\n', ' ' });
            char[] separators = new char[] { ',', ' ', '.', ':', ';', '\r', '\n', '-', '–' };
            foreach (string word in splittedText)
            {
                if (word == null || word == "") continue;
                string newWord = word.ToLower();
                newWord = word.Trim(separators);
                if (newWord == null || newWord == "") continue;


                string lemm = Lemmize(newWord);
                if (words.ContainsKey(lemm))
                {
                    words[lemm]++;
                }
                else
                {
                    words.Add(lemm, 1);
                }
            }
            return words;
        }
        public Dictionary<string, int> WordsWithCount(string text, int MaxWordCount)
        {
            Dictionary<string, int> words = new();
            string[] splittedText = text.Split(new char[] { '\n', ' ' });
            char[] separators = new char[] { ',', ' ', '.', ':', ';', '\r', '\n', '-', '–' };
            for (int i = 0; i < splittedText.Length; i++)
            {
                string word = splittedText[i];
                if (word == null || word == "") continue;
                string newWord = word.ToLower();
                newWord = word.Trim(separators);
                if (newWord == null || newWord == "") continue;

                string lemm = Lemmize(newWord);
                if (words.ContainsKey(lemm))
                {
                    words[lemm]++;
                }
                else
                {
                    words.Add(lemm, 1);
                }
                if (i >= MaxWordCount - 1) return words;
            }
            return words;
        }

        public string Lemmize(string word)
        {
            var stemmer = new EnglishStemmer();
            string lemm = stemmer.GetStem(word);
            if (dictionary.Verbs.ContainsKey(lemm)) return dictionary.Verbs[lemm];
            else return lemm;
        }
    }
}
    

