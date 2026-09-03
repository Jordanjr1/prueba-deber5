# Aplicaciones Distribuidas - Arquitectura de Microservicios

Guía completa y estructurada del proyecto de gestión de Vehículos y Categorías, diseñado para la comunicación asíncrona mediante mensajería por eventos.

---

## 1. Tecnologías Utilizadas

* **ASP.NET Core Web API:** Framework de backend para construir los microservicios.
* **RabbitMQ:** Broker de eventos para la comunicación asíncrona mediante mensajes.
* **Docker & Docker Compose:** Orquestación e independización de contenedores.
* **API Gateway (YARP):** Punto único de entrada y enrutador principal hacia las APIs.
* **SQL Server 2022:** Base de datos relacional para la persistencia de datos.
* **Swagger (OpenAPI):** Documentación e interfaz interactiva para probar los endpoints.

---

## 2. Estructura del Proyecto

```text
/
├── ApiGateway_Vehiculos/       # Reverse Proxy principal (YARP)
├── Microservicio_Categoria/    # Servicio de Categorías (Publisher de eventos)
├── Microservicio_Vehiculo/     # Servicio de Vehículos (Consumer de eventos)
├── BaseDatos/                  # Scripts SQL de creación e inicialización
├── docker-compose.yml          # Orquestación de infraestructura y servicios
└── README.md                   # Documentación oficial del repositorio
```

---

## 3. Servicios y Puertos Expuestos (URLs de Acceso)

Para probar el ecosistema en ejecución desde tu navegador web o cliente REST:

* **API Gateway (Enrutador Principal):** `http://localhost:5000`
* **Microservicio de Categorías (Swagger UI):** `http://localhost:5001`
* **Microservicio de Vehículos (Swagger UI):** `http://localhost:5002`
* **Panel de Administración de RabbitMQ:** `http://localhost:15672` (Usuario: `guest` | Contraseña: `guest`)
* **Motor SQL Server (Acceso Externo):** `localhost,1433`

---

## 4. Estado y Mapeo de Contenedores (Docker Desktop)

| Nombre del Contenedor | Imagen | Puertos (Host:Container) |
| :--- | :--- | :--- |
| **apigateway-1** | `apigatewayvehiculos` | `5000:8080` |
| **microservicio.categoria-1** | `microserviciocategoria` | `5001:8080` |
| **microservicio.vehiculo-1** | `microserviciovehiculo` | `5002:8080` |
| **rabbitmq_server** | `rabbitmq:3-management` | `5672:5672`, `15672:15672` |
| **sql_server_db** | `mssql/server:2022-latest` | `1433:1433` |

---

## 5. Configuración de Base de Datos

Ubicación de scripts: `BaseDatos/`

> **PASO PREVIO OBLIGATORIO:**
> Antes de levantar los contenedores, debes ejecutar los archivos `.sql` en tu instancia de SQL Server en el siguiente orden:

1. **`CategoriaDB.sql`** (Base de Datos para el Microservicio de Categorías)
   * Crea la base de datos `CategoriaDB`.
   * Crea el login a nivel de servidor y el usuario `Categoria_V` con contraseña `Skrillex1.`.
   * Asigna permisos de lectura y escritura (`db_owner`).
   * Crea la tabla `Categorias` e inserta el historial de migraciones.

2. **`VehiculoDB.sql`** (Base de Datos para el Microservicio de Vehículos)
   * Crea la base de datos `VehiculoDB`.
   * Crea el login a nivel de servidor y el usuario `Vehiculos` con contraseña `Skrillex1.`.
   * Asigna permisos de lectura y escritura (`db_owner`).
   * Crea la tabla `Vehiculos` e inserta el historial de migraciones.

---

## 6. Instrucciones Paso a Paso de Ejecución

1. **Preparar la Base de Datos:**
   Ejecuta los scripts `.sql` en SQL Server tal como se indica en la Sección 5.

2. **Abrir la Terminal:**
   Abre tu consola de comandos en la raíz del proyecto.

3. **Compilar y Levantar la Infraestructura:**
   Ejecuta el siguiente comando:
   ```bash
   docker-compose up --build
   ```

4. **Verificar la Comunicación:**
   * Haz una petición `POST` en Categorías (`http://localhost:5001`).
   * El microservicio guardará el registro y publicará un mensaje en RabbitMQ.
   * El microservicio de Vehículos (*Consumer*) escuchará automáticamente el evento de la cola `categoria-queue` y lo procesará en tiempo real.
