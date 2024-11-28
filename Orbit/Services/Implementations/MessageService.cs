using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Orbit.Models;
using Orbit.Services.Interfaces;

namespace Orbit.Services.Implementations;

public class MessageService : IMessageService
{
    // Constant for the Firebase collection name where messages are stored
    private const string CollectionName = "messages";

    // Firebase URL to connect to the Firebase database
    private readonly string _firebaseUrl;

    // Property to get an instance of the FirebaseClient to interact with the Firebase database
    private FirebaseClient Client
    {
        get
        {
            return new FirebaseClient(_firebaseUrl);
        }
    }

    // Constructor that initializes the service with the Firebase URL from the configuration
    public MessageService(IConfiguration configuration)
    {
        _firebaseUrl = configuration["ConnectionStrings:Firebase"] ?? throw new InvalidOperationException();
    }

    // Method to retrieve all messages sent to a specific user by another user
    public async Task<IEnumerable<Message>> GetAllMessagesAsync(string user, string with)
    {
        // Fetch messages for the user from Firebase
        var json = await Client.Child(CollectionName)
                                     .Child(user)
                                     .OrderByKey()
                                     .OnceAsJsonAsync();

        // Deserialize the JSON response to a dictionary of messages
        var messagesAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, Message>>(json);

        // Filter messages where the recipient matches the 'with' parameter
        var messages = messagesAsDictionary?.Values.Where(m => m.To == with);

        // Return the filtered messages or an empty collection if no messages were found
        return messages ?? new Dictionary<string, Message>().Values;
    }

    // Method to add a new message to Firebase for a specific user
    public async Task AddMessageAsync(Message message, string from)
    {
        // Post the message under the user's name in Firebase
        await Client.Child(CollectionName)
                    .Child(from)
                    .PostAsync(message);
    }
}
