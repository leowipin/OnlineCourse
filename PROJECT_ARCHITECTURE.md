# Resumen de Arquitectura y Estructura del Proyecto: OnlineCourse

Este proyecto ASP.NET Core (.NET 9) sigue una arquitectura por capas (Controller, Service, Repository, Unit of Work) utilizando Entity Framework Core y ASP.NET Core Identity para la gestión de usuarios y autenticación JWT. La API se documenta y prueba con Swagger.

## Estructura de Directorios Principal

*   **`/` (Raíz)**
    *   `OnlineCourse.csproj`
    *   `Program.cs` (Configuración de servicios, middleware, autenticación/autorización JWT, Swagger)
    *   `ApplicationDbContext.cs` (Contexto EF Core, DbSet, OnModelCreating, auditoría, soft deletes)
    *   `appsettings.json` (Configuraciones, ConnectionStrings, JwtSettings)

*   **`/Controllers`**: Endpoints de la API (ej. `InstructorController.cs`, `AuthController.cs`).
*   **`/Services`**: Lógica de negocio.
    *   `InstructorService.cs`
    *   **`/IServices`**: Interfaces (`IInstructorService.cs`).
*   **`/Repositories`**: Acceso a datos.
    *   `InstructorRepository.cs`
    *   **`/IRepositories`**: Interfaces (`IInstructorRepository.cs`).
*   **`/UnitOfWork`**: Patrón Unit of Work.
    *   `UnitOfWork.cs`, `IUnitOfWork.cs`.
*   **`/Entities`**: Modelos de datos (POCOs).
    *   `UserEntity.cs` (Identity), `InstructorEntity.cs`, `StudentEntity.cs`, etc.
    *   **`/Base`**: `AuditableEntity.cs`, `IAuditableEntity.cs`.
*   **`/Dtos`**: Data Transfer Objects (ej. `InstructorDto.cs`).
*   **`/Mappings`**: Perfiles de AutoMapper (ej. `InstructorMapping.cs`).
*   **`/Data/Constants`**: Constantes.
    *   `AppRoles.cs` (Nombres e IDs de roles: Instructor, Student, Admin).
    *   `AppPermissions.cs` (Strings de permisos).
    *   `AppClaimTypes.cs` (Tipos de claims, ej. "Permission").
*   **`/Configurations`**: Configuraciones Fluent API de EF Core y *seeding*.
    *   `IdentityRoleConfig.cs` (Seeding de roles base).
    *   `RoleClaimConfig.cs` (Seeding de permisos a roles).
*   **`/Primitives`**: Tipos utilitarios.
    *   `Result.cs` (Implementación del Result Pattern para servicios).
    *   `Error.cs` y sus derivados (Para manejo de errores).
*   **`/Middleware`**: Middlewares personalizados (ej. `GlobalExceptionHandlingMiddleware.cs`).
*   **`/Extensions`**: Métodos de extensión.
    *   **`/Logging`**: Extensiones de logging.
*   **`/Web`**: (Potencialmente extensiones o helpers para la capa web).
    *   `ControllerErrorHandlingExtensions.cs` (Contiene `HandleServiceError`).
*   **`/Migrations`**: Migraciones de EF Core.

## Conceptos Clave de la Arquitectura

*   **Capas:** Controller -> Service -> Repository/UoW.
*   **Identity:** `UserEntity.cs` (`IdentityUser<Guid>`), uso de `UserManager`/`SignInManager`.
*   **Roles y Permisos:** Definidos en `/Data/Constants`, sembrados en `/Configurations` como claims.
*   **Autenticación JWT:** Configurada en `Program.cs`/`appsettings.json`, generación en `AuthService.cs`.
*   **Result Pattern para Servicios:**
    *   Los servicios devuelven `Result<T>` (definido en `/Primitives/Result.cs`) para encapsular éxito (`IsSuccess=true`, `Data`) o fallo (`IsSuccess=false`, `Error`).
    *   `Result.Success(T data)` y `Result.Failure(Error error)` son los factories.
*   **Manejo de Errores:** `GlobalExceptionHandlingMiddleware` y `HandleServiceError` (en `/Web`) para convertir `Result.Error` en respuestas HTTP.
*   **Auditoría y Soft Delete:** Centralizado en `ApplicationDbContext.cs`.
*   **Documentación API (Swagger):**
    *   Configurado en `Program.cs`.
    *   Las acciones de controlador deben incluir comentarios `/// <summary>...</summary>` y atributos `[ProducesResponseType<...>]` para cada posible resultado HTTP.

## Convenciones de Código C# (.NET 9 / C# 12+)

*   **Propiedades `required`:** Preferidas sobre `[Required]` en DTOs/Entidades.
    *   Ej: `public required string Name { get; set; }`
*   **Constructores Primarios:** Preferidos para clases (servicios, repositorios, DTOs, entidades).
    *   Ej: `public class MiServicio(IDependencia dep) { ... }`
*   **Controladores API:**
    *   Con `[ApiController]`, la validación del modelo es automática. Evitar `if (!ModelState.IsValid)` explícito.
    *   Utilizar `/// <summary>` y `[ProducesResponseType]` para documentación Swagger.

---