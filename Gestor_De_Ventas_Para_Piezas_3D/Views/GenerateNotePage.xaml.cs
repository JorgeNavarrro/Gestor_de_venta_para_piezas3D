using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class GenerateNotePage : ContentPage
{
    private Pago _pagoActual;
    private bool _esEdicion;

    public GenerateNotePage()
    {
        InitializeComponent();
        _esEdicion = false;
        _pagoActual = new Pago();

        lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

        // Calculamos el ID visualmente (solo para mostrar al usuario qué número tocará)
        CargarSiguienteId();
    }

    public GenerateNotePage(Pago pagoAEditar)
    {
        InitializeComponent();
        _esEdicion = true;
        _pagoActual = pagoAEditar;

        CargarDatos();
    }

    // Nuevo método para predecir el siguiente ID
    private async void CargarSiguienteId()
    {
        var db = new DatabaseService();
        var pagos = await db.ObtenerPagosAsync();

        // Si hay pagos, tomamos el ID más alto y sumamos 1. Si no, empezamos en 1.
        int siguienteId = 1;
        if (pagos != null && pagos.Count > 0)
        {
            siguienteId = pagos.Max(p => p.Id) + 1;
        }

        lblNoNota.Text = $"NOTA-{siguienteId:D6}";
    }

    private void CargarDatos()
    {
        if (_pagoActual == null) return;

        lblTituloPagina.Text = $"Editar Nota #{_pagoActual.Id}";
        btnGenerar.Text = "Actualizar Nota";

        lblNoNota.Text = _pagoActual.NumeroNota;
        lblFecha.Text = _pagoActual.FechaEmision;

        txtIdPedido.Text = _pagoActual.IdPedido.ToString();
        txtCliente.Text = _pagoActual.NombreCliente;
        txtProducto.Text = _pagoActual.Productos;
        txtMonto.Text = _pagoActual.CostoTotal.ToString();
        txtObservaciones.Text = _pagoActual.Observaciones;

        pkrMetodo.SelectedItem = _pagoActual.MedioPago;
    }

    private async void BtnGenerar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtCliente.Text) || string.IsNullOrEmpty(txtMonto.Text))
        {
            await DisplayAlert("Error", "Por favor ingrese Cliente y Monto", "OK");
            return;
        }

        if (pkrMetodo.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Por favor seleccione un Método de Pago", "OK");
            return;
        }

        // Llenar objeto
        if (int.TryParse(txtIdPedido.Text, out int pid)) _pagoActual.IdPedido = pid;
        _pagoActual.NombreCliente = txtCliente.Text;
        _pagoActual.Productos = txtProducto.Text;
        _pagoActual.MedioPago = pkrMetodo.SelectedItem?.ToString();

        if (decimal.TryParse(txtMonto.Text, out decimal m)) _pagoActual.CostoTotal = m;

        // Si es nuevo, ponemos fecha de hoy
        if (!_esEdicion) _pagoActual.FechaEmision = lblFecha.Text;

        _pagoActual.Observaciones = txtObservaciones.Text;

        // Guardar en SQLite
        var db = new DatabaseService();
        await db.GuardarPagoAsync(_pagoActual);

        string accion = _esEdicion ? "actualizada" : "generada";
        await DisplayAlert("Éxito", $"Nota de venta {accion} correctamente.", "Aceptar");
        await Navigation.PopAsync();
    }
}