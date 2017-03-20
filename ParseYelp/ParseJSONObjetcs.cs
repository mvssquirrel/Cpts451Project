using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Text.RegularExpressions;

namespace parse_yelp
{
    
    class ParseJSONObjects
    {              

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
            //You may extract values for certain keys by specifying the key name. 
            //Example: extract business_id
            String business_id = my_jsonStr["business_id"].ToString();
                        
            /*To retrieve list of Keys in JSON :
                    my_jsonStr.Keys.ToArray()[0]  is the "business_id" key. */
            /*To retrieve list of Values in JSON 
                    my_jsonStr.Values.ToArray()[0]  is the value for "business_id".*/
            
            //Alternative ways to extract business_id:
            business_id = my_jsonStr[my_jsonStr.Keys.ToArray()[0]].ToString();
            business_id = my_jsonStr.Values.ToArray()[0].ToString();

            /* EXTRACT OTHER KEY VALUES */

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
            for(int i=0; i<categories.Count; i++){
                business_data = business_data + cleanTextforSQL(categories[i].ToString()) + ","; 
            }
            business_data = business_data + "]\n";

            return business_data;
            
        } 

        /* Extract user information*/
        public string ProcessUsers(JsonObject my_jsonStr)
        {
            //Example: extract user_id
            String user_id = cleanTextforSQL(my_jsonStr["user_id"].ToString());
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


                return (user_data);
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

            return (tip_data);
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

                return (checkin_data);
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
