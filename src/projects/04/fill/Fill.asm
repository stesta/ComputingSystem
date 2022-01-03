// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.

@pixels
M=0

(MAIN)
    // reset iterator
    @8192
    D=A
    @iter
    M=D

    // get keyboard input
    @KBD  
    D=M 

    // choose screen color
    @SETBLACK
    D;JNE
    @SETWHITE
    D;JEQ

(SETWHITE)
    @pixels
    M=0
    @DRAW
    0;JMP

(SETBLACK)
    @pixels
    M=-1
    @DRAW
    0;JMP

(DRAW)
    @iter
    D=M
    
    // store the current screen pixels address to write
    @SCREEN
    D=D+A
    @R0 
    M=D 

    // write the pixels
    @pixels
    D=M
    @R0
    A=M
    M=D

    // decrement the iterator 
    // having the 0 check at the bottoms means we're 
    // inclusive of @SCREEN+0
    @iter
    M=M-1
    D=M
    @MAIN
    D;JLT
    @DRAW
    0;JMP

(END)
    @END
    0;JMP