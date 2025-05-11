FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy all files into container
COPY . .

# Restore and build
RUN dotnet restore
RUN dotnet build --no-restore

# Ensure Results folder exists (for report output)
RUN mkdir -p /app/Results

# Run tests and generate report in /app/Results
ENTRYPOINT ["dotnet", "test", "--no-build", "--logger:trx;LogFileName=Results/test_results.trx"]
