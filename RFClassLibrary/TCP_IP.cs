/*
 * 10/11/2006 Rick Crone
 *  This TCP_IP class has basic TCP IP communications code.
 *  This class was developed for use in our
 *  Hardin County / Health Managment Systems 
 *  order / results interface.
 * 
 *  There are difference in a HOST listening for a connection and a client
 *  making a connection. Using the DOT NET classes has made the code for 
 *  reading and sending diffenent at this time (10/17/2006). rgc
 *  For now (10/17/2006) this class support the client side ONLY. rgc
 * 
 *  This class is designed so that it should be usable as a base class
 *  for the Meditech class allowing us one point of maintance for this code.
 *  Due to extream dead line limitations making the mods to the Meditech
 *  class at this time is impossible.
 * 
 * notes:
 *  Much of this code is lifted directly from the Meditech class.
 *  
 */
using System;
using System.Collections.Generic;
using System.Text;
// --- added -------
using System.Net;// IPAddress
using System.Net.Sockets;// mySocket

namespace RFClassLibrary
{

    /// <summary>
    ///*  This TCP_IP class has basic TCP IP communications code.
    ///*  This class was developed for use in our
    ///*  Hardin County / Health Managment Systems 
    ///*  order / results interface.
    ///* 
    ///*  There are difference in a HOST listening for a connection and a client
    ///*  making a connection. Using the DOT NET classes has made the code for 
    ///*  reading and sending diffenent at this time (10/17/2006). rgc
    ///*  For now (10/17/2006) this class support the client side ONLY. rgc
    ///* 
    ///*  This class is designed so that it should be usable as a base class
    ///*  for the Meditech class allowing us one point of maintance for this code.
    ///*  Due to extream dead line limitations making the mods to the Meditech
    ///*  class at this time is impossible.
    ///* 
    ///* notes:
    ///*  Much of this code is lifted directly from the Meditech class.
    ///*
    /// 10/11/2006 Rick Crone
    /// </summary>
    public class TCP_IP : RFCObject
    {
        /// <!-- client ip address -->
        public static string m_strClientIP;
        /// <!-- client port -->
        public static Int32 m_iClientPort;
        /// <!-- our TCP/IP connection we will read from and write to thru this Socket-->
        public static Socket m_ClientSocket;
        /// <!-- Client Result message string -->
        private static string m_strClientResult = "";

        /// <summary>
        /// So we can get the Client Socket.
        /// 10/17/2006 Rick Crone
        /// 
        /// </summary>
        public Socket propClientSocket
        {
            get
            {
                //throw new System.NotImplementedException();
                return m_ClientSocket;
            }
            set
            {
            }
        }

        /// <summary>
        /// Make a tcp/ip connection for a client application.
        /// Client's expect host to be listening for the connection attempt.
        /// 
        /// Pass TCP address as strIP and port number as iPort.
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <returns>true if connected - see m_strErrMsg</returns>
        public bool ClientConnect(string strIP, Int32 iPort)
        {
            //set these for possible re - connect in read
            m_strClientIP = strIP;
            m_iClientPort = iPort;


            m_strErrMsg = string.Format("SUCCESS! Connected to IP {0} on Port {1}.",
                                    strIP,
                                     iPort);
            //----------------
            //mlf.WriteLogFile("Entering ClientConnect())");

            m_ClientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            //10.32.1.30
            //IPAddress myIP = IPAddress.Parse("10.98.1.55"); // address HMS server
            IPAddress myIP = IPAddress.Parse(strIP);
            Console.WriteLine(myIP.ToString());
            // HMS port 10001 ORDERS - our server MCL03 10.12.1.22
            // HMS port 11001 RESULTS - there server in Nashville 10.98.1.55
            //IPEndPoint myIPEndPoint = new IPEndPoint(myIP, 11001); //23 is normal telnet port
            IPEndPoint myIPEndPoint = new IPEndPoint(myIP, iPort); //23 is normal telnet port
            if (!m_ClientSocket.Connected)
            {
                try
                {
                    //  mlf.WriteLogFile("Not connected in ConnectMeditech() - so try to connect");
                    m_ClientSocket.Connect(myIPEndPoint);
                }
                catch (SocketException e)
                {
                    m_strErrMsg = string.Format("FAILED  Connection to IP {0} on Port {1}. {2}",
                                    strIP,
                                     iPort,
                                      e.Message);

                    //string x = e.Message;
                    //Console.WriteLine(x);
                    //Console.ReadLine();// just to hold me on the screen until key press

                }
            }
            else
            {
                //mlf.WriteLogFile("Already connected in ConnectMeditech()");
                //Console.WriteLine("Already connected");
                m_strErrMsg = string.Format("Already connected to IP {0} on Port {1}",
                  strIP,
                   iPort);

                //Console.ReadLine();// just to hold me on the screen until key press

            }

            if (!m_ClientSocket.Connected)
            {
                //some error occurred in connection
                //System.Windows.Forms.MessageBox.Show("Error opening socket");
                //mlf.WriteLogFile("In ConnectMeditech() FAILED opening port");
                //m_strErrMsg = string.Format("FAILED  Connection to IP{0} on Port {1}.",
                //                   strIP,
                //                    iPort);


                return false;
            }
            return true;
        }


