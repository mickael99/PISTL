# Set your Azure SQL Database connection details
serverName = "tcp:datserver2.database.windows.net,1433"
databaseName = "DAT"
username = "walter"
password = "Daniel123"
scriptFolderPath = "/docker/dbo/Tables/"

# Loop through each SQL script and execute it
for scriptPath in "$scriptFolderPath"/*.sql; do
    sqlcmd -S "$serverName" -d "$databaseName" -U "$username" -P "$password" -i "$scriptPath"
done