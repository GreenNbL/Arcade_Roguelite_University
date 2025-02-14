using UnityEngine;

public class PrefabProbability : MonoBehaviour
{
    [SerializeField] private float probability;
    [SerializeField] private GameObject prefab;

    public float GetProbability()
    {
        return this.probability;
    }

    public GameObject GetPrefab()
    {
        return this.prefab;
    }
}
