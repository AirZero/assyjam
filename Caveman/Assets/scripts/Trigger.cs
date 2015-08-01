using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	
	public enum Mode {
		Activate  		= 0, // Activate the target GameObject
		Deactivate		= 1, // Deactivate target GameObject
		Jump			= 2, // Makes the player jump
		Shoot			= 3, // Shoot set projectile
		StayOn			= 4, // Activate trigger only when stayed on with source
		ReverseStayOn	= 5, // Deactivate trigger only when stayed on with source
		NextLevel		= 6, // Load next level
	}
	
	
	public Mode action = Mode.Activate;	// The action to accomplish
	
	public Object target;	// The game object to affect. If none, the trigger work on itself
	public GameObject source;	// The object that triggers the trigger
	public Rigidbody projectile;	// The projectile that shoots out on Shoot
	public int triggerCount = 1; // Here you can set how many times the trigger works.
	public bool triggerOnce = true;	// Only triggers when you enter the trigger if true.
	public bool repeatTrigger = false; // Here you can define if the trigger works over and over again.
	public int bulletSpeed = 10;	// This decides how fast the projectiles will fly
	
	/*
	 * Activates the trigger which is selected
	 */
	void ActivateTrigger (Collider activator) {
		Debug.Log (activator.name);
		triggerCount--;
		
		if (triggerCount == 0 || repeatTrigger) {
			Object currentTarget = target != null ? target : gameObject;
			GameObject targetGameObject = currentTarget as GameObject;
			
			switch (action) {
				// Activate any object you want.
			case Mode.Activate:
				if(source != null){
					if(activator.transform == source.transform || activator.name == source.name + "(Clone)")
						targetGameObject.SetActive(true);
				}
				else
					targetGameObject.SetActive(true);
				break;
				// Deactivate any object you want
			case Mode.Deactivate:
				if(source != null){
					if(activator.transform == source.transform || activator.name == source.name + "(Clone)")
						targetGameObject.SetActive(false);
				}
				else
					targetGameObject.SetActive(false);
				break;
				// Makes the player jump
			case Mode.Jump:
				ResponsivePlatformerController rpc = source.GetComponent<ResponsivePlatformerController>();
			
				if(activator.transform == source.transform) {
					if(rpc != null)
						rpc.forceJump = true;

				}
				break;
				// Make a clone of the GameObject you want
			case Mode.Shoot:
				if(activator.transform == source.transform) {
					if (projectile != null) {
						Rigidbody bullet;
						bullet = Instantiate (projectile, source.transform.position + Vector3.forward*2, source.transform.rotation) as Rigidbody;
						bullet.velocity = transform.TransformDirection(Vector3.forward * bulletSpeed);
					}
				}
				break;
				// Activate any object you want when stayed on.
			case Mode.StayOn:
				if(source != null){
					if(activator.transform == source.transform || activator.name == source.name + "(Clone)")
						targetGameObject.SetActive(true);
				}
				else
					targetGameObject.SetActive(true);
				break;
				// Same as StayOn but on reverse
			case Mode.ReverseStayOn:
				targetGameObject.SetActive(false);
				break;
				// Go to the next level.
			case Mode.NextLevel:
				if(source != null){
					if(activator.transform == source.transform || activator.name == source.name + "(Clone)")
						Application.LoadLevel(Application.loadedLevel+1);
				}
				else
					Application.LoadLevel(Application.loadedLevel+1);
				break;
			}
		}
	}
	
	/*
	 * Deactivate the trigger that needs this part as well, like StayOn and ReverseStayOn
	 */
	void DeactivateTrigger (Collider activator) {
		if (triggerCount == 0 || repeatTrigger) {
			Object currentTarget = target != null ? target : gameObject;
			GameObject targetGameObject = currentTarget as GameObject;
			
			switch (action) {
				// Deactivate any object when stepped off.
			case Mode.StayOn:
				if(source != null){
					if(activator.transform == source.transform || activator.name == source.name + "(Clone)")
						targetGameObject.SetActive(false);
				}
				else
					targetGameObject.SetActive(false);
				break;
				// Same as StayOn but on reverse
			case Mode.ReverseStayOn:
				targetGameObject.SetActive(true);
				break;
			}
		}
	}
	
	void OnTriggerEnter (Collider other) {
		ActivateTrigger (other);
	}
	
	void OnTriggerExit (Collider other) {
		DeactivateTrigger (other);
	}
	
	void OnTriggerStay (Collider other) {
		if(!triggerOnce)
			ActivateTrigger (other);
	}
}
