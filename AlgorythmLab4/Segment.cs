using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtSort
{
    class Segment
    {
        public StreamReader Reader { get; set; }
        public int Counter { get; set; }
        public string FilePath { get; set; }
        public bool Picked { get; set; }
        public bool End { get; set; }
        public string Value { get; set; }
        public int Column { get; set; }
        public Segment(int counter, string path, bool picked, bool end, string value, int columnn)
        {
            Counter = counter;
            FilePath = path;
            Reader = new StreamReader(path);
            Picked = picked;
            End = end;
            Value = value;
            Column = columnn;
        }

        public bool Compare(Segment second)
        {
            if (double.TryParse(Value.Split(';')[Column], out double fDouble))
            {
                double sDouble = double.Parse(second.Value.Split(';')[Column]);
                return fDouble >= sDouble;
            }

            if (DateTime.TryParse(Value.Split(';')[Column], out DateTime fDate))
            {
                DateTime sDate = DateTime.Parse(second.Value.Split(';')[Column]);
                return fDate >= sDate;
            }

            return string.Compare(Value.Split(';')[Column], second.Value.Split(';')[Column]) >= 0;
        }
    }
}
