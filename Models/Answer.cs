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
        public int AttemptId { get; set; }

        public Variant Variant { get; set; }
        public Attempt Attempt { get; set; }
    }
}
