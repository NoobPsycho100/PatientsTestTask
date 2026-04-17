FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PatientsTestTask.Web/PatientsTestTask.Web.csproj", "PatientsTestTask.Web/"]
RUN dotnet restore "./PatientsTestTask.Web/PatientsTestTask.Web.csproj"
COPY . .
WORKDIR "/src/PatientsTestTask.Web"
RUN dotnet build "./PatientsTestTask.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PatientsTestTask.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PatientsTestTask.Web.dll"]