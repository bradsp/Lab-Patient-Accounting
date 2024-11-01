﻿using System.Collections.Generic;

namespace LabBilling.Core.Models;

public static class Dictionaries
{
    public static readonly Dictionary<string, string> StateSource = new()
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

    public static readonly Dictionary<string, string> SexSource = new()
    {
        { "", "--Select--" },
        { "M", "Male" },
        { "F", "Female" },
        { "U", "Unknown" }
    };

    public static readonly Dictionary<string, string> WriteOffCodes = new()
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

    public static readonly Dictionary<string, string> MaritalSource = new()
    {
        { "", "--Select--" },
        { "U", "Unknown" },
        { "S", "Single" },
        { "M", "Married" },
        { "W", "Widowed" },
        { "X", "Separated" },
        { "D", "Divorced" },
    };

    public static readonly Dictionary<string, string> RelationSource = new()
    {
        //01 - Self;02 - Spouse;03 - Child;09 - Unknown;
        { "", "--Select--" },
        { "01", "Self" },
        { "02", "Spouse" },
        { "03", "Child" },
        { "04", "Other" },
        { "09", "Unknown" }
    };

    public static readonly Dictionary<string, string> PayorOrderSource = new()
    {
        { "", "--Select--" },
        { "A", "Primary" },
        { "B", "Secondary" },
        { "C", "Tertiary" }
    };

    public static readonly Dictionary<string, string> ClientType = new()
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

    public static readonly Dictionary<string, string> EmrType = new()
    {
        { "EHS", "EHS/Greenway" },
        { "INTERFACE", "Interfaced Clinic" },
        { "LIFEPOINT", "LifePoint Client" },
        { "MCLOE", "CLOE Client" },
        { "NONE", "No Integrated EMR" }
    };

    public static readonly Dictionary<string, string> FeeSchedule = new()
    {
        { "1", "1" },
        { "3", "3" }
    };

    public static readonly Dictionary<string, string> Counties = new()
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

    public static readonly Dictionary<string, string> ClaimFilingIndicatorCode = new()
    {
        {"","--Select--" },
        {"AM", "Automobile Medical"},
        {"BL", "Blue Cross/Blue Shield"},
        {"CH", "Champus"},
        {"CI", "Commercial Insurance Co."},
        {"17", "Dental Maintenance Organization"},
        {"DS", "Disability"},
        {"14", "Exclusive Provider Organization (EPO)"},
        {"FI", "Federal Employees Program"},
        {"HM", "Health Maintenance Organization"},
        {"16", "Health Maintenance Organization (HMO) Medicare Risk"},
        {"15", "Indemnity Insurance"},
        {"LM", "Liability Medical"},
        {"MA", "Medicare Part A"},
        {"MB", "Medicare Part B"},
        {"MC", "Medicaid"},
        {"ZZ", "Mutually Defined"},
        {"OF", "Other Federal Program"},
        {"11", "Other Non-Federal Programs"},
        {"13", "Point of Service (POS)"},
        {"12", "Preferred Provider Organization (PPO)"},
        {"TV", "Title V"},
        {"VA", "Veterans Affairs Plan"},
        {"WC", "Workers’ Compensation Health Claim"},
    };

    public static readonly Dictionary<string, string> CptModifiers = new()
    {
        {"<none>", "No Modifier" },
        {"TC", "Technical Component" },
        {"26", "Professional Component" },
        {"59", "Distinct Procedural Service" },
        {"90", "Reference (Outside) Laboratory" },
        {"91", "Repeat Test" },
        {"GZ", "No ABN obtained" },
        {"QW", "CLIA Waived Test" }
    };
}
