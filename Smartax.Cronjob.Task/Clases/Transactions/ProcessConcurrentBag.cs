using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Smartax.Cronjob.Process.Clases.Utilidades;
using Smartax.Cronjob.Process.Clases.Models;

namespace Smartax.Cronjob.Process.Clases.Transactions
{
    class ProcessConcurrentBag
    {
        #region DEFINICION DE VARIABLES 
        // aqui se cargan los registros a procesar
        private ConcurrentBag<InfoEstablecimientos> _bolsaTrabajo;
        // aqui se registra lo procesado para registrar en un log por ejemplo y al final saber si no se logro procesar algun recobro
        private ConcurrentBag<ResultadoProceso> _trabajoProcesado;
        // variables de control del proceso
        private int _cantTrabajoTotalProcesar;
        private int _cantTrabajoTotalProcesado;
        private int _cantTrabajoResultadosTotalGuardado;
        private bool _trabajoCargadoCompletamente;
        // control de errores
        private bool _errorAgregandoTrabajo;
        private bool _errorProcesandoTrabajo;
        private bool _errorGuardandoTrabajo;
        private string _errorMessage;
        public string ErrorMessage { get { return _errorMessage; } }
        // maximo de tareas que procesan el trabajo
        private int _maximaConcurrencia;
        // generador de tiempos de procesamiento aletaorios
        Random _rnd;
        #endregion

        public ProcessConcurrentBag()
        {
            _bolsaTrabajo = new ConcurrentBag<InfoEstablecimientos>();
            _trabajoProcesado = new ConcurrentBag<ResultadoProceso>();
            _errorAgregandoTrabajo = false;
            _errorProcesandoTrabajo = false;
            _errorGuardandoTrabajo = false;
            _cantTrabajoTotalProcesar = 0;
            _cantTrabajoTotalProcesado = 0;
            _cantTrabajoResultadosTotalGuardado = 0;
            _trabajoCargadoCompletamente = false;
            _errorMessage = "";
            _maximaConcurrencia = FixedData.MAXIMA_CONCURRENCIA;
            _rnd = new Random();
        }

        public async Task EjecutarProcessAsync(InfoEstablecimientos objBase, string carpetaBase, int tamLoteResultadosGuardar)
        {
            List<Task> todasLasTareas = new List<Task>();
            //--PASO 1
            Task addWorkTask = Task.Run(async () => { await AgregarTrabajoAsync(objBase, carpetaBase); });
            todasLasTareas.Add(addWorkTask);

            // hacemos una pausa de medio segundo para que se cargue algo de trabajo
            await Task.Delay(1500);

            //--PROCESAMIENTO DE TRABAJO DE FORMA ASINCRONA
            Task processWorkTask = Task.Run(async () => await ProcesarTrabajoAsync(objBase));
            todasLasTareas.Add(processWorkTask);

            // hacemos una pausa de medio segundo para que se procese algo de trabajo
            await Task.Delay(1500);

            // inserción de lotes de resultados de forma asincrona o log por ejemplo cada 500 resultados
            Task persistResultsTask = GuardarLogAsync(tamLoteResultadosGuardar);
            todasLasTareas.Add(persistResultsTask);

            // esperar a que finalicen las tareas
            await Task.WhenAll(todasLasTareas);

            // Finalizó el proceso
            if (_errorAgregandoTrabajo || _errorProcesandoTrabajo || _errorGuardandoTrabajo)
            {
                if (_errorAgregandoTrabajo)
                {
                    _errorMessage = "Se presentó un error durante el cargue de trabajo";
                }
                else if (_errorProcesandoTrabajo)
                {
                    _errorMessage = "Se presentó un error durante el procesamiento de trabajo";
                }
                else
                {
                    _errorMessage = "Se presentó un error guardando los resultados";
                }

            }
        }

        private async Task AgregarTrabajoAsync(InfoEstablecimientos objBase, string carpetaBase)
        {
            Console.WriteLine($"Inició la tarea que agrega trabajo a la cola {DateTime.Now}");
            try
            {
                //--INSTANCIAMOS EL OBJETO DE CLASE
                ProcessDb objProcessDb = new ProcessDb();
                objProcessDb.TipoConsulta = 5;
                objProcessDb.IdCliente = objBase.id_cliente;
                objProcessDb.IdEstablecimientoPadre = null;
                objProcessDb.AnioGravable = objBase.anio_gravable;
                objProcessDb.MesEf = objBase.mes_ef;
                objProcessDb.IdEstado = 1;
                //--
                DataTable dtEstablecimientos = new DataTable();
                dtEstablecimientos = objProcessDb.GetEstablecimientosCliente();
                if (dtEstablecimientos != null)
                {
                    if (dtEstablecimientos.Rows.Count > 0)
                    {
                        foreach (DataRow rowItem in dtEstablecimientos.Rows)
                        {
                            int _IdClienteEstablecimiento = Int32.Parse(rowItem["idcliente_establecimiento"].ToString().Trim());
                            int _IdMunicipio = Int32.Parse(rowItem["id_municipio"].ToString().Trim());
                            string _CodigoDane = rowItem["codigo_dane"].ToString().Trim();
                            //--
                            //--AQUI VAMOS AGREGANDO EL ESTABLECIMIENTO A LA BOLSA DE TRABAJO
                            InfoEstablecimientos unidadDeTrabajo = new InfoEstablecimientos()
                            {
                                id_cliente = objBase.id_cliente,
                                idform_impuesto = objBase.idform_impuesto,
                                anio_gravable = objBase.anio_gravable,
                                mes_ef = objBase.mes_ef,
                                idcliente_establecimiento = _IdClienteEstablecimiento,
                                id_municipio = _IdMunicipio,
                                codigo_dane = _CodigoDane,
                                version_ef = objBase.version_ef,
                                id_usuario = objBase.id_usuario
                            };

                            _bolsaTrabajo.Add(unidadDeTrabajo);
                            //--es el unico lugar donde se incrementa este valor, por eso no necesito sincronizar su incremento
                            _cantTrabajoTotalProcesar++;
                        }
                    }
                }

                _trabajoCargadoCompletamente = true;
            }
            catch (Exception ex)
            {
                _errorAgregandoTrabajo = true;
            }
            Console.WriteLine($"Finalizó la tarea que agrega trabajo a la cola {DateTime.Now}");
        }

