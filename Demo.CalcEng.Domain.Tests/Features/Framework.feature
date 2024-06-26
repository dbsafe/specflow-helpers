Feature: FrameworkTest
	Framework tests.

Scenario: Null check in an array - 01 - Provide friendly message when actual DateTime field is null
	When returned content is the array '[ { "PropA": 1, "PropB": "2000-01-01" }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:DateTime |
		| 1         | 2000-01-01     |
		| 2         | 2000-01-02     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <1/2/2000 12:00:00 AM>, Actual is null'

Scenario: Null check in an array - 02 - Provide friendly message when actual Decimal field is null
	When returned content is the array '[ { "PropA": 1, "PropB": 20.01 }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Number |
		| 1         | 20.01        |
		| 2         | 20.02        |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <20.02>, Actual is null'

Scenario: Null check in an array - 03 - Provide friendly message when actual Boolean field is null
	When returned content is the array '[ { "PropA": 1, "PropB": true }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Boolean |
		| 1         | true          |
		| 2         | false         |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <False>, Actual: <null>'

Scenario: Null check in an array - 04 - Provide friendly message when actual Guid field is null
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-000000000001" }, { "PropA": 2, "PropB": null } ]'

	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Guid                           |
		| 2         | 00000000-0000-0000-0000-000000000002 |
		| 1         | 00000000-0000-0000-0000-000000000001 |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <00000000-0000-0000-0000-000000000002>, Actual is null'


Scenario: Guid fields in an array - 01 - Expected values should pass
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Guid                           |
		| 1         | 00000000-0000-0000-0000-00000000000a |
		| 2         | 00000000-0000-0000-0000-00000000000B |

Scenario: Guid fields in an array - 02 - Unexpected values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Guid                           |
		| 1         | 00000000-0000-0000-0000-000000000001 |
		| 2         | 00000000-0000-0000-0000-00000000000b |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <00000000-0000-0000-0000-000000000001>, Actual: <00000000-0000-0000-0000-00000000000a>'

Scenario: Guid fields in an array - 03 - Invalid expected values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Guid                           |
		| 1         | 00000000-0000-0000-0000-00000000000- |
		| 2         | 00000000-0000-0000-0000-00000000000b |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <00000000-0000-0000-0000-00000000000-> is not a valid Guid'

Scenario: Guid fields in an array - 04 - Invalid actual values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000-" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA:key | PropB:Guid                           |
		| 1         | 00000000-0000-0000-0000-00000000000a |
		| 2         | 00000000-0000-0000-0000-00000000000b |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Actual <00000000-0000-0000-0000-00000000000-> is not a valid Guid'

Scenario: Guid fields in an array - 05 - Key Guid - Expected values should pass
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA | PropB:Key:Guid                       |
		| 1     | 00000000-0000-0000-0000-00000000000a |
		| 2     | 00000000-0000-0000-0000-00000000000B |

Scenario: Guid fields in an array - 06 - Key Guid - Unexpected values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA | PropB:Key:Guid                       |
		| 1     | 00000000-0000-0000-0000-000000000001 |
		| 2     | 00000000-0000-0000-0000-00000000000b |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Expected row not found in actual'

Scenario: Guid fields in an array - 07 - Key Guid - Invalid expected values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA | PropB:Key:Guid                       |
		| 1     | 00000000-0000-0000-0000-00000000000a |
		| 2     | 00000000-0000-0000-0000-00000000000- |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <00000000-0000-0000-0000-00000000000-> is not a valid Guid'

