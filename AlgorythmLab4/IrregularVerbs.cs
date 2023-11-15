using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    public class IrregularVerbs
    {
        public readonly Dictionary<string, string> Verbs = new Dictionary<string, string>
        {
            { "said", "say" },
            { "maid", "make"},
            { "went", "go" },
            { "gone", "go" },
            { "took", "take" },
            { "taken", "take" },
            { "came", "come" },
            { "saw", "see" },
            { "seen", "see" },
            { "knew", "know" },
            { "known", "know" },
            { "got", "get" },
            { "gave", "give" },
            { "given", "give" },
            { "found", "find" },
            { "thought", "think" },
            { "told", "tell" },
            { "became", "become" },
            { "showed", "show" },
            { "shown", "show" },
            { "left", "leave" },
            { "felt", "feel" },
            { "brought", "bring" },
            { "began", "begin" },
            { "begun", "begin" },
            { "kept", "keep" },
            { "held", "hold" },
            { "wrote", "write" },
            { "written", "write" },
            { "stood", "stand" },
            { "heard", "hear" },
            { "meant", "mean" },
            { "met", "meet" },
            { "ran", "run" },
            { "paid", "pay" },
            { "sat", "sit" },
            { "spoke", "speak" },
            { "spoken", "speak" },
            { "grew", "grow" },
            { "grown", "grow" },
            { "lost", "lose" },
            { "fell", "fall" },
            { "fallen", "fall" },
            { "sent", "send" },
            { "built", "build" },
            { "understood", "understand" },
            { "broke", "break" },
            { "broken", "break" },
            { "spent", "spend" },
            { "drove", "drive" },
            { "driven", "drive" },
            { "bought", "buy" },
            { "wore", "wear" },
            { "worn", "wear" },
            { "chose", "choose" },
            { "chosen", "choose" },
            { "ate", "eat" },
            { "did", "do" },
            { "done", "do" },
            { "", "" } //шаблон для новых пар слов
        };
        
        public string ToFirstForm (string word)
        {
            if (Verbs.ContainsKey (word)) return Verbs[word];
            return word;
        }
    }
}
