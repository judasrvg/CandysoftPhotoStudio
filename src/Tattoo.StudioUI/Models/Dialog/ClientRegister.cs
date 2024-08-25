using System.ComponentModel.DataAnnotations;

namespace Tattoo.StudioUI.Models.Dialog
{
    public class ClientRegister
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }= string.Empty;
        [Required(ErrorMessage = "El número de teléfono es requerido")]
        public string Phone { get; set; } = string.Empty;
    }
}
