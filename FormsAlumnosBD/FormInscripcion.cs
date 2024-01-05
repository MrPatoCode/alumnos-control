using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsAlumnosBD
{
    public partial class FormInscripcion : Form
    {
        Data.CData Data = new Data.CData();

        string sentencia, modo, id;
        string idAlum, idCurs, anio, numsms;
        DataTable dtA, dtC;
        int indxDg;

        public FormInscripcion()
        {
            InitializeComponent();
        }

        private void FormInscripcion_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy";
            FillCmbx();
            ResetForm();
        }

        private void FormInscripcion_FormClosing(object sender, FormClosingEventArgs e)
        {
            string mensaje = "Esta ventana se cerrará.  ¿Desea realizar la accion?";
            string caption = "Cerrar formulario";
            var resultado = MessageBox.Show(mensaje, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(FormMenu))
                    {
                        form.BringToFront();
                        form.Show();
                        return;
                    }
                }
                e.Cancel = false;
            }
            else
            {
                if (e.CloseReason == CloseReason.UserClosing)
                    e.Cancel = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                indxDg = e.RowIndex;

                id = dataGridView1.CurrentRow.Cells["id"].Value.ToString();
                cmbBxMatric.Text = dataGridView1.CurrentRow.Cells["matricula"].Value.ToString();
                cmbBxCurs.Text = dataGridView1.CurrentRow.Cells["abreviatura"].Value.ToString();
                string semestre = dataGridView1.CurrentRow.Cells["semestre"].Value.ToString();
                Semestre(semestre);
            }
        }

        private void cmbBxMatric_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNombre.Text = dtA.Rows[cmbBxMatric.SelectedIndex]["nombre"].ToString();
            lblNombre.Text += " " + dtA.Rows[cmbBxMatric.SelectedIndex]["apellido"].ToString();
        }

        private void cmbBxCurs_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCurso.Text = dtC.Rows[cmbBxCurs.SelectedIndex]["descripcion"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Salir")
                this.Close();
            else
                ResetForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Nuevo")
            {
                modo = "insertar";
                button2.Text = "Guardar";
                button1.Text = "Cancelar";

                button3.Enabled = false;
                button4.Enabled = false;

            }
            else if (button2.Text == "Guardar")
            {
                ObtData();

                if (modo == "editar")
                {
                    if (indxDg >= 0)
                    {
                        string mensaje = "¿Estás seguro de actualizar los datos?";
                        string caption = "Actualizar los datos";

                        if (Respuesta(mensaje, caption))
                        {
                            ActualizarData();
                            MessageBox.Show("Se han actualizado los datos correctamente", "Actualización de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetForm();
                        }
                    }
                    else
                        MessageBox.Show("Seleccione una celda de la tabla", "No se puede realizar la operación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (modo == "insertar")
                {
                    string mensaje = "¿Estás seguro de guardar los datos?";
                    string caption = "Guardar datos";

                    if (Respuesta(mensaje, caption))
                    {
                        InsertarData();
                        MessageBox.Show("Se han guardado los datos correctamente", "Actualización de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetForm();
                    }
                }
            }
            else
                MessageBox.Show("Ingrese los datos de la inscripción", "No datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            modo = "editar";

            button4.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;

            button1.Text = "Cancelar";
            button2.Text = "Guardar";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string mensaje = "¿Estás seguro de eliminar este registro?";
            string caption = "Eliminar registro";

            if (indxDg >= 0)
            {
                if (Respuesta(mensaje, caption))
                {
                    EliminarData();
                    MessageBox.Show("Se han eliminado los datos correctamente", "Eliminación de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Seleccione una celda de la tabla", "No se puede realizar la operación", MessageBoxButtons.OK, MessageBoxIcon.Error);

            ResetForm();
        }

        private void FillCmbx()
        {
            sentencia = "SELECT * FROM alumnos";
            dtA = Data.GetData(sentencia);
            cmbBxMatric.DataSource = dtA;
            cmbBxMatric.DisplayMember = "matricula";
            cmbBxMatric.ValueMember = "Id";

            sentencia = "SELECT * FROM cursos";
            dtC = Data.GetData(sentencia);
            cmbBxCurs.DataSource = dtC;
            cmbBxCurs.DisplayMember = "abreviatura";
            cmbBxCurs.ValueMember = "Id";
        }

        private void ResetForm()
        {
            sentencia = "SELECT  inscripciones.Id, alumnos.matricula, cursos.abreviatura,  inscripciones.semestre FROM  alumnos, cursos, inscripciones WHERE  alumnos.Id = inscripciones.idAlumno AND inscripciones.idCurso = cursos.Id";
            dataGridView1.DataSource = Data.GetData(sentencia);
            dataGridView1.Columns["id"].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Matricula";
            dataGridView1.Columns[2].HeaderText = "Curso";
            dataGridView1.Columns[3].HeaderText = "Semestre";

            cmbBxSmst.SelectedIndex = 0;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;

            button1.Text = "Salir";
            button2.Text = "Nuevo";
            button3.Text = "Editar";
            button4.Text = "Eliminar";

            indxDg = -1;
        }

        private void Semestre(string semestre)
        {
            anio = "";
            for (int i = 0; i < 4; i++)
            {
                anio += semestre[i].ToString();
            }
            dateTimePicker1.Value = Convert.ToDateTime("01/01/" + anio);

            numsms = Convert.ToString(semestre[4]);
            cmbBxSmst.Text = numsms;
        }

        private bool Respuesta(string mensaje, string caption)
        {
            bool bl;

            var resultado = MessageBox.Show(mensaje, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
                bl = true;
            else
                bl = false;

            return bl;
        }

        private string GetId(string table, string column, string valor)
        {
            if(table == "alumnos")
                sentencia = "SELECT id, " + column + " FROM " + table + " WHERE " + column + " =" + valor + ";";
            else
                sentencia = "SELECT id, " + column + " FROM " + table + " WHERE " + column + " ='" + valor + "';";

            DataTable dt = Data.GetData(sentencia);
            string id = dt.Rows[0]["id"].ToString();

            return id;
        }
        private void ObtData()
        {
            idAlum = GetId("alumnos", "matricula", cmbBxMatric.Text);
            idCurs = GetId("cursos", "abreviatura", cmbBxCurs.Text);
            numsms = dateTimePicker1.Value.ToString("yyyy");
            numsms += cmbBxSmst.Text.ToString();
        }

        private void ActualizarData()
        {
            sentencia = $"UPDATE inscripciones SET idAlumno = '{idAlum}', idCurso = '{idCurs}', semestre = '{numsms}' WHERE id = {id}";
            Data.WriteData(sentencia);
        }

        private void InsertarData()
        {
            sentencia = $"INSERT INTO inscripciones (idAlumno, idCurso, semestre) VALUES ('{idAlum}', '{idCurs}', '{numsms}')";
            Data.WriteData(sentencia);
        }

        private void EliminarData()
        {
            sentencia = $"DELETE FROM inscripciones WHERE Id = {id}";
            Data.WriteData(sentencia);
        }
    }
}
