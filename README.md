================================================================================
          APLICACIONES DISTRIBUIDAS - ARQUITECTURA DE MICROSERVICIOS
================================================================================

Guía completa y estructurada del proyecto de gestión de Vehículos y Categorías.[cite: 1]
Diseñado para la comunicación asíncrona mediante mensajería por eventos.[cite: 1]

--------------------------------------------------------------------------------
1. TECNOLOGÍAS UTILIZADAS
--------------------------------------------------------------------------------

* ASP.NET Core Web API: Framework de backend para construir los microservicios.[cite: 1]
* RabbitMQ: Broker de eventos para la comunicación asíncrona mediante mensajes.[cite: 1]
* Docker & Docker Compose: Orquestación e independización de contenedores.[cite: 1]
* API Gateway (YARP): Punto único de entrada y enrutador principal hacia las APIs.[cite: 1]
* SQL Server 2022: Base de datos relacional para la persistencia de datos.[cite: 1]
* Swagger (OpenAPI): Documentación e interfaz interactiva para probar los endpoints.[cite: 1]


--------------------------------------------------------------------------------
2. ESTRUCTURA DEL PROYECTO
--------------------------------------------------------------------------------

/
├── ApiGateway_Vehiculos/       # Reverse Proxy principal (YARP)[cite: 1]
├── Microservicio_Categoria/    # Servicio de Categorías (Publisher de eventos)[cite: 1]
├── Microservicio_Vehiculo/     # Servicio de Vehículos (Consumer de eventos)[cite: 1]
├── BaseDatos/                  # Scripts SQL de creación e inicialización[cite: 1]
├── docker-compose.yml          # Orquestación de infraestructura y servicios[cite: 1]
└── README.txt                  # Documentación oficial del repositorio[cite: 1]


--------------------------------------------------------------------------------
3. SERVICIOS Y PUERTOS EXPUESTOS (URLs DE ACCESO)
--------------------------------------------------------------------------------

Para probar el ecosistema en ejecución desde tu navegador web o cliente REST:[cite: 1]

* API Gateway (Enrutador Principal):
  http://localhost:5000[cite: 1]

* Microservicio de Categorías (Swagger UI):
  http://localhost:5001[cite: 1]

* Microservicio de Vehículos (Swagger UI):
  http://localhost:5002[cite: 1]

* Panel de Administración de RabbitMQ:
  http://localhost:15672[cite: 1]
  Credenciales por defecto -> Usuario: guest | Contraseña: guest[cite: 1]

* Motor SQL Server (Acceso Externo):
  localhost,1433[cite: 1]


--------------------------------------------------------------------------------
4. ESTADO Y MAPEO DE CONTENEDORES (DOCKER DESKTOP)
--------------------------------------------------------------------------------

Nombre del Contenedor          Imagen                             Puertos (Host:Container)
----------------------------------------------------------------------------------------
apigateway-1                   apigatewayvehiculos                5000:8080[cite: 1]
microservicio.categoria-1      microserviciocategoria             5001:8080[cite: 1]
microservicio.vehiculo-1       microserviciovehiculo              5002:8080[cite: 1]
rabbitmq_server                rabbitmq:3-management              5672:5672, 15672:15672[cite: 1]
sql_server_db                  mssql/server:2022-latest           1433:1433[cite: 1]


--------------------------------------------------------------------------------
5. CONFIGURACIÓN DE BASE DE DATOS
--------------------------------------------------------------------------------

Ubicación de scripts: BaseDatos/[cite: 1]

PASO PREVIO OBLIGATORIO:[cite: 1]
Antes de levantar los contenedores, debes ejecutar los archivos .sql en tu[cite: 1]
instancia de SQL Server en el siguiente orden:[cite: 1]

1. CategoriaDB.sql (Base de Datos para el Microservicio de Categorías)[cite: 1]
   * Crea la base de datos CategoriaDB.[cite: 1]
   * Crea el login a nivel de servidor y el usuario Categoria_V con contraseña 'Skrillex1.'.[cite: 1]
   * Asigna permisos de lectura y escritura (db_owner).[cite: 1]
   * Crea la tabla Categorias e inserta el historial de migraciones.[cite: 1]

2. VehiculoDB.sql (Base de Datos para el Microservicio de Vehículos)[cite: 1]
   * Crea la base de datos VehiculoDB.[cite: 1]
   * Crea el login a nivel de servidor y el usuario Vehiculos con contraseña 'Skrillex1.'.[cite: 1]
   * Asigna permisos de lectura y escritura (db_owner).[cite: 1]
   * Crea la tabla Vehiculos e inserta el historial de migraciones.[cite: 1]


--------------------------------------------------------------------------------
6. INSTRUCCIONES PASO A PASO DE EJECUCIÓN
--------------------------------------------------------------------------------

1. Preparar la Base de Datos:
   Ejecuta los scripts .sql en SQL Server tal como se indica en la Sección 5.[cite: 1]

2. Abrir la Terminal:
   Abre tu consola de comandos (PowerShell, CMD o Bash) en la raíz del proyecto.[cite: 1]

3. Compilar y Levantar la Infraestructura:
   Ejecuta el siguiente comando:

   docker-compose up --build[cite: 1]

4. Verificar la Comunicación:
   - Haz una petición POST en Categorías (http://localhost:5001).[cite: 1]
   - El microservicio guardará el registro y publicará un mensaje en RabbitMQ.[cite: 1]
   - El microservicio de Vehículos (Consumer) escuchará automáticamente el[cite: 1]
     evento de la cola 'categoria-queue' y lo procesará en tiempo real.[cite: 1]
================================================================================