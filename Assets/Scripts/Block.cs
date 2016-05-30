using UnityEngine;

public class Block {
	public Vector2 position;
	public Vector2 size;
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