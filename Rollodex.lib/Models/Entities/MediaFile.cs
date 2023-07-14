using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class MediaFile:BaseEntity
    {
        public string Title { get; set; }
        public string? FileUrl { get; set; } //can either have file path or file url
        public string ?FIlePath { get; set; }
        public string SaveAs { get; set; }
        public string Tags { get; set; }
        public DateTime? FileDate { get;set; }
    }
}
