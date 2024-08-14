using SchoolManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SchoolDbContext _context;

        public TeacherController()
        {
            _context = new SchoolDbContext();
        }

        public IActionResult Index()
        {
            var teachers = _context.GetAllTeachers();
            return View("List", teachers);
        }

        public IActionResult Show(int id)
        {
            var teacher = _context.GetTeacherById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.AddTeacher(teacher);
                    return Json(new { success = true, message = "Teacher created successfully" });
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
            var teacher = _context.GetTeacherById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return Json(new { success = false, message = "Teacher not found" });
            }

            if (ModelState.IsValid)
            {
                _context.UpdateTeacher(teacher);
                return Json(new { success = true, message = "Teacher updated successfully" });
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
            var teacher = _context.GetTeacherById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpDelete]
        [Route("~/Teacher/Delete/{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            var teacher = _context.GetTeacherById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            _context.DeleteTeacher(id);
            return Ok();
        }
    }
}
