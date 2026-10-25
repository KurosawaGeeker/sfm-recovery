using System;using ExposureUnnoticed2.ObjectUI.InventoryPanel;using ExposureUnnoticed2.Scripts.Base;using ExposureUnnoticed2.Scripts.InGame;using UnityEngine;
namespace SfmMvp{
[DefaultExecutionOrder(-100)]public sealed class CoreGameplayPanel:MonoBehaviour{
 public CoreGameplayData data;public OriginalMovementMvpBridge bridge;public bool Ready{get;private set;}public string Failure{get;private set;}
 static CoreGameplayPanel active;
 public static bool PointerOverPanel=>(active!=null&&active.open&&active.PanelRect.Contains(new Vector2(Input.mousePosition.x,Screen.height-Input.mousePosition.y)))||BodyCustomizePanel.PointerOverPanel||EquipmentManualPanel.PointerOverPanel;
 Rect PanelRect=>new Rect(Screen.width-440,52,425,Mathf.Min(610,Screen.height-65));
 public void SetInventoryPage(){page=1;SetOpen(true);}bool open;int page,category;Vector2 scroll;string status="按 F8 打开商店与背包，F9 增加测试资金";Font font;readonly string[] names={"衣服","服装配件","套装部件","装备","发型","道具","身体涂料"};
 void Start(){active=this;try{if(!bridge.Ready)throw new Exception(bridge.Failure);data.BindPurchaseInputs(bridge.BoundAccessor);Ready=true;}catch(Exception e){Failure=e.ToString();Debug.LogError("SFM_CORE_INIT "+Failure);}}
 public void SetOpen(bool value){open=value;if(value){bridge.legacyMotor.ControlsCaptured=false;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;}}
 public void AddTestFunds(){GameState.GameStateData.CurrentRp+=500;status="已添加 500 RP 测试资金";}
 void Update(){if(!Ready)return;if(Input.GetKeyDown(KeyCode.F8))SetOpen(!open);if(Input.GetKeyDown(KeyCode.F9))AddTestFunds();if(open&&bridge.legacyMotor.ControlsCaptured)SetOpen(true);}
 void OnGUI(){
  if(!Ready)return;if(!font)font=Font.CreateDynamicFontFromOSFont("Microsoft YaHei",16);GUI.skin.font=font;
  if(GUI.Button(new Rect(Screen.width-260,12,245,32),"商店 / 背包  [F8]    RP "+GameState.GameStateData.CurrentRp.ToString("0.##")))SetOpen(!open);
  Rect rect=PanelRect;if(!open)return;
  GUILayout.BeginArea(rect,GUI.skin.box);GUILayout.Label("核心玩法测试");GUILayout.BeginHorizontal();if(GUILayout.Toggle(page==0,"商店",GUI.skin.button))page=0;if(GUILayout.Toggle(page==1,"背包",GUI.skin.button))page=1;if(GUILayout.Button("关闭",GUILayout.Width(55)))SetOpen(false);GUILayout.EndHorizontal();
  GUILayout.Label("RP "+GameState.GameStateData.CurrentRp.ToString("0.##")+"    装备栏 "+data.EquippedCount()+" / "+data.CapacityCount());GUILayout.Label("购买状态已接入；装备操作与外观同步仍在恢复。");
  category=GUILayout.SelectionGrid(category,names,4);scroll=GUILayout.BeginScrollView(scroll);
  foreach(var record in data.Catalog(category)){bool owned=data.Owned(record);if(page==1&&!owned)continue;GUILayout.BeginHorizontal(GUI.skin.box);GUILayout.Label(Localizer.Trans(record.NameKey),GUILayout.Width(205));if(page==0){GUILayout.Label(record.Price+" RP",GUILayout.Width(65));if(GUILayout.Button(owned?"已有 "+data.Count(record):"购买",GUILayout.Width(90)))try{status=data.Purchase(record);}catch(Exception e){Failure=e.ToString();status="购买流程错误，已记录";Debug.LogError("SFM_CORE_PURCHASE "+Failure);}}else GUILayout.Label("数量 "+data.Count(record));GUILayout.EndHorizontal();}
  GUILayout.EndScrollView();GUILayout.Label(status);if(GUILayout.Button("添加 500 RP 测试资金 [F9]"))AddTestFunds();GUILayout.EndArea();
 }
 void OnDestroy(){if(ReferenceEquals(active,this))active=null;if(font)Destroy(font);}
}
}
