module ElementaryGates where

import Control.Arrow
import Data.List (mapAccumR)

-- | Nand gate: all other gates build off of this one
nand_ :: Int -> Int -> Int
nand_ a b
    | a == 1 && b == 1 = 0
    | otherwise        = 1
nand_mbit as bs = zipWith nand_ as bs

-- | Not gate
not_     a  = nand_ a a
not_mbit as = map not_ as

-- | And gate
and_     a  b  = not_(nand_ a b)
and_mbit as bs = zipWith and_ as bs
and_mway as b  = foldr (and_) b as
 
-- | Or gate
or_     a  b  = (nand_ a a) `nand_` (nand_ b b)
or_mbit as bs = zipWith or_ as bs
or_mway as    = foldr (or_) 0 as

-- | Xor gate
xor_     a  b  = (or_ a b) `and_` (nand_ a b)
xor_mbit as bs = zipWith xor_ as bs

-- | Mux: Multiplexor
mux_          s0       a  b                    = (and_ a (not_(s0))) `or_` (and_ b s0) 
mux_mbit      s0       as bs                   = zipWith (mux_ s0) as bs
mux_4way_mbit s0 s1    as bs cs ds             = (map (and_mway [not_(s0),not_(s1)]) as) `or_mbit` (map (and_mway [not_(s0), s1]) bs) `or_mbit` (map (and_mway [s0, not_(s1)]) cs) `or_mbit` (map (and_mway [s0, s1]) ds)
mux_8way_mbit s0 s1 s2 as bs cs ds es fs gs hs = (map (and_mway [not_(s0),not_(s1),not_(s2)]) as) `or_mbit` (map (and_mway [not_(s0),not_(s1),s2]) bs) `or_mbit` (map (and_mway [not_(s0),s1,not_(s2)]) cs) `or_mbit` (map (and_mway [not_(s0),s1,s2]) ds) `or_mbit` (map (and_mway [s0,not_(s1),not_(s2)]) es) `or_mbit` (map (and_mway [s0,not_(s1),s2]) fs) `or_mbit` (map (and_mway [s0,s1,not_(s2)]) gs) `or_mbit` (map (and_mway [s0,s1,s2]) hs)

-- | DMux: DeMultiplexor
dmux_          s0       a  = (and_ a (not_(s0)), and_ a s0)
dmux_mbit      s0       as = map (dmux_ s0) as
dmux_4way_mbit s0 s1    as = (map (and_mway [not_(s0), not_(s1)]) as, map (and_mway [not_(s0), s1]) as, map (and_mway [s0, not_(s1)]) as, map (and_mway [s0,s1]) as)
dmux_8way_mbit s0 s1 s2 as = (map (and_mway [not_(s0), not_(s1), not_(s2)]) as, map (and_mway [not_(s0), not_(s1), s2]) as, map (and_mway [not_(s0), s1, not_(s2)]) as, map (and_mway [not_(s0),s1, s2]) as, map (and_mway [s0, not_(s1), not_(s2)]) as, map (and_mway [s0, not_(s1), s2]) as, map (and_mway [s0, s1, not_(s2)]) as, map (and_mway [s0, s1, s2]) as)

-- | Adders
halfAdder (a,b)   = (and_ a b, xor_ a b)
fullAdder (c,a,b) = (\(cy,s) ->  first (or_ cy) $ halfAdder(b,s)) $ halfAdder(c,a)
add_mbit   as     = mapAccumR (\cy (f,a,b) -> f (cy,a,b)) 0 . zip3 (replicate (length as) fullAdder) as 