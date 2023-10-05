using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private DefaultControls controls;
    private void Awake()
    {
        controls = new();
        controls.Movement.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Movement.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        controls.Movement.Jump.performed += _ => Jump();
        controls.Movement.Slide.performed += _ => Slide();
        controls.Movement.Sprint.performed += _ => Sprint();
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void Move(Vector2 dir)
    {
        dir.Normalize();
        Debug.Log("Move: " + dir);
    }
    private void Jump()
    {
        Debug.Log("Jumping");
    }
    private void Slide()
    {
        Debug.Log("Sliding");
    }
    private void Sprint()
    {
        Debug.Log("Sprinting");
    }
    private void Look(Vector2 dir)
    {
        Debug.Log("Look: " + dir);
    }
}