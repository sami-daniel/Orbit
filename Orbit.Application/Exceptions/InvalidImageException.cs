﻿using System.Runtime.Serialization;

namespace Orbit.Application.Exceptions;
public class InvalidImageException : Exception
{
    public InvalidImageException()
    {
    }

    public InvalidImageException(string? message) : base(message)
    {
    }

    public InvalidImageException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}