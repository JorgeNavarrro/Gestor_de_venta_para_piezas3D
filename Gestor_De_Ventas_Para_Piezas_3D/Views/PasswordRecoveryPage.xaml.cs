using System;
using Microsoft.Maui.Controls;
using Gestor_De_Ventas_Para_Piezas_3D.Services;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas
{
    public partial class PasswordRecoveryPage : ContentPage
    {
        public PasswordRecoveryPage()
        {
            InitializeComponent();
        }

        private async void OnCambiarPasswordClicked(object sender, EventArgs e)
        {
            // 1. Validar que no haya campos vacíos
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtNuevaPass.Text) ||
                string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                await DisplayAlert("Error", "Por favor llena todos los campos.", "OK");
                return;
            }

            // 2. Validar que las contraseñas sean iguales
            if (txtNuevaPass.Text != txtRepitaPass.Text)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            // 3. Validar código (Simulado: el código debe ser '1234')
            // En un caso real, aquí validarías contra un código enviado por email.
            if (txtCodigo.Text != "1234")
            {
                await DisplayAlert("Error", "Código de validación incorrecto.", "OK");
                return;
            }

            // 4. Intentar actualizar en la base de datos
            var db = new DatabaseService();
            bool exito = await db.ActualizarContrasenaAsync(txtUsuario.Text, txtNuevaPass.Text);

            if (exito)
            {
                await DisplayAlert("Éxito", "Contraseña actualizada correctamente. Inicia sesión con tu nueva clave.", "OK");

                // Regresamos al Login
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "El usuario no existe. Verifica el nombre.", "OK");
            }
        }
    }
}