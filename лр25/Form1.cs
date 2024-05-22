using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лр25
{
    public partial class Form1 : Form
    {
        // Поля для работы с базой данных и интерфейсом
        private SqlConnection connection;  // Подключение к базе данных
        private SqlDataAdapter adapter;    // Адаптер для выполнения запросов и получения данных
        private DataTable dataTable;       // Таблица для хранения данных из базы

        // Поле для хранения ссылки на пользовательский элемент управления UCGrid
        private UCGrid ucGrid;

        public Form1()
        {
            InitializeComponent();
            // Строка подключения к локальной базе данных
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\работа\Практика КПиЯП\лр25\лр25\ConsultationDB.mdf;Integrated Security=True;";
            connection = new SqlConnection(connectionString); // Инициализация подключения
            LoadConsultations(); // Загрузка данных при инициализации формы
        }

        // Метод для загрузки данных из базы данных
        private void LoadConsultations()
        {
            try
            {
                string query = "SELECT * FROM Consultations"; // SQL-запрос для получения всех консультаций
                adapter = new SqlDataAdapter(query, connection); // Инициализация адаптера с запросом и подключением
                dataTable = new DataTable(); // Создание новой таблицы для данных
                adapter.Fill(dataTable); // Заполнение таблицы данными из базы
                ucGrid1.LoadData(dataTable); // Загрузка данных в пользовательский элемент управления
            }
            catch (Exception ex)
            {
                // Обработка ошибок загрузки данных
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        // Метод для обработки нажатия кнопки бронирования консультации
        private void btnBook_Click(object sender, EventArgs e)
        {
            var selectedRow = ucGrid1.SelectedRow; // Получаем выбранную строку в гриде
            // Проверяем, что строка выбрана и консультация не забронирована
            if (selectedRow != null && !(bool)selectedRow.Cells["isBookedDataGridViewCheckBoxColumn"].Value)
            {
                selectedRow.Cells["isBookedDataGridViewCheckBoxColumn"].Value = true; // Устанавливаем флаг бронирования
                SqlCommand command = new SqlCommand(
                    "UPDATE Consultations SET IsBooked = 1 WHERE Id = @Id",
                    connection); // Создание команды для обновления данных в базе
                command.Parameters.AddWithValue("@Id", (int)selectedRow.Cells["idDataGridViewTextBoxColumn"].Value); // Добавление параметра команды
                connection.Open(); // Открытие подключения к базе данных
                command.ExecuteNonQuery(); // Выполнение команды
                connection.Close(); // Закрытие подключения
                LoadConsultations(); // Обновление данных после изменения
            }
            else
            {
                // Сообщение, если консультация уже забронирована
                MessageBox.Show("Консультация уже занята.");
            }
        }

        // Метод для обработки нажатия кнопки отмены бронирования консультации
        private void btnCancel_Click(object sender, EventArgs e)
        {
            var selectedRow = ucGrid1.SelectedRow; // Получаем выбранную строку в гриде
            // Проверяем, что строка выбрана и консультация забронирована
            if (selectedRow != null && (bool)selectedRow.Cells["isBookedDataGridViewCheckBoxColumn"].Value)
            {
                selectedRow.Cells["isBookedDataGridViewCheckBoxColumn"].Value = false; // Снимаем флаг бронирования
                SqlCommand command = new SqlCommand(
                    "UPDATE Consultations SET IsBooked = 0 WHERE Id = @Id",
                    connection); // Создание команды для обновления данных в базе
                command.Parameters.AddWithValue("@Id", (int)selectedRow.Cells["idDataGridViewTextBoxColumn"].Value); // Добавление параметра команды
                connection.Open(); // Открытие подключения к базе данных
                command.ExecuteNonQuery(); // Выполнение команды
                connection.Close(); // Закрытие подключения
                LoadConsultations(); // Обновление данных после изменения
            }
            else
            {
                // Сообщение, если консультация уже свободна
                MessageBox.Show("Консультация уже свободна.");
            }
        }

        // Метод для обработки нажатия пункта меню фильтрации данных
        private void фильтрацияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters flt = new Filters(new DataView((DataTable)ucGrid1.DataSource)); // Создание окна фильтрации с данными
            // Открытие окна фильтрации и проверка результата
            if (flt.ShowDialog() == DialogResult.OK)
            {
                ucGrid1.DataSource = flt.DataView.ToTable(); // Применение фильтра и обновление данных в гриде
                MessageBox.Show("Фильтрация выполнена");
            }
        }

        // Метод для обработки нажатия пункта меню сортировки данных
        private void сортировкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSort f = new FSort(); // Создание окна сортировки
            f.DataView = new DataView((DataTable)ucGrid1.DataSource); // Передача данных для сортировки

            // Подписка на событие завершения сортировки
            f.SortingApplied += (s, args) => RefreshData();

            // Открытие окна сортировки и проверка результата
            if (f.ShowDialog() == DialogResult.OK)
            {
                ucGrid1.DataSource = f.DataView.ToTable(); // Применение сортировки и обновление данных в гриде
                MessageBox.Show("Сортировка выполнена");
            }
        }

        // Метод для обновления данных в гриде
        private void RefreshData()
        {
            LoadConsultations(); // Перезагрузка данных из базы
            ucGrid1.Refresh(); // Обновление отображения данных в гриде
        }

        // Метод для обработки нажатия кнопки перезагрузки данных
        private void button1_Click(object sender, EventArgs e)
        {
            LoadConsultations(); // Перезагрузка данных из базы
        }
    }
}
