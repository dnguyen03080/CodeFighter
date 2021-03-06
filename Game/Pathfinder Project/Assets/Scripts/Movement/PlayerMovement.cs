﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	private float _DefaultSpeed = 50f;
	private float _MoveSpeed;

	private GameObject RevertTrap;
	private GameObject SlowTrap;
	private GameObject SpeedTrap;

	private Vector3 _Input;
	private static Vector3 _SpawnLocation;
//	private static Vector3 _FloatPaintLocation;
//	private static Vector3 _DoNothingPaintLocation;

	private Rigidbody _RB;

	private bool _IsReverted;
	private bool _IsSlow;
	private bool _IsFast;

	private bool _DebugMode = true;

	private Color _OriginalColor;

	// Use this for initialization
	public void Start ()
	{
		_MoveSpeed = _DefaultSpeed;

		_OriginalColor = gameObject.GetComponent<Renderer> ().material.color;

		_SpawnLocation = transform.position;
		_RB = GetComponent<Rigidbody>();
	}

	public static Vector3 SpawnLocation
	{
		get
		{
			return _SpawnLocation;
		}
		set
		{
			_SpawnLocation = value;
		}
	}

	public void Update()
    {
		if (Input.GetKey(KeyCode.Escape))
			SceneManager.LoadScene("Main Menu");
    }

	public void FixedUpdate () 
	{
		UpdatePlayerPosition();
	}

	private void UpdatePlayerPosition()
	{
		// Get input from user
		_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		// Transform user input to align with the camera position
		Vector3 transformInputBasedOnCamera = Camera.main.transform.TransformDirection(_Input);

		// Apply force to the player
		_RB.AddForce(transformInputBasedOnCamera * _MoveSpeed);

		// Check to see if the player fell off the map or went too high
		if (transform.position.y < -6 || transform.position.y > 6)
		{
			Respawn();
		}

		if (_IsReverted)
		{
			_MoveSpeed = -Mathf.Abs(_MoveSpeed);
		}

		if (_IsSlow)
		{
			_MoveSpeed = 5f;
		}

		if(_IsFast)
		{   
			_MoveSpeed = 90f;
		}
	}

	#region Collision Example

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.name.StartsWith("RevertTrap", System.StringComparison.Ordinal))
		{
			if(_DebugMode)
			{
				print ("OnCollision: RevertTrap");
			}

			_IsReverted = true;
			_IsSlow = false;
			_IsFast = false;
		}
		if (other.transform.name.StartsWith("SlowTrap", System.StringComparison.Ordinal))
		{
			if (_DebugMode) 
			{
				print ("OnCollision: SlowTrap");
			}

			_IsSlow = true;
			_IsFast = false;
			_IsReverted = false;
		}
		if (other.transform.name.StartsWith("SpeedTrap", System.StringComparison.Ordinal))
		{
			if (_DebugMode) 
			{
				print ("OnCollision: SpeedTrap");
			}

			_IsFast = true;
			_IsSlow = false;
			_IsReverted = false;
		}
	}
	#endregion

	#region Trigger Example
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.name.StartsWith ("HollowTrap", System.StringComparison.Ordinal)) {
			Color _CurrentColor = gameObject.GetComponent<Renderer> ().material.color;
			Color _FloatColor = new Color (0.965f, 0.278f, 0.306f);

			if (_CurrentColor == _FloatColor) {
				print ("FLOAT");

				other.isTrigger = false;
			} else {
				other.isTrigger = true;
			}

			if (_DebugMode) {
				print ("OnTrigger: HollowTrap");
			}
		} 
		if (other.transform.name.StartsWith("HeavyPaint", System.StringComparison.Ordinal))
		{
			if (_DebugMode) 
			{
				print ("OnTrigger: HeavyPaint");
			}
				
			gameObject.GetComponent<Renderer> ().material.color = new Color(1.0f, 0.6f, 0.25f);

			_RB.mass = 10f;

//			Destroy (other.gameObject);
		}
		if (other.transform.name.StartsWith("DoNothingPaint", System.StringComparison.Ordinal))
		{
			if (_DebugMode) 
			{
				print ("OnTrigger: DoNothingPaint");
			}

			gameObject.GetComponent<Renderer> ().material.color = new Color(0.176f, 0.184f, 0.898f);

//			Destroy (other.gameObject);
		}
		if (other.transform.name.StartsWith("FloatPaint", System.StringComparison.Ordinal))
		{
			if (_DebugMode) 
			{
				print ("OnTrigger: FloatPaint");
			}

			gameObject.GetComponent<Renderer> ().material.color = new Color(0.965f, 0.278f, 0.306f);

//			Destroy (other.gameObject);

		}
	}

	#endregion

	void Respawn() 
	{
		SetPlayerPositionToDefault();
		SetPlayerSpeedToDefault();
		SetTrapBehaviorToDefault();
		SetPlayerColorToDefault ();
	}

	private void SetPlayerPositionToDefault()
	{
		Rigidbody rigidBody = GetComponent<Rigidbody>();

		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
		rigidBody.ResetCenterOfMass();
		rigidBody.ResetInertiaTensor();

		transform.position = _SpawnLocation;
	}

	private void SetPlayerSpeedToDefault()
	{
		_MoveSpeed = _DefaultSpeed;
	}

	private void SetPlayerColorToDefault()
	{
		gameObject.GetComponent<Renderer> ().material.color = _OriginalColor;
	}

	private void SetTrapBehaviorToDefault()
	{
		_IsSlow = false;
		_IsReverted = false;
		_IsFast = false;
	}
}
