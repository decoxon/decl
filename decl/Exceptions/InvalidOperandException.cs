using System;
using System.Collections.Generic;
using System.Text;

namespace declang
{
  public class InvalidOperandException : Exception
  {
    public InvalidOperandException(string message, Exception innerException = null)
        : base($"Invalid operand: {message}", innerException) { }
  }
}
