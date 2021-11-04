FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY . /app
WORKDIR /app
 
ENV ASPNETCORE_URLS http://*:5435
EXPOSE 5435
 
ENTRYPOINT ["dotnet", "Stock.Symbol.Feature.Load.dll"]