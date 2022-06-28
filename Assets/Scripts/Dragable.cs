using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    [SerializeField] private GameObject uICanvas;
    [SerializeField] private Vector3 canvasPos;
    private Transform dragableParent;
    private bool uIIsHidden = true;
    private GameObject UICamera;
    private GameObject selfCanvas;
    private LayerMask layerMask = 1 << 8;

    private void Start()
    {
        UICamera = GameObject.Find("UI Camera");
        dragableParent = transform.parent;
    }

    private void Update()
    {
        if(!uIIsHidden)
        {
            RotateCanvas();
            if(Physics.OverlapSphere(transform.position, 2f, layerMask).Length <= 0)
            {
                HideUI();
                uIIsHidden = true;
            }
        }
    }

    private void HideUI()
    {
        selfCanvas.SetActive(false);
    }

    public void ShowUI()
    {
        if (uICanvas == null)
            return;

        if (uIIsHidden)
        {
            if (selfCanvas == null)
            {
                selfCanvas = Instantiate(uICanvas);
                selfCanvas.transform.parent = this.transform;
                selfCanvas.transform.position = transform.position + canvasPos;
                RotateCanvas();
            }

            selfCanvas.SetActive(true);

        }

        uIIsHidden = false;
    }
    private void RotateCanvas()
    {
        selfCanvas.transform.LookAt(UICamera.transform.position, Vector3.up);
    }

    public void ToParent()
    {
        transform.parent = dragableParent;
    }

}
