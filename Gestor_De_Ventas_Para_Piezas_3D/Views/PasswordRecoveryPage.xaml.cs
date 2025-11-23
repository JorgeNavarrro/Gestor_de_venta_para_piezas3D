namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class PasswordRecoveryPage : ContentPage
{
    public PasswordRecoveryPage()
    {
        InitializeComponent();
    }

    private async void BtnRecuperar_Clicked(object sender, EventArgs e)
    {
        // Aquí iría la lógica real para conectar con la base de datos y validar el código

        // Simulamos éxito
        await DisplayAlert("Éxito", "Contraseña actualizada correctamente", "Aceptar");

        // Regresamos a la pantalla de Login (PopAsync cierra la pantalla actual)
        await Navigation.PopAsync();
    }
}