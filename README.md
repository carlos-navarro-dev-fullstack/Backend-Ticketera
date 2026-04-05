# Sistema de Gestión de Tickets - Backend API

## Resumen

El **Sistema de Gestión de Tickets** es una aplicación backend robusta desarrollada en ASP.NET Core 9.0 que proporciona una solución completa para la gestión de tickets de soporte técnico. Implementa una arquitectura limpia (Clean Architecture) con separación clara de responsabilidades, autenticación JWT, y una base de datos relacional SQL Server.

## Arquitectura del Sistema

### Clean Architecture Implementation

El proyecto sigue los principios de Clean Architecture con las siguientes capas:

#### **Core Layer** (Dominio)
- **Entidades**: Modelos de negocio puros sin dependencias externas
- **Interfaces**: Contratos de repositorios y servicios

#### **Application Layer** (Casos de Uso)
- **DTOs**: Objetos de Transferencia de Datos para comunicación entre capas
- **Services**: Lógica de negocio y coordinación de operaciones
- **Mappers**: Transformación entre entidades y DTOs

#### **Infrastructure Layer** (Persistencia)
- **Repositories**: Implementación concreta de acceso a datos
- **Data**: Configuración de Entity Framework Core

#### **Presentation Layer** (API)
- **Controllers**: Endpoints RESTful para consumo del cliente
- **Program.cs**: Configuración y startup de la aplicación

## Modelo de Datos

### Entidades Principales

#### **Usuario**
```csharp
- Id: int (PK)
- Nombre: string
- Email: string (único)
- PasswordHash: string (encriptado con BCrypt)
- RolId: int (FK)
- Relaciones: TicketsAsignados, Comentarios
```

#### **Ticket**
```csharp
- Id: int (PK)
- Titulo: string
- Descripcion: string
- FechaCreacion: DateTime
- Estado: EstadoTicket (Enum)
- ResponsableId: int (FK)
- Relaciones: Responsable, Comentarios
```

#### **Comentario**
```csharp
- Id: int (PK)
- Texto: string
- FechaCreacion: DateTime
- TicketId: int (FK)
- UsuarioId: int (FK)
- Relaciones: Ticket, Usuario
```

#### **Rol**
```csharp
- Id: int (PK) // 1=Admin, 2=Usuario
- Nombre: string
```

### Estados del Ticket
- **Nuevo**: Ticket recién creado
- **EnProceso**: Ticket siendo atendido
- **Resuelto**: Ticket completado exitosamente
- **Cancelado**: Ticket cancelado

## Seguridad

### Autenticación y Autorización
- **JWT Bearer Tokens**: Para autenticación stateless
- **Password Hashing**: BCrypt.Net-Next para almacenamiento seguro
- **Roles de Usuario**: Administrador y Usuario estándar

### Configuración de Seguridad
```json
{
  "Jwt": {
    "Key": "clave_super_secreta_123456789ABC",
    "Issuer": "SistemaTickets",
    "Audience": "SistemaTicketsUsers"
  }
}
```

## Funcionalidades Principales

### Gestión de Tickets
- **Crear tickets**: Registro de nuevos incidentes
- **Editar tickets**: Modificación de información existente
- **Eliminar tickets**: Remoción lógica de tickets
- **Cambiar estado**: Transición entre estados del flujo de trabajo
- **Asignar responsable**: Asignación a usuarios específicos
- **Filtrar por estado**: Consultas especializadas

### Gestión de Comentarios
- **Crear comentarios**: Seguimiento de conversaciones
- **Historial temporal**: Registro cronológico de interacciones

### Gestión de Usuarios
- **Registro de usuarios**: Creación de cuentas
- **Autenticación**: Validación de credenciales
- **Gestión de roles**: Asignación de permisos

## Stack Tecnológico

### Core Framework
- **.NET 9.0**: Última versión estable del framework
- **ASP.NET Core**: Framework web para APIs RESTful
- **C# 12**: Lenguaje de programación principal

### Base de Datos
- **SQL Server 2022**: Motor de base de datos relacional
- **Entity Framework Core 9.0**: ORM para acceso a datos
- **Migraciones automáticas**: Control de versiones de esquema

