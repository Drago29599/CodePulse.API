# Stage 1: Build and publish the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["CodePulse.API/CodePulse.API.csproj", "CodePulse.API/"]
RUN dotnet restore "./CodePulse.API/CodePulse.API.csproj"

# Copy everything else and build
COPY . .

WORKDIR "/src/CodePulse.API"
RUN dotnet build "./CodePulse.API.csproj" -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish "./CodePulse.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=publish /app/publish .

# Copy appsettings.json into the Docker image
COPY CodePulse.API/appsettings.json ./

# Define environment variable for ASP.NET Core environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Define environment variable for the connection string
ENV ConnectionStrings__CodePulseConnectionString="workstation id=CodePulseDB.mssql.somee.com;packet size=4096;user id=Drago29_SQLLogin_1;pwd=5b6r7dne2r;data source=CodePulseDB.mssql.somee.com;persist security info=False;initial catalog=CodePulseDB;TrustServerCertificate=True"

# Expose port 8080 for HTTP traffic (adjust as needed)
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "CodePulse.API.dll"]