using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace radProject
{
    public partial class Form1 : Form
    {
        public static List<user> usersList = PostgresConnect.dbGetListUser();
        public static List<string> usersID = new List<string>();
        public static string fileName = "D:/report.xlsx";
        public static DateTime dateFrom;
        public static DateTime dateTo;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = usersList;   // выводим список пользователей
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // достаём номер строки, на которую нажали
            // без повторений
            bool flag = true;
            for (int i = 0; i < usersID.Count; i++)
                if (dataGridView1[1, e.RowIndex].Value.ToString() == usersID[i])
                {
                    flag = false;
                    usersID.Remove(dataGridView1[1, e.RowIndex].Value.ToString());
                }

            if (flag)
                usersID.Add(dataGridView1[1, e.RowIndex].Value.ToString());

            if (dataGridView1[0, e.RowIndex].Value == "+")
                dataGridView1[0, e.RowIndex].Value = "";
            else
                dataGridView1[0, e.RowIndex].Value = "+";
        }

        private void label1_Click(object sender, DataGridViewCellEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) 
        {
            excelReport newReport = new excelReport(PostgresConnect.dbGetReport(usersID, dateFrom, dateTo), fileName); // получаю отчёт из бд и печатаю отчёт
            newReport.createReport();
            MessageBox.Show("Отчёт готов!");
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateFrom = dateTimePicker1.Value;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTo = dateTimePicker2.Value;
        }
    }
}
