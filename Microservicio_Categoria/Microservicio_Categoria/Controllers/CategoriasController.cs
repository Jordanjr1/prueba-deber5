using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microservicio_Categoria.Models;
using Microservicio_Categoria.Services; // <-- No olvides este using para el producer

namespace Microservicio_Categoria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public CategoriasController(ApplicationDbContext context, IRabbitMQProducer rabbitMQProducer)
        {
            _context = context;
            _rabbitMQProducer = rabbitMQProducer;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        // POST: api/Categorias
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            // Crear el objeto del evento con los datos que definimos en la pizarra
            var evento = new
            {
                IdCategoria = categoria.IdCategoria,
                NombreCategoria = categoria.Nombre
            };

            // Enviar el mensaje a RabbitMQ para que el microservicio de vehículos se entere
            await _rabbitMQProducer.EnviarMensajeAsync(evento);

            return CreatedAtAction(nameof(GetCategorias), new { id = categoria.IdCategoria }, categoria);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}