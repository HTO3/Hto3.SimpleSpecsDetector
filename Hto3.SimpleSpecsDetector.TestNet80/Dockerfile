#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Hto3.SimpleSpecsDetector.TestNet80/Hto3.SimpleSpecsDetector.TestNet80.csproj", "Hto3.SimpleSpecsDetector.TestNet80/"]
COPY ["Hto3.SimpleSpecsDetector/Hto3.SimpleSpecsDetector.csproj", "Hto3.SimpleSpecsDetector/"]
RUN dotnet restore "./Hto3.SimpleSpecsDetector.TestNet80/./Hto3.SimpleSpecsDetector.TestNet80.csproj"
COPY . .
WORKDIR "/src/Hto3.SimpleSpecsDetector.TestNet80"
RUN dotnet build "./Hto3.SimpleSpecsDetector.TestNet80.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hto3.SimpleSpecsDetector.TestNet80.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hto3.SimpleSpecsDetector.TestNet80.dll"]