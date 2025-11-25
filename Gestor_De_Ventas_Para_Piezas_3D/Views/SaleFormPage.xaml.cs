using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class SaleFormPage : ContentPage
{
    private Venta _ventaActual;
    private bool _esEdicion;

    // Constructor para NUEVA venta
    public SaleFormPage()
    {
        InitializeComponent();
        _esEdicion = false;
        _ventaActual = new Venta();
        txtId.Text = "Nuevo (Auto)";

        btnAccion.Clicked += BtnGuardar_Clicked;
    }

    // Constructor para EDITAR venta
    public SaleFormPage(Venta ventaAEditar)
    {
        InitializeComponent();
        _esEdicion = true;
        _ventaActual = ventaAEditar;

        CargarDatosEnFormulario();

        btnAccion.Clicked += BtnGuardar_Clicked;
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
        // 1. Validación mínima
        if (string.IsNullOrWhiteSpace(txtEmpleado.Text) || string.IsNullOrWhiteSpace(txtProducto.Text))
        {
            await DisplayAlert("Error", "Los campos Empleado y Producto son obligatorios.", "OK");
            return;
        }

        // 2. Pasar datos del formulario al objeto
        _ventaActual.NombreEmpleado = txtEmpleado.Text;
        _ventaActual.Cliente = txtCliente.Text;
        _ventaActual.Telefono = txtTelefono.Text;
        _ventaActual.Producto = txtProducto.Text;

        if (int.TryParse(txtCantidad.Text, out int cant)) _ventaActual.Cantidad = cant;
        if (decimal.TryParse(txtCosto.Text, out decimal costo)) _ventaActual.Costo = costo;

        // --- CORRECCIÓN DE FECHAS Y DURACIÓN ---

        // Obtener fecha de solicitud (si es nuevo es Hoy, si es edición mantenemos la original)
        DateTime fechaSolicitud = DateTime.Now;

        // Intentamos recuperar la fecha original si es edición
        if (_esEdicion && !string.IsNullOrEmpty(_ventaActual.FechaSolicitud))
        {
            if (DateTime.TryParse(_ventaActual.FechaSolicitud, out DateTime fechaGuardada))
            {
                fechaSolicitud = fechaGuardada;
            }
        }

        // Guardamos las fechas como texto
        _ventaActual.FechaSolicitud = fechaSolicitud.ToString("dd/MM/yyyy");
        _ventaActual.FechaEntrega = dpEntrega.Date.ToString("dd/MM/yyyy");

        // CALCULAR DURACIÓN (Ahora se calcula SIEMPRE, al crear y al editar)
        // Usamos .Date para que la resta sea exacta en días (sin horas)
        TimeSpan diferencia = dpEntrega.Date - fechaSolicitud.Date;
        int dias = diferencia.Days;

        if (dias < 0)
            _ventaActual.Duracion = "Fecha inválida"; // Entrega antes de solicitud
        else if (dias == 0)
            _ventaActual.Duracion = "Mismo día";
        else
            _ventaActual.Duracion = $"{dias} días";

        _ventaActual.Observaciones = txtObservaciones.Text;

        // 3. Guardar en Base de Datos
        var db = new DatabaseService();
        await db.GuardarVentaAsync(_ventaActual);

        string accion = _esEdicion ? "actualizada" : "registrada";
        await DisplayAlert("Éxito", $"Venta {accion} correctamente.", "Aceptar");
        await Navigation.PopAsync();
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}