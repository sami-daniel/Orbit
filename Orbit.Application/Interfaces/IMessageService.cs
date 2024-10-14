using Orbit.Domain.Entities;

namespace Orbit.Application.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetAllMessagesAsync(string user, string with);
    Task AddMessageAsync(Message message, string from);
}
