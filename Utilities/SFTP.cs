using System;
using System.IO;
using System.Net.Sockets;
using Renci.SshNet.Common;
using Renci.SshNet;
using System.Collections.Generic;

namespace Utilities
{

    public static class SFTP
    {

        public static void UploadSftp(string filename, string destinationFilename, string server, int port, string username, string password)
        {
            if (!File.Exists(filename))
                return;

            var methods = new List<AuthenticationMethod>
            {
                new PasswordAuthenticationMethod(username, password),
            };

            var con = new ConnectionInfo(server, port, username, methods.ToArray());

            using SftpClient client = new(con);
            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.UploadFile(File.OpenRead(filename), destinationFilename);
                    client.Disconnect();
                }
            }
            catch (Exception e) when (e is SshConnectionException || e is SocketException || e is ProxyException)
            {
                Console.WriteLine($"Error connecting to server: {e.Message}");
                Console.ReadKey();
            }
            catch (SshAuthenticationException e)
            {
                Console.WriteLine($"Failed to authenticate: {e.Message}");
                Console.ReadKey();
            }
            catch (SftpPermissionDeniedException e)
            {
                Console.WriteLine($"Operation denied by the server: {e.Message}");
                Console.ReadKey();
            }
            catch (SshException e)
            {
                Console.WriteLine($"Sftp Error: {e.Message}");
                Console.ReadKey();
            }
        }
    }
}
