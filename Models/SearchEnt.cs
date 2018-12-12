using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZsearch3.Models
{
    public class SearchEnt
    {
        public string Value { get; set; }
        public DateTime Date { get; set; }

        public SearchEnt()
        {
            Value = "";
            Date = DateTime.Now;
        }

        public SearchEnt(string v, DateTime d)
        {
            Value = v;
            Date = d;
        }
    }
}