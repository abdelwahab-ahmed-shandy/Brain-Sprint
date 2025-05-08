using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
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

        [HttpGet]
        public IActionResult Index()
        {
            var learningPaths = learningPathRepository.Get().ToList();

            var learningPathVMs = learningPaths.Select(lp => new ContentManagementVM
            {
                Id = lp.Id,
                Name = lp.Name,
                Description = lp.Description,
                IconUrl = lp.IconUrl,
                CreatedDateUtc = lp.CreatedDateUtc
            }).ToList();

            return View(learningPathVMs);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new LearningPathsCreateVM());
        }
        [HttpPost]
        public IActionResult Create(IFormFile ImgFile, ContentManagementCreateVM.LearningPathsCreateVM learningPathsCreateVM)
        {
            ModelState.Remove("IconUrl");
            ModelState.Remove("ImgFile");

            if (ModelState.IsValid)
            {
                if (ImgFile != null && ImgFile.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/learningPaths", imageName);

                    using (var stream = System.IO.File.Create(imagePath))
                    {
                        ImgFile.CopyTo(stream);
                    }
                    learningPathsCreateVM.IconUrl = imageName;
                }
                var learningPath = new LearningPath
                {
                    Name = learningPathsCreateVM.Name,
                    Description = learningPathsCreateVM.Description,
                    CreatedBy = User.Identity.Name,
                    IconUrl = learningPathsCreateVM.IconUrl,
                    CreatedDateUtc = DateTime.UtcNow

                };
                learningPathRepository.Create(learningPath);
                learningPathRepository.SaveDB();
                return View("Index");

            }
            return View(learningPathsCreateVM);
        }


        [HttpGet]
        public IActionResult Edit(int PathId)
        {
            var path = learningPathRepository.GetOne(e => e.Id == PathId);
            var pathVM = new ContentManagementEditVM.LearningPathEditVM
            {
                Id = PathId,
                Name = path.Name,
                Description = path.Description,
                IconUrl = path.IconUrl,


            };
            return View(pathVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IFormFile ImgFile, ContentManagementEditVM.LearningPathEditVM learningPathEditVM)
        {
            ModelState.Remove("IconUrl");
            ModelState.Remove("ImgFile");

            if (ModelState.IsValid)
            {
                var pathInDB = learningPathRepository.GetOne(e => e.Id == learningPathEditVM.Id, tracked: false);
                if (pathInDB == null)
                {
                    TempData["ErrorMessage"] = "Learning path not found";

                    return NotFound();
                }

                if (ImgFile != null && ImgFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\learningPaths", fileName);

                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\learningPaths", pathInDB.IconUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    using (var Stream = System.IO.File.Create(filePath))
                    {
                        ImgFile.CopyTo(Stream);

                    }
                    learningPathEditVM.IconUrl = fileName;
                }
                else
                {
                    learningPathEditVM.IconUrl = pathInDB.IconUrl;
                }
                pathInDB.Name = learningPathEditVM.Name;
                pathInDB.Description = learningPathEditVM.Description;
                pathInDB.UpdatedBy = User.Identity.Name;
                pathInDB.UpdatedDateUtc = DateTime.UtcNow;
                learningPathRepository.Edit(pathInDB);
                learningPathRepository.SaveDB();
                TempData["SuccessMessage"] = "Learning path updated successfully";

                return View("Index");


            }
            return View(learningPathEditVM);
        }

        public IActionResult Delete(int pathId)
        {
            var path = learningPathRepository.GetOne(e => e.Id == pathId);
            if (ModelState.IsValid)
            {
                learningPathRepository.Delete(path);
                learningPathRepository.SaveDB();
                return RedirectToAction("Index");
            }

            return View(path);
        }



    }
























}
