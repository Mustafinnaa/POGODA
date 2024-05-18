using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POGODA.DATA
{
    public class DataContext
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public Typess SelectedType { get; set; }

        public DataContext()
        {
            SelectedType = new Typess();
        }
    }

}
