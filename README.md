specflow-helpers
==================
[![Build status](https://dev.azure.com/dbsafe/dbsafe/_apis/build/status/dbsafe/specflow-helpers-CI)](https://dev.azure.com/dbsafe/dbsafe/_build/latest?definitionId=4)

Library to support writing unit test with Specflow

This library provides a base implementation that can be reused when writing the code behind for Specflow tests. 
In order to use the library some knowledge of Specflow is required.
For more information about Specflow visit: https://specflow.org/

Features
--------

specflow-helpers can be used for writing tests for methods of a class and for writing test for a WebApi.

NuGet packages
-------------
[Helpers.Specflow.Steps.Object](https://www.nuget.org/packages/Helpers.Specflow.Steps.Object/)
defines a base Steps Definition class with `Given` steps that set properties of an object and `Then` steps that assert properties of a result.

[Helpers.Specflow.Steps.WebApi](https://www.nuget.org/packages/Helpers.Specflow.Steps.WebApi/)
defines a base Steps Definition class with `Given` steps that set properties of a `HttpRequest` object and `Then` steps that assert properties of a `HttpResponse` object and its content. The base class has also one `When` step that executes a `HttpRequest` passing a url, method, and query parameters.

Example - Testing methods of a class
------------------------------------

Suppose we have the class CalcEng that has the method Sum.

```csharp
public class CalcEng
{
    public OperationResponse<decimal> Sum(TwoNumbersOperationRequest request)
    {
        var data = request.FirstNumber + request.SecondNumber;
        return OperationResponse.CreateSucceed(data);
    }
    
    // ...
}

public class TwoNumbersOperationRequest
{
    public decimal FirstNumber { get; set; }
    public decimal SecondNumber { get; set; }
}

public class OperationResponse<TData>
{
     public TData OperationResult { get; set; }
}
```

**Step Definition class**

The Step Definition class inherits from `JObjectBuilderSteps` and defines the method `ExecuteAddTwoNumbers`.

```csharp
[Binding]
[Scope(Feature = "CalcEng")]
public class CalcEngSteps : JObjectBuilderSteps
{
    private readonly CalcEng _calcEng = new CalcEng();

    [When(@"I Add two numbers")]
    public void ExecuteAddTwoNumbers()
    {
        var request = Request.ToObject<TwoNumbersOperationRequest>();
        var operationResponse = _calcEng.Sum(request);
        SetResponse(operationResponse);
    }

    // ...
}
```

Notice how the request object that is passed to the `Sum` method is created from the `Request` property.
```csharp
var request = Request.ToObject<TwoNumbersOperationRequest>();
```

The `Request` property defined in the base class `JObjectBuilderSteps` is a `JObject` and represents a json object.

The `SetResponse` method converts the response from the `Sum` method into a `JObject`.
```csharp
SetResponse(operationResponse);
```

**Feature file**
```
Feature: CalcEng
	Test the Calculation Engine

Scenario: Add two numbers - returns correct value
	Given property FirstNumber equals to the number 10
	And property SecondNumber equals to the number 20
	When I Add two numbers
	Then property OperationResult should be the number 30
```

The Step Definition class that supports `CalcEng` tests does not need to define custom steps for setting properties of the request and validating properties of the response.


Example – Testing endpoints of a WebApi
---------------------------------------

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
    
    //...
}
```

**Step Definition class**

```csharp
[Binding]
[Scope(Feature = "CalcEngApi")]
public class CalcEngApiSteps : WebApiSpecs
{
    private static readonly WebApiSpecsConfig _config = new WebApiSpecsConfig { BaseUrl = "http://localhost:5000" };
    public CalcEngApiSteps(TestContext testContext) : base(testContext, _config) { }
}
```

The class `CalcEngApiSteps` descends from `WebApiSpecs` and does not define any step.

**Feature file**

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

Projects in the solution
------------------------

Project Name | Description
------------ | -----------
Specflow.Steps.Object | specflow-helpers implementation for testing classes
Specflow.Steps.Object.Tests | Unit test for Specflow.Steps.Object
Specflow.Steps.WebApi | specflow-helpers implementation for testing WebApi services
Demo.CalcEng.Domain | Defines a service class used for demo
Demo.CalcEng.Domain.Tests | Demonstrates how to use specflow-helpers to write Specflow tests for a class. Uses the release version from NuGet. For debugging remove the reference to the NuGet package and add a reference to the project Specflow.Steps.Object
Demo.CalcEng.Api | A WebApi service used for demo 
Demo.CalcEng.Api.Tests | Demonstrates how to use specflow-helpers to write Specflow tests for a WebApi. Uses the release version from NuGet. For debugging remove the reference to the NuGet packages and add a reference to the projects Specflow.Steps.Object and Specflow.Steps.WebApi

Supported `Given` steps for testing a class or a WebApi service
---------------------------------------------------------------

**Assigning a text value to a property**
```
Given property FirstName equals to 'Maria'
```

This step can be use to set non-text properties. The serialization process that occurs when creating the actual request takes care of converting the text.

Example:
```
Given property FirstNumber equals to '10'
Given property Date equals to '2000-01-01'
Given property IsSmall equals to 'True'
```

**Assigning a numeric value to a property**
```
Given property SecondNumber equals to the number 20
```

**Assigning an empty array to a property**
```
Given property Numbers is an empty array
```

**Assigning an array to a property**
```
Given property Numbers is the array '1,2,3,4,5'
```

**Assigning severl properties in one step**
```
Given properties
| name         | value |
| FirstNumber  | 10    |
| SecondNumber | 0     |
```

Supported `Then` steps for testing a class or a WebApi service
--------------------------------------------------------------

**Assert a text property**
```
Then property Error should be 'Attempted to divide by zero.'
```

**Assert a numeric property**
```
Then property OperationResult should be the number 30
```

**Assert a boolean property**
```
Then property Succeed should be True
```

**Assert a DateTime property**
```
Then property Date should be the datetime '2000-01-02'
```

**Assert that a property is null**
```
Then property Error should be NULL
```

**Assert an array that contains single elements**
```
Then property OperationResult should be the single-element array '2, 3, 5, 7, 11, 13, 17, 19, 23'
```

**Assert an array that contains single elements**
```
Then property OperationResult should be the single-element array
| values |
| 2      |
| 3      |
| 5      |
| 7      |
| 11     |
| 13     |
| 17     |
| 19     |
| 23     |
```

**Assert an array that contains single elements (in one line)**
```
Then property OperationResult should be the single-element array '2, 3, 5, 7, 11, 13, 17, 19, 23'
```

**Assert an array that contains complex elements**
```
Then property OperationResult should be the complex-element array
| PropA:key | PropB    | Date:DateTime | Value:Number | IsSmall:Boolean |
| item1-pa  | item1-pb | 2000-01-01    | 100          | True            |
| item2-pa  | item2-pb | 2000-01-02    | 200          | False           |
```

- Each column represents a property of the elements in the array
- The headers indicates the property names and the data type
  
  Example:
  ```
  Date:DateTime
  ```
  
- When **key** is added to a property the property is used as the key in the array. 
  **key** can be added to more than one property to create a composite key. 
  **key** must be positioned before the property type when the type is specified.
  
  Example:
  ```
  PropA:key
  Value:key:Number
  ```
  
- Use `[NULL]` when a property is expected to be null

  Example:
  ```
  Then property OperationResult should be the complex-element array
  | PropA:key | PropB    | Date:DateTime | Value:Number | IsSmall:Boolean |
  | item1-pa  | [NULL]   | 2000-01-01    | 100          | True            |
  | item2-pa  | item2-pb | 2000-01-02    | 200          | False           |
  ```
  
  - Use `[IGNORE]` to skip the validation of a property
  
  Example:
  ```
  Then property OperationResult should be the complex-element array
  | PropA:key | PropB    | Date:DateTime | Value:Number | IsSmall:Boolean |
  | item1-pa  | [IGNORE] | 2000-01-01    | 100          | True            |
  | item2-pa  | item2-pb | 2000-01-02    | 200          | False           |
  ```
  
  
 **Retrieving a property using JPath**
  ```
 Then property OperationResult[1].Date should be the datetime '2000-01-02'
 Then property OperationResult[1].PropB should be 'item2-pb'
 ```
 
Supported `When` step for testing a WebApi service
--------------------------------------------------

**Sending a request to a WebApi**
```
When I send a POST request to api/CalcEng/Sum
```

Supported methods: `POST`, `GET`, `PUT`, `DELETE`
Query parameters can be included in the url


Supported `Then` step for testing a WebApi service
--------------------------------------------------

**Assert the StatusCode from the response**
```
Then StatusCode should be 200
```

**Assert the ReasonPhrase from the response**
```
Then ReasonPhrase should be 'OK'
```

**Assert a header from the response**
```
Then header Server should be 'Kestrel'
```

