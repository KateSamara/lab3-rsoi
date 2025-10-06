FROM  mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY ./src/ReservationSystem ./ReservationSystem

WORKDIR /app/ReservationSystem

RUN dotnet publish --configuration Release --runtime linux-x64 --self-contained true --output /app

WORKDIR /app

ENTRYPOINT ["/app/ReservationSystem.Web.Api"]