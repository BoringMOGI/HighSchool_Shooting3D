using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isStopController;            // �÷��̾��� ������ ���ߴ°�?

    [Header("Movement")]
    public CharacterController controller;          // ĳ���� ��Ʈ�ѷ�.
    public float moveSpeed;                         // �̵� �ӵ�.
    public float jumpHeight;                        // ������ ����.
    public AudioSource footstepSource;              // �̵� �Ҹ� ����Ŀ.
    public AudioClip footstepWalk;                  // �ȴ� �Ҹ�.
    public AudioClip footstepRun;                   // �ٴ� �Ҹ�.

    [Range(1f, 4f)]
    public float gravityScale;                      // �߷� ����.

    [Header("Ground")]
    public Transform groundChecker;                 // ������ üũ�ϴ� ��ġ
    public float groundRadius;                      // ��ŭ ũ��
    public LayerMask groundMask;                    // � ���� ������?

    [Header("Animator")]
    public Animator anim;                           // �ִϸ�����.

    [Header("Weapon")]
    public WeaponController mainGun;                // �ֹ���.
    public float zoom;                              // ���ؽ� Ȯ����.

    [Header("Pivots")]
    public Transform camEye;                        // ����ġ.
    public Transform camAimEye;                     // ������.

    float gravity = -9.81f;                         // �߷°�.
    float normalFov;                                // �⺻ �þ߰�.
    float zoomFov;                                  // Ȯ�� �þ߰�.

    Vector3 velocity;                               // �ӵ�.
    Camera cam;                                     // ī�޶�.

    bool isGrounded;                                // ���鿡 ���ִ����� ���� ����.
    bool isWalk;                                    // �Ȱ� �ִ� ���ΰ�?
    bool isRun;                                     // �ٰ� �ִ� ���ΰ�?

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

        float x = Input.GetAxisRaw("Horizontal");  // ��,�� ����Ű�� �Է� �޾ƿ´�. (��:-1, X:0, ��:1)
        float z = Input.GetAxisRaw("Vertical");    // ��,�� ����Ű�� �Է� �޾ƿ´�. (��:-1, X:0, ��:1)
        float accel = isAccel ? 2.0f : 1.0f;
        Vector3 direction = (transform.right * x) + (transform.forward * z);    // Ű �Է� > ����

        isWalk = (direction != Vector3.zero) && !isAccel;                       // �ȴ� ���ΰ�?
        isRun = (direction != Vector3.zero) && isAccel;                        // �ٴ� ���ΰ�?

        anim.SetBool("isWalk", isWalk);                                         // �ȴ� ���� ���� �Ķ���ͷ� ����.
        anim.SetBool("isRun", isRun);                                           // �ٴ� ���� ���� �Ķ���ͷ� ����.

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

        controller.Move(direction * moveSpeed * accel * Time.deltaTime);        // ���� ������
    }
    void Jump()
    {
        // ����.
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

        velocity.y += gravity * gravityScale * Time.deltaTime;                  // �߷� ���ӵ� �����ش�.
        controller.Move(velocity * Time.deltaTime);                             // �Ʒ� �������� �̵�.
    }

    void WeaponControl()
    {
        // ���.
        if(Input.GetMouseButton(0))
        {
            // ���� ó��..
            mainGun.Fire();
        }

        // ����.
        bool isAim = Input.GetMouseButton(1);
        anim.SetBool("isAim", isAim);

        Vector3 eyePosition = isAim ? camAimEye.position : camEye.position;
        float fieldOfView = isAim ? zoomFov : normalFov;

        cam.transform.position = Vector3.MoveTowards(cam.transform.position, eyePosition, 5f * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fieldOfView, 30f * Time.deltaTime);

        // ������.
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
            Gizmos.color = Color.green;     // ������ �׸��ǵ� ������ �ʷ��̴�.
            Gizmos.DrawWireSphere(groundChecker.position, groundRadius);
        }
    }
}