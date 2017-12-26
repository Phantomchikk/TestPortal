using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int? VariantId { get; set; }
        public int TestId { get; set; }

        public Variant Variant { get; set; }
        public Test Test { get; set; }
    }
}
