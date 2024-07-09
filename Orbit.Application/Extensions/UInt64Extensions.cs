namespace Orbit.Application.Extensions
{
    internal static class UInt64Extensions
    {
        public static bool ToBoolean(this UInt64 value)
        {
            return value == 1;
        }
    }
}
