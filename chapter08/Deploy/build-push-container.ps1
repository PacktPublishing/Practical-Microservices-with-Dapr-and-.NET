param (
    [string]$prefix = "davidebedin", 
    [string]$tag = "latest"
    )
$prefix = -join($prefix, "/")     
$tag = -join(":", $tag)

$order = $prefix + "sample.microservice.order" + $tag
docker build . -f .\sample.microservice.order\Dockerfile -t $order
docker push $order

$reservationactor = $prefix + "sample.microservice.reservationactor" + $tag
docker build . -f .\sample.microservice.reservationactor.service\Dockerfile -t $reservationactor
docker push $reservationactor

$reservation = $prefix + "sample.microservice.reservation" + $tag
docker build . -f .\sample.microservice.reservation\Dockerfile -t $reservation
docker push $reservation

$customization = $prefix + "sample.microservice.customization" + $tag
docker build . -f .\sample.microservice.customization\Dockerfile -t $customization
docker push $customization

$shipping = $prefix + "sample.microservice.shipping" + $tag
docker build . -f .\sample.microservice.shipping\Dockerfile -t $shipping
docker push $shipping
