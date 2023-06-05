using FrontSeccion;
using FrontSeccion.Models;

namespace BackSeccion1.Services.Cursos
{
    public class CursoServiceCrear
    {
        private readonly ApplicationDbContext _context;

        public CursoServiceCrear(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCurso(Curso curso)
        {
            try
            {
                if (curso == null)
                    throw new ArgumentNullException(nameof(curso));

                // Establecer el estado del curso
                curso.isDeleted = false; // Por defecto, el curso no está eliminado lógicamente

                _context.Add(curso);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Handle exception if needed
                return false;
            }
        }
    }
}

