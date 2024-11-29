using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOMIODMiddleware.Models
{
    public class Application
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime creation_datetime { get; set; }
    }
}