using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100f;
    public Transform laserPosition;
    public LineRenderer m_lineRenderer;
    private Transform m_transform;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }
    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        if(Physics2D.Raycast(m_transform.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserPosition.position, transform.right);
            Draw2DRay(laserPosition.position, _hit.point);
        }
        else
        {
            Draw2DRay(laserPosition.position, laserPosition.transform.right * defDistanceRay);
        }
    }
    private void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
            m_lineRenderer.SetPosition(0, startPos);
            m_lineRenderer.SetPosition(1, endPos);
    }
}
