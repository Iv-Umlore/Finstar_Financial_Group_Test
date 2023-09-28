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
            if (!ValidationInputString(inputString))
                throw new ArgumentException("Не подходит под формат строки {... : ... } - " + inputString);
            // Валидация проходит выше
            string[] parts = inputString.Split('\"');

            Code = parts[1];
            Value = parts[3];
        }

        private bool ValidationInputString(string input)
        {
            int validator = 0;
            int doublePointValidator = 0;

            bool validationResult = false;

            for (int i = 0; i < input.Length; i++)
            {
                validationResult = false;

                if (input[i] == '{')
                    validator++;

                if (input[i] == ':')
                    doublePointValidator++;

                if (input[i] == '}')
                {
                    validator--;
                    if (doublePointValidator != 1)
                        throw new ArgumentException("Не обнаружено двоеточие" + $" - Позиция {i}");
                    doublePointValidator = 0;
                    validationResult = true;
                }

                if (validator != 0 && validator != 1)
                    throw new ArgumentException("Неверно расставлены скобки { и }" + $" - Позиция {i}");
            }

            return validationResult;
        }

    }
}
