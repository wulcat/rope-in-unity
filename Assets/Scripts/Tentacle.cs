using UnityEngine;
using System.Collections.Generic;

public class Tentacle : MonoBehaviour {

	

	
	public TentacleData _tentacleData;
    public Setting _setting;

    public GameObject origin;

    public float nodeCount = 10;
    public float radius = 5;
    public float gravity = 0.5f;
    public float windForce = 0f;
    public float friction = 0.02f;

    void Start () {

		_setting = new Setting(nodeCount , radius , gravity , windForce ,friction) ;
		_tentacleData = new TentacleData(20 , 2 , 0.5f , 0.2f);

	}

	void Update() {

		_tentacleData.Move(origin.transform.position) ;
		_tentacleData.Update(_setting.radius , _setting.length , _setting.friction, _setting.wind , _setting.gravity) ;

	}

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < _tentacleData.nodes.Count - 1; i++)
            {
                Gizmos.DrawLine(_tentacleData.nodes[i].GetVec(), _tentacleData.nodes[i + 1].GetVec());
            }
        }
    }

    #region TentacleData
    //[System.SerializableAttribute]
    public class TentacleData
    {
        public float length;
        public float radius;
        public float spacing;
        public float friction;

        public List<Node> nodes;


        public TentacleData(float _length, float _radius, float _spacing, float _friction)
        {
            length = _length;
            radius = _radius;
            spacing = _spacing;
            friction = _friction;
            nodes = new List<Node>();
            for (var i = 0; i < _length; i++)
            {
                nodes.Add(new Node());
            }
        }
        public void Move(Vector2 _position)
        {
            nodes[0].x = _position.x;
            nodes[0].y = _position.y;
        }
        public void Update(float _radius, float _length, float _friction, float _wind, float _gravity)
        {

            var i = 0;
            var px = 0f;
            var py = 0f;
            var node = new Node();
            var prev = nodes[0];

            _radius = radius * _radius;
            var step = _radius / length;
            for (i = 1; i < length; i++)
            {
                node = nodes[i];

                node.x += node.vx;
                node.y += node.vy;

                //much more understandable, but when using a vector the rope doesn't unfold when origin is at (0,0,0) 
                Vector2 dir = new Vector2(prev.x, prev.y) - new Vector2(node.x, node.y);
                dir.Normalize();
                px = node.x + dir.x * spacing * _length;
                py = node.y + dir.y * spacing * _length;

                node.x = prev.x - (px - node.x);
                node.y = prev.y - (py - node.y);

                node.vx = node.x - node.ox;
                node.vy = node.y - node.oy;

                node.vx *= friction * (1 - _friction);
                node.vy *= friction * (1 - _friction);

                node.vx += _wind;
                node.vy += _gravity;

                node.ox = node.x;
                node.oy = node.y;

                radius -= step;
                prev = node;
            }
        }
    }
    #endregion

    #region Node
    //[System.SerializableAttribute]
    public class Node
    {
        public float x;
        public float y;
        public float ox;
        public float oy;
        public float vx;
        public float vy;
        public Node(float _x, float _y)
        {
            x = _x;
            y = _y;
            ox = _x;
            oy = _y;
        }
        public Node() { }
        public Vector2 GetVec()
        {
            return new Vector2(x, y);
        }
    }
    #endregion

    #region Settings
    //[System.SerializableAttribute]
	public class Setting {
		public float length ;
		public float radius ;
		public float gravity ;
		public float wind ;
		public float friction ;
		public float thickness ;
		public Setting(float _length , float _radius , float _gravity , float _wind , float _friction) {
			length = _length ;
			radius = _radius ;
			gravity = _gravity ;
			wind = _wind ;
			friction = _friction ;
		}
	}
    #endregion

    
}
