Feature: CalcEng
	Test the Calculation Engine

Scenario: Add two numbers - Operation succeeds
	# Setting a property as text
	Given property FirstNumber equals to "10" 
	
	# Setting a property as number
	And property SecondNumber equals to the number 20
	
	# Executing an operation
	When I execute the Sum operation
	
	# Assert a numeric property
	Then property OperationResult should be the number 30
	
	# Assert a boolean property
	And property Succeed should be True
	
	# Assert that a property is null
	And property Error should be NULL

Scenario: Divide two numbers - Operation fails
	Given property FirstNumber equals to "10"
	And property SecondNumber equals to "0"
	When I execute the Div operation
	Then property Succeed should be False

	# Assert a text property
	And property Error should be "Attempted to divide by zero."