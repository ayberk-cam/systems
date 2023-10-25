using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCheck : MonoBehaviour
{
    [HideInInspector]
    public GameObject objectHit;
    [HideInInspector]
    public GameObject parentObjectHit;

    void Update()
    {
        RaycastHit hit;
        Vector3 direction = (transform.position - Camera.main.transform.position).normalized;
        Ray ray = new Ray(Camera.main.transform.position, direction);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            objectHit = hit.transform.gameObject;
            GameObject findParent = hit.transform.gameObject;
            while (findParent.transform.parent != null)
            {
                findParent = findParent.transform.parent.gameObject;
            }
            parentObjectHit = findParent;

            //Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.green);
        }
        //Debug.DrawRay(Camera.main.transform.position, direction * 1000);
    }
}
