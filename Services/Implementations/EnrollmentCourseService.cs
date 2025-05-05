

namespace Services.Implementations
{
    public class EnrollmentCourseService : IEnrollmentCourseService
    {
        private readonly IStudentRepository _studentRepo;

        public EnrollmentCourseService(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public async Task<List<Course>> GetAll(string userId)
        {
            try
            {
                var student = await _studentRepo.GetOneAsync(s => s.ApplicationUserId == userId,
                includes: new Expression<Func<Student, object>>[]
                {
                    s => s.EnrollmentCourses
                },
                thenIncludes: new Func<IQueryable<Student>, IQueryable<Student>>[]
                {
                    q => q.Include(s => s.EnrollmentCourses)
                          .ThenInclude(ec => ec.Course).ThenInclude(th=>th.Instructor).ThenInclude(us=>us.ApplicationUser)
                });

                if (student == null)
                    throw new Exception("can't found the user");

                List<Course> courses = new List<Course>();
                foreach (var EC in student.EnrollmentCourses)
                    courses.Add(EC.Course);

                return courses;
            }
            catch (Exception ex)
            {
                throw new Exception("can't found the user");
            }
        }
    }
}
