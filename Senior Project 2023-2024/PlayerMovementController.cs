/*****************************************************************************
// File Name : PlayerMovementController.cs
// Author : Craig Hughes, Jacob Zydorowicz, Lucas Johnson
// Creation Date : September 12, 2023
// Last Updated : October 1 2023
// Brief Description : Controls player movement 
*****************************************************************************/
#region imported namespaces
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    Header["Player movement"]
    #region Variables
    public bool canMove = true;

    [Tooltip("The torque value that changes how much yaw changes")]
    [SerializeField] float yawTorque;

    [Tooltip("The torque value that changes how much pitch changes")]
    [SerializeField] float pitchTorque;

    [Tooltip("The torque value that changes how much roll occurs")]
    [SerializeField] float rollTorque;

    [SerializeField] float thrust;
    [SerializeField] float reverseThrust;
    private currentThrust;

    // Physics
    private Rigidbody playerRB;
    // Input Refrences
    private PlayerInput playerInput;

    // Control Setting Values
    [Range(0.1f, 2.0f)]
    [SerializeField] float mouseSensitivity = 1.0f;
    [SerializeField] bool invertPitch = false;
    [SerializeField] int altControlScheme = 0;

    // Input Values
    private float thrustInput;
    private float rollInput;
    private Vector2 pitchYawInput;
    private float yawInput;
    private float pitchInput;
    private float thrustDirection;
    [SerializeField] bool isForwardThrusting = true;

    public static PlayerMovementController Instance;
    #endregion
    public void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Instance = this;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        //changes the control scheme
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (altControlScheme == 0)
            {
                altControlScheme = 1;
                changeActionMap(altControlScheme);
            }
            else if (altControlScheme == 1)
            {
                altControlScheme = 0;
                changeActionMap(altControlScheme);
            }

        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            updateMovement();
        }
    }

    private void updateMovement()
    {
        float currentPitch = 0;
        float currentYaw = 0;

        //assigns input values based on active control scheme
        switch (altControlScheme)
        {
            case 0:
                currentPitch = pitchYawInput.y;
                currentYaw = pitchYawInput.x;
                break;
            case 1:
                currentPitch = pitchInput;
                currentYaw = yawInput;
                break;
        }

        if (invertPitch)
        {
            currentPitch *= -1;
        }

        //Rotation
        playerRB.AddRelativeTorque(Vector3.back * rollInput * rollTorque * Time.deltaTime);
        playerRB.AddRelativeTorque(Vector3.right * Mathf.Clamp(currentPitch, -0.5f, 0.5f) * pitchTorque * mouseSensitivity * Time.fixedDeltaTime);
        playerRB.AddRelativeTorque(Vector3.up * Mathf.Clamp(currentYaw, -0.5f, 0.5f) * yawTorque * mouseSensitivity * Time.fixedDeltaTime);

        //Forward acceleration and decceleration
        if (thrustInput >= 0.1f)
        {
            isForwardThrusting = true;
            thrustDirection = thrustInput;
            currentThrust = Mathf.Clamp((currentThrust + 750f), 0f, thrust);
            playerRB.AddRelativeForce(Vector3.forward * thrustInput * currentThrust * Time.fixedDeltaTime);
        }
        else if((thrustInput <= -0.1f))
        {
            if(!isForwardThrusting)
            {
                thrustDirection = thrustInput;
                currentThrust = Mathf.Clamp((currentThrust + 200f), 0f, reverseThrust);
                playerRB.AddRelativeForce(Vector3.forward * thrustInput * currentThrust * Time.fixedDeltaTime);
            }
            else if(isForwardThrusting)
            {
                currentThrust = Mathf.Clamp((currentThrust - 200f), 0f, thrust);
                playerRB.AddRelativeForce(Vector3.forward * thrustDirection * currentThrust * Time.fixedDeltaTime);
                
                if (currentThrust == 0)
                {
                    isForwardThrusting = false;
                }
            }
        }
        else
        {
            if (currentThrust == 0)
            {
                isForwardThrusting = false;
            }

            currentThrust = Mathf.Clamp((currentThrust - 150f), 0f, thrust);
            playerRB.AddRelativeForce(Vector3.forward * thrustDirection * currentThrust * Time.deltaTime);
        }
    }

    #region Inputs
    public void OnThrust(InputAction.CallbackContext obj)
    {
        thrustInput = obj.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext obj)
    {
        rollInput = obj.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext obj)
    {
        pitchYawInput = obj.ReadValue<Vector2>();
    }

    public void OnYaw(InputAction.CallbackContext obj)
    {
        yawInput = obj.ReadValue<float>();
    }

    public void OnPitch(InputAction.CallbackContext obj)
    {
        pitchInput = obj.ReadValue<float>();
    }

    #region Input Change Functions
    /// <summary>
    /// Method <c>changeSensitivity</c> Inverts the players pitch flight controls.
    /// </summary>
    public void makePitchInverted()
    {
        if (invertPitch)
        {
            invertPitch = false;
        }
        else
        {
            invertPitch = true;
        }
    }

    /// <summary>
    /// Method <c>changeSensitivity</c> Changes players mouse sensitivity.
    /// Param. <c>newValue</c> the new sensitivity from a scale of 0 to 2
    /// </summary>
    public void changeSensitivity(float newValue)
    {
        mouseSensitivity = newValue;
    }

    /// <summary>
    /// Method <c>changeActionMap</c> Changes active input mapping for the player
    /// Param. <c>mapValue</c> the new corresponding value of the new action map
    /// </summary>
    public void changeActionMap(int mapValue)
    {
        int mapSelection = mapValue;

        switch (mapSelection)
        {
            case 0:
                playerInput.SwitchCurrentActionMap("Default Shipflight");
                altControlScheme = 0;
                break;
            case 1:
                playerInput.SwitchCurrentActionMap("A/D Yaw ShipFlight");
                altControlScheme = 1;
                break;
        }

    }
    #endregion
    #endregion

    #region Getters Setters

    #region Getters
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetPitchTorque()
    {
        return pitchTorque;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetYawTorque()
    {
        return yawTorque;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetThrust()
    {
        return thrust;
    }
    #endregion

    #region Settters
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nextTorque"></param>
    public void SetPitchTorque(float nextTorque)
    {
        pitchTorque = nextTorque;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nextTorque"></param>
    public void SetYawTorque(float nextTorque)
    {
        yawTorque = nextTorque;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nextThrust"></param>
    public void SetThrust(float nextThrust)
    {
        thrust = nextThrust;
    }


    #endregion
    #endregion
}
