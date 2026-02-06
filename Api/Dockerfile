# --- ETAPA 1: BUILD (El Chef) ---
# Usamos la imagen con el SDK completo para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# 1. Copiar solo los archivos de proyecto (.csproj) y restaurar dependencias
# Esto se hace primero para aprovechar la caché de Docker (si no cambias paquetes, no descarga de nuevo)
COPY *.sln .
COPY Api/*.csproj ./Api/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY CleanEcommerce.UnitTest/*.csproj ./CleanEcommerce.UnitTest/

# Descargar las librerías (Restore)
RUN dotnet restore

# 2. Copiar todo el resto del código fuente
COPY Api/. ./Api/
COPY Application/. ./Application/
COPY Domain/. ./Domain/
COPY Infrastructure/. ./Infrastructure/

# 3. Compilar y Publicar (Crear la DLL final optimizada)
WORKDIR /app/Api
RUN dotnet publish -c Release -o /app/out

# --- ETAPA 2: RUNTIME (El Mesero) ---
# Usamos una imagen liviana (solo ASP.NET Core Runtime, sin compiladores)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# 4. Copiamos los archivos compilados desde la Etapa 1 (build)
COPY --from=build /app/out .

# 5. Decimos qué comando arranca la app
ENTRYPOINT ["dotnet", "Api.dll"]