using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
    public PlayerActionHandler actionHandler;


    InputActions controls;
    [SerializeField]
    float moveSpeed = 5.0f;

    Vector2 moveDir;



    private void Awake() {
        controls = new InputActions();
        moveDir = new Vector2();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.Action.performed += ctx => actionHandler.DoAction();

    }

    void Move(Vector2 dir) {

        // Debug.Log("Move!" + dir);
        moveDir = dir;

        animator.SetFloat("MoveSpeed", dir.sqrMagnitude);
        if(dir.sqrMagnitude > 0.01) {
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            animator.speed = 1;
        } else {
            animator.speed = 0;

        }
    }





    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        //controls.Disable();
    }

   

    void Update()
    {
        if(!actionHandler.inAction) { 
            rb.position += moveDir * moveSpeed * Time.deltaTime;
        }


    }
}
