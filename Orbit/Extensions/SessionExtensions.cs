using System.Text.Json;

namespace Orbit.Extensions;

public static class SessionExtensions
{
    /// <summary>
    /// Extension method to set an object into the session by serializing it into a byte array.
    /// </summary>
    /// <param name="session">The session to store the object in.</param>
    /// <param name="key">The key under which the object is stored in the session.</param>
    /// <param name="value">The object to be stored in the session.</param>
    public static void SetObject(this ISession session, string key, object value)
    {
        // Serialize the object to a byte array using UTF-8 encoding, with reference handling for circular references.
        byte[] serializedValue = JsonSerializer.SerializeToUtf8Bytes(value, new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        });
        
        // Store the serialized byte array in the session with the specified key.
        session.Set(key, serializedValue);
    }

    /// <summary>
    /// Extension method to retrieve an object from the session by deserializing the byte array stored under the specified key.
    /// </summary>
    /// <param name="session">The session from which to retrieve the object.</param>
    /// <param name="key">The key under which the object is stored in the session.</param>
    /// <typeparam name="T">The type of object to be retrieved from the session.</typeparam>
    /// <returns>The deserialized object of type T, or the default value if the key does not exist or deserialization fails.</returns>
    public static T? GetObject<T>(this ISession session, string key)
    {
        // Return default value if key is null.
        if (key == null)
        {
            return default;
        }

        // Retrieve the byte array from the session using the key.
        byte[]? serializedValue = session.Get(key);

        // If the value is null, return the default value for the specified type.
        return serializedValue == null ? default : JsonSerializer.Deserialize<T>(serializedValue, new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        });
    }
}
