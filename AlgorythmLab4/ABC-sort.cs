using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    public static class ABCSortAlgorithm
    { 

        public static string[] ABCSort(string[] array, int rank = 0)
        {
            if (array.Length < 2)
                return array;

            var table = new Dictionary<char, List<string>>();

            foreach (var str in array)
            {
                if (rank < str.Length)
                {
                    var key = char.ToUpper(str[rank]);
                    if (table.ContainsKey(key))
                        table[key].Add(str);
                    else
                        table.Add(key, new List<string> { str });
                }
            }

            int index = 0;
            for (var i = 'A'; i <= 'Z'; i++)
            {
                if (table.ContainsKey(i))
                {
                    foreach (var str in ABCSort(table[i].ToArray(), rank + 1))
                    {
                        array[index++] = str;
                    }
                }
            }

            return array;
        }
    }
}

