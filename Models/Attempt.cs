using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TestId { get; set; }

        public User User { get; set; }
        public Test Test { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
