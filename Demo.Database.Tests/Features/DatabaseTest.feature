Feature: DatabaseTest
	Demonstrates populating and asserting expected data in a SQL Server database

Scenario: Populate and validate a table
Given table '[dbo].[Product]' contains the data
| Id | Code   | Name      | Description | Cost   | ListPrice | CategoryId | SupplierId | IsActive | ReleaseDate | CreatedOn  |
| 1  | code-1 | product-1 | desc-1      | 101.10 | 111.10    | 1          | 2          | 1        | 2000-01-01  | 2000-02-01 |
| 2  | code-2 | product-2 | desc-2      | 102.10 | 112.10    | 1          | 2          | 1        | 2000-01-02  | 2000-02-02 |
| 3  | code-3 | product-3 | desc-3      | 103.10 | 113.10    | 2          | 1          | 1        | 2000-01-03  | 2000-02-03 |
| 4  | code-4 | product-4 |             | 104.10 | 114.10    | 2          | 1          | 1        | 2000-01-04  | 2000-02-04 |
| 5  | code-5 | product-5 | [NULL]      | 105.10 | 115.10    | 2          | 1          | 0        | 2000-01-05  | 2000-02-05 |
	
When I execute my operation

Then table '[dbo].[Product]' should contain the data
| Id:Key | Code   | Name      | Description | Cost:Numeric | ListPrice | CategoryId | SupplierId | IsActive:Boolean | ReleaseDate:DateTime | CreatedOn:DateTime |
| 1      | code-1 | product-1 | desc-1      | 101.10       | 111.10    | 1          | 2          | true             | 2000-01-01           | 2000-02-01         |
| 2      | code-2 | product-2 | desc-2      | 102.10       | 112.10    | 1          | 2          | true             | 2000-01-02           | 2000-02-02         |
| 3      | code-3 | product-3 | desc-3      | 103.10       | 113.10    | 2          | 1          | true             | 2000-01-03           | 2000-02-03         |
| 4      | code-4 | product-4 |             | 104.10       | 114.10    | 2          | 1          | true             | 2000-01-04           | 2000-02-04         |
| 5      | code-5 | product-5 | [NULL]      | 105.10       | 115.10    | 2          | 1          | false            | 2000-01-05           | [IGNORE]           |