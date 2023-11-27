# Demo test

#### Instructions base de données:

###### Prérequis: SQL Server (mssql extention sur Visual Code si vous n'êtes pas sous Windows)

> Installation Server (qu'une seule fois, pas besoin je pense sur Windows et peut-être même pour Mac)

```bash
sudo /opt/mssql/bin/mssql-conf setup # choix 3 puis fournir mot de passe

sudo systemctl enable mssql-server

sudo systemctl start mssql-server

sqlcmd -S localhost -U sa -P 'votreMotDePasse' -C # mot de passe choisi dans sudo /opt/mssql/bin/mssql-conf setup
```

Sur VS code `Ctrl+Shift+P`-> MS SQL: Manage Connection Profiles et rémplir avec `localhost` et le Login/MotDePasse

#### Instructions front/client:

###### Prérequis: Node.js, Angular

```bash
cd Project/frontend
```

```bash
npm install
```

```bash
ng build
```

```bash
ng serve
```

###### Aller sur http://localhost:4200

#### Instructions backend/serveur:

###### Prérequis: .NET, C#

```bash
cd  Project/backend
```

```bash
dotnet run
```

---

#### Test :

Ajouter un compte depuis SQL Server ou avec addLogin() une fois dans le `Program.cs`.
