using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;
using API.shared;

namespace API.Controllers
{
    [Route("api/publicaciones")]
    [ApiController]
    public class PropiedadesAppService : ControllerBase
    {
        private readonly PublicacionesContext _context;

        // Constructor que recibe el contexto de la base de datos
        public PropiedadesAppService(PublicacionesContext context)
        {
            _context = context;
        }

        // GET: api/publicaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicacionDto>>> GetPublicaciones()
        {
            // Obtener todas las publicaciones de la base de datos
            var publicaciones = await _context.Publicaciones
               .Include(p => p.Imagenes)
               .ToListAsync();

            // Transformar las publicaciones a DTOs para enviar al cliente
            var publicacionesDto = publicaciones.Select(p => new PublicacionDto
            {
                PublicacionId = p.PublicacionId,
                TipoOperacion = p.TipoOperacion,
                TipoPropiedad = p.TipoPropiedad,
                Descripcion = p.Descripcion,
                Ambientes = p.Ambientes,
                M2 = p.M2,
                Antiguedad = p.Antiguedad,
                CoordenadasUbicacion = p.CoordenadasUbicacion,
                Imagenes = p.Imagenes.Select(i => new ImagenDto
                {
                    ImagenId = i.ImagenId,
                    Nombre = i.Nombre,
                    Ruta = i.Ruta,
                    Contenido = Convert.ToBase64String(i.Contenido)
                }).ToList()
            }).ToList();

            return Ok(publicacionesDto);
        }

        // GET: api/publicaciones/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicacionDto>> GetPublicacion(int id)
        {
            // Obtener una publicación específica por su ID
            var publicacion = await _context.Publicaciones
             .Where(p => p.PublicacionId == id)
             .Include(p => p.Imagenes)
             .ToListAsync();

            // Transformar la publicación a DTO para enviar al cliente
            if (publicacion == null)
            {
                return NotFound();
            }

            var publicacionDto = publicacion.Select(p => new PublicacionDto
            {
                PublicacionId = p.PublicacionId,
                TipoOperacion = p.TipoOperacion,
                TipoPropiedad = p.TipoPropiedad,
                Descripcion = p.Descripcion,
                Ambientes = p.Ambientes,
                M2 = p.M2,
                Antiguedad = p.Antiguedad,
                CoordenadasUbicacion = p.CoordenadasUbicacion,
                Imagenes = p.Imagenes.Select(i => new ImagenDto
                {
                    ImagenId = i.ImagenId,
                    Nombre = i.Nombre,
                    Ruta = i.Ruta,
                    Contenido = Convert.ToBase64String(i.Contenido)
                }).ToList()
            }).ToList();

            return Ok(publicacionDto[0]);
        }

