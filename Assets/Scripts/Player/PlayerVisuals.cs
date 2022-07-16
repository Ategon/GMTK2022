using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private SpriteRenderer upper;
    private SpriteRenderer lower;
    private Animator upperAnim;
    private Animator lowerAnim;

    void Start()
    {
        upper = transform.Find("Player Upper").GetComponent<SpriteRenderer>();
        lower = transform.Find("Player Lower").GetComponent<SpriteRenderer>();
        upperAnim = upper.gameObject.GetComponent<Animator>();
        lowerAnim = lower.gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Run();
    }

    void Run()
    {
        lowerAnim.Play("clover-walk-lower");
        upperAnim.Play("clover-walk");
    }

    void Idle()
    {
        lowerAnim.Play("clover-idle-lower");
        upperAnim.Play("clover-idle");
    }

    //upper.flipX = false;
    //lower.flipX = false;

    //lowerAnim.Play("clover-charge");
    //lowerAnim.Play("clover-hold");
    //lowerAnim.Play("clover-release");
}
