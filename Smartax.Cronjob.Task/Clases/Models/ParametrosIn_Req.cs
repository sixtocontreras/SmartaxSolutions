using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartax.Cronjob.Process.Clases.Models
{
    public class BaseGravable_Req
    {
        public int tipo_consulta { get; set; }
        //public object idcliente_ef { get; set; }
        public string nombre_tarea { get; set; }
        public object id_cliente { get; set; }
        public int idform_impuesto { get; set; }
        public object idform_configuracion { get; set; }
        public object id_puc { get; set; }
        public int id_municipio { get; set; }
        public object idcliente_establecimiento { get; set; }
        public int anio_gravable { get; set; }
        public object codigo_dane { get; set; }
        public string version_ef { get; set; }
        public string mes_ef { get; set; }
        public int id_usuario { get; set; }
        public int tipo_proceso { get; set; }
    }

    public class LiqOficinas_Req
    {
        public int tipo_consulta { get; set; }
        public string nombre_tarea { get; set; }
        //public object idcliente_ef { get; set; }
        public object id_cliente { get; set; }
        public object idcliente_establecimiento { get; set; }
        public int idform_impuesto { get; set; }
        public int anio_gravable { get; set; }
        public string mes_ef { get; set; }
        public int id_estado { get; set; }
        public int tipo_proceso { get; set; }
    }

    public class InfoEstablecimientos
    {
        public object id_cliente { get; set; }
        public int idform_impuesto { get; set; }
        public int anio_gravable { get; set; }
        public string mes_ef { get; set; }
        public int id_municipio { get; set; }
        public int idcliente_establecimiento { get; set; }
        public string codigo_dane { get; set; }
        public string version_ef { get; set; }
        public int id_usuario { get; set; }
    }

    public class ResultadoProceso
    {
        public int anio_gravable { get; set; }
        public string mes_ef { get; set; }
        public int id_municipio { get; set; }
        public int idcliente_establecimiento { get; set; }
        public string codigo_dane { get; set; }
        public bool ProcesoOk { get; set; }
    }

    public class FileDavibox_Req
    {
        public int anio_gravable { get; set; }
        public string mes_procesar { get; set; }
        public string uuid { get; set; }
    }

}
