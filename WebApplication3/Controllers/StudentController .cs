using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebApplication3.Models;
using WebApplication3.Models.Repositories;

namespace WebApplication3.Controllers
{
   [Authorize(Roles = "Admin,Manager")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentRepository studentRepository, ISchoolRepository schoolRepository, ILogger<StudentController> logger)
        {
            _studentRepository = studentRepository;
            _schoolRepository = schoolRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var students = _studentRepository.GetAll();
            return View(students);
        }

        public IActionResult Details(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            ViewBag.SchoolID = new SelectList(_schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _studentRepository.Add(student);
                    return RedirectToAction(nameof(Index)); // Redirige vers la liste des étudiants
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating a student.");
                    ModelState.AddModelError("", "An error occurred while creating the student. Please try again.");
                }
            }

            // Recharger les écoles en cas d'erreur
            ViewBag.SchoolID = new SelectList(_schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View(student); // Retourne la vue avec les erreurs
        }



        public IActionResult Edit(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.SchoolID = new SelectList(_schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _studentRepository.Edit(student);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the student.");
                ModelState.AddModelError("", "An error occurred while editing the student. Please try again.");
            }

            ViewBag.SchoolID = new SelectList(_schoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View(student);
        }

        public IActionResult Delete(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var student = _studentRepository.GetById(id);
                if (student != null)
                {
                    _studentRepository.Delete(student);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the student.");
                ModelState.AddModelError("", "An error occurred while deleting the student. Please try again.");
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search(string name, int? schoolid)
        {
            var result = _studentRepository.GetAll(); // Récupérer tous les étudiants par défaut

            // Vérifier si un nom est fourni pour la recherche
            if (!string.IsNullOrEmpty(name))
            {
                result = _studentRepository.FindByName(name); // Méthode à définir dans votre repository
            }
            // Vérifier si une SchoolID est fournie
            else if (schoolid != null)
            {
                result = _studentRepository.GetStudentsBySchoolID(schoolid); // Méthode à définir dans votre repository
            }

            // Charger les écoles dans le ViewBag
            ViewBag.SchoolID = new SelectList(_schoolRepository.GetAll(), "SchoolID", "SchoolName");

            return View("Index", result); // Retourner à la vue Index avec les résultats
        }

    }
}
