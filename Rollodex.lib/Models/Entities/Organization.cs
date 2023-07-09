using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class Organization:BaseEntity
    {
        public string OrganizationName { get; set; }
        public string OrganizationEmail { get; set; }
        public string ContactPersonEmail { get; set;}
        public int RollodexSystemId { get; set; }
    }
}
