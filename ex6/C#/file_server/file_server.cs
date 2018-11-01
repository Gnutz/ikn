using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using tcp;

namespace tcp
{
	public class file_server
	{

		/// The PORT
		int PORT = 9000;
			/// The BUFSIZE
		const int BUFSIZE = 1000;
		Char[] bytes = new Char[BUFSIZE];
		IPAddress localAddress = IPAddress.Parse("10.0.0.1");


		private file_server (string[] args)
		{
			if(args.Length > 0)
				PORT = int.Parse(args[0]);

			if(args.Length > 1)
				localAddress = IPAddress.Parse(args[1]);

			TcpListener server = null;

			try
		  {
		  	server = new TcpListener(localAddress, PORT);
		    // Start listening for client requests.
		    server.Start();

      	// Enter the listening loop.

	      	        System.Console.Write("Waiting for a connection... ");

	        // Perform a blocking call to accept requests.
	        // You could also user server.AcceptSocket() here.
	        TcpClient client = server.AcceptTcpClient();
	        System.Console.WriteLine("Connected!");

	        
	        // Get a stream object for reading and writing
	        NetworkStream SocketStream = client.GetStream();

					string fileName = tcp.LIB.readTextTCP(SocketStream);

					System.Console.WriteLine(fileName);

					long fileSize = tcp.LIB.check_File_Exists(fileName);

					tcp.LIB.writeTextTCP(SocketStream, $"{fileSize}");

					if(fileSize != 0)
					{
						Console.WriteLine($"Sending file..");
						sendFile(fileName, fileSize, SocketStream);
					}
					else
					{
						Console.WriteLine($"File did not exist on sever. Closing connection..");
						SocketStream.Close();
						client.Close();
					}

				}
				catch(SocketException e){
					Console.WriteLine("SocketException: {0}", e);
				}
				finally{
					// Stop listening for new clients.
 	       server.Stop();
				}

			}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{

			 //our code
			FileStream fs = new FileStream(fileName, FileMode.Open , FileAccess.Read);
      StreamReader sr = new StreamReader(fs, Encoding.Default);

			int offset =	0;

			try
			{

			while(offset < fileSize){
				offset += sr.Read(bytes, offset, BUFSIZE);
				String toSend = new String(bytes);
				tcp.LIB.writeTextTCP(io, toSend);
			}
			tcp.LIB.writeTextTCP(io, "\0");

			}
			catch (Exception e)
        {
            // Let the user know what went wrong.
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
				}

				sr.Close();
				fs.Close();


		     /* var totalAmountSend = 0;

		    var bytes = new byte[BUFSIZE];
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

		    while (totalAmountSend < (int)fileSize)
		    {
		        var bytesRead = fs.Read(bytes, 0, BUFSIZE);

		        io.Write(bytes, 0, bytesRead);
		        totalAmountSend += bytesRead;

		        Console.WriteLine(bytesRead);
		    }

            fs.Close(); */

		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
 		/// </summary>
		public static void Main (string[] args)
		{

		Console.WriteLine ("Server starts...");
		new file_server(args);

		}
	}
}
