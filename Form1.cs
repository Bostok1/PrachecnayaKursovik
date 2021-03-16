using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prachecnaya
{
    public partial class Form1 : Form
    {

        info information= new info(6);
        uint sum = 0;
        public Form1()
        {
            
            InitializeComponent();
       

            label5.Text = sum.ToString();
            label10.Text = information.countOfMachine.ToString();

        }
        private void update()
        {
            label10.Text = information.countOfMachine.ToString();
            int i = 0;
            int j = 0;
            int z = 0;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            foreach (order Order in information.orders)
            {
                if (!Order.isReady && !Order.isTake)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = Order.id;
                    string value=" ";
                    foreach (serivce Serv in Order.serivces) {
                        value += Serv.getName()+" ";
                    
                    }
                    dataGridView1.Rows[i].Cells[1].Value = value;
                    dataGridView1.Rows[i].Cells[2].Value = Order.getPrice();
                    dataGridView1.Rows[i].Cells[3].Value = Order.getDateIn();
                    i++;

                }
                else if (Order.isReady && !Order.isTake)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[j].Cells[0].Value = Order.id;
                    j++;
                }
                else if (Order.isReady && Order.isTake)
                {
                    dataGridView3.Rows.Add();
                    dataGridView3.Rows[z].Cells[0].Value = Order.id;
                    dataGridView3.Rows[z].Cells[1].Value = Order.getPrice();
                    dataGridView3.Rows[z].Cells[2].Value = Order.getDateOut();
                    z++;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = information.getCountOfOrders().ToString();
          
        }

        private void label1_Click(object sender, EventArgs e)
        {
         
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                sum += 300;
            }
            else
            {
                sum -= 300;
            }
            label5.Text = sum.ToString();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                sum += 450;
            }
            else
            {
                sum -= 450;
            }
            label5.Text = sum.ToString();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                sum += 550;
            }
            else
            {
                sum -= 550;
            }
            label5.Text = sum.ToString();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                sum += 200;
            }
            else
            {
                sum -= 200;
            }
            label5.Text = sum.ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (information.countOfMachine != 0)
            {
                information.countOfMachine--;
                List<serivce> serivces = new List<serivce>();
                if (checkBox1.Checked)
                {
                   
                    serivces.Add(information.serivces[0]);
                }
                if (checkBox2.Checked)
                {
                    
                    serivces.Add(information.serivces[1]);
                }
                if (checkBox3.Checked)
                {
                   
                    serivces.Add(information.serivces[2]);
                }
                if (checkBox4.Checked)
                {
                    serivces.Add(information.serivces[3]);
                 
                }
                if (checkBox5.Checked)
                {
                    serivces.Add(information.serivces[4]);
               
                }
                if (checkBox6.Checked)
                {
                    serivces.Add(information.serivces[5]);
              
                }
                

                information.addOrder(information.getCountOfOrders(), serivces);
                information.plusOrder();
                label3.Text = information.getCountOfOrders().ToString();
        
                update();
            }
            else
            {
                label10.ForeColor = Color.Red ;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                sum += 250;
            }
            else
            {
                sum -= 250;
            }
            label5.Text = sum.ToString();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            information.countOfMachine++;
            label10.ForeColor = Color.Black;
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[4] is DataGridViewButtonColumn )
            {
                
                information.orders[Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value)].isReady = true;
                update();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[1] is DataGridViewButtonColumn)
            {
           
                information.orders[Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value)].isTake = true;
                information.orders[Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value)].setDataOut();
                update();
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                sum += 300;
            }
            else
            {
                sum -= 300;
            }
            label5.Text = sum.ToString();
        }
    }
}
