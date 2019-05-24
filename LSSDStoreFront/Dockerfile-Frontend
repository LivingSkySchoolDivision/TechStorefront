FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["LSSDStoreFront-FrontEnd/LSSDStoreFront-FrontEnd.csproj", "LSSDStoreFront-FrontEnd/"]
COPY ["LSSDStoreFront-Lib/LSSDStoreFront-Lib.csproj", "LSSDStoreFront-Lib/"]
COPY ["LSSDStoreFront-DB/LSSDStoreFront-DB.csproj", "LSSDStoreFront-DB/"]
RUN dotnet restore "LSSDStoreFront-FrontEnd/LSSDStoreFront-FrontEnd.csproj"
COPY . .
WORKDIR "/src/LSSDStoreFront-FrontEnd"
RUN dotnet build "LSSDStoreFront-FrontEnd.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LSSDStoreFront-FrontEnd.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LSSDStoreFront-FrontEnd.dll"]