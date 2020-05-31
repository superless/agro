using System.ComponentModel.DataAnnotations;

namespace trifenix.agro.swagger.model.input
{
    public class LoginInput
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>

        [Required]
        public string Username { get; set; }



        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        [Required]
        public string Password { get; set; }


    }
}
