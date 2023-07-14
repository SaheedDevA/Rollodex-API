using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class CompanyContactInformation:BaseEntity
    {
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string HeadOfficeLocation { get; set; }
        public string OfficeLocations { get; set; } //json list of strings
        public string LocationsWIthPaidStaff { get; set; } //json list of string
        public string ContactPersonsName { get; set; }
        public string ContactPersonsEmail { get; set; }
        public int CompanyId { get; set; }
    }
}
