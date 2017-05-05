using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Renci.SshNet;

namespace sshserver
{
    class Server
    {
        /* initialization ****/
        TcpListener server = null;
        Byte[] inbuffer = new Byte[2000];
        Byte[] outbuffer = new Byte[2000];
        private static  SshClient clientssh;
        int port = 13214;
        IPAddress localaddrr;
        StreamReader sr;
        StreamWriter sw;
        public int connection(String ip,String username,String password)
        {
            int connected = 1;

            /*** Using SSh.net to connect a remote server****/
            
                try
                {
                    clientssh = new SshClient(ip, username, password);

                    clientssh.Connect();
                    if (clientssh.IsConnected)
                    {
                        connected = 0;
                    }

                }
                catch (SystemException e)
                {
                    e.ToString();
                }

               
                

              

                return connected;

        }
        public void lancer()
        {
            try
            {
                localaddrr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localaddrr, port);
                server.Start();
                while (true)
                {
                    Console.WriteLine("waiting for connexion");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("connected");
                    /*** opening the stream for sending and receiving data **/
                    String str = "";
                    NetworkStream stream = client.GetStream();
                    sr = new StreamReader(stream);
                    sw = new StreamWriter(stream);
                    String address,user,mdp;
                    address = null;
                    user = null; mdp = null;

                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                        {
                            address = sr.ReadLine();
                        }
                        else if (i == 1)
                        {
                            user = sr.ReadLine();
                        }
                        else
                        {
                            mdp = sr.ReadLine();
                        }

                    }
                    if (connection(address, user, mdp) == 0)
                    {
                        sw.WriteLine("Welcome your are now Connected ");
                        sw.Flush();




                        do
                        {




                            str = sr.ReadLine();
                            /*** The clientssh is static so it use here to create a ssh Command***/
                            SshCommand cmd = clientssh.CreateCommand(str.ToString());
                            Console.WriteLine(str.ToString());
                            cmd.Execute();


                            if (str.Equals("end", StringComparison.OrdinalIgnoreCase))
                            {
                                str = "End transmission";

                            }
                            else
                            {
                                sw.WriteLine(">" + cmd.Result);
                                sw.Flush();
                            }

                            


                        } while (!str.Equals("End transmission", StringComparison.OrdinalIgnoreCase));
                        clientssh.Disconnect();
                        client.Close();
                    }
                    else
                    {
                        sw.WriteLine("Connexion failure ");
                        sw.Flush();

                    }




                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());

            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                server.Stop();
            }
        }

    }
}
