FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["blog-backend/blog-backend.csproj", "blog-backend/"]
RUN dotnet restore "blog-backend/blog-backend.csproj"
COPY . .
WORKDIR "/src/blog-backend"
RUN dotnet build "blog-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blog-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "blog-backend.dll"]
