using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontSeccion.Models
{
    [Table("Alumno")]
    public class Alumno
    {
        public Alumno()
        {
            Seccion = new HashSet<Seccion>();
        }

        [Key]
        public int id_alu { get; set; }

        [Required]
        [StringLength(8)]
        public string dni_alu { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_alu { get; set; }

        [Required]
        [StringLength(100)]
        public string apellidos_alu { get; set; }
        public bool isDeleted { get; set; }



        public virtual ICollection<Seccion> Seccion { get; set; }
    }
}
