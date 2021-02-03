using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Dancer
    {
        public Guid Id { get; set; }
        public string AuthenticationId { get; set; }
        public string DdrName { get; set; }
        public string DdrCode { get; set; }
        public string PrimaryMachineLocation { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<Score> Scores { get; set; }
        public ICollection<GradedDancerDish> GradedDishes { get; set; }
        public ICollection<GradedDancerIngredient> GradedIngredients { get; set; }
    }
}