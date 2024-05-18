using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace POGODA.DATA
{
    public class Weather
    {
        public static List<DataContext> WeatherDataList = new List<DataContext>
        {
            new DataContext { Date = DateTime.Today.AddDays(0), Temperature = +25 },
            new DataContext { Date = DateTime.Today.AddDays(1), Temperature = +15 },
            new DataContext { Date = DateTime.Today.AddDays(2), Temperature = -10 },
            new DataContext { Date = DateTime.Today.AddDays(3), Temperature = -5 },
            new DataContext { Date = DateTime.Today.AddDays(4), Temperature = -25 },
            new DataContext { Date = DateTime.Today.AddDays(5), Temperature = -10 }
        };
    }
}
