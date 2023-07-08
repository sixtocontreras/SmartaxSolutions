using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Modulos
{
    public class CalcularValor
    {
        #region AQUI DEFINIMOS LOS ATRIBUTOS DE LA CLASE
        public object IdMunicipio { get; set; }
        public object IdFormularioImpuesto { get; set; }
        public object AnioGravable { get; set; }
        public Int32 NumeroRenglon { get; set; }
        public string MotorBaseDatos { get; set; }
        public object IdEstado { get; set; }
        public int TipoConsulta { get; set; }
        #endregion

        public double GetCalcularValor()
        {
            double _ResultValor = 0;
            try
            {
                ConsultaLiqImpuesto ObjConsulta = new ConsultaLiqImpuesto();

            }
            catch (Exception ex)
            {
                _ResultValor = 0;                
            }

            return _ResultValor;
        }

    }
}