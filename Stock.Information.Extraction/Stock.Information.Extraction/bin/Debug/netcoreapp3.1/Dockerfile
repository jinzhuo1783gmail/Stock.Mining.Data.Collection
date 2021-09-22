FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY . /app
WORKDIR /app
 
ENV ASPNETCORE_URLS http://*:5431
EXPOSE 5431
 
ENTRYPOINT ["dotnet", "Stock.Information.Extraction.dll"]