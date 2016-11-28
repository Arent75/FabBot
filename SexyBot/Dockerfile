FROM microsoft/dotnet:latest
ARG source
WORKDIR /app
COPY ${source:-bin/Release/PublishOutput} .
ENTRYPOINT ["dotnet", "SexyBot.dll"]
