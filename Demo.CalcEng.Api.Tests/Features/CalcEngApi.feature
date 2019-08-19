Feature: CalcEngApi
	Test the Calculation Engine Api

Scenario: Request PI - Operation succeeds

	# Sending a GET request
	When I send a GET request to api/CalcEng/Pi

	# Validating properties from the response
	Then property operationResult should be the number 3.14
	And property succeed should be true
	And property error should be null

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
	Then StatusCode should be 400
	And ReasonPhrase should be 'Bad Request'


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

Scenario: Validating header and StatusCode when a request fails
	Given header CorrelationId equals to '1111-aaaa'
	When I send a GET request to api/MethodThatFails
	Then header Server should be 'Kestrel'
	And StatusCode should be 404
