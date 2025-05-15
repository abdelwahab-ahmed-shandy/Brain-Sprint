using static Models.ViewModels.ContentManagementCreateVM;
using static Models.ViewModels.ContentManagementEditVM;

namespace BrainSprint.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor,Admin,SuperAdmin")]
    public class LearnningPathController : Controller
    {
        private readonly ILearningPathRepository learningPathRepository;

        public LearnningPathController(ILearningPathRepository learningPathRepository)
        {
            this.learningPathRepository = learningPathRepository;
        }


    }
}
