using System;
using System.Collections.Generic;

namespace threexthree.Models
{

    public class Game{
        public int Id { get; set; }
        public string place {get; set; }
        public string address {get; set; }
        public DateTime data_hour {get; set;}
        public virtual Team one_team { get; set; }
        public virtual Team two_team { get; set; }
    }


}