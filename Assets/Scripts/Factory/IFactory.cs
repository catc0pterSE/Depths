using UnityEngine;

namespace Factory
{
    public interface IFactory <out T> where T : MonoBehaviour
    {
        T Create(Transform container);
    }
}