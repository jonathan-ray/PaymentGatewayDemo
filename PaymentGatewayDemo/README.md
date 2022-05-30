#Payment Gateway Demo

## Setup
### Encryption Keys
To use the payment gateway you need to setup your own encryption keys. In production this will be set using a secret management system, but to do this locally use the dotnet user-secrets tool.

To initialize, in the Developer PowerShell navigate to the directory that contains the .csproj file and run:

```
dotnet user-secrets init
```

Once complete add encryption keys for the following secrets:

```
dotnet user-secrets set "ApiKeyEncryptionKey" "##YOUR_ENCRYPTION_KEY_HERE"
dotnet user-secrets set "CardDetailsEncryptionKey" "##YOUR_ENCRYPTION_KEY_HERE"
```

## How To: Call the API
Use Swagger to view the full API calls.

You need to add a request header `PaymentGateway-ApiKey`