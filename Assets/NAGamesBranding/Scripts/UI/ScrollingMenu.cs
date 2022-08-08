using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

// ------------------------------------------------------------------------
// Name :   -
// Desc :   -
// ------------------------------------------------------------------------

    public class ScrollingMenu : MonoBehaviour 
    {
	    public int					CurrPageId { get; private set; }
        private  List< Image >		PageDotImageList = new List< Image >( );
		public	 GameObject			dotPrefab;

	    [ HideInInspector ]
	    public  List< GameObject >	ItemList    { get; private set; }
	    public  Transform			PageRootT;
	    public  Transform			DotRootT;
	    public	GameObject			InputMask;
	    public  iTween.EaseType		PageEaseType = iTween.EaseType.easeOutBack;

	    private bool 				IsPageScrollingOn = false;
	    private Vector3				DragOrigin = Vector3.zero;

	    private bool				IsTouched;
	    private float				ItemW;
        [ SerializeField ]
	    private float				PageGap = 2f;
        private int                 NumPages { get{ return ItemList.Count; } }
        private float               PageXOffset = 0f;
        private float               ScrollPanelW;
        [ SerializeField ]
        private Transform           SelectedPageFrame;
        [ SerializeField ]
        private GameObject          ItemPrefab;

        [ SerializeField ]
        private GameObject          ButtonScrollLeft;
        [ SerializeField ]
        private GameObject          ButtonScrollRight;

        private Transform           SelectedPageFrameParent;
        private Action< int >       PageSelCB;
        private bool                IsLocked;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		void Start()
		{
			// Display( 4, 0, null );
		}
 
        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

	    public void Display( int nItems, int startPage = 0, Action onPageSetUpAction = null, Action< int > pageSelCB = null )
	    {
            if( SelectedPageFrame )
                SelectedPageFrameParent = SelectedPageFrame.parent;

            // Skip one frame before creating panel, since if panel creation is at the same frame
            // where canvas is activated, some irregularities happens.
  		    StartCoroutine( CreatePanelInNextFrame( nItems, ( ) => {
                                                                        CurrPageId = startPage;
                                                                        if( PageSelCB != null ) PageSelCB.Invoke( CurrPageId );
                                                                        StartCoroutine( GoToPage( CurrPageId, true ) );
                                                                        if( onPageSetUpAction != null ) onPageSetUpAction.Invoke( );
                                                                    } ) );
            PageSelCB = pageSelCB;
            IsLocked = false;
	    }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public List< GameObject > GetItemList( )
        {
            return ItemList;
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        void Update( )
	    {
		    if( ! IsPageScrollingOn && ! IsLocked )
		    {
			    if( Input.GetMouseButtonDown( 0 ) )
			    {
				    DragOrigin  	  = Input.mousePosition;
				    IsTouched	  	  = true;
				    if( InputMask ) InputMask.SetActive( false );
			    }

			    if( Input.GetMouseButtonUp( 0 ) )
			    {
				    if( DragOrigin != Vector3.zero )
				    {
					    float dx = ( Input.mousePosition - DragOrigin ).x;

					    if( Mathf.Abs( dx ) > 60f )
					    {
						    if( dx > 0 )
						    {
							    GotoPreviousPage( );
						    }
						    else
						    {
							    GotoNextPage( );
						    }
					    }
					    else 
     				    {
						    GoBackToSamePage( Mathf.Abs( dx ) < 10f );
					    }

					    DragOrigin = Vector3.zero;
					    IsTouched  = false;
				    }
			    }

			    if( IsTouched && Input.GetMouseButton( 0 ) && DragOrigin != Vector3.zero )
			    {
				    float dMove = ( Input.mousePosition - DragOrigin ).x;
				    float dx 	= ( (float) CurrPageId * -( ItemW + PageGap ) ) + dMove + PageOffset( CurrPageId );
				    PageRootT.localPosition = new Vector3( dx, PageRootT.localPosition.y, PageRootT.localPosition.z );
			
				    if( Mathf.Abs( dMove ) > 3f )
				    {
					    if( InputMask ) InputMask.SetActive( true );
				    }
			    }
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    IEnumerator CreatePanelInNextFrame( int nItems, Action onCreationAction )
	    {
            yield return null;

            for( int i = 0; i < nItems; i++ )
            {
                Transform t = Instantiate( ItemPrefab ).transform;
                t.parent = PageRootT;
                t.localPosition = Vector3.zero;
                t.localScale = Vector3.one;
				if(dotPrefab) {
                	Transform d = Instantiate( dotPrefab ).transform;
                	d.parent = DotRootT;
					PageDotImageList.Add ( d.GetComponent<Image> () );
				}
            }

            ScrollPanelW = GetComponent< RectTransform >( ).rect.width;
            PageXOffset  = ScrollPanelW / 2f;

		    ItemList = new List< GameObject >( );
		    foreach( Transform t in PageRootT ) ItemList.Add( t.gameObject );

		    ItemW = ItemList[ 0 ].GetComponent< RectTransform >( ).sizeDelta.x;

		    float x = ItemW / 2f;
		    float y = ItemList[ 0 ].GetComponent< RectTransform >( ).localPosition.y;
		    for( int i = 0; i < ItemList.Count; i++ )
		    {
			    ItemList[ i ].GetComponent< RectTransform >( ).localPosition = new Vector3( x, y, 0f );
			    x += ItemW + PageGap;
		    }
		
		    DragOrigin = Input.mousePosition;
		    IsTouched  = false;
		    if( InputMask ) InputMask.SetActive( false );

            if( onCreationAction != null ) onCreationAction.Invoke( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    IEnumerator GoToPage( int pageId, bool immediate = false )
	    {
		    int pId 	= Mathf.Clamp( pageId, 0, ItemList.Count - 1 );	
		    float tarX 	= (float) pId * -( ItemW + PageGap ) + PageOffset( pId );

		    if( immediate )
		    {
			    PageRootT.localPosition = new Vector3( tarX, PageRootT.localPosition.y, PageRootT.localPosition.z );
			    SelectPage( pId );
		    }
		    else
		    {
			    if( ! IsPageScrollingOn )
			    {
				    IsPageScrollingOn = true;

				    float dx = 1000f * ( ( tarX > PageRootT.localPosition.x ) ? 1f : -1f );

				    {
					    PageRootT.localPosition = new Vector3( PageRootT.localPosition.x + dx * Time.deltaTime, PageRootT.localPosition.y, PageRootT.localPosition.z );

					    Vector3 p0 = PageRootT.localPosition;
					    Vector3 p1 = new Vector3( tarX, PageRootT.localPosition.y, PageRootT.localPosition.z );

					    bool scrollingOver = false;
					    StartCoroutine( ITweenFX.Move( PageRootT.gameObject, p0, p1, 0.5f, PageEaseType, obj => scrollingOver = true ) );

					    while( ! scrollingOver ) yield return null;
				    }

				    SelectPage( pId );
			    }
		    }

		    IsPageScrollingOn = false;
		    if( InputMask ) InputMask.SetActive( false );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public void SelectPage( int pageId )
	    {
		    if( PageDotImageList != null )
		    {
			    for( int i = 0; i < PageDotImageList.Count; i++ )
			    {
				    PageDotImageList[ i ].color = ( i == pageId ) ? new Color( 1f, 1f, 1f, 1f ) : new Color( 0.6f, 0.6f, 0.6f, 1f );
			    }
		    }

		    PageRootT.localPosition = new Vector3( -( ItemW + PageGap ) * pageId + PageOffset( pageId ), PageRootT.localPosition.y, PageRootT.localPosition.z );

		    CurrPageId = pageId;
            if( PageSelCB != null ) PageSelCB.Invoke( CurrPageId );

            if( SelectedPageFrame )
            {
                SelectedPageFrame.parent = ItemList[ pageId ].transform;
                SelectedPageFrame.GetComponent< RectTransform >( ).position = ItemList[ pageId ].transform.position;
            }

            ButtonScrollLeft.GetComponent< Button >( ).interactable = ( pageId > 0 );
            ButtonScrollRight.GetComponent< Button >( ).interactable = ( pageId < NumPages - 1 );
 	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public void GotoPreviousPage( )
	    {
		    StartCoroutine( GoToPage( CurrPageId - 1 ) );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public void GotoNextPage( )
	    {
		    StartCoroutine( GoToPage( CurrPageId + 1 ) );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    void GoBackToSamePage( bool snap )
	    {
		    StartCoroutine( GoToPage( CurrPageId, snap ) );
	    }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        float PageOffset( int pageId )
        {
            if( pageId == 0 ) return -PageXOffset + PageGap;
            else
            if( pageId == ItemList.Count - 1 ) return ( ScrollPanelW / 2 - ItemW ) -PageGap;
            else
            return -ItemW / 2f;
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        void OnDisable( )
        {
            for( int i = PageRootT.childCount - 1; i >= 0; i-- ) DestroyImmediate( PageRootT.GetChild( i ).gameObject );
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public void Clean( )
        {
            if( SelectedPageFrame ) 
                SelectedPageFrame.parent = SelectedPageFrameParent;
        }

        // ------------------------------------------------------------------------
        // Name :   -
        // Desc :   -
        // ------------------------------------------------------------------------

        public void SetLock( bool isLocked )
        {
            IsLocked = isLocked;
        }
    }

