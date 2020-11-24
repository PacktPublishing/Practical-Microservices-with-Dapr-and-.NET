FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
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

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sample.microservice.reservation.dll"]
