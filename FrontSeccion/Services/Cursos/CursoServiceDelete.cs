using FrontSeccion;

namespace BackSeccion1.Services.Cursos
{
    public class CursoServiceDelete
    {
        private readonly ApplicationDbContext _context;

        public CursoServiceDelete(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteCurso(int id)
        {
            try
            {
                var curso = await _context.Curso.FindAsync(id);
                if (curso == null)
                    return false;

                // Realizar eliminación lógica estableciendo isDeleted en true
                curso.isDeleted = true;

                //_context.Update(curso);
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
