# screenify-be

To run this program, you need to set up the `secrets.json` file in your `Presentation` project with the following commands:

```bash
cd Presentation
```
```bash
dotnet user-secrets init
```

Next, you have to set your connections strings and other things like this
```bash
{
  "ConnectionString": "(your_db_connection_string)",
  "AzureStorage": {
    "ConnectionString": "(your_azure_account_storage_connection_string_with_key)",
    "ContainerName": "(your_avatars_container_name)"
  },
  "JWT": {
    "Issuer": "(server_that_issues_token)",
    "Audience": "(server_that_receives_token)",
    "SigningKey": "(your_512bit_signing_key)"
  },
  "SendGrid": {
    "ApiKey": "(your_sendgrid_api_key)",
    "FromEmail": "(email_which_sends_mails)",
    "FromName": "(e.g. Screenify-reply)"
  },
  "HangfireConnection": "(your_hangfire_connection_string)",
  "BaseUrl": "(your_server_url_that_holds_backend)"
}
```

Replace `(scopes_and_text_within)` with your actual data described inside.

The repository already contains pre-created migrations, but it's recommended that you create your own migration using the following commands:
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

Once the database and secrets are set up, you can run the project.
By default, Swagger should automatically open in your browser, where you can test the backend functionality.

