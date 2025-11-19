using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 120.0f;

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        float move = Input.GetAxis("Vertical");    
        float turn = Input.GetAxis("Horizontal");  

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * move * moveSpeed);

        transform.Rotate(0, turn * rotationSpeed * Time.deltaTime, 0);

        animator.SetFloat("speed", Mathf.Abs(move));

        animator.SetFloat("turn", turn);
    }



    public void Inspect()
    {
        animator.SetBool("inspecting", true);

        Invoke("StopInspecting", 2.0f);
    }

    private void StopInspecting()
    {
        animator.SetBool("inspecting", false);
    }
}

