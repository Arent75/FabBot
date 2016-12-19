FROM microsoft/dotnet:1.1-runtime
ARG source
WORKDIR /app
COPY ${source:-bin/Release/PublishOutput} .
ENTRYPOINT ["dotnet", "SexyBot.dll"]
