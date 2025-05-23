namespace BrainSprint.ViewComponents
{
    public class LearningPathsDropdownViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public LearningPathsDropdownViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var paths = await _context.LearningPaths.Where(p => p.IsPublished == true)
                                                    .OrderBy(p => p.Name).AsNoTracking().ToListAsync();
            return View(paths);
        }
    }

}
