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
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void inscripcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInscripcion formInscripcion = new FormInscripcion();
            formInscripcion.Show();
            this.Hide();
        }

        private void alumnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAlumnos formAlumnos = new FormAlumnos();
            formAlumnos.Show();
            this.Hide();
        }

        private void cursosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCursos formCursos = new FormCursos();
            formCursos.Show();
            this.Hide();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-Alumnos Control-\nVersión: 1.0 \nProgramador: Paulino M.", "Acerca del programa");
        }
    }
}
