Markdown# DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos

Guía completa y estructurada del ecosistema de microservicios para la gestión de Vehículos, Categorías y Autenticación con OAuth JWT, desplegado en Microsoft Azure e interconectado mediante comunicación asíncrona por eventos con RabbitMQ.

---

## 1. Tecnologías Utilizadas

* **.NET 8 (ASP.NET Core Web API):** Framework backend para la construcción de los microservicios.
* **Autenticación OAuth / JWT:** Protección de endpoints mediante tokens de acceso firmados y roles de usuario.
* **RabbitMQ:** Broker de mensajería para la comunicación asíncrona basada en eventos.
* **API Gateway (YARP):** Enrutador centralizado y punto único de entrada a los microservicios.
* **Docker & Docker Compose:** Contenedorización e infraestructura como código.
* **SQL Server 2022:** Base de datos relacional alojada en contenedor con persistencia mediante volúmenes de Docker.
* **Microsoft Azure:** Máquina Virtual (Linux VM) como entorno de despliegue en la nube.

---

## 2. Estructura del Proyecto

```text
/
├── ApiGateway_Vehiculos/       # Reverse Proxy principal (YARP / Gateway)
├── Microservicio_Auth/         # Servicio de Autenticación, Usuarios y Tokens JWT
├── Microservicio_Categoria/    # Servicio de Categorías (Publisher de eventos)
├── Microservicio_Vehiculo/     # Servicio de Vehículos (Consumer de eventos)
├── docker-compose.yml          # Orquestación de infraestructura y contenedores
└── README.md                   # Documentación oficial del repositorio
3. Servicios y Puertos Expuestos (Azure / Local)Para acceder a los servicios desplegados en Azure, reemplaza <IP_AZURE> por la dirección IP pública de la Máquina Virtual (ejemplo: 20.9.128.203):API Gateway (Enrutador Principal): http://<IP_AZURE>:5000Microservicio Auth (Swagger UI): http://<IP_AZURE>:5003Microservicio Categorías (Swagger UI): http://<IP_AZURE>:5001Microservicio Vehículos (Swagger UI): http://<IP_AZURE>:5002Panel de Administración RabbitMQ: http://<IP_AZURE>:15672 (Usuario: admin | Contraseña: admin)Motor SQL Server (SSMS): <IP_AZURE>,1433 (Usuario: sa | Contraseña: Skrillex1.)4. Estado y Mapeo de ContenedoresNombre del ContenedorImagenPuertos (Host:Container)Red Dockerapigateway-1apigatewayvehiculos5000:8080red-microserviciosmicroservicio.auth-1microservicioauth5003:8080red-microserviciosmicroservicio.categoria-1microserviciocategoria5001:8080red-microserviciosmicroservicio.vehiculo-1microserviciovehiculo5002:8080red-microserviciosrabbitmqrabbitmq:3-management5672:5672, 15672:15672red-microserviciossql_server_dbmcr.microsoft.com/mssql/server:2022-latest1433:1433red-microservicios5. Base de Datos y PersistenciaEl contenedor de SQL Server gestiona automáticamente tres bases de datos independientes mediante Entity Framework Core:AuthDB: Almacenamiento de usuarios, credenciales y asignación de roles.CategoriaDB: Gestión del catálogo de categorías.VehiculoDB: Registro y detalle técnico de vehículos.Persistencia: Toda la información se conserva de forma persistente a través del volumen prueba-deber5_mssqldata asociado al contenedor sql_server_db.6. Instrucciones Paso a Paso de Despliegue en AzureClonar el Repositorio en la VM:Bashgit clone [https://github.com/Jordanjr1/DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos.git](https://github.com/Jordanjr1/DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos.git)
cd DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos
Configurar Reglas de Red en Azure Portal (NSG):Habilite el tráfico de entrada para los siguientes puertos TCP:5000 (API Gateway)1433 (SQL Server)15672 (RabbitMQ Management)Compilar y Levantar la Infraestructura:Bashdocker compose up -d
Verificación del Funcionamiento:Autenticación: Envia una petición POST al endpoint de Login en :5003 (o a través del Gateway en :5000) para obtener el Token JWT.Consumo por Gateway: Accede a endpoints protegidos enviando la cabecera Authorization: Bearer <TOKEN>.Comunicación Asíncrona: Crea o actualiza una categoría desde :5001. El Microservicio de Categorías publicará el evento en RabbitMQ y el Microservicio de Vehículos (:5002) lo procesará en tiempo real.Auditoría en SSMS: Conéctate desde tu máquina local mediante SQL Server Management Studio hacia <IP_AZURE>,1433 utilizando el login sa.
