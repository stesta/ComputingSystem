// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.

// Runs an infinite loop that listens to the keyboard input. 
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel. When no key is pressed, 
// the screen should be cleared.

(LOOP)
  // iter = 8192 (total # of screen registers)
  @8192
  D=A
  @iter
  M=D

  // set current pos to the first SCREEN register address
  // pos acts as a pointer
  @SCREEN
  D=A
  @pos
  M=D

  // check KBD input
  @KBD
  D=M
  @FILL
  D;JGT
  @CLEAR
  D;JEQ

(FILL)
  // fills current pos register
  @pos
  A=M
  M=-1
  // incremement pos
  @pos
  M=M+1
  // decrement iter
  @iter
  DM=M-1

  @FILL
  D;JGT
  @LOOP
  D;JEQ

// same as above just clear
(CLEAR)
  @pos
  A=M
  M=0
  @pos
  M=M+1
  @iter
  DM=M-1

  @CLEAR
  D;JGT
  @LOOP
  D;JEQ

