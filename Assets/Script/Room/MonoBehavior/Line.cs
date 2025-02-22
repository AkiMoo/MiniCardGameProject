using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float speed = 0.1f;
    private void Update()
    {
        if(lineRenderer != null)
        {
            //获取当前纹理偏移
            lineRenderer.material.color = Color.black;
            var offset = lineRenderer.material.mainTextureOffset;
            //通过设置线的偏移来实现动画效果
            offset.x += speed * Time.deltaTime;
            lineRenderer.material.mainTextureOffset = offset;
        }
    }
}
