using Devart.Data.PostgreSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Smartax.Web.Application.Clases.Parametros.ReteICA
{
    public class ConfRenglonConcepto
    {
        internal DataTable GetAll()
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            var _da = new PgSqlDataAdapter($@"
                SELECT renglon,
	                   cuenta,
	                   datoarchivo,
	                   case when datoarchivo = 0 then 'Producto'
                       when datoarchivo = 1 then 'Saldo inicial'
	                   when datoarchivo = 2 then 'Debito'
	                   when datoarchivo = 3 then 'Credito'
	                   when datoarchivo = 4 then 'Saldo final'
	                   when datoarchivo = 5 then 'Debitos - Creditos'
	                   when datoarchivo = 6 then 'Creditos - Debitos'
	                   else '' end nom_datoarchivo
                FROM public.tbl_concepto_renglon_ica
                ORDER BY renglon;", _cnn);
            var dt = new DataTable();
            _da.Fill(dt);
            return dt;
        }

        internal bool Insert(string idUsuario, int renglon, string cuenta, int? datoarchivo)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var command = datoarchivo.HasValue ? $"INSERT INTO public.tbl_concepto_renglon_ica (renglon, cuenta, datoarchivo, id_estado, id_usuario_add) VALUES({renglon}, '{cuenta}', {datoarchivo},1, {idUsuario});" :
                    $"INSERT INTO public.tbl_concepto_renglon_ica (renglon, cuenta, id_estado, id_usuario_add) VALUES({renglon}, '{cuenta}',1, {idUsuario});";
            var _da = new PgSqlCommand(command, _cnn);
            var affectedRows = _da.ExecuteNonQuery();
            _cnn.Close();
            return affectedRows == 1;
        }

        internal bool Delete(int renglon, string cuenta)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var _da = new PgSqlCommand($"DELETE FROM public.tbl_concepto_renglon_ica WHERE renglon = {renglon} AND cuenta = '{cuenta}'; ", _cnn);
            var affectedRows = _da.ExecuteNonQuery();
            _cnn.Close();
            return affectedRows == 1;
        }

        internal bool Update(string idUsuario, int renglon, string cuenta, int? datoarchivo, int oldRenglon, string oldCuenta)
        {
            var _cnn = new PgSqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString);
            _cnn.Open();
            var command = datoarchivo.HasValue ? $"UPDATE public.tbl_concepto_renglon_ica SET renglon = {renglon}, cuenta = '{cuenta}', datoarchivo = {datoarchivo}, id_usuario_up = {idUsuario}, fecha_actualizacion = NOW() WHERE renglon = {oldRenglon} AND cuenta = '{oldCuenta}';" :
                $"UPDATE public.tbl_concepto_renglon_ica SET renglon = {renglon}, cuenta = '{cuenta}', datoarchivo = null, id_usuario_up = {idUsuario}, fecha_actualizacion = NOW() WHERE renglon = {oldRenglon} AND cuenta = '{oldCuenta}'; ";
            var _da = new PgSqlCommand(command, _cnn);
            var affectedRows = _da.ExecuteNonQuery();
            _cnn.Close();
            return affectedRows == 1;
        }
    }
}