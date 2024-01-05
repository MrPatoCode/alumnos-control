using FormsAlumnosBD.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FormsAlumnosBD
{
    public partial class FormAlumnos : Form
    {
        Data.CData Data = new Data.CData(); 

        string sentencia, modo, id;
        string nombre, apellido , fecha;
        int matricula;
        int indxDg;

        public FormAlumnos()
        {
            InitializeComponent();
        }

        private void FormAlumnos_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void FormAlumnos_FormClosing(object sender, FormClosingEventArgs e)
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

                txtBxNombre.Text = dataGridView1.CurrentRow.Cells["nombre"].Value.ToString();
                txtBxApellido.Text = dataGridView1.CurrentRow.Cells["apellido"].Value.ToString();
                txtBxMatri.Text = dataGridView1.CurrentRow.Cells["matricula"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["fechaNac"].Value);
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

                txtBxNombre.ReadOnly = false;
                txtBxApellido.ReadOnly = false;
                txtBxMatri.ReadOnly = false;

                button3.Enabled = false;
                button4.Enabled = false;

            }
            else if (txtBxNombre.Text != "" & txtBxApellido.Text != "" & txtBxMatri.Text != "" & button2.Text == "Guardar")
            {
                ObtData();
                string mtrcl = Convert.ToString(matricula);

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

                    if (Respuesta(mensaje, caption) & mtrcl.Length == 6)
                    {
                        InsertarData();
                        MessageBox.Show("Se han guardado los datos correctamente", "Actualización de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetForm();
                    }
                    else
                        MessageBox.Show("La matricula no tiene 6 digitos", "No matricula", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
                MessageBox.Show("Ingrese los datos del alumno", "No datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            modo = "editar";

            button4.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;

            txtBxNombre.ReadOnly = false;
            txtBxApellido.ReadOnly = false;
            txtBxMatri.ReadOnly = false;

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

        private void txtBxNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtBxNombre.CharacterCasing = CharacterCasing.Upper;
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsLetter(e.KeyChar);
        }

        private void txtBxApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtBxApellido.CharacterCasing = CharacterCasing.Upper;
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsLetter(e.KeyChar);
        }

        private void txtBxMatri_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void ResetForm()
        {
            sentencia = "SELECT * FROM alumnos";
            dataGridView1.DataSource = Data.GetData(sentencia);
            dataGridView1.Columns["id"].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Nombre";
            dataGridView1.Columns[2].HeaderText = "Apellido";
            dataGridView1.Columns[3].HeaderText = "Matricula";
            dataGridView1.Columns[4].HeaderText = "Fch. Nacimiento";

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;

            button1.Text = "Salir";
            button2.Text = "Nuevo";
            button3.Text = "Editar";
            button4.Text = "Eliminar";

            txtBxNombre.ReadOnly = true;
            txtBxApellido.ReadOnly = true;
            txtBxMatri.ReadOnly = true;

            txtBxNombre.Text = "";
            txtBxApellido.Text = "";
            txtBxMatri.Text = "";

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
            nombre = txtBxNombre.Text.ToString();
            apellido = txtBxApellido.Text.ToString();
            matricula = int.Parse(txtBxMatri.Text);
            fecha = dateTimePicker1.Value.ToString("MM/dd/yyyy");
        }

        private void ActualizarData()
        {
            sentencia = $"UPDATE alumnos SET nombre = '{nombre}', apellido = '{apellido}', matricula = {matricula}, fechaNac = #{fecha}# WHERE id = {id}";
            Data.WriteData(sentencia);
        }

        private void InsertarData()
        {
            sentencia = $"INSERT INTO alumnos (nombre, apellido, matricula, fechaNac) VALUES ('{nombre}', '{apellido}', {matricula}, #{fecha}#)";
            Data.WriteData(sentencia);
        }

        private void EliminarData()
        {
            sentencia = $"DELETE FROM alumnos WHERE Id = {id}";
            Data.WriteData(sentencia);
        }
    }
}
