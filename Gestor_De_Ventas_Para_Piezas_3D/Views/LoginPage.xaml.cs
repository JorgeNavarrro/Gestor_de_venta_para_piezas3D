using Gestor_De_Ventas_Para_Piezas_3D.Modelos;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    // 1. Evento para el botón INGRESAR
    private async void BtnIngresar_Clicked(object sender, EventArgs e)
    {
        Usuario usuario = new Usuario();
        usuario.NombreUsuario = txtUsuario.Text;
        usuario.Password = txtPassword.Text;

        if (usuario.ValidarLogin())
        {
            await Navigation.PushAsync(new MenuPage());
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
        }
    }

    // 2. Evento para el label OLVIDÉ CONTRASEÑA
    private async void OnOlvidePasswordTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PasswordRecoveryPage());
    }

    // 3. Evento para el botón NUEVO USUARIO (Este es el que faltaba antes)
    private async void BtnNuevoUsuario_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}