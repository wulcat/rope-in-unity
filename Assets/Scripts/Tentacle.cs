using UnityEngine;
using System.Collections.Generic;

public class Tentacle : MonoBehaviour {


	public TentacleData _tentacleData;

    public GameObject origin;

    public int nodeCount = 10;
    public float nodeSpacing = 0.5f;
    public float gravity = -0.5f;
    public float windForce = 0f;
    public float friction = 0.02f;

    void Start () {

		_tentacleData = new TentacleData(nodeCount, nodeSpacing , friction);

	}

	void Update() {

		_tentacleData.Move(origin.transform.position);
		_tentacleData.Update(nodeCount , friction, windForce , gravity);

	}

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < _tentacleData.nodes.Length - 1; i++)
            {
                Gizmos.DrawLine(_tentacleData.nodes[i].position(), _tentacleData.nodes[i + 1].position());
            }
        }
    }

    #region TentacleData
    //[System.SerializableAttribute]
    public class TentacleData
    {
        public float length;
        public float spacing;
        public float friction;

        //public List<Node> nodes;
        public Node[] nodes;

        public TentacleData(float _length, float _spacing, float _friction)
        {
            length = _length;
            spacing = _spacing;
            friction = _friction;
            //nodes = new List<Node>();
            nodes = new Node[(int)_length];

            //for (var i = 0; i < _length; i++)
            //{
            //    nodes.Add(new Node());
            //}
        }
        public void Move(Vector2 _position)
        {
            nodes[0].x = _position.x;
            nodes[0].y = _position.y;
        }
        public void Update(float _length, float _friction, float _wind, float _gravity)
        {

            var i = 0;
            var px = 0f;
            var py = 0f;
            //var node = new Node();
            int prev = 0;

            for (i = 1; i < length; i++)
            {
                //node = nodes[i];

                nodes[i].x += nodes[i].vx;
                nodes[i].y += nodes[i].vy;

                //much more understandable, but when using a vector the rope doesn't unfold when origin is at (0,0,0) 
                Vector2 dir = new Vector2(nodes[prev].x, nodes[prev].y) - new Vector2(nodes[i].x, nodes[i].y);
                dir.Normalize();
                px = nodes[i].x + dir.x * spacing * _length;
                py = nodes[i].y + dir.y * spacing * _length;

                nodes[i].x = nodes[prev].x - (px - nodes[i].x);
                nodes[i].y = nodes[prev].y - (py - nodes[i].y);

                nodes[i].vx = nodes[i].x - nodes[i].ox;
                nodes[i].vy = nodes[i].y - nodes[i].oy;

                nodes[i].vx *= friction * (1 - _friction);
                nodes[i].vy *= friction * (1 - _friction);

                nodes[i].vx += _wind;
                nodes[i].vy += _gravity;

                nodes[i].ox = nodes[i].x;
                nodes[i].oy = nodes[i].y;

                prev = i;
            }
        }
    }
    #endregion

    #region Node
    //public class Node
    //{
    //    public float x;
    //    public float y;

    //    public float ox;
    //    public float oy;

    //    public float vx;
    //    public float vy;

    //    public Node() { }

    //    public Vector2 position()
    //    {
    //        return new Vector2(x, y);
    //    }
    //}

    public struct Node
    {
        public float x;
        public float y;

        public float ox;
        public float oy;

        public float vx;
        public float vy;

        public Vector2 position()
        {
            return new Vector2(x, y);
        }
    }
    #endregion

}
