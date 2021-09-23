using System.Collections.Generic;

namespace threexthree.Models
{

    public class Team{
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Championship Championship { get; set; }

    }



}