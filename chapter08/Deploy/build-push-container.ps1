# bonus content
# with this file you can build all the project docker images with version and tag
# in addition it pushes the images to the container registry
param (
    [string]$prefix = "davidebedin", 
    [string]$tag = "latest"
    )
$builddate = "2021-04-25"
$buildversion = "1.1"

$container = "sample.microservice.order"
$latest = "{0}/{1}:{2}" -f $prefix, $container, $tag 
$versioned = "{0}/{1}:{2}" -f $prefix, $container, $buildversion
docker build . -f .\sample.microservice.order\Dockerfile -t $latest -t $versioned --build-arg BUILD_DATE=$builddate --build-arg BUILD_VERSION=$buildversion
docker push $versioned
docker push $latest 

$container = "sample.microservice.reservationactor"
$latest = "{0}/{1}:{2}" -f $prefix, $container, $tag 
$versioned = "{0}/{1}:{2}" -f $prefix, $container, $buildversion
docker build . -f .\sample.microservice.reservationactor.service\Dockerfile -t $latest -t $versioned --build-arg BUILD_DATE=$builddate --build-arg BUILD_VERSION=$buildversion
docker push $versioned
docker push $latest 

$container = "sample.microservice.reservation"
$latest = "{0}/{1}:{2}" -f $prefix, $container, $tag 
$versioned = "{0}/{1}:{2}" -f $prefix, $container, $buildversion
docker build . -f .\sample.microservice.reservation\Dockerfile -t $latest -t $versioned --build-arg BUILD_DATE=$builddate --build-arg BUILD_VERSION=$buildversion
docker push $versioned
docker push $latest

$container = "sample.microservice.customization"
$latest = "{0}/{1}:{2}" -f $prefix, $container, $tag 
$versioned = "{0}/{1}:{2}" -f $prefix, $container, $buildversion
docker build . -f .\sample.microservice.customization\Dockerfile -t $latest -t $versioned --build-arg BUILD_DATE=$builddate --build-arg BUILD_VERSION=$buildversion
docker push $versioned
docker push $latest

$container = "sample.microservice.shipping"
$latest = "{0}/{1}:{2}" -f $prefix, $container, $tag
$versioned = "{0}/{1}:{2}" -f $prefix, $container, $buildversion
docker build . -f .\sample.microservice.shipping\Dockerfile -t $latest -t $versioned --build-arg BUILD_DATE=$builddate --build-arg BUILD_VERSION=$buildversion
docker push $versioned
docker push $latest 
