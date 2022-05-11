using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace MovieCatalog
{
    class DBUtils
    {
        public static void AddMovie(string title, int rating, int directorID, SqlConnection dbConnection)
        {
            try
            {
                SqlCommand createMovieEntryCommand = new SqlCommand($"INSERT INTO movie(title, rating, dateAdded, directorID) VALUES ('{title}', {rating}, '{DateTime.Today.ToString()}', {directorID});", dbConnection);
                createMovieEntryCommand.ExecuteReader();
                createMovieEntryCommand.Dispose();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        public static void AddMovie(string title, int rating, SqlConnection dbConnection)
        {
            try
            {
                SqlCommand createMovieEntryCommand = new SqlCommand($"INSERT INTO movie(title, rating, dateAdded) VALUES ('{title}', {rating}, '{DateTime.Today.ToString()}');", dbConnection);
                createMovieEntryCommand.ExecuteReader();
                createMovieEntryCommand.Dispose();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static (string, int, string, string, int) GetMovie(string title, SqlConnection dbConnection)
        {
            string movieTitle = title;
            int rating = -1;
            string dateAdded = "";
            string directorName = "";
            int directorID = -1;
            int movieID = -1;

            try
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM movie where movie.title = '{title}'", dbConnection);
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    rating = int.Parse(dataReader.GetValue(1).ToString()); // maybe error here
                    DateTime movieDate = (DateTime)dataReader.GetValue(2);
                    dateAdded = movieDate.ToString("yyyy-MM-dd");
                    directorID = (dataReader.GetValue(3).ToString() == "") ? -1 : int.Parse(dataReader.GetValue(3).ToString());
                    movieID = (dataReader.GetValue(4).ToString() == "") ? -1 : int.Parse(dataReader.GetValue(4).ToString());
                }

                dataReader.Close();
                command.Dispose();

                directorName = GetDirectorName(directorID, dbConnection);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return (movieTitle, rating, dateAdded, directorName, movieID);
        }

        public static void UpdateMovieWithDirector(int movieID, int directorID, SqlConnection dbConnection)
        {
            try
            {
                SqlCommand updateMovieCommand = new SqlCommand($"UPDATE movie SET movie.directorID={directorID} WHERE movie.movieID={movieID}", dbConnection);
                updateMovieCommand.ExecuteReader();
                updateMovieCommand.Dispose();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static string GetDirectorName(int id, SqlConnection dbConnection)
        {
            string directorName = "";
            try
            {
                SqlCommand retrieveDirectorName = new SqlCommand($"SELECT * FROM director WHERE director.directorID = {id}", dbConnection);
                SqlDataReader retrieveDirectorNameReader = retrieveDirectorName.ExecuteReader();

                while (retrieveDirectorNameReader.Read())
                {
                    directorName = retrieveDirectorNameReader.GetValue(0).ToString();
                }

                retrieveDirectorNameReader.Close();
                retrieveDirectorName.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return directorName;
        }

        public static int GetDirectorID(string name, SqlConnection dbConnection)
        {
            int directorID = -1;

            try
            {
                SqlCommand retrieveDirectorIDCommand = new SqlCommand($"SELECT * FROM director WHERE director.name = '{name}'", dbConnection);
                SqlDataReader retrieveDirectorIDDataReader = retrieveDirectorIDCommand.ExecuteReader();

                while (retrieveDirectorIDDataReader.Read())
                {
                    directorID = int.Parse(retrieveDirectorIDDataReader.GetValue(1).ToString());
                }

                retrieveDirectorIDDataReader.Close();
                retrieveDirectorIDCommand.Dispose();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return directorID;
        }

        public static int AddDirector(string name, SqlConnection dbConnection)
        {
            int directorID = -1;

            try
            {
                SqlCommand createDirectorCommand = new SqlCommand($"INSERT INTO director(name) VALUES('{name}');", dbConnection);
                SqlDataReader createDirectorExecution = createDirectorCommand.ExecuteReader();
                createDirectorExecution.Close();
                createDirectorCommand.Dispose();

                directorID = GetDirectorID(name, dbConnection);

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return directorID;
            
        }
    }
}