Scenario: Guid fields in an array - 08 - Compose Key Guid - Expected values should pass
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "C" }, { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "D" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Key:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1                | A     |
		| 00000000-0000-0000-0000-00000000000A | 2                | B     |
		| 00000000-0000-0000-0000-00000000000a | 3                | C     |
		| 00000000-0000-0000-0000-00000000000b | 1                | D     |

Scenario: Guid fields in an array - 09 - Compose Key Guid - Unexpected values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "C" }, { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "D" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Key:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1                | A     |
		| 00000000-0000-0000-0000-00000000000A | 2                | B     |
		| 00000000-0000-0000-0000-00000000000d | 3                | C     |
		| 00000000-0000-0000-0000-00000000000b | 1                | D     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 3.'
	And should throw exception of type AssertFailedException with message containing 'Expected row not found in actual'

Scenario: Guid fields in an array - 10 - Compose Key Guid - Invalid expected values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "C" }, { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "D" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Key:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1                | A     |
		| 00000000-0000-0000-0000-00000000000- | 2                | B     |
		| 00000000-0000-0000-0000-00000000000d | 3                | C     |
		| 00000000-0000-0000-0000-00000000000b | 1                | D     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Property: PropB. Expected <00000000-0000-0000-0000-00000000000-> is not a valid Guid'

Scenario: Guid fields in an array - 11 - Compose Key Guid - Invalid actual values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000-", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "C" }, { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "D" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Key:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1                | A     |
		| 00000000-0000-0000-0000-00000000000a | 2                | B     |
		| 00000000-0000-0000-0000-00000000000a | 3                | C     |
		| 00000000-0000-0000-0000-00000000000b | 1                | D     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Expected row not found in actual'

Scenario: DateTime fields in an array - 01 - Key DateTime - Expected values should pass
	When returned content is the array '[ { "PropA": 1, "PropB": "2020-01-02 10:20:30" }, { "PropA": 2, "PropB": "2020-01-02 10:20:31" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA | PropB:Key:DateTime  |
		| 1     | 2020-01-02 10:20:30 |
		| 2     | 2020-01-02 10:20:31 |

Scenario: DateTime fields in an array - 02 - Key DateTime - Unexpected values should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "2020-01-02 10:20:30" }, { "PropA": 2, "PropB": "2020-01-02 10:20:31" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA | PropB:Key:DateTime  |
		| 1     | 2020-01-02 10:20:32 |
		| 2     | 2020-01-02 10:20:31 |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Expected row not found in actual'

Scenario: DateTime fields in an array - 03 - Key DateTime - Invalid expected value should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "2020-01-02 10:20:30" }, { "PropA": 2, "PropB": "2020-01-02 10:20:31" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA | PropB:Key:DateTime  |
		| 1     | 2020-01-02 10:20:30 |
		| 2     | 2020-01-02 10:20:-- |
	
	And should throw exception of type CollectionException with message containing 'Property: PropB. Expected <2020-01-02 10:20:--> is not a valid DateTime'

Scenario: DateTime fields in an array - 04 - Compose Key DateTime - Expected values should pass
	When returned content is the array '[ { "PropA": 1, "PropB": "2020-01-02 10:20:30", "PropC": 2 }, { "PropA": 2, "PropB": "2020-01-02 10:20:31", "PropC": 3 }, { "PropA": 2, "PropB": "2020-01-02 10:20:31", "PropC": 1 } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropA | PropB:Key:DateTime  | PropC:Key |
		| 1     | 2020-01-02 10:20:30 | 2         |
		| 2     | 2020-01-02 10:20:31 | 3         |
		| 2     | 2020-01-02 10:20:31 | 1         |

Scenario: Fields in an array - 01 - Without Key - Should pass
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000c", "PropC": "C" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Guid                           | PropA:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1            | A     |
		| 00000000-0000-0000-0000-00000000000b | 2            | B     |
		| 00000000-0000-0000-0000-00000000000c | 3            | C     |

Scenario: Fields in an array - 02 - Single Key - Should pass
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000c", "PropC": "C" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1            | A     |
		| 00000000-0000-0000-0000-00000000000b | 2            | B     |
		| 00000000-0000-0000-0000-00000000000c | 3            | C     |

Scenario: Fields in an array - 03 - Single Key - Key found multiple times in actual should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000c", "PropC": "C" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1            | A     |
		| 00000000-0000-0000-0000-00000000000b | 2            | B     |
		| 00000000-0000-0000-0000-00000000000c | 3            | C     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Expected row key found 2 times in actual'

Scenario: Fields in an array - 04 - Compose Key - Key found multiple times in actual should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000c", "PropC": "C" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Key:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1                | A     |
		| 00000000-0000-0000-0000-00000000000b | 2                | B     |
		| 00000000-0000-0000-0000-00000000000c | 3                | C     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 1.'
	And should throw exception of type AssertFailedException with message containing 'Expected row key found 2 times in actual'

Scenario: Fields in an array - 05 - Single Key - Duplicated expected key should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000c", "PropC": "C" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1            | A     |
		| 00000000-0000-0000-0000-00000000000a | 2            | B     |
		| 00000000-0000-0000-0000-00000000000c | 3            | C     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Duplicated Key PropB: 00000000-0000-0000-0000-00000000000a'

Scenario: Fields in an array - 06 - Compose Key - Duplicated expected key should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropA": 2, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "C" }, { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "D" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Key:Guid                       | PropA:Key:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1                | A     |
		| 00000000-0000-0000-0000-00000000000a | 1                | B     |
		| 00000000-0000-0000-0000-00000000000d | 3                | C     |
		| 00000000-0000-0000-0000-00000000000b | 1                | D     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Duplicated Key PropB: 00000000-0000-0000-0000-00000000000a, PropA: 1'

Scenario: Fields in an array - 07 - Without Key - Property not found should fail
	When returned content is the array '[ { "PropA": 1, "PropB": "00000000-0000-0000-0000-00000000000a", "PropC": "A" }, { "PropAA": 2, "PropB": "00000000-0000-0000-0000-00000000000b", "PropC": "B" }, { "PropA": 3, "PropB": "00000000-0000-0000-0000-00000000000c", "PropC": "C" } ]'
	
	Then jpath '$' should be the complex-element array (framework-test)
		| PropB:Guid                           | PropA:Number | PropC |
		| 00000000-0000-0000-0000-00000000000a | 1            | A     |
		| 00000000-0000-0000-0000-00000000000b | 2            | B     |
		| 00000000-0000-0000-0000-00000000000c | 3            | C     |
	And should throw exception of type AssertFailedException with message containing 'Comparing rows at position 2.'
	And should throw exception of type AssertFailedException with message containing 'Property PropA not found'
