using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.CalcEng.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.CalcEng.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalcEngController : ControllerBase
    {
        private ICalcEngService _calcEngService;

        public CalcEngController(ICalcEngService calcEngService)
        {
            _calcEngService = calcEngService;
        }

        [HttpPost("sum")]
        public IActionResult Sum(MultiNumbersOperationRequest request)
        {
            var response = _calcEngService.Sum(request);
            return Ok(response);
        }
    }
}