using Microsoft.AspNetCore.Mvc;
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
                Name = lp.Name,
                Description = lp.Description,
                IconUrl = lp.IconUrl,
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
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "IconelearningPaths");
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

                    model.IconUrl = $"/Admin/IconelearningPaths/{uniqueFileName}";
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


        #region Edit LearningPath

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
                IconUrl = learningPath.IconUrl
            };

            return View(pathEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContentManagementEditVM.LearningPathEditVM learningPath, IFormFile icon)
        {
            if (!ModelState.IsValid)
            {
                return View(learningPath);
            }

            // Get the existing learning path with tracking enabled
            var learningPathInDB = _learningPathRepository.GetOne(lp => lp.Id == learningPath.Id);

            if (learningPathInDB == null)
            {
                TempData["notification"] = "The LearningPath Was Not Found!";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

            // Save the old icon path before potential update
            var oldIconPath = learningPathInDB.IconUrl;

            if (icon != null && icon.Length > 0)
            {
                var allowedExten = new[] { ".jpg", ".jpeg", ".png", ".svg" };
                var fileExten = Path.GetExtension(icon.FileName).ToLower();

                if (!allowedExten.Contains(fileExten))
                {
                    ModelState.AddModelError("icon", "The file type is not allowed. Please upload an image in JPG, PNG, or SVG format");
                    return View(learningPath);
                }

                // Create directory if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "IconelearningPaths");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}{fileExten}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await icon.CopyToAsync(stream);
                }

                // Update with new icon path
                learningPathInDB.IconUrl = $"/Admin/IconelearningPaths/{uniqueFileName}";

                // Delete old icon file if it exists
                if (!string.IsNullOrEmpty(oldIconPath))
                {
                    var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldIconPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPhysicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldPhysicalPath);
                        }
                        catch (Exception ex)
                        {
                            // Log the error but don't stop the process
                            _logger.LogError(ex, $"Failed to delete old icon file: {oldPhysicalPath}");
                            // You could optionally add a message to TempData to inform admin
                        }
                    }
                }
            }

            // Update other properties
            learningPathInDB.Name = learningPath.Name;
            learningPathInDB.Description = learningPath.Description;
            learningPathInDB.UpdatedBy = User.Identity.Name;
            learningPathInDB.UpdatedDateUtc = DateTime.UtcNow;

            await _learningPathRepository.EditAsync(learningPathInDB);
            await _learningPathRepository.SaveInDataBaseAsync();

            TempData["notification"] = "The LearningPath Was Edited Successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));
        }


        #endregion

    }
}
