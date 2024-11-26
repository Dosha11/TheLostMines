using System.Collections;
using UnityEngine;


public class Dot : MonoBehaviour
{

    GameObject DotHorizontal;
    GameObject DotAngle;
    GameObject DotFork;
    GameObject deadlock;
    GameObject descent;
    GameObject stash;


    public Vector3Int pointMe;
    public Generator generator;
    public int num;
    bool end;

    int x;
    int y;
    int z;


    public void Registration(Generator generator, Vector3Int me, int num)
    {
        pointMe = me;
        this.generator = generator;
        this.num = num;

        x = pointMe.x;
        y = pointMe.y;
        z = pointMe.z;

        generator.usedPoints.Add(me);

        DotHorizontal = generator.DotHorizontal;
        DotAngle = generator.DotAngle;
        DotFork = generator.DotFork;
        deadlock = generator.deadlock;
        descent = generator.descent;
        stash = generator.stash;
        {
            generator.listMyStep[y]++;

            if (generator.listMaxStep[y] > generator.listMyStep[y])
            {
                end = false;

            }
            else
            {
                end = true;
                //  Debug.LogWarning(this.gameObject.name);
            }
        }


        StartCoroutine(Step());
        CreatingOre();
       // Collapse(0);
    }
    public void CreatingOre()
    {

        int num = Random.Range(0, 100);


        if (y <= 0)
        {
            if (num < generator.PercentageLossCopper)
            {
                float x = Random.Range(-4, 5);
                float z = Random.Range(-4, 5);
                Vector3 pos = new Vector3((pointMe.x * 10) + x, pointMe.y * 10, (pointMe.z * 10) + z);
                GameObject clone = Instantiate(generator.Copper, pos, Quaternion.identity, transform);
                clone.GetComponent<OreMarker>().MyDot = this;
                // clone.transform.localPosition = new Vector3(x, 0, z);
            }
        }
        if (y <= -1)
        {
            if (num < generator.PercentageLossIron)
            {
                float x = Random.Range(-4, 5);
                float z = Random.Range(-4, 5);
                Vector3 pos = new Vector3((pointMe.x * 10) + x, pointMe.y * 10, (pointMe.z * 10) + z);
                GameObject clone = Instantiate(generator.Iron, pos, Quaternion.identity, transform);
                clone.GetComponent<OreMarker>().MyDot = this;
                // clone.transform.localPosition = new Vector3(x, 0, z);
            }
        }
        if (y <= -2)
        {
            if (num < generator.PercentageLossSilver)
            {
                float x = Random.Range(-4, 5);
                float z = Random.Range(-4, 5);
                Vector3 pos = new Vector3((pointMe.x * 10) + x, pointMe.y * 10, (pointMe.z * 10) + z);
                GameObject clone = Instantiate(generator.Silver, pos, Quaternion.identity, transform);
                clone.GetComponent<OreMarker>().MyDot = this;
                // clone.transform.localPosition = new Vector3(x, 0, z);
            }
        }
        if (y <= -3)
        {
            if (num < generator.PercentageLossGold )
            {
                float x = Random.Range(-4, 5);
                float z = Random.Range(-4, 5);
                Vector3 pos = new Vector3((pointMe.x * 10) + x, pointMe.y * 10, (pointMe.z * 10) + z);
                GameObject clone = Instantiate(generator.Gold, pos, Quaternion.identity, transform);
                clone.GetComponent<OreMarker>().MyDot = this;
                // clone.transform.localPosition = new Vector3(x, 0, z);
            }
        }
    }

