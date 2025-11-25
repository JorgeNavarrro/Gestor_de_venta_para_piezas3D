using System;
using Microsoft.Maui.Controls;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        // Botón "Registrar"
        private async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            // 1. Validaciones de campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombres.Text) ||
                string.IsNullOrWhiteSpace(txtApellidos.Text) ||
                string.IsNullOrWhiteSpace(txtNewUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtNewPass.Text))
            {
                await DisplayAlert("Error", "Por favor completa los campos obligatorios (Nombres, Apellidos, Usuario, Contraseña).", "OK");
                return;
            }

            // 2. Validar que las contraseñas coincidan
            if (txtNewPass.Text != txtRepeatPass.Text)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "OK");
                return;
            }

            // 3. Crear objeto Usuario con TODOS los datos del formulario
            // Asegúrate de que tu clase 'Usuario' en 'Modelos' tenga estas propiedades
            var nuevoUsuario = new Usuario
            {
                Nombres = txtNombres.Text,
                Apellidos = txtApellidos.Text,
                FechaNacimiento = dpNacimiento.Date,
                Telefono = txtTelefono.Text,
                Rol = txtRol.Text,
                NombreUsuario = txtNewUsuario.Text,
                Contrasena = txtNewPass.Text
            };

            // 4. Guardar en la base de datos SQLite
            var db = new DatabaseService();
            bool exito = await db.RegistrarUsuarioAsync(nuevoUsuario);

            if (exito)
            {
                await DisplayAlert("Éxito", $"Usuario {nuevoUsuario.NombreUsuario} registrado correctamente.", "OK");
                // Regresa a la pantalla anterior (Login)
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "El nombre de usuario ya existe o hubo un error al guardar.", "OK");
            }
        }

        // Botón "Limpiar"
        private void BtnLimpiar_Clicked(object sender, EventArgs e)
        {
            txtNombres.Text = string.Empty;
            txtApellidos.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtRol.Text = string.Empty;
            txtNewUsuario.Text = string.Empty;
            txtNewPass.Text = string.Empty;
            txtRepeatPass.Text = string.Empty;
            dpNacimiento.Date = DateTime.Now;
        }

        // Botón "Cancelar"
        private async void BtnCancelar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}