FROM microsoft/dotnet:aspnetcore-runtime
COPY . /app
WORKDIR /app
 
ENV ASPNETCORE_URLS http://*:5431
EXPOSE 5431
 
ENTRYPOINT ["dotnet", "Stock.Information.Extraction.dll"]