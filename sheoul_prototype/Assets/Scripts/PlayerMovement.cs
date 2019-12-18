using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement Constants

    private const float MAX_ACCELERATION         = 100.0f;
    private const float GRAVITY_ACCELERATION     = 20.0f;

    private const float MAX_FORWARD_VELOCITY     = 5.0f;
    private const float MAX_BACKWARD_VELOCITY    = 4.0f;
    private const float MAX_STRAFE_VELOCITY      = 4.0f;
    private const float MAX_FALL_VELOCITY        = 100.0f;
    private const float ANGULAR_VELOCITY_FACTOR  = 2.0f;

    private const float DIAGONAL_VELOCITY_FACTOR = 0.7f;
    private const float WALK_VELOCITY_FACTOR     = 1.0f;
    private const float RUN_VELOCITY_FACTOR      = 1.25f;

    private const float MIN_HEAD_LOOK_ROTATION   = 75.0f;
    private const float MAX_HEAD_LOOK_ROTATION   = 75.0f;


    //Istance Variables

    private CharacterController controller;
    private Transform           cameraTransform;

    private Vector3 acceleration;
    private Vector3 velocity;
    private float   velocityFactor;

    // Start is called before the first frame update
    private void Start()
    {
        controller       = GetComponent<CharacterController>();
        cameraTransform  = GetComponentInChildren<Camera>().transform;
        acceleration     = Vector3.zero;
        velocity         = Vector3.zero;
        velocityFactor   = WALK_VELOCITY_FACTOR;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateVelocityFactor();
        UpdateRotation();
        UpdateCamera();
    }

    private void UpdateVelocityFactor()
    {
        velocityFactor = Input.GetButton("Run") ?
            RUN_VELOCITY_FACTOR : WALK_VELOCITY_FACTOR;

        if (Input.GetAxis("Strafe") != 0 && Input.GetAxis("Forward") != 0)
            velocity *= DIAGONAL_VELOCITY_FACTOR;
    }

    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * ANGULAR_VELOCITY_FACTOR;

        transform.Rotate(0f, rotation, 0f);
    }

    private void UpdateCamera()
    {
        Vector3 cameraRotation = cameraTransform.localEulerAngles;

        cameraRotation.x -= Input.GetAxis("Mouse Y") * ANGULAR_VELOCITY_FACTOR;

        if (cameraRotation.x > 180.0f)
            cameraRotation.x = Mathf.Max(cameraRotation.x, MIN_HEAD_LOOK_ROTATION);
        else
            cameraRotation.x = Mathf.Min(cameraRotation.x, MAX_HEAD_LOOK_ROTATION);

        cameraTransform.localEulerAngles = cameraRotation;
    }

    // FixedUpdate is called every fixed framerate frame
    private void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }

    private void UpdateAcceleration()
    {
        acceleration.x = Input.GetAxis("Strafe") * MAX_ACCELERATION;
        acceleration.z = Input.GetAxis("Forward") * MAX_ACCELERATION;
        if(!controller.isGrounded) acceleration.y = -GRAVITY_ACCELERATION;
        else acceleration.y = 0f;
    }

    private void UpdateVelocity()
    {
        velocity += acceleration * Time.fixedDeltaTime;

        velocity.x = acceleration.x == 0f ? velocity.x = 0f : Mathf.Clamp(
            velocity.x, -MAX_STRAFE_VELOCITY * velocityFactor,
            MAX_STRAFE_VELOCITY * velocityFactor);

        velocity.y = acceleration.y == 0f ? velocity.y = -0.1f : Mathf.Clamp(
            velocity.y, -MAX_FALL_VELOCITY, 0f);

        velocity.z = acceleration.z == 0f ? velocity.z = 0f : Mathf.Clamp(
            velocity.z, -MAX_BACKWARD_VELOCITY * velocityFactor,
            MAX_FORWARD_VELOCITY * velocityFactor);
    }

    private void UpdatePosition()
    {
        Vector3 move = velocity * Time.fixedDeltaTime;

        controller.Move(transform.TransformVector(move));
    }
}
