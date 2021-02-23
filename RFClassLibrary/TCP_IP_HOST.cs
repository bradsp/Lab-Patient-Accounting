using System;
using System.Collections.Generic;
using System.Text;
// --- added -------
using System.Net;// IPAddress for listener
using System.Net.Sockets;// for listener
using System.IO;
using System.Threading;
using System.Collections;

namespace RFClassLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class TCP_IP_HOST : RFCObject
    {
        /// <!--TcpClient -->
        public TcpClient m_client;
        static TcpListener m_listener;
        static IPHostEntry m_hostInfo;
        static IPAddress m_hostIP; //("10.12.1.22"); 
        /// <!--Host port -->
        public int m_iHostPort;
        // static
        private Socket m_HostSocket;

        /// <summary>
        /// Constructor - Host a TCP IP session
        /// </summary>
        /// <param name="strHostPort"></param>
        public TCP_IP_HOST(string strHostPort)
        {
            if (strHostPort.Length == 0)
            {
                throw new System.IO.IOException("TCP_IP_HOST started with a blank PORT");
            }

            m_iHostPort = int.Parse(strHostPort);// throws exception so stop the mule train.


            // throw new System.NotImplementedException();
            m_client = new TcpClient();
            m_HostSocket = m_client.Client; //associate socket with client

            m_hostInfo = Dns.GetHostEntry(OS.GetMachineName());
            string strHostIP = m_hostInfo.AddressList[0].ToString();
            m_hostIP = IPAddress.Parse(strHostIP);

            m_listener = new TcpListener(m_hostIP, m_iHostPort);

        }
        /// <summary>
        /// Read upto 64000 bytes of what ever the host put on the stream.
        /// 10/17/2006 Rick Crone
        /// rgc/wdk 20091029 up bytes from 32000 to 64000
        /// </summary>
        /// <param name="str">has the message in it</param>
        /// <returns>Number of bytes read OR -1 error </returns>
        public int HostRead(out string str)
        {
            int iRetVal = 0;

            byte[] b = new byte[64000];
            str = "";
            NetworkStream ns;
            try
            {
                ns = m_client.GetStream();
            }
            catch (InvalidOperationException ioe)
            {
                propErrMsg = ioe.Message;
                m_ERR.m_Logfile.WriteLogFile(ioe.Message); // rgc/wdk 20091029 added
                return (-1); // lost or no connection at this time.
            }

            //  bool bBreak = ns.DataAvailable;

            try
            {
                iRetVal = ns.Read(b, 0, b.Length);
            }
            catch (IOException)
            {
                // bBreak = true;
                iRetVal = -1;
            }

            if (iRetVal > 0)
            {
                str = Encoding.ASCII.GetString(b, 0, iRetVal);
                propErrMsg = string.Format("{0} bytes read", iRetVal);
            }

            return iRetVal;
        }

        /// <summary>
        /// Drop the connection if connected by stopping listener.
        /// </summary>
        public void DisConnectHostListening()
        {
            if (m_HostSocket.Connected)
            {
                {
                    m_listener.Stop();
                }
            }
        }

        /// <summary>
        /// Allow client to connect if not connected.
        /// </summary>
        public void ConnectHostListening()
        {
            m_ERR.m_Logfile.WriteLogFile("Entering ConnectHostListening()");
            if (m_HostSocket.Poll(30000, SelectMode.SelectRead))
            {
                m_ERR.m_Logfile.WriteLogFile("TRUE POLL in connecthostlistening()");
            }
            else
            {
                m_ERR.m_Logfile.WriteLogFile("FALSE POLL in connecthostlistening()");
            }

            if (!m_HostSocket.Connected)
            {
                try
                {
                    m_ERR.m_Logfile.WriteLogFile("Before m_listener.Start()");
                    m_listener.Start();
                }
                catch (SocketException se) // only happens when the maximum number of connections has been queued.
                {
                    m_ERR.m_Logfile.WriteLogFile(string.Format("m_listener local end point {0}", m_listener.LocalEndpoint.ToString()));
                    m_ERR.m_Logfile.WriteLogFile(string.Format("*** ERROR initiating Listening for incoming connection requests in ConnectHostListening(). \r\n {0}", se));
                }
                catch (Exception ex)
                {
                    m_ERR.m_Logfile.WriteLogFile(ex.Message);
                }

            }
            else
            {
                m_ERR.m_Logfile.WriteLogFile("Called ConnectHostListening() while already connected.");
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ClientPending()
        {
            return m_listener.Pending();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AcceptNewClient()
        {
            ArrayList alError = new ArrayList();
            int nCount = 0;
            m_ERR.m_Logfile.WriteLogFile("Entering AcceptNewClient()"); // wdk 20100125 for debugging
            while (true)// wdk 20100125 for debugging
            {
                try
                {
                    m_ERR.m_Logfile.WriteLogFile(string.Format("m_listener: ExclusiveAddressUse {0}\r\nServer {1}\r\nPending {2}\r\nLocalEndpoint {3} ",
                            m_listener.ExclusiveAddressUse,
                                m_listener.Server,
                                    m_listener.Pending(),
                                        m_listener.LocalEndpoint)); // wdk 20100125 for debugging
                    if (m_listener.Pending())// wdk 20100125 for debugging
                    {
                        m_ERR.m_Logfile.WriteLogFile("Pending connection");// wdk 20100125 for debugging
                    }
                    else
                    {
                        if (nCount == 5)
                        {
                            m_ERR.m_Logfile.WriteLogFile("Returning because we could not connect in 2.5 minutes. Let service write to im_alive.");
                            return;
                        }
                        ConnectHostListening();// rgc/wdk 20100126 added
                        if (!m_listener.Pending())
                        {
                            ET.sWait(30000);
                            m_ERR.m_Logfile.WriteLogFile("TCP_IP_HOST waiting for pending for 30 seconds");
                            nCount++;
                            continue;
                        }
                    }

                    m_client = m_listener.AcceptTcpClient();
                    if (m_client.Connected) // wdk 20100125 for debugging to get out of while(true)
                    {
                        break;// wdk 20100125 for debugging
                    }

                }
                // wdk 20100125 added for better tracking MEDITECH_RESULTS_REC is coming here to die every three minutes
                // when data is not available.
                catch (InvalidOperationException ioe)
                {
                    propErrMsg = ioe.Message;
                    m_ERR.m_Logfile.WriteLogFile(string.Format("InvalidOperationException {0}", ioe.Message));
                    if (alError.Contains(ioe.Message))
                    {
                        m_ERR.m_Logfile.WriteLogFile("Break called because the ioe.Message is already in the array.");
                        break;
                    }
                    alError.Add(ioe.Message);

                }
                catch (SocketException se)
                {
                    propErrMsg = se.Message;
                    m_ERR.m_Logfile.WriteLogFile(string.Format("Socket Error Code: {0}\r\nMessage:{1}", se.ErrorCode,
                        se.Message));
                }
                // end of 20100125 catch additions
                catch (Exception e)
                {
                    propErrMsg = e.Message;
                    m_ERR.m_Logfile.WriteLogFile(string.Format("Exception {0}", e.Message)); // rgc/wdk 20091029 added
                }
                m_ERR.m_Logfile.WriteLogFile("end of while(true)"); // wdk 20100125 for debugging
            }
            m_ERR.m_Logfile.WriteLogFile("Leaving AcceptNewClient()"); // wdk 20100125 for debugging
        }


        /// <summary>
        /// Try to send empty string which should have no effect on other end
        /// of this connection. If it fails - there is no connection established.
        /// </summary>
        /// <returns></returns>
        public bool IsSocketConnected()
        {
            bool bRetVal = true;
            Socket mySocket = m_client.Client;
            byte[] msg = Encoding.ASCII.GetBytes("");
            try
            {
                mySocket.Send(msg, 0, msg.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                if (propErrMsg == e.Message)
                {
                    return false;
                }
                propErrMsg = e.Message;
                // wdk 20110103 reformatted for clarity and added brackets around values.
                m_ERR.m_Logfile.WriteLogFile(string.Format("Exception in IsSocketConnected() [{0}]. Socket connected value is [{1}].",
                    e.Message,
                        mySocket.Connected));// rgc/wdk 20091029 added
                ConnectHostListening();
                IsSocketConnected(); // wdk 20110103 recursive call 


            }
            return bRetVal;
        }

        /// <summary>
        /// Send a string message back to the client.
        /// Useful for ACK messages.
        /// 10/18/2006 Rick Crone
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool SendStringFromHost(string strData)
        {
            Socket mySocket = m_client.Client;

            //---- tying to not crash if other side stops 
            if (!mySocket.Connected)
            {
                return (false);
            }
            //---------

            if (strData == null)
            {
                // handle the null strData as if the strData.Length equals 0
                return false;
            }

            if (strData.Length == 0)
            {
                // should not be called with an empty string
                // so do nothing and return
                return false;
            }
            byte[] msg = Encoding.ASCII.GetBytes(strData);
            int i = 0;
            try
            {
                i = mySocket.Send(msg, 0, msg.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                m_ERR.m_Logfile.WriteLogFile(string.Format("*** ERROR on socket send in MeditechSend(string strData): [{0}]\r\nodeSchema", e.Message));
                // Console.WriteLine(string.Format("*** ERROR on socket send in SendString(string strData): [{0}]\r\nodeSchema", e));
                //   sw.WriteLine(string.Format("*** ERROR on socket send in SendString(string strData): [{0}]\r\nodeSchema", e));
                return false;
            }
            if (i > 0)
            {
                //mlf.WriteLogFile(string.Format("Client sent: [{0}]\r\nodeSchema", strData));
                // Console.WriteLine(string.Format("\r\nReply sent: [{0}]\r\nodeSchema", strData));
                // sw.WriteLine(string.Format("\r\nReply sent: [{0}]\r\nodeSchema", strData));
                return true;
            }
            else
            {
                return false;
            }
            //-----------------

        }

        /// <summary>
        /// Checks to see if there is any data to be read from the stream.
        /// </summary>
        /// <returns></returns>
        public bool DataAvailable()
        {
            bool bRetVal = false;
            NetworkStream ns = null;
            try
            {
                ns = m_client.GetStream();
            }
            catch (Exception e)
            {
                // wdk 20110103 added brackets around error message for clarity
                m_ERR.m_Logfile.WriteLogFile(string.Format("Exception in DataAvailable() [{0}]", e.Message));
            }
            if (ns != null)
            {
                bRetVal = ns.DataAvailable;
            }
            return bRetVal;
        }


    }
}
