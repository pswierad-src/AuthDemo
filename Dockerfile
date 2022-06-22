﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuthDemo.Api/AuthDemo.Api.csproj", "AuthDemo.Api/"]
RUN dotnet restore "AuthDemo.Api/AuthDemo.Api.csproj"
COPY . .
WORKDIR "/src/AuthDemo.Api"
RUN dotnet build "AuthDemo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthDemo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthDemo.Api.dll"]
