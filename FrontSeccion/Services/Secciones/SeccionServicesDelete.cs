using FrontSeccion;

namespace BackSeccion1.Services.Secciones
{
    public class SeccionServicesDelete
    {
        private readonly ApplicationDbContext _context;

        public SeccionServicesDelete(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteSeccion(int id)
        {
            try
            {
                var seccion = await _context.Seccion.FindAsync(id);

                if (seccion == null)
                {
                    return false;
                }

                // Aplicar eliminación lógica
                seccion.isDeleted = true;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);
            }

            return false;
        }
        //public async Task<bool> DeleteSeccion(int id)
        //{
        //    try
        //    {
        //        var seccion = await _context.Seccion.FindAsync(id);

        //        if (seccion == null)
        //        {
        //            return false;
        //        }

        //        _context.Seccion.Remove(seccion);
        //        await _context.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de la excepción
        //        Console.WriteLine("Se ha producido una excepción: " + ex.Message);
        //    }

        //    return false;
        //}
    }
}
