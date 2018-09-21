FROM microsoft/dotnet:latest
WORKDIR /app/storage_service
COPY . /app/storage_service
RUN dotnet restore
EXPOSE 5002
ENV ASPNETCORE_URLS http://*:5002
ENTRYPOINT ["dotnet","run"]