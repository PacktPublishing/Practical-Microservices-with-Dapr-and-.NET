docker build . -f .\sample.microservice.order\Dockerfile -t sample.microservice.order
docker build . -f .\sample.microservice.reservationactor.service\Dockerfile -t sample.microservice.reservationactor
docker build . -f .\sample.microservice.reservation\Dockerfile -t sample.microservice.reservation
docker build . -f .\sample.microservice.customization\Dockerfile -t sample.microservice.customization
docker build . -f .\sample.microservice.shipping\Dockerfile -t sample.microservice.shipping