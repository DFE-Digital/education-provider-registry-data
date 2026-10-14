#!/bin/sh

set -e

echo "Restoring tools..."
dotnet tool restore --tool-manifest /repo/generator/dotnet-tools.json

echo "Restoring packages..."
dotnet restore /repo/generator

# echo "Applying SQL scripts..."

# FILES=$(find /repo/db/init -name "*.sql" | sort -V)

# for file in $FILES
# do
#    echo "Running $file"

#    PGPASSWORD=$DB_PASSWORD psql \
#        -h $DB_HOST \
#        -U $DB_USER \
#        -d $DB_NAME \
#        -v ON_ERROR_STOP=1 \
#        -f "$file"
# done

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
