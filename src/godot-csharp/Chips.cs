namespace ComputingSystem;

public static class Chips
{
	public static int Nand(int a, int b) => 
		~(a & b);

	public static int Not(int @in) => 
		Nand(@in, @in);

	public static int And(int a, int b) => 
		Not(Nand(a, b));
	
	public static int Or(int a, int b) => 
		Nand(Not(a), Not(b));

	public static int Xor(int a, int b) =>
		Or(
			And(Not(a), b), 
			And(a, Not(b)));
	
	public static int Mux(int a, int b, int sel) =>
		Or(
			And(a, Not(sel)),
			And(b, sel));
	
	public static (int a, int b) DMux(int @in, int sel) =>
		(And(@in, Not(sel)), And(@in, sel));
} 
