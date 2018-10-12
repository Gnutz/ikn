/* A simple server in the internet domain using TCP
   The port number is passed as an argument */
#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <cstring>
#include <strings.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include "../lib/iknlib.hpp"

using namespace std;

void sendFile(std::string fileName, long fileSize, int outToClient);


/**
 * main starter serveren og venter på en forbindelse fra en klient
 * Læser filnavn som kommer fra klienten.
 * Undersøger om filens findes på serveren.
 * Sender filstørrelsen tilbage til klienten (0 = Filens findes ikke)
 * Hvis filen findes sendes den nu til klienten
 *
 * HUSK at lukke forbindelsen til klienten og filen nÃ¥r denne er sendt til klienten
 *
 * @throws IOException
 *
 */
int main(int argc, char *argv[])
{

     // Declarations
     int sockfd, newsockfd, portno;
     socklen_t clilen;
     char buffer[BUFSIZE];
     struct sockaddr_in serv_addr, cli_addr;
     int n;
     if (argc < 2) {
         portno = PORT;
     }
     else{
       portno = atoi(argv[1]);
     }

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
}

/**
 * Sender filen som har navnet fileName til klienten
 *
 * @param fileName Filnavn som skal sendes til klienten
 * @param fileSize Størrelsen på filen, 0 hvis den ikke findes
 * @param outToClient Stream som der skrives til socket
     */
void sendFile(std::string fileName, long fileSize, int outToClient)
{
    // TO DO Your own code
}
