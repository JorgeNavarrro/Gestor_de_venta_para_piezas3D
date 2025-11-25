using SQLite;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        private async Task InitAsync()
        {
            if (_database != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "tienda.db3");
            _database = new SQLiteAsyncConnection(databasePath);

            // --- CREACIÓN DE TABLAS ---
            // 1. Catálogo de Productos (Referencias visuales)
            await _database.CreateTableAsync<ModeloReference>();

            // 2. Usuarios (Login y Registro)
            await _database.CreateTableAsync<Usuario>();

            // 3. Ventas (Registro de pedidos)
            await _database.CreateTableAsync<Venta>();

            // 4. Pagos (Historial de pagos/notas)
            await _database.CreateTableAsync<Pago>();

            // 5. INVENTARIO (¡NUEVO!) - Materiales y stock
            await _database.CreateTableAsync<InventarioItem>();
        }

        // =================================================
        // 1. MÉTODOS DE INVENTARIO (LO NUEVO)
        // =================================================

        public async Task<int> GuardarItemInventarioAsync(InventarioItem item)
        {
            await InitAsync();
            if (item.Id != 0)
                return await _database.UpdateAsync(item);
            else
                return await _database.InsertAsync(item);
        }

        public async Task<List<InventarioItem>> ObtenerInventarioAsync()
        {
            await InitAsync();
            // Ordenar alfabéticamente por nombre para que se vea bonito
            return await _database.Table<InventarioItem>().OrderBy(i => i.NombreArticulo).ToListAsync();
        }

        public async Task<int> BorrarItemInventarioAsync(InventarioItem item)
        {
            await InitAsync();
            return await _database.DeleteAsync(item);
        }


        // =================================================
        // 2. MÉTODOS DE VENTAS
        // =================================================

        public async Task<int> GuardarVentaAsync(Venta venta)
        {
            await InitAsync();
            if (venta.Id != 0)
                return await _database.UpdateAsync(venta);
            else
                return await _database.InsertAsync(venta);
        }

        public async Task<List<Venta>> ObtenerVentasAsync()
        {
            await InitAsync();
            return await _database.Table<Venta>().OrderByDescending(v => v.Id).ToListAsync();
        }

        public async Task<int> BorrarVentaAsync(Venta venta)
        {
            await InitAsync();
            return await _database.DeleteAsync(venta);
        }


        // =================================================
        // 3. MÉTODOS DE PAGOS (NOTAS)
        // =================================================

        public async Task<int> GuardarPagoAsync(Pago pago)
        {
            await InitAsync();
            if (pago.Id != 0)
                return await _database.UpdateAsync(pago);
            else
                return await _database.InsertAsync(pago);
        }

        public async Task<List<Pago>> ObtenerPagosAsync()
        {
            await InitAsync();
            return await _database.Table<Pago>().OrderByDescending(p => p.Id).ToListAsync();
        }

        public async Task<int> BorrarPagoAsync(Pago pago)
        {
            await InitAsync();
            return await _database.DeleteAsync(pago);
        }


        // =================================================
        // 4. MÉTODOS DE USUARIOS
        // =================================================

        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            await InitAsync();
            try
            {
                await _database.InsertAsync(usuario);
                return true;
            }
            catch (Exception)
            {
                return false; // Usuario duplicado o error
            }
        }

        public async Task<Usuario> LoginUsuarioAsync(string username, string password)
        {
            await InitAsync();
            return await _database.Table<Usuario>()
                            .Where(u => u.NombreUsuario == username && u.Contrasena == password)
                            .FirstOrDefaultAsync();
        }

        public async Task<bool> ActualizarContrasenaAsync(string username, string nuevaContrasena)
        {
            await InitAsync();
            var usuario = await _database.Table<Usuario>()
                            .Where(u => u.NombreUsuario == username)
                            .FirstOrDefaultAsync();

            if (usuario != null)
            {
                usuario.Contrasena = nuevaContrasena;
                await _database.UpdateAsync(usuario);
                return true;
            }
            return false;
        }


        // =================================================
        // 5. MÉTODOS DE PRODUCTOS (CATÁLOGO)
        // =================================================

        public async Task<string> ProbarConexionAsync()
        {
            try
            {
                await InitAsync();
                int cantidad = await _database.Table<ModeloReference>().CountAsync();
                return $"¡Conexión SQLite Exitosa! (Productos en BD: {cantidad})";
            }
            catch (Exception ex) { return $"Error: {ex.Message}"; }
        }

        public async Task<int> GuardarProductoAsync(ModeloReference producto)
        {
            await InitAsync();
            if (producto.Id != 0)
                return await _database.UpdateAsync(producto);
            else
                return await _database.InsertAsync(producto);
        }

        public async Task<List<ModeloReference>> ObtenerProductosAsync()
        {
            await InitAsync();
            return await _database.Table<ModeloReference>().ToListAsync();
        }

        public async Task<int> BorrarProductoAsync(ModeloReference producto)
        {
            await InitAsync();
            return await _database.DeleteAsync(producto);
        }

        public async Task<string> CargarDatosInicialesAsync()
        {
            await InitAsync();
            // Aquí podrías insertar datos de prueba si quisieras, pero lo dejaremos limpio
            return "OK";
        }

        public async Task<string> InicializarBaseDeDatosAsync()
        {
            await InitAsync();
            return "OK";
        }
    }
}