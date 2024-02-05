# Projet DAT
Le projet DAT (Data Administration Tool) est une application de gestion de domaines et d'infrastructure fournit à la société UpClear.
Cette application est construit en 3-layers avec un frontend développé en Angular, une API Rest en C# et une base de données déployés sur un Azure SQL Server.

## Pré-requis
#### Environnement : Docker
Il faut avoir une version stable et à jour de Docker pour déployer l'application.

#### Backend : .NET
L'API est développé avec le framework .NET et le package Entity Framework. Il est aussi recommandé d'ajouter le package NUnit

#### Frontend : Node.js, Angular
Pour démarrer l'application il faut avoir préalablement installé Node.js, Angular

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
