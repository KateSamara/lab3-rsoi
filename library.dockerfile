FROM  mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY ./src/LibrarySystem ./LibrarySystem

WORKDIR /app/LibrarySystem

RUN dotnet publish --configuration Release --runtime linux-x64 --self-contained true --output /app

WORKDIR /app

ENTRYPOINT ["/app/LibrarySystem.Web.Api"]