using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.ProcessAPIs
{
    public class ModelApiSmartax
    {
        public class Token_Req
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Token_Resp
        {
            public string Message { get; set; }
            public bool Status { get; set; }
            public string Codigo { get; set; }
            public string token { get; set; }
        }

        public class BaseGravable_Req
        {
            public int tipo_consulta { get; set; }
            public object idcliente_ef { get; set; }
            public object id_cliente { get; set; }
            public object idcliente_establecimiento { get; set; }
            public int idform_impuesto { get; set; }
            public object idform_configuracion { get; set; }
            public object id_puc { get; set; }
            public int anio_gravable { get; set; }
            public string version_ef { get; set; }
            public string mes_ef { get; set; }
            public int id_usuario { get; set; }
        }

        public class BaseGravable_Resp
        {
            public string Message { get; set; }
            public bool Status { get; set; }
            public string Codigo { get; set; }
            public string fecha_proceso { get; set; }
        }

        public class LiquidarImpuesto_Req
        {
            public int estado_liquidacion { get; set; }
            public int idejecucion_lote { get; set; }
            public int tipo_impuesto { get; set; }
            public string data_procesar { get; set; }
            public string emails_confirmar { get; set; }
            public int id_usuario { get; set; }
            public string nombre_usuario { get; set; }
            public firmante info_firmante1 { get; set; }
            public firmante info_firmante2 { get; set; }
        }

        public class firmante
        {
            public object id_firmante { get; set; }
            public string nombre_firmante { get; set; }
            public object tipo_documento { get; set; }
            public string numero_documento { get; set; }
            public string numero_tp { get; set; }
            public int id_rol { get; set; }
            public byte[] imagen_firma { get; set; }
        }

        public class LiquidarImpuesto_Resp
        {
            public string Message { get; set; }
            public bool Status { get; set; }
            public string Codigo { get; set; }
            public string fecha_proceso { get; set; }
        }

        public class DownloadFileDavibox_Req
        {
            public string uuid { get; set; }
            public int month { get; set; }
            public int year { get; set; }
            public bool bimonthly { get; set; }
            public int prevmonth { get; set; }
        }

        public class DownloadFileDavibox_Resp
        {
            public string uuid { get; set; }
            public List<object> transferedfiles { get; set; }
            public List<object> failedfiles { get; set; }
        }

    }
}