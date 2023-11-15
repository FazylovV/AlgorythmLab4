using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorythmLab4
{
    class FileWriter
    {
        public StreamWriter Writer { get; set; }
        public string FileName { get; set; }
        public FileWriter(string filename)
        {
            FileName = filename;
            Writer = new StreamWriter(filename);
        }
    }
}
