using UnityEngine;
using System.Collections.Generic;


public static class MathB
{
    /// <summary>
    /// Returns a List of GameObjects all evenly spaced in one of 3 shapes.( Wall, HalfCircle, Circle)
    /// </summary>
    /// <param name="pf">Prefab</param>
    /// <param name="radius">This is also used for wall length</param>
    /// <param name="amount">Pillar amount</param>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static List<GameObject> ShapesOfGameObjects(GameObject pf, float radius, int amount, Shapes shape)
    {
        List<GameObject> objects = new List<GameObject>(amount);

        if (shape == Shapes.Wall)
        {
            for (int i = 0; i < amount; i++)
            {
                // Position along the Z-axis, spread the objects along the radius
                float wallPosZ = -radius + i * radius / amount * 2;

                // Position along the X should be constant (for example, 0), adjust the other axes
                var pos = new Vector3(0, 0, wallPosZ);

                // Instantiate and add to the list
                GameObject obj = Object.Instantiate(pf, pos, Quaternion.identity) as GameObject;
                objects.Add(obj);
            }
        }
        else
        {
            // Circle or Half Circle Shape
            float n = shape == Shapes.Circle ? 2 : 1; // n=2 for full circle, n=1 for half circle

            for (int i = 0; i < amount; i++)
            {
                float angle = i * Mathf.PI * n / amount;
                Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

                Quaternion rot = Quaternion.LookRotation(position - Vector3.zero);
                GameObject obj = Object.Instantiate(pf, position, rot) as GameObject;

                objects.Add(obj);
            }
        }

        return objects;
    }



}