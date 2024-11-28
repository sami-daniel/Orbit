namespace Orbit.Services.Interfaces;

public interface IUserPreferenceService
{
    /// <summary>
    /// Updates the user's preferences.
    /// </summary>
    /// <param name="userName">The name of the user whose preferences will be updated.</param>
    /// <param name="postID">The ID of the post related to the preference update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateUserPreferenceAsync(string userName, uint postID);
}
