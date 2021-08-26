using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isStopController;            // 플레이어의 조작을 멈추는가?

    [Header("Movement")]
    public CharacterController controller;          // 캐릭터 컨트롤러.
    public float moveSpeed;                         // 이동 속도.
    public float jumpHeight;                        // 점프의 높이.
    public AudioSource footstepSource;              // 이동 소리 스피커.
    public AudioClip footstepWalk;                  // 걷는 소리.
    public AudioClip footstepRun;                   // 뛰는 소리.

    [Range(1f, 4f)]
    public float gravityScale;                      // 중력 비율.

    [Header("Ground")]
    public Transform groundChecker;                 // 땅인지 체크하는 위치
    public float groundRadius;                      // 얼만큼 크게
    public LayerMask groundMask;                    // 어떤 것을 땅으로?

    [Header("Animator")]
    public Animator anim;                           // 애니메이터.

    [Header("Weapon")]
    public WeaponController mainGun;                // 주무기.
    public float zoom;                              // 조준시 확대율.

    [Header("Pivots")]
    public Transform camEye;                        // 눈위치.
    public Transform camAimEye;                     // 조준점.

    float gravity = -9.81f;                         // 중력값.
    float normalFov;                                // 기본 시야각.
    float zoomFov;                                  // 확대 시야각.

    Vector3 velocity;                               // 속도.
    Camera cam;                                     // 카메라.

    bool isGrounded;                                // 지면에 서있는지에 대한 여부.
    bool isWalk;                                    // 걷고 있는 중인가?
    bool isRun;                                     // 뛰고 있는 중인가?

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;

        normalFov = cam.fieldOfView;
        zoomFov = normalFov - zoom;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();

        if (isStopController != true)
        {
            Movement();
            Jump();
            WeaponControl();
        }

        Gravity();
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundRadius, groundMask);
    }
    void Movement()
    {
        bool isAccel = Input.GetKey(KeyCode.LeftShift);

        float x = Input.GetAxisRaw("Horizontal");  // 좌,우 방향키를 입력 받아온다. (좌:-1, X:0, 우:1)
        float z = Input.GetAxisRaw("Vertical");    // 상,하 방향키를 입력 받아온다. (하:-1, X:0, 상:1)
        float accel = isAccel ? 2.0f : 1.0f;
        Vector3 direction = (transform.right * x) + (transform.forward * z);    // 키 입력 > 방향

        isWalk = (direction != Vector3.zero) && !isAccel;                       // 걷는 중인가?
        isRun = (direction != Vector3.zero) && isAccel;                        // 뛰는 중인가?

        anim.SetBool("isWalk", isWalk);                                         // 걷는 상태 값을 파라미터로 전달.
        anim.SetBool("isRun", isRun);                                           // 뛰는 상태 값을 파라미터로 전달.

        if (isWalk)
        {
            footstepSource.clip = footstepWalk;
            if(footstepSource.isPlaying == false)
                footstepSource.Play();
        }
        else if (isRun)
        {
            footstepSource.clip = footstepRun;
            if (footstepSource.isPlaying == false)
                footstepSource.Play();
        }
        else
        {
            footstepSource.Stop();
        }

        controller.Move(direction * moveSpeed * accel * Time.deltaTime);        // 실제 움직임
    }
    void Jump()
    {
        // 점프.
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity * gravityScale);
        }
    }
    void Gravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * gravityScale * Time.deltaTime;                  // 중력 가속도 더해준다.
        controller.Move(velocity * Time.deltaTime);                             // 아래 방향으로 이동.
    }

    void WeaponControl()
    {
        // 사격.
        if(Input.GetMouseButton(0))
        {
            // 공격 처리..
            mainGun.Fire();
        }

        // 조준.
        bool isAim = Input.GetMouseButton(1);
        anim.SetBool("isAim", isAim);

        Vector3 eyePosition = isAim ? camAimEye.position : camEye.position;
        float fieldOfView = isAim ? zoomFov : normalFov;

        cam.transform.position = Vector3.MoveTowards(cam.transform.position, eyePosition, 5f * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fieldOfView, 30f * Time.deltaTime);

        // 재장전.
        if(Input.GetKeyDown(KeyCode.R))
        {
            mainGun.Reload();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            mainGun.MeleeAttack();
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            mainGun.GrenadeThrow();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundChecker != null)
        {
            Gizmos.color = Color.green;     // 도형을 그린건데 색상은 초록이다.
            Gizmos.DrawWireSphere(groundChecker.position, groundRadius);
        }
    }
}