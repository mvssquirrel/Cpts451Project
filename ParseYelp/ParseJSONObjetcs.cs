using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Text.RegularExpressions;
using Npgsql;
using System.Data.SqlClient;
namespace parse_yelp
{
    
    class ParseJSONObjects
    {
        private string buildConnString()
        {
            return "Host=localHost; Username=postgres; Password=muskrat; Database = Milestone3a";
        }
        public ParseJSONObjects( )
        {
        }
        
        public void Close( )
        {
        }


        private string cleanTextforSQL(string inStr)
        {
            String outStr = Encoding.GetEncoding("iso-8859-1").GetString(Encoding.UTF8.GetBytes(inStr));
            outStr = outStr.Replace("\"", "").Replace("'", "`").Replace(@"\n", " ").Replace(@"\u000a", " ").Replace("\\", " ").Replace("é", "e").Replace("ê", "e").Replace("Ã¼", "A").Replace("Ã", "A").Replace("¤", "").Replace("©", "c").Replace("¶","");
            outStr = Regex.Replace(outStr,@"[^\u0020-\u007E]", "?");         
            return outStr;
        }



        /* Extract business information*/
        public string ProcessBusiness(JsonObject my_jsonStr)
        {
            String business_id = my_jsonStr["business_id"].ToString();
            String category_list = my_jsonStr["categories"].ToString();

            String category = "";
            List<String> clist = new List<string>();
            bool in_category = false;

            for (int i = 1; i < category_list.Length - 1; i++)
            {
                if (category_list[i] == '"')
                {
                    if (in_category)
                    {
                        in_category = false;
                        clist.Add(category);
                    }
                    else
                    {
                        in_category = true;
                        category = "";
                    }
                }
                else if (in_category)
                {
                    category = category + category_list[i];
                }
            }

            
            
            
            //You may extract values for certain keys by specifying the key name. 
            //Example: extract business_id
            //String business_id = my_jsonStr["business_id"].ToString();

            /*To retrieve list of Keys in JSON :
                    my_jsonStr.Keys.ToArray()[0]  is the "business_id" key. 
            /*To retrieve list of Values in JSON 
                    my_jsonStr.Values.ToArray()[0]  is the value for "business_id".

            //Alternative ways to extract business_id:
            business_id = my_jsonStr[my_jsonStr.Keys.ToArray()[0]].ToString();
            business_id = my_jsonStr.Values.ToArray()[0].ToString();

             EXTRACT OTHER KEY VALUES */

            //Clean text and remove any characters that might cause errors in PostgreSQL.
            String business_data =
                    cleanTextforSQL(business_id) + "\t" +
                    cleanTextforSQL(my_jsonStr["name"].ToString()) + "\t" +
                    cleanTextforSQL(my_jsonStr["full_address"].ToString()) + "\t" +
                    cleanTextforSQL(my_jsonStr["state"].ToString()) + "\t" +
                    cleanTextforSQL(my_jsonStr["city"].ToString()) + "\t" +
                    my_jsonStr["latitude"].ToString() + "\t" +
                    my_jsonStr["longitude"].ToString() + "\t" +
                    my_jsonStr["stars"].ToString() + "\t" +
                    my_jsonStr["review_count"].ToString() + "\t" +
                    my_jsonStr["open"].ToString() + "\t[";
            JsonArray categories = (JsonArray)my_jsonStr["categories"];
            bool contains;
            string monOpen;
            string monClose;
            string tueOpen;
            string tueClose;
            string wedOpen;
            string wedClose;
            string thurOpen;
            string thurClose;
            string friOpen;
            string friClose;
            string satOpen;
            string satClose;
            string sunOpen;
            string sunClose;
            JsonObject hours = my_jsonStr["hours"].ToJsonObject();

            contains = hours.ContainsKey("Monday");
            if(contains)
            {
                JsonObject mon = hours["Monday"].ToJsonObject();
                monOpen = mon["open"].ToString();
                monClose = mon["close"].ToString();
            }
            else
            {
                monOpen = "closed";
                monClose = "closed";
            }
           

            contains = hours.ContainsKey("Tuesday");
            if (contains)
            {
                JsonObject tue = hours["Tuesday"].ToJsonObject();
                tueOpen = tue["open"].ToString();
                tueClose = tue["close"].ToString();
            }
            else
            {
                tueOpen = "closed";
                tueClose = "closed";
            }
          
            contains = hours.ContainsKey("Wednesday");
            if (contains)
            {
                JsonObject wed = hours["Wednesday"].ToJsonObject();
                wedOpen = wed["open"].ToString();
                wedClose = wed["close"].ToString();
            }
            else
            {
                wedOpen = "closed";
                wedClose = "closed";
            }

           
            contains = hours.ContainsKey("Thursday");
            if (contains)
            {
                JsonObject thur = hours["Thursday"].ToJsonObject();
                thurOpen = thur["open"].ToString();
                thurClose = thur["close"].ToString();
            }
            else
            {
                thurOpen = "closed";
                thurClose = "closed";
            }
           
            contains = hours.ContainsKey("Friday");
            if (contains)
            {
                JsonObject fri = hours["Friday"].ToJsonObject();
                friOpen = fri["open"].ToString();
                friClose = fri["close"].ToString();
            }
            else
            {
                friOpen = "closed";
                friClose = "closed";
            }
          
            contains = hours.ContainsKey("Saturday");
            if (contains)
            {
                JsonObject sat = hours["Saturday"].ToJsonObject();
                satOpen = sat["open"].ToString();
                satClose = sat["close"].ToString();
            }
            else
            {
                satOpen = "closed";
                satClose = "closed";
            }
           
            contains = hours.ContainsKey("Sunday");
            if (contains)
            {
                JsonObject sun = hours["Sunday"].ToJsonObject();
                sunOpen = sun["open"].ToString();
                sunClose = sun["close"].ToString();
            }
            else
            {
                sunOpen = "closed";
                sunClose = "closed";
            }
           
            //Console.WriteLine("categories: " + categories);
           
            double lat = Convert.ToDouble(my_jsonStr["latitude"].ToString());
            double lon = Convert.ToDouble(my_jsonStr["longitude"].ToString());
            double star = Convert.ToDouble(my_jsonStr["stars"].ToString());
            int review = Convert.ToInt32(my_jsonStr["review_count"].ToString());
            //Console.WriteLine(categories[0]);
            //Console.WriteLine("INSERT INTO buisness  VALUES(" + business_id + "," + my_jsonStr["name"].ToString() + "," +
            //      my_jsonStr["city"].ToString() + "," + my_jsonStr["state"].ToString() + "," + categories + "," +
            //  my_jsonStr["latitude"].ToString() + "," + my_jsonStr["longitude"].ToString() + "," +
            //my_jsonStr["stars"].ToString() + "," + "0" + "," + my_jsonStr["full_address"].ToString() + ");");

            
            string cat = "";
               for (int i = 0; i < categories.Count; i++)
               {
                   cat = cat + " " + cleanTextforSQL(categories[i].ToString());//THIS IS NEW AND NEEDED
                   business_data = business_data + cleanTextforSQL(categories[i].ToString()) + ",";
               }
            String address = my_jsonStr["full_address"].ToString();
            String zip = address.Substring(address.Length - 6, 5);
            if (zip[4] == '"') zip = zip.Substring(0, 4);
            int num_zip = 0;
            if (!Int32.TryParse(zip, out num_zip))
            {
                num_zip = -1;
            }

               using (var conn = new NpgsqlConnection(buildConnString()))
               {
                   conn.Open();
                   using (var cmd = new NpgsqlCommand())
                   {
                       cmd.Connection = conn;
                      //this is all the shit you need to insert into database
                       cmd.CommandText = "INSERT INTO business  VALUES(@bid,@name,@city,@state,@latitude,@longitude,@stars,0,@full_addr,@zip,@review_count);";
                       cmd.Parameters.AddWithValue("@bid",business_id.Trim('\"'));
                       cmd.Parameters.AddWithValue("@name",my_jsonStr["name"].ToString());
                       cmd.Parameters.AddWithValue("@city", my_jsonStr["city"].ToString());
                       cmd.Parameters.AddWithValue("@state", my_jsonStr["state"].ToString());
                       //cmd.Parameters.AddWithValue("@categories", cat);
                       cmd.Parameters.AddWithValue("@latitude", lat);
                       cmd.Parameters.AddWithValue("@longitude", lon);
                       cmd.Parameters.AddWithValue("@stars", star);
                       cmd.Parameters.AddWithValue("@full_addr", my_jsonStr["full_address"].ToString());
                       cmd.Parameters.AddWithValue("@zip", num_zip);
                       cmd.Parameters.AddWithValue("@review_count", review);

                       cmd.ExecuteNonQuery();
                   }
                   conn.Close();
               }
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    //this is all the shit you need to insert into database
                    cmd.CommandText = "INSERT INTO hours  VALUES(@bid,@monOpen,@monClose,@tueOpen,@tueClose,@wedOpen,@wedClose,@thurOpen,@thurClose,@friOpen,@friClose,@satOpen,@satClose,@sunOpen,@sunClose);";
                    cmd.Parameters.AddWithValue("@bid", business_id.Trim('\"'));
                    cmd.Parameters.AddWithValue("@monOpen", monOpen);
                    cmd.Parameters.AddWithValue("@monClose", monClose);
                    cmd.Parameters.AddWithValue("@tueOpen", tueOpen);
                    cmd.Parameters.AddWithValue("@tueClose", tueClose);
                    cmd.Parameters.AddWithValue("@wedOpen", wedOpen);
                    cmd.Parameters.AddWithValue("@wedClose", wedClose);
                    cmd.Parameters.AddWithValue("@thurOpen", thurOpen);
                    cmd.Parameters.AddWithValue("@thurClose", thurClose);
                    cmd.Parameters.AddWithValue("@friOpen", friOpen);
                    cmd.Parameters.AddWithValue("@friClose", friClose);
                    cmd.Parameters.AddWithValue("@satOpen", satOpen);
                    cmd.Parameters.AddWithValue("@satClose", satClose);
                    cmd.Parameters.AddWithValue("@sunOpen", sunOpen);
                    cmd.Parameters.AddWithValue("@sunClose", sunClose);

                    cmd.ExecuteNonQuery();

                }
                conn.Close();
            }

