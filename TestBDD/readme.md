# Demo test
#### Instructions base de données:
###### Prérequis: Docker
```bash
docker run -d --name mon-mysql -e MYSQL_ROOT_PASSWORD=password -e MYSQL_DATABASE=users -p 3306:3306 mysql:latest
```

#### Instructions front/client:
###### Prérequis: Node.js, Angular
```bash
cd testBDD/frontend
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
cd  testBDD
```
```bash
dotnet run
```
---
#### Tester avec:  
- Email: test@test.com  
- Password: motdepasse

