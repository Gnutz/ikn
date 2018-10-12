/* A simple server in the internet domain using TCP
   The port number is passed as an argument */
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>

void error(const char *msg)
{
    perror(msg);
    exit(1);
}

int main(int argc, char *argv[])
{
     int sockfd, newsockfd, portno; //To fileDescriptors en til welcomingsocket og en til connectionsocket
     socklen_t clilen;
     char buffer[256];
     struct sockaddr_in serv_addr, cli_addr; /*fra netinet/in.h
     struct'en bliver brugt til at gemme adresser for client/server
     */
     int n;
     if (argc < 2) {
         fprintf(stderr,"ERROR, no port provided\n");
         exit(1);
     }
     sockfd = socket(AF_INET, SOCK_STREAM, 0);/*socket(int domain, int type, int protocol)
     AF_INET = adress familiy (Internet protocol v4 addresses) = 2 fra  socket.h
     SOC_STREAM = 1, betyder at det er TCP socket fra socket.h
     */
     if (sockfd < 0)
        error("ERROR opening socket");
     bzero((char *) &serv_addr, sizeof(serv_addr));//Måske overflødig
     portno = atoi(argv[1]);
     serv_addr.sin_family = AF_INET;//Address family = 2
     serv_addr.sin_addr.s_addr = INADDR_ANY;//IPv6 Address
     serv_addr.sin_port = htons(portno); /*Port nummer htons host-to-network
     The htons() function converts the unsigned short integer hostshort from host byte order to network byte order.
     */

     if (bind(sockfd, (struct sockaddr *) &serv_addr,
              sizeof(serv_addr)) < 0) /* bind (int sockfd, const struct sockaddr *addr, socklen_t addrlen)
              When s socket is created witch socket() it exisst in a name space (address family)
              but has no address assigned to it
              bind() assigns the address specified by addr to the socket referred to by the filedescriptor
*/
              error("ERROR on binding");
     listen(sockfd,5);
     //listen(sockfd, listeningQueueSize)

     clilen = sizeof(cli_addr);

     newsockfd = accept(sockfd,
                 (struct sockaddr *) &cli_addr,
                 &clilen);
     if (newsockfd < 0)
          error("ERROR on accept");
     bzero(buffer,256);
     n = read(newsockfd,buffer,255);
     if (n < 0) error("ERROR reading from socket");
     printf("Here is the message: %s\n",buffer);
     n = write(newsockfd,"I got your message",18);
     if (n < 0) error("ERROR writing to socket");
     close(newsockfd);
     close(sockfd);
     return 0;
}
