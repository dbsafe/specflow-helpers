# specflow-helpers
Library to support writing unit test with Specflow

This library provides base implementation that can be reused when writing the code behind for Specflow tests. 
In order to use the library some knowledge of Specflow is required.
For more information about Specflow visit: https://specflow.org/

Features
--------

specflow-helpers provides methods for setting properties of a request object and methods for reading properties of a response object. 
These methods can be used by defining a Step Definition class that inherits from a class defined in specflow-helpers.

Demo - Testing methods of a class
---------------------------------

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

The properties of the request are set using a step definition from `JObjectBuilderSteps`.

```csharp
[Given(@"property ([^\s]+) equals to the number ([-+]?[\d]*[\.]?[\d]+)")]
public void SetRequestProperty(string name, decimal value)
```

When reading a property of the response a step definition from `JObjectBuilderSteps` is used.

```csharp
[Then(@"property ([^\s]+) should be the number ([-+]?[\d]*[\.]?[\d]+)")]
public void AssertNumericProperty(string propertyName, decimal expectedPropertyValue)
```

The Step Definition class that supports `CalcEng` tests does not need to define custom steps for setting properties of the request and validating properties of the response.

Projects in the solution
------------------------

Project Name | Description
------------ | -----------
Specflow.Steps.Object | specflow-helpers implementation
Specflow.Steps.Object.Tests | Unit test for specflow-helpers implementation
Demo.CalcEng.Domain | Defines a service class used in the demo
Demo.CalcEng.Domain.Tests | Demonstrates how to use specflow-helpers to write Specflow tests

Supported `Given` steps
-----------------------

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

Supported `Then` steps
----------------------

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
  
  
  
