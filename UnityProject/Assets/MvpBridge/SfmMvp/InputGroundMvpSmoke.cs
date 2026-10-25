using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
namespace SfmMvp
{
    // Owned scenario only. QA queues InputSystem events; original game methods
    // own all movement after a clearly marked one-time test-platform placement.
    public sealed class InputGroundMvpSmoke : MonoBehaviour
    {
        const string Stage=@"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\original-input-ground-mvp-candidate-v1";
        [Serializable] public sealed class Sample {
            public string phase; public int frame,updates,fixedCalls,inputUpdates,groundCallbacks,frontCallbacks;
            public Vector3 position,velocity,resolvedMove; public bool grounded,frontGrounded,ready;
            public float stamina,speed,time,fixedTime,animatorSpeed,animatorX,animatorY;
            public bool ccGrounded,animatorStrafe;
        }
        [Serializable] public sealed class MethodPin {public string method,sha256;}
        [Serializable] public sealed class Result {
            public bool passed,actual_Unity_frames=true,queued_input_events=true,physical_keyboard_tested,ground_flags_written;
            public bool saw_ground,saw_forward_edge,saw_airborne,saw_downward_velocity,saw_landing,stamina_increased,runtime_method_IL_unchanged;
            public string failure,unity_version,company,product;
            public string scope="Original input, selected normal walking, Stamina regeneration and two original ground checkers; owned sensor geometry, scenario, camera and animator upload. Not full gameplay.";
            public string placement="One explicit CC-disabled placement onto a remote owned QA platform, after the Apartment walk. No position writes during walk-off/fall/landing observations.";
            public float apartment_displacement,platform_height_drop;
            public int visible_renderers,active_renderers;
            public List<Sample> samples=new List<Sample>();
            public List<string> error_categories=new List<string>();
            public List<MethodPin> methods_before,methods_after;
            public Sample final;
        }
        OriginalMovementMvpBridge bridge; MvpOrbitCamera orbit; string output;
        Result result=new Result(); bool done; float started;
        public static void TryStart(OriginalMovementMvpBridge b,MvpOrbitCamera o) {
            var args=Environment.GetCommandLineArgs();if(Array.IndexOf(args,"-sfmInputGroundSmoke")<0)return;
            int i=Array.IndexOf(args,"-sfmSmokeOutput");if(i<0 || i+1>=args.Length)throw new Exception("Missing QA output");
            string p=Path.GetFullPath(args[i+1]);
            if(!p.StartsWith(Stage+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||Directory.Exists(p))throw new Exception("Fresh owned output required");
            Directory.CreateDirectory(p);
            var qa=new GameObject("Input ground MVP QA").AddComponent<InputGroundMvpSmoke>();
            qa.bridge=b;qa.orbit=o;qa.output=p;qa.started=Time.realtimeSinceStartup;
            qa.result.unity_version=Application.unityVersion;qa.result.company=Application.companyName;qa.result.product=Application.productName;
            Application.logMessageReceived+=qa.Log;
            qa.StartCoroutine(qa.Run());
        }
        void Log(string message,string stack,LogType type) {
            if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert)result.error_categories.Add(type+":"+(stack!=null && stack.Contains("OriginalMovementMvpBridge")?"bridge":"runtime"));
        }
        void Update(){if(!done && Time.realtimeSinceStartup-started>50f)Finish("QA timeout");}
        Sample Capture(string phase) {
            if(bridge==null||!bridge.Ready)throw new Exception("Bridge failed: "+bridge?.Failure);
            var animator=bridge.visibleAnimator;
            var s=new Sample{phase=phase,frame=Time.frameCount,updates=bridge.OriginalUpdateCalls,fixedCalls=bridge.OriginalFixedCalls,inputUpdates=bridge.InputUpdates,groundCallbacks=bridge.GroundCallbacks,frontCallbacks=bridge.FrontCallbacks,position=bridge.transform.position,velocity=bridge.OriginalMoveVector,resolvedMove=bridge.ResolvedMove,grounded=bridge.Grounded,frontGrounded=bridge.FrontGrounded,ready=bridge.Ready,stamina=bridge.StaminaCurrent,speed=bridge.OriginalMoveSpeed,time=Time.time,fixedTime=Time.fixedTime,ccGrounded=bridge.GetComponent<CharacterController>().isGrounded,animatorSpeed=animator.GetFloat("Speed"),animatorX=animator.GetFloat("StrafeX"),animatorY=animator.GetFloat("StrafeY"),animatorStrafe=animator.GetBool("IsStrafe")};
            result.samples.Add(s);return s;
        }
        static void Require(bool condition,string text){if(!condition)throw new Exception(text);}
        List<MethodPin> Methods() {
            var selected=new Dictionary<Type,string[]> {
                {typeof(InputManager),new[]{"Awake","OnUpdate"}}, {typeof(InputResolver),new[]{"Resolve"}},
                {typeof(StaminaManager),new[]{"OnUpdate"}}, {typeof(DownhillChecker),new[]{"Awake","FixedUpdate","OnTriggerStay","get_IsFrontDownhill"}},
                {typeof(PlayerLocomotion),new[]{"OnUpdate","OnFixedUpdate"}}, {typeof(PlayerController),new[]{"OnFixedUpdate"}}, {typeof(PlayerAnimationManager),new[]{"SetStrafe"}}
            };
            var rows=new List<MethodPin>();
            foreach(var type in selected)foreach(string name in type.Value){var method=type.Key.GetMethod(name,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);using(var sha=SHA256.Create())rows.Add(new MethodPin{method=type.Key.FullName+"::"+name,sha256=BitConverter.ToString(sha.ComputeHash(method.GetMethodBody().GetILAsByteArray())).Replace("-","")});}
            return rows;
        }
        void Platform(string name,Vector3 center,Vector3 size,Color color) {
            var p=GameObject.CreatePrimitive(PrimitiveType.Cube);p.name=name;p.transform.position=center;p.transform.localScale=size;
            p.GetComponent<Renderer>().material.color=color;
        }
        IEnumerator Scenario() {
            Require(bridge!=null && bridge.Ready,"Bridge not ready");
            result.methods_before=Methods();
            bridge.SetOwnedQaInput(Vector2.zero);
            for(int i=0;i<30;i++){yield return new WaitForEndOfFrame();Capture("apartment-idle");}
            Sample initial=Capture("apartment-walk-start");ScreenCapture.CaptureScreenshot(Path.Combine(output,"apartment-start.png"));
            bridge.SetOwnedQaInput(Vector2.up);
            for(int i=0;i<55;i++){yield return new WaitForEndOfFrame();Capture("apartment-walk");}
            Sample moved=Capture("apartment-walk-end");Vector3 delta=moved.position-initial.position;delta.y=0;
            result.apartment_displacement=delta.magnitude;
            ScreenCapture.CaptureScreenshot(Path.Combine(output,"apartment-walk.png"));
            bridge.SetOwnedQaInput(Vector2.zero);
            for(int i=0;i<30;i++){yield return new WaitForEndOfFrame();Capture("apartment-stop");}
            Require(result.apartment_displacement>.05f,"Original-input Apartment walk did not move");
            foreach(var r in bridge.GetComponentsInChildren<Renderer>(true))if(r.enabled&&r.gameObject.activeInHierarchy){result.active_renderers++;if(r.isVisible)result.visible_renderers++;}
            // Marked setup placement only; the observed test starts after settling.
            Vector3 center=new Vector3(1000f,100f,1000f);
            Platform("Owned QA upper step",center+Vector3.down*.2f,new Vector3(2f,.4f,2f),Color.gray);
            Platform("Owned QA lower landing",center+Vector3.down*1.1f,new Vector3(20f,.4f,20f),Color.blue);
            var cc=bridge.GetComponent<CharacterController>();cc.enabled=false;
            bridge.transform.position=center+Vector3.up*(.05f-cc.center.y+cc.height*.5f);cc.enabled=true;
            Physics.SyncTransforms();orbit.Bind(bridge.legacyMotor);
            for(int i=0;i<35;i++){yield return new WaitForEndOfFrame();Capture("platform-settle");}
            Sample top=Capture("platform-walk-start");Require(top.grounded && top.frontGrounded,"Both original sensors must detect upper platform");
            result.saw_ground=true;bridge.SetOwnedQaInput(Vector2.up);
            for(int i=0;i<170;i++){
                yield return new WaitForEndOfFrame();Sample s=Capture("walk-off-and-land");
                if(s.grounded && !s.frontGrounded)result.saw_forward_edge=true;
                if(!s.grounded && top.position.y-s.position.y>.06f)result.saw_airborne=true;
                if(!s.grounded && s.velocity.y<0f)result.saw_downward_velocity=true;
                if(result.saw_airborne && s.grounded && top.position.y-s.position.y>.5f){result.saw_landing=true;result.platform_height_drop=top.position.y-s.position.y;break;}
            }
            bridge.SetOwnedQaInput(Vector2.zero);
            for(int i=0;i<35;i++){yield return new WaitForEndOfFrame();Capture("landed-stop");}
            ScreenCapture.CaptureScreenshot(Path.Combine(output,"platform-landed.png"));
            Sample final=Capture("final");result.stamina_increased=final.stamina>initial.stamina;
            Require(result.saw_forward_edge&&result.saw_airborne&&result.saw_downward_velocity&&result.saw_landing,"Original sensors/gravity did not complete edge, fall and landing");
            Require(result.stamina_increased,"Original Stamina regeneration did not increase");
            Require(final.inputUpdates>initial.inputUpdates && final.fixedCalls>initial.fixedCalls && final.groundCallbacks>0,"Missing actual original dispatch/collisions");
            Require(result.active_renderers==4 && result.visible_renderers>0,"Apartment character was not visibly rendered");
            Require(Mathf.Abs(final.speed)<.00001f,"Original walking did not stop");
            for(int i=0;i<5;i++)yield return new WaitForEndOfFrame();
            result.final=Capture("actual-last-frame");
            Require(result.final.grounded && result.final.frontGrounded && result.final.frontCallbacks>0,"Both real sensors must observe final landing");
            Require(result.final.resolvedMove==Vector3.zero && new Vector2(result.final.velocity.x,result.final.velocity.z)==Vector2.zero && result.final.speed==0f,"Final original input and movement must stop");
            Require(result.final.animatorSpeed==0f && result.final.animatorX==0f && result.final.animatorY==0f && !result.final.animatorStrafe,"Final animator values must stop");
            result.methods_after=Methods();
            result.runtime_method_IL_unchanged=result.methods_before.Zip(result.methods_after,(a,b)=>a.method==b.method && a.sha256==b.sha256).All(x=>x);
            Require(result.runtime_method_IL_unchanged,"Original runtime IL changed");
        }
        IEnumerator Run() {
            var sequence=Scenario();
            while(!done){bool more=false;object current=null;string failure=null;
                try{more=sequence.MoveNext();if(more)current=sequence.Current;}catch(Exception e){failure=e.ToString();}
                if(failure!=null){Finish(failure);yield break;}if(!more){Finish(null);yield break;}yield return current;
            }
        }
        void Finish(string failure) {
            if(done)return;done=true;result.failure=failure;result.passed=failure==null && result.error_categories.Count==0;
            if(bridge!=null&&bridge.Ready)bridge.ReleaseOwnedQaInput();
            File.WriteAllText(Path.Combine(output,"input-ground-smoke.json"),JsonUtility.ToJson(result,true));
            Application.logMessageReceived-=Log;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
            Application.Quit(result.passed?0:3);
        }
    }
}
