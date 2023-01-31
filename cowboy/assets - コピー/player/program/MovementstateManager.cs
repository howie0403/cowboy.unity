using System;
using UnityEngine;

namespace player.program
{
    public class MovementStateManager : MonoBehaviour
    {
        //プレイヤーの移動速度
        public float moveSpeed = 3;
        
        //プレイヤーの座標取得
        [HideInInspector] public Vector3 dir;
        
        //スティックの情報
        float hzInput, vInput;
        
        //キャラクターコントローラー
        CharacterController controller;

        //地面からの高さ
        [SerializeField] float groundYOffset;

        //地面のレイヤー情報
        [SerializeField] LayerMask ground;
        
        //重力の設定　地球の重力＝-9.81f
        [SerializeField] float gravity = -9.81f;
        
        Vector3 velocity;
        Vector3 spherePos;
        //カメラ
        [SerializeField] public Camera camera;
        //スライダー
        [SerializeField] private LayerMask slider;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            if (camera == null) camera = Camera.main;
            //キャラクターコントローラーをプログラムと紐ずける？？

        }


        void Update()
        {
            GetDirectionAndMove();
        }

        void GetDirectionAndMove()
            // 関数作成　現在のプレイヤー情報の11行目に転送
        {
            hzInput = Input.GetAxis("Horizontal");

            vInput = Input.GetAxis("Vertical");
            //プレイヤーの移動をする為の式
            //https://tech.pjin.jp/blog/2016/11/04/unity_skill_5/
            if (IsOnSphere(slider)&&!IsGrounded()) Slide();
            else
            {
                Vector3 moveForward = camera.transform.forward * vInput + camera.transform.right * hzInput;
                moveForward.y = camera.transform.right.y;
                controller.Move(moveForward.normalized * moveSpeed * Time.deltaTime);
            }
            Gravity();
        }

        bool IsGrounded()
            //地面と接触しているかを確かめる為の処理
        {
            return IsOnSphere(ground);
        }
        
        //地面がmaskに接しているか
        bool IsOnSphere(LayerMask mask)
        {
            spherePos = new Vector3(transform.position.x, transform.position.y- groundYOffset, transform.position.z);
            if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, mask)) return true;
            return false;
        }

        //スライダーに乗ったときの移動
        //跳ねる
        void Slide()
        {
            Vector3 moveForward = camera.transform.forward * vInput + camera.transform.right * hzInput;
            float b = transform.position.y;
            controller.Move(moveForward.normalized*2/3 * moveSpeed * Time.deltaTime);
            if (transform.position.y < b)
            {
                moveForward.y +=gravity*Time.deltaTime*10;//10でなくてもよい
                controller.Move(moveForward.normalized*3/2 * moveSpeed * Time.deltaTime);
            }
        }

        void Gravity()
            //重力処理の為の関数
        {
            if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
            else if (velocity.y < 0) velocity.y = -2;
            controller.Move(velocity * Time.deltaTime);
        }


        private void OnDrawGizmos()
            //当たり判定の可視化
        {
            if (controller == null) controller = GetComponent<CharacterController>();
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spherePos, controller.radius - 0.05f);
        }
    }
}