using DbInteractionService;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("/InsertNewData")]
        public async Task<IActionResult> InsertNewData([FromBody] string jsonValues)
        {


            return Ok();
            
        }

        [HttpGet("/GetCurrentData")]
        public async Task<ActionResult<string>> GetCurrentData([FromQuery] int? codeFilter = null, [FromQuery] string? valueFilter = null)
        {


            return Ok("result");
        }
    }
}