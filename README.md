\# Práctica: Microservicios con .NET, Docker, SQL Server y RabbitMQ



Proyecto de arquitectura de microservicios desarrollado en .NET, enfocado en el manejo de Categorías y Vehículos, utilizando RabbitMQ como broker de eventos, SQL Server como base de datos y YARP como API Gateway.



\---



\## Estructura del Proyecto



```text

├── ApiGateway\_Vehiculos/         # API Gateway con YARP (Puerto 5000)

├── Microservicio\_Categoria/     # Microservicio de Categorías (Puerto 5001) - Publisher

├── Microservicio\_Vehiculo/      # Microservicio de Vehículos (Puerto 5002) - Consumer

├── docker-compose.yml           # Configuración de contenedores y orquestación

└── README.md                    # Documentación del proyecto

