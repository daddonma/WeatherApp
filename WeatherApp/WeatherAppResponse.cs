using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp
{
    public class WeatherAppResponse
    {
        public Main main;
        public List<Weather> weather;
        public Sys sys;
    }
}
