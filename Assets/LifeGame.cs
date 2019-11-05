using System.Collections.Generic;
using UnityEngine;


public class LifeGame : MonoBehaviour
{
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private Vector2 size;

    private List<List<GameObject>> cells = new List<List<GameObject>>();
    private List<List<Renderer>> renderers = new List<List<Renderer>>();
    private List<List<bool>> state = new List<List<bool>>();

    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        var center = this.transform.position;
        
        var prefabSize = cellPrefab.transform.localScale;
        for(var x = 0; x < size.x; x++)
        {
            List<GameObject> cellRow = new List<GameObject>();
            List<Renderer> rendererRow = new List<Renderer>();
            List<bool> stateRow = new List<bool>();
            for(var y = 0; y < size.y; y++)
            {
                GameObject c = Instantiate(
                    cellPrefab,
                    new Vector3(
                            this.transform.position.x + (size.x / 2) - x,
                            this.transform.position.y,
                            this.transform.position.z + (size.y / 2) - y),
                    Quaternion.identity
                    );
                c.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                cellRow.Add(c);
                rendererRow.Add(c.GetComponent<Renderer>());
                stateRow.Add((Random.Range(0.0f,1.0f) > 0.5) ? true: false);
                
            }
            cells.Add(cellRow);
            renderers.Add(rendererRow);
            state.Add(stateRow);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    state[x][y] = (Random.Range(0.0f, 1.0f) > 0.5) ? true : false;
                }
            }

        }

        bool[,] nextState = new bool[(int)size.x, (int)size.y];

        for (var x = 0; x < cells.Count; x++)
        {

            for (var y = 0; y < cells[x].Count; y++)
            {

                
                if (state[x][y])
                {
                    renderers[x][y].material.EnableKeyword("_EMISSION");
                    renderers[x][y].material.SetColor("_EmissionColor", new Color(0.6f, 1f, 0.7f));
                    renderers[x][y].material.color = new Color(0.6f, 1f, 0.7f);

    }
                else
                {
                    renderers[x][y].material.DisableKeyword("_EMISSION");
                    renderers[x][y].material.SetColor("_EmissionColor", new Color(0f, 0f, 0f));
                    renderers[x][y].material.color = Color.black;

                }

                int left = (x > 0) ? x - 1 : (int)size.x - 1;
                int right = (x + 1 < (int)size.x) ? x + 1 : 0;
                int top = (y > 0) ? y - 1 : (int)size.y - 1;
                int bottom = (y + 1 < (int)size.y) ? y + 1 : 0;
                int lifeCount = 0;

                if (state[left][top])
                {
                    lifeCount++;
                }
                if (state[x][top])
                {
                    lifeCount++;
                }
                if (state[right][top])
                {
                    lifeCount++;
                }

                if (state[left][y])
                {
                    lifeCount++;
                }
                if (state[right][y])
                {
                    lifeCount++;
                }
                if (state[left][bottom])
                {
                    lifeCount++;
                }
                if (state[x][bottom])
                {
                    lifeCount++;
                }
                if (state[right][bottom])
                {
                    lifeCount++;
                }

                bool next = state[x][y];
                if (state[x][y])
                {
                    if (lifeCount == 2 || lifeCount == 3)
                    {
                        next = true;
                    }
                    else
                    {
                        next = false;
                    }

                }
                else
                {
                    if (lifeCount == 3)
                    {
                        next = true;
                    }
                    else
                    {
                        next = false;
                    }
                }

                nextState[x, y] = next;
            }

        }
        for (var x = 0; x < cells.Count; x++)
        {

            for (var y = 0; y < cells[x].Count; y++)
            {
                state[x][y] = nextState[x, y];
            }
        }
    }

}
