using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class EditQuestionViewModel
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public int Var1Id { get; set; }
        public string Var1 { get; set; }
        public bool Var1IsCorrect { get; set; }
        public int Var2Id { get; set; }
        public string Var2 { get; set; }
        public bool Var2IsCorrect { get; set; }
        public int Var3Id { get; set; }
        public string Var3 { get; set; }
        public bool Var3IsCorrect { get; set; }
        public int Var4Id { get; set; }
        public string Var4 { get; set; }
        public bool Var4IsCorrect { get; set; }
    }
}
