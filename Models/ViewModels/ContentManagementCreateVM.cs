using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.ViewModels
{
    public class ContentManagementCreateVM
    {
        // LearningPaths
        public class LearningPathsCreateVM
        {
            [Required(ErrorMessage = "Name is required.")]
            [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Description is required.")]
            [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
            public string Description { get; set; } = string.Empty;

            [Url(ErrorMessage = "Please enter a valid URL.")]
            public string? IconUrl { get; set; }
        }

        // Courses
        public class CoursesCreateVM
        {
            [Required(ErrorMessage = "Title required")]
            [StringLength(100, ErrorMessage = "Title must not exceed 100 characters")]
            public string Title { get; set; } = string.Empty;

            [Required(ErrorMessage = "Description is required.")]
            [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
            public string Description { get; set; } = string.Empty;

            [Required(ErrorMessage = "Price required")]
            [Range(0.0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
            public double Price { get; set; }

            [Range(0.0, 100.0, ErrorMessage = "Discount must be between 0 and 100")]
            public double? Discount { get; set; }

            [Required(ErrorMessage = "Duration required")]
            [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 minutes")]
            public int Duration { get; set; }

            [Url(ErrorMessage = "Invalid video URL")]
            public string? VideoUrl { get; set; }

            [Url(ErrorMessage = "Invalid image URL")]
            public string? ImgUrl { get; set; }
        }

        public class CourseCreateFormVM
        {
            public CoursesCreateVM courses { get; set; }
            //public IEnumerable<SelectListItem> Instructors { get; set; } = new List<SelectListItem>();
            //public IEnumerable<SelectListItem> LearningPaths { get; set; } = new List<SelectListItem>();
        }

    }

}