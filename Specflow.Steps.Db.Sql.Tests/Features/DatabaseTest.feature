Feature: DatabaseTest
	Demonstrates populating and asserting expected data in a SQL Server database

Background:
Given table with identity columns '[dbo].[Supplier]' contains the data
| Id | Name   | ContactName | ContactPhone | ContactEmail |
| 1  | sup-1  | cont-1      | phone-1      | email-1      |
| 2  | sup-2  | cont-2      | phone-2      | email-2      |
Given table with identity columns '[dbo].[Category]' contains the data
| Id | Name  |
| 1  | cat-1 |
| 2  | cat-2 |

Scenario: Populate and validate a table
Given table '[dbo].[Product]' contains the data
| Code   | Name      | Description | Cost   | ListPrice | CategoryId | SupplierId | IsActive | ReleaseDate | CreatedOn  |
| code-1 | product-1 | [NULL]      | 101.10 | 111.10    | 1          | 2          | 1        | 2000-01-01  | 2000-02-01 |
| code-2 | product-2 | desc-2      | 102.10 | 112.10    | 1          | 2          | 1        | 2000-01-02  | 2000-02-02 |
| code-3 | product-3 | desc-3      | 103.10 | 113.10    | 2          | 1          | 1        | 2000-01-03  | 2000-02-03 |
| code-4 | product-4 |             | 104.10 | 114.10    | 2          | 1          | 0        | 2000-01-04  | 2000-02-04 |
| code-5 | product-5 | [NULL]      | 105.10 | 115.10    | 2          | 1          | 0        | 2000-01-05  | 2000-02-05 |
When I execute my operation
Then table '[dbo].[Product]' should contain the data
| Id:Key | Code   | Name      | Description | Cost:Number | ListPrice:Number | CategoryId | SupplierId | IsActive:Boolean | ReleaseDate:DateTime | CreatedOn:DateTime |
| 101    | code-1 | product-1 | [NULL]      | 101.10      | 111.10           | 1          | 2          | true             | 2000-01-01           | 2000-02-01         |
| 102    | code-2 | product-2 | desc-2      | 102.10      | 112.10           | 1          | 2          | true             | 2000-01-02           | 2000-02-02         |
| 103    | code-3 | product-3 | desc-3      | 103.10      | 113.10           | 2          | 1          | true             | 2000-01-03           | 2000-02-03         |
| 104    | code-4 | product-4 |             | 104.10      | 114.10           | 2          | 1          | false            | 2000-01-04           | 2000-02-04         |
| 105    | code-5 | product-5 | [NULL]      | 105.10      | 115.10           | 2          | 1          | false            | 2000-01-05           | 2000-02-05         |

Scenario: Validate a table using a query
Given table '[dbo].[Product]' contains the data
| Code   | Name      | Description | Cost   | ListPrice | CategoryId | SupplierId | IsActive | ReleaseDate | CreatedOn  |
| code-1 | product-1 | [NULL]      | 101.10 | 111.10    | 1          | 2          | 1        | 2000-01-01  | 2000-02-01 |
| code-2 | product-2 | desc-2      | 102.10 | 112.10    | 1          | 2          | 1        | 2000-01-02  | 2000-02-02 |
| code-3 | product-3 | desc-3      | 103.10 | 113.10    | 2          | 1          | 1        | 2000-01-03  | 2000-02-03 |
| code-4 | product-4 |             | 104.10 | 114.10    | 2          | 1          | 0        | 2000-01-04  | 2000-02-04 |
| code-5 | product-5 | [NULL]      | 105.10 | 115.10    | 2          | 1          | 0        | 2000-01-05  | 2000-02-05 |
When I execute my operation
Then query '(SELECT * FROM [dbo].[Product]) AS prod' should return the data
| Id:Key | Code   | Name      | Description | Cost:Number | ListPrice:Number | CategoryId | SupplierId | IsActive:Boolean | ReleaseDate:DateTime | CreatedOn:DateTime |
| 101    | code-1 | product-1 | [NULL]      | 101.10      | 111.10           | 1          | 2          | true             | 2000-01-01           | 2000-02-01         |
| 102    | code-2 | product-2 | desc-2      | 102.10      | 112.10           | 1          | 2          | true             | 2000-01-02           | 2000-02-02         |
| 103    | code-3 | product-3 | desc-3      | 103.10      | 113.10           | 2          | 1          | true             | 2000-01-03           | 2000-02-03         |
| 104    | code-4 | product-4 |             | 104.10      | 114.10           | 2          | 1          | false            | 2000-01-04           | 2000-02-04         |
| 105    | code-5 | product-5 | [NULL]      | 105.10      | 115.10           | 2          | 1          | false            | 2000-01-05           | 2000-02-05         |

