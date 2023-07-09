using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class Company:BaseEntity
    {
        public string CompanyName { get; set; } 
        public string CompanyLogo { get; set; }
        public string WebsiteUrl { get; set; }
        public string YearFounded { get; set; }
        public string CompanyBio { get; set; }

    }
}
