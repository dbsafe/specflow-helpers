using Demo.CalcEng.Domain;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("CalculateTotals")]
        public IActionResult CalculateTotals([FromBody] Row[] request)
        {
            return Execute<OperationResponse>(() => _calcEngService.CalculateTotals(request));
        }

        [HttpGet("Pi")]
        public IActionResult Pi()
        {
            var correlationId = Request.Headers["CorrelationId"];
            Response.Headers.Add("CorrelationId", correlationId);

            Response.Headers.Add("test-header", new string[] { "value-1", "value-2" });
            
            var content = _calcEngService.Pi();
            return Ok(content);
        }

        [HttpGet("MethodThatFails")]
        public IActionResult MethodWithError()
        {
            return NotFound();
        }

        [HttpDelete("DeleteTest/{id}")]
        public IActionResult Delete(int id)
        {
            var data = OperationResponse.CreateSucceed($"deleted item {id}");
            return Ok(data);
        }

        [HttpPut("PutTest/{id}")]
        public IActionResult PutTest(int id, DomainItem item)
        {
            var data = OperationResponse.CreateSucceed($"Item: {id}, new PropA: {item.PropA}");
            return Ok(data);
        }
    }
}