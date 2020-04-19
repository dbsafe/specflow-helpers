﻿Feature: CalcEng
	Test the Calculation Engine

Scenario: Add two numbers - Operation succeeds
	# Setting a property as text -
	Given property FirstNumber equals to '10'
	
	# Setting a property as number -
	And property SecondNumber equals to the number 20
	
	# Executing an operation
	When I Add two numbers
	
	# Assert a numeric property
	Then property OperationResult should be the number 30
	
	# Assert a boolean property -
	And property Succeed should be True
	
	# Assert that a property is null -
	And property Error should be NULL


Scenario: Add several numbers - Passing a list - Passing an empty array
	# Setting a property as an empty array -
	Given property Numbers is an empty array
	When I Add several numbers
	Then property Succeed should be False
	And property Error should be 'The list is empty'


Scenario: Add several numbers - Passing a list
	# Setting a property as an array -
	Given property Numbers is the array '1,2,3,4,5'
	When I Add several numbers
	Then property OperationResult should be the number 15


Scenario: Divide two numbers - Operation fails
	# Setting several properties using a table -
	Given properties
	| name         | value |
	| FirstNumber  | 10    |
	| SecondNumber | 0     |
	When I execute the Div operation
	Then property Succeed should be False

	# Assert a text property
	And property Error should be 'Attempted to divide by zero.'


Scenario: Request Prime Numbers
	When I request prime numbers

	# Assert an array with single elements -
	Then property OperationResult should be the single-element array '2, 3, 5, 7, 11, 13, 17, 19, 23'


Scenario: Request Prime Numbers - Using a table
	When I request prime numbers

	# Assert an array with single elements using a table -
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
	

Scenario: Request Domain Items
	When I request domain items

	# Assert an array with complex elements -
	Then property OperationResult should be the complex-element array
	# key column(s)
	# data type
	# expected null value
	| PropA:key | PropB    | Date:DateTime | Value:Number | IsSmall:Boolean |
	| item1-pa  | [NULL]   | 2000-01-01    | 100          | True            |
	| item2-pa  | item2-pb | 2000-01-02    | 200          | False           |
	| item3-pa  | item3-pb | 2000-01-03    | 300          | False           |
	| item4-pa  | item4-pb | 2000-01-04    | 400          | False           |


Scenario: Request Domain Items - Ignore field
	When I request domain items
	Then property OperationResult should be the complex-element array
	# Ignore a field in the array -
	| PropA:key | PropB    | Date:DateTime | Value:Number | IsSmall:Boolean |
	| item1-pa  | [IGNORE] | 2000-01-01    | 100          | True            |
	| item2-pa  | [IGNORE] | 2000-01-02    | 200          | False           |
	| item3-pa  | [IGNORE] | 2000-01-03    | 300          | False           |
	| item4-pa  | [IGNORE] | 2000-01-04    | 400          | False           |

	Then property OperationResult should be an array with 4 items


Scenario: Request Domain Items By Date
	# Setting a DateTime property -
	Given property Date equals to '2000-01-01'
	# Setting a Boolean property -
	And property IsSmall equals to 'True'
	
	When I request domain items by date

	Then property OperationResult should be the complex-element array
	| PropA:key | PropB  | Date:DateTime | Value:Number | IsSmall:Boolean |
	| item1-pa  | [NULL] | 2000-01-01    | 100          | True            |

	Then property OperationResult should be an array with 1 item


Scenario: Request Domain Items - Assert DtateTime property
	When I request domain items
	# assert a property using its path -
	# assert a datetime property
	Then property OperationResult[1].Date should be the datetime '2000-01-02'
	# assert a datetime property including the time
	Then property OperationResult[1].Date should be the datetime '2000-01-02 00:00:00'

Scenario: Request Domain Items By Date - Returns empty array
	Given property Date equals to '1900-01-01'
	When I request domain items by date
	# assert an empty array
	Then property OperationResult should be an empty array


Scenario: Calling a method that does not need parameters
	When I request pi
	Then property OperationResult should be the number 3.14

Scenario: Validating that a numeric value is in a range
	When I request pi
	Then property OperationResult should be a number between 3.1 and 3.2

Scenario: Validating that a datetime value is in a range
	When I request domain items
	Then property OperationResult[1].Date should be a datetime between '2000-01-01' and '2000-01-03'