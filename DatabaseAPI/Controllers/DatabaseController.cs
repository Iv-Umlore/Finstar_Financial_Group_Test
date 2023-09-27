using Common.Functions;
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

        // Предполагая сложную сериализацию
        // Нет такой модели с структуры данных, прошу прощения, не нашел
        // "[ {"1": "value1"}, {"5": "value2"}, { "10": "value32"} ]"
        [HttpPost("/InsertNewData_Json")]
        public async Task<IActionResult> InsertNewData([FromQuery] string jsonValues)
        {
            try
            {
                if (jsonValues[0] != '[' || jsonValues[jsonValues.Length-1] != ']')
                    throw new ArgumentException("Неверный формат строки - Должна начинаться с [ и заканчиваться ]");

                List<InsertInputModel> models = new List<InsertInputModel>();

                int validator = 0;
                int doublePointValidator = 0;

                int openPos = 0;

                for (int i = 0; i < jsonValues.Length; i++)
                {
                    if (jsonValues[i] == '{')
                    {
                        openPos = i;
                        validator++;
                    }

                    if (jsonValues[i] == ':')
                        doublePointValidator++;

                    if (jsonValues[i] == '}')
                    {
                        validator--;
                        if (doublePointValidator != 1)
                            throw new ArgumentException("Не обнаружено двоеточие" + $" - Позиция {i}");

                        doublePointValidator = 0;

                        models.Add(new InsertInputModel() { Info = Helper.GetCodeValue_FromBadPartOfJson(jsonValues.Substring(openPos, i - openPos + 1)) } );
                    }

                    if (validator != 0 && validator != 1)
                        throw new ArgumentException("Неверно расставлены скобки { и }" + $" - Позиция {i}");
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

        /// <summary>
        /// Считая что при оформлении ТЗ есть ошибка, предположу, что используется словарь
        /// </summary>
        [HttpPost("/InsertNewData_Dictionary")]
        public async Task<IActionResult> InsertNewData([FromBody] Dictionary<string,string> jsonValues)
        {
            try
            {
                var input = jsonValues.Select(it => new InsertInputModel() { Info = (it.Key, it.Value) }).ToList();
                await _dataModelInteraction.ConvertAndSendToDB(input);

                return Ok();
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