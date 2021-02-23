/*
	B4 08/18/2005 Rick Crone
	
	Hist:
		08/18/2005 Rick Crone
			Added m_strErrMsg public string to hold exception
			message should Send() fail. Send() now returns
			a bool to indicate success or failure.
			
			*note: m_strErrMsg is cleared on each call to
					Send();

*/
using System;
//using System.Web.Mail; // email the 1.1 way
using System.Net.Mail; // email the 2.0 way


namespace RFClassLibrary
{
    /// <summary>
    /// 08/11/2005 Rick Crone - easy email
    /// 12/14/2006 Rick Crone - updated for 2.0.
    /// </summary>
    public class EMail
    {
        //private System.Web.Mail.MailMessage mailMsg=null; // obsolete in 2.0
        private System.Net.Mail.MailMessage mailMsg = null;
        /// <!-- storage for error message - this class should be derived from RFCObject
        ///  then this variable would not be needed here-->
        public string m_strErrMsg;

        /// <summary>
        /// contructor
        /// </summary>
        public EMail()
        {

            //mailMsg=new System.Web.Mail.MailMessage();// obsolete in 2.0
            mailMsg = new System.Net.Mail.MailMessage();

        }

        /// <summary>
        /// Send email Using the SmtpClient class. Be sure the email addresses include the "@" sign.
        /// You may send to multiple email addresses just seperate them with the ; colon character.
        /// 12/14/2006 Rick Crone
        /// </summary>
        /// <param name="strFrom">Should be in the format DAVID.KELLY@WTH.ORG</param>
        /// <param name="strTo">Should be in the format DAVID.KELLY@WTH.ORG</param>
        /// <param name="strSubject"></param>
        /// <param name="strBody"></param>
        /// <returns></returns>
        public bool Send(string strFrom, string strTo, string strSubject, string strBody)
        {
            bool bRetVal = false; // fail!
            m_strErrMsg = "";
            MailAddress from = new MailAddress(strFrom);//, "");


            // Add a carbon copy recipient.
            //MailAddress copy = new MailAddress("Notification_List@contoso.com");
            //message.CC.Add(copy);
            SmtpClient client = new SmtpClient("WTHEXCH.WTH.ORG");
            // Include credentials if the server requires them.
            //client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            //Console.WriteLine("Sending an e-mail message to {0} by using the SMTP host {1}.",
            //     to.Address, client.Host);
            string[] strTos = strTo.Split(new char[] { ';' });
            for (int i = 0; i <= strTos.GetUpperBound(0); i++)
            {
                try
                {
                    MailAddress to = new MailAddress(strTos[i]);//, "");
                    MailMessage message = new MailMessage(from, to);
                    // message.Subject = "Using the SmtpClient class.";
                    message.Subject = strSubject;
                    message.Body = strBody;
                    client.Send(message);
                    bRetVal = true;
                }
                catch (Exception excpt)
                {
                    m_strErrMsg += (excpt.ToString() + " ");

                }
            }
            return (bRetVal);

        }

        /*
                1.1 version now obsolete
         *      public bool Send(string strFrom, string strTo, string strSubject, string strBody)
                {
                    m_strErrMsg = "";
                    bool bRetVal = true;
                    mailMsg.From = strFrom; //"rick.crone@wth.org"; the 1.1 way

                    //Message To
                    mailMsg.To = strTo; //"rick.crone@wth.org";
                    //Message Subject
                    mailMsg.Subject = strSubject;// "testing";
                    //Message Body
                    mailMsg.Body = strBody; //"Hello email";
                    //Everything set..now send the mail
                    try
                    {
                        //SmtpMail.SmtpServer = "EXCHCLUSTER.wth.org";// "your mail server name goes here";
                        //changed the server to WTHEXCH.wth.org 08/16/2006 rgc
                        SmtpMail.SmtpServer = "WTHEXCH.wth.org";// "your mail server name goes here";

                        SmtpMail.Send(mailMsg);
                    }
                    catch(Exception excpt)
                    {
                        m_strErrMsg = excpt.ToString();
                        bRetVal = false;
                    }

                return(bRetVal);
                }
         */
    }
}
