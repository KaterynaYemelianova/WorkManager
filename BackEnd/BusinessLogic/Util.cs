namespace BusinessLogic
{
    public static class Util
    {
        public static string Capitalize(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length == 1)
                return str.ToUpper();

            return str[0].ToString().ToUpper() + str.Substring(1).ToLower();
        }
    }
}
