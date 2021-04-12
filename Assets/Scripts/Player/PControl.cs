using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PControl : MonoBehaviour
{
    //Camera FP
    float mouseX; //L/R
    float camSpeed; 
    float mouseY;// U/D
    float cameraX;
    Transform cameraTrn;

    // movement
    [SerializeField]
    float horizontal;
    float vertical;
    float movementSpeed;
    CharacterController controller;
    Vector3 loc;
    public KeyCode JumpKey;

    //Gravity
    public LayerMask groundLayerMask; //XXXXXXXXX
    Vector3 velocity;
    float gravity = -9.81f;
    bool isGround; // used mainly to correct jumping at start of game without it charactercontroller doesnot gert isGrounded on start right

    //Crouching
    public KeyCode CrouchKey;
    private float cpHeight; //cpHeight is the current player height
    [SerializeField]
    private bool isCrouching; // true when crouch

    //Animations
    private Animator pAnim;
    private void Awake()
    {
        pAnim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cameraTrn = gameObject.transform.GetChild(0).GetComponent<Transform>();
    }
    private void Start()
    {
        camSpeed = 500;
        cameraX = 0;
        movementSpeed = 10;
        cpHeight = 2f;
        isGround = false;
        isCrouching = false;
    }
    private void Update()
    {
        //movement- jump
        //animations
        //interactions
        //attack

        // Camera Control 3ps
        mouseX = Input.GetAxis("Mouse X") * camSpeed * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);
        mouseY = Input.GetAxis("Mouse Y") * camSpeed * Time.deltaTime;
        cameraX -= mouseY; // cameraX = cameraX - mouseY;
        cameraX = Mathf.Clamp(cameraX, 10, 20);
        cameraTrn.localRotation = Quaternion.Euler(cameraX, 0, 0);


        // movement
        horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        loc = transform.forward * vertical + transform.right * horizontal;
        controller.Move(loc);
        if ((Mathf.Abs(horizontal)>=0.01f)||(Mathf.Abs(vertical)>=0.01f)) //delay for realistic animation
        {
            pAnim.SetBool("isWalking", true);
        }
        else
        {
            pAnim.SetBool("isWalking", false);
        }

        //Gravity 
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            isGround = true;
            velocity.y = 0;
        }

        // Jumping
        if ((Input.GetKeyDown(JumpKey)) && (isGround) && (!isCrouching))
        {
            velocity.y += 8;
            isGround = false;
        }
        controller.Move(velocity * Time.deltaTime); // the control over gravity and jumping velocity of player

        // Crouching OPTIONAL 
        /*
        if (Input.GetKeyDown(CrouchKey))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                cpHeight = 1f;
            }
            else
            {
                isCrouching = false;
                cpHeight = 2f;
            }
        }
        controller.height = Mathf.Lerp(controller.height, cpHeight, 10f * Time.deltaTime);*/

        // Attack

        if (Input.GetMouseButtonDown(0))
        {
            PAttack("Regular");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PAttack("Warcry");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PAttack("Shield");
        }

    }
    private void PAttack(string AttType) // Attacking Func
    {
        //SkillAttack - normal attack, SkillWarcry - active skill 1, SkillShield - active skill 2
        if (AttType == "Regular")
        {
            pAnim.SetTrigger("SkillAttack");
        }
        else if (AttType == "Warcry")
        {
            pAnim.SetTrigger("SkillWarcry"); 
        }
        else if(AttType=="Shield")
        {
            pAnim.SetTrigger("SkillShield"); 
        }
                                       
    }
}
