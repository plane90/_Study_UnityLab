namespace UtilMyExtention
{
    public static class MyExtention
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        public static bool NotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
    }
}