using System.Collections;

using System.Collections.Generic;
using UnityEngine;


public class Touchs : MonoBehaviour
{


    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  


    void Start()
    {


    }


    void Update()
    {


        //没有触摸  
        if (Input.touchCount <= 0)
        {
            return;
        }




        //单点触摸， 水平左右移动，上下旋转  
        if (1 == Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;  //自最后一帧所改变的屏幕位置
                                                     //  transform.Rotate(Vector3.down * deltaPos.x, Space.World);
                                                     // transform.Rotate(Vector3.right * deltaPos.y, Space.World);
            float x = Input.touches[0].deltaPosition.x / 2;
            float y = Input.touches[0].deltaPosition.y / 10;
            transform.Translate(new Vector3(x * Time.deltaTime, 0, 0));//相机水平左右移动


            this.transform.RotateAround(GameObject.Find("Cube").transform.position, Vector3.right, y); //相机视角上下旋转
        }





        //多点触摸, 放大缩小  
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);


        //第2点刚开始接触屏幕, 只记录，不做处理  
        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            return;
        }


        //计算老的两点距离和新的两点间距离，变大要减小view，变小要增大view  
        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

        //两个距离之差，为正表示放大手势， 为负表示缩小手势  
        float offset = newDistance - oldDistance;


        //放大因子， 一个像素按 0.1倍来算(10可调整)  
        float scaleFactor = offset / 10f;


        float Camview = this.GetComponent<Camera>().fieldOfView - scaleFactor;
        if (Camview >= 1 && Camview <= 170)
        {
            this.GetComponent<Camera>().fieldOfView = Camview;
        }


        //计算两向量之间角度，旋转相机角度


        Vector2 t1 = oldTouch1.position - oldTouch2.position;
        Vector3 t2 = oldTouch1.position - newTouch2.position;
        float a = newTouch2.position.x - oldTouch2.position.x;
        float angle = Vector3.Angle(t1, t2);


        if (a > 0)
            this.transform.RotateAround(GameObject.Find("Cube").transform.position, Vector3.down, angle);//相机绕物体cube旋转
        else
            this.transform.RotateAround(GameObject.Find("Cube").transform.position, Vector3.up, angle);

        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
        return;
    }
}