### Seguridad
- **BCrypt.Net-Next 4.1.0**: Hashing de contraseñas
- **JWT Bearer Authentication**: Tokens de autenticación

### Documentación y Testing
- **Swashbuckle.AspNetCore 6.5.0**: Documentación Swagger/OpenAPI

### Contenerización
- **Docker**: Empaquetado y despliegue consistente
- **Docker Compose**: Orquestación de servicios múltiples

## Estructura del Proyecto

```
Backend-Ticketera/
├── Core/                          # Dominio del negocio
│   ├── Entities/                  # Entidades principales
│   └── Interfaces/                 # Contratos de servicios
├── Application/                   # Lógica de aplicación
│   ├── DTOs/                      # Objetos de transferencia
│   ├── Mappers/                   # Transformaciones de datos
│   └── Services/                  # Servicios de negocio
├── Infraestructure/               # Capa de persistencia
│   ├── Data/                      # Contexto de base de datos
│   └── Repositories/              # Implementación de repositorios
├── Presentation/                  # Capa de presentación
│   └── Controllers/               # Controladores API
├── Properties/                    # Configuraciones
├── SistemaTIcketsDB.sql          # Script de base de datos
├── docker-compose.yml            # Orquestación Docker
├── Dockerfile                    # Configuración de imagen
└── appsettings.*.json           # Configuraciones por entorno
```

## Configuración y Despliegue

### Entornos Soportados
- **Development**: Entorno local de desarrollo
- **UAT**: Entorno de pruebas de aceptación de usuario

### Variables de Configuración
```json
{
  "ConnectionStrings": {
    "TicketsConnection": "Server=...;Database=SistemaTicketsDB;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Despliegue con Docker
```bash
# Construir y ejecutar todos los servicios
docker-compose up --build

# Acceso a la API
http://localhost:5000

# Acceso a SQL Server
Server: localhost:1433
Database: SistemaTicketsDB
```

### Despliegue Local
```bash
# Restaurar dependencias
dotnet restore

# Ejecutar aplicación
dotnet run

# Publicar para producción
dotnet publish -c Release -o out
```

## Endpoints Principales

### Autenticación
- `POST /api/usuario/login` - Inicio de sesión
- `POST /api/usuario/register` - Registro de usuarios

### Tickets
- `GET /api/ticket` - Listar tickets
- `POST /api/ticket` - Crear ticket
- `PUT /api/ticket/{id}` - Actualizar ticket
- `DELETE /api/ticket/{id}` - Eliminar ticket

### Comentarios
- `GET /api/comentario` - Listar comentarios
- `POST /api/comentario` - Crear comentario

### Usuarios y Roles
- `GET /api/usuario` - Listar usuarios
- `GET /api/rol` - Listar roles

## Decisiones Técnicas

### Clean Architecture
- **Separación de responsabilidades**: Cada capa tiene un propósito definido
- **Inversión de dependencias**: Desacoplamiento entre componentes
- **Testabilidad**: Facilita la creación de pruebas unitarias

### Entity Framework Core
- **Code First**: Modelo de datos definido en código
- **Migraciones automáticas**: Evolución controlada del esquema
- **LINQ Queries**: Consultas tipadas y seguras

### JWT Authentication
- **Stateless**: No requiere estado en el servidor
- **Escalabilidad**: Facilita el balanceo de carga
- **Seguridad**: Tokens firmados y validados

## Características Adicionales

### Rendimiento
- **Async/Await**: Operaciones asíncronas para mejor concurrencia
- **Entity Framework Core**: Optimización automática de consultas
- **Connection Pooling**: Reutilización de conexiones a base de datos

### Escalabilidad
- **Docker Container**: Despliegue consistente en cualquier plataforma
- **Microservices Ready**: Arquitectura preparada para descomposición
- **Cloud Ready**: Compatible con Azure, AWS, Google Cloud

### Mantenimiento
- **Logging Integrado**: Registro de eventos y errores
- **Configuration Management**: Configuraciones por entorno
- **Health Checks**: Monitoreo de estado de la aplicación
