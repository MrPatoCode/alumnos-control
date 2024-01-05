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
    public partial class FormCursos : Form
    {
        Data.CData Data = new Data.CData();

        string sentencia, modo, id;
        string abrev, descri;
        int cred;
        int indxDg;

        public FormCursos()
        {
            InitializeComponent();
        }

        private void FormCursos_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void FormCursos_FormClosing(object sender, FormClosingEventArgs e)
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
                txtBxAbrevia.Text = dataGridView1.CurrentRow.Cells["abreviatura"].Value.ToString();
                txtBxDescrip.Text = dataGridView1.CurrentRow.Cells["descripcion"].Value.ToString();
                txtBxCredit.Text = dataGridView1.CurrentRow.Cells["creditos"].Value.ToString();
            }
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

                txtBxAbrevia.ReadOnly = false;
                txtBxDescrip.ReadOnly = false;
                txtBxCredit.ReadOnly = false;

                button3.Enabled = false;
                button4.Enabled = false;

            }
            else if (txtBxAbrevia.Text != "" & txtBxDescrip.Text != "" & txtBxCredit.Text != "" & button2.Text == "Guardar")
            {
                ObtData();
                string abrv = txtBxAbrevia.Text;

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

                    if (Respuesta(mensaje, caption) & abrev.Length == 4)
                    {
                        InsertarData();
                        MessageBox.Show("Se han guardado los datos correctamente", "Actualización de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetForm();
                    }
                    else
                        MessageBox.Show("La abreviatura no tiene 4 letras", "No abreviatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Ingrese los datos del curso", "No datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            modo = "editar";

            txtBxAbrevia.ReadOnly = false;
            txtBxDescrip.ReadOnly = false;
            txtBxCredit.ReadOnly = false;

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
                MessageBox.Show("Seleccione una celda de la tabla", "No se puede realizar la operación", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ResetForm();
        }

        private void txtBxAbrevia_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtBxAbrevia.CharacterCasing = CharacterCasing.Upper;
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsLetter(e.KeyChar);
        }

        private void txtBxDescrip_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsLetter(e.KeyChar);
        }

        private void txtBxCredit_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void ResetForm()
        {
            sentencia = "SELECT * FROM cursos";
            dataGridView1.DataSource = Data.GetData(sentencia);
            dataGridView1.Columns["id"].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Abreviatura";
            dataGridView1.Columns[2].HeaderText = "Descripción";
            dataGridView1.Columns[3].HeaderText = "Créditos";

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;

            button1.Text = "Salir";
            button2.Text = "Nuevo";
            button3.Text = "Editar";
            button4.Text = "Eliminar";

            txtBxAbrevia.ReadOnly = true;
            txtBxDescrip.ReadOnly = true;
            txtBxCredit.ReadOnly = true;

            txtBxAbrevia.Text = "";
            txtBxDescrip.Text = "";
            txtBxCredit.Text = "";

            indxDg = -1;

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

        private void ObtData()
        {
            abrev = txtBxAbrevia.Text.ToString();
            descri = txtBxDescrip.Text.ToString();
            cred = int.Parse(txtBxCredit.Text.ToString());
        }
        private void ActualizarData()
        {
            sentencia = $"UPDATE cursos SET abreviatura = '{abrev}', descripcion = '{descri}', creditos = {cred} WHERE id = {id}";
            Data.WriteData(sentencia);
        }

        private void InsertarData()
        {
            sentencia = $"INSERT INTO cursos (abreviatura, descripcion, creditos) VALUES ('{abrev}', '{descri}', {cred})";
            Data.WriteData(sentencia);
        }

        private void EliminarData()
        {
            sentencia = $"DELETE FROM cursos WHERE Id = {id}";
            Data.WriteData(sentencia);
        }
    }
}
