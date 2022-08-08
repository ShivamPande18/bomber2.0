using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

    // ------------------------------------------------------------------------
    // Name	:	-
    // Desc	:	-
    // ------------------------------------------------------------------------

    public class ITweenFX : MonoBehaviour 
    {
	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator Move( GameObject obj, Vector2 from, Vector2 to, float time, iTween.EaseType easeType = iTween.EaseType.linear, Action< GameObject > onComplete = null )
	    {
		    bool waitForAnimOver = true;

		    obj.transform.localPosition = from;
		    iTween.MoveTo( obj, iTween.Hash( "position", Utility.Vec2To3( to ),
										     "islocal", true, 
										     "time", time, 
										     "oncomplete", (Action)( ( )=> { waitForAnimOver = false; } ),
										     "easetype", easeType ));

		    while( waitForAnimOver ) yield return null;

		    if( onComplete != null ) onComplete( obj ); 
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator MoveBy( GameObject obj, Vector2 by, float time, iTween.EaseType easeType = iTween.EaseType.linear, Action onComplete = null )
	    {
		    bool waitForAnimOver = true;

		    Vector2 to = Utility.Vec3To2( obj.transform.localPosition ) + by;

		    iTween.MoveTo( obj, iTween.Hash( "position", Utility.Vec2To3( to ),
										     "islocal", true, 
										     "time", time, 
										     "oncomplete", (Action)( ( )=> { waitForAnimOver = false; } ),
										     "easetype", easeType ));

		    while( waitForAnimOver ) yield return null;

		    if( onComplete != null ) onComplete( ); 
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void MoveAlongXxis( GameObject obj, Transform targetT, float fromX, float toX, float time, iTween.EaseType easeType, Action onComplete = null )
	    {
		    obj.transform.localPosition = new Vector3( fromX, targetT.localPosition.y, targetT.localPosition.z );
		    iTween.MoveTo( obj, iTween.Hash( "position", new Vector3( toX, targetT.localPosition.y, targetT.localPosition.z ),
										     "islocal", true, 
										     "time", time, 
										     "oncomplete", (Action)( ( )=> { if( onComplete != null ) onComplete( ); } ),
										     "easetype", easeType ));
	    }


	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void MoveAlongYxis( GameObject obj, Transform targetT, float fromY, float toY, float time, iTween.EaseType easeType, Action onComplete = null )
	    {
		    obj.transform.localPosition = new Vector3( targetT.localPosition.x, fromY, targetT.localPosition.z );
		    iTween.MoveTo( obj, iTween.Hash( "position", new Vector3( targetT.localPosition.x, toY, targetT.localPosition.z ),
										     "islocal", true, 
										     "time", time, 
										     "oncomplete", (Action)( ( )=> { if( onComplete != null ) onComplete( ); } ),
										     "easetype", easeType ));
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator Pulse( GameObject obj, float startScale, float time, Action onComplete = null )
	    {
		    bool waitForAnimOver = true;

		    obj.transform.localScale = new Vector3( startScale, startScale, 1f );
		    iTween.ScaleTo( obj, iTween.Hash( "x", startScale * 1.06, 
										      "y", startScale * 1.06, 
										      "time", time, 
										      "easetype", iTween.EaseType.linear, 
										      "oncomplete", (Action)( ( )=> { waitForAnimOver = false; } ) ) );

		    while( waitForAnimOver ) yield return null;

            waitForAnimOver = true;
		    iTween.ScaleTo( obj, iTween.Hash( "x", startScale, 
										      "y", startScale, 
										      "time", time, 
										      "easetype", iTween.EaseType.linear, 
										      "oncomplete", (Action)( ( )=> { waitForAnimOver = false; } ) ) );

            while( waitForAnimOver ) yield return null;

		    if( onComplete != null ) onComplete( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void ScaleTo( GameObject obj, float sScale, float eScale, float time, iTween.EaseType easeType = iTween.EaseType.linear, Action onComplete = null )
	    {
		    obj.transform.localScale = new Vector3( sScale, sScale, sScale );
		    iTween.ScaleTo( obj, iTween.Hash( "x", eScale, 
										      "y", eScale, 
                                              "z", eScale, 
										      "time", time,
										      "easetype", easeType, 
										      "oncomplete", (Action)( ( )=> { 
											  								    if( onComplete != null ) onComplete( ); 
																			    obj.transform.localScale = new Vector3( eScale, eScale, eScale );
																		    } ) ) );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void ScaleYTo( GameObject obj, float sScale, float eScale, float time, iTween.EaseType easeType, Action onComplete = null )
	    {
		    obj.transform.localScale = new Vector3( obj.transform.localScale.x, sScale, 1f );
		    iTween.ScaleTo( obj, iTween.Hash( "y", eScale, 
										      "time", time,
										      "easetype", easeType, 
										      "oncomplete", (Action)( ( )=> { if( onComplete != null ) onComplete( ); } ) ) );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator RippleScaleAnimate( 	bool isIn, 
													    List< GameObject > objList, 
													    int sId, 
													    iTween.EaseType easeType,
													    bool scaleStartObj = false, 
													    Action< int > onAnimComplete = null )
	    {
		    float sScale = isIn ? 0f : 1f;
		    float eScale = isIn ? 1f : 0f;

		    foreach( GameObject obj in objList ) obj.transform.localScale = isIn ? Vector3.zero : obj.transform.localScale;

		    float objExtraScale = scaleStartObj ? 1.1f : 1f;

		    ScaleTo( objList[ sId ],
				     sScale * objExtraScale, 
				     eScale * objExtraScale, 
				     0.5f, 
				     easeType,
				    ( )=> 
						    {  
							    if( onAnimComplete != null ) onAnimComplete( sId );
						    } 
				    );

		    yield return new WaitForSeconds( 0.2f );

		    for( int i = 1; i < objList.Count; i++ )
		    {
			    if( sId - i >= 0 )
			    {
				    int id = sId - i;
				    ScaleTo( objList[ sId - i ],
						     isIn ? sScale : objList[ sId - i ].transform.localScale.x, 
						     eScale, 
						     0.3f, 
						     easeType,
						    ( )=> 
								    {
									    if( onAnimComplete != null ) onAnimComplete( id );
								    } 
						    );
			    }

			    if( sId + i <  objList.Count )
			    {
				    int id = sId + i;
				    ScaleTo( objList[ sId + i ],
						     isIn ? sScale : objList[ sId + i ].transform.localScale.x, 
						     eScale, 
						     0.3f, 
						     easeType,
						    ( )=> 
								    {
									    if( onAnimComplete != null ) onAnimComplete( id );
								    } 
						    );
			    }

			    if( sId - i < 0 && sId + i >= objList.Count ) 
				    break;
			    else 
				    yield return new WaitForSeconds( 0.2f );
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator SinScaleY( GameObject obj, float dur, Action onComplete = null )
	    {
		    float t = 0;
		    float yScale, sAng = 0f, eAng = -180 * 9;
		    Transform objT   =  obj.transform;
		    float startScale = objT.localScale.y;
		    float minScale   =  0f;
		    float maxScale   = .3f;
		    float xScale     =  objT.localScale.x;

		    while( t < 1f )
		    {
			    t += Time.deltaTime / dur;
			
			    yScale = startScale + Mathf.Sin( Mathf.Lerp( sAng, eAng, t ) * 
								      Mathf.Deg2Rad ) * Mathf.Lerp( maxScale, minScale, Mathf.Pow( t, .33f ) );
			
			    objT.localScale = new Vector3( xScale, yScale, 1f );

			    yield return null;
		    }

		    if( onComplete != null ) onComplete( ); 
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator FadeAlpha( GameObject obj, float alphaTo, float dur, Action onComplete = null )
	    {
		    Image image 	= obj.GetComponent< Image >( );
		    Color startClr	= image.color;
		    Color endClr	= new Color( startClr.r, startClr.g, startClr.b, alphaTo );

		    float t = 0f;
            while( true )
            {
			    image.color = Color.Lerp( startClr, endClr, t );
			    t += Time.deltaTime / dur;

			    if( t > 1f )
			    {
				    image.color = endClr;
				    break;
			    }

                yield return null;
            }

		    if( onComplete != null ) onComplete( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator PopInPopOut( 	Transform objT, 
											    float dur, 
											    float sAng, 
											    float eAng, 
											    bool popInExtra, float maxScale = 1.2f, Action onComplete = null )
	    {
		    float t = 0;
		    float startScale = objT.localScale.x;
		    float endScale   = ( popInExtra ? startScale : 0f ) + Mathf.Sin( eAng * Mathf.Deg2Rad ) * maxScale;

		    while( true )
		    {
			    float ang = Mathf.Lerp( sAng, eAng, t );

			    float scale = ( popInExtra ? startScale : 0f ) + Mathf.Sin( ang * Mathf.Deg2Rad ) * maxScale;
			    objT.localScale = new Vector3( scale, scale, 1f );

			    t += Time.deltaTime / dur;

			    if( t > 1f ) break;
			    else		 yield return null;
		    }

		    objT.localScale = new Vector3( endScale, endScale, 1f );

		    if( onComplete != null ) onComplete( ); 
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator FlipImage( GameObject obj, 
										     float time, 
										     Action< int > midFlipAction = null, 
										     int numFlips = 1, 
										     float toScale = 0f,
										     Action onCompleteAction = null
									       )
	    {
		    float defScale	= obj.transform.localScale.x;
		    float dScale	= toScale / numFlips / 2f;
		    float tarScale 	= toScale > 0f ? dScale : defScale;
		
		    time /= ( (float) numFlips );

		    bool waitForAnimOver = false;

		    for( int i = 0; i < numFlips; i++ )
		    {
			    waitForAnimOver = true;

			    iTween.ScaleTo( obj, 
							    iTween.Hash( "x", 0f,
										     "y", toScale > 0f ? tarScale : defScale,
										     "time", time / 2f, 
										     "easetype", "linear", 
										     "oncomplete", (Action)( ( )=>
																		    {  
																			    if( midFlipAction != null ) midFlipAction( i );
																			    iTween.ScaleTo( obj, iTween.Hash( "x", toScale > 0f ? tarScale + dScale : defScale,
																											      "y", toScale > 0f ? tarScale + dScale : defScale,
																											      "time", time / 2f, 
																											      "easetype", "linear", 
																											      "oncomplete", (Action)( ( )=> { waitForAnimOver = false; } )
																											    ) );
																		    } 
																    )
										    ) );

			    while( waitForAnimOver ) yield return null;
			
			    if( toScale > 0f )
			    {
				    tarScale += dScale * 2f;
			    }
		    }

		    if( onCompleteAction != null ) onCompleteAction( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator AxisPulse( GameObject obj, Vector3 axis, float maxScale, float dur, Action onComplete = null )
	    {
		    Transform objT 			= obj.transform;
		    Vector3	  baseLocalPos 	= obj.transform.localPosition;

		    axis.Normalize( );

		    float t = 0, sAng = 0f, eAng = 180f;
		    while( t < 1f )
		    {
			    t += Time.deltaTime / dur;
			    objT.localPosition = baseLocalPos + axis * Mathf.Sin( Mathf.Lerp( sAng, eAng, t ) * Mathf.Deg2Rad ) * maxScale;
			    yield return null;
		    }

		    if( onComplete != null ) onComplete( );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void GetValue( GameObject gameObject, float sVal, float eVal, iTween.EaseType easeType, float dur, string callBack, Action onComplete = null )
	    {
		    iTween.ValueTo( gameObject,  iTween.Hash(   "from", 
													    sVal, 
													    "to", 
													    eVal, 
													    "time", 
													    dur, 
													    "onupdate",
													    callBack,
													    "oncomplete", (Action)( ( )=> { if( onComplete != null ) onComplete( ); } ) ) );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator SimpleValueChange( float sVal, float eVal, float dur, Action< float > onValueUpdate, Action onComplete = null )
	    {
		    float t = 0;
		    while( t < 1f )
		    {
			    t += Time.deltaTime / dur;
			
			    onValueUpdate( Mathf.Lerp( sVal, eVal, t ) );
			
			    yield return null;
		    }

		    if( onComplete != null ) onComplete( ); 
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static void PunchRotation( GameObject obj, Vector3 punchAmount, float time )
	    {
		    iTween.PunchRotation( obj, punchAmount, time );
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public static IEnumerator FadeAlphaInOut( Image image, 
											      float dur, 
											      bool loop, 
											      float startPerc = 0f, 
											      Action onHalfComplete = null,
											      Action onComplete = null )
	    {
		    float t = 0, sAng = 180f * startPerc, eAng = 180f;
		    Color clr = image.color;

		    if( loop )
		    {
			    float ang = 270f;
			    while( true )
			    {
				    ang += 200f * Time.deltaTime;
				    if( ang > 360f ) ang -= 360f;
				    image.color = new Color( clr.r, clr.g, clr.b, .5f + Mathf.Sin( ang * Mathf.Deg2Rad ) * .5f );
				    yield return null;
			    }
		    }
		    else
		    {
			    bool toPlayHalfwayCallBack = true;

			    while( t < 1f )
			    {
				    t += Time.deltaTime / dur;

				    if( onHalfComplete != null && t > 0.5f && toPlayHalfwayCallBack )
				    {
					    onHalfComplete( );
					    toPlayHalfwayCallBack = false;
				    }

				    image.color = new Color( clr.r, clr.g, clr.b, Mathf.Sin( Mathf.Lerp( sAng, eAng, t ) * Mathf.Deg2Rad ) );
				    yield return null;
			    }

			    image.color = new Color( clr.r, clr.g, clr.b, 0f );

			    if( onComplete != null ) onComplete( );
		    }
	    }

	    // ------------------------------------------------------------------------
	    // Name	:	-
	    // Desc	:	-
	    // ------------------------------------------------------------------------

	    public IEnumerator XInOutStackAnimation( List< GameObject > objList, 
											     List< Vector3 > defPosList, 
											     float startXOffset,
											     bool isPopIn,
											     bool isFromLeft )
	    {
		    Vector3 dV 			 = new Vector3( startXOffset, 0f );
		    bool waitForAnimOver = true;

		    int sId  = ( isPopIn ? ( isFromLeft ? objList.Count - 1 : 0 ) : ( isFromLeft ? 0 : objList.Count - 1 ) );
		    int eId  = ( isPopIn ? ( isFromLeft ? 0 : objList.Count - 1 ) : ( isFromLeft ? objList.Count - 1 : 0 ) );
		    int incr = ( isPopIn ? ( isFromLeft ? -1 : 1 ) : ( isFromLeft ? 1 : -1 ) );

		    for( int j = 0; j < objList.Count; j++ )
		    {
			    objList[ j ].transform.localPosition = defPosList[ j ] + ( isPopIn ? isFromLeft ? -dV : dV : Vector3.zero );
		    }

		    bool breakOnNext = false;

		    int i = sId;
		    while( true )
		    {
			    Vector3 pS = objList[ i ].transform.localPosition;
			    Vector3 pE = defPosList[ i ] + ( isPopIn ? Vector3.zero : isFromLeft ? -dV : dV );

			    {
				    StartCoroutine( ITweenFX.Move( objList[ i ], 
												    pS, 
												    pE, 
												    .4f, 
												    isPopIn ? iTween.EaseType.linear : iTween.EaseType.linear,
												    ( GameObject obj ) => 
												    {
													    if( isPopIn )
														    StartCoroutine( ITweenFX.AxisPulse( obj.gameObject, 
																							    isFromLeft ? Vector3.left : Vector3.right, 
																							    50f, 
																							    0.1f,
																							    ( ) => { 
																										    if( obj == objList[ eId ] ) 
																											    waitForAnimOver = false; 
																									    } 
																						    ) 
																		    );
													    else if( obj == objList[ eId ] ) waitForAnimOver = false;
												    } 
											     ) );
			    }

			    if( breakOnNext ) break;
			    else
			    {
				    i += incr;
				    if( i == eId ) breakOnNext = true;
				
				    yield return new WaitForSeconds( 0.1f );
			    }
		    }

		    while( waitForAnimOver ) yield return null;
	    }
    }
