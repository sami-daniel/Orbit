using System.ComponentModel.DataAnnotations;

namespace Orbit.Application.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValid(object obj)
        {
            ValidationContext validationContext = new(obj);

            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults);

            if (!isValid)
            {
                return false;
            }

            return true;
        }

    }
}
