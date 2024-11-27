using System.ComponentModel.DataAnnotations;

namespace Orbit.CustomDataAnnotations;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly long _maxSize;

    public MaxFileSizeAttribute(long maxSize)
    {
        _maxSize = maxSize;
    }

    public override bool IsValid(object? value)
    {
        if (value is IFormFile file)
        {
            return file.Length <= _maxSize;
        }
        return true;
    }
}