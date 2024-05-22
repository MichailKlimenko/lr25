using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лр25
{
    public partial class Filters : Form
    {
        // Поле для хранения представления данных, которые будут фильтроваться
        private DataView view;

        // Свойство для доступа к представлению данных
        public DataView DataView
        {
            set { view = value; }
            get { return view; }
        }

        // Конструктор формы Filters, принимает DataView как параметр
        public Filters(DataView dv)
        {
            InitializeComponent();
            DataView = dv;

            // Заполнение comboBox2 уникальными значениями поля Subject
            var subjects = DataView.ToTable(true, "Subject").AsEnumerable()
                                  .Select(row => row.Field<string>("Subject"))
                                  .ToList();
            comboBox2.DataSource = subjects; // Установка источника данных для comboBox2

            comboBox2.SelectedIndex = -1; // Сброс выделения в comboBox2
            this.Text = "Фильтрация по предмету"; // Установка заголовка формы
        }

        // Обработчик события нажатия кнопки для применения фильтрации
        private void button1_Click(object sender, EventArgs e)
        {
            string exp = ""; // Строка для хранения выражения фильтрации
            string arg = comboBox2.Text; // Получение выбранного значения из comboBox2
            switch (comboBox1.Text) // Проверка выбранного условия фильтрации
            {
                case "Равно":
                    exp = $"Subject='{arg}'"; // Фильтрация по равенству
                    break;
                case "Не равно":
                    exp = $"Subject<>'{arg}'"; // Фильтрация по неравенству
                    break;
                case "Содержит":
                    exp = $"Subject LIKE '%{arg}%'"; // Фильтрация по содержанию
                    break;
                case "Начинается с":
                    exp = $"Subject LIKE '{arg}%'"; // Фильтрация по началу строки
                    break;
            }
            view.RowFilter = exp; // Применение фильтрации к DataView

            this.DialogResult = DialogResult.OK; // Установка результата диалога
            this.Close(); // Закрытие формы
        }
    }
}
