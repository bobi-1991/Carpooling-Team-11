using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Exceptions
{
    public class DriverPassengerMachingException : ApplicationException
    {
        public DriverPassengerMachingException(string message)
            : base(message)
        {
        }
    }
}