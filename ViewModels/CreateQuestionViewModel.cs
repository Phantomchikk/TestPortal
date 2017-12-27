using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class CreateQuestionViewModel
    {
        public int TestId { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string Variant1 { get; set; }
        public bool Var1IsCorrect { get; set; }
        public string Variant2 { get; set; }
        public bool Var2IsCorrect { get; set; }
        public string Variant3 { get; set; }
        public bool Var3IsCorrect { get; set; }
        public string Variant4 { get; set; }
        public bool Var4IsCorrect { get; set; }
    }
}
