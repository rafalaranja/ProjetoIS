﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIODMiddleware.Models
{
    public class Container
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime creation_datetime { get; set; }
        public int parent { get; set; }
    }
}