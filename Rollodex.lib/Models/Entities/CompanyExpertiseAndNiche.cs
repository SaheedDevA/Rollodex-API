using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class CompanyExpertiseAndNiche : BaseEntity
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int CategoryItemId { get; set; }
        public int SystemId { get; set; }
    }
}
