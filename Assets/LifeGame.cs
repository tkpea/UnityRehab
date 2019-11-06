using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    
    private bool _live = false;
    private Cell[] _neighbours = new Cell[8];
    private GameObject _gameObject;
    private Renderer _renderer;
    private int _liveCount = 0;

    // コンストラクター
    public Cell(GameObject gameObject, Renderer renderer)
    {
        this._gameObject = gameObject;
        this._renderer = renderer;
        this._live = Random.Range(0.0f, 1.0f) > 0.5 ? true : false;
        this.resetLive();
        this.rendererUpdate();
    }



    public void setNeighbours(Cell[] neighbours)
    {
        _neighbours = neighbours;
    }

    public void setLiveCount()
    {
        this._liveCount = 0;
        foreach (Cell n in _neighbours)
        {
            
            if (n._live)
            {
                this._liveCount++;
            }
        }
       
    }

    public void setLive()
    {
        if (this._live)
        {
            if (this._liveCount == 2 || this._liveCount == 3)
            {
                // 生存
                this._live = true;
            }
            else
            {
                // 過疎 または　過密
                this._live = false;
            }
        }
        else
        {
            if (this._liveCount == 3)
            {
                // 誕生
                this._live = true;
            }
            else
            {
                // 死
                this._live = false;
            }
        }

        this.rendererUpdate();

    }


    private void rendererUpdate()
    {
        if (this._live)
        {
            _renderer.material.EnableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", new Color(0.6f, 1f, 0.7f));
            _renderer.material.color = new Color(0.6f, 1f, 0.7f);

        }
        else
        {
            _renderer.material.DisableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", new Color(0f, 0f, 0f));
            _renderer.material.color = Color.black;

        }
    }
    public void resetLive()
    {
        this._live = Random.Range(0.0f, 1.0f) > 0.5 ? true : false;
    }

}

public class LifeGame : MonoBehaviour
{
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private Vector2 size;
    private List<List<Cell>> cells = new List<List<Cell>>();

    // Start is called before the first frame update
    void Start()
    {
        for (var x = 0; x < size.x; x++)
        {
   
            List<Cell> cellsRows = new List<Cell>();
            for (var y = 0; y < size.y; y++)
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

                Cell co = new Cell(
                    c,
                    c.GetComponent<Renderer>()
                   );
                cellsRows.Add(co);
            }
            cells.Add(cellsRows);
        }

        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                int left = (x > 0) ? x - 1 : (int)size.x - 1;
                int right = (x + 1 < (int)size.x) ? x + 1 : 0;
                int top = (y > 0) ? y - 1 : (int)size.y - 1;
                int bottom = (y + 1 < (int)size.y) ? y + 1 : 0;
                Cell[] neighbours = {
                    cells[left][top],
                    cells[x][top],
                    cells[right][top],
                    cells[left][y],
                    cells[right][y],
                    cells[left][bottom],
                    cells[x][bottom],
                    cells[right][bottom]
                };
                cells[x][y].setNeighbours(neighbours);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        if (Input.GetKey(KeyCode.Space))
        {
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    cells[x][y].resetLive();
             
                }
            }

        }


        for (var x = 0; x < cells.Count; x++)
        {

            for (var y = 0; y < cells[x].Count; y++)
            {
                cells[x][y].setLiveCount();
            }

        }
        for (var x = 0; x < cells.Count; x++)
        {

            for (var y = 0; y < cells[x].Count; y++)
            {
                cells[x][y].setLive();
            }
        }

        sw.Stop();
        Debug.Log(sw.ElapsedMilliseconds + "ms");

    }
}
