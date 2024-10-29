using UnityEngine;
namespace PathCreation.Examples 
{
    [RequireComponent(typeof(PathCreator))]
    public class GeneratePathExample : MonoBehaviour {
        public Transform[] waypoints;
        public bool ClosePath;
        public PathCreation.PathSpace pathSpace = PathCreation.PathSpace.xy;
        void Start () {
            if (waypoints.Length > 0) {
                var objPath = GetComponent<PathCreator>();
                BezierPath bezierPath = new BezierPath (waypoints, ClosePath, pathSpace);
                GetComponent<PathCreator> ().bezierPath = bezierPath;
            }
        }
    }
}