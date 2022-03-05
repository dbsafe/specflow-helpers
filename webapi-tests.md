Testing a WebApi
----------------

Package | Description
-|-
[Helpers.Specflow.Steps.WebApi](https://www.nuget.org/packages/Helpers.Specflow.Steps.WebApi/)| Used for testing a WebApi.

[Helpers.Specflow.Steps.WebApi](https://www.nuget.org/packages/Helpers.Specflow.Steps.WebApi/) supports all `Given` and `Then` steps defined in [Helpers.Specflow.Steps.Object](https://www.nuget.org/packages/Helpers.Specflow.Steps.Object/) for testing classes.

Step definition class
---------------------

Suppose we have the WebApi with the endpoint Sum.

```csharp
[Route("api/CalcEng")]
[ApiController]
public class CalcEngController : ControllerBase
{
    private ICalcEngService _calcEngService;

    [HttpPost("Sum")]
    public IActionResult Sum(TwoNumbersOperationRequest request)
    {
        var data = _calcEngService.Sum(request);
        return Ok(data);
    }
}
```

The step class inherits from `WebApiSpecs`.

```csharp
[Binding]
[Scope(Feature = "CalcEngApi")]
public class CalcEngApiSteps : WebApiSpecs
{
    private static readonly WebApiSpecsConfig _config = new WebApiSpecsConfig { BaseUrl = "http://localhost:5000" };

    public CalcEngApiSteps(TestContext testContext) : base(testContext, _config) { }
}

```

**Inside the feature file**

```
Feature: CalcEngApi
	Test the Calculation Engine Api
	
Scenario: Add two numbers - Operation succeeds
	Given property FirstNumber equals to the number 10
	And property SecondNumber equals to the number 20
	When I send a POST request to api/CalcEng/Sum
	Then property operationResult should be the number 30
	And StatusCode should be 200
```

Setting the content with a json
```
Given content equals to '{ "PropA": "abc" }'
```

Setting the content with an array
```
Given content is the complex-element array
| id:Integer | name   | description |
| 1          | name-1 | desc-1      |
| 2          | name-2 |             |
| 3          | name-3 | [NULL]      |
```

Supported `When` steps
----------------------

Sending a request to a WebApi
```
When I send a POST request to api/CalcEng/Sum
```

Supported methods: `POST`, `GET`, `PUT`, `DELETE`, and `PATCH`
Query parameters can be included in the url


Supported `Then` steps
----------------------

Assert the StatusCode from the response
```
Then StatusCode should be 200
```

Assert the ReasonPhrase from the response
```
Then ReasonPhrase should be 'OK'
```

Assert a header from the response
```
Then header Server should be 'Kestrel'
```