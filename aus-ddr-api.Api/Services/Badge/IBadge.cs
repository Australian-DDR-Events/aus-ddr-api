using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BadgeEntity = AusDdrApi.Entities.Badge;

namespace AusDdrApi.Services.Badges
{
    public interface IBadge
    {
        public IEnumerable<BadgeEntity> GetAll();
        public BadgeEntity? Get(Guid badgeId);

        public Task<BadgeEntity?> Add(BadgeEntity badge);
        public BadgeEntity? Update(BadgeEntity badge);
        public void Delete(Guid badgeId);

        public bool AssignBadge(Guid badgeId, Guid dancerId);
        public bool RevokeBadge(Guid badgeId, Guid dancerId);

    }
}