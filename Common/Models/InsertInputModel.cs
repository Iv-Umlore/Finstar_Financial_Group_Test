namespace Common.Models
{
    public class InsertInputModel
    {
        public string Code { get ; set; } = "";

        public string Value { get; set; } = "";

        public InsertInputModel() { }

        /// <summary>
        /// Преобразует строку вида {"1": "value1"} к объекту InsertInputModel
        /// Соблюдая валидацию
        /// </summary>
        /// <param name="inputString"> Строка вида {"1": "value1"} </param>
        public InsertInputModel(string inputString) {

            // Попробовать Regex, на данный момент база
            if (!RegexModel.techReqRegex.IsMatch(inputString))
                throw new ArgumentException("Не подходит под формат строки {... : ... } - " + inputString);
            // Валидация проходит выше
            string[] parts = inputString.Split('\"');

            Code = parts[1];
            Value = parts[3];
        }
    }
}
