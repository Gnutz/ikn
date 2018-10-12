//============================================================================
// Name        : file_client.cpp
// Author      : Lars Mortensen
// Version     : 1.0
// Description : file_client in C++, Ansi-style
//============================================================================

#include <iostream>
#include <fstream>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include "../lib/iknlib.hpp"

using namespace std;

void receiveFile(string fileName, int socketfd);

int main(int argc, char *argv[])
{
	// TO DO Your own code
  int sockfd, portno, n;
  struct sockaddr_in serv_addr;
  struct hostent *server;

  char buffer[BUFSIZE];
  if (argc < 2) {
     fprintf(stderr,"usage %s hostname port default: %d\n", argv[0], PORT);
     exit(0);
  }
  if( argc == 3){
    portno = atoi(argv[2]);
  }
  else{
    portno = PORT;
  }
  sockfd = socket(AF_INET, SOCK_STREAM, 0);
  if (sockfd < 0)
      error("ERROR opening socket");
  server = gethostbyname(argv[1]);
  if (server == NULL) {
      fprintf(stderr,"ERROR, no such host\n");
      exit(0);
  }
  bzero((char *) &serv_addr, sizeof(serv_addr));
  serv_addr.sin_family = AF_INET;
  bcopy((char *)server->h_addr,
       (char *)&serv_addr.sin_addr.s_addr,
       server->h_length);
  serv_addr.sin_port = htons(portno);
  if (connect(sockfd,(struct sockaddr *) &serv_addr,sizeof(serv_addr)) < 0)
      error("ERROR connecting");
  printf("Please enter the message: ");
  bzero(buffer,BUFSIZE);
  fgets(buffer,255,stdin);
  writeTextTCP(sockfd, buffer);
  bzero(buffer,256);
  n = read(sockfd,buffer,255);
  if (n < 0)
       error("ERROR reading from socket");
  printf("%s\n",buffer);
  close(sockfd);
  return 0;
}

/**
 * Modtager filstørrelsen og udskriver meddelelsen: "Filen findes ikke" hvis størrelsen = 0
 * ellers
 * Åbnes filen for at skrive de bytes som senere modtages fra serveren (HUSK kun selve filnavn)
 * Modtag og gem filen i blokke af 1000 bytes indtil alle bytes er modtaget.
 * Luk filen, samt input output streams
 *
 * @param fileName Det fulde filnavn incl. evt. stinavn
 * @param sockfd Stream for at skrive til/læse fra serveren
 */
void receiveFile(string fileName, int sockfd)
{
	// TO DO Your own code
}
