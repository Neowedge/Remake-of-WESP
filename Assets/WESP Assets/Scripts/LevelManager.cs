using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.IO;

namespace com.MLR.Wesp
{
    public class LevelManager : SingletonBehaviour<LevelManager>
    {
        public GameObject playerPrefab;
        public GameObject tilePrefab;

        public int randomLevelRows = 10;
        public int randomLevelRowsCols = 10;

        public float xSize = 1.2f;
        public float ySize = 1.2f;

        int levelLoaded = 0;
        char[][] levelMap = null;

        Transform player;
        Transform levelBoard = null;
        GameObject[][] levelGrid = null;
        int countTiles;

        public void Start()
        {
#if UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY
            this.playerPrefab = (GameObject)Resources.Load("Prefabs/TouchPlayer", typeof(GameObject));    
#else
            this.playerPrefab = (GameObject)Resources.Load("Prefabs/KeyboardPlayer", typeof(GameObject));       
#endif
        }

        public void StartLevel(GameManager.GameMode mode, int levelCode)
        {
            this.DestroyLevel();
            this.LoadLevel(mode, levelCode);
            this.DrawLevel();
        }

        public void RemoveTile(int x, int y)
        {
            GameObject.Destroy(this.getTile(x, y));
            this.levelGrid[y][x] = null;
            this.countTiles--;
        }

        public int CountTiles()
        {
            return this.countTiles;
        }

        public GameObject getTile(int x, int y)
        {
            if (y < 0 || y >= levelGrid.Length || x < 0 || x >= levelGrid[y].Length)
            {
                return null;
            }

            return this.levelGrid[y][x];
        }

        public int CountColumns()
        {
            return this.levelMap != null ? this.levelMap[0].Length : 0;
        }

        public int CountRows()
        {
            return this.levelMap != null ? this.levelMap.Length : 0;
        }

        public int HightlightOnlyPassableTiles(int playerX, int playerY) {
            int passableTiles = 0;
            for (int y = 0; y < this.levelGrid.Length; y++)
            {
                for (int x = 0; x < this.levelGrid[y].Length; x++)
                {
                    GameObject goTile;

                    if (this.CanMove(playerX, playerY, x, y, out goTile))
                    {
                        Tile tile = goTile.GetComponent<Tile>();
                        tile.HightlightTile(true);

                        if (x != playerX || y != playerY) {
                            passableTiles++;
                        }
                    }
                    else if (goTile != null)
                    {
                        goTile.GetComponent<Tile>().HightlightTile(false);
                    }
                }
            }

            return passableTiles;
        }

        public bool CanMove(int fromX, int fromY, int toX, int toY, out GameObject goTile)
        {
            goTile = this.getTile(toX, toY);
            if (goTile == null) {
                return false;
            }

            if ((fromX == toX && Mathf.Abs(fromY - toY) <= 2) ||
                    (fromY == toY && Mathf.Abs(fromX - toX) <= 2))
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public void UnloadLevel()
        {
            this.levelLoaded = 0;
            this.DestroyLevel();
        }

        public void HideBoard()
        {
            Renderer[] renderers = this.levelBoard.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }
        }

        public void ShowBoard()
        {
            Renderer[] renderers = this.levelBoard.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }
        }

        void DestroyLevel()
        {
            if (this.levelBoard != null)
            {
                Destroy(this.levelBoard.gameObject);
                this.levelBoard = null;
            }
        }

        void LoadLevel(GameManager.GameMode mode, int levelCode)
        {
            switch (mode)
            {
                case GameManager.GameMode.Classic:
                    ReadLevel(mode.ToString(), levelCode);
                    break;

                case GameManager.GameMode.Random:
                    RandomLevel(mode.ToString(), levelCode);
                    break;
            }
        }

        void ReadLevel(string folder, int levelCode)
        {
            if (levelCode != levelLoaded)
            {
                TextAsset levelAsset = Resources.Load(String.Format("Levels/{0}/Level_{1}", folder, levelCode)) as TextAsset;
                string[] levelRows = levelAsset.text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                this.levelGrid = new GameObject[levelRows.Length][];
                this.levelMap = new char[levelRows.Length][];

                for (int y = 0; y < levelRows.Length; y++)
                {
                    this.levelMap[y] = levelRows[y].ToCharArray();
                    this.levelGrid[y] = new GameObject[levelRows[y].Length];
                }

                this.levelLoaded = levelCode;

            }
        }

