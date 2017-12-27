using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ViewModels;
using WebApplication1.Models;

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
            return View(db.Tests.ToList());
        }

        public IActionResult Create()
        {
            List<string> list = new List<string>();
            foreach (Section sect in db.Sections)
            {
                list.Add(sect.Name);
            }
            ViewBag.Sections = list;
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
                    return RedirectToAction("Index");
                }
                else ModelState.AddModelError(string.Empty, "Ошибочка, нет такого раздела!");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Test test = await db.Tests.FirstOrDefaultAsync(t => t.Id == id);
            if (test == null)
            {
                return NotFound();
            }
            EditTestViewModel model = new EditTestViewModel { Id = test.Id, Name = test.Name, Section = test.Section.Name };
            List<string> list = new List<string>();
            foreach (Section sect in db.Sections)
            {
                list.Add(sect.Name);
            }
            ViewBag.Sections = list;
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
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            //User user = await _userManager.FindByIdAsync(id);
            Test test = await db.Tests.FirstOrDefaultAsync(t => t.Id == id);
            if (test != null)
            {
                db.Tests.Remove(test);
            }
            return RedirectToAction("Index");
        }
    }
}