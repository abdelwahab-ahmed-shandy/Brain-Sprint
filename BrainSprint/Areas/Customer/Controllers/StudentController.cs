
namespace BrainSprint.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Student Dashboard
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var applicationUserId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(applicationUserId)) return Unauthorized();



                var student = await _context.Students
                          .Where(s => s.ApplicationUserId == applicationUserId)
                          .Select(s => new
                          {
                              s.Id,
                              s.ApplicationUser.FirstName,
                              s.ApplicationUser.LastName,
                              s.Level,
                              EnrollmentCourses = s.EnrollmentCourses.Select(e => new
                              {
                                  e.Course.Id,
                                  e.Course.Title,
                                  e.Progress,
                                  e.IsCompleted
                              }),
                              UsersBadges = s.UsersBadges.Select(b => new
                              {
                                  b.Id,
                                  b.Badge.Name,
                                  b.AwardedDate
                              }),
                              Orders = s.Orders.Select(o => new
                              {
                                  o.Id,
                                  o.OrderDate,
                                  o.TotalAmount,
                                  o.Status
                              }),
                              ExamAttempts = s.UserExamAttemps.Select(a => new
                              {
                                  a.ExamId,
                                  a.Exam.Title,
                                  a.ExamScore,
                                  a.StartedAt,
                                  a.FinishedAt
                              }),
                              UsersWatchedNodesCount = s.UsersWatchedNodes.Count,
                              LatestCart = s.Carts
                                  .OrderByDescending(c => c.Id)
                                  .Select(c => new
                                  {
                                      CartItemsCount = c.CartItems.Count,
                                      CartTotal = c.CartItems.Sum(i => i.PriceAtPurchase)
                                  })
                                  .FirstOrDefault(),
                              CourseReviews = s.CourseReviews.Select(r => new
                              {
                                  r.CourseId,
                                  r.Course.Title,
                                  r.Rating,
                                  r.Comment
                              })
                          })
                          .AsNoTracking()
                          .FirstOrDefaultAsync();

                if (student == null)
                    return NotFound();


                var vm = new StudentDashboardVM
                {
                    StudentId = student.Id,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    Level = student.Level,
                    EnrolledCourses = student.EnrollmentCourses.Select(e => new StudentDashboardVM.CourseProgressInfo
                    {
                        CourseId = e.Id,
                        CourseTitle = e.Title,
                        Progress = Math.Clamp(e.Progress ?? 0, 0, 100),
                        IsCompleted = e.IsCompleted
                    }).ToList(),
                    Badges = student.UsersBadges.Select(b => new StudentDashboardVM.BadgeInfo
                    {
                        BadgeId = b.Id,
                        BadgeName = b.Name,
                        AwardedDate = b.AwardedDate
                    }).ToList(),
                    Orders = student.Orders.Select(o => new StudentDashboardVM.OrderInfo
                    {
                        OrderId = o.Id,
                        OrderDate = o.OrderDate,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status
                    }).ToList(),
                    ExamAttempts = student.ExamAttempts.Select(a => new StudentDashboardVM.ExamAttemptInfo
                    {
                        ExamId = a.ExamId,
                        ExamTitle = a.Title,
                        UserScore = a.ExamScore,
                        StartedAt = a.StartedAt,
                        FinishedAt = a.FinishedAt
                    }).ToList(),
                    TotalNodesWatched = student.UsersWatchedNodesCount,
                    TotalNodes = await _context.Nodes.CountAsync(),
                    CartItemsCount = student.LatestCart?.CartItemsCount ?? 0,
                    CartTotalPrice = student.LatestCart?.CartTotal ?? 0,
                    Reviews = student.CourseReviews.Select(r => new StudentDashboardVM.CourseReviewInfo
                    {
                        CourseId = r.CourseId,
                        CourseTitle = r.Title,
                        Rating = r.Rating,
                        Comment = r.Comment
                    }).ToList()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        #endregion

    }
}
