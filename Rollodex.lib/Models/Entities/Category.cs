using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Entities
{
    public class Category:BaseEntity
    {
        public int CategoryId { get;set; }
        public string CategoryName { get; set; }
        public int SystemId { get;set; }
    }
}
