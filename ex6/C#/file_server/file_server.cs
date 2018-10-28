using System;
using System.IO;
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


      	String data = null;

      	// Enter the listening loop.

	      	        System.Console.Write("Waiting for a connection... ");

	        // Perform a blocking call to accept requests.
	        // You could also user server.AcceptSocket() here.
	        TcpClient client = server.AcceptTcpClient();
	        System.Console.WriteLine("Connected!");

	        data = null;

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



					/*

	        // Loop to receive all the data sent by the client.
	        while((i = stream.Read(bytes, 0, bytes.Length))!=0)
	        {
	          // Translate data bytes to a ASCII string.
	          data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
	          Console.WriteLine("Received: {0}", data);

	          // Process the data sent by the client.
	          data = data.ToUpper();

	          byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

	          // Send back a response.
	          stream.Write(msg, 0, msg.Length);
	          Console.WriteLine("Sent: {0}", data);
	        }

	        // Shutdown and end connection
	        client.Close();
	      }
	    }
	    catch(SocketException e)
	    {
	      Console.WriteLine("SocketException: {0}", e);
	    }
	    finally
	    {
	       // Stop listening for new clients.
	       server.Stop();
	    }


	    Console.WriteLine("\nHit enter to continue...");
	    Console.Read();
				}
			} */

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
			FileStream fileStream = new FileStream(
      fileName, FileMode.OpenOrCreate,
      FileAccess.ReadWrite, FileShare.None);

			int offset = 0;

			try
			{
			StreamReader sr = new StreamReader(fileName);
			string toSend;

			while(offset != fileSize){
				offset += sr.Read(bytes, offset, BUFSIZE);
				toSend = new string(bytes);
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

/*


			     //main starter serveren og venter på en forbindelse fra en klient
			     sockfd = socket(AF_INET, SOCK_STREAM, 0);
			     if (sockfd < 0)
			        error("ERROR opening socket");
			     bzero((char *) &serv_addr, sizeof(serv_addr));

			     serv_addr.sin_family = AF_INET;
			     serv_addr.sin_addr.s_addr = INADDR_ANY;
			     serv_addr.sin_port = htons(portno);
			     if (bind(sockfd, (struct sockaddr *) &serv_addr,
			              sizeof(serv_addr)) < 0)
			              error("ERROR on binding");
			     listen(sockfd,5);
			     clilen = sizeof(cli_addr);
			     newsockfd = accept(sockfd,
			                 (struct sockaddr *) &cli_addr,
			                 &clilen);
			     if (newsockfd < 0)
			          error("ERROR on accept");

			    //printf("client connected \n");

			     // Læser filnavn som kommer fra klienten.
			     printf("client connected \n");
			      bzero(buffer,BUFSIZE);
			      printf("this is in the buffer:%s", buffer);
			     readTextTCP(buffer, BUFSIZE, newsockfd);
			     printf("this is in the buffer:%s", buffer);
			     //if (n < 0) error("ERROR reading from socket");
			     printf("Here is the file name: %s",buffer);
			     writeTextTCP(newsockfd, "File name recieved");

			     // * Undersøger om filens findes på serveren.
			     char file[256];
			     snprintf(file, strlen(buffer), buffer);
			    long fileSize = check_File_Exists(&buffer);

			    //if (fileSize == 0) error("ERROR opening file");
			     printf("size: %ld bytes\n", fileSize);

			     //Sender filstørrelsen tilbage til klienten (0 = Filens findes ikke)
			     //Hvis filen findes sendes den nu til klienten
			      sendFile(buffer, fileSize, newsockfd);
			     //Luk forbindelsen til klienten
			     close(newsockfd);
			     close(sockfd);
			     return 0;

					 TcpListener server=null;
    try
    {
      // Set the TcpListener on port 13000.

			if(args.Count)
      Int32 port = 13000;
      IPAddress localAddr = IPAddress.Parse("127.0.0.1");

      // TcpListener server = new TcpListener(port);
      server = new TcpListener(localAddr, port);

      // Start listening for client requests.
      server.Start();

      // Buffer for reading data
      Byte[] bytes = new Byte[256];
      String data = null;

      // Enter the listening loop.
      while(true)
      {
        Console.Write("Waiting for a connection... ");

        // Perform a blocking call to accept requests.
        // You could also user server.AcceptSocket() here.
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!");

        data = null;

        // Get a stream object for reading and writing
        NetworkStream stream = client.GetStream();

        int i;

        // Loop to receive all the data sent by the client.
        while((i = stream.Read(bytes, 0, bytes.Length))!=0)
        {
          // Translate data bytes to a ASCII string.
          data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
          Console.WriteLine("Received: {0}", data);

          // Process the data sent by the client.
          data = data.ToUpper();

          byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

          // Send back a response.
          stream.Write(msg, 0, msg.Length);
          Console.WriteLine("Sent: {0}", data);
        }{}

				TcpListener server=null;
	    try
	    {
	      // Set the TcpListener on port 13000.
	      Int32 port = 13000;
	      IPAddress localAddr = IPAddress.Parse("127.0.0.1");

	      // TcpListener server = new TcpListener(port);
	      server = new TcpListener(localAddr, port);

	      // Start listening for client requests.
	      server.Start();

	      // Buffer for reading data
	      Byte[] bytes = new Byte[256];
	      String data = null;

	      // Enter the listening loop.
	      while(true)
	      {
	        Console.Write("Waiting for a connection... ");

	        // Perform a blocking call to accept requests.
	        // You could also user server.AcceptSocket() here.
	        TcpClient client = server.AcceptTcpClient();
	        Console.WriteLine("Connected!");

	        data = null;

	        // Get a stream object for reading and writing
	        NetworkStream stream = client.GetStream();

	        int i;

	        // Loop to receive all the data sent by the client.
	        while((i = stream.Read(bytes, 0, bytes.Length))!=0)
	        {
	          // Translate data bytes to a ASCII string.
	          data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
	          Console.WriteLine("Received: {0}", data);

	          // Process the data sent by the client.
	          data = data.ToUpper();

	          byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

	          // Send back a response.
	          stream.Write(msg, 0, msg.Length);
	          Console.WriteLine("Sent: {0}", data);
	        }

	        // Shutdown and end connection
	        client.Close();
	      }
	    }
	    catch(SocketException e)
	    {
	      Console.WriteLine("SocketException: {0}", e);
	    }
	    finally
	    {
	       // Stop listening for new clients.
	       server.Stop();
	    }


	    Console.WriteLine("\nHit enter to continue...");
	    Console.Read();
			*/

		}
	}
}
