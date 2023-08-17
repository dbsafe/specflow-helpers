Feature: CalcEngApi
	Test the Calculation Engine Api

Scenario: Request PI - Operation succeeds

	# Sending a GET request
	When I send a GET request to api/CalcEng/Pi

	# Validating properties from the response
	Then property operationResult should be the number 3.14
	And property succeed should be true
	And property error should be null

	# A missing property is treated as null (For webapi only)
	And property missingProp should be null

	# Validate headers
	And header Server should be 'Kestrel'
	And header test-header should be 'value-2'

	# Validating properties form the http response
	And StatusCode should be 200
	And ReasonPhrase should be 'OK'


Scenario: Request PI - Setting a HTTP Header

	# Setting a header
	Given header CorrelationId equals to '1111-aaaa'
	When I send a GET request to api/CalcEng/Pi
	Then header CorrelationId should be '1111-aaaa'

Scenario: Add two numbers - Operation fails when expected request is not present
	When I send a POST request to api/CalcEng/Sum
	Then StatusCode should be 415
	And ReasonPhrase should be 'Unsupported Media Type'


Scenario: Add two numbers - Operation succeeds
	Given property FirstNumber equals to '10'
	And property SecondNumber equals to the number 20
	When I send a POST request to api/CalcEng/Sum
	Then property operationResult should be the number 30


Scenario: Calling Delete method
	When I send a DELETE request to api/CalcEng/DeleteTest/10
	Then property operationResult should be 'deleted item 10'


Scenario: Calling Put method
	# setting the content
	Given content equals to '{ "PropA": "abc" }'
	When I send a PUT request to api/CalcEng/PutTest/11
	Then property operationResult should be 'Item: 11, new PropA: abc'

Scenario: Calling Patch method
	Given content equals to '{ "PropA": "abc" }'
	When I send a PATCH request to api/CalcEng/PatchTest/11
	Then property operationResult should be 'Item: 11, updated PropA: abc'

Scenario: Validating header and StatusCode when a request fails
	Given header CorrelationId equals to '1111-aaaa'
	When I send a GET request to api/MethodThatFails
	Then header Server should be 'Kestrel'
	And StatusCode should be 404

Scenario: Calculate totals
	Given content is the complex-element array
	| FieldA:Integer | FieldB | FieldC:Number | IsActive:Boolean |
	| 1              | 10     | 100.1         | true             |
	| 2              | 20     | 200.2         | true             |
	| 3              | 30     | 300.3         | true             |
	| 1000           | 2000   | 3000          | false            |
	When I send a POST request to api/CalcEng/CalculateTotals
	Then StatusCode should be 200
	And property operationResult.fieldA should be the number 6
	And property operationResult.fieldB should be the number 60
	And property operationResult.fieldC should be the number 600.6

Scenario: Receive a list
	Given content is the complex-element array
	| id:Integer | name   | description |
	| 1          | name-1 | desc-1      |
	| 2          | name-2 | desc-2      |
	| 3          | name-3 | desc-3      |
	When I send a POST request to api/CalcEng/EchoList
	Then StatusCode should be 200
	And property operationResult should be the complex-element array
	| id:Key:Integer | name   | description |
	| 1              | name-1 | desc-1      |
	| 2              | name-2 | desc-2      |
	| 3              | name-3 | desc-3      |

Scenario: Receive a list with null properties
	Given content is the complex-element array
	| id:Integer | name   | description |
	| 1          | name-1 | desc-1      |
	| 2          | name-2 |             |
	| 3          | name-3 | [NULL]      |
	When I send a POST request to api/CalcEng/EchoList
	Then StatusCode should be 200
	And property operationResult should be the complex-element array
	| id:Key:Integer | name   | description |
	| 1              | name-1 | desc-1      |
	| 2              | name-2 |             |
	| 3              | name-3 | [NULL]      |

Scenario: Receive a list missing null properties
	Given content is the complex-element array
	| id:Integer | name   | description |
	| 1          | name-1 | desc-1      |
	| 2          | name-2 |             |
	| 3          | name-3 | [NULL]      |
	When I send a POST request to api/CalcEng/EchoListWithoutNulls
	Then StatusCode should be 200
	And property operationResult should be the complex-element array
	| id:Key:Integer | name   | description |
	| 1              | name-1 | desc-1      |
	| 2              | name-2 |             |
	| 3              | name-3 | [NULL]      |

Scenario: Receive a response where the content is an array
	When I send a GET request to api/CalcEng/GetAListRoot
	Then StatusCode should be 200
	And content should be the complex-element array
	| id:Key:Integer | name   | description |
	| 1              | name-1 | desc-1      |
	| 2              | name-2 | desc-2      |
	| 3              | name-3 | desc-3      |

Scenario: Receive a response where the content is an array - using jPath
	When I send a GET request to api/CalcEng/GetAListRoot
	Then StatusCode should be 200
	And jpath '$' should be the complex-element array
	| id:Key:Integer | name   | description |
	| 1              | name-1 | desc-1      |
	| 2              | name-2 | desc-2      |
	| 3              | name-3 | desc-3      |
	
	And jpath '$[?(@.id==2)].name' should be 'name-2'

Scenario: Validate a response without a body
	When I send a POST request to api/CalcEng/Command
	Then StatusCode should be 202
	And content header Content-Length should be '0'

Scenario: Validate a response with a raw body
	When I send a POST request to api/CalcEng/CommandWithRawResponse/test-body
	Then StatusCode should be 200
	And content should be 'test-body'

Scenario: Validate a boolean content
	When I send a POST request to api/CalcEng/ReturnBoolean/true
	Then StatusCode should be 200
	And content should be true

	When I send a POST request to api/CalcEng/ReturnBoolean/false
	Then StatusCode should be 200
	And content should be false
	And content should be 'false'

Scenario: Validate a Numeric content 
	When I send a POST request to api/CalcEng/ReturnNumber/100
	Then StatusCode should be 200
	And content should be the number 100

Scenario: Validate a DateTime content 
	When I send a POST request to api/CalcEng/ReturnDateTime/2020-12-10
	Then StatusCode should be 200
	And content should be the datetime '12/10/2020'

# This is just to see the response when an action returns an array
Scenario: Validate a content with bytes
	When I send a POST request to api/CalcEng/ReturnBytes/ABC
	Then StatusCode should be 200

Scenario: Validate a content with null
	When I send a POST request to api/CalcEng/ReturnNull
	Then StatusCode should be 204
	And content header Content-Length should be '0'