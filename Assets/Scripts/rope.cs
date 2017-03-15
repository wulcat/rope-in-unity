using UnityEngine;
using System.Collections.Generic;

public class Tentacle : MonoBehaviour {

	[System.SerializableAttribute]
	public class TentacleData {
		public float length ;
		public float radius ;
		public float spacing ;
		public float friction ;
		// public float shade ;

		public List<Node> nodes ;

		
		public TentacleData(float _length , float _radius , float _spacing , float _friction) {
			length = _length ;
			radius = _radius ;
			spacing = _spacing ;
			friction = _friction ;
			// shade = _shade ;
			nodes = new List<Node>() ;
			for(var i = 0 ; i < _length ; i++ ) {
				nodes.Add(new Node() );
			}
		}
		public void Move(Vector2 _position) {
			nodes[0].x = _position.x ;
			nodes[0].y = _position.y ;
		}
		public void Update(float _radius , float _length , float _friction , float _wind , float _gravity) {
			var i = 0 ;
			var j = 0 ;
			// var n = 0;
			var s = 0f;
			var c = 0f;
			var dx = 0f;
			var dy = 0f;
			var da = 0f;
			var px = 0f;
			var py = 0f;
			var node = new Node() ;
			var prev = nodes[0];
			
			_radius = radius * _radius ;
			var step = _radius/length ;
			for(j = 0 , i = 1 ; i < length ; i++ , j++) {
				node = nodes[i] ;

				node.x += node.vx ;
				node.y += node.vy ;

				dx = prev.x - node.x ;
				dy = prev.y - node.y ;
				da = Mathf.Atan2(dy , dx) ;

				px = node.x + Mathf.Cos(da) * spacing * _length ;
				py = node.y + Mathf.Sin(da) * spacing * _length ;

				node.x = prev.x - (px - node.x) ;
				node.y = prev.y - (py - node.y) ;

				node.vx = node.x - node.ox ;
				node.vy = node.y - node.oy ;

				node.vx *= friction * (1-_friction) ;
				node.vy *= friction * (1-_friction) ;

				node.vx += _wind ;
				node.vy += _gravity ;
				
				node.ox = node.x ;
				node.oy = node.y ;

				s = Mathf.Sin(da + 1.57079632679489661923f) ;
				c = Mathf.Cos(da + 1.57079632679489661923f) ;

				//

				radius -= step ;
				prev = node ;
			}
		}
	}
	[System.SerializableAttribute]
	public class Node {
		public float x ;
		public float y ;
		public float ox ;
		public float oy ;
		public float vx ;
		public float vy ;
		public Node(float _x , float _y) {
			x = _x ;
			y = _y ;
			ox = _x ;
			oy = _y ;
		}
		public Node() {}
		public Vector2 GetVec() {
			return new Vector2(x,y);
		}
	}
	public float CustomLength ;
	public TentacleData _tentacleData ;
	void Start () {
		// StartParts() ;
		_setting = new Setting(10 , 5 , 0.5f , -0.5f ,0.02f , 2f) ;
		_tentacleData = new TentacleData(20 , 2 , 0.5f , 0.2f);

	}
	public GameObject origin ;
	void Update() {
		// UpdateParts() ;
		_tentacleData.Move(origin.transform.position) ;
		_tentacleData.Update(_setting.radius , _setting.length , _setting.friction, _setting.wind , _setting.gravity) ;

	}
	public Setting _setting ;
	[System.SerializableAttribute]
	public class Setting {
		public float length ;
		public float radius ;
		public float gravity ;
		public float wind ;
		public float friction ;
		public float thickness ;
		public Setting(float _length , float _radius , float _gravity , float _wind , float _friction , float _thickness) {
			length = _length ;
			radius = _radius ;
			gravity = _gravity ;
			wind = _wind ;
			friction = _friction ;
			thickness = _thickness ;
		}
	}
	void OnDrawGizmos() {
		Gizmos.color = Color.cyan ;
		for (int i = 0; i < _tentacleData.nodes.Count - 1; i++) {
			Gizmos.DrawLine(_tentacleData.nodes[i].GetVec() , _tentacleData.nodes[i+1].GetVec());
		}
	}
}
