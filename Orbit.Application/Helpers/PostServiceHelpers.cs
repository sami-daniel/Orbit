using System.Drawing;
using Orbit.Domain.Entities;

namespace Orbit.Application.Helpers;

internal static class PostServiceHelpers
{
    public static void ValidatePost(Post post)
    {
        ArgumentNullException.ThrowIfNull(post);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(post.PostContent);

        if (post.PostLikes < 0)
        {
            throw new ArgumentException("A quantidade de likes não pode ser menor do que 0.");
        }

        if (post.PostContent.Length >= 65535)
        {
            throw new ArgumentException("O post pode ter no máximo 65535 caracteres.");
        }

        if (post.PostDate < DateTime.Now)
        {
            throw new ArgumentException("A data do Post é inválida");
        }

        if (post.PostImageByteType != null)
        {
            try
            {
                using (var ms = new MemoryStream(post.PostImageByteType))
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    Image.FromStream(ms);
#pragma warning restore CA1416 // Validate platform compatibility
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("A imagem do post é invalida");
            }
        }
    }
}
