#!/bin/sh

set -e

echo "Restoring tools..."

dotnet tool restore \
    --tool-manifest /repo/generator/dotnet-tools.json

echo "Restoring packages..."

dotnet restore \
    /repo/generator/src/DfE.EducationProviderRegistry.Data.DatabaseModels

# Remove files and regenerate
rm -f /repo/generator/src/DfE.EducationProviderRegistry.Data.DatabaseModels/Models/*.cs
rm -f /repo/generator/src/DfE.EducationProviderRegistry.Data.DatabaseModels/Context/*.cs

echo "Scaffolding models..."

cd /repo/generator

dotnet ef dbcontext scaffold \
    "$CONNECTION_STRING" \
    Npgsql.EntityFrameworkCore.PostgreSQL \
    --project src/DfE.EducationProviderRegistry.Data.DatabaseModels \
    --startup-project src/DfE.EducationProviderRegistry.Data.DatabaseModels \
    --output-dir Models \
    --context-dir Context \
    --context EducationProviderRegistryDbContext \
    --no-pluralize \
    --no-onconfiguring \
    --force \
    --verbose

echo "Done"