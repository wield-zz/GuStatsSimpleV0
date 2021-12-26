using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    class Stats
    {
        public int total { get; set; }
        public List<Records> records { get; set; } = new List<Records>();

      
    }
    class Records
    {
        public int total_rounds { get; set; }
        public Records()
        {

        }
    }
}
