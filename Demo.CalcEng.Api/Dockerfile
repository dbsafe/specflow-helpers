#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.CalcEng.Api/Demo.CalcEng.Api.csproj", "Demo.CalcEng.Api/"]
RUN dotnet restore "Demo.CalcEng.Api/Demo.CalcEng.Api.csproj"
COPY . .
WORKDIR "/src/Demo.CalcEng.Api"
RUN dotnet build "Demo.CalcEng.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.CalcEng.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.CalcEng.Api.dll"]