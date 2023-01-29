using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementstateManager : MonoBehaviour
{
    public float moveSpeed = 3; 
    //プレイヤーの移動速度設定
    [HideInInspector] public Vector3 dir;
    //プレイヤーの座標取得
    float hzInput, vInput;
    //情報をUnityに転送
    CharacterController controller;
    //キャラクターコントローラーを呼び出す

    [SerializeField] float groundYoffset;
    //関数地面のレイヤーの取得
    [SerializeField] LayerMask ground;
    //地面のレイヤーを取得
    [SerializeField] float gravity = -9.81f;
    //重力の設定　地球の重力＝-9.81f
    Vector3 velocity;
    Vector3 spherePos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        Vector3 moveForward = Camera.main.transform.forward * vInput + Camera.main.transform.right * hzInput;
        moveForward.y = Camera.main.transform.right.y;
        controller.Move(moveForward * moveSpeed*Time.deltaTime);
    }

    bool IsGrounded()
        //地面と接触しているかを確かめる為の処理
    {
        //空中にいても当たり判定ある
        spherePos = new Vector3(transform.position.x,transform.position.y - groundYoffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, ground)) return true;
        return false;
    }

    void Gravity()
    //重力処理の為の関数
    {
        if(!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if(velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }
    

    private void OnDrawGizmos() 
    //当たり判定の可視化
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spherePos, controller.radius - 0.05f);    
    }
}
