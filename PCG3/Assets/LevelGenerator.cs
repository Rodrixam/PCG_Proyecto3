using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    TextAsset levelTxt;
    [SerializeField] GameObject sky;

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
    //Tubería cuerpo Izq
    [SerializeField] GameObject pipeBodyL;
    //Tubería cuerpo Der
    [SerializeField] GameObject pipeBodyR;
    //Tubería cabeza Izq
    [SerializeField] GameObject pipeHeadL;
    //Tubería cabeza Der
    [SerializeField] GameObject pipeHeadR;
    //Bandera
    [SerializeField] GameObject flag;
    //Moneda
    [SerializeField] GameObject coin;
    //Bombardero cabeza
    [SerializeField] GameObject bomberHead;
    //Bombardero cuerpo
    [SerializeField] GameObject bomberBody;
    //Default
    [SerializeField] GameObject def;

    List<GameObject> levelObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    public void EmptyList()
    {
        foreach (GameObject go in levelObjects)
        {
            Destroy(go);
        }
        levelObjects = new List<GameObject>();
    }
    
    public void GenerateLevel()
    {
        int x = 0;
        int y = 14;

        int rowLength = 0;

        EmptyList();

        for (int i = 0; i < levelTxt.text.Length; i++)
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
                //Tubería Cuerpo Izq
                case '[':
                    levelObjects.Add(Instantiate(pipeBodyL, new Vector3(x, y), transform.rotation));
                    break;
                //Tubería Cuerpo Der
                case ']':
                    levelObjects.Add(Instantiate(pipeBodyR, new Vector3(x, y), transform.rotation));
                    break;
                //Tubería Cabeza Izq
                case '<':
                    levelObjects.Add(Instantiate(pipeHeadL, new Vector3(x, y), transform.rotation));
                    break;
                //Tubería Cabeza Der
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
                //Moneda
                case 'B':
                    levelObjects.Add(Instantiate(bomberHead, new Vector3(x, y), transform.rotation));
                    break;
                //Moneda
                case 'b':
                    levelObjects.Add(Instantiate(bomberBody, new Vector3(x, y), transform.rotation));
                    break;
                //Aire
                case '-':
                    break;
                //Salto de linea
                case '\n':
                    y--;
                    x = -1;
                    rowLength = 0;
                    break;
                //Otros
                default:
                    levelObjects.Add(Instantiate(def, new Vector3(x, y), transform.rotation));
                    break;
            }
            x++;
            rowLength++;
        }

        sky.transform.localScale = new Vector2(levelTxt.text.Length, 15);
        sky.transform.position = new Vector2(levelTxt.text.Length / 2 - 0.5f, 7);
    }

    public void SetLevel(TextAsset txt)
    {
        levelTxt = txt;
    }

}
