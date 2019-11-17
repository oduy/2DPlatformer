using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;


namespace ODUY
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    public class MainPlayer : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Field
        [Header("Main Character Detail")]
        [SerializeField] protected float m_moveSpeed;
        [SerializeField] protected float m_moveHorizontal;
        [SerializeField] protected float m_moveVertical;
        [SerializeField] protected bool m_isFacingRight = true;

        [Header("Main Character References")]
        [SerializeField] protected Rigidbody2D m_myBody2D;
        [SerializeField] protected Animator m_myAni;
		public float checkRadius = 0.2f;
        public LayerMask groundPlayer;

        public float jumpForce;
        public Transform groundPos;
		
		[Header("Main Character Shoot")]
		[SerializeField] private GameObject m_bullet = default;
		[SerializeField] private Transform m_fireTransform = default;
		[SerializeField] private float m_speedBullet = 5f;

        private Vector3 vectorMovement;
        private bool isGrounded;

        #endregion


        #region Init
        public static GameObject LocalPlayerInstance;


        #endregion

        #region UNITY

        private void Awake()
        {
            if(photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }
        private void Start()
        {
            m_myBody2D = GetComponent<Rigidbody2D>();
            m_myAni = GetComponent<Animator>();

            CameraFollow m_camera = Camera.main.GetComponent<CameraFollow>();
            if(m_camera != null)
            {
                if(photonView.IsMine)
                {
                    m_camera.OnStartFollowing(this.gameObject);
                }
            }

        }

        private void Update()
        {
            if(photonView.IsMine)
            {
                PlayerShoot();
            }
                
        }

        private void FixedUpdate()
        {
            if(photonView.IsMine)
            {
                InputPlayer();

                if (m_moveHorizontal > 0 && m_isFacingRight)
                {
                    Flip();
                }
                else if (m_moveHorizontal < 0 && !m_isFacingRight)
                {
                    Flip();
                }
            }
            
        }
        #endregion

        private void InputPlayer()
        {

            //run on window
            m_moveHorizontal = Input.GetAxisRaw("Horizontal");
            m_moveVertical = Input.GetAxisRaw("Vertical");
            m_myBody2D.velocity = new Vector2(m_moveHorizontal * m_moveSpeed, m_myBody2D.velocity.y);

            if(m_moveHorizontal != 0)
            {
                m_myAni.SetBool("isWalk", true);
            }
            else
            {
                m_myAni.SetBool("isWalk", false);
            }

            //jump
            isGrounded = Physics2D.OverlapCircle(groundPos.position, checkRadius, groundPlayer);
            if (isGrounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) )
            {
                isGrounded = false;
                m_myBody2D.velocity = Vector2.up * jumpForce * Time.deltaTime;
                m_myAni.SetBool("isJump", true);
            }
            else
            {
                m_myAni.SetBool("isJump", false);
            }



            //run on mobile
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 touchPosition = Input.mousePosition;

                if (touchPosition.x <= (Screen.width / 2))
                {
                    //Debug.Log("Left");
                }
                else if ((touchPosition.x > Screen.width / 2) )
                {
                    //Debug.Log("Right" );
                }

            }

        }

        private void Flip()
        {
            m_isFacingRight = !m_isFacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

        }

		
		private void PlayerShoot()
		{
			if(Input.GetMouseButtonDown(0))
            {
                m_myAni.SetBool("isAttack", true);

                GameObject bullet = Instantiate(m_bullet, m_fireTransform.position, m_fireTransform.rotation);
                Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();

                if (!m_isFacingRight)
                {
                    rb2D.AddForce(transform.right * m_speedBullet);
                }
                else
                {
                    rb2D.AddForce(-transform.right * m_speedBullet);
                }
            }
            
		}

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }

        public void TurnOffAttackAnimation()
        {
            m_myAni.SetBool("isAttack", false);
        }
    }

}
