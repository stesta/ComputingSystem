module ALU where

import ElementaryGates

-- | ALU: Aritmetic Logic Unit
-- | inputs: 
-- |    if zx then x = 0
-- |    if nx then x = !x
-- |    if zy then y = 0
-- |    if ny then y = !y
-- |    if f then out x+y else out x&y
-- |    if no the out = !out
-- |    xs = multi-bit x input
-- |    ys = multi-bit y input
-- | outputs:
-- |    [Int] output
-- |    zr true iff out == 0
-- |    ng true iff out < 0
-- | comments:
-- |    overflow is neither detected nor handled

-- | todo: return zr and ng
alu :: (Int, Int, Int, Int, Int, Int) -> [Int] -> [Int] -> [Int]
alu (zx, nx, zy, ny, f, no) xs ys 
-- |              |-output negation-|               |-negate x---| |-zero x----------------|  |-negate y---| |-zero y----------------|
    | f == 1    = map (xor_ no)      (snd (add_mbit (map (xor_ nx) (map (and_ (not_ zx)) xs)) (map (xor_ ny) (map (and_ (not_ zy)) ys))))
    | otherwise = map (xor_ no)           (and_mbit (map (xor_ nx) (map (and_ (not_ zx)) xs)) (map (xor_ ny) (map (and_ (not_ zy)) ys)))
