using Orbit.Application.Interfaces;
using Orbit.Domain.Entities;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Orbit.Application.Services;

public class MessageService : IMessageService
{
    private const string CollectionName = "messages";
    private readonly string _firebaseUrl;
    private FirebaseClient Client
    {
        get
        {
            return new FirebaseClient(_firebaseUrl);
        }
    }

    public MessageService(IConfiguration configuration)
    {
        _firebaseUrl = configuration["ConnectionStrings:Firebase"] ?? throw new InvalidOperationException();
    }

    public async Task<IEnumerable<Message>> GetAllMessagesAsync(string user, string with)
    {
        var json = await Client.Child(CollectionName)
                                     .Child(user)
                                     .OrderByKey()
                                     .OnceAsJsonAsync();

        var messagesAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, Message>>(json);
        var messages = messagesAsDictionary?.Values.Where(m => m.To == with);

        return messages ?? new Dictionary<string, Message>().Values;
    }

    public async Task AddMessageAsync(Message message, string from)
    {
        await Client.Child(CollectionName)
                    .Child(from)
                    .PostAsync(message);
    }
}
