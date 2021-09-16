FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AutofacExample/AutofacExample.csproj", "AutofacExample/"]
RUN dotnet restore "AutofacExample/AutofacExample.csproj"
COPY . .
WORKDIR "/src/AutofacExample"
RUN dotnet build "AutofacExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutofacExample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutofacExample.dll"]
