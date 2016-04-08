using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour 
{

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.position += new Vector3(x, y) * Time.deltaTime;
    }
}
