using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    //TOP and BOTTOM MOVEMENT LIMITS FOR PLAYER
    [HideInInspector]
    public List<float> playerLimits = new List<float>() { 5.5f, 35f };

    //MOVEMENT VARIABLES
    [Range(0f, 5f)]
    public float playerGravityScale;
    [Range(0f, 20f)]
    public float jumpForce;
    [Range(0f, 2f)]
    public float rotationTime;
    public float forwardSpeed;
    [Range(0f, 5f)]
    public float topKickForce;
    [Range(0f, 10f)]
    public float paintDistance;

    [HideInInspector]
    public float defaultAngle = 90f;
    [HideInInspector]
    public float rotationStartAngle = 45f;
    [HideInInspector]
    public float rotationEndAngle = 135f;

    //MOVEMENT CONTROL VARIABLES
    [HideInInspector]
    public bool isRotationAllowed = true;
    [HideInInspector]
    public bool isPlayerInLimits = true;
    
    //PENCIL 
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Color frontTipColor;
    public Material frontPenTip, backPenTip, bodyPenTip;

    [HideInInspector]
    public GameObject currentWall;

    //INSTANCE
    public static Player Instance;
    [HideInInspector]
    public bool isGameStarted = false;


    void Awake()
    {
        //SET INSTANCE
        Instance = this;
    }


    void Start()
    {
        Time.timeScale = 0;
        //TURN OFF DEFAULT UNITY GRAVITY FUNCTION
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        //SET THE SELECTED COLORS TO PEN TIPS
        /*
        transform.GetChild(1).GetComponent<Renderer>().material.color = GameMaker.Instance.selectedColorList[0];
        transform.GetChild(2).GetComponent<Renderer>().material.color = GameMaker.Instance.selectedColorList[1];
        */

        //SET THE SELECTED COLORS TO PEN TIPS
        frontPenTip.color = GameMaker.Instance.selectedColorList[0];
        backPenTip.color = GameMaker.Instance.selectedColorList[1];

        //SET FRONT TIP COLOR
        frontTipColor = GameMaker.Instance.selectedColorList[0];

        //PLAYER IS ALIVE
        GameMaker.Instance.isPlayerAlive = true;

        //GIVE PLAYER FORWARD SPEED
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed);
    }


    void FixedUpdate()
    {
        //USE A DIFFERENT GRAVITY SCALE FOR PENCIL
        UseChangedGravity();
    }


    void Update()
    {
        //IF PLAYER HITS THE TOP
        if (transform.position.y >= playerLimits[1])
            rb.velocity = new Vector3(0f, -topKickForce, rb.velocity.z);

        //CHECK PLAYER IS IN ROTATION LIMITS
        CheckPlayerLimits();
        if(Input.GetMouseButtonDown(0))
        {
            isGameStarted = true;
            UIManager.Instance.GameStartMenu();

        }
        //IF PLAYER CAN ROTATE AND USER CLICKS, ROTATE THE PLAYER
        if (Input.GetMouseButtonDown(0) && UIManager.Instance.isGameResume && GameMaker.Instance.isPlayerAlive && Input.mousePosition.y <= Screen.height * UIManager.Instance.clickableAreaRatio && isRotationAllowed && isPlayerInLimits && isGameStarted)
        {
            Time.timeScale = 1;
            //DONT ALLOW ROTATION WHILE ALREADY ROTATING
            isRotationAllowed = false;
            RotatePlayer();
        }

        //Debug.Log(transform.localRotation.eulerAngles.x);
    }

    //ROTATES PENCIL
    private void RotatePlayer()
    {
        //PLAY THE SOUND EFFECT and VIBRATE PHONE
        FindObjectOfType<AudioManager>().Play("PlayerRotateSound");
        GameMaker.Instance.Vibrate();

        //ROTATE THE PLAYER 

        transform.DORotate(new Vector3(180, 0, 0), rotationTime, RotateMode.LocalAxisAdd).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            if(!GameMaker.Instance.isPlayerInAnim)
                 isRotationAllowed = true;

        });

        //JUMP THE PLAYER
        rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);

        //SET FRONT TIP COLOR
        if (frontTipColor == GameMaker.Instance.selectedColorList[0])
            frontTipColor = GameMaker.Instance.selectedColorList[1];
        else
            frontTipColor = GameMaker.Instance.selectedColorList[0];


        Material temp;
        temp = frontPenTip;
        frontPenTip = backPenTip;
        backPenTip = temp;

        PaintManager.Instance.ChangeCurrentPenTipPoint();
        
    }


    //CHECK PLAYER IS IN ROTATION LIMITS
    private void CheckPlayerLimits()
    {
        //PENCIL POSITION Y
        float posY = transform.position.y;

        //CHECK POSITION
        if (posY < playerLimits[0] || posY > playerLimits[1])
            isPlayerInLimits = false;
        else
            isPlayerInLimits = true;
    }


    //CHANGES THE GRAVITY SCALE FOR PENCIL
    private void UseChangedGravity()
    {
        Vector3 changedGravity = 9.81f * playerGravityScale * Vector3.down;
        rb.AddForce(changedGravity, ForceMode.Acceleration);
    }


    public void EnterDrawingAnim(GameObject wall)
    {
        isRotationAllowed = false;
        GameMaker.Instance.isPlayerInAnim = true;
        rb.velocity = Vector3.zero;
        float startY = transform.position.y;


        defaultAngle = 90f;
        rotationStartAngle = 45f;
        rotationEndAngle = 135f;

        if (transform.localRotation.eulerAngles.x > 180f || transform.localRotation.eulerAngles.x < 0)
        {
            rotationStartAngle = 225f;
            rotationEndAngle = 315f;
            defaultAngle *= -1f;
        }


        transform.DOMoveZ(currentWall.transform.position.z - paintDistance, 0.25f);
        transform.DOMoveY(transform.position.y - 1f, 0.25f);
        transform.DOLocalRotateQuaternion(Quaternion.Euler(rotationStartAngle, 0f, 0f), 0.25f).OnComplete(() =>
        {
            StartCoroutine(PaintManager.Instance.PaintWall(currentWall, currentWall.GetComponent<Wall>().wallColor));            
            transform.DOLocalRotateQuaternion(Quaternion.Euler(rotationEndAngle, 0f, 0f), 0.75f);                           
            transform.DOMoveY(playerLimits[0], 0.75f).OnComplete(() =>
            {
                PaintManager.Instance.StopPainting();
                Destroy(wall);
                PaintManager.Instance.ClearActiveBrushes();
                transform.DOLocalRotateQuaternion(Quaternion.Euler(defaultAngle, 0f, 0f), 0.25f);
                transform.DOMoveY(startY, 0.25f).OnComplete(() =>
                {                    
                    GameMaker.Instance.isPlayerInAnim = false;
                    rb.velocity = new Vector3(0f, 2f, forwardSpeed);
                    isRotationAllowed = true;
                    SetCollider(true);
                });

            });

        });
        

    }

    public void CollectInk(Color color)
    {
        GameMaker.Instance.selectedColorList.Remove(frontTipColor);
        GameMaker.Instance.selectedColorList.Add(color);
        frontPenTip.color = color;
        frontTipColor = color;
    }


    public void SetCollider(bool isActive)
    {
        GetComponent<Collider>().enabled = isActive;
    }
   

}
