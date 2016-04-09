using UnityEngine;
using System.Collections;

public class GridHelper : MonoBehaviour 
{
    public Color color = Color.white;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.01f);
    }
}
