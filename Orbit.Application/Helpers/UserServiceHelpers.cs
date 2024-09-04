using System.Drawing;
using System.Text.RegularExpressions;
using Orbit.Application.Exceptions;
using Orbit.Domain.Entities;

internal static class UserServiceHelpers
{
    public static void ValidateUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserName);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserProfileName);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserPassword);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(user.UserEmail);

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

        // Value validations, like valid email, valid username ...
        if (!Regex.Match(user.UserEmail, "^[^\\.\\s][\\w\\-\\.{2,}]+@([\\w-]+\\.)+[\\w-]{2,}$").Success)
        {
            throw new ArgumentException("O email do usuário é invalido.");
        }

        if (!Regex.Match(user.UserName, "^[a-zA-Z0-9_]*$").Success)
        {
            throw new ArgumentException("O nome do usuário é invalido. Deve conter somente letras, números e underscore");
        }

        // Verifing valid images
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
