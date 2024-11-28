namespace Orbit.Services.Interfaces;

public interface IUserPreferenceService
{
    /// <summary>
<<<<<<< HEAD
    /// Updates the user's preferences.
    /// </summary>
    /// <param name="userName">The name of the user whose preferences will be updated.</param>
    /// <param name="postID">The ID of the post related to the preference update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateUserPreferenceAsync(string userName, uint postID);
}
=======
    /// Atualiza as preferências do usuário.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task UpdateUserPreferenceAsync(string userName, uint postID);
}
>>>>>>> 35189761d3d1ce18be8ee88be049a8f9dcaf53a6
