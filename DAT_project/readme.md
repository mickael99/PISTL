### Explications ###
J'ai créé un projet API.NET Core Web pour tester des fonctionnalités du framework dont on va avoir besoin, notamment EntityFramework qui permet 
de générer les structures en C# des tables de la bdd

Le projet se divise en 2 partie : API et UI

## API ##
La partie API c'est .NET, c'est la ou on va créer la structure de la bdd (.\Models), on va créer des controllers (.\Controllers) et des infterfaces

L'idée c'est que pour chaque "objet métier" (login, domain, server, ...) on va avoir besoin d'un controller qui pourra executer les requêtes (GET,POST,DELETE,..)

Ces controllers vont ensuite être utilisés par les composants de l'UI (le front)

Pour charger générer les models de la bdd il faut utiliser la commande scaffold (ça change en fonction de si on utilise Visual Studio sur windows ou dotnet sur Linux)
avec les infos "Server=nom_du_server;Database=nom_de_la_db;Trusted_Connection=true;" 

Il faudra créer la db avant sur SQL server manager (outil de .NET) ce que j'avait fait en amont

## UI ##
Le front en Angular

J'ai fait vite fait un truc avec une navbar si on va sur admin on a le form pour ajouter un user

L'idée est les inputs sont récupérés par un composant qui va appeler le controller sur l'api et lui fournir les info nécessaires pour faire la requête demandée

## Fonctionnement ##
Il faut lancer les 2 en meme temps donc d'un côté : dotnet run 

et de l'autre : ng serve --open

Pour l'instant j'arrive pas à faire communiquer les deux donc j'ai push l'appli purement pour montrer l'architecture dont ça devrait avoir l'air après

Je vous envoi ca juste pour que vous visualisiez vite fait la gueule du truc et on repartira from scratch c'est rapide à faire vu qu'on peut générer la plus part
des choses
