Testing classes
---------------

Package | Description
-|-
[Helpers.Specflow.Steps.Object](https://www.nuget.org/packages/Helpers.Specflow.Steps.Object/)| Used for testing properties of an object returned by a method.

Step definition class
---------------------

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

The Step Definition class inherits from `JObjectBuilderSteps` and defines the method `ExecuteAddTwoNumbers`.

```csharp
[Binding]
[Scope(Feature = "CalcEng")]
public class CalcEngSteps : JObjectBuilderSteps
{
    private readonly CalcEng _calcEng = new CalcEng();

    [When(@"I add two numbers")]
    public void ExecuteAddTwoNumbers()
    {
        var request = Request.ToObject<TwoNumbersOperationRequest>();
        var operationResponse = _calcEng.Sum(request);
        SetResponse(operationResponse);
    }
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

**Inside the feature file**

```
Scenario: Add two numbers - returns correct value
	Given property FirstNumber equals to the number 10
	And property SecondNumber equals to the number 20
	When I add two numbers
	Then property OperationResult should be the number 30
```

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

**Assigning an array with complex objects to a property**
```
Given property Items is the complex-element array
| PropA | PropB |
| 10    | 100   |
| 11    | 110   |
| 22    | 220   |
```

**Assigning several properties in one step**
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

**Assert that a numeric property is in a range**
```
Then property OperationResult should be a number between 3.1 and 3.2
```

**Assert a boolean property**
```
Then property Succeed should be True
```

**Assert a DateTime property**
```
Then property Date should be the datetime '2000-01-02'
```

**Assert that a DateTime property is in a range**
```
Then property Date should be a datetime between '2000-01-01' and '2000-01-03'
```

**Assert that a property is null**
```
Then property Error should be NULL
```

**Assert an array that contains single elements (in-line)**
```
Then property OperationResult should be the single-element array '2, 3, 5, 7, 11, 13, 17, 19, 23'
```

**Assert an array that contains single elements (using a table)**
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

- When `key` is added to a property, the property is used as the key in the array.
  `key` can be added to more than one property to create a composite key.
  `key` must be positioned before the property type when the type is specified.

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
