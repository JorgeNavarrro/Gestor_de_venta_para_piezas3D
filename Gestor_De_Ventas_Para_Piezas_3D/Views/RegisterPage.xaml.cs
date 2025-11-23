namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }

    private void BtnLimpiar_Clicked(object sender, EventArgs e)
    {
        // Limpiamos todos los campos
        txtNombres.Text = string.Empty;
        txtApellidos.Text = string.Empty;
        txtTelefono.Text = string.Empty;
        txtRol.Text = string.Empty;
        txtNewUsuario.Text = string.Empty;
        txtNewPass.Text = string.Empty;
        txtRepeatPass.Text = string.Empty;
        dpNacimiento.Date = DateTime.Now;
    }

    private async void BtnRegistrar_Clicked(object sender, EventArgs e)
    {
        // Validación simple
        if (string.IsNullOrEmpty(txtNombres.Text) || string.IsNullOrEmpty(txtNewUsuario.Text) || string.IsNullOrEmpty(txtNewPass.Text))
        {
            await DisplayAlert("Error", "Por favor complete los campos obligatorios", "OK");
            return;
        }

        if (txtNewPass.Text != txtRepeatPass.Text)
        {
            await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        // Éxito simulado
        await DisplayAlert("Éxito", $"Usuario {txtNewUsuario.Text} registrado correctamente.", "Aceptar");

        // Regresar al Login
        await Navigation.PopAsync();
    }

    private async void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        // Regresar sin hacer nada
        await Navigation.PopAsync();
    }
}