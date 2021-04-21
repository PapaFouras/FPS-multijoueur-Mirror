
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]


public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private float mouseSensitivityX = 3f;

        [SerializeField]
    private float mouseSensitivityY = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;

    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;

    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount(){
        return thrusterFuelAmount;
    }
    
    [Header("Joint Options")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 80f;

    private ConfigurableJoint joint ;
    private Animator animator;
   

    private PlayerMotor motor;


    private void Start() {
        animator = GetComponent<Animator>();
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(jointSpring);
    } 

    private void Update() {

        if(PauseMenu.isOn == true) {

            if(Cursor.lockState != CursorLockMode.None){
                Cursor.lockState = CursorLockMode.None;
            }

            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
           motor.RotateCamera(0f);
           motor.ApplyThruster(Vector3.zero);
            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked){
            Cursor.lockState = CursorLockMode.Locked;
        }

        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f)){
            joint.targetPosition = new Vector3(0f,-_hit.point.y,0f);
        }
        else{
            joint.targetPosition = Vector3.zero;
        }
        //Calculer la vélocité du mouvement de notre joueur
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        //jouer animation thruster
        animator.SetFloat("ForwardVelocity", zMov);
        motor.Move(velocity);

        // on calcule la rotation du player
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0,yRot,0) * mouseSensitivityX;

        motor.Rotate(rotation);

        // on calcule la rotation de la cam
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * mouseSensitivityY;

        motor.RotateCamera(cameraRotationX);

    Vector3 thrusterVelocity = Vector3.zero;
        //calcul la variable thrusterForce
        if(Input.GetButton("Jump" ) && thrusterFuelAmount > 0){
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount>=0.1f){
                thrusterVelocity = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
            
        }
        else{
            
                SetJointSettings(jointSpring);
            if(!Input.GetButton("Jump")){
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            }
            
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount,0f,1f);
        //applique la variable thrusterForce

        motor.ApplyThruster(thrusterVelocity);
    }

    private void SetJointSettings( float _jointSpring){
        joint.yDrive = new JointDrive{ positionSpring = _jointSpring, maximumForce = jointMaxForce};
    }
}
