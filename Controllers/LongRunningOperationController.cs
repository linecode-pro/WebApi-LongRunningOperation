using Microsoft.AspNetCore.Mvc;
using MyWebApi.Services;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class LongRunningOperationController : ControllerBase
    {
        private readonly ILongRunningOperationService _operationService;

        public LongRunningOperationController(ILongRunningOperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpPost("start")]
        public IActionResult StartOperation()
        {
            try
            {
                _operationService.StartOperation();
                return Ok(new { message = "Операция успешно запущена." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("stop")]
        public IActionResult StopOperation()
        {
            try
            {
                _operationService.StopOperation();
                return Ok(new { message = "Операция успешно остановлена." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("status")]
        public IActionResult GetOperationStatus()
        {
            var status = _operationService.IsRunning ? "Running" : "Stopped";
            return Ok(new { status });
        }
    }
}
