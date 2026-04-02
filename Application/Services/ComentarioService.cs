using Microsoft.EntityFrameworkCore;
using Sistema_tickets_api.Core.Entities;
using Sistema_tickets_api.Core.Interfaces;
using Sistema_tickets_api.Infraestructure.Data;

namespace Sistema_tickets_api.Application.Services
{
    public class ComentarioService
    {
        private readonly AppDbContext _context;

        public ComentarioService(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los comentarios
        public IEnumerable<Comentario> ObtenerTodos()
        {
            return _context.Comentarios
                           .OrderBy(c => c.FechaCreacion)
                           .ToList();
        }

        // Obtener un comentario por Id
        public Comentario? ObtenerPorId(int id)
        {
            return _context.Comentarios
                           .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<object> ObtenerPorTicket(int ticketId)
        {
            return _context.Comentarios
                .Include(c => c.Usuario)
                .Where(c => c.TicketId == ticketId)
                .OrderBy(c => c.FechaCreacion)
                .Select(c => new {
                    c.Id,
                    c.Texto,
                    c.FechaCreacion,
                    UsuarioNombre = c.Usuario != null
                        ? c.Usuario.Nombre
                        : "Usuario" 
                })
                .ToList();
        }

        // Crear un comentario
        public void Crear(Comentario comentario)
        {
            _context.Comentarios.Add(comentario);
            _context.SaveChanges();
        }

        // Actualizar un comentario
        public void Actualizar(Comentario comentario)
        {
            _context.Comentarios.Update(comentario);
            _context.SaveChanges();
        }

        // Eliminar un comentario
        public void Eliminar(int id)
        {
            var comentario = _context.Comentarios.Find(id);
            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
                _context.SaveChanges();
            }
        }
    }
}