using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody rb;
    private Animator anim;

    private Vector3 moveVelocity;
    private Vector3 lookPos;
     
    private Transform cam;
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;

    float forwardAmount;
    float turnAmount;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        mainCamera = FindObjectOfType<Camera>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 100))
        {
            lookPos = hit.point;
        }
        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;
        transform.LookAt(transform.position + lookDir, Vector3.up);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
        move = vertical * camForward + horizontal * cam.right;

        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        Move(move);

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        moveVelocity = movement * moveSpeed;
        UpdateAnimator();
    }

    void Move(Vector3 move)
    {
        if(move.magnitude > 1)
        {
            move.Normalize();
        }
        this.moveInput = move;
        ConvertMoveInput();
        UpdateAnimator();
    }
    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveVelocity;
    }
    private void UpdateAnimator()
    {
        anim.SetFloat("Forward", forwardAmount);
        anim.SetFloat("Turn", turnAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

        }
    }
}
