using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour
{
    //input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;
    
    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;
    
    [SerializeField]
    private Camera playerCamera;
    private Animator Animator;
    public bool canMove = true;
    public float counter = 0f;
    public bool timer = false;
    public float CooldownDuration = 9.0f;
    private void FixedUpdate()
    {
        if (canMove == true)
        {
            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

            rb.AddForce(forceDirection, ForceMode.Impulse);
            forceDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementForce = 7;
            }
            else
            {
                movementForce = 1;
            }


            if (rb.velocity.y < 0f)
            {
                rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
            }

            Vector3 horizontalVelocity = rb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
                rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
            LookAt();
         
        }
      
    }
     void StartWalking()
    {
        movementForce = 1;
    }
    void StopWalking()
    {

    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > .9f && direction.sqrMagnitude > .9f)
        
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity= Vector3.zero;
        
    }
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward= playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void Awake()
    {

      
        rb = GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        Animator = this.GetComponent<Animator>();
      
    }

    private void OnEnable() {
        playerActionsAsset.land.Jump.started += DoJump;
        playerActionsAsset.land.Attack.started += DoAttack;
        move = playerActionsAsset.land.Move;
       
        playerActionsAsset.Enable();
        
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        Animator.SetTrigger("attack");
        timer = true;
        if (timer)
        {
            canMove = false;
            StartCoroutine(StartCooldown());
            
}
      
    }
    public IEnumerator StartCooldown()
    {
       
       yield return new WaitForSeconds(4.0f);
        canMove = true;
        timer = false;
    }
    private void OnDisable()
    {
      playerActionsAsset.land.Attack.started -= DoAttack;
        playerActionsAsset.land.Jump.started -= DoJump;
        playerActionsAsset.Disable();
       
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return true;
        else return false;
    }

    
}
