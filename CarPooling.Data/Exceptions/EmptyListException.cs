using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Exceptions
{
    public class EmptyListException : ApplicationException
    {
        public EmptyListException(string message)
            : base(message)
        {
        }
    }
}
