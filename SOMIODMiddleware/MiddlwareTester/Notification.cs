using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddlwareTester
{
    internal class Notification
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime creation_datetime { get; set; }
        public int parent { get; set; }
        public string @event { get; set; }  // exceção, pois o event é uma palavra reservada
        public string endpoint { get; set; }
        public bool enabled { get; set; }
    }
}
