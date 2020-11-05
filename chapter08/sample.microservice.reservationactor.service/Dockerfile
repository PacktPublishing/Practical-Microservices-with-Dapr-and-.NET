FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["sample.microservice.reservationactor.interfaces/sample.microservice.reservationactor.interfaces.csproj", "sample.microservice.reservationactor.interfaces/"]
COPY ["sample.microservice.reservationactor.service/sample.microservice.reservationactor.service.csproj", "sample.microservice.reservationactor.service/"]
RUN dotnet restore "sample.microservice.reservationactor.interfaces/sample.microservice.reservationactor.interfaces.csproj"
RUN dotnet restore "sample.microservice.reservationactor.service/sample.microservice.reservationactor.service.csproj"
COPY . .
WORKDIR "/src/sample.microservice.reservationactor.service"
RUN dotnet publish "sample.microservice.reservationactor.service.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sample.microservice.reservationactor.service.dll"]