        /// <summary>
        /// 03/27/2003 Rick Crone - send CR LF
        /// 
        /// </summary>
        public bool SendCRFromClient()
        {
            byte[] msg = new byte[3];
            msg[0] = 13;
            msg[1] = 10;
            try
            {
                int i = m_ClientSocket.Send(msg, 0, 2, SocketFlags.None);
            }
            catch (Exception)
            {
                //mlf.WriteLogFile(string.Format("*** Client sent: [CR LF]\r\nodeSchema and we received the [{0}] as an error", e.ToString()));
                return false;
            }
            //mlf.WriteLogFile("Client sent: [CR LF]\r\nodeSchema");
            return true;
        }

        /// <summary>
        /// Attempt to send an empty string which should have no effect on the
        /// other end of the connection. If it fails we are NOT connected.
        /// </summary>
        /// <returns></returns>
        public bool IsClientConnected()
        {
            // wdk 20100929 
            return m_ClientSocket.Connected;

            //byte[] msg = Encoding.ASCII.GetBytes("");
            //int i = 0;
            //try
            //{
            //    i = m_ClientSocket.Send(msg, 0, msg.Length, SocketFlags.None);
            //}
            //catch (Exception e)
            //{
            //    //mlf.WriteLogFile(string.Format("*** ERROR on socket send in MeditechSend(string strData): [{0}]\r\nodeSchema", e));
            //    m_strErrMsg = string.Format("*** ERROR on socket send in IsClientConnected (): [{0}]\r\n", e);
            //    return false;
            //}
            //if (i > 0)
            //{
            //    //mlf.WriteLogFile(string.Format("Client sent: [{0}]\r\nodeSchema", strData));
            //    m_strErrMsg = string.Format("Client is connected");
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}


        }

        /// <summary>
        /// Send the string then sends the CR LF
        /// 10/17/2006 Rick Crone
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool SendLineFromClient(string strData)
        {
            bool bRetVal = SendStringFromClient(strData);
            SendCRFromClient();
            return (bRetVal);

        }

        /// <summary>
        /// 
        /// 10/17/2006 Rick Crone
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public bool SendStringFromClient(string strData)
        {
            //<change>------------------
            /*
             * wdk 02/27/04
             * reinstated this line of code becuse something is sending a null
             * string. FivePointOh is not sending a MeditechSend but MeditechSendLine
             */
            if (strData == null)
            {
                // handle the null strData as if the strData.Length equals 0
                return false;
            }
            // end of wdk 02/27/04 modification
            // </change>

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
                i = m_ClientSocket.Send(msg, 0, msg.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                //mlf.WriteLogFile(string.Format("*** ERROR on socket send in MeditechSend(string strData): [{0}]\r\nodeSchema", e));
                m_strErrMsg = string.Format("*** ERROR on socket send in MeditechSend(string strData): [{0}]\r\n", e);
                return false;
            }
            if (i > 0)
            {
                //mlf.WriteLogFile(string.Format("Client sent: [{0}]\r\nodeSchema", strData));
                m_strErrMsg = string.Format("Client sent: [{0}]\r\n", strData);
                return true;
            }
            else
            {
                return false;
            }
            //-----------------

        }

        // <summary>
        // no longer used!
        // Listen for a connection.
        // 10/17/2006 Rick Crone
        // </summary>
        // <param name="strIP">address ex:10.12.1.22 or MCL03.wth.org</param>
        // <param name="iPort">port number - 23 is standard telnet</param>
        // <param name="iSeconds">time out - or less than 1 to try forever</param>
        // <returns></returns>
        //public bool OldListen(string strIP, Int32 iPort, int iSeconds)
        //{
        //    throw new System.NotImplementedException();// no longer used
        //   < m_strErrMsg = string.Format("SUCCESS! Connected to IP {0} on Port {1}.",
        //                 strIP,
        //                  iPort);
        //    //----------------
        //    //mlf.WriteLogFile("Entering ConnectMeditech()");

