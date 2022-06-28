using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler PlayerDead;

    [SerializeField] private float speed = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private Behaviour[] deactivateOnDeath;
    private int dragLayer = 1 << 6;
    private Collider holdingObject = null;
    private bool isHolding = false;




    private void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("isRunning", true);
            if (!isHolding)
                FreeMove();
            else if (isHolding)
                HoldingMove();
        }
        else
            animator.SetBool("isRunning", false);
    }

    private void Update()
    {

        Collider[] UIcolls = Physics.OverlapSphere(transform.position, 0.5f, dragLayer);
        if (UIcolls.Length > 0)
        {
            foreach (var col in UIcolls)
            {
                col.GetComponent<Dragable>().ShowUI();
            }
            Collider[] colls = Physics.OverlapSphere(transform.position, 0.5f, dragLayer);
            //colls[0].GetComponent<Dragable>().ShowUI();
            if (Input.GetKey(KeyCode.E) && colls.Length > 0 && holdingObject == null)
            {
                isHolding = true;
                animator.SetBool("EPressed", isHolding);
                holdingObject = colls[0];
                holdingObject.transform.parent = this.transform;


            }
            else
                colls = null;

        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isHolding = false;
            animator.SetBool("EPressed", isHolding);
            if (holdingObject != null)
                //holdingObject.transform.parent = dragableParent.transform;
                holdingObject.GetComponent<Dragable>().ToParent();
            holdingObject = null;
        }
    }
    private void FreeMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(
            transform.position.x + h, transform.position.y, transform.position.z + v), speed * Time.deltaTime);

        float rot = v + h;
        switch (rot)
        {
            case 2:
                {
                    if (transform.rotation != Quaternion.Euler(0, 45, 0))
                        transform.rotation = Quaternion.Euler(0, 45, 0);
                    break;
                }
            case 1:
                {
                    if (Input.GetKey(KeyCode.UpArrow) && transform.rotation != Quaternion.Euler(0, 0, 0))
                        transform.rotation = Quaternion.Euler(0, 0, 0);

                    else if (Input.GetKey(KeyCode.RightArrow) && transform.rotation != Quaternion.Euler(0, 90, 0))
                        transform.rotation = Quaternion.Euler(0, 90, 0);

                    break;
                }
            case 0:
                {
                    if (Input.GetKey(KeyCode.LeftArrow) && transform.rotation != Quaternion.Euler(0, -45, 0))
                        transform.rotation = Quaternion.Euler(0, -45, 0);

                    else if (Input.GetKey(KeyCode.RightArrow) && transform.rotation != Quaternion.Euler(0, 135, 0))
                        transform.rotation = Quaternion.Euler(0, 135, 0);
                    break;
                }
            case -1:
                {
                    if (Input.GetKey(KeyCode.LeftArrow) && transform.rotation != Quaternion.Euler(0, -90, 0))
                        transform.rotation = Quaternion.Euler(0, -90, 0);

                    else if (Input.GetKey(KeyCode.DownArrow) && transform.rotation != Quaternion.Euler(0, 180, 0))
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                }
            case -2:
                {
                    if (transform.rotation != Quaternion.Euler(0, -135, 0))
                        transform.rotation = Quaternion.Euler(0, -135, 0);
                    break;
                }

            default:
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                }
        }



    }
    private void HoldingMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.Rotate(0, h * rotationSpeed, 0, Space.Self);
        transform.position = Vector3.MoveTowards(transform.position, transform.forward + transform.position, speed / 2 * Time.deltaTime * v);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CameraTrigger>() != null)
        {
            StartCoroutine(other.GetComponent<CameraTrigger>().SetPos());
        }
        if (other.CompareTag("DeadZone"))
            Die();

    }

    public void Die()
    {
        foreach (var item in deactivateOnDeath)
        {
            item.enabled = false;
            animator.SetBool("Cought", true);
            PlayerDead?.Invoke();
        }
    }
}
