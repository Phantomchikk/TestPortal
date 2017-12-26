using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string Name { get; set; }

        public Section Section { get; set; }
        public List<Question> Questions { get; set; }
        public List<Attempt> Attempts { get; set; }
    }
}
