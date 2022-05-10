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
    public partial class AddMoviePage : Page
    {

        private SqlConnection dbConnection; // TODO(Dillon): Maybe get this connection context from the main window, since we already grab it?

        public AddMoviePage()
        {
            InitializeComponent();
        }

        private void addMovieButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionStr = "Server= RMDITX\\MSSQLSERVER01; Database=MovieCatalog; Integrated Security=SSPI;";
            dbConnection = new SqlConnection(connectionStr);

            if(ratingInput.Text == "" || ratingInput.Text.ElementAt(0) < 49 || ratingInput.Text.ElementAt(0) > 53)
            {
                MessageBox.Show("Rating must be a whole number from 1 - 5");
            }
            else if (titleInput.Text == "")
            {
                MessageBox.Show("Movie must have a title!");
            }
            else
            {
                try
                {
                    dbConnection.Open();
                    int rating = int.Parse(ratingInput.Text);
                    
                    SqlCommand createMovieEntryCommand;

                    if(directorNameInput.Text != "")
                    {
                        SqlCommand retrieveDirectorIDCommand = new SqlCommand($"SELECT * FROM director WHERE director.name = '{directorNameInput.Text}'", dbConnection);
                        SqlDataReader retrieveDirectorIDDataReader = retrieveDirectorIDCommand.ExecuteReader();
                        int directorID = -1;
                        bool createDirectorEntry = true;
                        while (retrieveDirectorIDDataReader.Read())
                        {
                            createDirectorEntry = false;
                            directorID = int.Parse(retrieveDirectorIDDataReader.GetValue(1).ToString());
                        }
                        retrieveDirectorIDDataReader.Close();
                        retrieveDirectorIDCommand.Dispose();

                        if (createDirectorEntry)
                        {
                            // Create entry for director
                            SqlCommand createDirectorCommand = new SqlCommand($"INSERT INTO director(name) VALUES('{directorNameInput.Text}');", dbConnection);
                            SqlDataReader createDirectorExecution = createDirectorCommand.ExecuteReader();
                            createDirectorExecution.Close();
                            createDirectorCommand.Dispose();

                            // Get that entries ID
                            retrieveDirectorIDCommand = new SqlCommand($"SELECT * FROM director WHERE director.name = '{directorNameInput.Text}'", dbConnection);
                            retrieveDirectorIDDataReader = retrieveDirectorIDCommand.ExecuteReader();
                            while (retrieveDirectorIDDataReader.Read())
                            {
                                directorID = int.Parse(retrieveDirectorIDDataReader.GetValue(1).ToString());
                            }
                            retrieveDirectorIDDataReader.Close();
                            retrieveDirectorIDCommand.Dispose();
                        }

                        // Pass that ID to the movie creation entry
                        createMovieEntryCommand = new SqlCommand($"INSERT INTO movie(title, rating, dateAdded, directorID) VALUES ('{titleInput.Text}', {rating}, '{DateTime.Today.ToString()}', {directorID});", dbConnection);
                    }
                    else
                    {
                        // no director inputted
                        createMovieEntryCommand = new SqlCommand($"INSERT INTO movie(title, rating, dateAdded) VALUES ('{titleInput.Text}', {rating}, '{DateTime.Today.ToString()}');", dbConnection);
                    }


                    createMovieEntryCommand.ExecuteReader();
                    createMovieEntryCommand.Dispose();
                    dbConnection.Close();
                }
                catch (Exception except)
                {
                    MessageBox.Show(except.Message);
                }

                if(this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
            }
            
        }
    }
}
