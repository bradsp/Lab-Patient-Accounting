using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Models
{
    public static class Dictionaries
    {
        public static Dictionary<string, string> stateSource = new Dictionary<string, string>
            {
                { "", "--Select--" },
                { "AL", "Alabama" },
                { "AK", "Alaska" },
                { "AS", "American Samoa" },
                { "AZ", "Arizona" },
                { "AR", "Arkansas" },
                { "CA", "California" },
                { "CO", "Colorado" },
                { "CT", "Connecticut" },
                { "DE", "Delaware" },
                { "DC", "District of Columbia" },
                { "FL", "Florida" },
                { "GA", "Georgia" },
                { "GU", "Guam" },
                { "HI", "Hawaii" },
                { "ID", "Idaho" },
                { "IL", "Illinois" },
                { "IN", "Indiana" },
                { "IA", "Iowa" },
                { "KS", "Kansas" },
                { "KY", "Kentucky" },
                { "LA", "Louisiana" },
                { "ME", "Maine" },
                { "MD", "Maryland" },
                { "MA", "Massachusetts" },
                { "MI", "Michigan" },
                { "MN", "Minnesota" },
                { "UM", "Minor Outlying Islands" },
                { "MS", "Mississippi" },
                { "MO", "Missouri" },
                { "MT", "Montana" },
                { "NE", "Nebraska" },
                { "NV", "Nevada" },
                { "NH", "New Hampshire" },
                { "NJ", "New Jersey" },
                { "NM", "New Mexico" },
                { "NY", "New York" },
                { "NC", "North Carolina" },
                { "ND", "North Dakota" },
                { "MP", "Northern Mariana Islands" },
                { "OH", "Ohio" },
                { "OK", "Oklahoma" },
                { "OR", "Oregon" },
                { "PA", "Pennsylvania" },
                { "PR", "Puerto Rico" },
                { "RI", "Rhode Island" },
                { "SC", "South Carolina" },
                { "SD", "South Dakota" },
                { "TN", "Tennessee" },
                { "TX", "Texas" },
                { "VI", "U.S. Virgin Islands" },
                { "UT", "Utah" },
                { "VT", "Vermont" },
                { "VA", "Virginia" },
                { "WA", "Washington" },
                { "WV", "West Virginia" },
                { "WI", "Wisconsin" },
                { "WY", "Wyoming" }
            };
        public static Dictionary<string, string> sexSource = new Dictionary<string, string>
        {
            { "", "--Select--" },
            { "M", "Male" },
            { "F", "Female" },
            { "U", "Unknown" }
        };

        public static Dictionary<string, string> WriteOffCodes = new Dictionary<string, string>
        {
            { "", "--Select--" },
            { "0100", "Overlapping hospital charges" },
            { "0200", "LMRP / Not medically necessary" },
            { "0300", "Timely Filing" },
            { "0400", "Out of network lab" },
            { "0500", "Tenncare secondary / pt not liable" },
            { "0600", "Indigent Care" },
            { "0700", "Physician Courtesy" },
            { "0800", "MCL employees Courtesy" },
            { "0900", "Ordering / processing errors(MCL error)" },
            { "1000", "Small balance write off" },
            { "1100", "Employer Services special price" },
            { "1200", "Drug Screen special price" },
            { "1300", "Prompt pay discount" },
            { "1400", "Self Pay discount" },
            { "1500", "Other reason not listed above / will need to add detail explanation in comment." }
        };

        public static Dictionary<string, string> maritalSource = new Dictionary<string, string>
        {
            { "", "--Select--" },
            { "U", "Unknown" },
            { "S", "Single" },
            { "M", "Married" },
            { "W", "Widowed" },
            { "X", "Separated" },
            { "D", "Divorced" },
        };

        public static Dictionary<string, string> relationSource = new Dictionary<string, string>
        {
            //01 - Self;02 - Spouse;03 - Child;09 - Unknown;
            { "", "--Select--" },
            { "01", "Self" },
            { "02", "Spouse" },
            { "03", "Child" },
            { "04", "Other" },
            { "09", "Unknown" }
        };

        public static Dictionary<string, string> payorOrderSource = new Dictionary<string, string>
        {
            { "", "--Select--" },
            { "A", "Primary" },
            { "B", "Secondary" },
            { "C", "Tertiary" }
        };

        public static Dictionary<string, string> clientType = new Dictionary<string, string>
        {
            { "0", "Affiliate Hospital" },
            { "1", "Alliance Hospital" },
            { "2", "Other Hospital" },
            { "3", "Affiliate Clinic" },
            { "4", "Owned Clinic Lab" },
            { "5", "Other Clinic" },
            { "6", "Industry" },
            { "7", "Nursing Home" },
            { "8", "Home Health" },
            { "9", "Other Lab" },
            { "10", "Veterinary Clinic" }
        };

        public static Dictionary<string, string> emrType = new Dictionary<string, string>
        {
            { "EHS", "EHS/Greenway" },
            { "INTERFACE", "Interfaced Clinic" },
            { "LIFEPOINT", "LifePoint Client" },
            { "MCLOE", "CLOE Client" },
            { "NONE", "No Integrated EMR" }
        };

        public static Dictionary<string, string> feeSchedule = new Dictionary<string, string>
        {
            { "1", "1" },
            { "3", "3" }
        };

        public static Dictionary<string, string> counties = new Dictionary<string, string>
        {
            {"","--Select--" },
            {"Anderson","Anderson"},
            {"Bedford","Bedford"},
            {"Benton","Benton"},
            {"Bledsoe","Bledsoe"},
            {"Blount","Blount"},
            {"Bradley","Bradley"},
            {"Campbell","Campbell"},
            {"Cannon","Cannon"},
            {"Carroll","Carroll"},
            {"Carter","Carter"},
            {"Cheatham","Cheatham"},
            {"Chester","Chester"},
            {"Claiborne","Claiborne"},
            {"Clay","Clay"},
            {"Cocke","Cocke"},
            {"Coffee","Coffee"},
            {"Crockett","Crockett"},
            {"Cumberland","Cumberland"},
            {"Davidson","Davidson"},
            {"Decatur","Decatur"},
            {"DeKalb","DeKalb"},
            {"Dickson","Dickson"},
            {"Dyer","Dyer"},
            {"Fayette","Fayette"},
            {"Fentress","Fentress"},
            {"Franklin","Franklin"},
            {"Gibson","Gibson"},
            {"Giles","Giles"},
            {"Grainger","Grainger"},
            {"Greene","Greene"},
            {"Grundy","Grundy"},
            {"Hamblen","Hamblen"},
            {"Hamilton","Hamilton"},
            {"Hancock","Hancock"},
            {"Hardeman","Hardeman"},
            {"Hardin","Hardin"},
            {"Hawkins","Hawkins"},
            {"Haywood","Haywood"},
            {"Henderson","Henderson"},
            {"Henry","Henry"},
            {"Hickman","Hickman"},
            {"Houston","Houston"},
            {"Humphreys","Humphreys"},
            {"Jackson","Jackson"},
            {"Jefferson","Jefferson"},
            {"Johnson","Johnson"},
            {"Knox","Knox"},
            {"Lake","Lake"},
            {"Lauderdale","Lauderdale"},
            {"Lawrence","Lawrence"},
            {"Lewis","Lewis"},
            {"Lincoln","Lincoln"},
            {"Loudon","Loudon"},
            {"Macon","Macon"},
            {"Madison","Madison"},
            {"Marion","Marion"},
            {"Marshall","Marshall"},
            {"Maury","Maury"},
            {"McMinn","McMinn"},
            {"McNairy","McNairy"},
            {"Meigs","Meigs"},
            {"Monroe","Monroe"},
            {"Montgomery","Montgomery"},
            {"Moore","Moore"},
            {"Morgan","Morgan"},
            {"Obion","Obion"},
            {"Overton","Overton"},
            {"Perry","Perry"},
            {"Pickett","Pickett"},
            {"Polk","Polk"},
            {"Putnam","Putnam"},
            {"Rhea","Rhea"},
            {"Roane","Roane"},
            {"Robertson","Robertson"},
            {"Rutherford","Rutherford"},
            {"Scott","Scott"},
            {"Sequatchie","Sequatchie"},
            {"Sevier","Sevier"},
            {"Shelby","Shelby"},
            {"Smith","Smith"},
            {"Stewart","Stewart"},
            {"Sullivan","Sullivan"},
            {"Sumner","Sumner"},
            {"Tipton","Tipton"},
            {"Trousdale","Trousdale"},
            {"Unicoi","Unicoi"},
            {"Union","Union"},
            {"Van Buren","Van Buren"},
            {"Warren","Warren"},
            {"Washington","Washington"},
            {"Wayne","Wayne"},
            {"Weakley","Weakley"},
            {"White","White"},
            {"Williamson","Williamson"},
            {"Wilson","Wilson"}
        };

    }
}
