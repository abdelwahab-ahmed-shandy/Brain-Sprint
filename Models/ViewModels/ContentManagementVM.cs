
namespace Models.ViewModels
{
    public class ContentManagementVM
    {
        //Base:
        public int Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public string? BlockedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? UpdatedDateUtc { get; set; }
        public bool? IsPublished { get; set; } = false;
        public CourseStatus Status { get; set; } = CourseStatus.Pending;
        public string? RejectionReason { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public string? ReviewedBy { get; set; }



        // LearningPaths
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? IconUrl { get; set; }
        public int CourseCount { get; set; }
        public List<CourseDetailsVM> Courses { get; set; } = new();


        // Courses
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public double? Discount { get; set; }
        public int? Duration { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImgUrl { get; set; }


        // Instructors
        public string? InstructorName { get; set; }
        public string? LearningPathName { get; set; }


    }
    public class LearningPathsVM
    {
        public IEnumerable<ContentManagementVM> LearningPaths { get; set; }
        public PaginationVM Pagination { get; set; }
    }

    public class CourseDetailsVM
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public CourseStatus Status { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        public string? ThumbnailUrl { get; set; }
    }

    public class LearningPathDeleteVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CourseCount { get; set; }
        public string CreatedDate { get; set; } = string.Empty;
    }

    public class CoursesVM
    {
        public IEnumerable<ContentManagementVM> Courses { get; set; }
        public PaginationVM Pagination { get; set; }

    }

    public class CourseLearningPathCustomerVM
    {
        public List<ContentManagementVM> CourseLearningPaths { get; set; }
        public PaginationVM Pagination { get; set; }
        public string? SearchQuery { get; set; }
        public string? StatusFilter { get; set; }
    }

}
