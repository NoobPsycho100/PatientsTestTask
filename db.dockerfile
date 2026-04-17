FROM mcr.microsoft.com/mssql/server:2025-latest

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=IH4teStr0ng!estPassw0rd$

# Optional: Copy initialization scripts into the container
# Scripts in this directory won't run automatically like Postgres; 
# you need a custom entrypoint script to execute them [3, 16]
#COPY ./init.sql /usr/config/init.sql

# Expose the default SQL Server port
EXPOSE 1433