using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private TestContext db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public HomeController(TestContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Sections.ToList());
        }

        [Authorize]
        public async Task<IActionResult> ToSection(int id)
        {
            Section sect = await db.Sections.Include("Tests").FirstOrDefaultAsync(t => t.Id == id);
            if (sect == null)
                return Content("Нет раздела с таким id");
            return View(sect);
        }

        public async Task<IActionResult> GoTest(int id)
        {
            Test test = await db.Tests.Include("Attempts").FirstOrDefaultAsync(t => t.Id == id);
            if (test == null)
            {
                return Content("Нет теста с таким id");
            }
            GoTestViewModel model = new GoTestViewModel
            {
                Id = test.Id,
                Name = test.Name,
                Attempts = test.Attempts.Where(t => t.UserId == _userManager.GetUserId(User))
            };
            return View(model);
        }

        private TestingViewModel GetQuest(int n, int id, int attemptId)
        {
            Test test = db.Tests.Include("Questions").Include("Questions.Variants").FirstOrDefault(t => t.Id == id);
            TestingViewModel model = new TestingViewModel
            {
                Id = test.Questions[n].Id,
                Numb = n,
                AttemptId = attemptId,
                TestId = test.Questions[n].TestId,
                Type = test.Questions[n].Type,
                Text = test.Questions[n].Text,
                Var1Id = test.Questions[n].Variants[0].Id,
                Var1 = test.Questions[n].Variants[0].Text,
                Var1IsCorrect = false,
                Var2Id = test.Questions[n].Variants[1].Id,
                Var2 = test.Questions[n].Variants[1].Text,
                Var2IsCorrect = false,
                Var3Id = test.Questions[n].Variants[2].Id,
                Var3 = test.Questions[n].Variants[2].Text,
                Var3IsCorrect = false,
                Var4Id = test.Questions[n].Variants[3].Id,
                Var4 = test.Questions[n].Variants[3].Text,
                Var4IsCorrect = false
            };
            return model;
        }

        public async Task<IActionResult> Testing(int id)
        {
            Test test = await db.Tests.Include("Questions").Include("Questions.Variants").FirstOrDefaultAsync(t => t.Id == id);
            if (test == null)
            {
                return Content("Нет теста с таким id");
            }
            Attempt attempt = new Attempt { TestId = test.Id, UserId = _userManager.GetUserId(User) };
            db.Attempts.Add(attempt);
            db.SaveChanges();
            TestingViewModel model = GetQuest(0, test.Id, attempt.Id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Testing(TestingViewModel model)
        {
            List<Answer> answers = new List<Answer>();
            if (model.Var1IsCorrect)
            {
                answers.Add(new Answer { AttemptId = model.AttemptId, VariantId = model.Var1Id });
            }
            if (model.Var2IsCorrect)
            {
                answers.Add(new Answer { AttemptId = model.AttemptId, VariantId = model.Var2Id });
            }
            if (model.Var3IsCorrect)
            {
                answers.Add(new Answer { AttemptId = model.AttemptId, VariantId = model.Var3Id });
            }
            if (model.Var4IsCorrect)
            {
                answers.Add(new Answer { AttemptId = model.AttemptId, VariantId = model.Var4Id });
            }
            await db.Answers.AddRangeAsync(answers);
            db.SaveChanges();
            Test test = await db.Tests.Include("Questions").Include("Questions.Variants").FirstOrDefaultAsync(t => t.Id == model.TestId);
            if (model.Numb != test.Questions.Count - 1)
            {
                TestingViewModel newModel = GetQuest(model.Numb + 1, model.TestId, model.AttemptId);
                return View(newModel);
            }
            else
            {
                Attempt attempt = await db.Attempts.Include("Answers").Include("Answers.Variant").FirstOrDefaultAsync(t => t.Id == model.AttemptId);
                int a = attempt.Answers.Where(t => t.Variant.IsCorrect).Count();
                int b = attempt.Answers.Count;
                TestResultViewModel result = new TestResultViewModel
                {
                    Id = model.TestId,
                    TestName = test.Name,
                    Result = (double)a / b * 100
                };
                return View("TestResult", result);
            }
        }
    }
}
