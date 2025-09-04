// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)
// The algorithm is based on repetitive addition.

// R2 = 0 - resets values
@R2
DM=0

// i = R0 - sets up iterator
// END if R0 == 0
@R0
D=M
@END
D;JEQ
@i
M=D

// END if R1 == 0
@R1
D=M
@END
D;JEQ

(LOOP)
  // R2 = R2 + R1
  @R2
  D=M
  @R1
  D=D+M
  @R2
  M=D
  // update iterator
  @i
  DM=M-1
  @LOOP
  D;JGT

(END)
  @END
  0;JMP
