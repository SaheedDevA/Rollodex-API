using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class UseCase:BaseEntity
    {
        public string Title { get; set; }
        public string? FileUrl { get; set; } //can either have url or file path
        public string? FIlePath { get; set; }
        public string SaveAs { get; set; }
        public string Tags { get; set; }
        public DateTime? FileDate { get; set; }
    }
}
