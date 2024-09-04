using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Application.Exceptions;
public class UserAlredyExistsException : Exception
{
    public UserAlredyExistsException()
    {
    }

    public UserAlredyExistsException(string? message) : base(message)
    {
    }

    public UserAlredyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
