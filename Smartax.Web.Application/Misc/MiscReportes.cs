using System.Data;
using System.IO;
using System.Linq;

namespace Smartax.Web.Application.Misc
{
    internal static class MiscReportes
    {
        internal static byte[] DataTableByArrayBite(DataTable table,bool cabecera,string separador)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            string linea = "";

            if (cabecera)
            {
                linea = string.Join(separador, table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray());
                tw.WriteLine(linea);
            }

            foreach (DataRow item in table.Rows)
            {
                linea = string.Join(separador, item.ItemArray);
                tw.WriteLine(linea);
            }
            tw.Flush();
            byte[] respuesta = ms.ToArray();
            ms.Close();

            return respuesta;
        }
    }
}