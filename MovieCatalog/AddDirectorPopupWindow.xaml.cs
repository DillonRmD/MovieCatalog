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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace MovieCatalog
{
    public partial class AddDirectorPopupWindow : Window
    {
        private int movieID;
        private SqlConnection dbConnection;

        public AddDirectorPopupWindow()
        {
            InitializeComponent();
        }

        public AddDirectorPopupWindow(int id):this()
        {
            InitializeComponent();
            movieID = id;
        }

        private void addDirector_Click(object sender, RoutedEventArgs e)
        {
            string directorName = nameInput.Text;

            string connectionStr = "Server= RMDITX\\MSSQLSERVER01; Database=MovieCatalog; Integrated Security=SSPI; MultipleActiveResultSets=True;";
            dbConnection = new SqlConnection(connectionStr);
            try
            {
                // If director already exists in database
                //      don't create a new director entry, but UPDATE the movie with teh correct directorID
                // If the director does NOT exist in database
                //      create a new director entry and update the movuie with the correct directorID

                dbConnection.Open();
                int directorID = DBUtils.GetDirectorID(directorName, dbConnection);
                
                if(directorID == -1)
                {
                    // director doesn't exist in DB, create a new entry
                    directorID = DBUtils.AddDirector(directorName, dbConnection);
                }

                DBUtils.UpdateMovieWithDirector(movieID, directorID, dbConnection);

                dbConnection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show("Failed to contact database: " + except.Message);
            }

            this.Close();
        }
    }
}
