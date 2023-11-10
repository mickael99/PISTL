### Prerequis ###
Il faut avoir docker ouvert si vous etes sur windows ou mac je crois

### Build ###
si vous regardez le dockerfile y'a la ligne 

RUN dotnet publish -c Release -o out

qui est en commentaire. Enfait sur mon windows cette commande provoque une erreur lorsque je build l'image docker donc pour éviter ce problème je fais la commande 

dotnet publish -c Release -o out

en amont. Ensuite on peut lancer la commande :

docker build -t datapi .  

Ca va créer l'image docker (vous pouvez vérifier avec : docker images). Apres il faut créer le container :

docker create --name core-datapi datapi

### Run ###
Enfin plus qu'a le lancer avec :

docker run -p 8080:80 datapi