FROM mcr.microsoft.com/mssql/server:2022-latest

# Install sqlpackage
USER root
RUN touch /var/log/sqlservr.log && chmod go+rxw /var/log/sqlservr.log && chmod ugo+rwx /var/run/ && chmod ugo+rwx /var/log/

USER mssql
COPY dbo /dbo
COPY sqlservr-helper.sh /

RUN ACCEPT_EULA=Y SA_PASSWORD="Daniel123" MSSQL_COLLATION=Latin1_General_CI_AS bash /sqlservr-helper.sh start && \
    cd /dbo/Tables && \
    /opt/mssql-tools/bin/sqlcmd -Q "CREATE Database DAT" -S localhost -U sa -P 'Daniel123' && \
    find . -name "*.sql" -print0 | \
    xargs -n 1 -0 /opt/mssql-tools/bin/sqlcmd -b -S localhost -U sa -P 'Daniel123' -d DAT -i && \
    cd / && bash /sqlservr-helper.sh stop

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Daniel123

ENV MSSQL_DB_NAME=DAT

EXPOSE 1433