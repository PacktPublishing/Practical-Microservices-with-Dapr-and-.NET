FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["sample.microservice.common/sample.microservice.common.csproj", "sample.microservice.common/"]
COPY ["sample.microservice.dto.customization/sample.microservice.dto.customization.csproj", "sample.microservice.dto.customization/"]
COPY ["sample.microservice.dto.order/sample.microservice.dto.order.csproj", "sample.microservice.dto.order/"]
COPY ["sample.microservice.dto.reservation/sample.microservice.dto.reservation.csproj", "sample.microservice.dto.reservation/"]
COPY ["sample.microservice.reservationactor.interfaces/sample.microservice.reservationactor.interfaces.csproj", "sample.microservice.reservationactor.interfaces/"]
COPY ["sample.microservice.reservation/sample.microservice.reservation.csproj", "sample.microservice.reservation/"]
RUN dotnet restore "sample.microservice.common/sample.microservice.common.csproj"
RUN dotnet restore "sample.microservice.dto.customization/sample.microservice.dto.customization.csproj"
RUN dotnet restore "sample.microservice.dto.order/sample.microservice.dto.order.csproj"
RUN dotnet restore "sample.microservice.dto.reservation/sample.microservice.dto.reservation.csproj"
RUN dotnet restore "sample.microservice.reservationactor.interfaces\sample.microservice.reservationactor.interfaces.csproj"
RUN dotnet restore "sample.microservice.reservation/sample.microservice.reservation.csproj"
COPY . .
WORKDIR "/src/sample.microservice.reservation"
RUN dotnet publish "sample.microservice.reservation.csproj" -c Release -o /app/publish --no-restore

ARG BUILD_DATE
ARG BUILD_VERSION
LABEL org.label-schema.build-date=$BUILD_DATE
LABEL org.label-schema.version=$BUILD_VERSION
LABEL org.label-schema.description="Practical Microservices with Dapr and .NET sample, Reservation microservice"

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sample.microservice.reservation.dll"]
