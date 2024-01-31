FROM mcr.microsoft.com/mssql/server:2022-latest

# Install sqlpackage
USER root
RUN touch /var/log/sqlservr.log && chmod go+rxw /var/log/sqlservr.log && chmod ugo+rwx /var/run/ && chmod ugo+rwx /var/log/

# Create the data directory and set permissions
RUN mkdir -p /var/opt/mssql/data && chown -R mssql /var/opt/mssql/data && chgrp -R 1000 /var/opt/mssql/data



USER mssql
COPY dbo /dbo
COPY sqlservr-helper.sh /

# Start SQL Server and create database
RUN ACCEPT_EULA=Y SA_PASSWORD="Daniel123" MSSQL_COLLATION=Latin1_General_CI_AS bash /sqlservr-helper.sh start && \
    cd /dbo/Tables && \
    /opt/mssql-tools/bin/sqlcmd -Q "CREATE DATABASE DAT" -S localhost -U sa -P 'Daniel123' && \
    bash /sqlservr-helper.sh stop

# Run SQL scripts

RUN ACCEPT_EULA=Y SA_PASSWORD="Daniel123" MSSQL_COLLATION=Latin1_General_CI_AS bash /sqlservr-helper.sh start && \
find /dbo/Tables -name "*.sql" | sort | tee /dev/stderr | xargs -n 1 /opt/mssql-tools/bin/sqlcmd -b -S localhost -U sa -P 'Daniel123' -d DAT -i && \
bash /sqlservr-helper.sh stop

COPY DevData /DevData

RUN ACCEPT_EULA=Y SA_PASSWORD="Daniel123" MSSQL_COLLATION=Latin1_General_CI_AS bash /sqlservr-helper.sh start && \
cd /DevData && /opt/mssql-tools/bin/sqlcmd -b -S localhost -U sa -P 'Daniel123' -d DAT -i LoadDevData_Script.sql && \
bash /sqlservr-helper.sh stop

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Daniel123

ENV MSSQL_DB_NAME=DAT

EXPOSE 1433
