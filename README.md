
### Add migrations

```
$ cd src/Sample.Data
$ dotnet ef migrations add "Migrations name" --context AppDbContext --startup-project ../Sample.App --project ../Sample.Data.SqlServer 
```