using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows.Forms;


// not only connect

namespace radProject
{
    public class PostgresConnect
    {

        public static List<user> usersList = new List<user>();
        public static List<report> report = new List<report>();
        public static List<preReport> preReports = new List<preReport>();

        public static string Host = "localhost";
        public static string User = "postgres";
        public static string DBname = "employees";
        public static string Password = "123";
        public static string Port = "5432";

        public static string connString =
            String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
            Host,
            User,
            DBname,
            Port,
            Password);

        public static List<user> dbGetListUser()
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                //Console.Out.WriteLine("Opening connection");
                conn.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM employees", conn))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read()) { usersList.Add(new user(reader.GetInt32(0), reader.GetString(1))); }
                    reader.Close();
                }
            }
            return usersList;
        }
        public static List<report> dbGetReport(List<string> usersID)
        {

            using (var conn = new NpgsqlConnection(connString))
            {
                // debug
                string usersString = "'" + usersID[0] + "'";
                for (int i = 1; i < usersID.Count; i++)
                {
                    usersString += ", '" + usersID[i] + "'";
                }
                MessageBox.Show(usersString);
                foreach (var user in usersID)
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand("SELECT employees.name, expenses.name, expenseReport.amount, expenseReport.firstReported " +
                                                       "FROM employees, expenses, expenseReport " +
                                                       "WHERE employees.id = " + user +
                                                       "AND expenseReport.id_employee = " + user +
                                                       "AND expenses.id = id_expense ", conn))
                    {
                        // формируем список работников со всеми данными с повторением рабоников
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            preReports.Add(new preReport(reader.GetString(0), reader.GetString(1), reader.GetDouble(2), reader.GetBoolean(3)));
                        }
                        reader.Close();
                    }
                    conn.Close();
                }
                // формируем отчёт работников, в к-м нет повторений рабоников
                for (int i = 0; i < preReports.Count; i++)
                {
                    // если у какого-то работника есть отчёт до трат, то заходим в цикл и ищем все его отчёты с проверкой на отчёт до траты
                    double sumTax = 0;
                    if (preReports[i].firstReported == true)
                    {
                        sumTax += preReports[i].expenseAmount;
                        for (int j = 0; j < preReports.Count; j++)
                        {
                            // на первой проходке получаем все данные о 1м в списке работнике
                            if (preReports[i].employeeName == preReports[j].employeeName && preReports[j].firstReported == true && i != j)
                            {
                                sumTax += preReports[j].expenseAmount;
                            }
                        }
                    }

                    bool flag = true;
                    for (int j = 0; j < report.Count; j++)
                        if (preReports[i].employeeName == report[j].employeeName)
                            flag = false;
                    // если не нашли совпадений - записываем
                    if (flag)
                        report.Add(new report(preReports[i].employeeName, sumTax, sumTax * 0.13));
                }
            }
            return report;
        }
    }
}