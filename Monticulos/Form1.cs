using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System . Threading;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monticulos
{
    public partial class Form1 : Form
    {
        //Inicializacion de variables 
        int xo, yo, tam; //variable para valor inicial de x, de y y de tamaño 
        bool ec = false; //bandera  booleana en falso
        bool estado = false;//estado inicializado en falso
        int n = 0, i = 1;//inicializacion de variables

        int[] Arreglo_numeros;//arreglo de numeros ingresados
        Button[] Arreglo;//arreglo de botones para simular valores ingresadas

        //Inicializacion del formulario
        public Form1()
        {
            InitializeComponent();
            tam = tabPage1.Width / 2; //tam sera la mitad del ancho del tapbpage
            xo = tam;//el valor inicial de x sera la mitad del ancho del tabpage
            yo = 20;//el valor inicial en y sera de 20
            txtNumeros.Focus();//cursor en textbox
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int num = int.Parse(txtNumeros.Text);//capturar el valor ingresado

                Array.Resize<int>(ref Arreglo_numeros, i + 1);//incrementando el arreglo en base al nuevo valor ingresado
                Arreglo_numeros[i] = num;//asignamos ese valor a la posicion i del arreglo
                Array.Resize<Button>(ref Arreglo, i + 1);//incrementos el arreglo de botones
                Arreglo[i] = new Button();//creamos nuevo boton i
                Arreglo[i].Text = Arreglo_numeros[i].ToString();//texto del boton sera el valor ingresado de posicion i
                Arreglo[i].Height = 50;//alto e boton 50
                Arreglo[i].Width = 50;//ancho del boton 50
                Arreglo[i].BackColor = Color.GreenYellow;
                Arreglo[i].Location = new Point(xo, yo) + new Size(-20, 0);

                //para poder dibujar el arbol y crear los niveles

                if ((i+1)== Math.Pow(2,n+1))
                {
                    n++;
                    tam = tam / 2;
                    xo = tam;
                    yo += 60;
                }
                else
                {
                    xo += (2 * tam);

                }

                i++;
                estado = true;
                ec = false;
                tabPage1.Refresh();
                txtNumeros.Clear();
                txtNumeros.Focus();
                btnOrdenar_Click(null, null);


            }
            catch
            {
                MessageBox.Show("Valor no valido");
            }
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            if (estado)
            {
                try
                {
                    Dibujar_Arreglo(ref Arreglo, ref tabPage1);
                    dibujar_Ramas(ref Arreglo, ref tabPage1, e);
                }
                catch
                { }
                estado = false;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            n = 0;
            i = 1;
            tam = tabPage1.Width / 2;
            xo = tam;
            yo = 20;
            tabPage1.Controls.Clear();
            tabPage1.Refresh();
            Array.Resize<int>(ref Arreglo_numeros, 1);
            Array.Resize<Button>(ref Arreglo, 1);

        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            if (i == 1)
                MessageBox.Show("No hay elementos que ordenar");
            else
            {
                btnAgregar.Enabled = false;
                btnLimpiar.Enabled = false;
                btnOrdenarMax.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                if (!ec)
                {
                    Heap_Num();
                }
                else
                {
                    HPN();
                }
                btnAgregar.Enabled = true;
                btnLimpiar.Enabled = true;
                btnOrdenarMax.Enabled = true;
                this.Cursor = Cursors.Default;

            }
        }

        public void intercambio(ref Button[]botones, int a, int b)
        {
            string temp = botones[a].Text;

            Point pa = botones[a].Location;
            Point pb = botones[b].Location;

            int diferenciaX = Math.Abs(pa.X - pb.X);
            int diferenciaY = Math.Abs(pa.Y - pb.Y);

            int x = 10;
            int y = 10;
            int t = 70;
            while(y!= diferenciaY + 10)
            {
                Thread.Sleep(t);
                botones[a].Location += new Size(0, - 10);
                botones[b].Location += new Size(0, +10);
                y += 10;
            }
            while(x!= diferenciaX-diferenciaX % 10 + 10)
            {
                if (pa.X < pb.X)
                {
                    Thread.Sleep(t);
                    botones[a].Location += new Size(+10, 0);
                    botones[b].Location += new Size(-10, 0);
                    x += 10;
                }
                else
                {
                    Thread.Sleep(t);
                    botones[a].Location += new Size(-10, 0);
                    botones[b].Location += new Size(+10, 0);
                    x += 10;
                }
            }
            botones[a].Text = botones[b].Text;
            botones[b].Text = temp;
            botones[b].Location = pb;
            botones[a].Location = pa;
            estado = true;

            tabPage1.Refresh();


        }

        public void Dibujar_Arreglo(ref Button[]botones, ref TabPage tb)
        {
            for (int j = 1; j < botones.Length; j++)
            {
                tb.Controls.Add(this.Arreglo[j]);
            }
        }

        public void dibujar_Ramas(ref Button[]botones,ref TabPage tb, PaintEventArgs e)
        {
            Pen lapiz = new Pen(Color.Gray, 1.5f);
            Graphics g;
            g = e.Graphics;

            for (int j = 1; j < Arreglo.Length; j++)
            {
                if (Arreglo[(2 * j)] != null)
                    g.DrawLine(lapiz, Arreglo[j].Location.X, Arreglo[j].Location.Y + 20, Arreglo[(2 * j)].Location.X + 20, Arreglo[(2 * j)].Location.Y);
                if (Arreglo[(2 * j) + 1] != null)
                    g.DrawLine(lapiz, Arreglo[j].Location.X + 40, Arreglo[j].Location.Y + 20, Arreglo[(2 * j)+1].Location.X + 20, Arreglo[(2 * j)+1].Location.Y);
            }
        }

        public void Heap_Num()
        {
            ec = true;

            int x = Arreglo.Length;

            for (int i = (x) / 2; i > 0; i--)
                Max_Num(Arreglo_numeros, x, i, ref Arreglo);
        }

       

        public void HPN()
        {
            int temp;
            int x = Arreglo_numeros.Length;

            for(int i= x-1; i>=1; i--)
            {
                intercambio(ref Arreglo, i, 1);
                temp = Arreglo_numeros[i];
                Arreglo_numeros[i] = Arreglo_numeros[1];
                Arreglo_numeros[1] = temp;
                x--;
            }
        }
        public void Max_Num(int[] a, int x, int indice, ref Button[] botones)
        {
            int izquierdo = (indice) * 2;
            int derecho = (indice) * 2 + 1;
            int mayor = 0;

            if (izquierdo < x && a[izquierdo] > a[indice])
            {
                mayor = izquierdo;

            }
            else
            {
                mayor = indice;
            }
            if (derecho < x && a[derecho] > a[mayor])
            {
                mayor = derecho;
            }
            if (mayor!=indice)
            {
                int temp = a[indice];
                a[indice] = a[mayor];
                a[mayor] = temp;

                intercambio(ref Arreglo, mayor, indice);
                Max_Num(a, x, mayor, ref botones);
            }
        }
        

        






    }
}
