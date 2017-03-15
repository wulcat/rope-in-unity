using UnityEngine;

public class Rope : MonoBehaviour {


	public RopeData ropeData;

    public int nodeCount = 10;
    public float nodeSpacing = 0.5f;
    public float gravity = -0.5f;
    public float windForce = 0f;
    public float friction = 0.02f;

    void Start () {

        ropeData = new RopeData(nodeCount, nodeSpacing , friction);

	}

	void Update() {

        ropeData.Move(transform.position);
        ropeData.Update(friction, windForce , gravity);

	}

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < ropeData.nodes.Length - 1; i++)
            {
                Gizmos.DrawLine(ropeData.nodes[i].position(), ropeData.nodes[i + 1].position());
            }
        }
    }

    #region Rope
    public class RopeData
    {
        public int nodeCount;
        public float spacing;
        public float friction;

        public Node[] nodes;

        public RopeData(int _nodeCount, float _spacing, float _friction)
        {
            nodeCount = _nodeCount;
            spacing = _spacing;
            friction = _friction;

            nodes = new Node[nodeCount];

        }
        public void Move(Vector2 _position)
        {
            nodes[0].x = _position.x;
            nodes[0].y = _position.y;
        }
        public void Update(float _friction, float _wind, float _gravity)
        {

            var i = 0;
            var px = 0f;
            var py = 0f;

            int previousNodeIndex = 0;

            for (i = 1; i < nodeCount; i++)
            {

                nodes[i].x += nodes[i].vx;
                nodes[i].y += nodes[i].vy;

                //much more understandable, but when using a vector the rope doesn't unfold when origin is at (0,0,0) 
                Vector2 dir = new Vector2(nodes[previousNodeIndex].x, nodes[previousNodeIndex].y) - new Vector2(nodes[i].x, nodes[i].y);
                dir.Normalize();
                px = nodes[i].x + dir.x * spacing * nodeCount;
                py = nodes[i].y + dir.y * spacing * nodeCount;

                nodes[i].x = nodes[previousNodeIndex].x - (px - nodes[i].x);
                nodes[i].y = nodes[previousNodeIndex].y - (py - nodes[i].y);

                nodes[i].vx = nodes[i].x - nodes[i].ox;
                nodes[i].vy = nodes[i].y - nodes[i].oy;

                nodes[i].vx *= friction * (1 - _friction);
                nodes[i].vy *= friction * (1 - _friction);

                nodes[i].vx += _wind;
                nodes[i].vy += _gravity;

                nodes[i].ox = nodes[i].x;
                nodes[i].oy = nodes[i].y;

                previousNodeIndex = i;
            }
        }
    }
    #endregion

    #region Node
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
