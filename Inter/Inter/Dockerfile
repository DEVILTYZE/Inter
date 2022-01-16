FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Inter/Inter.csproj", "Inter/"]
RUN dotnet restore "Inter/Inter.csproj"
COPY . .
WORKDIR "/src/Inter"
RUN dotnet build "Inter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inter.dll"]
