using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SfmMvp
{
    // Resource replay mode for a legitimate Mono Player template. No original game code is present.
    public sealed class MvpOriginalBundleLoader : MonoBehaviour
    {
        private const string PlayerAsset = "assets/zzz-externalencryptasset/player.prefab";
        private const string ModelsSha = "98621b2c4fa4f9de80f355fae5776ab17efc016bcc81c581bcd0917af4308367";
        private static bool started;
        private string bundlePath;
        private AssetBundle bundle;

        public static void Begin(string requested)
        {
            if (started) return;
            string path = Path.GetFullPath(requested);
            if (path.IndexOf("ReverseEngineering", StringComparison.OrdinalIgnoreCase) < 0 || !File.Exists(path))
                throw new InvalidOperationException("Models bundle must be an existing owned ReverseEngineering copy.");
            started = true;
            var owner = new GameObject("MvpOriginalResourceLoader");
            DontDestroyOnLoad(owner);
            var loader = owner.AddComponent<MvpOriginalBundleLoader>(); loader.bundlePath = path;
            loader.StartCoroutine(loader.Coordinate());
        }

        private IEnumerator Coordinate()
        {
            IEnumerator run = Load();
            while (true)
            {
                bool next = false; object current = null; Exception failure = null;
                try { next = run.MoveNext(); if (next) current = run.Current; }
                catch (Exception error) { failure = error; }
                if (failure != null)
                { Debug.LogError("SFM_MVP resource_mode_failed " + failure); Application.Quit(4); yield break; }
                if (!next) yield break;
                yield return current;
            }
        }

        private IEnumerator Load()
        {
            using (var stream = File.OpenRead(bundlePath))
            using (var algorithm = SHA256.Create())
            {
                string hash = BitConverter.ToString(algorithm.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                if (hash != ModelsSha) throw new InvalidOperationException("Owned models bundle SHA mismatch.");
            }
            MvpBootstrap.Log("resource_mode_started", "Owned decrypted UnityFS verified; no original GameAssembly or save logic.");
            if (SceneManager.sceneCountInBuildSettings < 2)
                throw new InvalidOperationException("Resource replay data has no original Apartment buildIndex1.");
            AsyncOperation loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            if (loading == null) throw new InvalidOperationException("Original Apartment scene load request failed.");
            while (!loading.isDone) yield return null;
            yield return null;
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name.IndexOf("Apartment", StringComparison.OrdinalIgnoreCase) < 0)
                throw new InvalidOperationException("BuildIndex1 did not load original Apartment: " + scene.name);
            MvpBootstrap.Log("original_apartment_loaded", "buildIndex1 name=" + scene.name + "; source native resources only.");
            bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null) throw new InvalidOperationException("Owned original models bundle did not load.");
            GameObject prefab = bundle.LoadAsset<GameObject>(PlayerAsset);
            if (prefab == null) throw new InvalidOperationException("Original Player asset not found: " + PlayerAsset);
            GameObject player = Instantiate(prefab);
            player.name = "OriginalPlayer_MvpResourceReplay";
            player.SetActive(false);
            foreach (Animator animator in player.GetComponentsInChildren<Animator>(true)) animator.enabled = false;
            foreach (Rigidbody body in player.GetComponentsInChildren<Rigidbody>(true)) { body.isKinematic=true; body.useGravity=false; }
            foreach (Camera old in FindObjectsOfType<Camera>()) old.enabled=false;
            foreach (AudioListener old in FindObjectsOfType<AudioListener>()) old.enabled=false;
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller == null) controller = player.AddComponent<CharacterController>();
            foreach (Collider collider in player.GetComponentsInChildren<Collider>(true)) if (collider!=controller) collider.enabled=false;
            controller.enabled=false; controller.height=1.8f; controller.radius=.3f; controller.center=new Vector3(0,.9f,0);
            controller.stepOffset=.25f; controller.skinWidth=.04f;
            // Retain every original child active flag. Enable only the prefab root for the temporary MVP.
            player.SetActive(true);
            if (player.GetComponentsInChildren<Renderer>().Length == 0)
                throw new InvalidOperationException("Original native active hierarchy contains no visible renderer candidates.");
            Physics.SyncTransforms();
            player.transform.position=NativeSpawn(scene,player);
            controller.enabled=true;
            var motor=player.AddComponent<MvpPlayerMotor>();
            var cameraObject=new GameObject("MvpThirdPersonCamera",typeof(Camera),typeof(AudioListener),typeof(MvpOrbitCamera));
            cameraObject.tag="MainCamera";
            Camera camera=cameraObject.GetComponent<Camera>(); camera.nearClipPlane=.05f; camera.farClipPlane=500f; camera.fieldOfView=60f;
            var orbit=cameraObject.GetComponent<MvpOrbitCamera>(); orbit.Bind(motor); motor.BindView(cameraObject.transform);
            if (!FindObjectsOfType<Light>().Any(light=>light.enabled))
            {
                Light light=new GameObject("MvpFallbackLight",typeof(Light)).GetComponent<Light>();
                light.type=LightType.Directional;light.intensity=.8f;light.transform.rotation=Quaternion.Euler(45,-30,0);
            }
            RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight=new Color(.45f,.45f,.45f,1);
            Application.targetFrameRate=60;
            MvpBootstrap.Log("original_character_present", "container="+PlayerAsset+"; activeRenderers="+player.GetComponentsInChildren<Renderer>().Length+"; animatorsDisabled=true; childActiveFlagsPreserved=true");
            MvpBootstrap.Log("camera_bound", "Resource replay uses same temporary motor/orbit/QA as Editor generated MVP.");
            MvpSmoke.TryStart(motor,orbit);
            // Keep AssetBundle loaded; unloading its assets would invalidate the actual native character.
        }

        private static Vector3 NativeSpawn(Scene scene,GameObject player)
        {
            var all=scene.GetRootGameObjects().SelectMany(root=>root.GetComponentsInChildren<Transform>(true)).Where(t=>!t.IsChildOf(player.transform)).ToArray();
            foreach(string name in new[]{"SpawnPoint","PlayerStart","PlayerSpawn","StartPosition","PlayerStartPoint"})
            {
                Transform marker=all.FirstOrDefault(t=>String.Equals(t.name,name,StringComparison.OrdinalIgnoreCase));
                if(marker==null)continue;
                Vector3 at=marker.position+Vector3.up*.08f;
                foreach(RaycastHit hit in Physics.RaycastAll(at+Vector3.up*.2f,Vector3.down,3f,~0,QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance))
                {
                    if(hit.transform==null||hit.transform.IsChildOf(player.transform)||hit.normal.y<.75f)continue;
                    at=hit.point+Vector3.up*.1f;break;
                }
                MvpBootstrap.Log("native_spawn_selected", "marker="+marker.name+" world="+at.ToString("F3"));
                return at;
            }
            var renderers=all.SelectMany(t=>t.GetComponents<Renderer>()).Where(r=>r.enabled).ToArray();
            if(renderers.Length==0)throw new InvalidOperationException("Original scene has no native map renderers.");
            Bounds bounds=renderers[0].bounds;
            foreach(Renderer renderer in renderers.Skip(1))bounds.Encapsulate(renderer.bounds);
            foreach(Vector2 offset in new[]{Vector2.zero,Vector2.right,Vector2.left,Vector2.up,Vector2.down})
            {
                Vector3 origin=new Vector3(bounds.center.x+offset.x,bounds.max.y+2,bounds.center.z+offset.y);
                foreach(RaycastHit hit in Physics.RaycastAll(origin,Vector3.down,bounds.size.y+10,~0,QueryTriggerInteraction.Ignore).OrderByDescending(h=>h.point.y))
                {
                    if(hit.transform==null||hit.transform.IsChildOf(player.transform)||hit.normal.y<.75f)continue;
                    Vector3 at=hit.point+Vector3.up*.1f;
                    if(Physics.CheckCapsule(at+Vector3.up*.36f,at+Vector3.up*1.44f,.3f,~0,QueryTriggerInteraction.Ignore))continue;
                    MvpBootstrap.Log("native_spawn_selected", "nativeFloor="+hit.transform.name+" world="+at.ToString("F3"));return at;
                }
            }
            throw new InvalidOperationException("No native floor spawn with capsule clearance; no test plane substituted.");
        }
    }
}
