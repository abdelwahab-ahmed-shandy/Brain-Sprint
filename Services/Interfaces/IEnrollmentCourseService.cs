
namespace Services.Interfaces
{
    public interface IEnrollmentCourseService
    {
        public Task<List<Course>> GetAll(string userId);
    }
}
