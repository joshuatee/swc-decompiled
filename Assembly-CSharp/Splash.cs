using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using WinRTBridge;

public class Splash : MonoBehaviour, IUnitySerializable
{
	private const string PREF_SFX_VOLUME = "prefSfxVolume";

	private const string MAIN_SCENE = "MainScene";

	private const string STARTUP_SOUND = "stingerStartup";

	private const string UX_ROOT = "UX Root";

	private const string UX_CAMERA_NAME = "UX Camera";

	private const float FADE_DURATION = 0.4f;

	private float[] durations;

	private GameObject uxRoot;

	private int state;

	private float startTime;

	private AudioSource source;

	private float fadePerc;

	private bool transitioningOut;

	private Camera uxCamera;

	public static float CalculateScale()
	{
		float num;
		if (Screen.height >= 640)
		{
			if (Screen.height < 640 || Screen.height > 768)
			{
				if (Screen.height != 1080)
				{
					if (Screen.height != 1440)
					{
						num = (float)Screen.height / 768f;
						num = Mathf.Floor(num * 4f) * 0.25f;
					}
				}
			}
		}
		num = 1f;
		int num2 = Screen.width * Screen.height;
		if (num2 > 2073600)
		{
			num = (float)(num2 / 2073600);
		}
		return num;
	}

	private void Start()
	{
		float num = Splash.CalculateScale();
		this.uxCamera = GameObject.Find("UX Camera").GetComponent<Camera>();
		this.uxCamera.transform.localScale = Vector3.one * num;
		NGUIText.fontResolutionMultiplier = num;
		this.uxRoot = GameObject.Find("UX Root");
		this.uxRoot.GetComponent<UIPanel>().alpha = 0f;
		this.durations = new float[]
		{
			0f,
			0.75f,
			1f,
			0.5f,
			0.5f
		};
		base.gameObject.AddComponent<AudioListener>();
		AudioClip clip = Resources.Load("stingerStartup") as AudioClip;
		this.source = base.gameObject.AddComponent<AudioSource>();
		this.source.clip = clip;
		this.source.volume = this.GetSfxVolume();
	}

	private float GetSfxVolume()
	{
		if (!PlayerPrefs.HasKey("prefSfxVolume"))
		{
			return 1f;
		}
		return PlayerPrefs.GetFloat("prefSfxVolume");
	}

	private void Update()
	{
		if (Time.time >= this.startTime)
		{
			this.state++;
			switch (this.state)
			{
			case 1:
				this.FadeIn();
				break;
			case 3:
				this.FadeOut();
				break;
			case 4:
				this.transitioningOut = true;
				break;
			case 5:
				this.Finish();
				return;
			}
			this.startTime += this.durations[this.state];
		}
		if (this.transitioningOut)
		{
			if (this.fadePerc < 1f)
			{
				this.fadePerc += Time.deltaTime / 0.4f;
				return;
			}
			this.transitioningOut = false;
		}
	}

	private void FadeIn()
	{
		TweenAlpha.Begin(this.uxRoot, this.durations[this.state], 1f);
		this.source.Play();
	}

	private void FadeOut()
	{
		TweenAlpha.Begin(this.uxRoot, this.durations[this.state], 0f);
	}

	private void Finish()
	{
		SceneManager.LoadScene("MainScene");
	}

	public Splash()
	{
	}

	public override void Unity_Serialize(int depth)
	{
	}

	public override void Unity_Deserialize(int depth)
	{
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public override void Unity_NamedSerialize(int depth)
	{
	}

	public override void Unity_NamedDeserialize(int depth)
	{
	}

	protected internal Splash(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(Splash.CalculateScale());
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).FadeIn();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).FadeOut();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Finish();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((Splash)GCHandledObjects.GCHandleToObject(instance)).GetSfxVolume());
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((Splash)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
