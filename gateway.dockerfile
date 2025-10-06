FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY ./src/GatewayService ./GatewayService

WORKDIR /app/GatewayService

RUN dotnet publish --configuration Release --runtime linux-x64 --self-contained true --output /app

WORKDIR /app

ENTRYPOINT ["/app/GatewayService.Web.Api"]