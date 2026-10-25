using UnityEngine;using ExposureUnnoticed2.Scripts.InGame;
namespace SfmMvp{
[DefaultExecutionOrder(10)]public sealed class BodyCustomizePanel:MonoBehaviour{
 public BodyRuntimeData data;public OriginalMovementMvpBridge bridge;bool open;Font font;Vector2 scroll;static BodyCustomizePanel active;
 Rect Area=>new Rect(15,112,365,Mathf.Min(560,Screen.height-125));
 public static bool PointerOverPanel=>active&&active.open&&active.Area.Contains(new Vector2(Input.mousePosition.x,Screen.height-Input.mousePosition.y));
 public void SetOpen(bool value){open=value;if(value){bridge.legacyMotor.ControlsCaptured=false;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;}}
 void Start(){active=this;}
 void Update(){if(!data.Ready)return;if(Input.GetKeyDown(KeyCode.F7))SetOpen(!open);if(open&&bridge.legacyMotor.ControlsCaptured)SetOpen(true);}
 void Slider(string label,string field,int min=0,int max=100){int previous=data.GetInt(field);GUILayout.BeginHorizontal();GUILayout.Label(label,GUILayout.Width(100));GUILayout.Label(previous.ToString(),GUILayout.Width(35));int next=Mathf.RoundToInt(GUILayout.HorizontalSlider(previous,min,max));GUILayout.EndHorizontal();if(next!=previous)data.SetInt(field,next);}
 void OnGUI(){if(!data.Ready)return;if(!font)font=Font.CreateDynamicFontFromOSFont("Microsoft YaHei",16);GUI.skin.font=font;
  if(GUI.Button(new Rect(15,72,245,32),"身体调节 [F7]"))SetOpen(!open);if(!open)return;
  GUILayout.BeginArea(Area,GUI.skin.box);GUILayout.BeginHorizontal();GUILayout.Label("原始身体调节代码");if(GUILayout.Button("关闭",GUILayout.Width(55)))SetOpen(false);GUILayout.EndHorizontal();GUILayout.Label("临时面板；参数通过原始事件处理。服装同步仍在恢复。");scroll=GUILayout.BeginScrollView(scroll);
  Slider("身高","CustomizeHeight",120,200);Slider("头部大小","CustomizeHead");bool adjust=GUILayout.Toggle(GameState.PlayerCustomData.CustomizeAdjustHeight,"按身高调节头部比例");if(adjust!=GameState.PlayerCustomData.CustomizeAdjustHeight)data.SetAdjustHeight(adjust);
  Slider("颈部长度","CustomizeNeck");Slider("颈部粗细","CustomizeNeckThick");Slider("肩部","CustomizeShoulder");Slider("腰部","CustomizeWaist");Slider("大腿粗细","CustomizeThighs");Slider("大腿长度","CustomizeThighsLength");Slider("小腿粗细","CustomizeLowerLeg");Slider("小腿长度","CustomizeLowerLegLength");Slider("上身长度","CustomizeUpperBodyLength");Slider("手臂粗细","CustomizeArm");Slider("肤色","CustomizeSkinColor",0,data.colors.SkinPattern.Length-1);Slider("眼睛样式","CustomizeEyePattern",0,data.colors.EyeTextures.Length-1);
  GUILayout.EndScrollView();if(GUILayout.Button("恢复原始默认参数"))data.ResetOriginalDefaults();GUILayout.EndArea();
 }
 void OnDestroy(){if(ReferenceEquals(active,this))active=null;if(font)Destroy(font);}
}
}
