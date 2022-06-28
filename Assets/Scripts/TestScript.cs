using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Color c;
    public Material m;
    // Start is called before the first frame update


    // Update is called once per frame
    private void Start()
    {
        m = transform.GetComponent<Renderer>().material;
        c = m.color;
    }
    void Update()
    {
        
    }
}
