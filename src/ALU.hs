module ALU where

import Data.Bits ((.&.), complement, zeroBits)

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

negation :: Int -> Int -> Int
negation n bytes
    | n == 1    = complement bytes
    | otherwise = bytes

zero :: Int -> Int -> Int
zero z bytes
    | z == 1    = zeroBits 
    | otherwise = bytes

alu :: (Int, Int, Int, Int, Int, Int) -> Int -> Int -> Int 
alu (zx, nx, zy, ny, f, no) x y = no' $ f' x' y'
    where 
        no' = negation no 
        f'
            | f == 1 = (+)
            | otherwise = (.&.)
        x'  = (negation nx $ zero zx x)
        y'  = (negation ny $ zero zy y)
        
-- | Implementation based off ElementaryGates, verbose
-- alu :: (Int, Int, Int, Int, Int, Int) -> [Int] -> [Int] -> [Int]
-- alu (zx, nx, zy, ny, f, no) xs ys 
-- -- |              |-output negation-|               |-negate x---| |-zero x----------------|  |-negate y---| |-zero y----------------|
--     | f == 1    = map (xor_ no)      (snd (add_mbit (map (xor_ nx) (map (and_ (not_ zx)) xs)) (map (xor_ ny) (map (and_ (not_ zy)) ys))))
--     | otherwise = map (xor_ no)           (and_mbit ) (map (xor_ nx) (map (and_ (not_ zx)) xs)) (map (xor_ ny) (map (and_ (not_ zy)) ys))