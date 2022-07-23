using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DataPipeline;

public class Cursor : MonoBehaviour, IHandler<PlayerInteractionState>
{
    Image image;
    private float rotationSpeed = 100;
    private float size = 0;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false;
        GameObject.Find("Player").GetComponent<PlayerInteractionPipline>().AddHandler(this);
        image = GetComponent<Image>();
    }

    void FixedUpdate()
    {
        if (size > 0) size -= Time.deltaTime;
        else if (size < 0) size = 0;
    }

    public void Handle(in PlayerInteractionState data)
    {
        //Vector3 pos = data.sharedData.MainCamera.ScreenToWorldPoint(data.PlayerState.CursorPos);
        //transform.position = new Vector3(pos.x, 1, pos.z);
        image.rectTransform.localPosition = new Vector3(data.PlayerState.CursorPos.x - Screen.width / 2, data.PlayerState.CursorPos.y - Screen.height / 2, 1);
        transform.Rotate(0f, 0.0f, rotationSpeed * Time.deltaTime, Space.Self);

        if (data.PlayerAttackState.ShootDir != Vector3.zero)
        {
            size = 0.5f;
        }

        transform.localScale = new Vector3(1 + size, 1 + size, 1);
    }
}