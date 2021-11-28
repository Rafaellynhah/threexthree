using System;
using System.Collections.Generic;

namespace threexthree.Models
{

    public class Game{
        public int Id { get; set; }
        public int ScoreOneTeam { get; set; }
        public int ScoreTwoTeam { get; set; }
        public string Place {get; set; }
        public string Address {get; set; }
        public string DataHour {get; set;}
        public virtual Team OneTeam { get; set; }
        public virtual Team TwoTeam { get; set; }
        public virtual Key Key { get; set; }
    }


}