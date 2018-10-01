SOURCES=part1.cpp part2.cpp main.cpp
OBJECTS=${SOURCES:.cpp=.o}
EXECUTABLE=PARTS
CXX=g++
TARGETS=all, build, part1.o, part2.o, main.o, clean

all: ${OBJECTS}
	${CXX} -o ${EXECUTABLE} ${OBJECTS} # Creates the big one

#Løsning lavet ud fra:
#https://www.gnu.org/software/make/manual/html_node/Pattern-Examples.html#Pattern-Examples
%.o: %.c %.h
	#${CXX} -c ${SOURCES} $< -o $@
	${CXX} -c $@ -o $<
#Kan ikke helt forklare den endnu :)^

clean:
	rm ${EXECUTABLE} ${OBJECTS} # Removes all files made by this makefile

help:
	@echo ${TARGETS}
	@echo Objects are: ${OBJECTS}
	@echo $(CURDIR)
