namespace WebAppJobs4UK.Models
{
    public class EmployeeListViewModel
    {

        public Employee EmployeeForm { get; set; } = new Employee();
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
      

        public int CurrentPage { get; set; }
        public int PageSize { get; set; } // Add PageSize to the ViewModel for consistency and potential display
        public int TotalPages { get; set; }
        public int TotalItems { get; set; } // New: Total count of all items (useful for "Showing X of Y")

        // Optional: For search/filter
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; } // "asc" or "desc"

        // Helper properties for UI
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
