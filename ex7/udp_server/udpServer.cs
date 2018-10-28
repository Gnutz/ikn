using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class UDPServer
{

  private int PORT;
  private IPAddress localAddress;
  private UdpClient udpListener;
  private IPEndPoint udpClient;

    private UDPServer(string[] args)
    {
      if(args.Length > 0)
				PORT = int.Parse(args[0]);
      else
      {
          PORT = 9000;
      }
			if(args.Length > 1)
        localAddress = IPAddress.Parse(args[1]);
      else
        localAddress = IPAddress.Parse("10.0.0.1");

        bool done = false;

        udpListener = new UdpClient(PORT);
        udpClient = new IPEndPoint(IPAddress.Any,PORT);
        byte[] bytes;


        try
        {
            while (!done)
            {
                String receivedCommand = receiveClientCommand();

              /*  Console.WriteLine("Received broadcast from {0} :{1}",
                    udpClient.ToString(),
                    Encoding.ASCII.GetString(bytes,0,bytes.Length)); */

                switch(receivedCommand.ToUpper())
                {

                  case "U":
                    sendFileContentUDP("/proc/uptime");
                    break;
                  case "L":
                    sendFileContentUDP("/proc/loadavg");
                    break;
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.ToString());
        }
        finally
        {
            udpListener.Close();
        }
    }

    String receiveClientCommand()
    {
      byte[] bytes = udpListener.Receive( ref udpClient);

      return Encoding.ASCII.GetString(bytes);

    }

    void sendFileContentUDP(string file)
    {
      FileStream fs = new FileStream(file, FileMode.Open);
      StreamReader s = new StreamReader(fs, Encoding.Default);
      string line =  s.ReadLine();
      Console.WriteLine(line);
      s.Close();
      fs.Close();


     byte[] sendbuf = Encoding.ASCII.GetBytes(line);
        //IPEndPoint ep = new IPEndPoint(, 11000);

        udpListener.Send(sendbuf, sendbuf.Length, udpClient);
    }

    public static void Main(string[] args)
    {
        new UDPServer(args);

    }


}
