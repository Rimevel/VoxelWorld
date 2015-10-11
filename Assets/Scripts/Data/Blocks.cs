using UnityEngine;
using System.Collections;

public class Blocks
{
	public static Block air = new BlockAir(0);
	public static Block stone = new Block(1);
	public static Block grass = new BlockGrass(2);
	public static Block dirt = new BlockDirt(3);

	public Blocks(){}
}
