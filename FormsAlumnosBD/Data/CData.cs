using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAlumnosBD.Data
{
    internal class CData
    {
        private OleDbConnection conexion = CConectar.Conectar();

        public DataTable GetData(string sentencia)
        {
            try
            {
                DataTable dt = new DataTable();
                conexion.Open();
                OleDbCommand comando = new OleDbCommand();
                comando.Connection = conexion;
                comando.CommandText = sentencia;
                OleDbDataAdapter da = new OleDbDataAdapter(comando);
                da.Fill(dt);
                conexion.Close();
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

        }

        public void WriteData(string sentencia)
        {
            try
            {
                conexion.Open();
                OleDbCommand comando = new OleDbCommand();
                comando.Connection = conexion;
                comando.CommandText = sentencia;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
