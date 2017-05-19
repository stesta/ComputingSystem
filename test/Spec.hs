import Test.Hspec
import ALU

main :: IO ()
main = hspec $ do
  describe "alu" $ do
    it "zx:1, nx:0, zy:1, ny:0, f:1, no:0 (0)" $ do 
      (alu (1,0,1,0,1,0) 3 5) `shouldBe` 0

    it "zx:1, nx:1, zy:1, ny:1, f:1, no:1 (1)" $ do 
      (alu (1,1,1,1,1,1) 3 5) `shouldBe` 1

    it "zx:1, nx:1, zy:1, ny:0, f:1, no:0 (-1)" $ do 
      (alu (1,1,1,0,1,0) 3 5) `shouldBe` -1

    it "zx:0, nx:0, zy:1, ny:1, f:0, no:0 (x)" $ do
      (alu (0,0,1,1,0,0) 3 5) `shouldBe` 3

    it "zx:1, nx:1, zy:0, ny:0, f:0, no:0 (y)" $ do
      (alu (1,1,0,0,0,0) 3 5) `shouldBe` 5

    it "zx:0, nx:0, zy:1, ny:1, f:0, no:1 (!x)" $ do
      (alu (0,0,1,1,0,1) 3 5) `shouldBe` -4

    it "zx:1, nx:1, zy:0, ny:0, f:0, no:1 (!y)" $ do
      (alu (1,1,0,0,0,1) 3 5) `shouldBe` -6

    it "zx:0, nx:0, zy:1, ny:1, f:1, no:1 (-x)" $ do
      (alu (0,0,1,1,1,1) 3 5) `shouldBe` -3

    it "zx:1, nx:1, zy:0, ny:0, f:1, no:1 (-y)" $ do
      (alu (1,1,0,0,1,1) 3 5) `shouldBe` -5

    it "zx:0, nx:1, zy:1, ny:1, f:1, no:1 (x+1)" $ do
      (alu (0,1,1,1,1,1) 3 5) `shouldBe` 4

    it "zx:1, nx:1, zy:0, ny:1, f:1, no:1 (y+1)" $ do
      (alu (1,1,0,1,1,1) 3 5) `shouldBe` 6

    it "zx:0, nx:0, zy:1, ny:1, f:1, no:0 (x-1)" $ do
      (alu (0,0,1,1,1,0) 3 5) `shouldBe` 2

    it "zx:1, nx:1, zy:0, ny:0, f:1, no:0 (y-1)" $ do
      (alu (1,1,0,0,1,0) 3 5) `shouldBe` 4

    it "zx:0, nx:0, zy:0, ny:0, f:1, no:0 (x+y)" $ do
      (alu (0,0,0,0,1,0) 3 5) `shouldBe` 8

    it "zx:0, nx:1, zy:0, ny:0, f:1, no:1 (x-y)" $ do
      (alu (0,1,0,0,1,1) 3 5) `shouldBe` -2

    it "zx:0, nx:0, zy:0, ny:1, f:1, no:1 (y-x)" $ do
      (alu (0,0,0,1,1,1) 3 5) `shouldBe` 2

    it "zx:0, nx:0, zy:0, ny:0, f:0, no:0 (x&y)" $ do
      (alu (0,0,0,0,0,0) 3 5) `shouldBe` 1

    it "zx:0, nx:1, zy:0, ny:1, f:0, no:1 (x|y)" $ do
      (alu (0,1,0,1,0,1) 3 5) `shouldBe` 7
