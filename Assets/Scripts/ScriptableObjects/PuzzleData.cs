using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleData", menuName = "Data/Puzzle", order = 0)]
public class PuzzleData : ScriptableObject
{
	public Sprite Sprite;
	public int CountSlicesX;
	public int CountSlicesY;
}
