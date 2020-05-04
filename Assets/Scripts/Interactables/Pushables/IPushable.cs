//Main author: Maximiliam Rosén

using UnityEngine;

namespace Interactables.Pushables
{
    public interface IPushable
    { 
        void Push();
        Vector3 GetPushLocation(Vector3 pusherLocation);
        Transform GetPushableTransform();
    }
}
