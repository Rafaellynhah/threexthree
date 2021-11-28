using System;

namespace threexthree.Models
{

    public class TypeKey{
        public int Id { get; set; }

        public int QuantityKey { get; set; }
        public virtual Championship Championship { get; set; }

    }



}