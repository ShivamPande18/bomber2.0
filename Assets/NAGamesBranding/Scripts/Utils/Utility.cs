using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

    // ------------------------------------------------------------------------
    // Name	:	SetActiveRecursively
    // Desc	:	-
    // ------------------------------------------------------------------------

    public static class Utility 
    {
	    public static Vector3 InValidVector 	= new Vector3( float.MinValue, float.MinValue, float.MinValue );
	    public static Vector3 OffScreenPosition = new Vector3( -9999f, -9999f, -9999f );
        public static float   Zero => 0.000001f;
        public static bool    IsZero( float value ) => Mathf.Abs( value ) < 0.00001f;

        public static string DeviceUniqueIdentifier
	    {
		    get
		    {
			    var deviceId = "";
    #if UNITY_ANDROID && ! UNITY_EDITOR
			    AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			    AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
			    AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");
			    AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
			    deviceId = secure.CallStatic<string> ("getString", contentResolver, "android_id");
    #else
			    deviceId = SystemInfo.deviceUniqueIdentifier;
    #endif
			    return deviceId;
		    }
	    }

        public static string FormatNumber( int n ) => n < 10 ? $"0{n}" : $"{n}";

	    // ------------------------------------------------------------------------
	    // Name	:	SetActiveRecursively
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void SetActiveRecursively( GameObject rootObj, bool active )
	    {
		    rootObj.SetActive( active );

		    foreach ( Transform cT in rootObj.transform )
		    {
			    SetActiveRecursively( cT.gameObject, active );
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	GetChildObject
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static Transform GetChildObjectWithTag( Transform parent, string tag)
        {
            foreach( Transform t in parent )
		    {
			    if( t.tag == tag )
				    return t;
		    }

		    return null;
        }

        // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

        public static bool GetChildObjectWithComponent< T >( Transform parent, ref Transform objWithComponent )
        {
            if( parent.GetComponent< T >( ) != null )
			{
                objWithComponent = parent;
                return true;
			}
            else
            {
                foreach( Transform child in parent )
                {
                    if( GetChildObjectWithComponent< T >( child, ref objWithComponent ) ) return true;
                }
            }

            return false;
        }

	    // ------------------------------------------------------------------------
	    // Name	:	CreateMaterial
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static bool GetImageSize(Texture asset, out int width, out int height) 
	    {
    #if UNITY_EDITOR		
		    if (asset != null) 
		    {
			    string assetPath = AssetDatabase.GetAssetPath(asset);
			    TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

			    if (importer != null) 
			    {
				    object[] args = new object[ 2 ] { 0, 0 };
				    MethodInfo mi = typeof(TextureImporter).GetMethod( "GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance );
				    mi.Invoke(importer, args);

				    width 	= (int)args[ 0 ];
				    height 	= (int)args[ 1 ];

				    return true;
			    }
		    }
    #endif
		    height = width = 0;
		    return false;
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	CreateMaterial
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static Material CreateMaterial( string shaderStr, string texturePath )
	    {
		    var material 	= new Material ( Shader.Find( shaderStr ) );
		    Texture tex 	= (Texture) Resources.Load( texturePath, typeof( Texture ) );
		    material.SetTexture( "_MainTex", tex );

		    return material;
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	StringToStringList
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static List< string > StringToStringList( string s )
	    {
		    return s.Split(',').ToList( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	StringToStringList
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static string StringListToString( List< string > strList )
	    {
		    string str = string.Empty;

		    for( int i = 0; i < strList.Count - 1; i++ ) str += ( strList[ i ] + "," );
		    str += strList[ strList.Count - 1 ];
		    return str;
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	Vector3FromString
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static Vector3 Vector3FromString( string s )
	    {
		    Vector3 v = Vector3.zero;

		    List< string > strList = StringToStringList( s );

		    v.x = float.Parse( strList[ 0 ] );
		    v.y = float.Parse( strList[ 1 ] );

		    if( strList.Count > 2 )
		    {
			    v.z = float.Parse( strList[ 2 ] );
		    }

		    return v;
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static string ListToString< T >( List< T > list )
	    {
		    string str = string.Empty;

		    if( list.Count > 0 )
		    {
			    for( int i = 0; i < list.Count - 1; i++ ) str += ( list[ i ] + "," );
			    str += list[ list.Count - 1 ];
		    }

		    return str;
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static List< int > StringToIntList( string str )
	    {
		    List< int > list = new List< int >( );
		    if( ! str.Equals( string.Empty ) ) StringToStringList( str ).ForEach( x => list.Add( int.Parse( x ) ) );
		    return list;
	    } 

	    // ------------------------------------------------------------------------
	    // Name	:	CreateXmlDocument
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void CreateXmlDocumentInFolder( string s, bool printToConsole = false, string fileName = null )
	    {
		    XmlDocument xmlDoc = new XmlDocument( );
		    xmlDoc.LoadXml( s.ToLower( ) );

		    string formattedStr = Utility.FormatXmlString( xmlDoc );

		    if( printToConsole )
		    {
    #if DEBUG_ENABLED
			    Debug.Log( formattedStr );
    #endif
		    }

		    if( fileName != null )
		    {
			    File.WriteAllText( fileName.ToLower( ) + ".xml", formattedStr );
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	CreateXmlDocument
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void CreateXmlDocumentInFolder( XmlDocument xmlDoc, bool printToConsole = false, string fileName = null )
	    {
		    string formattedStr = Utility.FormatXmlString( xmlDoc );

		    if( printToConsole )
		    {
    #if UNITY_EDITOR
			    Debug.Log( formattedStr );
    #endif
		    }

		    if( fileName != null )
		    {
			    using( TextWriter sw = new StreamWriter( fileName, false, Encoding.UTF8 ) )
			    {
				    xmlDoc.Save(sw);
			    }
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	FormatXmlString
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    static public string FormatXmlString( this XmlDocument doc )
	    {
		    StringBuilder sb = new StringBuilder( );

		    XmlWriterSettings settings = new XmlWriterSettings
		    {
			    Indent 			= true,
			    IndentChars 	= "\t",
			    NewLineChars 	= "\r\n",
			    NewLineHandling = NewLineHandling.Replace
		    };

		    using( XmlWriter writer = XmlWriter.Create( sb, settings ) )
		    {
			    doc.Save( writer );
		    }

		    return sb.ToString( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	SaveLongValueToPref
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    static public void SaveLongValueToPref( string prefStr, long value )
	    {
		    int[ ] al = Long2DoubleInt( value );

		    PlayerPrefs.SetInt( prefStr + "long0", al[ 0 ] );
		    PlayerPrefs.SetInt( prefStr + "long1", al[ 1 ] );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	LoadLongValueFromPref
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    static public long LoadLongValueFromPref( string prefStr, long defL )
	    {
		    int[ ] defI = Long2DoubleInt( defL );

		    int i0 = PlayerPrefs.GetInt( prefStr + "long0", defI[ 0 ] );
		    int i1 = PlayerPrefs.GetInt( prefStr + "long1", defI[ 1 ] );

		    return DoubleInt2Long( i0, i1 );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	Long2DoubleInt
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    static public int[ ] Long2DoubleInt( long l )
	    {
            int i0 = (int)( l & uint.MaxValue );
		    int i1 = (int)( l >> 32 );

		    return new int[ ] { i0, i1 };
        }

	    // ------------------------------------------------------------------------
	    // Name	:	DoubleInt2Long
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    static public long DoubleInt2Long( int i0, int i1 )
        {
		    long l = i1;
		    l = l << 32;
		    l = l | (uint) i0;
		    return l;
        }

	    // ------------------------------------------------------------------------
	    // Name	:	LoadMaterial
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static Material LoadMaterial( string path )
	    {
		    return (Material) Resources.Load( path, typeof( Material ) ) as Material;
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	DestroyObject
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void DestroyObject( GameObject obj )
	    {
		    if( Application.isPlaying )
		    {
			    GameObject.Destroy( obj );
		    }
		    else
		    {
    #if UNITY_EDITOR	
			    GameObject.DestroyImmediate( obj );
    #endif
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	DestroyAllChildrenImmediate
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void DestroyAllChildren( Transform root )
	    {
		    if( root != null )
		    {
			    for( int i = root.childCount - 1; i >= 0; i-- )
			    {
				    GameObject.Destroy( root.GetChild( i ).gameObject );
			    }
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	FormatTime
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static string FormatTime( int time )
	    {
		    int min = time / 60;
		    int sec = time % 60;

		    string minStr = ( min < 10 ) ? "0" + min.ToString( ) : min.ToString( );
		    string secStr = ( sec < 10 ) ? "0" + sec.ToString( ) : sec.ToString( );

		    return minStr + " : " + secStr;
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	Vec2To3
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static Vector3 Vec2To3( Vector2 v )
	    {
		    return new Vector3( v.x, v.y, 0f );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	Vec3To2
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static Vector2 Vec3To2( Vector2 v )
	    {
		    return new Vector2( v.x, v.y );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	LoadSprite
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static Sprite LoadSprite( string path )
	    {
		    return Resources.Load< Sprite >( path );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static List< T > GetRandomElements< T >( List< T > list, int nItems )
	    {
		    UnityEngine.Assertions.Assert.IsTrue( list.Count >= nItems );

		    return list.OrderBy( x => System.Guid.NewGuid( ) ).Take( nItems ).ToList( );
	    }

        // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

        public static T GetARandomElement< T >( List< T > list )
        {
            return list[ Random.Range( 0, list.Count ) ];
        }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static bool AreListsSame< T >( List< T > list0, List< T > list1 )
	    {
		    var firstNotSecond = list0.Except( list1 ).ToList( );
		    var secondNotFirst = list1.Except( list0 ).ToList( );

		    return ! firstNotSecond.Any( ) && ! secondNotFirst.Any( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	LoadSprite
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void PrintList< T >( List< T > list, string titleStr = "", bool isErrorLog = false )
	    {
		    string s = titleStr.Equals( string.Empty ) ? string.Empty : titleStr + " ";

		    for( int i = 0; i < list.Count; i++ )
		    {
			    s += list[ i ] + ( i < list.Count - 1 ? "," : string.Empty );
		    }

		    s += "\n";

		    if( isErrorLog )
                Debug.LogError( s );
            else
                Debug.Log( s );
	    }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static void ShuffleList<T>(this IList<T> list)  
        {  
            System.Random rng = new System.Random( );

            int n = list.Count;  
            while ( n > 1 ) 
            {  
                n--;  
                int k = rng.Next( n + 1 );  
                T value = list[ k ];  
                list[ k ] = list[ n ];  
                list[ n ] = value;  
            }  
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static IEnumerator CheckInternetAvailability( System.Action<bool> action )
        {
            using( UnityWebRequest webRequest = UnityWebRequest.Get( "https://google.com" ) )
            {
                yield return webRequest.SendWebRequest( );

                if ( webRequest.isNetworkError )
                {
                    if( action != null ) action.Invoke( false );
                }
                else
                {
                    if( action != null ) action.Invoke( true );
                }
            }
        }

        // ------------------------------------------------------------------------
    // Name :   -
    // Desc :   -
    // ------------------------------------------------------------------------

    public static string StripDeviceId( string name, char delimiter = '~' )
    {
        int startId = name.IndexOf( delimiter );

        if( startId < 0 )
            return name;
        else
            return name.Substring( startId + 1 );
    }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static string ReplaceCharacters( string str, char replaceChar, params char[ ] charsToReplace )
        {
            string replaceStr = str;

            foreach( var iC in charsToReplace )
                replaceStr = new string( replaceStr.Select( r => ( r == iC ? replaceChar : r ) ).ToArray( ) );

            return replaceStr;
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static IEnumerator DownloadImageFromURL( string url, System.Action< Texture2D > onCompleteAction ) 
        {
            using( UnityWebRequest uwr = UnityWebRequestTexture.GetTexture( url ) )
            {
                yield return uwr.SendWebRequest( );
    
                if ( uwr.isNetworkError || uwr.isHttpError )
                {
                    Debug.Log( uwr.error );
                }
                else
                {
                    Texture2D tex = DownloadHandlerTexture.GetContent( uwr );
                    onCompleteAction.Invoke( tex );
                }
            }
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static void SwapListItems<T>( List< T > list, int index0, int index1 )
        {
            var item0 = list[ index0 ];

            Assert.IsTrue( index0 < list.Count );
            Assert.IsTrue( index1 < list.Count );

            list[ index0 ] = list[ index1 ];
            list[ index1 ] = item0;
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static List< bool > ByteToBoolList( IEnumerable< byte > intList )
        {
            return intList.Select( x => x == 1 ? true : false ).ToList( );
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static List< int > BoolToIntList ( IEnumerable< bool > boolList )
        {
            return boolList.Select( x => x ? 1 : 0 ).ToList( );
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static List< T > RemoveDuplicates< T >( IEnumerable< T > list )
        {
            return new HashSet< T >( list ).ToList( );
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static string GetLongestStringInList( List< string > list )
        {
            return list.Count == 0 ? string.Empty : list.Aggregate( "", ( max, cur ) => max.Length > cur.Length ? max : cur );
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static bool ObjectTouched( GameObject obj )
        {
            if( Input.GetMouseButtonDown( 0 ) )
            {
                Ray raycast = Camera.main.ScreenPointToRay( Input.mousePosition );

                RaycastHit raycastHit;

                if( Physics.Raycast( raycast, out raycastHit ) &&
                    raycastHit.collider.gameObject == obj ) return true;
            }

            return false;
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :  eg. { 4, 5, 1, 1 } cumulative weight : 4, 9, 10, 11
        // ------------------------------------------------------------------------

        public static int WeightedRandomValueIndex( int[ ] weightArray )
        {
            var totalW = weightArray.Sum( );
            var randomVal = Random.Range( 0, totalW );

            var cummW = 0;
            for( int i = 0; i < weightArray.Length; i++ )
            {
                cummW += weightArray[ i ];
                if( randomVal < cummW )
                    return i;
            }

            Debug.LogError( $"Error : Random value : {randomVal} Total W : {totalW} : array : { ListToString( weightArray.ToList( ) ) }" );

            return Random.Range( 0, weightArray.Length );
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public static void ShuffleList<T>( this IList<T> list, int sIndex, int eIndex )
        {
            List< T > subList = new List<T>( );

            for( int i = sIndex; i <= eIndex; i++ )
            {
                subList.Add( list[ i ] );
            }

            ShuffleList( subList );

            for( int i = sIndex; i <= eIndex; i++ )
            {
                list[ i ] = subList[ i ];
            }
        }
    }
