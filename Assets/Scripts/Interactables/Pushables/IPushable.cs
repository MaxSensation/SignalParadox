//Main author: Maximiliam Rosén

using UnityEngine;

namespace Interactables.Pushables
{
    public interface IPushable
    { 
        void Pushing();
        void NotPushing();
        Vector3 GetPushLocation(Vector3 pusherLocation);
        Transform GetPushableTransform();
    }
}
