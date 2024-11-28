using System.Drawing;
using Orbit.Models;

namespace Orbit.Services.Helpers;

internal static class PostServiceHelpers
{
    /// <summary>
    /// Validates the properties of the Post object.
    /// </summary>
    /// <param name="post">The Post object to be validated.</param>
    /// <exception cref="ArgumentNullException">Thrown when the Post object or its required properties are null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when the Post object's properties do not meet validation criteria.</exception>
    public static void ValidatePost(Post post)
    {
        // Check if the post object is null
        ArgumentNullException.ThrowIfNull(post);

        // Validate that the PostContent property is not null or whitespace
        ArgumentNullException.ThrowIfNullOrWhiteSpace(post.PostContent);

        // Ensure that the number of likes is not negative
        if (post.PostLikes < 0)
        {
            throw new ArgumentException("A quantidade de likes não pode ser menor do que 0.");
        }

        // Validate the maximum length of the post content
        if (post.PostContent.Length >= 65535)
        {
            throw new ArgumentException("O post pode ter no máximo 65535 caracteres.");
        }
    }
}
