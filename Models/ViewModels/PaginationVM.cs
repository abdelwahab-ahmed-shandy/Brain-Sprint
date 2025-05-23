
namespace Models.ViewModels
{
    public class PaginationVM
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public string? Query { get; set; }
        public string? StatusFilter { get; set; }
    }
}
