FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Test.MVC/Test.MVC.csproj", "Test.MVC/"]
RUN dotnet restore "Test.MVC/Test.MVC.csproj"
COPY . .
WORKDIR "/src/Test.MVC"
RUN dotnet build "Test.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.MVC.dll"]