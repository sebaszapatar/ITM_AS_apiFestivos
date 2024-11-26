FROM mrc.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

#Establecer puerto que escuchara ASP.NET Core
ENV ASPNETCORE_URLS=http://+:5235

#Exponer puerto que usa la aplicacion
EXPOSE 5235

FROM mrc.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "apiFestivos.Presentacion.dll"]
