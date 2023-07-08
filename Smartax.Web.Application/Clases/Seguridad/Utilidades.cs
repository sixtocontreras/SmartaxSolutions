using log4net;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Telerik.Web.UI;

namespace Smartax.Web.Application.Clases.Seguridad
{
    public class Utilidades
    {
        private static readonly ILog _log = LogManager.GetLogger(FixedData.LOG_AUDITORIA_NAME);
        private static Bitmap cortado;

        public void CambiarGrillaAEspanol(RadGrid Grilla)
        {
            Grilla.HierarchySettings.ExpandTooltip = Properties.Resources.RadGridHierarchySettingsExpandTooltip;
            Grilla.HierarchySettings.CollapseTooltip = Properties.Resources.RadGridHierarchySettingsCollapseTooltip;
            Grilla.HierarchySettings.SelfCollapseTooltip = Properties.Resources.RadGridHierarchySettingsSelfCollapseTooltip;
            Grilla.HierarchySettings.SelfExpandTooltip = Properties.Resources.RadGridHierarchySettingsSelfExpandTooltip;

            Grilla.GroupingSettings.GroupContinuesFormatString = Properties.Resources.RadGridGroupingSettingsGroupContinuesFormatString;
            Grilla.GroupingSettings.CollapseTooltip = Properties.Resources.RadGridGroupingSettingsCollapseTooltip;
            //Grilla.GroupingSettings.ExpandTooltip = Properties.Resources.RadGridGroupingSettingsExpandTooltip;
            Grilla.GroupingSettings.UnGroupTooltip = Properties.Resources.RadGridGroupingSettingsUnGroupTooltip;
            Grilla.GroupingSettings.GroupSplitDisplayFormat = Properties.Resources.RadGridGroupingSettingsGroupSplitDisplayFormat;

            Grilla.GroupPanel.Text = Properties.Resources.RadGridGroupPanelText;

            Grilla.ClientSettings.ClientMessages.DropHereToReorder = Properties.Resources.RadGridClientSettingsClientMessagesDropHereToReorder;
            Grilla.ClientSettings.ClientMessages.DragToGroupOrReorder = Properties.Resources.RadGridClientSettingsClientMessagesDragToGroupOrReorder;
            Grilla.ClientSettings.ClientMessages.DragToResize = Properties.Resources.RadGridClientSettingsClientMessagesDragToResize;
            Grilla.ClientSettings.ClientMessages.PagerTooltipFormatString = Properties.Resources.RadGridClientSettingsClientMessagesPagerTooltipFormatString;

            Grilla.SortingSettings.SortedAscToolTip = Properties.Resources.RadGridSortingSettingsSortedAscToolTip;
            Grilla.SortingSettings.SortedDescToolTip = Properties.Resources.RadGridSortingSettingsSortedDescToolTip;
            Grilla.SortingSettings.SortToolTip = Properties.Resources.RadGridSortingSettingsSortToolTip;

            Grilla.MasterTableView.NoMasterRecordsText = Properties.Resources.RadGridMasterTableViewNoMasterRecordsText;
            Grilla.MasterTableView.NoDetailRecordsText = Properties.Resources.RadGridMasterTableViewNoDetailRecordsText;

            Grilla.StatusBarSettings.LoadingText = Properties.Resources.RadGridStatusBarSettingsLoadingText;
            Grilla.StatusBarSettings.ReadyText = Properties.Resources.RadGridStatusBarSettingsReadyText;

            Grilla.PagerStyle.PrevPageToolTip = Properties.Resources.RadGridPagerStylePrevPageToolTip;
            Grilla.PagerStyle.PrevPagesToolTip = Properties.Resources.RadGridPagerStylePrevPagesToolTip;
            Grilla.PagerStyle.NextPageToolTip = Properties.Resources.RadGridPagerStyleNextPageToolTip;
            Grilla.PagerStyle.NextPagesToolTip = Properties.Resources.RadGridPagerStyleNextPagesToolTip;

            Grilla.MasterTableView.CommandItemSettings.AddNewRecordText = Properties.Resources.RadGridMasterTableViewCommandItemSettingsAddNewRecordText;
            Grilla.MasterTableView.CommandItemSettings.RefreshText = Properties.Resources.RadGridMasterTableViewCommandItemSettingsRefreshText;

            Grilla.MasterTableView.EditFormSettings.EditColumn.CancelText = Properties.Resources.RadGridMasterTableViewEditFormSettingsEditColumnCancelText;
            Grilla.MasterTableView.EditFormSettings.EditColumn.EditText = Properties.Resources.RadGridMasterTableViewEditFormSettingsEditColumEditText;
            Grilla.MasterTableView.EditFormSettings.EditColumn.FilterImageToolTip = Properties.Resources.RadGridMasterTableViewEditFormSettingsEditColumnFilterImageToolTip;
            Grilla.MasterTableView.EditFormSettings.EditColumn.InsertText = Properties.Resources.RadGridMasterTableViewEditFormSettingsEditColumnInsertText;
            Grilla.MasterTableView.EditFormSettings.EditColumn.UpdateText = Properties.Resources.RadGridMasterTableViewEditFormSettingsEditColumnUpdateText;

            Grilla.MasterTableView.PagerStyle.PagerTextFormat = Properties.Resources.RadGridMasterTableViewPagerStylePagerTextFormat;

            GridFilterMenu Menu = Grilla.FilterMenu;
            //RadMenuItem item = default(RadMenuItem);
            foreach (RadMenuItem item in Menu.Items)
            {
                switch (item.Text)
                {
                    case "StartsWith":
                        item.Text = "Comienza por";
                        break;
                    case "NoFilter":
                        item.Text = "Sin filtro";
                        break;
                    case "EqualTo":
                        item.Text = "Igual a";
                        break;
                    case "NotEqualTo":
                        item.Text = "Diferente de";
                        break;
                    case "GreaterThan":
                        item.Text = "Mayor que";
                        break;
                    case "LessThan":
                        item.Text = "Menor que";
                        break;
                    case "GreaterThanOrEqualTo":
                        item.Text = "Mayor o igual que";
                        break;
                    case "LessThanOrEqualTo":
                        item.Text = "Menor o igual que";
                        break;
                    case "IsNull":
                        item.Text = "Es nulo";
                        break;
                    case "NotIsNull":
                        item.Text = "No es nulo";
                        break;
                    case "Contains":
                        item.Text = "Contiene";
                        break;
                    case "DoesNotContain":
                        item.Text = "No Contiene";
                        break;
                    case "EndsWith":
                        item.Text = "Termina en";
                        break;
                    case "Between":
                        item.Text = "Entre";
                        break;
                    case "NotBetween":
                        item.Text = "No entre";
                        break;
                    case "IsEmpty":
                        item.Text = "Es vacio";
                        break;
                    case "NotIsEmpty":
                        item.Text = "No es vacio";
                        break;
                }
            }
        }

