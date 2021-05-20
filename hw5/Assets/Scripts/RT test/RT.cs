using System;
using UnityEngine;

public class RT : MonoBehaviour
{
    public GameObject box;

    // Start is called before the first frame update
    void Start()
    {
        // 通常的x,y,z轴分三次旋转
        // 为什么叫通常的呢，因为不同软件同样是三次旋转不一定相同，
        // 比如顺序，相对轴在几次旋转中是不变还是随着物体转了轴也变，本地坐标还是世界坐标旋转，结果都是不同的
        // 比如下面的示例，分三次转和直接设置rotation = eulerAngles(new Vector3(50, 20, 60))结果是不同的
        Quaternion qx = Quaternion.AngleAxis(50, box.transform.right);
        box.transform.localRotation *= Euler(new Vector3(50, 0, 0));
        Quaternion qy = Quaternion.AngleAxis(20, box.transform.up);
        box.transform.localRotation *= Euler(new Vector3(0, 20, 0));
        Quaternion qz = Quaternion.AngleAxis(60, box.transform.forward);
        box.transform.localRotation *= Euler(new Vector3(0, 0, 60));

        // qxyz3与物体三次旋转后的transform.rotation相等
        // 划重点，三次旋转，可以用三个四元数相乘，
        // 但获得这三个四元数轻松，比如上面三次就每次都要获取当前物体的旋转轴，而不是用旋转开始前那个固定的旋转轴
        // 划重点，当前的四元数乘以另一个四元数，就会按这个四元数可换算出的各轴角度旋转
        // 注意以上分三次旋转，不同于直接设置rotation = eulerAngles(new Vector3(50, 20, 60))
        Quaternion qxyz3 = qz * qy * qx; // unity是z,y,z的循序相乘
        Debug.Log($"{qxyz3.ToString("f3")} <== qxyz3");
        Debug.Log($"{box.transform.rotation.ToString("f3")} <== transform.rotation");

        // 自定义eulerAngles
        Quaternion qeuler = eulerAngles(new Vector3(50, 20, 60));
        Debug.Log($"{qeuler.ToString("f3")} <== my eulerAngles");

        // 引擎的eulerAngles
        Quaternion qeuler2 = new Quaternion();
        qeuler2.eulerAngles = new Vector3(50, 20, 60);
        Debug.Log($"{qeuler2.ToString("f3")} <== unity eulerAngles");
    }

    public Quaternion eulerAngles(Vector3 eulers)
    {
        return Euler(eulers);
    }

    public Quaternion Euler(Vector3 eulers)
    {
        return FromEulerRad(eulers * Mathf.Deg2Rad);
    }

    // 将欧拉角转为弧度后计算完成旋转后的四元数
    public Quaternion FromEulerRad(Vector3 euler, string order = "ZYX")
    {
        var _x = euler.x * 0.5; // theta θ
        var _y = euler.y * 0.5; // psi ψ
        var _z = euler.z * 0.5; // phi φ

        float cX = (float)Math.Cos(_x);
        float cY = (float)Math.Cos(_y);
        float cZ = (float)Math.Cos(_z);

        float sX = (float)Math.Sin(_x);
        float sY = (float)Math.Sin(_y);
        float sZ = (float)Math.Sin(_z);

        return new Quaternion(
            sX * cY * cZ + cX * sY * sZ,
            cX * sY * cZ - sX * cY * sZ,
            cX * cY * sZ - sX * sY * cZ,
            cX * cY * cZ + sX * sY * sZ);

        // if (order == "ZXY") {
        //     return new Quaternion(
        //         cX * cY * cZ - sX * sY * sZ,
        //         sX * cY * cZ - cX * sY * sZ,
        //         cX * sY * cZ + sX * cY * sZ,
        //         cX * cY * sZ + sX * sY * cZ
        //         );
        // }

        // if (order == "XYZ") {
        //     return new Quaternion(
        //         cX * cY * cZ - sX * sY * sZ,
        //         sX * cY * cZ + cX * sY * sZ,
        //         cX * sY * cZ - sX * cY * sZ,
        //         cX * cY * sZ + sX * sY * cZ);
        // }

        // if (order == "YXZ") {
        //     return new Quaternion(
        //         cX * cY * cZ + sX * sY * sZ,
        //         sX * cY * cZ + cX * sY * sZ,
        //         cX * sY * cZ - sX * cY * sZ,
        //         cX * cY * sZ - sX * sY * cZ);
        // }

        // if (order == "ZYX") {
        //     return new Quaternion(
        //         cX * cY * cZ + sX * sY * sZ,
        //         sX * cY * cZ - cX * sY * sZ,
        //         cX * sY * cZ + sX * cY * sZ,
        //         cX * cY * sZ - sX * sY * cZ);
        // }

        // if (order == "YZX") {
        //     return new Quaternion(
        //         cX * cY * cZ - sX * sY * sZ,
        //         sX * cY * cZ + cX * sY * sZ,
        //         cX * sY * cZ + sX * cY * sZ,
        //         cX * cY * sZ - sX * sY * cZ);
        // }

        // if (order == "XZY") {
        //     return new Quaternion(
        //         cX * cY * cZ + sX * sY * sZ,
        //         sX * cY * cZ - cX * sY * sZ,
        //         cX * sY * cZ - sX * cY * sZ,
        //         cX * cY * sZ + sX * sY * cZ);
        // }
        // return new Quaternion(0,0,0,0);
    }

}