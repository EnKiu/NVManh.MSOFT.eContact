#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MS.eContact.Web/MS.eContact.Web.csproj", "MS.eContact.Web/"]
COPY ["MS.ApplicationCore/MS.ApplicationCore.csproj", "MS.ApplicationCore/"]
COPY ["MS.Infrastructure/MS.Infrastructure.csproj", "MS.Infrastructure/"]
RUN dotnet restore "MS.eContact.Web/MS.eContact.Web.csproj"

COPY . .
WORKDIR "/src/MS.eContact.Web"
RUN dotnet build "MS.eContact.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MS.eContact.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MS.eContact.Web.dll"]