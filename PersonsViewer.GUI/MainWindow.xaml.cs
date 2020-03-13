using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using PersonsViewer.DataLayer.SQL;
using PersonsViewer.Model;

namespace PersonsViewer.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TsqlDataManage dataManage;
        private StatusRepository statusRepository;
        private DepartamentRepository departamentRepository;
        private PostRepository postRepository;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            StreamReader connectionConfigureFile = new StreamReader("connectionString.txt");

            string connectionString = connectionConfigureFile.ReadLine();

            dataManage = new TsqlDataManage(connectionString);
            statusRepository = new StatusRepository(connectionString);
            departamentRepository = new DepartamentRepository(connectionString);
            postRepository = new PostRepository(connectionString);

            statusBox.Items.Add(new Status() { Id = -1 , Name = "*"});
            foreach(var status in statusRepository.GetAllEntitys())
            {
                statusBox.Items.Add(status);
            }
            statusBox.SelectedIndex = 0;

            statusBox2.ItemsSource = statusRepository.GetAllEntitys();
            statusBox2.SelectedIndex = 0;

            departmentBox.Items.Add(new Departament() { Id = -1, Name = "*" });
            foreach (var departament in departamentRepository.GetAllEntitys())
            {
                departmentBox.Items.Add(departament);
            }
            departmentBox.SelectedIndex = 0;

            postBox.Items.Add(new Post() { Id = -1, Name = "*" });
            foreach (var post in postRepository.GetAllEntitys())
            {
                postBox.Items.Add(post);
            }
            postBox.SelectedIndex = 0;
        }

        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Status statusFilter = (Status)statusBox.SelectedItem;
                if (statusFilter.Id == -1) statusFilter = null;

                Departament departmenFilter = (Departament)departmentBox.SelectedItem;
                if (departmenFilter.Id == -1) departmenFilter = null;

                Post postFilter = (Post)postBox.SelectedItem;
                if (postFilter.Id == -1) postFilter = null;

                Filter filterOptions = new Filter()
                {
                    Status = statusFilter,
                    Departament = departmenFilter,
                    Post = postFilter,
                    LastName = lastNameBox.Text
                };

                IEnumerable<Person> people = dataManage.GetPeople(filterOptions);

                peopleDataGrid.ItemsSource = RefreshPeopleData(dataManage.GetPeople(filterOptions));
            }
            catch (System.Data.SqlClient.SqlException exception)
            {
                MessageBox.Show("Ошибка: " + exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private IEnumerable<PersonGridRow> RefreshPeopleData(IEnumerable<Person> people)
        {
            List<PersonGridRow> data = new List<PersonGridRow>();

            foreach(var person in people)
            {
                data.Add(new PersonGridRow(person));
            }

            return data;
        }

        private void peopleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private IDictionary<DateTime, int> staticticData;

        private void statisticButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Status status = (Status)statusBox2.SelectedItem;
                bool isDateEmploy = isEmploedBox.SelectedIndex == 0;
                DateTime startDate = DateTime.Parse(startDateBox.Text);
                DateTime endDate = DateTime.Parse(endDateBox.Text);

                if (endDate < startDate) throw new ArgumentException();

                staticticData = dataManage.GetStatistic(status, isDateEmploy, startDate, endDate);

                statisticDataGrid.ItemsSource = staticticData;
                if (statisticDataGrid.Columns.Count == 2)
                {
                    statisticDataGrid.Columns[0].Header = "Дата";
                    statisticDataGrid.Columns[1].Header = "Кол-во";
                }
            }
            catch (System.FormatException exception)
            {
                MessageBox.Show("Ошибка: Некорректно введена дата." , "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.Data.SqlTypes.SqlTypeException exception)
            {
                MessageBox.Show("Ошибка: Дата должна быть в пределах от 01.01.1753 до 31.12.9999", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show("Ошибка:Некорректно введённый промежуток (начальное значение больше конечного)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (System.Data.SqlClient.SqlException exception)
            {
                MessageBox.Show("Ошибка: " + exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