Scenario: Populate and validate a table with identity column
Given table with identity columns '[dbo].[Product]' contains the data
| Id | Code   | Name      | Description | Cost   | ListPrice | CategoryId | SupplierId | IsActive | ReleaseDate | CreatedOn  |
| 1  | code-1 | product-1 | desc-1      | 101.10 | 111.10    | 1          | 2          | 1        | 2000-01-01  | 2000-02-01 |
| 2  | code-2 | product-2 | desc-2      | 102.10 | 112.10    | 1          | 2          | 1        | 2000-01-02  | 2000-02-02 |
| 3  | code-3 | product-3 | desc-3      | 103.10 | 113.10    | 2          | 1          | 1        | 2000-01-03  | 2000-02-03 |
| 4  | code-4 | product-4 |             | 104.10 | 114.10    | 2          | 1          | 0        | 2000-01-04  | 2000-02-04 |
| 5  | code-5 | product-5 | [NULL]      | 105.10 | 115.10    | 2          | 1          | 0        | 2000-01-05  | 2000-02-05 |
When I execute my operation
Then table '[dbo].[Product]' should contain the data
| Id:Key | Code   | Name      | Description | Cost:Number | ListPrice:Number | CategoryId | SupplierId | IsActive:Boolean | ReleaseDate:DateTime |
| 1      | code-1 | product-1 | desc-1      | 101.10      | 111.10           | 1          | 2          | true             | 2000-01-01           |
| 2      | code-2 | product-2 | desc-2      | 102.10      | 112.10           | 1          | 2          | true             | 2000-01-02           |
| 3      | code-3 | product-3 | desc-3      | 103.10      | 113.10           | 2          | 1          | true             | 2000-01-03           |
| 4      | code-4 | product-4 |             | 104.10      | 114.10           | 2          | 1          | false            | 2000-01-04           |
| 5      | code-5 | product-5 | [NULL]      | 105.10      | 115.10           | 2          | 1          | false            | 2000-01-05           |

Scenario: Filter and validate a table - one field, one value
Given table with identity columns '[dbo].[Product]' contains the data
| Id | Code   | Name      | Description | Cost   | ListPrice | CategoryId | SupplierId | IsActive | ReleaseDate | CreatedOn  |
| 1  | code-1 | product-1 | desc-1      | 101.10 | 111.10    | 1          | 2          | 1        | 2000-01-01  | 2000-02-01 |
| 2  | code-2 | product-2 | desc-2      | 102.10 | 112.10    | 1          | 2          | 1        | 2000-01-02  | 2000-02-02 |
| 3  | code-3 | product-3 | desc-3      | 103.10 | 113.10    | 2          | 1          | 1        | 2000-01-03  | 2000-02-03 |
| 4  | code-4 | product-4 |             | 104.10 | 114.10    | 2          | 1          | 0        | 2000-01-04  | 2000-02-04 |
| 5  | code-5 | product-5 | [NULL]      | 105.10 | 115.10    | 2          | 1          | 0        | 2000-01-05  | 2000-02-05 |
Given I filter table '[dbo].[Product]' by
| FieldName | FieldValues |
| Id        | 2           |
Then table '[dbo].[Product]' should contain the data
| Id:Key | Code   | Name      | Description | Cost:Number | ListPrice:Number | CategoryId | SupplierId | IsActive:Boolean | ReleaseDate:DateTime |
| 2      | code-2 | product-2 | desc-2      | 102.10      | 112.10           | 1          | 2          | true             | 2000-01-02           |

Scenario: Filter and validate a table - multiple fields and values
Given table with identity columns '[dbo].[Product]' contains the data
| Id | Code   | Name      | Description | Cost   | ListPrice | CategoryId | SupplierId | IsActive | ReleaseDate | CreatedOn  |
| 1  | code-1 | product-1 | desc-1      | 101.10 | 111.10    | 1          | 2          | 1        | 2000-01-01  | 2000-02-01 |
| 2  | code-2 | product-2 | desc-2      | 102.10 | 112.10    | 1          | 2          | 1        | 2000-01-02  | 2000-02-02 |
| 3  | code-3 | product-3 | desc-3      | 103.10 | 113.10    | 2          | 1          | 1        | 2000-01-03  | 2000-02-03 |
| 4  | code-4 | product-4 |             | 104.10 | 114.10    | 2          | 1          | 0        | 2000-01-04  | 2000-02-04 |
| 5  | code-5 | product-5 | [NULL]      | 105.10 | 115.10    | 2          | 1          | 0        | 2000-01-05  | 2000-02-05 |
Given I filter table '[dbo].[Product]' by
| FieldName | FieldValues |
| Id        | 1,2,5       |
| IsActive  | 1           |
Then table '[dbo].[Product]' should contain the data
| Id:Key | Code   | Name      | Description | Cost:Number | ListPrice:Number | CategoryId | SupplierId | IsActive:Boolean | ReleaseDate:DateTime |
| 1      | code-1 | product-1 | desc-1      | 101.10      | 111.10           | 1          | 2          | true             | 2000-01-01           |
| 2      | code-2 | product-2 | desc-2      | 102.10      | 112.10           | 1          | 2          | true             | 2000-01-02           |

Scenario: Filter and validate multiple tables
Given I filter table '[dbo].[Supplier]' by
| FieldName | FieldValues |
| Name      | sup-1       |
Given I filter table '[dbo].[Category]' by
| FieldName | FieldValues |
| Name      | cat-2       |
Then table '[dbo].[Supplier]' should contain the data
| Id | Name   | ContactName | ContactPhone | ContactEmail |
| 1  | sup-1  | cont-1      | phone-1      | email-1      |
Then table '[dbo].[Category]' should contain the data
| Id | Name  |
| 2  | cat-2 |

Scenario: Validate empty table
Given I filter table '[dbo].[Supplier]' by
| FieldName | FieldValues |
| Name      | sup-10      |
Then table '[dbo].[Supplier]' should be empty

Scenario: Validate fields as GUID
Then query '(SELECT '1aaaaaaa-bbbb-cccc-dddd-2eeeeeeeeeee' AS Col1, '2AAAAAAA-BBBB-CCCC-DDDD-1EEEEEEEEEEE' AS Col2) AS Guids' should return the data
| Col1:Key:Guid                        | Col2:Guid                            |
| 1aaaaaaa-bbbb-cccc-dddd-2eeeeeeeeeee | 2AAAAAAA-BBBB-CCCC-DDDD-1EEEEEEEEEEE |
