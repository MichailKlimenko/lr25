using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лр25
{
    public partial class FSort : Form
    {
        // Поле для хранения представления данных, которые будут сортироваться
        private DataView view;

        // Событие, которое вызывается при применении сортировки
        public event EventHandler SortingApplied;

        // Свойство для доступа к представлению данных
        public DataView DataView
        {
            set { view = value; }
            get { return view; }
        }

        public FSort()
        {
            InitializeComponent();
        }

        // Метод для заполнения listBox1 именами столбцов из представления данных
        private void setupItems()
        {
            for (int i = 0; i < view.Table.Columns.Count; i++)
            {
                listBox1.Items.Add(view.Table.Columns[i].ColumnName); // Добавление имени столбца в listBox1
            }
        }

        // Обработчик события нажатия кнопки для добавления выбранного элемента из listBox1 в listBox2
        private void button1_Click(object sender, EventArgs e)
        {
            var item = listBox1.SelectedItem; // Получаем выбранный элемент из listBox1
            if (!listBox2.Items.Contains(item)) // Проверяем, что элемент еще не добавлен в listBox2
            {
                listBox2.Items.Add(item); // Добавляем элемент в listBox2
                listBox2.SelectedItem = item; // Устанавливаем его как выбранный
            }
        }

        // Обработчик события нажатия кнопки для применения сортировки
        private void button3_Click(object sender, EventArgs e)
        {
            StringBuilder build = new StringBuilder();
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                build.Append(listBox2.Items[i]); // Добавляем имя столбца в строку сортировки
                if (i < listBox2.Items.Count - 1)
                {
                    build.Append(", "); // Добавляем запятую, если это не последний элемент
                }
            }

            // Определяем порядок сортировки
            string order = "ASC"; // По умолчанию сортировка по возрастанию
            if (radioButton2.Checked)
            {
                order = "DESC"; // Если выбран radioButton2, сортировка по убыванию
            }

            // Устанавливаем строку сортировки для DataView
            DataView.Sort = $"{build} {order}";

            // Вызываем событие после применения сортировки
            SortingApplied?.Invoke(this, EventArgs.Empty);

            // Закрываем форму с результатом OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Обработчик события загрузки формы, вызывающий метод для заполнения listBox1
        private void FSort_Load(object sender, EventArgs e)
        {
            setupItems(); // Заполнение listBox1 именами столбцов при загрузке формы
        }
    }
}
