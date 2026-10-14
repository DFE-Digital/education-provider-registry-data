#!/bin/sh

set -e

echo "Restoring tools..."

dotnet tool restore \
    --tool-manifest /repo/generator/dotnet-tools.json

echo "Restoring packages..."

dotnet restore \
    /repo/generator/src/DfE.EducationProviderRegistry.Data.DatabaseModels

echo "Scaffolding models..."

cd /repo/generator

dotnet ef dbcontext scaffold \
    "$CONNECTION_STRING" \
    Npgsql.EntityFrameworkCore.PostgreSQL \
    --project src/DfE.EducationProviderRegistry.Data.DatabaseModels \
    --output-dir Models \
    --context-dir Context \
    --context EducationProviderRegistryDbContext \
    --no-pluralize \
    --no-onconfiguring \
    --force

echo "Done"