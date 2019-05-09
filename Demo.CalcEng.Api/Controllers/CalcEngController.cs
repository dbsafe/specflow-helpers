using Demo.CalcEng.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.CalcEng.Api.Controllers
{
    [Route("api/CalcEng")]
    [ApiController]
    public class CalcEngController : CalcEngApiController<CalcEngController>
    {
        private ICalcEngService _calcEngService;

        public CalcEngController(ILogger<CalcEngController> logger, ICalcEngService calcEngService)
            : base(logger)
        {
            _calcEngService = calcEngService;
        }

        [HttpPost("Sum")]
        public IActionResult Sum(TwoNumbersOperationRequest request)
        {
            return Execute<OperationResponse>(() => _calcEngService.Sum(request));
        }

        [HttpGet("Pi")]
        public IActionResult Pi()
        {
            Response.Headers.Add("test-header", new string[] { "value-1", "value-2" });
            var content = _calcEngService.Pi();
            return Ok(content);
        }
    }
}