using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartax.WebApi.Services.Models
{
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

    public class ComprobanteContabilizacion_Req
    {
        public int idform_impuesto { get; set; }
        public int idperiodicidad_impuesto { get; set; }
        public int anio_gravable { get; set; }
        public string mes_procesar { get; set; }
        public int id_estado { get; set; }
        public string emails_confirmar { get; set; }
    }
}