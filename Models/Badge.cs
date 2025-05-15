
namespace Models
{
    public class Badge : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int? PointsRequired { get; set; }

        public List<UsersBadge> UsersBadges { get; set; } = new();
    }
}

