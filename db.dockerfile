### firstly build dacpac schema:
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["PatientsTestTask.DB/PatientsTestTask.DB2.csproj", "PatientsTestTask.DB/"]
RUN dotnet restore "./PatientsTestTask.DB/PatientsTestTask.DB2.csproj"
COPY . .
WORKDIR "/src/PatientsTestTask.DB"
RUN dotnet build "PatientsTestTask.DB2.csproj" -c Release -o /app/build
# Build - /app/build/PatientsTestTask.DB2.dacpac created



FROM mcr.microsoft.com/mssql/server:2025-latest AS mssql

COPY --from=build /app/build/PatientsTestTask.DB2.dacpac /tmp/db.dacpac

USER root
### Install SQLPackage for Linux and make it executable
RUN apt-get update \
    && apt-get install unzip libunwind8 -y

# Install SQLPackage for Linux and make it executable
RUN wget -progress=bar:force -q -O sqlpackage.zip https://aka.ms/sqlpackage-linux \
    && unzip -qq sqlpackage.zip -d /opt/sqlpackage \
    && chmod +x /opt/sqlpackage/sqlpackage \
    && chown -R mssql /opt/sqlpackage \
    && mkdir /tmp/db \
    && chown -R mssql /tmp/db
	
USER mssql

ARG DBNAME=PatientsTestTaskDB
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=IH4teStr0ng!estPassw0rd$

# Launch SQL Server, confirm startup is complete, deploy the DACPAC, then terminate SQL Server.
# See https://itnext.io/how-to-build-and-run-a-sql-container-using-a-dacpac-file-c7b0d30f6255
RUN ( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started" \
    && /opt/sqlpackage/sqlpackage /a:Publish /tsn:localhost /tdn:${DBNAME} /tu:sa /tp:$MSSQL_SA_PASSWORD /TargetTrustServerCertificate:True /sf:/tmp/db.dacpac \
    && rm -r /tmp/db \
    && pkill sqlservr

USER root
RUN rm -r /opt/sqlpackage
USER mssql

EXPOSE 1433