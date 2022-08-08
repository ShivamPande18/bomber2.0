
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
                where T : Component
{
    private static  T _Instance           = default;
    public  static  T  Instance
    {
        get 
        { 
            if ( _Instance == null ) 
            { 
                _Instance = (T) FindObjectOfType( typeof( T ) );
            }

            return _Instance;
        }
    }
}