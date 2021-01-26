# ARM Template for Azure SQL Server

## Summary

This template deploys a single Azure SQL Server including one SQL database using the naming conventions of DEVDEER.

## Usage

In order to test this template you could run it inside the DEVDEER test sub with

```shell
.\deploy.ps1 -Stage Test -ResourceGroupName rg-sqlarm-test -TenantId 18ca94d4-b294-485e-b973-27ef77addb3e -SubscriptionId c764670f-e928-42c2-86c1-e984e524018a
```

This uses the `azuredeployment.parameters.test.json` and assumes your project name is `devdeer`.

## Basic settings

- Default firewall settings with allowing Azure resources to access
- SQL Server authentication is configured
- Min TLS is set to 1.2
- 1 database is created (name is configured in parameters with project name)

## Known issues

When trying to set the advanced security like this:

```json
{
    "name": "Default",
    "type": "securityAlertPolicies",
    "apiVersion": "2017-03-01-preview",
    "dependsOn": [
        "[resourceId('Microsoft.Sql/servers', variables('sql-server-name'))]"
    ],
    "properties": {
        "state": "Enabled",
        "emailAddresses": [
            "some.mail@devdeer.com"
        ],
        "emailAccountAdmins": false,
        "storageEndpoint": "[parameters('storage-endpoint')]",
        "storageAccountAccessKey": "[parameters('storage-key')]"
    }
}
```

it will fail with 500-errors (as of today which is 2020-07-10).

