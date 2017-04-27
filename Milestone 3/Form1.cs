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
    public partial class Main_Screen : Form
    {
        private string buildConnString()
        {
            return "Host=localHost; Username=postgres; Password=muskrat; Database = Milestone3a";
        }
        public Main_Screen()
        {
            InitializeComponent();
            addStates();
            fillDays();
            fillFrom();
            fillTo();
            addColumns();
            addColToFriends();
            addColtoTips();
        }

        private void Business_Page_Click(object sender, EventArgs e)
        {

        }

        private void Main_Screen_Load(object sender, EventArgs e)
        {

        }

        private void State_Drop_Down_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = State_Drop_Down.SelectedItem;
            Zip_Box.Items.Clear();
            addCities(selection);
            fillSearchResultsState(selection);
        }
        public void addStates()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            State_Drop_Down.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addCities(object state)
        {
            City_List.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT city FROM business where state='" + state + "'ORDER BY city";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            City_List.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addZips(object city, object state)
        {
            Zip_Box.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT zip FROM business where city='" + city + "' AND state='" + state + "' ORDER BY zip";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Zip_Box.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }

        }
        public void fillDays()
        {
            Day_Box.Items.Add("Sunday");
            Day_Box.Items.Add("Moday");
            Day_Box.Items.Add("Tuesday");
            Day_Box.Items.Add("Wednesday");
            Day_Box.Items.Add("Thursday");
            Day_Box.Items.Add("Friday");
            Day_Box.Items.Add("Saturday");
        }
        public void fillFrom()
        {
            From_Box.Items.Add("00:00");
            From_Box.Items.Add("01:00");
            From_Box.Items.Add("02:00");
            From_Box.Items.Add("03:00");
            From_Box.Items.Add("04:00");
            From_Box.Items.Add("05:00");
            From_Box.Items.Add("06:00");
            From_Box.Items.Add("07:00");
            From_Box.Items.Add("08:00");
            From_Box.Items.Add("09:00");
            From_Box.Items.Add("10:00");
            From_Box.Items.Add("11:00");
            From_Box.Items.Add("12:00");
            From_Box.Items.Add("13:00");
            From_Box.Items.Add("14:00");
            From_Box.Items.Add("15:00");
            From_Box.Items.Add("16:00");
            From_Box.Items.Add("17:00");
            From_Box.Items.Add("18:00");
            From_Box.Items.Add("19:00");
            From_Box.Items.Add("20:00");
            From_Box.Items.Add("21:00");
            From_Box.Items.Add("22:00");
            From_Box.Items.Add("23:00");
        }
        public void fillTo()
        {
            To_Box.Items.Add("00:00");
            To_Box.Items.Add("01:00");
            To_Box.Items.Add("02:00");
            To_Box.Items.Add("03:00");
            To_Box.Items.Add("04:00");
            To_Box.Items.Add("05:00");
            To_Box.Items.Add("06:00");
            To_Box.Items.Add("07:00");
            To_Box.Items.Add("08:00");
            To_Box.Items.Add("09:00");
            To_Box.Items.Add("10:00");
            To_Box.Items.Add("11:00");
            To_Box.Items.Add("12:00");
            To_Box.Items.Add("13:00");
            To_Box.Items.Add("14:00");
            To_Box.Items.Add("15:00");
            To_Box.Items.Add("16:00");
            To_Box.Items.Add("17:00");
            To_Box.Items.Add("18:00");
            To_Box.Items.Add("19:00");
            To_Box.Items.Add("20:00");
            To_Box.Items.Add("21:00");
            To_Box.Items.Add("22:00");
            To_Box.Items.Add("23:00");
        }
        public void addColumns()
        {
            Search_Results.Columns.Add("BusinessName", "BusinessName");
            Search_Results.Columns.Add("FullAddress", "FullAddress");
            Search_Results.Columns.Add("#ofTips", "#ofTips");
            Search_Results.Columns.Add("TotalCheckins", "TotalCheckins");

        }
        public void addColToFriends()
        {
            Friends_Grid.Columns.Add("User Id", "User Id");
            Friends_Grid.Columns.Add("Name", "Name");
            Friends_Grid.Columns.Add("Avg Stars", "Avg Stars");
            Friends_Grid.Columns.Add("Yelping Since", "Yelping Since");
        }
        public void addColtoTips()
        {
            Tips_Grid.Columns.Add("User Name", "User Name");
            Tips_Grid.Columns.Add("Business", "Business");
            Tips_Grid.Columns.Add("City", "City");
            Tips_Grid.Columns.Add("Tip", "Tip");
        }
        public void fillSearchResults(object state, object city, object zip)
        {
            Search_Results.Rows.Clear();
            Search_Results.Refresh();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT name,full_addr,num_checkins,review_count FROM business where state='" + state + "' AND city='" + city + "' AND zip='" + zip + "' ORDER BY business";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var add = reader.GetString(1);
                            var bname = reader.GetString(0);
                            object numTips, numCheck;
                            if (reader.IsDBNull(2))
                                numCheck = 0;
                            else
                                numCheck = reader.GetValue(2);
                            if (reader.IsDBNull(3))
                                numTips = 0;
                            else
                                numTips = reader.GetValue(3);
                            Search_Results.Rows.Add(new object[] { bname, add, numTips, numCheck });
                        }
                    }
                }
                conn.Close();
            }
        }

        //for now this is a pseudo one to practice
        public void fillSearchResultsState(object state)
        {
            Search_Results.Rows.Clear();
            Search_Results.Refresh();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT name,full_addr,num_checkins,review_count FROM business where state='" + state + "' ORDER BY business";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var add = reader.GetString(1);
                            var bname = reader.GetString(0);
                            object numTips, numCheck;
                            if (reader.IsDBNull(2))
                                numCheck = 0;
                            else
                                numCheck = reader.GetValue(2);
                            if (reader.IsDBNull(3))
                                numTips = 0;
                            else
                                numTips = reader.GetValue(3);
                            Search_Results.Rows.Add(new object[] { bname, add, numTips, numCheck });
                        }
                    }
                }
                conn.Close();
            }
        }
        public void SearchResults2(object state, object city)
        {
            // Console.WriteLine(state);
            // Console.WriteLine("\n");
            // Console.WriteLine(city);
            Search_Results.Rows.Clear();
            Search_Results.Refresh();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT name,full_addr,num_checkins,review_count FROM business where state='" + state + "' AND city='"+city+"'ORDER BY business";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var add = reader.GetString(1);
                            var bname = reader.GetString(0);
                            object numTips, numCheck;
                            if (reader.IsDBNull(2))
                                numCheck = 0;
                            else
                                numCheck = reader.GetValue(2);
                            if (reader.IsDBNull(3))
                                numTips = 0;
                            else
                                numTips = reader.GetValue(3);
                            Search_Results.Rows.Add(new object[] { bname,add,numTips,numCheck});
                        }
                    }
                }
                conn.Close();
            }
        }

        public void fillSearchResultsCategory(object state, object city, object zip)
        {
                Search_Results.Rows.Clear();
            Search_Results.Refresh();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    /*cmd.CommandText = "Select name,full_addr,num_checkins,review_count From business Join Categories On Business.bid=Categories.bid where state='" + state + "' AND city='" + city + "' AND zip='" + zip + "'";
                    if (Categories_Checklist.CheckedItems.Count > 0)
                    {
                        cmd.CommandText = cmd.CommandText + "and ( ";
                        for (int i = 0; i < Categories_Checklist.CheckedItems.Count; i++)
                        {
                            cmd.CommandText = cmd.CommandText + " category='" + Categories_Checklist.CheckedItems[i].ToString() + "'";
                            if (i + 1 < Categories_Checklist.CheckedItems.Count)
                                cmd.CommandText = cmd.CommandText + " and ";
                        }
                        cmd.CommandText = cmd.CommandText + ") ";
                    }
                    cmd.CommandText = cmd.CommandText + " ORDER BY business";*/
                    cmd.CommandText = "Select b1.name,b1.full_addr,b1.num_checkins,b1.review_count From (select bid from business where business.state='" +
                        state +
                        "' and business.city='" +
                        city +
                        "' and business.zip='" +
                        zip +
                        "') bidPick, business b1";
                    for (int i = 0; i < Categories_Checklist.CheckedItems.Count; i++)
                    {
                        cmd.CommandText = cmd.CommandText + ",categories c" + i.ToString();
                    }
                    cmd.CommandText = cmd.CommandText + " where";
                    for (int i = 0 ; i < Categories_Checklist.CheckedItems.Count; i++ )
                    {
                        cmd.CommandText = cmd.CommandText + " bidPick.bid=c" + i.ToString() + ".bid and c" + i.ToString() + ".category='" + Categories_Checklist.CheckedItems[i].ToString() + "' and";
                    }
                    cmd.CommandText = cmd.CommandText + " b1.bid=bidPick.bid";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var add = reader.GetString(1);
                                var bname = reader.GetString(0);
                                object numTips, numCheck;
                                if (reader.IsDBNull(2))
                                    numCheck = 0;
                                else
                                    numCheck = reader.GetValue(2);
                                if (reader.IsDBNull(3))
                                    numTips = 0;
                                else
                                    numTips = reader.GetValue(3);
                                Search_Results.Rows.Add(new object[] { bname, add, numTips, numCheck });
                            }
                        }
                }
                conn.Close();
            }
        }
        public void refineSearchHour()
        {
            // Console.WriteLine(state);
            // Console.WriteLine("\n");
            // Console.WriteLine(city);
            Search_Results.Rows.Clear();
            Search_Results.Refresh();
            string day;
            if (Day_Box.SelectedItem.ToString() == "Sunday")
                day = "sun";
            else if (Day_Box.SelectedItem.ToString() == "Monday")
                day = "mon";
            else if (Day_Box.SelectedItem.ToString() == "Tuesday")
                day = "tue";
            else if (Day_Box.SelectedItem.ToString() == "Wednesday")
                day = "wed";
            else if (Day_Box.SelectedItem.ToString() == "Thursday")
                day = "thur";
            else if (Day_Box.SelectedItem.ToString() == "Friday")
                day = "fri";
            else
                day = "sat";
            string opHour = From_Box.SelectedItem.ToString();
            string clHour = To_Box.SelectedItem.ToString();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "Select name,full_addr,review_count,num_checkins from business JOIN hours on business.bid=hours.bid where "+day+"open='\""+opHour+"\"' and "+day+"close ='\""+clHour+"\"'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var add = reader.GetString(1);
                            var bname = reader.GetString(0);
                            object numTips, numCheck;
                            if (reader.IsDBNull(2))
                                numCheck = 0;
                            else
                                numCheck = reader.GetValue(2);
                            if (reader.IsDBNull(3))
                                numTips = 0;
                            else
                                numTips = reader.GetValue(3);
                            Search_Results.Rows.Add(new object[] { bname, add, numTips, numCheck });
                        }
                    }
                }
                conn.Close();
            }
        }
        public void popShowBus()
        {

            string temp = Search_Results.SelectedCells[0].Value.ToString();


            Selected_Business.Text = temp;
        }

        public void addCat(object state, object city, object zip)
        {
            
            Categories_Checklist.Items.Clear();

            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT category FROM categories WHERE state='" + state + "' AND city='" + city + "'ORDER BY business";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var add = reader.GetString(1);
                            var bname = reader.GetString(0);
                            object numTips, numCheck;
                            if (reader.IsDBNull(2))
                                numCheck = 0;
                            else
                                numCheck = reader.GetValue(2);
                            if (reader.IsDBNull(3))
                                numTips = 0;
                            else
                                numTips = reader.GetValue(3);
                            Search_Results.Rows.Add(new object[] { bname, add, numTips, numCheck });
                        }
                    }
                }
                conn.Close();
            }

        }

        public void setCatTable(object state, object city, object zip)
        {
            if (Categories_Checklist.CheckedItems.Count > 0)
                return;
            Categories_Checklist.Items.Clear();
            Categories_Checklist.Refresh();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT category FROM categories Order by Category";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categories_Checklist.Items.Add(reader.GetValue(0));
                        }
                    }
                    
                }
                conn.Close();
            }
        }
        public void setUID()
        {
            var uid = User_ID_Text.Text;
            
            object name, stars, fans, yelp_since, funny, cool, usefull;
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM user_table WHERE uid='" + uid + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            
                            name = reader.GetValue(1);
                            cool = reader.GetValue(3);
                            funny = reader.GetValue(4);
                            usefull = reader.GetValue(5);
                            stars = reader.GetValue(6);
                            yelp_since = reader.GetValue(8);
                            fans = reader.GetValue(9);


                            Name_Box.Text = name.ToString();
                            Cool_Box.Text = cool.ToString();
                            Fun_Box.Text = funny.ToString();
                            Useful_Box.Text = usefull.ToString();
                            Stars_Box.Text = stars.ToString();
                            Yelp_Box.Text = yelp_since.ToString();
                            Fans_Box.Text = fans.ToString();
                            
                        }
                    }
                }
                conn.Close();
            }
            setFriendsList();
           
        }

        public void setFriendsList()
        {
            Friends_Grid.Rows.Clear();
            Friends_Grid.Refresh();
            var uid = User_ID_Text.Text;

            object name, stars, since, fuid;
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM friends JOIN user_table ON user_table.uid=friends.friend_uid WHERE friends.uid='" + uid + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            fuid = reader.GetString(1);
                            name = reader.GetValue(3);
                            stars = reader.GetValue(8);
                            since = reader.GetValue(10);

                            Friends_Grid.Rows.Add(new object[] { fuid, name, stars, since });
                        }
                    }
                }
                conn.Close();
            }
            setTips();
        }
        public void setTips()
        {
            var uid = User_ID_Text.Text;

            object Uname,Bname, tip, city;
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM friends JOIN user_table ON user_table.uid=friends.friend_uid JOIN tips on friends.friend_uid=tips.uid JOIN business ON tips.bid=business.bid WHERE friends.uid='" + uid + "' order by date_of_tip DESC"; 
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            Uname = reader.GetValue(3);
                            Bname = reader.GetValue(18);
                            city = reader.GetValue(19);
                            tip = reader.GetValue(14);

                            Tips_Grid.Rows.Add(new object[] { Uname, Bname, city, tip });
                        }
                    }
                }
                conn.Close();
            }
        }
        private void Tip_Box_TextChanged(object sender, EventArgs e)
        {
            string tip = Tip_Box.Text;
            //then only run the command once the button has been pressed
        }

        private void City_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = State_Drop_Down.SelectedItem;
            var city = City_List.SelectedItem;

            SearchResults2(state, city);
            addZips(city,state);

        }

        private void Show_Checkins_Button_Click(object sender, EventArgs e)
        {

            View_Checkins pop = new View_Checkins();
            pop.Show();



        }
       

        private void Search_Results_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            popShowBus();
        }

        private void Zip_Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = State_Drop_Down.SelectedItem;
            var city = City_List.SelectedItem;
            var zip = Zip_Box.SelectedItem;

            fillSearchResults(state, city, zip);
            setCatTable(state, city, zip);
        }

        private void User_ID_Text_TextChanged(object sender, EventArgs e)
        {
           
            

        }

        private void Set_ID_Click(object sender, EventArgs e)
        {
            /*Friends_Grid.Rows.Clear();
            Friends_Grid.Refresh();
            //Object[] Friend_list = new Object[]();
            List<String> friend_list = new List<string>();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT friend_uid FROM friends Where uid='" + User_ID_Text.Text + "'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            friend_list.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }

            for (int i = 0; i < friend_list.Count; i++ )
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT uid,name,average_stars,time_yelping FROM user_table Where uid='" + friend_list[i]  + "'";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetString(0);
                                var name = reader.GetString(1);
                                var average_stars = reader.GetString(2);
                                var yelping_since = reader.GetString(3);
                                Friends_Grid.Rows.Add(new object[] { id, id, average_stars, yelping_since });
                            }
                        }
                    }
                    conn.Close();
                }
            }*/
                setUID();
        }

        private void To_Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            refineSearchHour();
        }

        private void Remove_Friend_Click(object sender, EventArgs e)
        {
            var Fuid = Friends_Grid.SelectedCells[0].Value.ToString();
            var uid = User_ID_Text.Text;
            
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM friends WHERE uid='"+ uid + "' AND friend_uid='"+ Friends_Grid.SelectedCells[0].EditedFormattedValue.ToString() +"'";
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));

                           
                        }
                    }
                }
                conn.Close();
            }
            setFriendsList();
        }

        private void Rate_Button_Click(object sender, EventArgs e)
        {
            //DataGridViewRow row = Friends_Grid.SelectedRows[0];
            //var name = row.Cells[1];
            //Console.WriteLine();
            int rate = Int32.Parse(Rate_T.Text); ;
            if (rate > 5)
                rate = 5;
            if (rate < 0)
                rate = 0;
            

            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    /*cmd.CommandText = "Select uid from user_table where name='" + name.ToString() + "' AND average_stars='"+Friends_Grid.SelectedRows[1].ToString()+"' AND time_yelping='"+Friends_Grid.SelectedRows[2].ToString()+"'";
                    Console.WriteLine(cmd.CommandText);
                    var uid = cmd.ExecuteReader();
                    Console.WriteLine(uid.GetValue(0).ToString());*/
                    cmd.CommandText = "UPDATE user_table SET average_stars=(average_stars+" + rate + ")/2 WHERE uid='" + Friends_Grid.SelectedCells[0].EditedFormattedValue.ToString() + "';";
                    var i = cmd.ExecuteNonQuery();
                    
                   
                }
                conn.Close();
            }
            setFriendsList();
        }

        private void Show_Tips_Button_Click(object sender, EventArgs e)
        {
            //Object name = Search_Results.SelectedCells.
            Show_Tips tipsWin = new Show_Tips();
            String bid = "";
            Object name = Search_Results.SelectedCells[0].EditedFormattedValue;
            tipsWin.populateTipsColumns();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    //"where "+day+"open='\""+opHour+"\"' and "+day+"close ='\""+clHour+"\"'";
                    cmd.CommandText = "Select bid from business where '" + State_Drop_Down.SelectedItem.ToString() + "'=business.state and '" + City_List.SelectedItem.ToString() + "'=business.city and '" + name.ToString()+ "'=business.name";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bid = reader.GetString(0);
                        }
                    }
                }
                conn.Close();
            }
            tipsWin.populateTipsGrid(bid);//Search_Results.SelectedCells[0].Value.ToString());
           
            tipsWin.Show();

            

        }

        private void Day_Box_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Categories_Checklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = State_Drop_Down.SelectedItem;
            var city = City_List.SelectedItem;
            var zip = Zip_Box.SelectedItem;

            fillSearchResultsCategory(state, city, zip);
            setCatTable(state, city, zip);
        }

        private void Add_tip_Click(object sender, EventArgs e)
        {
            Object name = Search_Results.SelectedCells[0].EditedFormattedValue;
            String bid = "";
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT bid FROM business WHERE state='" + State_Drop_Down.SelectedItem.ToString() + "' and '" + City_List.SelectedItem.ToString() + "'=business.city and '" + name.ToString()+ "'=business.name";
 
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            bid = reader.GetString(0);
                        }
                    }
                }
                conn.Close();
            }

            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                String something = "";
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "insert into tips values('" + uid_tip_box.Text +"','" + bid + "','" + Tip_Box.Text + "','2017-4-24',0)";
                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Tip_Box.Text = "Tip submitted";
                            something = reader.GetString(0);
                        }
                    }
                }
                conn.Close();
            }
        }
    
    }
}
