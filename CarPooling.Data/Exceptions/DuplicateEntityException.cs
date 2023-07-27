﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Exceptions
{
    public class DuplicateEntityException : ApplicationException
    {
        public DuplicateEntityException(string message)
            : base(message)
        {
        }
    }
}
