namespace Orbit.Services.Interfaces;

public interface IUserPreferenceService
{
    /// <summary>
    /// Atualiza as preferências do usuário.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task UpdateUserPreferenceAsync(string userName, uint postID);
}