using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Models.Repositories;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class SchoolController : Controller
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolController(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var schools = _schoolRepository.GetAll();
            return View(schools);
        }

        public IActionResult Details(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(School school)
        {
            try
            {
                _schoolRepository.Add(school);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(school);
            }
        }

        public IActionResult Edit(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(School school)
        {
            if (ModelState.IsValid)
            {
                _schoolRepository.Edit(school);
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }

        public IActionResult Delete(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var school = _schoolRepository.GetById(id);
            if (school != null)
            {
                _schoolRepository.Delete(school);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
