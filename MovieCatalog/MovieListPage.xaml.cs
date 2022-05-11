using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace MovieCatalog
{
    public partial class MovieListPage : Page
    {
        private SqlConnection dbConnection;
        public MovieListPage()
        {
            InitializeComponent();

            string connectionStr = "Server= RMDITX\\MSSQLSERVER01; Database=MovieCatalog; Integrated Security=SSPI;";
            dbConnection = new SqlConnection(connectionStr);

            try
            {
                dbConnection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM movie", dbConnection);
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = dataReader.GetValue(0) + " > rating: " + dataReader.GetValue(1);
                    watchedMovies.Items.Add(item);
                }

                dataReader.Close();
                command.Dispose();
                dbConnection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show("Failed to contact database: " + except.Message);
            }
        }

        private void addMovieButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddMoviePage());
        }

        private void watchedMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = (sender as ListBox).SelectedItem as ListBoxItem;
            MovieDetailsPage page = new MovieDetailsPage();
            NavigationService.LoadCompleted += page.NavigationService_LoadCompleted;

            this.NavigationService.Navigate(new MovieDetailsPage(), item.Content.ToString());
        }
    }
}
