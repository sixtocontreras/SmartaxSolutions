using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Smartax.WebApi.Services.Clases.Seguridad;
using Smartax.WebApi.Services.Models;

namespace Smartax.WebApi.Services.Clases.Procesos
{
    public class Comprobantes
    {
        Functions objFunctions = new Functions();
        EnviarEmails objEmails = new EnviarEmails();
        const string quote = "\"";

    }
}