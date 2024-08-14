using SchoolManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentController()
        {
            _context = new SchoolDbContext();
        }

        public IActionResult Index()
        {
            var students = _context.GetAllStudents();
            return View("List", students);
        }

        public IActionResult Show(int id)
        {
            var student = _context.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.AddStudent(student);
                    return Json(new { success = true, message = "Student created successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors = errors });
        }

        public IActionResult Update(int id)
        {
            var student = _context.GetStudentById(id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            if (ModelState.IsValid)
            {
                _context.UpdateStudent(student);
                return Json(new { success = true, message = "Student updated successfully" });
            }

            var errors = ModelState
                .Where(ms => ms.Value != null && ms.Value.Errors.Any(e => !string.IsNullOrEmpty(e.ErrorMessage)))
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value != null && ms.Value.Errors.Any(e => !string.IsNullOrEmpty(e.ErrorMessage))
                        ? string.Join(" ", ms.Value.Errors.Where(e => !string.IsNullOrEmpty(e.ErrorMessage)).Select(e => e.ErrorMessage))
                        : string.Empty
                );

            return Json(new { success = false, errors = errors });
        }

        public IActionResult Delete(int id)
        {
            var student = _context.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpDelete]
        [Route("~/Student/Delete/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _context.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            _context.DeleteStudent(id);
            return Ok();
        }
    }
}
