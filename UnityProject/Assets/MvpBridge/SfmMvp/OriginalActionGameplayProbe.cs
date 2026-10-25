using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;

namespace SfmMvp
{
    // Owned observation/commands only. Real Player main, action methods,
    // Animator, original events and Scheduler own the actual state changes.
    public sealed class OriginalActionEventObservation : MonoBehaviour
    {
        public int endEvents, lastEndFrame, footEvents;
        public void CommonOnEndAnim() { endEvents++; lastEndFrame=Time.frameCount; }
        public void FootL(AnimationEvent value) { footEvents++; }
        public void FootR(AnimationEvent value) { footEvents++; }
    }

    [DefaultExecutionOrder(-20000)]
    public sealed class OriginalActionGameplayProbe : MonoBehaviour
    {
        public MAction originalMaster;
        [Serializable] public sealed class Sample
        {
            public string phase;
            public int frame,main,input,fixedCalls,action,animatorAction,oldAction,currentState,nextState,endEvents;
            public bool canMove,transition,ready;
            public float time,speed,normalizedTime;
            public Vector3 position,move,rightHandLocal;
        }
        [Serializable] public sealed class Report
        {
            public bool passed,actualUnity=true,originalGameNativeLoaded=false,originalSavesAccessed=false;
            public bool commonTimerPassed,originalEventChainPassed,movementAfterEventPassed;
            public string failure,unity,product,gameSHA256;
            public int checks,masterRecords,parameters,layers,commonCallbacks,commonReturnFrame,eventFrame,eventReturnFrame,observedEndEvents;
            public float commonBlockedDisplacement,afterCommonDisplacement,afterEventDisplacement;
            public List<Sample> samples=new List<Sample>();
            public List<string> errors=new List<string>();
            public string[] scope;
        }
        OriginalMovementMvpBridge bridge;
        OriginalActionEventObservation observer;
        Report report=new Report();
        bool smoke,eventPhase,finished;
        string output;
        float started;
        int priorMain;

