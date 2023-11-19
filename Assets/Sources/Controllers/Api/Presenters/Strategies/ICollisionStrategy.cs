using System;
using UnityEngine;

namespace Sources.Controllers.Api.Presenters.Strategies
{
    public interface ICollisionStrategy : IStrategy
    {
        event Action<Collider2D> Collided;
    }
}