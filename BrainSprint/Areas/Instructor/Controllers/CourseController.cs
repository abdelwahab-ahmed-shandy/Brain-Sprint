using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;

namespace BrainSprint.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor,Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository courseRepository;
        private readonly ILearningPathRepository learningPathRepository;
        private readonly ICourseLearningPathRepository courseLearningPathRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(ICourseRepository courseRepository, ILearningPathRepository learningPathRepository, ICourseLearningPathRepository courseLearningPathRepository, UserManager<ApplicationUser> userManager)
        {
            this.courseRepository = courseRepository;
            this.learningPathRepository = learningPathRepository;
            this.courseLearningPathRepository = courseLearningPathRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var courses = courseRepository.Get().ToList();
            return View(courses);
        }
        [HttpGet]
        /* public async Task<IActionResult> Create()
         {
             var user = await userManager.GetUserAsync(User);
             if (user == null) return Challenge();

             var instructor = await context.Instructors
                 .FirstOrDefaultAsync(i => i.UserId == user.Id);

             if (instructor == null)
             {
                 return Forbid();
             }

             var model = new ContentManagementCreateVM.CoursesCreateVM
             {
                 CurrentState = CurrentState.Active,
                 CourseLevel = CourseLevel.Beginner,
                 CreatedBy = user.UserName,
                 InstructorId = instructor.Id
             };

             return View(model);
         }
         [HttpPost]
         public IActionResult Create(CourseVM courseVM,IFormFile ImgFile,IFormFile VideoFile)
         {
             ModelState.Remove("ImgUrl");
             ModelState.Remove("VideoUrl");
             ModelState.Remove("ImgFile");
             ModelState.Remove("VideoFile");
             ModelState.Remove("CreatedBy");
             if (ModelState.IsValid) {
                 var Course = new Course
                 {
                     Title = courseVM.Title,
                     Description = courseVM.Description,
                     Price = courseVM.Price,
                     Duration = courseVM.Duration,
                     Discount = courseVM.Discount,
                     VideoUrl = courseVM.VideoUrl,
                     ImgUrl = courseVM.ImgUrl,
                     CurrentState=courseVM.CurrentState,
                     CourseLevel=courseVM.CourseLevel,
                     CreatedBy=User.Identity.Name,
                     CreatedDateUtc= DateTime.Now


                 };
                 // معالجة ملف الصورة
                 if (ImgFile != null && ImgFile.Length > 0)
                 {
                     var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                     var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Courses", imageName);

                     using (var stream = System.IO.File.Create(imagePath))
                     {
                          ImgFile.CopyTo(stream); // استخدام النسخ غير المتزامن
                     }
                     courseVM.ImgUrl = imageName;
                 }

                 // معالجة ملف الفيديو
                 if (VideoFile != null && VideoFile.Length > 0)
                 {
                     var videoName = Guid.NewGuid().ToString() + Path.GetExtension(VideoFile.FileName);
                     var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos/Courses", videoName);

                     using (var stream = System.IO.File.Create(videoPath))
                     {
                          VideoFile.CopyTo(stream);
                     }
                     courseVM.VideoUrl = videoName;
                 }

                 courseRepository.CreateAsync(Course);
                 courseRepository.SaveInDataBaseAsync();
                 return RedirectToAction("Index");
             }
             return View(courseVM);
         }
        */
        public IActionResult Delete(int CourseId)
        {
            var course = courseRepository.GetOne(e => e.Id == CourseId);
            if (ModelState.IsValid)
            {
                courseRepository.Delete(course);
                courseRepository.SaveDB();
                return RedirectToAction("Index");
            }

            return View(course);
        }
    }
}
