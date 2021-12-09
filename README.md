# Practical Microservices with Dapr and .NET

<a href="https://www.packtpub.com/web-development/practical-microservices-with-dapr-and-net?utm_source=github&utm_medium=repository&utm_campaign=9781800568372"><img src="https://static.packt-cdn.com/products/9781800568372/cover/smaller" alt="Practical Microservices with Dapr and .NET" height="256px" align="right"></a>

This is the code repository for [Practical Microservices with Dapr and .NET](https://www.packtpub.com/web-development/practical-microservices-with-dapr-and-net?utm_source=github&utm_medium=repository&utm_campaign=9781800568372), published by Packt.

**A developer's guide to build cloud native applications using the Dapr event-driven runtime**

## What is this book about?
Over the last decade, there has been a huge shift from heavily coded monolithic applications to finer, self-contained microservices. Dapr is a new, open source project by Microsoft that provides proven techniques and best practices for developing modern applications. It offers platform-agnostic features for running your applications on public cloud, on-premises, and even on edge devices. 

* This book covers the following exciting features:
* Use Dapr to create services, invoking them directly and via pub/sub
* Discover best practices for working with microservice architectures
* Leverage the actor model to orchestrate data and behavior
* Use Azure Kubernetes Service to deploy a sample application
* Monitor Dapr applications using Zipkin, Prometheus, and Grafana
* Scale and load test Dapr applications on Kubernetes


If you feel this book is for you, get your [copy](https://www.amazon.com/dp/1800568371) today!

<a href="https://www.packtpub.com/?utm_source=github&utm_medium=banner&utm_campaign=GitHubBanner"><img src="https://raw.githubusercontent.com/PacktPublishing/GitHub/master/GitHub.png" 
alt="https://www.packtpub.com/" border="5" /></a>

## Errata

* page 182: The file path in the sentence "The corresponding ingress for order-service, which is available in the ```Deploy\
ingress-order.yaml``` file, is as follows:" must be read as "The corresponding ingress for order-service, which is available in the ``Deploy\
ingress-nginx.yaml``` file, is as follows:"
* page 182: The command after the sentence "To apply these configurations to our Kubernetes environment, we must use the following
command:" must be read as 
  ```
  kubectl apply -f .\Deploy\ingress-nginx.yaml
  ```

## Instructions and Navigations
All of the code is organized into folders. For example, Chapter02.

The code will look like the following:
```
"compounds":
[
  {
    "name": "webApi + webApi2 w/Dapr",
    "configurations": [".NET Launch w/Dapr (webapi)",
    ".NET Core Launch w/Dapr (webapi2)"]
  }
]
```

**Following is what you need for this book:**
This book is for developers looking to explore microservices architectures and implement them in Dapr applications using examples on Microsoft .NET Core. Whether you are new to microservices or have knowledge of this architectural approach and want to get hands-on experience in using Dapr, you’ll find this book useful. Familiarity with .NET Core will help you to understand the C# samples and code snippets used in the book.

With the following software and hardware list you can run all code files present in the book (Chapter 1-10).
### Software and Hardware List
| Chapter | Software required | OS required |
| -------- | ------------------------------------ | ----------------------------------- |
| 1-10 | Docker Engine – the latest version  | Windows, Mac OS X, and Linux (Any) |
| 1-10 | .NET 5  | Windows, Mac OS X, and Linux (Any) |
| 1-10 | Dapr, version 1 or later  | Windows, Mac OS X, and Linux (Any) |
| 1-10 | VS Code – the latest version  | Windows, Mac OS X, and Linux (Any) |
| 10 | Python 3.8  | Windows, Mac OS X, and Linux (Any) |
| 1-10 | The Azure CLI – 2.15.1 or later  | Windows, Mac OS X, and Linux (Any) |
| 10 | Locust 1.3.1 or later  | Windows, Mac OS X, and Linux (Any) |

We also provide a PDF file that has color images of the screenshots/diagrams used in this book. [Click here to download it](https://static.packt-cdn.com/downloads/9781800568372_ColorImages.pdf).

### Related products
* C# 9 and .NET 5 – Modern Cross-Platform Development - Fifth Edition [[Packt]](https://www.packtpub.com/product/c-9-and-net-5-modern-cross-platform-development-fifth-edition/9781800568105?utm_source=github&utm_medium=repository&utm_campaign=9781800568105) [[Amazon]](https://www.amazon.com/dp/180056810X)

* Learn C# Programming [[Packt]](packtpub.com/product/learn-c-programming/9781789805864?utm_source=github&utm_medium=repository&utm_campaign=9781789805864) [[Amazon]](https://www.amazon.com/dp/1789805864)

## Get to Know the Author
**Davide Bedin**
is a cloud-native architecture enthusiast, with strong and relevant experience with cloud platforms.
As CTO of an ISV, Davide led its significant transformational process with the objective of creating new solutions based on the Microsoft Azure cloud.
Davide particularly focused on the evolution of distributed computing to service-oriented architectures, and ultimately microservices, spending most of his developer career creating web services.
As a cloud solution architect at Microsoft, Davide is responsible for the guidance and support of enterprise customers in embracing the cloud paradigm, a key enabler of their digital transformation; lately, he also plays with Dapr.

