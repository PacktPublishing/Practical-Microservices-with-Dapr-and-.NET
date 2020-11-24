FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["sample.microservice.common/sample.microservice.common.csproj", "sample.microservice.common/"]
COPY ["sample.microservice.dto.shipping/sample.microservice.dto.shipping.csproj", "sample.microservice.dto.shipping/"]
COPY ["sample.microservice.shipping/sample.microservice.shipping.csproj", "sample.microservice.shipping/"]
RUN dotnet restore "sample.microservice.common/sample.microservice.common.csproj"
RUN dotnet restore "sample.microservice.dto.shipping/sample.microservice.dto.shipping.csproj"
RUN dotnet restore "sample.microservice.shipping/sample.microservice.shipping.csproj"
COPY . .
WORKDIR "/src/sample.microservice.shipping"
RUN dotnet publish "sample.microservice.shipping.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sample.microservice.shipping.dll"]
