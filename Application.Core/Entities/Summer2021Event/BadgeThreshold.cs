
namespace Application.Core.Entities
{
    public class BadgeThreshold : BaseEntity
    {
        public Badge? Badge { get; set; }
        public int RequiredPoints { get; set; }
    }
}