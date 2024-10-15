using UnityEngine;
namespace PathCreation.Examples 
{
    [RequireComponent(typeof(PathCreator))]
    public class GeneratePathExample : MonoBehaviour {
        public Transform[] waypoints;
        void Start () {
            if (waypoints.Length > 0) {
                var objPath = GetComponent<PathCreator>();
                BezierPath bezierPath = new BezierPath (waypoints);
                GetComponent<PathCreator> ().bezierPath = bezierPath;
            }
        }
    }
}