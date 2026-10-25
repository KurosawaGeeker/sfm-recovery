using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SfmMvp
{
 [DefaultExecutionOrder(-20000)]
 public sealed class PamEngineSmoke:MonoBehaviour
 {
  public string output,expectedPamUpdate,expectedPamHelper;
  [Serializable]public class Scalar{public string name,bits;public float value;}
  [Serializable]public class Smooth{public string name,current_bits,target_bits;public float current,target;}
  [Serializable]public class Sample{public int frame,main,input,fixedCalls;public string phase,pulse_bits;public float time,delta,scale,pulse;public Vector3 position,move;public Vector2 real_action_move;public bool keyboard_w_pressed,keyboard_enabled,action_canMove,state_fixed,state_fixedWarp,state_ecstasy,ui_fade,ui_cancel;public bool crouch,strafe;public List<Scalar> parameters=new List<Scalar>();public List<Scalar> layers=new List<Scalar>();public List<Smooth> smooth=new List<Smooth>();public string[] playing_clips;}
  [Serializable]public class Result{public bool passed,actual_Unity=true,real_PAM_no_hooks=true,old_three_uploads_removed=true,original_graph_restored=false,cold_original_bootstrap_restored=false;public int original_methods_candidate=2,bridge_original_methods_recovered=0,checks,samples_count,pulse_once_checks;public string failure,unity,company,product,pam_update_IL,pam_helper_IL;public float displacement;public Sample final;public List<Sample> samples=new List<Sample>();public List<string> errors=new List<string>();public string scope="Original main/PAM/Handcuffs/Smooth on owned neutral state; real Animator14 params/11 layers with owned no-motion sinks, original Avatar/idle-walk; not full original graph.";}
  Result result=new Result();OriginalMovementMvpBridge bridge;PlayerAnimationManager pam;bool seeded,done;float started,lastPulse,oldScale,oldFixed;int lastMain;Vector3 origin;bool queuedWalk,queuedStop;
  const BindingFlags F=BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic;
  static object Get(object o,string n)=>o.GetType().GetField(n,F).GetValue(o);
  static void Set(object o,string n,object v)=>o.GetType().GetField(n,F).SetValue(o,v);
  static string Bits(float f)=>BitConverter.ToUInt32(BitConverter.GetBytes(f),0).ToString("X8");
  static string Pin(string n){var m=typeof(PlayerAnimationManager).GetMethod(n,F);using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(m.GetMethodBody().GetILAsByteArray())).Replace("-","");}
  void Require(bool b,string m){result.checks++;if(!b)throw new Exception(m);}
  void Awake(){Application.logMessageReceived+=Log;bridge=GetComponent<OriginalMovementMvpBridge>();oldScale=Time.timeScale;oldFixed=Time.fixedDeltaTime;started=Time.realtimeSinceStartup;result.unity=Application.unityVersion;result.company=Application.companyName;result.product=Application.productName;}
  void Log(string m,string s,LogType t){if(t==LogType.Error||t==LogType.Exception||t==LogType.Assert)result.errors.Add(t+":"+m);}
  void Start(){try{Require(!Directory.Exists(output),"Fresh output");Directory.CreateDirectory(output);Require(bridge!=null&&bridge.Ready,"Ready bridge:"+bridge?.Failure);pam=(PlayerAnimationManager)Get(bridge,"animation");Require(TimeScaleManager.Instance!=null,"Real clock receiver");Require(bridge.visibleAnimator.layerCount==11&&bridge.visibleAnimator.parameters.Length==14,"Real compatible graph shape");Require(Pin("OnUpdate")==expectedPamUpdate&&Pin("UpdateHandcuffsWeight")==expectedPamHelper,"Actual PAM two-body pins");result.pam_update_IL=expectedPamUpdate;result.pam_helper_IL=expectedPamHelper;
   Set(pam,"pulseTargetArmLayerWeight",.75f);((SmoothFloat)Get(pam,"strafeX")).Current=.4f;((SmoothFloat)Get(pam,"strafeY")).Current=-.2f;lastPulse=.75f;lastMain=bridge.OriginalUpdateCalls;origin=bridge.transform.position;bridge.SetOwnedQaInput(Vector2.zero);seeded=true;started=Time.realtimeSinceStartup;
  }catch(Exception e){Finish(e.ToString());}}
  void LateUpdate(){if(done)return;try{if(!seeded){if(Time.realtimeSinceStartup-started>10)Finish("Startup timeout");return;}Require(bridge.Ready,"Bridge failed:"+bridge.Failure);float elapsed=Time.realtimeSinceStartup-started;string phase=elapsed<.35f?"input-warmup":elapsed<1.25f?"walk":elapsed<2.2f?"stop":elapsed<2.7f?"positive-half-scale":"restore-stop";if(elapsed>=.35f&&!queuedWalk){bridge.SetOwnedQaInput(Vector2.up);queuedWalk=true;}if(elapsed>=1.25f&&!queuedStop){bridge.SetOwnedQaInput(Vector2.zero);queuedStop=true;}
   var a=bridge.visibleAnimator;var sample=new Sample{frame=Time.frameCount,main=bridge.OriginalUpdateCalls,input=bridge.InputUpdates,fixedCalls=bridge.OriginalFixedCalls,phase=phase,time=Time.time,delta=Time.deltaTime,scale=Time.timeScale,position=bridge.transform.position,move=bridge.ResolvedMove,crouch=a.GetBool("IsCrouch"),strafe=a.GetBool("IsStrafe")};
   sample.real_action_move=InputManager.GetInputAction(InputManager.InputType.Move).ReadValue<Vector2>();var keyboard=(Keyboard)Get(bridge,"qaKeyboard");sample.keyboard_w_pressed=keyboard.wKey.isPressed;sample.keyboard_enabled=keyboard.enabled;var state=(PlayerStateModel)Get(bridge,"state");sample.action_canMove=state.CurrentAction.CanMove;sample.state_fixed=state.IsFixedPositionRotation;sample.state_fixedWarp=state.IsFixedWarpPositionRotation;sample.state_ecstasy=state.IsEcstasyMotion;var ui=Get(bridge,"inputUi");sample.ui_fade=(bool)ui.GetType().GetProperty("IsOpenFade",F).GetValue(ui,null);sample.ui_cancel=(bool)ui.GetType().GetProperty("IsOpenSomeCancelInputUi",F).GetValue(ui,null);
   Require(sample.main==sample.input&&sample.main-lastMain==1,"One actual original main/input per observed frame");lastMain=sample.main;sample.pulse=(float)Get(pam,"pulseTargetArmLayerWeight");sample.pulse_bits=Bits(sample.pulse);if(lastPulse>0){float expected=lastPulse-.1f;if(expected<0)expected=0;Require(Bits(expected)==sample.pulse_bits,"Original pulse detects exactly one PAM tick");result.pulse_once_checks++;}lastPulse=sample.pulse;
   foreach(var n in new[]{"coatStateA","coatStateB","headLayerWeight","faceLayerWeight","armLayerWeight","overrideArmForHandcuffLayerWeight","strafeX","strafeY","dokidoki","moveSpeedBlend","handcaffsWeightBlend","anotherMotionIndex","locomotionMotionSpeed"}){var s=(SmoothFloat)Get(pam,n);sample.smooth.Add(new Smooth{name=n,current=s.Current,target=s.Target,current_bits=Bits(s.Current),target_bits=Bits(s.Target)});}
   foreach(var n in new[]{"CoatState","CoatStateB","Speed","BreastSize","StrafeX","StrafeY","Dokidoki","PlayerEcstasy","MotionSpeedScaleWhenSlow","AnotherMotionIndex","LocomotionMotionSpeed","HandcuffsWeight"}){float v=a.GetFloat(n);sample.parameters.Add(new Scalar{name=n,value=v,bits=Bits(v)});}
   foreach(var i in new List<int>{3,8,10}){float v=a.GetLayerWeight(i);sample.layers.Add(new Scalar{name=i.ToString(),value=v,bits=Bits(v)});}
   var mapping=new Dictionary<string,string>{{"CoatState","coatStateA"},{"CoatStateB","coatStateB"},{"Speed","moveSpeedBlend"},{"StrafeX","strafeX"},{"StrafeY","strafeY"},{"Dokidoki","dokidoki"},{"AnotherMotionIndex","anotherMotionIndex"},{"LocomotionMotionSpeed","locomotionMotionSpeed"},{"HandcuffsWeight","handcaffsWeightBlend"}};
   foreach(var p in mapping)Require(sample.parameters.Single(x=>x.name==p.Key).bits==sample.smooth.Single(x=>x.name==p.Value).current_bits,"Genuine Animator Current readback:"+p.Key);
   Require(sample.layers.Single(x=>x.name=="3").bits==sample.smooth.Single(x=>x.name=="armLayerWeight").current_bits,"Actual arm layer");Require(sample.layers.Single(x=>x.name=="8").bits==sample.smooth.Single(x=>x.name=="overrideArmForHandcuffLayerWeight").current_bits,"Actual hand layer");Require(sample.layers.Single(x=>x.name=="10").bits==sample.smooth.Single(x=>x.name=="faceLayerWeight").current_bits,"Actual face layer");Require(sample.parameters.Single(x=>x.name=="BreastSize").value==0f,"Neutral body input");
   sample.playing_clips=a.GetCurrentAnimatorClipInfo(0).Where(x=>x.weight>0&&x.clip!=null).Select(x=>x.clip.name).ToArray();result.samples.Add(sample);
   if(elapsed>3.1f){var d=sample.position-origin;d.y=0;result.displacement=d.magnitude;Require(result.displacement>.04f,"Actual original movement");Require(result.pulse_once_checks>=7,"PAM pulse single-call windows");Require(sample.move==Vector3.zero&&bridge.OriginalMoveSpeed==0f,"Original final movement stopped");Require(sample.parameters.Single(x=>x.name=="Speed").value==0f,"Original final smooth animator speed stopped");Require(sample.playing_clips.Any(x=>x.Contains("Idle")),"Original idle clip evaluates");Require(result.samples.Any(x=>x.playing_clips.Any(n=>n.Contains("Walk"))),"Original walk clip evaluates");Require(result.samples.Any(x=>x.parameters.Single(p=>p.name=="MotionSpeedScaleWhenSlow").value==2f),"Original half-scale reciprocal through real engine");Require(Pin("OnUpdate")==expectedPamUpdate&&Pin("UpdateHandcuffsWeight")==expectedPamHelper,"Two runtime IL bodies unchanged");Finish(null);}
   Time.timeScale=phase=="positive-half-scale"?.5f:1f;
   Thread.Sleep(15); // Owned QA pacing only: real Unity clock/physics, never replace Time.
  }catch(Exception e){Finish(e.ToString());}}
  void Finish(string failure){if(done)return;done=true;result.failure=failure;result.passed=failure==null&&result.errors.Count==0;result.samples_count=result.samples.Count;result.final=result.samples.LastOrDefault();Time.timeScale=oldScale;Time.fixedDeltaTime=oldFixed;Application.logMessageReceived-=Log;try{if(bridge!=null&&bridge.Ready)bridge.ReleaseOwnedQaInput();Directory.CreateDirectory(output);File.WriteAllText(Path.Combine(output,"pam-engine-smoke.json"),JsonUtility.ToJson(result,true));}finally{
#if UNITY_EDITOR
   UnityEditor.EditorApplication.isPlaying=false;
#else
   Application.Quit(result.passed?0:3);
#endif
  }}
 }
}