        // PUT: api/publicaciones/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PublicacionDto>> EditPublicacion(int id, PublicacionDto publicacionActualizada)
        {
            try
            {
                // Buscar la publicación existente por su ID
                var publicacionExistente = await _context.Publicaciones
                    .Include(p => p.Imagenes)
                    .FirstOrDefaultAsync(p => p.PublicacionId == id);

                if (publicacionExistente == null)
                {
                    return NotFound($"No se encontró ninguna publicación con el ID {id}");
                }

                // Actualizar las propiedades según la nueva información
                publicacionExistente.TipoOperacion = publicacionActualizada.TipoOperacion;
                publicacionExistente.TipoPropiedad = publicacionActualizada.TipoPropiedad;
                publicacionExistente.Descripcion = publicacionActualizada.Descripcion;
                publicacionExistente.Ambientes = publicacionActualizada.Ambientes;
                publicacionExistente.M2 = publicacionActualizada.M2;
                publicacionExistente.Antiguedad = publicacionActualizada.Antiguedad;
                publicacionExistente.CoordenadasUbicacion = publicacionActualizada.CoordenadasUbicacion;

                // Eliminar imágenes existentes relacionadas con la publicación
                var imagenExist = await _context.Imagenes.Where(p => p.PublicacionId == publicacionExistente.PublicacionId).ToListAsync();
                if (imagenExist != null)
                {
                    _context.Imagenes.RemoveRange(imagenExist);
                    await _context.SaveChangesAsync();
                }

                // Agregar las nuevas imágenes a la publicación
                if (publicacionActualizada.Imagenes != null && publicacionActualizada.Imagenes.Count > 0)
                {
                    foreach (var imagenDto in publicacionActualizada.Imagenes)
                    {
                        if (imagenDto != null)
                        {
                            // Asegurarse de que la propiedad Contenido no sea nula
                            if (!string.IsNullOrEmpty(imagenDto.Contenido))
                            {
                                Imagen imagen = new Imagen
                                {
                                    Nombre = imagenDto.Nombre,
                                    Ruta = imagenDto.Ruta,
                                    Contenido = Convert.FromBase64String(imagenDto.Contenido)
                                };

                                publicacionExistente.Imagenes.Add(imagen);
                            }
                        }
                    }
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(publicacionActualizada);
            }
            catch (Exception ex)
            {
                // Manejar errores según tus necesidades
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/publicaciones
        [HttpPost]
        public async Task<ActionResult<PublicacionDto>> CrearPublicacion(PublicacionDto nuevaPublicacion)
        {
            try
            {
                // Validar si la publicación proporcionada es nula
                if (nuevaPublicacion == null)
                {
                    return BadRequest("La publicación proporcionada es nula.");
                }

                // Crear una nueva entidad Publicacion a partir del DTO
                Publicacion publicacion = new Publicacion
                {
                    TipoOperacion = nuevaPublicacion.TipoOperacion,
                    TipoPropiedad = nuevaPublicacion.TipoPropiedad,
                    Descripcion = nuevaPublicacion.Descripcion,
                    Ambientes = nuevaPublicacion.Ambientes,
                    M2 = nuevaPublicacion.M2,
                    Antiguedad = nuevaPublicacion.Antiguedad,
                    CoordenadasUbicacion = nuevaPublicacion.CoordenadasUbicacion,
                    Imagenes = new List<Imagen>()
                };

                // Agregar imágenes a la nueva publicación
                if (publicacion.Imagenes != null || publicacion.Imagenes.Count > 0)
                {
                    foreach (var imagenDto in nuevaPublicacion.Imagenes)
                    {
                        // Asegurarse de que la propiedad Contenido no sea nula
                        if (!string.IsNullOrEmpty(imagenDto.Contenido))
                        {
                            Imagen imagen = new Imagen
                            {
                                Nombre = imagenDto.Nombre,
                                Ruta = imagenDto.Ruta,
                                Contenido = Convert.FromBase64String(imagenDto.Contenido)
                            };

                            publicacion.Imagenes.Add(imagen);
                        }
                    }
                }

                // Agregar la nueva publicación a la base de datos
                _context.Publicaciones.Add(publicacion);
                await _context.SaveChangesAsync();

                // Devolver el DTO de la nueva publicación
                return Ok(nuevaPublicacion);
            }
            catch (Exception ex)
            {
                // Manejar errores según tus necesidades
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/publicaciones/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropiedad(int id)
        {
            // Buscar la publicación por su ID
            var propiedad = await _context.Publicaciones.FirstOrDefaultAsync(p => p.PublicacionId == id);
            if (propiedad == null)
            {
                return NotFound();
            }

            // Eliminar la publicación y las imágenes relacionadas
            _context.Publicaciones.Remove(propiedad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/publicaciones/Configs
        [HttpGet("Configs")]
        public IActionResult GetConfigs()
        {
            // Obtener las enumeraciones de TipoPropiedad y TipoOperacion
            var enumPropiedad = Enum.GetValues(typeof(TipoPropiedad))
                            .Cast<TipoPropiedad>()
                            .Select(e => new { Value = (int)e, Name = e.ToString() })
                            .ToList();

            var enumOperacion = Enum.GetValues(typeof(TipoOperacion))
                            .Cast<TipoOperacion>()
                            .Select(e => new { Value = (int)e, Name = e.ToString() })
                            .ToList();

            // Devolver las enumeraciones como resultado
            var result = new
            {
                TipoPropiedadEnum = enumPropiedad,
                TipoOperacionEnum = enumOperacion
            };

            return Ok(result);
        }
    }
}
