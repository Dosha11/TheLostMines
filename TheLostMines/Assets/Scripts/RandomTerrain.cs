using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rnd = UnityEngine.Random; //СОЗДАЕМ ПСЕВДОНИМ КЛАССА Random, ЧТОБЫ МОЖНО БЫЛО К НЕМУ ПРОЩЕ ОБРАЩАТЬСЯ!!!

public class RandomTerrain : MonoBehaviour
{


    [SerializeField] private List<GroundLout> groundList;

    private Terrain terrain; //ссылка на террейн

    [SerializeField] private GameObject _player;

    private List<Height> _elevationHeights = new List<Height>();
    private List<Height> _lowlandHeights = new List<Height>();
    [SerializeField] private List<Height> _plainHeights = new List<Height>();

    #region Terrain
    public int terrainWidth = 1024; //ширина террейна
    public int terrainLength = 1024; //длина террейна
    public int terrainHeightPlain; //высота террейна
    public int terrainHeight; //высота террейна

    public float scale = 10; //масштаб

    private float offsetX = 0; //смещение шума по X
    private float offsetZ = 0; //смещение шума по Z

    public int chunkWidth = 256; //размер чанка
    public int chunkLength = 256; //размер чанка

    private int chunkColumns = 0; //Количество чанков (клеток) на террейне. Один чанк = один остров
    private int chunkRows = 0; //Количество чанков (клеток) на террейне. Один чанк = один остров
    #endregion

    #region Biomes
    [Serializable] //Структура биомов
    public struct Biome
    {
        public Color color; //каким цветом красить биом
        public float height; // на какой высоте начинается биом
    }

    public Biome[] biomes; //массив биомов
    #endregion
    
    #region Water
    public Transform water; //объект воды

    public float waterLevel;//Уровень воды
    #endregion

    #region Trees
    public GameObject treePref; //префаб дерева
    #endregion

    #region Village
    public GameObject _village;
    #endregion

    #region Cave
    public GameObject _cave;
    #endregion


    #region Elementary
    public GameObject _stone;
    public GameObject _bush;
    public GameObject _branch;
    #endregion

    #region Ores
    public GameObject _coal; //Уголь - низины
    public GameObject _clate; //Сланец - возвышенность
    public GameObject _basalt; //Базальт - низины
    public GameObject _marble; //Мрамор - возвышенности
    #endregion

