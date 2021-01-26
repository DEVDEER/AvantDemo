# ARM Template for Azure Storage Account

## Summary

This ARM template deploys an Azure Storage Account in the default settings regarding the DEVDEER rules.

## Usage

In order to test this template you could run it inside the DEVDEER test sub with

```shell
.\deploy.ps1 -Stage Test -ResourceGroupName rg-storage-test -TenantId 18ca94d4-b294-485e-b973-27ef77addb3e -SubscriptionId c764670f-e928-42c2-86c1-e984e524018a
```

This uses the `azuredeployment.parameters.test.json` and assumes your project name is `cftest`.

## Basic settings

- Standard SKU with Hot tier is created
- Minimum TLS is version 1.2
- Soft-Delete is enabled (30 days retention)

## Known issues

- It's unclear where to set the minimum TLS version in ARM (tried properties).