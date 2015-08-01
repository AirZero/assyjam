using UnityEngine;
using System.Collections;

public class ResponsivePlatformerController : MonoBehaviour {
	
	public float gravity = 50f;
	public float terminalVelocity = 30f;
	
	public float moveSpeed = 15.0f;
	public float jumpForce = 25f;
	public bool forceJump = false;
	private bool jumped = false;
	private CharacterController controller;
	
	public Vector3 moveVector {get; set;}
	public float verticalVelocity {get; set;}

	private Animator animator;
	
	void Awake () {
		controller = gameObject.GetComponent("CharacterController") as CharacterController;
		animator = this.GetComponentInChildren<Animator>();
	}
	
	void Update () {
		Move();
		if(!controller.isGrounded && jumped && Input.GetButtonUp("Jump") && verticalVelocity > 0){
			verticalVelocity = 0f;
			jumped = false;
		}
		ProcessMovement();

		if(controller.isGrounded)
			animator.SetBool("Grounded", true);
		else
			animator.SetBool("Grounded", false);
		if(controller.velocity != Vector3.zero)
			animator.SetBool("Moving", true);
		else
			animator.SetBool("Moving", false);
	}
	
	public void Move(){
		var deadZone = 0.1f;
		verticalVelocity = moveVector.y;
		moveVector = Vector3.zero;
		if(Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone)
			moveVector += new Vector3(Input.GetAxis("Horizontal"),0,0);
		if((Input.GetButtonDown("Jump") && controller.isGrounded) || forceJump == true){
			verticalVelocity = jumpForce;
			jumped = true;
			forceJump = false;
		}
		moveVector *= moveSpeed;
	}
	
	public void ProcessMovement(){
		moveVector = new Vector3((moveVector.x), verticalVelocity, 0);
		if(moveVector.y > -terminalVelocity)
			moveVector = new Vector3(moveVector.x, (moveVector.y - gravity * Time.deltaTime), 0);
		if(controller.isGrounded && moveVector.y < -1)
			moveVector = new Vector3(moveVector.x, (-1), 0);
		controller.Move(moveVector * Time.deltaTime);
	}
}