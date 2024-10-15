using UnityEngine;
namespace PathCreation
{
    public class PathFollower : MonoBehaviour
    {
        #region Variables
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public bool StartMove = true;
        public float speed = 5;
        public bool RotateObject = false;
        float distanceTravelled;
        #endregion

        #region Start & Update
        void Start()
        {
            UpdatePathFollow();
        }
        void Update()
        {
            if (pathCreator != null && StartMove)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                if(RotateObject)
                {
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }
            }
        }
        #endregion

        #region General
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
        public void UpdatePathFollow()
        {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }
        public void ResetPathFollow()
        {
            distanceTravelled = 0f;
        }
        #endregion
    }
}