using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace Prachecnaya
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        smo SMO;
        smo2 SMO2;

        //Коэфициенты sleep_time - время остаовки потока, n-кол-во потоков, m-кол-во мест в очереди каждого потока l-интенсивность потока заявок , time - время
        int sleep_time = 0, time = 0;
        int n, m;
        double l;


        // 3 массива с экспонентоциальным распределением
        double[] rnd_l = new double[1000000];
        double[] rnd_obr = new double[1000000];
        double[] rnd_T = new double[1000000];

        //ver_otkaza - вероятность отказа, lambda-кол-во всего запускаемых заявок L-кол-во заявок в ед. времени k- целое кол-во заявок в ед. времени
        double ver_otkaza_1 = 0, ver_otkaza_2 = 0;
        double L = 0, k = 0;
        int lambda = 0, lambda_quality = 0;

        double ordersInSMO1 = 0, ordersInSMO2 = 0;
        double ordersInQuery1 = 0, ordersInQuery2 = 0;
        double busyCanals1 = 0, busyCanals2 = 0;

        //счетчики рандома
        int r_l = 0, r_obr = 0, r_T = 0;

        public static int success1 = 0, fail1 = 0;
        public static int success2 = 0, fail2 = 0;

        public static double average_time_in_SMO1 = 0;
        public static double average_time_in_query1 = 0;
        public static double counter_time_in_smo1 = 0;
        public static double counter_time_in_query1 = 0;

        public static double average_time_in_SMO2 = 0;
        public static double average_time_in_query2 = 0;
        public static double counter_time_in_smo2 = 0;
        public static double counter_time_in_query2 = 0;
        public Form1()
        {

            InitializeComponent();


            label5.Text = sum.ToString();
            label10.Text = information.countOfMachine.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.sleep_time = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            success1 = 0; fail1 = 0;
            success2 = 0; fail2 = 0;
            lambda = 0;
            ordersInSMO1 = 0;
            ordersInSMO2 = 0;
            ordersInQuery1 = 0;
            ordersInQuery2 = 0;
            busyCanals1 = 0;
            busyCanals2 = 0;

            n = Convert.ToInt32(textBox_n.Text);
            m = Convert.ToInt32(textBox_m.Text);
            l = Convert.ToDouble(textBox_l.Text);
            lambda_quality = Convert.ToInt32(textBox_lambda.Text);

            SMO = new smo(n, m);
            SMO2 = new smo2(n, m);


            if (n >= 0 & m >= 0 & l >= 0)
            {
                for (int i = 0; i < 10000; i++)
                {
                    rnd_l[i] = ExponentialDistribution(l);
                }

                for (int i = 0; i < 10000; i++)
                {
                    rnd_obr[i] = ExponentialDistribution(l);
                }

                for (int i = 0; i < 10000; i++)
                {
                    rnd_T[i] = ExponentialDistribution(l);
                }
            }

            start();
        }


        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        private double nextT()
        {
            if (r_T > 9999) { r_T = 0; }
            r_T++;
            return rnd_T[r_T - 1];
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private double nextl()
        {
            if (r_l > 9999) { r_l = 0; }
            r_l++;
            return rnd_l[r_l - 1];
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private double nextobr()
        {
            if (r_obr > 9999) { r_obr = 0; }
            r_obr++;
            return rnd_obr[r_obr - 1];
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private void start()
        {
            while (lambda < lambda_quality)
            {
                time++;
                Generate();
                calculateFailChanse();

                ordersInSMO1 += (double)SMO.OrdersInSMO();
                ordersInSMO2 += (double)SMO2.OrdersInSMO();

                ordersInQuery1 += (double)SMO.OrdersInQuery();
                ordersInQuery2 += (double)SMO2.OrdersInQuery();

                textBox_MidleQualityOrdersInSMO1.Text = (ordersInSMO1 / (double)time).ToString();
               

                textBox_MidleQualityOrdersInQuery1.Text = (ordersInQuery1 / (double)time).ToString();
              

                textBox_averageTimeInSMO1.Text = (average_time_in_SMO1 / counter_time_in_smo1).ToString();
               

             
                

                busyCanals1 += (double)SMO.AverageBusyCanals();
                busyCanals2 += (double)SMO2.AverageBusyCanals();

                textBox_averageTimeOfBusyCanals1.Text = (busyCanals1 / (double)time).ToString();
                textBox_averageTimeOfBusyCanals2.Text = (busyCanals2 / (double)time).ToString();

                this.Refresh();
            }
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private void Generate()
        {
            if (lambda == 0)
            {
                L = nextl();
                k = k + L;
                int i = (int)Math.Truncate(k);
                k -= i;
                lambda += i;
                orderPush(i);
            }

            else if (lambda > 0)
            {
                SMO.dec(1);
                SMO2.dec(1);
                L = nextl();
                k = k + L;
                int i = (int)Math.Truncate(k);
                k -= i;
                lambda += i;
                orderPush(i);
            }
            Thread.Sleep(sleep_time);
        }

        //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        private void orderPush(int i)
        {
            while (i >= 1)
            {
                if (SMO.incomingOrder(new Order(nextT(), nextobr()))) { success1++; }
                else { fail1++; }
                if (SMO2.incomingOrder(new Order(nextT(), nextobr()))) { success2++; }
                else { fail2++; }
                i--;
            }
        }




        private void calculateFailChanse()
        {
            if (lambda != 0)
            {
                ver_otkaza_1 = (double)fail1 / (double)lambda;
                ver_otkaza_2 = (double)fail2 / (double)lambda;
            }

        
        }

        private double ExponentialDistribution(double l)
        {
            double λ = l;    // Параметр экспоненциального распределения
            double z = 0;
            z = -Math.Log(rand.NextDouble()) / λ;
            return z;
        }



        info information = new info(6);
        uint sum = 0;
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
                    string value = " ";
                    foreach (serivce Serv in Order.serivces)
                    {
                        value += Serv.getName() + " ";

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

        private void button2_Click_1(object sender, EventArgs e)
        {
            success1 = 0; fail1 = 0;
            success2 = 0; fail2 = 0;
            lambda = 0;
            ordersInSMO1 = 0;
            ordersInSMO2 = 0;
            ordersInQuery1 = 0;
            ordersInQuery2 = 0;
            busyCanals1 = 0;
            busyCanals2 = 0;

            n = Convert.ToInt32(textBox_n.Text);
            m = Convert.ToInt32(textBox_m.Text);
            l = Convert.ToDouble(textBox_l.Text);
            lambda_quality = Convert.ToInt32(textBox_lambda.Text);

            SMO = new smo(n, m);
            SMO2 = new smo2(n, m);


            if (n >= 0 & m >= 0 & l >= 0)
            {
                for (int i = 0; i < 10000; i++)
                {
                    rnd_l[i] = ExponentialDistribution(l);
                }

                for (int i = 0; i < 10000; i++)
                {
                    rnd_obr[i] = ExponentialDistribution(l);
                }

                for (int i = 0; i < 10000; i++)
                {
                    rnd_T[i] = ExponentialDistribution(l);
                }
            }

            start();
        }

        private void button4_Click(object sender, EventArgs e)
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
                label10.ForeColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

           
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

            if (senderGrid.Columns[4] is DataGridViewButtonColumn)
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

