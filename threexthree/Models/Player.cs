using System.ComponentModel.DataAnnotations;

namespace threexthree.Models
{

    public class Player{
        public int Id { get; set; }

        [StringLength(50, MinimumLength=5)]
        public string Name { get; set; }

        [StringLength(11, MinimumLength=11)]
        public string Cpf {get; set;}

        public int Jersey {get; set;}
        public virtual Team Team { get; set; }

    }



}