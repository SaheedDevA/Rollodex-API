using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class EngagementHistory:BaseEntity
    {
        public int CompanyId { get; set; }
        public bool HasBeenContacted { get; set; }
        public bool OngoingPartnership { get;set; }
        public bool OngoingConversation { get;set; }
        public string EngagementCall { get;set; }
        public string EngagementEmail { get;set; }
    }
}
