/*WSU EECS CptS 451*/
/*Instructor: Sakire Arslan Ay*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parse_yelp
{
    class Parser
    {
        //initialize the input/output data directory. Currently set to execution folder. 
        public static String dataDir = "C:\\Users\\mvs6_000\\Desktop\\Databases\\ParseYelpData-CptS451(1)\\ParseYelpData-CptS451\\ParseYelp\\yelp\\";
        static void Main(string[] args)
        {
            JSONParser my_parser =  new JSONParser();

            //Parse yelp_business.json 
            my_parser.parseJSONFile(dataDir + "yelp_business.json", dataDir + "business.txt");

            //Parse yelp_user.json 
            my_parser.parseJSONFile(dataDir + "yelp_user.json", dataDir + "user.txt");

            //Parse yelp_tip.json 
            my_parser.parseJSONFile(dataDir + "yelp_tip.json", dataDir + "tip.txt");

            //Parse yelp_checkin.json 
            my_parser.parseJSONFile(dataDir + "yelp_checkin.json", dataDir + "checkin.txt");

        }
    }
}
