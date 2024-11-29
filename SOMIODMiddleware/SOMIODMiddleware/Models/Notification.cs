using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIODMiddleware.Models
{
    public class Notification
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime creation_datetime { get; set; }
        public int parent { get; set; }
        public int @event { get; set; }  // exceção, pois o event é uma palavra reservada
        public string endpoint { get; set; }
        public bool enabled { get; set; }
    }
}