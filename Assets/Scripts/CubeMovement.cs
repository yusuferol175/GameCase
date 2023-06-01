using UnityEngine;

namespace DefaultNamespace
{
    public class CubeMovement : MonoBehaviour
    {
        private float _rotationSpeed = 10f;

        void Update()
        {
            transform.Rotate(Vector3.right, _rotationSpeed * Time.deltaTime);
        }
    }
}