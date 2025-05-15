
namespace Models.ViewModels
{
    public class ContentManagementEditVM
    {
        // Learning Paths : 
        public class LearningPathEditVM
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string? IconUrl { get; set; }
        }


    }
}
