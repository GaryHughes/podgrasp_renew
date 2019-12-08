# Build using the SDK image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
RUN apt-get update && apt-get install -y npm
WORKDIR /podgrasp
COPY . .
RUN dotnet publish -c Release -o out

# Deploy with the runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /podgrasp
COPY --from=build-env /podgrasp/out .

ENTRYPOINT [ "/podgrasp/Podgrasp.Service" ]