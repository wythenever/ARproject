using UnityEngine;
using System.Collections;
public class Drawcard : MonoBehaviour
{
    public Transform GreatTransfrom;                  //脱卡后最佳位置
    public GameObject cubePos;                              //模型脱卡时存放位置
    public Transform[] TargetTransfrom;              //模型在卡片上的最佳位置
    public GameObject[] Target;                          //卡片
    public GameObject[] cube;                            //模型

    GameObject musicPlayer;
    public GameObject musicPlayerPrefab;
    private bool ifcheck = true;
    void Start()
    {
        for (int i = 0; i < cube.Length; i++)
        {          //所有模型初始化全部不显示
            cube[i].SetActive(false);
        }
    }
    public void tiaozheng()                              //模型倾斜时调整最佳位置
    {
        GreatTransfrom.localPosition = new Vector3(0f, 0f, 0f);
        GreatTransfrom.localRotation = Quaternion.identity;
        for (int i = 0; i < cube.Length; i++)
        {
            cube[i].transform.localPosition = GreatTransfrom.localPosition;
            cube[i].transform.localRotation = GreatTransfrom.localRotation;
        }
    }
    void Update()
    {
        WhoShouldShow();                            //哪个模型应该显示
        TargetT();                                          //有卡片时
        TargetF();                                        //无卡片时
        //tiaozheng();
    }
    int index = -1;
    void WhoShouldShow()                      //哪个模型应该显示
    {
        for (int i = 0; i < Target.Length; i++)
        {
            if (Target[i].activeSelf == true)
            {
                cube[i].SetActive(true);
                index = i;
            }
            if (i != index)
            {
                cube[i].SetActive(false);
            }
        }
    }
    void TargetT()                                    //不脱卡
    {
        for (int i = 0; i < Target.Length; i++)
        {
            if (Target[i].activeSelf == true)
            {
                //cube[i].transform.parent = Target[i].transform;
                //cube[i].transform.position = TargetTransfrom[i].position;
                cube[i].transform.rotation = TargetTransfrom[i].rotation;
            }
        }
    }
    void TargetF()                                    //脱卡
    {
        for (int i = 0; i < Target.Length; i++)
        {
            if (Target[i].activeSelf == false)
            {
                cube[i].transform.parent = cubePos.transform;
                cube[i].transform.rotation = GreatTransfrom.rotation;
                cube[i].transform.localPosition = GreatTransfrom.localPosition;
            }

            if (Target[i].activeSelf == true && ifcheck == true)
          {
                musicPlayer = (GameObject)Instantiate(musicPlayerPrefab);
              StartMusic();
             ifcheck = false;

           }
        }
    }

    public void StartMusic()
    {

        foreach (var source in musicPlayer.GetComponentsInChildren<AudioSource>())
            source.Play();
    }
}
