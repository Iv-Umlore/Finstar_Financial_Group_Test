using System.Text.RegularExpressions;
using System.Text;

namespace Common.Functions
{
    public static class Helper
    {
        /// <summary>
        /// Парсит строку {"1": "value1"}, {"5": "value2"}, { "10": "value32"} на составляющие
        /// </summary>
        /// <param name="startString"></param>
        /// <returns> Список строк вида "{"1": "value1"}" , "{"5": "value2"}" </returns>
        public static List<string> SplitStringByRegex(string startString, string splitRegex)
        {
            string[] res = Regex.Split(startString, splitRegex);
            List<string> result = new List<string>();

            StringBuilder tmpBuilder = new StringBuilder();

            foreach (string s in res)
            {
                tmpBuilder.Append(s);
                if (s == "}")
                {
                    result.Add(tmpBuilder.ToString());
                    tmpBuilder.Clear();
                }
            }

            result.Add(tmpBuilder.ToString());

            return result;
        }
    }
}
