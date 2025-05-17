using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class InstructorDashboardVM
    {
        public int TotalCourses { get; set; }
        public int TotalLearningPaths { get; set; }
        public int TotalStudents { get; set; }
        public int TotalReviews { get; set; }
        public int TotalCertificatesIssued { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CanceledOrders { get; set; }

    }

}
