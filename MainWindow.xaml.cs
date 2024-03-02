using AdoNetLess2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace AdoNetLess2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string connetionString = ConfigurationManager.ConnectionStrings["hw_02"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connetionString))
            {
                try
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(
                        @"create table [student] (
                            id int primary key identity,
                            [name] varchar(30) not null,
                            [last_name] varchar(30) not null,
                            [age] int not null,
                        )"
                        , sqlConnection);
                    sqlCommand.ExecuteNonQuery();

                    List<Tuple<string, string, int>> data = new List<Tuple<string, string, int>>()
                    {
                        new Tuple<string, string, int>("John","Smith", 18),
                        new Tuple<string, string, int>("Emily","Johnson", 19),
                        new Tuple<string, string, int>("William","Jones", 20),
                        new Tuple<string, string, int>("Olivia","Brown", 22),
                        new Tuple<string, string, int>("Samantha","Adams", 21),
                    };
                    
                    SqlCommand insertCommand = new SqlCommand("", sqlConnection);

                    foreach (var p in data)
                    {
                        insertCommand.CommandText =
                            $"insert student values ('{p.Item1}', '{p.Item2}', '{p.Item3}')";
                        insertCommand.ExecuteNonQuery();
                    }

                    SqlCommand sqlCommand1 = new SqlCommand(
                        "Select * from student", 
                        sqlConnection);

                    using (SqlDataReader reader = sqlCommand1.ExecuteReader())
                    {
                        List<Student> students = new List<Student>();
                        while (reader.Read())
                        {
                           students.Add(new Student { 
                                Id = reader.GetFieldValue<int>(0),
                                Name = reader.GetFieldValue<string>(1),
                                LastName = reader.GetFieldValue<string>(2),
                                Age = reader.GetFieldValue<int>(3)
                                });

                            grid.ItemsSource = students;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            
        }


    }
}
