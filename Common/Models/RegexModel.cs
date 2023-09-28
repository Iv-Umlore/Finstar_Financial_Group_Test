using System.Text.RegularExpressions;

namespace Common.Models
{
    public static class RegexModel
    {
        public static Regex techReqRegex = new Regex("^{\\s?\"\\d*\"\\s?\\t?:\\s?\\t?\".*\"\\s?}\\s?\\t?$");

        public static string splitReq = "\\s?\\t?(})\\s?\\t?,\\s?\\t?({\\s?\\t?)";
    }
}
