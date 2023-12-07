ARG VERSION=8.0-alpine3.18
ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS base
USER app
WORKDIR /app 

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
WORKDIR /src
COPY ["samples/Krosoft.Extensions.Samples.DotNet8.BlazorApp/Krosoft.Extensions.Samples.DotNet8.BlazorApp.csproj", "samples/Krosoft.Extensions.Samples.DotNet8.BlazorApp/"]
RUN dotnet restore "./samples/Krosoft.Extensions.Samples.DotNet8.BlazorApp/./Krosoft.Extensions.Samples.DotNet8.BlazorApp.csproj"
COPY . .
WORKDIR "/src/samples/Krosoft.Extensions.Samples.DotNet8.BlazorApp"
RUN dotnet build "./Krosoft.Extensions.Samples.DotNet8.BlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./Krosoft.Extensions.Samples.DotNet8.BlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Krosoft.Extensions.Samples.DotNet8.BlazorApp.dll"]