using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class UseCase:BaseEntity
    {
        public string? FileUrl { get; set; }
        public string? FIlePath { get; set; }
        public string SaveAs { get; set; }
        public string Tags { get; set; }
    }
}