    public IEnumerator Step()
    {

        yield return new WaitForSeconds(0.3f);
        //прямые
        if (num == 10)
        {
            if (generator.Compare(new Vector3Int(x - 1, y, z)))
            {
                CreateDot(new Vector3Int(x - 1, y, z));
            }

            if (generator.Compare(new Vector3Int(x + 1, y, z)))
            {
                CreateDot(new Vector3Int(x + 1, y, z));
            }
        }
        if (num == 5)
        {
            if (generator.Compare(new Vector3Int(x, y, z + 1)))
            {
                CreateDot(new Vector3Int(x, y, z + 1));
            }

            if (generator.Compare(new Vector3Int(x, y, z - 1)))
            {
                CreateDot(new Vector3Int(x, y, z - 1));
            }
        }

        //углы
        if (num == 3)
        {
            if (generator.Compare(new Vector3Int(x, y, z + 1)))
            {
                CreateDot(new Vector3Int(x, y, z + 1));
            }

            if (generator.Compare(new Vector3Int(x + 1, y, z)))
            {
                CreateDot(new Vector3Int(x + 1, y, z));
            }
        }
        if (num == 6)
        {
            if (generator.Compare(new Vector3Int(x, y, z - 1)))
            {
                CreateDot(new Vector3Int(x, y, z - 1));
            }

            if (generator.Compare(new Vector3Int(x + 1, y, z)))
            {
                CreateDot(new Vector3Int(x + 1, y, z));
            }
        }
        if (num == 12)
        {
            if (generator.Compare(new Vector3Int(x, y, z - 1)))
            {
                CreateDot(new Vector3Int(x, y, z - 1));
            }

            if (generator.Compare(new Vector3Int(x - 1, y, z)))
            {
                CreateDot(new Vector3Int(x - 1, y, z));
            }
        }
        if (num == 9)
        {
            if (generator.Compare(new Vector3Int(x, y, z + 1)))
            {
                CreateDot(new Vector3Int(x, y, z + 1));
            }

            if (generator.Compare(new Vector3Int(x - 1, y, z)))
            {
                CreateDot(new Vector3Int(x - 1, y, z));
            }
        }

        //перекрсетки
        if (num == 7)
        {
            if (generator.Compare(new Vector3Int(x, y, z + 1)))
            {
                CreateDot(new Vector3Int(x, y, z + 1));
            }

            if (generator.Compare(new Vector3Int(x, y, z - 1)))
            {
                CreateDot(new Vector3Int(x, y, z - 1));
            }

            if (generator.Compare(new Vector3Int(x + 1, y, z)))
            {
                CreateDot(new Vector3Int(x + 1, y, z));
            }
        }
        if (num == 11)
        {
            if (generator.Compare(new Vector3Int(x, y, z + 1)))
            {
                CreateDot(new Vector3Int(x, y, z + 1));
            }

            if (generator.Compare(new Vector3Int(x - 1, y, z)))
            {
                CreateDot(new Vector3Int(x - 1, y, z));
            }

            if (generator.Compare(new Vector3Int(x + 1, y, z)))
            {
                CreateDot(new Vector3Int(x + 1, y, z));
            }
        }
        if (num == 13)
        {
            if (generator.Compare(new Vector3Int(x, y, z + 1)))
            {
                CreateDot(new Vector3Int(x, y, z + 1));
            }

            if (generator.Compare(new Vector3Int(x, y, z - 1)))
            {
                CreateDot(new Vector3Int(x, y, z - 1));
            }

            if (generator.Compare(new Vector3Int(x - 1, y, z)))
            {
                CreateDot(new Vector3Int(x - 1, y, z));
            }
        }
        if (num == 14)
        {

            if (generator.Compare(new Vector3Int(x, y, z - 1)))
            {
                CreateDot(new Vector3Int(x, y, z - 1));
            }
            if (generator.Compare(new Vector3Int(x - 1, y, z)))
            {
                CreateDot(new Vector3Int(x - 1, y, z));
            }
            if (generator.Compare(new Vector3Int(x + 1, y, z)))
            {
                CreateDot(new Vector3Int(x + 1, y, z));
            }
        }

        //спуски
        if (num == 21)
        {
            if (generator.Compare(new Vector3Int(x, y, z - 1)))
            {
                CreateDot(new Vector3Int(x, y, z - 1));
            }
        }
        if (num == 22)
        {
            if (generator.Compare(new Vector3Int(x, y, z + 1)))
            {
                CreateDot(new Vector3Int(x, y, z + 1));
            }
        }
        if (num == 23)
        {
            if (generator.Compare(new Vector3Int(x - 1, y, z)))
            {
                CreateDot(new Vector3Int(x - 1, y, z));
            }
        }
        if (num == 24)
        {
            if (generator.Compare(new Vector3Int(x + 1, y, z)))
            {
                CreateDot(new Vector3Int(x + 1, y, z));
            }
        }
    }


