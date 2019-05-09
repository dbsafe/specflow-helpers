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


Scenario: Add two numbers - Operation fails when expected request is not present
	When I send a POST request to api/CalcEng/Sum
	Then StatusCode should be 400
	And ReasonPhrase should be 'Bad Request'


Scenario: Add two numbers - Operation succeeds
	When I send a POST request to api/CalcEng/Sum
