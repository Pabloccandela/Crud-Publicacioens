using API.shared;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class PublicacionDto
    {
        [Key]
        public int PublicacionId { get; set; }
        public TipoOperacion TipoOperacion { get; set; }
        public TipoPropiedad TipoPropiedad { get; set; }
        public string Descripcion { get; set; }
        public int Ambientes { get; set; }
        public double M2 { get; set; }
        public DateTime Antiguedad { get; set; }
        public string CoordenadasUbicacion { get; set; }
        public ICollection<ImagenDto> Imagenes { get; set; }

    }

    public class ImagenDto
    {
        [Key]
        public int ImagenId { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Contenido { get; set; }

        [ForeignKey(nameof(Imagen))]
        public int PublicacionId { get; set; }
    }



}
