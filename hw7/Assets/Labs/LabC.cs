using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 实验C：模拟单摆运动
// TODO:
// 1. 分别使用Explicit Euler, Midpoint, Trapezoid方法实现单摆模拟
// 2. 创建一个名为fixP的对象，scale设为(1, 1, 1),其位置即为单摆运动的固定点
// 3. 创建一个名为line的立方体对象，scale设为(1, length, 1)，将被作为单摆的摆线
// 4. 设置摆长和初始角度，可以修改对应参数，length和theta
// 5. 尝试不同的时间步长，修改step的值
class LabC : MonoBehaviour
{
    public GameObject fixP;
    public GameObject line;
    private float g = 9.79f;
    private Vector3 fixedPos; // position of the fixed point
    public float length = 5; // length of pendulum. you can change it!
    public float theta = -15; // initial angle, range [-90 degree, 90 degree]. you can change it!
    private float omega; // angular velocity
    private int step = 2; // time step, you can change it, too!
    private int count; // used to control the frequency of updates

    // TODO: complete the function
    void UpdatePosition_Explicit()
    {
        // the difference in angles during this time step
        float deltaTheta = 0;
        float t = step * Time.deltaTime;
        // 1. save the theta and omega of last time step for later use
        float tmpTheta = theta;
        float tmpOmega = omega;
        // 2. calculate tmp theta and omega if needed

        // 3. update theta
        theta = tmpTheta + tmpOmega * t;
        // 4. update omega
        omega = tmpOmega - t * Mathf.Sin(tmpTheta) * g / length;
        // 5. move the object to the new postion
        deltaTheta = theta - tmpTheta;
        SetPosition(deltaTheta);
    }

    // TODO: complete the function
    void UpdatePosition_Midpoint()
    {
        // the difference in angles during this time step
        float deltaTheta = 0;
        float t = step * Time.deltaTime;
        // 1. save the theta and omega of last time step for later use
        float tmpTheta = theta;
        float tmpOmega = omega;
        // 2. calculate tmp theta and omega if needed
        float midPointTheta;
        midPointTheta = tmpTheta + tmpOmega * t / 2 ;
        float midPointOmega;
        midPointOmega = tmpOmega - t / 2 * Mathf.Sin(midPointTheta) * g / length;
        // 3. update theta
        theta = tmpTheta + midPointOmega * t;
        // 4. update omega
        omega = midPointOmega - t / 2 * Mathf.Sin(midPointTheta) * g / length;
        // 5. move the object to the new postion
        deltaTheta = theta - tmpTheta;
        SetPosition(deltaTheta);
    }

    // TODO: complete the function
    void UpdatePosition_Trapezoid()
    {
        // the difference in angles during this time step
        float deltaTheta = 0;
        float t = step * Time.deltaTime;
        // 1. save the theta and omega of last time step for later use
        float tmpTheta = theta;
        float startOmega = omega;
        // 2. calculate tmp theta and omega if needed
        float endOmega;
        endOmega = startOmega - t * Mathf.Sin(tmpTheta) * g / length;
        float averageOmega = (startOmega + endOmega) / 2;
        float t1 = (averageOmega - startOmega) / (Mathf.Sin(tmpTheta) * g / length);
        float t2 = t - t1;
        float midTheta = tmpTheta + t1 * (startOmega+averageOmega)/2;
        float trueFinalOmega = averageOmega - t2 * Mathf.Sin(midTheta) * g / length;
        // 3. update theta
        theta = tmpTheta + t * averageOmega;
        // 4. update omega
        omega = trueFinalOmega;
        // 5. move the object to the new postion
        deltaTheta = theta - tmpTheta;
        SetPosition(deltaTheta);
    }

    // Start is called before the first frame update
    void Start()
    {
        // set the fixed position by gameobjtect
        fixedPos = fixP.transform.position;
        // set the initial position
        theta = theta * Mathf.Deg2Rad;
        SetPosition(theta);
        omega = 0;
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
        if (count >= step)
        {
            //UpdatePosition_Explicit();
            //UpdatePosition_Midpoint();
            UpdatePosition_Trapezoid();
            count = 0;
        }
    }

    // calculate the position according to the pendulum length, angle and fixed point
    // angle: angle of rotation since last update, unit is Rad
    void SetPosition(float angle)
    {
        Vector3 pos = fixedPos;
        pos.x = fixedPos.x + length * Mathf.Sin(theta);
        pos.y = fixedPos.y - length * Mathf.Cos(theta);
        transform.position = pos;
        transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg));
        // update position of the line
        Vector3 linePos = fixedPos;
        linePos.x = fixedPos.x + length * Mathf.Sin(theta) / 2;
        linePos.y = fixedPos.y - length * Mathf.Cos(theta) / 2;
        line.transform.position = linePos;
        line.transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg));
    }
}
