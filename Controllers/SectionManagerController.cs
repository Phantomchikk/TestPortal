using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class SectionManagerController : Controller
    {
        private TestContext db;

        public SectionManagerController(TestContext section)
        {
            db = section;
        }

        public IActionResult Index()
        {
            return View(db.Sections.ToList());
        }

        [HttpGet]
        public IActionResult Create()  //Переделать!!!!
        {
            List<string> list = new List<string>();
            foreach (Test test in db.Tests)
            {
                list.Add(test.Name);
            }
            ViewBag.Tests = list;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Section section = new Section { Name = model.Name };
                await db.Sections.AddAsync(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id) 
        {
            Section section = db.Sections.FirstOrDefault(t => t.Id == id);
            if (section == null)
            {
                return NotFound();
            }
            EditSectionViewModel model = new EditSectionViewModel { Id = section.Id, Name = section.Name };
            List<string> list = new List<string>();
            foreach (Test test in db.Tests)
            {
                list.Add(test.Name);
            }
            ViewBag.Tests = list;
            db.SaveChanges();
            return View(model);
        }

        
        [HttpPost]
        public IActionResult Edit(EditSectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Section section = db.Sections.FirstOrDefault(t => t.Id == model.Id);
                if (section != null)
                {
                    section.Name = model.Name;
                    db.Sections.Update(section);
                    db.SaveChanges();
                }
            }
            return View(model);
        }

        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Section section = db.Sections.FirstOrDefault(t => t.Id == id);
            if (section != null)
            {
                db.Sections.Remove(section);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}