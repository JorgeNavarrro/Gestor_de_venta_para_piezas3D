using Microsoft.Maui.Controls;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // Botón "Ingresar"
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                await DisplayAlert("Error", "Por favor ingresa usuario y contraseña", "OK");
                return;
            }

            var db = new DatabaseService();

            // Consultamos a la base de datos SQLite
            var usuario = await db.LoginUsuarioAsync(txtUser.Text, txtPass.Text);

            if (usuario != null)
            {
                // LOGIN EXITOSO -> Vamos al Menú Principal
                // Cambiamos la raíz para que no puedan volver al login con "Atrás"
                Application.Current.MainPage = new NavigationPage(new MenuPage());
            }
            else
            {
                await DisplayAlert("Acceso Denegado", "Usuario o contraseña incorrectos.", "OK");
            }
        }

        // Botón "Nuevo Usuario"
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Navegar a la pantalla de registro
            await Navigation.PushAsync(new RegisterPage());
        }

        // Label "Olvidé mi contraseña" (CORREGIDO)
        private async void OnOlvidePassClicked(object sender, TappedEventArgs e)
        {
            // ANTES: Mostraba alerta
            // AHORA: Navega a la pantalla real que creaste
            await Navigation.PushAsync(new PasswordRecoveryPage());
        }
    }
}