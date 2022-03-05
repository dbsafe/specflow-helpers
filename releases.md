### Version 1.6.0
- Package: Helpers.Specflow.Steps.Db.WebApi
  Added support for testing http endpoints using `PATCH`.

- Packages Helpers.Specflow.Steps.Db.Pg and Helpers.Specflow.Steps.Db.Dql
  Added `Given` to filter a table before validating the content
```
Given I filter table 'public.product' by
| FieldName | FieldValues |
| id        | 2           |
```

  Added	`Then` to validate that a table is empty
```
Then table 'public.supplier' should be empty
```

### Version 1.5.0
- Added package `Helpers.Specflow.Steps.Db.Pg` to support populating and validating tables in PostgreSql.

### Version 1.4.0
- Added package `Helpers.Specflow.Steps.Db.Sql` to support populating and validating tables in MS-SQL Server.
- Removed package `Helpers.Specflow.Steps.Db`.

### Version 1.3.0.106-beta
- Added package `Helpers.Specflow.Steps.Db` with methods to support populating tables and validating data in MS-SQL Server.

### Version 1.3.0.94 and before
- Included packages `Helpers.Specflow.Steps.Object` and `Helpers.Specflow.Steps.WebApi`
