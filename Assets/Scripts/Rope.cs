using UnityEngine;

public class Rope : MonoBehaviour {


	public RopeData ropeData;

    public int nodeCount = 10;
    public float nodeSpacing = 0.5f;
    public float gravity = -0.5f;
    public float windForce = 0f;
    public float friction = 0.02f;

    void Start () {

        ropeData = new RopeData(nodeCount);

	}

	void Update() {

        ropeData.MoveNode(transform.position, 1);
        ropeData.UpdateNodes(nodeSpacing, gravity, friction, windForce);

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
        int nodeCount;
        float nodeSpacing;
        float gravity;
        float friction;
        float windForce;

        public Node[] nodes;

        public RopeData(int _nodeCount)
        {
            nodeCount = _nodeCount;
            nodes = new Node[nodeCount];

        }

        public void MoveNode(Vector2 _position, int _nodeIndex)
        {
            nodes[_nodeIndex].x = _position.x;
            nodes[_nodeIndex].y = _position.y;
        }

        public void UpdateNodes(float _nodeSpacing, float _gravity, float _friction, float _windForce)
        {
            nodeSpacing = _nodeSpacing;
            gravity = _gravity;
            friction = _friction;
            windForce = _windForce;

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
                px = nodes[i].x + dir.x * nodeSpacing * nodeCount;
                py = nodes[i].y + dir.y * nodeSpacing * nodeCount;

                nodes[i].x = nodes[previousNodeIndex].x - (px - nodes[i].x);
                nodes[i].y = nodes[previousNodeIndex].y - (py - nodes[i].y);

                nodes[i].vx = nodes[i].x - nodes[i].ox;
                nodes[i].vy = nodes[i].y - nodes[i].oy;

                nodes[i].vx *= friction * (1 - friction);
                nodes[i].vy *= friction * (1 - friction);

                nodes[i].vx += windForce;
                nodes[i].vy += gravity;

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
