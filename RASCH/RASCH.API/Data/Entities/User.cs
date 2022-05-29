using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using RASCH.Common.Enums;

namespace RASCH.API.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Business Business { get; set; }


        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://vehiclessalazar.azurewebsites.net/images/noimage.png"
            : $"https://vehiclessalazar.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        //public ICollection<Vehicle> Vehicles { get; set; }

        //[Display(Name = "# Vehículos")]
        //public int VehiclesCount => Vehicles == null ? 0 : Vehicles.Count;
    }
}