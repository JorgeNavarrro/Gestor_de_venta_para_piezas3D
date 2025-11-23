using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    public class Usuario
    {
        public string NombreUsuario { get; set; }
        public string Password { get; set; }

        public bool ValidarLogin()
        {
            if (NombreUsuario == "usuario1" && Password == "1234567")
            {
                return true;
            }
            return false;
        }
    }
}