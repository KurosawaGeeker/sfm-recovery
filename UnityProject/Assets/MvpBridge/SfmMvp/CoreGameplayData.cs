using System;using System.Collections.Generic;using System.Linq;using System.Reflection;using ExposureUnnoticed2.Master.Accessory;
using ExposureUnnoticed2.Master.Action;using ExposureUnnoticed2.Master.AdultGoods;using ExposureUnnoticed2.Master.BodyPaint;using ExposureUnnoticed2.Master.Clothes;using ExposureUnnoticed2.Master.Common;using ExposureUnnoticed2.Master.Cosplay;using ExposureUnnoticed2.Master.Costume;using ExposureUnnoticed2.Master.Hair;using ExposureUnnoticed2.Master.Item;using ExposureUnnoticed2.Master.Mission;using ExposureUnnoticed2.Master.Rank;using ExposureUnnoticed2.Master.Skill;using ExposureUnnoticed2.Master.Skill.SkillSlow;using ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum;using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.IngameManager;using ExposureUnnoticed2.Object3D.Player.Scripts;using ExposureUnnoticed2.Object3D.Player.Scripts.Costume;using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;using ExposureUnnoticed2.ObjectUI.InventoryPanel;using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;using ExposureUnnoticed2.ObjectUI.TutorialPanel;using ExposureUnnoticed2.Scripts.Base;using ExposureUnnoticed2.Scripts.InGame;using ExposureUnnoticed2.Scripts.Sound;using UnityEngine;
namespace SfmMvp{
// Owned bootstrap/data presentation only. Original methods own purchases and state rules.
public sealed class CoreGameplayData:MonoBehaviour{
 public MCommon Common;public MSkillStamina StaminaRecords;public MHair Hair;public MStage Stage;public MMission Mission;public MSkill Skill;public MCosplay Cosplay;public MAdultGoods AdultGoods;public MClothes Clothes;public MAction Actions;public MRank Ranks;public MItem Items;public MCostume Costumes;public MBodyPaint Paints;public MSkillMaxAccessoryNum Capacity;
 public OnlineShopPanelView Shop{get;private set;}public InventoryPanelView Inventory{get;private set;}public bool Initialized{get;private set;}public int OriginalCosplayParts{get;private set;}
 GameObject dataHost,presentationHost,audioHost;AssetReferencer assets;
 const BindingFlags F=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance;
 public static FieldInfo Field(Type t,string n)=>t.GetField(n,F)??t.GetField("<"+n+">k__BackingField",F)??throw new Exception(t+":"+n);
 public static void Set(object o,string n,object v)=>Field(o.GetType(),n).SetValue(o,v);public static object Get(object o,string n)=>Field(o.GetType(),n).GetValue(o);
 static void Need(bool b,string n){if(!b)throw new Exception(n);}
 public AssetReferencer CreateMasterBindings(){
  Need(Common&&StaminaRecords&&Hair&&Stage&&Mission&&Skill&&Cosplay&&AdultGoods&&Clothes&&Actions&&Ranks&&Items&&Costumes&&Paints&&Capacity,"All fifteen original core masters required");
  dataHost=new GameObject("Core original master data");dataHost.SetActive(false);assets=dataHost.AddComponent<AssetReferencer>();assets.MCommon=Common;assets.MSkillStamina=StaminaRecords;assets.MHair=Hair;assets.MStage=Stage;assets.MMission=Mission;assets.MSkill=Skill;assets.MCosplay=Cosplay;assets.mAdultGoods=AdultGoods;assets.MClothes=Clothes;assets.MAction=Actions;assets.MRank=Ranks;assets.MItem=Items;assets.MCostume=Costumes;assets.MBodyPaint=Paints;assets.MSkillMaxAccessoryNum=Capacity;return assets;
 }
 GameObject Child(string name){var o=new GameObject(name);o.transform.SetParent(presentationHost.transform,false);return o;}
 static Material Mat(){var shader=Shader.Find("Hidden/SfmCoreFixture");Need(shader,"Core presentation shader missing");return new Material(shader);}
 SkinnedMeshRenderer Surface(string name){var r=Child(name).AddComponent<SkinnedMeshRenderer>();r.materials=new[]{Mat(),Mat()};return r;}
 public void BindPurchaseInputs(PlayerClassAccessor pca){
  Need(!Initialized&&ReferenceEquals(AssetReferencer.Instance,assets),"Fresh original master host required");Need(pca.PlayerState.CosplayBuffList!=null&&pca.PlayerState.CostumeBuffList!=null,"Original PlayerState constructor collections required");
  presentationHost=new GameObject("Core inactive presentation inputs");presentationHost.SetActive(false);var a=pca.PlayerAvatarObjectReferencer;var all=MCosplay.GetAll();a.CosplayControllers=new CosplayController[all.Values.Max(x=>(int)x.Type)+1];
  foreach(var series in all.Values){var c=Child("Part slots "+series.Type).AddComponent<CosplayController>();var parts=series.GetAll();var slots=new GameObject[parts.Max(x=>x.Id)+1];foreach(var part in parts){slots[part.Id]=Child("Part "+part.UniqueId);slots[part.Id].SetActive(false);OriginalCosplayParts++;}Set(c,"parts",slots);a.CosplayControllers[(int)series.Type]=c;}
  a.CensorPussy=Child("Censor flag A");a.CensorAnal=Child("Censor flag B");a.RubberSuitDummy=Child("Inactive rubber flag");a.RubberSuitDummy.SetActive(false);
  var detail=Child("Detail flag controller").AddComponent<InmouController>();Set(detail,"inmouObjects",Enumerable.Range(0,5).Select(i=>Child("Detail flag "+i)).ToArray());a.InmouController=detail;
  a.bodyMeshRenderer=Surface("Material receiver");a.FutanariMeshSao=Surface("Secondary receiver A");a.FutanariMeshTama=Surface("Secondary receiver B");assets.SweatBody=Mat();assets.FutanariMaterial=Mat();
  var bodyInput=GetComponent<BodyRuntimeData>();Need(bodyInput&&bodyInput.colors,"Original body palette input required");assets.MCustomizeColor=bodyInput.colors;
  Set(pca.PlayerCosplayManager,"pca",pca);Set(pca.PlayerBodyMaterialController,"pca",pca);Set(pca.FutanariController,"pca",pca);pca.PlayerReinforceEffectManager.Initialize(pca);pca.PlayerShoesController.Initialize(pca);
  var fog=new VisibilityFogController();fog.Initialize();Set(InGameManager.Instance,"VisibilityFogController",fog);
  var sounds=Child("Original sound manager").AddComponent<InGameSoundManager>();InGameSoundManager.Instance=sounds;Set(sounds,"Buy",new InGameSoundManager.AudioData{Clip=AudioClip.Create("Core silent purchase input",64,1,44100,false),Volume=0f});
  audioHost=new GameObject("Original UI audio");audioHost.AddComponent<AudioSource>();audioHost.AddComponent<AudioSource>();audioHost.AddComponent<UiSoundPlayer>();
  Shop=Child("Original shop purchase receiver").AddComponent<OnlineShopPanelView>();Set(Shop,"tabView",Shop.gameObject.AddComponent<CommonTabView>());Inventory=Child("Original inventory rule receiver").AddComponent<InventoryPanelView>();
  GameState.TutorialData=new TutorialData();foreach(var f in typeof(TutorialData).GetFields(F))if(f.FieldType==typeof(bool))f.SetValue(GameState.TutorialData,true);
  Localizer.LoadLocalizeDataIfNot();Need(MItem.GetAll().Count>0&&MSkillMaxAccessoryNum.GetAll().Count==4&&OriginalCosplayParts==188,"Original core tables retain their real records");Initialized=true;
 }
 public IOnlineShopItemRecord[] Catalog(int category){
  switch(category){case 0:return MClothes.GetAll().Values.Cast<IOnlineShopItemRecord>().Where(x=>x.Price>0).ToArray();case 1:return ((RCostume[])Get(Costumes,"records")).Cast<IOnlineShopItemRecord>().Where(x=>x.Price>0).ToArray();case 2:return MCosplay.GetAll().Values.SelectMany(x=>x.GetAll()).Cast<IOnlineShopItemRecord>().Where(x=>x.Price>0).ToArray();case 3:return MAdultGoods.GetAll().Values.Cast<IOnlineShopItemRecord>().Where(x=>x.Price>0).ToArray();case 4:return MHair.GetAll().Values.Cast<IOnlineShopItemRecord>().Where(x=>x.Price>0).ToArray();case 5:return MItem.GetAll().Values.Cast<IOnlineShopItemRecord>().Where(x=>x.Price>0).ToArray();case 6:return ((RBodyPaint[])Get(Paints,"records")).Cast<IOnlineShopItemRecord>().Where(x=>x.Price>0).ToArray();default:throw new ArgumentOutOfRangeException();}
 }
 public bool Owned(IOnlineShopItemRecord record){var d=GameState.GameStateData;var c=GameState.PlayerCustomData;if(record is MClothesRecord a)return d.ClothesDict.ContainsKey(a.ClothesType);if(record is RCostume b)return d.PlayerCostumeDict.ContainsKey(b.Type);if(record is RCosplayParts p)return d.PlayerCosplayPartsDict.ContainsKey(p.UniqueId);if(record is MAdultGoodsRecord e)return d.MAdultGoodsDict.ContainsKey(e.Type);if(record is MHairRecord h)return d.HairTypeList.Contains(h.Type);if(record is MItemRecord i)return d.ItemDict.TryGetValue(i.Type,out var n)&&n>0;if(record is RBodyPaint q)return c.BodyPaintTypeDict.ContainsKey(q.Type);throw new Exception("Unknown original record");}
 public int Count(IOnlineShopItemRecord record){if(record is MItemRecord i)return GameState.GameStateData.ItemDict.TryGetValue(i.Type,out var n)?n:0;return Owned(record)?1:0;}
 public string Purchase(IOnlineShopItemRecord record){
  Need(Initialized,"Core data not ready");var d=GameState.GameStateData;bool owned=Owned(record);if(record is MItemRecord i&&!i.IsValuables)owned=false;
  Set(Shop,"isBring",owned);Set(Shop,"CurrentRecord",record);Set(Shop,"playerCostumeDictCache",new Dictionary<CostumeType,bool>(d.PlayerCostumeDict));Set(Shop,"playerCosplayPartsDictCache",new Dictionary<int,bool>(d.PlayerCosplayPartsDict));Set(Shop,"playerBodyPaintDictCache",new Dictionary<BodyPaintType,bool>(GameState.PlayerCustomData.BodyPaintTypeDict));
  double before=d.CurrentRp;Shop.OnClickPurchase();if(Shop.CurrentRecord==null)return "购买成功";if(owned)return "已经拥有";if(before<record.Price)return "RP 不足";if(record is MItemRecord item&&Count(item)>=99)return "数量已达上限";return "未完成购买";
 }
 public int EquippedCount()=>GameState.GameStateData.PlayerCostumeDict.Count(x=>x.Value)+GameState.GameStateData.PlayerCosplayPartsDict.Count(x=>x.Value);
 public int CapacityCount()=>MSkillMaxAccessoryNum.Get(GameStateFacade.GetSkillLevel((SkillType)11)).MaxNum;
 void OnDestroy(){if(dataHost)Destroy(dataHost);if(presentationHost)Destroy(presentationHost);if(audioHost)Destroy(audioHost);}
}
}
