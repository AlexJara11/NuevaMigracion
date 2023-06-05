using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontSeccion.Models
{
    [Table("Curso")]
    public class Curso
    {
        public Curso()
        {
            Seccion = new HashSet<Seccion>();
        }

        [Key]
        public int id_cur { get; set; }

        [Required]
        [StringLength(25)]
        public string descripcion_cur { get; set; }

        public bool estado_cur { get; set; }
        public bool isDeleted { get; set; }


        public virtual ICollection<Seccion> Seccion { get; set; }
    }
}
