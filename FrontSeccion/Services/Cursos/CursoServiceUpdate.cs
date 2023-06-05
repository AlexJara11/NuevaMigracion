using FrontSeccion;
using FrontSeccion.Models;
using Microsoft.EntityFrameworkCore;

namespace BackSeccion1.Services.Cursos
{
    public class CursoServiceUpdate
    {
        private readonly ApplicationDbContext _context;

        public CursoServiceUpdate(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateCurso(Curso curso)
        {
            if (curso == null)
                throw new ArgumentNullException(nameof(curso));

            var existingCurso = await _context.Curso.FindAsync(curso.id_cur);

            if (existingCurso == null)
                return false; // Curso no encontrado

            if (existingCurso.isDeleted)
                return false; // No permitir actualizar un curso eliminado lógicamente

            try
            {
                _context.Entry(existingCurso).CurrentValues.SetValues(curso);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception (e.g., if the curso was deleted by another process)
                return false;
            }
        }
    }
}