        public static string HexToString(byte[] bytes)
        {
            string hexString = "";
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }

        public static int RandomNumber(int MaxNumber, int MinNumber = 0)
        {
            //initialize random number generator
            Random r = new Random(System.DateTime.Now.Millisecond);

            //if passed incorrect arguments, swap them
            //can also throw exception or return 0

            if (MinNumber > MaxNumber)
            {
                int t = MinNumber;
                MinNumber = MaxNumber;
                MaxNumber = t;
            }

            return r.Next(MinNumber, MaxNumber);
        }

        public static byte[] HexGetBytes(string hexString, [System.Runtime.InteropServices.Out()] int discarded)
        {
            discarded = 0;
            string newString = "";
            char c = '\0';
            // remove all none A-F, 0-9, characters
            for (int i = 0; i <= hexString.Length - 1; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                {
                    newString += c;
                }
                else
                {
                    discarded += 1;
                }
            }

            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded += 1;
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            byte[] tempBytes = new byte[9];
            string hex = null;
            int j = 0;

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                hex = new string(new char[] {
            newString[j],
            newString[j + 1]
        });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        public static bool IsHexDigit(char c)
        {
            int numChar = 0;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
            {
                return true;
            }
            if (numChar >= num1 && numChar < (num1 + 10))
            {
                return true;
            }
            return false;
        }

        public static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
            {
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            }
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }

        #region METODOS DE UTILIDADES DEL SISTEMA
        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public string GetHashPassword(string strLogin, string strPassword)
        {
            string _Hash = null;
            try
            {
                byte[] result = null;
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                result = sha.ComputeHash(encoding.GetBytes(strLogin.ToString().Trim() + strPassword.ToString().Trim()));
                _Hash = HexToString(result);
                sha.Clear();
            }
            catch (Exception ex)
            {
                _Hash = null;
                _log.Error("Error al generar el Hash del Password. Para el Login [" + strLogin.ToString().Trim() + "]. Motivo: " + ex.Message.ToString().Trim());
            }

            return _Hash.ToString().Trim();
        }

        public string GetHashPassword(string strPassword)
        {
            string _Hash = null;
            try
            {
                byte[] result = null;
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                result = sha.ComputeHash(encoding.GetBytes(strPassword.ToString().Trim()));
                _Hash = HexToString(result);
                sha.Clear();
            }
            catch (Exception ex)
            {
                _Hash = null;
                _log.Error("Error al generar el Hash del Password. Motivo: " + ex.Message.ToString().Trim());
            }

            return _Hash.ToString().Trim();
        }

        public string GetRandom()
        {
            string strRandom = "";
            try
            {
                Random rnd = new Random();
                strRandom = Convert.ToString(rnd.Next(1, 9999999));
            }
            catch (Exception ex)
            {
                strRandom = "";
                _log.Error("Error al generar el Random del Password. Motivo: " + ex.Message.ToString().Trim());
            }

            return strRandom.ToString().Trim();
        }

        public string GetTokenRandom(int iStart, int iEnd)
        {
            int iRandomValue = 0;
            try
            {
                Random rnd = new Random();
                Int32 numRandom = rnd.Next(iStart, iEnd);

                //iRandomValue = iStart + (numRandom * (iEnd - iStart));
                iRandomValue = numRandom;
            }
            catch (Exception ex)
            {
                iRandomValue = 0;
                _log.Error("Error al generar el Random del Token. Motivo: " + ex.Message.ToString().Trim());
            }

            return iRandomValue.ToString().Trim().PadRight(6, '0');
        }

        public string GetSerialPC()
        {
            string _SerialPC = "";
            try
            {
                string _TipoProcesador = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER").ToString().Trim();
                string _CantProcesadores = Environment.ProcessorCount.ToString().Trim();
                string _NombrePC = Environment.MachineName.ToString().Trim();
                string _VersionOS = Environment.OSVersion.ToString().Trim();
                string _Dominio = Environment.UserDomainName.ToString().Trim();
                string _UserName = Environment.UserName.ToString().Trim();

                _SerialPC = _NombrePC;
            }
            catch (Exception ex)
            {
                _SerialPC = "";
                _log.Error("Error al Obtener el Tipo Procesador. Motivo: " + ex.Message.ToString().Trim());
            }

            return _SerialPC;
        }