        void RandomLevel(string folder, int levelCode)
        {
            if (levelCode != levelLoaded)
            {
                this.levelGrid = new GameObject[randomLevelRowsCols][];
                this.levelMap = new char[randomLevelRowsCols][];

                for (int y = 0; y < randomLevelRowsCols; y++)
                {
                    this.levelMap[y] = new char[randomLevelRows];
                    for (int x = 0; x < randomLevelRows; x++)
                    {
                        this.levelMap[y][x] = '0';
                    }
                    this.levelGrid[y] = new GameObject[randomLevelRows];
                }

                int movements = Math.Min(80, Random.Range(5, 15) + levelCode * 2);

                int posY = Random.Range(0, randomLevelRowsCols);
                int posX = Random.Range(0, randomLevelRows);


                this.levelMap[posY][posX] = '1';
                for (int i = 0; i < movements; i++)
                {
                    List<Vector2> possibleMovements = new List<Vector2>();
                    possibleMovements.Add(new Vector2(1, 0));
                    possibleMovements.Add(new Vector2(-1, 0));
                    possibleMovements.Add(new Vector2(0, 1));
                    possibleMovements.Add(new Vector2(0, -1));
                    possibleMovements.Add(new Vector2(2, 0));
                    possibleMovements.Add(new Vector2(-2, 0));
                    possibleMovements.Add(new Vector2(0, 2));
                    possibleMovements.Add(new Vector2(0, -2));

                    int tryPosX = -1;
                    int tryPosY = -1;
                    while (possibleMovements.Count > 0)
                    {
                        int tryIndex = Random.Range(0, possibleMovements.Count);
                        Vector2 tryMovement = possibleMovements[tryIndex];
                        possibleMovements.RemoveAt(tryIndex);

                        tryPosX = posX + (int)tryMovement.x;
                        tryPosY = posY + (int)tryMovement.y;

                        if (tryPosX >= 0 && tryPosX < randomLevelRows && tryPosY >= 0 && tryPosY < randomLevelRowsCols && this.levelMap[tryPosY][tryPosX] == '0')
                        {
                            posX = tryPosX;
                            posY = tryPosY;

                            this.levelMap[posY][posX] = '2';
                            break;
                        }
                    }

                    //Si el último intento no se ha adjudicado a la posición actual, es que ninguno de los intentos ha sido válido, por tanto finaliza la creación del nivel
                    if (tryPosX != posX || tryPosY != posY)
                    {
                        break;
                    }
                }
            }
        }

        void DrawLevel()
        {
            int playerX = -1;
            int playerY = -1;
            this.levelBoard = new GameObject("LevelBoard").transform;

            this.countTiles = 0;

            float posY = 0f;
            //this.transform.Translate(new Vector3(-this.xSize * 2, 0, 0));
            for (int y = this.levelMap.Length - 1; y >= 0; y--)
            {
                float posX = 0f;
                for (int x = 0; x < this.levelMap[y].Length; x++)
                {
                    //this.levelBoard.Translate(new Vector3(-this.xSize * 2, 0, 0));
                    if (this.levelMap[y][x] == '1')
                    {
                        GameObject player = GameObject.Instantiate(this.playerPrefab, new Vector3(posX, posY, -0.05f), Quaternion.identity) as GameObject;
                        player.transform.SetParent(this.levelBoard);

                        GameObject goTile = Instantiate(this.tilePrefab, new Vector3(posX, posY), Quaternion.identity) as GameObject;
                        Tile tile = goTile.GetComponent<Tile>();
                        tile.x = x;
                        tile.y = y;

                        this.levelGrid[y][x] = goTile;
                        this.levelGrid[y][x].transform.SetParent(this.levelBoard);

                        playerX = x;
                        playerY = y;

                        this.countTiles++;

                    }
                    else if (this.levelMap[y][x] == '2')
                    {
                        GameObject goTile = Instantiate(this.tilePrefab, new Vector3(posX, posY), Quaternion.identity) as GameObject;
                        Tile tile = goTile.GetComponent<Tile>();
                        tile.x = x;
                        tile.y = y;

                        this.levelGrid[y][x] = goTile;
                        this.levelGrid[y][x].transform.SetParent(this.levelBoard);
                        this.countTiles++;

                    }
                    else
                    {
                        this.levelGrid[y][x] = null;
                    }

                    posX += xSize;
                }
                posY += ySize;
            }

            if (playerX == -1 || playerY == -1) {
                Debug.LogError("Player does not found");
            }

#if UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY
            this.HightlightOnlyPassableTiles(playerX, playerY);
#endif
        }
    }
    
}