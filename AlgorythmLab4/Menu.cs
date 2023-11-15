using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg4
{
    public class Menu
    {
        public int SelectedItemIndex;
        public List<MenuItem> Items;

        public Menu(List<MenuItem> items)
        {
            Items = items;
            SelectedItemIndex = 0;
        }
    }
}
