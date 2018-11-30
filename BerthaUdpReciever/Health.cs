using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaUdpReciever
{
    class Health
    {

        public int BloodPressure { get; set; }
        public int HeartBeat { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public Health( int bloodPressure, int age, int weight, int userId, DateTime dateTime)
        {
            BloodPressure = bloodPressure;
            Age = age;
            Weight = weight;
            UserId = userId;
            DateTime = dateTime;
        }

        public Health()
        {
        }
    }
}
