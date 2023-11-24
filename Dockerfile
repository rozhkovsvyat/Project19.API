#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN apt-get update \
    && apt-get install -y curl \
    && apt-get install -y wget \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Project 19.Api/Project 19.Api.csproj", "Project 19.Api/"]
RUN dotnet restore "Project 19.Api/Project 19.Api.csproj"
COPY . .
WORKDIR "/src/Project 19.Api"
RUN dotnet build "Project 19.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Project 19.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project 19.Api.dll"]