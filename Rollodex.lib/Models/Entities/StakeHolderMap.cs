using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class StakeHolderMap:BaseEntity
    {
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
    }
}
