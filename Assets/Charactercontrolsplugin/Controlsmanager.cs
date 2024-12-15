using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using TouchControlsKit;
using UnityEngine;
using UnityEngine.UI;
using KinematicCharacterController;
public class Controlsmanager : MonoBehaviour
{
    public KinematicCharacterMotor kinematicCharacterMotor;
    public static Controlsmanager instance;
    public TCKTouchpad touchpad;
    public Charactercontroller charactercontroller;
    public FloatingJoystick joystick;
    public PlayervehicleInteraction playervehicleInteraction;
    float characterStoppingSpeed;
    public float characterMoveSpeed;
    public Image crosshair;
    private Animator anim;
    public bool isButtonDown;
    void Awake()
    {
        instance = this;
        characterMoveSpeed=charactercontroller.MaxStableMoveSpeed;
        anim = GetComponent<Animator>();

    }
    private void Start()
    {
        StartCoroutine(ActivateControls());
    }
    IEnumerator ActivateControls()
    {
        yield return new WaitForSeconds(.2f);
        ActivateControls(true, characterMoveSpeed);
    }

    public void ChangePlayerPosition(Transform newPosition)
    {
        kinematicCharacterMotor.SetPositionAndRotation(newPosition.position, Quaternion.identity);
    }

    public void UpdateCharacterSpeed(float _speed)
    {
        charactercontroller.setMaxSpeed(_speed);
    }

    //this will enable/diable touchpad interaction
    public void ActivateTouchPad(bool state)
    {
        touchpad.isActive = state;
    }

    public void ActivateControls(bool state,float speed)
    {
        crosshair.gameObject.SetActive(state);
        ActivateTouchPad(state);

        joystick.gameObject.SetActive(state);

        joystick.gameObject.GetComponent<FloatingJoystick>().SimulatePointerUp();

        if (playervehicleInteraction.acquireTime > 0)
        {
            return;
        }
        charactercontroller.setMaxSpeed(speed);
    }

    /// <summary>
    /// Stop player by passing 0 to this method
    /// </summary>
    public void StopPlayer()
    {
        charactercontroller.setMaxSpeed(characterStoppingSpeed);
    }

    /// <summary>
    /// Move player by passing required value 3.5 is by default
    /// </summary>
    public void MovePlayer()
    {
        charactercontroller.setMaxSpeed(characterMoveSpeed);
    }


    public void UpdateSensitivity(float value)
    {
        touchpad.sensitivity = value;

    }

    public void OnUpdateTool(string _toolName)
    {
        print("Tool name is   " + _toolName);
        anim.Play(_toolName);

    }
    public void ApplyTool()
    {
        anim.SetBool("Apply", isButtonDown);
    }
    public void PointerUp()
    {
        isButtonDown = false;
        anim.SetTrigger("stopApply");
    }
    public void WashTool()
    {
        anim.SetTrigger("wash");
    }
    public void Dustbin()
    {
        anim.SetTrigger("apply");
    }
    public void AddToIgnoreCollision(BoxCollider _box)
    {
        charactercontroller.IgnoredColliders.Add(_box);
    }
    public void RemoveItemToIgnoreCollision()
    {
        charactercontroller.IgnoredColliders.RemoveAt(charactercontroller.IgnoredColliders.Count-1);
    }
}