    public void CreateDot(Vector3Int pos)
    {
        if (!end)
        {
            int a = Random.Range(0, 90);
            // Debug.Log(a + " " + gameObject.name);
            if (a >= 0 && a <= 20)
            {
                if (pos.z == z)
                {
                    GameObject clone = Instantiate(DotHorizontal, pos * generator.offset, Quaternion.identity);
                    clone.GetComponent<Dot>().Registration(generator, pos, 10);
                    clone.gameObject.name = pos.ToString();
                }
                else
                {
                    GameObject clone = Instantiate(DotHorizontal, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 5);
                    clone.gameObject.name = pos.ToString();
                }
            }//линия
            else if (a > 20 && a <= 40)
            {
                int b = Random.Range(0, 2);
                if (pos.z == z)
                {
                    if (pos.x > x)
                    {
                        if (b == 0)
                        {
                            GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 12);
                            clone.gameObject.name = pos.ToString();
                        }
                        else
                        {
                            GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 9);
                            clone.gameObject.name = pos.ToString();
                        }
                    }
                    else
                    {
                        if (b == 0)
                        {
                            GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 6);
                            clone.gameObject.name = pos.ToString();

                        }
                        else
                        {
                            GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 3);
                            clone.gameObject.name = pos.ToString();
                        }
                    }


                }
                else if (pos.z > z)
                {
                    if (b == 0)
                    {
                        GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 6);
                        clone.gameObject.name = pos.ToString();
                    }
                    else
                    {
                        GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 12);
                        clone.gameObject.name = pos.ToString();
                    }
                }
                else
                {
                    if (b == 0)
                    {
                        GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 9);
                        clone.gameObject.name = pos.ToString();

                    }
                    else
                    {
                        GameObject clone = Instantiate(DotAngle, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 3);
                        clone.gameObject.name = pos.ToString();
                    }
                }
            }//угол
            else if (a > 40 && a <= 90)
            {
                int b = Random.Range(0, 3);

                if (pos.z == z)
                {
                    if (pos.x > x)
                    {
                        if (b == 0)
                        {
                            GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 14);
                            clone.gameObject.name = pos.ToString();

                        }
                        else if (b == 1)
                        {
                            GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 13);
                            clone.gameObject.name = pos.ToString();
                        }
                        else if (b == 2)
                        {
                            GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 11);
                            clone.gameObject.name = pos.ToString();

                        }
                    }
                    else
                    {
                        if (b == 0)
                        {
                            GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 14);
                            clone.gameObject.name = pos.ToString();

                        }
                        else if (b == 1)
                        {
                            GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 7);
                            clone.gameObject.name = pos.ToString();
                        }
                        else if (b == 2)
                        {
                            GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                            clone.GetComponent<Dot>().Registration(generator, pos, 11);
                            clone.gameObject.name = pos.ToString();

                        }
                    }
                }
                else if (pos.z > z)
                {
                    if (b == 0)
                    {
                        GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 14);
                        clone.gameObject.name = pos.ToString();

                    }
                    else if (b == 1)
                    {
                        GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 7);
                        clone.gameObject.name = pos.ToString();
                    }
                    else if (b == 2)
                    {
                        GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 13);
                        clone.gameObject.name = pos.ToString();

                    }
                }
                else
                {
                    if (b == 0)
                    {
                        GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 11);
                        clone.gameObject.name = pos.ToString();

                    }
                    else if (b == 1)
                    {
                        GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 7);
                        clone.gameObject.name = pos.ToString();
                    }
                    else if (b == 2)
                    {
                        GameObject clone = Instantiate(DotFork, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 13);
                        clone.gameObject.name = pos.ToString();

                    }
                }
            }//перекресток 
            else if (a > 90 && a <= 100)
            {
                if (pos.z > z)
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 22);
                    clone.gameObject.name = pos.ToString();
                }
                else if (pos.z < z)
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 21);
                    clone.gameObject.name = pos.ToString();
                }
                else if (pos.x > x)
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 24);
                    clone.gameObject.name = pos.ToString();
                }
                else
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 23);
                    clone.gameObject.name = pos.ToString();
                }
            }//спуск
        }
        else
        {

            int value = 0;
            if (generator.listMyStep.TryGetValue(y - 1, out value) && generator.listMyStep[y - 1] == 0)
            {
                pos.y -= 1;
                if (pos.z > z)
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 22);
                    clone.gameObject.name = pos.ToString();
                }
                else if (pos.z < z)
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 21);
                    clone.gameObject.name = pos.ToString();
                }
                else if (pos.x > x)
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 24);
                    clone.gameObject.name = pos.ToString();
                }
                else
                {
                    GameObject clone = Instantiate(descent, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                    clone.GetComponent<Dot>().Registration(generator, pos, 23);
                    clone.gameObject.name = pos.ToString();
                }
            }
            else
            {

                int b = Random.Range(0, 2);

                if (b == 0)
                {
                    if (pos.x > x)
                    {
                        GameObject clone = Instantiate(deadlock, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                        clone.gameObject.name = pos.ToString();
                        //  Debug.LogError("AAAAA - " + clone.gameObject.name + " - x > x");
                    }
                    else if (pos.x < x)
                    {
                        GameObject clone = Instantiate(deadlock, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                        clone.gameObject.name = pos.ToString();
                        //  Debug.LogError("AAAAA - " + clone.gameObject.name + " - x < x");
                    }

                    else if (pos.z > z)
                    {
                        GameObject clone = Instantiate(deadlock, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                        clone.gameObject.name = pos.ToString();
                        // Debug.LogError("AAAAA - " + clone.gameObject.name + " - z > z");
                    }
                    else if (pos.z < z)
                    {
                        GameObject clone = Instantiate(deadlock, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                        clone.gameObject.name = pos.ToString();
                        //  Debug.LogError("AAAAA - " + clone.gameObject.name + " - z < z");
                    }

                }
                if (b == 1)
                {
                    if (pos.x > x)
                    {
                        GameObject clone = Instantiate(stash, pos * generator.offset, Quaternion.Euler(0, -90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 8);
                        clone.gameObject.name = pos.ToString();
                    }
                    else if (pos.x < x)
                    {
                        GameObject clone = Instantiate(stash, pos * generator.offset, Quaternion.Euler(0, 90, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 2);
                        clone.gameObject.name = pos.ToString();
                    }

                    else if (pos.z > z)
                    {
                        GameObject clone = Instantiate(stash, pos * generator.offset, Quaternion.Euler(0, 180, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 4);
                        clone.gameObject.name = pos.ToString();
                    }
                    else if (pos.z < z)
                    {
                        GameObject clone = Instantiate(stash, pos * generator.offset, Quaternion.Euler(0, 0, 0));
                        clone.GetComponent<Dot>().Registration(generator, pos, 1);
                        clone.gameObject.name = pos.ToString();
                    }

                }
            }

        }
    }


    /// <summary>
    /// Обвал
    /// </summary>
    /// <param name="Complication">Усложнение</param>
    public void Collapse(int Complication)
    {
        int num = Random.Range(0, 20);
        num += Complication;
        if (num + Mathf.Abs(pointMe.y) > 20)
        {
            Vector3 pos = new Vector3((pointMe.x * 10), (pointMe.y * 10)+5 , (pointMe.z * 10));
            GameObject clone = Instantiate(generator.collaps, pos, Quaternion.identity, transform);
        }

    }

}