        void Awake()
        {
            var args=Environment.GetCommandLineArgs();
            smoke=Array.IndexOf(args,"-sfmOriginalActionSmoke")>=0;
            eventPhase=Array.IndexOf(args,"-sfmOriginalActionEvents")>=0;
            started=Time.realtimeSinceStartup;
            Application.logMessageReceived+=ObserveLog;
            if(smoke)
            {
                int k=Array.IndexOf(args,"-sfmActionOutput");
                if(k<0||k+1>=args.Length)throw new Exception("Fresh action output required");
                output=Path.GetFullPath(args[k+1]);
                if(Directory.Exists(output)||File.Exists(output))throw new Exception("Fresh action output directory required");
                Directory.CreateDirectory(output);
                Time.captureDeltaTime=1f/60f;Time.fixedDeltaTime=1f/60f;
                QualitySettings.vSyncCount=0;Application.targetFrameRate=60;Application.runInBackground=true;
            }
        }
        void ObserveLog(string message,string stack,LogType kind)
        {
            if(kind==LogType.Error||kind==LogType.Exception||kind==LogType.Assert)report.errors.Add(kind+": "+message);
        }
        void Require(bool condition,string reason) { report.checks++;if(!condition)throw new InvalidOperationException(reason); }
        static string Hash(byte[] bytes) { using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(bytes)).Replace("-",""); }
        IEnumerator Start()
        {
            bridge=GetComponent<OriginalMovementMvpBridge>();
            try
            {
                Require(bridge!=null&&bridge.Ready,"Bridge startup: "+(bridge==null?"missing":bridge.Failure));
                Require(originalMaster!=null,"Original MAction asset required");
                bridge.BindOriginalActionData(originalMaster);
                observer=bridge.visibleAnimator.gameObject.AddComponent<OriginalActionEventObservation>();
                report.unity=Application.unityVersion;report.product=Application.productName;
                report.gameSHA256=Hash(File.ReadAllBytes(typeof(PlayerController).Assembly.Location));
                report.parameters=bridge.visibleAnimator.parameters.Length;report.layers=bridge.visibleAnimator.layerCount;
                report.masterRecords=((System.Collections.IDictionary)typeof(MAction).GetField("dict",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).GetValue(null)).Count;
                report.scope=new[]{"Original Player main once per frame, actual original MAction records, full repaired controller and original humanoid Avatar",
                    "Owned neutral bootstrap, camera and requests; not complete original startup or washing-machine interaction",
                    "CommonEquip completion is the original PlayerAction time comparison, not an invented animation-end callback",
                    "Optional IntoWasher completion must come from the original clip event and original Scheduler callback; harness never calls CommonOnEndAnim or cancels it manually",
                    "Original FootL/FootR receivers remain declared incomplete dependencies; no footstep audio restoration claim"};
                Require(report.parameters==40&&report.layers==12&&report.masterRecords==78,"Original graph/master shape");
                if(!smoke){bridge.visibleAnimator.fireEvents=true;yield break;}
                bridge.SetOwnedQaInput(Vector2.zero);priorMain=bridge.OriginalUpdateCalls;
            }
            catch(Exception e){Finish(e.ToString());yield break;}
            // This owned QA gate orders the actual equipment loop before the unchanged action regression.
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-sfmEquipmentSmoke")>=0){float wait=Time.realtimeSinceStartup;while(!EquipmentSmoke.Done&&Time.realtimeSinceStartup-wait<25f)yield return null;if(!EquipmentSmoke.Passed){Finish("Equipment integration did not pass before action regression");yield break;}yield return new WaitForEndOfFrame();priorMain=bridge.OriginalUpdateCalls;started=Time.realtimeSinceStartup;yield return null;}
            yield return Guarded(Run());
        }
        IEnumerator Guarded(IEnumerator routine)
        {
            var stack=new Stack<IEnumerator>();stack.Push(routine);
            while(stack.Count>0&&!finished)
            {
                object current=null;bool moved=false;Exception error=null;
                try{moved=stack.Peek().MoveNext();if(moved)current=stack.Peek().Current;}catch(Exception e){error=e;}
                if(error!=null){Finish(error.ToString());yield break;}
                if(!moved){stack.Pop();continue;}
                var nested=current as IEnumerator;if(nested!=null)stack.Push(nested);else yield return current;
            }
        }
        IEnumerator Frame(string phase)
        {
            yield return new WaitForEndOfFrame();
            Require(bridge.Ready,"Original main failed: "+bridge.Failure);
            Require(bridge.OriginalUpdateCalls==priorMain+1,"Exactly one original main dispatch per frame");priorMain=bridge.OriginalUpdateCalls;
            var a=bridge.visibleAnimator;var state=a.GetCurrentAnimatorStateInfo(1);var hand=a.GetBoneTransform(HumanBodyBones.RightHand);
            report.samples.Add(new Sample{phase=phase,frame=Time.frameCount,time=Time.time,main=bridge.OriginalUpdateCalls,input=bridge.InputUpdates,fixedCalls=bridge.OriginalFixedCalls,
                action=(int)bridge.BoundState.CurrentAction.Type,canMove=bridge.BoundState.CurrentAction.CanMove,animatorAction=a.GetInteger("ActionId"),oldAction=a.GetInteger("OldActionId"),
                currentState=state.fullPathHash,nextState=a.GetNextAnimatorStateInfo(1).fullPathHash,normalizedTime=state.normalizedTime,transition=a.IsInTransition(1),
                speed=bridge.OriginalMoveSpeed,position=bridge.transform.position,move=bridge.OriginalMoveVector,rightHandLocal=hand==null?Vector3.zero:a.transform.InverseTransformPoint(hand.position),endEvents=observer.endEvents,ready=bridge.Ready});
            Require(Time.realtimeSinceStartup-started<35,"Action smoke deadline");
        }
        static float Horizontal(Vector3 a,Vector3 b){return new Vector2(a.x-b.x,a.z-b.z).magnitude;}
        void Capture(string label)
        {
            var image=ScreenCapture.CaptureScreenshotAsTexture();Require(image!=null,"Real rendered screenshot");
            try{File.WriteAllBytes(Path.Combine(output,label+".png"),image.EncodeToPNG());}finally{Destroy(image);}
        }
        IEnumerator Run()
        {
            for(int i=0;i<30;i++)yield return Frame("warmup");
            Vector3 before=bridge.transform.position;Vector3 lastBlocked=before;
            bridge.BoundState.CommonEquipEndTime=Time.time+1f;
            bridge.BoundState.OnEndCommonEquip=()=>report.commonCallbacks++;
            bridge.BoundAccessor.PlayerAction.ChangeAction(ActionType.CommonEquip);
            Require(bridge.BoundState.CurrentAction.Type==ActionType.CommonEquip,"Real ChangeAction accepted original CommonEquip record");
            bridge.SetOwnedQaInput(Vector2.up);
            for(int i=0;i<125;i++)
            {
                yield return Frame("common-equip");
                if(bridge.BoundState.CurrentAction.Type==ActionType.CommonEquip)lastBlocked=bridge.transform.position;
                else if(report.commonReturnFrame==0)report.commonReturnFrame=Time.frameCount;
                if(i==30)Capture("common-equip");
            }
            report.commonBlockedDisplacement=Horizontal(before,lastBlocked);
            report.afterCommonDisplacement=Horizontal(lastBlocked,bridge.transform.position);
            Require(report.commonCallbacks==1&&bridge.BoundState.CurrentAction.Type==ActionType.None,"Original common timer and ChangeAction(None) completed once");
            Require(report.commonBlockedDisplacement<.025f&&report.afterCommonDisplacement>.15f,"Original action blocks movement then restores it");
            report.commonTimerPassed=true;
            bridge.SetOwnedQaInput(Vector2.zero);for(int i=0;i<35;i++)yield return Frame("common-stop");
            bridge.BoundState.OnEndCommonEquip=null;
            if(eventPhase)
            {
                observer.endEvents=0;observer.lastEndFrame=0;
                bridge.visibleAnimator.fireEvents=true;
                bridge.BoundAccessor.PlayerAction.ChangeAction(ActionType.IntoWasher);
                Require(bridge.BoundState.CurrentAction.Type==ActionType.IntoWasher,"Real ChangeAction accepted original IntoWasher record");
                before=bridge.transform.position;
                for(int i=0;i<420;i++)
                {
                    yield return Frame("original-event-action");
                    if(i==75)Capture("original-action");
                    if(bridge.BoundState.CurrentAction.Type==ActionType.None)
                    {report.eventReturnFrame=Time.frameCount;break;}
                }
                report.eventFrame=observer.lastEndFrame;report.observedEndEvents=observer.endEvents;
                Require(observer.endEvents==1&&bridge.BoundState.CurrentAction.Type==ActionType.None,"Original clip CommonOnEndAnim naturally returned action to None");
                Require(report.eventReturnFrame>report.eventFrame&&report.eventReturnFrame-report.eventFrame<=6,"Original scheduled completion occurred after event");
                Require(Horizontal(before,bridge.transform.position)<.025f,"Event action did not move actor through owned controls");
                report.originalEventChainPassed=true;
                bridge.visibleAnimator.fireEvents=false;
                // The first walk ended at the real entrance collision. Walk
                // back into the room to measure regained control in free space.
                bridge.SetOwnedQaInput(Vector2.down);Vector3 walkStart=bridge.transform.position;
                for(int i=0;i<75;i++)yield return Frame("after-event-walk");
                report.afterEventDisplacement=Horizontal(walkStart,bridge.transform.position);
                Require(report.afterEventDisplacement>.10f,"Control movement restored after original event completion");report.movementAfterEventPassed=true;
                bridge.SetOwnedQaInput(Vector2.zero);for(int i=0;i<30;i++)yield return Frame("final-stop");
            }
            Capture("final");Require(report.errors.Count==0,"Unity runtime errors");Finish(null);
        }
        void Update()
        {
            if(smoke||finished||bridge==null||!bridge.Ready)return;
            if(Input.GetKeyDown(KeyCode.Q)&&bridge.BoundState.CurrentAction.Type==ActionType.None)
                bridge.BoundAccessor.PlayerAction.ChangeAction(ActionType.IntoWasher);
        }
        void OnGUI()
        {
            if(smoke)return;
            GUI.Box(new Rect(15,115,430,45),"Q: play throwing gesture, then return to WASD movement");
        }
        void Finish(string error)
        {
            if(finished)return;finished=true;
            if(bridge!=null&&bridge.Ready)bridge.ReleaseOwnedQaInput();
            report.failure=error;report.passed=error==null&&report.errors.Count==0;
            Application.logMessageReceived-=ObserveLog;
            if(output!=null)File.WriteAllText(Path.Combine(output,"action-gameplay.json"),JsonUtility.ToJson(report,true)+"\n");
            Debug.Log("SFM_ORIGINAL_ACTION_GAMEPLAY passed="+report.passed+" common="+report.commonTimerPassed+" event="+report.originalEventChainPassed);
            if(smoke)Application.Quit(report.passed?0:3);
        }
    }
}
