# specflow-helpers
Library to support writing unit test with Specflow

This library provides base implementation that can be reused when writing the code behind for Specflow tests. 
In order to use the library some knowledge of Specflow is required.
For more information about Specflow visit: https://specflow.org/

Features
--------

specflow-helpers provides methods for setting properties of a request object and methods for reading properties of a response object. 
These methods can be used by defining a Step Definition class that inherits from a class defined in specflow-helpers.

Testing methods of a class
--------------------------

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
        SetResponse(JObject.FromObject(operationResponse));
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

The properties of the request are set using a step definition from `JObjectBuilderSteps`.

```csharp
[Given(@"property ([^\s]+) equals to the number ([-+]?[\d]*[\.]?[\d]+)")]
public void SetRequestProperty(string name, decimal value)
```

The same when validating the response. When reading a property of the response a step definition from `JObjectBuilderSteps` is used.

```csharp
[Then(@"property ([^\s]+) should be the number ([-+]?[\d]*[\.]?[\d]+)")]
public void AssertNumericProperty(string propertyName, decimal expectedPropertyValue)
```

There is not need to define steps to set and read properties in the Step Definition class.

