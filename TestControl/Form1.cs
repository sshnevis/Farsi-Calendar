using System.Collections.Generic;
using AmirCalendar;
using System;
using System.Windows.Forms;

namespace TestControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void DatePicker_DateChanged(object sender, FarsiDatePickerEventArgs e)
        {
            var datePicker = (FarsiCalendar)sender;
            var mes = string.Format("Old FarsiDate: {0}New FarsiDate: {1}DatePicker Format: {2}Persian SelectedDate: {3}Gregorian SelectedDate: {4}" +
                                    "Number Of Days In Persian SelectedMonth: {5}Persian Year: {6}Persian Month: {7}Persian Day: {8}Persian selectedDate In Long Format: {9}Is Holiday: {10}", 
                                    e.OldFarsiDate + Environment.NewLine, 
                                    e.NewFarsiDate + Environment.NewLine,
                                    datePicker.Value.Format + Environment.NewLine,
                                    datePicker.Value.FarsiSelectedDate + Environment.NewLine,
                                    datePicker.Value.GregorianSelectedDate.ToShortDateString() + Environment.NewLine,
                                    datePicker.Value.NumberOfDaysInFarsiSelectedMonth + Environment.NewLine,
                                    datePicker.Value.FarsiYear + Environment.NewLine,
                                    datePicker.Value.FarsiMonth + Environment.NewLine,
                                    datePicker.Value.FarsiDay + Environment.NewLine,
                                    FarsiDateHelper.GetLongFarsiDate(datePicker.Value.GregorianSelectedDate) + Environment.NewLine,
                                    FarsiDateHelper.IsHolidayFarsiDate(datePicker.Value.FarsiSelectedDate) ? "تعطیل" : "غیر تعطیل");
            MessageBox.Show(mes);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var persons = new List<Person>
                              {
                                  new Person { BirthDate = "1364/01/01", Fullname = "AAAAA"},
                                  new Person { BirthDate = "1352/07/24", Fullname = "BBBBB"},
                                  new Person { BirthDate = "1354/11/23", Fullname = "CCCCC"},
                                  new Person { BirthDate = "1380/05/11", Fullname = "DDDDD"}
                              };
            gridPerson.AutoGenerateColumns = false;
            gridPerson.DataSource = persons;

            BirthDate.CellDateChanged += BirthDate_CellDateChanged;
        }

        void BirthDate_CellDateChanged(DataGridViewFarsiDatePickerCell cell, FarsiDatePickerEventArgs e)
        {
            MessageBox.Show(string.Format(" Cell[{0},{1}] \r\n Old FarsiDate: {2} \r\n New FarsiDate: {3}", cell.RowIndex,
                                          cell.ColumnIndex, e.OldFarsiDate, e.NewFarsiDate));
        }
    }

    public class Person
    {
        public string BirthDate { get; set; }
        public string Fullname { get; set; }
    }
}
