FROM microsoft/dotnet:latest
WORKDIR /app/backend
COPY . /app/backend
RUN dotnet restore
EXPOSE 5001
ENV ASPNETCORE_URLS http://*:5001
ENTRYPOINT ["dotnet","run"]