using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour
{

    private Vector3 velocity;
    private Vector3 rotation;

private Vector3 thrusterVelocity;

    [SerializeField]
    private Camera cam;

private float cameraRotationX = 0f;
private float currentCameraRotationX = 0f;

    private Rigidbody rb;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private void Start() {
        rb = GetComponent<Rigidbody>();

    }

    public void Move(Vector3 _velocity){
        velocity = _velocity;

    }

    public void Rotate(Vector3 _rotation){
        rotation = _rotation;

    }
    public void RotateCamera(float _cameraRotationX){
        cameraRotationX = _cameraRotationX;

    }

    private void FixedUpdate() {
        
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement(){

        if(velocity != Vector3.zero){
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if(thrusterVelocity != Vector3.zero){
            rb.AddForce(thrusterVelocity * Time.fixedDeltaTime,ForceMode.Acceleration);
        }

    }

    private void PerformRotation(){
        //On calcule la rotation de la camera
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        //on applique la rotation de la caméra
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX,0f,0f);


    }

    public void ApplyThruster(Vector3 _thrusterVelocity){
        thrusterVelocity = _thrusterVelocity;
    }
   
}
