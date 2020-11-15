#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Hto3.SimpleSpecsDetector.TestCore31/Hto3.SimpleSpecsDetector.TestCore31.csproj", "Hto3.SimpleSpecsDetector.TestCore31/"]
COPY ["Hto3.SimpleSpecsDetector/Hto3.SimpleSpecsDetector.csproj", "Hto3.SimpleSpecsDetector/"]
RUN dotnet restore "Hto3.SimpleSpecsDetector.TestCore31/Hto3.SimpleSpecsDetector.TestCore31.csproj"
COPY . .
WORKDIR "/src/Hto3.SimpleSpecsDetector.TestCore31"
RUN dotnet build "Hto3.SimpleSpecsDetector.TestCore31.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hto3.SimpleSpecsDetector.TestCore31.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hto3.SimpleSpecsDetector.TestCore31.dll"]