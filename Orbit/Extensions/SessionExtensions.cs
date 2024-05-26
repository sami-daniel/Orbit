using System.Text.Json;

namespace AutoMenu.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            // Serialize the object to a byte array
            byte[] serializedValue = JsonSerializer.SerializeToUtf8Bytes(value);
            // Store the byte array in the session
            session.Set(key, serializedValue);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            // If the key is null, return the default value for the type
            if (key == null)
                return default;

            // Try to get the byte array associated with the session key
            byte[]? serializedValue = session.Get(key);
            // If the byte array is null, return the default value for the type
            if (serializedValue == null)
                return default;

            // Deserialize the byte array to the specified type and return the object
            return JsonSerializer.Deserialize<T>(serializedValue);
        }
    }
}