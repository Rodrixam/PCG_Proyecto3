using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] TextAsset levelTxt;
    [SerializeField] TextAsset levelTxt2;

    //OBJETOS
    //Ladrillo Irrompible
    [SerializeField] GameObject unbreakBrick;
    //Ladrillo Rompible
    [SerializeField] GameObject breakBrick;
    //Moneda en cubo
    [SerializeField] GameObject coinQuestion;
    //Hongo en cubo
    [SerializeField] GameObject mshrmQuestion;
    //Enemigo
    [SerializeField] GameObject enemy;
    //Tuber�a cuerpo Izq
    [SerializeField] GameObject pipeBodyL;
    //Tuber�a cuerpo Der
    [SerializeField] GameObject pipeBodyR;
    //Tuber�a cabeza Izq
    [SerializeField] GameObject pipeHeadL;
    //Tuber�a cabeza Der
    [SerializeField] GameObject pipeHeadR;
    //Bandera
    [SerializeField] GameObject flag;
    //Moneda
    [SerializeField] GameObject coin;
    //Default
    [SerializeField] GameObject def;

    List<GameObject> levelObjects;

    // Start is called before the first frame update
    void Start()
    {
        levelObjects = new List<GameObject>();
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateLevel();
        }
    }
    


    public void GenerateLevel()
    {
        int x = 0;
        int y = 14;

        for(int i = 0; i < levelTxt.text.Length; i++)
        {
            switch (levelTxt.text[i])
            {
                //Ladrillo Irrompible
                case 'X':
                    levelObjects.Add(Instantiate(unbreakBrick, new Vector3(x,y), transform.rotation));
                    break;
                //Ladrillo Rompible
                case 'S':
                    levelObjects.Add(Instantiate(breakBrick, new Vector3(x, y), transform.rotation));
                    break;
                //Moneda en cubo
                case 'Q':
                    levelObjects.Add(Instantiate(coinQuestion, new Vector3(x, y), transform.rotation));
                    break;
                //Hongo en cubo
                case '?':
                    levelObjects.Add(Instantiate(mshrmQuestion, new Vector3(x, y), transform.rotation));
                    break;
                //Enemigo
                case 'E':
                    levelObjects.Add(Instantiate(enemy, new Vector3(x, y), transform.rotation));
                    break;
                //Tuber�a Cuerpo Izq
                case '[':
                    levelObjects.Add(Instantiate(pipeBodyL, new Vector3(x, y), transform.rotation));
                    break;
                //Tuber�a Cuerpo Der
                case ']':
                    levelObjects.Add(Instantiate(pipeBodyR, new Vector3(x, y), transform.rotation));
                    break;
                //Tuber�a Cabeza Izq
                case '<':
                    levelObjects.Add(Instantiate(pipeHeadL, new Vector3(x, y), transform.rotation));
                    break;
                //Tuber�a Cabeza Der
                case '>':
                    levelObjects.Add(Instantiate(pipeHeadR, new Vector3(x, y), transform.rotation));
                    break;
                //Bandera
                case 'F':
                    levelObjects.Add(Instantiate(flag, new Vector3(x, y), transform.rotation));
                    break;
                //Moneda
                case 'o':
                    levelObjects.Add(Instantiate(coin, new Vector3(x, y), transform.rotation));
                    break;
                //Aire
                case '-':
                    break;
                //Salto de linea
                case '\n':
                    y--;
                    x = -1;
                    break;
                //Otros
                default:
                    levelObjects.Add(Instantiate(def, new Vector3(x, y), transform.rotation));
                    break;
            }
            x++;
        }
    }

}
