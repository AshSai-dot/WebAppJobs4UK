namespace WebAppJobs4UK.Models
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
        // Task<List<Employee>> GetPagedEmployeesAsync(int page, int pageSize);
        // Task<int> GetTotalCountAsync();
        // Modified for search and sort
        Task<(List<Employee> Employees, int TotalCount)> GetPagedEmployeesAsync(
            int page,
            int pageSize,
            string searchTerm,
            string sortBy,
            string sortOrder);
    }
}
