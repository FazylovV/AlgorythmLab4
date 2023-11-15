using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter
{
    public static class BubbleSort
    {
        public static List<string>? Sort (List<string> list)
        {
            if (list == null) return null;
           List<string> result = list;
            bool WasSorted = true;
            while (WasSorted)
            {
                WasSorted = false;
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].CompareTo(list[i + 1]) > 0)
                    {
                        WasSorted = true;
                        string temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }
                }
            }
            return result;
        }
    }
}
