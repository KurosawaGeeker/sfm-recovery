using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AkilliMum.Standard.Mirror
{
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	public class MirrorManager : MonoBehaviour
	{
		[Tooltip("You can use this to show useful info about what is the script is doing on the console")]
		public bool IsDebug;

		[Tooltip("Please use this to enable/disable the script. DO NOT USE script's enable/disable check box!")]
		public bool IsEnabled;

		[Tooltip("You can name your script for to understand what is it for. Because if you use a lot of reflection scripts, you can forget which one is which :)")]
		public string Name;

		[Tooltip("Please use this to enable/disable the mirror in mirror effect. Use careful for performance reasons!")]
		public bool IsMirrorInMirror;

		[Tooltip("Please use this to give unique id's to mirrors which will be drawn together. So if you want to see a mirror inside another mirror, their id must be same!")]
		public string MirrorInMirrorId;

		[Tooltip("Check this if you suffer reflection glitches. Because camera may occlude some objects according to unity settings!")]
		public bool TurnOffOcclusion;

		[Tooltip("Please select the correct platform, Normal for stand alone like desktop or mobile etc., VR for virtual reality, AR for augmented reality!")]
		public Platform Platform;

		[Tooltip("It forces to render in MultiPass, please be sure that your device is in MultiPass mode!")]
		public bool ForceMultiPass;

		[Tooltip("'Reflect' (mirror, reflective surface, transparent glass etc.) and 'Transparent' (transparent AR surface) is supported only right now.")]
		public WorkingType WorkingType;

		[Tooltip("Mirror camera background clearing option. You can set it to color if you do not want the skybox to be reflected etc.!")]
		public ClearBackGround ClearBackGround;

		[Tooltip("Clear color for camera (you may change it for some effects like blur, so it blends with the background nicely)!")]
		public Color ClearColor;

		[Tooltip("The mirror object's looking direction. Most of the time default 'GreenY' works perfectly. But try others if you get strange behavior.")]
		public UpVector UpVector;

		[Tooltip("Starts to drawing at this level of LOD. So if you do not creating perfect mirrors, you can use lower lod for surface and gain fps.")]
		[Range(0f, 10f)]
		public int CameraLODLevel;

		[Tooltip("Clips the rendering on mirror position by the percentage of the value. So if your distance from mirror is absolute (like car mirrors) you can gain very much performance!")]
		public bool UseClipping;

		[Range(1f, 100f)]
		[Tooltip("Percentage of the mirror according to screen size.")]
		public int ClippingPercentage;

		[Tooltip("Which UV will be used to draw on Texture (it is generally UV0).")]
		public ClipUV ClipUV;

		[Tooltip("If you are using VR, this will select to which eye to render to texture. For example if you mimic the car mirrors, use Left for left and interior mirror, but use Right for the right mirror for consistency!")]
		public ClipEye ClipEye;

		[Tooltip("Enables the HDR, so post effects will be visible (like bloom) on the reflection.")]
		public bool HDR;

		[Tooltip("Enables the FOV on mirror camera (if applicable). '0' means, use main camera FOV :)")]
		[Range(0f, 180f)]
		public float FOV;

		[Tooltip("Enables the anti aliasing (if only enabled in the project settings) for reflection. May decrease the performance, use with caution!")]
		public MSAALevel MSAALevel;

		[Tooltip("Should mirror camera render post effects (not recommended)")]
		public bool RenderPostProcessing;

		[Tooltip("Does mirror cam requires depth texture?")]
		public bool RequiresDepthTexture;

		[Tooltip("Does mirror cam requires opaque texture?")]
		public bool RequiresOpaqueTexture;

		[Tooltip("Filter mode to render texture (transparent render should render to Point!).")]
		public FilterMode FilterMode;

		[Tooltip("Enables mip mapping for the texture. So some effects like depth blur can work on shader. But it needs extra compute power!")]
		public bool MipMapping;

		[Tooltip("Disables the point and spot lights for the reflection. You may gain the fps, but you will lose the reality of reflection.")]
		public bool DisablePixelLights;

		[Tooltip("Enables the shadow on reflection. If you disable it; you may gain the fps, but you will lose the reality of reflection.")]
		public bool Shadow;

		[Tooltip("Enables the culling distance calculations.")]
		public bool Cull;

		[Tooltip("Cull circle distance, so it just draws the objects in the distance. You may gain the fps, but you will lose the reality of reflection.")]
		public float CullDistance;

		[Tooltip("Easy selection for reflection quality. Select 'Full' for perfect reflections! VR can render half; so select x2 etc. and try to find the best visual!")]
		public RenderTextureSize RenderTextureSize;

		[Tooltip("The size (quality) of the reflection if manual is selected above! It should be set to width of the screen for perfect reflection! But try lower values to gain fps.")]
		public double ManualSize;

		[Tooltip("Clipping distance to draw the reflection X units from the surface.")]
		public float ClipPlaneOffset;

		[Tooltip("Only these layers will be rendered by the reflection. So you can select what to be reflected with the reflection by putting them on certain layers.")]
		public LayerMask ReflectLayers;

		[Tooltip("Reflective surfaces (gameObjects) must be put here! Script calculates the reflection according to their position etc. and uses their material.")]
		public GameObject[] ReflectiveObjects;

		[Tooltip("Normally if camera does not see the objects(mirror meshes), for performance, we do not draw it. But there can be cases you always want them to be drawn. So you can check this!")]
		public bool DrawAlways;

		[Tooltip("If you want the camera to draw at a certain position (or according to another object like security cams), you can put the object here!")]
		public GameObject FakeCamera;

		[Tooltip("Enables depth render (texture), so shader can use some depth based effects.")]
		public bool EnableDepth;

		[Tooltip("Enables the advanced depth blur calculations.")]
		public bool EnableDepthBlur;

		[Tooltip("Depth blur shader to run on reflection texture.")]
		public Shader DepthBlurShader;

		public Material DepthBlurMaterial;

		[Range(0f, 30f)]
		[Tooltip("Changes the depth distance, so the objects near to the reflective surface becomes more visible or not.")]
		public float DepthBlurCutoff;

		[Range(1f, 20f)]
		[Tooltip("Runs the depth blur shader on reflection texture X times. Larger iterations make more blurry images (but may decrease the fps)!")]
		public int DepthBlurIterations;

		[Range(1f, 20f)]
		[Tooltip("This option makes the less blur on the pixels which are closer to surface. So you can make the far away pixels (on depth) more blurry!")]
		public float DepthBlurSurfacePower;

		[Range(1f, 50f)]
		[Tooltip("Normally depth blur makes a circle blur. You can change this value if you want more horizontal blur!")]
		public int DepthBlurHorizontalMultiplier;

		[Tooltip("Normally depth blur makes a circle blur. You can change this value if you want more vertical blur!")]
		[Range(1f, 50f)]
		public int DepthBlurVerticalMultiplier;

		[Tooltip("You can add your custom shader paths here (like 'Shader Graphs/MyShader') and use mirror texture on your own shader!")]
		public string[] CustomShaders;

		[Tooltip("Texture to visualize output (reflection) on a UI-texture (RawImage) etc. You can use this (for example) to mimic car mirrors on UI!")]
		public RawImage UIImage;

		public RawImage UIImageDepth;

		[NonSerialized]
		public Camera _camera;

		[Tooltip("If your scene has more than one cam, do not forget to attach your main cam here to reflect correct mirror :)")]
		public Camera _cameraAttached;

		public CameraManager _cameraManager;

		public RendererManager _rendererManager;

		public RenderTextureManager _renderTextureManager;

		public OptionManager _optionManager;

		private FrameDrawn _frameDrawn;

		protected virtual void OnEnable()
		{
		}

		public void InitializeMirror()
        {
            _cameraManager?.Destroy();
            _rendererManager?.Destroy();
            _renderTextureManager?.Destroy();
            _optionManager?.Destroy();
            _cameraManager = new CameraManager(new CameraManager.CameraSettings
            {
                IsDebug = IsDebug,
                WorkingType = WorkingType,
                ClearBackGround = ClearBackGround,
                ClearColor = ClearColor,
                Platform = Platform,
                ForceMultiPass = ForceMultiPass,
                HDR = HDR,
                FOV = FOV,
                UseClipping = UseClipping,
                ClipUV = (int)ClipUV,
                ClipEye = (int)ClipEye,
                ClippingPercentage = ClippingPercentage,
                TurnOffOcclusion = TurnOffOcclusion,
                MSAALevel = MSAALevel,
                RenderPostProcessing = RenderPostProcessing,
                RequiresDepthTexture = RequiresDepthTexture,
                RequiresOpaqueTexture = RequiresOpaqueTexture,
                Cull = Cull,
                CullDistance = CullDistance,
                ReflectLayers = ReflectLayers,
                ClipPlaneOffset = ClipPlaneOffset,
                EnableDepth = EnableDepth,
                EnableDepthBlur = EnableDepthBlur,
                DepthBlurShader = DepthBlurShader,
                DepthBlurMaterial = DepthBlurMaterial,
                DepthBlurCutoff = DepthBlurCutoff,
                DepthBlurIterations = DepthBlurIterations,
                DepthBlurSurfacePower = DepthBlurSurfacePower,
                DepthBlurHorizontalMultiplier = DepthBlurHorizontalMultiplier,
                DepthBlurVerticalMultiplier = DepthBlurVerticalMultiplier,
                FakeCamera = FakeCamera
            });
            _rendererManager = new RendererManager(ReflectiveObjects, new RendererManager.RendererSettings
            {
                IsDebug = IsDebug,
                UpVector = UpVector,
                CustomShaders = CustomShaders == null ? null : new List<string>(CustomShaders),
                DrawAlways = DrawAlways
            });
            _renderTextureManager = new RenderTextureManager(new RenderTextureManager.RenderTextureSettings
            {
                IsDebug = IsDebug,
                HDR = HDR,
                Size = RenderTextureSize,
                ManualSize = ManualSize,
                MSAALevel = MSAALevel,
                FilterMode = FilterMode,
                MipMapping = MipMapping
            });
            _optionManager = new OptionManager(new OptionManager.OptionSettings
            {
                IsDebug = IsDebug,
                LODLevel = CameraLODLevel,
                DisablePixelLights = DisablePixelLights,
                Shadow = Shadow
            });
        }

		private RenderTextureManager.RenderTextureSettings GetRenderTextureSettings()
		{
			return default;
		}

		private RendererManager.RendererSettings GetRendererSettings()
		{
			return default;
		}

		private CameraManager.CameraSettings GetCameraSettings()
		{
			return default;
		}

		private OptionManager.OptionSettings GetOptionSettings()
		{
			return default;
		}

		private void OnPreRender()
		{
		}

		public virtual IList<Camera> RenderReflective(Camera sentCamera = null, bool invert = true, bool renderCam = true)
		{
			return null;
		}

		private Camera GetCamera(Camera sentCamera)
		{
			return null;
		}

		public virtual void CommonRender()
		{
		}

		private Camera RenderMe(StereoTargetEyeMask stereoTargetEyeMask, Camera sentCamera, bool invert = true, bool renderCam = true)
		{
			return null;
		}

		protected virtual void OnDisable()
		{
		}
	}
}
