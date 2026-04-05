# Contributing to Sistema de Gestión de Tickets - Backend API

¡Gracias por tu interés en contribuir al Sistema de Gestión de Tickets! Este documento te guiará sobre cómo contribuir de manera efectiva a este proyecto.

## Tabla de Contenidos

- [Requisitos Previos](#requisitos-previos)
- [Configuración del Entorno de Desarrollo](#configuración-del-entorno-de-desarrollo)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Estándares de Código](#estándares-de-código)
- [Proceso de Contribución](#proceso-de-contribución)
- [Envío de Pull Requests](#envío-de-pull-requests)
- [Guía de Estilo](#guía-de-estilo)

## Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- **.NET 9.0 SDK** o superior
- **Docker** y **Docker Compose**
- **SQL Server 2022** (o usar Docker)
- **Visual Studio 2022** o **Visual Studio Code**
- **Git**

## Configuración del Entorno de Desarrollo

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/Backend-Ticketera.git
cd Backend-Ticketera
```

### 2. Configurar Variables de Entorno

Copia el archivo de entorno de ejemplo:

```bash
cp .env.example .env
```

Edita el archivo `.env` con tus configuraciones locales:

```env
# SQL Server Configuration
SA_PASSWORD=YourStrongPassword123!
ACCEPT_EULA=Y

# Database Connection String Components
DB_SERVER=sistema-tickets-sql,1433
DB_DATABASE=SistemaTicketsDB
DB_USER=sa
DB_PASSWORD=YourStrongPassword123!
DB_TRUST_SERVER_CERTIFICATE=True
```

### 3. Ejecutar con Docker (Recomendado)

```bash
docker-compose up --build
```

La API estará disponible en `http://localhost:5000`
La base de datos en `localhost:1433`

### 4. Ejecutar Localmente (Alternativa)

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar aplicación
dotnet run
```

## Estructura del Proyecto

El proyecto sigue **Clean Architecture** con las siguientes capas:

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
└── Properties/                    # Configuraciones
```

## Estándares de Código

### Convenciones de Nomenclatura

- **Clases**: PascalCase (ej: `TicketService`)
- **Métodos**: PascalCase (ej: `GetAllTickets`)
- **Propiedades**: PascalCase (ej: `FechaCreacion`)
- **Campos privados**: _camelCase con guion bajo (ej: `_ticketRepository`)
- **Variables locales**: camelCase (ej: `ticketId`)
- **Constantes**: PascalCase (ej: `MaxRetries`)

### Organización de Archivos

- **Un archivo por clase** (excepto enums pequeños)
- **Namespace**: `Sistema_tickets_api.Capa.Subcapa`
- **Using statements**: Al principio, ordenados alfabéticamente
- **Regiones**: Usar `#region` para organizar código extenso

### Ejemplo de Estructura de Clase

```csharp
using Microsoft.EntityFrameworkCore;
using Sistema_tickets_api.Core.Entities;
using Sistema_tickets_api.Core.Interfaces;

namespace Sistema_tickets_api.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Public Methods

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .Include(t => t.Responsable)
            .Include(t => t.Comentarios)
            .ToListAsync();
    }

    #endregion

    #region Private Methods

    private async Task<bool> TicketExistsAsync(int id)
    {
        return await _context.Tickets.AnyAsync(t => t.Id == id);
    }

    #endregion
}
```

## Proceso de Contribución

### 1. Crear una Rama

```bash
git checkout -b feature/tu-nueva-funcionalidad
```

**Nomenclatura de ramas:**
- `feature/nombre-funcionalidad` - Nuevas características
- `bugfix/descripcion-bug` - Corrección de errores
- `hotfix/correcion-urgente` - Fixes críticos
- `refactor/mejora-codigo` - Refactorización

### 2. Desarrollar tu Funcionalidad

- Sigue los estándares de código establecidos
- Asegúrate de que el código compile sin errores
- Implementa logging apropiado
- Maneja excepciones correctamente

### 3. Probar tu Cambio

```bash
# Compilar proyecto
dotnet build

# Verificar funcionamiento manual
dotnet run
```

## Envío de Pull Requests

### Antes de Enviar

1. **Actualiza tu rama**:
   ```bash
   git fetch origin
   git rebase origin/main
   ```

2. **Resuelve conflictos** si existen

3. **Ejecuta compilación completa**:
   ```bash
   dotnet build
   ```

4. **Formatea tu código**:
   ```bash
   dotnet format
   ```

### Estructura del Pull Request

**Título**: Breve y descriptivo
- `feat: Add user authentication endpoint`
- `fix: Resolve ticket filtering bug`
- `refactor: Improve repository pattern`

**Descripción**:
```markdown
## Descripción
Breve descripción de los cambios implementados.

## Cambios
- [ ] Nueva funcionalidad X
- [ ] Corrección del bug Y
- [ ] Mejora en el rendimiento Z

## Verificación
- [ ] Compilación exitosa
- [ ] Funcionamiento manual verificado

## Checklist
- [ ] Código sigue los estándares del proyecto
- [ ] Sin errores de compilación
- [ ] Documentación actualizada si es necesario
```

## Guía de Estilo Específica

### Entity Framework Core

```csharp
// Correcto: Usar async/await
public async Task<Ticket> GetByIdAsync(int id)
{
    return await _context.Tickets
        .Include(t => t.Responsable)
        .FirstOrDefaultAsync(t => t.Id == id);
}

// Incorrecto: Bloquear llamadas asíncronas
public Ticket GetById(int id)
{
    return _context.Tickets
        .Include(t => t.Responsable)
        .FirstOrDefault(t => t.Id == id);
}
```

### Controladores API

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // Cuando se requiera autenticación
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll()
    {
        try
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            // Logging aquí
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
```

### DTOs y Mappers

```csharp
// DTO - Solo para transferencia de datos
public class CreateTicketDto
{
    [Required]
    [StringLength(100)]
    public string Titulo { get; set; }

    [Required]
    [StringLength(500)]
    public string Descripcion { get; set; }

    public int? ResponsableId { get; set; }
}

// Mapper - Transformación entre entidades y DTOs
public static class TicketMapper
{
    public static TicketDto ToDto(this Ticket ticket)
    {
        return new TicketDto
        {
            Id = ticket.Id,
            Titulo = ticket.Titulo,
            Descripcion = ticket.Descripcion,
            FechaCreacion = ticket.FechaCreacion,
            Estado = ticket.Estado.ToString(),
            ResponsableNombre = ticket.Responsable?.Nombre
        };
    }
}
```

## Configuración de Desarrollo

### appsettings.json

No commits de información sensible. Usa variables de entorno:

```json
{
  "ConnectionStrings": {
    "TicketsConnection": "${ConnectionStrings__TicketsConnection}"
  },
  "Jwt": {
    "Key": "${Jwt__Key}",
    "Issuer": "${Jwt__Issuer}",
    "Audience": "${Jwt__Audience}"
  }
}
```

### Secrets para Desarrollo

```bash
# Para desarrollo local
dotnet user-secrets set "ConnectionStrings:TicketsConnection" "Server=..."
dotnet user-secrets set "Jwt:Key" "tu-clave-secreta"
```

## Recursos Adicionales

- [Documentación de .NET](https://docs.microsoft.com/es-es/dotnet/)
- [Guía de Estilo C# de Microsoft](https://docs.microsoft.com/es-es/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Entity Framework Core Documentation](https://docs.microsoft.com/es-es/ef/core/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/es-es/aspnet/core/)
