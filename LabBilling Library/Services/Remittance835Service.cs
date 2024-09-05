using EdiTools;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

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
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "LabBilling.Core.Specifications.x12-835-map.xml";

        using Stream stream = assembly.GetManifestResourceStream(resourceName);

        EdiDocument ediDocument = EdiDocument.Load(filePath);
        EdiMapping ediMapping = EdiMapping.Load(stream);

        XDocument xml = ediMapping.Map(ediDocument.Segments);

        try
        {
            List<Eob> eobs = new();
            List<EobDetail> eobDetails = new();
            Eob eob = new();
            EobDetail eobDetail = new();
            eob.CheckNo = xml.XPathSelectElement("/mapping/trn/trn02")!.Value;

            eob.PayDataNetReimbAmt = double.Parse(xml.XPathSelectElement("/mapping/bpr/bpr02").Value);
            eob.EftDate = DateTime.Parse(xml.XPathSelectElement("/mapping/bpr/bpr16").Value);

            //eob.Payor = xml.XPathSelectElement("/mapping/n1loop/n1/n102")

            foreach (XElement clp in xml.Descendants("clploop"))
            {
                eob.AccountNo = clp.Element("clp").Element("clp01").Value;
                eob.pay_data_cont_adj_amt = double.Parse(clp.Element("clp").Element("clp03").Value);
                eob.ClaimType = $"{clp.Element("clp").Element("clp08").Value}{clp.Element("clp").Element("clp09").Value}";


                foreach (XElement nm1 in clp.Descendants("nm1loop"))
                {
                    if (nm1.Element("nm1").Element("nm101").Value == "QC")
                    {
                        string lastname = nm1.Element("nm1").Element("nm103").Value;
                        string firstname = nm1.Element("nm1").Element("nm104").Value;
                        eob.SubscriberName = $"{lastname},{firstname}";
                    }
                }

                foreach (XElement dtm in clp.Descendants("dtmloop"))
                {
                    if (dtm.Element("dtm").Element("dtm01").Value == "232") { }
                    // statement from date

                    if (dtm.Element("dtm").Element("dtm01").Value == "233") { }
                    // statement thru date

                    if (dtm.Element("dtm").Element("dtm01").Value == "050") { }
                    // ??
                }

                foreach (XElement svc in clp.Descendants("svcloop"))
                {
                    //svc
                    eobDetail.ServiceCode = svc.Element("svc").Element("svc01").Element("svc0102").Value;
                    eobDetail.ChargeAmt = double.Parse(svc.Element("svc").Element("svc02").Value);
                    eobDetail.RevenueCode = svc.Element("svc").Element("svc04").Value;

                    foreach (XElement svcamt in svc.Descendants("amtloop"))
                    {
                        eobDetail.ChargeAmt = double.Parse(svcamt.Element("amt").Element("amt02").Value); //amt
                    }

                    foreach (XElement dtm in clp.Descendants("dtmloop"))
                    {
                        if (dtm.Element("dtm").Element("dtm01").Value == "405")
                        {
                            eob.EftDate = DateTime.Parse(dtm.Element("dtm").Element("dtm02").Value); //dtm 405
                            
                        }

                        if (dtm.Element("dtm").Element("dtm01").Value == "472")
                            eob.DateOfService = DateTime.Parse(dtm.Element("dtm").Element("dtm02").Value); //dtm 472

                    }

                    foreach (XElement cas in clp.Descendants("casloop"))
                    {
                        //cas
                        // adjustment code PR = patient responsibility
                        // adjustment code PI = patient responsibility
                        eobDetail.ReasonType = cas.Element("cas").Element("cas01").Value; // Extract adjustment code
                        eobDetail.ReasonCode = cas.Element("cas").Element("cas02").Value;
                        eobDetail.AdjAmt = double.Parse(cas.Element("cas").Element("cas03").Value);

                        eobDetails.Add(eobDetail);

                        eobDetail = NewEobDetail(eobDetail);

                        //need additional eobDetail here.
                        eobDetail.ReasonCode = cas.Element("cas").Element("cas05").Value;
                        eobDetail.AdjAmt = double.Parse(cas.Element("cas").Element("cas06").Value);

                        eobDetails.Add(eobDetail);
                    }
                }

                //write eob record to database
                eobs.Add(eob);
                eob = new();
            }

        }
        catch
        {

        }
    }

    private static EobDetail NewEobDetail(EobDetail eobDetail)
    {
        EobDetail eobDetailNew = new()
        {
            AccountNo = eobDetail.AccountNo,
            IsDeleted = eobDetail.IsDeleted,
            ClaimStatus = eobDetail.ClaimStatus,
            ServiceCode = eobDetail.ServiceCode,
            RevenueCode = eobDetail.RevenueCode,
            Units = eobDetail.Units,
            ApcNR = eobDetail.ApcNR,
            AllowedAmt = eobDetail.AllowedAmt,
            Stat = eobDetail.Stat,
            Wght = eobDetail.Wght,
            DateOfService = eobDetail.DateOfService,
            ChargeAmt = eobDetail.ChargeAmt,
            PaidAmt = eobDetail.PaidAmt,
            BillCycleDate = eobDetail.BillCycleDate,
            CheckNo = eobDetail.CheckNo,
        };

        return eobDetailNew;
    }

}

