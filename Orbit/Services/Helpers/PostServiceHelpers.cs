using System.Drawing;
using Orbit.Models;

namespace Orbit.Services.Helpers;

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
    }
}
