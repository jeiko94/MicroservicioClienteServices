
using Cliente.Aplicacion.Repositorios;
using Cliente.Dominio.Models;

namespace Clientes.Aplicacion.Servicios
{
    /// Lógica de aplicación para gestionar clientes.

    public class ClienteServicio
    {
        private readonly IClienteRepositorio _clienteRepo;

        public ClienteServicio(IClienteRepositorio clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

   
        // Crea un nuevo cliente, validando duplicados y hasheando el password.
        public async Task<int> RegistrarClienteAsync(string nombre, string email, string passwordClaro)
        {
            // Verificar si existe un cliente con este email
            var existente = await _clienteRepo.ObtenerPorEmailAsync(email);
            if (existente != null)
                throw new InvalidOperationException("El email ya está registrado.");

            // Hashear password
            string passwordHashed = HashPassword(passwordClaro);

            var nuevo = new Customer
            {
                Nombre = nombre,
                Email = email,
                PasswordHashed = passwordHashed,
                FechaRegistro = DateTime.UtcNow,
                Activo = true
            };

            await _clienteRepo.CrearAsync(nuevo);
            return nuevo.Id; // tras crear, EF le asigna un Id
        }


        // Autenticar: retorna el cliente si la password coincide, o null si no coincide.
        // O lanza una excepción, dependiendo de tu preferencia.

        public async Task<Customer> AutenticarClienteAsync(string email, string passwordClaro)
        {
            var cliente = await _clienteRepo.ObtenerPorEmailAsync(email);
            if (cliente == null) return null;

            var hashIngresado = HashPassword(passwordClaro);
            if (hashIngresado == cliente.PasswordHashed)
                return cliente;

            return null;
        }

        public async Task<Customer> ObtenerClienteAsync(int id)
        {
            return await _clienteRepo.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> ListarClientesAsync()
        {
            return await _clienteRepo.ObtenerTodosAsync();
        }

        public async Task ActualizarClienteAsync(int id, string nuevoNombre, string nuevoEmail)
        {
            var cliente = await _clienteRepo.ObtenerPorIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente no encontrado.");

            // Verificar si el nuevoEmail ya está en uso
            var otro = await _clienteRepo.ObtenerPorEmailAsync(nuevoEmail);
            if (otro != null && otro.Id != id)
                throw new InvalidOperationException("Ese email pertenece a otro cliente.");

            cliente.Nombre = nuevoNombre;
            cliente.Email = nuevoEmail;
            await _clienteRepo.ActualizarAsync(cliente);
        }

        public async Task EliminarClienteAsync(int id)
        {
            await _clienteRepo.EliminarAsync(id);
        }


        //Ejemplo sencillo de hashear password. 
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
