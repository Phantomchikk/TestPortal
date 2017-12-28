using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class GoTestViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Attempt> Attempts { get; set; }
    }
}
