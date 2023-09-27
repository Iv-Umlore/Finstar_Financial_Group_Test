namespace Common.Functions
{
    public static class Helper
    {
        /// <summary>
        /// Достаёт код + значение из строки вида "{\"key\": \"value\"}"
        /// </summary>
        /// <param name="jsonPart"></param>
        /// <returns></returns>
        public static (string Code, string Value) GetCodeValue_FromBadPartOfJson(string jsonPart)
        {
            // Валидация проходит выше
            string[] parts = jsonPart.Split('\"');

            return (parts[1], parts[3]);
        }
    }
}
