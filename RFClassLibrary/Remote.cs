/*
 * Rick Crone 01/25/2007
 * This was developed for use with our in house developed services and
 * the monitor application that controls them.
 * 
 * This class was to hold the code for RPC Remote Process Calls.
 * At this time it seems best that most of that type of code be in 
 * the applications. The only function that is being used in this 
 * file 'Remote.cs' is RemoteServicePorts. This allow both the server
 * and the client access to the port information.
 * 
 * Note: The Remobject code (that is commented out) is a good example
 * of 'server' code that we put in the services.
 */

namespace Utilities
{
    /// <summary>
    /// * Rick Crone 01/25/2007
    /// * This was developed for use with our in house developed services and
    /// * the monitor application that controls them.
    /// * 
    /// * This class was to hold the code for RPC Remote Process Calls.
    /// * At this time it seems best that most of that type of code be in 
    /// * the applications. The only function that is being used in this 
    /// * file 'Remote.cs' is RemoteServicePorts. This allow both the server
    /// * and the client access to the port information.
    /// * 
    /// * Note: The Remobject code (that is commented out) is a good example
    /// * of 'server' code that we put in the services.
    /// </summary>
    public class RemoteServicePorts
    {
        /// <summary>
        /// Gets a port number for a service to use for TCP IP communications.
        /// </summary>
        /// <param name="strService">service you want the port number for</param>
        /// <returns>port number</returns>
        public static int GetPort(string strService)
        {
            int iRetVal = -1;
            string str = string.Format("{0}^{1}|", "HMS_REC", 9090);
            str += string.Format("{0}^{1}|", "ORD_PROC", 9091);
            str += string.Format("{0}^{1}|", "MED_REC", 9092);
            str += string.Format("{0}^{1}|", "RES_PARSE", 9093);
            str += string.Format("{0}^{1}|", "HMS_SEND", 9094);
            str += string.Format("{0}^{1}|", "HIS_SEND", 9095);
            str += string.Format("{0}^{1}|", "FordReg", 9096);
            str += string.Format("{0}^{1}|", "FordReq", 9097);
            str += string.Format("{0}^{1}|", "GetAcc", 9098);
            str += string.Format("{0}^{1}|", "QpmReg", 9099);
            str += string.Format("{0}^{1}|", "QpmReq", 9100);
            str += string.Format("{0}^{1}|", "RES_POST", 9101); // 08/19/2008 wdk added
            str += string.Format("{0}^{1}|", "FordOrders", 9102); // rgc/wdk 201005050 added
            str += string.Format("{0}^{1}|", "HIS_HL7_REC", 9103); // rgc/wdk 20111006 added

            string[] strArr = str.Split(new char[] { '|' });
            foreach (string s in strArr)
            {
                string[] strEach = s.Split(new char[] { '^' });
                if (strEach[0] == strService)
                {
                    iRetVal = int.Parse(strEach[1]);
                    break;
                }
            }
            return iRetVal;
        }

    }

    //// Remote object.
    //public class RemoteObject : MarshalByRefObject
    //{

    //    static string m_strRem = "RemoteObject.rem";
    //    static bool m_bLive = false;

    //    private int callCount = 0;

    //    public bool prop_b_Live
    //    {
    //        get
    //        {
    //            //throw new System.NotImplementedException();
    //            return (m_bLive);
    //        }
    //        set
    //        {
    //            m_bLive = value;
    //        }
    //    }


    //    public int GetCount()
    //    {
    //        callCount++;
    //        return (callCount);
    //    }
    //    public string GetHost()
    //    {
    //        RFCObject m_RFCObject = new RFCObject();
    //        return (m_RFCObject.propAppName);
    //    }
    //    //--------

    //    //[SecurityPermission(SecurityAction.Demand)]
    //    public void Listen(int iPort/*string[] args*/)
    //    {
    //        // Create the server channel.
    //        TcpChannel serverChannel = new TcpChannel(iPort);

    //        // Register the server channel.
    //        ChannelServices.RegisterChannel(serverChannel, false);

    //        // Show the URIs associated with the channel.
    //        //ChannelDataStore data = (ChannelDataStore)serverChannel.ChannelData;
    //        //foreach (string uri in data.ChannelUris)
    //        //{
    //        //    m_LogFile.WriteLogFile(string.Format("The channel URI is {0}.", uri));
    //        //}

    //        // Expose an object for remote calls.
    //        //RemotingConfiguration.RegisterWellKnownServiceType(
    //        //    typeof(RemoteObject), "RemoteObject.rem",
    //        //   WellKnownObjectMode.Singleton);

