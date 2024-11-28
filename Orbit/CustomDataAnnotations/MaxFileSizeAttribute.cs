using System.ComponentModel.DataAnnotations;

namespace Orbit.CustomDataAnnotations;

// Custom validation attribute to check the maximum file size for file uploads
public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly long _maxSize; // Store the maximum allowed file size

    // Constructor that initializes the maximum file size
    public MaxFileSizeAttribute(long maxSize)
    {
        _maxSize = maxSize;
    }

    // Override the IsValid method to implement the custom validation logic
    public override bool IsValid(object? value)
    {
        // Check if the value is of type IFormFile (a file upload)
        if (value is IFormFile file)
        {
            // Return true if the file size is less than or equal to the maximum size
            return file.Length <= _maxSize;
        }
        // If the value is not a file (null or other type), consider it valid
        return true;
    }
}
