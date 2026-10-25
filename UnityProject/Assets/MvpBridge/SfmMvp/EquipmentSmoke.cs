using System;using System.Collections;using System.Collections.Generic;using System.IO;using System.Security.Cryptography;using UnityEngine;
namespace SfmMvp{
[DefaultExecutionOrder(250)]public sealed class EquipmentSmoke:MonoBehaviour{
 [Serializable]public sealed class Report{public bool passed,methodHooks,originalSavesAccessed,originalGeneratedCallback,originalSerializedButtonEvent;public string failure,gameSHA256;public int checks;public List<string>checksPassed=new List<string>(),errors=new List<string>();public string[]scope;}
 public static bool Done{get;private set;}public static bool Passed{get;private set;}
 [Serializable]sealed class BodyReport{public bool passed;public int checks;public string gameSHA256;}
 readonly Report r=new Report();string output;bool observing,finished;EquipmentBootstrapDraft qa;
 void Error(string m,string s,LogType t){if(t==LogType.Error||t==LogType.Exception||t==LogType.Assert)r.errors.Add(t+":"+m);}
 void Need(bool v,string text){r.checks++;if(!v)throw new InvalidOperationException(text);r.checksPassed.Add(text);}
 static string Hash(string p){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(File.ReadAllBytes(p))).Replace("-","");}
 IEnumerator Start(){var args=Environment.GetCommandLineArgs();if(Array.IndexOf(args,"-sfmEquipmentSmoke")<0)yield break;
  Application.logMessageReceived+=Error;observing=true;
  try{int i=Array.IndexOf(args,"-sfmActionOutput");Need(i>=0&&i+1<args.Length,"Shared fresh combined output required");output=Path.GetFullPath(args[i+1]);Need(Directory.Exists(output)&&!File.Exists(Path.Combine(output,"equipment-runtime.json")),"Fresh equipment runtime report");qa=GetComponent<EquipmentBootstrapDraft>();Need(qa&&!qa.RunQa,"Single owned coordinator; standalone draft runner disabled");r.gameSHA256=Hash(typeof(ExposureUnnoticed2.Scripts.InGame.GameStateFacade).Assembly.Location);}catch(Exception e){Finish(e);yield break;}
  float started=Time.realtimeSinceStartup;while(!File.Exists(Path.Combine(output,"body-runtime.json"))&&Time.realtimeSinceStartup-started<20)yield return null;
  try{Need(File.Exists(Path.Combine(output,"body-runtime.json")),"Original body and seven-category shop completed before equipment");var body=JsonUtility.FromJson<BodyReport>(File.ReadAllText(Path.Combine(output,"body-runtime.json")));Need(body.passed&&body.checks==33&&body.gameSHA256==r.gameSHA256,"Same actual actor passed33 original body checks");qa.Player=GetComponent<OriginalMovementMvpBridge>().BoundPlayer;Need(qa.Player&&qa.Player.Pca!=null,"Use same actual runtime controller/PCA");}catch(Exception e){Finish(e);yield break;}
  // Only traverse the owned QA enumerators to capture errors. Engine yields are
  // yielded unchanged; the existing original frame pump alone runs Scheduler.
  var stack=new Stack<IEnumerator>();stack.Push(qa.Qa());
  while(stack.Count>0){object current=null;bool more=false;Exception error=null;try{more=stack.Peek().MoveNext();if(more)current=stack.Peek().Current;}catch(Exception e){error=e;}
   if(error!=null){while(stack.Count>0){var d=stack.Pop()as IDisposable;if(d!=null)d.Dispose();}Finish(error);yield break;}
   if(!more){var d=stack.Pop()as IDisposable;if(d!=null)d.Dispose();continue;}
   var nested=current as IEnumerator;if(nested!=null&&!(current is CustomYieldInstruction))stack.Push(nested);else yield return current;
  }
  yield return new WaitForEndOfFrame();try{Need(qa.Passed&&r.errors.Count==0,"Original generated-button equip/unequip pipeline passed without runtime errors");var slow=GetComponent<EquipmentSlowBinding>();Need(slow&&slow.Ready,"Original Slow initialization retained for full equipment transaction");slow.VerifyDisposeBeforeAction();Need(slow.Passed,"Original Slow Dispose verified before unchanged action regression");var image=ScreenCapture.CaptureScreenshotAsTexture();try{Need(image&&image.width==1280&&image.height==720,"Real visible equipment runtime screenshot");File.WriteAllBytes(Path.Combine(output,"equipment-runtime.png"),image.EncodeToPNG());}finally{if(image)Destroy(image);}r.originalGeneratedCallback=true;r.originalSerializedButtonEvent=true;r.passed=true;Finish(null);}catch(Exception e){Finish(e);}
 }
 void Finish(Exception error){if(finished)return;finished=true;if(error!=null)r.failure=error.ToString();if(qa){r.checks+=qa.Checks;r.checksPassed.AddRange(qa.Evidence);}r.scope=new[]{"Same actual original actor after seven priced purchases and body33; original Type6 bought for50RP, equipped and unequipped via original serialized Unity Button event→View→original generated panel callback→full original transaction/CalcEffect/Apply/Fog","Original eleven attachment slots with nine object identities, original masters/audio, natural original Scheduler frames; no Game method hooks and no original saves","Owned inactive panel input fixture and temporary reinforcement sentinel; original panel Open/Initialize/OnDestroy lifecycle, full other equipment categories and complete UI remain separate"};if(observing)Application.logMessageReceived-=Error;if(output!=null)File.WriteAllText(Path.Combine(output,"equipment-runtime.json"),JsonUtility.ToJson(r,true)+"\n");Passed=r.passed;Done=true;if(!Passed){Debug.LogError("SFM_EQUIPMENT_SMOKE "+r.failure);Application.Quit(5);}}
}
}
