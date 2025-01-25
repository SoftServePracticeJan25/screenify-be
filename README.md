# screenify-be

To run this program, you need to set up the `secrets.json` file in your `Presentation` project with the following commands:

```bash
cd Presentation
```
```bash
dotnet user-secrets init
```
```bash
dotnet user-secrets set "ConnectionString" "(your_db_connection_string)"
```

Replace `(your_db_connection_string)` with your actual database connection string.

The repository already contains pre-created migrations, but if necessary, you can create your own migration using the following commands:
```bash
cd ..
```
```bash
dotnet ef migrations add YourMigrationName --project Infrastructure --startup-project Presentation
```
You can optionally replace `YourMigrationName` with your own migration name.

Then, update database with:
```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```
After running these commands, check your database to ensure that the tables have been created.

Once the database is set up, you can run the project.
By default, Swagger should automatically open in your browser, where you can test the backend functionality.