            for (int j = 0; j < clist.Count; j++ )
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {

                        cmd.Connection = conn;

                        cmd.CommandText = "Insert Into categories Values(@bid,@category);";
                        cmd.Parameters.AddWithValue("@bid", business_id);
                        cmd.Parameters.AddWithValue("@category", cleanTextforSQL(clist[j]));
                        cmd.ExecuteNonQuery();

                    }
                    conn.Close();
                }
            }

            business_data = business_data + "]\n";

            //return business_data;
            return "";
        } 

        /* Extract user information*/
        public string ProcessUsers(JsonObject my_jsonStr)
        {
            String user_id = cleanTextforSQL(my_jsonStr["user_id"].ToString());
            String friend_list = my_jsonStr["friends"].ToString();
            //friend_list = friend_list.Substring(1, friend_list.Length - 1);

            String friend = "";
            List<String> flist = new List<string>();
            bool in_friend = false;

            for (int i = 1; i < friend_list.Length-1; i++ )
            {
                if (friend_list[i] == '"')
                {
                    if (in_friend)
                    {
                        in_friend = false;
                        flist.Add(friend);
                    }
                    else
                    {
                        in_friend = true;
                        friend = "";
                    }
                }
                else if (in_friend)
                {
                    friend = friend + friend_list[i];
                }
            }

            

                
                //Example: extract user_id
                //String user_id = cleanTextforSQL(my_jsonStr["user_id"].ToString());
                /* EXTRACT OTHER KEY VALUES */

                String user_data =
                    cleanTextforSQL(user_id) + "\t" +
                    cleanTextforSQL(my_jsonStr["name"].ToString()) + "\t" +
                    cleanTextforSQL(my_jsonStr["fans"].ToString()) + "\t" +
                    cleanTextforSQL(my_jsonStr["average_stars"].ToString()) + "\t" +
                    cleanTextforSQL(my_jsonStr["yelping_since"].ToString()) + "\t" ;
                JsonObject vote_stuff = (JsonObject)my_jsonStr["votes"];
                user_data = user_data +
                    cleanTextforSQL("votes") + "\t[" +
                    cleanTextforSQL(vote_stuff["cool"].ToString()) + "," +
                    cleanTextforSQL(vote_stuff["funny"].ToString()) + "," +
                    cleanTextforSQL(vote_stuff["useful"].ToString()) + "]\n";
                int vote_count = 0, temp = 0;
                if (Int32.TryParse(vote_stuff["cool"].ToString(), out temp))
                {
                    vote_count += temp;
                }
                if (Int32.TryParse(vote_stuff["funny"].ToString(), out temp))
                {
                    vote_count += temp;
                }
                if (Int32.TryParse(vote_stuff["useful"].ToString(), out temp))
                {
                    vote_count += temp;
                }

                int review_count = Convert.ToInt32(my_jsonStr["review_count"].ToString());
                double average_stars = Convert.ToDouble(my_jsonStr["average_stars"].ToString());
                int fans = Convert.ToInt32(my_jsonStr["fans"].ToString());

                List<String> friends = new List<string>();
                string person1 = "";
                string friend_list1 = my_jsonStr["friends"].ToString();
                bool in_out = false;
                for (int i = 1; i < friend_list1.Length; i++)
                {
                    if (friend_list1[i] == ']')
                        break;
                    if (friend_list1[i] == '"')
                    {
                        if (in_out)
                        {
                            in_out = false;
                            friends.Add(person1);
                            person1 = "";
                        }
                        else
                            in_out = true;
                    }
                    if (in_out)
                    {
                        person1 = person1 + friend_list1[i].ToString();
                    }
                }
                String[] friends2 = new String[friends.Count];
                for (int i = 0; i < friends.Count; i++)
                {
                    friends2[i] = String.Copy(friends[i]);
                }

                JsonObject vote_string = (JsonObject)my_jsonStr["votes"];
            String cool = vote_string["cool"].ToString();
            String funny = vote_string["funny"].ToString();
            String useful = vote_string["useful"].ToString();
            int coolnum = 0, funnynum = 0, usefulnum = 0;
            if (!Int32.TryParse(cool, out coolnum)) coolnum = -1;
            if (!Int32.TryParse(funny, out funnynum)) funnynum = -1;
            if (!Int32.TryParse(useful, out usefulnum)) usefulnum = -1;
            
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        cmd.CommandText = "Insert Into user_table Values(@uid,@name,@review_count,@cool,@funny,@useful,@average_stars,@votes,@time_yelping,@fans);";
                        cmd.Parameters.AddWithValue("@uid", user_id);
                        cmd.Parameters.AddWithValue("@name", my_jsonStr["name"].ToString());
                        cmd.Parameters.AddWithValue("@review_count", review_count);
                        cmd.Parameters.AddWithValue("@cool", coolnum);
                        cmd.Parameters.AddWithValue("@funny", funnynum);
                        cmd.Parameters.AddWithValue("@useful", usefulnum);
                        cmd.Parameters.AddWithValue("@average_stars", average_stars);
                        cmd.Parameters.AddWithValue("@votes", vote_count);
                        //cmd.Parameters.AddWithValue("@friends", friends2);
                        cmd.Parameters.AddWithValue("@time_yelping", my_jsonStr["yelping_since"].ToString());
                        cmd.Parameters.AddWithValue("@fans", fans);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                for (int j = 0; j < flist.Count; j++)
                {
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {

                            cmd.Connection = conn;

                            cmd.CommandText = "Insert Into friends Values(@uid,@friend_uid);";
                            cmd.Parameters.AddWithValue("@uid", user_id);
                            cmd.Parameters.AddWithValue("@friend_uid", cleanTextforSQL(flist[j]));
                            cmd.ExecuteNonQuery();

                        }
                        conn.Close();
                    }
                }

                    //return (user_data);
                return "";
        }

        /* Extract tips information*/
        public string ProcessTips(JsonObject my_jsonStr)
        {
            //Example: extract business_id
            String user_id = cleanTextforSQL(my_jsonStr["user_id"].ToString());
            /* EXTRACT OTHER KEY VALUES */

            String tip_data =
                cleanTextforSQL(user_id) + "\t" +
                cleanTextforSQL(my_jsonStr["business_id"].ToString()) + "\t" +
                cleanTextforSQL(my_jsonStr["text"].ToString()) + "\t" +
                cleanTextforSQL(my_jsonStr["date"].ToString()) + "\t" +
                cleanTextforSQL(my_jsonStr["likes"].ToString()) + "\n";

            int likes = Convert.ToInt32(my_jsonStr["likes"].ToString());

            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "Insert Into Tips Values(@uid,@bid,@tip,@date_of_tip,@likes);";
                    cmd.Parameters.AddWithValue("@uid", user_id);
                    cmd.Parameters.AddWithValue("@bid", my_jsonStr["business_id"].ToString().Trim('\"'));
                    cmd.Parameters.AddWithValue("@tip", my_jsonStr["text"].ToString());
                    cmd.Parameters.AddWithValue("@date_of_tip", my_jsonStr["date"].ToString());
                    cmd.Parameters.AddWithValue("@likes",likes);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            //return (tip_data);
            return "";
        }

        /* Extract checkin information*/
        public string ProcessCheckin(JsonObject my_jsonStr)
        {
            int [] hours = new int [28];
            //Example: extract business_id
            String business_id = cleanTextforSQL(my_jsonStr["business_id"].ToString());
            /* EXTRACT OTHER KEY VALUES */

            String checkin_data =
                cleanTextforSQL(business_id) + "\t" +
                cleanTextforSQL("checkin_info") + "\t[";
            JsonObject checks = (JsonObject)my_jsonStr["checkin_info"];
            for (int i = 0; i < 28; i++)
                hours[i] = 0;
            int hour = 0;
            int day = 0;
            for (int i = 0; i < checks.Count; i++ )
            {
                Tuple<String, String> pair = stripHours(checks.ElementAt(i).Key.ToString());
                if(Int32.TryParse(pair.Item1, out hour))
                {
                    if (Int32.TryParse(pair.Item2, out day))
                    {
                        //hours[4 * day + hour]++;
                        if (4 * day + 3 < 28)
                        { 
                            if (hour >= 6 && hour < 12)
                                hours[4 * day + 0]++;
                            else if (hour >= 12 && hour < 17)
                                hours[4 * day + 1]++;
                            else if (hour >= 17 && hour < 23)
                                hours[4 * day + 2]++;
                            else
                                hours[4 * day + 3]++;
                        }
                    }
                }
                    /*if (hour >= 6 && hour < 12)
                    {
                        checkin_data = checkin_data + cleanTextforSQL(pair.Item2) + "," + cleanTextforSQL("morning") + "," + cleanTextforSQL(checks.ElementAt(i).Value.ToString()) + "\t"; 
                    }
                    else if (hour >= 12 && hour < 17)
                    {
                        checkin_data = checkin_data + cleanTextforSQL(pair.Item2) + "," + cleanTextforSQL("afternoon") + "," + cleanTextforSQL(checks.ElementAt(i).Value.ToString()) + "\t"; 
                    }
                    else if (hour >= 17 && hour < 23)
                    {
                        checkin_data = checkin_data + cleanTextforSQL(pair.Item2) + "," + cleanTextforSQL("evening") + "," + cleanTextforSQL(checks.ElementAt(i).Value.ToString()) + "\t"; 
                    }
                    else
                    {
                        checkin_data = checkin_data + cleanTextforSQL(pair.Item2) + "," + cleanTextforSQL("night") + "," + cleanTextforSQL(checks.ElementAt(i).Value.ToString()) + "\t"; 
                    }*/
                
            }
            for (int i = 0; i < 7; i++ )
            {
                for (int j = 0; j < 4; j++)
                {
                    switch (i)
                    {
                        case 0:
                            checkin_data = checkin_data + cleanTextforSQL("Sunday-");
                            break;
                        case 1:
                            checkin_data = checkin_data + cleanTextforSQL("Monday-");
                            break;
                        case 2:
                            checkin_data = checkin_data + cleanTextforSQL("Tuesday-");
                            break;
                        case 3:
                            checkin_data = checkin_data + cleanTextforSQL("Wednesday-");
                            break;
                        case 4:
                            checkin_data = checkin_data + cleanTextforSQL("Thursday-");
                            break;
                        case 5:
                            checkin_data = checkin_data + cleanTextforSQL("Friday-");
                            break;
                        case 6:
                            checkin_data = checkin_data + cleanTextforSQL("Saturday-");
                            break;

                    }

                    if (j == 0)
                        checkin_data = checkin_data + cleanTextforSQL("Morning :") + cleanTextforSQL(hours[4 * i + j].ToString());
                    else if (j == 1)
                        checkin_data = checkin_data + cleanTextforSQL("Afternoon :") + cleanTextforSQL(hours[4 * i + j].ToString());
                    else if (j == 2)
                        checkin_data = checkin_data + cleanTextforSQL("Evening :") + cleanTextforSQL(hours[4 * i + j].ToString());
                    else
                        checkin_data = checkin_data + cleanTextforSQL("Night :") + cleanTextforSQL(hours[4 * i + j].ToString());
                    if (4 * i + j < 27)
                        checkin_data = checkin_data + ",";
                }
                
            }
                checkin_data = checkin_data + "]\n";
            
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        cmd.CommandText = "Insert Into Check_In Values(@bid,@mon,@tue,@wed,@thur,@fri,@sat,@sun);";
                        cmd.Parameters.AddWithValue("@bid", business_id);
                        cmd.Parameters.AddWithValue("@mon", hours[4] + hours[5] + hours[6] + hours[7]);
                        cmd.Parameters.AddWithValue("@tue", hours[8] + hours[9] + hours[10] + hours[11]);
                        cmd.Parameters.AddWithValue("@wed", hours[12] + hours[13] + hours[14] + hours[15]);
                        cmd.Parameters.AddWithValue("@thur", hours[16] + hours[17] + hours[18] + hours[19]);
                        cmd.Parameters.AddWithValue("@fri", hours[20] + hours[21] + hours[22] + hours[23]);
                        cmd.Parameters.AddWithValue("@sat", hours[24] + hours[25] + hours[26] + hours[27]);
                        cmd.Parameters.AddWithValue("@sun", hours[0] + hours[1] + hours[2] + hours[3]);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                //return (checkin_data);
            return "";
        }
                              
        public Tuple<String, String> stripHours (String group)
        {
            String hour = "", day = "";
            Boolean flag = false;
            for (int i = 0; i < group.Length; i++)
            {
                if (group[i] == '-')
                {
                    flag = true;
                }
                else if (!flag)
                {
                    hour = hour + group[i].ToString();
                }
                else
                {
                    day = day + group[i].ToString();
                }

            }
            return new Tuple<String, String>(hour, day);
        }

        //public int hourToSegment()
    }
}
