FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["sample.microservice.reservationactor.interfaces/sample.microservice.reservationactor.interfaces.csproj", "sample.microservice.reservationactor.interfaces/"]
COPY ["sample.microservice.reservationactor.service/sample.microservice.reservationactor.service.csproj", "sample.microservice.reservationactor.service/"]
RUN dotnet restore "sample.microservice.reservationactor.interfaces/sample.microservice.reservationactor.interfaces.csproj"
RUN dotnet restore "sample.microservice.reservationactor.service/sample.microservice.reservationactor.service.csproj"
COPY . .
WORKDIR "/src/sample.microservice.reservationactor.service"
RUN dotnet publish "sample.microservice.reservationactor.service.csproj" -c Release -o /app/publish --no-restore

ARG BUILD_DATE
ARG BUILD_VERSION
LABEL org.label-schema.build-date=$BUILD_DATE
LABEL org.label-schema.version=$BUILD_VERSION
LABEL org.label-schema.description="Practical Microservices with Dapr and .NET sample, ReservationActor microservice"

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sample.microservice.reservationactor.service.dll"]