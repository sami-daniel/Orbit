namespace Orbit.Application.Extensions
{
    internal static class UInt64Extensions
    {
        public static bool ToBoolean(this ulong value)
        {
            return value == 1;
        }
    }
}
