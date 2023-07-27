using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Exceptions
{
    public class EntityUnauthorizatedException : ArgumentException
    {
        public EntityUnauthorizatedException(string message) 
            : base(message) 
        {
        }
    }
}
