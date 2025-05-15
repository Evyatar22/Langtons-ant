using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameManger : MonoBehaviour
{
    [SerializeField] private Image antPrefab;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private int antAmount = 1;
    [SerializeField] private Color tileColor;

    [SerializeField] private bool baseLongtan = false;
    private TableGenrator genrator;

    Ant[] ants;
    Image[,] tileMap;

    private void Start()
    {
        genrator = GameObject.Find("TableGenrator").GetComponent<TableGenrator>();

        tileColor = genrator.tileColor;
        tileMap = genrator.GetTileData();


        ants = new Ant[antAmount];
        Vector3 tmpPos;
        RectTransform tmpTr;
       
        for (int i = 0; i < ants.Length; i++)
        {
            int tilex = (tileMap.GetLength(0) / 2);
            int tiley = (tileMap.GetLength(1) / 2);

            tmpPos = tileMap[tilex, tiley].GetComponent<RectTransform>().position;

       

            ants[i] = Instantiate(antPrefab, 
                tmpPos, transform.rotation).GetComponent<Ant>();

            int[] pos = new int[] {tilex,tiley};
            ants[i].SetPos(pos);
            ants[i].SetNum(i);
            ants[i].colorSequence[0] = tileColor;

            tmpTr = ants[i].GetComponent<RectTransform>();
            tmpTr.sizeDelta = new Vector2(tileMap[0,0].GetComponent<RectTransform>().sizeDelta.x * 30, tileMap[0, 0].GetComponent<RectTransform>().sizeDelta.y / 2);
            tmpTr.SetParent(canvas);
        }
    }
    public void GiveNewPos(Ant ant)
    {
        int tilex = Random.Range(0, tileMap.GetLength(0));
        int tiley = Random.Range(0, tileMap.GetLength(1));

        int[] pos = new int[] { tilex, tiley };
        ant.SetPos(pos);
    }

    void FixedUpdate()
    {
       
       for(int i = 0; i < ants.Length; i++)
       {
           ants[i].NextMove();
       }
    }


    public void UpdateImage(Color[] antColor,Ant ant)
    {
        if (baseLongtan)
        {
            float newAngle = ant.angle;
            int[] newPos = ant.position;

            if (tileMap[newPos[0], newPos[1]].color == tileColor)
            {
                tileMap[newPos[0], newPos[1]].color = antColor[1];
                newAngle -= 90f;
            }
            else
            {
                tileMap[newPos[0], newPos[1]].color = tileColor;
                newAngle += 90f;
            }




            ant.SetAngle(newAngle);
            newPos = FindIndex2(ant);
            ant.SetPos(newPos);
            ant.GetComponent<RectTransform>().position = tileMap[newPos[0], newPos[1]].rectTransform.position;
        }
        else
        {
            float newAngle = ant.angle;
            int[] newPos = ant.position;

            for(int i = 0; i < antColor.Length; i++)
            {
                if(tileMap[newPos[0], newPos[1]].color == antColor[i])
                {
                    if(i == antColor.Length - 1)
                    {
                        tileMap[newPos[0], newPos[1]].color = antColor[0];
                        newAngle += ant.anglesAddition[i];
                        break;
                    }
                    tileMap[newPos[0], newPos[1]].color = antColor[i + 1];
                    newAngle += ant.anglesAddition[i]; 
                    break;
                }
            }
            




            ant.SetAngle(newAngle);
            newPos = FindIndex2(ant);
            ant.SetPos(newPos);
            ant.GetComponent<RectTransform>().position = tileMap[newPos[0], newPos[1]].rectTransform.position;
        }


        
    }
    
    private int[] FindIndex2(Ant ant)
    {

        float min = float.MaxValue;

        int[] index = new int[2];
        Vector2 tmpPos = ant.pointer.position;
        
        for(int x = ant.position[0] - 1; x <= ant.position[0] + 1; x++)
        {
            for(int y = ant.position[1] - 1; y <= ant.position[1] + 1; y++)
            {
                if(y ==  ant.position[0] &&  x == ant.position[1]) { continue; }

                float dis = Vector2.Distance(tmpPos, tileMap[x, y].rectTransform.position);
                if(dis < min)
                {
                    min = dis;
                    index[0] = x;
                    index[1] = y;
                }
            }
        }

        return index;

    }


}
