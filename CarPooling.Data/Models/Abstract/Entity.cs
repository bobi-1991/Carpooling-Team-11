using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Models.Abstract
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime DeleteOn { get; set; }
    }
}
