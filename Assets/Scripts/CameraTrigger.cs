using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 setPos;
    [SerializeField] private GameObject gameCamera;
    [SerializeField] private GameObject secondFloor;
    [SerializeField] private GameObject roof;
    private Material roofMaterial;
    private Color roofColor;
    private Vector3 _lowerPos = new Vector3(13f, 7f, -16.5f);
    private Vector3 _higherPos = new Vector3(13f, 10.5f, -16.5f);
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roofMaterial = roof.GetComponent<Renderer>().material;
        roofColor = roofMaterial.color;
    }

    // public void SetPos()
    // { 
    //     while(Mathf.Abs(gameCamera.transform.position.y - setPos.y)> 0.2f)
    //     {
    //         gameCamera.transform.Translate(setPos - gameCamera.transform.position);
    //     }
    // }


    public IEnumerator SetPos()
    { 
        while(Mathf.Abs(gameCamera.transform.position.y - setPos.y)> 0.1f)
        {
            //gameCamera.transform.Translate(setPos - gameCamera.transform.position);
            gameCamera.transform.position = Vector3.MoveTowards(gameCamera.transform.position,
            setPos, 10f * Time.deltaTime);
            yield return new WaitForSeconds(0.0001f);
        }
        if(secondFloor.activeInHierarchy && setPos == _lowerPos)
        {
            secondFloor.SetActive(false);
            roofColor.a = 0.2f;
            roofMaterial.SetColor("_Color",roofColor);
        }
        else if(!secondFloor.activeInHierarchy && setPos == _higherPos)
        {
            secondFloor.SetActive(true);
            roofColor.a = 1f;
            roofMaterial.SetColor("_Color", roofColor);
        }
            
        
    }
}