    private void Start()
    {
        //Сохраняем ссылку на компонент террейна
        terrain = GetComponent<Terrain>();

        Generate();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Console.Clear();
            SceneManager.LoadScene(0);
        }
    }

    #region Terrain
    public void Generate()
    {
        //У архипелага количество чанков зависит от размера всего террейна и размеров чанка
        //Если размер террейна равен размеру чанка = будет один остров
        //Сам террейн можно представить как двумерный массив или сетку
        chunkColumns = terrainWidth / chunkWidth; //количество столбцов - чанков по горизонтали
        chunkRows = terrainLength / chunkLength; //количество рядов - чанков по вертикали

        //Генерируем ландшафт
        GenerateTerrain();
        //Красим ландашфт
        PaintTerrain();
        //Устанавливаем воду
        SetWater();
        //Генерируем деревья
        GenerateTrees();
        //Телепортируем игрока
        SetPlayerPosition();
        ////Генерация руды
       // GenerateOres();
    }

    public void GenerateTerrain()
    {
        var data = terrain.terrainData;
        data.heightmapResolution = terrainWidth + 1;
        data.alphamapResolution = terrainWidth + 1;
        data.size = new Vector3(terrainWidth, terrainHeight, terrainLength);

        //Генерация равнины
        float[,] elevations = GenerateElevationsPlain();

        data.SetHeights(0, 0, elevations);

        //Генерация чанков с горами и низменностями
        float[,] elevationsR = GenerateElevationsRock();

        //Соединение
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int y = 0; y < terrainLength; y++)
            {
                elevationsR[x, y] += elevations[x, y];
            }
        }

        data.SetHeights(0, 0, elevationsR);
    }

    public float[,] GenerateElevationsPlain()
    {
        float[,] elevations = new float[terrainWidth, terrainLength];

        //пробегаемся циклами по каждому чанку (у пангеи один чанк, у архипелага их несколько)
        for (int c = 0; c < 1; c++)
        {
            for (int r = 0; r < 1; r++)
            {
                offsetX = Rnd.Range(0, 5000);
                offsetZ = Rnd.Range(0, 5000);

                //Пробегаемся по каждой клетке внутри чанка
                for (int x = 0; x < terrainWidth; x++)
                {
                    for (int z = 0; z < terrainLength; z++)
                    {
                        //генерируем высоту для этой клетки, передавая размер чанка и координаты клетки
                        float elevation = GenerateElevationPlain(x, z, terrainWidth, terrainLength);
                        //Высоту сохраняем в массив высот
                        elevation -= 0.5f;
                        elevations[c * terrainWidth + x, r * terrainLength + z] = elevation;
                    }
                }
            }
        }

        return elevations;
    }

    public float[,] GenerateElevationsRock()
    {
        float[,] elevations = new float[terrainWidth, terrainLength];

        int countPit = 0;

        int countRock = 0;

        int countVillage = 0;

        int countCave = 0;

        while (countVillage == 0)
        {
            //пробегаемся циклами по каждому чанку (у пангеи один чанк, у архипелага их несколько)
            for (int c = 0; c < chunkColumns; c++)
            {
                for (int r = 0; r < chunkRows; r++)
                {
                    //шанс генерации острова, если случайное число больше чем количество колон.
                    //Пангея дает имеет шанс 100%, потому что произведение колон и строк дает 1
                    //Генерация между 0 и 1 всегда дает 0 -> 0 > 1 -> условие не выполняется
                    if (countVillage == 0 && Rnd.Range(0, 100) <= 50)
                    {
                        Instantiate(_village, new Vector3(r * (chunkLength) + (chunkLength / 2), terrain.terrainData.GetHeight(r * (chunkLength) + (chunkLength / 2), c * (chunkWidth) + (chunkWidth / 2)), c * (chunkWidth) + (chunkWidth / 2)), Quaternion.identity);
                        countVillage++;
                    }
                    else if (countPit > countRock)
                    {
                        //Генерируем случайный отступ шума, чтобы не было повторений в островах
                        offsetX = Rnd.Range(0, 5000);
                        offsetZ = Rnd.Range(0, 5000);

                        //Пробегаемся по каждой клетке внутри чанка
                        for (int x = 0; x < chunkWidth; x++)
                        {
                            for (int z = 0; z < chunkLength; z++)
                            {
                                //генерируем высоту для этой клетки, передавая размер чанка и координаты клетки
                                float elevation = GenerateElevation(x, z, chunkWidth, chunkLength);
                                //Высоту сохраняем в массив высот
                                elevations[c * chunkWidth + x, r * chunkLength + z] = elevation;
                                _elevationHeights.Add(new Height { X = c * chunkWidth + x, Y = r * chunkLength + z, Elevation = elevation });
                            }
                        }
                        countRock++;
                    }
                    else if (Rnd.Range(0, chunkColumns * chunkRows) > chunkColumns + 6)
                    {
                        offsetX = Rnd.Range(0, 5000);
                        offsetZ = Rnd.Range(0, 5000);

                        //Пробегаемся по каждой клетке внутри чанка
                        for (int x = 0; x < chunkWidth; x++)
                        {
                            for (int z = 0; z < chunkLength; z++)
                            {
                                //генерируем высоту для этой клетки, передавая размер чанка и координаты клетки
                                float elevation = GenerateElevation(x, z, chunkWidth, chunkLength);
                                //Высоту сохраняем в массив высот
                                elevation *= -1;
                                elevations[c * chunkWidth + x, r * chunkLength + z] = elevation;
                                _lowlandHeights.Add(new Height { X = c * chunkWidth + x, Y = r * chunkLength + z, Elevation = elevation });
                            }
                        }
                        countPit++;
                    }
                    else
                    {
                        for (int x = 0; x < chunkWidth; x++)
                        {
                            for (int z = 0; z < chunkLength; z++)
                            {
                                _plainHeights.Add(new Height { X = r * chunkLength + z, Y = c * chunkWidth + x, Elevation = terrain.terrainData.GetHeight(c * chunkWidth + x, r * chunkLength + z) });
                            }
                        }

                        //Instantiate(_cave, new Vector3(r * (chunkLength) + (Rnd.Range(20, chunkLength-20)), terrain.terrainData.GetHeight(r * (chunkLength) + (Rnd.Range(20, chunkLength - 20)), c * (chunkWidth) + (Rnd.Range(20, chunkLength -20))), c * (chunkWidth) + (Rnd.Range(20, chunkLength - 20))), Quaternion.identity);
                        //_cave.GetComponent<CaveView>().Key = countCave;
                        countCave++;
                    }
                }
            }
        }
        return elevations;
    }

    public float GenerateElevationPlain(int x, int z, int w, int l)
    {
        float nx = (2f * (float)x / w - 2f); //нормализация X ЗАПОМНИТЬ ФОРМУЛУ
        float nz = (2f * (float)z / l - 2f); //нормализация Z ЗАПОМНИТЬ ФОРМУЛУ

        nx *= scale-1; //скалирование нормализированого X
        nz *= scale-1; //скалирование нормализированого Z

        nx += offsetX; //Смещение по X для неповторимости шума
        nz += offsetZ; //Смещение по Z для неповторимости шума

        float e = Mathf.PerlinNoise(nx, nz); //генерация шума

        e += 0.6f;

        return e;
    }

    public float GenerateElevation(int x, int z, int w, int l)
    {
        float nx = (4f * (float)x / w - 2f); //нормализация X ЗАПОМНИТЬ ФОРМУЛУ
        float nz = (4f * (float)z / l - 2f); //нормализация Z ЗАПОМНИТЬ ФОРМУЛУ

        float d = Mathf.Min(1f, (nx * nx + nz * nz) / Mathf.Sqrt(2f)); //формула дистанции от краев ЗАПОМНИТЬ ФОРМУЛУ

        nx *= scale; //скалирование нормализированого X
        nz *= scale; //скалирование нормализированого Z

        nx += offsetX; //Смещение по X для неповторимости шума
        nz += offsetZ; //Смещение по Z для неповторимости шума

        float e = Mathf.PerlinNoise(nx, nz); //генерация шума

        e = Mathf.Lerp(e, 1f - d * 1.5f, 0.5f);//Округления для острова ЗАПОМНИТЬ ФОРМУЛУ

        return e;
    }
    #endregion

    public void PaintTerrain()
    {
        var data = terrain.terrainData;
        //Создаем новую текстуру для террейна
        Texture2D tex = new Texture2D(terrainWidth, terrainLength);

        //пробегаемся по каждой клетке Террейна по X, Z кооридинатам
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int z = 0; z < terrainLength; z++)
            {
                //Выясняем высоту террейна на клетке террейна
                var elevation = data.GetHeight(x, z);
                //Высчитываем температуру клетки. Где полюса, там должно быть холодно, на экваторе - тепло.
                //Данную формулу достаточно запомнить.
                var temperature = Mathf.Abs(4 * (float)z / terrainLength - 2);

                //Просматриваем каждый биом
                foreach (var b in biomes)
                { 
                    //проверяем что текущая высота клетки больше или равна высоте биома
                    if (elevation >= b.height)
                    {
                        //если условие выполняется, красим клетку текстуры в цвет биома
                        //tex.SetPixel(x, z, Color.Lerp(b.color, Color.white, temperature));
                        tex.SetPixel(x, z, b.color);
                    }
                }
            }
        }
        //!!!ЭТО НУЖНО СДЕЛАТЬ ОБЯЗАТЕЛЬНО!!!
        //Принимает изменения текстуры
        tex.Apply();
        //Текстуру присваиваем материалу террейна
        terrain.materialTemplate.mainTexture = tex;
    }

    public void SetWater()
    {
        //Устанавливаем позицию по центру террейна и на высоте ватерлинии
        water.position = new Vector3(terrainWidth / 2, waterLevel, terrainLength / 2);
        //Размер растягиваем по размерам террейна
        water.localScale = new Vector3(terrainWidth, terrainLength, 1);
        //Квад разворачиваем по Х на 90 градусов, чтобы он был горизонтальный
        water.rotation = Quaternion.Euler(90, 0, 0);
    }


    public void GenerateTrees()
    {
        var data = terrain.terrainData;
        int index=0;
        for (int i = 0; i < _plainHeights.Count; i++)
        {
            //Выясняем высоту террейна на клетке террейна
            var elevation = data.GetHeight(_plainHeights[i].X, _plainHeights[i].Y);
            Vector3 pos = new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y);
            for (int w = 0; w < groundList.Count; w++)
            {
                GroundLout groundLout = groundList[w];

                if (elevation > groundLout.minRange && elevation < groundLout.maxRange)
                {
                    if (Utility.Chance(groundList[w].chance))
                    {
                        
                        GameObject clone = Instantiate(groundLout.createObject, pos, Quaternion.identity, terrain.transform);
                        clone.transform.rotation = Utility.RotationSystem(groundLout.randomRotateX,groundLout.randomRotateY, groundLout.randomRotateZ);
                        clone.transform.localScale = Utility.RandomScale(groundLout.randomScaleX, groundLout.randomScaleY, groundLout.randomScaleZ);

                        if (groundLout.index)
                        {
                           
                            clone.GetComponent<CaveView>().Key = index;
                            index++;
                        }
                    }
                }
            }

        }
    }

 



    //public void GenerateTrees()
    //{
    //    var data = terrain.terrainData;

    //    for (int i = 0; i < _plainHeights.Count; i++)
    //    {
    //        //Выясняем высоту террейна на клетке террейна
    //        var elevation = data.GetHeight(_plainHeights[i].X, _plainHeights[i].Y);

    //        ////Проверяем что высота ниже или ровна уровню воды - если да, то пропускаем итерацию 
    //        //if (elevation <= biomes[8].height) continue;
    //        ////Проверяем что высота выше или ровна уровню гор (второй биом) - если да, то пропускаем итерацию 
    //        //if (elevation >= biomes[11].height) continue;
    //        //шанс появления дерева на клетки 1%
    //        if (elevation >= biomes[9].height && elevation <= biomes[10].height)
    //        {
    //            if (Rnd.Range(0, 100) <= 3)
    //            {
    //                Instantiate(treePref, new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y), Quaternion.identity, terrain.transform);//Деревья 
    //            }
    //            else
    //            {
    //                if (Rnd.Range(0, 100) <= 3)
    //                {
    //                    Instantiate(_branch, new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y), Utility.RotationSystem(true,true), terrain.transform);//Палки
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (Rnd.Range(0, 2000) <= 1)
    //            {
    //                Instantiate(treePref, new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y), Quaternion.identity, terrain.transform);
    //            }
    //            else 
    //            {
    //                if (Rnd.Range(0, 100) <= 3)
    //                {
    //                    Instantiate(_branch, new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y), Utility.RotationSystem(true, true), terrain.transform);
    //                }
    //            }
    //        }
    //        if (elevation >= biomes[1].height) 
    //        {
    //            if (Rnd.Range(0, 100) <= 3)
    //            {
    //                Debug.Log(elevation);
    //                Instantiate(_bush, new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y), Quaternion.identity, terrain.transform);//Деревья 
    //            }
    //        }
    //        //Если не пропустили итерацию - ставим дерево на X,Z клетки. Y позиция - высота террейна для этой клетке.
    //       // Instantiate(treePref, new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y), Quaternion.identity, terrain.transform);
    //    }
    //}

    public void GenerateOres()
    {
        var data = terrain.terrainData;

        for (int i = 0; i < _plainHeights.Count; i++)
        {
            //Выясняем высоту террейна на клетке террейна
            var elevation = data.GetHeight(_plainHeights[i].X, _plainHeights[i].Y);
            Vector3 pos = new Vector3(_plainHeights[i].X, elevation, _plainHeights[i].Y);


            if (elevation >= biomes[3].height && elevation <= biomes[8].height)
            {
                if (Rnd.Range(0, 1000) <= 2)
                {
                   
                    if (Rnd.Range(0, 100) <= 70)
                    {
                        Instantiate(_coal, pos, Quaternion.identity, terrain.transform);
                    }
                    else
                    {
                        Instantiate(_basalt, pos, Quaternion.identity, terrain.transform);
                    }
                }
            }
            else if (elevation >= biomes[6].height && elevation <= biomes[10].height)
            {
                if (Rnd.Range(0, 2000) <= 1)
                {
                    Instantiate(_marble, pos, Quaternion.identity, terrain.transform);
                }
                if (Rnd.Range(0, 500) <= 3)
                {
                    Instantiate(_stone, pos, Utility.RotationSystem(), terrain.transform);//камень
                }

            }
            else if (elevation >= biomes[12].height)
            {
                if (Rnd.Range(0, 500) <= 3)
                {
                    Instantiate(_clate, pos, Quaternion.identity, terrain.transform);
                }
                if (Rnd.Range(0, 500) <= 3)
                {
                    Instantiate(_stone, pos, Utility.RotationSystem(), terrain.transform);//камень
                }
            }
        }
    }

    public void SetPlayerPosition()
    {
        var data = terrain.terrainData;

        for (int i = _plainHeights.Count/2; i < _plainHeights.Count; i++)
        {
            //Выясняем высоту террейна на клетке террейна
            var elevation = data.GetHeight(_plainHeights[i].X, _plainHeights[i].Y);

            //Проверяем что высота ниже или ровна уровню воды - если да, то пропускаем итерацию 
            if (elevation <= biomes[8].height) continue;
            //Проверяем что высота выше или ровна уровню гор (второй биом) - если да, то пропускаем итерацию 
            if (elevation >= biomes[11].height) continue;
            //шанс появления дерева на клетки 1%
            if (elevation >= biomes[9].height && elevation < biomes[10].height) continue;
            else
            {
                if (Rnd.Range(0, 1000) > 0) continue;
            }

            _player.transform.position = new Vector3(_plainHeights[i].X, elevation+2 , _plainHeights[i].Y);
            break;
        }
    }
}



[Serializable]
public class Height
{
    public int X;
    public int Y;
    public float Elevation;
}