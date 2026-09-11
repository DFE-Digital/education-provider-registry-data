ADF="/subscriptions/5c83eb53-a94f-4778-b258-1f33efe49655/resourceGroups/s189d01-eprdat-ts-rg/providers/Microsoft.DataFactory/factories/s189d01-eprdat-development-adf"

npm run start export ./adf $ADF ./export

node node_modules/@microsoft/azure-data-factory-utilities/lib/index.js export ./adf $ADF ./export/
node node_modules/@microsoft/azure-data-factory-utilities/lib/index.js validate ./adf $ADF