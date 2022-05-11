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
    public partial class MovieDetailsPage : Page
    {
        private SqlConnection dbConnection;
        public string selectedMovie;

        public void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            selectedMovie = (string)e.ExtraData;

            char[] seperator = { '>' };
            string[] seperatedMovieDetails = selectedMovie.Split(seperator);
            string movieTitle = seperatedMovieDetails[0];

            string connectionStr = "Server= RMDITX\\MSSQLSERVER01; Database=MovieCatalog; Integrated Security=SSPI;";
            dbConnection = new SqlConnection(connectionStr);
            try
            {
                dbConnection.Open();

                SqlCommand command = new SqlCommand($"SELECT * FROM movie where movie.title = '{movieTitle}'", dbConnection);
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    MovieTitleLabel.Text = dataReader.GetValue(0).ToString();
                    MovieRatingLabel.Text = "Rating: " + dataReader.GetValue(1).ToString();
                    DateTime movieDate = (DateTime)dataReader.GetValue(2);
                    MovieDateLabel.Text = "Date Added: " + movieDate.ToString("yyyy-MM-dd");
                    if (dataReader.GetValue(3).ToString() != "")
                    {
                        MovieDirectorLabel.Text = "Director: " + dataReader.GetValue(3).ToString();
                    }
                    else
                    {
                        MovieDirectorLabel.Text = "Click to add a director";
                    }
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

        public MovieDetailsPage()
        {
            InitializeComponent();
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
