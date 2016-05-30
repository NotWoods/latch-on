using UnityEngine;

///<summary>Represents a platform</summary>
public class Block {
	public Vector2 position;
	public Vector2 size;
	///<summary>
	///If true, the block cannot be grappled to,
	///although the rope will still wrap around it.</summary>
	public bool isSolid = false; 
	
	public Block(Vector2 _position, Vector2 _size) {
		position = _position;
		size = _size;
	}
	public Block(Vector2 _position, Vector2 _size, bool _isSolid) {
		position = _position;
		size = _size;
		isSolid = _isSolid;
	}
}