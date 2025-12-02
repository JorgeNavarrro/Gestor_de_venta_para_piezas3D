using SQLite;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using Microsoft.Maui.Storage;

namespace Gestor_De_Ventas_Para_Piezas_3D.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        // Esta variable se llena solo al iniciar métodos async, por eso fallaba la exportación
        private string _databasePath;

        private async Task InitAsync()
        {
            if (_database != null)
                return;

            _databasePath = Path.Combine(FileSystem.AppDataDirectory, "tienda.db3");
            _database = new SQLiteAsyncConnection(_databasePath);

            await _database.CreateTableAsync<ModeloReference>();
            await _database.CreateTableAsync<Usuario>();
            await _database.CreateTableAsync<Venta>();
            await _database.CreateTableAsync<Pago>();
            await _database.CreateTableAsync<InventarioItem>();
        }

        // ✅ MÉTODO CORREGIDO: Define la ruta por sí mismo
        public string ExportarBaseDeDatos()
        {
            string carpetaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string nombreArchivo = $"tienda_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db3";
            string rutaDestino = Path.Combine(carpetaDescargas, nombreArchivo);

            // CORRECCIÓN AQUÍ: Calculamos la ruta explícitamente
            string rutaOrigen = Path.Combine(FileSystem.AppDataDirectory, "tienda.db3");

            try
            {
                if (!File.Exists(rutaOrigen))
                {
                    return "Error: El archivo de base de datos 'tienda.db3' no existe en el dispositivo.";
                }

                // Intentamos cerrar conexiones previas si existen (opcional en este contexto síncrono)
                // Copiamos el archivo
                File.Copy(rutaOrigen, rutaDestino, true);

                return $"Éxito: Base de datos exportada a:\n{rutaDestino}";
            }
            catch (Exception ex)
            {
                return $"Error al exportar: {ex.Message}";
            }
        }

        // ... (Resto de métodos sin cambios) ...

        public async Task<int> GuardarVentaAsync(Venta venta)
        {
            await InitAsync();
            if (venta.Id != 0) return await _database.UpdateAsync(venta);
            else return await _database.InsertAsync(venta);
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

        public async Task<int> GuardarPagoAsync(Pago pago)
        {
            await InitAsync();
            if (pago.Id != 0) return await _database.UpdateAsync(pago);
            else return await _database.InsertAsync(pago);
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

        public async Task<int> GuardarItemInventarioAsync(InventarioItem item)
        {
            await InitAsync();
            if (item.Id != 0) return await _database.UpdateAsync(item);
            else return await _database.InsertAsync(item);
        }

        public async Task<List<InventarioItem>> ObtenerInventarioAsync()
        {
            await InitAsync();
            return await _database.Table<InventarioItem>().OrderBy(i => i.NombreArticulo).ToListAsync();
        }

        public async Task<int> BorrarItemInventarioAsync(InventarioItem item)
        {
            await InitAsync();
            return await _database.DeleteAsync(item);
        }

        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            await InitAsync();
            try { await _database.InsertAsync(usuario); return true; }
            catch (Exception) { return false; }
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
            return producto.Id != 0 ? await _database.UpdateAsync(producto) : await _database.InsertAsync(producto);
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
            return "OK";
        }

        public async Task<string> InicializarBaseDeDatosAsync()
        {
            await InitAsync();
            return "OK";
        }
    }
}