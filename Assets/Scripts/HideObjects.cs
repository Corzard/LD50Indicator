using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjects : MonoBehaviour
{
    [SerializeField] private Transform player;
    private List<GameObject> hiddenObjects = new List<GameObject>();
    private int hideableLayer = 1 << 7;
    [SerializeField] private LayerMask layermask;
    private RaycastHit[] hits = null;
    private Vector3 direction;
    private float distance;
    private bool contains;
    private int indexToUnhide;

    // Update is called once per frame
    void Update()
    {
        direction = player.position - transform.position;
        distance = Vector3.Distance(player.position, transform.position);
        //Debug.DrawRay(transform.position, player.position - transform.position, Color.blue, 4f);
        //RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, distance, layermask))
        {
            //Debug.Log("Not empty");
            //hit something!
            hits = Physics.RaycastAll(transform.position, direction, distance, LayerMask.GetMask("Hideable"));
            if (hits != null)
            {
                // Debug.Log("Not empty");
                //Debug.Log($"{hits[0].transform.gameObject.name}");
                foreach (var hit in hits)
                {
                    if (!hiddenObjects.Contains(hit.transform.gameObject))
                    {
                        //Debug.Log("Hided");
                        //hit.transform.GetComponent<MeshRenderer>().enabled = false;
                        StartCoroutine(LowerAlpha(hit.transform.gameObject));
                        //ChangeColor(hit.transform.gameObject);
                        hiddenObjects.Add(hit.transform.gameObject);
                    }

                }
                GameObject hideThis = null;
                foreach (var obj in hiddenObjects)
                {
                    foreach (var hit in hits)
                        if (hit.transform.gameObject == obj.gameObject)
                        {
                            Debug.Log("Contains");
                            contains = true;
                            hideThis = null;
                            break;
                        }
                        else
                        {
                            contains = false;
                            Debug.Log($"Dosen't contain {hiddenObjects[indexToUnhide].transform.name}");
                            //indexToUnhide = hiddenObjects.IndexOf(obj);
                            hideThis = obj.transform.gameObject;
                        }

                }
                if (!contains)
                {
                    //hiddenObjects[indexToUnhide].transform.GetComponent<MeshRenderer>().enabled = true;
                    if(hiddenObjects.Contains(hideThis))
                    StartCoroutine(HigherAlpha(hiddenObjects[hiddenObjects.IndexOf(hideThis)].transform.gameObject));
                    Debug.Log($"Changin {hiddenObjects[hiddenObjects.IndexOf(hideThis)].transform.name}");
                    hiddenObjects.Remove(hideThis);
                }
            }

            Debug.DrawRay(transform.position, direction, Color.green, 4f, true);
        }
        else if (hiddenObjects != null)
        {
            foreach (var item in hiddenObjects)
            {
                //item.transform.GetComponent<MeshRenderer>().enabled = true;
                StartCoroutine(HigherAlpha(item.transform.gameObject));
            }
            hiddenObjects.Clear();
        }
    }
    private IEnumerator LowerAlpha(GameObject go)
    {
        Material currentMaterial = go.GetComponent<Renderer>().material;
        Color startColor = currentMaterial.color;
        Color endColor = startColor;
        while (currentMaterial.color.a > 0.2f && currentMaterial.color.a <=1)
        {
            endColor.a -= 0.1f;
            currentMaterial.SetColor("_Color", endColor);
            yield return new WaitForSeconds(0.001f);
        }
        if(currentMaterial.color.a<0.2f || currentMaterial.color.a >= 1)
        {
            endColor.a = 0.2f;
            currentMaterial.SetColor("_Color", endColor);
        }
            
    }

    private IEnumerator HigherAlpha(GameObject go)
    {
        Material currentMaterial = go.GetComponent<Renderer>().material;
        Color startColor = currentMaterial.color;
        Color endColor = startColor;
        while (currentMaterial.color.a < 1 && currentMaterial.color.a >=0.2f)
        {
            endColor.a += 0.1f;
            currentMaterial.SetColor("_Color", endColor);
            yield return new WaitForSeconds(0.001f);
        }
        if (currentMaterial.color.a < 1f || currentMaterial.color.a >= 0.2f)
        {
            endColor.a = 1f;
            currentMaterial.SetColor("_Color", endColor);
        }
    }

    private void ChangeColor(GameObject go)
    {
        Material currentMaterial = go.GetComponent<Renderer>().material;
        Color startColor = currentMaterial.color;
        Color endColor = startColor;
        endColor.a -= 0.8f;
        currentMaterial.SetColor("_Color", endColor);

    }
}
