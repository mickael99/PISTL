### How to create a Docker SQL image

## Utilité de ce dossier
Le dossier /docker contient les scripts et le Dockerfile utile à l'installation d'une base de donnée locale sur un Docker container.
Cette installation est optionnelle et demande des modifications dans le code pour l'accés à la base de données locale depuis le container backend.

## Génération de l'image et du container

```bash
docker build -t sql_dat .

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Daniel123' -p 1434:1433 --name sql_dat_container sql_dat
```


> You can connect now to the server with Microsoft SQL Server or **Azure Data Studio** using the this address: **127.0.0.1, 1434** to better visualize data.

Once in the backend you can do the scaffold to create the /Models: (PAS OBLIGATOIRE)

```bash
dotnet ef dbcontext scaffold "Server=127.0.0.1,1434;Database=DAT;User Id=sa;Password=Daniel123;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -o Models --force
```

You have to remplace in the **appsettings.json** file the connection string like that:

```json
"ConnectionStrings": {
"DAT_projectConnectionString":  "Data Source=127.0.0.1,1434;Initial Catalog=DAT;User ID=sa;Password=Daniel123"
}
```

Also you have to add this in your **Startup.cs** file, at the end of the **ConfigureServices()** method:

```csharp
services.AddDbContext<DbContext>(options  => {
	options.UseSqlServer(Configuration.GetConnectionString("DAT_projectConnectionString"));
});
```

---
