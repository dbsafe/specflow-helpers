using Demo.CalcEng.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.CalcEng.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalcEngController : AppSimController<CalcEngController>
    {
        private ICalcEngService _calcEngService;

        public CalcEngController(ILogger<CalcEngController> logger, ICalcEngService calcEngService)
            : base(logger)
        {
            _calcEngService = calcEngService;
        }

        [HttpPost("sum")]
        public IActionResult Sum(TwoNumbersOperationRequest request)
        {
            var response = _calcEngService.Sum(request);
            return Ok(response);
        }
    }
}