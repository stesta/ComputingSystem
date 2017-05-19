clock :: [Int]
clock = 0 : 1 : clock

slowClock :: [Int]
slowClock = 0 : 0 : 1 : slowClock