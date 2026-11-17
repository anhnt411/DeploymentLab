
# Stage 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY DeploymentLab.Api/DeploymentLab.Api.csproj DeploymentLab.Api/

RUN dotnet restore DeploymentLab.Api/DeploymentLab.Api.csproj

COPY DeploymentLab.Api/ DeploymentLab.Api/

WORKDIR /src/DeploymentLab.Api

RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "DeploymentLab.Api.dll"]