        public string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH &&
                    !tempMac.Contains("000000"))
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }

        #endregion

        #region METODOS PARA EL TRATAMIENTO DE IMAGEN
        //Método para Ridemencionar la Imagen con el Tamaño y Cortarla
        public Boolean GetRedimencionar(string strImagenOld, string strNuevaImagen)
        {
            bool Result = false;
            try
            {
                //VALIDAR QUE NO EXISTA EL ARCHIVO A GUARDAR
                if (File.Exists(strNuevaImagen))
                {
                    try
                    {
                        File.Delete(strNuevaImagen);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Error al eliminar la imagen. Motivo: " + ex.Message.ToString().Trim());
                        return false;
                    }
                }

                //RENOMBRAR IMAGEN
                System.IO.File.Move(strImagenOld, strNuevaImagen);
                Bitmap MapaBits = new Bitmap(strNuevaImagen);

                Bitmap MapaBits_dest = new Bitmap(90, 70);
                Size ObjTamano = new Size();

                if (MapaBits.Width != 100 || MapaBits.Height != 100)
                {
                    if (MapaBits.Width > MapaBits.Height)
                    {
                        ObjTamano.Width = 90;
                        ObjTamano.Height = 90;
                        ObjTamano = TamanoProporcional(MapaBits.Size, ObjTamano);
                    }
                    else
                    {
                        ObjTamano.Width = 90;
                        ObjTamano.Height = 90;
                        ObjTamano = TamanoProporcional(MapaBits.Size, ObjTamano);
                    }

                    MapaBits_dest = new Bitmap(ObjTamano.Width, 90);
                    Graphics gr_dest = Graphics.FromImage(MapaBits_dest);
                    gr_dest.DrawImage(MapaBits, 0, 0, MapaBits_dest.Width, MapaBits_dest.Height);
                    gr_dest = null;
                }
                else
                {
                    MapaBits_dest = new Bitmap(90, 90);
                    Graphics gr_dest = Graphics.FromImage(MapaBits_dest);
                    gr_dest.DrawImage(MapaBits, 0, 0, MapaBits_dest.Width, MapaBits_dest.Height);
                    gr_dest = null;
                }
                MapaBits.Dispose();
                MapaBits = null;

                if (MapaBits_dest.Width != MapaBits_dest.Height)
                {
                    MapaBits_dest = CortarImagen(MapaBits_dest, 0, 0, 90, 90);
                }

                //Guargar Imagen
                //---------------------------------
                MapaBits_dest.Save(strNuevaImagen, System.Drawing.Imaging.ImageFormat.Jpeg);
                MapaBits_dest = null;

                //Aqui hacemos el llamado de la funcion para convertir la Imagen a Binario
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                _log.Error("Error al dimencionar la Imagen [GetRedimencionar]. Motivo: " + ex.Message.ToString().Trim());
            }

            return Result;
        }

        //Método para Ridemencionar la Imagen deja el tamaño original de la imagen
        public Boolean GetRedimencionar(Image strPathImagenOriginal, string strNombreImgen)
        {
            bool Result = false;
            try
            {
                //RUTA DEL DIRECTORIO TEMPORAL
                String DirTmp = Path.GetTempPath() + @"\" + strNombreImgen + ".gif";

                //IMAGEN ORIGINAL A REDIMENCIONAR 
                Bitmap imagen = new Bitmap(strPathImagenOriginal);

                //CREAMOS UN MAPA DE BIT CON LAS DIMENCIONES QUE QUEREMOS PARA LA NUEVA IMAGEN
                Bitmap NuevaImagen = new Bitmap(strPathImagenOriginal.Width, strPathImagenOriginal.Height);

                //CREAMOS UN NUEVO GRAFICO
                Graphics gr = Graphics.FromImage(NuevaImagen);

                //DIBUJAMOS LA NUEVA IMAGEN
                gr.DrawImage(imagen, 0, 0, NuevaImagen.Width, NuevaImagen.Height);

                //LIBERAMOS RECURSOS
                gr.Dispose();

                //GUARDAMOS LA NUEVA IMAGEN ESPECIFICAMOS LA RUTA Y EL FORMATO
                NuevaImagen.Save(DirTmp, System.Drawing.Imaging.ImageFormat.Gif);

                //LIBERAMOS RECURSOS
                NuevaImagen.Dispose();
                Result = true;
            }
            catch (Exception ex)
            {
                Result = false;
                _log.Error("Error al dimencionar la Imagen [GetRedimencionar]. Motivo: " + ex.Message.ToString().Trim());
            }

            return Result;
        }

        //Método para convertir la Imagen a Bytes
        public Byte[] GetImagenBytes(string strRutaImagen)
        {
            Byte[] Arreglo = null;
            try
            {
                FileStream Imagen = new FileStream(strRutaImagen, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                Arreglo = new Byte[Imagen.Length];
                BinaryReader reader = new BinaryReader(Imagen);
                Arreglo = reader.ReadBytes(Convert.ToInt32(Imagen.Length));
                reader.Close();
            }
            catch (Exception ex)
            {
                //Aqui hacemos el llamado de la Función que guardar los Logs de Errores que suceden en el sistema
                _log.Error("Error al pasar la Imagen a Binario. Motivo: " + ex.Message.ToString().Trim());
            }

            return Arreglo;
        }

        //Método para convertir de Byte a Imagen
        public Image GetByteImagen(Byte[] ImgBytes)
        {
            Bitmap Imagen = null;
            try
            {
                Byte[] bytes = (Byte[])(ImgBytes);
                MemoryStream ms = new MemoryStream(bytes);
                Imagen = new Bitmap(ms);
            }
            catch (Exception ex)
            {
                Imagen = null;
                _log.Error("Error al obtener la imagen [GetByteImagen]. Motivo: " + ex.Message.ToString().Trim());
            }

            return Imagen;
        }

        //Método para recortar la imagen
        protected Bitmap CortarImagen(Bitmap MapadeBits, int X, int Y, int Ancho, int Alto)
        {
            try
            {
                Bitmap _MapadeBits = new Bitmap(MapadeBits);
                Rectangle rectangulo = new Rectangle(X, Y, Ancho, Alto);
                cortado = _MapadeBits.Clone(rectangulo, _MapadeBits.PixelFormat);
                //cortado = cloneBitmap;
            }
            catch (Exception ex)
            {
                _log.Error("Error con la función CortarImagen. Motivo: " + ex.Message.ToString().Trim());
            }

            return cortado;
        }

        //Método para fijarle el tamaño de la imagen
        protected Size TamanoProporcional(Size TamanoImagen, Size MaxDimension)
        {
            try
            {
                int ViejoAncho = 0;
                int ViejoAlto = 0;
                int NuevoAncho = 0;
                int NuevoAlto = 0;
                int AspectRatio = 0;

                ViejoAncho = TamanoImagen.Width;
                ViejoAlto = TamanoImagen.Height;

                NuevoAncho = MaxDimension.Width;
                NuevoAlto = MaxDimension.Height;

                if (NuevoAncho != 0)
                {
                    AspectRatio = ((NuevoAncho * 100) / ViejoAncho) / 200;
                    NuevoAlto = ViejoAlto * AspectRatio;
                }
                else
                {
                    AspectRatio = ((NuevoAlto * 100) / ViejoAlto) / 200;
                    NuevoAncho = ViejoAncho * AspectRatio;
                }

                TamanoImagen = new Size(Convert.ToInt32(NuevoAncho), Convert.ToInt32(NuevoAlto));
            }
            catch (Exception ex)
            {
                _log.Error("Error en la funcion TamanoProporcional. Motivo: " + ex.Message.ToString().Trim());
            }

            return TamanoImagen;
        }
        #endregion

        public string GetTime(int nHora)
        {
            string ResultTime = "";
            try
            {
                if (nHora <= 12)
                {
                    ResultTime = "Buenos Días";
                }
                else if (nHora <= 18)
                {
                    ResultTime = "Buenas Tarde";
                }
                else
                {
                    ResultTime = "Buenas Noche";
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error al obtener el Time. Motivo: " + ex.Message);
            }

            return ResultTime;
        }

        //Metodo para pasar de CSV a Datatable
        public DataTable GetEtl(string strPathFile, char[] _Separador, ref string _MsgError)
        {
            DataTable dtEtl = new DataTable();
            dtEtl.TableName = "DtDatos";
            int _ContadorFilas = 0;
            try
            {
                StreamReader sr = new StreamReader(strPathFile);
                //string[] headers = sr.ReadLine().Split(';');
                //char[] delimiters = new char[] { '\t' };
                string[] headers = sr.ReadLine().Split(_Separador, StringSplitOptions.RemoveEmptyEntries);
                //string[] headers = sr.ReadLine().Split(strSeparador);
                //string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                //Creamos el encabezado del DataTable
                foreach (string dtColumn in headers)
                {
                    dtEtl.Columns.Add(dtColumn.ToString().Trim());
                }

                //Insertamos los datos al Datatable
                string[] csvRows = File.ReadAllLines(strPathFile);
                string[] fields = null;
                int ContadorRow = 0;
                foreach (string csvRow in csvRows)
                {
                    try
                    {
                        //fields = csvRow.Split(_Separador, StringSplitOptions.RemoveEmptyEntries);
                        fields = csvRow.Split(_Separador);
                        if (fields.Length > 0)
                        {
                            //--AQUI VALIDAMOS QUE CODIGO DE OFICINA Y # DE CUENTA VENGAN LLENOS
                            if (fields[0].ToString().Trim().Length > 0 && fields[1].ToString().Trim().Length > 0)
                            {
                                if (ContadorRow != 0)
                                {
                                    try
                                    {
                                        DataRow ItemRow = dtEtl.NewRow();
                                        ItemRow.ItemArray = fields;
                                        dtEtl.Rows.Add(ItemRow);
                                        _ContadorFilas++;
                                    }
                                    catch (Exception ex)
                                    {
                                        _MsgError = "1. Error al obtener los datos de la fila [" + _ContadorFilas + "] del archivo en el proceso de ETL. Motivo: " + ex.Message;
                                        _log.Error(_MsgError);
                                        return dtEtl;
                                    }
                                }
                            }
                            ContadorRow++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _MsgError = "2. Error al realizar el proceso ETL. Motivo: " + ex.Message;
                        _log.Error(_MsgError);
                        return dtEtl;
                    }
                }

                _MsgError = "";
                sr.Close();
            }
            catch (Exception ex)
            {
                dtEtl = null;
                _MsgError = "3. Error al realizar el proceso ETL. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }

            return dtEtl;
        }

        //Metodo para pasar de CSV a Datatable
        public DataTable GetEtl(string strPathFile, char strSeparador, ref string _MsgError)
        {
            DataTable dtEtl = new DataTable();
            dtEtl.TableName = "DtDatos";
            try
            {
                StreamReader sr = new StreamReader(strPathFile);
                //string[] headers = sr.ReadLine().Split(';');
                string[] headers = sr.ReadLine().Split(strSeparador);

                //Creamos el encabezado del DataTable
                foreach (string dtColumn in headers)
                {
                    dtEtl.Columns.Add(dtColumn.ToString().Trim());
                }

                //Insertamos los datos al Datatable
                string[] csvRows = File.ReadAllLines(strPathFile);
                string[] fields = null;
                int ContadorRow = 0;
                foreach (string csvRow in csvRows)
                {
                    fields = csvRow.Split(strSeparador);

                    if (ContadorRow != 0)
                    {
                        DataRow ItemRow = dtEtl.NewRow();
                        ItemRow.ItemArray = fields;
                        dtEtl.Rows.Add(ItemRow);
                    }
                    ContadorRow++;
                }

                _MsgError = "";
                sr.Close();
            }
            catch (Exception ex)
            {
                dtEtl = null;
                _MsgError = "Error al realizar el proceso ETL. Motivo: " + ex.Message;
                _log.Error(_MsgError);
            }

            return dtEtl;
        }

        #region DEFINICION DE METODOS PARA ENCRIPTAR Y DESENCRIPTAR
        private static String key = "012345678901234567890123";

        public string Encrypt(String stringToEncrypt)
        {
            byte[] output = null;
            try
            {
                TripleDES des = CreateDES(key);
                ICryptoTransform ct = des.CreateEncryptor();
                byte[] input = Encoding.Unicode.GetBytes(stringToEncrypt);
                output = ct.TransformFinalBlock(input, 0, input.Length);
            }
            catch (Exception ex)
            {
                _log.Error("Error con el Metodo [Encrypt]. Motivo: " + ex.Message);
            }
            //return output;
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string encryptedString)
        {
            byte[] output = null;
            try
            {
                byte[] input = Convert.FromBase64String(encryptedString);
                TripleDES des = CreateDES(key);
                ICryptoTransform ct = des.CreateDecryptor();
                output = ct.TransformFinalBlock(input, 0, input.Length);
            }
            catch (Exception ex)
            {
                _log.Error("Error con el Metodo [Decrypt]. Motivo: " + ex.Message);
            }

            return Encoding.Unicode.GetString(output);
        }

        public TripleDES CreateDES(string key)
        {
            TripleDES des = new TripleDESCryptoServiceProvider();
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                des = new TripleDESCryptoServiceProvider();
                des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
                des.IV = new byte[des.BlockSize / 8];
            }
            catch (Exception ex)
            {
                _log.Error("Error con el Metodo [CreateDES]. Motivo: " + ex.Message);
            }

            return des;
        }
        #endregion

        public string GetLimpiarCadena(string _Cadena)
        {
            string _Result = "";
            try
            {
                _Result = _Cadena.ToString().Trim().ToUpper().Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N").Replace("(", "").Replace(")", "").Replace("*", "").Replace("°", "").Replace("-", " ").Replace(";", "").Replace(".", "").Replace(",", "").Replace("¿", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("=", "").Replace("&", "").Replace("%", "").Replace("$", "").Replace("#", "").Replace("\"", "").Replace("!", "").Replace("'", "").Replace("/", "").Replace("\"", "");
                //_Result = _Cadena.ToString().Trim().Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");
            }
            catch (Exception ex)
            {
                _Result = _Cadena;
            }

            return _Result;
        }

        public string GetLimpiarCadena2(string _Cadena)
        {
            string _Result = "";
            try
            {
                _Result = _Cadena.ToString().Trim().Replace("Á", "A").Replace("É", "E").Replace("Í", "I").Replace("Ó", "O").Replace("Ú", "U").Replace("Ñ", "N");
            }
            catch (Exception ex)
            {
                _Result = _Cadena;
            }

            return _Result;
        }

        public String StringEncodingConvert(String strText, String strSrcEncoding, String strDestEncoding)
        {
            Encoding srcEnc = Encoding.GetEncoding(strSrcEncoding);
            Encoding destEnc = Encoding.GetEncoding(strDestEncoding);
            byte[] bData = srcEnc.GetBytes(strText);
            byte[] bResult = Encoding.Convert(srcEnc, destEnc, bData);
            //--
            return destEnc.GetString(bResult);
        }

        public string GetFormatNumberConSigno(string _Valor)
        {
            string _Result = "";
            try
            {
                double _ValorResult = _Valor.ToString().Trim().Length > 0 ? Double.Parse(_Valor.ToString().Trim().Replace(FixedData.SeparadorMilesAp, "")) : 0;
                _Result = String.Format(String.Format("{0:$ ###,###,##0}", _ValorResult));
            }
            catch (Exception ex)
            {
                _Result = String.Format(String.Format("{0:$ ###,###,##0}", 0));
            }

            return _Result;
        }

        public string GetFormatNumberSinSigno(string _Valor)
        {
            string _Result = "";
            try
            {
                double _ValorResult = _Valor.ToString().Trim().Length > 0 ? Double.Parse(_Valor.ToString().Trim().Replace(FixedData.SeparadorMilesAp, "")) : 0;
                _Result = String.Format(String.Format("{0:###,###,##0}", _ValorResult));
            }
            catch (Exception ex)
            {
                _Result = String.Format(String.Format("{0:###,###,##0}", 0));
            }

            return _Result;
        }

        public static double round(double input)
        {
            double _Result = 0;
            double rem = input % 1000;
            //return rem >= 5 ? (num - rem + 10) : (num - rem);
            if (rem >= 500)
            {
                _Result = (double)(1000 * Math.Ceiling(input / 1000));
            }
            else
            {
                _Result = (double)(1000 * Math.Round(input / 1000));
            }

            return _Result;
        }

    }
}