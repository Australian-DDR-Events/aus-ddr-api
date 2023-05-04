using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        
        public Guid ChartId { get; set; }
        public Chart? Chart { get; set; }
        
        public virtual ICollection<Dish> Dishes { get; set; } = default!;
    }
}