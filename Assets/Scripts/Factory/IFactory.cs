using UnityEngine;

namespace Factory
{
    public interface IFactory <T> where T : MonoBehaviour
    {
        T Create(Transform container);
    }
}