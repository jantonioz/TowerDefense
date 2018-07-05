using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditorWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            changeSize();
        }

        void changeSize()
        {
            int x = (textBox1.Text.ToString() != string.Empty ? Convert.ToInt32(textBox1.Text) : 0);
            int y = (textBox2.Text.ToString() != string.Empty ? Convert.ToInt32(textBox2.Text) : 0);

            drawTest1.map.setSize(x, y);
        }
        

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                changeSize();
                e.Handled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawTest1.selectedType(listBox1.SelectedIndex);
        }

        private void drawTest1_MouseHover(object sender, EventArgs e)
        {
            drawTest1.isMouseOver = true;
            lblMouse.ForeColor = Color.Green;
            lblMouse.Text = $"Mouse: {drawTest1.map.SelectedType}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drawTest1.map.clearWith(listBox1.SelectedIndex);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            drawTest1.map.clearWith(listBox1.SelectedIndex, false);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            drawTest1.map.SelectedType = 3;
        }

        private void drawTest1_MouseLeave(object sender, EventArgs e)
        {
            drawTest1.isMouseOver = false;
            lblMouse.ForeColor = Color.Red;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            drawTest1.map.SelectedType = 4;
        }

        private void drawTest1_Click(object sender, EventArgs e)
        {
            if (drawTest1.map.SelectedType == 3 )
                txtStartMap.Text = $"{drawTest1.map.CurrentCol}, {drawTest1.map.CurrentRow}";
            if (drawTest1.map.SelectedType == 4)
                txtEndMap.Text = $"{drawTest1.map.CurrentCol}, {drawTest1.map.CurrentRow}";
        }

        private void guardarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            drawTest1.map.WriteToFile();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {

                MessageBox.Show(openFileDialog1.FileName);
                drawTest1.map.ReadFile(openFileDialog1.FileName);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            drawTest1.gameState = 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            drawTest1.gameState = 0;
        }
    }
}