using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class PendingCategoryItem : BaseEntity
    {
        public string SuggestedBy { get; set; }
        public string CategoryItemName { get; set; }
        public int CategoryId { get; set; }
    }
}
