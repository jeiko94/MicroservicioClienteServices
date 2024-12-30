using Cliente.Aplicacion.Repositorios;
using Cliente.Dominio.Models;
using Cliente.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cliente.Infraestructura.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly ClienteDbContext _context;

        public ClienteRepositorio(ClienteDbContext context)
        {
            _context = context;
        }

        public async Task CrearAsync(Customer customer)
        {
            _context.Clientes.Add(customer);
            await _context.SaveChangesAsync();
        }
        public async Task<Customer> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }
        public async Task<Customer> ObtenerPorEmailAsync(string email)
        {
            return await _context.Clientes.FirstOrDefaultAsync(e => e.Email == email);
        }
        public async Task<IEnumerable<Customer>> ObtenerTodosAsync()
        {
            return await _context.Clientes.ToListAsync();
        }
        public async Task ActualizarAsync(Customer customer)
        {
            _context.Clientes.Update(customer);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarAsync(int id)
        {
            var cliente = await ObtenerPorIdAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
