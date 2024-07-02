namespace Orbit.Application.Extensions
{
    public static class StringExtensions
    {
        public static ulong ToLong(this string value)
        {
            return value == "on" ? 1u : 0u;
        }
    }
}
