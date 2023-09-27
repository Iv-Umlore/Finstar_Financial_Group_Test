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

        //[HttpPost("/InsertNewData_Json")]
        //public async Task<IActionResult> InsertNewData([FromQuery] string jsonValues)
        //{
        //    try
        //    {


        //        var input = jsonValues.Select(it => new InsertInputModel() { Info = (it.V1, it.V2) }).ToList();
        //        await _dataModelInteraction.ConvertAndSendToDB(input);

        //        return Ok();
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        //    }
        //}

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