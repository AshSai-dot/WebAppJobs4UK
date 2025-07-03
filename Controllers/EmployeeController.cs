// Updated EmployeeController.cs with true server-side paging
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppJobs4UK.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures; // <--- Add this line

namespace WebAppJobs4UK.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        //public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        //{


        //    // Get only the page of employees needed
        //    var pagedEmployees = await _repository.GetPagedEmployeesAsync(page, pageSize);
        //    var totalCount = await _repository.GetTotalCountAsync();

        //    var model = new EmployeeListViewModel
        //    {
        //        EmployeeForm = new Employee(),
        //        Employees = pagedEmployees,
        //        CurrentPage = page,
        //        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        //    };

        //    return View(model);
        //}
        public async Task<IActionResult> Index(
           int page = 1,
           int pageSize = 5,
           string searchTerm = "",
           string sortBy = "id", // Default sort column
           string sortOrder = "desc" // Default sort order
       )
        {
            ViewBag.Departments = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "IT" },
                new SelectListItem { Value = "2", Text = "HR" },
                new SelectListItem { Value = "3", Text = "Finance" },
                new SelectListItem { Value = "4", Text = "Marketing" }
            };
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 5; // Enforce minimum page size           

            var (employees, totalCount) = await _repository.GetPagedEmployeesAsync(
                page, pageSize, searchTerm, sortBy, sortOrder);

            var model = new EmployeeListViewModel
            {
                EmployeeForm = new Employee(), // Initialize if you have a form on the same page
                Employees = employees,
                CurrentPage = page,
                PageSize = pageSize, // Pass PageSize to ViewModel
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("PartialIndex", model); // Return only the partial view HTML
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> InsertEmployee(EmployeeListViewModel model)
        {
            try
            {
                var employee = model.EmployeeForm;

                if (employee.Id == 0)
                {
                    await _repository.AddAsync(employee);
                    TempData["Success"] = "Employee added successfully!";
                }
                else
                {
                    await _repository.UpdateAsync(employee);
                    TempData["Success"] = "Employee updated successfully!";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<JsonResult> GetAllData()
        {
            try
            {
                var employees = await _repository.GetAllAsync();
                return Json(employees);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> GetData(int id)
        {
            try
            {
                var employee = await _repository.GetByIdAsync(id);
                return Json(employee);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return Json(ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                TempData["Success"] = "Deleted successfully!";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting: " + ex.Message;
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
