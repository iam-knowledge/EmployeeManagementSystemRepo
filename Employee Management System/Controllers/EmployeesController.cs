
//using Employee_Management_System.Data;
//using Employee_Management_System.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Employee_Management_System.Controllers
//{

//    public class EmployeesController : Controller
//    {
//        private readonly AppDbContext _context;
//        public EmployeesController(AppDbContext context) => _context = context;

//        // GET: Employees

//        public async Task<IActionResult> Index(string? search, string? sortOrder)
//        {
//            ViewData["CurrentFilter"] = search;
//            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
//            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

//            var employees = from e in _context.Employees select e;

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                employees = employees.Where(e =>
//                    e.FirstName.Contains(search) ||
//                    e.LastName.Contains(search) ||
//                    e.Email.Contains(search));
//            }

//            employees = sortOrder switch
//            {
//                "name_desc" => employees.OrderByDescending(e => e.LastName),
//                "Date" => employees.OrderBy(e => e.HireDate),
//                "date_desc" => employees.OrderByDescending(e => e.HireDate),
//                _ => employees.OrderBy(e => e.LastName),
//            };

//            return View(await employees.AsNoTracking().ToListAsync());
//        }

//        // GET: Employees/Details/
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null) return NotFound();
//            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
//            if (employee == null) return NotFound();
//            return View(employee);
//        }

//        // GET: Employees/Create
//        public IActionResult Create() => View();

//        // POST: Employees/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,HireDate,Department,Salary,IsActive")] Employee employee)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(employee);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(employee);
//        }

//        // GET: Employees/Edit/
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return NotFound();
//            var employee = await _context.Employees.FindAsync(id);
//            if (employee == null) return NotFound();
//            return View(employee);
//        }

//        // POST: Employees/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,HireDate,Department,Salary,IsActive")] Employee employee)
//        {
//            if (id != employee.Id) return NotFound();

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(employee);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!await _context.Employees.AnyAsync(e => e.Id == employee.Id))
//                        return NotFound();
//                    else
//                        throw;
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(employee);
//        }

//        // GET: Employees/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return NotFound();
//            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
//            if (employee == null) return NotFound();
//            return View(employee);
//        }

//        // POST: Employees/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var employee = await _context.Employees.FindAsync(id);
//            if (employee != null)
//            {
//                _context.Employees.Remove(employee);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
