FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

COPY . .

WORKDIR /app/Project1

RUN dotnet restore
WORKDIR /app/Project1
RUN dotnet publish -c Release -o /app/build -v detailed
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/build .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ="Asia/Tashkent"

ENTRYPOINT ["dotnet", "Project1.dll", "--urls=http://0.0.0.0:8001"]
