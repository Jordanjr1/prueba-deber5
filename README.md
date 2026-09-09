# DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos

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

```
/
├── ApiGateway_Vehiculos/       # Reverse Proxy principal (YARP / Gateway)
├── Microservicio_Auth/         # Servicio de Autenticación, Usuarios y Tokens JWT
├── Microservicio_Categoria/    # Servicio de Categorías (Publisher de eventos)
├── Microservicio_Vehiculo/     # Servicio de Vehículos (Consumer de eventos)
├── BaseDatos/                  # Scripts SQL de creación e inicialización de BDs
├── CLAVES_AZURE_EJEMPLO.txt    # Plantilla con estructura de credenciales (placeholders)
├── MEMORIA_COMANDOS_AZURE.txt  # Secuencia ordenada de comandos ejecutados en Azure CLI
├── docker-compose.yml          # Orquestación de infraestructura y contenedores
└── README.md                   # Documentación oficial del repositorio
```

---

## 3. Servicios y Puertos Expuestos (Azure / Local)

Para acceder a los servicios desplegados en la IP pública de la Máquina Virtual (`20.9.128.203`):

* **API Gateway (Enrutador Principal):** http://20.9.128.203:5000
* **Microservicio Auth (Swagger UI):** http://20.9.128.203:5003
* **Microservicio Categorías (Swagger UI):** http://20.9.128.203:5001
* **Microservicio Vehículos (Swagger UI):** http://20.9.128.203:5002
* **Panel de Administración RabbitMQ:** http://20.9.128.203:15672 (Usuario: admin | Contraseña: admin)
* **Motor SQL Server (SSMS):** `20.9.128.203,1433` (Usuario: sa | Contraseña: Skrillex1.)

---

## 4. Estado y Mapeo de Contenedores

| Nombre del Contenedor       | Imagen                                   | Puertos (Host:Container)      | Red Docker          |
|------------------------------|-------------------------------------------|--------------------------------|----------------------|
| apigateway-1                 | apigatewayvehiculos                       | 5000:8080                      | red-microservicios  |
| microservicio.auth-1         | microservicioauth                         | 5003:8080                      | red-microservicios  |
| microservicio.categoria-1    | microserviciocategoria                    | 5001:8080                      | red-microservicios  |
| microservicio.vehiculo-1     | microserviciovehiculo                     | 5002:8080                      | red-microservicios  |
| rabbitmq                     | rabbitmq:3-management                     | 5672:5672, 15672:15672         | red-microservicios  |
| sql_server_db                | mcr.microsoft.com/mssql/server:2022-latest| 1433:1433                      | red-microservicios  |

---

## 5. Configuración de Base de Datos y Persistencia

**Ubicación de scripts SQL:** `BaseDatos/`

El contenedor de SQL Server gestiona tres bases de datos relacionales independientes:

### `AuthDB.sql` (Base de Datos de Autenticación)
* Crea la base de datos `AuthDB`.
* Almacena usuarios, credenciales y asignación de roles para la generación de tokens JWT.

### `CategoriaDB.sql` (Base de Datos para el Microservicio de Categorías)
* Crea la base de datos `CategoriaDB`.
* Crea el login a nivel de servidor y el usuario `Categoria_V` con contraseña `Skrillex1.`.
* Asigna permisos de lectura y escritura (`db_owner`).
* Crea la tabla `Categorias` e inserta los registros iniciales.

### `VehiculoDB.sql` (Base de Datos para el Microservicio de Vehículos)
* Crea la base de datos `VehiculoDB`.
* Crea el login a nivel de servidor y el usuario `Vehiculos` con contraseña `Skrillex1.`.
* Asigna permisos de lectura y escritura (`db_owner`).
* Crea la tabla `Vehiculos` e inserta los registros iniciales.

**Persistencia:** Toda la información se conserva de forma persistente a través del volumen `prueba-deber5_mssqldata` asociado al contenedor `sql_server_db`.

---

## 6. Archivos de Soporte y Seguridad en Azure

### `CLAVES_AZURE_EJEMPLO.txt`

Contiene la plantilla estándar con la estructura de variables y credenciales requeridas por la aplicación (placeholders sin contraseñas reales):

* Estructura de cadenas de conexión a bases de datos (AuthDB, CategoriaDB, VehiculoDB).
* Configuración de la clave secreta JWT (`Jwt:SecretKey`).
* Credenciales de acceso a RabbitMQ y SQL Server.

> **Nota:** Por seguridad, las credenciales reales se gestionan mediante variables de entorno en Azure y se mantienen fuera del repositorio público.

### `MEMORIA_COMANDOS_AZURE.txt`

Documento de trazabilidad que registra en orden cronológico los comandos ejecutados en Azure CLI durante el despliegue:

1. **Inicio de Sesión y Verificación:** `az login`, `az account show`
2. **Resource Group:** `az group create`
3. **Redes y Reglas NSG:** `az network nsg rule create` (Apertura de puertos 5000, 1433, 15672)
4. **Instancia de VM y Docker:** Provisionamiento de la Máquina Virtual Ubuntu en Azure.
5. **Despliegue e Inspección:** `docker compose up -d`, `docker compose ps`, `docker compose logs`
6. **Eliminación de Recursos:** Comandos para detener servicios y eliminar el Resource Group.

---

## 7. Instrucciones Paso a Paso de Despliegue en Azure

### Clonar el Repositorio en la VM:

```bash
git clone https://github.com/Jordanjr1/DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos.git
cd DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos
```

### Configurar Reglas de Red en Azure Portal (NSG):

Habilite el tráfico de entrada para los siguientes puertos TCP:

* 5000 (API Gateway)
* 1433 (SQL Server)
* 15672 (RabbitMQ Management)

### Compilar y Levantar la Infraestructura:

```bash
docker compose up -d
```

### Verificación del Funcionamiento:

* **Autenticación:** Envía una petición POST al endpoint de Login en :5003 (o a través del Gateway en :5000) para obtener el Token JWT.
* **Consumo por Gateway:** Accede a endpoints protegidos enviando la cabecera `Authorization: Bearer <TOKEN>`.
* **Comunicación Asíncrona:** Crea o actualiza una categoría desde :5001. El Microservicio de Categorías publicará el evento en RabbitMQ y el Microservicio de Vehículos (:5002) lo procesará en tiempo real.
* **Auditoría en SSMS:** Conéctate desde tu máquina local mediante SQL Server Management Studio hacia `20.9.128.203,1433` utilizando el login `sa`.