        //    m_ClientSocket = new Socket(AddressFamily.InterNetwork,
        //        SocketType.Stream,
        //        ProtocolType.Tcp);
        //    //10.32.1.30
        //    //IPAddress myIP = IPAddress.Parse("10.98.1.55"); // address HMS server
        //    IPAddress myIP = IPAddress.Parse(strIP);
        //    Console.WriteLine(myIP.ToString());
        //    // HMS port 10001 ORDERS - our server MCL03 10.12.1.22
        //    // HMS port 11001 RESULTS - there server in Nashville 10.98.1.55
        //    //IPEndPoint myIPEndPoint = new IPEndPoint(myIP, 11001); //23 is normal telnet port
        //    IPEndPoint myIPEndPoint = new IPEndPoint(myIP, iPort); //23 is normal telnet port
        //    if (!m_ClientSocket.IsBound)
        //    {
        //        m_ClientSocket.Bind(myIPEndPoint);
        //    }
        //    if (!m_ClientSocket.Connected)
        //    {
        //        try
        //        {
        //            //  mlf.WriteLogFile("Not connected in ConnectMeditech() - so try to connect");
        //            m_ClientSocket.Listen(1);// .Connect(myIPEndPoint);
        //            //mySocket.BeginAccept(
        //        }
        //        catch (SocketException e)
        //        {
        //            m_strErrMsg = string.Format("FAILED  Connection to IP {0} on Port {1}. {2}",
        //                            strIP,
        //                             iPort,
        //                              e.Message);

        //            //string x = e.Message;
        //            //Console.WriteLine(x);
        //            //Console.ReadLine();// just to hold me on the screen until key press

        //        }
        //    }
        //    else
        //    {
        //        //mlf.WriteLogFile("Already connected in ConnectMeditech()");
        //        //Console.WriteLine("Already connected");
        //        m_strErrMsg = string.Format("Already connected to IP {0} on Port {1}",
        //          strIP,
        //           iPort);

        //        //Console.ReadLine();// just to hold me on the screen until key press

        //    }

        //    if (!m_ClientSocket.Connected)
        //    {
        //        //some error occurred in connection
        //        //System.Windows.Forms.MessageBox.Show("Error opening socket");
        //        //mlf.WriteLogFile("In ConnectMeditech() FAILED opening port");
        //        //m_strErrMsg = string.Format("FAILED  Connection to IP{0} on Port {1}.",
        //        //                   strIP,
        //        //                    iPort);


        //        return false;
        //    }
        //    return true;


        //}

        /// <summary>
        /// Read from the TCP IP port - if any data available.
        /// 10/17/2006 Rick Crone
        /// </summary>
        public string ClientRead()
        {
            //throw new System.NotImplementedException();
            //---------------
            if (!m_ClientSocket.Connected)
            {
                ClientConnect(m_strClientIP, m_iClientPort);
            }
            //mlf.WriteLogFile("Entering ReadMeditech() - will return if nothing to read");
            if (m_ClientSocket.Available < 1)
            {
                // don't try to read with no data pending!
                // we will hang here if we do
                return ("");
            }

            //mlf.WriteLogFile("ReadMeditech() - something to read");

            byte[] bytes = new byte[1];
            int bytesrecv = 0;
            m_strClientResult = "";

            ET ReadMaxTime = new ET(10);// 10 second to read message or we time out

            while (m_ClientSocket.Available > 0)
            {

                if (ReadMaxTime.IsExpired())
                {
                    //mlf.WriteLogFile("Timed out in Read()");
                    //Environment.Exit(13);

                    return (m_strClientResult); // return what we read so far

                }

                bytesrecv = m_ClientSocket.Receive(bytes, 0, 1, SocketFlags.None);
                //if(bytes[0] > 31 && bytes[0] < 128)
                {
                    m_strClientResult += Encoding.ASCII.GetString(bytes);
                }
                //else
                {
                    // rgc 03/03/04 added bell char for any message
                    //if(rc < 6 || bytes[0] == 7)
                    //{
                    //    myResult += string.Format("[{0:D}]",bytes[0]);
                    //}
                }

                // sometimes sender is a little slow with it's message
                // this keeps the message from being broken up
                if (m_ClientSocket.Available < 1)
                {
                    // wait one second
                    //WriteLogFile("wait in MeditechRead() loop");
                    Time.Wait(1000);
                }

                //WriteLogFile("Read() - just after wait for complete message");

            }// end of while more bytes to read
             //rc++;
             //string msg;
             //msg = string.Format("{0:D} Read:[{1}]\r\nodeSchema",
             //    rc,
             //    myResult);
             //mlf.WriteLogFile(msg);

            return m_strClientResult;

            //=================================

        }
    }
}
