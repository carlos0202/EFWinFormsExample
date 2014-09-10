using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace EFWinFormsExample.Models.ViewModels
{
    public partial class ClientViewModel : IDataErrorInfo
    {
        [Browsable(false)]
        public int Id { get; set; }
        [DisplayName("Nombre")]
        public string FirstName { get; set; }
        [DisplayName("Apellido")]
        public string LastName { get; set; }
        [DisplayName("Cédula")]
        public string DNI { get; set; }
        [DisplayName("Dirección")]
        public string Address { get; set; }

        [Browsable(false)]
        [NotMapped]
        public string Error
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this["FirstName"]);
                sb.Append(this["LastName"]);
                sb.Append(this["DNI"]);
                sb.Append(this["Address"]);
                return sb.ToString();
            }
        }

        [Browsable(false)]
        [NotMapped]
        public string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName")
                {
                    if (String.IsNullOrEmpty(FirstName))
                        return "Debe introducir el Nombre.";

                }
                else if (columnName == "LastName")
                {
                    if (String.IsNullOrEmpty(LastName))
                        return "Debe introducir el apellido.";
                }

                if (columnName == "DNI")
                {
                    if (String.IsNullOrEmpty(DNI))
                        return "Debe introducir la cédula.";

                    if (DNI.Length != 11)
                        return "La cédula debe tener 11 dígitos.";

                    if (!Regex.IsMatch(DNI, "^[0-9]*$"))
                        return "La cédula debe tener sólo dígitos.";

                }

                return string.Empty;
            }
        }

        [Browsable(false)]
        public bool IsValid()
        {
            if (Error == string.Empty)
                return true;
            else
                return false;
        }  
    }
}
