using System.Text.Json;

namespace Orbit.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            byte[] serializedValue = JsonSerializer.SerializeToUtf8Bytes(value);
            session.Set(key, serializedValue);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            if (key == null)
            {
                return default;
            }

            byte[]? serializedValue = session.Get(key);


            return serializedValue == null ? default : JsonSerializer.Deserialize<T>(serializedValue);
        }
    }
}