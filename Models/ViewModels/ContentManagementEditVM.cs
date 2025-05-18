
namespace Models.ViewModels
{
    public class ContentManagementEditVM
    {
        // Learning Paths : 
        public class LearningPathEditVM
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Name is required.")]
            [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Description is required.")]
            [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
            public string Description { get; set; } = string.Empty;

            [Url(ErrorMessage = "Please enter a valid URL.")]
            [Display(Name = "Icon URL")]
            public string? IconUrl { get; set; }
        }


    }
}
