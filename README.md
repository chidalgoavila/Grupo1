# Proyecto Final - API de Gestión de Fórmula 1

## 📌 1. Explicación del Proyecto

Este sistema es una **API RESTful** desarrollada con **.NET 9** diseñada para la gestión integral de una escudería de Fórmula 1.

El proyecto resuelve el problema de la administración descentralizada de datos críticos en el deporte motor, permitiendo un control unificado sobre los monoplazas, pilotos y sus patrocinios. Su objetivo principal es ofrecer una interfaz segura, rápida y escalable para consultar y manipular estos datos, garantizando la integridad de las relaciones complejas entre las entidades.

### Arquitectura
El sistema sigue una **Arquitectura por Capas (Layered Architecture)** utilizando el patrón **Repository**, lo que asegura un código limpio, mantenible y desacoplado:

* **Controllers:** Manejan las peticiones HTTP.
* **Services:** Contienen la lógica de negocio y validaciones.
* **Repositories:** Se encargan del acceso directo a datos mediante Entity Framework Core.
* **Data/Models:** Definición de entidades y contexto de base de datos.

---

## 🧩 2. Funcionalidades del Sistema

* **Gestión de Usuarios (Auth):** Registro e inicio de sesión seguro con encriptación de contraseñas y generación de tokens de acceso.
* **Gestión de Monoplazas (TeamCars):** CRUD completo para registrar los autos de la temporada (modelo, motor, equipo).
* **Gestión de Pilotos (Drivers):** Administración de corredores, asignándoles automáticamente su auto y patrocinador principal.
* **Gestión de Patrocinadores (Sponsors):** Control de las marcas que financian al equipo y sus montos de aporte.
* **Asignación de Patrocinios (CarSponsors):** Funcionalidad específica para colocar múltiples patrocinadores en distintas partes de un mismo auto (alerones, chasis, etc.).
* **Seguridad Avanzada:** Protección de endpoints sensibles mediante roles de Administrador.
* **Rate Limiting:** Protección contra ataques de fuerza bruta limitando las peticiones por segundo.

---

## 🏛️ 3. Diagrama ER (Entidad-Relación)

El modelo de datos utiliza una base de datos relacional **PostgreSQL**. A continuación, se describen las entidades y sus relaciones:

### Entidades y Atributos

| Tabla | Atributos Principales | Descripción |
| :--- | :--- | :--- |
| **Users** | `Id` (PK), `Username`, `Email`, `PasswordHash`, `Role` | Usuarios del sistema con roles y credenciales encriptadas. |
| **TeamCars** | `Id` (PK), `Model`, `TeamName`, `Engine`, `Year` | Representa los monoplazas de la escudería. |
| **Drivers** | `Id` (PK), `FirstName`, `LastName`, `Number`, `Nationality` | Los pilotos. Contiene FKs hacia TeamCar y Sponsor. |
| **Sponsors** | `Id` (PK), `Name`, `Industry`, `Amount` | Marcas patrocinadoras. |
| **CarSponsors** | `Id` (PK), `TeamCarId` (FK), `SponsorId` (FK), `Location` | Tabla intermedia con atributo extra (`Location`) para definir dónde va el logo en el auto. |

### Relaciones del Modelo

1.  **1 a 1 (Driver ↔ TeamCar):**
    * Un piloto tiene asignado un único auto específico para la temporada.
    * *Implementación:* `Driver` tiene la clave foránea `TeamCarId`.

2.  **1 a Muchos (Sponsor ↔ Driver):**
    * Un patrocinador principal puede gestionar a varios pilotos, pero un piloto tiene un solo sponsor principal directo en este modelo.
    * *Implementación:* `Driver` tiene la clave foránea `SponsorId`.

3.  **Muchos a Muchos (TeamCar ↔ Sponsor):**
    * Un auto tiene muchos patrocinadores (calcomanías) y un patrocinador puede estar en varios autos.
    * *Implementación:* Se utiliza la tabla intermedia `CarSponsor` que vincula `TeamCarId` y `SponsorId`.

---

## 🔐 4. Autenticación, Autorización y Roles

El sistema utiliza **JWT (JSON Web Tokens)** para asegurar las comunicaciones.

* **Autenticación:** El usuario envía sus credenciales (`email` y `password`) al endpoint de login. Si son correctas, el servidor devuelve un `AccessToken` y un `RefreshToken`.
* **Autorización:** El token debe enviarse en cada petición a endpoints protegidos dentro del header HTTP.

### Roles del Sistema
* **Admin:** Tiene acceso total. Puede crear, editar y eliminar (POST, PUT, DELETE) recursos.
* **User:** Puede registrarse e iniciar sesión, y acceder a rutas de lectura (GET) donde se permita acceso autenticado básico.

### Uso del Token
Para consumir un endpoint protegido, se debe incluir la siguiente cabecera:

```http
Authorization: Bearer <tu_token_jwt_aqui>
````

-----

## 🌐 5. Endpoints + Ejemplos

### 🔐 Auth (Público)

| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `POST` | `/api/v1/Auth/register` | Registrar un nuevo usuario. |
| `POST` | `/api/v1/Auth/login` | Iniciar sesión y obtener token. |
| `POST` | `/api/v1/Auth/refresh` | Renovar token vencido. |

**Ejemplo Login (Request):**

```json
{
  "email": "admin@f1.com",
  "password": "Password123!"
}
```

### 🏎️ TeamCar (Requiere Auth)

| Método | Endpoint | Permiso | Descripción |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/TeamCar` | Público/Auth | Listar todos los autos. |
| `GET` | `/api/TeamCar/{id}` | Auth | Obtener detalle de un auto. |
| `POST` | `/api/TeamCar` | **Admin** | Crear un nuevo auto. |
| `PUT` | `/api/TeamCar/{id}` | **Admin** | Actualizar auto. |
| `DELETE` | `/api/TeamCar/{id}` | **Admin** | Eliminar auto. |

**Ejemplo Create Car (Request):**

```json
{
  "model": "AMR24",
  "teamName": "Aston Martin",
  "engine": "Mercedes",
  "year": 2024
}
```

### 👤 Driver (Requiere Auth)

| Método | Endpoint | Permiso | Descripción |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Driver` | Público/Auth | Listar pilotos. |
| `POST` | `/api/Driver` | **Admin** | Registrar piloto (requiere IDs de Auto y Sponsor). |

### 🏷️ CarSponsor (Relación N:M)

| Método | Endpoint | Permiso | Descripción |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/CarSponsor` | **Admin** | Asignar un patrocinador a un auto. |

**Ejemplo Request:**

```json
{
  "teamCarId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "sponsorId": "8a225f64-1234-4562-b3fc-2c963f66afa6",
  "location": "Alerón Trasero"
}
```

-----

## 📎 6. Swagger

El proyecto incluye documentación interactiva automática generada con Swagger.

  * **URL de Acceso:** `http://localhost:8080/swagger/index.html` (cuando se ejecuta localmente o en Docker).
  * **Uso:** Permite probar todos los endpoints directamente desde el navegador. Incluye un botón **"Authorize"** arriba a la derecha para pegar el token JWT y probar las rutas protegidas.

-----

## ⏱️ 7. TimeGate (Rate Limiting)

El sistema implementa un **TimeGate** (Rate Limiter) configurado en el `Program.cs` para proteger la API contra el abuso y ataques de denegación de servicio.

  * **Configuración:** Ventana fija (`FixedWindow`).
  * **Límite:** Máximo **10 peticiones cada 10 segundos** por cliente.
  * **Respuesta al exceder:** El servidor responderá con un código `429 Too Many Requests`.

-----

## 🛠️ 8. Instalación y Configuración

### Requisitos Previos

  * .NET 9.0 SDK instalado.
  * Docker Desktop instalado y corriendo.
  * Postman (opcional, para pruebas).

### Pasos de Instalación

1.  **Clonar el repositorio:**

    ```bash
    git clone [https://github.com/TU_USUARIO/Proyecto-FInal-Grupo-1.git](https://github.com/TU_USUARIO/Proyecto-FInal-Grupo-1.git)
    cd Proyecto-FInal-Grupo-1
    ```

2.  **Configurar Variables de Entorno:**
    Crea un archivo `.env` en la raíz con el siguiente contenido (basado en `docker-compose.yml`):

    ```properties
    POSTGRES_DB=formula1db
    POSTGRES_USER=postgres
    POSTGRES_PASSWORD=supersecret
    JWT_KEY=EstaEsUnaClaveSuperSecretaYLoSuficientementeLargaParaQueFuncioneElJWT123!
    JWT_ISSUER=FormulaOneApi
    JWT_AUDIENCE=FormulaOneClient
    ```

3.  **Levantar Infraestructura (Docker):**
    Esto iniciará la base de datos PostgreSQL automáticamente.

    ```bash
    docker-compose up -d
    ```

4.  **Ejecutar Migraciones (Crear Tablas):**

    ```bash
    dotnet tool install --global dotnet-ef
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

5.  **Ejecutar la API:**

    ```bash
    dotnet run
    ```

    La API estará disponible en `http://localhost:5030` (o el puerto indicado en la consola).

-----

## 📦 9. Datos de Prueba

Para probar el sistema, utiliza las siguientes credenciales de ejemplo:

| Cuenta | Email | Password | Role |
| :--- | :--- | :--- | :--- |
| **Administrador** | `admin@f1.com` | `Password123!` | Admin |
| **Usuario** | `user@f1.com` | `UserPass123!` | User |

-----

## 🧪 10. Pruebas con Postman

Se recomienda seguir este orden para probar la funcionalidad completa:

1.  **Carpeta Auth:** Ejecuta `Register` para crear el admin y luego `Login`.
2.  **Configuración:** Copia el `token` recibido en el Login. En Postman, ve a la pestaña **Authorization**, selecciona **Bearer Token** y pega el token.
3.  **Carpeta TeamCar:** Crea un auto (POST). Copia su `id`.
4.  **Carpeta Sponsor:** Crea un patrocinador (POST). Copia su `id`.
5.  **Carpeta Driver:** Crea un piloto (POST) usando los IDs del auto y sponsor creados anteriormente.
6.  **Carpeta CarSponsor:** Asigna el sponsor al auto usando el endpoint POST.
7.  **Validación:** Usa los endpoints GET de cada carpeta para verificar que los datos se guardaron y relacionaron correctamente.


```
```
