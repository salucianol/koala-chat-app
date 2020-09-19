git clone https://github.com/salucianol/koala-chat-app.git KoalaChatApp
docker pull rabbitmq:3-management
docker run -d -P --hostname koala-chat-rabbit --name koala-chat-rabbit -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=Abcd1234! rabbitmq:3-management
cd .\KoalaChatApp\KoalaChatApp.Bot
dotnet publish -c Release --self-contained false -r win10-x64 -o published
sc.exe create KoalaChatAppBot binPath="$PWD\published\KoalaChatApp.Bot.exe" DisplayName="Koala Chat App Bot"
sc.exe start KoalaChatAppBot
cd .\KoalaChatApp\KoalaChatApp.Web
dotnet publish -c Release -o published
cd published
dotnet KoalaChatApp.Web.dll