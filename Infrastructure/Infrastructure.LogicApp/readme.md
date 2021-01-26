# ARM Template for Logic App 

## Summary

This template deploys a Logic App using a JSON definition put in a single file.

## Usage

In order to test this template you could run it inside the DEVDEER test sub with

```shell
.\deploy.ps1 -Stage Test -ResourceGroupName rg-logic-app-test -TenantId 18ca94d4-b294-485e-b973-27ef77addb3e -SubscriptionId c764670f-e928-42c2-86c1-e984e524018a
```

This uses the `azuredeployment.parameters.test.json` and assumes your project name is `logic-app`.
