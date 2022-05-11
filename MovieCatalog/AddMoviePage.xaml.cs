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

        // returns the director id based on the name, if the director is not found returns a -1
        private int getDirectorIDFromDB(string name)
        {
            SqlCommand retrieveDirectorIDCommand = new SqlCommand($"SELECT * FROM director WHERE director.name = '{name}'", dbConnection);
            SqlDataReader retrieveDirectorIDDataReader = retrieveDirectorIDCommand.ExecuteReader();

            int directorID = -1;
            while (retrieveDirectorIDDataReader.Read())
            {
                directorID = int.Parse(retrieveDirectorIDDataReader.GetValue(1).ToString());
            }

            retrieveDirectorIDDataReader.Close();
            retrieveDirectorIDCommand.Dispose();

            return directorID;
        }

        // Adds a director to the DB based on the name
        private void addDirectorToDB(string name)
        {
            SqlCommand createDirectorCommand = new SqlCommand($"INSERT INTO director(name) VALUES('{name}');", dbConnection);
            SqlDataReader createDirectorExecution = createDirectorCommand.ExecuteReader();
            createDirectorExecution.Close();
            createDirectorCommand.Dispose();
        }

        private void addMovieButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionStr = "Server= RMDITX\\MSSQLSERVER01; Database=MovieCatalog; Integrated Security=SSPI;";
            dbConnection = new SqlConnection(connectionStr);

            // ratingInput is either empty or is not a number between 1 and 5
            if(ratingInput.Text == "" || ratingInput.Text.ElementAt(0) < 49 || ratingInput.Text.ElementAt(0) > 53)
            {
                MessageBox.Show("Rating must be a whole number from 1 - 5");
            }
            // titleInput is empty
            else if (titleInput.Text == "")
            {
                MessageBox.Show("Movie must have a title!");
            }
            // all required forms are correct
            else
            {
                try
                {
                    dbConnection.Open();

                    // Get the rating value from the text field
                    int rating = int.Parse(ratingInput.Text);
                    
                    SqlCommand createMovieEntryCommand;

                    // if the input field is not empty then we want to add a director to this movie
                    if(directorNameInput.Text != "")
                    {
                        int directorID = getDirectorIDFromDB(directorNameInput.Text);

                        // If we don't find a directorID for that name, create one
                        if (directorID == -1)
                        {
                            // Make call to DB and create it
                            addDirectorToDB(directorNameInput.Text);

                            // Get that new entries' ID
                            directorID = getDirectorIDFromDB(directorNameInput.Text);
                        }

                        // Pass that ID to the movie creation entry
                        createMovieEntryCommand = new SqlCommand($"INSERT INTO movie(title, rating, dateAdded, directorID) VALUES ('{titleInput.Text}', {rating}, '{DateTime.Today.ToString()}', {directorID});", dbConnection);
                    }
                    // otherwise we don't want a director added to this movie
                    else
                    {
                        // no director inputted
                        createMovieEntryCommand = new SqlCommand($"INSERT INTO movie(title, rating, dateAdded) VALUES ('{titleInput.Text}', {rating}, '{DateTime.Today.ToString()}');", dbConnection);
                    }

                    // cleanup
                    createMovieEntryCommand.ExecuteReader();
                    createMovieEntryCommand.Dispose();
                    dbConnection.Close();
                }
                catch (Exception except)
                {
                    MessageBox.Show(except.Message);
                }

                // After adding the movie go back to the previous page
                if(this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                }
            }
            
        }
    }
}
