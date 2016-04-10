using UnityEngine;
using System.Collections.Generic;

using UnityStandardAssets.ImageEffects;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class HighlightsPostEffect : MonoBehaviour 
{
	#region enums
	public enum HighlightType
	{
		Glow = 0,
		Solid = 1
	}
	public enum SortingType
	{
		Overlay = 3,
		DepthFilter = 4
	}
	public enum FillType
	{
		Fill,
		Outline
	}
	public enum RTResolution
	{
		Quarter = 4,
		Half = 2,
		Full = 1
	}
	#endregion

	#region public vars

    public Renderer[] pugsRenderers;

    private Dictionary<PlayerEnum, Color> highlightColors = new Dictionary<PlayerEnum, Color>();
    private Dictionary<PlayerEnum, Material> highlightMaterials = new Dictionary<PlayerEnum, Material>();

	public HighlightType m_selectionType = HighlightType.Glow;
	public SortingType m_sortingType = SortingType.DepthFilter;	
	public FillType m_fillType = FillType.Outline;
	public RTResolution m_resolution = RTResolution.Full;

	public Shader m_highlightShader;
    public Shader m_spriteShader;

	#endregion

	#region private field

	private BlurOptimized m_blur;

	private Renderer[] highlightObjects;
	private Renderer[] m_occluders = null;
	
	private Material m_highlightMaterial;
	
	private CommandBuffer m_renderBuffer;

	private int m_RTWidth = 512;
	private int m_RTHeight = 512;

	#endregion

	private void Awake()
	{
        
        highlightColors[PlayerEnum.Player1] = Color.red;
        highlightColors[PlayerEnum.Player2] = Color.blue;
        highlightColors[PlayerEnum.Player3] = Color.yellow;
        highlightColors[PlayerEnum.Player4] = Color.cyan;

        highlightMaterials[PlayerEnum.Player1] = new Material(m_spriteShader);
        highlightMaterials[PlayerEnum.Player2] = new Material(m_spriteShader);
        highlightMaterials[PlayerEnum.Player3] = new Material(m_spriteShader);
        highlightMaterials[PlayerEnum.Player4] = new Material(m_spriteShader);

        highlightMaterials[PlayerEnum.Player1].SetColor("_Color", highlightColors[PlayerEnum.Player1]);
        highlightMaterials[PlayerEnum.Player2].SetColor("_Color", highlightColors[PlayerEnum.Player2]);
        highlightMaterials[PlayerEnum.Player3].SetColor("_Color", highlightColors[PlayerEnum.Player3]);
        highlightMaterials[PlayerEnum.Player4].SetColor("_Color", highlightColors[PlayerEnum.Player4]);



		CreateBuffers();
		CreateMaterials();
		//SetOccluderObjects();
		
		m_blur = gameObject.AddComponent<BlurOptimized>();
        m_blur.blurShader = Shader.Find("Hidden/FastBlur");
		m_blur.enabled = false;

		//GameObject[] occludees = GameObject.FindGameObjectsWithTag(m_occludeesTag);
		//highlightObjects = new Renderer[occludees.Length];


		//for( int i = 0; i < occludees.Length; i++ )
		//	highlightObjects[i] = occludees[i].GetComponent<Renderer>();

		m_RTWidth = (int) (Screen.width / (float) m_resolution);
		m_RTHeight = (int) (Screen.height / (float) m_resolution);
	}

	private void CreateBuffers()
	{
		m_renderBuffer = new CommandBuffer();
	}

	private void ClearCommandBuffers()
	{
		m_renderBuffer.Clear();
	}
	
	private void CreateMaterials()
	{
		m_highlightMaterial = new Material( m_highlightShader );
	}
       
	
	private void RenderHighlights( RenderTexture rt)
	{
		RenderTargetIdentifier rtid = new RenderTargetIdentifier(rt);
		m_renderBuffer.SetRenderTarget( rtid );


        for(int i = 0; i < pugsRenderers.Length; i++)
		{
            m_renderBuffer.SetGlobalColor("_PugColor", highlightColors[(PlayerEnum) i]);
            m_renderBuffer.DrawRenderer( pugsRenderers[i], m_highlightMaterial, 0, 5);
		}

        //for(int i = 0; i < LevelGrid.Instance.takenGridCells.Count; i++)
        //{
        //    GridCell cell = LevelGrid.Instance.takenGridCells[i];
        //
        //    m_renderBuffer.SetGlobalColor("_PugColor", highlightColors[cell.owner]);
        //    m_renderBuffer.DrawRenderer( cell.renderer, m_highlightMaterial, 0, 5);
        //}

		RenderTexture.active = rt;
		Graphics.ExecuteCommandBuffer(m_renderBuffer);
		RenderTexture.active = null;
	}
	
	private void RenderOccluders( RenderTexture rt)
	{
		if( m_occluders == null )
			return;

		RenderTargetIdentifier rtid = new RenderTargetIdentifier(rt);
		m_renderBuffer.SetRenderTarget( rtid );

		m_renderBuffer.Clear();
		
		foreach(Renderer renderer in m_occluders)
		{	
			m_renderBuffer.DrawRenderer( renderer, m_highlightMaterial, 0, (int) m_sortingType );
		}

		RenderTexture.active = rt;
		Graphics.ExecuteCommandBuffer(m_renderBuffer);
		RenderTexture.active = null;
	}


	/// Final image composing.
	/// 1. Renders all the highlight objects either with Overlay shader or DepthFilter
	/// 2. Downsamples and blurs the result image using standard BlurOptimized image effect
	/// 3. Renders occluders to the same render texture
	/// 4. Substracts the occlusion map from the blurred image, leaving the highlight area
	/// 5. Renders the result image over the main camera's G-Buffer
	private void OnRenderImage( RenderTexture source, RenderTexture destination )
	{
		RenderTexture highlightRT;

        RenderTexture.active = highlightRT = RenderTexture.GetTemporary(m_RTWidth, m_RTHeight, 0, RenderTextureFormat.ARGB32 );
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = null;

		ClearCommandBuffers();

		RenderHighlights(highlightRT);

        RenderTexture blurred = RenderTexture.GetTemporary( m_RTWidth, m_RTHeight, 0, RenderTextureFormat.ARGB32 );


		m_blur.OnRenderImage( highlightRT, blurred );

	
		RenderOccluders(highlightRT);

		if( m_fillType == FillType.Outline )
		{
            RenderTexture occluded = RenderTexture.GetTemporary( m_RTWidth, m_RTHeight, 0, RenderTextureFormat.ARGB32);

			// Excluding the original image from the blurred image, leaving out the areal alone
			m_highlightMaterial.SetTexture("_OccludeMap", highlightRT);
			Graphics.Blit( blurred, occluded, m_highlightMaterial, 2 );

			m_highlightMaterial.SetTexture("_OccludeMap", occluded);

			RenderTexture.ReleaseTemporary(occluded);

		}
		else
		{
			m_highlightMaterial.SetTexture("_OccludeMap", blurred);
		}
            
		Graphics.Blit (source, destination, m_highlightMaterial, (int) m_selectionType);


		RenderTexture.ReleaseTemporary(blurred);
		RenderTexture.ReleaseTemporary(highlightRT);
	}
}
