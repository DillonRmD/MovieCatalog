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

        private string returnedMovieTitle;
        private int rating;
        private string dateAdded;
        private string directorName;
        private int movieID;

        public MovieDetailsPage()
        {
            InitializeComponent();
        }

        public MovieDetailsPage(string title):this()
        {
            InitializeComponent();

            selectedMovie = title;
            
            char[] seperator = { '>' };
            string[] seperatedMovieDetails = selectedMovie.Split(seperator);
            string movieTitle = seperatedMovieDetails[0];

            string connectionStr = "Server= RMDITX\\MSSQLSERVER01; Database=MovieCatalog; Integrated Security=SSPI; MultipleActiveResultSets=True;";
            dbConnection = new SqlConnection(connectionStr);
            try
            {
                dbConnection.Open();

                (returnedMovieTitle, rating, dateAdded, directorName, movieID) = DBUtils.GetMovie(movieTitle, dbConnection);

                MovieTitleLabel.Text = returnedMovieTitle;
                MovieRatingLabel.Text = "Rating: " + rating.ToString();
                MovieDateLabel.Text = "Date Added: " + dateAdded;
                MovieDirectorLabel.Text = (directorName == "") ? "Click to add a director" : "Director: " + directorName;

                dbConnection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show("Failed to contact database: " + except.Message);
            }
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Navigate("MovieListPage.xaml");
        }

        private void MovieDirectorLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddDirectorPopupWindow window = new AddDirectorPopupWindow(movieID);
            window.ShowDialog();
        }
    }
}
