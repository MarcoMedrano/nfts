#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/NftSample/NftSample.csproj", "/src/NftSample/"]
COPY ["src/NftSample.Api/NftSample.Api.csproj", "/src/NftSample.Api/"]
COPY ["src/NftSample.Client/NftSample.Client.csproj", "/src/NftSample.Client/"]
COPY ["src/NftSample.Infraestructure/NftSample.Infraestructure.csproj", "/src/NftSample.Infraestructure/"]
COPY ["src/NftSample.Business/NftSample.Business.csproj", "/src/NftSample.Business/"]
COPY ["src/NftSample.Dal/NftSample.Dal.csproj", "/src/NftSample.Dal/"]
COPY ["src/NftSample.Dtos/NftSample.Dtos.csproj", "/src/NftSample.Dtos/"]
COPY ["src/NftSample.Entities/NftSample.Entities.csproj", "/src/NftSample.Entities/"]

RUN dotnet restore "/src/NftSample/NftSample.csproj"
RUN dotnet restore "/src/NftSample.Api/NftSample.Api.csproj"
RUN dotnet restore "/src/NftSample.Client/NftSample.Client.csproj"
RUN dotnet restore "/src/NftSample.Infraestructure/NftSample.Infraestructure.csproj"
RUN dotnet restore "/src/NftSample.Business/NftSample.Business.csproj"
RUN dotnet restore "/src/NftSample.Dal/NftSample.Dal.csproj"
RUN dotnet restore "/src/NftSample.Dtos/NftSample.Dtos.csproj"
RUN dotnet restore "/src/NftSample.Entities/NftSample.Entities.csproj"
WORKDIR /src
COPY /src .
WORKDIR /src/NftSample.Client
RUN dotnet build "/src/NftSample/NftSample.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "/src/NftSample/NftSample.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NftSample.dll"]