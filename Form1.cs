using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeServiceTest
{
    public partial class Form1 : Form
    {
        EmployeeServiceDBContent db;

        public Form1()
        {
            InitializeComponent();

            db = new EmployeeServiceDBContent();
            db.Employee.Load();
            dataGridView1.DataSource = db.Employee.Local.ToBindingList();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //добавление
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //проводим добавление в новой форме
            Form2 form2 = new Form2();

            //создаем список Отделов в combobox
            List<Department> departments = db.Department.ToList();
            form2.comboBoxDepartment.DataSource = departments;
            form2.comboBoxDepartment.ValueMember = "Id";
            form2.comboBoxDepartment.DisplayMember = "Name";

            DialogResult result = form2.ShowDialog(this);

            //при нажатии кнопки Cancel делаем отмену
            if (result == DialogResult.Cancel)
                return;

            //создаем новый экземпляр сотрудника
            Employee employee = new Employee();
            //заполняем поля сотрудника данными из textBox-ов
            employee.Name = form2.nameTextBox.Text;
            employee.Surname = form2.surnameTextBox.Text;
            employee.Middlename = form2.middlenameTextBox.Text;
            employee.Date_of_birth = form2.date_of_birthDateTimePicker.Value.Date;
            employee.Address = form2.addressTextBox.Text;
            employee.Department = (Department)form2.comboBoxDepartment.SelectedItem;
            employee.About = form2.aboutTextBox.Text;

            //добавляем сотрудника
            db.Employee.Add(employee);
            db.SaveChanges();
        }


        //редактирование
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            //проводим изменение при условии что сотрудник выбран
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //находим id сотрудника, пытаемся спарсить его в int 
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                //находим сотрудника по id
                Employee employee = db.Employee.Find(id);


                Form2 form2 = new Form2();
                //создаем список Отделов в combobox
                List<Department> departments = db.Department.ToList();
                form2.comboBoxDepartment.DataSource = departments;
                form2.comboBoxDepartment.ValueMember = "Id";
                form2.comboBoxDepartment.DisplayMember = "Name";

                //заполняем все textbox, combobox и datepicker данными сотрудника которого выбрали для изменения
                form2.nameTextBox.Text = employee.Name;
                form2.surnameTextBox.Text = employee.Surname;
                form2.middlenameTextBox.Text = employee.Middlename;
                form2.date_of_birthDateTimePicker.Value = employee.Date_of_birth;
                form2.addressTextBox.Text = employee.Address;
                form2.aboutTextBox.Text = employee.About;

                //присваиваем сотруднику id выбранного отдела
                if (employee.Department != null)
                    form2.comboBoxDepartment.SelectedValue = employee.Department.Id;

                DialogResult result = form2.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;

                //присваиваем полям сотрудника новые данные
                employee.Name = form2.nameTextBox.Text;
                employee.Surname = form2.surnameTextBox.Text;
                employee.Middlename = form2.middlenameTextBox.Text;
                employee.Date_of_birth = form2.date_of_birthDateTimePicker.Value.Date;
                employee.Address = form2.addressTextBox.Text;
                employee.Department = (Department)form2.comboBoxDepartment.SelectedItem;
                employee.About = form2.aboutTextBox.Text;

                //изменяем сотрудника в БД
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();

                //обновляем отображение datagridview, что бы увидеть изменения
                db.Employee.Load();
                dataGridView1.DataSource = db.Employee.Local.ToBindingList();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //находим id сотрудника, пытаемся спарсить его в int 
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;
                //находим сотрудника по id
                Employee employee = db.Employee.Find(id);

                //удаляем сотрудника
                db.Employee.Remove(employee);
                db.SaveChanges();

                //обновляем отображение datagridview, что бы увидеть изменения
                db.Employee.Load();
                dataGridView1.DataSource = db.Employee.Local.ToBindingList();

            }
        }
    }
}
