using Demo.CalcEng.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.CalcEng.Api.Controllers
{
    [Route("api/CalcEng")]
    [ApiController]
    public class CalcEngController : CalcEngApiController<CalcEngController>
    {
        private readonly ICalcEngService _calcEngService;

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
            var response = OperationResponse.CreateSucceed($"deleted item {id}");
            return Ok(response);
        }

        [HttpPut("PutTest/{id}")]
        public IActionResult PutTest(int id, DomainItem item)
        {
            var response = OperationResponse.CreateSucceed($"Item: {id}, new PropA: {item.PropA}");
            return Ok(response);
        }

        [HttpPatch("PatchTest/{id}")]
        public IActionResult PatchTest(int id, DomainItem item)
        {
            var response = OperationResponse.CreateSucceed($"Item: {id}, updated PropA: {item.PropA}");
            return Ok(response);
        }

        [HttpGet("GetAList")]
        public IActionResult GetAList()
        {
            var data = new List<ListItem>
            { 
                new ListItem { Id = 1, Name="name-1", Description="desc-1" },
                new ListItem { Id = 2, Name="name-2", Description="desc-2" },
                new ListItem { Id = 3, Name="name-3", Description="desc-3" },
            };

            var response = OperationResponse.CreateSucceed(data);
            return Ok(response);
        }

        [HttpGet("GetAListRoot")]
        public IActionResult GetAListContent()
        {
            var data = new List<ListItem>
            {
                new ListItem { Id = 1, Name="name-1", Description="desc-1" },
                new ListItem { Id = 2, Name="name-2", Description="desc-2" },
                new ListItem { Id = 3, Name="name-3", Description="desc-3" },
            };

            return Ok(data);
        }

        [HttpPost("EchoList")]
        public IActionResult EchoList(IEnumerable<ListItem> list)
        {
            var response = OperationResponse.CreateSucceed(list);
            return Ok(response);
        }

        [HttpPost("EchoListWithoutNulls")]
        public IActionResult EchoListWithoutNulls(IEnumerable<ListItem> list)
        {
            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            
            var response = OperationResponse.CreateSucceed(list);
            return new JsonResult(response, serializerOptions);
        }

        [HttpPost("Command")]
        public IActionResult Command()
        {
            return Accepted();
        }

        [HttpPost("CommandWithRawResponse/{body}")]
        public IActionResult Command(string body)
        {
            return Ok(body);
        }

        [HttpPost("ReturnBoolean/{value}")]
        public IActionResult ReturnBoolean(bool value)
        {
            return Ok(value);
        }

        [HttpPost("ReturnNumber/{value}")]
        public IActionResult ReturnNumber(int value)
        {
            return Ok(value);
        }

        [HttpPost("ReturnDateTime/{value}")]
        public IActionResult ReturnDateTime(string value)
        {
            return Ok(DateTime.Parse(value));
        }

        [HttpPost("ReturnBytes/{value}")]
        public IActionResult ReturnBytes(string value)
        {
            return Ok(Encoding.UTF8.GetBytes(value));
        }

        [HttpPost("ReturnNull")]
        public IActionResult ReturnNull()
        {
            return Ok(null);
        }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}