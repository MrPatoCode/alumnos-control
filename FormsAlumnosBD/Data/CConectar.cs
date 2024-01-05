using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAlumnosBD.Data
{
    internal class CConectar
    {
        public static OleDbConnection Conectar()
        {
            try
            {
                OleDbConnection Conexion;
                string sPath = System.Windows.Forms.Application.StartupPath;
                string strConexion = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + sPath + "\\alumnosV1.accdb; Jet OLEDB:Database Password = 1234";
                Conexion = new OleDbConnection(strConexion);
                return Conexion;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
