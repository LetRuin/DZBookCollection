using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDZ_16._05._2021
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string connection = "Server=mysql60.hostland.ru;Database=host1323541_irkutsk6;Uid=host1323541_itstep;Pwd=269f43dc;";
        private MySqlConnection bd = new MySqlConnection(connection);
        private MySqlCommand querry;
        DataSet dataSet;
        int id;
        public MainWindow()
        {
            InitializeComponent();
            querry = new MySqlCommand
            {
                Connection = bd
            };
        }

        private void BookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var rows = BookDataGrid.SelectedIndex;
            if (dataSet.Tables[0].Rows.Count > rows && rows>=0) {
            id = Convert.ToInt32(dataSet.Tables[0].Rows[rows].ItemArray[0]);
            var sql = $"SELECT id , name , author , genre , publisher FROM `table_book` WHERE id = {id}";

            querry.CommandText = sql;
            var res = querry.ExecuteReader();
            if (res.Read())
            {
                id = res.GetInt32("id");
                BookName.Text = res.GetString("name");
                BookAuthor.Text = res.GetString("author");
                BookGenre.Text = res.GetString("genre");
                BookPublisher.Text = res.GetString("publisher");
            }
            res.Close();
        }
      
        }

        private void BottonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!bd.Ping())
            {
                bd.Open();
                if (bd.Ping())
                {
                    LabelInfo.Content = "Успешное подключение";
                    GridUpdate();
                }
                else { LabelInfo.Content = "Подключение не удалось"; }
            }
            else { LabelInfo.Content = "Вы уже подключенны к базе"; }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            id = BookDataGrid.SelectedIndex;
            var sql = $"INSERT INTO table_book (name, author , genre , publisher) VALUES ('{BookName.Text}', '{BookAuthor.Text}' , '{BookGenre.Text}' , '{BookPublisher.Text}');";
            querry.CommandText = sql;
            if (bd.Ping())
            {
                var res = querry.ExecuteNonQuery();
                if (res == 1)
                {
                    LabelInfo.Content = "Книга дабавленна";
                    GridUpdate();
                    TextBoxClear();
                }
            }
            else { LabelInfo.Content = "Вы не подключились к базе"; }
        }
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!(id == 0))
            {
                var sql = $"UPDATE table_book SET name = '{BookName.Text}', author = '{BookAuthor.Text}', genre = '{BookGenre.Text}', publisher = '{BookPublisher.Text}' WHERE id = '{id}';";

                querry.CommandText = sql;
                               
                if (bd.Ping())
                {
                    var res = querry.ExecuteNonQuery();
                    if (res == 1)
                    {
                        LabelInfo.Content = "Книга Изменина";
                        GridUpdate();
                        id = 0;
                        TextBoxClear();
                    }
                }
                else { LabelInfo.Content = "Вы не подключились к базе"; }
            }
            else { LabelInfo.Content = "Вы не выбрали что менять"; }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(id == 0))
            {
                var sql = $"DELETE FROM table_book WHERE id = '{id}';";

                querry.CommandText = sql;

                if (bd.Ping())
                {
                    var res = querry.ExecuteNonQuery();
                    if (res == 1)
                    {
                        LabelInfo.Content = "Книга удаленна";
                        GridUpdate();
                        id = 0;
                        TextBoxClear();
                    }
                }
                else { LabelInfo.Content = "Вы не подключились к базе"; }
            }
            else { LabelInfo.Content = "Вы не выбрали что удалить"; }

        }
        private void GridUpdate()
        {
            var sql = "SELECT id,name FROM `table_book` WHERE 1";
            var adapter = new MySqlDataAdapter(sql, bd);
            dataSet = new DataSet();
            adapter.Fill(dataSet);
            BookDataGrid.ItemsSource = dataSet.Tables[0].DefaultView;
        }
        private void TextBoxClear()
        {
            BookName.Text = null;
            BookAuthor.Text = null;
            BookGenre.Text = null;
            BookPublisher.Text = null;
        }
    }
}
