FROM microsoft/dotnet:latest
WORKDIR /app/myshop
COPY . /app/myshop
RUN dotnet restore
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet","run"]