FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Homees.WebAPI/Homees.WebAPI.csproj", "Homees.WebAPI/"]
RUN dotnet restore "src/Homees.WebAPI/Homees.WebAPI.csproj"
COPY . .
WORKDIR "/src/Homees.WebAPI"
RUN dotnet build "Homees.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Homees.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Homees.WebAPI.dll"]