    //        RemotingConfiguration.RegisterWellKnownServiceType(
    //            typeof(RemoteObject), m_strRem,
    //           WellKnownObjectMode.Singleton);

    //        // Parse the channel's URI.
    //        //string[] urls = serverChannel.GetUrlsForUri(m_strRem);

    //        //if (urls.Length > 0)
    //        //{
    //        //    string objectUrl = urls[0];
    //        //    string objectUri;
    //        //    string channelUri = serverChannel.Parse(objectUrl, out objectUri);
    //        //    m_LogFile.WriteLogFile(string.Format("The object URL is {0}.", objectUrl));
    //        //    m_LogFile.WriteLogFile(string.Format("The object URI is {0}.", objectUri));
    //        //    m_LogFile.WriteLogFile(string.Format("The channel URI is {0}.", channelUri));
    //        //}

    //        //--------
    //    }
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //public class RemoteClient :RFCObject
    //{
    //    TcpChannel clientChannel;
    //    WellKnownClientTypeEntry remoteType;

    //    public RemoteClient(string strRem)
    //    {
    //        //throw new System.NotImplementedException();
    //        Init(strRem);
    //    }

    //    ~RemoteClient()
    //    {
    //       // throw new System.NotImplementedException();
    //        try
    //        {
    //            ChannelServices.UnregisterChannel(clientChannel);
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    /// <summary>
    //    /// Querries the Server.
    //    /// 01/11/2007 Rick Crone
    //    /// </summary>
    //    /// <param name="strRem"></param>
    //    /// <example> strRem = @"tcp://localhost:9090/RemoteObject.rem";
    //    ///           strRem = @"tcp://LABLIS5:9090/RemoteObject.rem";
    //    /// </example>
    //    private void Init(string strRem)
    //    {
    //        // Create the channel.

    //        clientChannel = new TcpChannel();
    //        // Register the channel.

    //        try
    //        {
    //            ChannelServices.RegisterChannel(clientChannel, false);

    //        }
    //        catch
    //        {

    //        }

    //        // Register as client for remote object.

    //        remoteType = new WellKnownClientTypeEntry(
    //            typeof(RemoteObject), strRem);
    //        WellKnownClientTypeEntry[] wk = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
    //        bool bFound = false;
    //        int ub = wk.GetUpperBound(0);
    //        for (int i = 0; i <= ub; i++)
    //        {
    //            if (remoteType.ToString() == wk[i].ToString())
    //            {
    //                bFound = true;
    //            }
    //            if (remoteType == wk[i]) 
    //            {
    //                bFound = true;
    //                break;
    //            }
    //        }

    //        if (!bFound)
    //        {
    //           RemotingConfiguration.RegisterWellKnownClientType(remoteType);

    //        }

    //        // Create a message sink.
    //        string objectUri;
    //        System.Runtime.Remoting.Messaging.IMessageSink messageSink =
    //            clientChannel.CreateMessageSink(
    //                strRem, null,
    //                out objectUri);
    //        //Console.WriteLine("The URI of the message sink is {0}.",
    //        //    objectUri);
    //        if (messageSink != null)
    //        {
    //          //  Console.WriteLine("The type of the message sink is {0}.",
    //            //    messageSink.GetType().ToString());
    //        }

    //        return;
    //    }

    //    public bool IsLive()
    //    {
    //        // Create an instance of the remote object.
    //        RemoteObject service = new RemoteObject();


    //        // Invoke a method on the remote object.
    //        //Console.WriteLine("The client is invoking the remote object.");
    //        //try
    //        //{

    //        //    Console.WriteLine("The remote object has been called: returned:\r\n {0}.",
    //        //         service.GetHost());//.GetCount());
    //        //}
    //        //catch (Exception e)
    //        //{
    //        //    // server may not be runing
    //        //    Console.WriteLine("REMOTE METHOD CALL FAILED - IS SERVER RUNNING?\r\n{0}", e);

    //        //}
    //        return service.prop_b_Live;
    //    }
    //    public string GetHost()
    //    {
    //        RemoteObject service = new RemoteObject();
    //        return(service.GetHost());

    //    }

    //}
    ////class Remote
    //{
    //}
    //class RemoteTCPServer
    //{

    //}

    //public class RemoteObject : RemotingException, ISerializable
    //{

    //public RFCObject m_RFCObject;
    //private int callCount = 0;

    //public RemoteObject()
    //    {
    //        //throw new System.NotImplementedException();
    //    m_RFCObject = new RFCObject();


    //    }

    //    public int GetCount()
    //    {
    //        callCount++;
    //        return (callCount);
    //    }

    //    public string GetHost()
    //    {
    //        return (m_RFCObject.propAppName);
    //    }
    //}
}
