using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML;


namespace ML
{
    public class UsuarioML
    {
        public int IdUsuario { get; set; }
                
        [Required(ErrorMessage = "Nombre es requerido")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Minimo 5 caracteres")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "ApellidoPaterno es requerido")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Minimo 5 caracteres")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "ApellidoMaterno es requerido")]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Minimo 5 caracteres")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "Username es requerido")]
        [DataType(DataType.Text)]
        [StringLength(8, ErrorMessage = "Solo 8 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9._]{5,8}$", ErrorMessage ="Solo se permiten Letras, Numeros , _ y .")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email es requerido")]
        [EmailAddress]
        [StringLength(30, ErrorMessage = "Solo 30 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password es requerido")]        
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Solo 8 caracteres")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]).{8}$",ErrorMessage = "La contraseña debe contener letras, números y un carácter especial")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El Sexo es requerido")]
        [StringLength(2, ErrorMessage = "Selecciona F o M")]
        public string Sexo { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Ingresa un Telefono a 10 digitos")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Solo se permiten numeros")]
        public string Telefono { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Ingresa un Telefono a 10 digitos")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Solo se permiten numeros")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Formato Invalido , ingresa dd/MM/YYYY")]
        [RegularExpression(@"^[0-9/]+$", ErrorMessage = "Solo se permiten numeros")]
        public string FechaDeNacimiento { get; set; }

        [StringLength(18, ErrorMessage = "Ingresa un CURP valido")]
        [RegularExpression(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$")]
        public string CURP { get; set; }
        public Rol Rol { get; set; }
        public List<object> Usuarios { get; set; }      
        public byte[] Imagen { get; set; }
        public string ImagenBase64 { get; set; }    
        public ML.Direccion Direccion { get; set; } 
        public bool Estatus { get; set; }

        public List<object> Errores { get; set; }




    }
}
    