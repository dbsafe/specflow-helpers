Database testing
----------------

Package | Database
-|-
[Helpers.Specflow.Steps.Db.Sql](https://www.nuget.org/packages/Helpers.Specflow.Steps.Db.Sql/) | MS-SQL Server
[Helpers.Specflow.Steps.Db.Pg](https://www.nuget.org/packages/Helpers.Specflow.Steps.Db.Pg/) | PostgreSql

Step definition class
---------------------

There are two ways of using the steps defined in the library.
1. The Steps definition class can inherit from `PgSteps` or `SqlSteps` depending on the target database.
2. The Steps definition class can use an instance of `PgSteps` or `SqlSteps` depending on the target database and then define custom `Given` and `Then` steps that call methods in `PgSteps` or `SqlSteps`

**Dependencies**

`PgSteps` and `SqlSteps` use [DbSafe](https://github.com/dbsafe/dbsafe) for loading and validating the data. 
[DbSafe](https://github.com/dbsafe/dbsafe) uses [formatters](https://github.com/dbsafe/dbsafe#column-formatters) to simplify the expected data.
[DbSafe](https://github.com/dbsafe/dbsafe) can be used for preparing the database for the test.

When validating the expected data in a table, `PgSteps` and `SqlSteps` use the method to validate an complex-element array from Helpers.Specflow.Steps.Object.

Steps definition class inheriting from `PgSteps`
------------------------------------------------

pros: Less code in steps file.

```csharp
[Binding]
public class DatabaseTestSteps : PgSteps
{
    private static string _connectionString = "loaded 😊 or hardcoded 😒";

    private static FormatterManager _formatter;

    static DatabaseTestSteps()
    {
        _formatter = new FormatterManager();
        // Adding formatters if needed.
        // _formatter.Register(typeof(decimal), new DecimalFormatter("0.00"));
    }

    public DatabaseTestSteps() : base(_connectionString, _formatter)
    {
    }

    [When(@"I execute my operation")]
    public void ExecuteOperation()
    {
        // custom logic
    }
}
```

**Inside the feature file**

Populating a table:
```
Given table 'public.supplier' contains the data
| name  | contact_name | contact_phone | contact_email |
| sup-1 | cont-1       | phone-1       | email-1       |
| sup-2 | cont-2       | phone-2       | email-2       |
```

Populating a table that has an identity column:
```
Given table with identity columns 'public.supplier' contains the data
| id | name  | contact_name | contact_phone | contact_email |
| 1  | sup-1 | cont-1       | phone-1       | email-1       |
| 2  | sup-2 | cont-2       | phone-2       | email-2       |
```

Validating the expected data in a table
```
Then table 'public.product' should contain the data
| id:Key | code   | name      | description | cost:Number | list_price:Number | category_id | supplier_id | is_active:Boolean | release_date:DateTime | created_on:DateTime |
| 101    | code-1 | product-1 | [NULL]      | 101.10      | 111.10            | 1           | 2           | true              | 2000-01-01            | 2000-02-01          |
| 102    | code-2 | product-2 | desc-2      | 102.10      | 112.10            | 1           | 2           | true              | 2000-01-02            | 2000-02-02          |
| 103    | code-3 | product-3 | desc-3      | 103.10      | 113.10            | 2           | 1           | true              | 2000-01-03            | 2000-02-03          |
| 104    | code-4 | product-4 |             | 104.10      | 114.10            | 2           | 1           | false             | 2000-01-04            | 2000-02-04          |
| 105    | code-5 | product-5 | [NULL]      | 105.10      | 115.10            | 2           | 1           | false             | 2000-01-05            | 2000-02-05          |
```

Validating the expected data filtering the content of a table
```
Given I filter table 'public.product' by
| FieldName | FieldValues |
| id        | 1,2,5       |
| is_active | 1           |
Then table 'public.product' should contain the data
| id:Key | code   | name      | description | cost:Number | list_price:Number | category_id | supplier_id | is_active:Boolean | release_date:DateTime |
| 1      | code-1 | product-1 | desc-1      | 101.10      | 111.10            | 1           | 2           | true              | 2000-01-01            |
| 2      | code-2 | product-2 | desc-2      | 102.10      | 112.10            | 1           | 2           | true              | 2000-01-02            |
```

Validating that a table (or the result of filtering the table) is empty
```
Then table 'public.supplier' should be empty
```

Steps definition class using an instance of `PgSteps` and defining custom phrases
---------------------------------------------------------------------------------

pros: SpecFlow scenarios can use domain-related phrases.

```csharp
[Binding]
public class DatabaseTestSteps
{
    private static string _connectionString = "loaded 😊 or hardcoded 😒";

    private static FormatterManager _formatter;

    private readonly PgSteps _pgSteps;

    static DatabaseTestSteps()
    {
        _formatter = new FormatterManager();
        // Adding formatters if needed.
        // _formatter.Register(typeof(decimal), new DecimalFormatter("0.00"));
    }

    public DatabaseTestSteps()
    {
        _pgSteps = new PgSteps(_connectionString, _formatter);
    }

    [Given(@"my suppliers are")]
    public void SetSuppliers(Table table)
    {
        _pgSteps.SetTableWithoutIdentityColumns("public.supplier", table);
    }

    [Then(@"products should contain")]
    public void AssertProducts(Table table)
    {
        _pgSteps.AssertTable("public.product", table);
    }

    [When(@"I execute my operation")]
    public void ExecuteOperation()
    {
        // custom logic
    }
}
```

**Inside the feature file**

Populating a table:
```
Given my suppliers are
| id | name  | contact_name | contact_phone | contact_email |
| 1  | sup-1 | cont-1       | phone-1       | email-1       |
| 2  | sup-2 | cont-2       | phone-2       | email-2       |
```

Validating the expected data in a table
```
Then products should contain
| id:Key | code   | name      | description | cost:Number | list_price:Number | category_id | supplier_id | is_active:Boolean | release_date:DateTime |
| 1      | code-1 | product-1 | desc-1      | 101.10      | 111.10            | 1           | 2           | true              | 2000-01-01            |
| 2      | code-2 | product-2 | desc-2      | 102.10      | 112.10            | 1           | 2           | true              | 2000-01-02            |
| 3      | code-3 | product-3 | desc-3      | 103.10      | 113.10            | 2           | 1           | true              | 2000-01-03            |
| 4      | code-4 | product-4 |             | 104.10      | 114.10            | 2           | 1           | false             | 2000-01-04            |
| 5      | code-5 | product-5 | [NULL]      | 105.10      | 115.10            | 2           | 1           | false             | 2000-01-05            |
```
