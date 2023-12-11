using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    [SerializeField] List<TextAsset> list = new List<TextAsset>();
    int currentLevel = 0;

    [SerializeField] GameObject player;

    LevelGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = new LevelGenerator();
        LoadCurentLevel();
    }

    // Update is called once per frame
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
        LoadCurentLevel();
    }

    public void LoadCurentLevel()
    {
        generator.SetLevel(list[currentLevel]);
        generator.GenerateLevel();
        player.transform.position = new Vector3(0,2,0);
    }
}
