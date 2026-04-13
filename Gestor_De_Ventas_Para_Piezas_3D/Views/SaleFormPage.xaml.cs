using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class SaleFormPage : ContentPage
{
    private Venta _ventaActual;
    private bool _esEdicion;

    // Lista para tener los objetos del inventario a la mano
    private List<InventarioItem> _materialesDisponibles;

    public SaleFormPage()
    {
        InitializeComponent();
        _esEdicion = false;
        _ventaActual = new Venta();
        txtId.Text = "Nuevo (Auto)";

        btnAccion.Clicked += BtnGuardar_Clicked;
    }

    public SaleFormPage(Venta ventaAEditar)
    {
        InitializeComponent();
        _esEdicion = true;
        _ventaActual = ventaAEditar;

        btnAccion.Clicked += BtnGuardar_Clicked;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Cargar materiales al entrar para llenar el Picker
        await CargarMateriales();

        if (_esEdicion)
        {
            CargarDatosEnFormulario();
        }
    }

    private async Task CargarMateriales()
    {
        var db = new DatabaseService();
        _materialesDisponibles = await db.ObtenerInventarioAsync();

        if (_materialesDisponibles != null && _materialesDisponibles.Count > 0)
        {
            pkrMaterial.ItemsSource = _materialesDisponibles.Select(m => m.NombreArticulo).ToList();
        }
        else
        {
            pkrMaterial.Title = "Inventario vacío";
        }
    }

    private void CargarDatosEnFormulario()
    {
        if (_ventaActual == null) return;

        lblTitulo.Text = $"Editar Venta ID {_ventaActual.Id}";
        btnAccion.Text = "ACTUALIZAR SOLICITUD";

        txtId.Text = _ventaActual.Id.ToString();
        txtEmpleado.Text = _ventaActual.NombreEmpleado;
        txtCliente.Text = _ventaActual.Cliente;
        txtTelefono.Text = _ventaActual.Telefono;
        txtProducto.Text = _ventaActual.Producto;
        txtCantidad.Text = _ventaActual.Cantidad.ToString();
        txtCosto.Text = _ventaActual.Costo.ToString();
        txtObservaciones.Text = _ventaActual.Observaciones;

        if (DateTime.TryParse(_ventaActual.FechaEntrega, out DateTime fecha))
            dpEntrega.Date = fecha;
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        // 1. Validaciones
        if (string.IsNullOrWhiteSpace(txtEmpleado.Text) || string.IsNullOrWhiteSpace(txtProducto.Text))
        {
            await DisplayAlert("Error", "Los campos Empleado y Producto son obligatorios.", "OK");
            return;
        }

        // Validar que se seleccionó un material para descontar
        if (pkrMaterial.SelectedIndex == -1)
        {
            await DisplayAlert("Atención", "Por favor selecciona el material a descontar del inventario.", "OK");
            return;
        }

        // 2. Llenar objeto Venta
        _ventaActual.NombreEmpleado = txtEmpleado.Text;
        _ventaActual.Cliente = txtCliente.Text;
        _ventaActual.Telefono = txtTelefono.Text;
        _ventaActual.Producto = txtProducto.Text;

        int cantidadVenta = 0;
        if (int.TryParse(txtCantidad.Text, out int cant))
        {
            _ventaActual.Cantidad = cant;
            cantidadVenta = cant; // Guardamos este número para restarlo al inventario
        }

        if (decimal.TryParse(txtCosto.Text, out decimal costo)) _ventaActual.Costo = costo;

        // Fechas y Duración
        DateTime fechaSolicitud = DateTime.Now;
        if (_esEdicion && !string.IsNullOrEmpty(_ventaActual.FechaSolicitud))
        {
            if (DateTime.TryParse(_ventaActual.FechaSolicitud, out DateTime fechaGuardada))
            {
                fechaSolicitud = fechaGuardada;
            }
        }

        _ventaActual.FechaSolicitud = fechaSolicitud.ToString("dd/MM/yyyy");
        _ventaActual.FechaEntrega = dpEntrega.Date.ToString("dd/MM/yyyy");

        TimeSpan diferencia = dpEntrega.Date - fechaSolicitud.Date;
        int dias = diferencia.Days;
        _ventaActual.Duracion = (dias <= 0) ? "Mismo día" : $"{dias} días";
        _ventaActual.Observaciones = txtObservaciones.Text;

        // 3. Guardar Venta en BD
        var db = new DatabaseService();
        await db.GuardarVentaAsync(_ventaActual);

        // ============================================================
        // 4. ✅ DESCONTAR DEL INVENTARIO (Lógica agregada)
        // ============================================================
        string nombreMaterialSeleccionado = pkrMaterial.SelectedItem.ToString();

        // Buscamos el objeto real en nuestra lista usando el nombre
        var itemInventario = _materialesDisponibles.FirstOrDefault(m => m.NombreArticulo == nombreMaterialSeleccionado);

        if (itemInventario != null)
        {
            // Restamos la cantidad vendida
            itemInventario.StockActual -= cantidadVenta;

            // (Opcional) Evitar números negativos
            if (itemInventario.StockActual < 0) itemInventario.StockActual = 0;

            // Guardamos la actualización en la base de datos
            await db.GuardarItemInventarioAsync(itemInventario);
        }
        // ============================================================

        string accion = _esEdicion ? "actualizada" : "registrada";
        await DisplayAlert("Éxito", $"Venta {accion} y stock actualizado.\n(Se descontaron {cantidadVenta} de {nombreMaterialSeleccionado})", "Aceptar");

        await Navigation.PopAsync();
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}