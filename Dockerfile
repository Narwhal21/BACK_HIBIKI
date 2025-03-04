# Usa la imagen base del SDK de .NET para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establece el directorio de trabajo
WORKDIR /source

# Copia los archivos .csproj al contenedor
COPY ./*.csproj ./

# Restaura las dependencias de NuGet
RUN dotnet restore

# Copia todo el código fuente
COPY ./ ./

# Compila la aplicación
RUN dotnet build BACK_HIBIKI.csproj -c Release -o /app

# Usa la imagen base de ASP.NET para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Establece el directorio de trabajo en el contenedor de ejecución
WORKDIR /app

# Copia los archivos compilados desde la etapa anterior
COPY --from=build /app .

# Expone el puerto en el que la aplicación estará disponible
EXPOSE 80

# Establece el comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "BACK_HIBIKI.dll"]
