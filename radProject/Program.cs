using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace radProject
{
    public class user
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public user(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        public user() { }
    }
    public class preReport
    {
        public string employeeName { get; set; }
        public string expenseName { get; set; }
        public double expenseAmount { get; set; }
        public bool firstReported { get; set; }
        public preReport(string employeeName, string expenseName, double expenseAmount, bool firstReported)
        {
            this.employeeName = employeeName;
            this.expenseName = expenseName;
            this.expenseAmount = expenseAmount;
            this.firstReported = firstReported;
        }
    }
    public class report
    {
        public string employeeName { get; set; }
        public double taxableAmount { get; set; }
        public double summTax { get; set; }
        public report(string employeeName, double taxableAmount, double summTax)
        {
            this.employeeName = employeeName;
            this.taxableAmount = taxableAmount;
            this.summTax = summTax;
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
