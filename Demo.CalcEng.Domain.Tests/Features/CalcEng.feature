Feature: CalcEng
	Test the Calculation Engine

Scenario: Add two numbers
	Given property FirstNumber equals to "10"
	And property SecondNumber equals to the number 20
	When I execute the Sum operation
	Then property OperationResult should be the number 30
	And property Succeed should be True
	And property Error should be NULL