using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class SaleFormPage : ContentPage
{
    private Venta _ventaActual;
    private bool _esEdicion;
    private DatabaseService _dbService = new DatabaseService();

    // === NUEVO: Carrito temporal para los materiales ===
    public ObservableCollection<ItemCarrito> CarritoMateriales { get; set; } = new ObservableCollection<ItemCarrito>();

    public SaleFormPage()
    {
        InitializeComponent();
        _esEdicion = false;
        _ventaActual = new Venta();
        txtId.Text = "Nuevo (Auto)";

        // Conectamos la lista visual con el carrito
        cvMaterialesAgregados.ItemsSource = CarritoMateriales;
    }

    public SaleFormPage(Venta ventaAEditar)
    {
        InitializeComponent();
        _esEdicion = true;
        _ventaActual = ventaAEditar;

        // Conectamos la lista visual con el carrito
        cvMaterialesAgregados.ItemsSource = CarritoMateriales;
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
        var inventario = await _dbService.ObtenerInventarioAsync();

        if (inventario != null && inventario.Count > 0)
        {
            // Ahora le pasamos la lista de objetos completa al Picker (el XAML ya extrae el NombreArticulo)
            pckMateriales.ItemsSource = inventario;
        }
        else
        {
            pckMateriales.Title = "Inventario vacío";
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
        txtCosto.Text = _ventaActual.Costo.ToString();
        txtObservaciones.Text = _ventaActual.Observaciones;

        if (DateTime.TryParse(_ventaActual.FechaEntrega, out DateTime fecha))
            dpEntrega.Date = fecha;
    }

    // ============================================================
    // LÓGICA DEL CARRITO DE MATERIALES
    // ============================================================
    private void OnAgregarMaterialClicked(object sender, EventArgs e)
    {
        var materialSeleccionado = pckMateriales.SelectedItem as InventarioItem;

        if (materialSeleccionado != null && int.TryParse(txtCantidadMaterial.Text, out int cantidad))
        {
            // Agregamos a la lista
            CarritoMateriales.Add(new ItemCarrito
            {
                InventarioOriginal = materialSeleccionado,
                NombreArticulo = materialSeleccionado.NombreArticulo,
                CantidadUsada = cantidad
            });

            // Limpiamos los campos para agregar otro rápido
            pckMateriales.SelectedItem = null;
            txtCantidadMaterial.Text = string.Empty;
        }
        else
        {
            DisplayAlert("Aviso", "Selecciona un material del inventario y escribe una cantidad válida.", "OK");
        }
    }

    private void OnEliminarMaterialClicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var itemAQuitar = boton?.CommandParameter as ItemCarrito;

        if (itemAQuitar != null)
        {
            CarritoMateriales.Remove(itemAQuitar);
        }
    }

    // ============================================================
    // GUARDAR VENTA Y DESCONTAR INVENTARIO
    // ============================================================
    private async void BtnGuardarVenta_Clicked(object sender, EventArgs e)
    {
        // 1. Validaciones
        if (string.IsNullOrWhiteSpace(txtEmpleado.Text) || string.IsNullOrWhiteSpace(txtProducto.Text))
        {
            await DisplayAlert("Error", "Los campos Empleado y Producto son obligatorios.", "OK");
            return;
        }

        // 2. Llenar objeto Venta
        _ventaActual.NombreEmpleado = txtEmpleado.Text;
        _ventaActual.Cliente = txtCliente.Text;
        _ventaActual.Telefono = txtTelefono.Text;
        _ventaActual.Producto = txtProducto.Text;

        // Por defecto la venta es de 1 producto principal (Figura 3D)
        _ventaActual.Cantidad = 1;

        if (decimal.TryParse(txtCosto.Text, out decimal costo))
            _ventaActual.Costo = costo;

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

        // Si se usaron materiales, los agregamos al texto de observaciones para no perder el registro
        if (CarritoMateriales.Count > 0)
        {
            string materialesUsados = "\n[Materiales extra: " + string.Join(", ", CarritoMateriales.Select(c => $"{c.CantidadUsada}x {c.NombreArticulo}")) + "]";
            _ventaActual.Observaciones = txtObservaciones.Text + materialesUsados;
        }
        else
        {
            _ventaActual.Observaciones = txtObservaciones.Text;
        }

        // 3. Guardar Venta en BD
        await _dbService.GuardarVentaAsync(_ventaActual);

        // 4. ✅ DESCONTAR LISTA DE MATERIALES DEL INVENTARIO
        foreach (var itemCarrito in CarritoMateriales)
        {
            // Usamos tu propiedad StockActual
            var materialEnBD = itemCarrito.InventarioOriginal;
            materialEnBD.StockActual -= itemCarrito.CantidadUsada;

            // (Opcional) Evitar números negativos
            if (materialEnBD.StockActual < 0) materialEnBD.StockActual = 0;

            // Actualizamos en la base de datos
            await _dbService.GuardarItemInventarioAsync(materialEnBD);
        }

        string accion = _esEdicion ? "actualizada" : "registrada";
        await DisplayAlert("Éxito", $"Venta {accion} y stock actualizado.\n(Se descontaron {CarritoMateriales.Count} tipos de material del inventario)", "Aceptar");

        await Navigation.PopAsync();
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// === CLASE AUXILIAR PARA LA LISTA (Fuera de SaleFormPage) ===
public class ItemCarrito
{
    public InventarioItem InventarioOriginal { get; set; }
    public string NombreArticulo { get; set; }
    public int CantidadUsada { get; set; }
}