FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app

COPY . .

WORKDIR /app/Movizone.MVC

RUN dotnet restore
WORKDIR /app/Movizone.MVC
RUN dotnet publish -c Release -o /app/build -v detailed
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/build .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ="Asia/Tashkent"

ENTRYPOINT ["dotnet", "Movizone.MVC.dll", "--urls=http://0.0.0.0:8001"]
