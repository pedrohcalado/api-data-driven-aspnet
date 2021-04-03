using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class User
    {
        // Id, username, password, role
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 20 caracteres")]
        [MaxLength(20, ErrorMessage = "Este campo deve ter entre 3 e 20 caracteres")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MinLength(3, ErrorMessage = "Este campo deve ter entre 3 e 20 caracteres")]
        [MaxLength(20, ErrorMessage = "Este campo deve ter entre 3 e 20 caracteres")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Role { get; set; }
    }
}
