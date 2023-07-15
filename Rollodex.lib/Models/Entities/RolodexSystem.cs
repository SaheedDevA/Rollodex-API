using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class RolodexSystem : BaseEntity
    {
        public string SystemName { get; set; }
        public string WorkSpaceUrl { get; set; }
        public string ThemeColor { get; set; }
        public string SystemLogo { get;set; }
        public int AdminId { get; set; }
        public bool HasCreatedDataSet { get; set; }
        public bool HasInvitedOrganizations { get; set; }
        public bool HasStartedMapping { get; set;}
    }
}
