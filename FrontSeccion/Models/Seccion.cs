using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontSeccion.Models
{
    [Table("Seccion")]
    public class Seccion
    {
        public Seccion()
        {
            Alumno = new HashSet<Alumno>();
        }

        [Key]
        public int id_sec { get; set; }

        public int aula_sec { get; set; }

        //public int id_cur { get; set; }
        public int CursoId { get; set; }

        public bool estado_sec { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_registro_sec { get; set; }

        public bool isDeleted { get; set; }

        public virtual Curso Curso { get; set; }



        public virtual ICollection<Alumno> Alumno { get; set; }
    }
}
