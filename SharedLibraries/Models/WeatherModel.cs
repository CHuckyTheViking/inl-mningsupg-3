using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.Models
{
    class WeatherModel
    {
        public Current current { get; set; }
    }
    public class Current
    { 
        public int temperature { get; set; }
        public int humidity { get; set; }
    }
}
