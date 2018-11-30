using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaUdpReciever
{
    class Temp
    {
        public int UserId { get; set; }
        public decimal Humidity { get; set; }
        public decimal Temperatur { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }


        public Temp(int userId, decimal humidity, decimal temperatur, DateTime dateTime, string location)
        {
            UserId = userId;
            Humidity = humidity;
            Temperatur = temperatur;
            DateTime = dateTime;
            Location = location;
        }
        public Temp()
        {

        }

    }
}
