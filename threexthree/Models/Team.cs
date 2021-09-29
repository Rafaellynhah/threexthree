using System.ComponentModel.DataAnnotations;

namespace threexthree.Models
{

    public class Team{
        public int Id { get; set; }

        [StringLength(30, MinimumLength=3)]
        public string Name { get; set; }
        public virtual Championship Championship { get; set; }

    }



}