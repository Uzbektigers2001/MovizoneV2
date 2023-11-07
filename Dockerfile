FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
COPY . .

WORKDIR /app/Project1/Project1
RUN dotnet publish ./Project1.csproj -o /app/build -c Release


FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ="Asia/Tashkent"
COPY --from=build-env /app/build .
ENTRYPOINT ["dotnet", "Project1.dll", "--urls=http://0.0.0.0:8001"]
