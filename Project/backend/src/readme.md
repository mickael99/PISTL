# Entity Framework

#### Pour pouvoir utiliser les Models et ecrire/lire dans la DB, s'assurer que les Models sont fait à partir de notre serveur

> Sous Linux:

```bash
dotnet ef dbcontext scaffold "Server=nom_serveur;Database=master;User Id=sa;Password=mot_de_passe;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -o Models --force # remplacer nom_serveur par votre serveur et mot_de_passe par votre mot de passe

```

> Sous Linux:

```bash
dotnet ef dbcontext scaffold "Server=nom_serveur;Database=master;TrustServerCertificate=true; Trusted_Connection=True;
" Microsoft.EntityFrameworkCore.SqlServer -o Models --force # remplacer nom_serveur par votre serveur
```
