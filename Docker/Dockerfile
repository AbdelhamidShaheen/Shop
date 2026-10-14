FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

# Remove stale restore/build files
RUN rm -rf bin obj

# Restore packages inside Docker
RUN dotnet restore Shop.csproj

# Publish using the restored packages
RUN dotnet publish Shop.csproj \
    -c Release \
    -o /out \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /out .

EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "Shop.dll"]
