using System;
using System.Collections.Generic;
using EdiTools;
using LabBilling.Core.DataAccess;
using System.IO;
using System.Reflection;
using System.Xml.Xsl;
using System.Xml;
using EdiTools;

namespace LabBilling.Core.Services;

public class Remittance835Service
{
    private readonly IAppEnvironment _appEnvironment;


    public Remittance835Service(IAppEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment;
    }


    public void ProcessFile(string filePath)
    {

        EdiDocument ediDocument = EdiDocument.Load(filePath);

        try
        {
            string account = string.Empty, patientName = string.Empty, adjustmentCode = string.Empty;
            string checkNo = string.Empty;
            decimal adjustmentAmount, paymentAmount;
            DateTime serviceDate;
            DateTime eftDate;
            DateTime chkDate;
            foreach (var segment in ediDocument.Segments)
            {
                switch (segment.Id)
                {
                    case "TRN": //check number
                        checkNo = segment[2];
                        break;
                    //get patient info
                    //NM1
                    case "NMI":
                        if (segment[1] == "QC")
                            patientName = $"{segment[3]},{segment[4]}";
                        break;
                    //BPR - payment method and amount
                    case "BPR":
                        paymentAmount = decimal.Parse(segment[2]);
                        eftDate = DateTime.Parse(segment[16]);
                        break;

                    //CLP - claim payment information -- start of new 2100 loop
                    case "CLP":
                        if (!string.IsNullOrEmpty(account) && segment[1] != account)
                        {
                            //new claim - write out remit data

                        }
                        account = segment[1];
                        adjustmentAmount = decimal.Parse(segment[3]);
                        break;
                    //CAS - claim adjustment information
                    case "CAS":
                        // adjustment code PR = patient responsibility
                        // adjustment code PI = patient responsibility
                        adjustmentCode = segment[1]; // Extract adjustment code
                        adjustmentAmount = decimal.Parse(segment[3]);
                        break;
                    //PLB - provider level adjustments
                    case "PLB":
                        break;
                    case "DTM":
                        switch (segment[1])
                        {
                            case "405": //check date
                                chkDate = DateTime.ParseExact(segment[2], "yyyyMMdd", null);
                                break;
                            case "472": //service date
                                serviceDate = DateTime.ParseExact(segment[2], "yyyyMMdd", null);
                                break;
                            case "232": //statement from date
                                break;
                            case "233": //statement thru date
                                break;
                            default:
                                break;
                        }
                        break;
                    case "SVC": //start loop 2110
                        break;
                    default:
                        break;
                }
            }

        }
        catch
        {

        }
    }

}