        private async Task ProcesarTrabajoAsync(InfoEstablecimientos unidadDeTrabajo)
        {
            Console.WriteLine($"Inició la tarea que procesa el trabajo {DateTime.Now}");
            try
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < _maximaConcurrencia; i++)
                {
                    var t = Task.Run(async () =>
                    {
                        await UnidadDeProcesamientoDeTrabajo(unidadDeTrabajo);
                    });
                    tasks.Add(t);
                }
                await Task.WhenAll(tasks);
            }
            catch
            {
                _errorProcesandoTrabajo = true;
            }
            Console.WriteLine($"Finalizó la tarea que procesa el trabajo {DateTime.Now}");
        }

        private async Task UnidadDeProcesamientoDeTrabajo(InfoEstablecimientos unidadDeTrabajo)
        {
            bool continuar = true;
            InfoEstablecimientos objTrabajo;

            while (continuar)
            {
                try
                {
                    // intentamos tomar un objeto de trabajo
                    if (_bolsaTrabajo.TryTake(out objTrabajo))
                    {
                        #region 4. AQUI HACEMOS EL LLAMADO DEL METODO PARA GENERAR LA BASE GRAVABLE POR ESTABLECIMIENTO
                        //--
                        Transactions objTransactions = new Transactions();
                        //--
                        ResultadoProceso resultado = new ResultadoProceso()
                        {
                            anio_gravable = objTrabajo.anio_gravable,
                            mes_ef = objTrabajo.mes_ef,
                            id_municipio = objTrabajo.id_municipio,
                            idcliente_establecimiento = objTrabajo.idcliente_establecimiento,
                            codigo_dane = objTrabajo.codigo_dane,
                            ProcesoOk = true
                        };
                        //--
                        _trabajoProcesado.Add(resultado);
                        Interlocked.Increment(ref _cantTrabajoTotalProcesado);
                        //--
                        unidadDeTrabajo.idcliente_establecimiento = resultado.idcliente_establecimiento;
                        unidadDeTrabajo.codigo_dane = resultado.codigo_dane;
                        unidadDeTrabajo.id_municipio = resultado.id_municipio;
                        //--
                        //await objTransactions.TaskBaseGravable(unidadDeTrabajo);
                        #endregion
                        ///---
                    }

                    // validar si hay más trabajo por procesar
                    if (_trabajoCargadoCompletamente)
                    {
                        continuar = _cantTrabajoTotalProcesar > _cantTrabajoTotalProcesado;
                    }
                    if (_errorAgregandoTrabajo)
                    {
                        continuar = false;
                    }
                }
                catch
                {
                    _errorProcesandoTrabajo = true;
                    continuar = false;
                }
            }
        }

        private async Task GuardarLogAsync(int tamLoteResultadosGuardar)
        {
            ResultadoProceso resultado;
            bool continuar = true;
            Console.WriteLine($"Inició la tarea que guarda los resultados o logs del proceso {DateTime.Now}");

            while (continuar)
            {
                try
                {
                    if (_trabajoProcesado.Count > tamLoteResultadosGuardar || (_cantTrabajoResultadosTotalGuardado + _trabajoProcesado.Count) == _cantTrabajoTotalProcesar)
                    {
                        int conteoLocal = 0;
                        for (int i = 0; i < tamLoteResultadosGuardar; i++)
                        {
                            if (_trabajoProcesado.TryTake(out resultado))
                            {
                                conteoLocal++;
                            }
                        }

                        // simular 1 segundos de tiempo de escritura de resultados ya sea en log o db
                        await Task.Delay(1000);
                        // lo puedo escribir así normal ya que es la unica tarea donde se actualiza
                        _cantTrabajoResultadosTotalGuardado += conteoLocal;
                        Console.WriteLine($"[Guardado]: se han guardado {_cantTrabajoResultadosTotalGuardado} registros de {_cantTrabajoTotalProcesar}, {DateTime.Now}");
                    }
                    else
                    {
                        Console.WriteLine($"[Guardado]: soy muy rápido esperando por más, {DateTime.Now}");
                        // esperar medio segundo ya que la escritura de resultados está más rapida que el procesamiento de trabajo
                        await Task.Delay(4000);
                    }
                    // validar si hay más trabajo por procesar
                    if (_trabajoCargadoCompletamente)
                    {
                        continuar = _cantTrabajoTotalProcesar > _cantTrabajoResultadosTotalGuardado;
                    }
                    if (_errorAgregandoTrabajo || _errorProcesandoTrabajo)
                    {
                        continuar = false;
                    }
                }
                catch
                {
                    continuar = false;
                    _errorGuardandoTrabajo = true;
                }
            }

            Console.WriteLine($"Finalizó la tarea que guarda los resultados o logs del proceso {DateTime.Now}");
        }

    }
}
