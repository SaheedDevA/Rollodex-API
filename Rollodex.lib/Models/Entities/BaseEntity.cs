using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Created = DateTime.UtcNow;
        }
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdatedBy { get; set; }
    }
}
}
