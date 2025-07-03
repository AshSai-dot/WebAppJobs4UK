using Microsoft.EntityFrameworkCore;

namespace WebAppJobs4UK.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                                    .OrderByDescending(e => e.Id)
                                    .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees
       .Where(e => e.Id == employee.Id)
       .FirstOrDefaultAsync();

            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.Email = employee.Email;
                existingEmployee.Department = employee.Department;
                existingEmployee.HireDate = employee.HireDate;
                existingEmployee.Salary = employee.Salary;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> GetPagedEmployeesAsync(int page, int pageSize)
        {
            return await _context.Employees
                .OrderByDescending(e => e.Id) // or any sorting logic
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Employees.CountAsync();
        }

        public async Task<(List<Employee> Employees, int TotalCount)> GetPagedEmployeesAsync(
            int page,
            int pageSize,
            string searchTerm,
            string sortBy,
            string sortOrder)
        {
            IQueryable<Employee> query = _context.Employees;

            // 1. Filtering (Search)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm) ||
                                         e.Email.Contains(searchTerm) ||
                                         e.Department.ToString().Contains(searchTerm)); // Adjust Department search as needed
            }

            // 2. Total Count (before pagination)
            int totalCount = await query.CountAsync();

            // 3. Sorting
            switch (sortBy?.ToLower())
            {
                case "name":
                    query = sortOrder == "asc" ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
                    break;
                case "email":
                    query = sortOrder == "asc" ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                    break;
                case "department":
                    query = sortOrder == "asc" ? query.OrderBy(e => e.Department) : query.OrderByDescending(e => e.Department);
                    break;
                case "hiredate":
                    query = sortOrder == "asc" ? query.OrderBy(e => e.HireDate) : query.OrderByDescending(e => e.HireDate);
                    break;
                case "salary":
                    query = sortOrder == "asc" ? query.OrderBy(e => e.Salary) : query.OrderByDescending(e => e.Salary);
                    break;
                default: // Default sort
                    query = query.OrderByDescending(e => e.Id);
                    break;
            }

            // 4. Pagination
            List<Employee> employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (employees, totalCount);
        }

    }
}
