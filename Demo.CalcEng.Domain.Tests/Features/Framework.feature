Feature: FrameworkTest
	Framework tests.

Scenario: Provide friendly message when actual DateTime field is null
	When returned content is the array '[ { "PropA": 1, "PropB": "2000-01-01" }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:DateTime |
		| 1         | 2000-01-01     |
		| 2         | 2000-01-02     |
	And should throw exception of type AssertFailedException with message containing 'The rows at position 2 are different.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <1/2/2000 12:00:00 AM>, Actual is null'

Scenario: Provide friendly message when actual Decimal field is null
	When returned content is the array '[ { "PropA": 1, "PropB": 20.01 }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Number |
		| 1         | 20.01        |
		| 2         | 20.02        |
	And should throw exception of type AssertFailedException with message containing 'The rows at position 2 are different.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <20.02>, Actual is null'

Scenario: Provide friendly message when actual Boolean field is null
	When returned content is the array '[ { "PropA": 1, "PropB": true }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Boolean |
		| 1         | true          |
		| 2         | false         |
	And should throw exception of type AssertFailedException with message containing 'The rows at position 2 are different.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <False>, Actual: <null>'

Scenario: Provide friendly message when actual Guid field is null
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-000000000001" }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Guid                           |
		| 1         | 00000000-0000-0000-0000-000000000001 |
		| 2         | 00000000-0000-0000-0000-000000000002 |
	And should throw exception of type AssertFailedException with message containing 'The rows at position 2 are different.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <00000000-0000-0000-0000-000000000002>, Actual: <null>'
