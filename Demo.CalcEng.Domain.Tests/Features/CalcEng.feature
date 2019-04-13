Feature: CalcEng
	Test the Calculation Engine

Scenario: Add two numbers
	Given property FirstNumber equals to "10"
	Given property SecondNumber equals to the number 20
	When I execute the Sum operation
	Then property OperationResult should be the number 30