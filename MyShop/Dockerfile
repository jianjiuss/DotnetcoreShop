FROM microsoft/dotnet:latest
WORKDIR /app/myshop
COPY ./MyShop /app/myshop
COPY ./Models /app/Models
RUN dotnet restore
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet","run"]