
public abstract class NonMonoSingleton<T>
        where T : new( )
{
    private static  T _Instance = default;
    public  static  T  Instance
    {
        get 
        { 
            if ( _Instance == null )
            { 
                _Instance = new T( );
            }

            return _Instance;
        }
    }
}