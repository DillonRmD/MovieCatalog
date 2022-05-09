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
                    SqlCommand command = new SqlCommand($"INSERT INTO movie(title, rating) VALUES ('{titleInput.Text}', {rating});", dbConnection);
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        MessageBox.Show(dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2));
                    }
                    dataReader.Close();
                    command.Dispose();
                    dbConnection.Close();
                }
                catch (Exception except)
                {
                    MessageBox.Show("Failed to open connection: " + except.Message);
                }
            }
            
        }
    }
}
