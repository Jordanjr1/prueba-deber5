
## 5. Base de Datos y Persistencia

El contenedor de SQL Server gestiona automáticamente tres bases de datos independientes mediante Entity Framework Core:

1. **`AuthDB`**: Almacenamiento de usuarios, credenciales y asignación de roles.
2. **`CategoriaDB`**: Gestión del catálogo de categorías.
3. **`VehiculoDB`**: Registro y detalle técnico de vehículos.

> **Persistencia:** Toda la información se conserva de forma persistente a través del volumen `prueba-deber5_mssqldata` asociado al contenedor `sql_server_db`.

---

## 6. Instrucciones Paso a Paso de Despliegue en Azure

1. **Clonar el Repositorio en la VM:**
   ```bash
   git clone https://github.com/Jordanjr1/DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos.git
   cd DIST-4AM-B3-AA-DockerCompose-OAuthJWT-AZURE-Jordan-Ramos
   ```

2. **Configurar Reglas de Red en Azure Portal (NSG):**
   Habilite el tráfico de entrada para los siguientes puertos TCP:
   * `5000` (API Gateway)
   * `1433` (SQL Server)
   * `15672` (RabbitMQ Management)

3. **Compilar y Levantar la Infraestructura:**
   ```bash
   docker compose up -d
   ```

4. **Verificación del Funcionamiento:**
   * **Autenticación:** Envía una petición `POST` al endpoint de Login en `:5003` (o a través del Gateway en `:5000`) para obtener el Token JWT.
   * **Consumo por Gateway:** Accede a endpoints protegidos enviando la cabecera `Authorization: Bearer <TOKEN>`.
   * **Comunicación Asíncrona:** Crea o actualiza una categoría desde `:5001`. El Microservicio de Categorías publicará el evento en RabbitMQ y el Microservicio de Vehículos (`:5002`) lo procesará en tiempo real.
   * **Auditoría en SSMS:** Conéctate desde tu máquina local mediante SQL Server Management Studio hacia `<IP_AZURE>,1433` utilizando el login `sa`.
