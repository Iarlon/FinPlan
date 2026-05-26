FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /financeiro


COPY financeiro/Financeiro.csproj ./
RUN dotnet restore

COPY financeiro ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /financeiro
COPY --from=build-env /financeiro/out .

USER $APP_UID

EXPOSE 80
ENTRYPOINT ["dotnet", "Financeiro.dll"]