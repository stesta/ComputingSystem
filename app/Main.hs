module Main where

import ElementaryGates
import ALU

main :: IO ()
main = do 
    print (alu (1,0,1,0,1,0) 3 5) -- 0
    print (alu (1,1,1,1,1,1) 3 5) -- 1
    print (alu (1,1,1,0,1,0) 3 5) -- -1
    print (alu (0,0,1,1,0,0) 3 5) -- x
    print (alu (1,1,0,0,0,0) 3 5) -- y
    print (alu (0,0,1,1,0,1) 3 5) -- !x
    print (alu (1,1,0,0,0,1) 3 5) -- !y
    print (alu (0,0,1,1,1,1) 3 5) -- -x
    print (alu (1,1,0,0,1,1) 3 5) -- -y
    print (alu (0,1,1,1,1,1) 3 5) -- x+1
    print (alu (1,1,0,1,1,1) 3 5) -- y+1
    print (alu (0,0,1,1,1,0) 3 5) -- x-1
    print (alu (1,1,0,0,1,0) 3 5) -- y-1
    print (alu (0,0,0,0,1,0) 3 5) -- x+y
    print (alu (0,1,0,0,1,1) 3 5) -- x-y
    print (alu (0,0,0,1,1,1) 3 5) -- y-x
    print (alu (0,0,0,0,0,0) 3 5) -- x&y
    print (alu (0,1,0,1,0,1) 3 5) -- x|y
