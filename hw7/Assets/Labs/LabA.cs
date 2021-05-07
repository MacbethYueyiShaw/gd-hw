using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 实验A：模拟自由落体运动-Explicit Euler方法
// TODO: 
// 1. 参考注释补全代码，用Explicit Euler方法模拟自由落体和反弹运动
// 2. 尝试不同大小的时间步长，通过修改step的值来控制步长
// 3. 不同时间步长之间进行对比，以及与解析式方式对比
class LabA : MonoBehaviour
{
    private double g = 9.79;
    public double height;
    private float x;
    private float z;
    public double v;

    private int step = 2; // You can change this!
    private int count; // used to control the frequency of updates

    // TODO: complete the function with Explicit Euler method
    void UpdateHeight()
    {
        // 1. update height, move at speed of v for one time step
        double t = step * Time.deltaTime;
        height +=  v * t;
        // 2. calculate v in the next time step
        v -= g * t;
        // 3. change direction if needed, reset initial values
        if (height<= transform.localScale.y / 2)
        {
            v = -v;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // set initial values
        height = transform.position.y;
        x = transform.position.x;
        z = transform.position.z;
        v = 0;
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
        if (count >= step)
        {
            // calculate new position
            UpdateHeight();
            // set new position
            transform.position = new Vector3(x, (float)height, z);
            count = 0;
        }
    }

}
