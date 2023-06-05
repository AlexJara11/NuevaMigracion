using FrontSeccion;
using FrontSeccion.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackSeccion1.Services.Secciones
{
    public class SeccionServicesCrear
    {
        private readonly ApplicationDbContext _context;

        public SeccionServicesCrear(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateSeccion(Seccion seccion)
        {
            try
            {
                var validationContext = new ValidationContext(seccion, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(seccion, validationContext, validationResults, validateAllProperties: true);

                if (isValid)
                {
                    seccion.isDeleted = false; // Establecer isDeleted en false al crear el alumno
                    _context.Add(seccion);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);
            }

            return false;
        }

    }
}
