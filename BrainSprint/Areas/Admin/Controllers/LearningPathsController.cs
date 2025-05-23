using static Models.ViewModels.ContentManagementCreateVM;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin"))]
    public class LearningPathsController : Controller
    {
        private readonly ILearningPathRepository _learningPathRepository;
        private readonly ILogger<LearningPathsController> _logger;
        public LearningPathsController(ILearningPathRepository learningPathRepository, ILogger<LearningPathsController> logger)
        {
            _learningPathRepository = learningPathRepository;
            _logger = logger;
        }


        #region View All LearningPaths
        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var learningPaths = _learningPathRepository.Get().ToList();

            var learningPathVMs = learningPaths.Select(lp => new ContentManagementVM
            {
                Id = lp.Id,
                Name = lp.Name,
                Description = lp.Description,
                IconUrl = lp.IconUrl,
                IsPublished = lp.IsPublished,
            }).ToList();

            // Apply filters
            if (!string.IsNullOrEmpty(query))
            {
                learningPathVMs = learningPathVMs.Where(u =>
                    (u.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.IconUrl?.Contains(query, StringComparison.OrdinalIgnoreCase) == true)
                ).ToList();
            }

            // Create pagination info
            var pagination = new PaginationVM
            {
                CurrentPage = page,
                PageSize = 5,
                TotalItems = learningPathVMs.Count,
                Query = query,
                StatusFilter = status
            };

            // Validate page against total pages
            if (page > pagination.TotalPages && pagination.TotalPages > 0)
            {
                page = pagination.TotalPages;
                pagination.CurrentPage = page;
            }

            // Apply pagination
            var paginatedlearningPath = learningPathVMs
                .Skip((page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return View(new LearningPathsVM
            {
                Pagination = pagination,
                LearningPaths = paginatedlearningPath
            });

        }

        #endregion


        #region Create New LearningPath
        [HttpGet]
        public IActionResult Create()
        {
            return View(new LearningPathsCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentManagementCreateVM.LearningPathsCreateVM model, IFormFile icon)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (icon != null && icon.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg" };
                    var fileExtension = Path.GetExtension(icon.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("icon", "The file type is not allowed. Please upload an image in JPG, PNG, or SVG format");
                        return View(model);
                    }

                    // Create directory if it doesn't exist
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Assets", "Admin", "IconelearningPaths");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await icon.CopyToAsync(stream);
                    }

                    model.IconUrl = $"/Assets/Admin/IconelearningPaths/{uniqueFileName}";
                }

                var learningPath = new LearningPath
                {
                    Name = model.Name,
                    Description = model.Description,
                    IconUrl = model.IconUrl,
                    CreatedBy = User.Identity.Name,
                    CreatedDateUtc = DateTime.UtcNow
                };

                await _learningPathRepository.CreateAsync(learningPath);
                await _learningPathRepository.SaveInDataBaseAsync();

                TempData["notification"] = "The LearningPath Was Created Successfully!";
                TempData["MessageType"] = "success";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception error)
            {
                ModelState.AddModelError("", $"An error occurred while saving: {error.Message}");
                return View(model);
            }
        }

        #endregion




        #region Edit Learning Path

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var learningPath = _learningPathRepository.GetOne(lp => lp.Id == Id);

            if (learningPath == null)
            {
                TempData["notification"] = "The LearningPath Was Not Found!";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            var pathEdit = new ContentManagementEditVM.LearningPathEditVM
            {
                Id = learningPath.Id,
                Name = learningPath.Name,
                Description = learningPath.Description,
                IconUrl = learningPath.IconUrl,

            };

            return View(pathEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContentManagementEditVM.LearningPathEditVM learningPath)
        {
            if (!ModelState.IsValid)
            {
                return View(learningPath);
            }

            try
            {
                var learningPathInDB = _learningPathRepository.GetOne(lp => lp.Id == learningPath.Id);
                if (learningPathInDB == null)
                {
                    TempData["notification"] = "The LearningPath Was Not Found!";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Index));
                }

                // Update other properties
                learningPathInDB.Name = learningPath.Name;
                learningPathInDB.Description = learningPath.Description;
                learningPathInDB.UpdatedBy = User.Identity.Name;
                learningPathInDB.UpdatedDateUtc = DateTime.UtcNow;
                learningPathInDB.IconUrl = learningPath.IconUrl;

                await _learningPathRepository.EditAsync(learningPathInDB);
                await _learningPathRepository.SaveInDataBaseAsync();

                TempData["notification"] = "Learning path updated successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the learning path");
                TempData["notification"] = "An error occurred while updating the learning path";
                TempData["MessageType"] = "error";
                return View(learningPath);
            }
        }

        #endregion



        #region Detils Learning Path

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var learningPath = _learningPathRepository.GetOne(lp => lp.Id == Id);

            if (learningPath == null)
            {
                TempData["notification"] = "The LearningPath Was Not Found!";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            var pathDetails = new ContentManagementVM
            {
                Id = learningPath.Id,
                Name = learningPath.Name,
                Description = learningPath.Description,
                IconUrl = learningPath.IconUrl,
                CreatedBy = learningPath.CreatedBy,
                CreatedDateUtc = learningPath.CreatedDateUtc,
                UpdatedBy = learningPath.UpdatedBy,
                UpdatedDateUtc = learningPath.UpdatedDateUtc,
                BlockedBy = learningPath.BlockedBy
            };

            return View(pathDetails);
        }

        #endregion


        #region Delete Learning Path

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var learningPath = _learningPathRepository.GetOne(lp => lp.Id == Id);
            if (learningPath == null)
            {
                TempData["notification"] = "The LearningPath Was Not Found!";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Delete the icon file if it exists
                if (!string.IsNullOrEmpty(learningPath.IconUrl))
                {
                    var fileName = Path.GetFileName(learningPath.IconUrl);
                    var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Assets\\Admin\\IconelearningPaths", fileName);

                    if (System.IO.File.Exists(oldPhysicalPath))
                    {
                        System.IO.File.Delete(oldPhysicalPath);
                    }
                }

                // Delete the learning path from database
                await _learningPathRepository.DeleteAsync(learningPath);
                await _learningPathRepository.SaveInDataBaseAsync();

                TempData["notification"] = "The LearningPath Was Deleted Successfully!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting learning path with ID {Id}");
                TempData["notification"] = "An error occurred while deleting the learning path!";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region Publish && Unpublish 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            var learningPath = _learningPathRepository.GetOne(lp => lp.Id == id);
            if (learningPath == null)
            {
                TempData["notification"] = "Learning path not found!";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            learningPath.IsPublished = true;
            learningPath.UpdatedDateUtc = DateTime.UtcNow;
            learningPath.UpdatedBy = User.Identity.Name;
            learningPath.CurrentState = CurrentState.Active;

            await _learningPathRepository.EditAsync(learningPath);
            await _learningPathRepository.SaveInDataBaseAsync();



            TempData["notification"] = "Learning path has been published.";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpublish(int id)
        {
            var learningPath = _learningPathRepository.GetOne(lp => lp.Id == id);
            if (learningPath == null)
            {
                TempData["notification"] = "Learning path not found!";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            learningPath.IsPublished = false;
            learningPath.UpdatedDateUtc = DateTime.UtcNow;
            learningPath.UpdatedBy = User.Identity.Name;
            learningPath.CurrentState = CurrentState.Inactive;

            await _learningPathRepository.EditAsync(learningPath);
            await _learningPathRepository.SaveInDataBaseAsync();

            TempData["notification"] = "Learning path has been unpublished.";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(Index));
        }

        #endregion




    }
}
