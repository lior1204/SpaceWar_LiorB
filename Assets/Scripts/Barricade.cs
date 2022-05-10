using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Projectile"))
        {
            Material mat = GetComponent<MeshRenderer>().material;
            mat.SetFloat("Vector1_B41B51E2", 1);
            mat.SetVector("Vector2_C9DA6C6F", collision.contacts[0].point);
        }
    }
}
