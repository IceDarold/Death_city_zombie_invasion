using System;
using UnityEngine;

[Serializable]
public class ColliderInfo
{
	public ColliderInfo(string _name, int _layer, Vector3 _position, Vector3 _rotation, Vector3 _scale, Vector3 _center, Vector3 _size) : this(_name, _layer, _position, _rotation, _scale, _center)
	{
		this.type = ColliderType.BOX;
		this.size = _size;
	}

	public ColliderInfo(string _name, int _layer, Vector3 _position, Vector3 _rotation, Vector3 _scale, Vector3 _center, float _radius, float _height, int _direction) : this(_name, _layer, _position, _rotation, _scale, _center)
	{
		this.type = ColliderType.CAPSULE;
		this.radius = _radius;
		this.height = _height;
		this.direction = _direction;
	}

	public ColliderInfo(string _name, int _layer, Vector3 _position, Vector3 _rotation, Vector3 _scale, Vector3 _center)
	{
		this.name = _name;
		this.layer = _layer;
		this.position = _position;
		this.rotation = _rotation;
		this.scale = _scale;
		this.center = _center;
	}

	public ColliderType type;

	public string name;

	public int layer;

	public Vector3 position;

	public Vector3 rotation;

	public Vector3 scale;

	public Vector3 center;

	public Vector3 size;

	public float radius;

	public float height;

	public int direction;
}
