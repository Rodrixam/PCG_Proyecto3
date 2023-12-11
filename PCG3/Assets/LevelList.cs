using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    [SerializeField] List<TextAsset> list = new List<TextAsset>();
    int currentLevel = 0;

    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraObj;

    LevelGenerator generator;

    void Start()
    {
        generator = GetComponent<LevelGenerator>();
        LoadCurentLevel();
    }

    void Update()
    {
        //Pasar al siguiente nivel
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
        {
            GoNextLevel();
        }

        //Volver al nivel anterior
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.I))
        {
            currentLevel--;
            LoadCurentLevel();
        }

        //Recargar nivel
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O))
        {
            currentLevel--;
            LoadCurentLevel();
        }

    }

    public void GoNextLevel()
    {
        currentLevel++;
        if(currentLevel >= list.Count) { currentLevel = 0; }
        LoadCurentLevel();
    }

    public void LoadCurentLevel()
    {
        generator.SetLevel(list[currentLevel]);
        generator.GenerateLevel();
        player.transform.position = new Vector3(0,2,0);
        cameraObj.GetComponent<CameraFollow>().ResetCamera();
    }
}
