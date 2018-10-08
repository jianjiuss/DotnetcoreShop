FROM microsoft/dotnet:latest
WORKDIR /app/backend
COPY ./Backend /app/backend
COPY ./Models /app/Models
RUN dotnet restore
EXPOSE 5001
ENV ASPNETCORE_URLS http://*:5001
ENTRYPOINT ["dotnet","run"]