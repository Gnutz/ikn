#include <stdio.h>
#include <stdlib.h>














int main(int argc, char *argv[])
{
     char buffer[256];
     if (argc < 2) {
         fprintf(stderr,"ERROR, no file proovided");
         exit(1);
     }
     sprintf(buffer, argv[1]);
     printf("trying to open: %s.\n", buffer);
         //Undersøger om filens findes på serveren.
     FILE *fp  = fopen(buffer, "r");
     if (fp == nullptr) printf("ERROR opening file\n");
     fseek(fp, 0, SEEK_END);
     int fileLen=ftell(fp);
     fseek(fp, 0, SEEK_SET);
     printf("size: %d bytes\n", fileLen);

     return 0;
}
