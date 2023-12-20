### How to create a Docker SQL image

```bash
docker build -t [image_name] .

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=[your_password]' -p 1434:1433 --name [container_name] [image_name]
```

> You can connect now to the server with Microsoft SQL Server or Azure Data Studio using the this address: **127.0.0.1, 1434** to better visualize data.

Once in the backend you can do the scaffold to create the /Models:

```bash
dotnet ef dbcontext scaffold "Server=127.0.0.1,1434;Database=DAT;User Id=sa;Password=[your_password];TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -o Models --force
```

You have to remplace in the **appsettings.json** file the connection string like that:

```json
"ConnectionStrings": {
"DAT_projectConnectionString":  "Data Source=127.0.0.1,1434;Initial Catalog=DAT;User ID=sa;Password=[your_password]"
}
```

Also you have to add this in your **Startup.cs** file, at the end of the **ConfigureServices()** method:

```csharp
services.AddDbContext<DbContext>(options  => {
	options.UseSqlServer(Configuration.GetConnectionString("DAT_projectConnectionString"));
});
```

---
