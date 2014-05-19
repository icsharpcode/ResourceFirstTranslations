using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcesFirstTranslations.Web.Models
{
    public class GenericResponse
    {
        public GenericResponse(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public GenericResponse(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}