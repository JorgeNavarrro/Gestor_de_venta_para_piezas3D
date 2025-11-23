using Gestor_De_Ventas_Para_Piezas_3D.Modelos;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class SaleFormPage : ContentPage
{
    private Venta _ventaActual;
    private bool _esEdicion;

    // Constructor para REGISTRAR una nueva venta
    public SaleFormPage()
    {
        InitializeComponent();
        _esEdicion = false;
        txtId.Text = "Nuevo (Auto)";
        btnAccion.Clicked += BtnGuardar_Clicked; // Conectar el evento al botón
    }

    // Constructor para EDITAR una venta existente (POO)
    public SaleFormPage(Venta ventaAEditar)
    {
        InitializeComponent();
        _esEdicion = true;
        _ventaActual = ventaAEditar;

        CargarDatosVenta();
        btnAccion.Clicked += BtnGuardar_Clicked; // Conectar el evento al botón
    }

    private void CargarDatosVenta()
    {
        if (_ventaActual == null) return;

        // Actualizar la interfaz para modo Edición
        lblTitulo.Text = $"Editar Venta ID {_ventaActual.Id}";
        btnAccion.Text = "ACTUALIZAR SOLICITUD";

        // Llenar campos con datos de la venta seleccionada
        txtId.Text = _ventaActual.Id.ToString();
        txtEmpleado.Text = _ventaActual.NombreEmpleado;
        txtCliente.Text = _ventaActual.Cliente;
        txtTelefono.Text = _ventaActual.Telefono;
        txtProducto.Text = _ventaActual.Producto;
        txtCantidad.Text = _ventaActual.Cantidad.ToString();
        txtCosto.Text = _ventaActual.Costo.ToString();

        // Asignar fecha, asegurando que sea válida
        dpEntrega.Date = DateTime.TryParse(_ventaActual.FechaEntrega, out var date) ? date : DateTime.Today;

        txtObservaciones.Text = _ventaActual.Observaciones;
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        // Validación mínima
        if (string.IsNullOrWhiteSpace(txtEmpleado.Text) || string.IsNullOrWhiteSpace(txtProducto.Text))
        {
            await DisplayAlert("Error", "Los campos Empleado y Producto son obligatorios.", "OK");
            return;
        }

        // Lógica de guardado/actualización simulada
        string accion = _esEdicion ? "actualizada" : "registrada";

        // Mensaje de éxito (simulando que se guardó en la base de datos)
        await DisplayAlert("Éxito", $"Venta {accion} correctamente. [Simulación de guardado]", "Aceptar");
        await Navigation.PopAsync();
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        // Simplemente cierra el formulario y vuelve a la pantalla anterior
        await Navigation.PopAsync();
    }
}