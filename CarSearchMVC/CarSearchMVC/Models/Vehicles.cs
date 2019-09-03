using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarSearchMVC.Models
{

    public class Vehicles
    {
        public int Id { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public string color { get; set; }
    }

}
