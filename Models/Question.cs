using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }

        public Test Test { get; set; }
        public List<Variant> Variants { get; set; }
    }
}
