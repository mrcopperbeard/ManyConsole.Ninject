﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ManyConsole.Ninject
{
    public class ConsoleHelpAsException : Exception
    {
        public ConsoleHelpAsException(string message) : base(message)
        {
        }
    }
}
