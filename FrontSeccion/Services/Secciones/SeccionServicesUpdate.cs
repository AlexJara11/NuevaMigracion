using FrontSeccion;
using FrontSeccion.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackSeccion1.Services.Secciones
{
    public class SeccionServicesUpdate
    {
        private readonly ApplicationDbContext _context;

        public SeccionServicesUpdate(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> EditSeccion(Seccion seccion)
        {
            if (seccion == null)
                throw new ArgumentNullException(nameof(seccion));

            var existingSeccion = await _context.Seccion.FindAsync(seccion.id_sec);

            if (existingSeccion == null)
                return false; // Sección no encontrada

            if (existingSeccion.isDeleted)
                return false; // No permitir actualizar una sección eliminada lógicamente

            try
            {
                _context.Entry(existingSeccion).CurrentValues.SetValues(seccion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception (e.g., if the seccion was deleted by another process)
                return false;
            }
        }
        //public async Task<bool> EditSeccion(Seccion seccion)
        //{
        //    try
        //    {
        //        var existingSeccion = await _context.Seccion.FindAsync(seccion.id_sec);

        //        if (existingSeccion == null)
        //        {
        //            return false;
        //        }

        //        var validationContext = new ValidationContext(seccion, serviceProvider: null, items: null);
        //        var validationResults = new List<ValidationResult>();

        //        bool isValid = Validator.TryValidateObject(seccion, validationContext, validationResults, validateAllProperties: true);

        //        if (isValid)
        //        {
        //            _context.Entry(existingSeccion).CurrentValues.SetValues(seccion);
        //            await _context.SaveChangesAsync();
        //            return true;
        //        }
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
