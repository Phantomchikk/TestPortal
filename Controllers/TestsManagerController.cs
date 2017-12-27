using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ViewModels;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class TestsManagerController : Controller
    {
        TestContext db;

        public TestsManagerController(TestContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            List<TestViewModel> list = new List<TestViewModel>();
            foreach (var test in db.Tests.Include("Section"))
            {
                list.Add(new TestViewModel { Id = test.Id, Name = test.Name, SectionName = test.Section.Name });
            }
            return View(list);
        }

        public IActionResult Create()
        {
            List<string> list = new List<string>();
            foreach (Section sect in db.Sections)
            {
                list.Add(sect.Name);
            }
            ViewBag.Sections = new SelectList(list.ToArray());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Sections.Where(t => t.Name == model.Section).Count() > 0)
                {
                    Test test = new Test { Name = model.Name, SectionId = db.Sections.FirstOrDefault(t => t.Name == model.Section).Id };
                    await db.Tests.AddAsync(test);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError(string.Empty, "Ошибочка, нет такого раздела!");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Test test = await db.Tests.Include("Section").FirstOrDefaultAsync(t => t.Id == id);
            if (test == null)
            {
                return Content("Нет теста с таким Id");
            }
            EditTestViewModel model = new EditTestViewModel { Id = test.Id, Name = test.Name, Section = test.Section.Name };
            List<string> list = new List<string>();
            foreach (Section sect in db.Sections)
            {
                list.Add(sect.Name);
            }
            ViewBag.Sections = new SelectList(list.ToArray());
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                Test test = await db.Tests.FirstOrDefaultAsync(t => t.Id == model.Id);
                if (test != null)
                {
                    test.Name = model.Name;
                    test.SectionId = db.Sections.FirstOrDefault(t => t.Name == model.Section).Id;
                    db.Tests.Update(test);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError(string.Empty, "Нет такого теста в базе, возможно он был удалён");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Test test = await db.Tests.FirstOrDefaultAsync(t => t.Id == id);
            if (test != null)
            {
                db.Tests.Remove(test);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Questions(int id)
        {
            Test test = await db.Tests.Include("Questions").FirstOrDefaultAsync(t => t.Id == id);
            if (test != null)
            {
                return View(test);
            }
            else return Content("Нет теста с таким Id");
        }

        public IActionResult CreateQuestion(int testId)
        {
            ViewBag.TestId = testId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Question quest = new Question { TestId = model.TestId, Text = model.Text, Type = model.Type };
                await db.Questions.AddAsync(quest);
                Variant v1 = new Variant { QuestionId = quest.Id, IsCorrect = model.Var1IsCorrect, Text = model.Variant1 };
                Variant v2 = new Variant { QuestionId = quest.Id, IsCorrect = model.Var2IsCorrect, Text = model.Variant2 };
                Variant v3 = new Variant { QuestionId = quest.Id, IsCorrect = model.Var3IsCorrect, Text = model.Variant3 };
                Variant v4 = new Variant { QuestionId = quest.Id, IsCorrect = model.Var4IsCorrect, Text = model.Variant4 };
                await db.Variants.AddRangeAsync(v1, v2, v3, v4);
                await db.SaveChangesAsync();
                return RedirectToAction("Questions", new { id = model.TestId });
            }
            return View(model);
        }

        public async Task<IActionResult> EditQuestion(int id)
        {
            Question quest = await db.Questions.Include("Variants").FirstOrDefaultAsync(q => q.Id == id);
            if (quest == null)
            {
                return Content("Нет записи с таким Id");
            }
            EditQuestionViewModel model = new EditQuestionViewModel
            {
                Id = quest.Id,
                TestId = quest.TestId,
                Type = quest.Type,
                Text = quest.Text,
                Var1Id = quest.Variants[0].Id,
                Var1 = quest.Variants[0].Text,
                Var1IsCorrect = quest.Variants[0].IsCorrect,
                Var2Id = quest.Variants[1].Id,
                Var2 = quest.Variants[1].Text,
                Var2IsCorrect = quest.Variants[1].IsCorrect,
                Var3Id = quest.Variants[2].Id,
                Var3 = quest.Variants[2].Text,
                Var3IsCorrect = quest.Variants[2].IsCorrect,
                Var4Id = quest.Variants[3].Id,
                Var4 = quest.Variants[3].Text,
                Var4IsCorrect = quest.Variants[3].IsCorrect
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditQuestion(EditQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Question quest = await db.Questions.Include("Variants").FirstOrDefaultAsync(t => t.Id == model.Id);
                if (quest != null)
                {
                    quest.Type = model.Type;
                    quest.Text = model.Text;
                    quest.Variants[0].QuestionId = model.Id;
                    quest.Variants[0].Text = model.Var1;
                    quest.Variants[0].IsCorrect = model.Var1IsCorrect;
                    quest.Variants[1].QuestionId = model.Id;
                    quest.Variants[1].Text = model.Var2;
                    quest.Variants[1].IsCorrect = model.Var2IsCorrect;
                    quest.Variants[2].QuestionId = model.Id;
                    quest.Variants[2].Text = model.Var3;
                    quest.Variants[2].IsCorrect = model.Var3IsCorrect;
                    quest.Variants[3].QuestionId = model.Id;
                    quest.Variants[3].Text = model.Var4;
                    quest.Variants[3].IsCorrect = model.Var4IsCorrect;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Questions", new { id = model.TestId });
                }
                else ModelState.AddModelError(string.Empty, "Нет такого вопроса в базе, возможно он был удалён");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(int id, int testId)
        {
            Question quest = await db.Questions.FirstOrDefaultAsync(t => t.Id == id);
            if (quest != null)
            {
                db.Questions.Remove(quest);
                db.SaveChanges();
            }
            return RedirectToAction("Questions", new { id = testId });
        }
    }
}