using UnityEngine;

namespace Sidema
{
    public class RotateObject : MonoBehaviour
    {

        public bool m_randomX;
        public bool m_randomY;
        public bool m_randomZ;
        public Vector3 m_speedPerAxis;
        public Space m_space;

        void Start()
        {
            if (m_randomX) m_speedPerAxis.x = UnityEngine.Random.Range(0.01f, m_speedPerAxis.x);
            if (m_randomY) m_speedPerAxis.y = UnityEngine.Random.Range(0.01f, m_speedPerAxis.y);
            if (m_randomZ) m_speedPerAxis.z = UnityEngine.Random.Range(0.01f, m_speedPerAxis.z);
        }

        void Update()
        {
            transform.Rotate(m_speedPerAxis.x * Time.deltaTime,
                                m_speedPerAxis.y * Time.deltaTime,
                                m_speedPerAxis.z * Time.deltaTime,
                                m_space);
        }
    }
}
