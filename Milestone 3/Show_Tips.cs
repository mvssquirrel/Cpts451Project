using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Milestone_3
{
    public partial class Show_Tips : Form
    {
        private string buildConnString()
        {
            return "Host=localHost; Username=postgres; Password=muskrat; Database = Milestone3a";
        }
        public Show_Tips()
        {
            InitializeComponent();
           
        }

        public void populateTipsColumns()
        {
            Show_Tips_Pop.Columns.Add("User Name","User Name");
            Show_Tips_Pop.Columns.Add("Date of Tip","Date of Tip");
            Show_Tips_Pop.Columns.Add("Likes","Likes");
            Show_Tips_Pop.Columns.Add("Tip","Tip");
        }

        public void populateTipsGrid(object bid)
        {
            //object tip, date, likes, name;
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT tip,date_of_tip,likes,name FROM tips JOIN user_table ON tips.uid=user_table.uid WHERE tips.bid='"+bid.ToString()+"';";
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var tip = reader.GetString(0);
                            var date = reader.GetDate(1);
                            var likes = reader.GetString(2);
                            var name = reader.GetString(3);

                            Show_Tips_Pop.Rows.Add(new object[] { name, date, likes, tip });
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}
