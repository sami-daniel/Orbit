using System.Drawing;
using System.Text.RegularExpressions;
using Orbit.Models;
using Orbit.Services.Exceptions;

namespace Orbit.Services.Helpers;

internal static class UserServiceHelpers
{
    /// <summary>
    /// Validates the properties of the User object.
    /// </summary>
    /// <param name="user">The User object to be validated.</param>
    /// <exception cref="ArgumentNullException">Thrown when the User object or its required properties are null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when the User object's properties do not meet validation criteria.</exception>
    /// <exception cref="InvalidImageException">Thrown when the user's profile or banner images are invalid.</exception>
    public static void ValidateUser(User user)
    {
        // Check if the user object is null
        ArgumentNullException.ThrowIfNull(user);

        // Validate that required properties are not null or whitespace
        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserName);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserProfileName);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserPassword);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserEmail);

        // Validate maximum length constraints for User properties
        if (user.UserName.Length > 255)
        {
            throw new ArgumentException("O nome de usuário não pode ter mais de 255 caracteres.");
        }

        if (user.UserEmail.Length > 255)
        {
            throw new ArgumentException("O email do usuário não pode ter mais de 255 caracteres.");
        }

        if (user.UserProfileName.Length > 255)
        {
            throw new ArgumentException("O nome do perfil do usuário não pode ter mais de 255 caracteres.");
        }

        if (user.UserPassword.Length > 255)
        {
            throw new ArgumentException("A senha do usuário não pode ter mais de 255 caracteres.");
        }

        if (user.UserDescription != null && user.UserDescription.Length > 65535)
        {
            throw new ArgumentException("A descrição do usuário não pode ter mais de 65535 caracteres.");
        }

        // Value validations: Check if the email and username match the expected formats
        if (!Regex.Match(user.UserEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$").Success)
        {
            throw new ArgumentException("O email do usuário é invalido.");
        }

        if (!Regex.Match(user.UserName, "^[a-zA-Z0-9_]*$").Success)
        {
            throw new ArgumentException("O nome do usuário é invalido. Deve conter somente letras, números e underscore");
        }

        // Check if the profile image is valid
        if (user.UserProfileImageByteType != null)
        {
            try
            {
                using (var ms = new MemoryStream(user.UserProfileImageByteType))
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    Image.FromStream(ms);
#pragma warning restore CA1416 // Validate platform compatibility
                }
            }
            catch (Exception)
            {
                throw new InvalidImageException("A imagem do usuário é inválida.");
            }
        }

        // Check if the banner image is valid
        if (user.UserProfileBannerImageByteType != null)
        {
            try
            {
                using (var ms = new MemoryStream(user.UserProfileBannerImageByteType))
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    Image.FromStream(ms);
#pragma warning restore CA1416 // Validate platform compatibility
                }
            }
            catch (Exception)
            {
                throw new InvalidImageException("A imagem de banner do usuário é inválida.");
            }
        }
    }
}
