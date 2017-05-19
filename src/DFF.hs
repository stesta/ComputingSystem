import Data.Bits (complement)

dff :: Int -> Int -> Int 
dff x clock = 
    let out = 0 : (.|.) ((.&.) x clock) ((.&.) out (complement clk))
    out