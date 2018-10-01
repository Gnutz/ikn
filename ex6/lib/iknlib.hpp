#ifndef LIB_HPP
#define LIB_HPP

#define PORT 9000
#define BUFSIZE 1000

void error(const char *msg);
const char *extractFileName(const char *fileName);
const char *readTextTCP(char *text, int length, int inFromServer);
void writeTextTCP(int outToServer, char *line);
long getFileSizeTCP(int inFromServer);
long check_File_Exists(char *fileName);


#endif // LIB_H
