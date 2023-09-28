using Common.Models;
using DataAccessLayer.Models;
using DbInteractionService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DatabaseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private ILogger<DatabaseController> _logger;
        private IDataModelInteraction _dataModelInteraction;

        public DatabaseController(ILogger<DatabaseController> logger, IDataModelInteraction dataModelInteraction)
        {
            _logger = logger;
            _dataModelInteraction = dataModelInteraction;
        }

        // "[ {"1": "value1"}, {"5": "value2"}, { "10": "value32"} ]"
        /// <summary>
        /// Считая что при оформлении ТЗ есть ошибка, предположу, что используется словарь
        /// </summary>
        [HttpPost("/InsertNewData_Dictionary")]
        public async Task<IActionResult> InsertNewData([FromBody] Dictionary<string,string> jsonValues)
        {
            try
            {
                var input = jsonValues.Select(it => new InsertInputModel() { Code = it.Key, Value = it.Value }).ToList();
                await _dataModelInteraction.ConvertAndSendToDB(input);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Предполагая сложную сериализацию
        // Нет такой модели с структуры данных, прошу прощения, не нашел
        // "[ {"1": "value1"}, {"5": "value2"}, { "10": "value32"} ]"
        [HttpPost("/InsertNewData_Json")]
        public async Task<IActionResult> InsertNewData([FromQuery] string jsonValues)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonValues))
                    throw new ArgumentException("Входящее значение пусто");

                if (jsonValues[0] != '[' || jsonValues[jsonValues.Length - 1] != ']')
                    throw new ArgumentException("Неверный формат строки - Должна начинаться с [ и заканчиваться ]");

                // Regex ???
                string[] separators = { ",\r\n", ",\t", ", ", ", \t" };

                string[] goodStrings = jsonValues.Substring(1, jsonValues.Length-2)
                    .Split(separators, StringSplitOptions.TrimEntries);

                List<InsertInputModel> models = new List<InsertInputModel>();

                foreach (string goodString in goodStrings)
                {
                    models.Add(new InsertInputModel(goodString));
                }

                await _dataModelInteraction.ConvertAndSendToDB(models);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("/GetCurrentData")]
        public async Task<ActionResult<string>> GetCurrentData(
            [FromQuery] int? limit = null, [FromQuery] long? codeFilter = null, [FromQuery] string? valueFilter = "")
        {
            List<ProductInfo> data = await _dataModelInteraction.GetFromDB(limit, codeFilter, valueFilter);

            string result = JsonSerializer.Serialize(data);
            return Ok(result);
        }
    }
}