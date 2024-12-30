using Cliente.Dominio.Models;

namespace Cliente.Aplicacion.Repositorios
{
    public interface IClienteRepositorio
    {
        Task CrearAsync(Customer customer);
        Task<Customer> ObtenerPorIdAsync(int id);
        Task<Customer> ObtenerPorEmailAsync(string email);
        Task<IEnumerable<Customer>> ObtenerTodosAsync();
        Task ActualizarAsync(Customer customer);
        Task EliminarAsync(int id);
    }
}
