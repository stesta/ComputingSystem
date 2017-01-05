import Test.Hspec
import ElementaryGates
import ALU

main :: IO ()
main = hspec $ do
  describe "alu" $ do
    it "zx:0, nx:0, zy:1, ny:1, f:0, no:0 (x)" $ do
      (alu (0,0,1,1,0,0) [0,0,1,1] [0,1,0,1]) `shouldBe` [0,0,1,1]
