# Makefile for common backend .NET 8 CMDB commands

run:
	dotnet run --urls=http://localhost:5082

build:
	dotnet build

restore:
	dotnet restore

test:
	dotnet test

migrate:
	dotnet ef database update

add-migration:
	dotnet ef migrations add $${name}

format:
	dotnet format

clean:
	dotnet clean

# Usage:
# make run            # Run the backend API
# make build          # Build the project
# make restore        # Restore NuGet packages
# make test           # Run tests (if any)
# make migrate        # Apply latest EF Core migrations
# make add-migration name=MigrationName  # Add a new migration
# make format         # Format code
# make clean          # Clean build artifacts
