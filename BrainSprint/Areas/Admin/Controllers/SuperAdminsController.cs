using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace BrainSprint.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("SuperAdmin"))]
    public class SuperAdminsController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuperAdminsController(IApplicationUserRepository applicationUserRepository,
                                        UserManager<ApplicationUser> userManager)
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
        }



        #region View Super Admin
        public async Task<IActionResult> Index(string? query, string? status, int page = 1)
        {
            var applicationUsers = await _applicationUserRepository.Get(tracked: false).ToListAsync();
            var superAdminVMs = new List<SuperAdminVM>();

            foreach (var user in applicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("SuperAdmin"))
                {
                    superAdminVMs.Add(new SuperAdminVM
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        ProfileImage = user.ProfileImage,
                        Status = user.IsBlocked ? "Blocked" : (user.IsActive ? "Active" : "Inactive"),
                        Bio = user.Bio,
                        RegistrationDate = user.RegistrationDate,
                        LastLogin = user.LastLogin,
                        AccountState = user.AccountState ?? AccountStateType.PendingActivation,
                    });
                }
            }

            // Apply filters
            if (!string.IsNullOrEmpty(query))
            {
                superAdminVMs = superAdminVMs.Where(u =>
                    (u.FirstName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.LastName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.Email?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                    (u.PhoneNumber?.Contains(query) == true) ||
                    (u.Status?.Contains(query, StringComparison.OrdinalIgnoreCase) == true))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                superAdminVMs = superAdminVMs
                    .Where(u => u.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Create pagination info
            var pagination = new PaginationVM
            {
                CurrentPage = page,
                PageSize = 5,
                TotalItems = superAdminVMs.Count,
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
            var paginatedSuperAdmins = superAdminVMs
                .Skip((page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            // Create and return view model
            var viewModel = new SuperAdminListVM
            {
                SuperAdmins = paginatedSuperAdmins,
                Pagination = pagination
            };

            return View(viewModel);
        }
        #endregion






    }
}
