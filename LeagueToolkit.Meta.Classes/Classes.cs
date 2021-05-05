using System.Numerics;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.Meta;
using System.Collections.Generic;
using LeagueToolkit.Meta.Attributes;
using LeagueToolkit.IO.PropertyBin;
namespace LeagueToolkit.Meta.Classes
{
    [MetaClass(284479679)]
    public class Class0x10f4d0bf : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
    }
    [MetaClass("TempTable2Table")]
    public class TempTable2Table : ScriptTable
    {
    }
    [MetaClass("CharacterHealthBarDataRecord")]
    public class CharacterHealthBarDataRecord : IMetaClass
    {
        [MetaProperty("unitHealthBarStyle", BinPropertyType.Byte)]
        public byte UnitHealthBarStyle { get; set; } = 0;
        [MetaProperty("attachToBone", BinPropertyType.String)]
        public string AttachToBone { get; set; } = "";
        [MetaProperty("hpPerTick", BinPropertyType.Float)]
        public float HpPerTick { get; set; } = 0f;
        [MetaProperty(1722275594, BinPropertyType.Bool)]
        public bool m1722275594 { get; set; } = false;
        [MetaProperty(2131456110, BinPropertyType.Bool)]
        public bool m2131456110 { get; set; } = false;
        [MetaProperty(2346514948, BinPropertyType.Bool)]
        public bool m2346514948 { get; set; } = false;
        [MetaProperty("showWhileUntargetable", BinPropertyType.Bool)]
        public bool ShowWhileUntargetable { get; set; } = false;
        [MetaProperty(2622563520, BinPropertyType.Bool)]
        public bool m2622563520 { get; set; } = true;
        [MetaProperty(3884244271, BinPropertyType.UInt32)]
        public uint m3884244271 { get; set; } = 0;
    }
    [MetaClass("MapVisibilityFlagRange")]
    public class MapVisibilityFlagRange : IMetaClass
    {
        [MetaProperty("minIndex", BinPropertyType.Byte)]
        public byte MinIndex { get; set; } = 0;
        [MetaProperty("maxIndex", BinPropertyType.Byte)]
        public byte MaxIndex { get; set; } = 0;
    }
    [MetaClass("TFTHudAnnouncementData")]
    public class TFTHudAnnouncementData : IMetaClass
    {
        [MetaProperty("mSceneTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionIn { get; set; } = new (new ());
        [MetaProperty("mSceneTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionOut { get; set; } = new (new ());
    }
    [MetaClass("X3DSharedConstantDef")]
    public class X3DSharedConstantDef : IMetaClass
    {
        [MetaProperty("register", BinPropertyType.Int32)]
        public int Register { get; set; } = -1;
        [MetaProperty("Count", BinPropertyType.UInt32)]
        public uint Count { get; set; } = 1;
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
        [MetaProperty("SetManually", BinPropertyType.Bool)]
        public bool SetManually { get; set; } = false;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("PlatformMask", BinPropertyType.UInt32)]
        public uint PlatformMask { get; set; } = 0;
    }
    [MetaClass("IGameCalculationPartByCharLevel")]
    public interface IGameCalculationPartByCharLevel : IGameCalculationPart
    {
    }
    [MetaClass("TimeMultiplierCheat")]
    public class TimeMultiplierCheat : Cheat
    {
        [MetaProperty("mSpeedDown", BinPropertyType.Bool)]
        public bool SpeedDown { get; set; } = false;
        [MetaProperty("mSpeedUp", BinPropertyType.Bool)]
        public bool SpeedUp { get; set; } = false;
    }
    [MetaClass("StatUIData")]
    public class StatUIData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mAbbreviation", BinPropertyType.String)]
        public string Abbreviation { get; set; } = "";
        [MetaProperty("mIconKey", BinPropertyType.String)]
        public string IconKey { get; set; } = "";
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        public string ScalingTagKey { get; set; } = "";
        [MetaProperty("mDisplayType", BinPropertyType.Byte)]
        public byte DisplayType { get; set; } = 0;
    }
    [MetaClass("OptionTemplateLabel")]
    public class OptionTemplateLabel : IOptionTemplate
    {
        [MetaProperty("Label1", BinPropertyType.Hash)]
        public MetaHash Label1 { get; set; } = new(0);
        [MetaProperty("Label2", BinPropertyType.Hash)]
        public MetaHash Label2 { get; set; } = new(0);
    }
    [MetaClass("FloatOffsetTableGet")]
    public class FloatOffsetTableGet : IFloatGet
    {
        [MetaProperty("offset", BinPropertyType.Float)]
        public float Offset { get; set; } = 0f;
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
    }
    [MetaClass("DeathTimes")]
    public class DeathTimes : IMetaClass
    {
        [MetaProperty("mAllowRespawnMods", BinPropertyType.Bool)]
        public bool AllowRespawnMods { get; set; } = true;
        [MetaProperty("mStartDeathTimerForZombies", BinPropertyType.Bool)]
        public bool StartDeathTimerForZombies { get; set; } = true;
        [MetaProperty("mScalingStartTime", BinPropertyType.UInt32)]
        public uint ScalingStartTime { get; set; } = 0;
        [MetaProperty("mScalingPercentCap", BinPropertyType.Float)]
        public float ScalingPercentCap { get; set; } = 0f;
        [MetaProperty("mScalingPercentIncrease", BinPropertyType.Float)]
        public float ScalingPercentIncrease { get; set; } = 0f;
        [MetaProperty("mTimeDeadPerLevel", BinPropertyType.Container)]
        public MetaContainer<float> TimeDeadPerLevel { get; set; } = new();
        [MetaProperty("mScalingIncrementTime", BinPropertyType.UInt32)]
        public uint ScalingIncrementTime { get; set; } = 0;
        [MetaProperty("mScalingPoints", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DeathTimesScalingPoint>> ScalingPoints { get; set; } = new();
    }
    [MetaClass("AboveHealthPercentCastRequirement")]
    public class AboveHealthPercentCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mCurrentPercentHealth", BinPropertyType.Float)]
        public float CurrentPercentHealth { get; set; } = 0f;
    }
    [MetaClass("TftGameStartViewController")]
    public class TftGameStartViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(342965096, BinPropertyType.Float)]
        public float m342965096 { get; set; } = 0f;
        [MetaProperty(1454921878, BinPropertyType.Float)]
        public float m1454921878 { get; set; } = 0f;
        [MetaProperty(2167504614, BinPropertyType.Hash)]
        public MetaHash m2167504614 { get; set; } = new(0);
        [MetaProperty(2281071129, BinPropertyType.Float)]
        public float m2281071129 { get; set; } = 0f;
        [MetaProperty(4137962166, BinPropertyType.Hash)]
        public MetaHash m4137962166 { get; set; } = new(0);
        [MetaProperty(4233736280, BinPropertyType.Hash)]
        public MetaHash m4233736280 { get; set; } = new(0);
    }
    [MetaClass("ESportTeamEntry")]
    public class ESportTeamEntry : IMetaClass
    {
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string LeagueName { get; set; } = "";
        [MetaProperty("teamName", BinPropertyType.String)]
        public string TeamName { get; set; } = "";
        [MetaProperty("textureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
    }
    [MetaClass("CheatMenuUIData")]
    public class CheatMenuUIData : IMetaClass
    {
        [MetaProperty("mHotkeys", BinPropertyType.Container)]
        public MetaContainer<string> Hotkeys { get; set; } = new();
        [MetaProperty("mFloatingTextDisplayName", BinPropertyType.String)]
        public string FloatingTextDisplayName { get; set; } = "";
        [MetaProperty("mDisplayName", BinPropertyType.String)]
        public string DisplayName { get; set; } = "";
        [MetaProperty("mIsToggleCheat", BinPropertyType.Bool)]
        public bool IsToggleCheat { get; set; } = false;
        [MetaProperty("mHotkey", BinPropertyType.String)]
        public string Hotkey { get; set; } = "";
        [MetaProperty("mDynamicTooltipText", BinPropertyType.String)]
        public string DynamicTooltipText { get; set; } = "";
        [MetaProperty("mTooltipText", BinPropertyType.String)]
        public string TooltipText { get; set; } = "";
    }
    [MetaClass("ContextualConditionCharacterHasTimeRemainingForAnimation")]
    public class ContextualConditionCharacterHasTimeRemainingForAnimation : ICharacterSubcondition
    {
        [MetaProperty("mMinRemainingTime", BinPropertyType.Float)]
        public float MinRemainingTime { get; set; } = 0.5f;
        [MetaProperty("mAnimationName", BinPropertyType.Hash)]
        public MetaHash AnimationName { get; set; } = new(0);
    }
    [MetaClass("ShaderLogicalParameter")]
    public class ShaderLogicalParameter : IMetaClass
    {
        [MetaProperty("fields", BinPropertyType.UInt32)]
        public uint Fields { get; set; } = 0;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
    }
    [MetaClass(347010316)]
    public class Class0x14aef50c : IMetaClass
    {
        [MetaProperty("mInventoryType", BinPropertyType.String)]
        public string InventoryType { get; set; } = "";
        [MetaProperty("mOrder", BinPropertyType.Byte)]
        public byte Order { get; set; } = 0;
        [MetaProperty("mInventoryFilters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x14aef50c>> InventoryFilters { get; set; } = new();
        [MetaProperty("mItemIDs", BinPropertyType.Container)]
        public MetaContainer<int> ItemIDs { get; set; } = new();
    }
    [MetaClass("DelayedBoolMaterialDriver")]
    public class DelayedBoolMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mDelayOff", BinPropertyType.Float)]
        public float DelayOff { get; set; } = 0f;
        [MetaProperty("mDelayOn", BinPropertyType.Float)]
        public float DelayOn { get; set; } = 0f;
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; } = null;
    }
    [MetaClass("FxTableEntry")]
    public class FxTableEntry : IMetaClass
    {
        [MetaProperty("Sequence", BinPropertyType.ObjectLink)]
        public MetaObjectLink Sequence { get; set; } = new(0);
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
    }
    [MetaClass("ConditionBoolClipData")]
    public class ConditionBoolClipData : ClipBaseData
    {
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mUpdaterType", BinPropertyType.UInt32)]
        public uint UpdaterType { get; set; } = 4294967295;
        [MetaProperty("mFalseConditionClipName", BinPropertyType.Hash)]
        public MetaHash FalseConditionClipName { get; set; } = new(0);
        [MetaProperty(836456042, BinPropertyType.Bool)]
        public bool m836456042 { get; set; } = false;
        [MetaProperty("mChangeAnimationMidPlay", BinPropertyType.Bool)]
        public bool ChangeAnimationMidPlay { get; set; } = false;
        [MetaProperty("mTrueConditionClipName", BinPropertyType.Hash)]
        public MetaHash TrueConditionClipName { get; set; } = new(0);
        [MetaProperty(2451652078, BinPropertyType.Bool)]
        public bool m2451652078 { get; set; } = false;
        [MetaProperty("mChildAnimDelaySwitchTime", BinPropertyType.Float)]
        public float ChildAnimDelaySwitchTime { get; set; } = 0f;
    }
    [MetaClass("ByCharLevelInterpolationCalculationPart")]
    public class ByCharLevelInterpolationCalculationPart : IGameCalculationPartByCharLevel
    {
        [MetaProperty("mEndValue", BinPropertyType.Float)]
        public float EndValue { get; set; } = 0f;
        [MetaProperty(2145969075, BinPropertyType.Bool)]
        public bool m2145969075 { get; set; } = false;
        [MetaProperty(2737960639, BinPropertyType.Bool)]
        public bool m2737960639 { get; set; } = true;
        [MetaProperty("mStartValue", BinPropertyType.Float)]
        public float StartValue { get; set; } = 0f;
    }
    [MetaClass("MissileBehaviorSpec")]
    public interface MissileBehaviorSpec : IMetaClass
    {
    }
    [MetaClass("TransitionClipBlendData")]
    public class TransitionClipBlendData : BaseBlendData
    {
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash ClipName { get; set; } = new(0);
    }
    [MetaClass(374383822)]
    public interface Class0x1650a4ce : IMetaClass
    {
    }
    [MetaClass("MapSunProperties")]
    public class MapSunProperties : MapComponent
    {
        [MetaProperty("fogColor", BinPropertyType.Vector4)]
        public Vector4 FogColor { get; set; } = new Vector4(0.20000000298023224f, 0.20000000298023224f, 0.4000000059604645f, 1f);
        [MetaProperty("fogEmissiveRemap", BinPropertyType.Float)]
        public float FogEmissiveRemap { get; set; } = 1.899999976158142f;
        [MetaProperty("fogLowQualityModeEmissiveRemap", BinPropertyType.Float)]
        public float FogLowQualityModeEmissiveRemap { get; set; } = 0.019999999552965164f;
        [MetaProperty("useBloom", BinPropertyType.Bool)]
        public bool UseBloom { get; set; } = false;
        [MetaProperty("fogAlternateColor", BinPropertyType.Vector4)]
        public Vector4 FogAlternateColor { get; set; } = new Vector4(0.10000000149011612f, 0.10000000149011612f, 0.20000000298023224f, 1f);
        [MetaProperty("groundColor", BinPropertyType.Vector4)]
        public Vector4 GroundColor { get; set; } = new Vector4(0.10000000149011612f, 0.10000000149011612f, 0.10000000149011612f, 1f);
        [MetaProperty("sunColor", BinPropertyType.Vector4)]
        public Vector4 SunColor { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        [MetaProperty("fogStartAndEnd", BinPropertyType.Vector2)]
        public Vector2 FogStartAndEnd { get; set; } = new Vector2(0f, -2000f);
        [MetaProperty("fogEnabled", BinPropertyType.Bool)]
        public bool FogEnabled { get; set; } = true;
        [MetaProperty("lightMapColorScale", BinPropertyType.Float)]
        public float LightMapColorScale { get; set; } = 1f;
        [MetaProperty("surfaceAreaToShadowMapScale", BinPropertyType.Float)]
        public float SurfaceAreaToShadowMapScale { get; set; } = 0.05000000074505806f;
        [MetaProperty(2689325503, BinPropertyType.Optional)]
        public MetaOptional<Vector3> m2689325503 { get; set; } = new MetaOptional<Vector3>(default(Vector3), false);
        [MetaProperty("skyLightColor", BinPropertyType.Vector4)]
        public Vector4 SkyLightColor { get; set; } = new Vector4(0.7049999833106995f, 0.8799999952316284f, 1f, 1f);
        [MetaProperty("skyLightScale", BinPropertyType.Float)]
        public float SkyLightScale { get; set; } = 0.20000000298023224f;
        [MetaProperty(3120754966, BinPropertyType.Float)]
        public float m3120754966 { get; set; } = 1f;
        [MetaProperty(3632599555, BinPropertyType.Float)]
        public float m3632599555 { get; set; } = 0f;
        [MetaProperty("sunDirection", BinPropertyType.Vector3)]
        public Vector3 SunDirection { get; set; } = new Vector3(0f, 0.7070000171661377f, 0.7070000171661377f);
        [MetaProperty("horizonColor", BinPropertyType.Vector4)]
        public Vector4 HorizonColor { get; set; } = new Vector4(0.4000000059604645f, 0.4000000059604645f, 0.4000000059604645f, 1f);
    }
    [MetaClass("NotificationsPanelViewController")]
    public class NotificationsPanelViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty("ClearAllButtonDefinition", BinPropertyType.Hash)]
        public MetaHash ClearAllButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("EmblemSettings")]
    public class EmblemSettings : IMetaClass
    {
        [MetaProperty("mBottomFraction", BinPropertyType.Float)]
        public float BottomFraction { get; set; } = 0.699999988079071f;
        [MetaProperty("mDebugDrawEmblems", BinPropertyType.Bool)]
        public bool DebugDrawEmblems { get; set; } = false;
    }
    [MetaClass("FloatTextIconData")]
    public class FloatTextIconData : IMetaClass
    {
        [MetaProperty("mIconFileName", BinPropertyType.String)]
        public string IconFileName { get; set; } = "";
        [MetaProperty("mOffset", BinPropertyType.Vector2)]
        public Vector2 Offset { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("mColor", BinPropertyType.Color)]
        public Color Color { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("mDisplaySize", BinPropertyType.Vector2)]
        public Vector2 DisplaySize { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("mAlignment", BinPropertyType.UInt32)]
        public uint Alignment { get; set; } = 0;
    }
    [MetaClass("ScaleByScaleSkinCoef")]
    public class ScaleByScaleSkinCoef : MissileBehaviorSpec
    {
    }
    [MetaClass("IGameCalculation")]
    public interface IGameCalculation : IMetaClass
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        IGameCalculationPart Multiplier { get; set; }
        [MetaProperty(923208333, BinPropertyType.Byte)]
        byte m923208333 { get; set; }
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        byte m3419063832 { get; set; }
        [MetaProperty("tooltipOnly", BinPropertyType.Bool)]
        bool TooltipOnly { get; set; }
        [MetaProperty(3874405167, BinPropertyType.Byte)]
        byte m3874405167 { get; set; }
    }
    [MetaClass("FloatingTextTunables")]
    public class FloatingTextTunables : IMetaClass
    {
        [MetaProperty("mAnimatedTextQueueDelay", BinPropertyType.Float)]
        public float AnimatedTextQueueDelay { get; set; } = 0.10000000149011612f;
        [MetaProperty("mMinionComparisonMultiplier", BinPropertyType.Float)]
        public float MinionComparisonMultiplier { get; set; } = 1.25f;
        [MetaProperty("mLocalPlayerHealthComparison", BinPropertyType.Float)]
        public float LocalPlayerHealthComparison { get; set; } = 12.5f;
        [MetaProperty("mYResolutionBaseline", BinPropertyType.Float)]
        public float YResolutionBaseline { get; set; } = 1200f;
        [MetaProperty("mScrollSpeed", BinPropertyType.Float)]
        public float ScrollSpeed { get; set; } = 45f;
        [MetaProperty("mMaxFloatingTextItems", BinPropertyType.UInt32)]
        public uint MaxFloatingTextItems { get; set; } = 25;
        [MetaProperty("mIntervalInPix", BinPropertyType.Float)]
        public float IntervalInPix { get; set; } = -1f;
        [MetaProperty("mMaximumDynamicScale", BinPropertyType.Float)]
        public float MaximumDynamicScale { get; set; } = 1.75f;
        [MetaProperty("mComparisonByLevel", BinPropertyType.Container)]
        public MetaContainer<float> ComparisonByLevel { get; set; } = new();
        [MetaProperty(3438744487, BinPropertyType.Float)]
        public float m3438744487 { get; set; } = 1000000f;
        [MetaProperty("mMinimumDynamicScale", BinPropertyType.Float)]
        public float MinimumDynamicScale { get; set; } = 1f;
        [MetaProperty(4117694812, BinPropertyType.Float)]
        public float m4117694812 { get; set; } = -1f;
    }
    [MetaClass("ItemRecommendationCondition")]
    public class ItemRecommendationCondition : IMetaClass
    {
        [MetaProperty("mGroupId", BinPropertyType.Byte)]
        public byte GroupId { get; set; } = 0;
        [MetaProperty("mItem", BinPropertyType.Hash)]
        public MetaHash Item { get; set; } = new(0);
        [MetaProperty("mDisplayLimit", BinPropertyType.UInt32)]
        public uint DisplayLimit { get; set; } = 0;
    }
    [MetaClass("GameModeItemList")]
    public class GameModeItemList : IMetaClass
    {
        [MetaProperty("mItems", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaHash> Items { get; set; } = new();
    }
    [MetaClass("NumberFormattingBehavior")]
    public class NumberFormattingBehavior : IMetaClass
    {
        [MetaProperty(905883269, BinPropertyType.Bool)]
        public bool m905883269 { get; set; } = true;
        [MetaProperty(1778472996, BinPropertyType.UInt32)]
        public uint m1778472996 { get; set; } = 4;
        [MetaProperty(2559746888, BinPropertyType.Bool)]
        public bool m2559746888 { get; set; } = true;
    }
    [MetaClass("TargeterDefinitionMinimap")]
    public class TargeterDefinitionMinimap : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("useCasterBoundingBox", BinPropertyType.Optional)]
        public MetaOptional<bool> UseCasterBoundingBox { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; } = new (new ());
        [MetaProperty("overrideBaseRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideBaseRange { get; set; } = new (new ());
    }
    [MetaClass("OrScriptCondition")]
    public class OrScriptCondition : IScriptCondition
    {
        [MetaProperty("Conditions", BinPropertyType.Container)]
        public MetaContainer<IScriptCondition> Conditions { get; set; } = new();
    }
    [MetaClass("HudLoadingScreenWidgetProgressBar")]
    public class HudLoadingScreenWidgetProgressBar : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string SceneName { get; set; } = "";
    }
    [MetaClass("HudEmotePopupData")]
    public class HudEmotePopupData : IMetaClass
    {
        [MetaProperty("mUiSound", BinPropertyType.String)]
        public string UiSound { get; set; } = "";
        [MetaProperty(1809279107, BinPropertyType.Float)]
        public float m1809279107 { get; set; } = 0f;
        [MetaProperty("mEmotePopupTransitionOutro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> EmotePopupTransitionOutro { get; set; } = new (new ());
        [MetaProperty(1921880927, BinPropertyType.Float)]
        public float m1921880927 { get; set; } = 0f;
        [MetaProperty(1976912936, BinPropertyType.Float)]
        public float m1976912936 { get; set; } = 0f;
        [MetaProperty(130878851, BinPropertyType.Float)]
        public float m130878851 { get; set; } = 0f;
        [MetaProperty("mEmotePopupDisplayTime", BinPropertyType.Float)]
        public float EmotePopupDisplayTime { get; set; } = 2.5f;
        [MetaProperty("mEmotePopupTransitionIntro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> EmotePopupTransitionIntro { get; set; } = new (new ());
        [MetaProperty("mEmoteFloatEnabled", BinPropertyType.Bool)]
        public bool EmoteFloatEnabled { get; set; } = false;
        [MetaProperty(4135630809, BinPropertyType.Float)]
        public float m4135630809 { get; set; } = 0f;
    }
    [MetaClass("VfxFieldAttractionDefinitionData")]
    public class VfxFieldAttractionDefinitionData : IMetaClass
    {
        [MetaProperty("acceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Acceleration { get; set; } = new (new ());
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Position { get; set; } = new (new ());
        [MetaProperty("radius", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Radius { get; set; } = new (new ());
    }
    [MetaClass("TriggerOnHit")]
    public class TriggerOnHit : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; } = new();
    }
    [MetaClass("HudLoadingScreenProgressBarData")]
    public class HudLoadingScreenProgressBarData : IMetaClass
    {
        [MetaProperty(2671588403, BinPropertyType.Float)]
        public float m2671588403 { get; set; } = 0.10000000149011612f;
        [MetaProperty(3041975949, BinPropertyType.Float)]
        public float m3041975949 { get; set; } = 1f;
        [MetaProperty(3600106471, BinPropertyType.Float)]
        public float m3600106471 { get; set; } = 20f;
        [MetaProperty(3836273065, BinPropertyType.Float)]
        public float m3836273065 { get; set; } = 1f;
    }
    [MetaClass("PhysicsMovement")]
    public class PhysicsMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mDrag", BinPropertyType.Float)]
        public float Drag { get; set; } = 1f;
        [MetaProperty("mInitialSpeed", BinPropertyType.Float)]
        public float InitialSpeed { get; set; } = 0f;
        [MetaProperty("mWallSliding", BinPropertyType.Bool)]
        public bool WallSliding { get; set; } = false;
        [MetaProperty(3396802375, BinPropertyType.Float)]
        public float m3396802375 { get; set; } = 1f;
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float Lifetime { get; set; } = 0f;
    }
    [MetaClass("HudLoadingScreenCarouselData")]
    public class HudLoadingScreenCarouselData : IMetaClass
    {
        [MetaProperty("mTipType", BinPropertyType.Byte)]
        public byte TipType { get; set; } = 0;
        [MetaProperty("image", BinPropertyType.String)]
        public string Image { get; set; } = "";
    }
    [MetaClass("BuffStackingTemplate")]
    public class BuffStackingTemplate : IMetaClass
    {
        [MetaProperty("BuffAddType", BinPropertyType.UInt32)]
        public uint BuffAddType { get; set; } = 1;
        [MetaProperty("maxStacks", BinPropertyType.Int32)]
        public int MaxStacks { get; set; } = 0;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(3010375308, BinPropertyType.Int32)]
        public int m3010375308 { get; set; } = 0;
        [MetaProperty("StacksExclusive", BinPropertyType.Bool)]
        public bool StacksExclusive { get; set; } = false;
    }
    [MetaClass("CustomTableContainsValueBlock")]
    public class CustomTableContainsValueBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; } = null;
        [MetaProperty("OutWasFound", BinPropertyType.Embedded)]
        public MetaEmbedded<BoolTableSet> OutWasFound { get; set; } = new (new ());
        [MetaProperty("OutKey", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutKey { get; set; } = new (new ());
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("MapNavGrid")]
    public class MapNavGrid : MapComponent
    {
        [MetaProperty("NavGridPath", BinPropertyType.String)]
        public string NavGridPath { get; set; } = "";
    }
    [MetaClass("SyncCircleMovement")]
    public class SyncCircleMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty(382773397, BinPropertyType.Bool)]
        public bool m382773397 { get; set; } = false;
        [MetaProperty(640692266, BinPropertyType.Byte)]
        public byte m640692266 { get; set; } = 1;
        [MetaProperty("mAngularVelocity", BinPropertyType.Float)]
        public float AngularVelocity { get; set; } = 0f;
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float Lifetime { get; set; } = 0f;
    }
    [MetaClass("TargetLaserComponentEffects")]
    public class TargetLaserComponentEffects : IMetaClass
    {
        [MetaProperty("towerTargetingEffectDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect> TowerTargetingEffectDefinition { get; set; } = new (new ());
        [MetaProperty("beamEffectDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect> BeamEffectDefinition { get; set; } = new (new ());
        [MetaProperty("champTargetingEffectDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect> ChampTargetingEffectDefinition { get; set; } = new (new ());
    }
    [MetaClass("TFTModeData")]
    public class TFTModeData : IMetaClass
    {
        [MetaProperty(313192920, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m313192920 { get; set; } = new();
        [MetaProperty("mDefaultSetData", BinPropertyType.ObjectLink)]
        public MetaObjectLink DefaultSetData { get; set; } = new(0);
        [MetaProperty("mTftMapSkinDefault", BinPropertyType.Hash)]
        public MetaHash TftMapSkinDefault { get; set; } = new(0);
        [MetaProperty(1018083252, BinPropertyType.Float)]
        public float m1018083252 { get; set; } = 3f;
        [MetaProperty("mTftDamageSkinDefault", BinPropertyType.Hash)]
        public MetaHash TftDamageSkinDefault { get; set; } = new(0);
        [MetaProperty(1243157057, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1243157057 { get; set; } = new(0);
        [MetaProperty("mTutorialTftCompanion", BinPropertyType.Hash)]
        public MetaHash TutorialTftCompanion { get; set; } = new(0);
        [MetaProperty("mDragData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTDragData> DragData { get; set; } = new (new ());
        [MetaProperty(3076159825, BinPropertyType.Float)]
        public float m3076159825 { get; set; } = 1.149999976158142f;
        [MetaProperty("mDefaultTftCompanion", BinPropertyType.Hash)]
        public MetaHash DefaultTftCompanion { get; set; } = new(0);
        [MetaProperty("mMobileDragData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTDragData> MobileDragData { get; set; } = new (new ());
        [MetaProperty(236177322, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x3604b3e3>> m236177322 { get; set; } = new();
    }
    [MetaClass("VfxSoftParticleDefinitionData")]
    public class VfxSoftParticleDefinitionData : IMetaClass
    {
        [MetaProperty("deltaIn", BinPropertyType.Float)]
        public float DeltaIn { get; set; } = 0f;
        [MetaProperty("beginIn", BinPropertyType.Float)]
        public float BeginIn { get; set; } = 0f;
        [MetaProperty("deltaOut", BinPropertyType.Float)]
        public float DeltaOut { get; set; } = 0f;
        [MetaProperty("beginOut", BinPropertyType.Float)]
        public float BeginOut { get; set; } = 0f;
    }
    [MetaClass("ContextualActionTriggerEvent")]
    public class ContextualActionTriggerEvent : IContextualAction
    {
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        public uint MaxOccurences { get; set; } = 0;
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        public MetaHash HashedSituationTrigger { get; set; } = new(0);
    }
    [MetaClass("AvatarVarsTable")]
    public class AvatarVarsTable : ScriptTable
    {
    }
    [MetaClass("PerkScript")]
    public class PerkScript : IMetaClass
    {
        [MetaProperty("mSpellScriptName", BinPropertyType.String)]
        public string SpellScriptName { get; set; } = "";
        [MetaProperty("mSpellScript", BinPropertyType.Structure)]
        public LolSpellScript SpellScript { get; set; } = null;
        [MetaProperty("mSpellScriptData", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkScriptData> SpellScriptData { get; set; } = new (new ());
    }
    [MetaClass("AnnouncementDefinitionData")]
    public class AnnouncementDefinitionData : IMetaClass
    {
        [MetaProperty("CommonElements", BinPropertyType.Hash)]
        public MetaHash CommonElements { get; set; } = new(0);
        [MetaProperty("MakeLowerPriorityEventsObsolete", BinPropertyType.Bool)]
        public bool MakeLowerPriorityEventsObsolete { get; set; } = true;
        [MetaProperty("AlliedElements", BinPropertyType.Hash)]
        public MetaHash AlliedElements { get; set; } = new(0);
        [MetaProperty("CanBeMadeObsolete", BinPropertyType.Bool)]
        public bool CanBeMadeObsolete { get; set; } = true;
        [MetaProperty("TextKey", BinPropertyType.String)]
        public string TextKey { get; set; } = "";
        [MetaProperty("SoundKey", BinPropertyType.String)]
        public string SoundKey { get; set; } = "";
        [MetaProperty("ChatMessageKey", BinPropertyType.String)]
        public string ChatMessageKey { get; set; } = "";
        [MetaProperty("priority", BinPropertyType.UInt16)]
        public ushort Priority { get; set; } = 0;
        [MetaProperty("SpectatorSoundKey", BinPropertyType.String)]
        public string SpectatorSoundKey { get; set; } = "";
        [MetaProperty("Style", BinPropertyType.ObjectLink)]
        public MetaObjectLink Style { get; set; } = new(0);
        [MetaProperty("EnemyElements", BinPropertyType.Hash)]
        public MetaHash EnemyElements { get; set; } = new(0);
    }
    [MetaClass("HudHealthBarProgressiveTickData")]
    public class HudHealthBarProgressiveTickData : IMetaClass
    {
        [MetaProperty("healthPerTick", BinPropertyType.Float)]
        public float HealthPerTick { get; set; } = 1000f;
        [MetaProperty("microTickPerTickData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MicroTicksPerTickData>> MicroTickPerTickData { get; set; } = new();
    }
    [MetaClass("MasteryData")]
    public class MasteryData : IMetaClass
    {
        [MetaProperty("texture", BinPropertyType.String)]
        public string Texture { get; set; } = "";
        [MetaProperty("LevelTraKey", BinPropertyType.String)]
        public string LevelTraKey { get; set; } = "";
        [MetaProperty("DetailsTraKey", BinPropertyType.String)]
        public string DetailsTraKey { get; set; } = "";
    }
    [MetaClass("ForEachInCustomTableBlock")]
    public class ForEachInCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; } = new (new ());
        [MetaProperty("SortedByKeys", BinPropertyType.Bool)]
        public bool SortedByKeys { get; set; } = false;
        [MetaProperty("OutKey", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutKey { get; set; } = new (new ());
        [MetaProperty("OutValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutValue { get; set; } = new (new ());
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("SummonerEmoteSettings")]
    public class SummonerEmoteSettings : IMetaClass
    {
        [MetaProperty("mFirstBlood", BinPropertyType.ObjectLink)]
        public MetaObjectLink FirstBlood { get; set; } = new(0);
        [MetaProperty("mAce", BinPropertyType.ObjectLink)]
        public MetaObjectLink Ace { get; set; } = new(0);
    }
    [MetaClass("FloatPerSpellLevel")]
    public class FloatPerSpellLevel : IMetaClass
    {
        [MetaProperty("mValueType", BinPropertyType.UInt32)]
        public uint ValueType { get; set; } = 0;
        [MetaProperty("mPerLevelValues", BinPropertyType.Container)]
        public MetaContainer<float> PerLevelValues { get; set; } = new();
    }
    [MetaClass(510412798)]
    public class Class0x1e6c47fe : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string KeyName { get; set; } = "";
    }
    [MetaClass("CooldownMultiplierCalculationPart")]
    public class CooldownMultiplierCalculationPart : IGameCalculationPart
    {
    }
    [MetaClass("OptionTemplateSlider")]
    public class OptionTemplateSlider : IOptionTemplate
    {
        [MetaProperty(1778722188, BinPropertyType.Hash)]
        public MetaHash m1778722188 { get; set; } = new(0);
        [MetaProperty("valueElement", BinPropertyType.Hash)]
        public MetaHash ValueElement { get; set; } = new(0);
        [MetaProperty("labelElement", BinPropertyType.Hash)]
        public MetaHash LabelElement { get; set; } = new(0);
    }
    [MetaClass("MinimapData")]
    public class MinimapData : IMetaClass
    {
        [MetaProperty("mCustomIcons", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<MinimapIcon>> CustomIcons { get; set; } = new();
        [MetaProperty("mIcons", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<MinimapIcon>> Icons { get; set; } = new();
    }
    [MetaClass("MinimapIcon")]
    public class MinimapIcon : IMetaClass
    {
        [MetaProperty("mBaseTexture", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapIconTextureData> BaseTexture { get; set; } = new (new ());
        [MetaProperty("mRelativeTeams", BinPropertyType.Bool)]
        public bool RelativeTeams { get; set; } = false;
        [MetaProperty("mTeamTextures", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<MinimapIconTextureData>> TeamTextures { get; set; } = new();
        [MetaProperty("mSize", BinPropertyType.Vector2)]
        public Vector2 Size { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("mTeamColors", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<MinimapIconColorData>> TeamColors { get; set; } = new();
        [MetaProperty("mBaseColor", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapIconColorData> BaseColor { get; set; } = new (new ());
        [MetaProperty("mMinScale", BinPropertyType.Float)]
        public float MinScale { get; set; } = 0f;
        [MetaProperty("mMaxScale", BinPropertyType.Float)]
        public float MaxScale { get; set; } = 0f;
    }
    [MetaClass("TargetOrLocation")]
    public class TargetOrLocation : TargetingTypeData
    {
    }
    [MetaClass("VfxBeamDefinitionData")]
    public class VfxBeamDefinitionData : IMetaClass
    {
        [MetaProperty("mSegments", BinPropertyType.Int32)]
        public int Segments { get; set; } = 0;
        [MetaProperty("mTrailMode", BinPropertyType.Byte)]
        public byte TrailMode { get; set; } = 0;
        [MetaProperty("mBirthTilingSize", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthTilingSize { get; set; } = new (new ());
        [MetaProperty("mIsColorBindedWithDistance", BinPropertyType.Bool)]
        public bool IsColorBindedWithDistance { get; set; } = false;
        [MetaProperty("mMode", BinPropertyType.Byte)]
        public byte Mode { get; set; } = 0;
        [MetaProperty("mAnimatedColorWithDistance", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> AnimatedColorWithDistance { get; set; } = new (new ());
        [MetaProperty("mLocalSpaceTargetOffset", BinPropertyType.Vector3)]
        public Vector3 LocalSpaceTargetOffset { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("mLocalSpaceSourceOffset", BinPropertyType.Vector3)]
        public Vector3 LocalSpaceSourceOffset { get; set; } = new Vector3(0f, 0f, 0f);
    }
    [MetaClass(532586595)]
    public class Class0x1fbea063 : IDynamicMaterialDriver
    {
    }
    [MetaClass("CustomShaderDef")]
    public class CustomShaderDef : IShaderDef
    {
        [MetaProperty("featureMask", BinPropertyType.UInt32)]
        public uint FeatureMask { get; set; } = 0;
        [MetaProperty("staticSwitches", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderStaticSwitch>> StaticSwitches { get; set; } = new();
        [MetaProperty("parameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderPhysicalParameter>> Parameters { get; set; } = new();
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderTexture>> Textures { get; set; } = new();
        [MetaProperty(2617146753, BinPropertyType.UInt32)]
        public uint m2617146753 { get; set; } = 0;
        [MetaProperty("featureDefines", BinPropertyType.Map)]
        public Dictionary<string, string> FeatureDefines { get; set; } = new();
        [MetaProperty("objectPath", BinPropertyType.String)]
        public string ObjectPath { get; set; } = "";
    }
    [MetaClass("ReturnToCasterOnMovementComplete")]
    public class ReturnToCasterOnMovementComplete : MissileBehaviorSpec
    {
        [MetaProperty("mPreserveSpeed", BinPropertyType.Bool)]
        public bool PreserveSpeed { get; set; } = false;
        [MetaProperty("mOverrideSpec", BinPropertyType.Structure)]
        public MissileMovementSpec OverrideSpec { get; set; } = null;
    }
    [MetaClass(547029623)]
    public class Class0x209b0277 : IMetaClass
    {
        [MetaProperty(554607262, BinPropertyType.String)]
        public string m554607262 { get; set; } = "";
        [MetaProperty(72483660, BinPropertyType.String)]
        public string m72483660 { get; set; } = "";
        [MetaProperty("MouseUpEvent", BinPropertyType.String)]
        public string MouseUpEvent { get; set; } = "";
        [MetaProperty("RolloverEvent", BinPropertyType.String)]
        public string RolloverEvent { get; set; } = "";
        [MetaProperty(3030163781, BinPropertyType.String)]
        public string m3030163781 { get; set; } = "";
        [MetaProperty("MouseDownEvent", BinPropertyType.String)]
        public string MouseDownEvent { get; set; } = "";
    }
    [MetaClass("IResource")]
    public interface IResource : IMetaClass
    {
    }
    [MetaClass(550561409)]
    public class Class0x20d0e681 : ElementGroupData
    {
        [MetaProperty(1995400414, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1995400414 { get; set; } = new(0);
    }
    [MetaClass("FxActionContinue")]
    public class FxActionContinue : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
    }
    [MetaClass("ParallelClipData")]
    public class ParallelClipData : ClipBaseData
    {
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mClipNameList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ClipNameList { get; set; } = new();
    }
    [MetaClass("VfxPrimitiveTrailBase")]
    public interface VfxPrimitiveTrailBase : VfxPrimitiveBase
    {
        [MetaProperty("mTrail", BinPropertyType.Embedded)]
        MetaEmbedded<VfxTrailDefinitionData> Trail { get; set; }
    }
    [MetaClass("DynamicMaterialTextureSwapOption")]
    public class DynamicMaterialTextureSwapOption : IMetaClass
    {
        [MetaProperty("textureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Driver { get; set; } = null;
    }
    [MetaClass("ItemAdviceAttribute")]
    public class ItemAdviceAttribute : IMetaClass
    {
        [MetaProperty("mAttribute", BinPropertyType.String)]
        public string Attribute { get; set; } = "";
    }
    [MetaClass("MapPerInstanceInfo")]
    public class MapPerInstanceInfo : IMetaClass
    {
        [MetaProperty("shadowMapUVScaleAndBias", BinPropertyType.Vector4)]
        public Vector4 ShadowMapUVScaleAndBias { get; set; } = new Vector4(1f, 1f, 0f, 0f);
        [MetaProperty("shadowMapPath", BinPropertyType.String)]
        public string ShadowMapPath { get; set; } = "";
    }
    [MetaClass("ScriptTableGet")]
    public class ScriptTableGet : IScriptValueGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
    }
    [MetaClass("IContextualConditionSpell")]
    public interface IContextualConditionSpell : IContextualCondition
    {
    }
    [MetaClass("VerticalFacingFaceTarget")]
    public class VerticalFacingFaceTarget : VerticalFacingType
    {
    }
    [MetaClass("ContextualRule")]
    public class ContextualRule : IMetaClass
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<IContextualCondition> Conditions { get; set; } = new();
        [MetaProperty("mAnimationAction", BinPropertyType.Structure)]
        public ContextualActionPlayAnimation AnimationAction { get; set; } = null;
        [MetaProperty(1761534916, BinPropertyType.Bool)]
        public bool m1761534916 { get; set; } = false;
        [MetaProperty("mConditionRelationship", BinPropertyType.UInt32)]
        public uint ConditionRelationship { get; set; } = 0;
        [MetaProperty("mTriggerEventAction", BinPropertyType.Structure)]
        public ContextualActionTriggerEvent TriggerEventAction { get; set; } = null;
        [MetaProperty("mAudioAction", BinPropertyType.Structure)]
        public ContextualActionPlayAudio AudioAction { get; set; } = null;
        [MetaProperty("mPriority", BinPropertyType.Optional)]
        public MetaOptional<uint> Priority { get; set; } = new MetaOptional<uint>(default(uint), false);
    }
    [MetaClass("RegionSettings")]
    public class RegionSettings : IMetaClass
    {
        [MetaProperty("mContentTypeAllowedSettings", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<RegionsThatAllowContent>> ContentTypeAllowedSettings { get; set; } = new();
    }
    [MetaClass("FloatTextFormattingData")]
    public class FloatTextFormattingData : IMetaClass
    {
        [MetaProperty("disableHorizontalReverse", BinPropertyType.Bool)]
        public bool DisableHorizontalReverse { get; set; } = false;
        [MetaProperty("verticalAlignment", BinPropertyType.Byte)]
        public byte VerticalAlignment { get; set; } = 1;
        [MetaProperty("randomStartOffsetMaxY", BinPropertyType.Float)]
        public float RandomStartOffsetMaxY { get; set; } = 0f;
        [MetaProperty("randomStartOffsetMaxX", BinPropertyType.Float)]
        public float RandomStartOffsetMaxX { get; set; } = 0f;
        [MetaProperty("ignoreCombineRules", BinPropertyType.Bool)]
        public bool IgnoreCombineRules { get; set; } = false;
        [MetaProperty("minYVelocity", BinPropertyType.Float)]
        public float MinYVelocity { get; set; } = 1f;
        [MetaProperty("disabled", BinPropertyType.Bool)]
        public bool Disabled { get; set; } = false;
        [MetaProperty("relativeOffsetMin", BinPropertyType.Float)]
        public float RelativeOffsetMin { get; set; } = 0f;
        [MetaProperty("combinableCounterDisplay", BinPropertyType.Bool)]
        public bool CombinableCounterDisplay { get; set; } = false;
        [MetaProperty("combinableCounterCategory", BinPropertyType.Int32)]
        public int CombinableCounterCategory { get; set; } = 0;
        [MetaProperty("offsetByBoundingBox", BinPropertyType.Bool)]
        public bool OffsetByBoundingBox { get; set; } = false;
        [MetaProperty("relativeOffsetMax", BinPropertyType.Float)]
        public float RelativeOffsetMax { get; set; } = 0f;
        [MetaProperty("growthYScalar", BinPropertyType.Float)]
        public float GrowthYScalar { get; set; } = 0f;
        [MetaProperty("overwritePreviousNumber", BinPropertyType.Bool)]
        public bool OverwritePreviousNumber { get; set; } = false;
        [MetaProperty("maxInstances", BinPropertyType.Int32)]
        public int MaxInstances { get; set; } = -1;
        [MetaProperty("extendTimeOnNewDamage", BinPropertyType.Float)]
        public float ExtendTimeOnNewDamage { get; set; } = 0.25f;
        [MetaProperty("refreshExisting", BinPropertyType.Bool)]
        public bool RefreshExisting { get; set; } = false;
        [MetaProperty("continualForceYBase", BinPropertyType.Float)]
        public float ContinualForceYBase { get; set; } = 0f;
        [MetaProperty("minXVelocity", BinPropertyType.Float)]
        public float MinXVelocity { get; set; } = 1f;
        [MetaProperty("continualForceXBase", BinPropertyType.Float)]
        public float ContinualForceXBase { get; set; } = 0f;
        [MetaProperty("scale", BinPropertyType.Float)]
        public float Scale { get; set; } = 1f;
        [MetaProperty("mFontDescription", BinPropertyType.ObjectLink)]
        public MetaObjectLink FontDescription { get; set; } = new(0);
        [MetaProperty("hangTime", BinPropertyType.Float)]
        public float HangTime { get; set; } = 0f;
        [MetaProperty("combinableNumberFormat", BinPropertyType.String)]
        public string CombinableNumberFormat { get; set; } = "";
        [MetaProperty("priority", BinPropertyType.Int32)]
        public int Priority { get; set; } = 10;
        [MetaProperty("maxXVelocity", BinPropertyType.Float)]
        public float MaxXVelocity { get; set; } = 1f;
        [MetaProperty("maxLifeTime", BinPropertyType.Float)]
        public float MaxLifeTime { get; set; } = 5f;
        [MetaProperty("randomStartOffsetMinY", BinPropertyType.Float)]
        public float RandomStartOffsetMinY { get; set; } = 0f;
        [MetaProperty("randomStartOffsetMinX", BinPropertyType.Float)]
        public float RandomStartOffsetMinX { get; set; } = 0f;
        [MetaProperty("shrinkTime", BinPropertyType.Float)]
        public float ShrinkTime { get; set; } = 1f;
        [MetaProperty("continualForceY", BinPropertyType.Float)]
        public float ContinualForceY { get; set; } = 0f;
        [MetaProperty("shrinkScale", BinPropertyType.Float)]
        public float ShrinkScale { get; set; } = 1f;
        [MetaProperty("continualForceX", BinPropertyType.Float)]
        public float ContinualForceX { get; set; } = 0f;
        [MetaProperty("startOffsetX", BinPropertyType.Float)]
        public float StartOffsetX { get; set; } = 0f;
        [MetaProperty("startOffsetY", BinPropertyType.Float)]
        public float StartOffsetY { get; set; } = 0f;
        [MetaProperty("momentumFromHit", BinPropertyType.Bool)]
        public bool MomentumFromHit { get; set; } = false;
        [MetaProperty("isAnimated", BinPropertyType.Bool)]
        public bool IsAnimated { get; set; } = false;
        [MetaProperty("maxYVelocity", BinPropertyType.Float)]
        public float MaxYVelocity { get; set; } = 1f;
        [MetaProperty("alternateRightLeft", BinPropertyType.Bool)]
        public bool AlternateRightLeft { get; set; } = false;
        [MetaProperty("decayDelay", BinPropertyType.Float)]
        public float DecayDelay { get; set; } = 0f;
        [MetaProperty("attachToHealthBar", BinPropertyType.Bool)]
        public bool AttachToHealthBar { get; set; } = false;
        [MetaProperty("decay", BinPropertyType.Float)]
        public float Decay { get; set; } = 0f;
        [MetaProperty("disableVerticalReverse", BinPropertyType.Bool)]
        public bool DisableVerticalReverse { get; set; } = false;
        [MetaProperty("growthXScalar", BinPropertyType.Float)]
        public float GrowthXScalar { get; set; } = 0f;
        [MetaProperty("height", BinPropertyType.Float)]
        public float Height { get; set; } = 0f;
        [MetaProperty("followSource", BinPropertyType.Bool)]
        public bool FollowSource { get; set; } = false;
        [MetaProperty("mTypeName", BinPropertyType.Hash)]
        public MetaHash TypeName { get; set; } = new(0);
        [MetaProperty("combinableNegativeNumberFormat", BinPropertyType.String)]
        public string CombinableNegativeNumberFormat { get; set; } = "";
        [MetaProperty("colorOffsetB", BinPropertyType.Int32)]
        public int ColorOffsetB { get; set; } = 0;
        [MetaProperty("colorOffsetG", BinPropertyType.Int32)]
        public int ColorOffsetG { get; set; } = 0;
        [MetaProperty("ignoreQueue", BinPropertyType.Bool)]
        public bool IgnoreQueue { get; set; } = false;
        [MetaProperty("colorOffsetR", BinPropertyType.Int32)]
        public int ColorOffsetR { get; set; } = 0;
        [MetaProperty("horizontalAlignment", BinPropertyType.Byte)]
        public byte HorizontalAlignment { get; set; } = 1;
        [MetaProperty("icons", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FloatTextIconData>> Icons { get; set; } = new();
    }
    [MetaClass("MapPointLight")]
    public class MapPointLight : MapPlaceable
    {
        [MetaProperty("overrideCastStaticShadows", BinPropertyType.Optional)]
        public MetaOptional<bool> OverrideCastStaticShadows { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("intensityScale", BinPropertyType.Float)]
        public float IntensityScale { get; set; } = 1f;
        [MetaProperty("type", BinPropertyType.ObjectLink)]
        public MetaObjectLink Type { get; set; } = new(0);
        [MetaProperty("overrideUseSpecular", BinPropertyType.Optional)]
        public MetaOptional<bool> OverrideUseSpecular { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("radiusScale", BinPropertyType.Float)]
        public float RadiusScale { get; set; } = 1f;
    }
    [MetaClass(589156770)]
    public interface Class0x231dd1a2 : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        string Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        MetaObjectLink Scene { get; set; }
    }
    [MetaClass("CharacterRecord")]
    public class CharacterRecord : IMetaClass
    {
        [MetaProperty(273490580, BinPropertyType.Bool)]
        public bool m273490580 { get; set; } = false;
        [MetaProperty("mEducationToolData", BinPropertyType.Embedded)]
        public MetaEmbedded<ToolEducationData> EducationToolData { get; set; } = new (new ());
        [MetaProperty("mAdaptiveForceToAbilityPowerWeight", BinPropertyType.Float)]
        public float AdaptiveForceToAbilityPowerWeight { get; set; } = 0f;
        [MetaProperty("baseHP", BinPropertyType.Float)]
        public float BaseHP { get; set; } = 100f;
        [MetaProperty("areaIndicatorTextureSize", BinPropertyType.Float)]
        public float AreaIndicatorTextureSize { get; set; } = 50f;
        [MetaProperty("attackSpeedPerLevel", BinPropertyType.Float)]
        public float AttackSpeedPerLevel { get; set; } = 0f;
        [MetaProperty("charAudioNameOverride", BinPropertyType.String)]
        public string CharAudioNameOverride { get; set; } = "";
        [MetaProperty("minionFlags", BinPropertyType.UInt32)]
        public uint MinionFlags { get; set; } = 0;
        [MetaProperty("hoverIndicatorRotateToPlayer", BinPropertyType.Bool)]
        public bool HoverIndicatorRotateToPlayer { get; set; } = false;
        [MetaProperty("criticalAttack", BinPropertyType.String)]
        public string CriticalAttack { get; set; } = "BaseSpell";
        [MetaProperty("outlineBBoxExpansion", BinPropertyType.Float)]
        public float OutlineBBoxExpansion { get; set; } = 0f;
        [MetaProperty("passiveLuaName", BinPropertyType.String)]
        public string PassiveLuaName { get; set; } = "Passive";
        [MetaProperty("mPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkReplacementList> PerkReplacements { get; set; } = new (new ());
        [MetaProperty("mFallbackCharacterName", BinPropertyType.String)]
        public string FallbackCharacterName { get; set; } = "";
        [MetaProperty("hoverLineIndicatorWidth", BinPropertyType.Float)]
        public float HoverLineIndicatorWidth { get; set; } = 50f;
        [MetaProperty("baseSpellBlock", BinPropertyType.Float)]
        public float BaseSpellBlock { get; set; } = 0f;
        [MetaProperty("unitTagsString", BinPropertyType.String)]
        public string UnitTagsString { get; set; } = "";
        [MetaProperty("recSpellRankUpInfo", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<RecSpellRankUpInfo>> RecSpellRankUpInfo { get; set; } = new();
        [MetaProperty("assetCategory", BinPropertyType.String)]
        public string AssetCategory { get; set; } = "character";
        [MetaProperty("targetLaserEffects", BinPropertyType.Structure)]
        public TargetLaserComponentEffects TargetLaserEffects { get; set; } = null;
        [MetaProperty("hoverIndicatorRadius", BinPropertyType.Float)]
        public float HoverIndicatorRadius { get; set; } = 100f;
        [MetaProperty("selfChampSpecificHealthSuffix", BinPropertyType.String)]
        public string SelfChampSpecificHealthSuffix { get; set; } = "";
        [MetaProperty("MovingTowardEnemyActivationAngle", BinPropertyType.Float)]
        public float MovingTowardEnemyActivationAngle { get; set; } = 45f;
        [MetaProperty("mCharacterCalculations", BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> CharacterCalculations { get; set; } = new();
        [MetaProperty("onKillEvent", BinPropertyType.UInt32)]
        public uint OnKillEvent { get; set; } = 227;
        [MetaProperty("baseDamage", BinPropertyType.Float)]
        public float BaseDamage { get; set; } = 10f;
        [MetaProperty("critPerLevel", BinPropertyType.Float)]
        public float CritPerLevel { get; set; } = 0f;
        [MetaProperty("hoverIndicatorRadiusMinimap", BinPropertyType.Float)]
        public float HoverIndicatorRadiusMinimap { get; set; } = 200f;
        [MetaProperty("mClientSideItemInventory", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ClientSideItemInventory { get; set; } = new();
        [MetaProperty("localExpGivenOnDeath", BinPropertyType.Float)]
        public float LocalExpGivenOnDeath { get; set; } = 0f;
        [MetaProperty("baseStaticHPRegen", BinPropertyType.Float)]
        public float BaseStaticHPRegen { get; set; } = 1f;
        [MetaProperty("selectionRadius", BinPropertyType.Float)]
        public float SelectionRadius { get; set; } = -1f;
        [MetaProperty("perceptionBoundingBoxSize", BinPropertyType.Optional)]
        public MetaOptional<Vector3> PerceptionBoundingBoxSize { get; set; } = new MetaOptional<Vector3>(default(Vector3), false);
        [MetaProperty("hoverLineIndicatorWidthMinimap", BinPropertyType.Float)]
        public float HoverLineIndicatorWidthMinimap { get; set; } = 100f;
        [MetaProperty("onKillEventForSpectator", BinPropertyType.UInt32)]
        public uint OnKillEventForSpectator { get; set; } = 227;
        [MetaProperty("mPreferredPerkStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink PreferredPerkStyle { get; set; } = new(0);
        [MetaProperty("hoverLineIndicatorTargetTextureName", BinPropertyType.String)]
        public string HoverLineIndicatorTargetTextureName { get; set; } = "";
        [MetaProperty("deathEventListeningRadius", BinPropertyType.Float)]
        public float DeathEventListeningRadius { get; set; } = 1000f;
        [MetaProperty("baseFactorHPRegen", BinPropertyType.Float)]
        public float BaseFactorHPRegen { get; set; } = 0f;
        [MetaProperty("attackSpeedRatio", BinPropertyType.Float)]
        public float AttackSpeedRatio { get; set; } = 1f;
        [MetaProperty("enemyTooltip", BinPropertyType.String)]
        public string EnemyTooltip { get; set; } = "";
        [MetaProperty("areaIndicatorRadius", BinPropertyType.Float)]
        public float AreaIndicatorRadius { get; set; } = 100f;
        [MetaProperty("significance", BinPropertyType.Float)]
        public float Significance { get; set; } = 0f;
        [MetaProperty("acquisitionRange", BinPropertyType.Float)]
        public float AcquisitionRange { get; set; } = 750f;
        [MetaProperty("firstAcquisitionRange", BinPropertyType.Optional)]
        public MetaOptional<float> FirstAcquisitionRange { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("goldRadius", BinPropertyType.Float)]
        public float GoldRadius { get; set; } = 0f;
        [MetaProperty("wakeUpRange", BinPropertyType.Optional)]
        public MetaOptional<float> WakeUpRange { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("towerTargetingPriorityBoost", BinPropertyType.Float)]
        public float TowerTargetingPriorityBoost { get; set; } = 0f;
        [MetaProperty("areaIndicatorMinRadius", BinPropertyType.Float)]
        public float AreaIndicatorMinRadius { get; set; } = 120f;
        [MetaProperty("evolutionData", BinPropertyType.Structure)]
        public EvolutionDescription EvolutionData { get; set; } = null;
        [MetaProperty("platformEnabled", BinPropertyType.Bool)]
        public bool PlatformEnabled { get; set; } = false;
        [MetaProperty("experienceRadius", BinPropertyType.Float)]
        public float ExperienceRadius { get; set; } = 0f;
        [MetaProperty("allyChampSpecificHealthSuffix", BinPropertyType.String)]
        public string AllyChampSpecificHealthSuffix { get; set; } = "";
        [MetaProperty("extraAttacks", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AttackSlotData>> ExtraAttacks { get; set; } = new();
        [MetaProperty("baseMoveSpeed", BinPropertyType.Float)]
        public float BaseMoveSpeed { get; set; } = 100f;
        [MetaProperty("baseArmor", BinPropertyType.Float)]
        public float BaseArmor { get; set; } = 1f;
        [MetaProperty("attackSpeed", BinPropertyType.Float)]
        public float AttackSpeed { get; set; } = 0f;
        [MetaProperty("baseDodge", BinPropertyType.Float)]
        public float BaseDodge { get; set; } = 0f;
        [MetaProperty("friendlyTooltip", BinPropertyType.String)]
        public string FriendlyTooltip { get; set; } = "";
        [MetaProperty("friendlyUxOverrideTeam", BinPropertyType.UInt32)]
        public uint FriendlyUxOverrideTeam { get; set; } = 0;
        [MetaProperty("overrideGameplayCollisionRadius", BinPropertyType.Optional)]
        public MetaOptional<float> OverrideGameplayCollisionRadius { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("abilityPower", BinPropertyType.Float)]
        public float AbilityPower { get; set; } = 0f;
        [MetaProperty("perceptionBubbleRadius", BinPropertyType.Optional)]
        public MetaOptional<float> PerceptionBubbleRadius { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("tips2", BinPropertyType.String)]
        public string Tips2 { get; set; } = "None";
        [MetaProperty("tips1", BinPropertyType.String)]
        public string Tips1 { get; set; } = "None";
        [MetaProperty("critDamageMultiplier", BinPropertyType.Float)]
        public float CritDamageMultiplier { get; set; } = 1.75f;
        [MetaProperty("extraSpells", BinPropertyType.Container)]
        public MetaContainer<string> ExtraSpells { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("hoverIndicatorMinimapOverride", BinPropertyType.String)]
        public string HoverIndicatorMinimapOverride { get; set; } = "";
        [MetaProperty("pathfindingCollisionRadius", BinPropertyType.Float)]
        public float PathfindingCollisionRadius { get; set; } = -1f;
        [MetaProperty("globalGoldGivenOnDeath", BinPropertyType.Float)]
        public float GlobalGoldGivenOnDeath { get; set; } = 0f;
        [MetaProperty("untargetableSpawnTime", BinPropertyType.Float)]
        public float UntargetableSpawnTime { get; set; } = 0f;
        [MetaProperty("parName", BinPropertyType.String)]
        public string ParName { get; set; } = "";
        [MetaProperty("DodgePerLevel", BinPropertyType.Float)]
        public float DodgePerLevel { get; set; } = 0f;
        [MetaProperty("spellLevelUpInfo", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellLevelUpInfo>> SpellLevelUpInfo { get; set; } = new();
        [MetaProperty("flags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 8398080;
        [MetaProperty("globalExpGivenOnDeath", BinPropertyType.Float)]
        public float GlobalExpGivenOnDeath { get; set; } = 0f;
        [MetaProperty("mAbilitySlotCC", BinPropertyType.Container)]
        public MetaContainer<int> AbilitySlotCC { get; set; } = new();
        [MetaProperty("OnKillEventSteal", BinPropertyType.UInt32)]
        public uint OnKillEventSteal { get; set; } = 227;
        [MetaProperty("healthBarHeight", BinPropertyType.Float)]
        public float HealthBarHeight { get; set; } = 100f;
        [MetaProperty("highlightHealthbarIcons", BinPropertyType.Bool)]
        public bool HighlightHealthbarIcons { get; set; } = false;
        [MetaProperty("silhouetteAttachmentAnim", BinPropertyType.String)]
        public string SilhouetteAttachmentAnim { get; set; } = "Idle1";
        [MetaProperty("attackAutoInterruptPercent", BinPropertyType.Float)]
        public float AttackAutoInterruptPercent { get; set; } = 0.20000000298023224f;
        [MetaProperty("passiveToolTip", BinPropertyType.String)]
        public string PassiveToolTip { get; set; } = "Desc";
        [MetaProperty("areaIndicatorTextureName", BinPropertyType.String)]
        public string AreaIndicatorTextureName { get; set; } = "";
        [MetaProperty("areaIndicatorTargetDistance", BinPropertyType.Float)]
        public float AreaIndicatorTargetDistance { get; set; } = 400f;
        [MetaProperty("damagePerLevel", BinPropertyType.Float)]
        public float DamagePerLevel { get; set; } = 0f;
        [MetaProperty("healthBarFullParallax", BinPropertyType.Bool)]
        public bool HealthBarFullParallax { get; set; } = false;
        [MetaProperty("deathTime", BinPropertyType.Float)]
        public float DeathTime { get; set; } = -1f;
        [MetaProperty("localGoldSplitWithLastHitter", BinPropertyType.Bool)]
        public bool LocalGoldSplitWithLastHitter { get; set; } = false;
        [MetaProperty("basicAttack", BinPropertyType.Embedded)]
        public MetaEmbedded<AttackSlotData> BasicAttack { get; set; } = new (new ());
        [MetaProperty("lore1", BinPropertyType.String)]
        public string Lore1 { get; set; } = "None";
        [MetaProperty("mCharacterPassiveBuffs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CharacterPassiveData>> CharacterPassiveBuffs { get; set; } = new();
        [MetaProperty("primaryAbilityResource", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceSlotInfo> PrimaryAbilityResource { get; set; } = new (new ());
        [MetaProperty("spellBlockPerLevel", BinPropertyType.Float)]
        public float SpellBlockPerLevel { get; set; } = 0f;
        [MetaProperty("friendlyUxOverrideExcludeTagsString", BinPropertyType.String)]
        public string FriendlyUxOverrideExcludeTagsString { get; set; } = "";
        [MetaProperty("spellNames", BinPropertyType.Container)]
        public MetaContainer<string> SpellNames { get; set; } = new();
        [MetaProperty("occludedUnitSelectableDistance", BinPropertyType.Float)]
        public float OccludedUnitSelectableDistance { get; set; } = 0f;
        [MetaProperty("attackRange", BinPropertyType.Float)]
        public float AttackRange { get; set; } = 100f;
        [MetaProperty("selfCBChampSpecificHealthSuffix", BinPropertyType.String)]
        public string SelfCBChampSpecificHealthSuffix { get; set; } = "";
        [MetaProperty("baseCritChance", BinPropertyType.Float)]
        public float BaseCritChance { get; set; } = 0f;
        [MetaProperty("AbilityPowerIncPerLevel", BinPropertyType.Float)]
        public float AbilityPowerIncPerLevel { get; set; } = 0f;
        [MetaProperty("passiveName", BinPropertyType.String)]
        public string PassiveName { get; set; } = "Passive";
        [MetaProperty(3410252243, BinPropertyType.Float)]
        public float m3410252243 { get; set; } = 0f;
        [MetaProperty("hpRegenPerLevel", BinPropertyType.Float)]
        public float HpRegenPerLevel { get; set; } = 0f;
        [MetaProperty("expGivenOnDeath", BinPropertyType.Float)]
        public float ExpGivenOnDeath { get; set; } = 48f;
        [MetaProperty("DisabledTargetLaserEffects", BinPropertyType.Structure)]
        public TargetLaserComponentEffects DisabledTargetLaserEffects { get; set; } = null;
        [MetaProperty("weaponMaterials", BinPropertyType.Container)]
        public MetaContainer<string> WeaponMaterials { get; set; } = new();
        [MetaProperty("passiveRange", BinPropertyType.Float)]
        public float PassiveRange { get; set; } = 0f;
        [MetaProperty("critAttacks", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AttackSlotData>> CritAttacks { get; set; } = new();
        [MetaProperty("localGoldGivenOnDeath", BinPropertyType.Float)]
        public float LocalGoldGivenOnDeath { get; set; } = 0f;
        [MetaProperty("jointForAnimAdjustedSelection", BinPropertyType.String)]
        public string JointForAnimAdjustedSelection { get; set; } = "";
        [MetaProperty("armorPerLevel", BinPropertyType.Float)]
        public float ArmorPerLevel { get; set; } = 0f;
        [MetaProperty("minimapIconOverride", BinPropertyType.String)]
        public string MinimapIconOverride { get; set; } = "";
        [MetaProperty("hoverLineIndicatorBaseTextureName", BinPropertyType.String)]
        public string HoverLineIndicatorBaseTextureName { get; set; } = "";
        [MetaProperty("mAbilities", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Abilities { get; set; } = new();
        [MetaProperty("mDefaultStatOverrides", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFormulaDataList> DefaultStatOverrides { get; set; } = new (new ());
        [MetaProperty("mCharacterPassiveSpell", BinPropertyType.ObjectLink)]
        public MetaObjectLink CharacterPassiveSpell { get; set; } = new(0);
        [MetaProperty("areaIndicatorMaxDistance", BinPropertyType.Float)]
        public float AreaIndicatorMaxDistance { get; set; } = 120f;
        [MetaProperty("selectionHeight", BinPropertyType.Float)]
        public float SelectionHeight { get; set; } = -1f;
        [MetaProperty("characterToolData", BinPropertyType.Embedded)]
        public MetaEmbedded<CharacterToolData> CharacterToolData { get; set; } = new (new ());
        [MetaProperty("useRiotRelationships", BinPropertyType.Bool)]
        public bool UseRiotRelationships { get; set; } = false;
        [MetaProperty("hoverIndicatorTextureName", BinPropertyType.String)]
        public string HoverIndicatorTextureName { get; set; } = "";
        [MetaProperty("passive1IconName", BinPropertyType.String)]
        public string Passive1IconName { get; set; } = "";
        [MetaProperty("recordAsWard", BinPropertyType.Bool)]
        public bool RecordAsWard { get; set; } = false;
        [MetaProperty("enemyChampSpecificHealthSuffix", BinPropertyType.String)]
        public string EnemyChampSpecificHealthSuffix { get; set; } = "";
        [MetaProperty("baseMissChance", BinPropertyType.Float)]
        public float BaseMissChance { get; set; } = 0f;
        [MetaProperty("useableData", BinPropertyType.Embedded)]
        public MetaEmbedded<UseableData> UseableData { get; set; } = new (new ());
        [MetaProperty("passiveSpell", BinPropertyType.String)]
        public string PassiveSpell { get; set; } = "";
        [MetaProperty("goldGivenOnDeath", BinPropertyType.Float)]
        public float GoldGivenOnDeath { get; set; } = 25f;
        [MetaProperty("friendlyUxOverrideIncludeTagsString", BinPropertyType.String)]
        public string FriendlyUxOverrideIncludeTagsString { get; set; } = "";
        [MetaProperty("areaIndicatorMinDistance", BinPropertyType.Float)]
        public float AreaIndicatorMinDistance { get; set; } = 25f;
        [MetaProperty("secondaryAbilityResource", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceSlotInfo> SecondaryAbilityResource { get; set; } = new (new ());
        [MetaProperty("mUseCCAnimations", BinPropertyType.Bool)]
        public bool UseCCAnimations { get; set; } = false;
        [MetaProperty("purchaseIdentities", BinPropertyType.Container)]
        public MetaContainer<MetaHash> PurchaseIdentities { get; set; } = new();
        [MetaProperty("hitFxScale", BinPropertyType.Float)]
        public float HitFxScale { get; set; } = 1f;
        [MetaProperty("hpPerLevel", BinPropertyType.Float)]
        public float HpPerLevel { get; set; } = 0f;
        [MetaProperty("minionScoreValue", BinPropertyType.Float)]
        public float MinionScoreValue { get; set; } = 1f;
        [MetaProperty("mCharacterName", BinPropertyType.String)]
        public string CharacterName { get; set; } = "";
    }
    [MetaClass("ModeProgressionRewardData")]
    public class ModeProgressionRewardData : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string BuffName { get; set; } = "";
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; } = null;
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        public MetaContainer<string> Characters { get; set; } = new();
    }
    [MetaClass("IVfxMaterialDriver")]
    public interface IVfxMaterialDriver : IMetaClass
    {
    }
    [MetaClass("ByCharLevelFormulaCalculationPart")]
    public class ByCharLevelFormulaCalculationPart : IGameCalculationPartByCharLevel
    {
        [MetaProperty("mValues", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; } = new();
    }
    [MetaClass("CatalogEntry")]
    public class CatalogEntry : IMetaClass
    {
        [MetaProperty("itemID", BinPropertyType.UInt32)]
        public uint ItemID { get; set; } = 0;
        [MetaProperty("contentId", BinPropertyType.String)]
        public string ContentId { get; set; } = "";
        [MetaProperty("offerId", BinPropertyType.String)]
        public string OfferId { get; set; } = "";
    }
    [MetaClass("IRunFunctionBlock")]
    public interface IRunFunctionBlock : IScriptBlock
    {
        [MetaProperty("InputParameters", BinPropertyType.Container)]
        MetaContainer<IScriptValueGet> InputParameters { get; set; }
        [MetaProperty("OutputParameters", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ScriptTableSet>> OutputParameters { get; set; }
        [MetaProperty("Function", BinPropertyType.Embedded)]
        MetaEmbedded<FunctionTableGet> Function { get; set; }
    }
    [MetaClass("ItemData")]
    public class ItemData : IMetaClass
    {
        [MetaProperty("itemID", BinPropertyType.Int32)]
        public int ItemID { get; set; } = 0;
        [MetaProperty("mSidegradeCredit", BinPropertyType.Float)]
        public float SidegradeCredit { get; set; } = 0f;
        [MetaProperty("mFlatHPRegenMod", BinPropertyType.Float)]
        public float FlatHPRegenMod { get; set; } = 0f;
        [MetaProperty("mFlatCooldownMod", BinPropertyType.Float)]
        public float FlatCooldownMod { get; set; } = 0f;
        [MetaProperty("mDeathRecapName", BinPropertyType.String)]
        public string DeathRecapName { get; set; } = "";
        [MetaProperty("mPercentSlowResistMod", BinPropertyType.Float)]
        public float PercentSlowResistMod { get; set; } = 0f;
        [MetaProperty("mFlatMovementSpeedMod", BinPropertyType.Float)]
        public float FlatMovementSpeedMod { get; set; } = 0f;
        [MetaProperty("PercentPhysicalVampMod", BinPropertyType.Float)]
        public float PercentPhysicalVampMod { get; set; } = 0f;
        [MetaProperty("mDataValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemDataValue>> DataValues { get; set; } = new();
        [MetaProperty("itemVOGroup", BinPropertyType.Hash)]
        public MetaHash ItemVOGroup { get; set; } = new(0);
        [MetaProperty(575289365, BinPropertyType.Int32)]
        public int m575289365 { get; set; } = 0;
        [MetaProperty("mEffectAmount", BinPropertyType.Container)]
        public MetaContainer<float> EffectAmount { get; set; } = new();
        [MetaProperty("mFlatMagicDamageMod", BinPropertyType.Float)]
        public float FlatMagicDamageMod { get; set; } = 0f;
        [MetaProperty("recipeItemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> RecipeItemLinks { get; set; } = new();
        [MetaProperty("mPercentCritDamageMod", BinPropertyType.Float)]
        public float PercentCritDamageMod { get; set; } = 0f;
        [MetaProperty("mPercentLifeStealMod", BinPropertyType.Float)]
        public float PercentLifeStealMod { get; set; } = 0f;
        [MetaProperty("PercentMPRegenMod", BinPropertyType.Float)]
        public float PercentMPRegenMod { get; set; } = 0f;
        [MetaProperty("mPercentHPRegenMod", BinPropertyType.Float)]
        public float PercentHPRegenMod { get; set; } = 0f;
        [MetaProperty("mRequiredAlly", BinPropertyType.String)]
        public string RequiredAlly { get; set; } = "";
        [MetaProperty("mRequiredBuffCurrencyCost", BinPropertyType.Int32)]
        public int RequiredBuffCurrencyCost { get; set; } = 0;
        [MetaProperty("mPercentArmorPenetrationMod", BinPropertyType.Float)]
        public float PercentArmorPenetrationMod { get; set; } = 0f;
        [MetaProperty("sellBackModifier", BinPropertyType.Float)]
        public float SellBackModifier { get; set; } = 0.699999988079071f;
        [MetaProperty("mFlatCastRangeMod", BinPropertyType.Float)]
        public float FlatCastRangeMod { get; set; } = 0f;
        [MetaProperty("consumed", BinPropertyType.Bool)]
        public bool Consumed { get; set; } = false;
        [MetaProperty("mFlatPhysicalDamageMod", BinPropertyType.Float)]
        public float FlatPhysicalDamageMod { get; set; } = 0f;
        [MetaProperty(1361468553, BinPropertyType.Byte)]
        public byte m1361468553 { get; set; } = 0;
        [MetaProperty("mItemCalloutSpectator", BinPropertyType.Bool)]
        public bool ItemCalloutSpectator { get; set; } = false;
        [MetaProperty("mFlatArmorPenetrationMod", BinPropertyType.Float)]
        public float FlatArmorPenetrationMod { get; set; } = 0f;
        [MetaProperty("mFlatMagicReduction", BinPropertyType.Float)]
        public float FlatMagicReduction { get; set; } = 0f;
        [MetaProperty("PercentMPPoolMod", BinPropertyType.Float)]
        public float PercentMPPoolMod { get; set; } = 0f;
        [MetaProperty("mItemAttributes", BinPropertyType.Container)]
        public MetaContainer<byte> ItemAttributes { get; set; } = new();
        [MetaProperty("mItemDataAvailability", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemDataAvailability> ItemDataAvailability { get; set; } = new (new ());
        [MetaProperty("mPercentMagicPenetrationMod", BinPropertyType.Float)]
        public float PercentMagicPenetrationMod { get; set; } = 0f;
        [MetaProperty("mItemAdviceAttributes", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemAdviceAttributes { get; set; } = new();
        [MetaProperty("mAbilityHasteMod", BinPropertyType.Float)]
        public float AbilityHasteMod { get; set; } = 0f;
        [MetaProperty("epicness", BinPropertyType.Byte)]
        public byte Epicness { get; set; } = 2;
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; } = null;
        [MetaProperty("mBuildDepth", BinPropertyType.Int32)]
        public int BuildDepth { get; set; } = 0;
        [MetaProperty("mPercentPhysicalReduction", BinPropertyType.Float)]
        public float PercentPhysicalReduction { get; set; } = 0f;
        [MetaProperty("mPercentBonusArmorPenetrationMod", BinPropertyType.Float)]
        public float PercentBonusArmorPenetrationMod { get; set; } = 0f;
        [MetaProperty("mPercentMagicDamageMod", BinPropertyType.Float)]
        public float PercentMagicDamageMod { get; set; } = 0f;
        [MetaProperty("sidegradeItemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> SidegradeItemLinks { get; set; } = new();
        [MetaProperty("mPercentSpellVampMod", BinPropertyType.Float)]
        public float PercentSpellVampMod { get; set; } = 0f;
        [MetaProperty("mPercentBonusMagicPenetrationMod", BinPropertyType.Float)]
        public float PercentBonusMagicPenetrationMod { get; set; } = 0f;
        [MetaProperty("mFlatHPPoolMod", BinPropertyType.Float)]
        public float FlatHPPoolMod { get; set; } = 0f;
        [MetaProperty("mHiddenFromOpponents", BinPropertyType.Bool)]
        public bool HiddenFromOpponents { get; set; } = false;
        [MetaProperty("mFlatBubbleRadiusMod", BinPropertyType.Float)]
        public float FlatBubbleRadiusMod { get; set; } = 0f;
        [MetaProperty("specialRecipe", BinPropertyType.Int32)]
        public int SpecialRecipe { get; set; } = 0;
        [MetaProperty("mItemGroups", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemGroups { get; set; } = new();
        [MetaProperty("mDisplayName", BinPropertyType.String)]
        public string DisplayName { get; set; } = "";
        [MetaProperty("mFlatPhysicalReduction", BinPropertyType.Float)]
        public float FlatPhysicalReduction { get; set; } = 0f;
        [MetaProperty("mMajorActiveItem", BinPropertyType.Bool)]
        public bool MajorActiveItem { get; set; } = false;
        [MetaProperty("consumeOnAcquire", BinPropertyType.Bool)]
        public bool ConsumeOnAcquire { get; set; } = false;
        [MetaProperty("spellName", BinPropertyType.String)]
        public string SpellName { get; set; } = "BaseSpell";
        [MetaProperty("mRequiredPurchaseIdentities", BinPropertyType.Container)]
        public MetaContainer<MetaHash> RequiredPurchaseIdentities { get; set; } = new();
        [MetaProperty("mFlatAttackRangeMod", BinPropertyType.Float)]
        public float FlatAttackRangeMod { get; set; } = 0f;
        [MetaProperty("mPercentSpellBlockMod", BinPropertyType.Float)]
        public float PercentSpellBlockMod { get; set; } = 0f;
        [MetaProperty("maxStack", BinPropertyType.Int32)]
        public int MaxStack { get; set; } = 0;
        [MetaProperty("mItemDataBuild", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemDataBuild> ItemDataBuild { get; set; } = new (new ());
        [MetaProperty("mRequiredBuffCurrencyName", BinPropertyType.String)]
        public string RequiredBuffCurrencyName { get; set; } = "";
        [MetaProperty("usableInStore", BinPropertyType.Bool)]
        public bool UsableInStore { get; set; } = false;
        [MetaProperty("SecondaryEpicness", BinPropertyType.Byte)]
        public byte SecondaryEpicness { get; set; } = 0;
        [MetaProperty("mPercentEXPBonus", BinPropertyType.Float)]
        public float PercentEXPBonus { get; set; } = 0f;
        [MetaProperty("mPercentArmorMod", BinPropertyType.Float)]
        public float PercentArmorMod { get; set; } = 0f;
        [MetaProperty("mPercentHPPoolMod", BinPropertyType.Float)]
        public float PercentHPPoolMod { get; set; } = 0f;
        [MetaProperty("mDisabledDescriptionOverride", BinPropertyType.String)]
        public string DisabledDescriptionOverride { get; set; } = "";
        [MetaProperty("mScripts", BinPropertyType.Container)]
        public MetaContainer<string> Scripts { get; set; } = new();
        [MetaProperty("mPercentCooldownMod", BinPropertyType.Float)]
        public float PercentCooldownMod { get; set; } = 0f;
        [MetaProperty("flatMPPoolMod", BinPropertyType.Float)]
        public float FlatMPPoolMod { get; set; } = 0f;
        [MetaProperty("mIsEnchantment", BinPropertyType.Bool)]
        public bool IsEnchantment { get; set; } = false;
        [MetaProperty("percentBaseMPRegenMod", BinPropertyType.Float)]
        public float PercentBaseMPRegenMod { get; set; } = 0f;
        [MetaProperty("mPercentMultiplicativeMovementSpeedMod", BinPropertyType.Float)]
        public float PercentMultiplicativeMovementSpeedMod { get; set; } = 0f;
        [MetaProperty("mItemCalculations", BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> ItemCalculations { get; set; } = new();
        [MetaProperty("mPercentMagicReduction", BinPropertyType.Float)]
        public float PercentMagicReduction { get; set; } = 0f;
        [MetaProperty("mPercentBubbleRadiusMod", BinPropertyType.Float)]
        public float PercentBubbleRadiusMod { get; set; } = 0f;
        [MetaProperty("price", BinPropertyType.Int32)]
        public int Price { get; set; } = 0;
        [MetaProperty("mFlatArmorMod", BinPropertyType.Float)]
        public float FlatArmorMod { get; set; } = 0f;
        [MetaProperty("mItemModifiers", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemModifiers { get; set; } = new();
        [MetaProperty("mPercentMovementSpeedMod", BinPropertyType.Float)]
        public float PercentMovementSpeedMod { get; set; } = 0f;
        [MetaProperty("PercentOmnivampMod", BinPropertyType.Float)]
        public float PercentOmnivampMod { get; set; } = 0f;
        [MetaProperty("mFlatDodgeMod", BinPropertyType.Float)]
        public float FlatDodgeMod { get; set; } = 0f;
        [MetaProperty("mFlatSpellBlockMod", BinPropertyType.Float)]
        public float FlatSpellBlockMod { get; set; } = 0f;
        [MetaProperty("mRequiredLevel", BinPropertyType.Int32)]
        public int RequiredLevel { get; set; } = 0;
        [MetaProperty("mFlatCritChanceMod", BinPropertyType.Float)]
        public float FlatCritChanceMod { get; set; } = 0f;
        [MetaProperty(3223041757, BinPropertyType.Byte)]
        public byte m3223041757 { get; set; } = 0;
        [MetaProperty("mPercentBaseHPRegenMod", BinPropertyType.Float)]
        public float PercentBaseHPRegenMod { get; set; } = 0f;
        [MetaProperty("mPercentHealingAmountMod", BinPropertyType.Float)]
        public float PercentHealingAmountMod { get; set; } = 0f;
        [MetaProperty("mRequiredSpellName", BinPropertyType.String)]
        public string RequiredSpellName { get; set; } = "";
        [MetaProperty("mPercentAttackSpeedMod", BinPropertyType.Float)]
        public float PercentAttackSpeedMod { get; set; } = 0f;
        [MetaProperty("mFlatMissChanceMod", BinPropertyType.Float)]
        public float FlatMissChanceMod { get; set; } = 0f;
        [MetaProperty("mPercentMultiplicativeAttackSpeedMod", BinPropertyType.Float)]
        public float PercentMultiplicativeAttackSpeedMod { get; set; } = 0f;
        [MetaProperty("mCooldownShowDisabledDuration", BinPropertyType.Float)]
        public float CooldownShowDisabledDuration { get; set; } = 0f;
        [MetaProperty("mPercentAttackRangeMod", BinPropertyType.Float)]
        public float PercentAttackRangeMod { get; set; } = 0f;
        [MetaProperty("mCategories", BinPropertyType.Container)]
        public MetaContainer<string> Categories { get; set; } = new();
        [MetaProperty("mPercentTenacityItemMod", BinPropertyType.Float)]
        public float PercentTenacityItemMod { get; set; } = 0f;
        [MetaProperty("clearUndoHistory", BinPropertyType.Byte)]
        public byte ClearUndoHistory { get; set; } = 1;
        [MetaProperty("mFlatCritDamageMod", BinPropertyType.Float)]
        public float FlatCritDamageMod { get; set; } = 0f;
        [MetaProperty("mCanBeSold", BinPropertyType.Bool)]
        public bool CanBeSold { get; set; } = false;
        [MetaProperty("mPercentPhysicalDamageMod", BinPropertyType.Float)]
        public float PercentPhysicalDamageMod { get; set; } = 0f;
        [MetaProperty("flatMPRegenMod", BinPropertyType.Float)]
        public float FlatMPRegenMod { get; set; } = 0f;
        [MetaProperty("mItemDataClient", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemDataClient> ItemDataClient { get; set; } = new (new ());
        [MetaProperty("mItemCalloutPlayer", BinPropertyType.Bool)]
        public bool ItemCalloutPlayer { get; set; } = false;
        [MetaProperty("mPercentCastRangeMod", BinPropertyType.Float)]
        public float PercentCastRangeMod { get; set; } = 0f;
        [MetaProperty("parentItemLink", BinPropertyType.ObjectLink)]
        public MetaObjectLink ParentItemLink { get; set; } = new(0);
        [MetaProperty("parentEnchantmentLink", BinPropertyType.ObjectLink)]
        public MetaObjectLink ParentEnchantmentLink { get; set; } = new(0);
        [MetaProperty("mRequiredChampion", BinPropertyType.String)]
        public string RequiredChampion { get; set; } = "";
        [MetaProperty("mParentEnchantmentGroup", BinPropertyType.String)]
        public string ParentEnchantmentGroup { get; set; } = "";
        [MetaProperty("mFlatMagicPenetrationMod", BinPropertyType.Float)]
        public float FlatMagicPenetrationMod { get; set; } = 0f;
        [MetaProperty("clickable", BinPropertyType.Bool)]
        public bool Clickable { get; set; } = false;
        [MetaProperty("mPercentSpellEffectivenessMod", BinPropertyType.Float)]
        public float PercentSpellEffectivenessMod { get; set; } = 0f;
        [MetaProperty("mEnchantmentEffectAmount", BinPropertyType.Container)]
        public MetaContainer<float> EnchantmentEffectAmount { get; set; } = new();
        [MetaProperty("requiredItemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> RequiredItemLinks { get; set; } = new();
        [MetaProperty(4216742028, BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<Class0x61f8c41c>> m4216742028 { get; set; } = new();
        [MetaProperty("mEffectByLevelAmount", BinPropertyType.Container)]
        public MetaContainer<float> EffectByLevelAmount { get; set; } = new();
    }
    [MetaClass("AlternateSpellAssets")]
    public class AlternateSpellAssets : IMetaClass
    {
        [MetaProperty("mHitEffectOrientType", BinPropertyType.UInt32)]
        public uint HitEffectOrientType { get; set; } = 0;
        [MetaProperty("mHitEffectPlayerKey", BinPropertyType.Hash)]
        public MetaHash HitEffectPlayerKey { get; set; } = new(0);
        [MetaProperty("mHaveHitBone", BinPropertyType.Bool)]
        public bool HaveHitBone { get; set; } = false;
        [MetaProperty("mHitBoneName", BinPropertyType.String)]
        public string HitBoneName { get; set; } = "";
        [MetaProperty("mHitEffectKey", BinPropertyType.Hash)]
        public MetaHash HitEffectKey { get; set; } = new(0);
        [MetaProperty("mAfterEffectName", BinPropertyType.String)]
        public string AfterEffectName { get; set; } = "";
        [MetaProperty("mUseAnimatorFramerate", BinPropertyType.Bool)]
        public bool UseAnimatorFramerate { get; set; } = false;
        [MetaProperty("mHaveHitEffect", BinPropertyType.Bool)]
        public bool HaveHitEffect { get; set; } = false;
        [MetaProperty("mHitEffectName", BinPropertyType.String)]
        public string HitEffectName { get; set; } = "";
        [MetaProperty("mAnimationLeadOutName", BinPropertyType.String)]
        public string AnimationLeadOutName { get; set; } = "";
        [MetaProperty("mAfterEffectKey", BinPropertyType.Hash)]
        public MetaHash AfterEffectKey { get; set; } = new(0);
        [MetaProperty("mAnimationLoopName", BinPropertyType.String)]
        public string AnimationLoopName { get; set; } = "";
        [MetaProperty("mHitEffectPlayerName", BinPropertyType.String)]
        public string HitEffectPlayerName { get; set; } = "";
        [MetaProperty("mAnimationWinddownName", BinPropertyType.String)]
        public string AnimationWinddownName { get; set; } = "";
        [MetaProperty("mAnimationName", BinPropertyType.String)]
        public string AnimationName { get; set; } = "";
    }
    [MetaClass("ScriptTable")]
    public interface ScriptTable : IMetaClass
    {
    }
    [MetaClass("ObjectTags")]
    public class ObjectTags : IMetaClass
    {
        [MetaProperty("mTagList", BinPropertyType.String)]
        public string TagList { get; set; } = "";
    }
    [MetaClass("TFTPhaseData")]
    public class TFTPhaseData : IMetaClass
    {
        [MetaProperty("mDisplay", BinPropertyType.UInt32)]
        public uint Display { get; set; } = 0;
        [MetaProperty("mAnnouncementData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnnouncementData { get; set; } = new(0);
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("mDuration", BinPropertyType.Float)]
        public float Duration { get; set; } = 0f;
        [MetaProperty("mLabel", BinPropertyType.String)]
        public string Label { get; set; } = "";
    }
    [MetaClass(638641575)]
    public class Class0x2610e5a7 : IMetaClass
    {
        [MetaProperty(806640254, BinPropertyType.Hash)]
        public MetaHash m806640254 { get; set; } = new(0);
        [MetaProperty(2031830946, BinPropertyType.Hash)]
        public MetaHash m2031830946 { get; set; } = new(0);
        [MetaProperty(2063105501, BinPropertyType.Hash)]
        public MetaHash m2063105501 { get; set; } = new(0);
        [MetaProperty(3184323325, BinPropertyType.Hash)]
        public MetaHash m3184323325 { get; set; } = new(0);
        [MetaProperty("ItemIcon", BinPropertyType.Hash)]
        public MetaHash ItemIcon { get; set; } = new(0);
        [MetaProperty(4195634750, BinPropertyType.Hash)]
        public MetaHash m4195634750 { get; set; } = new(0);
    }
    [MetaClass("OptionItemSliderInt")]
    public class OptionItemSliderInt : OptionItemSlider
    {
        [MetaProperty("TooltipTraKey", BinPropertyType.String)]
        public string TooltipTraKey { get; set; } = "";
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("optionScale", BinPropertyType.UInt32)]
        public uint OptionScale { get; set; } = 100;
        [MetaProperty("option", BinPropertyType.UInt16)]
        public ushort Option { get; set; } = 65535;
    }
    [MetaClass("TFTStreak")]
    public class TFTStreak : IMetaClass
    {
        [MetaProperty("mMinimumStreakLength", BinPropertyType.Optional)]
        public MetaOptional<uint> MinimumStreakLength { get; set; } = new MetaOptional<uint>(default(uint), false);
        [MetaProperty("mMaximumStreakLength", BinPropertyType.Optional)]
        public MetaOptional<uint> MaximumStreakLength { get; set; } = new MetaOptional<uint>(default(uint), false);
        [MetaProperty("mStreakFormat", BinPropertyType.String)]
        public string StreakFormat { get; set; } = "";
        [MetaProperty("mGoldRewardAmount", BinPropertyType.UInt32)]
        public uint GoldRewardAmount { get; set; } = 0;
    }
    [MetaClass("GDSMapObjectLightingInfo")]
    public class GDSMapObjectLightingInfo : GDSMapObjectExtraInfo
    {
        [MetaProperty("colors", BinPropertyType.Container)]
        public MetaContainer<Vector4> Colors { get; set; } = new();
    }
    [MetaClass(662826347)]
    public class Class0x2781ed6b : IMetaClass
    {
        [MetaProperty("BodyKey", BinPropertyType.String)]
        public string BodyKey { get; set; } = "";
        [MetaProperty("TitleKey", BinPropertyType.String)]
        public string TitleKey { get; set; } = "";
    }
    [MetaClass("IntTableSet")]
    public class IntTableSet : ScriptTableSet
    {
    }
    [MetaClass("AddPARCheat")]
    public class AddPARCheat : Cheat
    {
        [MetaProperty("mAmount", BinPropertyType.Float)]
        public float Amount { get; set; } = 0f;
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass("VfxPrimitiveArbitrarySegmentBeam")]
    public class VfxPrimitiveArbitrarySegmentBeam : VfxPrimitiveBeamBase
    {
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; } = new (new ());
    }
    [MetaClass("MapPointLightType")]
    public class MapPointLightType : IMetaClass
    {
        [MetaProperty("Impact", BinPropertyType.Int32)]
        public int Impact { get; set; } = 3;
        [MetaProperty("falloffColor", BinPropertyType.Vector4)]
        public Vector4 FalloffColor { get; set; } = new Vector4(0f, 0f, 0f, 1f);
        [MetaProperty("lightColor", BinPropertyType.Vector4)]
        public Vector4 LightColor { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        [MetaProperty("specular", BinPropertyType.Bool)]
        public bool Specular { get; set; } = true;
        [MetaProperty("castStaticShadows", BinPropertyType.Bool)]
        public bool CastStaticShadows { get; set; } = true;
        [MetaProperty("radius", BinPropertyType.Float)]
        public float Radius { get; set; } = 500f;
    }
    [MetaClass("GearSkinUpgrade")]
    public class GearSkinUpgrade : IMetaClass
    {
        [MetaProperty(898435083, BinPropertyType.String)]
        public string m898435083 { get; set; } = "";
        [MetaProperty("mGearData", BinPropertyType.Structure)]
        public GearData GearData { get; set; } = null;
    }
    [MetaClass("TargeterDefinitionSpline")]
    public class TargeterDefinitionSpline : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("frontTextureName", BinPropertyType.String)]
        public string FrontTextureName { get; set; } = "";
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; } = new (new ());
        [MetaProperty("constraintRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> ConstraintRange { get; set; } = new (new ());
        [MetaProperty("isConstrainedToRange", BinPropertyType.Bool)]
        public bool IsConstrainedToRange { get; set; } = false;
        [MetaProperty("splineWidth", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> SplineWidth { get; set; } = new (new ());
        [MetaProperty("overrideSpline", BinPropertyType.Structure)]
        public ISplineInfo OverrideSpline { get; set; } = null;
        [MetaProperty("minSegmentCount", BinPropertyType.UInt32)]
        public uint MinSegmentCount { get; set; } = 10;
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; } = new (new ());
        [MetaProperty("baseTextureName", BinPropertyType.String)]
        public string BaseTextureName { get; set; } = "";
        [MetaProperty("maxSegmentLength", BinPropertyType.Float)]
        public float MaxSegmentLength { get; set; } = 50f;
    }
    [MetaClass("VfxPrimitiveCameraTrail")]
    public class VfxPrimitiveCameraTrail : VfxPrimitiveTrailBase
    {
        [MetaProperty("mTrail", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxTrailDefinitionData> Trail { get; set; } = new (new ());
    }
    [MetaClass("LootStatus")]
    public class LootStatus : IMetaClass
    {
        [MetaProperty("mLifetimeMax", BinPropertyType.Int32)]
        public int LifetimeMax { get; set; } = 0;
        [MetaProperty("mTags", BinPropertyType.Container)]
        public MetaContainer<string> Tags { get; set; } = new();
        [MetaProperty("mImageTexturePath", BinPropertyType.String)]
        public string ImageTexturePath { get; set; } = "";
        [MetaProperty("mInactiveDate", BinPropertyType.String)]
        public string InactiveDate { get; set; } = "";
        [MetaProperty("mActiveDate", BinPropertyType.String)]
        public string ActiveDate { get; set; } = "";
        [MetaProperty("mActive", BinPropertyType.Bool)]
        public bool Active { get; set; } = false;
        [MetaProperty("mAutoRedeem", BinPropertyType.Bool)]
        public bool AutoRedeem { get; set; } = false;
    }
    [MetaClass("KillAllTurretsCheat")]
    public class KillAllTurretsCheat : Cheat
    {
    }
    [MetaClass("BlendedLinearHeightSolver")]
    public class BlendedLinearHeightSolver : HeightSolverType
    {
    }
    [MetaClass("RecallDecalData")]
    public class RecallDecalData : IMetaClass
    {
        [MetaProperty("effectFile", BinPropertyType.String)]
        public string EffectFile { get; set; } = "";
        [MetaProperty("EmpoweredArrivalFile", BinPropertyType.String)]
        public string EmpoweredArrivalFile { get; set; } = "";
        [MetaProperty("EmpoweredEffectFile", BinPropertyType.String)]
        public string EmpoweredEffectFile { get; set; } = "";
        [MetaProperty("recallDecalId", BinPropertyType.UInt32)]
        public uint RecallDecalId { get; set; } = 0;
        [MetaProperty("arrivalEffectFile", BinPropertyType.String)]
        public string ArrivalEffectFile { get; set; } = "";
    }
    [MetaClass("ContextualActionPlayVO")]
    public class ContextualActionPlayVO : ContextualActionPlayAudio
    {
        [MetaProperty("mWaitForAnnouncerQueue", BinPropertyType.Bool)]
        public bool WaitForAnnouncerQueue { get; set; } = false;
        [MetaProperty("mWaitTimeout", BinPropertyType.Float)]
        public float WaitTimeout { get; set; } = 0f;
        [MetaProperty("mEnemyEventName", BinPropertyType.String)]
        public string EnemyEventName { get; set; } = "";
        [MetaProperty(1422745546, BinPropertyType.Bool)]
        public bool m1422745546 { get; set; } = false;
        [MetaProperty(1721877131, BinPropertyType.String)]
        public string m1721877131 { get; set; } = "";
        [MetaProperty("mAllyEventName", BinPropertyType.String)]
        public string AllyEventName { get; set; } = "";
        [MetaProperty(3199620533, BinPropertyType.Bool)]
        public bool m3199620533 { get; set; } = false;
        [MetaProperty("mSelfEventName", BinPropertyType.String)]
        public string SelfEventName { get; set; } = "";
        [MetaProperty("mSpectatorEventName", BinPropertyType.String)]
        public string SpectatorEventName { get; set; } = "";
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        public uint MaxOccurences { get; set; } = 0;
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        public MetaHash HashedSituationTrigger { get; set; } = new(0);
    }
    [MetaClass(702535597)]
    public class Class0x29dfd7ad : IMetaClass
    {
        [MetaProperty(1692727801, BinPropertyType.Float)]
        public float m1692727801 { get; set; } = 0f;
        [MetaProperty(2829252295, BinPropertyType.Float)]
        public float m2829252295 { get; set; } = 0f;
        [MetaProperty(2934688733, BinPropertyType.Float)]
        public float m2934688733 { get; set; } = 0f;
    }
    [MetaClass("OptionTemplateHotkeys")]
    public class OptionTemplateHotkeys : IOptionTemplate
    {
        [MetaProperty(789705163, BinPropertyType.Hash)]
        public MetaHash m789705163 { get; set; } = new(0);
        [MetaProperty("hotkeyModifierText", BinPropertyType.Hash)]
        public MetaHash HotkeyModifierText { get; set; } = new(0);
        [MetaProperty("CastAllButtonDefinition", BinPropertyType.Hash)]
        public MetaHash CastAllButtonDefinition { get; set; } = new(0);
        [MetaProperty("HotkeyButtonDefinition", BinPropertyType.Hash)]
        public MetaHash HotkeyButtonDefinition { get; set; } = new(0);
        [MetaProperty(2833578361, BinPropertyType.Hash)]
        public MetaHash m2833578361 { get; set; } = new(0);
        [MetaProperty("Labels", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<OptionTemplateHotkeysLabel>> Labels { get; set; } = new();
        [MetaProperty("HotkeyButtonTextSmall", BinPropertyType.Hash)]
        public MetaHash HotkeyButtonTextSmall { get; set; } = new(0);
        [MetaProperty("HotkeyQuickCastButtonDefinition", BinPropertyType.Hash)]
        public MetaHash HotkeyQuickCastButtonDefinition { get; set; } = new(0);
        [MetaProperty("keys", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<OptionTemplateHotkeysKey>> Keys { get; set; } = new();
    }
    [MetaClass("IntGet")]
    public class IntGet : IIntGet
    {
        [MetaProperty("value", BinPropertyType.Int32)]
        public int Value { get; set; } = 0;
    }
    [MetaClass("HasAtleastNSubRequirementsCastRequirement")]
    public class HasAtleastNSubRequirementsCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mSuccessesRequired", BinPropertyType.UInt32)]
        public uint SuccessesRequired { get; set; } = 0;
        [MetaProperty("mSubRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> SubRequirements { get; set; } = new();
    }
    [MetaClass("MinimapBackgroundConfig")]
    public class MinimapBackgroundConfig : IMetaClass
    {
        [MetaProperty("mCustomMinimapBackgrounds", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<MinimapBackground>> CustomMinimapBackgrounds { get; set; } = new();
        [MetaProperty("mDefaultTextureName", BinPropertyType.String)]
        public string DefaultTextureName { get; set; } = "2DLevelMiniMap.dds";
    }
    [MetaClass("VfxSineMaterialDriver")]
    public class VfxSineMaterialDriver : IVfxMaterialDriver
    {
        [MetaProperty("mBias", BinPropertyType.Float)]
        public float Bias { get; set; } = 0f;
        [MetaProperty("mFrequency", BinPropertyType.Float)]
        public float Frequency { get; set; } = 1f;
        [MetaProperty("mScale", BinPropertyType.Float)]
        public float Scale { get; set; } = 1f;
    }
    [MetaClass("ScriptCommentBlock")]
    public class ScriptCommentBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; } = new (new ());
    }
    [MetaClass("GameModeConstantsGroup")]
    public class GameModeConstantsGroup : IMetaClass
    {
        [MetaProperty("mConstants", BinPropertyType.Map)]
        public Dictionary<MetaHash, GameModeConstant> Constants { get; set; } = new();
    }
    [MetaClass("MaskData")]
    public class MaskData : IMetaClass
    {
        [MetaProperty("mWeightList", BinPropertyType.Container)]
        public MetaContainer<float> WeightList { get; set; } = new();
        [MetaProperty("mId", BinPropertyType.UInt32)]
        public uint Id { get; set; } = 0;
    }
    [MetaClass("AnnouncementIcon")]
    public class AnnouncementIcon : IMetaClass
    {
        [MetaProperty(277776144, BinPropertyType.Hash)]
        public MetaHash m277776144 { get; set; } = new(0);
        [MetaProperty("AlliedElementGroup", BinPropertyType.Hash)]
        public MetaHash AlliedElementGroup { get; set; } = new(0);
        [MetaProperty(1043842619, BinPropertyType.Hash)]
        public MetaHash m1043842619 { get; set; } = new(0);
        [MetaProperty(3269632735, BinPropertyType.Hash)]
        public MetaHash m3269632735 { get; set; } = new(0);
        [MetaProperty(3681849836, BinPropertyType.Hash)]
        public MetaHash m3681849836 { get; set; } = new(0);
        [MetaProperty(3698627455, BinPropertyType.Hash)]
        public MetaHash m3698627455 { get; set; } = new(0);
        [MetaProperty(3715405074, BinPropertyType.Hash)]
        public MetaHash m3715405074 { get; set; } = new(0);
        [MetaProperty(3732182693, BinPropertyType.Hash)]
        public MetaHash m3732182693 { get; set; } = new(0);
        [MetaProperty("EnemyElementGroup", BinPropertyType.Hash)]
        public MetaHash EnemyElementGroup { get; set; } = new(0);
    }
    [MetaClass("FxSequence")]
    public class FxSequence : IMetaClass
    {
        [MetaProperty("Actions", BinPropertyType.Container)]
        public MetaContainer<IFxAction> Actions { get; set; } = new();
    }
    [MetaClass("MapVisibilityFlagDefinition")]
    public class MapVisibilityFlagDefinition : IMetaClass
    {
        [MetaProperty("PublicName", BinPropertyType.String)]
        public string PublicName { get; set; } = "";
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float TransitionTime { get; set; } = 3f;
        [MetaProperty("name", BinPropertyType.Hash)]
        public MetaHash Name { get; set; } = new(0);
        [MetaProperty("BitIndex", BinPropertyType.Byte)]
        public byte BitIndex { get; set; } = 0;
    }
    [MetaClass(732494899)]
    public class Class0x2ba8fc33 : IMetaClass
    {
        [MetaProperty(1528812482, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<RegaliaData>> m1528812482 { get; set; } = new();
    }
    [MetaClass("VfxColorOverLifeMaterialDriver")]
    public class VfxColorOverLifeMaterialDriver : IVfxMaterialDriver
    {
        [MetaProperty("frequency", BinPropertyType.Byte)]
        public byte Frequency { get; set; } = 0;
        [MetaProperty("colors", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedColorVariableData> Colors { get; set; } = new (new ());
    }
    [MetaClass("SurrenderTypeData")]
    public class SurrenderTypeData : IMetaClass
    {
        [MetaProperty(1064497274, BinPropertyType.Float)]
        public float m1064497274 { get; set; } = 1200f;
        [MetaProperty("windowLength", BinPropertyType.Float)]
        public float WindowLength { get; set; } = 60f;
        [MetaProperty("VoteTimeout", BinPropertyType.Float)]
        public float VoteTimeout { get; set; } = 60f;
        [MetaProperty("percentageRequired", BinPropertyType.Float)]
        public float PercentageRequired { get; set; } = 0.699999988079071f;
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 1200f;
    }
    [MetaClass("FloatingTextTypeList")]
    public class FloatingTextTypeList : IMetaClass
    {
        [MetaProperty("EnemyMagicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink EnemyMagicalDamage { get; set; } = new(0);
        [MetaProperty("ScoreProject0", BinPropertyType.ObjectLink)]
        public MetaObjectLink ScoreProject0 { get; set; } = new(0);
        [MetaProperty("ScoreProject1", BinPropertyType.ObjectLink)]
        public MetaObjectLink ScoreProject1 { get; set; } = new(0);
        [MetaProperty("MagicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink MagicalDamage { get; set; } = new(0);
        [MetaProperty("Dodge", BinPropertyType.ObjectLink)]
        public MetaObjectLink Dodge { get; set; } = new(0);
        [MetaProperty("TFTUnitLabel", BinPropertyType.ObjectLink)]
        public MetaObjectLink TFTUnitLabel { get; set; } = new(0);
        [MetaProperty("Heal", BinPropertyType.ObjectLink)]
        public MetaObjectLink Heal { get; set; } = new(0);
        [MetaProperty("PracticeToolTotal", BinPropertyType.ObjectLink)]
        public MetaObjectLink PracticeToolTotal { get; set; } = new(0);
        [MetaProperty("ManaDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink ManaDamage { get; set; } = new(0);
        [MetaProperty("Debug", BinPropertyType.ObjectLink)]
        public MetaObjectLink Debug { get; set; } = new(0);
        [MetaProperty("EnemyTrueDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink EnemyTrueDamageCritical { get; set; } = new(0);
        [MetaProperty("EnemyMagicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink EnemyMagicalDamageCritical { get; set; } = new(0);
        [MetaProperty("MagicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink MagicalDamageCritical { get; set; } = new(0);
        [MetaProperty("Invulnerable", BinPropertyType.ObjectLink)]
        public MetaObjectLink Invulnerable { get; set; } = new(0);
        [MetaProperty("ScoreDarkStar", BinPropertyType.ObjectLink)]
        public MetaObjectLink ScoreDarkStar { get; set; } = new(0);
        [MetaProperty("Absorbed", BinPropertyType.ObjectLink)]
        public MetaObjectLink Absorbed { get; set; } = new(0);
        [MetaProperty("EnemyPhysicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink EnemyPhysicalDamage { get; set; } = new(0);
        [MetaProperty("Experience", BinPropertyType.ObjectLink)]
        public MetaObjectLink Experience { get; set; } = new(0);
        [MetaProperty("PhysicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink PhysicalDamageCritical { get; set; } = new(0);
        [MetaProperty("PracticeToolDPS", BinPropertyType.ObjectLink)]
        public MetaObjectLink PracticeToolDPS { get; set; } = new(0);
        [MetaProperty("level", BinPropertyType.ObjectLink)]
        public MetaObjectLink Level { get; set; } = new(0);
        [MetaProperty("EnemyTrueDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink EnemyTrueDamage { get; set; } = new(0);
        [MetaProperty("QuestReceived", BinPropertyType.ObjectLink)]
        public MetaObjectLink QuestReceived { get; set; } = new(0);
        [MetaProperty("OMW", BinPropertyType.ObjectLink)]
        public MetaObjectLink OMW { get; set; } = new(0);
        [MetaProperty("Countdown", BinPropertyType.ObjectLink)]
        public MetaObjectLink Countdown { get; set; } = new(0);
        [MetaProperty("QuestComplete", BinPropertyType.ObjectLink)]
        public MetaObjectLink QuestComplete { get; set; } = new(0);
        [MetaProperty("Disable", BinPropertyType.ObjectLink)]
        public MetaObjectLink Disable { get; set; } = new(0);
        [MetaProperty("EnemyPhysicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink EnemyPhysicalDamageCritical { get; set; } = new(0);
        [MetaProperty("Score", BinPropertyType.ObjectLink)]
        public MetaObjectLink Score { get; set; } = new(0);
        [MetaProperty("TrueDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink TrueDamageCritical { get; set; } = new(0);
        [MetaProperty("Special", BinPropertyType.ObjectLink)]
        public MetaObjectLink Special { get; set; } = new(0);
        [MetaProperty("PracticeToolLastHit", BinPropertyType.ObjectLink)]
        public MetaObjectLink PracticeToolLastHit { get; set; } = new(0);
        [MetaProperty("TrueDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink TrueDamage { get; set; } = new(0);
        [MetaProperty("ShieldBonusDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink ShieldBonusDamage { get; set; } = new(0);
        [MetaProperty("ManaHeal", BinPropertyType.ObjectLink)]
        public MetaObjectLink ManaHeal { get; set; } = new(0);
        [MetaProperty("Gold", BinPropertyType.ObjectLink)]
        public MetaObjectLink Gold { get; set; } = new(0);
        [MetaProperty("PhysicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink PhysicalDamage { get; set; } = new(0);
    }
    [MetaClass("HudHealthBarDefenseModifierData")]
    public class HudHealthBarDefenseModifierData : IMetaClass
    {
        [MetaProperty("defenseDownL3Percent", BinPropertyType.Float)]
        public float DefenseDownL3Percent { get; set; } = -0.20999999344348907f;
        [MetaProperty("defenseUpPercent", BinPropertyType.Float)]
        public float DefenseUpPercent { get; set; } = 0.20000000298023224f;
        [MetaProperty("defenseDownL2Percent", BinPropertyType.Float)]
        public float DefenseDownL2Percent { get; set; } = -0.14000000059604645f;
        [MetaProperty("defenseDownL1Percent", BinPropertyType.Float)]
        public float DefenseDownL1Percent { get; set; } = -0.07000000029802322f;
    }
    [MetaClass("LineTargetToCaster")]
    public class LineTargetToCaster : TargetingTypeData
    {
    }
    [MetaClass("FxTransform")]
    public class FxTransform : IMetaClass
    {
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
        [MetaProperty("Index", BinPropertyType.Int32)]
        public int Index { get; set; } = 0;
        [MetaProperty("AttachmentName", BinPropertyType.String)]
        public string AttachmentName { get; set; } = "";
    }
    [MetaClass("IsOwnerAliveSpawnConditions")]
    public class IsOwnerAliveSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<IsOwnerAliveConditionData>> Conditions { get; set; } = new();
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; } = new (new ());
    }
    [MetaClass("ElementGroupSliderData")]
    public class ElementGroupSliderData : ElementGroupData
    {
        [MetaProperty("SliderClickedState", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupSliderState> SliderClickedState { get; set; } = new (new ());
        [MetaProperty("SliderHitRegion", BinPropertyType.ObjectLink)]
        public MetaObjectLink SliderHitRegion { get; set; } = new(0);
        [MetaProperty(96062416, BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupSliderState> m96062416 { get; set; } = new (new ());
        [MetaProperty("soundEvents", BinPropertyType.Structure)]
        public Class0xaf7ac937 SoundEvents { get; set; } = null;
        [MetaProperty(3035679710, BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupSliderState> m3035679710 { get; set; } = new (new ());
        [MetaProperty("DefaultState", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupSliderState> DefaultState { get; set; } = new (new ());
        [MetaProperty("direction", BinPropertyType.Byte)]
        public byte Direction { get; set; } = 0;
        [MetaProperty("BarHitRegion", BinPropertyType.ObjectLink)]
        public MetaObjectLink BarHitRegion { get; set; } = new(0);
    }
    [MetaClass("EsportsBannerData")]
    public class EsportsBannerData : IMetaClass
    {
        [MetaProperty("bannerName", BinPropertyType.String)]
        public string BannerName { get; set; } = "";
        [MetaProperty("Team", BinPropertyType.UInt32)]
        public uint Team { get; set; } = 0;
    }
    [MetaClass("RegaliaRankedBannerMap")]
    public class RegaliaRankedBannerMap : IMetaClass
    {
        [MetaProperty(3317216616, BinPropertyType.Map)]
        public Dictionary<string, MetaObjectLink> m3317216616 { get; set; } = new();
    }
    [MetaClass("ElementGroupGridData")]
    public class ElementGroupGridData : ElementGroupData
    {
        [MetaProperty("LayoutType", BinPropertyType.UInt32)]
        public uint LayoutType { get; set; } = 0;
        [MetaProperty("GridRegion", BinPropertyType.ObjectLink)]
        public MetaObjectLink GridRegion { get; set; } = new(0);
    }
    [MetaClass("DynamicMaterialDef")]
    public class DynamicMaterialDef : IMetaClass
    {
        [MetaProperty("staticSwitch", BinPropertyType.Structure)]
        public DynamicMaterialStaticSwitch StaticSwitch { get; set; } = null;
        [MetaProperty("parameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialParameterDef>> Parameters { get; set; } = new();
        [MetaProperty(1590877069, BinPropertyType.Bool)]
        public bool m1590877069 { get; set; } = false;
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialTextureSwapDef>> Textures { get; set; } = new();
    }
    [MetaClass(765791391)]
    public class Class0x2da50c9f : IMetaClass
    {
        [MetaProperty(1943793530, BinPropertyType.String)]
        public string m1943793530 { get; set; } = "";
        [MetaProperty(2400858989, BinPropertyType.String)]
        public string m2400858989 { get; set; } = "";
        [MetaProperty(3624350102, BinPropertyType.String)]
        public string m3624350102 { get; set; } = "";
    }
    [MetaClass("FontType")]
    public class FontType : IMetaClass
    {
        [MetaProperty("localeTypes", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FontLocaleType>> LocaleTypes { get; set; } = new();
    }
    [MetaClass("ContextualConditionChanceToPlay")]
    public class ContextualConditionChanceToPlay : IContextualCondition
    {
        [MetaProperty("mPercentChanceToPlay", BinPropertyType.Byte)]
        public byte PercentChanceToPlay { get; set; } = 0;
    }
    [MetaClass("WidthPerSecond")]
    public class WidthPerSecond : MissileBehaviorSpec
    {
        [MetaProperty("mWidthPerSecond", BinPropertyType.Float)]
        public float mWidthPerSecond { get; set; } = 0f;
    }
    [MetaClass("MapCloudsLayer")]
    public class MapCloudsLayer : IMetaClass
    {
        [MetaProperty("speed", BinPropertyType.Float)]
        public float Speed { get; set; } = 0.0010000000474974513f;
        [MetaProperty("scale", BinPropertyType.Float)]
        public float Scale { get; set; } = 10f;
        [MetaProperty("direction", BinPropertyType.Vector2)]
        public Vector2 Direction { get; set; } = new Vector2(1f, 0f);
    }
    [MetaClass("ISplineInfo")]
    public interface ISplineInfo : IMetaClass
    {
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        bool UseMissilePositionAsOrigin { get; set; }
        [MetaProperty("mStartPositionOffset", BinPropertyType.Vector3)]
        Vector3 StartPositionOffset { get; set; }
    }
    [MetaClass("VfxAnimatedVector2fVariableData")]
    public class VfxAnimatedVector2fVariableData : IMetaClass
    {
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<Vector2> Values { get; set; } = new();
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; } = new();
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; } = new();
    }
    [MetaClass("ItemRecommendationMatrix")]
    public class ItemRecommendationMatrix : IMetaClass
    {
        [MetaProperty("mrows", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationMatrixRow>> Mrows { get; set; } = new();
    }
    [MetaClass(779902682)]
    public class Class0x2e7c5eda : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("level", BinPropertyType.UInt32)]
        public uint Level { get; set; } = 0;
    }
    [MetaClass("ContextualConditionAnyOtherHero")]
    public class ContextualConditionAnyOtherHero : IContextualCondition
    {
        [MetaProperty("mChildConditions", BinPropertyType.Container)]
        public MetaContainer<ICharacterSubcondition> ChildConditions { get; set; } = new();
    }
    [MetaClass("AbovePARPercentCastRequirement")]
    public class AbovePARPercentCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mPARType", BinPropertyType.Byte)]
        public byte PARType { get; set; } = 0;
        [MetaProperty("mCurrentPercentPAR", BinPropertyType.Float)]
        public float CurrentPercentPAR { get; set; } = 0f;
    }
    [MetaClass("ScriptPreloadCharacter")]
    public class ScriptPreloadCharacter : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string PreloadResourceName { get; set; } = "";
    }
    [MetaClass("FlexTypeFloat")]
    public class FlexTypeFloat : IMetaClass
    {
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint FlexID { get; set; } = 0;
    }
    [MetaClass("FunctionTableGet")]
    public class FunctionTableGet : IFunctionGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
    }
    [MetaClass("AreaClamped")]
    public class AreaClamped : TargetingTypeData
    {
    }
    [MetaClass("TFTCharacterRecord")]
    public class TFTCharacterRecord : CharacterRecord
    {
        [MetaProperty("mMoveProximity", BinPropertyType.Float)]
        public float MoveProximity { get; set; } = 300f;
        [MetaProperty("mLinkedTraits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTTraitContributionData>> LinkedTraits { get; set; } = new();
        [MetaProperty(1246904587, BinPropertyType.Bool)]
        public bool m1246904587 { get; set; } = true;
        [MetaProperty(1311286886, BinPropertyType.String)]
        public string m1311286886 { get; set; } = "C_Buffbone_Glb_Overhead_Loc";
        [MetaProperty("mInitialMana", BinPropertyType.Float)]
        public float InitialMana { get; set; } = 0f;
        [MetaProperty("tier", BinPropertyType.Byte)]
        public byte Tier { get; set; } = 0;
        [MetaProperty("mMoveRange", BinPropertyType.Float)]
        public float MoveRange { get; set; } = 1200f;
        [MetaProperty(2092713826, BinPropertyType.Float)]
        public float m2092713826 { get; set; } = 10f;
        [MetaProperty("mMoveHeight", BinPropertyType.Float)]
        public float MoveHeight { get; set; } = 20f;
        [MetaProperty(3144797065, BinPropertyType.Bool)]
        public bool m3144797065 { get; set; } = false;
        [MetaProperty(3645083651, BinPropertyType.Vector3)]
        public Vector3 m3645083651 { get; set; } = new Vector3(100f, 0f, 0f);
        [MetaProperty("mMoveInterval", BinPropertyType.Float)]
        public float MoveInterval { get; set; } = 2f;
        [MetaProperty("mShopData", BinPropertyType.ObjectLink)]
        public MetaObjectLink ShopData { get; set; } = new(0);
        [MetaProperty(4015458703, BinPropertyType.Bool)]
        public bool m4015458703 { get; set; } = true;
        [MetaProperty("PortraitIcon", BinPropertyType.String)]
        public string PortraitIcon { get; set; } = "";
    }
    [MetaClass("LearnedSpellDynamicMaterialBoolDriver")]
    public class LearnedSpellDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mSlot", BinPropertyType.Byte)]
        public byte Slot { get; set; } = 0;
    }
    [MetaClass("SineMaterialDriver")]
    public class SineMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mBias", BinPropertyType.Float)]
        public float Bias { get; set; } = 0f;
        [MetaProperty("mFrequency", BinPropertyType.Float)]
        public float Frequency { get; set; } = 1f;
        [MetaProperty("mScale", BinPropertyType.Float)]
        public float Scale { get; set; } = 1f;
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; } = null;
    }
    [MetaClass("ComparisonScriptCondition")]
    public class ComparisonScriptCondition : IScriptCondition
    {
        [MetaProperty("Operation", BinPropertyType.UInt32)]
        public uint Operation { get; set; } = 0;
        [MetaProperty("Value2", BinPropertyType.Structure)]
        public IScriptValueGet Value2 { get; set; } = null;
        [MetaProperty("Value1", BinPropertyType.Structure)]
        public IScriptValueGet Value1 { get; set; } = null;
    }
    [MetaClass("RecSpellRankUpInfo")]
    public class RecSpellRankUpInfo : IMetaClass
    {
        [MetaProperty("mDefaultPriority", BinPropertyType.Container)]
        public MetaContainer<byte> DefaultPriority { get; set; } = new();
        [MetaProperty("mEarlyLevelOverrides", BinPropertyType.Container)]
        public MetaContainer<byte> EarlyLevelOverrides { get; set; } = new();
    }
    [MetaClass("ShaderTexture")]
    public class ShaderTexture : IMetaClass
    {
        [MetaProperty("defaultTexturePath", BinPropertyType.String)]
        public string DefaultTexturePath { get; set; } = "";
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
    }
    [MetaClass("ContextualConditionCharacterPlayingEmote")]
    public class ContextualConditionCharacterPlayingEmote : ICharacterSubcondition
    {
        [MetaProperty("mEmoteID", BinPropertyType.Byte)]
        public byte EmoteID { get; set; } = 0;
    }
    [MetaClass("NextBuffVarsTable")]
    public class NextBuffVarsTable : ScriptTable
    {
    }
    [MetaClass("Target")]
    public class Target : TargetingTypeData
    {
        [MetaProperty(1871894195, BinPropertyType.Bool)]
        public bool m1871894195 { get; set; } = false;
    }
    [MetaClass("LoadoutDamageSkinInfoPanel")]
    public class LoadoutDamageSkinInfoPanel : ILoadoutInfoPanel
    {
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty("tierButton", BinPropertyType.Container)]
        public MetaContainer<MetaHash> TierButton { get; set; } = new();
    }
    [MetaClass("LockRootOrientationRigPoseModifierData")]
    public class LockRootOrientationRigPoseModifierData : BaseRigPoseModifierData
    {
    }
    [MetaClass("MapAlternateAssets")]
    public class MapAlternateAssets : IMetaClass
    {
        [MetaProperty("mAlternateAssets", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapAlternateAsset>> AlternateAssets { get; set; } = new();
    }
    [MetaClass(851321958)]
    public class Class0x32be2466 : IMetaClass
    {
        [MetaProperty(438884130, BinPropertyType.Map)]
        public Dictionary<uint, MetaObjectLink> m438884130 { get; set; } = new();
    }
    [MetaClass("SkinCharacterDataProperties_CharacterIdleEffect")]
    public class SkinCharacterDataProperties_CharacterIdleEffect : IMetaClass
    {
        [MetaProperty("boneName", BinPropertyType.String)]
        public string BoneName { get; set; } = "";
        [MetaProperty("effectName", BinPropertyType.String)]
        public string EffectName { get; set; } = "";
        [MetaProperty("position", BinPropertyType.Vector3)]
        public Vector3 Position { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("effectKey", BinPropertyType.Hash)]
        public MetaHash EffectKey { get; set; } = new(0);
        [MetaProperty("targetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
    }
    [MetaClass("TFTGameVariationData")]
    public class TFTGameVariationData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mAnnouncementData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnnouncementData { get; set; } = new(0);
        [MetaProperty("mTooltipIconPath", BinPropertyType.String)]
        public string TooltipIconPath { get; set; } = "";
        [MetaProperty("mTooltipTitleTra", BinPropertyType.String)]
        public string TooltipTitleTra { get; set; } = "";
        [MetaProperty("mTooltipDescriptionTra", BinPropertyType.String)]
        public string TooltipDescriptionTra { get; set; } = "";
        [MetaProperty("mStageIconPath", BinPropertyType.String)]
        public string StageIconPath { get; set; } = "";
    }
    [MetaClass("NamedDataValueCalculationPart")]
    public class NamedDataValueCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mDataValue", BinPropertyType.Hash)]
        public MetaHash DataValue { get; set; } = new(0);
    }
    [MetaClass("UseAutoattackCastTimeData")]
    public class UseAutoattackCastTimeData : IMetaClass
    {
        [MetaProperty("mAutoattackCastTimeCalculation", BinPropertyType.Structure)]
        public IGameCalculation AutoattackCastTimeCalculation { get; set; } = null;
        [MetaProperty(2251275924, BinPropertyType.Bool)]
        public bool m2251275924 { get; set; } = true;
    }
    [MetaClass("TargeterDefinitionLine")]
    public class TargeterDefinitionLine : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("minAngleRangeFactor", BinPropertyType.Float)]
        public float MinAngleRangeFactor { get; set; } = 1f;
        [MetaProperty("lineWidth", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> LineWidth { get; set; } = new (new ());
        [MetaProperty("fadeAngle", BinPropertyType.Float)]
        public float FadeAngle { get; set; } = 0f;
        [MetaProperty("textureBaseMaxGrowName", BinPropertyType.String)]
        public string TextureBaseMaxGrowName { get; set; } = "";
        [MetaProperty("useGlobalLineIndicator", BinPropertyType.Bool)]
        public bool UseGlobalLineIndicator { get; set; } = false;
        [MetaProperty("textureBaseOverrideName", BinPropertyType.String)]
        public string TextureBaseOverrideName { get; set; } = "";
        [MetaProperty("mCenterArrowToEndPoint", BinPropertyType.Bool)]
        public bool CenterArrowToEndPoint { get; set; } = false;
        [MetaProperty("indicatorType", BinPropertyType.Structure)]
        public ILineIndicatorType IndicatorType { get; set; } = null;
        [MetaProperty("textureTargetOverrideName", BinPropertyType.String)]
        public string TextureTargetOverrideName { get; set; } = "";
        [MetaProperty("rangeGrowthMax", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthMax { get; set; } = new (new ());
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; } = new (new ());
        [MetaProperty("facingLine", BinPropertyType.Bool)]
        public bool FacingLine { get; set; } = false;
        [MetaProperty("lineStopsAtEndPosition", BinPropertyType.Optional)]
        public MetaOptional<bool> LineStopsAtEndPosition { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("alwaysDraw", BinPropertyType.Bool)]
        public bool AlwaysDraw { get; set; } = false;
        [MetaProperty("rangeGrowthDuration", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthDuration { get; set; } = new (new ());
        [MetaProperty("maxAngleRangeFactor", BinPropertyType.Float)]
        public float MaxAngleRangeFactor { get; set; } = 1f;
        [MetaProperty("maxAngle", BinPropertyType.Float)]
        public float MaxAngle { get; set; } = 180f;
        [MetaProperty("mAngleLineToEndpointHeight", BinPropertyType.Bool)]
        public bool AngleLineToEndpointHeight { get; set; } = false;
        [MetaProperty("fallbackDirection", BinPropertyType.UInt32)]
        public uint FallbackDirection { get; set; } = 1;
        [MetaProperty("arrowSize", BinPropertyType.Float)]
        public float ArrowSize { get; set; } = 0f;
        [MetaProperty("rangeGrowthStartTime", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthStartTime { get; set; } = new (new ());
        [MetaProperty("minimumDisplayedRange", BinPropertyType.Float)]
        public float MinimumDisplayedRange { get; set; } = 50f;
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; } = new (new ());
        [MetaProperty("overrideBaseRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideBaseRange { get; set; } = new (new ());
        [MetaProperty("hasMaxGrowRange", BinPropertyType.Bool)]
        public bool HasMaxGrowRange { get; set; } = false;
        [MetaProperty("textureTargetMaxGrowName", BinPropertyType.String)]
        public string TextureTargetMaxGrowName { get; set; } = "";
        [MetaProperty("fade", BinPropertyType.Bool)]
        public bool Fade { get; set; } = true;
        [MetaProperty("minAngle", BinPropertyType.Float)]
        public float MinAngle { get; set; } = 0f;
    }
    [MetaClass("X3DSharedData")]
    public class X3DSharedData : IMetaClass
    {
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<X3DSharedTextureDef>> Textures { get; set; } = new();
        [MetaProperty(3851804024, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m3851804024 { get; set; } = new();
    }
    [MetaClass("ContextualConditionSpellSlot")]
    public class ContextualConditionSpellSlot : IContextualConditionSpell
    {
        [MetaProperty("mSpellSlot", BinPropertyType.UInt32)]
        public uint SpellSlot { get; set; } = 0;
    }
    [MetaClass("ContextualConditionKillCount")]
    public class ContextualConditionKillCount : IContextualCondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 0;
        [MetaProperty("mTotalKills", BinPropertyType.UInt16)]
        public ushort TotalKills { get; set; } = 0;
    }
    [MetaClass("TerrainType")]
    public class TerrainType : TargetingTypeData
    {
        [MetaProperty("mWallCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorData> WallCursor { get; set; } = new (new ());
        [MetaProperty("mBrushCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorData> BrushCursor { get; set; } = new (new ());
        [MetaProperty("mRiverCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorData> RiverCursor { get; set; } = new (new ());
    }
    [MetaClass("OverridePerkSelectionSet")]
    public class OverridePerkSelectionSet : IMetaClass
    {
        [MetaProperty("mPerks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Perks { get; set; } = new();
        [MetaProperty("mStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink Style { get; set; } = new(0);
        [MetaProperty("mSubStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink SubStyle { get; set; } = new(0);
    }
    [MetaClass("ValueProcessorData")]
    public class ValueProcessorData : IMetaClass
    {
    }
    [MetaClass(894011560)]
    public class Class0x354988a8 : IMetaClass
    {
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("IScriptSequence")]
    public interface IScriptSequence : RScript
    {
    }
    [MetaClass("EnchantmentGroup")]
    public class EnchantmentGroup : IMetaClass
    {
        [MetaProperty("mEnchantments", BinPropertyType.Container)]
        public MetaContainer<int> Enchantments { get; set; } = new();
        [MetaProperty("mBaseItems", BinPropertyType.Container)]
        public MetaContainer<int> BaseItems { get; set; } = new();
        [MetaProperty("mItemIdRangeMaximum", BinPropertyType.Int32)]
        public int ItemIdRangeMaximum { get; set; } = 0;
        [MetaProperty("mCanSidegrade", BinPropertyType.Bool)]
        public bool CanSidegrade { get; set; } = false;
        [MetaProperty("mItemIdRangeMinimum", BinPropertyType.Int32)]
        public int ItemIdRangeMinimum { get; set; } = 0;
    }
    [MetaClass("TooltipInstanceItem")]
    public class TooltipInstanceItem : TooltipInstance
    {
    }
    [MetaClass(906277859)]
    public class Class0x3604b3e3 : IMetaClass
    {
        [MetaProperty(2044394071, BinPropertyType.UInt32)]
        public uint m2044394071 { get; set; } = 0;
        [MetaProperty(183068798, BinPropertyType.Float)]
        public float m183068798 { get; set; } = 0f;
    }
    [MetaClass("MinMaterialDriver")]
    public class MinMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialDriver> Drivers { get; set; } = new();
    }
    [MetaClass("AudioSystemDataProperties")]
    public class AudioSystemDataProperties : IMetaClass
    {
        [MetaProperty("systemTagEventList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AudioTagListProperties>> SystemTagEventList { get; set; } = new();
    }
    [MetaClass("VoiceChatViewController")]
    public class VoiceChatViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("PlayerSlotData", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x86504cef> PlayerSlotData { get; set; } = new (new ());
        [MetaProperty("PlayerGrid", BinPropertyType.Hash)]
        public MetaHash PlayerGrid { get; set; } = new(0);
        [MetaProperty("errorText", BinPropertyType.Hash)]
        public MetaHash ErrorText { get; set; } = new(0);
        [MetaProperty(2022204093, BinPropertyType.Hash)]
        public MetaHash m2022204093 { get; set; } = new(0);
        [MetaProperty(2353392454, BinPropertyType.Hash)]
        public MetaHash m2353392454 { get; set; } = new(0);
        [MetaProperty("Backdrop", BinPropertyType.Hash)]
        public MetaHash Backdrop { get; set; } = new(0);
        [MetaProperty("SelfSlot", BinPropertyType.Embedded)]
        public MetaEmbedded<VoiceChatViewSelfSlot> SelfSlot { get; set; } = new (new ());
        [MetaProperty("PanelSceneHandle", BinPropertyType.Hash)]
        public MetaHash PanelSceneHandle { get; set; } = new(0);
    }
    [MetaClass("VFXDefaultSpawnConditionData")]
    public class VFXDefaultSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; } = new();
    }
    [MetaClass("GoldSourceFilter")]
    public class GoldSourceFilter : IStatStoneLogicDriver
    {
        [MetaProperty("ValidGoldSource", BinPropertyType.Byte)]
        public byte ValidGoldSource { get; set; } = 0;
    }
    [MetaClass("TFTHudUnitShopData")]
    public class TFTHudUnitShopData : IMetaClass
    {
        [MetaProperty(2288019587, BinPropertyType.Float)]
        public float m2288019587 { get; set; } = 1.5f;
        [MetaProperty(3280759721, BinPropertyType.Float)]
        public float m3280759721 { get; set; } = 3f;
    }
    [MetaClass("X3DSharedTextureDef")]
    public class X3DSharedTextureDef : IMetaClass
    {
        [MetaProperty("register", BinPropertyType.Int32)]
        public int Register { get; set; } = -1;
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("PlatformMask", BinPropertyType.UInt32)]
        public uint PlatformMask { get; set; } = 0;
    }
    [MetaClass("HudVoiceChatData")]
    public class HudVoiceChatData : IMetaClass
    {
        [MetaProperty("highlightTimeoutSeconds", BinPropertyType.Float)]
        public float HighlightTimeoutSeconds { get; set; } = 20f;
    }
    [MetaClass("ChangeHeightSolver")]
    public class ChangeHeightSolver : MissileTriggeredActionSpec
    {
        [MetaProperty("mOverrideHeightSolver", BinPropertyType.Structure)]
        public HeightSolverType OverrideHeightSolver { get; set; } = null;
    }
    [MetaClass("HudHealthBarDefenseIconData")]
    public class HudHealthBarDefenseIconData : IMetaClass
    {
        [MetaProperty("enlargeSize", BinPropertyType.Float)]
        public float EnlargeSize { get; set; } = 1.2999999523162842f;
        [MetaProperty("settleTime", BinPropertyType.Float)]
        public float SettleTime { get; set; } = 0.10000000149011612f;
        [MetaProperty("enlargeTime", BinPropertyType.Float)]
        public float EnlargeTime { get; set; } = 0.10000000149011612f;
    }
    [MetaClass(946411408)]
    public class Class0x38691790 : IMetaClass
    {
        [MetaProperty(2206191507, BinPropertyType.Container)]
        public MetaContainer<byte> m2206191507 { get; set; } = new();
        [MetaProperty("groupName", BinPropertyType.String)]
        public string GroupName { get; set; } = "";
    }
    [MetaClass("GetSizeOfCustomTableBlock")]
    public class GetSizeOfCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("OutSize", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutSize { get; set; } = new (new ());
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("AddExperienceCheat")]
    public class AddExperienceCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
        [MetaProperty("mGiveMaxLevel", BinPropertyType.Bool)]
        public bool GiveMaxLevel { get; set; } = false;
    }
    [MetaClass("MapNavigationGridOverlay")]
    public class MapNavigationGridOverlay : IMetaClass
    {
        [MetaProperty("navGridFileName", BinPropertyType.String)]
        public string NavGridFileName { get; set; } = "";
        [MetaProperty("regionsFilename", BinPropertyType.String)]
        public string RegionsFilename { get; set; } = "";
    }
    [MetaClass("LiveFeatureToggles")]
    public class LiveFeatureToggles : IMetaClass
    {
        [MetaProperty("mLoLToggles", BinPropertyType.Embedded)]
        public MetaEmbedded<LoLFeatureToggles> LoLToggles { get; set; } = new (new ());
        [MetaProperty("mGameplayToggles", BinPropertyType.Embedded)]
        public MetaEmbedded<GameplayFeatureToggles> GameplayToggles { get; set; } = new (new ());
        [MetaProperty("mEngineToggles", BinPropertyType.Embedded)]
        public MetaEmbedded<EngineFeatureToggles> EngineToggles { get; set; } = new (new ());
    }
    [MetaClass("MapSkinColorizationPostEffect")]
    public class MapSkinColorizationPostEffect : IMetaClass
    {
        [MetaProperty("mMultipliersRGB", BinPropertyType.Vector3)]
        public Vector3 MultipliersRGB { get; set; } = new Vector3(1f, 1f, 1f);
        [MetaProperty("mMultipliersSaturation", BinPropertyType.Float)]
        public float MultipliersSaturation { get; set; } = 1f;
    }
    [MetaClass("MinimapIconColorData")]
    public class MinimapIconColorData : IMetaClass
    {
        [MetaProperty("mBase", BinPropertyType.Color)]
        public Color Base { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mColorblind", BinPropertyType.Optional)]
        public MetaOptional<Color> Colorblind { get; set; } = new MetaOptional<Color>(default(Color), false);
    }
    [MetaClass("NeutralTimerData")]
    public class NeutralTimerData : IMetaClass
    {
        [MetaProperty("mTooltipCampName", BinPropertyType.String)]
        public string TooltipCampName { get; set; } = "";
        [MetaProperty("mSourceIcons", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<NeutralTimerSourceIconData>> SourceIcons { get; set; } = new();
        [MetaProperty("mTimerKeyName", BinPropertyType.String)]
        public string TimerKeyName { get; set; } = "";
        [MetaProperty("mTooltipChatNameChaos", BinPropertyType.String)]
        public string TooltipChatNameChaos { get; set; } = "";
        [MetaProperty("mTooltipRespawn", BinPropertyType.String)]
        public string TooltipRespawn { get; set; } = "";
        [MetaProperty("mTooltipChatNameOrder", BinPropertyType.String)]
        public string TooltipChatNameOrder { get; set; } = "";
        [MetaProperty("mTooltip", BinPropertyType.String)]
        public string Tooltip { get; set; } = "";
    }
    [MetaClass("CircleMovement")]
    public class CircleMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mAngularVelocity", BinPropertyType.Float)]
        public float AngularVelocity { get; set; } = 0f;
        [MetaProperty("mLinearVelocity", BinPropertyType.Float)]
        public float LinearVelocity { get; set; } = 0f;
        [MetaProperty("mRadialVelocity", BinPropertyType.Float)]
        public float RadialVelocity { get; set; } = 0f;
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float Lifetime { get; set; } = 0f;
    }
    [MetaClass(968608392)]
    public class Class0x39bbca88 : Class0x75259ad3
    {
        [MetaProperty("ActionButtonDefinition", BinPropertyType.Hash)]
        public MetaHash ActionButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("EncounterUITunables")]
    public class EncounterUITunables : IMetaClass
    {
        [MetaProperty("mProgressMeterSuffix", BinPropertyType.String)]
        public string ProgressMeterSuffix { get; set; } = "";
        [MetaProperty(1070132460, BinPropertyType.Bool)]
        public bool m1070132460 { get; set; } = false;
        [MetaProperty("mProgressBarEaseRate", BinPropertyType.Float)]
        public float ProgressBarEaseRate { get; set; } = 0f;
        [MetaProperty("mProgressMeterPingText", BinPropertyType.String)]
        public string ProgressMeterPingText { get; set; } = "";
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; } = new (new ());
        [MetaProperty("mUnitBarFadeSpeed", BinPropertyType.Float)]
        public float UnitBarFadeSpeed { get; set; } = 0f;
        [MetaProperty("mTimerMeterSuffix", BinPropertyType.String)]
        public string TimerMeterSuffix { get; set; } = "";
        [MetaProperty("mProgressMeterHoverText", BinPropertyType.String)]
        public string ProgressMeterHoverText { get; set; } = "";
        [MetaProperty("mPipsHoverText", BinPropertyType.String)]
        public string PipsHoverText { get; set; } = "";
    }
    [MetaClass("HeightSolverType")]
    public interface HeightSolverType : IMetaClass
    {
    }
    [MetaClass("FixedDurationTriggeredBoolDriver")]
    public class FixedDurationTriggeredBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mCustomDuration", BinPropertyType.Float)]
        public float CustomDuration { get; set; } = 0f;
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; } = null;
    }
    [MetaClass("ItemGroup")]
    public class ItemGroup : IMetaClass
    {
        [MetaProperty("mPurchaseCooldown", BinPropertyType.Float)]
        public float PurchaseCooldown { get; set; } = 0f;
        [MetaProperty("mItemGroupID", BinPropertyType.Hash)]
        public MetaHash ItemGroupID { get; set; } = new(0);
        [MetaProperty("mCooldownExtendedByAmbientGoldStart", BinPropertyType.Bool)]
        public bool CooldownExtendedByAmbientGoldStart { get; set; } = false;
        [MetaProperty("mMaxGroupOwnable", BinPropertyType.Int32)]
        public int MaxGroupOwnable { get; set; } = -1;
        [MetaProperty("mItemModifiers", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemModifiers { get; set; } = new();
        [MetaProperty("mInventorySlotMin", BinPropertyType.Int32)]
        public int InventorySlotMin { get; set; } = 0;
        [MetaProperty("mInventorySlotMax", BinPropertyType.Int32)]
        public int InventorySlotMax { get; set; } = 5;
    }
    [MetaClass("AbilityResourceTypeConfig")]
    public class AbilityResourceTypeConfig : IMetaClass
    {
        [MetaProperty("Wind", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Wind { get; set; } = new (new ());
        [MetaProperty("Heat", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Heat { get; set; } = new (new ());
        [MetaProperty("Energy", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Energy { get; set; } = new (new ());
        [MetaProperty("Shield", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Shield { get; set; } = new (new ());
        [MetaProperty("BattleFury", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> BattleFury { get; set; } = new (new ());
        [MetaProperty("Ammo", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Ammo { get; set; } = new (new ());
        [MetaProperty("Rage", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Rage { get; set; } = new (new ());
        [MetaProperty("Ferocity", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Ferocity { get; set; } = new (new ());
        [MetaProperty("DragonFury", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> DragonFury { get; set; } = new (new ());
        [MetaProperty("PrimalFury", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> PrimalFury { get; set; } = new (new ());
        [MetaProperty("None", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> None { get; set; } = new (new ());
        [MetaProperty("Moonlight", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Moonlight { get; set; } = new (new ());
        [MetaProperty("Other", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Other { get; set; } = new (new ());
        [MetaProperty("Bloodwell", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Bloodwell { get; set; } = new (new ());
        [MetaProperty("mana", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Mana { get; set; } = new (new ());
    }
    [MetaClass("TftSurrenderCheat")]
    public class TftSurrenderCheat : Cheat
    {
    }
    [MetaClass("IconElementData")]
    public class IconElementData : BaseElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("mColor", BinPropertyType.Color)]
        public Color Color { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("mUseAlpha", BinPropertyType.Bool)]
        public bool UseAlpha { get; set; } = true;
        [MetaProperty("mMaterial", BinPropertyType.ObjectLink)]
        public MetaObjectLink Material { get; set; } = new(0);
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasDataBase Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
        [MetaProperty("mExtension", BinPropertyType.Structure)]
        public IconElementDataExtension Extension { get; set; } = null;
    }
    [MetaClass("IDynamicMaterialDriver")]
    public interface IDynamicMaterialDriver : IMetaClass
    {
    }
    [MetaClass("IScriptBlock")]
    public interface IScriptBlock : IMetaClass
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        bool IsDisabled { get; set; }
    }
    [MetaClass("ContextualConditionGameTimer")]
    public class ContextualConditionGameTimer : IContextualCondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 2;
        [MetaProperty("mGameTimeInMinutes", BinPropertyType.Float)]
        public float GameTimeInMinutes { get; set; } = 0f;
    }
    [MetaClass("LootTableDialogViewController")]
    public class LootTableDialogViewController : Class0x75259ad3
    {
    }
    [MetaClass("MapBehavior")]
    public class MapBehavior : GenericMapPlaceable
    {
        [MetaProperty("Actions", BinPropertyType.Container)]
        public MetaContainer<MapAction> Actions { get; set; } = new();
        [MetaProperty("Cue", BinPropertyType.String)]
        public string Cue { get; set; } = "";
    }
    [MetaClass("FxActionMoveBase")]
    public interface FxActionMoveBase : IFxAction
    {
        [MetaProperty("OvershootDistance", BinPropertyType.Float)]
        float OvershootDistance { get; set; }
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        byte EasingType { get; set; }
        [MetaProperty("FaceVelocity", BinPropertyType.Bool)]
        bool FaceVelocity { get; set; }
        [MetaProperty("TargetObject", BinPropertyType.Embedded)]
        MetaEmbedded<FxTarget> TargetObject { get; set; }
    }
    [MetaClass("TriggerOnStart")]
    public class TriggerOnStart : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; } = new();
    }
    [MetaClass("PerkBuff")]
    public class PerkBuff : IMetaClass
    {
        [MetaProperty("mBuffScriptName", BinPropertyType.String)]
        public string BuffScriptName { get; set; } = "";
        [MetaProperty("mBuffSpellObject", BinPropertyType.Embedded)]
        public MetaEmbedded<SpellObject> BuffSpellObject { get; set; } = new (new ());
    }
    [MetaClass("TimeBlendData")]
    public class TimeBlendData : BaseBlendData
    {
        [MetaProperty("mTime", BinPropertyType.Float)]
        public float Time { get; set; } = 0.20000000298023224f;
    }
    [MetaClass("HudHealthBarFadeData")]
    public class HudHealthBarFadeData : IMetaClass
    {
        [MetaProperty("fadeAcceleration", BinPropertyType.Float)]
        public float FadeAcceleration { get; set; } = 0f;
        [MetaProperty("fadeHoldTime", BinPropertyType.Float)]
        public float FadeHoldTime { get; set; } = 0f;
        [MetaProperty("fadeSpeed", BinPropertyType.Float)]
        public float FadeSpeed { get; set; } = 0.5f;
    }
    [MetaClass("MissileGroupSpawnerSpec")]
    public class MissileGroupSpawnerSpec : IMetaClass
    {
        [MetaProperty("mChildMissileSpell", BinPropertyType.ObjectLink)]
        public MetaObjectLink ChildMissileSpell { get; set; } = new(0);
    }
    [MetaClass(1046078154)]
    public class Class0x3e59e2ca : IOptionItemFilter
    {
        [MetaProperty(932731737, BinPropertyType.Bool)]
        public bool m932731737 { get; set; } = false;
        [MetaProperty(2073624541, BinPropertyType.Bool)]
        public bool m2073624541 { get; set; } = false;
        [MetaProperty(2166835362, BinPropertyType.Bool)]
        public bool m2166835362 { get; set; } = false;
    }
    [MetaClass("SetRespawnTimerCheat")]
    public class SetRespawnTimerCheat : Cheat
    {
        [MetaProperty("mTimerValue", BinPropertyType.Float)]
        public float TimerValue { get; set; } = 0f;
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass("FloatingTextOverride")]
    public class FloatingTextOverride : IMetaClass
    {
        [MetaProperty("OverriddenFloatingTextTypes", BinPropertyType.Map)]
        public Dictionary<uint, bool> OverriddenFloatingTextTypes { get; set; } = new();
    }
    [MetaClass("MapGraphicsFeature")]
    public interface MapGraphicsFeature : MapComponent
    {
    }
    [MetaClass("IndicatorTypeLocal")]
    public class IndicatorTypeLocal : ILineIndicatorType
    {
    }
    [MetaClass("IContextualAction")]
    public interface IContextualAction : IMetaClass
    {
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        uint MaxOccurences { get; set; }
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        MetaHash HashedSituationTrigger { get; set; }
    }
    [MetaClass("GameModeConstantInteger")]
    public class GameModeConstantInteger : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Int32)]
        public int Value { get; set; } = 0;
    }
    [MetaClass("MaterialInstanceDynamicTexture")]
    public class MaterialInstanceDynamicTexture : IMetaClass
    {
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("options", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialTextureSwapOption>> Options { get; set; } = new();
    }
    [MetaClass("SyncedAnimationRigPoseModifierData")]
    public class SyncedAnimationRigPoseModifierData : BaseRigPoseModifierData
    {
    }
    [MetaClass("ContextualConditionItemID")]
    public class ContextualConditionItemID : IContextualCondition
    {
        [MetaProperty("mItems", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Items { get; set; } = new();
    }
    [MetaClass("IDynamicMaterialFloatDriver")]
    public interface IDynamicMaterialFloatDriver : IDynamicMaterialDriver
    {
    }
    [MetaClass("VfxReflectionDefinitionData")]
    public class VfxReflectionDefinitionData : IMetaClass
    {
        [MetaProperty("reflectionOpacityDirect", BinPropertyType.Float)]
        public float ReflectionOpacityDirect { get; set; } = 0f;
        [MetaProperty("reflectionFresnel", BinPropertyType.Float)]
        public float ReflectionFresnel { get; set; } = 1f;
        [MetaProperty("reflectionOpacityGlancing", BinPropertyType.Float)]
        public float ReflectionOpacityGlancing { get; set; } = 1f;
        [MetaProperty("fresnelColor", BinPropertyType.Vector4)]
        public Vector4 FresnelColor { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("reflectionFresnelColor", BinPropertyType.Vector4)]
        public Vector4 ReflectionFresnelColor { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        [MetaProperty("reflectionMapTexture", BinPropertyType.String)]
        public string ReflectionMapTexture { get; set; } = "";
        [MetaProperty("fresnel", BinPropertyType.Float)]
        public float Fresnel { get; set; } = 1f;
    }
    [MetaClass("FlexValueVector2")]
    public class FlexValueVector2 : IMetaClass
    {
        [MetaProperty("mValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> Value { get; set; } = new (new ());
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint FlexID { get; set; } = 0;
    }
    [MetaClass(68729178)]
    public class Class0x418b95a : IContextualConditionBuff
    {
        [MetaProperty(287338010, BinPropertyType.Byte)]
        public byte m287338010 { get; set; } = 0;
        [MetaProperty("mBuff", BinPropertyType.Hash)]
        public MetaHash Buff { get; set; } = new(0);
    }
    [MetaClass("ItemCareyOverrideStartingItemSetSet")]
    public class ItemCareyOverrideStartingItemSetSet : IMetaClass
    {
        [MetaProperty(1822917069, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemCareyOverrideStartingItemSet>> m1822917069 { get; set; } = new();
    }
    [MetaClass("ContextualConditionNegation")]
    public class ContextualConditionNegation : IContextualCondition
    {
        [MetaProperty("mChildCondition", BinPropertyType.Structure)]
        public IContextualCondition ChildCondition { get; set; } = null;
    }
    [MetaClass("HudLoadingScreenWidgetClash")]
    public class HudLoadingScreenWidgetClash : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string SceneName { get; set; } = "";
    }
    [MetaClass("FlexValueVector3")]
    public class FlexValueVector3 : IMetaClass
    {
        [MetaProperty("mValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Value { get; set; } = new (new ());
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint FlexID { get; set; } = 0;
    }
    [MetaClass("SkinAnimationProperties")]
    public class SkinAnimationProperties : IMetaClass
    {
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnimationGraphData { get; set; } = new(0);
    }
    [MetaClass("LoadScreenTip")]
    public class LoadScreenTip : IMetaClass
    {
        [MetaProperty("mLocalizationKey", BinPropertyType.String)]
        public string LocalizationKey { get; set; } = "";
        [MetaProperty("mMaximumSummonerLevel", BinPropertyType.Optional)]
        public MetaOptional<uint> MaximumSummonerLevel { get; set; } = new MetaOptional<uint>(default(uint), false);
        [MetaProperty("mHeaderLocalizationKey", BinPropertyType.String)]
        public string HeaderLocalizationKey { get; set; } = "";
        [MetaProperty("mId", BinPropertyType.UInt16)]
        public ushort Id { get; set; } = 0;
        [MetaProperty("mMinimumSummonerLevel", BinPropertyType.Optional)]
        public MetaOptional<uint> MinimumSummonerLevel { get; set; } = new MetaOptional<uint>(default(uint), false);
    }
    [MetaClass("AudioTagListProperties")]
    public class AudioTagListProperties : IMetaClass
    {
        [MetaProperty("Key", BinPropertyType.String)]
        public string Key { get; set; } = "";
        [MetaProperty("tags", BinPropertyType.Container)]
        public MetaContainer<string> Tags { get; set; } = new();
    }
    [MetaClass("ValueFloat")]
    public class ValueFloat : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Float)]
        public float ConstantValue { get; set; } = 0f;
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedFloatVariableData Dynamics { get; set; } = null;
    }
    [MetaClass(1124978957)]
    public class Class0x430dd10d : IMetaClass
    {
        [MetaProperty("EventName", BinPropertyType.String)]
        public string EventName { get; set; } = "";
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
    }
    [MetaClass("SpellDataResource")]
    public class SpellDataResource : IMetaClass
    {
        [MetaProperty("mDoesntBreakChannels", BinPropertyType.Bool)]
        public bool DoesntBreakChannels { get; set; } = false;
        [MetaProperty("mCursorChangesInTerrain", BinPropertyType.Bool)]
        public bool CursorChangesInTerrain { get; set; } = false;
        [MetaProperty(372438780, BinPropertyType.Container)]
        public MetaContainer<SpellPassiveData> m372438780 { get; set; } = new();
        [MetaProperty("missileSpeed", BinPropertyType.Float)]
        public float MissileSpeed { get; set; } = 500f;
        [MetaProperty("mCastRangeGrowthDuration", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeGrowthDuration { get; set; } = new();
        [MetaProperty("mBelongsToAvatar", BinPropertyType.Bool)]
        public bool BelongsToAvatar { get; set; } = false;
        [MetaProperty("delayCastOffsetPercent", BinPropertyType.Float)]
        public float DelayCastOffsetPercent { get; set; } = 0f;
        [MetaProperty("mChannelDuration", BinPropertyType.Container)]
        public MetaContainer<float> ChannelDuration { get; set; } = new();
        [MetaProperty("mLineWidth", BinPropertyType.Float)]
        public float LineWidth { get; set; } = 0f;
        [MetaProperty("mFloatVarsDecimals", BinPropertyType.Container)]
        public MetaContainer<int> FloatVarsDecimals { get; set; } = new();
        [MetaProperty("castRange", BinPropertyType.Container)]
        public MetaContainer<float> CastRange { get; set; } = new();
        [MetaProperty("canCastOrQueueWhileCasting", BinPropertyType.Bool)]
        public bool CanCastOrQueueWhileCasting { get; set; } = false;
        [MetaProperty("mDataValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellDataValue>> DataValues { get; set; } = new();
        [MetaProperty("mIsDelayedByCastLocked", BinPropertyType.Bool)]
        public bool IsDelayedByCastLocked { get; set; } = false;
        [MetaProperty("mHitEffectOrientType", BinPropertyType.UInt32)]
        public uint HitEffectOrientType { get; set; } = 1;
        [MetaProperty("mMissileEffectEnemyName", BinPropertyType.String)]
        public string MissileEffectEnemyName { get; set; } = "";
        [MetaProperty(615998402, BinPropertyType.Bool)]
        public bool m615998402 { get; set; } = false;
        [MetaProperty("mEffectAmount", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellEffectAmount>> EffectAmount { get; set; } = new();
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool ProjectTargetToCastRange { get; set; } = false;
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float Coefficient { get; set; } = 0f;
        [MetaProperty("mCastRequirementsCaster", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> CastRequirementsCaster { get; set; } = new();
        [MetaProperty(959977248, BinPropertyType.Bool)]
        public bool m959977248 { get; set; } = false;
        [MetaProperty("delayTotalTimePercent", BinPropertyType.Float)]
        public float DelayTotalTimePercent { get; set; } = 0f;
        [MetaProperty("mIgnoreRangeCheck", BinPropertyType.Bool)]
        public bool IgnoreRangeCheck { get; set; } = false;
        [MetaProperty("mCastType", BinPropertyType.UInt32)]
        public uint CastType { get; set; } = 0;
        [MetaProperty("mRequiredUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> RequiredUnitTags { get; set; } = new (new ());
        [MetaProperty(1031040799, BinPropertyType.Byte)]
        public byte m1031040799 { get; set; } = 0;
        [MetaProperty("mHitEffectPlayerKey", BinPropertyType.Hash)]
        public MetaHash HitEffectPlayerKey { get; set; } = new(0);
        [MetaProperty("mSpellTags", BinPropertyType.Container)]
        public MetaContainer<string> SpellTags { get; set; } = new();
        [MetaProperty("mMinimapIconDisplayFlag", BinPropertyType.UInt16)]
        public ushort MinimapIconDisplayFlag { get; set; } = 1;
        [MetaProperty(66335398, BinPropertyType.Byte)]
        public byte m66335398 { get; set; } = 0;
        [MetaProperty("mDoNotNeedToFaceTarget", BinPropertyType.Bool)]
        public bool DoNotNeedToFaceTarget { get; set; } = false;
        [MetaProperty("mCanMoveWhileChanneling", BinPropertyType.Bool)]
        public bool CanMoveWhileChanneling { get; set; } = false;
        [MetaProperty("canOnlyCastWhileDisabled", BinPropertyType.Bool)]
        public bool CanOnlyCastWhileDisabled { get; set; } = false;
        [MetaProperty("mAlternateName", BinPropertyType.String)]
        public string AlternateName { get; set; } = "";
        [MetaProperty("mClientData", BinPropertyType.Embedded)]
        public MetaEmbedded<SpellDataResourceClient> ClientData { get; set; } = new (new ());
        [MetaProperty("castRangeUseBoundingBoxes", BinPropertyType.Bool)]
        public bool CastRangeUseBoundingBoxes { get; set; } = false;
        [MetaProperty("castFrame", BinPropertyType.Float)]
        public float CastFrame { get; set; } = 0f;
        [MetaProperty("castTargetAdditionalUnitsRadius", BinPropertyType.Float)]
        public float CastTargetAdditionalUnitsRadius { get; set; } = 0f;
        [MetaProperty("mMissileEffectPlayerKey", BinPropertyType.Hash)]
        public MetaHash MissileEffectPlayerKey { get; set; } = new(0);
        [MetaProperty("mTurnSpeedScalar", BinPropertyType.Float)]
        public float TurnSpeedScalar { get; set; } = 1f;
        [MetaProperty("mSpellRevealsChampion", BinPropertyType.Bool)]
        public bool SpellRevealsChampion { get; set; } = true;
        [MetaProperty("mCantCancelWhileChanneling", BinPropertyType.Bool)]
        public bool CantCancelWhileChanneling { get; set; } = false;
        [MetaProperty("mMaxAmmo", BinPropertyType.Container)]
        public MetaContainer<int> MaxAmmo { get; set; } = new();
        [MetaProperty("mPostCastLockoutDeltaTimeData", BinPropertyType.Structure)]
        public SpellLockDeltaTimeData PostCastLockoutDeltaTimeData { get; set; } = null;
        [MetaProperty("mCancelChargeOnRecastTime", BinPropertyType.Float)]
        public float CancelChargeOnRecastTime { get; set; } = 0f;
        [MetaProperty("castConeDistance", BinPropertyType.Float)]
        public float CastConeDistance { get; set; } = 400f;
        [MetaProperty("mTargetingTypeData", BinPropertyType.Structure)]
        public TargetingTypeData TargetingTypeData { get; set; } = null;
        [MetaProperty("mChannelIsInterruptedByDisables", BinPropertyType.Bool)]
        public bool ChannelIsInterruptedByDisables { get; set; } = true;
        [MetaProperty("mHideRangeIndicatorWhenCasting", BinPropertyType.Bool)]
        public bool HideRangeIndicatorWhenCasting { get; set; } = false;
        [MetaProperty("mIsDisabledWhileDead", BinPropertyType.Bool)]
        public bool IsDisabledWhileDead { get; set; } = true;
        [MetaProperty("mParticleStartOffset", BinPropertyType.Vector3)]
        public Vector3 ParticleStartOffset { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("mPreCastLockoutDeltaTimeData", BinPropertyType.Structure)]
        public SpellLockDeltaTimeData PreCastLockoutDeltaTimeData { get; set; } = null;
        [MetaProperty("mDisableCastBar", BinPropertyType.Bool)]
        public bool DisableCastBar { get; set; } = false;
        [MetaProperty("mHitBoneName", BinPropertyType.String)]
        public string HitBoneName { get; set; } = "";
        [MetaProperty("castConeAngle", BinPropertyType.Float)]
        public float CastConeAngle { get; set; } = 45f;
        [MetaProperty("selectionPriority", BinPropertyType.UInt32)]
        public uint SelectionPriority { get; set; } = 3;
        [MetaProperty("mStartCooldown", BinPropertyType.Float)]
        public float StartCooldown { get; set; } = 0f;
        [MetaProperty(2057207177, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x7a9e7d89>> m2057207177 { get; set; } = new();
        [MetaProperty("mHitEffectKey", BinPropertyType.Hash)]
        public MetaHash HitEffectKey { get; set; } = new(0);
        [MetaProperty("mSpellCooldownOrSealedQueueThreshold", BinPropertyType.Optional)]
        public MetaOptional<float> SpellCooldownOrSealedQueueThreshold { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mVOEventCategory", BinPropertyType.String)]
        public string VOEventCategory { get; set; } = "";
        [MetaProperty("mMissileEffectPlayerName", BinPropertyType.String)]
        public string MissileEffectPlayerName { get; set; } = "";
        [MetaProperty("mMinimapIconName", BinPropertyType.String)]
        public string MinimapIconName { get; set; } = "";
        [MetaProperty(2117350048, BinPropertyType.Bool)]
        public bool m2117350048 { get; set; } = false;
        [MetaProperty("mAmmoCountHiddenInUI", BinPropertyType.Bool)]
        public bool AmmoCountHiddenInUI { get; set; } = false;
        [MetaProperty("cantCastWhileRooted", BinPropertyType.Bool)]
        public bool CantCastWhileRooted { get; set; } = false;
        [MetaProperty("luaOnMissileUpdateDistanceInterval", BinPropertyType.Float)]
        public float LuaOnMissileUpdateDistanceInterval { get; set; } = 0f;
        [MetaProperty(2307898068, BinPropertyType.Bool)]
        public bool m2307898068 { get; set; } = false;
        [MetaProperty("mDoesNotConsumeMana", BinPropertyType.Bool)]
        public bool DoesNotConsumeMana { get; set; } = false;
        [MetaProperty("mMissileEffectKey", BinPropertyType.Hash)]
        public MetaHash MissileEffectKey { get; set; } = new(0);
        [MetaProperty("mAmmoUsed", BinPropertyType.Container)]
        public MetaContainer<int> AmmoUsed { get; set; } = new();
        [MetaProperty("bIsToggleSpell", BinPropertyType.Bool)]
        public bool BIsToggleSpell { get; set; } = false;
        [MetaProperty("castRadiusSecondary", BinPropertyType.Container)]
        public MetaContainer<float> CastRadiusSecondary { get; set; } = new();
        [MetaProperty("bHaveHitBone", BinPropertyType.Bool)]
        public bool BHaveHitBone { get; set; } = false;
        [MetaProperty("mCastTime", BinPropertyType.Float)]
        public float CastTime { get; set; } = 0f;
        [MetaProperty("mAffectsStatusFlags", BinPropertyType.UInt32)]
        public uint AffectsStatusFlags { get; set; } = 0;
        [MetaProperty("mResourceResolvers", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ResourceResolvers { get; set; } = new();
        [MetaProperty("mExcludedUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> ExcludedUnitTags { get; set; } = new (new ());
        [MetaProperty("mSpellCalculations", BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> SpellCalculations { get; set; } = new();
        [MetaProperty("mLookAtPolicy", BinPropertyType.UInt32)]
        public uint LookAtPolicy { get; set; } = 2;
        [MetaProperty("mPlatformSpellInfo", BinPropertyType.Embedded)]
        public MetaEmbedded<PlatformSpellInfo> PlatformSpellInfo { get; set; } = new (new ());
        [MetaProperty("mAfterEffectName", BinPropertyType.String)]
        public string AfterEffectName { get; set; } = "";
        [MetaProperty("mUpdateRotationWhenCasting", BinPropertyType.Bool)]
        public bool UpdateRotationWhenCasting { get; set; } = false;
        [MetaProperty("canOnlyCastWhileDead", BinPropertyType.Bool)]
        public bool CanOnlyCastWhileDead { get; set; } = false;
        [MetaProperty("flags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mShowChannelBar", BinPropertyType.Bool)]
        public bool ShowChannelBar { get; set; } = true;
        [MetaProperty("cooldownTime", BinPropertyType.Container)]
        public MetaContainer<float> CooldownTime { get; set; } = new();
        [MetaProperty("mKeywordWhenAcquired", BinPropertyType.String)]
        public string KeywordWhenAcquired { get; set; } = "";
        [MetaProperty("mNoWinddownIfCancelled", BinPropertyType.Bool)]
        public bool NoWinddownIfCancelled { get; set; } = false;
        [MetaProperty(2679664068, BinPropertyType.Bool)]
        public bool m2679664068 { get; set; } = false;
        [MetaProperty("mAmmoNotAffectedByCDR", BinPropertyType.Bool)]
        public bool AmmoNotAffectedByCDR { get; set; } = false;
        [MetaProperty("mApplyMaterialOnHitSound", BinPropertyType.Bool)]
        public bool ApplyMaterialOnHitSound { get; set; } = false;
        [MetaProperty("mCastRangeGrowthStartTime", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeGrowthStartTime { get; set; } = new();
        [MetaProperty("mDoesNotConsumeCooldown", BinPropertyType.Bool)]
        public bool DoesNotConsumeCooldown { get; set; } = false;
        [MetaProperty("mChannelIsInterruptedByAttacking", BinPropertyType.Bool)]
        public bool ChannelIsInterruptedByAttacking { get; set; } = true;
        [MetaProperty("mPostCastLockoutDeltaTime", BinPropertyType.Float)]
        public float PostCastLockoutDeltaTime { get; set; } = 0f;
        [MetaProperty(2833975761, BinPropertyType.Bool)]
        public bool m2833975761 { get; set; } = false;
        [MetaProperty("mCooldownNotAffectedByCDR", BinPropertyType.Bool)]
        public bool CooldownNotAffectedByCDR { get; set; } = false;
        [MetaProperty("mApplyAttackDamage", BinPropertyType.Bool)]
        public bool ApplyAttackDamage { get; set; } = false;
        [MetaProperty("mUseChargeChanneling", BinPropertyType.Bool)]
        public bool UseChargeChanneling { get; set; } = false;
        [MetaProperty("mCanTriggerChargeSpellWhileDisabled", BinPropertyType.Bool)]
        public bool CanTriggerChargeSpellWhileDisabled { get; set; } = false;
        [MetaProperty("mApplyAttackEffect", BinPropertyType.Bool)]
        public bool ApplyAttackEffect { get; set; } = false;
        [MetaProperty("castRadius", BinPropertyType.Container)]
        public MetaContainer<float> CastRadius { get; set; } = new();
        [MetaProperty("mOrientRadiusTextureFromPlayer", BinPropertyType.Bool)]
        public bool OrientRadiusTextureFromPlayer { get; set; } = false;
        [MetaProperty("mMinimapIconRotation", BinPropertyType.Bool)]
        public bool MinimapIconRotation { get; set; } = false;
        [MetaProperty("mCostAlwaysShownInUI", BinPropertyType.Bool)]
        public bool CostAlwaysShownInUI { get; set; } = false;
        [MetaProperty("mCharacterPassiveBuffs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CharacterPassiveData>> CharacterPassiveBuffs { get; set; } = new();
        [MetaProperty("mCastRangeGrowthMax", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeGrowthMax { get; set; } = new();
        [MetaProperty("mCursorChangesInGrass", BinPropertyType.Bool)]
        public bool CursorChangesInGrass { get; set; } = false;
        [MetaProperty("mHitEffectName", BinPropertyType.String)]
        public string HitEffectName { get; set; } = "";
        [MetaProperty("mAnimationLeadOutName", BinPropertyType.String)]
        public string AnimationLeadOutName { get; set; } = "";
        [MetaProperty("mCastRequirementsTarget", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> CastRequirementsTarget { get; set; } = new();
        [MetaProperty("manaUiOverride", BinPropertyType.Container)]
        public MetaContainer<float> ManaUiOverride { get; set; } = new();
        [MetaProperty("mIgnoreAnimContinueUntilCastFrame", BinPropertyType.Bool)]
        public bool IgnoreAnimContinueUntilCastFrame { get; set; } = false;
        [MetaProperty("mUseMinimapTargeting", BinPropertyType.Bool)]
        public bool UseMinimapTargeting { get; set; } = false;
        [MetaProperty("mLineDragLength", BinPropertyType.Float)]
        public float LineDragLength { get; set; } = 0f;
        [MetaProperty("cannotBeSuppressed", BinPropertyType.Bool)]
        public bool CannotBeSuppressed { get; set; } = false;
        [MetaProperty("mAlternateSpellAssets", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AlternateSpellAssets>> AlternateSpellAssets { get; set; } = new();
        [MetaProperty("useAnimatorFramerate", BinPropertyType.Bool)]
        public bool UseAnimatorFramerate { get; set; } = false;
        [MetaProperty("castRangeDisplayOverride", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeDisplayOverride { get; set; } = new();
        [MetaProperty("mOverrideAttackTime", BinPropertyType.Structure)]
        public OverrideAttackTimeData OverrideAttackTime { get; set; } = null;
        [MetaProperty("mAfterEffectKey", BinPropertyType.Hash)]
        public MetaHash AfterEffectKey { get; set; } = new(0);
        [MetaProperty("mCoefficient2", BinPropertyType.Float)]
        public float Coefficient2 { get; set; } = 0f;
        [MetaProperty("mana", BinPropertyType.Container)]
        public MetaContainer<float> Mana { get; set; } = new();
        [MetaProperty("mAmmoRechargeTime", BinPropertyType.Container)]
        public MetaContainer<float> AmmoRechargeTime { get; set; } = new();
        [MetaProperty("canCastWhileDisabled", BinPropertyType.Bool)]
        public bool CanCastWhileDisabled { get; set; } = false;
        [MetaProperty("mAnimationLoopName", BinPropertyType.String)]
        public string AnimationLoopName { get; set; } = "";
        [MetaProperty("mMissileEffectEnemyKey", BinPropertyType.Hash)]
        public MetaHash MissileEffectEnemyKey { get; set; } = new(0);
        [MetaProperty("mConsideredAsAutoAttack", BinPropertyType.Bool)]
        public bool ConsideredAsAutoAttack { get; set; } = false;
        [MetaProperty("alwaysSnapFacing", BinPropertyType.Bool)]
        public bool AlwaysSnapFacing { get; set; } = false;
        [MetaProperty("mMissileSpec", BinPropertyType.Structure)]
        public MissileSpecification MissileSpec { get; set; } = null;
        [MetaProperty("mHitEffectPlayerName", BinPropertyType.String)]
        public string HitEffectPlayerName { get; set; } = "";
        [MetaProperty("mCantCancelWhileWindingUp", BinPropertyType.Bool)]
        public bool CantCancelWhileWindingUp { get; set; } = false;
        [MetaProperty("mAIData", BinPropertyType.Embedded)]
        public MetaEmbedded<AISpellData> AIData { get; set; } = new (new ());
        [MetaProperty("mAffectsTypeFlags", BinPropertyType.UInt32)]
        public uint AffectsTypeFlags { get; set; } = 0;
        [MetaProperty("mImgIconName", BinPropertyType.Container)]
        public MetaContainer<string> ImgIconName { get; set; } = new();
        [MetaProperty("mAnimationWinddownName", BinPropertyType.String)]
        public string AnimationWinddownName { get; set; } = "";
        [MetaProperty("mLockedSpellOriginationCastID", BinPropertyType.Bool)]
        public bool LockedSpellOriginationCastID { get; set; } = false;
        [MetaProperty("mPreCastLockoutDeltaTime", BinPropertyType.Float)]
        public float PreCastLockoutDeltaTime { get; set; } = 0f;
        [MetaProperty("mPingableWhileDisabled", BinPropertyType.Bool)]
        public bool PingableWhileDisabled { get; set; } = false;
        [MetaProperty("mMissileEffectName", BinPropertyType.String)]
        public string MissileEffectName { get; set; } = "";
        [MetaProperty("bHaveHitEffect", BinPropertyType.Bool)]
        public bool BHaveHitEffect { get; set; } = false;
        [MetaProperty("mUseAutoattackCastTimeData", BinPropertyType.Structure)]
        public UseAutoattackCastTimeData UseAutoattackCastTimeData { get; set; } = null;
        [MetaProperty("mChargeUpdateInterval", BinPropertyType.Float)]
        public float ChargeUpdateInterval { get; set; } = 0f;
        [MetaProperty(16246204, BinPropertyType.Bool)]
        public bool m16246204 { get; set; } = false;
        [MetaProperty(4216742028, BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<SpellDataValueVector>> m4216742028 { get; set; } = new();
        [MetaProperty("mAnimationName", BinPropertyType.String)]
        public string AnimationName { get; set; } = "Attack1";
    }
    [MetaClass("OptionItemDropdownItem")]
    public class OptionItemDropdownItem : IMetaClass
    {
        [MetaProperty("value", BinPropertyType.Int32)]
        public int Value { get; set; } = 0;
        [MetaProperty("TraKey", BinPropertyType.String)]
        public string TraKey { get; set; } = "";
    }
    [MetaClass("VfxAnimatedColorVariableData")]
    public class VfxAnimatedColorVariableData : IMetaClass
    {
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<Vector4> Values { get; set; } = new();
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; } = new();
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; } = new();
    }
    [MetaClass(1132045746)]
    public class Class0x4379a5b2 : IMetaClass
    {
        [MetaProperty(1480434725, BinPropertyType.Container)]
        public MetaContainer<IGameCalculationPart> m1480434725 { get; set; } = new();
        [MetaProperty(1553119218, BinPropertyType.Byte)]
        public byte m1553119218 { get; set; } = 31;
        [MetaProperty(2896542132, BinPropertyType.Byte)]
        public byte m2896542132 { get; set; } = 0;
    }
    [MetaClass("TFTSetData")]
    public class TFTSetData : IMetaClass
    {
        [MetaProperty("number", BinPropertyType.UInt32)]
        public uint Number { get; set; } = 0;
        [MetaProperty("DropRateTables", BinPropertyType.Map)]
        public Dictionary<string, MetaObjectLink> DropRateTables { get; set; } = new();
        [MetaProperty("stages", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTStageData>> Stages { get; set; } = new();
        [MetaProperty("itemLists", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemLists { get; set; } = new();
        [MetaProperty("UnitUpgrades", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> UnitUpgrades { get; set; } = new();
        [MetaProperty("DebugCharacterLists", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> DebugCharacterLists { get; set; } = new();
        [MetaProperty("characterLists", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> CharacterLists { get; set; } = new();
        [MetaProperty("traits", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Traits { get; set; } = new();
        [MetaProperty("ScriptDataObjectLists", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ScriptDataObjectLists { get; set; } = new();
        [MetaProperty("VfxResourceResolver", BinPropertyType.ObjectLink)]
        public MetaObjectLink VfxResourceResolver { get; set; } = new(0);
        [MetaProperty("TftGameType", BinPropertyType.UInt32)]
        public uint TftGameType { get; set; } = 0;
        [MetaProperty("traitList", BinPropertyType.ObjectLink)]
        public MetaObjectLink TraitList { get; set; } = new(0);
        [MetaProperty("Mutator", BinPropertyType.String)]
        public string Mutator { get; set; } = "";
        [MetaProperty("ScriptData", BinPropertyType.Map)]
        public Dictionary<string, GameModeConstant> ScriptData { get; set; } = new();
    }
    [MetaClass("LinearTransformProcessorData")]
    public class LinearTransformProcessorData : ValueProcessorData
    {
        [MetaProperty("mMultiplier", BinPropertyType.Float)]
        public float Multiplier { get; set; } = 0f;
        [MetaProperty("mIncrement", BinPropertyType.Float)]
        public float Increment { get; set; } = 0f;
    }
    [MetaClass("StoreViewController")]
    public class StoreViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("StoreCategoryButtonDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StoreCategoryButtonDefinition>> StoreCategoryButtonDefinitions { get; set; } = new();
        [MetaProperty(2662239426, BinPropertyType.Hash)]
        public MetaHash m2662239426 { get; set; } = new(0);
        [MetaProperty("MainViewPaneDefinition", BinPropertyType.Structure)]
        public ViewPaneDefinition MainViewPaneDefinition { get; set; } = null;
        [MetaProperty(3536587999, BinPropertyType.Hash)]
        public MetaHash m3536587999 { get; set; } = new(0);
    }
    [MetaClass("CharacterVarsTable")]
    public class CharacterVarsTable : ScriptTable
    {
    }
    [MetaClass("MapComponent")]
    public interface MapComponent : IMetaClass
    {
    }
    [MetaClass("BoolGet")]
    public class BoolGet : IBoolGet
    {
        [MetaProperty("value", BinPropertyType.Bool)]
        public bool Value { get; set; } = false;
    }
    [MetaClass("CheatSet")]
    public class CheatSet : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mAssociatedChampion", BinPropertyType.ObjectLink)]
        public MetaObjectLink AssociatedChampion { get; set; } = new(0);
        [MetaProperty("mIsPlayerFacing", BinPropertyType.Bool)]
        public bool IsPlayerFacing { get; set; } = false;
        [MetaProperty("mCheatPages", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CheatPage>> CheatPages { get; set; } = new();
        [MetaProperty("mUseIconsForButtons", BinPropertyType.Bool)]
        public bool UseIconsForButtons { get; set; } = false;
        [MetaProperty("mGameMutator", BinPropertyType.String)]
        public string GameMutator { get; set; } = "";
        [MetaProperty("mGameModeName", BinPropertyType.Hash)]
        public MetaHash GameModeName { get; set; } = new(0);
        [MetaProperty("mIsUIAlwaysShown", BinPropertyType.Bool)]
        public bool IsUIAlwaysShown { get; set; } = false;
    }
    [MetaClass("IStringGet")]
    public interface IStringGet : IScriptValueGet
    {
    }
    [MetaClass("VfxSystemDefinitionData")]
    public class VfxSystemDefinitionData : IResource
    {
        [MetaProperty("selfIllumination", BinPropertyType.Float)]
        public float SelfIllumination { get; set; } = -1f;
        [MetaProperty("assetCategory", BinPropertyType.String)]
        public string AssetCategory { get; set; } = "fx";
        [MetaProperty("materialOverrideDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxMaterialOverrideDefinitionData>> MaterialOverrideDefinitions { get; set; } = new();
        [MetaProperty("overrideScaleCap", BinPropertyType.Optional)]
        public MetaOptional<float> OverrideScaleCap { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("voiceOverOnCreateDefault", BinPropertyType.String)]
        public string VoiceOverOnCreateDefault { get; set; } = "";
        [MetaProperty("audioParameterFlexID", BinPropertyType.Int32)]
        public int AudioParameterFlexID { get; set; } = -1;
        [MetaProperty("soundPersistentDefault", BinPropertyType.String)]
        public string SoundPersistentDefault { get; set; } = "";
        [MetaProperty("systemTranslation", BinPropertyType.Vector3)]
        public Vector3 SystemTranslation { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("simpleEmitterDefinitionData", BinPropertyType.Container)]
        public MetaContainer<VfxEmitterDefinitionData> SimpleEmitterDefinitionData { get; set; } = new();
        [MetaProperty("mEyeCandy", BinPropertyType.Bool)]
        public bool EyeCandy { get; set; } = false;
        [MetaProperty("complexEmitterDefinitionData", BinPropertyType.Container)]
        public MetaContainer<VfxEmitterDefinitionData> ComplexEmitterDefinitionData { get; set; } = new();
        [MetaProperty("flags", BinPropertyType.Byte)]
        public byte Flags { get; set; } = 196;
        [MetaProperty("audioParameterTimeScaledDuration", BinPropertyType.Float)]
        public float AudioParameterTimeScaledDuration { get; set; } = 0f;
        [MetaProperty("HudLayerAspect", BinPropertyType.Float)]
        public float HudLayerAspect { get; set; } = 1.3333333730697632f;
        [MetaProperty("drawingLayer", BinPropertyType.Byte)]
        public byte DrawingLayer { get; set; } = 0;
        [MetaProperty(3473471718, BinPropertyType.Bool)]
        public bool m3473471718 { get; set; } = false;
        [MetaProperty("assetRemappingTable", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxAssetRemap>> AssetRemappingTable { get; set; } = new();
        [MetaProperty("hudAnchorPositionFromWorldProjection", BinPropertyType.Bool)]
        public bool HudAnchorPositionFromWorldProjection { get; set; } = true;
        [MetaProperty("voiceOverPersistentDefault", BinPropertyType.String)]
        public string VoiceOverPersistentDefault { get; set; } = "";
        [MetaProperty("soundOnCreateDefault", BinPropertyType.String)]
        public string SoundOnCreateDefault { get; set; } = "";
        [MetaProperty("systemScale", BinPropertyType.Vector3)]
        public Vector3 SystemScale { get; set; } = new Vector3(1f, 1f, 1f);
        [MetaProperty("particlePath", BinPropertyType.String)]
        public string ParticlePath { get; set; } = "";
        [MetaProperty("buildUpTime", BinPropertyType.Float)]
        public float BuildUpTime { get; set; } = 0f;
        [MetaProperty("particleName", BinPropertyType.String)]
        public string ParticleName { get; set; } = "";
        [MetaProperty("scaleDynamicallyWithAttachedBone", BinPropertyType.Bool)]
        public bool ScaleDynamicallyWithAttachedBone { get; set; } = false;
        [MetaProperty("hudLayerDimension", BinPropertyType.Float)]
        public float HudLayerDimension { get; set; } = 1024f;
        [MetaProperty("visibilityRadius", BinPropertyType.Float)]
        public float VisibilityRadius { get; set; } = 250f;
    }
    [MetaClass("MaterialInstanceDynamicSwitch")]
    public class MaterialInstanceDynamicSwitch : IMetaClass
    {
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Driver { get; set; } = null;
    }
    [MetaClass("SwitchMaterialDriverElement")]
    public class SwitchMaterialDriverElement : IMetaClass
    {
        [MetaProperty("mValue", BinPropertyType.Structure)]
        public IDynamicMaterialDriver Value { get; set; } = null;
        [MetaProperty("mCondition", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Condition { get; set; } = null;
    }
    [MetaClass("MapPrefabInstance")]
    public class MapPrefabInstance : MapPlaceable
    {
        [MetaProperty("prefabDefinition", BinPropertyType.ObjectLink)]
        public MetaObjectLink PrefabDefinition { get; set; } = new(0);
    }
    [MetaClass("TrophyData")]
    public class TrophyData : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        public float PerceptionBubbleRadius { get; set; } = 250f;
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; } = new (new ());
        [MetaProperty("mBracketTRAKey", BinPropertyType.String)]
        public string BracketTRAKey { get; set; } = "";
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; } = null;
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnimationGraphData { get; set; } = new(0);
    }
    [MetaClass("BuffData")]
    public class BuffData : IMetaClass
    {
        [MetaProperty("mFloatVarsDecimals", BinPropertyType.Container)]
        public MetaContainer<int> FloatVarsDecimals { get; set; } = new();
        [MetaProperty("mTooltipData", BinPropertyType.Structure)]
        public TooltipInstanceBuff TooltipData { get; set; } = null;
        [MetaProperty(1659011754, BinPropertyType.Bool)]
        public bool m1659011754 { get; set; } = true;
        [MetaProperty("mVfxSpawnConditions", BinPropertyType.Container)]
        public MetaContainer<VfxSpawnConditions> VfxSpawnConditions { get; set; } = new();
        [MetaProperty("mShowDuration", BinPropertyType.Bool)]
        public bool ShowDuration { get; set; } = true;
        [MetaProperty(13638081, BinPropertyType.Bool)]
        public bool m13638081 { get; set; } = true;
        [MetaProperty("mBuffAttributeFlag", BinPropertyType.Byte)]
        public byte BuffAttributeFlag { get; set; } = 0;
        [MetaProperty("mDescription", BinPropertyType.String)]
        public string Description { get; set; } = "";
    }
    [MetaClass(1179857030)]
    public class Class0x46533086 : IMetaClass
    {
        [MetaProperty("secondaryQuest", BinPropertyType.Embedded)]
        public MetaEmbedded<QuestDefinition> SecondaryQuest { get; set; } = new (new ());
        [MetaProperty(1449424944, BinPropertyType.Embedded)]
        public MetaEmbedded<QuestDefinition> m1449424944 { get; set; } = new (new ());
        [MetaProperty("primaryQuest", BinPropertyType.Embedded)]
        public MetaEmbedded<QuestDefinition> PrimaryQuest { get; set; } = new (new ());
        [MetaProperty(1827542789, BinPropertyType.Embedded)]
        public MetaEmbedded<QuestDefinition> m1827542789 { get; set; } = new (new ());
        [MetaProperty("scoreDisplayQuest", BinPropertyType.Embedded)]
        public MetaEmbedded<QuestDefinition> ScoreDisplayQuest { get; set; } = new (new ());
        [MetaProperty("objectiveQuest", BinPropertyType.Embedded)]
        public MetaEmbedded<QuestDefinition> ObjectiveQuest { get; set; } = new (new ());
    }
    [MetaClass("SpecificColorMaterialDriver")]
    public class SpecificColorMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mColor", BinPropertyType.Vector4)]
        public Vector4 Color { get; set; } = new Vector4(1f, 0f, 0f, 1f);
    }
    [MetaClass("ItemRecommendationItemList")]
    public class ItemRecommendationItemList : IMetaClass
    {
        [MetaProperty("mItemList", BinPropertyType.Container)]
        public MetaContainer<uint> ItemList { get; set; } = new();
    }
    [MetaClass("QualitySetting")]
    public class QualitySetting : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mFxAa", BinPropertyType.Bool)]
        public bool FxAa { get; set; } = false;
        [MetaProperty("mFrameCap", BinPropertyType.UInt32)]
        public uint FrameCap { get; set; } = 0;
        [MetaProperty(54440845, BinPropertyType.Float)]
        public float m54440845 { get; set; } = 0.75f;
        [MetaProperty("mShadowQuality", BinPropertyType.UInt32)]
        public uint ShadowQuality { get; set; } = 2;
        [MetaProperty(2832832311, BinPropertyType.UInt32)]
        public uint m2832832311 { get; set; } = 2;
        [MetaProperty("mEffectsQuality", BinPropertyType.UInt32)]
        public uint EffectsQuality { get; set; } = 2;
        [MetaProperty("mEnvironmentQuality", BinPropertyType.UInt32)]
        public uint EnvironmentQuality { get; set; } = 2;
    }
    [MetaClass("TargeterDefinitionRange")]
    public class TargeterDefinitionRange : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("textureOrientation", BinPropertyType.UInt32)]
        public uint TextureOrientation { get; set; } = 0;
        [MetaProperty("rangeGrowthMax", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthMax { get; set; } = new (new ());
        [MetaProperty("textureMaxGrowName", BinPropertyType.String)]
        public string TextureMaxGrowName { get; set; } = "";
        [MetaProperty("rangeGrowthDuration", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthDuration { get; set; } = new (new ());
        [MetaProperty("textureOverrideName", BinPropertyType.String)]
        public string TextureOverrideName { get; set; } = "";
        [MetaProperty("rangeGrowthStartTime", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthStartTime { get; set; } = new (new ());
        [MetaProperty("useCasterBoundingBox", BinPropertyType.Optional)]
        public MetaOptional<bool> UseCasterBoundingBox { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("hideWithLineIndicator", BinPropertyType.Bool)]
        public bool HideWithLineIndicator { get; set; } = false;
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; } = new (new ());
        [MetaProperty("overrideBaseRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideBaseRange { get; set; } = new (new ());
        [MetaProperty("hasMaxGrowRange", BinPropertyType.Bool)]
        public bool HasMaxGrowRange { get; set; } = false;
    }
    [MetaClass("TurretFirstBloodLogic")]
    public class TurretFirstBloodLogic : IStatStoneLogicDriver
    {
    }
    [MetaClass("CameraTrapezoid")]
    public class CameraTrapezoid : IMetaClass
    {
        [MetaProperty("mMaxXTop", BinPropertyType.Float)]
        public float MaxXTop { get; set; } = 0f;
        [MetaProperty(2194368105, BinPropertyType.Float)]
        public float m2194368105 { get; set; } = 0f;
        [MetaProperty(2551311184, BinPropertyType.Float)]
        public float m2551311184 { get; set; } = 0f;
        [MetaProperty("mMaxXBottom", BinPropertyType.Float)]
        public float MaxXBottom { get; set; } = 0f;
    }
    [MetaClass(1210599257)]
    public class Class0x48284759 : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("Distance", BinPropertyType.Float)]
        public float Distance { get; set; } = 0f;
    }
    [MetaClass("ESportLeagueEntry")]
    public class ESportLeagueEntry : IMetaClass
    {
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string LeagueName { get; set; } = "";
        [MetaProperty("textureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
    }
    [MetaClass(1221197624)]
    public class Class0x48c9ff38 : AnnouncementStyleBasic
    {
        [MetaProperty("SourceIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink SourceIcon { get; set; } = new(0);
    }
    [MetaClass("MapParticleGroups")]
    public class MapParticleGroups : IMetaClass
    {
        [MetaProperty("groups", BinPropertyType.Container)]
        public MetaContainer<string> Groups { get; set; } = new();
    }
    [MetaClass("GDSMapObjectExtraInfo")]
    public interface GDSMapObjectExtraInfo : IMetaClass
    {
    }
    [MetaClass("VfxDistortionDefinitionData")]
    public class VfxDistortionDefinitionData : IMetaClass
    {
        [MetaProperty("distortionMode", BinPropertyType.Byte)]
        public byte DistortionMode { get; set; } = 1;
        [MetaProperty("distortion", BinPropertyType.Float)]
        public float Distortion { get; set; } = 0f;
        [MetaProperty("normalMapTexture", BinPropertyType.String)]
        public string NormalMapTexture { get; set; } = "";
    }
    [MetaClass(1239049582)]
    public class Class0x49da656e : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty("value", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
        [MetaProperty("PropName", BinPropertyType.String)]
        public string PropName { get; set; } = "";
        [MetaProperty("Key", BinPropertyType.String)]
        public string Key { get; set; } = "";
    }
    [MetaClass("ISecondaryResourceDisplayData")]
    public interface ISecondaryResourceDisplayData : IMetaClass
    {
    }
    [MetaClass("SelectorClipData")]
    public class SelectorClipData : ClipBaseData
    {
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mSelectorPairDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SelectorPairData>> SelectorPairDataList { get; set; } = new();
    }
    [MetaClass("StatFormulaDataList")]
    public class StatFormulaDataList : IMetaClass
    {
        [MetaProperty("StatFormulas", BinPropertyType.Map)]
        public Dictionary<uint, MetaEmbedded<StatFormulaData>> StatFormulas { get; set; } = new();
    }
    [MetaClass("InteractionData")]
    public class InteractionData : IMetaClass
    {
        [MetaProperty("idleAnim", BinPropertyType.String)]
        public string IdleAnim { get; set; } = "Idle1";
        [MetaProperty("shouldRandomizeIdleAnimPhase", BinPropertyType.Bool)]
        public bool ShouldRandomizeIdleAnimPhase { get; set; } = false;
    }
    [MetaClass("BuffCounterByCoefficientCalculationPart")]
    public class BuffCounterByCoefficientCalculationPart : IGameCalculationPartWithBuffCounter
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash BuffName { get; set; } = new(0);
        [MetaProperty("mIconKey", BinPropertyType.String)]
        public string IconKey { get; set; } = "";
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        public string ScalingTagKey { get; set; } = "";
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float Coefficient { get; set; } = 0f;
    }
    [MetaClass("QuestTrackerViewController")]
    public class QuestTrackerViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(414900293, BinPropertyType.Float)]
        public float m414900293 { get; set; } = 0.20000000298023224f;
        [MetaProperty(687202616, BinPropertyType.Float)]
        public float m687202616 { get; set; } = 0.699999988079071f;
        [MetaProperty(103721739, BinPropertyType.UInt32)]
        public uint m103721739 { get; set; } = 15;
        [MetaProperty("MessageDisplayData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMessageDisplayData> MessageDisplayData { get; set; } = new (new ());
        [MetaProperty(2678288798, BinPropertyType.Float)]
        public float m2678288798 { get; set; } = 1f;
        [MetaProperty(2695771816, BinPropertyType.Float)]
        public float m2695771816 { get; set; } = 5f;
        [MetaProperty(3329922468, BinPropertyType.Float)]
        public float m3329922468 { get; set; } = 165f;
        [MetaProperty(3685154491, BinPropertyType.Float)]
        public float m3685154491 { get; set; } = 5f;
    }
    [MetaClass("ColorChooserMaterialDriver")]
    public class ColorChooserMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mColorOn", BinPropertyType.Vector4)]
        public Vector4 ColorOn { get; set; } = new Vector4(1f, 0f, 0f, 1f);
        [MetaProperty("mColorOff", BinPropertyType.Vector4)]
        public Vector4 ColorOff { get; set; } = new Vector4(0f, 0f, 1f, 1f);
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; } = null;
    }
    [MetaClass("OptionItemResolutionDropdown")]
    public class OptionItemResolutionDropdown : OptionItemDropdown
    {
    }
    [MetaClass("FunctionTableSet")]
    public class FunctionTableSet : ScriptTableSet
    {
    }
    [MetaClass("HudMessageDisplayData")]
    public class HudMessageDisplayData : IMetaClass
    {
        [MetaProperty("messageCount", BinPropertyType.UInt32)]
        public uint MessageCount { get; set; } = 4;
        [MetaProperty("TransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> TransitionIn { get; set; } = new (new ());
        [MetaProperty("MessageDuration", BinPropertyType.Float)]
        public float MessageDuration { get; set; } = 5f;
    }
    [MetaClass("VfxPrimitiveArbitraryQuad")]
    public class VfxPrimitiveArbitraryQuad : VfxPrimitiveBase
    {
    }
    [MetaClass("UnitStatusData")]
    public class UnitStatusData : IMetaClass
    {
        [MetaProperty("attackableUnitStatusType", BinPropertyType.UInt32)]
        public uint AttackableUnitStatusType { get; set; } = 11;
        [MetaProperty("statusName", BinPropertyType.String)]
        public string StatusName { get; set; } = "";
        [MetaProperty("textColor", BinPropertyType.Optional)]
        public MetaOptional<Color> TextColor { get; set; } = new MetaOptional<Color>(default(Color), false);
        [MetaProperty("imageInfo", BinPropertyType.Embedded)]
        public MetaEmbedded<HealthbarImageInfo> ImageInfo { get; set; } = new (new ());
        [MetaProperty("locType", BinPropertyType.UInt32)]
        public uint LocType { get; set; } = 0;
    }
    [MetaClass("TargeterDefinition")]
    public interface TargeterDefinition : IMetaClass
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        ITargeterFadeBehavior FadeBehavior { get; set; }
    }
    [MetaClass("FadeEventData")]
    public class FadeEventData : BaseEventData
    {
        [MetaProperty("mTargetAlpha", BinPropertyType.Float)]
        public float TargetAlpha { get; set; } = 0.5f;
        [MetaProperty("mTimeToFade", BinPropertyType.Float)]
        public float TimeToFade { get; set; } = 0f;
    }
    [MetaClass("NeutralTimers")]
    public class NeutralTimers : IMetaClass
    {
        [MetaProperty("mTimers", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<NeutralTimerData>> Timers { get; set; } = new();
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string SceneName { get; set; } = "";
        [MetaProperty(4002892974, BinPropertyType.String)]
        public string m4002892974 { get; set; } = "";
    }
    [MetaClass("HudGearSelectionUIData")]
    public class HudGearSelectionUIData : IMetaClass
    {
        [MetaProperty("timerEnabled", BinPropertyType.Bool)]
        public bool TimerEnabled { get; set; } = false;
        [MetaProperty("mGearSelectionTransitionIntro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> GearSelectionTransitionIntro { get; set; } = new (new ());
        [MetaProperty("timerCountdownWarningStart", BinPropertyType.Float)]
        public float TimerCountdownWarningStart { get; set; } = 10f;
        [MetaProperty("selectionButtonDelayTime", BinPropertyType.Float)]
        public float SelectionButtonDelayTime { get; set; } = 0.25f;
        [MetaProperty("mGearSelectionTransitionOutro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> GearSelectionTransitionOutro { get; set; } = new (new ());
        [MetaProperty("timerCountdownDuration", BinPropertyType.Float)]
        public float TimerCountdownDuration { get; set; } = 30f;
    }
    [MetaClass(1295117638)]
    public class Class0x4d31ed46 : IMetaClass
    {
        [MetaProperty("mCatalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mIdentityInstance", BinPropertyType.Embedded)]
        public MetaEmbedded<IdentityInstance> IdentityInstance { get; set; } = new (new ());
    }
    [MetaClass("DynamicMaterialTextureSwapDef")]
    public class DynamicMaterialTextureSwapDef : IMetaClass
    {
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("options", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialTextureSwapOption>> Options { get; set; } = new();
    }
    [MetaClass("MaterialInstanceTextureDef")]
    public class MaterialInstanceTextureDef : IMetaClass
    {
        [MetaProperty("uncensoredTextures", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredTextures { get; set; } = new();
        [MetaProperty("texturePath", BinPropertyType.String)]
        public string TexturePath { get; set; } = "";
    }
    [MetaClass(1313586219)]
    public class Class0x4e4bbc2b : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
    }
    [MetaClass("MapCamera")]
    public class MapCamera : MapPlaceable
    {
        [MetaProperty(1446648129, BinPropertyType.Float)]
        public float m1446648129 { get; set; } = 3600f;
        [MetaProperty("yaw", BinPropertyType.Float)]
        public float Yaw { get; set; } = 0f;
        [MetaProperty(1866351399, BinPropertyType.Float)]
        public float m1866351399 { get; set; } = 1800f;
        [MetaProperty("FieldOfView", BinPropertyType.Float)]
        public float FieldOfView { get; set; } = 29.25f;
        [MetaProperty("pitch", BinPropertyType.Float)]
        public float Pitch { get; set; } = 46f;
    }
    [MetaClass("FollowTerrainHeightSolver")]
    public class FollowTerrainHeightSolver : HeightSolverType
    {
        [MetaProperty("mMaxSlope", BinPropertyType.Float)]
        public float MaxSlope { get; set; } = 10f;
        [MetaProperty("mHeightOffset", BinPropertyType.Float)]
        public float HeightOffset { get; set; } = 0f;
    }
    [MetaClass("ScriptPreloadModule")]
    public class ScriptPreloadModule : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string PreloadResourceName { get; set; } = "";
    }
    [MetaClass("HudSpellSlotResetFeedbackData")]
    public class HudSpellSlotResetFeedbackData : IMetaClass
    {
        [MetaProperty("spellResetFlashFadeDuration", BinPropertyType.Float)]
        public float SpellResetFlashFadeDuration { get; set; } = 0.20000000298023224f;
        [MetaProperty("spellResetFlashScaleDownDuration", BinPropertyType.Float)]
        public float SpellResetFlashScaleDownDuration { get; set; } = 0.20000000298023224f;
        [MetaProperty("spellResetScaleMultiplier", BinPropertyType.Float)]
        public float SpellResetScaleMultiplier { get; set; } = 1.2999999523162842f;
    }
    [MetaClass("IsSkinSpawnConditionData")]
    public class IsSkinSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; } = new();
        [MetaProperty("mSkinId", BinPropertyType.UInt32)]
        public uint SkinId { get; set; } = 0;
    }
    [MetaClass("MaterialSwitchData")]
    public class MaterialSwitchData : IMetaClass
    {
        [MetaProperty("on", BinPropertyType.Bool)]
        public bool On { get; set; } = true;
    }
    [MetaClass("EffectDesaturateElementData")]
    public class EffectDesaturateElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("MaximumSaturation", BinPropertyType.Float)]
        public float MaximumSaturation { get; set; } = 1f;
        [MetaProperty("MinimumSaturation", BinPropertyType.Float)]
        public float MinimumSaturation { get; set; } = 0f;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
    }
    [MetaClass("MasteryBadgeData")]
    public class MasteryBadgeData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.Hash)]
        public MetaHash Name { get; set; } = new(0);
        [MetaProperty("mVerticalOffset", BinPropertyType.Float)]
        public float VerticalOffset { get; set; } = 0f;
        [MetaProperty("mMasteryLevel", BinPropertyType.UInt32)]
        public uint MasteryLevel { get; set; } = 0;
        [MetaProperty("mSummonerIconId", BinPropertyType.Int32)]
        public int SummonerIconId { get; set; } = -1;
        [MetaProperty("mRenderScale", BinPropertyType.Float)]
        public float RenderScale { get; set; } = 1f;
        [MetaProperty("mParticleName", BinPropertyType.String)]
        public string ParticleName { get; set; } = "";
    }
    [MetaClass("TFTHudTunables")]
    public class TFTHudTunables : IMetaClass
    {
        [MetaProperty("mStageData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudStageData> StageData { get; set; } = new (new ());
        [MetaProperty("mCombatRecapData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudCombatRecapData> CombatRecapData { get; set; } = new (new ());
        [MetaProperty(614458760, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m614458760 { get; set; } = new (new ());
        [MetaProperty("mMobileDownscaleData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudMobileDownscaleData> MobileDownscaleData { get; set; } = new (new ());
        [MetaProperty("mScoreboardData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudScoreboardData> ScoreboardData { get; set; } = new (new ());
        [MetaProperty("mAnnouncementData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudAnnouncementData> AnnouncementData { get; set; } = new (new ());
        [MetaProperty("mUnitShopData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudUnitShopData> UnitShopData { get; set; } = new (new ());
        [MetaProperty("mZoomTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xda3c6dc6> ZoomTransitionData { get; set; } = new (new ());
        [MetaProperty("mNotificationsData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudNotificationsData> NotificationsData { get; set; } = new (new ());
    }
    [MetaClass("SpellDataValue")]
    public class SpellDataValue : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mValues", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; } = new();
    }
    [MetaClass("BlendableClipData")]
    public interface BlendableClipData : ClipBaseData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        MetaHash MaskDataName { get; set; }
        [MetaProperty("mSyncGroupDataName", BinPropertyType.Hash)]
        MetaHash SyncGroupDataName { get; set; }
        [MetaProperty("mTrackDataName", BinPropertyType.Hash)]
        MetaHash TrackDataName { get; set; }
        [MetaProperty("mEventDataMap", BinPropertyType.Map)]
        Dictionary<MetaHash, BaseEventData> EventDataMap { get; set; }
    }
    [MetaClass("ITargeterFadeBehavior")]
    public interface ITargeterFadeBehavior : IMetaClass
    {
    }
    [MetaClass("HasBuffNameSpawnConditions")]
    public class HasBuffNameSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HasBuffSpawnConditionData>> Conditions { get; set; } = new();
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; } = new (new ());
    }
    [MetaClass("AnnouncementsViewController")]
    public class AnnouncementsViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(884045158, BinPropertyType.Hash)]
        public MetaHash m884045158 { get; set; } = new(0);
    }
    [MetaClass("ClearTargetCooldownCheat")]
    public class ClearTargetCooldownCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass("HermiteSplineInfo")]
    public class HermiteSplineInfo : ISplineInfo
    {
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        public bool UseMissilePositionAsOrigin { get; set; } = false;
        [MetaProperty("mStartPositionOffset", BinPropertyType.Vector3)]
        public Vector3 StartPositionOffset { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("mControlPoint1", BinPropertyType.Vector3)]
        public Vector3 ControlPoint1 { get; set; } = new Vector3(-1f, 0f, 0f);
        [MetaProperty("mControlPoint2", BinPropertyType.Vector3)]
        public Vector3 ControlPoint2 { get; set; } = new Vector3(1f, 0f, 0f);
    }
    [MetaClass(1368219584)]
    public class Class0x518d5fc0 : IMetaClass
    {
        [MetaProperty("EventName", BinPropertyType.String)]
        public string EventName { get; set; } = "";
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
    }
    [MetaClass("HudHealthBarData")]
    public class HudHealthBarData : IMetaClass
    {
        [MetaProperty("championProgressiveTickData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarProgressiveTickData> ChampionProgressiveTickData { get; set; } = new (new ());
        [MetaProperty("burstHealData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarBurstHealData> BurstHealData { get; set; } = new (new ());
        [MetaProperty("burstData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarBurstData> BurstData { get; set; } = new (new ());
        [MetaProperty("resurrectingAlpha", BinPropertyType.Float)]
        public float ResurrectingAlpha { get; set; } = 0.4000000059604645f;
        [MetaProperty("defenseIconData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarDefenseIconData> DefenseIconData { get; set; } = new (new ());
        [MetaProperty("towerBurstData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarBurstData> TowerBurstData { get; set; } = new (new ());
        [MetaProperty("defenseModifierData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarDefenseModifierData> DefenseModifierData { get; set; } = new (new ());
        [MetaProperty("untargetableAlpha", BinPropertyType.Float)]
        public float UntargetableAlpha { get; set; } = 0.4000000059604645f;
        [MetaProperty("fadeData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarFadeData> FadeData { get; set; } = new (new ());
    }
    [MetaClass("FlexValueFloat")]
    public class FlexValueFloat : IMetaClass
    {
        [MetaProperty("mValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Value { get; set; } = new (new ());
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint FlexID { get; set; } = 0;
    }
    [MetaClass("VfxFloatOverLifeMaterialDriver")]
    public class VfxFloatOverLifeMaterialDriver : IVfxMaterialDriver
    {
        [MetaProperty("frequency", BinPropertyType.Byte)]
        public byte Frequency { get; set; } = 0;
        [MetaProperty("graph", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedFloatVariableData> Graph { get; set; } = new (new ());
    }
    [MetaClass("PlayerCardWidgetConfig")]
    public class PlayerCardWidgetConfig : IMetaClass
    {
        [MetaProperty("mCardType", BinPropertyType.Byte)]
        public byte CardType { get; set; } = 0;
        [MetaProperty(2073776835, BinPropertyType.UInt32)]
        public uint m2073776835 { get; set; } = 0;
        [MetaProperty("mTeamBased", BinPropertyType.Bool)]
        public bool TeamBased { get; set; } = true;
    }
    [MetaClass("ContextualConditionShopCloseCount")]
    public class ContextualConditionShopCloseCount : IContextualCondition
    {
        [MetaProperty("mShopCloseCount", BinPropertyType.UInt32)]
        public uint ShopCloseCount { get; set; } = 0;
    }
    [MetaClass("SkinCharacterMetaDataProperties_SpawningSkinOffset")]
    public class SkinCharacterMetaDataProperties_SpawningSkinOffset : IMetaClass
    {
        [MetaProperty("offset", BinPropertyType.Int32)]
        public int Offset { get; set; } = 0;
        [MetaProperty("tag", BinPropertyType.String)]
        public string Tag { get; set; } = "";
    }
    [MetaClass("AnchorSingle")]
    public class AnchorSingle : AnchorBase
    {
        [MetaProperty("anchor", BinPropertyType.Vector2)]
        public Vector2 Anchor { get; set; } = new Vector2(0f, 0f);
    }
    [MetaClass("FxTarget")]
    public class FxTarget : IMetaClass
    {
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
        [MetaProperty("Index", BinPropertyType.Int32)]
        public int Index { get; set; } = 0;
    }
    [MetaClass("StatFilterDefinition")]
    public class StatFilterDefinition : IMetaClass
    {
        [MetaProperty("MatchingCategories", BinPropertyType.Container)]
        public MetaContainer<MetaHash> MatchingCategories { get; set; } = new();
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("VfxProbabilityTableData")]
    public class VfxProbabilityTableData : IMetaClass
    {
        [MetaProperty("keyTimes", BinPropertyType.Container)]
        public MetaContainer<float> KeyTimes { get; set; } = new();
        [MetaProperty("singleValue", BinPropertyType.Float)]
        public float SingleValue { get; set; } = 1f;
        [MetaProperty("keyValues", BinPropertyType.Container)]
        public MetaContainer<float> KeyValues { get; set; } = new();
    }
    [MetaClass("MissileTriggeredActionSpec")]
    public interface MissileTriggeredActionSpec : IMetaClass
    {
    }
    [MetaClass("MaterialParameterData")]
    public class MaterialParameterData : IMetaClass
    {
        [MetaProperty("type", BinPropertyType.Byte)]
        public byte Type { get; set; } = 4;
        [MetaProperty("DefaultValue", BinPropertyType.Vector4)]
        public Vector4 DefaultValue { get; set; } = new Vector4(0f, 0f, 0f, 1f);
    }
    [MetaClass("PassThroughParamsTable")]
    public class PassThroughParamsTable : ScriptTable
    {
    }
    [MetaClass("ParticleEventData")]
    public class ParticleEventData : BaseEventData
    {
        [MetaProperty("mIsDetachable", BinPropertyType.Bool)]
        public bool IsDetachable { get; set; } = false;
        [MetaProperty("mEffectName", BinPropertyType.String)]
        public string EffectName { get; set; } = "";
        [MetaProperty("mParticleEventDataPairList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ParticleEventDataPair>> ParticleEventDataPairList { get; set; } = new();
        [MetaProperty("mIsKillEvent", BinPropertyType.Bool)]
        public bool IsKillEvent { get; set; } = true;
        [MetaProperty("mScalePlaySpeedWithAnimation", BinPropertyType.Bool)]
        public bool ScalePlaySpeedWithAnimation { get; set; } = false;
        [MetaProperty("mEnemyEffectKey", BinPropertyType.Hash)]
        public MetaHash EnemyEffectKey { get; set; } = new(0);
        [MetaProperty(2743230979, BinPropertyType.Bool)]
        public bool m2743230979 { get; set; } = false;
        [MetaProperty("mIsLoop", BinPropertyType.Bool)]
        public bool IsLoop { get; set; } = true;
        [MetaProperty("mEffectKey", BinPropertyType.Hash)]
        public MetaHash EffectKey { get; set; } = new(0);
    }
    [MetaClass("TFTDragData")]
    public class TFTDragData : IMetaClass
    {
        [MetaProperty("mDragSoundEvent", BinPropertyType.String)]
        public string DragSoundEvent { get; set; } = "";
        [MetaProperty(1152070302, BinPropertyType.Bool)]
        public bool m1152070302 { get; set; } = true;
        [MetaProperty("mDragReleaseDuration", BinPropertyType.Float)]
        public float DragReleaseDuration { get; set; } = 0.30000001192092896f;
        [MetaProperty("mDragPickupHeight", BinPropertyType.Float)]
        public float DragPickupHeight { get; set; } = 50f;
        [MetaProperty(1494391998, BinPropertyType.Float)]
        public float m1494391998 { get; set; } = 150f;
        [MetaProperty("mDropSoundEvent", BinPropertyType.String)]
        public string DropSoundEvent { get; set; } = "";
        [MetaProperty("mDragOvershootHeight", BinPropertyType.Float)]
        public float DragOvershootHeight { get; set; } = 1.2999999523162842f;
        [MetaProperty(1838159659, BinPropertyType.Float)]
        public float m1838159659 { get; set; } = 0.03999999910593033f;
        [MetaProperty("mDragOvershootDuration", BinPropertyType.Float)]
        public float DragOvershootDuration { get; set; } = 0.10000000149011612f;
        [MetaProperty("mDragPickupDuration", BinPropertyType.Float)]
        public float DragPickupDuration { get; set; } = 0.30000001192092896f;
        [MetaProperty("mDragBlendTime", BinPropertyType.Float)]
        public float DragBlendTime { get; set; } = 0.3499999940395355f;
        [MetaProperty("mHoldToDrag", BinPropertyType.Bool)]
        public bool HoldToDrag { get; set; } = false;
        [MetaProperty(3509153429, BinPropertyType.Float)]
        public float m3509153429 { get; set; } = 5f;
        [MetaProperty(3797316178, BinPropertyType.Float)]
        public float m3797316178 { get; set; } = 2.5f;
        [MetaProperty("mHoldToHover", BinPropertyType.Bool)]
        public bool HoldToHover { get; set; } = false;
    }
    [MetaClass("TrackMouseMovement")]
    public class TrackMouseMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool ProjectTargetToCastRange { get; set; } = false;
        [MetaProperty(1615432143, BinPropertyType.Float)]
        public float m1615432143 { get; set; } = 0.25f;
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool UseGroundHeightAtTarget { get; set; } = true;
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool InferDirectionFromFacingIfNeeded { get; set; } = true;
        [MetaProperty("mInitialSpeed", BinPropertyType.Float)]
        public float InitialSpeed { get; set; } = 0f;
        [MetaProperty("mAcceleration", BinPropertyType.Float)]
        public float Acceleration { get; set; } = 0f;
        [MetaProperty(2226849642, BinPropertyType.Container)]
        public MetaContainer<float> m2226849642 { get; set; } = new();
        [MetaProperty("mMinSpeed", BinPropertyType.Float)]
        public float MinSpeed { get; set; } = 0f;
        [MetaProperty("mMaxSpeed", BinPropertyType.Float)]
        public float MaxSpeed { get; set; } = 0f;
    }
    [MetaClass("WorldVarsTable")]
    public class WorldVarsTable : ScriptTable
    {
    }
    [MetaClass("ContextualConditionHasGold")]
    public class ContextualConditionHasGold : IContextualCondition
    {
        [MetaProperty("mGold", BinPropertyType.Float)]
        public float Gold { get; set; } = 0f;
    }
    [MetaClass("UpdaterResourceData")]
    public class UpdaterResourceData : IMetaClass
    {
        [MetaProperty("mUpdaterDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<UpdaterData>> UpdaterDataList { get; set; } = new();
    }
    [MetaClass("RigResource")]
    public class RigResource : IMetaClass
    {
        [MetaProperty("mAssetName", BinPropertyType.String)]
        public string AssetName { get; set; } = "";
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mShaderJointList", BinPropertyType.Container)]
        public MetaContainer<ushort> ShaderJointList { get; set; } = new();
        [MetaProperty("mJointList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Joint>> JointList { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt16)]
        public ushort Flags { get; set; } = 0;
    }
    [MetaClass(1428235105)]
    public class Class0x55212361 : IMetaClass
    {
        [MetaProperty("Column0LabelTraKey", BinPropertyType.String)]
        public string Column0LabelTraKey { get; set; } = "";
        [MetaProperty("Column1LabelTraKey", BinPropertyType.String)]
        public string Column1LabelTraKey { get; set; } = "";
    }
    [MetaClass("OptionTemplateHotkeysLabel")]
    public class OptionTemplateHotkeysLabel : IMetaClass
    {
        [MetaProperty("Label", BinPropertyType.Hash)]
        public MetaHash Label { get; set; } = new(0);
    }
    [MetaClass(1439708695)]
    public class Class0x55d03617 : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> Position { get; set; } = new (new ());
    }
    [MetaClass("MaterialOverrideCallbackDynamicMaterial")]
    public class MaterialOverrideCallbackDynamicMaterial : IMetaClass
    {
    }
    [MetaClass("ContextualConditionMarkerName")]
    public class ContextualConditionMarkerName : IContextualCondition
    {
        [MetaProperty("mMarkerNames", BinPropertyType.Container)]
        public MetaContainer<string> MarkerNames { get; set; } = new();
    }
    [MetaClass("SourceTypeFilter")]
    public class SourceTypeFilter : IStatStoneLogicDriver
    {
        [MetaProperty(507497828, BinPropertyType.Bool)]
        public bool m507497828 { get; set; } = true;
        [MetaProperty(1203421971, BinPropertyType.Bool)]
        public bool m1203421971 { get; set; } = true;
        [MetaProperty(3588584256, BinPropertyType.Bool)]
        public bool m3588584256 { get; set; } = false;
    }
    [MetaClass("ContextualConditionCharacterLevel")]
    public class ContextualConditionCharacterLevel : IContextualCondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 0;
        [MetaProperty("mCharacterLevel", BinPropertyType.Byte)]
        public byte CharacterLevel { get; set; } = 0;
    }
    [MetaClass("ToggleAfkDetectionCheat")]
    public class ToggleAfkDetectionCheat : Cheat
    {
    }
    [MetaClass("BlendingSwitchMaterialDriver")]
    public class BlendingSwitchMaterialDriver : SwitchMaterialDriver
    {
        [MetaProperty("mOverrideBlendTimes", BinPropertyType.Container)]
        public MetaContainer<float> OverrideBlendTimes { get; set; } = new();
        [MetaProperty("mBlendTime", BinPropertyType.Float)]
        public float BlendTime { get; set; } = 1f;
    }
    [MetaClass("VfxPrimitiveArbitraryTrail")]
    public class VfxPrimitiveArbitraryTrail : VfxPrimitiveTrailBase
    {
        [MetaProperty("mTrail", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxTrailDefinitionData> Trail { get; set; } = new (new ());
    }
    [MetaClass("CSSStyle")]
    public class CSSStyle : IMetaClass
    {
        [MetaProperty("color", BinPropertyType.Optional)]
        public MetaOptional<Color> Color { get; set; } = new MetaOptional<Color>(default(Color), false);
        [MetaProperty("italics", BinPropertyType.Optional)]
        public MetaOptional<bool> Italics { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("bold", BinPropertyType.Optional)]
        public MetaOptional<bool> Bold { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("underline", BinPropertyType.Optional)]
        public MetaOptional<bool> Underline { get; set; } = new MetaOptional<bool>(default(bool), false);
    }
    [MetaClass("HasUnitTagsCastRequirement")]
    public class HasUnitTagsCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> UnitTags { get; set; } = new (new ());
    }
    [MetaClass("BuffCounterDynamicMaterialFloatDriver")]
    public class BuffCounterDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mScriptName", BinPropertyType.String)]
        public string ScriptName { get; set; } = "";
    }
    [MetaClass("OverrideAttackTimeData")]
    public class OverrideAttackTimeData : IMetaClass
    {
        [MetaProperty(546903361, BinPropertyType.Structure)]
        public IGameCalculation m546903361 { get; set; } = null;
        [MetaProperty("mCastTimePercent", BinPropertyType.Float)]
        public float CastTimePercent { get; set; } = 0.6000000238418579f;
    }
    [MetaClass("StatByCoefficientCalculationPart")]
    public class StatByCoefficientCalculationPart : IGameCalculationPartWithStats
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte Stat { get; set; } = 0;
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte StatFormula { get; set; } = 0;
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float Coefficient { get; set; } = 0f;
    }
    [MetaClass("ContextualConditionNeutralMinionMapSide")]
    public class ContextualConditionNeutralMinionMapSide : IContextualCondition
    {
        [MetaProperty("mTeamCompareOp", BinPropertyType.Byte)]
        public byte TeamCompareOp { get; set; } = 0;
    }
    [MetaClass("MapVisibilityFlagDefinitions")]
    public class MapVisibilityFlagDefinitions : IMetaClass
    {
        [MetaProperty("FlagDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapVisibilityFlagDefinition>> FlagDefinitions { get; set; } = new();
        [MetaProperty(1610350815, BinPropertyType.Bool)]
        public bool m1610350815 { get; set; } = false;
        [MetaProperty("FlagRange", BinPropertyType.Embedded)]
        public MetaEmbedded<MapVisibilityFlagRange> FlagRange { get; set; } = new (new ());
        [MetaProperty(2183354083, BinPropertyType.Bool)]
        public bool m2183354083 { get; set; } = false;
    }
    [MetaClass("HudRadialWheelData")]
    public class HudRadialWheelData : IMetaClass
    {
        [MetaProperty("mRadialWheelButtonTransitionOutro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> RadialWheelButtonTransitionOutro { get; set; } = new (new ());
        [MetaProperty("mRadialWheelUITransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> RadialWheelUITransitionData { get; set; } = new (new ());
        [MetaProperty("activateWheelDelayTime", BinPropertyType.Float)]
        public float ActivateWheelDelayTime { get; set; } = 0.17499999701976776f;
        [MetaProperty("mRadialWheelButtonTransitionIntro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> RadialWheelButtonTransitionIntro { get; set; } = new (new ());
    }
    [MetaClass("MapParticle")]
    public class MapParticle : MapPlaceable
    {
        [MetaProperty("startDisabled", BinPropertyType.Bool)]
        public bool StartDisabled { get; set; } = false;
        [MetaProperty("system", BinPropertyType.ObjectLink)]
        public MetaObjectLink System { get; set; } = new(0);
        [MetaProperty("eyeCandy", BinPropertyType.Bool)]
        public bool EyeCandy { get; set; } = false;
        [MetaProperty("Transitional", BinPropertyType.Bool)]
        public bool Transitional { get; set; } = false;
        [MetaProperty("quality", BinPropertyType.Int32)]
        public int Quality { get; set; } = 31;
        [MetaProperty("colorModulate", BinPropertyType.Vector4)]
        public Vector4 ColorModulate { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        [MetaProperty("groupName", BinPropertyType.String)]
        public string GroupName { get; set; } = "";
        [MetaProperty("visibilityMode", BinPropertyType.UInt32)]
        public uint VisibilityMode { get; set; } = 0;
    }
    [MetaClass("MaterialInstanceParamDef")]
    public class MaterialInstanceParamDef : IMetaClass
    {
        [MetaProperty("value", BinPropertyType.Vector4)]
        public Vector4 Value { get; set; } = new Vector4(0f, 0f, 0f, 0f);
    }
    [MetaClass("MapPlaceable")]
    public class MapPlaceable : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mVisibilityFlags", BinPropertyType.Byte)]
        public byte VisibilityFlags { get; set; } = 255;
        [MetaProperty("transform", BinPropertyType.Matrix44)]
        public R3DMatrix44 Transform { get; set; } = new R3DMatrix44(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
    }
    [MetaClass("AnnouncementDefinition")]
    public class AnnouncementDefinition : IMetaClass
    {
        [MetaProperty("MutatorOverrides", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<AnnouncementDefinitionData>> MutatorOverrides { get; set; } = new();
        [MetaProperty("DefaultData", BinPropertyType.Embedded)]
        public MetaEmbedded<AnnouncementDefinitionData> DefaultData { get; set; } = new (new ());
    }
    [MetaClass(1513866541)]
    public class Class0x5a3bc52d : IMetaClass
    {
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Items { get; set; } = new();
        [MetaProperty(1357847074, BinPropertyType.UInt32)]
        public uint m1357847074 { get; set; } = 6;
    }
    [MetaClass("PerkReplacement")]
    public class PerkReplacement : IMetaClass
    {
        [MetaProperty("mReplaceWith", BinPropertyType.Hash)]
        public MetaHash ReplaceWith { get; set; } = new(0);
        [MetaProperty("mReplaceTarget", BinPropertyType.Hash)]
        public MetaHash ReplaceTarget { get; set; } = new(0);
    }
    [MetaClass(95149995)]
    public class Class0x5abdfab : IGameCalculationPart
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte Stat { get; set; } = 0;
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte StatFormula { get; set; } = 0;
        [MetaProperty("mDataValue", BinPropertyType.Hash)]
        public MetaHash DataValue { get; set; } = new(0);
        [MetaProperty(3219565825, BinPropertyType.Float)]
        public float m3219565825 { get; set; } = 0f;
    }
    [MetaClass("PlatformSpellInfo")]
    public class PlatformSpellInfo : IMetaClass
    {
        [MetaProperty("mGameModes", BinPropertyType.Container)]
        public MetaContainer<string> GameModes { get; set; } = new();
        [MetaProperty("mAvatarLevelRequired", BinPropertyType.Int32)]
        public int AvatarLevelRequired { get; set; } = -1;
        [MetaProperty("mSpellID", BinPropertyType.Int32)]
        public int SpellID { get; set; } = -1;
        [MetaProperty("mPlatformEnabled", BinPropertyType.Bool)]
        public bool PlatformEnabled { get; set; } = false;
    }
    [MetaClass("OptionItemSliderGraphicsQuality")]
    public class OptionItemSliderGraphicsQuality : OptionItemSlider
    {
        [MetaProperty("TooltipTraKey", BinPropertyType.String)]
        public string TooltipTraKey { get; set; } = "";
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
    }
    [MetaClass("PerkConfig")]
    public class PerkConfig : IMetaClass
    {
        [MetaProperty(277771373, BinPropertyType.UInt32)]
        public uint m277771373 { get; set; } = 0;
        [MetaProperty("mPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkReplacementList> PerkReplacements { get; set; } = new (new ());
        [MetaProperty("mBotOverrideSet", BinPropertyType.ObjectLink)]
        public MetaObjectLink BotOverrideSet { get; set; } = new(0);
    }
    [MetaClass("HasBuffCastRequirement")]
    public class HasBuffCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash BuffName { get; set; } = new(0);
        [MetaProperty("mFromAnyone", BinPropertyType.Bool)]
        public bool FromAnyone { get; set; } = false;
    }
    [MetaClass("QueueDisplayData")]
    public class QueueDisplayData : IMetaClass
    {
        [MetaProperty(35565451, BinPropertyType.Hash)]
        public MetaHash m35565451 { get; set; } = new(0);
        [MetaProperty("IllustrationIconElement", BinPropertyType.Hash)]
        public MetaHash IllustrationIconElement { get; set; } = new(0);
        [MetaProperty("DisplayNameTraKey", BinPropertyType.String)]
        public string DisplayNameTraKey { get; set; } = "";
        [MetaProperty(3465986044, BinPropertyType.Hash)]
        public MetaHash m3465986044 { get; set; } = new(0);
        [MetaProperty("queueId", BinPropertyType.Int64)]
        public long QueueId { get; set; } = 0;
        [MetaProperty(4062300114, BinPropertyType.Hash)]
        public MetaHash m4062300114 { get; set; } = new(0);
    }
    [MetaClass("IntOffsetTableGet")]
    public class IntOffsetTableGet : IIntGet
    {
        [MetaProperty("offset", BinPropertyType.Int32)]
        public int Offset { get; set; } = 0;
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
    }
    [MetaClass("TftTraitList")]
    public class TftTraitList : IMetaClass
    {
        [MetaProperty("mTraits", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Traits { get; set; } = new();
        [MetaProperty("VfxResourceResolver", BinPropertyType.ObjectLink)]
        public MetaObjectLink VfxResourceResolver { get; set; } = new(0);
    }
    [MetaClass("AtomicClipData")]
    public class AtomicClipData : BlendableClipData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        public MetaHash MaskDataName { get; set; } = new(0);
        [MetaProperty("mSyncGroupDataName", BinPropertyType.Hash)]
        public MetaHash SyncGroupDataName { get; set; } = new(0);
        [MetaProperty("mTrackDataName", BinPropertyType.Hash)]
        public MetaHash TrackDataName { get; set; } = new(0);
        [MetaProperty("mEventDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, BaseEventData> EventDataMap { get; set; } = new();
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mTickDuration", BinPropertyType.Float)]
        public float TickDuration { get; set; } = 0.03333333507180214f;
        [MetaProperty("mUpdaterResourceData", BinPropertyType.Structure)]
        public UpdaterResourceData UpdaterResourceData { get; set; } = null;
        [MetaProperty("mAnimationResourceData", BinPropertyType.Embedded)]
        public MetaEmbedded<AnimationResourceData> AnimationResourceData { get; set; } = new (new ());
    }
    [MetaClass("LootItem")]
    public class LootItem : LootOutputBase
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mHoverDescription", BinPropertyType.String)]
        public string HoverDescription { get; set; } = "";
        [MetaProperty("mDetails", BinPropertyType.Embedded)]
        public MetaEmbedded<LootItemDetails> Details { get; set; } = new (new ());
        [MetaProperty("mAdminDescription", BinPropertyType.String)]
        public string AdminDescription { get; set; } = "";
        [MetaProperty("mInternalName", BinPropertyType.String)]
        public string InternalName { get; set; } = "";
        [MetaProperty("mStatus", BinPropertyType.Embedded)]
        public MetaEmbedded<LootStatus> Status { get; set; } = new (new ());
    }
    [MetaClass("ICastRequirement")]
    public interface ICastRequirement : IMetaClass
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        bool InvertResult { get; set; }
    }
    [MetaClass("IsOwnerHeroSpawnConditions")]
    public class IsOwnerHeroSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<IsOwnerHeroConditionData>> Conditions { get; set; } = new();
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; } = new (new ());
    }
    [MetaClass("ByCharLevelBreakpointsCalculationPart")]
    public class ByCharLevelBreakpointsCalculationPart : IGameCalculationPartByCharLevel
    {
        [MetaProperty(48149840, BinPropertyType.Float)]
        public float m48149840 { get; set; } = 0f;
        [MetaProperty("mBreakpoints", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Breakpoint>> Breakpoints { get; set; } = new();
        [MetaProperty("mLevel1Value", BinPropertyType.Float)]
        public float Level1Value { get; set; } = 0f;
    }
    [MetaClass("GetKeyValueInCustomTableBlock")]
    public class GetKeyValueInCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("Key", BinPropertyType.Structure)]
        public IScriptValueGet Key { get; set; } = null;
        [MetaProperty("OutValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutValue { get; set; } = new (new ());
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("EmblemPosition")]
    public class EmblemPosition : IMetaClass
    {
        [MetaProperty("mHorizontal", BinPropertyType.String)]
        public string Horizontal { get; set; } = "";
        [MetaProperty("mVertical", BinPropertyType.String)]
        public string Vertical { get; set; } = "";
    }
    [MetaClass("MapClouds")]
    public class MapClouds : MapGraphicsFeature
    {
        [MetaProperty("IsEyeCandy", BinPropertyType.Bool)]
        public bool IsEyeCandy { get; set; } = false;
        [MetaProperty("CloudsTexturePath", BinPropertyType.String)]
        public string CloudsTexturePath { get; set; } = "";
        [MetaProperty(1686403411, BinPropertyType.Vector4)]
        public Vector4 m1686403411 { get; set; } = new Vector4(0.5f, 0.5f, 0.5f, 1f);
        [MetaProperty("Layers", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapCloudsLayer>> Layers { get; set; } = new();
    }
    [MetaClass("TFTHudCombatRecapData")]
    public class TFTHudCombatRecapData : IMetaClass
    {
        [MetaProperty(1162113435, BinPropertyType.Float)]
        public float m1162113435 { get; set; } = 0.30000001192092896f;
        [MetaProperty("mPanelTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> PanelTransition { get; set; } = new (new ());
    }
    [MetaClass("ParticleEventDataPair")]
    public class ParticleEventDataPair : IMetaClass
    {
        [MetaProperty("mTargetBoneName", BinPropertyType.Hash)]
        public MetaHash TargetBoneName { get; set; } = new(0);
        [MetaProperty("mBoneName", BinPropertyType.Hash)]
        public MetaHash BoneName { get; set; } = new(0);
    }
    [MetaClass("SourceLessThanHealthPercentageFilter")]
    public class SourceLessThanHealthPercentageFilter : IStatStoneLogicDriver
    {
        [MetaProperty("healthPercentage", BinPropertyType.Float)]
        public float HealthPercentage { get; set; } = 50f;
    }
    [MetaClass("EffectLineElementData")]
    public class EffectLineElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mThickness", BinPropertyType.Float)]
        public float Thickness { get; set; } = 1f;
        [MetaProperty("mRightSlicePercentage", BinPropertyType.Float)]
        public float RightSlicePercentage { get; set; } = 0.8999999761581421f;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
    }
    [MetaClass("SpellObject")]
    public class SpellObject : IMetaClass
    {
        [MetaProperty("mScript", BinPropertyType.Structure)]
        public LolSpellScript Script { get; set; } = null;
        [MetaProperty("mSpell", BinPropertyType.Structure)]
        public SpellDataResource Spell { get; set; } = null;
        [MetaProperty("mBuff", BinPropertyType.Structure)]
        public BuffData Buff { get; set; } = null;
        [MetaProperty("mScriptName", BinPropertyType.String)]
        public string ScriptName { get; set; } = "";
    }
    [MetaClass("VfxAlphaErosionDefinitionData")]
    public class VfxAlphaErosionDefinitionData : IMetaClass
    {
        [MetaProperty("erosionFeatherOut", BinPropertyType.Float)]
        public float ErosionFeatherOut { get; set; } = 0.10000000149011612f;
        [MetaProperty("UseLingerErosionDriveCurve", BinPropertyType.Bool)]
        public bool UseLingerErosionDriveCurve { get; set; } = false;
        [MetaProperty("erosionMapAddressMode", BinPropertyType.Byte)]
        public byte ErosionMapAddressMode { get; set; } = 2;
        [MetaProperty("erosionFeatherIn", BinPropertyType.Float)]
        public float ErosionFeatherIn { get; set; } = 0.10000000149011612f;
        [MetaProperty("erosionMapName", BinPropertyType.String)]
        public string ErosionMapName { get; set; } = "";
        [MetaProperty("erosionSliceWidth", BinPropertyType.Float)]
        public float ErosionSliceWidth { get; set; } = 1.5f;
        [MetaProperty("LingerErosionDriveCurve", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> LingerErosionDriveCurve { get; set; } = new (new ());
        [MetaProperty("erosionMapChannelMixer", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> ErosionMapChannelMixer { get; set; } = new (new ());
        [MetaProperty("erosionDriveCurve", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> ErosionDriveCurve { get; set; } = new (new ());
        [MetaProperty("erosionDriveSource", BinPropertyType.UInt32)]
        public uint ErosionDriveSource { get; set; } = 0;
    }
    [MetaClass("TooltipInstanceBuff")]
    public class TooltipInstanceBuff : TooltipInstance
    {
        [MetaProperty("mEnableExtendedTooltip", BinPropertyType.Bool)]
        public bool EnableExtendedTooltip { get; set; } = false;
    }
    [MetaClass("ItemRecommendationChoices")]
    public class ItemRecommendationChoices : IMetaClass
    {
        [MetaProperty("mChoices", BinPropertyType.Container)]
        public MetaContainer<uint> Choices { get; set; } = new();
    }
    [MetaClass("EVOSettings")]
    public class EVOSettings : IMetaClass
    {
        [MetaProperty("mEnableAnnouncerVOReplacement", BinPropertyType.Bool)]
        public bool EnableAnnouncerVOReplacement { get; set; } = true;
        [MetaProperty("mEnableChatVO", BinPropertyType.Bool)]
        public bool EnableChatVO { get; set; } = true;
        [MetaProperty("mPingVOThrottleThreshold", BinPropertyType.Float)]
        public float PingVOThrottleThreshold { get; set; } = 3f;
        [MetaProperty("mChatVOThrottleCounterThreshold", BinPropertyType.Int32)]
        public int ChatVOThrottleCounterThreshold { get; set; } = 3;
        [MetaProperty("mChatVOThrottleCounterDecayTime", BinPropertyType.Float)]
        public float ChatVOThrottleCounterDecayTime { get; set; } = 3f;
    }
    [MetaClass("StatByNamedDataValueCalculationPart")]
    public class StatByNamedDataValueCalculationPart : IGameCalculationPartWithStats
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte Stat { get; set; } = 0;
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte StatFormula { get; set; } = 0;
        [MetaProperty("mDataValue", BinPropertyType.Hash)]
        public MetaHash DataValue { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionMoveDistance")]
    public class ContextualConditionMoveDistance : IContextualCondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 0;
        [MetaProperty("mDistance", BinPropertyType.Float)]
        public float Distance { get; set; } = 0f;
    }
    [MetaClass("SpawningUIDefinition")]
    public class SpawningUIDefinition : IMetaClass
    {
        [MetaProperty("buffNameFilter", BinPropertyType.String)]
        public string BuffNameFilter { get; set; } = "";
        [MetaProperty("maxNumberOfUnits", BinPropertyType.Int32)]
        public int MaxNumberOfUnits { get; set; } = 0;
    }
    [MetaClass("FloatGet")]
    public class FloatGet : IFloatGet
    {
        [MetaProperty("value", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
    }
    [MetaClass("ContextualConditionSpellName")]
    public class ContextualConditionSpellName : IContextualConditionSpell
    {
        [MetaProperty("mSpell", BinPropertyType.Hash)]
        public MetaHash Spell { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionCharacterIsCastingRecall")]
    public class ContextualConditionCharacterIsCastingRecall : ICharacterSubcondition
    {
    }
    [MetaClass("ShaderStaticSwitch")]
    public class ShaderStaticSwitch : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("onByDefault", BinPropertyType.Bool)]
        public bool OnByDefault { get; set; } = false;
    }
    [MetaClass("StaticMaterialTechniqueDef")]
    public class StaticMaterialTechniqueDef : IMetaClass
    {
        [MetaProperty("passes", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialPassDef>> Passes { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
    }
    [MetaClass("SameTeamCastRequirement")]
    public class SameTeamCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
    }
    [MetaClass("ScriptDataObject")]
    public class ScriptDataObject : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mConstants", BinPropertyType.Map)]
        public Dictionary<string, GameModeConstant> Constants { get; set; } = new();
        [MetaProperty(2615371617, BinPropertyType.ObjectLink)]
        public MetaObjectLink m2615371617 { get; set; } = new(0);
    }
    [MetaClass("SkinMeshDataProperties")]
    public class SkinMeshDataProperties : IMetaClass
    {
        [MetaProperty("reflectionOpacityDirect", BinPropertyType.Float)]
        public float ReflectionOpacityDirect { get; set; } = 0f;
        [MetaProperty("reflectionFresnel", BinPropertyType.Float)]
        public float ReflectionFresnel { get; set; } = 1f;
        [MetaProperty("allowCharacterInking", BinPropertyType.Bool)]
        public bool AllowCharacterInking { get; set; } = true;
        [MetaProperty("reflectionOpacityGlancing", BinPropertyType.Float)]
        public float ReflectionOpacityGlancing { get; set; } = 1f;
        [MetaProperty("materialOverride", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinMeshDataProperties_MaterialOverride>> MaterialOverride { get; set; } = new();
        [MetaProperty("selfIllumination", BinPropertyType.Float)]
        public float SelfIllumination { get; set; } = 0f;
        [MetaProperty(625166346, BinPropertyType.Bool)]
        public bool m625166346 { get; set; } = false;
        [MetaProperty("submeshRenderOrder", BinPropertyType.String)]
        public string SubmeshRenderOrder { get; set; } = "";
        [MetaProperty("initialSubmeshMouseOversToHide", BinPropertyType.String)]
        public string InitialSubmeshMouseOversToHide { get; set; } = "";
        [MetaProperty("fresnelColor", BinPropertyType.Color)]
        public Color FresnelColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("texture", BinPropertyType.String)]
        public string Texture { get; set; } = "";
        [MetaProperty("boundingCylinderRadius", BinPropertyType.Float)]
        public float BoundingCylinderRadius { get; set; } = 50f;
        [MetaProperty("reflectionFresnelColor", BinPropertyType.Color)]
        public Color ReflectionFresnelColor { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("overrideBoundingBox", BinPropertyType.Optional)]
        public MetaOptional<Vector3> OverrideBoundingBox { get; set; } = new MetaOptional<Vector3>(default(Vector3), false);
        [MetaProperty("boundingCylinderHeight", BinPropertyType.Float)]
        public float BoundingCylinderHeight { get; set; } = 150f;
        [MetaProperty("materialController", BinPropertyType.Structure)]
        public SkinnedMeshDataMaterialController MaterialController { get; set; } = null;
        [MetaProperty("initialSubmeshToHide", BinPropertyType.String)]
        public string InitialSubmeshToHide { get; set; } = "";
        [MetaProperty("fresnel", BinPropertyType.Float)]
        public float Fresnel { get; set; } = 0f;
        [MetaProperty("boundingSphereRadius", BinPropertyType.Optional)]
        public MetaOptional<float> BoundingSphereRadius { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("emissiveTexture", BinPropertyType.String)]
        public string EmissiveTexture { get; set; } = "";
        [MetaProperty("skinScale", BinPropertyType.Float)]
        public float SkinScale { get; set; } = 1f;
        [MetaProperty("castShadows", BinPropertyType.Bool)]
        public bool CastShadows { get; set; } = true;
        [MetaProperty("skeleton", BinPropertyType.String)]
        public string Skeleton { get; set; } = "";
        [MetaProperty("brushAlphaOverride", BinPropertyType.Float)]
        public float BrushAlphaOverride { get; set; } = 0.25f;
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink Material { get; set; } = new(0);
        [MetaProperty(3593334908, BinPropertyType.Bool)]
        public bool m3593334908 { get; set; } = false;
        [MetaProperty("simpleSkin", BinPropertyType.String)]
        public string SimpleSkin { get; set; } = "";
        [MetaProperty("reflectionMap", BinPropertyType.String)]
        public string ReflectionMap { get; set; } = "";
        [MetaProperty("glossTexture", BinPropertyType.String)]
        public string GlossTexture { get; set; } = "";
        [MetaProperty("usesSkinVO", BinPropertyType.Bool)]
        public bool UsesSkinVO { get; set; } = false;
        [MetaProperty("rigPoseModifierData", BinPropertyType.Container)]
        public MetaContainer<BaseRigPoseModifierData> RigPoseModifierData { get; set; } = new();
        [MetaProperty("initialSubmeshShadowsToHide", BinPropertyType.String)]
        public string InitialSubmeshShadowsToHide { get; set; } = "";
    }
    [MetaClass("ContextualConditionOwnerTeamNetChampionKills")]
    public class ContextualConditionOwnerTeamNetChampionKills : IContextualCondition
    {
        [MetaProperty("mTimeFrameSeconds", BinPropertyType.Float)]
        public float TimeFrameSeconds { get; set; } = 0f;
        [MetaProperty("mOwnerTeamNetKillAdvantage", BinPropertyType.Int32)]
        public int OwnerTeamNetKillAdvantage { get; set; } = 0;
        [MetaProperty("mKillAdvantageCompareOp", BinPropertyType.Byte)]
        public byte KillAdvantageCompareOp { get; set; } = 3;
    }
    [MetaClass("FxTable")]
    public class FxTable : IMetaClass
    {
        [MetaProperty("Entries", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FxTableEntry>> Entries { get; set; } = new();
    }
    [MetaClass(1643693084)]
    public class Class0x61f8c41c : IMetaClass
    {
        [MetaProperty(112193307, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemDataValue>> m112193307 { get; set; } = new();
    }
    [MetaClass("HudDamageDisplayData")]
    public class HudDamageDisplayData : IMetaClass
    {
        [MetaProperty(3754862555, BinPropertyType.Float)]
        public float m3754862555 { get; set; } = 0f;
    }
    [MetaClass("TargeterDefinitionWall")]
    public class TargeterDefinitionWall : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("wallOrientation", BinPropertyType.UInt32)]
        public uint WallOrientation { get; set; } = 3;
        [MetaProperty("thickness", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> Thickness { get; set; } = new (new ());
        [MetaProperty("wallRotation", BinPropertyType.Float)]
        public float WallRotation { get; set; } = 0f;
        [MetaProperty("length", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> Length { get; set; } = new (new ());
        [MetaProperty("textureWallOverrideName", BinPropertyType.String)]
        public string TextureWallOverrideName { get; set; } = "";
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; } = new (new ());
    }
    [MetaClass("HybridMaterialDefPreset")]
    public class HybridMaterialDefPreset : IMetaClass
    {
        [MetaProperty("srcColorBlendFactor", BinPropertyType.Byte)]
        public byte SrcColorBlendFactor { get; set; } = 1;
        [MetaProperty("blendEnable", BinPropertyType.Bool)]
        public bool BlendEnable { get; set; } = false;
        [MetaProperty("depthOffsetSlope", BinPropertyType.Float)]
        public float DepthOffsetSlope { get; set; } = 0f;
        [MetaProperty("stencilEnable", BinPropertyType.Bool)]
        public bool StencilEnable { get; set; } = false;
        [MetaProperty("cullEnable", BinPropertyType.Bool)]
        public bool CullEnable { get; set; } = true;
        [MetaProperty("windingToCull", BinPropertyType.Byte)]
        public byte WindingToCull { get; set; } = 1;
        [MetaProperty("stencilPassDepthPassOp", BinPropertyType.Byte)]
        public byte StencilPassDepthPassOp { get; set; } = 0;
        [MetaProperty("dstAlphaBlendFactor", BinPropertyType.Byte)]
        public byte DstAlphaBlendFactor { get; set; } = 0;
        [MetaProperty("blendEquation", BinPropertyType.Byte)]
        public byte BlendEquation { get; set; } = 0;
        [MetaProperty("stencilCompareFunc", BinPropertyType.Byte)]
        public byte StencilCompareFunc { get; set; } = 1;
        [MetaProperty("srcAlphaBlendFactor", BinPropertyType.Byte)]
        public byte SrcAlphaBlendFactor { get; set; } = 1;
        [MetaProperty("depthCompareFunc", BinPropertyType.Byte)]
        public byte DepthCompareFunc { get; set; } = 3;
        [MetaProperty(2863927372, BinPropertyType.Bool)]
        public bool m2863927372 { get; set; } = false;
        [MetaProperty("stencilPassDepthFailOp", BinPropertyType.Byte)]
        public byte StencilPassDepthFailOp { get; set; } = 0;
        [MetaProperty("stencilMask", BinPropertyType.UInt32)]
        public uint StencilMask { get; set; } = 0;
        [MetaProperty("writeMask", BinPropertyType.Byte)]
        public byte WriteMask { get; set; } = 31;
        [MetaProperty("dstColorBlendFactor", BinPropertyType.Byte)]
        public byte DstColorBlendFactor { get; set; } = 0;
        [MetaProperty("stencilReferenceVal", BinPropertyType.UInt32)]
        public uint StencilReferenceVal { get; set; } = 0;
        [MetaProperty("depthEnable", BinPropertyType.Bool)]
        public bool DepthEnable { get; set; } = true;
        [MetaProperty("stencilFailOp", BinPropertyType.Byte)]
        public byte StencilFailOp { get; set; } = 0;
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; } = new();
        [MetaProperty("depthOffsetBias", BinPropertyType.Float)]
        public float DepthOffsetBias { get; set; } = 0f;
    }
    [MetaClass("GenericSplineMovementSpec")]
    public interface GenericSplineMovementSpec : MissileMovementSpec
    {
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        bool UseMissilePositionAsOrigin { get; set; }
        [MetaProperty("mSplineInfo", BinPropertyType.Structure)]
        ISplineInfo SplineInfo { get; set; }
    }
    [MetaClass("TftEffectAmount")]
    public class TftEffectAmount : IMetaClass
    {
        [MetaProperty("formatString", BinPropertyType.String)]
        public string FormatString { get; set; } = "";
        [MetaProperty("value", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
        [MetaProperty("name", BinPropertyType.Hash)]
        public MetaHash Name { get; set; } = new(0);
    }
    [MetaClass("ItemRecommendationOverride")]
    public class ItemRecommendationOverride : IMetaClass
    {
        [MetaProperty("mStartingItemSets", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationOverrideStartingItemSet>> StartingItemSets { get; set; } = new();
        [MetaProperty("mCoreItems", BinPropertyType.Container)]
        public MetaContainer<MetaHash> CoreItems { get; set; } = new();
        [MetaProperty("mRecItemRanges", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x5a3bc52d>> RecItemRanges { get; set; } = new();
        [MetaProperty("mRecommendedItems", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationCondition>> RecommendedItems { get; set; } = new();
        [MetaProperty("mForceOverride", BinPropertyType.Bool)]
        public bool ForceOverride { get; set; } = false;
        [MetaProperty("mOverrideContexts", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationOverrideContext>> OverrideContexts { get; set; } = new();
    }
    [MetaClass("FixedSpeedMovement")]
    public class FixedSpeedMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool ProjectTargetToCastRange { get; set; } = false;
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float Speed { get; set; } = 0f;
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool UseGroundHeightAtTarget { get; set; } = true;
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool InferDirectionFromFacingIfNeeded { get; set; } = true;
    }
    [MetaClass("TempTable3Table")]
    public class TempTable3Table : ScriptTable
    {
    }
    [MetaClass("LootItemDetails")]
    public class LootItemDetails : IMetaClass
    {
        [MetaProperty("mRarity", BinPropertyType.UInt32)]
        public uint Rarity { get; set; } = 0;
        [MetaProperty("mValue", BinPropertyType.UInt32)]
        public uint Value { get; set; } = 0;
        [MetaProperty("mStoreId", BinPropertyType.UInt32)]
        public uint StoreId { get; set; } = 0;
    }
    [MetaClass("VfxFieldNoiseDefinitionData")]
    public class VfxFieldNoiseDefinitionData : IMetaClass
    {
        [MetaProperty("frequency", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Frequency { get; set; } = new (new ());
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Position { get; set; } = new (new ());
        [MetaProperty("velocityDelta", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> VelocityDelta { get; set; } = new (new ());
        [MetaProperty("axisFraction", BinPropertyType.Vector3)]
        public Vector3 AxisFraction { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("radius", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Radius { get; set; } = new (new ());
    }
    [MetaClass("ContextualActionPlayAudio")]
    public interface ContextualActionPlayAudio : IContextualAction
    {
        [MetaProperty("mWaitForAnnouncerQueue", BinPropertyType.Bool)]
        bool WaitForAnnouncerQueue { get; set; }
        [MetaProperty("mWaitTimeout", BinPropertyType.Float)]
        float WaitTimeout { get; set; }
        [MetaProperty("mEnemyEventName", BinPropertyType.String)]
        string EnemyEventName { get; set; }
        [MetaProperty(1422745546, BinPropertyType.Bool)]
        bool m1422745546 { get; set; }
        [MetaProperty(1721877131, BinPropertyType.String)]
        string m1721877131 { get; set; }
        [MetaProperty("mAllyEventName", BinPropertyType.String)]
        string AllyEventName { get; set; }
        [MetaProperty(3199620533, BinPropertyType.Bool)]
        bool m3199620533 { get; set; }
        [MetaProperty("mSelfEventName", BinPropertyType.String)]
        string SelfEventName { get; set; }
        [MetaProperty("mSpectatorEventName", BinPropertyType.String)]
        string SpectatorEventName { get; set; }
    }
    [MetaClass("GameModeConstant")]
    public interface GameModeConstant : IMetaClass
    {
    }
    [MetaClass("ConditionFloatPairData")]
    public class ConditionFloatPairData : IMetaClass
    {
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
        [MetaProperty("mHoldAnimationToLower", BinPropertyType.Float)]
        public float HoldAnimationToLower { get; set; } = 0f;
        [MetaProperty("mHoldAnimationToHigher", BinPropertyType.Float)]
        public float HoldAnimationToHigher { get; set; } = 0f;
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash ClipName { get; set; } = new(0);
    }
    [MetaClass("GameModeConstantBool")]
    public class GameModeConstantBool : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Bool)]
        public bool Value { get; set; } = false;
    }
    [MetaClass("ContextualConditionCharacterDistance")]
    public class ContextualConditionCharacterDistance : ICharacterSubcondition
    {
        [MetaProperty("mDistanceTarget", BinPropertyType.Byte)]
        public byte DistanceTarget { get; set; } = 0;
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 5;
        [MetaProperty("mDistance", BinPropertyType.Float)]
        public float Distance { get; set; } = 0f;
    }
    [MetaClass("MapContainsOtherMaps")]
    public class MapContainsOtherMaps : MapComponent
    {
        [MetaProperty("MapContainerLocations", BinPropertyType.ObjectLink)]
        public MetaObjectLink MapContainerLocations { get; set; } = new(0);
    }
    [MetaClass("Self")]
    public class Self : TargetingTypeData
    {
    }
    [MetaClass("MicroTicksPerTickData")]
    public class MicroTicksPerTickData : IMetaClass
    {
        [MetaProperty("minHealth", BinPropertyType.Float)]
        public float MinHealth { get; set; } = 1000f;
        [MetaProperty("microTicksBetween", BinPropertyType.UInt32)]
        public uint MicroTicksBetween { get; set; } = 9;
    }
    [MetaClass("SponsoredBanner")]
    public class SponsoredBanner : IMetaClass
    {
        [MetaProperty("banner", BinPropertyType.ObjectLink)]
        public MetaObjectLink Banner { get; set; } = new(0);
        [MetaProperty("SponsorTexturePath", BinPropertyType.WadEntryLink)]
        public MetaWadEntryLink SponsorTexturePath { get; set; } = new(0);
    }
    [MetaClass("MaxMaterialDriver")]
    public class MaxMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialDriver> Drivers { get; set; } = new();
    }
    [MetaClass("LoadingScreenRankedProperties")]
    public class LoadingScreenRankedProperties : IMetaClass
    {
        [MetaProperty("mDescriptor", BinPropertyType.String)]
        public string Descriptor { get; set; } = "";
        [MetaProperty("mDivision", BinPropertyType.Byte)]
        public byte Division { get; set; } = 0;
    }
    [MetaClass("TftItemComposition")]
    public class TftItemComposition : IMetaClass
    {
        [MetaProperty("mComponents", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Components { get; set; } = new();
    }
    [MetaClass("ISpellRankUpRequirement")]
    public interface ISpellRankUpRequirement : IMetaClass
    {
    }
    [MetaClass("EmblemData")]
    public class EmblemData : IMetaClass
    {
        [MetaProperty("mShowOnLoadingScreen", BinPropertyType.Bool)]
        public bool ShowOnLoadingScreen { get; set; } = true;
        [MetaProperty("mLoadingScreenScale", BinPropertyType.Float)]
        public float LoadingScreenScale { get; set; } = 0.800000011920929f;
        [MetaProperty("mImagePath", BinPropertyType.String)]
        public string ImagePath { get; set; } = "";
    }
    [MetaClass("MusicAudioDataProperties")]
    public class MusicAudioDataProperties : IMetaClass
    {
        [MetaProperty("defeatBannerSound", BinPropertyType.String)]
        public string DefeatBannerSound { get; set; } = "";
        [MetaProperty("ambientEvent", BinPropertyType.String)]
        public string AmbientEvent { get; set; } = "";
        [MetaProperty("defeatMusicID", BinPropertyType.String)]
        public string DefeatMusicID { get; set; } = "";
        [MetaProperty("legacyThemeMusicTransitionID", BinPropertyType.String)]
        public string LegacyThemeMusicTransitionID { get; set; } = "";
        [MetaProperty("victoryBannerSound", BinPropertyType.String)]
        public string VictoryBannerSound { get; set; } = "";
        [MetaProperty("themeMusicTransitionID", BinPropertyType.String)]
        public string ThemeMusicTransitionID { get; set; } = "";
        [MetaProperty("reverbPreset", BinPropertyType.String)]
        public string ReverbPreset { get; set; } = "";
        [MetaProperty("victoryMusicID", BinPropertyType.String)]
        public string VictoryMusicID { get; set; } = "";
        [MetaProperty("GameQuitEvent", BinPropertyType.String)]
        public string GameQuitEvent { get; set; } = "";
        [MetaProperty("legacyThemeMusicID", BinPropertyType.String)]
        public string LegacyThemeMusicID { get; set; } = "";
        [MetaProperty("themeMusicID", BinPropertyType.String)]
        public string ThemeMusicID { get; set; } = "";
        [MetaProperty("GameStartEvent", BinPropertyType.String)]
        public string GameStartEvent { get; set; } = "";
    }
    [MetaClass(1715297792)]
    public class Class0x663d5e00 : IGameCalculationPart
    {
        [MetaProperty("epicness", BinPropertyType.Byte)]
        public byte Epicness { get; set; } = 1;
        [MetaProperty("Coefficient", BinPropertyType.Float)]
        public float Coefficient { get; set; } = 0f;
    }
    [MetaClass("SyncGroupData")]
    public class SyncGroupData : IMetaClass
    {
        [MetaProperty("mType", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
    }
    [MetaClass("ChangeTurnRadius")]
    public class ChangeTurnRadius : MissileTriggeredActionSpec
    {
        [MetaProperty(2226849642, BinPropertyType.Container)]
        public MetaContainer<float> m2226849642 { get; set; } = new();
    }
    [MetaClass("DestroyOnHit")]
    public class DestroyOnHit : MissileBehaviorSpec
    {
    }
    [MetaClass("OptionTemplateGroup")]
    public class OptionTemplateGroup : IOptionTemplate
    {
        [MetaProperty("ExpandButtonDefinition", BinPropertyType.Hash)]
        public MetaHash ExpandButtonDefinition { get; set; } = new(0);
        [MetaProperty(2576771507, BinPropertyType.Float)]
        public float m2576771507 { get; set; } = 0.25f;
        [MetaProperty(3882223319, BinPropertyType.Hash)]
        public MetaHash m3882223319 { get; set; } = new(0);
        [MetaProperty("labelElement", BinPropertyType.Hash)]
        public MetaHash LabelElement { get; set; } = new(0);
    }
    [MetaClass("HealthbarImageInfo")]
    public class HealthbarImageInfo : IMetaClass
    {
        [MetaProperty("mOffset", BinPropertyType.Vector2)]
        public Vector2 Offset { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("mTextureUvs", BinPropertyType.Vector4)]
        public Vector4 TextureUvs { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
    }
    [MetaClass("TempTable1Table")]
    public class TempTable1Table : ScriptTable
    {
    }
    [MetaClass("MissileVisibilitySpec")]
    public interface MissileVisibilitySpec : IMetaClass
    {
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        float PerceptionBubbleRadius { get; set; }
        [MetaProperty("mTargetControlsVisibility", BinPropertyType.Bool)]
        bool TargetControlsVisibility { get; set; }
        [MetaProperty("mVisibleToOwnerTeamOnly", BinPropertyType.Bool)]
        bool VisibleToOwnerTeamOnly { get; set; }
        [MetaProperty(3143864407, BinPropertyType.Float)]
        float m3143864407 { get; set; }
    }
    [MetaClass("MaterialTextureDataCollection")]
    public class MaterialTextureDataCollection : IMetaClass
    {
        [MetaProperty("Entries", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<IdMappingEntry>> Entries { get; set; } = new();
        [MetaProperty("nextID", BinPropertyType.UInt16)]
        public ushort NextID { get; set; } = 1;
        [MetaProperty("data", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialTextureData>> Data { get; set; } = new();
        [MetaProperty(3931619090, BinPropertyType.String)]
        public string m3931619090 { get; set; } = "";
    }
    [MetaClass("VfxPrimitiveBeam")]
    public class VfxPrimitiveBeam : VfxPrimitiveBase
    {
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; } = new (new ());
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; } = new (new ());
    }
    [MetaClass("VectorGet")]
    public class VectorGet : IVectorGet
    {
        [MetaProperty("value", BinPropertyType.Vector3)]
        public Vector3 Value { get; set; } = new Vector3(0f, 0f, 0f);
    }
    [MetaClass("ValueVector3")]
    public class ValueVector3 : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Vector3)]
        public Vector3 ConstantValue { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedVector3fVariableData Dynamics { get; set; } = null;
    }
    [MetaClass("SkinUpgradeData")]
    public class SkinUpgradeData : IMetaClass
    {
        [MetaProperty("mGearSkinUpgrades", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> GearSkinUpgrades { get; set; } = new();
    }
    [MetaClass("GameModeAutoItemPurchasingConfig")]
    public class GameModeAutoItemPurchasingConfig : IMetaClass
    {
        [MetaProperty(341404937, BinPropertyType.Hash)]
        public MetaHash m341404937 { get; set; } = new(0);
        [MetaProperty(435120034, BinPropertyType.Int32)]
        public int m435120034 { get; set; } = 0;
        [MetaProperty(2213596365, BinPropertyType.Container)]
        public MetaContainer<string> m2213596365 { get; set; } = new();
        [MetaProperty(3366845884, BinPropertyType.Container)]
        public MetaContainer<string> m3366845884 { get; set; } = new();
        [MetaProperty(3813630672, BinPropertyType.Container)]
        public MetaContainer<string> m3813630672 { get; set; } = new();
    }
    [MetaClass("LolSpellScript")]
    public class LolSpellScript : RScript
    {
        [MetaProperty("PreloadData", BinPropertyType.Embedded)]
        public MetaEmbedded<LoLSpellPreloadData> PreloadData { get; set; } = new (new ());
        [MetaProperty("CustomSequences", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<ScriptSequence>> CustomSequences { get; set; } = new();
        [MetaProperty("globalProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptGlobalProperties> GlobalProperties { get; set; } = new (new ());
        [MetaProperty("sequences", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<ScriptSequence>> Sequences { get; set; } = new();
    }
    [MetaClass("ValueVector2")]
    public class ValueVector2 : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Vector2)]
        public Vector2 ConstantValue { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedVector2fVariableData Dynamics { get; set; } = null;
    }
    [MetaClass("GDSMapObjectBannerInfo")]
    public class GDSMapObjectBannerInfo : GDSMapObjectExtraInfo
    {
        [MetaProperty("BannerData", BinPropertyType.ObjectLink)]
        public MetaObjectLink BannerData { get; set; } = new(0);
    }
    [MetaClass("RegaliaLookup")]
    public class RegaliaLookup : IMetaClass
    {
        [MetaProperty("RegaliaTrim", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaTrim { get; set; } = new(0);
        [MetaProperty("regaliaCrest", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaCrest { get; set; } = new(0);
        [MetaProperty("tier", BinPropertyType.String)]
        public string Tier { get; set; } = "";
        [MetaProperty("regaliaCrown1", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaCrown1 { get; set; } = new(0);
        [MetaProperty("regaliaCrown3", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaCrown3 { get; set; } = new(0);
        [MetaProperty("regaliaCrown2", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaCrown2 { get; set; } = new(0);
        [MetaProperty("regaliaCrown4", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaCrown4 { get; set; } = new(0);
        [MetaProperty("regaliaSplit2", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaSplit2 { get; set; } = new(0);
        [MetaProperty("regaliaSplit3", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaSplit3 { get; set; } = new(0);
        [MetaProperty("regaliaSplit1", BinPropertyType.ObjectLink)]
        public MetaObjectLink RegaliaSplit1 { get; set; } = new(0);
    }
    [MetaClass("MapBakeProperties")]
    public class MapBakeProperties : MapComponent
    {
        [MetaProperty(584207002, BinPropertyType.Float)]
        public float m584207002 { get; set; } = 1f;
        [MetaProperty(792417393, BinPropertyType.Float)]
        public float m792417393 { get; set; } = 1f;
        [MetaProperty("lightGridSize", BinPropertyType.UInt32)]
        public uint LightGridSize { get; set; } = 256;
        [MetaProperty("lightGridCharacterFullBrightIntensity", BinPropertyType.Float)]
        public float LightGridCharacterFullBrightIntensity { get; set; } = 0.25f;
        [MetaProperty("lightGridFileName", BinPropertyType.String)]
        public string LightGridFileName { get; set; } = "";
        [MetaProperty(3931004104, BinPropertyType.Float)]
        public float m3931004104 { get; set; } = 5000f;
    }
    [MetaClass("VfxMeshDefinitionData")]
    public class VfxMeshDefinitionData : IMetaClass
    {
        [MetaProperty("mAnimationVariants", BinPropertyType.Container)]
        public MetaContainer<string> AnimationVariants { get; set; } = new();
        [MetaProperty("mMeshName", BinPropertyType.String)]
        public string MeshName { get; set; } = "";
        [MetaProperty("mMeshSkeletonName", BinPropertyType.String)]
        public string MeshSkeletonName { get; set; } = "";
        [MetaProperty("mSubmeshesToDrawAlways", BinPropertyType.Container)]
        public MetaContainer<MetaHash> SubmeshesToDrawAlways { get; set; } = new();
        [MetaProperty("mSimpleMeshName", BinPropertyType.String)]
        public string SimpleMeshName { get; set; } = "";
        [MetaProperty("mSubmeshesToDraw", BinPropertyType.Container)]
        public MetaContainer<MetaHash> SubmeshesToDraw { get; set; } = new();
        [MetaProperty("mLockMeshToAttachment", BinPropertyType.Bool)]
        public bool LockMeshToAttachment { get; set; } = false;
        [MetaProperty("mAnimationName", BinPropertyType.String)]
        public string AnimationName { get; set; } = "";
    }
    [MetaClass("OptionItemSecondaryHotkeys2Column")]
    public class OptionItemSecondaryHotkeys2Column : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("Rows", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x430dd10d>> Rows { get; set; } = new();
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("Header", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xea321356> Header { get; set; } = new (new ());
    }
    [MetaClass("IdentityInstance")]
    public class IdentityInstance : IMetaClass
    {
        [MetaProperty("mItemTexturePath", BinPropertyType.String)]
        public string ItemTexturePath { get; set; } = "";
    }
    [MetaClass("HasBuffRequirement")]
    public class HasBuffRequirement : ISpellRankUpRequirement
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash BuffName { get; set; } = new(0);
        [MetaProperty("mFromAnyone", BinPropertyType.Bool)]
        public bool FromAnyone { get; set; } = false;
    }
    [MetaClass("AttackSlotData")]
    public class AttackSlotData : IMetaClass
    {
        [MetaProperty("mAttackName", BinPropertyType.Optional)]
        public MetaOptional<string> AttackName { get; set; } = new MetaOptional<string>(default(string), false);
        [MetaProperty("mOverrideAutoattackCastTime", BinPropertyType.Structure)]
        public OverrideAutoAttackCastTimeData OverrideAutoattackCastTime { get; set; } = null;
        [MetaProperty("mAttackCastTime", BinPropertyType.Optional)]
        public MetaOptional<float> AttackCastTime { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mAttackProbability", BinPropertyType.Optional)]
        public MetaOptional<float> AttackProbability { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mAttackTotalTime", BinPropertyType.Optional)]
        public MetaOptional<float> AttackTotalTime { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mAttackDelayCastOffsetPercentAttackSpeedRatio", BinPropertyType.Optional)]
        public MetaOptional<float> AttackDelayCastOffsetPercentAttackSpeedRatio { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mAttackDelayCastOffsetPercent", BinPropertyType.Optional)]
        public MetaOptional<float> AttackDelayCastOffsetPercent { get; set; } = new MetaOptional<float>(default(float), false);
    }
    [MetaClass("AndScriptCondition")]
    public class AndScriptCondition : IScriptCondition
    {
        [MetaProperty("Conditions", BinPropertyType.Container)]
        public MetaContainer<IScriptCondition> Conditions { get; set; } = new();
    }
    [MetaClass("MissileMovementSpec")]
    public interface MissileMovementSpec : IMetaClass
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        bool TracksTarget { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        float TargetHeightAugment { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        string TargetBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        float StartDelay { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        bool m2856647070 { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        string StartBoneName { get; set; }
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        bool UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        float OffsetInitialTargetHeight { get; set; }
    }
    [MetaClass("SpellDataValueVector")]
    public class SpellDataValueVector : IMetaClass
    {
        [MetaProperty("SpellDataValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellDataValue>> SpellDataValues { get; set; } = new();
    }
    [MetaClass("MinimapPingEffectDefinition")]
    public class MinimapPingEffectDefinition : IMetaClass
    {
        [MetaProperty("startDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty("repeatCount", BinPropertyType.UInt32)]
        public uint RepeatCount { get; set; } = 0;
        [MetaProperty("scaleSpeed", BinPropertyType.Float)]
        public float ScaleSpeed { get; set; } = 0f;
        [MetaProperty("scaleStart", BinPropertyType.Float)]
        public float ScaleStart { get; set; } = 1f;
        [MetaProperty("blendWithAlpha", BinPropertyType.Bool)]
        public bool BlendWithAlpha { get; set; } = true;
        [MetaProperty("alphaStart", BinPropertyType.UInt32)]
        public uint AlphaStart { get; set; } = 255;
        [MetaProperty("onDeathFadeSpeed", BinPropertyType.Float)]
        public float OnDeathFadeSpeed { get; set; } = 180f;
        [MetaProperty("life", BinPropertyType.Float)]
        public float Life { get; set; } = 0f;
        [MetaProperty("alphaFadeSpeed", BinPropertyType.Float)]
        public float AlphaFadeSpeed { get; set; } = 0f;
    }
    [MetaClass("IntegratedValueVector2")]
    public class IntegratedValueVector2 : ValueVector2
    {
    }
    [MetaClass("SHData")]
    public class SHData : IMetaClass
    {
        [MetaProperty("bandData", BinPropertyType.Container)]
        public MetaContainer<Vector3> BandData { get; set; } = new();
    }
    [MetaClass("InvalidDeviceViewController")]
    public class InvalidDeviceViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
    }
    [MetaClass("UIButtonAdditionalState")]
    public class UIButtonAdditionalState : IMetaClass
    {
        [MetaProperty("displayElements", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> DisplayElements { get; set; } = new();
    }
    [MetaClass("UIButtonDefinition")]
    public class UIButtonDefinition : IMetaClass
    {
        [MetaProperty("defaultStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> DefaultStateElements { get; set; } = new (new ());
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash ObjectPath { get; set; } = new(0);
        [MetaProperty("selectedStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> SelectedStateElements { get; set; } = new (new ());
        [MetaProperty("InactiveTooltipTraKey", BinPropertyType.String)]
        public string InactiveTooltipTraKey { get; set; } = "";
        [MetaProperty("SelectedClickedStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> SelectedClickedStateElements { get; set; } = new (new ());
        [MetaProperty("hitRegionElement", BinPropertyType.ObjectLink)]
        public MetaObjectLink HitRegionElement { get; set; } = new(0);
        [MetaProperty("InactiveStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> InactiveStateElements { get; set; } = new (new ());
        [MetaProperty("ClickReleaseParticleElement", BinPropertyType.ObjectLink)]
        public MetaObjectLink ClickReleaseParticleElement { get; set; } = new(0);
        [MetaProperty("ActiveTooltipTraKey", BinPropertyType.String)]
        public string ActiveTooltipTraKey { get; set; } = "";
        [MetaProperty("soundEvents", BinPropertyType.Structure)]
        public Class0x209b0277 SoundEvents { get; set; } = null;
        [MetaProperty(2903476354, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m2903476354 { get; set; } = new (new ());
        [MetaProperty("SelectedHoverStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> SelectedHoverStateElements { get; set; } = new (new ());
        [MetaProperty("hoverStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> HoverStateElements { get; set; } = new (new ());
    }
    [MetaClass("TFTMapCharacterLists")]
    public class TFTMapCharacterLists : IMetaClass
    {
        [MetaProperty("MapName", BinPropertyType.String)]
        public string MapName { get; set; } = "";
        [MetaProperty("characterLists", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftMapCharacterList>> CharacterLists { get; set; } = new();
    }
    [MetaClass("IntegratedValueVector3")]
    public class IntegratedValueVector3 : ValueVector3
    {
    }
    [MetaClass(1826338589)]
    public class Class0x6cdbb71d : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty("pathname", BinPropertyType.String)]
        public string Pathname { get; set; } = "";
        [MetaProperty("duration", BinPropertyType.Float)]
        public float Duration { get; set; } = 10f;
        [MetaProperty("velocity", BinPropertyType.Float)]
        public float Velocity { get; set; } = 100f;
        [MetaProperty(987642461, BinPropertyType.Bool)]
        public bool m987642461 { get; set; } = false;
        [MetaProperty("targetName", BinPropertyType.String)]
        public string TargetName { get; set; } = "";
        [MetaProperty(4009466127, BinPropertyType.Bool)]
        public bool m4009466127 { get; set; } = true;
    }
    [MetaClass(1827239481)]
    public class Class0x6ce97639 : Class0xd1951f45
    {
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float TransitionTime { get; set; } = 0.10000000149011612f;
        [MetaProperty("endAlpha", BinPropertyType.Byte)]
        public byte EndAlpha { get; set; } = 255;
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        public byte EasingType { get; set; } = 0;
        [MetaProperty("startAlpha", BinPropertyType.Byte)]
        public byte StartAlpha { get; set; } = 0;
        [MetaProperty("Edge", BinPropertyType.Byte)]
        public byte Edge { get; set; } = 0;
    }
    [MetaClass("LoginViewController")]
    public class LoginViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
    }
    [MetaClass("SelectorPairData")]
    public class SelectorPairData : IMetaClass
    {
        [MetaProperty("mProbability", BinPropertyType.Float)]
        public float Probability { get; set; } = 0f;
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash ClipName { get; set; } = new(0);
    }
    [MetaClass("TFTHudStageData")]
    public class TFTHudStageData : IMetaClass
    {
        [MetaProperty(2702329528, BinPropertyType.Float)]
        public float m2702329528 { get; set; } = 5f;
    }
    [MetaClass("TFTHudScoreboardData")]
    public class TFTHudScoreboardData : IMetaClass
    {
        [MetaProperty(625770807, BinPropertyType.Float)]
        public float m625770807 { get; set; } = 1f;
        [MetaProperty(2215596130, BinPropertyType.Float)]
        public float m2215596130 { get; set; } = 1f;
        [MetaProperty(4110713210, BinPropertyType.Float)]
        public float m4110713210 { get; set; } = 0.5f;
    }
    [MetaClass("BuffStackingSettings")]
    public class BuffStackingSettings : IMetaClass
    {
        [MetaProperty("templateDefinition", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BuffStackingTemplate>> TemplateDefinition { get; set; } = new();
    }
    [MetaClass("RegionElementData")]
    public class RegionElementData : BaseElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
    }
    [MetaClass("MapPathCurveSegment")]
    public class MapPathCurveSegment : MapPathSegment
    {
        [MetaProperty("EndPosition", BinPropertyType.Vector3)]
        public Vector3 EndPosition { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("ControlPoint1", BinPropertyType.Vector3)]
        public Vector3 ControlPoint1 { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("ControlPoint2", BinPropertyType.Vector3)]
        public Vector3 ControlPoint2 { get; set; } = new Vector3(0f, 0f, 0f);
    }
    [MetaClass("DelayStart")]
    public class DelayStart : MissileBehaviorSpec
    {
        [MetaProperty("mDelayTime", BinPropertyType.Float)]
        public float DelayTime { get; set; } = 0f;
    }
    [MetaClass("ContextualConditionSpellLevel")]
    public class ContextualConditionSpellLevel : IContextualConditionSpell
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 0;
        [MetaProperty("mSpellLevel", BinPropertyType.Byte)]
        public byte SpellLevel { get; set; } = 0;
    }
    [MetaClass("TftTraitData")]
    public class TftTraitData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mDescriptionNameTra", BinPropertyType.String)]
        public string DescriptionNameTra { get; set; } = "";
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string IconPath { get; set; } = "";
        [MetaProperty("mTraitSets", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTTraitSetData>> TraitSets { get; set; } = new();
        [MetaProperty("mDisplayNameIcon", BinPropertyType.String)]
        public string DisplayNameIcon { get; set; } = "";
        [MetaProperty("mDisplayNameTra", BinPropertyType.String)]
        public string DisplayNameTra { get; set; } = "";
        [MetaProperty("mUnitSectionTra", BinPropertyType.String)]
        public string UnitSectionTra { get; set; } = "";
    }
    [MetaClass("FxActionMoveReset")]
    public class FxActionMoveReset : FxActionMoveBase
    {
        [MetaProperty("OvershootDistance", BinPropertyType.Float)]
        public float OvershootDistance { get; set; } = 0f;
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        public byte EasingType { get; set; } = 0;
        [MetaProperty("FaceVelocity", BinPropertyType.Bool)]
        public bool FaceVelocity { get; set; } = true;
        [MetaProperty("TargetObject", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTarget> TargetObject { get; set; } = new (new ());
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
    }
    [MetaClass("MaterialDataCollections")]
    public class MaterialDataCollections : IMetaClass
    {
        [MetaProperty("ParameterData", BinPropertyType.Embedded)]
        public MetaEmbedded<MaterialParameterDataCollection> ParameterData { get; set; } = new (new ());
        [MetaProperty("switchData", BinPropertyType.Embedded)]
        public MetaEmbedded<MaterialSwitchDataCollection> SwitchData { get; set; } = new (new ());
        [MetaProperty("TextureData", BinPropertyType.Embedded)]
        public MetaEmbedded<MaterialTextureDataCollection> TextureData { get; set; } = new (new ());
    }
    [MetaClass("SwapChampionCheat")]
    public class SwapChampionCheat : Cheat
    {
    }
    [MetaClass("HudStatStoneDeathRecapData")]
    public class HudStatStoneDeathRecapData : IMetaClass
    {
        [MetaProperty("MinDisplayTime", BinPropertyType.Float)]
        public float MinDisplayTime { get; set; } = 5f;
        [MetaProperty("MaxDisplayTime", BinPropertyType.Float)]
        public float MaxDisplayTime { get; set; } = 15f;
        [MetaProperty("DeathRecapTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> DeathRecapTransitionOut { get; set; } = new (new ());
        [MetaProperty("DetailsTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> DetailsTransitionData { get; set; } = new (new ());
        [MetaProperty("DeathRecapTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> DeathRecapTransitionIn { get; set; } = new (new ());
    }
    [MetaClass("TableValueExistsScriptCondition")]
    public class TableValueExistsScriptCondition : IScriptCondition
    {
        [MetaProperty("TableValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableGet> TableValue { get; set; } = new (new ());
    }
    [MetaClass("GameCalculationModified")]
    public class GameCalculationModified : IGameCalculation
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        public IGameCalculationPart Multiplier { get; set; } = null;
        [MetaProperty(923208333, BinPropertyType.Byte)]
        public byte m923208333 { get; set; } = 8;
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        public byte m3419063832 { get; set; } = 8;
        [MetaProperty("tooltipOnly", BinPropertyType.Bool)]
        public bool TooltipOnly { get; set; } = false;
        [MetaProperty(3874405167, BinPropertyType.Byte)]
        public byte m3874405167 { get; set; } = 8;
        [MetaProperty("mModifiedGameCalculation", BinPropertyType.Hash)]
        public MetaHash ModifiedGameCalculation { get; set; } = new(0);
        [MetaProperty("mOverrideSpellLevel", BinPropertyType.Optional)]
        public MetaOptional<int> OverrideSpellLevel { get; set; } = new MetaOptional<int>(default(int), false);
    }
    [MetaClass("FloatLiteralMaterialDriver")]
    public class FloatLiteralMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
    }
    [MetaClass("ContextualConditionCustomTimer")]
    public class ContextualConditionCustomTimer : IContextualCondition
    {
        [MetaProperty("mCustomTimer", BinPropertyType.Float)]
        public float CustomTimer { get; set; } = 0f;
    }
    [MetaClass("AtlasData")]
    public class AtlasData : AtlasDataBase
    {
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
        [MetaProperty("mTextureUV", BinPropertyType.Vector4)]
        public Vector4 TextureUV { get; set; } = new Vector4(0f, 0f, 1f, 1f);
    }
    [MetaClass("GlobalPerLevelStatsFactor")]
    public class GlobalPerLevelStatsFactor : IMetaClass
    {
        [MetaProperty("mPerLevelStatsFactor", BinPropertyType.Container)]
        public MetaContainer<float> PerLevelStatsFactor { get; set; } = new();
    }
    [MetaClass("EffectAnimationElementData")]
    public class EffectAnimationElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("mLooping", BinPropertyType.Bool)]
        public bool Looping { get; set; } = false;
        [MetaProperty("NumberOfFramesPerRowInAtlas", BinPropertyType.Float)]
        public float NumberOfFramesPerRowInAtlas { get; set; } = 0f;
        [MetaProperty("TotalNumberOfFrames", BinPropertyType.Float)]
        public float TotalNumberOfFrames { get; set; } = 0f;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFinishBehavior", BinPropertyType.Byte)]
        public byte FinishBehavior { get; set; } = 0;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
        [MetaProperty("FramesPerSecond", BinPropertyType.Float)]
        public float FramesPerSecond { get; set; } = 0f;
    }
    [MetaClass("HasGearDynamicMaterialBoolDriver")]
    public class HasGearDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mGearIndex", BinPropertyType.Byte)]
        public byte GearIndex { get; set; } = 0;
    }
    [MetaClass("BaseResourceResolver")]
    public class BaseResourceResolver : IResourceResolver
    {
        [MetaProperty("resourceMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> ResourceMap { get; set; } = new();
    }
    [MetaClass("MissionAsset")]
    public class MissionAsset : IMetaClass
    {
        [MetaProperty("mIconTexturePath", BinPropertyType.String)]
        public string IconTexturePath { get; set; } = "";
        [MetaProperty("mInternalName", BinPropertyType.String)]
        public string InternalName { get; set; } = "";
        [MetaProperty(4163829446, BinPropertyType.Bool)]
        public bool m4163829446 { get; set; } = false;
    }
    [MetaClass("TargeterDefinitionMultiAOE")]
    public class TargeterDefinitionMultiAOE : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("angelOffsetRadian", BinPropertyType.Float)]
        public float AngelOffsetRadian { get; set; } = 0f;
        [MetaProperty("overrideMinCastRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideMinCastRange { get; set; } = new (new ());
        [MetaProperty("innerTextureName", BinPropertyType.String)]
        public string InnerTextureName { get; set; } = "";
        [MetaProperty("rightTextureName", BinPropertyType.String)]
        public string RightTextureName { get; set; } = "";
        [MetaProperty("leftTextureName", BinPropertyType.String)]
        public string LeftTextureName { get; set; } = "";
        [MetaProperty("overrideAOERadius", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideAOERadius { get; set; } = new (new ());
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; } = new (new ());
        [MetaProperty("overrideMaxCastRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideMaxCastRange { get; set; } = new (new ());
        [MetaProperty("numOfInnerAOE", BinPropertyType.UInt32)]
        public uint NumOfInnerAOE { get; set; } = 0;
    }
    [MetaClass("ContextualConditionShopOpenCount")]
    public class ContextualConditionShopOpenCount : IContextualCondition
    {
        [MetaProperty("mShopOpenCount", BinPropertyType.UInt32)]
        public uint ShopOpenCount { get; set; } = 0;
    }
    [MetaClass("StaticMaterialChildTechniqueDef")]
    public class StaticMaterialChildTechniqueDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("parentName", BinPropertyType.String)]
        public string ParentName { get; set; } = "";
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; } = new();
    }
    [MetaClass("IHudLoadingScreenWidget")]
    public interface IHudLoadingScreenWidget : IMetaClass
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        string SceneName { get; set; }
    }
    [MetaClass("DynamicMaterialParameterDef")]
    public class DynamicMaterialParameterDef : IMetaClass
    {
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialDriver Driver { get; set; } = null;
    }
    [MetaClass("SummonerEmote")]
    public class SummonerEmote : IMetaClass
    {
        [MetaProperty("vfxSystem", BinPropertyType.ObjectLink)]
        public MetaObjectLink VfxSystem { get; set; } = new(0);
        [MetaProperty("announcementIcon", BinPropertyType.String)]
        public string AnnouncementIcon { get; set; } = "";
        [MetaProperty("selectionIcon", BinPropertyType.String)]
        public string SelectionIcon { get; set; } = "";
        [MetaProperty("renderScale", BinPropertyType.Float)]
        public float RenderScale { get; set; } = 1f;
        [MetaProperty("summonerEmoteId", BinPropertyType.UInt32)]
        public uint SummonerEmoteId { get; set; } = 0;
        [MetaProperty("visibleSelectionName", BinPropertyType.String)]
        public string VisibleSelectionName { get; set; } = "";
        [MetaProperty("verticalOffset", BinPropertyType.Float)]
        public float VerticalOffset { get; set; } = -100f;
    }
    [MetaClass("ClashLogo")]
    public class ClashLogo : IMetaClass
    {
        [MetaProperty("mLogoPath", BinPropertyType.String)]
        public string LogoPath { get; set; } = "";
        [MetaProperty("mClashLogoColorId", BinPropertyType.UInt32)]
        public uint ClashLogoColorId { get; set; } = 0;
        [MetaProperty("mClashLogoId", BinPropertyType.UInt32)]
        public uint ClashLogoId { get; set; } = 0;
    }
    [MetaClass("ValueColor")]
    public class ValueColor : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Vector4)]
        public Vector4 ConstantValue { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedColorVariableData Dynamics { get; set; } = null;
    }
    [MetaClass("TFTUnitUpgradeData")]
    public class TFTUnitUpgradeData : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(3604921790, BinPropertyType.Byte)]
        public byte m3604921790 { get; set; } = 1;
    }
    [MetaClass("MaterialInstanceDynamicParam")]
    public class MaterialInstanceDynamicParam : IMetaClass
    {
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialDriver Driver { get; set; } = null;
    }
    [MetaClass(1965398739)]
    public class Class0x75259ad3 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("CloseButtonDefinition", BinPropertyType.Hash)]
        public MetaHash CloseButtonDefinition { get; set; } = new(0);
        [MetaProperty("CancelButtonDefinition", BinPropertyType.Hash)]
        public MetaHash CancelButtonDefinition { get; set; } = new(0);
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Structure)]
        public ViewPaneDefinition ViewPaneDefinition { get; set; } = null;
        [MetaProperty("ContentScene", BinPropertyType.Hash)]
        public MetaHash ContentScene { get; set; } = new(0);
        [MetaProperty("ConfirmButtonDefinition", BinPropertyType.Hash)]
        public MetaHash ConfirmButtonDefinition { get; set; } = new(0);
    }
    [MetaClass(1973062744)]
    public class Class0x759a8c58 : Class0x75259ad3
    {
        [MetaProperty("errorText", BinPropertyType.Hash)]
        public MetaHash ErrorText { get; set; } = new(0);
        [MetaProperty("RarityFrameCommon", BinPropertyType.Hash)]
        public MetaHash RarityFrameCommon { get; set; } = new(0);
        [MetaProperty(1106065574, BinPropertyType.Hash)]
        public MetaHash m1106065574 { get; set; } = new(0);
        [MetaProperty("RarityFrameEpic", BinPropertyType.Hash)]
        public MetaHash RarityFrameEpic { get; set; } = new(0);
        [MetaProperty("ItemFrame", BinPropertyType.Hash)]
        public MetaHash ItemFrame { get; set; } = new(0);
        [MetaProperty("upgradeText", BinPropertyType.Hash)]
        public MetaHash UpgradeText { get; set; } = new(0);
        [MetaProperty("RarityFrameLegendary", BinPropertyType.Hash)]
        public MetaHash RarityFrameLegendary { get; set; } = new(0);
        [MetaProperty(3666595123, BinPropertyType.Hash)]
        public MetaHash m3666595123 { get; set; } = new(0);
        [MetaProperty("ItemIcon", BinPropertyType.Hash)]
        public MetaHash ItemIcon { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionEnemyDeathsNearby")]
    public class ContextualConditionEnemyDeathsNearby : IContextualCondition
    {
        [MetaProperty("mEnemyDeaths", BinPropertyType.UInt32)]
        public uint EnemyDeaths { get; set; } = 0;
    }
    [MetaClass("OptionItemLabel")]
    public class OptionItemLabel : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("Label2TraKey", BinPropertyType.String)]
        public string Label2TraKey { get; set; } = "";
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("Label1TraKey", BinPropertyType.String)]
        public string Label1TraKey { get; set; } = "";
    }
    [MetaClass("AbilityResourceByCoefficientCalculationPart")]
    public class AbilityResourceByCoefficientCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float Coefficient { get; set; } = 0f;
        [MetaProperty("mAbilityResource", BinPropertyType.Byte)]
        public byte AbilityResource { get; set; } = 0;
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte StatFormula { get; set; } = 0;
    }
    [MetaClass("SpellModifier")]
    public class SpellModifier : IMetaClass
    {
        [MetaProperty(1142566944, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<RatioConversion>> m1142566944 { get; set; } = new();
        [MetaProperty(1527878389, BinPropertyType.UInt32)]
        public uint m1527878389 { get; set; } = 0;
        [MetaProperty("mModifierID", BinPropertyType.Hash)]
        public MetaHash ModifierID { get; set; } = new(0);
        [MetaProperty(2759808727, BinPropertyType.Byte)]
        public byte m2759808727 { get; set; } = 31;
        [MetaProperty(2848730102, BinPropertyType.Byte)]
        public byte m2848730102 { get; set; } = 31;
        [MetaProperty(3720257620, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x4379a5b2>> m3720257620 { get; set; } = new();
    }
    [MetaClass("LocationClamped")]
    public class LocationClamped : TargetingTypeData
    {
    }
    [MetaClass("HasNNearbyUnitsRequirement")]
    public class HasNNearbyUnitsRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mUnitsRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> UnitsRequirements { get; set; } = new();
        [MetaProperty("mDistanceType", BinPropertyType.UInt32)]
        public uint DistanceType { get; set; } = 0;
        [MetaProperty("mRange", BinPropertyType.Float)]
        public float Range { get; set; } = 0f;
        [MetaProperty("mUnitsRequired", BinPropertyType.UInt32)]
        public uint UnitsRequired { get; set; } = 0;
    }
    [MetaClass("IGameCalculationPartWithBuffCounter")]
    public interface IGameCalculationPartWithBuffCounter : IGameCalculationPart
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        MetaHash BuffName { get; set; }
        [MetaProperty("mIconKey", BinPropertyType.String)]
        string IconKey { get; set; }
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        string ScalingTagKey { get; set; }
    }
    [MetaClass("OptionItemSlider")]
    public interface OptionItemSlider : IOptionItem
    {
        [MetaProperty("TooltipTraKey", BinPropertyType.String)]
        string TooltipTraKey { get; set; }
        [MetaProperty("template", BinPropertyType.Hash)]
        MetaHash Template { get; set; }
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        string LabelTraKey { get; set; }
    }
    [MetaClass("FaceCameraEventData")]
    public class FaceCameraEventData : BaseEventData
    {
        [MetaProperty("BlendOutTime", BinPropertyType.Float)]
        public float BlendOutTime { get; set; } = 0f;
        [MetaProperty("BlendInTime", BinPropertyType.Float)]
        public float BlendInTime { get; set; } = 0f;
        [MetaProperty(3117400491, BinPropertyType.Float)]
        public float m3117400491 { get; set; } = 0f;
    }
    [MetaClass("ContextualConditionCharacterRole")]
    public class ContextualConditionCharacterRole : ICharacterSubcondition
    {
        [MetaProperty("mRole", BinPropertyType.Byte)]
        public byte Role { get; set; } = 0;
    }
    [MetaClass("AbilityResourceStateData")]
    public class AbilityResourceStateData : IMetaClass
    {
        [MetaProperty("animationSuffix", BinPropertyType.String)]
        public string AnimationSuffix { get; set; } = "";
        [MetaProperty("ColorblindPalette", BinPropertyType.Structure)]
        public AbilityResourceStateColorOptions ColorblindPalette { get; set; } = null;
        [MetaProperty("textureSuffix", BinPropertyType.String)]
        public string TextureSuffix { get; set; } = "";
        [MetaProperty("DefaultPalette", BinPropertyType.Structure)]
        public AbilityResourceStateColorOptions DefaultPalette { get; set; } = null;
    }
    [MetaClass(2002344617)]
    public class Class0x77595aa9 : IMetaClass
    {
        [MetaProperty("Health", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> Health { get; set; } = new (new ());
        [MetaProperty("MagicPenetration", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> MagicPenetration { get; set; } = new (new ());
        [MetaProperty("attackSpeed", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> AttackSpeed { get; set; } = new (new ());
        [MetaProperty("abilityPower", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> AbilityPower { get; set; } = new (new ());
        [MetaProperty("DisableStatFilters", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> DisableStatFilters { get; set; } = new (new ());
        [MetaProperty("MoveSpeed", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> MoveSpeed { get; set; } = new (new ());
        [MetaProperty("MagicResist", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> MagicResist { get; set; } = new (new ());
        [MetaProperty("ArmorPenetration", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> ArmorPenetration { get; set; } = new (new ());
        [MetaProperty(3592979039, BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> m3592979039 { get; set; } = new (new ());
        [MetaProperty("mana", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> Mana { get; set; } = new (new ());
        [MetaProperty("CriticalStrike", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> CriticalStrike { get; set; } = new (new ());
        [MetaProperty("abilityHaste", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> AbilityHaste { get; set; } = new (new ());
        [MetaProperty("OnHit", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> OnHit { get; set; } = new (new ());
        [MetaProperty("PhysicalDamage", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> PhysicalDamage { get; set; } = new (new ());
        [MetaProperty("Armor", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFilterDefinition> Armor { get; set; } = new (new ());
    }
    [MetaClass("FixedSpeedSplineMovement")]
    public class FixedSpeedSplineMovement : GenericSplineMovementSpec
    {
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        public bool UseMissilePositionAsOrigin { get; set; } = true;
        [MetaProperty("mSplineInfo", BinPropertyType.Structure)]
        public ISplineInfo SplineInfo { get; set; } = null;
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float Speed { get; set; } = 0f;
    }
    [MetaClass("CCScoreMultipliers")]
    public class CCScoreMultipliers : IMetaClass
    {
        [MetaProperty("blind", BinPropertyType.Float)]
        public float Blind { get; set; } = 0f;
        [MetaProperty("root", BinPropertyType.Float)]
        public float Root { get; set; } = 0f;
        [MetaProperty("knockup", BinPropertyType.Float)]
        public float Knockup { get; set; } = 0f;
        [MetaProperty("nearsight", BinPropertyType.Float)]
        public float Nearsight { get; set; } = 0f;
        [MetaProperty("grounded", BinPropertyType.Float)]
        public float Grounded { get; set; } = 0f;
        [MetaProperty("attackSpeedSlow", BinPropertyType.Float)]
        public float AttackSpeedSlow { get; set; } = 0f;
        [MetaProperty("charm", BinPropertyType.Float)]
        public float Charm { get; set; } = 0f;
        [MetaProperty("slow", BinPropertyType.Float)]
        public float Slow { get; set; } = 0f;
        [MetaProperty("drowsy", BinPropertyType.Float)]
        public float Drowsy { get; set; } = 0f;
        [MetaProperty("disarm", BinPropertyType.Float)]
        public float Disarm { get; set; } = 0f;
        [MetaProperty("silence", BinPropertyType.Float)]
        public float Silence { get; set; } = 0f;
        [MetaProperty("taunt", BinPropertyType.Float)]
        public float Taunt { get; set; } = 0f;
        [MetaProperty("knockback", BinPropertyType.Float)]
        public float Knockback { get; set; } = 0f;
        [MetaProperty("fear", BinPropertyType.Float)]
        public float Fear { get; set; } = 0f;
        [MetaProperty("stun", BinPropertyType.Float)]
        public float Stun { get; set; } = 0f;
        [MetaProperty("polymorph", BinPropertyType.Float)]
        public float Polymorph { get; set; } = 0f;
        [MetaProperty("flee", BinPropertyType.Float)]
        public float Flee { get; set; } = 0f;
        [MetaProperty("asleep", BinPropertyType.Float)]
        public float Asleep { get; set; } = 0f;
        [MetaProperty("suppression", BinPropertyType.Float)]
        public float Suppression { get; set; } = 0f;
    }
    [MetaClass("IdleParticlesVisibilityEventData")]
    public class IdleParticlesVisibilityEventData : BaseEventData
    {
        [MetaProperty("mShow", BinPropertyType.Bool)]
        public bool Show { get; set; } = false;
    }
    [MetaClass("VfxShape")]
    public class VfxShape : IMetaClass
    {
        [MetaProperty("emitRotationAngles", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ValueFloat>> EmitRotationAngles { get; set; } = new();
        [MetaProperty("emitRotationAxes", BinPropertyType.Container)]
        public MetaContainer<Vector3> EmitRotationAxes { get; set; } = new();
        [MetaProperty("emitOffset", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> EmitOffset { get; set; } = new (new ());
        [MetaProperty("birthTranslation", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthTranslation { get; set; } = new (new ());
    }
    [MetaClass("ToggleBarracksCheat")]
    public class ToggleBarracksCheat : Cheat
    {
        [MetaProperty("mKillExistingMinions", BinPropertyType.Bool)]
        public bool KillExistingMinions { get; set; } = false;
        [MetaProperty("mKillWards", BinPropertyType.Bool)]
        public bool KillWards { get; set; } = false;
    }
    [MetaClass("NextSpellVarsTable")]
    public class NextSpellVarsTable : ScriptTable
    {
    }
    [MetaClass(2016968373)]
    public class Class0x78387eb5 : AnnouncementDefinition
    {
    }
    [MetaClass("EnableLookAtEventData")]
    public class EnableLookAtEventData : BaseEventData
    {
        [MetaProperty("mEnableLookAt", BinPropertyType.Bool)]
        public bool EnableLookAt { get; set; } = true;
        [MetaProperty("mLockCurrentValues", BinPropertyType.Bool)]
        public bool LockCurrentValues { get; set; } = true;
    }
    [MetaClass("BannerFlagData")]
    public class BannerFlagData : IMetaClass
    {
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; } = new (new ());
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnimationGraphData { get; set; } = new(0);
    }
    [MetaClass("HudCheatMenuData")]
    public class HudCheatMenuData : IMetaClass
    {
        [MetaProperty("tooltipDelay", BinPropertyType.Float)]
        public float TooltipDelay { get; set; } = 0.10000000149011612f;
        [MetaProperty("groupDividerIndex", BinPropertyType.Byte)]
        public byte GroupDividerIndex { get; set; } = 7;
        [MetaProperty("groupDividerGapSize", BinPropertyType.Float)]
        public float GroupDividerGapSize { get; set; } = 0.6000000238418579f;
    }
    [MetaClass("ModeSelectViewController")]
    public class ModeSelectViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("PageRightButtonDefinition", BinPropertyType.Hash)]
        public MetaHash PageRightButtonDefinition { get; set; } = new(0);
        [MetaProperty("SoundOnDeActivate", BinPropertyType.String)]
        public string SoundOnDeActivate { get; set; } = "";
        [MetaProperty("queueDisplayData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<QueueDisplayData>> QueueDisplayData { get; set; } = new();
        [MetaProperty(1899466123, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m1899466123 { get; set; } = new();
        [MetaProperty("PageLeftButtonDefinition", BinPropertyType.Hash)]
        public MetaHash PageLeftButtonDefinition { get; set; } = new(0);
        [MetaProperty(2655774189, BinPropertyType.Hash)]
        public MetaHash m2655774189 { get; set; } = new(0);
        [MetaProperty("SoundOnActivate", BinPropertyType.String)]
        public string SoundOnActivate { get; set; } = "";
    }
    [MetaClass("IOptionItem")]
    public interface IOptionItem : IMetaClass
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        bool LiveUpdate { get; set; }
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        byte ShowOnPlatform { get; set; }
        [MetaProperty("Filter", BinPropertyType.Structure)]
        IOptionItemFilter Filter { get; set; }
    }
    [MetaClass(127412095)]
    public class Class0x798277f : MissileBehaviorSpec
    {
        [MetaProperty(1578749410, BinPropertyType.Float)]
        public float m1578749410 { get; set; } = 0f;
        [MetaProperty(1811605788, BinPropertyType.UInt32)]
        public uint m1811605788 { get; set; } = 0;
        [MetaProperty(1967818150, BinPropertyType.UInt32)]
        public uint m1967818150 { get; set; } = 0;
    }
    [MetaClass("ChatViewController")]
    public class ChatViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("defaultWordWrapMargin", BinPropertyType.Byte)]
        public byte DefaultWordWrapMargin { get; set; } = 10;
        [MetaProperty("SceneChat", BinPropertyType.Hash)]
        public MetaHash SceneChat { get; set; } = new(0);
        [MetaProperty(2189749171, BinPropertyType.Hash)]
        public MetaHash m2189749171 { get; set; } = new(0);
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty("hideAfterSeconds", BinPropertyType.Float)]
        public float HideAfterSeconds { get; set; } = 7.5f;
        [MetaProperty(3163311853, BinPropertyType.Hash)]
        public MetaHash m3163311853 { get; set; } = new(0);
    }
    [MetaClass("BaseEventData")]
    public class BaseEventData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.Hash)]
        public MetaHash Name { get; set; } = new(0);
        [MetaProperty("mStartFrame", BinPropertyType.Float)]
        public float StartFrame { get; set; } = 0f;
        [MetaProperty("mIsSelfOnly", BinPropertyType.Bool)]
        public bool IsSelfOnly { get; set; } = false;
        [MetaProperty("mFireIfAnimationEndsEarly", BinPropertyType.Bool)]
        public bool FireIfAnimationEndsEarly { get; set; } = false;
        [MetaProperty("mEndFrame", BinPropertyType.Float)]
        public float EndFrame { get; set; } = -1f;
    }
    [MetaClass("NullMovement")]
    public class NullMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mDelayTime", BinPropertyType.Float)]
        public float DelayTime { get; set; } = 3f;
        [MetaProperty("mWaitForChildren", BinPropertyType.Bool)]
        public bool WaitForChildren { get; set; } = true;
    }
    [MetaClass(2057207177)]
    public class Class0x7a9e7d89 : IMetaClass
    {
        [MetaProperty(376262977, BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> m376262977 { get; set; } = new();
        [MetaProperty(392062544, BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> m392062544 { get; set; } = new();
        [MetaProperty("mAffectsStatusOverride", BinPropertyType.UInt32)]
        public uint AffectsStatusOverride { get; set; } = 0;
        [MetaProperty(1428183755, BinPropertyType.Float)]
        public float m1428183755 { get; set; } = 0f;
        [MetaProperty("mAffectsTypeOverride", BinPropertyType.UInt32)]
        public uint AffectsTypeOverride { get; set; } = 0;
        [MetaProperty(3666077328, BinPropertyType.Bool)]
        public bool m3666077328 { get; set; } = false;
    }
    [MetaClass("HudTunables")]
    public class HudTunables : IMetaClass
    {
        [MetaProperty("mGameScoreboardTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> GameScoreboardTransitionData { get; set; } = new (new ());
        [MetaProperty("mReplayData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudReplayData> ReplayData { get; set; } = new (new ());
        [MetaProperty("mRadailWheelData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudRadialWheelData> RadailWheelData { get; set; } = new (new ());
        [MetaProperty("mHealthBarData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarData> HealthBarData { get; set; } = new (new ());
        [MetaProperty("mScaleSettings", BinPropertyType.Embedded)]
        public MetaEmbedded<HudScaleSettingsData> ScaleSettings { get; set; } = new (new ());
        [MetaProperty("mLevelUpTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudLevelUpData> LevelUpTransitionData { get; set; } = new (new ());
        [MetaProperty("mEmotePopupData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudEmotePopupData> EmotePopupData { get; set; } = new (new ());
        [MetaProperty("DamageDisplayData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudDamageDisplayData> DamageDisplayData { get; set; } = new (new ());
        [MetaProperty(1221021762, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMessageDisplayData> m1221021762 { get; set; } = new (new ());
        [MetaProperty(1316827209, BinPropertyType.Embedded)]
        public MetaEmbedded<HudAbilityPromptData> m1316827209 { get; set; } = new (new ());
        [MetaProperty("StatStoneMilestoneData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatStoneMilestoneData> StatStoneMilestoneData { get; set; } = new (new ());
        [MetaProperty("mStatPanelStatStoneData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatPanelStatStoneData> StatPanelStatStoneData { get; set; } = new (new ());
        [MetaProperty("mReplayGameStatsTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> ReplayGameStatsTransitionData { get; set; } = new (new ());
        [MetaProperty(2252352223, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMessageDisplayData> m2252352223 { get; set; } = new (new ());
        [MetaProperty(2632753136, BinPropertyType.Embedded)]
        public MetaEmbedded<HudBannerData> m2632753136 { get; set; } = new (new ());
        [MetaProperty("mVoiceChatData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudVoiceChatData> VoiceChatData { get; set; } = new (new ());
        [MetaProperty("mElementalSelectionAnimationData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudElementalSectionUIData> ElementalSelectionAnimationData { get; set; } = new (new ());
        [MetaProperty("mPingData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudPingData> PingData { get; set; } = new (new ());
        [MetaProperty("mStatStoneDeathRecapData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatStoneDeathRecapData> StatStoneDeathRecapData { get; set; } = new (new ());
        [MetaProperty("mEndOfGameData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudEndOfGameData> EndOfGameData { get; set; } = new (new ());
        [MetaProperty("mReplayScoreboardTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> ReplayScoreboardTransitionData { get; set; } = new (new ());
        [MetaProperty("mGearSelectionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudGearSelectionUIData> GearSelectionData { get; set; } = new (new ());
        [MetaProperty("mLoadingScreenData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudLoadingScreenData> LoadingScreenData { get; set; } = new (new ());
        [MetaProperty("mCheatMenuData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudCheatMenuData> CheatMenuData { get; set; } = new (new ());
        [MetaProperty("mHudSpellSlotResetFeedbackData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudSpellSlotResetFeedbackData> HudSpellSlotResetFeedbackData { get; set; } = new (new ());
        [MetaProperty("FightRecapUiData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudFightRecapUIData> FightRecapUiData { get; set; } = new (new ());
        [MetaProperty("mInputBoxData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudInputBoxData> InputBoxData { get; set; } = new (new ());
        [MetaProperty("mStatStoneData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatStoneData> StatStoneData { get; set; } = new (new ());
    }
    [MetaClass("MapMotionPath")]
    public class MapMotionPath : MapPlaceable
    {
        [MetaProperty("Segments", BinPropertyType.Container)]
        public MetaContainer<MapPathSegment> Segments { get; set; } = new();
    }
    [MetaClass("ContextualConditionSpell")]
    public class ContextualConditionSpell : IContextualCondition
    {
        [MetaProperty("mSpellSlot", BinPropertyType.UInt32)]
        public uint SpellSlot { get; set; } = 0;
        [MetaProperty("mChildConditions", BinPropertyType.Container)]
        public MetaContainer<IContextualConditionSpell> ChildConditions { get; set; } = new();
    }
    [MetaClass("MinimapPingData")]
    public class MinimapPingData : IMetaClass
    {
        [MetaProperty("mOMWPingRangeCutoffs", BinPropertyType.Map)]
        public Dictionary<byte, float> OMWPingRangeCutoffs { get; set; } = new();
        [MetaProperty("mPings", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MinimapPingTypeContainer>> Pings { get; set; } = new();
    }
    [MetaClass("BuffCounterByNamedDataValueCalculationPart")]
    public class BuffCounterByNamedDataValueCalculationPart : IGameCalculationPartWithBuffCounter
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash BuffName { get; set; } = new(0);
        [MetaProperty("mIconKey", BinPropertyType.String)]
        public string IconKey { get; set; } = "";
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        public string ScalingTagKey { get; set; } = "";
        [MetaProperty("mDataValue", BinPropertyType.Hash)]
        public MetaHash DataValue { get; set; } = new(0);
    }
    [MetaClass("TFTHudNotificationsData")]
    public class TFTHudNotificationsData : IMetaClass
    {
        [MetaProperty("mSceneTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionIn { get; set; } = new (new ());
        [MetaProperty("mSceneTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionOut { get; set; } = new (new ());
    }
    [MetaClass("AtlasDataBase")]
    public interface AtlasDataBase : IMetaClass
    {
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        uint TextureSourceResolutionHeight { get; set; }
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        uint TextureSourceResolutionWidth { get; set; }
        [MetaProperty("mTextureName", BinPropertyType.String)]
        string TextureName { get; set; }
    }
    [MetaClass("CelebrationViewController")]
    public class CelebrationViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(1340559364, BinPropertyType.String)]
        public string m1340559364 { get; set; } = "";
        [MetaProperty(1633434665, BinPropertyType.Float)]
        public float m1633434665 { get; set; } = 0f;
        [MetaProperty(3303847490, BinPropertyType.Map)]
        public Dictionary<string, string> m3303847490 { get; set; } = new();
    }
    [MetaClass("SkinEmblem")]
    public class SkinEmblem : IMetaClass
    {
        [MetaProperty("mLoadingScreenAnchor", BinPropertyType.UInt32)]
        public uint LoadingScreenAnchor { get; set; } = 5;
        [MetaProperty("mEmblemData", BinPropertyType.ObjectLink)]
        public MetaObjectLink EmblemData { get; set; } = new(0);
    }
    [MetaClass("CharacterLevelRequirement")]
    public class CharacterLevelRequirement : ISpellRankUpRequirement
    {
        [MetaProperty("mLevel", BinPropertyType.UInt32)]
        public uint Level { get; set; } = 1;
    }
    [MetaClass("UIButtonAdditionalElements")]
    public class UIButtonAdditionalElements : IMetaClass
    {
        [MetaProperty("defaultStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> DefaultStateElements { get; set; } = new (new ());
        [MetaProperty("selectedStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> SelectedStateElements { get; set; } = new (new ());
        [MetaProperty("SelectedClickedStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> SelectedClickedStateElements { get; set; } = new (new ());
        [MetaProperty("InactiveStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> InactiveStateElements { get; set; } = new (new ());
        [MetaProperty(2903476354, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m2903476354 { get; set; } = new (new ());
        [MetaProperty("SelectedHoverStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> SelectedHoverStateElements { get; set; } = new (new ());
        [MetaProperty("hoverStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> HoverStateElements { get; set; } = new (new ());
    }
    [MetaClass("TeamBuffData")]
    public class TeamBuffData : IMetaClass
    {
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string BuffName { get; set; } = "";
        [MetaProperty(2589193282, BinPropertyType.Bool)]
        public bool m2589193282 { get; set; } = false;
        [MetaProperty("mUiName", BinPropertyType.String)]
        public string UiName { get; set; } = "";
    }
    [MetaClass("CompanionData")]
    public class CompanionData : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("speciesLink", BinPropertyType.String)]
        public string SpeciesLink { get; set; } = "";
        [MetaProperty("rarity", BinPropertyType.UInt32)]
        public uint Rarity { get; set; } = 0;
        [MetaProperty("mDisabled", BinPropertyType.Bool)]
        public bool Disabled { get; set; } = false;
        [MetaProperty("mCharacter", BinPropertyType.Hash)]
        public MetaHash Character { get; set; } = new(0);
        [MetaProperty("mStandaloneLoadoutsIcon", BinPropertyType.String)]
        public string StandaloneLoadoutsIcon { get; set; } = "";
        [MetaProperty("mStandaloneLoadoutsLargeIcon", BinPropertyType.String)]
        public string StandaloneLoadoutsLargeIcon { get; set; } = "";
        [MetaProperty("level", BinPropertyType.UInt32)]
        public uint Level { get; set; } = 0;
        [MetaProperty("mSkinId", BinPropertyType.UInt32)]
        public uint SkinId { get; set; } = 0;
        [MetaProperty("mStandaloneCircleIcon", BinPropertyType.String)]
        public string StandaloneCircleIcon { get; set; } = "";
        [MetaProperty("mLoadScreen", BinPropertyType.String)]
        public string LoadScreen { get; set; } = "";
    }
    [MetaClass("ContextualConditionObjectiveTakeByMyTeam")]
    public class ContextualConditionObjectiveTakeByMyTeam : IContextualCondition
    {
        [MetaProperty("mTakenObjective", BinPropertyType.UInt32)]
        public uint TakenObjective { get; set; } = 1;
    }
    [MetaClass("TFTBotLoadoutConfiguration")]
    public class TFTBotLoadoutConfiguration : IMetaClass
    {
        [MetaProperty("mapSkins", BinPropertyType.Container)]
        public MetaContainer<MetaHash> MapSkins { get; set; } = new();
        [MetaProperty(4026254940, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTCompanionBucket>> m4026254940 { get; set; } = new();
    }
    [MetaClass("TFTStatData")]
    public class TFTStatData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mType", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
        [MetaProperty("mLifetime", BinPropertyType.UInt32)]
        public uint Lifetime { get; set; } = 0;
        [MetaProperty(255472540, BinPropertyType.Int32)]
        public int m255472540 { get; set; } = 0;
        [MetaProperty("mContext", BinPropertyType.UInt32)]
        public uint Context { get; set; } = 0;
    }
    [MetaClass("TFTNotificationData")]
    public class TFTNotificationData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(511705008, BinPropertyType.String)]
        public string m511705008 { get; set; } = "";
        [MetaProperty("mDurationSeconds", BinPropertyType.Float)]
        public float DurationSeconds { get; set; } = 3f;
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string IconPath { get; set; } = "";
        [MetaProperty(2793884270, BinPropertyType.String)]
        public string m2793884270 { get; set; } = "";
        [MetaProperty(3730563465, BinPropertyType.String)]
        public string m3730563465 { get; set; } = "";
        [MetaProperty("mToplineTra", BinPropertyType.String)]
        public string ToplineTra { get; set; } = "";
        [MetaProperty("mBottomlineTra", BinPropertyType.String)]
        public string BottomlineTra { get; set; } = "";
    }
    [MetaClass("TerrainLocation")]
    public class TerrainLocation : TargetingTypeData
    {
    }
    [MetaClass("ItemModifier")]
    public class ItemModifier : IMetaClass
    {
        [MetaProperty("mDescriptionToPrepend", BinPropertyType.String)]
        public string DescriptionToPrepend { get; set; } = "";
        [MetaProperty("mDeltaBuffCurrencyCostPercent", BinPropertyType.Float)]
        public float DeltaBuffCurrencyCostPercent { get; set; } = 0f;
        [MetaProperty("mVisualPriority", BinPropertyType.Int32)]
        public int VisualPriority { get; set; } = 0;
        [MetaProperty("inventoryIconToOverlay", BinPropertyType.String)]
        public string InventoryIconToOverlay { get; set; } = "";
        [MetaProperty("mMaximumDeltasToStack", BinPropertyType.Int32)]
        public int MaximumDeltasToStack { get; set; } = 1;
        [MetaProperty("mDynamicTooltipToReplace", BinPropertyType.String)]
        public string DynamicTooltipToReplace { get; set; } = "";
        [MetaProperty("mDynamicTooltipToPrepend", BinPropertyType.String)]
        public string DynamicTooltipToPrepend { get; set; } = "";
        [MetaProperty("mDeltaRequiredLevel", BinPropertyType.Int32)]
        public int DeltaRequiredLevel { get; set; } = 0;
        [MetaProperty("mModifiedIfBuildsFromItem", BinPropertyType.ObjectLink)]
        public MetaObjectLink ModifiedIfBuildsFromItem { get; set; } = new(0);
        [MetaProperty("mModifiedGroup", BinPropertyType.ObjectLink)]
        public MetaObjectLink ModifiedGroup { get; set; } = new(0);
        [MetaProperty("mDynamicTooltipToAppend", BinPropertyType.String)]
        public string DynamicTooltipToAppend { get; set; } = "";
        [MetaProperty("mAddedBuildFrom", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> AddedBuildFrom { get; set; } = new();
        [MetaProperty("mIgnoreMaxGroupOwnable", BinPropertyType.Bool)]
        public bool IgnoreMaxGroupOwnable { get; set; } = false;
        [MetaProperty("mSpellNameToReplace", BinPropertyType.String)]
        public string SpellNameToReplace { get; set; } = "";
        [MetaProperty("mDisplayNameToReplace", BinPropertyType.String)]
        public string DisplayNameToReplace { get; set; } = "";
        [MetaProperty("mRemovedBuildFrom", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> RemovedBuildFrom { get; set; } = new();
        [MetaProperty("mModifiedItem", BinPropertyType.ObjectLink)]
        public MetaObjectLink ModifiedItem { get; set; } = new(0);
        [MetaProperty("mDescriptionToReplace", BinPropertyType.String)]
        public string DescriptionToReplace { get; set; } = "";
        [MetaProperty("mDisplayNameToPrepend", BinPropertyType.String)]
        public string DisplayNameToPrepend { get; set; } = "";
        [MetaProperty("mReplaceInsteadOfAddingBuildFrom", BinPropertyType.Bool)]
        public bool ReplaceInsteadOfAddingBuildFrom { get; set; } = false;
        [MetaProperty("mModifierIsInheritedByOwnedParentItems", BinPropertyType.Bool)]
        public bool ModifierIsInheritedByOwnedParentItems { get; set; } = false;
        [MetaProperty("mDescriptionToAppend", BinPropertyType.String)]
        public string DescriptionToAppend { get; set; } = "";
        [MetaProperty("mDeltaGoldCost", BinPropertyType.Float)]
        public float DeltaGoldCost { get; set; } = 0f;
        [MetaProperty("mMinimumModifierInstancesToBeActive", BinPropertyType.Int32)]
        public int MinimumModifierInstancesToBeActive { get; set; } = 1;
        [MetaProperty("mDeltaGoldCostPercent", BinPropertyType.Float)]
        public float DeltaGoldCostPercent { get; set; } = 0f;
        [MetaProperty("mDeltaMaxStacks", BinPropertyType.Int32)]
        public int DeltaMaxStacks { get; set; } = 0;
        [MetaProperty("mMajorActiveItemToEnable", BinPropertyType.Bool)]
        public bool MajorActiveItemToEnable { get; set; } = false;
        [MetaProperty("mDisplayNameToAppend", BinPropertyType.String)]
        public string DisplayNameToAppend { get; set; } = "";
        [MetaProperty("mItemModifierID", BinPropertyType.Hash)]
        public MetaHash ItemModifierID { get; set; } = new(0);
        [MetaProperty("mShowAsModifiedInUI", BinPropertyType.Bool)]
        public bool ShowAsModifiedInUI { get; set; } = false;
        [MetaProperty("mClickableToEnable", BinPropertyType.Bool)]
        public bool ClickableToEnable { get; set; } = false;
        [MetaProperty("mMaximumModifierInstancesToBeActive", BinPropertyType.Int32)]
        public int MaximumModifierInstancesToBeActive { get; set; } = 2147483647;
        [MetaProperty("mDeltaBuffCurrencyCost", BinPropertyType.Int32)]
        public int DeltaBuffCurrencyCost { get; set; } = 0;
        [MetaProperty("mIgnoreSpecificMaxGroupOwnable", BinPropertyType.Hash)]
        public MetaHash IgnoreSpecificMaxGroupOwnable { get; set; } = new(0);
    }
    [MetaClass("SpellDataResourceClient")]
    public class SpellDataResourceClient : IMetaClass
    {
        [MetaProperty("mTargeterDefinitions", BinPropertyType.Container)]
        public MetaContainer<TargeterDefinition> TargeterDefinitions { get; set; } = new();
        [MetaProperty("mTooltipData", BinPropertyType.Structure)]
        public TooltipInstanceSpell TooltipData { get; set; } = null;
        [MetaProperty("mRightClickSpellAction", BinPropertyType.UInt32)]
        public uint RightClickSpellAction { get; set; } = 2;
        [MetaProperty(928405213, BinPropertyType.Hash)]
        public MetaHash m928405213 { get; set; } = new(0);
        [MetaProperty(2102005358, BinPropertyType.Hash)]
        public MetaHash m2102005358 { get; set; } = new(0);
        [MetaProperty("mLeftClickSpellAction", BinPropertyType.UInt32)]
        public uint LeftClickSpellAction { get; set; } = 1;
        [MetaProperty("mSpawningUIDefinition", BinPropertyType.Structure)]
        public SpawningUIDefinition SpawningUIDefinition { get; set; } = null;
        [MetaProperty("mCustomTargeterDefinitions", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<CustomTargeterDefinitions>> CustomTargeterDefinitions { get; set; } = new();
        [MetaProperty("mMissileTargeterDefinitions", BinPropertyType.Container)]
        public MetaContainer<MissileAttachedTargetingDefinition> MissileTargeterDefinitions { get; set; } = new();
    }
    [MetaClass("LoadingScreenRankedData")]
    public class LoadingScreenRankedData : IMetaClass
    {
        [MetaProperty("mRankedData", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<LoadingScreenRankedProperties>> RankedData { get; set; } = new();
    }
    [MetaClass("FxActionAnimate")]
    public class FxActionAnimate : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("PauseOnEnd", BinPropertyType.Bool)]
        public bool PauseOnEnd { get; set; } = false;
        [MetaProperty("Loop", BinPropertyType.Bool)]
        public bool Loop { get; set; } = false;
        [MetaProperty("TargetObject", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTarget> TargetObject { get; set; } = new (new ());
        [MetaProperty("animName", BinPropertyType.String)]
        public string AnimName { get; set; } = "";
        [MetaProperty(4001159322, BinPropertyType.Bool)]
        public bool m4001159322 { get; set; } = true;
    }
    [MetaClass("OptionTemplateDropdown")]
    public class OptionTemplateDropdown : IOptionTemplate
    {
        [MetaProperty("labelElement", BinPropertyType.Hash)]
        public MetaHash LabelElement { get; set; } = new(0);
        [MetaProperty("ComboBoxDefinition", BinPropertyType.Hash)]
        public MetaHash ComboBoxDefinition { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionHasItemFromVOGroup")]
    public class ContextualConditionHasItemFromVOGroup : IContextualCondition
    {
        [MetaProperty("mItemVOGroupHash", BinPropertyType.Hash)]
        public MetaHash ItemVOGroupHash { get; set; } = new(0);
    }
    [MetaClass("HudLevelUpData")]
    public class HudLevelUpData : IMetaClass
    {
        [MetaProperty("idleSheenInterval", BinPropertyType.Float)]
        public float IdleSheenInterval { get; set; } = 1f;
        [MetaProperty("delay", BinPropertyType.Float)]
        public float Delay { get; set; } = 0f;
        [MetaProperty("minAlpha", BinPropertyType.Byte)]
        public byte MinAlpha { get; set; } = 0;
        [MetaProperty("animTime", BinPropertyType.Float)]
        public float AnimTime { get; set; } = 1f;
        [MetaProperty("overshoot", BinPropertyType.Float)]
        public float Overshoot { get; set; } = 0.5f;
        [MetaProperty("maxAlpha", BinPropertyType.Byte)]
        public byte MaxAlpha { get; set; } = 255;
        [MetaProperty("maxOffset", BinPropertyType.Float)]
        public float MaxOffset { get; set; } = 0f;
        [MetaProperty("playerBuffsOffset", BinPropertyType.Float)]
        public float PlayerBuffsOffset { get; set; } = 59f;
        [MetaProperty("inertia", BinPropertyType.Float)]
        public float Inertia { get; set; } = 0.5f;
    }
    [MetaClass("AnchorBase")]
    public interface AnchorBase : IMetaClass
    {
    }
    [MetaClass("MapActionPlaySoundAtLocation")]
    public class MapActionPlaySoundAtLocation : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty("SoundEventName", BinPropertyType.String)]
        public string SoundEventName { get; set; } = "";
        [MetaProperty("LocationName", BinPropertyType.String)]
        public string LocationName { get; set; } = "";
    }
    [MetaClass("ILoadoutInfoPanel")]
    public interface ILoadoutInfoPanel : IMetaClass
    {
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; }
    }
    [MetaClass("MinimapPingEffectAndTextureData")]
    public class MinimapPingEffectAndTextureData : IMetaClass
    {
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("mOrder", BinPropertyType.Structure)]
        public TextureAndColorData Order { get; set; } = null;
        [MetaProperty("mChaos", BinPropertyType.Structure)]
        public TextureAndColorData Chaos { get; set; } = null;
        [MetaProperty("mEffect", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapPingEffectDefinition> Effect { get; set; } = new (new ());
        [MetaProperty("mDefault", BinPropertyType.Embedded)]
        public MetaEmbedded<TextureAndColorData> Default { get; set; } = new (new ());
    }
    [MetaClass("HudStatStoneData")]
    public class HudStatStoneData : IMetaClass
    {
        [MetaProperty(769045550, BinPropertyType.String)]
        public string m769045550 { get; set; } = "";
        [MetaProperty(1016048105, BinPropertyType.String)]
        public string m1016048105 { get; set; } = "";
    }
    [MetaClass("SyncedAnimationEventData")]
    public class SyncedAnimationEventData : BaseEventData
    {
        [MetaProperty("mLerpTime", BinPropertyType.Float)]
        public float LerpTime { get; set; } = 0.20000000298023224f;
    }
    [MetaClass("AnimationFractionDynamicMaterialFloatDriver")]
    public class AnimationFractionDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mAnimationName", BinPropertyType.Hash)]
        public MetaHash AnimationName { get; set; } = new(0);
    }
    [MetaClass("TargetingTypeData")]
    public interface TargetingTypeData : IMetaClass
    {
    }
    [MetaClass(2151525964)]
    public class Class0x803dae4c : IGameCalculationPart
    {
        [MetaProperty("mCeiling", BinPropertyType.Optional)]
        public MetaOptional<float> Ceiling { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mSubparts", BinPropertyType.Container)]
        public MetaContainer<IGameCalculationPart> Subparts { get; set; } = new();
        [MetaProperty("mFloor", BinPropertyType.Optional)]
        public MetaOptional<float> Floor { get; set; } = new MetaOptional<float>(default(float), false);
    }
    [MetaClass("TftMapCharacterData")]
    public class TftMapCharacterData : IMetaClass
    {
        [MetaProperty("SkinData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftMapCharacterSkinData>> SkinData { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("charData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftMapCharacterRecordData>> CharData { get; set; } = new();
    }
    [MetaClass("ContextualConditionTurretPosition")]
    public class ContextualConditionTurretPosition : IContextualCondition
    {
        [MetaProperty("mTurretPosition", BinPropertyType.Byte)]
        public byte TurretPosition { get; set; } = 0;
    }
    [MetaClass("FloatingTextGlobalConfig")]
    public class FloatingTextGlobalConfig : IMetaClass
    {
        [MetaProperty("mDamageDisplayTypes", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatingTextDamageDisplayTypeList> DamageDisplayTypes { get; set; } = new (new ());
        [MetaProperty("mTunables", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatingTextTunables> Tunables { get; set; } = new (new ());
        [MetaProperty("mFloatingTextTypes", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatingTextTypeList> FloatingTextTypes { get; set; } = new (new ());
    }
    [MetaClass("EffectAnimatedRotatingIconElementData")]
    public class EffectAnimatedRotatingIconElementData : EffectAnimationElementData
    {
    }
    [MetaClass("ShaderPhysicalParameter")]
    public class ShaderPhysicalParameter : IMetaClass
    {
        [MetaProperty("logicalParameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderLogicalParameter>> LogicalParameters { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("data", BinPropertyType.Vector4)]
        public Vector4 Data { get; set; } = new Vector4(0f, 0f, 0f, 0f);
    }
    [MetaClass("EffectCooldownRadialElementData")]
    public class EffectCooldownRadialElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("mIsFill", BinPropertyType.Bool)]
        public bool IsFill { get; set; } = false;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
    }
    [MetaClass("ConformToPathEventData")]
    public class ConformToPathEventData : BaseEventData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        public MetaHash MaskDataName { get; set; } = new(0);
        [MetaProperty("mBlendOutTime", BinPropertyType.Float)]
        public float BlendOutTime { get; set; } = 0f;
        [MetaProperty("mBlendInTime", BinPropertyType.Float)]
        public float BlendInTime { get; set; } = 0f;
    }
    [MetaClass("VfxMigrationResources")]
    public class VfxMigrationResources : IMetaClass
    {
        [MetaProperty("resourceMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> ResourceMap { get; set; } = new();
    }
    [MetaClass("ItemRecommendationContextList")]
    public class ItemRecommendationContextList : IMetaClass
    {
        [MetaProperty(2701742862, BinPropertyType.Map)]
        public Dictionary<uint, MetaEmbedded<ItemRecommendationItemList>> m2701742862 { get; set; } = new();
        [MetaProperty("mAllStartingItemIds", BinPropertyType.Map)]
        public Dictionary<uint, MetaEmbedded<ItemRecommendationItemList>> AllStartingItemIds { get; set; } = new();
        [MetaProperty("mContexts", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationContext>> Contexts { get; set; } = new();
    }
    [MetaClass("ContextualConditionCharacterPlayingAnimation")]
    public class ContextualConditionCharacterPlayingAnimation : ICharacterSubcondition
    {
        [MetaProperty("mAnimationNameHash", BinPropertyType.Hash)]
        public MetaHash AnimationNameHash { get; set; } = new(0);
    }
    [MetaClass("ToggleBuffCheat")]
    public class ToggleBuffCheat : Cheat
    {
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string BuffName { get; set; } = "";
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
        [MetaProperty("UseTargetAsCaster", BinPropertyType.Bool)]
        public bool UseTargetAsCaster { get; set; } = false;
    }
    [MetaClass("ElementSoundEvents")]
    public class ElementSoundEvents : IMetaClass
    {
        [MetaProperty(554607262, BinPropertyType.String)]
        public string m554607262 { get; set; } = "";
        [MetaProperty(72483660, BinPropertyType.String)]
        public string m72483660 { get; set; } = "";
        [MetaProperty("MouseUpEvent", BinPropertyType.String)]
        public string MouseUpEvent { get; set; } = "";
        [MetaProperty("RolloverEvent", BinPropertyType.String)]
        public string RolloverEvent { get; set; } = "";
        [MetaProperty(3030163781, BinPropertyType.String)]
        public string m3030163781 { get; set; } = "";
        [MetaProperty("MouseDownEvent", BinPropertyType.String)]
        public string MouseDownEvent { get; set; } = "";
    }
    [MetaClass("HomeViewController")]
    public class HomeViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("ThemeMusicState", BinPropertyType.String)]
        public string ThemeMusicState { get; set; } = "";
        [MetaProperty("TeamPlannerButtonDefinition", BinPropertyType.Hash)]
        public MetaHash TeamPlannerButtonDefinition { get; set; } = new(0);
        [MetaProperty("PlayGameButtonDefinition", BinPropertyType.Hash)]
        public MetaHash PlayGameButtonDefinition { get; set; } = new(0);
        [MetaProperty(2259066423, BinPropertyType.Byte)]
        public byte m2259066423 { get; set; } = 0;
        [MetaProperty("SpecialOfferButtonDefinition", BinPropertyType.Hash)]
        public MetaHash SpecialOfferButtonDefinition { get; set; } = new(0);
        [MetaProperty("ModeSelectCustomButtonData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ModeSelectButtonData>> ModeSelectCustomButtonData { get; set; } = new();
        [MetaProperty("ThemeMusicStateGroup", BinPropertyType.String)]
        public string ThemeMusicStateGroup { get; set; } = "";
        [MetaProperty("StoreButtonDefinition", BinPropertyType.Hash)]
        public MetaHash StoreButtonDefinition { get; set; } = new(0);
        [MetaProperty(3692913182, BinPropertyType.Float)]
        public float m3692913182 { get; set; } = 0.5f;
        [MetaProperty("ModeSelectDefaultButtonData", BinPropertyType.Hash)]
        public MetaHash ModeSelectDefaultButtonData { get; set; } = new(0);
        [MetaProperty("BattlepassButtonDefinition", BinPropertyType.Hash)]
        public MetaHash BattlepassButtonDefinition { get; set; } = new(0);
        [MetaProperty("SpecialOfferController", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xfbd72d16> SpecialOfferController { get; set; } = new (new ());
    }
    [MetaClass("SumOfSubPartsCalculationPart")]
    public class SumOfSubPartsCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mSubparts", BinPropertyType.Container)]
        public MetaContainer<IGameCalculationPart> Subparts { get; set; } = new();
    }
    [MetaClass("ItemDataClient")]
    public class ItemDataClient : IMetaClass
    {
        [MetaProperty("mFloatVarsDecimals", BinPropertyType.Container)]
        public MetaContainer<int> FloatVarsDecimals { get; set; } = new();
        [MetaProperty("InventoryIconLarge", BinPropertyType.String)]
        public string InventoryIconLarge { get; set; } = "";
        [MetaProperty("mTooltipData", BinPropertyType.Structure)]
        public TooltipInstanceItem TooltipData { get; set; } = null;
        [MetaProperty("mShopTooltip", BinPropertyType.String)]
        public string ShopTooltip { get; set; } = "";
        [MetaProperty("epicness", BinPropertyType.Byte)]
        public byte Epicness { get; set; } = 2;
        [MetaProperty("effectRadius", BinPropertyType.Float)]
        public float EffectRadius { get; set; } = 0f;
        [MetaProperty("mDynamicTooltip", BinPropertyType.String)]
        public string DynamicTooltip { get; set; } = "";
        [MetaProperty("inventoryIcon", BinPropertyType.String)]
        public string InventoryIcon { get; set; } = "";
        [MetaProperty("ShopExtendedTooltip", BinPropertyType.String)]
        public string ShopExtendedTooltip { get; set; } = "";
        [MetaProperty("InventoryIconMaterial", BinPropertyType.ObjectLink)]
        public MetaObjectLink InventoryIconMaterial { get; set; } = new(0);
        [MetaProperty("InventoryIconSmall", BinPropertyType.String)]
        public string InventoryIconSmall { get; set; } = "";
        [MetaProperty("mDescription", BinPropertyType.String)]
        public string Description { get; set; } = "";
    }
    [MetaClass("TftMapGroupData")]
    public class TftMapGroupData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mId", BinPropertyType.UInt32)]
        public uint Id { get; set; } = 0;
    }
    [MetaClass("StaticMaterialPassDef")]
    public class StaticMaterialPassDef : IMetaClass
    {
        [MetaProperty("srcColorBlendFactor", BinPropertyType.UInt32)]
        public uint SrcColorBlendFactor { get; set; } = 1;
        [MetaProperty("blendEnable", BinPropertyType.Bool)]
        public bool BlendEnable { get; set; } = false;
        [MetaProperty("shader", BinPropertyType.ObjectLink)]
        public MetaObjectLink Shader { get; set; } = new(0);
        [MetaProperty("depthOffsetSlope", BinPropertyType.Float)]
        public float DepthOffsetSlope { get; set; } = 0f;
        [MetaProperty("stencilEnable", BinPropertyType.Bool)]
        public bool StencilEnable { get; set; } = false;
        [MetaProperty("cullEnable", BinPropertyType.Bool)]
        public bool CullEnable { get; set; } = true;
        [MetaProperty("windingToCull", BinPropertyType.UInt32)]
        public uint WindingToCull { get; set; } = 1;
        [MetaProperty("stencilPassDepthPassOp", BinPropertyType.UInt32)]
        public uint StencilPassDepthPassOp { get; set; } = 0;
        [MetaProperty("dstAlphaBlendFactor", BinPropertyType.UInt32)]
        public uint DstAlphaBlendFactor { get; set; } = 0;
        [MetaProperty("blendEquation", BinPropertyType.UInt32)]
        public uint BlendEquation { get; set; } = 0;
        [MetaProperty("stencilCompareFunc", BinPropertyType.UInt32)]
        public uint StencilCompareFunc { get; set; } = 1;
        [MetaProperty("srcAlphaBlendFactor", BinPropertyType.UInt32)]
        public uint SrcAlphaBlendFactor { get; set; } = 1;
        [MetaProperty("depthCompareFunc", BinPropertyType.UInt32)]
        public uint DepthCompareFunc { get; set; } = 3;
        [MetaProperty(2863927372, BinPropertyType.Bool)]
        public bool m2863927372 { get; set; } = false;
        [MetaProperty("stencilPassDepthFailOp", BinPropertyType.UInt32)]
        public uint StencilPassDepthFailOp { get; set; } = 0;
        [MetaProperty("stencilMask", BinPropertyType.UInt32)]
        public uint StencilMask { get; set; } = 0;
        [MetaProperty("writeMask", BinPropertyType.UInt32)]
        public uint WriteMask { get; set; } = 31;
        [MetaProperty("dstColorBlendFactor", BinPropertyType.UInt32)]
        public uint DstColorBlendFactor { get; set; } = 0;
        [MetaProperty("stencilReferenceVal", BinPropertyType.UInt32)]
        public uint StencilReferenceVal { get; set; } = 0;
        [MetaProperty("paramValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialShaderParamDef>> ParamValues { get; set; } = new();
        [MetaProperty("depthEnable", BinPropertyType.Bool)]
        public bool DepthEnable { get; set; } = true;
        [MetaProperty("stencilFailOp", BinPropertyType.UInt32)]
        public uint StencilFailOp { get; set; } = 0;
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; } = new();
        [MetaProperty("depthOffsetBias", BinPropertyType.Float)]
        public float DepthOffsetBias { get; set; } = 0f;
    }
    [MetaClass("ForceSpawnNeutralCampsCheat")]
    public class ForceSpawnNeutralCampsCheat : Cheat
    {
        [MetaProperty("mSpawnBaron", BinPropertyType.Bool)]
        public bool SpawnBaron { get; set; } = false;
    }
    [MetaClass("VfxPrimitiveMesh")]
    public class VfxPrimitiveMesh : VfxPrimitiveMeshBase
    {
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; } = new (new ());
        [MetaProperty(3934657962, BinPropertyType.Bool)]
        public bool m3934657962 { get; set; } = false;
        [MetaProperty(4227234111, BinPropertyType.Bool)]
        public bool m4227234111 { get; set; } = false;
    }
    [MetaClass("TFTItemMaterialController")]
    public class TFTItemMaterialController : SkinnedMeshDataMaterialController
    {
    }
    [MetaClass("TooltipInstance")]
    public class TooltipInstance : IMetaClass
    {
        [MetaProperty("mLocKeys", BinPropertyType.Map)]
        public Dictionary<string, string> LocKeys { get; set; } = new();
        [MetaProperty("mFormat", BinPropertyType.ObjectLink)]
        public MetaObjectLink Format { get; set; } = new(0);
        [MetaProperty("mLists", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<TooltipInstanceList>> Lists { get; set; } = new();
        [MetaProperty("mObjectName", BinPropertyType.String)]
        public string ObjectName { get; set; } = "";
    }
    [MetaClass(2253409519)]
    public class Class0x86504cef : IMetaClass
    {
        [MetaProperty("Portrait", BinPropertyType.Hash)]
        public MetaHash Portrait { get; set; } = new(0);
        [MetaProperty("VolumeText", BinPropertyType.Hash)]
        public MetaHash VolumeText { get; set; } = new(0);
        [MetaProperty("Group", BinPropertyType.Hash)]
        public MetaHash Group { get; set; } = new(0);
        [MetaProperty("VolumeSliderBar", BinPropertyType.Hash)]
        public MetaHash VolumeSliderBar { get; set; } = new(0);
        [MetaProperty("Halo", BinPropertyType.Hash)]
        public MetaHash Halo { get; set; } = new(0);
        [MetaProperty("NameText", BinPropertyType.Hash)]
        public MetaHash NameText { get; set; } = new(0);
        [MetaProperty("MuteButton", BinPropertyType.Hash)]
        public MetaHash MuteButton { get; set; } = new(0);
    }
    [MetaClass("AudioStatusEvents")]
    public class AudioStatusEvents : IMetaClass
    {
        [MetaProperty("rtpcName", BinPropertyType.String)]
        public string RtpcName { get; set; } = "";
        [MetaProperty("stopEvent", BinPropertyType.String)]
        public string StopEvent { get; set; } = "";
        [MetaProperty("startEvent", BinPropertyType.String)]
        public string StartEvent { get; set; } = "";
    }
    [MetaClass("VfxProbabilityTableDataGrande")]
    public class VfxProbabilityTableDataGrande : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("Scene", BinPropertyType.Hash)]
        public MetaHash Scene { get; set; } = new(0);
        [MetaProperty("Backdrop", BinPropertyType.Hash)]
        public MetaHash Backdrop { get; set; } = new(0);
        [MetaProperty("authoredWidth", BinPropertyType.Float)]
        public float AuthoredWidth { get; set; } = 480f;
        [MetaProperty("SourceResolutionWidth", BinPropertyType.Float)]
        public float SourceResolutionWidth { get; set; } = 1920f;
    }
    [MetaClass("GlobalAudioDataProperties")]
    public class GlobalAudioDataProperties : IMetaClass
    {
        [MetaProperty(272515140, BinPropertyType.UInt32)]
        public uint m272515140 { get; set; } = 1000;
        [MetaProperty("localPlayerStatusEvents", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<AudioStatusEvents>> LocalPlayerStatusEvents { get; set; } = new();
        [MetaProperty("cooldownVoiceOver", BinPropertyType.Float)]
        public float CooldownVoiceOver { get; set; } = 12f;
        [MetaProperty("systems", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> Systems { get; set; } = new();
    }
    [MetaClass("ClearAlreadyHitTracking")]
    public class ClearAlreadyHitTracking : MissileTriggeredActionSpec
    {
    }
    [MetaClass("SetKeyValueInCustomTableBlock")]
    public class SetKeyValueInCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; } = null;
        [MetaProperty("Key", BinPropertyType.Structure)]
        public IScriptValueGet Key { get; set; } = null;
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("TFTDamageSkin")]
    public class TFTDamageSkin : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(833576390, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftDamageSkinDescriptor>> m833576390 { get; set; } = new();
        [MetaProperty("disabled", BinPropertyType.Bool)]
        public bool Disabled { get; set; } = false;
        [MetaProperty("AudioBankPaths", BinPropertyType.Container)]
        public MetaContainer<string> AudioBankPaths { get; set; } = new();
        [MetaProperty("StandaloneLoadoutsIcon", BinPropertyType.String)]
        public string StandaloneLoadoutsIcon { get; set; } = "";
        [MetaProperty("rarity", BinPropertyType.UInt32)]
        public uint Rarity { get; set; } = 0;
        [MetaProperty("SkinID", BinPropertyType.UInt32)]
        public uint SkinID { get; set; } = 0;
        [MetaProperty(2127934631, BinPropertyType.Float)]
        public float m2127934631 { get; set; } = 0f;
        [MetaProperty("level", BinPropertyType.UInt32)]
        public uint Level { get; set; } = 0;
        [MetaProperty("VfxResourceResolver", BinPropertyType.ObjectLink)]
        public MetaObjectLink VfxResourceResolver { get; set; } = new(0);
        [MetaProperty("DamageBuffName", BinPropertyType.String)]
        public string DamageBuffName { get; set; } = "";
        [MetaProperty("StandaloneLoadoutsLargeIcon", BinPropertyType.String)]
        public string StandaloneLoadoutsLargeIcon { get; set; } = "";
    }
    [MetaClass("IsAnimationPlayingDynamicMaterialBoolDriver")]
    public class IsAnimationPlayingDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mAnimationNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationNames { get; set; } = new();
    }
    [MetaClass("TargeterDefinitionCone")]
    public class TargeterDefinitionCone : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("coneFollowsEnd", BinPropertyType.Bool)]
        public bool ConeFollowsEnd { get; set; } = false;
        [MetaProperty("rangeGrowthMax", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthMax { get; set; } = new (new ());
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; } = new (new ());
        [MetaProperty("rangeGrowthDuration", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthDuration { get; set; } = new (new ());
        [MetaProperty("fallbackDirection", BinPropertyType.UInt32)]
        public uint FallbackDirection { get; set; } = 1;
        [MetaProperty("rangeGrowthStartTime", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthStartTime { get; set; } = new (new ());
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; } = new (new ());
        [MetaProperty("textureConeOverrideName", BinPropertyType.String)]
        public string TextureConeOverrideName { get; set; } = "";
        [MetaProperty("hasMaxGrowRange", BinPropertyType.Bool)]
        public bool HasMaxGrowRange { get; set; } = false;
        [MetaProperty("coneRange", BinPropertyType.Optional)]
        public MetaOptional<float> ConeRange { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("textureConeMaxGrowName", BinPropertyType.String)]
        public string TextureConeMaxGrowName { get; set; } = "";
        [MetaProperty("coneAngleDegrees", BinPropertyType.Optional)]
        public MetaOptional<float> ConeAngleDegrees { get; set; } = new MetaOptional<float>(default(float), false);
    }
    [MetaClass("RegaliaData")]
    public class RegaliaData : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("texture", BinPropertyType.String)]
        public string Texture { get; set; } = "";
    }
    [MetaClass("GameModeConstantVector3f")]
    public class GameModeConstantVector3f : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Vector3)]
        public Vector3 Value { get; set; } = new Vector3(0f, 0f, 0f);
    }
    [MetaClass("ILoadoutFeatureDataBehavior")]
    public class ILoadoutFeatureDataBehavior : IMetaClass
    {
        [MetaProperty("Region", BinPropertyType.Hash)]
        public MetaHash Region { get; set; } = new(0);
        [MetaProperty(1311529430, BinPropertyType.Hash)]
        public MetaHash m1311529430 { get; set; } = new(0);
        [MetaProperty("IllustrationIcon", BinPropertyType.Hash)]
        public MetaHash IllustrationIcon { get; set; } = new(0);
        [MetaProperty("LoadoutType", BinPropertyType.UInt32)]
        public uint LoadoutType { get; set; } = 0;
        [MetaProperty("DisplayNameTraKey", BinPropertyType.String)]
        public string DisplayNameTraKey { get; set; } = "";
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("GameModeDefaultStats")]
    public class GameModeDefaultStats : IMetaClass
    {
        [MetaProperty("DefaultCharacterStats", BinPropertyType.Embedded)]
        public MetaEmbedded<StatFormulaDataList> DefaultCharacterStats { get; set; } = new (new ());
    }
    [MetaClass("GDSMapObjectAnimationInfo")]
    public class GDSMapObjectAnimationInfo : GDSMapObjectExtraInfo
    {
        [MetaProperty("destroyOnCompletion", BinPropertyType.Bool)]
        public bool DestroyOnCompletion { get; set; } = false;
        [MetaProperty("duration", BinPropertyType.Float)]
        public float Duration { get; set; } = 0f;
        [MetaProperty("looping", BinPropertyType.Bool)]
        public bool Looping { get; set; } = true;
        [MetaProperty("defaultAnimation", BinPropertyType.String)]
        public string DefaultAnimation { get; set; } = "";
    }
    [MetaClass("PerkReplacementList")]
    public class PerkReplacementList : IMetaClass
    {
        [MetaProperty("mReplacements", BinPropertyType.Container)]
        public MetaContainer<PerkReplacement> Replacements { get; set; } = new();
    }
    [MetaClass("GameModeMapData")]
    public class GameModeMapData : IMetaClass
    {
        [MetaProperty("AnnouncementsMapping", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnnouncementsMapping { get; set; } = new(0);
        [MetaProperty("mPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkReplacementList> PerkReplacements { get; set; } = new (new ());
        [MetaProperty("mGameModeConstants", BinPropertyType.ObjectLink)]
        public MetaObjectLink GameModeConstants { get; set; } = new(0);
        [MetaProperty("itemLists", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> ItemLists { get; set; } = new();
        [MetaProperty("mMapLocators", BinPropertyType.ObjectLink)]
        public MetaObjectLink MapLocators { get; set; } = new(0);
        [MetaProperty("mStatsUiData", BinPropertyType.ObjectLink)]
        public MetaObjectLink StatsUiData { get; set; } = new(0);
        [MetaProperty("ItemShopEnabled", BinPropertyType.Bool)]
        public bool ItemShopEnabled { get; set; } = true;
        [MetaProperty("mExperienceModData", BinPropertyType.ObjectLink)]
        public MetaObjectLink ExperienceModData { get; set; } = new(0);
        [MetaProperty("mLoadScreenTipConfiguration", BinPropertyType.ObjectLink)]
        public MetaObjectLink LoadScreenTipConfiguration { get; set; } = new(0);
        [MetaProperty(1765926418, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1765926418 { get; set; } = new(0);
        [MetaProperty("mFloatingTextOverride", BinPropertyType.ObjectLink)]
        public MetaObjectLink FloatingTextOverride { get; set; } = new(0);
        [MetaProperty(1890753597, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m1890753597 { get; set; } = new();
        [MetaProperty("mRenderStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink RenderStyle { get; set; } = new(0);
        [MetaProperty("mChampionIndicatorEnabled", BinPropertyType.Bool)]
        public bool ChampionIndicatorEnabled { get; set; } = false;
        [MetaProperty(2284479568, BinPropertyType.Bool)]
        public bool m2284479568 { get; set; } = false;
        [MetaProperty("mNeutralTimersDisplay", BinPropertyType.ObjectLink)]
        public MetaObjectLink NeutralTimersDisplay { get; set; } = new(0);
        [MetaProperty("mItemShopData", BinPropertyType.ObjectLink)]
        public MetaObjectLink ItemShopData { get; set; } = new(0);
        [MetaProperty("mLoadingScreenBackground", BinPropertyType.String)]
        public string LoadingScreenBackground { get; set; } = "";
        [MetaProperty("mModeName", BinPropertyType.Hash)]
        public MetaHash ModeName { get; set; } = new(0);
        [MetaProperty("mDeathTimes", BinPropertyType.ObjectLink)]
        public MetaObjectLink DeathTimes { get; set; } = new(0);
        [MetaProperty("mScriptDataObjectLists", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ScriptDataObjectLists { get; set; } = new();
        [MetaProperty("mMissionBuffData", BinPropertyType.ObjectLink)]
        public MetaObjectLink MissionBuffData { get; set; } = new(0);
        [MetaProperty("mHudScoreData", BinPropertyType.Hash)]
        public MetaHash HudScoreData { get; set; } = new(0);
        [MetaProperty("mGameplayConfig", BinPropertyType.ObjectLink)]
        public MetaObjectLink GameplayConfig { get; set; } = new(0);
        [MetaProperty("mRelativeColorization", BinPropertyType.Bool)]
        public bool RelativeColorization { get; set; } = false;
        [MetaProperty("mCursorConfig", BinPropertyType.Hash)]
        public MetaHash CursorConfig { get; set; } = new(0);
        [MetaProperty("mChampionLists", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> ChampionLists { get; set; } = new();
        [MetaProperty("mSurrenderSettings", BinPropertyType.ObjectLink)]
        public MetaObjectLink SurrenderSettings { get; set; } = new(0);
        [MetaProperty("mCursorConfigUpdate", BinPropertyType.Hash)]
        public MetaHash CursorConfigUpdate { get; set; } = new(0);
        [MetaProperty(4148979643, BinPropertyType.String)]
        public string m4148979643 { get; set; } = "";
        [MetaProperty("mExperienceCurveData", BinPropertyType.ObjectLink)]
        public MetaObjectLink ExperienceCurveData { get; set; } = new(0);
    }
    [MetaClass("LobbyViewController")]
    public class LobbyViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(439752578, BinPropertyType.String)]
        public string m439752578 { get; set; } = "";
        [MetaProperty("InQueueMusicState", BinPropertyType.String)]
        public string InQueueMusicState { get; set; } = "";
        [MetaProperty("LobbyCloseButton", BinPropertyType.Hash)]
        public MetaHash LobbyCloseButton { get; set; } = new(0);
        [MetaProperty("loadoutsButton", BinPropertyType.Hash)]
        public MetaHash LoadoutsButton { get; set; } = new(0);
        [MetaProperty(1729901768, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1729901768 { get; set; } = new(0);
        [MetaProperty(1925484875, BinPropertyType.String)]
        public string m1925484875 { get; set; } = "";
        [MetaProperty("ReadyCheckDeclineButton", BinPropertyType.Hash)]
        public MetaHash ReadyCheckDeclineButton { get; set; } = new(0);
        [MetaProperty("FriendEditButton", BinPropertyType.Hash)]
        public MetaHash FriendEditButton { get; set; } = new(0);
        [MetaProperty(2744078369, BinPropertyType.String)]
        public string m2744078369 { get; set; } = "";
        [MetaProperty(3017995790, BinPropertyType.Hash)]
        public MetaHash m3017995790 { get; set; } = new(0);
        [MetaProperty(3173636438, BinPropertyType.Map)]
        public Dictionary<long, string> m3173636438 { get; set; } = new();
        [MetaProperty(3209819674, BinPropertyType.String)]
        public string m3209819674 { get; set; } = "";
        [MetaProperty("ReadyCheckAcceptButton", BinPropertyType.Hash)]
        public MetaHash ReadyCheckAcceptButton { get; set; } = new(0);
        [MetaProperty("ThemeMusicStateGroup", BinPropertyType.String)]
        public string ThemeMusicStateGroup { get; set; } = "";
        [MetaProperty("LobbyMusicState", BinPropertyType.String)]
        public string LobbyMusicState { get; set; } = "";
        [MetaProperty(3583548078, BinPropertyType.Hash)]
        public MetaHash m3583548078 { get; set; } = new(0);
        [MetaProperty("FriendInviteButton", BinPropertyType.Hash)]
        public MetaHash FriendInviteButton { get; set; } = new(0);
    }
    [MetaClass("MapLightingVolume")]
    public class MapLightingVolume : MapPlaceable
    {
        [MetaProperty("fogColor", BinPropertyType.Vector4)]
        public Vector4 FogColor { get; set; } = new Vector4(0.20000000298023224f, 0.20000000298023224f, 0.4000000059604645f, 1f);
        [MetaProperty("fogEmissiveRemap", BinPropertyType.Float)]
        public float FogEmissiveRemap { get; set; } = 1.899999976158142f;
        [MetaProperty("fogLowQualityModeEmissiveRemap", BinPropertyType.Float)]
        public float FogLowQualityModeEmissiveRemap { get; set; } = 0.019999999552965164f;
        [MetaProperty("fogAlternateColor", BinPropertyType.Vector4)]
        public Vector4 FogAlternateColor { get; set; } = new Vector4(0.10000000149011612f, 0.10000000149011612f, 0.20000000298023224f, 1f);
        [MetaProperty("groundColor", BinPropertyType.Vector4)]
        public Vector4 GroundColor { get; set; } = new Vector4(0.10000000149011612f, 0.10000000149011612f, 0.10000000149011612f, 1f);
        [MetaProperty("sunColor", BinPropertyType.Vector4)]
        public Vector4 SunColor { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        [MetaProperty("fogStartAndEnd", BinPropertyType.Vector2)]
        public Vector2 FogStartAndEnd { get; set; } = new Vector2(0f, -2000f);
        [MetaProperty("fogEnabled", BinPropertyType.Bool)]
        public bool FogEnabled { get; set; } = true;
        [MetaProperty("lightMapColorScale", BinPropertyType.Float)]
        public float LightMapColorScale { get; set; } = 1f;
        [MetaProperty(2689325503, BinPropertyType.Optional)]
        public MetaOptional<Vector3> m2689325503 { get; set; } = new MetaOptional<Vector3>(default(Vector3), false);
        [MetaProperty("skyLightColor", BinPropertyType.Vector4)]
        public Vector4 SkyLightColor { get; set; } = new Vector4(0.7049999833106995f, 0.8799999952316284f, 1f, 1f);
        [MetaProperty("skyLightScale", BinPropertyType.Float)]
        public float SkyLightScale { get; set; } = 0.20000000298023224f;
        [MetaProperty(3120754966, BinPropertyType.Float)]
        public float m3120754966 { get; set; } = 1f;
        [MetaProperty(3632599555, BinPropertyType.Float)]
        public float m3632599555 { get; set; } = 0f;
        [MetaProperty("sunDirection", BinPropertyType.Vector3)]
        public Vector3 SunDirection { get; set; } = new Vector3(0f, 0.7070000171661377f, 0.7070000171661377f);
        [MetaProperty("horizonColor", BinPropertyType.Vector4)]
        public Vector4 HorizonColor { get; set; } = new Vector4(0.4000000059604645f, 0.4000000059604645f, 0.4000000059604645f, 1f);
    }
    [MetaClass("GameModeChampionList")]
    public class GameModeChampionList : IMetaClass
    {
        [MetaProperty("mChampions", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaHash> Champions { get; set; } = new();
    }
    [MetaClass("ViewPaneDefinition")]
    public class ViewPaneDefinition : IMetaClass
    {
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash ObjectPath { get; set; } = new(0);
        [MetaProperty("dragRegionElement", BinPropertyType.Hash)]
        public MetaHash DragRegionElement { get; set; } = new(0);
        [MetaProperty("scissorRegionElement", BinPropertyType.Hash)]
        public MetaHash ScissorRegionElement { get; set; } = new(0);
        [MetaProperty(1778722188, BinPropertyType.Hash)]
        public MetaHash m1778722188 { get; set; } = new(0);
        [MetaProperty("scrollingScene", BinPropertyType.Hash)]
        public MetaHash ScrollingScene { get; set; } = new(0);
        [MetaProperty("scrollDirection", BinPropertyType.Byte)]
        public byte ScrollDirection { get; set; } = 1;
        [MetaProperty("scrollRegionElement", BinPropertyType.Hash)]
        public MetaHash ScrollRegionElement { get; set; } = new(0);
    }
    [MetaClass("HasBuffDynamicMaterialBoolDriver")]
    public class HasBuffDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mScriptName", BinPropertyType.String)]
        public string ScriptName { get; set; } = "";
        [MetaProperty(4286635898, BinPropertyType.Float)]
        public float m4286635898 { get; set; } = 0f;
    }
    [MetaClass("FxActionMoveTo")]
    public class FxActionMoveTo : FxActionMoveBase
    {
        [MetaProperty("OvershootDistance", BinPropertyType.Float)]
        public float OvershootDistance { get; set; } = 0f;
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        public byte EasingType { get; set; } = 0;
        [MetaProperty("FaceVelocity", BinPropertyType.Bool)]
        public bool FaceVelocity { get; set; } = true;
        [MetaProperty("TargetObject", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTarget> TargetObject { get; set; } = new (new ());
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("Destination", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> Destination { get; set; } = new (new ());
    }
    [MetaClass("Character")]
    public class Character : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
    }
    [MetaClass("EffectCreationData")]
    public class EffectCreationData : IMetaClass
    {
        [MetaProperty(1161004262, BinPropertyType.Bool)]
        public bool m1161004262 { get; set; } = false;
        [MetaProperty("mEffectName", BinPropertyType.String)]
        public string EffectName { get; set; } = "";
        [MetaProperty(1660255353, BinPropertyType.Bool)]
        public bool m1660255353 { get; set; } = false;
        [MetaProperty(2270784147, BinPropertyType.UInt32)]
        public uint m2270784147 { get; set; } = 0;
        [MetaProperty("mFaceTarget", BinPropertyType.Bool)]
        public bool FaceTarget { get; set; } = false;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2688193858, BinPropertyType.Hash)]
        public MetaHash m2688193858 { get; set; } = new(0);
        [MetaProperty(2757679739, BinPropertyType.Bool)]
        public bool m2757679739 { get; set; } = false;
        [MetaProperty(3291281549, BinPropertyType.Bool)]
        public bool m3291281549 { get; set; } = false;
        [MetaProperty("mBoneName", BinPropertyType.String)]
        public string BoneName { get; set; } = "";
        [MetaProperty("mPlaySpeedModifier", BinPropertyType.Float)]
        public float PlaySpeedModifier { get; set; } = 1f;
        [MetaProperty("mEffectKey", BinPropertyType.Hash)]
        public MetaHash EffectKey { get; set; } = new(0);
        [MetaProperty(4246608820, BinPropertyType.Bool)]
        public bool m4246608820 { get; set; } = false;
        [MetaProperty(4269114704, BinPropertyType.String)]
        public string m4269114704 { get; set; } = "";
    }
    [MetaClass("SkinMeshDataProperties_MaterialOverride")]
    public class SkinMeshDataProperties_MaterialOverride : IMetaClass
    {
        [MetaProperty("texture", BinPropertyType.String)]
        public string Texture { get; set; } = "";
        [MetaProperty("submesh", BinPropertyType.String)]
        public string Submesh { get; set; } = "";
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink Material { get; set; } = new(0);
        [MetaProperty("glossTexture", BinPropertyType.String)]
        public string GlossTexture { get; set; } = "";
    }
    [MetaClass("HudStatPanelStatStoneData")]
    public class HudStatPanelStatStoneData : IMetaClass
    {
        [MetaProperty("mSlideTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SlideTransitionIn { get; set; } = new (new ());
        [MetaProperty(1256611322, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m1256611322 { get; set; } = new (new ());
        [MetaProperty("mAnimationDelayTime", BinPropertyType.Float)]
        public float AnimationDelayTime { get; set; } = 0f;
        [MetaProperty(2010657113, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2010657113 { get; set; } = new (new ());
        [MetaProperty("mSlideTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SlideTransitionOut { get; set; } = new (new ());
        [MetaProperty(3397678954, BinPropertyType.Float)]
        public float m3397678954 { get; set; } = 0f;
        [MetaProperty(4134905527, BinPropertyType.Float)]
        public float m4134905527 { get; set; } = 0f;
    }
    [MetaClass("EffectValueCalculationPart")]
    public class EffectValueCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mEffectIndex", BinPropertyType.Int32)]
        public int EffectIndex { get; set; } = 0;
    }
    [MetaClass("GenericMapPlaceable")]
    public class GenericMapPlaceable : MapPlaceable
    {
    }
    [MetaClass(2349695221)]
    public class Class0x8c0d80f5 : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty("MapParticleName", BinPropertyType.Container)]
        public MetaContainer<string> MapParticleName { get; set; } = new();
        [MetaProperty("Shuffle", BinPropertyType.Bool)]
        public bool Shuffle { get; set; } = false;
    }
    [MetaClass("IndicatorTypeGlobal")]
    public class IndicatorTypeGlobal : ILineIndicatorType
    {
        [MetaProperty("mIsFloating", BinPropertyType.Bool)]
        public bool IsFloating { get; set; } = false;
    }
    [MetaClass("SequencerClipData")]
    public class SequencerClipData : ClipBaseData
    {
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mClipNameList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ClipNameList { get; set; } = new();
    }
    [MetaClass("CompanionSpeciesData")]
    public class CompanionSpeciesData : IMetaClass
    {
        [MetaProperty("mSpeciesId", BinPropertyType.UInt32)]
        public uint SpeciesId { get; set; } = 0;
        [MetaProperty("mSpeciesName", BinPropertyType.String)]
        public string SpeciesName { get; set; } = "";
    }
    [MetaClass("Breakpoint")]
    public class Breakpoint : IMetaClass
    {
        [MetaProperty(1476248632, BinPropertyType.Float)]
        public float m1476248632 { get; set; } = 0f;
        [MetaProperty("mLevel", BinPropertyType.UInt32)]
        public uint Level { get; set; } = 1;
        [MetaProperty(3590129645, BinPropertyType.Float)]
        public float m3590129645 { get; set; } = 0f;
    }
    [MetaClass("CharacterPassiveData")]
    public class CharacterPassiveData : IMetaClass
    {
        [MetaProperty("mComponentBuffs", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ComponentBuffs { get; set; } = new();
        [MetaProperty("mDisplayFlags", BinPropertyType.Byte)]
        public byte DisplayFlags { get; set; } = 0;
        [MetaProperty(1665575238, BinPropertyType.Bool)]
        public bool m1665575238 { get; set; } = true;
        [MetaProperty("SkinFilter", BinPropertyType.Structure)]
        public SkinFilterData SkinFilter { get; set; } = null;
        [MetaProperty(3174838756, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3174838756 { get; set; } = new(0);
        [MetaProperty("mChildSpells", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ChildSpells { get; set; } = new();
    }
    [MetaClass("ConditionFloatClipData")]
    public class ConditionFloatClipData : ClipBaseData
    {
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mUpdaterType", BinPropertyType.UInt32)]
        public uint UpdaterType { get; set; } = 4294967295;
        [MetaProperty("mConditionFloatPairDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ConditionFloatPairData>> ConditionFloatPairDataList { get; set; } = new();
        [MetaProperty(836456042, BinPropertyType.Bool)]
        public bool m836456042 { get; set; } = false;
        [MetaProperty("mChangeAnimationMidPlay", BinPropertyType.Bool)]
        public bool ChangeAnimationMidPlay { get; set; } = false;
        [MetaProperty(2451652078, BinPropertyType.Bool)]
        public bool m2451652078 { get; set; } = false;
        [MetaProperty("mChildAnimDelaySwitchTime", BinPropertyType.Float)]
        public float ChildAnimDelaySwitchTime { get; set; } = 0f;
    }
    [MetaClass(2401135138)]
    public class Class0x8f1e6a22 : Class0xd1951f45
    {
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float TransitionTime { get; set; } = 0.10000000149011612f;
        [MetaProperty("endAlpha", BinPropertyType.Byte)]
        public byte EndAlpha { get; set; } = 255;
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        public byte EasingType { get; set; } = 0;
        [MetaProperty("startAlpha", BinPropertyType.Byte)]
        public byte StartAlpha { get; set; } = 0;
    }
    [MetaClass("DefaultSplashedPerkStyle")]
    public class DefaultSplashedPerkStyle : IMetaClass
    {
        [MetaProperty("mPerk1", BinPropertyType.ObjectLink)]
        public MetaObjectLink Perk1 { get; set; } = new(0);
        [MetaProperty("mPerk2", BinPropertyType.ObjectLink)]
        public MetaObjectLink Perk2 { get; set; } = new(0);
        [MetaProperty("mStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink Style { get; set; } = new(0);
    }
    [MetaClass("SkinAudioProperties")]
    public class SkinAudioProperties : IMetaClass
    {
        [MetaProperty("tagEventList", BinPropertyType.Container)]
        public MetaContainer<string> TagEventList { get; set; } = new();
        [MetaProperty("bankUnits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BankUnit>> BankUnits { get; set; } = new();
    }
    [MetaClass("WallFollowMovement")]
    public class WallFollowMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool ProjectTargetToCastRange { get; set; } = false;
        [MetaProperty(948380516, BinPropertyType.Bool)]
        public bool m948380516 { get; set; } = false;
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float Speed { get; set; } = 0f;
        [MetaProperty("mCounterClockwise", BinPropertyType.Bool)]
        public bool CounterClockwise { get; set; } = false;
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool UseGroundHeightAtTarget { get; set; } = true;
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool InferDirectionFromFacingIfNeeded { get; set; } = true;
        [MetaProperty("mWallOffset", BinPropertyType.Float)]
        public float WallOffset { get; set; } = 0f;
        [MetaProperty("mWallLength", BinPropertyType.Float)]
        public float WallLength { get; set; } = 1000f;
        [MetaProperty(3170840289, BinPropertyType.Bool)]
        public bool m3170840289 { get; set; } = false;
        [MetaProperty("mWallSearchRadius", BinPropertyType.Float)]
        public float WallSearchRadius { get; set; } = 200f;
    }
    [MetaClass("HasBuffSpawnConditionData")]
    public class HasBuffSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; } = new();
        [MetaProperty("mBuffComparisons", BinPropertyType.Embedded)]
        public MetaEmbedded<HasBuffComparisonData> BuffComparisons { get; set; } = new (new ());
    }
    [MetaClass("RegaliaRankedCrestMap")]
    public class RegaliaRankedCrestMap : IMetaClass
    {
        [MetaProperty(1916628881, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<RegaliaRankedCrestEntry>> m1916628881 { get; set; } = new();
    }
    [MetaClass("FloorFloatMaterialDriver")]
    public class FloorFloatMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; } = null;
    }
    [MetaClass("StaticMaterialShaderSamplerDef")]
    public class StaticMaterialShaderSamplerDef : IMetaClass
    {
        [MetaProperty("addressV", BinPropertyType.UInt32)]
        public uint AddressV { get; set; } = 0;
        [MetaProperty("addressU", BinPropertyType.UInt32)]
        public uint AddressU { get; set; } = 0;
        [MetaProperty("filterMin", BinPropertyType.UInt32)]
        public uint FilterMin { get; set; } = 2;
        [MetaProperty("filterMip", BinPropertyType.UInt32)]
        public uint FilterMip { get; set; } = 0;
        [MetaProperty("samplerName", BinPropertyType.String)]
        public string SamplerName { get; set; } = "";
        [MetaProperty("filterMag", BinPropertyType.UInt32)]
        public uint FilterMag { get; set; } = 2;
        [MetaProperty("uncensoredTextures", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredTextures { get; set; } = new();
        [MetaProperty("textureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
        [MetaProperty("addressW", BinPropertyType.UInt32)]
        public uint AddressW { get; set; } = 0;
    }
    [MetaClass("FloatComparisonMaterialDriver")]
    public class FloatComparisonMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mValueA", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver ValueA { get; set; } = null;
        [MetaProperty("mValueB", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver ValueB { get; set; } = null;
        [MetaProperty("mOperator", BinPropertyType.UInt32)]
        public uint Operator { get; set; } = 0;
    }
    [MetaClass("FadeOverTimeBehavior")]
    public class FadeOverTimeBehavior : ITargeterFadeBehavior
    {
        [MetaProperty("mStartAlpha", BinPropertyType.Float)]
        public float StartAlpha { get; set; } = 1f;
        [MetaProperty("mTimeEnd", BinPropertyType.Float)]
        public float TimeEnd { get; set; } = 0f;
        [MetaProperty("mEndAlpha", BinPropertyType.Float)]
        public float EndAlpha { get; set; } = 1f;
        [MetaProperty("mTimeStart", BinPropertyType.Float)]
        public float TimeStart { get; set; } = 0f;
    }
    [MetaClass("DistanceToPlayerMaterialFloatDriver")]
    public class DistanceToPlayerMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("minDistance", BinPropertyType.Float)]
        public float MinDistance { get; set; } = 100f;
        [MetaProperty("maxDistance", BinPropertyType.Float)]
        public float MaxDistance { get; set; } = 1000f;
    }
    [MetaClass(2449846901)]
    public class Class0x9205b275 : Class0xef05ba42
    {
        [MetaProperty("AdviceLabel", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceLabel { get; set; } = new(0);
        [MetaProperty("CardHoverMythic", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardHoverMythic { get; set; } = new(0);
        [MetaProperty("AdviceEmptyTextHover", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceEmptyTextHover { get; set; } = new(0);
        [MetaProperty("CardRefreshNonMythicVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardRefreshNonMythicVfx { get; set; } = new(0);
        [MetaProperty("AdviceCharBorderHover1", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceCharBorderHover1 { get; set; } = new(0);
        [MetaProperty("AdviceCharBorderHover0", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceCharBorderHover0 { get; set; } = new(0);
        [MetaProperty("BundleItemIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink BundleItemIcon { get; set; } = new(0);
        [MetaProperty("CardSelectedNonMythic", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardSelectedNonMythic { get; set; } = new(0);
        [MetaProperty("CardHoverNonMythic", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardHoverNonMythic { get; set; } = new(0);
        [MetaProperty("BriefText", BinPropertyType.ObjectLink)]
        public MetaObjectLink BriefText { get; set; } = new(0);
        [MetaProperty(1509670169, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1509670169 { get; set; } = new(0);
        [MetaProperty("BundleStackText", BinPropertyType.ObjectLink)]
        public MetaObjectLink BundleStackText { get; set; } = new(0);
        [MetaProperty("BundleItemFrameHoverIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink BundleItemFrameHoverIcon { get; set; } = new(0);
        [MetaProperty("BundleItemFrameIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink BundleItemFrameIcon { get; set; } = new(0);
        [MetaProperty("CardHoverMythicVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardHoverMythicVfx { get; set; } = new(0);
        [MetaProperty("CardDefault", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardDefault { get; set; } = new(0);
        [MetaProperty(2506512462, BinPropertyType.ObjectLink)]
        public MetaObjectLink m2506512462 { get; set; } = new(0);
        [MetaProperty(163752404, BinPropertyType.ObjectLink)]
        public MetaObjectLink m163752404 { get; set; } = new(0);
        [MetaProperty("CardSelectedMythic", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardSelectedMythic { get; set; } = new(0);
        [MetaProperty("AdviceEmptyIconDefault", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceEmptyIconDefault { get; set; } = new(0);
        [MetaProperty("CardHoverNonMythicVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardHoverNonMythicVfx { get; set; } = new(0);
        [MetaProperty("PlusIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink PlusIcon { get; set; } = new(0);
        [MetaProperty("AdviceIconDefault", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceIconDefault { get; set; } = new(0);
        [MetaProperty(3331072719, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3331072719 { get; set; } = new(0);
        [MetaProperty("AdviceCharBorder1", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceCharBorder1 { get; set; } = new(0);
        [MetaProperty("AdviceCharBorder0", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceCharBorder0 { get; set; } = new(0);
        [MetaProperty("CardSelectedNonMythicVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardSelectedNonMythicVfx { get; set; } = new(0);
        [MetaProperty("CardSelectedMythicVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardSelectedMythicVfx { get; set; } = new(0);
        [MetaProperty("BriefTextBackdrop", BinPropertyType.ObjectLink)]
        public MetaObjectLink BriefTextBackdrop { get; set; } = new(0);
        [MetaProperty("AdviceEmptyText", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceEmptyText { get; set; } = new(0);
        [MetaProperty("CardRefreshMythicVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink CardRefreshMythicVfx { get; set; } = new(0);
        [MetaProperty("AdviceCharIcon0", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceCharIcon0 { get; set; } = new(0);
        [MetaProperty("AdviceCharIcon1", BinPropertyType.ObjectLink)]
        public MetaObjectLink AdviceCharIcon1 { get; set; } = new(0);
    }
    [MetaClass("OptionItemHotkeys")]
    public class OptionItemHotkeys : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
    }
    [MetaClass("AllTrueMaterialDriver")]
    public class AllTrueMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialBoolDriver> Drivers { get; set; } = new();
    }
    [MetaClass("EnterFOWVisibility")]
    public class EnterFOWVisibility : MissileVisibilitySpec
    {
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        public float PerceptionBubbleRadius { get; set; } = 0f;
        [MetaProperty("mTargetControlsVisibility", BinPropertyType.Bool)]
        public bool TargetControlsVisibility { get; set; } = false;
        [MetaProperty("mVisibleToOwnerTeamOnly", BinPropertyType.Bool)]
        public bool VisibleToOwnerTeamOnly { get; set; } = false;
        [MetaProperty(3143864407, BinPropertyType.Float)]
        public float m3143864407 { get; set; } = 0f;
        [MetaProperty("mMissileClientWaitForTargetUpdateBeforeMissileShow", BinPropertyType.Bool)]
        public bool MissileClientWaitForTargetUpdateBeforeMissileShow { get; set; } = false;
        [MetaProperty("mMissileClientExitFOWPrediction", BinPropertyType.Bool)]
        public bool MissileClientExitFOWPrediction { get; set; } = false;
    }
    [MetaClass("SpellEffectAmount")]
    public class SpellEffectAmount : IMetaClass
    {
        [MetaProperty("value", BinPropertyType.Container)]
        public MetaContainer<float> Value { get; set; } = new();
    }
    [MetaClass("VectorTableSet")]
    public class VectorTableSet : ScriptTableSet
    {
    }
    [MetaClass("ICatalogEntryOwner")]
    public interface ICatalogEntryOwner : IMetaClass
    {
    }
    [MetaClass("ExperienceCurveData")]
    public class ExperienceCurveData : IMetaClass
    {
        [MetaProperty("mLevelDifferenceExperienceMultiplier", BinPropertyType.Float)]
        public float LevelDifferenceExperienceMultiplier { get; set; } = 0.15000000596046448f;
        [MetaProperty(2575366702, BinPropertyType.Container)]
        public MetaContainer<float> m2575366702 { get; set; } = new();
        [MetaProperty("mExperienceGrantedForKillPerLevel", BinPropertyType.Container)]
        public MetaContainer<float> ExperienceGrantedForKillPerLevel { get; set; } = new();
        [MetaProperty("mExperienceRequiredPerLevel", BinPropertyType.Container)]
        public MetaContainer<float> ExperienceRequiredPerLevel { get; set; } = new();
        [MetaProperty("mMinimumExperienceMultiplier", BinPropertyType.Float)]
        public float MinimumExperienceMultiplier { get; set; } = 0.4000000059604645f;
        [MetaProperty("mBaseExperienceMultiplier", BinPropertyType.Float)]
        public float BaseExperienceMultiplier { get; set; } = 1f;
    }
    [MetaClass("RegionsThatAllowContent")]
    public class RegionsThatAllowContent : IMetaClass
    {
        [MetaProperty("mRegionList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> RegionList { get; set; } = new();
    }
    [MetaClass("Cheat")]
    public class Cheat : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mRecastFrequency", BinPropertyType.Float)]
        public float RecastFrequency { get; set; } = -1f;
        [MetaProperty("mCheatMenuUIData", BinPropertyType.Structure)]
        public CheatMenuUIData CheatMenuUIData { get; set; } = null;
        [MetaProperty("mIsPlayerFacing", BinPropertyType.Bool)]
        public bool IsPlayerFacing { get; set; } = false;
    }
    [MetaClass("LootOutputBase")]
    public interface LootOutputBase : IMetaClass
    {
    }
    [MetaClass("TFTStreakData")]
    public class TFTStreakData : IMetaClass
    {
        [MetaProperty("mLossStreaks", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTStreak>> LossStreaks { get; set; } = new();
        [MetaProperty(3287630061, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTStreak>> m3287630061 { get; set; } = new();
    }
    [MetaClass("ContextualConditionCharacterMetadata")]
    public class ContextualConditionCharacterMetadata : ICharacterSubcondition
    {
        [MetaProperty("mData", BinPropertyType.String)]
        public string Data { get; set; } = "";
        [MetaProperty("mCategory", BinPropertyType.String)]
        public string Category { get; set; } = "";
    }
    [MetaClass("BaseLoadoutData")]
    public interface BaseLoadoutData : IMetaClass,  ICatalogEntryOwner
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        string DescriptionTraKey { get; set; }
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        string NameTraKey { get; set; }
    }
    [MetaClass("X3DSharedConstantBufferDef")]
    public class X3DSharedConstantBufferDef : IMetaClass
    {
        [MetaProperty("register", BinPropertyType.Int32)]
        public int Register { get; set; } = -1;
        [MetaProperty("frequency", BinPropertyType.UInt32)]
        public uint Frequency { get; set; } = 0;
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(2825931196, BinPropertyType.Bool)]
        public bool m2825931196 { get; set; } = false;
        [MetaProperty("constants", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<X3DSharedConstantDef>> Constants { get; set; } = new();
        [MetaProperty("PlatformMask", BinPropertyType.UInt32)]
        public uint PlatformMask { get; set; } = 0;
    }
    [MetaClass("NeutralMinionCampClearedLogic")]
    public class NeutralMinionCampClearedLogic : IStatStoneLogicDriver
    {
    }
    [MetaClass("StatStoneSet")]
    public class StatStoneSet : IMetaClass,  ICatalogEntryOwner
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("statStones", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> StatStones { get; set; } = new();
    }
    [MetaClass("VfxChildIdentifier")]
    public class VfxChildIdentifier : IMetaClass
    {
        [MetaProperty("effectName", BinPropertyType.String)]
        public string EffectName { get; set; } = "";
        [MetaProperty("effect", BinPropertyType.ObjectLink)]
        public MetaObjectLink Effect { get; set; } = new(0);
        [MetaProperty("effectKey", BinPropertyType.Hash)]
        public MetaHash EffectKey { get; set; } = new(0);
    }
    [MetaClass("GravityHeightSolver")]
    public class GravityHeightSolver : HeightSolverType
    {
        [MetaProperty("mGravity", BinPropertyType.Float)]
        public float Gravity { get; set; } = 0f;
    }
    [MetaClass("VfxPrimitiveCameraQuad")]
    public class VfxPrimitiveCameraQuad : VfxPrimitiveBase
    {
    }
    [MetaClass("ToonInkingFilterParams")]
    public class ToonInkingFilterParams : IMetaClass
    {
        [MetaProperty("mResultScale", BinPropertyType.Float)]
        public float ResultScale { get; set; } = 0f;
        [MetaProperty("mPixelSize", BinPropertyType.Float)]
        public float PixelSize { get; set; } = 0f;
        [MetaProperty("mMaxVal", BinPropertyType.Float)]
        public float MaxVal { get; set; } = 0f;
        [MetaProperty("mMinVal", BinPropertyType.Float)]
        public float MinVal { get; set; } = 0f;
    }
    [MetaClass(2539231955)]
    public class Class0x97599ad3 : IMetaClass
    {
        [MetaProperty(697394402, BinPropertyType.Hash)]
        public MetaHash m697394402 { get; set; } = new(0);
        [MetaProperty(1665946782, BinPropertyType.Hash)]
        public MetaHash m1665946782 { get; set; } = new(0);
    }
    [MetaClass("GameModeConstantFloat")]
    public class GameModeConstantFloat : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
    }
    [MetaClass("TFTItemList")]
    public class TFTItemList : IMetaClass
    {
        [MetaProperty("VfxResourceResolver", BinPropertyType.ObjectLink)]
        public MetaObjectLink VfxResourceResolver { get; set; } = new(0);
        [MetaProperty("mItems", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Items { get; set; } = new();
    }
    [MetaClass("TrackData")]
    public class TrackData : IMetaClass
    {
        [MetaProperty("mBlendMode", BinPropertyType.UInt32)]
        public uint BlendMode { get; set; } = 0;
        [MetaProperty("mBlendWeight", BinPropertyType.Float)]
        public float BlendWeight { get; set; } = 1f;
        [MetaProperty("mPriority", BinPropertyType.UInt32)]
        public uint Priority { get; set; } = 0;
    }
    [MetaClass("ItemRecommendationOverrideStartingItemSet")]
    public class ItemRecommendationOverrideStartingItemSet : IMetaClass
    {
        [MetaProperty("mStartingItems", BinPropertyType.Container)]
        public MetaContainer<MetaHash> StartingItems { get; set; } = new();
    }
    [MetaClass("VelocityDynamicMaterialFloatDriver")]
    public class VelocityDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass("ContextualConditionMapID")]
    public class ContextualConditionMapID : IContextualCondition
    {
        [MetaProperty("mMapIDs", BinPropertyType.UInt32)]
        public uint MapIDs { get; set; } = 0;
    }
    [MetaClass("CreateFunctionBlock")]
    public class CreateFunctionBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("Function", BinPropertyType.Embedded)]
        public MetaEmbedded<FunctionTableSet> Function { get; set; } = new (new ());
        [MetaProperty("FunctionDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<FunctionDefinition> FunctionDefinition { get; set; } = new (new ());
    }
    [MetaClass("SubPartScaledProportionalToStat")]
    public class SubPartScaledProportionalToStat : IGameCalculationPart
    {
        [MetaProperty("mSubpart", BinPropertyType.Structure)]
        public IGameCalculationPart Subpart { get; set; } = null;
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte Stat { get; set; } = 0;
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte StatFormula { get; set; } = 0;
        [MetaProperty("mStyleTag", BinPropertyType.String)]
        public string StyleTag { get; set; } = "";
        [MetaProperty(2775882578, BinPropertyType.String)]
        public string m2775882578 { get; set; } = "";
        [MetaProperty("mRatio", BinPropertyType.Float)]
        public float Ratio { get; set; } = 0f;
    }
    [MetaClass("Companion")]
    public class Companion : Character
    {
    }
    [MetaClass("MapPathLineSegment")]
    public class MapPathLineSegment : MapPathSegment
    {
        [MetaProperty("EndPosition", BinPropertyType.Vector3)]
        public Vector3 EndPosition { get; set; } = new Vector3(0f, 0f, 0f);
    }
    [MetaClass("CheatPage")]
    public class CheatPage : IMetaClass
    {
        [MetaProperty("mCheats", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Cheats { get; set; } = new();
    }
    [MetaClass(161271356)]
    public class Class0x99cce3c : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("ZoomEase", BinPropertyType.Byte)]
        public byte ZoomEase { get; set; } = 2;
    }
    [MetaClass("NumberFormattingData")]
    public class NumberFormattingData : IMetaClass
    {
        [MetaProperty(19785452, BinPropertyType.String)]
        public string m19785452 { get; set; } = "";
        [MetaProperty(823565823, BinPropertyType.String)]
        public string m823565823 { get; set; } = "";
        [MetaProperty(1089846550, BinPropertyType.String)]
        public string m1089846550 { get; set; } = "";
        [MetaProperty(1535520071, BinPropertyType.String)]
        public string m1535520071 { get; set; } = "";
        [MetaProperty(1880587249, BinPropertyType.String)]
        public string m1880587249 { get; set; } = "";
        [MetaProperty(2051901883, BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> m2051901883 { get; set; } = new();
        [MetaProperty(2309425659, BinPropertyType.String)]
        public string m2309425659 { get; set; } = "";
        [MetaProperty(2965779045, BinPropertyType.String)]
        public string m2965779045 { get; set; } = "";
        [MetaProperty(3113614111, BinPropertyType.String)]
        public string m3113614111 { get; set; } = "";
        [MetaProperty(3274771674, BinPropertyType.String)]
        public string m3274771674 { get; set; } = "";
        [MetaProperty(3515031867, BinPropertyType.String)]
        public string m3515031867 { get; set; } = "";
        [MetaProperty(3710897474, BinPropertyType.String)]
        public string m3710897474 { get; set; } = "";
        [MetaProperty(3841310158, BinPropertyType.String)]
        public string m3841310158 { get; set; } = "";
        [MetaProperty(3990416003, BinPropertyType.String)]
        public string m3990416003 { get; set; } = "";
        [MetaProperty(4018485617, BinPropertyType.String)]
        public string m4018485617 { get; set; } = "";
        [MetaProperty(4092495889, BinPropertyType.String)]
        public string m4092495889 { get; set; } = "";
        [MetaProperty(4252791735, BinPropertyType.String)]
        public string m4252791735 { get; set; } = "";
    }
    [MetaClass("DeathTimesScalingPoint")]
    public class DeathTimesScalingPoint : IMetaClass
    {
        [MetaProperty("mPercentIncrease", BinPropertyType.Float)]
        public float PercentIncrease { get; set; } = 0f;
        [MetaProperty("mStartTime", BinPropertyType.UInt32)]
        public uint StartTime { get; set; } = 0;
    }
    [MetaClass("TooltipInstanceListElement")]
    public class TooltipInstanceListElement : IMetaClass
    {
        [MetaProperty("typeIndex", BinPropertyType.Int32)]
        public int TypeIndex { get; set; } = -1;
        [MetaProperty("type", BinPropertyType.String)]
        public string Type { get; set; } = "";
        [MetaProperty("nameOverride", BinPropertyType.String)]
        public string NameOverride { get; set; } = "";
        [MetaProperty("Style", BinPropertyType.UInt32)]
        public uint Style { get; set; } = 0;
        [MetaProperty("multiplier", BinPropertyType.Float)]
        public float Multiplier { get; set; } = 1f;
    }
    [MetaClass("PatchingViewController")]
    public class PatchingViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("BeginPatchingButtonDefinition", BinPropertyType.Hash)]
        public MetaHash BeginPatchingButtonDefinition { get; set; } = new(0);
        [MetaProperty("OpenTeamPlannerButtonDefinition", BinPropertyType.Hash)]
        public MetaHash OpenTeamPlannerButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("AnimationResourceData")]
    public class AnimationResourceData : IMetaClass
    {
        [MetaProperty("mAnimationFilePath", BinPropertyType.String)]
        public string AnimationFilePath { get; set; } = "";
    }
    [MetaClass("CustomReductionMultiplierCalculationPart")]
    public class CustomReductionMultiplierCalculationPart : IGameCalculationPart
    {
        [MetaProperty(1854058873, BinPropertyType.Structure)]
        public IGameCalculationPart m1854058873 { get; set; } = null;
        [MetaProperty("mMaximumReductionPercent", BinPropertyType.Float)]
        public float MaximumReductionPercent { get; set; } = 0f;
    }
    [MetaClass("FontLocaleType")]
    public class FontLocaleType : IMetaClass
    {
        [MetaProperty("FontFilePathBold", BinPropertyType.String)]
        public string FontFilePathBold { get; set; } = "";
        [MetaProperty("localeName", BinPropertyType.String)]
        public string LocaleName { get; set; } = "en_us";
        [MetaProperty("FontFilePathItalics", BinPropertyType.String)]
        public string FontFilePathItalics { get; set; } = "";
        [MetaProperty("mFontFilePath", BinPropertyType.String)]
        public string FontFilePath { get; set; } = "";
    }
    [MetaClass("Area")]
    public class Area : TargetingTypeData
    {
    }
    [MetaClass("MapPathSegment")]
    public interface MapPathSegment : IMetaClass
    {
        [MetaProperty("EndPosition", BinPropertyType.Vector3)]
        Vector3 EndPosition { get; set; }
    }
    [MetaClass("SpellLevelUpInfo")]
    public class SpellLevelUpInfo : IMetaClass
    {
        [MetaProperty("mRequirements", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellRankUpRequirements>> Requirements { get; set; } = new();
    }
    [MetaClass("SkinCharacterDataProperties")]
    public class SkinCharacterDataProperties : IMetaClass
    {
        [MetaProperty("defaultAnimations", BinPropertyType.Container)]
        public MetaContainer<string> DefaultAnimations { get; set; } = new();
        [MetaProperty("skipVOOverride", BinPropertyType.Bool)]
        public bool SkipVOOverride { get; set; } = false;
        [MetaProperty("mOptionalBin", BinPropertyType.Embedded)]
        public MetaEmbedded<HudOptionalBinData> OptionalBin { get; set; } = new (new ());
        [MetaProperty("particleOverride_ChampionKillDeathParticle", BinPropertyType.String)]
        public string ParticleOverride_ChampionKillDeathParticle { get; set; } = "";
        [MetaProperty("HudMuteEvent", BinPropertyType.String)]
        public string HudMuteEvent { get; set; } = "";
        [MetaProperty("iconCircleScale", BinPropertyType.Optional)]
        public MetaOptional<float> IconCircleScale { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty(637119090, BinPropertyType.UInt32)]
        public uint m637119090 { get; set; } = 0;
        [MetaProperty("emoteBuffbone", BinPropertyType.String)]
        public string EmoteBuffbone { get; set; } = "Root";
        [MetaProperty("endOfGameAlias", BinPropertyType.String)]
        public string EndOfGameAlias { get; set; } = "";
        [MetaProperty("armorMaterial", BinPropertyType.String)]
        public string ArmorMaterial { get; set; } = "";
        [MetaProperty("championSkinName", BinPropertyType.String)]
        public string ChampionSkinName { get; set; } = "";
        [MetaProperty("alternateIconsCircle", BinPropertyType.Container)]
        public MetaContainer<string> AlternateIconsCircle { get; set; } = new();
        [MetaProperty("HudUnmuteEvent", BinPropertyType.String)]
        public string HudUnmuteEvent { get; set; } = "";
        [MetaProperty("skinAnimationProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinAnimationProperties> SkinAnimationProperties { get; set; } = new (new ());
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; } = new (new ());
        [MetaProperty(1182316791, BinPropertyType.String)]
        public string m1182316791 { get; set; } = "";
        [MetaProperty("attributeFlags", BinPropertyType.UInt32)]
        public uint AttributeFlags { get; set; } = 0;
        [MetaProperty("skinParent", BinPropertyType.Int32)]
        public int SkinParent { get; set; } = -1;
        [MetaProperty("healthBarData", BinPropertyType.Embedded)]
        public MetaEmbedded<CharacterHealthBarDataRecord> HealthBarData { get; set; } = new (new ());
        [MetaProperty("themeMusic", BinPropertyType.Container)]
        public MetaContainer<string> ThemeMusic { get; set; } = new();
        [MetaProperty("skinAudioNameOverride", BinPropertyType.String)]
        public string SkinAudioNameOverride { get; set; } = "";
        [MetaProperty(93493004, BinPropertyType.Bool)]
        public bool m93493004 { get; set; } = true;
        [MetaProperty("godrayFXbone", BinPropertyType.String)]
        public string GodrayFXbone { get; set; } = "BUFFBONE_GLB_GROUND_LOC";
        [MetaProperty("mResourceResolver", BinPropertyType.ObjectLink)]
        public MetaObjectLink ResourceResolver { get; set; } = new(0);
        [MetaProperty("extraCharacterPreloads", BinPropertyType.Container)]
        public MetaContainer<string> ExtraCharacterPreloads { get; set; } = new();
        [MetaProperty("skinUpgradeData", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinUpgradeData> SkinUpgradeData { get; set; } = new (new ());
        [MetaProperty("voiceOverOverride", BinPropertyType.String)]
        public string VoiceOverOverride { get; set; } = "";
        [MetaProperty("mAdditionalResourceResolvers", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> AdditionalResourceResolvers { get; set; } = new();
        [MetaProperty("idleParticlesEffects", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect>> IdleParticlesEffects { get; set; } = new();
        [MetaProperty("skinClassification", BinPropertyType.UInt32)]
        public uint SkinClassification { get; set; } = 0;
        [MetaProperty("iconAvatar", BinPropertyType.String)]
        public string IconAvatar { get; set; } = "";
        [MetaProperty("uncensoredIconCircles", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredIconCircles { get; set; } = new();
        [MetaProperty("skinAudioProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinAudioProperties> SkinAudioProperties { get; set; } = new (new ());
        [MetaProperty("metaDataTags", BinPropertyType.String)]
        public string MetaDataTags { get; set; } = "";
        [MetaProperty("loadscreen", BinPropertyType.Embedded)]
        public MetaEmbedded<CensoredImage> Loadscreen { get; set; } = new (new ());
        [MetaProperty("uncensoredIconSquares", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredIconSquares { get; set; } = new();
        [MetaProperty("iconSquare", BinPropertyType.Optional)]
        public MetaOptional<string> IconSquare { get; set; } = new MetaOptional<string>(default(string), false);
        [MetaProperty("rarityGemOverride", BinPropertyType.Optional)]
        public MetaOptional<int> RarityGemOverride { get; set; } = new MetaOptional<int>(default(int), false);
        [MetaProperty("alternateIconsSquare", BinPropertyType.Container)]
        public MetaContainer<string> AlternateIconsSquare { get; set; } = new();
        [MetaProperty("mEmblems", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinEmblem>> Emblems { get; set; } = new();
        [MetaProperty("mSpawnParticleName", BinPropertyType.String)]
        public string SpawnParticleName { get; set; } = "";
        [MetaProperty("loadscreenVintage", BinPropertyType.Embedded)]
        public MetaEmbedded<CensoredImage> LoadscreenVintage { get; set; } = new (new ());
        [MetaProperty("mContextualActionData", BinPropertyType.ObjectLink)]
        public MetaObjectLink ContextualActionData { get; set; } = new(0);
        [MetaProperty("secondaryResourceHudDisplayData", BinPropertyType.Structure)]
        public ISecondaryResourceDisplayData SecondaryResourceHudDisplayData { get; set; } = null;
        [MetaProperty("iconCircle", BinPropertyType.Optional)]
        public MetaOptional<string> IconCircle { get; set; } = new MetaOptional<string>(default(string), false);
        [MetaProperty("emoteYOffset", BinPropertyType.Float)]
        public float EmoteYOffset { get; set; } = 0f;
        [MetaProperty("emoteLoadout", BinPropertyType.Hash)]
        public MetaHash EmoteLoadout { get; set; } = new(0);
        [MetaProperty("particleOverride_DeathParticle", BinPropertyType.String)]
        public string ParticleOverride_DeathParticle { get; set; } = "";
    }
    [MetaClass("MissionsPanelViewController")]
    public class MissionsPanelViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty(3967028638, BinPropertyType.Byte)]
        public byte m3967028638 { get; set; } = 19;
        [MetaProperty(4202464323, BinPropertyType.Float)]
        public float m4202464323 { get; set; } = 0.5f;
    }
    [MetaClass("EffectArcFillElementData")]
    public class EffectArcFillElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
    }
    [MetaClass("NotificationSettings")]
    public class NotificationSettings : IMetaClass
    {
        [MetaProperty("defaultSound", BinPropertyType.String)]
        public string DefaultSound { get; set; } = "";
        [MetaProperty(2305194088, BinPropertyType.Map)]
        public Dictionary<byte, string> m2305194088 { get; set; } = new();
    }
    [MetaClass("ContextualConditionMultikillSize")]
    public class ContextualConditionMultikillSize : IContextualCondition
    {
        [MetaProperty("mMultikillSize", BinPropertyType.Byte)]
        public byte MultikillSize { get; set; } = 0;
    }
    [MetaClass("CallOnMissileBounce")]
    public class CallOnMissileBounce : MissileTriggeredActionSpec
    {
    }
    [MetaClass(2614239024)]
    public class Class0x9bd21f30 : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string KeyName { get; set; } = "";
    }
    [MetaClass("SoundEventData")]
    public class SoundEventData : BaseEventData
    {
        [MetaProperty(108144598, BinPropertyType.Bool)]
        public bool m108144598 { get; set; } = false;
        [MetaProperty("mIsKillEvent", BinPropertyType.Bool)]
        public bool IsKillEvent { get; set; } = true;
        [MetaProperty("mSoundName", BinPropertyType.String)]
        public string SoundName { get; set; } = "";
        [MetaProperty("mIsLoop", BinPropertyType.Bool)]
        public bool IsLoop { get; set; } = true;
    }
    [MetaClass("TextElementData")]
    public class TextElementData : BaseElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mTextAlignmentVertical", BinPropertyType.Byte)]
        public byte TextAlignmentVertical { get; set; } = 1;
        [MetaProperty("WrappingMode", BinPropertyType.Byte)]
        public byte WrappingMode { get; set; } = 0;
        [MetaProperty(2081063769, BinPropertyType.ObjectLink)]
        public MetaObjectLink m2081063769 { get; set; } = new(0);
        [MetaProperty("mFontDescription", BinPropertyType.ObjectLink)]
        public MetaObjectLink FontDescription { get; set; } = new(0);
        [MetaProperty("mTextAlignmentHorizontal", BinPropertyType.Byte)]
        public byte TextAlignmentHorizontal { get; set; } = 0;
        [MetaProperty("TraKey", BinPropertyType.String)]
        public string TraKey { get; set; } = "";
        [MetaProperty("iconScale", BinPropertyType.Float)]
        public float IconScale { get; set; } = 1f;
    }
    [MetaClass("OptionItemCheckbox")]
    public class OptionItemCheckbox : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("TooltipTraKey", BinPropertyType.String)]
        public string TooltipTraKey { get; set; } = "";
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("Negate", BinPropertyType.Bool)]
        public bool Negate { get; set; } = false;
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
        [MetaProperty("option", BinPropertyType.UInt16)]
        public ushort Option { get; set; } = 65535;
    }
    [MetaClass("VfxEmitterDefinitionData")]
    public class VfxEmitterDefinitionData : IMetaClass
    {
        [MetaProperty("isRandomStartFrameMult", BinPropertyType.BitBool)]
        public MetaBitBool IsRandomStartFrameMult { get; set; } = new (1);
        [MetaProperty("doesParticleLifetimeScale", BinPropertyType.BitBool)]
        public MetaBitBool DoesParticleLifetimeScale { get; set; } = new (0);
        [MetaProperty("particleUVRotateRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueFloat> ParticleUVRotateRateMult { get; set; } = new (new ());
        [MetaProperty("keywordsRequired", BinPropertyType.Container)]
        public MetaContainer<string> KeywordsRequired { get; set; } = new();
        [MetaProperty("colorRenderFlags", BinPropertyType.Byte)]
        public byte ColorRenderFlags { get; set; } = 0;
        [MetaProperty("texDivMult", BinPropertyType.Vector2)]
        public Vector2 TexDivMult { get; set; } = new Vector2(1f, 1f);
        [MetaProperty("voiceOverPersistentName", BinPropertyType.String)]
        public string VoiceOverPersistentName { get; set; } = "";
        [MetaProperty("timeActiveDuringPeriod", BinPropertyType.Optional)]
        public MetaOptional<float> TimeActiveDuringPeriod { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("meshRenderFlags", BinPropertyType.Byte)]
        public byte MeshRenderFlags { get; set; } = 1;
        [MetaProperty("stencilMode", BinPropertyType.Byte)]
        public byte StencilMode { get; set; } = 0;
        [MetaProperty("useLingerColor", BinPropertyType.BitBool)]
        public MetaBitBool UseLingerColor { get; set; } = new (0);
        [MetaProperty("scaleEmitOffsetByBoundObjectHeight", BinPropertyType.Float)]
        public float ScaleEmitOffsetByBoundObjectHeight { get; set; } = 0f;
        [MetaProperty("birthRotationalVelocity0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthRotationalVelocity0 { get; set; } = new (new ());
        [MetaProperty("numFrames", BinPropertyType.UInt16)]
        public ushort NumFrames { get; set; } = 1;
        [MetaProperty("birthRotationalVelocity1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthRotationalVelocity1 { get; set; } = new (new ());
        [MetaProperty("uvScrollRate1", BinPropertyType.Vector2)]
        public Vector2 UvScrollRate1 { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("emissionMeshName", BinPropertyType.String)]
        public string EmissionMeshName { get; set; } = "";
        [MetaProperty("timeBeforeFirstEmission", BinPropertyType.Float)]
        public float TimeBeforeFirstEmission { get; set; } = 0f;
        [MetaProperty("censorPolicy", BinPropertyType.Byte)]
        public byte CensorPolicy { get; set; } = 0;
        [MetaProperty("particleLinger", BinPropertyType.Optional)]
        public MetaOptional<float> ParticleLinger { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("flexBirthUVOffset", BinPropertyType.Structure)]
        public FlexValueVector2 FlexBirthUVOffset { get; set; } = null;
        [MetaProperty("colorLookUpTypeX", BinPropertyType.Byte)]
        public byte ColorLookUpTypeX { get; set; } = 1;
        [MetaProperty("uvMode", BinPropertyType.Byte)]
        public byte UvMode { get; set; } = 0;
        [MetaProperty("colorLookUpTypeY", BinPropertyType.Byte)]
        public byte ColorLookUpTypeY { get; set; } = 0;
        [MetaProperty("isGroundLayer", BinPropertyType.BitBool)]
        public MetaBitBool IsGroundLayer { get; set; } = new (0);
        [MetaProperty("particleLifetime", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> ParticleLifetime { get; set; } = new (new ());
        [MetaProperty("isLocalOrientation", BinPropertyType.BitBool)]
        public MetaBitBool IsLocalOrientation { get; set; } = new (0);
        [MetaProperty("flexScaleBirthScale", BinPropertyType.Structure)]
        public FlexTypeFloat FlexScaleBirthScale { get; set; } = null;
        [MetaProperty("textureMult", BinPropertyType.String)]
        public string TextureMult { get; set; } = "";
        [MetaProperty("acceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Acceleration { get; set; } = new (new ());
        [MetaProperty("birthUvScrollRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUvScrollRateMult { get; set; } = new (new ());
        [MetaProperty("texAddressModeBase", BinPropertyType.Byte)]
        public byte TexAddressModeBase { get; set; } = 0;
        [MetaProperty("velocity", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Velocity { get; set; } = new (new ());
        [MetaProperty("UseLingerVelocity", BinPropertyType.BitBool)]
        public MetaBitBool UseLingerVelocity { get; set; } = new (0);
        [MetaProperty("birthUvRotateRate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthUvRotateRate { get; set; } = new (new ());
        [MetaProperty("materialOverrideDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxMaterialOverrideDefinitionData>> MaterialOverrideDefinitions { get; set; } = new();
        [MetaProperty("disabled", BinPropertyType.Bool)]
        public bool Disabled { get; set; } = false;
        [MetaProperty("hasFixedOrbit", BinPropertyType.BitBool)]
        public MetaBitBool HasFixedOrbit { get; set; } = new (0);
        [MetaProperty("isUniformScale", BinPropertyType.BitBool)]
        public MetaBitBool IsUniformScale { get; set; } = new (1);
        [MetaProperty("birthRotationalAcceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthRotationalAcceleration { get; set; } = new (new ());
        [MetaProperty("particleIsLocalOrientation", BinPropertyType.BitBool)]
        public MetaBitBool ParticleIsLocalOrientation { get; set; } = new (0);
        [MetaProperty("scaleBirthScaleByBoundObjectRadius", BinPropertyType.Float)]
        public float ScaleBirthScaleByBoundObjectRadius { get; set; } = 0f;
        [MetaProperty("uvTransformCenterMult", BinPropertyType.Vector2)]
        public Vector2 UvTransformCenterMult { get; set; } = new Vector2(0.5f, 0.5f);
        [MetaProperty("particleLingerType", BinPropertyType.Byte)]
        public byte ParticleLingerType { get; set; } = 0;
        [MetaProperty("emitterLinger", BinPropertyType.Optional)]
        public MetaOptional<float> EmitterLinger { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("texture", BinPropertyType.String)]
        public string Texture { get; set; } = "";
        [MetaProperty("disableBackfaceCull", BinPropertyType.Bool)]
        public bool DisableBackfaceCull { get; set; } = false;
        [MetaProperty("emitterName", BinPropertyType.String)]
        public string EmitterName { get; set; } = "";
        [MetaProperty("color", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> Color { get; set; } = new (new ());
        [MetaProperty("reflectionDefinition", BinPropertyType.Structure)]
        public VfxReflectionDefinitionData ReflectionDefinition { get; set; } = null;
        [MetaProperty("isSingleParticle", BinPropertyType.BitBool)]
        public MetaBitBool IsSingleParticle { get; set; } = new (0);
        [MetaProperty("scaleBias", BinPropertyType.Vector2)]
        public Vector2 ScaleBias { get; set; } = new Vector2(1f, 1f);
        [MetaProperty("colorblindVisibility", BinPropertyType.Byte)]
        public byte ColorblindVisibility { get; set; } = 0;
        [MetaProperty("keywordsIncluded", BinPropertyType.Container)]
        public MetaContainer<string> KeywordsIncluded { get; set; } = new();
        [MetaProperty("offsetLifeScalingSymmetryMode", BinPropertyType.Byte)]
        public byte OffsetLifeScalingSymmetryMode { get; set; } = 0;
        [MetaProperty("rateByVelocityFunction", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> RateByVelocityFunction { get; set; } = new (new ());
        [MetaProperty("birthOrbitalVelocity", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthOrbitalVelocity { get; set; } = new (new ());
        [MetaProperty("particleBind", BinPropertyType.Vector2)]
        public Vector2 ParticleBind { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("voiceOverOnCreateName", BinPropertyType.String)]
        public string VoiceOverOnCreateName { get; set; } = "";
        [MetaProperty("emitterUvScrollRate", BinPropertyType.Vector2)]
        public Vector2 EmitterUvScrollRate { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("lifetime", BinPropertyType.Optional)]
        public MetaOptional<float> Lifetime { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty(1401880484, BinPropertyType.BitBool)]
        public MetaBitBool m1401880484 { get; set; } = new (0);
        [MetaProperty("emissionSurfaceDefinition", BinPropertyType.Structure)]
        public VfxEmissionSurfaceData EmissionSurfaceDefinition { get; set; } = null;
        [MetaProperty("birthRotation0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthRotation0 { get; set; } = new (new ());
        [MetaProperty("birthRotation1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthRotation1 { get; set; } = new (new ());
        [MetaProperty("texAddressModeMult", BinPropertyType.Byte)]
        public byte TexAddressModeMult { get; set; } = 0;
        [MetaProperty("UseLingerScale", BinPropertyType.BitBool)]
        public MetaBitBool UseLingerScale { get; set; } = new (0);
        [MetaProperty("particleUVScrollRate", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector2> ParticleUVScrollRate { get; set; } = new (new ());
        [MetaProperty("miscRenderFlags", BinPropertyType.Byte)]
        public byte MiscRenderFlags { get; set; } = 0;
        [MetaProperty("modulationFactor", BinPropertyType.Vector4)]
        public Vector4 ModulationFactor { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        [MetaProperty(1734953377, BinPropertyType.BitBool)]
        public MetaBitBool m1734953377 { get; set; } = new (0);
        [MetaProperty("depthBiasFactors", BinPropertyType.Vector2)]
        public Vector2 DepthBiasFactors { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("offsetLifetimeScaling", BinPropertyType.Vector3)]
        public Vector3 OffsetLifetimeScaling { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("LingerRotation0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> LingerRotation0 { get; set; } = new (new ());
        [MetaProperty("particleUVScrollRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector2> ParticleUVScrollRateMult { get; set; } = new (new ());
        [MetaProperty("flexParticleLifetime", BinPropertyType.Structure)]
        public FlexValueFloat FlexParticleLifetime { get; set; } = null;
        [MetaProperty("rotation1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Rotation1 { get; set; } = new (new ());
        [MetaProperty("rotation0", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector3> Rotation0 { get; set; } = new (new ());
        [MetaProperty("birthUVOffsetMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUVOffsetMult { get; set; } = new (new ());
        [MetaProperty("uvTransformCenter", BinPropertyType.Vector2)]
        public Vector2 UvTransformCenter { get; set; } = new Vector2(0.5f, 0.5f);
        [MetaProperty("flexBirthRotationalVelocity1", BinPropertyType.Structure)]
        public FlexValueFloat FlexBirthRotationalVelocity1 { get; set; } = null;
        [MetaProperty("startFrame", BinPropertyType.UInt16)]
        public ushort StartFrame { get; set; } = 0;
        [MetaProperty("renderPhaseOverride", BinPropertyType.Byte)]
        public byte RenderPhaseOverride { get; set; } = 7;
        [MetaProperty("flexRate", BinPropertyType.Structure)]
        public FlexValueFloat FlexRate { get; set; } = null;
        [MetaProperty("flexScaleEmitOffset", BinPropertyType.Structure)]
        public FlexTypeFloat FlexScaleEmitOffset { get; set; } = null;
        [MetaProperty("flexBirthRotationalVelocity0", BinPropertyType.Structure)]
        public FlexValueVector3 FlexBirthRotationalVelocity0 { get; set; } = null;
        [MetaProperty("doesLifetimeScale", BinPropertyType.BitBool)]
        public MetaBitBool DoesLifetimeScale { get; set; } = new (0);
        [MetaProperty("scaleEmitOffsetByBoundObjectRadius", BinPropertyType.Float)]
        public float ScaleEmitOffsetByBoundObjectRadius { get; set; } = 0f;
        [MetaProperty("uvScrollClampMult", BinPropertyType.BitBool)]
        public MetaBitBool UvScrollClampMult { get; set; } = new (0);
        [MetaProperty("directionVelocityScale", BinPropertyType.Float)]
        public float DirectionVelocityScale { get; set; } = 0f;
        [MetaProperty("primitive", BinPropertyType.Structure)]
        public VfxPrimitiveBase Primitive { get; set; } = null;
        [MetaProperty("stencilRef", BinPropertyType.Byte)]
        public byte StencilRef { get; set; } = 0;
        [MetaProperty("pass", BinPropertyType.Int16)]
        public short Pass { get; set; } = 0;
        [MetaProperty("useEmissionMeshNormalForBirth", BinPropertyType.BitBool)]
        public MetaBitBool UseEmissionMeshNormalForBirth { get; set; } = new (0);
        [MetaProperty("LingerScale0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> LingerScale0 { get; set; } = new (new ());
        [MetaProperty("FlexInstanceScale", BinPropertyType.Structure)]
        public FlexTypeFloat FlexInstanceScale { get; set; } = null;
        [MetaProperty("birthDrag", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthDrag { get; set; } = new (new ());
        [MetaProperty("birthColor", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> BirthColor { get; set; } = new (new ());
        [MetaProperty("texDiv", BinPropertyType.Vector2)]
        public Vector2 TexDiv { get; set; } = new Vector2(1f, 1f);
        [MetaProperty("paletteDefinition", BinPropertyType.Structure)]
        public VfxPaletteDefinitionData PaletteDefinition { get; set; } = null;
        [MetaProperty("censorModulateValue", BinPropertyType.Vector4)]
        public Vector4 CensorModulateValue { get; set; } = new Vector4(1f, 1f, 1f, 1f);
        [MetaProperty("isRotationEnabled", BinPropertyType.BitBool)]
        public MetaBitBool IsRotationEnabled { get; set; } = new (0);
        [MetaProperty("flexBirthTranslation", BinPropertyType.Structure)]
        public FlexValueVector3 FlexBirthTranslation { get; set; } = null;
        [MetaProperty("soundOnCreateName", BinPropertyType.String)]
        public string SoundOnCreateName { get; set; } = "";
        [MetaProperty("scaleBirthScaleByBoundObjectSize", BinPropertyType.Float)]
        public float ScaleBirthScaleByBoundObjectSize { get; set; } = 0f;
        [MetaProperty("uvRotation", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> UvRotation { get; set; } = new (new ());
        [MetaProperty("lingerVelocity", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> LingerVelocity { get; set; } = new (new ());
        [MetaProperty("lockedToEmitter", BinPropertyType.BitBool)]
        public MetaBitBool LockedToEmitter { get; set; } = new (0);
        [MetaProperty("period", BinPropertyType.Optional)]
        public MetaOptional<float> Period { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("soundPersistentName", BinPropertyType.String)]
        public string SoundPersistentName { get; set; } = "";
        [MetaProperty("frameRate", BinPropertyType.Float)]
        public float FrameRate { get; set; } = 0f;
        [MetaProperty("shape", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxShape> Shape { get; set; } = new (new ());
        [MetaProperty("flexBirthVelocity", BinPropertyType.Structure)]
        public FlexValueVector3 FlexBirthVelocity { get; set; } = null;
        [MetaProperty("childParticleSetDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxChildParticleSetDefinitionData> ChildParticleSetDefinition { get; set; } = new (new ());
        [MetaProperty("birthAcceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthAcceleration { get; set; } = new (new ());
        [MetaProperty("falloffTexture", BinPropertyType.String)]
        public string FalloffTexture { get; set; } = "ASSETS/Shared/Particles/DefaultFalloff.DDS";
        [MetaProperty("sliceTechniqueRange", BinPropertyType.Float)]
        public float SliceTechniqueRange { get; set; } = 0f;
        [MetaProperty("uvScrollClamp", BinPropertyType.BitBool)]
        public MetaBitBool UvScrollClamp { get; set; } = new (0);
        [MetaProperty("fieldCollectionDefinition", BinPropertyType.Structure)]
        public VfxFieldCollectionDefinitionData FieldCollectionDefinition { get; set; } = null;
        [MetaProperty("rate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Rate { get; set; } = new (new ());
        [MetaProperty("UseLingerDrag", BinPropertyType.BitBool)]
        public MetaBitBool UseLingerDrag { get; set; } = new (0);
        [MetaProperty("translationOverride", BinPropertyType.Vector3)]
        public Vector3 TranslationOverride { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("doesCastShadow", BinPropertyType.BitBool)]
        public MetaBitBool DoesCastShadow { get; set; } = new (1);
        [MetaProperty("particleColorTexture", BinPropertyType.String)]
        public string ParticleColorTexture { get; set; } = "ASSETS/Shared/Particles/DefaultColorOverlifetime.dds";
        [MetaProperty("rotationOverride", BinPropertyType.Vector3)]
        public Vector3 RotationOverride { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("flexBirthUVScrollRateMult", BinPropertyType.Structure)]
        public FlexValueVector2 FlexBirthUVScrollRateMult { get; set; } = null;
        [MetaProperty("drag", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Drag { get; set; } = new (new ());
        [MetaProperty("birthUVOffset", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUVOffset { get; set; } = new (new ());
        [MetaProperty("importance", BinPropertyType.Byte)]
        public byte Importance { get; set; } = 1;
        [MetaProperty("alphaRef", BinPropertyType.Byte)]
        public byte AlphaRef { get; set; } = 5;
        [MetaProperty("isFollowingTerrain", BinPropertyType.BitBool)]
        public MetaBitBool IsFollowingTerrain { get; set; } = new (0);
        [MetaProperty("distortionDefinition", BinPropertyType.Structure)]
        public VfxDistortionDefinitionData DistortionDefinition { get; set; } = null;
        [MetaProperty("fixedOrbitType", BinPropertyType.Byte)]
        public byte FixedOrbitType { get; set; } = 1;
        [MetaProperty(3181085639, BinPropertyType.BitBool)]
        public MetaBitBool m3181085639 { get; set; } = new (0);
        [MetaProperty("emissionMeshScale", BinPropertyType.Float)]
        public float EmissionMeshScale { get; set; } = 1f;
        [MetaProperty("flexBirthUVScrollRate", BinPropertyType.Structure)]
        public FlexValueVector2 FlexBirthUVScrollRate { get; set; } = null;
        [MetaProperty("softParticleParams", BinPropertyType.Structure)]
        public VfxSoftParticleDefinitionData SoftParticleParams { get; set; } = null;
        [MetaProperty("particleUVRotateRate", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueFloat> ParticleUVRotateRate { get; set; } = new (new ());
        [MetaProperty("scaleEmitOffsetByBoundObjectSize", BinPropertyType.Float)]
        public float ScaleEmitOffsetByBoundObjectSize { get; set; } = 0f;
        [MetaProperty("keywordsExcluded", BinPropertyType.Container)]
        public MetaContainer<string> KeywordsExcluded { get; set; } = new();
        [MetaProperty("alphaErosionDefinition", BinPropertyType.Structure)]
        public VfxAlphaErosionDefinitionData AlphaErosionDefinition { get; set; } = null;
        [MetaProperty("materialDrivers", BinPropertyType.Map)]
        public Dictionary<string, IVfxMaterialDriver> MaterialDrivers { get; set; } = new();
        [MetaProperty("orientation", BinPropertyType.Byte)]
        public byte Orientation { get; set; } = 0;
        [MetaProperty("birthFrameRate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthFrameRate { get; set; } = new (new ());
        [MetaProperty("isTexturePixelated", BinPropertyType.Bool)]
        public bool IsTexturePixelated { get; set; } = false;
        [MetaProperty("MaximumRateByVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MaximumRateByVelocity { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("bindWeight", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BindWeight { get; set; } = new (new ());
        [MetaProperty("uvScaleMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> UvScaleMult { get; set; } = new (new ());
        [MetaProperty("colorLookUpOffsets", BinPropertyType.Vector2)]
        public Vector2 ColorLookUpOffsets { get; set; } = new Vector2(0f, 0f);
        [MetaProperty(3472013936, BinPropertyType.Float)]
        public float m3472013936 { get; set; } = 0f;
        [MetaProperty("birthUvScrollRate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUvScrollRate { get; set; } = new (new ());
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink Material { get; set; } = new(0);
        [MetaProperty("scale1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Scale1 { get; set; } = new (new ());
        [MetaProperty("scale0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Scale0 { get; set; } = new (new ());
        [MetaProperty("useNavmeshMask", BinPropertyType.BitBool)]
        public MetaBitBool UseNavmeshMask { get; set; } = new (0);
        [MetaProperty("isDirectionOriented", BinPropertyType.BitBool)]
        public MetaBitBool IsDirectionOriented { get; set; } = new (0);
        [MetaProperty("UseLingerRotation", BinPropertyType.BitBool)]
        public MetaBitBool UseLingerRotation { get; set; } = new (0);
        [MetaProperty("lingerAcceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> LingerAcceleration { get; set; } = new (new ());
        [MetaProperty("isRandomStartFrame", BinPropertyType.BitBool)]
        public MetaBitBool IsRandomStartFrame { get; set; } = new (1);
        [MetaProperty("emitterUvScrollRateMult", BinPropertyType.Vector2)]
        public Vector2 EmitterUvScrollRateMult { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("flexOffset", BinPropertyType.Structure)]
        public FlexValueVector3 FlexOffset { get; set; } = null;
        [MetaProperty("scaleOverride", BinPropertyType.Vector3)]
        public Vector3 ScaleOverride { get; set; } = new Vector3(1f, 1f, 1f);
        [MetaProperty("scaleBirthScaleByBoundObjectHeight", BinPropertyType.Float)]
        public float ScaleBirthScaleByBoundObjectHeight { get; set; } = 0f;
        [MetaProperty("spectatorPolicy", BinPropertyType.Byte)]
        public byte SpectatorPolicy { get; set; } = 0;
        [MetaProperty("lingerColor", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> LingerColor { get; set; } = new (new ());
        [MetaProperty("uvScrollAlphaMult", BinPropertyType.BitBool)]
        public MetaBitBool UvScrollAlphaMult { get; set; } = new (0);
        [MetaProperty("worldAcceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector3> WorldAcceleration { get; set; } = new (new ());
        [MetaProperty("directionVelocityMinScale", BinPropertyType.Float)]
        public float DirectionVelocityMinScale { get; set; } = 1f;
        [MetaProperty("hasPostRotateOrientation", BinPropertyType.BitBool)]
        public MetaBitBool HasPostRotateOrientation { get; set; } = new (1);
        [MetaProperty("birthUvRotateRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthUvRotateRateMult { get; set; } = new (new ());
        [MetaProperty("uvScale", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> UvScale { get; set; } = new (new ());
        [MetaProperty("colorLookUpScales", BinPropertyType.Vector2)]
        public Vector2 ColorLookUpScales { get; set; } = new Vector2(1f, 1f);
        [MetaProperty("birthScale0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthScale0 { get; set; } = new (new ());
        [MetaProperty("birthScale1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthScale1 { get; set; } = new (new ());
        [MetaProperty("UseLingerAcceleration", BinPropertyType.BitBool)]
        public MetaBitBool UseLingerAcceleration { get; set; } = new (0);
        [MetaProperty("uvParallaxScale", BinPropertyType.Float)]
        public float UvParallaxScale { get; set; } = 0f;
        [MetaProperty("scaleUpFromOrigin", BinPropertyType.BitBool)]
        public MetaBitBool ScaleUpFromOrigin { get; set; } = new (0);
        [MetaProperty("birthVelocity", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthVelocity { get; set; } = new (new ());
        [MetaProperty("postRotateOrientationAxis", BinPropertyType.Vector3)]
        public Vector3 PostRotateOrientationAxis { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("excludedAttachmentTypes", BinPropertyType.Container)]
        public MetaContainer<string> ExcludedAttachmentTypes { get; set; } = new();
        [MetaProperty("blendMode", BinPropertyType.Byte)]
        public byte BlendMode { get; set; } = 0;
        [MetaProperty("lingerDrag", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> LingerDrag { get; set; } = new (new ());
    }
    [MetaClass("AddHealthCheat")]
    public class AddHealthCheat : Cheat
    {
        [MetaProperty("mAmount", BinPropertyType.Float)]
        public float Amount { get; set; } = 0f;
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass(2642491558)]
    public class Class0x9d8138a6 : IMetaClass
    {
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash ObjectPath { get; set; } = new(0);
        [MetaProperty("SliderClickedState", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xf2cfc48c> SliderClickedState { get; set; } = new (new ());
        [MetaProperty("SliderHitRegion", BinPropertyType.ObjectLink)]
        public MetaObjectLink SliderHitRegion { get; set; } = new(0);
        [MetaProperty(96062416, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xf2cfc48c> m96062416 { get; set; } = new (new ());
        [MetaProperty("soundEvents", BinPropertyType.Structure)]
        public Class0x2da50c9f SoundEvents { get; set; } = null;
        [MetaProperty(3035679710, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xf2cfc48c> m3035679710 { get; set; } = new (new ());
        [MetaProperty("DefaultState", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xf2cfc48c> DefaultState { get; set; } = new (new ());
        [MetaProperty("direction", BinPropertyType.Byte)]
        public byte Direction { get; set; } = 0;
        [MetaProperty("BarHitRegion", BinPropertyType.ObjectLink)]
        public MetaObjectLink BarHitRegion { get; set; } = new(0);
    }
    [MetaClass("ClientStateCommonSettings")]
    public class ClientStateCommonSettings : IMetaClass
    {
        [MetaProperty(1530984701, BinPropertyType.UInt32)]
        public uint m1530984701 { get; set; } = 120;
        [MetaProperty(1788708839, BinPropertyType.UInt32)]
        public uint m1788708839 { get; set; } = 25;
        [MetaProperty(4025033036, BinPropertyType.UInt32)]
        public uint m4025033036 { get; set; } = 300;
    }
    [MetaClass("ContextualConditionNeutralMinionCampName")]
    public class ContextualConditionNeutralMinionCampName : IContextualCondition
    {
        [MetaProperty("mCampName", BinPropertyType.Hash)]
        public MetaHash CampName { get; set; } = new(0);
    }
    [MetaClass("CastOnMovementComplete")]
    public class CastOnMovementComplete : MissileBehaviorSpec
    {
    }
    [MetaClass("ContextualConditionTimeSinceStealthStateChange")]
    public class ContextualConditionTimeSinceStealthStateChange : IContextualCondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 5;
        [MetaProperty("mStateToCheck", BinPropertyType.Byte)]
        public byte StateToCheck { get; set; } = 1;
        [MetaProperty("mTimeThreshold", BinPropertyType.Float)]
        public float TimeThreshold { get; set; } = 1f;
    }
    [MetaClass("VectorTableGet")]
    public class VectorTableGet : IVectorGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<Vector3> Default { get; set; } = new MetaOptional<Vector3>(default(Vector3), false);
    }
    [MetaClass("TftMapCharacterSkinData")]
    public class TftMapCharacterSkinData : IMetaClass
    {
        [MetaProperty("SquareIconTexturePath", BinPropertyType.String)]
        public string SquareIconTexturePath { get; set; } = "";
    }
    [MetaClass(2653653638)]
    public class Class0x9e2b8a86 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(339627414, BinPropertyType.UInt32)]
        public uint m339627414 { get; set; } = 16;
        [MetaProperty("Scene", BinPropertyType.Hash)]
        public MetaHash Scene { get; set; } = new(0);
        [MetaProperty(986986398, BinPropertyType.String)]
        public string m986986398 { get; set; } = "";
        [MetaProperty(1101698582, BinPropertyType.UInt32)]
        public uint m1101698582 { get; set; } = 5;
        [MetaProperty(1269553309, BinPropertyType.Hash)]
        public MetaHash m1269553309 { get; set; } = new(0);
        [MetaProperty(121430694, BinPropertyType.String)]
        public string m121430694 { get; set; } = "";
        [MetaProperty(1979305081, BinPropertyType.Hash)]
        public MetaHash m1979305081 { get; set; } = new(0);
        [MetaProperty(2339816255, BinPropertyType.String)]
        public string m2339816255 { get; set; } = "";
        [MetaProperty(2414981457, BinPropertyType.String)]
        public string m2414981457 { get; set; } = "";
        [MetaProperty(2647627549, BinPropertyType.Hash)]
        public MetaHash m2647627549 { get; set; } = new(0);
        [MetaProperty(3122463628, BinPropertyType.Float)]
        public float m3122463628 { get; set; } = 0f;
        [MetaProperty(207379616, BinPropertyType.Hash)]
        public MetaHash m207379616 { get; set; } = new(0);
        [MetaProperty(3938520714, BinPropertyType.String)]
        public string m3938520714 { get; set; } = "";
        [MetaProperty(4060237202, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x2610e5a7> m4060237202 { get; set; } = new (new ());
        [MetaProperty(4122477162, BinPropertyType.Hash)]
        public MetaHash m4122477162 { get; set; } = new(0);
        [MetaProperty(4159587618, BinPropertyType.String)]
        public string m4159587618 { get; set; } = "";
    }
    [MetaClass(2656759159)]
    public class Class0x9e5aed77 : IMetaClass
    {
        [MetaProperty(445332680, BinPropertyType.Int32)]
        public int m445332680 { get; set; } = 0;
        [MetaProperty(1203616745, BinPropertyType.Int32)]
        public int m1203616745 { get; set; } = 0;
        [MetaProperty(1461360547, BinPropertyType.Int32)]
        public int m1461360547 { get; set; } = 0;
        [MetaProperty(2292470017, BinPropertyType.Int32)]
        public int m2292470017 { get; set; } = 0;
        [MetaProperty(3146852779, BinPropertyType.Int32)]
        public int m3146852779 { get; set; } = 0;
        [MetaProperty(3806656962, BinPropertyType.Int32)]
        public int m3806656962 { get; set; } = 0;
    }
    [MetaClass("NumberCalculationPart")]
    public class NumberCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mNumber", BinPropertyType.Float)]
        public float Number { get; set; } = 0f;
    }
    [MetaClass(2673469741)]
    public class Class0x9f59e92d : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("mOrder", BinPropertyType.Byte)]
        public byte Order { get; set; } = 0;
        [MetaProperty("mInventoryFilters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x14aef50c>> InventoryFilters { get; set; } = new();
    }
    [MetaClass("CustomTableGet")]
    public class CustomTableGet : IScriptValueGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
    }
    [MetaClass("SurrenderData")]
    public class SurrenderData : IMetaClass
    {
        [MetaProperty("mTypeData", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<SurrenderTypeData>> TypeData { get; set; } = new();
        [MetaProperty(989768947, BinPropertyType.Float)]
        public float m989768947 { get; set; } = 0.5f;
        [MetaProperty(1140283803, BinPropertyType.Float)]
        public float m1140283803 { get; set; } = 5f;
        [MetaProperty(3430961411, BinPropertyType.Float)]
        public float m3430961411 { get; set; } = 180f;
        [MetaProperty(244881724, BinPropertyType.Float)]
        public float m244881724 { get; set; } = 180f;
    }
    [MetaClass(2690290802)]
    public class Class0xa05a9472 : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty(1514380197, BinPropertyType.Float)]
        public float m1514380197 { get; set; } = 4500f;
        [MetaProperty("follow", BinPropertyType.Bool)]
        public bool Follow { get; set; } = false;
        [MetaProperty("yaw", BinPropertyType.Float)]
        public float Yaw { get; set; } = 0f;
        [MetaProperty("ZoomEase", BinPropertyType.Byte)]
        public byte ZoomEase { get; set; } = 2;
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> Position { get; set; } = new (new ());
        [MetaProperty("fov", BinPropertyType.Float)]
        public float Fov { get; set; } = 45f;
        [MetaProperty("pitch", BinPropertyType.Float)]
        public float Pitch { get; set; } = 45f;
    }
    [MetaClass("HudItemShopData")]
    public class HudItemShopData : IMetaClass
    {
        [MetaProperty("InventoryPanelDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xd20000f6> InventoryPanelDefinition { get; set; } = new (new ());
        [MetaProperty("PurchaseButtonDefinition", BinPropertyType.Hash)]
        public MetaHash PurchaseButtonDefinition { get; set; } = new(0);
        [MetaProperty(518100384, BinPropertyType.Hash)]
        public MetaHash m518100384 { get; set; } = new(0);
        [MetaProperty("BuildTreeIconDefinition", BinPropertyType.Hash)]
        public MetaHash BuildTreeIconDefinition { get; set; } = new(0);
        [MetaProperty("CloseButtonDefinition", BinPropertyType.Hash)]
        public MetaHash CloseButtonDefinition { get; set; } = new(0);
        [MetaProperty("CommonlyBuiltIconDefinition", BinPropertyType.Hash)]
        public MetaHash CommonlyBuiltIconDefinition { get; set; } = new(0);
        [MetaProperty("TabButtonDefinition", BinPropertyType.Hash)]
        public MetaHash TabButtonDefinition { get; set; } = new(0);
        [MetaProperty("SearchIconDefinition", BinPropertyType.Hash)]
        public MetaHash SearchIconDefinition { get; set; } = new(0);
        [MetaProperty("UndoButtonDefinition", BinPropertyType.Hash)]
        public MetaHash UndoButtonDefinition { get; set; } = new(0);
        [MetaProperty("BuildsIntoIconDefinition", BinPropertyType.Hash)]
        public MetaHash BuildsIntoIconDefinition { get; set; } = new(0);
        [MetaProperty(1210316573, BinPropertyType.Hash)]
        public MetaHash m1210316573 { get; set; } = new(0);
        [MetaProperty(1229605698, BinPropertyType.Hash)]
        public MetaHash m1229605698 { get; set; } = new(0);
        [MetaProperty("SellButtonDefinition", BinPropertyType.Hash)]
        public MetaHash SellButtonDefinition { get; set; } = new(0);
        [MetaProperty(1522120281, BinPropertyType.Hash)]
        public MetaHash m1522120281 { get; set; } = new(0);
        [MetaProperty("ItemSetsIconDefinition", BinPropertyType.Hash)]
        public MetaHash ItemSetsIconDefinition { get; set; } = new(0);
        [MetaProperty(1634417284, BinPropertyType.Hash)]
        public MetaHash m1634417284 { get; set; } = new(0);
        [MetaProperty("QuickBuyIconDefinition", BinPropertyType.Hash)]
        public MetaHash QuickBuyIconDefinition { get; set; } = new(0);
        [MetaProperty("AllItemsIconDefinition", BinPropertyType.Hash)]
        public MetaHash AllItemsIconDefinition { get; set; } = new(0);
        [MetaProperty(1700556964, BinPropertyType.Hash)]
        public MetaHash m1700556964 { get; set; } = new(0);
        [MetaProperty("InventoryIconDefinition", BinPropertyType.Hash)]
        public MetaHash InventoryIconDefinition { get; set; } = new(0);
        [MetaProperty(1771605430, BinPropertyType.Hash)]
        public MetaHash m1771605430 { get; set; } = new(0);
        [MetaProperty(1834167418, BinPropertyType.Hash)]
        public MetaHash m1834167418 { get; set; } = new(0);
        [MetaProperty("ItemDetailsScene", BinPropertyType.Hash)]
        public MetaHash ItemDetailsScene { get; set; } = new(0);
        [MetaProperty(1878129342, BinPropertyType.Hash)]
        public MetaHash m1878129342 { get; set; } = new(0);
        [MetaProperty(1909552450, BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> m1909552450 { get; set; } = new (new ());
        [MetaProperty(2002344617, BinPropertyType.Hash)]
        public MetaHash m2002344617 { get; set; } = new(0);
        [MetaProperty(2040756048, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x38691790>> m2040756048 { get; set; } = new();
        [MetaProperty("ItemSetsItemRegion", BinPropertyType.Hash)]
        public MetaHash ItemSetsItemRegion { get; set; } = new(0);
        [MetaProperty(2717405335, BinPropertyType.Hash)]
        public MetaHash m2717405335 { get; set; } = new(0);
        [MetaProperty(2852516434, BinPropertyType.Hash)]
        public MetaHash m2852516434 { get; set; } = new(0);
        [MetaProperty(2855612289, BinPropertyType.Hash)]
        public MetaHash m2855612289 { get; set; } = new(0);
        [MetaProperty(3018445638, BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> m3018445638 { get; set; } = new (new ());
        [MetaProperty("AllItemsItemRegion", BinPropertyType.Hash)]
        public MetaHash AllItemsItemRegion { get; set; } = new(0);
        [MetaProperty("SearchViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> SearchViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty(216091685, BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> m216091685 { get; set; } = new (new ());
        [MetaProperty(3525322849, BinPropertyType.Hash)]
        public MetaHash m3525322849 { get; set; } = new(0);
        [MetaProperty(3606622714, BinPropertyType.String)]
        public string m3606622714 { get; set; } = "";
        [MetaProperty("AllItemsHeaderText", BinPropertyType.Hash)]
        public MetaHash AllItemsHeaderText { get; set; } = new(0);
        [MetaProperty(3706895331, BinPropertyType.Hash)]
        public MetaHash m3706895331 { get; set; } = new(0);
        [MetaProperty(3758876689, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m3758876689 { get; set; } = new();
        [MetaProperty("ItemSetsHeaderRegion", BinPropertyType.Hash)]
        public MetaHash ItemSetsHeaderRegion { get; set; } = new(0);
        [MetaProperty("BootsPanelDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xd20000f6> BootsPanelDefinition { get; set; } = new (new ());
        [MetaProperty("ConsumablesPanelDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xd20000f6> ConsumablesPanelDefinition { get; set; } = new (new ());
        [MetaProperty(4055334374, BinPropertyType.Hash)]
        public MetaHash m4055334374 { get; set; } = new(0);
        [MetaProperty(4102157670, BinPropertyType.Hash)]
        public MetaHash m4102157670 { get; set; } = new(0);
        [MetaProperty("AllItemsHeaderRegion", BinPropertyType.Hash)]
        public MetaHash AllItemsHeaderRegion { get; set; } = new(0);
        [MetaProperty(260247722, BinPropertyType.Float)]
        public float m260247722 { get; set; } = 1f;
        [MetaProperty(4175508524, BinPropertyType.Hash)]
        public MetaHash m4175508524 { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionNeutralMinionCampIsAlive")]
    public class ContextualConditionNeutralMinionCampIsAlive : IContextualCondition
    {
        [MetaProperty("mCampIsAlive", BinPropertyType.Bool)]
        public bool CampIsAlive { get; set; } = false;
    }
    [MetaClass("VfxPrimitiveRay")]
    public class VfxPrimitiveRay : VfxPrimitiveBase
    {
    }
    [MetaClass("ScriptPreloadSpell")]
    public class ScriptPreloadSpell : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string PreloadResourceName { get; set; } = "";
    }
    [MetaClass("HudLoadingScreenWidgetCarousel")]
    public class HudLoadingScreenWidgetCarousel : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string SceneName { get; set; } = "";
        [MetaProperty("mCarouselData", BinPropertyType.ObjectLink)]
        public MetaObjectLink CarouselData { get; set; } = new(0);
        [MetaProperty(2065649608, BinPropertyType.Byte)]
        public byte m2065649608 { get; set; } = 1;
    }
    [MetaClass("GameModeConstantString")]
    public class GameModeConstantString : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.String)]
        public string Value { get; set; } = "";
    }
    [MetaClass("ContextualConditionCharacterUnitTags")]
    public class ContextualConditionCharacterUnitTags : ICharacterSubcondition
    {
        [MetaProperty("mUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> UnitTags { get; set; } = new (new ());
        [MetaProperty("mTagMode", BinPropertyType.Byte)]
        public byte TagMode { get; set; } = 0;
    }
    [MetaClass("MinimapBackground")]
    public class MinimapBackground : IMetaClass
    {
        [MetaProperty("mSize", BinPropertyType.Vector2)]
        public Vector2 Size { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("mOrigin", BinPropertyType.Vector2)]
        public Vector2 Origin { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
    }
    [MetaClass("ElementGroupSliderState")]
    public class ElementGroupSliderState : IMetaClass
    {
        [MetaProperty("BarBackdrop", BinPropertyType.ObjectLink)]
        public MetaObjectLink BarBackdrop { get; set; } = new(0);
        [MetaProperty("BarFill", BinPropertyType.ObjectLink)]
        public MetaObjectLink BarFill { get; set; } = new(0);
        [MetaProperty("sliderIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink SliderIcon { get; set; } = new(0);
    }
    [MetaClass("MaterialInstanceSwitchDef")]
    public class MaterialInstanceSwitchDef : IMetaClass
    {
        [MetaProperty("on", BinPropertyType.Bool)]
        public bool On { get; set; } = false;
    }
    [MetaClass("SecondaryResourceDisplayFractional")]
    public class SecondaryResourceDisplayFractional : ISecondaryResourceDisplayData
    {
    }
    [MetaClass("ItemRecommendationMatrixRow")]
    public class ItemRecommendationMatrixRow : IMetaClass
    {
        [MetaProperty("mChoicesMap", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<ItemRecommendationChoices>> ChoicesMap { get; set; } = new();
    }
    [MetaClass("BoolTableSet")]
    public class BoolTableSet : ScriptTableSet
    {
    }
    [MetaClass("VoiceChatViewSelfSlot")]
    public class VoiceChatViewSelfSlot : IMetaClass
    {
        [MetaProperty("ConnectionButton", BinPropertyType.Hash)]
        public MetaHash ConnectionButton { get; set; } = new(0);
        [MetaProperty("MicVolumeSliderBar", BinPropertyType.Hash)]
        public MetaHash MicVolumeSliderBar { get; set; } = new(0);
        [MetaProperty("MicVolumeText", BinPropertyType.Hash)]
        public MetaHash MicVolumeText { get; set; } = new(0);
        [MetaProperty("Portrait", BinPropertyType.Hash)]
        public MetaHash Portrait { get; set; } = new(0);
        [MetaProperty("Halo", BinPropertyType.Hash)]
        public MetaHash Halo { get; set; } = new(0);
        [MetaProperty("NameText", BinPropertyType.Hash)]
        public MetaHash NameText { get; set; } = new(0);
        [MetaProperty("MuteButton", BinPropertyType.Hash)]
        public MetaHash MuteButton { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionCharacter")]
    public class ContextualConditionCharacter : IContextualCondition
    {
        [MetaProperty("mChildConditions", BinPropertyType.Container)]
        public MetaContainer<ICharacterSubcondition> ChildConditions { get; set; } = new();
        [MetaProperty("mCharacterType", BinPropertyType.Byte)]
        public byte CharacterType { get; set; } = 0;
    }
    [MetaClass("TftMapSkin")]
    public class TftMapSkin : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("AudioBankPaths", BinPropertyType.Container)]
        public MetaContainer<string> AudioBankPaths { get; set; } = new();
        [MetaProperty("StandaloneLoadoutsIcon", BinPropertyType.String)]
        public string StandaloneLoadoutsIcon { get; set; } = "";
        [MetaProperty("rarity", BinPropertyType.UInt32)]
        public uint Rarity { get; set; } = 0;
        [MetaProperty(1849274442, BinPropertyType.UInt16)]
        public ushort m1849274442 { get; set; } = 0;
        [MetaProperty("characters", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaHash> Characters { get; set; } = new();
        [MetaProperty(2948884459, BinPropertyType.UInt16)]
        public ushort m2948884459 { get; set; } = 0;
        [MetaProperty("mapContainer", BinPropertyType.String)]
        public string MapContainer { get; set; } = "";
        [MetaProperty("StandaloneLoadoutsLargeIcon", BinPropertyType.String)]
        public string StandaloneLoadoutsLargeIcon { get; set; } = "";
        [MetaProperty("GroupLink", BinPropertyType.String)]
        public string GroupLink { get; set; } = "";
    }
    [MetaClass("HudBannerData")]
    public class HudBannerData : IMetaClass
    {
        [MetaProperty("pulseInterval", BinPropertyType.Float)]
        public float PulseInterval { get; set; } = 15f;
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float TransitionTime { get; set; } = 0.10000000149011612f;
        [MetaProperty("pulseTime", BinPropertyType.Float)]
        public float PulseTime { get; set; } = 0.25f;
        [MetaProperty("TransitionMinAlpha", BinPropertyType.Byte)]
        public byte TransitionMinAlpha { get; set; } = 0;
        [MetaProperty("transitionOffset", BinPropertyType.Vector2)]
        public Vector2 TransitionOffset { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("TransitionMaxAlpha", BinPropertyType.Byte)]
        public byte TransitionMaxAlpha { get; set; } = 255;
        [MetaProperty("pulseDuration", BinPropertyType.Float)]
        public float PulseDuration { get; set; } = 1f;
        [MetaProperty("pulseOffset", BinPropertyType.Vector2)]
        public Vector2 PulseOffset { get; set; } = new Vector2(0f, 0f);
    }
    [MetaClass("CommonUiTunables")]
    public class CommonUiTunables : IMetaClass
    {
        [MetaProperty(1194034797, BinPropertyType.Float)]
        public float m1194034797 { get; set; } = 1.5f;
        [MetaProperty(3019154977, BinPropertyType.Byte)]
        public byte m3019154977 { get; set; } = 8;
    }
    [MetaClass("FxActionTimeDilate")]
    public class FxActionTimeDilate : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("intime", BinPropertyType.Float)]
        public float Intime { get; set; } = 0.5f;
        [MetaProperty("TimeDilation", BinPropertyType.Float)]
        public float TimeDilation { get; set; } = 1f;
        [MetaProperty("InEase", BinPropertyType.Byte)]
        public byte InEase { get; set; } = 4;
        [MetaProperty("OutEase", BinPropertyType.Byte)]
        public byte OutEase { get; set; } = 4;
        [MetaProperty("OutTime", BinPropertyType.Float)]
        public float OutTime { get; set; } = 0.5f;
    }
    [MetaClass("EffectElementData")]
    public interface EffectElementData : BaseElementData
    {
    }
    [MetaClass("WadFileDescriptor")]
    public interface WadFileDescriptor : IMetaClass
    {
    }
    [MetaClass("TriggerOnDistanceFromCaster")]
    public class TriggerOnDistanceFromCaster : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; } = new();
        [MetaProperty("mDistance", BinPropertyType.Float)]
        public float Distance { get; set; } = 0f;
    }
    [MetaClass(2748390021)]
    public class Class0xa3d11a85 : AtlasDataBase
    {
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
        [MetaProperty(367825281, BinPropertyType.Vector2)]
        public Vector2 m367825281 { get; set; } = new Vector2(0.25f, 0.25f);
        [MetaProperty("TextureUs", BinPropertyType.Vector2)]
        public Vector2 TextureUs { get; set; } = new Vector2(0f, 1f);
        [MetaProperty("TextureVs", BinPropertyType.Vector4)]
        public Vector4 TextureVs { get; set; } = new Vector4(0f, 0.25f, 0.75f, 1f);
    }
    [MetaClass("LoadoutCompanionInfoPanel")]
    public class LoadoutCompanionInfoPanel : ILoadoutInfoPanel
    {
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty(1784034802, BinPropertyType.Hash)]
        public MetaHash m1784034802 { get; set; } = new(0);
        [MetaProperty("upgradeTierButton", BinPropertyType.Container)]
        public MetaContainer<MetaHash> UpgradeTierButton { get; set; } = new();
        [MetaProperty(2430432239, BinPropertyType.Hash)]
        public MetaHash m2430432239 { get; set; } = new(0);
        [MetaProperty("tierButton", BinPropertyType.Container)]
        public MetaContainer<MetaHash> TierButton { get; set; } = new();
        [MetaProperty("upgradeErrorText", BinPropertyType.Hash)]
        public MetaHash UpgradeErrorText { get; set; } = new(0);
        [MetaProperty("upgradeInfoText", BinPropertyType.Hash)]
        public MetaHash UpgradeInfoText { get; set; } = new(0);
    }
    [MetaClass("ItemRecommendationOverrideSet")]
    public class ItemRecommendationOverrideSet : IMetaClass
    {
        [MetaProperty("mOverrides", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationOverride>> Overrides { get; set; } = new();
    }
    [MetaClass("BankUnit")]
    public class BankUnit : IMetaClass
    {
        [MetaProperty("events", BinPropertyType.Container)]
        public MetaContainer<string> Events { get; set; } = new();
        [MetaProperty("bankPath", BinPropertyType.Container)]
        public MetaContainer<string> BankPath { get; set; } = new();
        [MetaProperty("voiceOver", BinPropertyType.Bool)]
        public bool VoiceOver { get; set; } = false;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("asynchrone", BinPropertyType.Bool)]
        public bool Asynchrone { get; set; } = false;
    }
    [MetaClass("SettingsViewController")]
    public class SettingsViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("SoundOnDeActivate", BinPropertyType.String)]
        public string SoundOnDeActivate { get; set; } = "";
        [MetaProperty("tabButton", BinPropertyType.Hash)]
        public MetaHash TabButton { get; set; } = new(0);
        [MetaProperty("SoundOnActivate", BinPropertyType.String)]
        public string SoundOnActivate { get; set; } = "";
        [MetaProperty("closeButton", BinPropertyType.Hash)]
        public MetaHash CloseButton { get; set; } = new(0);
        [MetaProperty("GeneralSettings", BinPropertyType.Embedded)]
        public MetaEmbedded<GeneralSettingsGroup> GeneralSettings { get; set; } = new (new ());
    }
    [MetaClass(2761167747)]
    public class Class0xa4941383 : IOptionItemFilter
    {
    }
    [MetaClass("VfxPrimitiveAttachedMesh")]
    public class VfxPrimitiveAttachedMesh : VfxPrimitiveMeshBase
    {
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; } = new (new ());
        [MetaProperty(3934657962, BinPropertyType.Bool)]
        public bool m3934657962 { get; set; } = false;
        [MetaProperty(4227234111, BinPropertyType.Bool)]
        public bool m4227234111 { get; set; } = false;
    }
    [MetaClass("TooltipInstanceSpell")]
    public class TooltipInstanceSpell : TooltipInstance
    {
        [MetaProperty("EnableExtendedTooltip", BinPropertyType.Bool)]
        public bool EnableExtendedTooltip { get; set; } = true;
    }
    [MetaClass("PerkScriptData")]
    public class PerkScriptData : IMetaClass
    {
        [MetaProperty("mEffectAmount", BinPropertyType.Map)]
        public Dictionary<MetaHash, float> EffectAmount { get; set; } = new();
        [MetaProperty("mEffectAmountGameMode", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<PerkEffectAmountPerMode>> EffectAmountGameMode { get; set; } = new();
        [MetaProperty("mCalculations", BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> Calculations { get; set; } = new();
    }
    [MetaClass("MapLaneComponent")]
    public class MapLaneComponent : MapComponent
    {
        [MetaProperty("mLanes", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<LaneData>> Lanes { get; set; } = new();
    }
    [MetaClass("ChampionMasteryMap")]
    public class ChampionMasteryMap : IMetaClass
    {
        [MetaProperty("masteryData", BinPropertyType.Map)]
        public Dictionary<int, MetaObjectLink> MasteryData { get; set; } = new();
    }
    [MetaClass("SummonerIconData")]
    public class SummonerIconData : IMetaClass
    {
        [MetaProperty("gameTexture", BinPropertyType.String)]
        public string GameTexture { get; set; } = "";
        [MetaProperty("eSportsEventMutator", BinPropertyType.String)]
        public string ESportsEventMutator { get; set; } = "";
        [MetaProperty(1357231841, BinPropertyType.Bool)]
        public bool m1357231841 { get; set; } = false;
        [MetaProperty("esportsTeam", BinPropertyType.ObjectLink)]
        public MetaObjectLink EsportsTeam { get; set; } = new(0);
        [MetaProperty("iconId", BinPropertyType.UInt32)]
        public uint IconId { get; set; } = 0;
    }
    [MetaClass("EngineFeatureToggles")]
    public class EngineFeatureToggles : IMetaClass
    {
        [MetaProperty(100560457, BinPropertyType.Bool)]
        public bool m100560457 { get; set; } = true;
        [MetaProperty(3451635425, BinPropertyType.Bool)]
        public bool m3451635425 { get; set; } = true;
    }
    [MetaClass(174539687)]
    public class Class0xa6743a7 : IOptionItemFilter
    {
        [MetaProperty("Filters", BinPropertyType.Container)]
        public MetaContainer<IOptionItemFilter> Filters { get; set; } = new();
    }
    [MetaClass("ScriptSequence")]
    public class ScriptSequence : IScriptSequence
    {
        [MetaProperty("blocks", BinPropertyType.Container)]
        public MetaContainer<IScriptBlock> Blocks { get; set; } = new();
    }
    [MetaClass("UIButtonState")]
    public class UIButtonState : IMetaClass
    {
        [MetaProperty("displayElements", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> DisplayElements { get; set; } = new();
        [MetaProperty("TextElement", BinPropertyType.ObjectLink)]
        public MetaObjectLink TextElement { get; set; } = new(0);
    }
    [MetaClass(2795848259)]
    public class Class0xa6a54243 : IOptionItemFilter
    {
    }
    [MetaClass("ToggleTeamCheat")]
    public class ToggleTeamCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass("IFloatGet")]
    public interface IFloatGet : IScriptValueGet
    {
    }
    [MetaClass("LoadoutArenaSkinInfoPanel")]
    public class LoadoutArenaSkinInfoPanel : ILoadoutInfoPanel
    {
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
    }
    [MetaClass("HudReplayData")]
    public class HudReplayData : IMetaClass
    {
        [MetaProperty("TeamFightData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudTeamFightData> TeamFightData { get; set; } = new (new ());
        [MetaProperty("messageVisibleTime", BinPropertyType.Float)]
        public float MessageVisibleTime { get; set; } = 5f;
    }
    [MetaClass("MapAudio")]
    public class MapAudio : GenericMapPlaceable
    {
        [MetaProperty("EventName", BinPropertyType.String)]
        public string EventName { get; set; } = "";
        [MetaProperty(1368345017, BinPropertyType.Float)]
        public float m1368345017 { get; set; } = 0f;
        [MetaProperty("AudioType", BinPropertyType.UInt32)]
        public uint AudioType { get; set; } = 0;
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty(3743124039, BinPropertyType.Float)]
        public float m3743124039 { get; set; } = 0f;
    }
    [MetaClass("AbilityResourceThresholdIndicatorRange")]
    public class AbilityResourceThresholdIndicatorRange : IMetaClass
    {
        [MetaProperty("rangeEnd", BinPropertyType.Float)]
        public float RangeEnd { get; set; } = 0f;
        [MetaProperty("rangeStart", BinPropertyType.Float)]
        public float RangeStart { get; set; } = 0f;
    }
    [MetaClass("IdMappingEntry")]
    public class IdMappingEntry : IMetaClass
    {
        [MetaProperty("ID", BinPropertyType.UInt16)]
        public ushort ID { get; set; } = 0;
        [MetaProperty("Count", BinPropertyType.UInt16)]
        public ushort Count { get; set; } = 0;
    }
    [MetaClass("TogglePlantFastRespawnCheat")]
    public class TogglePlantFastRespawnCheat : Cheat
    {
    }
    [MetaClass("EffectRotatingIconElementData")]
    public class EffectRotatingIconElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
    }
    [MetaClass("ItemCareyOverrideStartingItemSet")]
    public class ItemCareyOverrideStartingItemSet : IMetaClass
    {
        [MetaProperty("mAttribute", BinPropertyType.Hash)]
        public MetaHash Attribute { get; set; } = new(0);
    }
    [MetaClass("MapLocator")]
    public class MapLocator : GenericMapPlaceable
    {
    }
    [MetaClass("NotScriptCondition")]
    public class NotScriptCondition : IScriptCondition
    {
        [MetaProperty("Condition", BinPropertyType.Structure)]
        public IScriptCondition Condition { get; set; } = null;
    }
    [MetaClass("VfxPaletteDefinitionData")]
    public class VfxPaletteDefinitionData : IMetaClass
    {
        [MetaProperty("palleteSrcMixColor", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> PalleteSrcMixColor { get; set; } = new (new ());
        [MetaProperty(886635206, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> m886635206 { get; set; } = new (new ());
        [MetaProperty(1157448907, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> m1157448907 { get; set; } = new (new ());
        [MetaProperty("PaletteTextureAddressMode", BinPropertyType.Byte)]
        public byte PaletteTextureAddressMode { get; set; } = 1;
        [MetaProperty("paletteSelector", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> PaletteSelector { get; set; } = new (new ());
        [MetaProperty("paletteCount", BinPropertyType.Int32)]
        public int PaletteCount { get; set; } = 1;
        [MetaProperty("paletteTexture", BinPropertyType.String)]
        public string PaletteTexture { get; set; } = "";
    }
    [MetaClass("FadeToExplicitValueBehavior")]
    public class FadeToExplicitValueBehavior : ITargeterFadeBehavior
    {
        [MetaProperty("mAlpha", BinPropertyType.Float)]
        public float Alpha { get; set; } = 1f;
    }
    [MetaClass("VfxSpawnConditions")]
    public class VfxSpawnConditions : IMetaClass
    {
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; } = new (new ());
        [MetaProperty("mConditionalVfxData", BinPropertyType.Container)]
        public MetaContainer<VFXSpawnConditionData> ConditionalVfxData { get; set; } = new();
    }
    [MetaClass("RemoveFromCustomTableBlock")]
    public class RemoveFromCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("Key", BinPropertyType.Structure)]
        public IScriptValueGet Key { get; set; } = null;
        [MetaProperty("Index", BinPropertyType.Structure)]
        public IIntGet Index { get; set; } = null;
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("LoadoutSelectViewController")]
    public class LoadoutSelectViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("LoadoutsButtonData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ILoadoutFeatureDataBehavior>> LoadoutsButtonData { get; set; } = new();
    }
    [MetaClass("VfxFieldAccelerationDefinitionData")]
    public class VfxFieldAccelerationDefinitionData : IMetaClass
    {
        [MetaProperty("acceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Acceleration { get; set; } = new (new ());
        [MetaProperty("isLocalSpace", BinPropertyType.Bool)]
        public bool IsLocalSpace { get; set; } = true;
    }
    [MetaClass("OptionItemSliderFloat")]
    public class OptionItemSliderFloat : OptionItemSlider
    {
        [MetaProperty("TooltipTraKey", BinPropertyType.String)]
        public string TooltipTraKey { get; set; } = "";
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("UpdateOnDrag", BinPropertyType.Bool)]
        public bool UpdateOnDrag { get; set; } = true;
        [MetaProperty("scale", BinPropertyType.Float)]
        public float Scale { get; set; } = 1f;
        [MetaProperty("option", BinPropertyType.UInt16)]
        public ushort Option { get; set; } = 65535;
    }
    [MetaClass("RemapFloatMaterialDriver")]
    public class RemapFloatMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mMinValue", BinPropertyType.Float)]
        public float MinValue { get; set; } = 0f;
        [MetaProperty("mMaxValue", BinPropertyType.Float)]
        public float MaxValue { get; set; } = 1f;
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; } = null;
        [MetaProperty("mOutputMaxValue", BinPropertyType.Float)]
        public float OutputMaxValue { get; set; } = 1f;
        [MetaProperty("mOutputMinValue", BinPropertyType.Float)]
        public float OutputMinValue { get; set; } = 0f;
    }
    [MetaClass("GameModeConstantFloatPerLevel")]
    public class GameModeConstantFloatPerLevel : GameModeConstant
    {
        [MetaProperty("mValues", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; } = new();
    }
    [MetaClass("Cast")]
    public class Cast : MissileTriggeredActionSpec
    {
    }
    [MetaClass("OptionTemplateCheckbox")]
    public class OptionTemplateCheckbox : IOptionTemplate
    {
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
        [MetaProperty("labelElement", BinPropertyType.Hash)]
        public MetaHash LabelElement { get; set; } = new(0);
    }
    [MetaClass("DamageShieldedLogic")]
    public class DamageShieldedLogic : IStatStoneLogicDriver
    {
        [MetaProperty(385993226, BinPropertyType.Bool)]
        public bool m385993226 { get; set; } = false;
        [MetaProperty(718612390, BinPropertyType.Bool)]
        public bool m718612390 { get; set; } = false;
        [MetaProperty(788241703, BinPropertyType.Bool)]
        public bool m788241703 { get; set; } = true;
        [MetaProperty(1622655414, BinPropertyType.Bool)]
        public bool m1622655414 { get; set; } = true;
        [MetaProperty(1646138587, BinPropertyType.Bool)]
        public bool m1646138587 { get; set; } = true;
        [MetaProperty(2034521402, BinPropertyType.Bool)]
        public bool m2034521402 { get; set; } = true;
        [MetaProperty(2715825086, BinPropertyType.Bool)]
        public bool m2715825086 { get; set; } = true;
        [MetaProperty(3418552506, BinPropertyType.Bool)]
        public bool m3418552506 { get; set; } = true;
    }
    [MetaClass("EffectGlowingRotatingIconElementData")]
    public class EffectGlowingRotatingIconElementData : EffectRotatingIconElementData
    {
        [MetaProperty(88872846, BinPropertyType.Float)]
        public float m88872846 { get; set; } = 0f;
        [MetaProperty("CycleTime", BinPropertyType.Float)]
        public float CycleTime { get; set; } = 0f;
    }
    [MetaClass(2861933169)]
    public class Class0xaa95a271 : AnnouncementStyleBasic
    {
        [MetaProperty("RightIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink RightIcon { get; set; } = new(0);
        [MetaProperty("LeftIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink LeftIcon { get; set; } = new(0);
    }
    [MetaClass("CustomTableSet")]
    public class CustomTableSet : ScriptTableSet
    {
    }
    [MetaClass("SceneData")]
    public class SceneData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(574697843, BinPropertyType.Bool)]
        public bool m574697843 { get; set; } = false;
        [MetaProperty("mHealthBar", BinPropertyType.Bool)]
        public bool HealthBar { get; set; } = false;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("SceneTransitionOut", BinPropertyType.Structure)]
        public Class0xd1951f45 SceneTransitionOut { get; set; } = null;
        [MetaProperty("SceneTransitionIn", BinPropertyType.Structure)]
        public Class0xd1951f45 SceneTransitionIn { get; set; } = null;
        [MetaProperty("mParentScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink ParentScene { get; set; } = new(0);
        [MetaProperty(4160985070, BinPropertyType.Bool)]
        public bool m4160985070 { get; set; } = true;
    }
    [MetaClass(2872907111)]
    public class Class0xab3d1567 : AtlasDataBase
    {
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
        [MetaProperty(367825281, BinPropertyType.Vector2)]
        public Vector2 m367825281 { get; set; } = new Vector2(0.25f, 0.25f);
        [MetaProperty(458738727, BinPropertyType.Vector2)]
        public Vector2 m458738727 { get; set; } = new Vector2(0.25f, 0.25f);
        [MetaProperty("TextureUs", BinPropertyType.Vector4)]
        public Vector4 TextureUs { get; set; } = new Vector4(0f, 0.25f, 0.75f, 1f);
        [MetaProperty("TextureVs", BinPropertyType.Vector4)]
        public Vector4 TextureVs { get; set; } = new Vector4(0f, 0.25f, 0.75f, 1f);
    }
    [MetaClass("QuestDefinition")]
    public class QuestDefinition : IMetaClass
    {
        [MetaProperty("recievedSoundPath", BinPropertyType.String)]
        public string RecievedSoundPath { get; set; } = "";
        [MetaProperty("completedSoundPath", BinPropertyType.String)]
        public string CompletedSoundPath { get; set; } = "";
        [MetaProperty("categoryTitleText", BinPropertyType.String)]
        public string CategoryTitleText { get; set; } = "";
        [MetaProperty("maxViewableQuests", BinPropertyType.UInt32)]
        public uint MaxViewableQuests { get; set; } = 5;
        [MetaProperty("failedSoundPath", BinPropertyType.String)]
        public string FailedSoundPath { get; set; } = "";
    }
    [MetaClass("SwitchMaterialDriver")]
    public class SwitchMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mElements", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SwitchMaterialDriverElement>> Elements { get; set; } = new();
        [MetaProperty("mDefaultValue", BinPropertyType.Structure)]
        public IDynamicMaterialDriver DefaultValue { get; set; } = null;
    }
    [MetaClass("CrowdControlFilter")]
    public class CrowdControlFilter : IStatStoneLogicDriver
    {
        [MetaProperty(550470828, BinPropertyType.Container)]
        public MetaContainer<byte> m550470828 { get; set; } = new();
        [MetaProperty("TrackDuration", BinPropertyType.Bool)]
        public bool TrackDuration { get; set; } = true;
    }
    [MetaClass("VfxAnimatedVector3fVariableData")]
    public class VfxAnimatedVector3fVariableData : IMetaClass
    {
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<Vector3> Values { get; set; } = new();
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; } = new();
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; } = new();
    }
    [MetaClass("EffectInstancedElementData")]
    public class EffectInstancedElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mAdditionalOffsets", BinPropertyType.Container)]
        public MetaContainer<Vector2> AdditionalOffsets { get; set; } = new();
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
    }
    [MetaClass("MapLocatorArray")]
    public class MapLocatorArray : IMetaClass
    {
        [MetaProperty("locators", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapLocator>> Locators { get; set; } = new();
    }
    [MetaClass("ProductOfSubPartsCalculationPart")]
    public class ProductOfSubPartsCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mPart1", BinPropertyType.Structure)]
        public IGameCalculationPart Part1 { get; set; } = null;
        [MetaProperty("mPart2", BinPropertyType.Structure)]
        public IGameCalculationPart Part2 { get; set; } = null;
    }
    [MetaClass("IBoolGet")]
    public interface IBoolGet : IScriptValueGet
    {
    }
    [MetaClass("TrueDamageGivenFilter")]
    public class TrueDamageGivenFilter : IStatStoneLogicDriver
    {
    }
    [MetaClass("TftMapCharacterList")]
    public class TftMapCharacterList : IMetaClass
    {
        [MetaProperty("ListName", BinPropertyType.String)]
        public string ListName { get; set; } = "";
        [MetaProperty("characters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftMapCharacterData>> Characters { get; set; } = new();
    }
    [MetaClass("Joint")]
    public class Joint : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mNameHash", BinPropertyType.UInt32)]
        public uint NameHash { get; set; } = 0;
        [MetaProperty("mParentIndex", BinPropertyType.Int16)]
        public short ParentIndex { get; set; } = -1;
        [MetaProperty("mRadius", BinPropertyType.Float)]
        public float Radius { get; set; } = 0f;
        [MetaProperty("mIndex", BinPropertyType.UInt16)]
        public ushort Index { get; set; } = 0;
        [MetaProperty("mFlags", BinPropertyType.UInt16)]
        public ushort Flags { get; set; } = 0;
    }
    [MetaClass("TftUnitShopViewController")]
    public class TftUnitShopViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("BuyExpButton", BinPropertyType.Hash)]
        public MetaHash BuyExpButton { get; set; } = new(0);
        [MetaProperty(662826347, BinPropertyType.Map)]
        public Dictionary<uint, MetaEmbedded<Class0x2781ed6b>> m662826347 { get; set; } = new();
        [MetaProperty("ToggleShopButton", BinPropertyType.Hash)]
        public MetaHash ToggleShopButton { get; set; } = new(0);
        [MetaProperty("InfoButton", BinPropertyType.Hash)]
        public MetaHash InfoButton { get; set; } = new(0);
        [MetaProperty("LockButton", BinPropertyType.Hash)]
        public MetaHash LockButton { get; set; } = new(0);
        [MetaProperty("RerollButton", BinPropertyType.Hash)]
        public MetaHash RerollButton { get; set; } = new(0);
        [MetaProperty(2695789818, BinPropertyType.Hash)]
        public MetaHash m2695789818 { get; set; } = new(0);
        [MetaProperty(3292151071, BinPropertyType.Float)]
        public float m3292151071 { get; set; } = 0f;
    }
    [MetaClass(2938323084)]
    public class Class0xaf23408c : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty(366677381, BinPropertyType.String)]
        public string m366677381 { get; set; } = "";
        [MetaProperty(698871750, BinPropertyType.Float)]
        public float m698871750 { get; set; } = 1f;
        [MetaProperty(1543776582, BinPropertyType.Container)]
        public MetaContainer<string> m1543776582 { get; set; } = new();
        [MetaProperty("Shuffle", BinPropertyType.Bool)]
        public bool Shuffle { get; set; } = false;
    }
    [MetaClass("DragDirection")]
    public class DragDirection : TargetingTypeData
    {
    }
    [MetaClass(2944059703)]
    public class Class0xaf7ac937 : IMetaClass
    {
        [MetaProperty(1943793530, BinPropertyType.String)]
        public string m1943793530 { get; set; } = "";
        [MetaProperty(2400858989, BinPropertyType.String)]
        public string m2400858989 { get; set; } = "";
        [MetaProperty(3624350102, BinPropertyType.String)]
        public string m3624350102 { get; set; } = "";
    }
    [MetaClass("AcceleratingMovement")]
    public class AcceleratingMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool ProjectTargetToCastRange { get; set; } = false;
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool UseGroundHeightAtTarget { get; set; } = true;
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool InferDirectionFromFacingIfNeeded { get; set; } = true;
        [MetaProperty("mInitialSpeed", BinPropertyType.Float)]
        public float InitialSpeed { get; set; } = 0f;
        [MetaProperty("mAcceleration", BinPropertyType.Float)]
        public float Acceleration { get; set; } = 0f;
        [MetaProperty("mMinSpeed", BinPropertyType.Float)]
        public float MinSpeed { get; set; } = 0f;
        [MetaProperty("mMaxSpeed", BinPropertyType.Float)]
        public float MaxSpeed { get; set; } = 0f;
    }
    [MetaClass("NamedIconData")]
    public class NamedIconData : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("Icon", BinPropertyType.Hash)]
        public MetaHash Icon { get; set; } = new(0);
    }
    [MetaClass("OptionsTab")]
    public class OptionsTab : IMetaClass
    {
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<IOptionItem> Items { get; set; } = new();
        [MetaProperty(2455093330, BinPropertyType.Bool)]
        public bool m2455093330 { get; set; } = true;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("TabNameTraKey", BinPropertyType.String)]
        public string TabNameTraKey { get; set; } = "";
        [MetaProperty("ShowOn", BinPropertyType.Byte)]
        public byte ShowOn { get; set; } = 1;
    }
    [MetaClass("AlwaysSpawnCondition")]
    public class AlwaysSpawnCondition : IVFXSpawnConditions
    {
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; } = new (new ());
    }
    [MetaClass("ToolEducationData")]
    public class ToolEducationData : IMetaClass
    {
        [MetaProperty("firstItem", BinPropertyType.Int32)]
        public int FirstItem { get; set; } = 0;
        [MetaProperty("skillOrder", BinPropertyType.Int32)]
        public int SkillOrder { get; set; } = 0;
    }
    [MetaClass("TargetHasBuffFilter")]
    public class TargetHasBuffFilter : IStatStoneLogicDriver
    {
        [MetaProperty("ValidBuffs", BinPropertyType.Container)]
        public MetaContainer<byte> ValidBuffs { get; set; } = new();
    }
    [MetaClass("PerkEffectAmountPerMode")]
    public class PerkEffectAmountPerMode : IMetaClass
    {
        [MetaProperty("mEffectAmountPerMode", BinPropertyType.Map)]
        public Dictionary<MetaHash, float> EffectAmountPerMode { get; set; } = new();
    }
    [MetaClass("NavHeaderViewController")]
    public class NavHeaderViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("StarShardsButton", BinPropertyType.Hash)]
        public MetaHash StarShardsButton { get; set; } = new(0);
        [MetaProperty("settingsButton", BinPropertyType.Hash)]
        public MetaHash SettingsButton { get; set; } = new(0);
        [MetaProperty(2072938133, BinPropertyType.Hash)]
        public MetaHash m2072938133 { get; set; } = new(0);
        [MetaProperty(2476808443, BinPropertyType.Hash)]
        public MetaHash m2476808443 { get; set; } = new(0);
        [MetaProperty("notificationsButton", BinPropertyType.Hash)]
        public MetaHash NotificationsButton { get; set; } = new(0);
        [MetaProperty("socialButton", BinPropertyType.Hash)]
        public MetaHash SocialButton { get; set; } = new(0);
        [MetaProperty("backButton", BinPropertyType.Hash)]
        public MetaHash BackButton { get; set; } = new(0);
        [MetaProperty("MissionsButton", BinPropertyType.Hash)]
        public MetaHash MissionsButton { get; set; } = new(0);
    }
    [MetaClass("HudScaleSettingsData")]
    public class HudScaleSettingsData : IMetaClass
    {
        [MetaProperty("maximumMinimapScale", BinPropertyType.Float)]
        public float MaximumMinimapScale { get; set; } = 1f;
        [MetaProperty("minimumMinimapScale", BinPropertyType.Float)]
        public float MinimumMinimapScale { get; set; } = 0.5f;
        [MetaProperty("maximumPracticeToolScale", BinPropertyType.Float)]
        public float MaximumPracticeToolScale { get; set; } = 1f;
        [MetaProperty("minimumGlobalScale", BinPropertyType.Float)]
        public float MinimumGlobalScale { get; set; } = 0.5f;
        [MetaProperty("maximumGlobalScale", BinPropertyType.Float)]
        public float MaximumGlobalScale { get; set; } = 1f;
        [MetaProperty(1804113590, BinPropertyType.Float)]
        public float m1804113590 { get; set; } = 1f;
        [MetaProperty("minimumChatScale", BinPropertyType.Float)]
        public float MinimumChatScale { get; set; } = 0.5f;
        [MetaProperty("minimumPracticeToolScale", BinPropertyType.Float)]
        public float MinimumPracticeToolScale { get; set; } = 0.5f;
        [MetaProperty(3043348288, BinPropertyType.Float)]
        public float m3043348288 { get; set; } = 0.5f;
        [MetaProperty("maximumChatScale", BinPropertyType.Float)]
        public float MaximumChatScale { get; set; } = 1f;
    }
    [MetaClass("LoadoutViewController")]
    public class LoadoutViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("SoundOnDeActivate", BinPropertyType.String)]
        public string SoundOnDeActivate { get; set; } = "";
        [MetaProperty("equipButton", BinPropertyType.Hash)]
        public MetaHash EquipButton { get; set; } = new(0);
        [MetaProperty("DamageSkinInfoPanel", BinPropertyType.Embedded)]
        public MetaEmbedded<LoadoutDamageSkinInfoPanel> DamageSkinInfoPanel { get; set; } = new (new ());
        [MetaProperty("EmoteInfoPanel", BinPropertyType.Embedded)]
        public MetaEmbedded<LoadoutEmoteInfoPanel> EmoteInfoPanel { get; set; } = new (new ());
        [MetaProperty(2439455768, BinPropertyType.Hash)]
        public MetaHash m2439455768 { get; set; } = new(0);
        [MetaProperty("SoundOnActivate", BinPropertyType.String)]
        public string SoundOnActivate { get; set; } = "";
        [MetaProperty("closeButton", BinPropertyType.Hash)]
        public MetaHash CloseButton { get; set; } = new(0);
        [MetaProperty("gridItemButton", BinPropertyType.Hash)]
        public MetaHash GridItemButton { get; set; } = new(0);
        [MetaProperty(3517705117, BinPropertyType.Hash)]
        public MetaHash m3517705117 { get; set; } = new(0);
        [MetaProperty("CompanionInfoPanel", BinPropertyType.Embedded)]
        public MetaEmbedded<LoadoutCompanionInfoPanel> CompanionInfoPanel { get; set; } = new (new ());
        [MetaProperty("upgradeButton", BinPropertyType.Hash)]
        public MetaHash UpgradeButton { get; set; } = new(0);
        [MetaProperty("ArenaInfoPanel", BinPropertyType.Embedded)]
        public MetaEmbedded<LoadoutArenaSkinInfoPanel> ArenaInfoPanel { get; set; } = new (new ());
    }
    [MetaClass("MapPlaceableContainer")]
    public class MapPlaceableContainer : IMetaClass
    {
        [MetaProperty("items", BinPropertyType.Map)]
        public Dictionary<MetaHash, MapPlaceable> Items { get; set; } = new();
    }
    [MetaClass("DecelToLocationMovement")]
    public class DecelToLocationMovement : AcceleratingMovement
    {
    }
    [MetaClass("OptionItemDropdown")]
    public class OptionItemDropdown : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<OptionItemDropdownItem>> Items { get; set; } = new();
        [MetaProperty("TooltipTraKey", BinPropertyType.String)]
        public string TooltipTraKey { get; set; } = "";
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
        [MetaProperty("option", BinPropertyType.UInt16)]
        public ushort Option { get; set; } = 65535;
    }
    [MetaClass("TooltipFormat")]
    public class TooltipFormat : IMetaClass
    {
        [MetaProperty("mOutputStrings", BinPropertyType.Map)]
        public Dictionary<string, string> OutputStrings { get; set; } = new();
        [MetaProperty("mListNames", BinPropertyType.Container)]
        public MetaContainer<string> ListNames { get; set; } = new();
        [MetaProperty("mListStyles", BinPropertyType.Map)]
        public Dictionary<uint, string> ListStyles { get; set; } = new();
        [MetaProperty("mListGridPrefix", BinPropertyType.String)]
        public string ListGridPrefix { get; set; } = "";
        [MetaProperty("mListTypeChoices", BinPropertyType.Map)]
        public Dictionary<string, string> ListTypeChoices { get; set; } = new();
        [MetaProperty("mListGridSeparator", BinPropertyType.String)]
        public string ListGridSeparator { get; set; } = "";
        [MetaProperty("mListGridPostfix", BinPropertyType.String)]
        public string ListGridPostfix { get; set; } = "";
        [MetaProperty("mObjectName", BinPropertyType.String)]
        public string ObjectName { get; set; } = "";
        [MetaProperty("mListValueSeparator", BinPropertyType.String)]
        public string ListValueSeparator { get; set; } = "";
        [MetaProperty("mInputLocKeysWithDefaults", BinPropertyType.Map)]
        public Dictionary<string, string> InputLocKeysWithDefaults { get; set; } = new();
        [MetaProperty("mUsesListValues", BinPropertyType.Bool)]
        public bool UsesListValues { get; set; } = false;
    }
    [MetaClass("ItemDataValue")]
    public class ItemDataValue : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
    }
    [MetaClass("BaseRigPoseModifierData")]
    public interface BaseRigPoseModifierData : IMetaClass
    {
    }
    [MetaClass("CursorData")]
    public class CursorData : IMetaClass
    {
        [MetaProperty("mColorblindTextureName", BinPropertyType.String)]
        public string ColorblindTextureName { get; set; } = "";
        [MetaProperty("mHotSpot", BinPropertyType.Vector2)]
        public Vector2 HotSpot { get; set; } = new Vector2(0f, 0f);
        [MetaProperty(3527462479, BinPropertyType.String)]
        public string m3527462479 { get; set; } = "";
        [MetaProperty(4132662353, BinPropertyType.String)]
        public string m4132662353 { get; set; } = "";
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
    }
    [MetaClass(3009075672)]
    public class Class0xb35ad9d8 : IMetaClass
    {
        [MetaProperty("defaultIndex", BinPropertyType.Int32)]
        public int DefaultIndex { get; set; } = -1;
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Items { get; set; } = new();
        [MetaProperty("SwapData", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xd149dd3f> SwapData { get; set; } = new (new ());
        [MetaProperty(3636372294, BinPropertyType.Bool)]
        public bool m3636372294 { get; set; } = false;
        [MetaProperty("ItemsPerRow", BinPropertyType.UInt32)]
        public uint ItemsPerRow { get; set; } = 1;
    }
    [MetaClass("HudEndOfGameData")]
    public class HudEndOfGameData : IMetaClass
    {
        [MetaProperty("mVictoryTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> VictoryTransitionIn { get; set; } = new (new ());
        [MetaProperty("mDefeatTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> DefeatTransitionIn { get; set; } = new (new ());
    }
    [MetaClass("MapAudioDataProperties")]
    public class MapAudioDataProperties : IMetaClass
    {
        [MetaProperty("features", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Features { get; set; } = new();
        [MetaProperty("BaseData", BinPropertyType.ObjectLink)]
        public MetaObjectLink BaseData { get; set; } = new(0);
        [MetaProperty("bankUnits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BankUnit>> BankUnits { get; set; } = new();
    }
    [MetaClass("JointSnapRigPoseModifilerData")]
    public class JointSnapRigPoseModifilerData : BaseRigPoseModifierData
    {
    }
    [MetaClass("AbilityResourceTypeData")]
    public class AbilityResourceTypeData : IMetaClass
    {
        [MetaProperty("ThresholdIndicatorRanges", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AbilityResourceThresholdIndicatorRange>> ThresholdIndicatorRanges { get; set; } = new();
        [MetaProperty("showRegen", BinPropertyType.Bool)]
        public bool ShowRegen { get; set; } = false;
        [MetaProperty("showAbilityResource", BinPropertyType.Bool)]
        public bool ShowAbilityResource { get; set; } = false;
        [MetaProperty("states", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AbilityResourceStateData>> States { get; set; } = new();
    }
    [MetaClass("TooltipViewController")]
    public class TooltipViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("DefaultAdjustments", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x9e5aed77> DefaultAdjustments { get; set; } = new (new ());
        [MetaProperty(1963643445, BinPropertyType.Float)]
        public float m1963643445 { get; set; } = 0f;
        [MetaProperty(2653932244, BinPropertyType.Float)]
        public float m2653932244 { get; set; } = 0f;
        [MetaProperty(4126940474, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<Class0x9e5aed77>> m4126940474 { get; set; } = new();
    }
    [MetaClass("MapActionPlayAnimation")]
    public class MapActionPlayAnimation : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty("PropName", BinPropertyType.String)]
        public string PropName { get; set; } = "";
        [MetaProperty("looping", BinPropertyType.Bool)]
        public bool Looping { get; set; } = false;
        [MetaProperty("animationName", BinPropertyType.String)]
        public string AnimationName { get; set; } = "";
    }
    [MetaClass("SummonerSpellPerkReplacementList")]
    public class SummonerSpellPerkReplacementList : IMetaClass
    {
        [MetaProperty("mReplacements", BinPropertyType.Container)]
        public MetaContainer<SummonerSpellPerkReplacement> Replacements { get; set; } = new();
    }
    [MetaClass("ContextualConditionCharacterHealth")]
    public class ContextualConditionCharacterHealth : ICharacterSubcondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 0;
        [MetaProperty("mTargetHealth", BinPropertyType.Float)]
        public float TargetHealth { get; set; } = 0f;
    }
    [MetaClass("GameplayFeatureToggles")]
    public class GameplayFeatureToggles : IMetaClass
    {
        [MetaProperty("NewActorStuckPathfinding", BinPropertyType.Bool)]
        public bool NewActorStuckPathfinding { get; set; } = false;
        [MetaProperty("fowCastRayAccurate", BinPropertyType.Bool)]
        public bool FowCastRayAccurate { get; set; } = false;
        [MetaProperty("disableSpellLevelMinimumProtections", BinPropertyType.Bool)]
        public bool DisableSpellLevelMinimumProtections { get; set; } = false;
        [MetaProperty("IndividualItemVisibility", BinPropertyType.Bool)]
        public bool IndividualItemVisibility { get; set; } = true;
        [MetaProperty("AFKDetection2", BinPropertyType.Bool)]
        public bool AFKDetection2 { get; set; } = true;
    }
    [MetaClass("SkinFilterData")]
    public class SkinFilterData : IMetaClass
    {
        [MetaProperty("skinIds", BinPropertyType.Container)]
        public MetaContainer<uint> SkinIds { get; set; } = new();
        [MetaProperty("FilterType", BinPropertyType.UInt32)]
        public uint FilterType { get; set; } = 0;
        [MetaProperty(3245789543, BinPropertyType.Bool)]
        public bool m3245789543 { get; set; } = true;
    }
    [MetaClass("GearData")]
    public class GearData : IMetaClass
    {
        [MetaProperty(565581438, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m565581438 { get; set; } = new();
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; } = new (new ());
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; } = null;
        [MetaProperty("mSelfOnlyPortraitIcon", BinPropertyType.String)]
        public string SelfOnlyPortraitIcon { get; set; } = "";
        [MetaProperty("mPortraitIcon", BinPropertyType.String)]
        public string PortraitIcon { get; set; } = "";
        [MetaProperty("mEquipAnimation", BinPropertyType.String)]
        public string EquipAnimation { get; set; } = "";
        [MetaProperty(3066053883, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m3066053883 { get; set; } = new();
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnimationGraphData { get; set; } = new(0);
    }
    [MetaClass("ClientStateAudioDataProperties")]
    public class ClientStateAudioDataProperties : IMetaClass
    {
        [MetaProperty("themeMusic", BinPropertyType.String)]
        public string ThemeMusic { get; set; } = "";
        [MetaProperty("BankPaths", BinPropertyType.Container)]
        public MetaContainer<string> BankPaths { get; set; } = new();
    }
    [MetaClass("ContextualConditionRuleCooldown")]
    public class ContextualConditionRuleCooldown : IContextualCondition
    {
        [MetaProperty("mRuleCooldown", BinPropertyType.Float)]
        public float RuleCooldown { get; set; } = 0f;
    }
    [MetaClass("LerpMaterialDriver")]
    public class LerpMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mTurnOffTimeSec", BinPropertyType.Float)]
        public float TurnOffTimeSec { get; set; } = 1f;
        [MetaProperty("mOnValue", BinPropertyType.Float)]
        public float OnValue { get; set; } = 1f;
        [MetaProperty("mTurnOnTimeSec", BinPropertyType.Float)]
        public float TurnOnTimeSec { get; set; } = 1f;
        [MetaProperty(2756886175, BinPropertyType.Bool)]
        public bool m2756886175 { get; set; } = false;
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; } = null;
        [MetaProperty("mOffValue", BinPropertyType.Float)]
        public float OffValue { get; set; } = 0f;
    }
    [MetaClass("VfxChildParticleSetDefinitionData")]
    public class VfxChildParticleSetDefinitionData : IMetaClass
    {
        [MetaProperty("boneToSpawnAt", BinPropertyType.Container)]
        public MetaContainer<string> BoneToSpawnAt { get; set; } = new();
        [MetaProperty("childrenIdentifiers", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxChildIdentifier>> ChildrenIdentifiers { get; set; } = new();
        [MetaProperty("childEmitOnDeath", BinPropertyType.Bool)]
        public bool ChildEmitOnDeath { get; set; } = false;
        [MetaProperty("childrenProbability", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> ChildrenProbability { get; set; } = new (new ());
    }
    [MetaClass("ToggleRegenCheat")]
    public class ToggleRegenCheat : Cheat
    {
        [MetaProperty("mToggleHP", BinPropertyType.Bool)]
        public bool ToggleHP { get; set; } = false;
        [MetaProperty("mTogglePAR", BinPropertyType.Bool)]
        public bool TogglePAR { get; set; } = false;
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass("JointSnapEventData")]
    public class JointSnapEventData : BaseEventData
    {
        [MetaProperty("mJointNameToOverride", BinPropertyType.Hash)]
        public MetaHash JointNameToOverride { get; set; } = new(0);
        [MetaProperty("mJointNameToSnapTo", BinPropertyType.Hash)]
        public MetaHash JointNameToSnapTo { get; set; } = new(0);
    }
    [MetaClass(3050387163)]
    public class Class0xb5d136db : AtlasDataBase
    {
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint TextureSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string TextureName { get; set; } = "";
        [MetaProperty(458738727, BinPropertyType.Vector2)]
        public Vector2 m458738727 { get; set; } = new Vector2(0.25f, 0.25f);
        [MetaProperty("TextureUs", BinPropertyType.Vector4)]
        public Vector4 TextureUs { get; set; } = new Vector4(0f, 0.25f, 0.75f, 1f);
        [MetaProperty("TextureVs", BinPropertyType.Vector2)]
        public Vector2 TextureVs { get; set; } = new Vector2(0f, 1f);
    }
    [MetaClass("IGameCalculationPartWithStats")]
    public interface IGameCalculationPartWithStats : IGameCalculationPart
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        byte Stat { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        byte StatFormula { get; set; }
    }
    [MetaClass("IGameCalculationPart")]
    public interface IGameCalculationPart : IMetaClass
    {
    }
    [MetaClass("ModeSelectButtonData")]
    public class ModeSelectButtonData : IMetaClass
    {
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
        [MetaProperty("queueId", BinPropertyType.Int64)]
        public long QueueId { get; set; } = 0;
    }
    [MetaClass("VfxFieldOrbitalDefinitionData")]
    public class VfxFieldOrbitalDefinitionData : IMetaClass
    {
        [MetaProperty("isLocalSpace", BinPropertyType.Bool)]
        public bool IsLocalSpace { get; set; } = true;
        [MetaProperty("direction", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Direction { get; set; } = new (new ());
    }
    [MetaClass("IFxAction")]
    public interface IFxAction : IMetaClass
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        MetaEmbedded<FxTiming> Start { get; set; }
        [MetaProperty("End", BinPropertyType.Embedded)]
        MetaEmbedded<FxTiming> End { get; set; }
    }
    [MetaClass("ContextualConditionCharacterFormName")]
    public class ContextualConditionCharacterFormName : ICharacterSubcondition
    {
        [MetaProperty("mFormName", BinPropertyType.String)]
        public string FormName { get; set; } = "";
    }
    [MetaClass("Perk")]
    public class Perk : BasePerk
    {
        [MetaProperty("mPingTextLocalizationKey", BinPropertyType.String)]
        public string PingTextLocalizationKey { get; set; } = "";
        [MetaProperty("mDisplayNameLocalizationKey", BinPropertyType.String)]
        public string DisplayNameLocalizationKey { get; set; } = "";
        [MetaProperty("mPerkId", BinPropertyType.UInt32)]
        public uint PerkId { get; set; } = 0;
        [MetaProperty("mTooltipNameLocalizationKey", BinPropertyType.String)]
        public string TooltipNameLocalizationKey { get; set; } = "";
        [MetaProperty("mScript", BinPropertyType.Structure)]
        public PerkScript Script { get; set; } = null;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("mPerkName", BinPropertyType.String)]
        public string PerkName { get; set; } = "";
        [MetaProperty("mIconTextureName", BinPropertyType.String)]
        public string IconTextureName { get; set; } = "";
        [MetaProperty("mShortDescLocalizationKey", BinPropertyType.String)]
        public string ShortDescLocalizationKey { get; set; } = "";
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; } = null;
        [MetaProperty("mLongDescLocalizationKey", BinPropertyType.String)]
        public string LongDescLocalizationKey { get; set; } = "";
        [MetaProperty("mStackable", BinPropertyType.Bool)]
        public bool Stackable { get; set; } = false;
        [MetaProperty("mDisplayStatLocalizationKey", BinPropertyType.String)]
        public string DisplayStatLocalizationKey { get; set; } = "";
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        public MetaContainer<string> Characters { get; set; } = new();
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        public MetaContainer<PerkBuff> Buffs { get; set; } = new();
        [MetaProperty("mEndOfGameStatDescriptions", BinPropertyType.Container)]
        public MetaContainer<string> EndOfGameStatDescriptions { get; set; } = new();
        [MetaProperty("mMajorChangePatchVersion", BinPropertyType.String)]
        public string MajorChangePatchVersion { get; set; } = "";
        [MetaProperty("mDefault", BinPropertyType.Bool)]
        public bool Default { get; set; } = false;
        [MetaProperty("mSummonerPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<SummonerSpellPerkReplacementList> SummonerPerkReplacements { get; set; } = new (new ());
    }
    [MetaClass("ScissorRegionElementData")]
    public class ScissorRegionElementData : BaseElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty(2899083445, BinPropertyType.ObjectLink)]
        public MetaObjectLink m2899083445 { get; set; } = new(0);
    }
    [MetaClass("MaterialSwitchDataCollection")]
    public class MaterialSwitchDataCollection : IMetaClass
    {
        [MetaProperty("Entries", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<IdMappingEntry>> Entries { get; set; } = new();
        [MetaProperty("nextID", BinPropertyType.UInt16)]
        public ushort NextID { get; set; } = 1;
        [MetaProperty("data", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialSwitchData>> Data { get; set; } = new();
        [MetaProperty(3931619090, BinPropertyType.String)]
        public string m3931619090 { get; set; } = "";
    }
    [MetaClass("StatFormulaData")]
    public class StatFormulaData : IMetaClass
    {
        [MetaProperty("StatComponents", BinPropertyType.Map)]
        public Dictionary<byte, float> StatComponents { get; set; } = new();
    }
    [MetaClass("ContextualActionPlayAnimation")]
    public class ContextualActionPlayAnimation : IContextualAction
    {
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        public uint MaxOccurences { get; set; } = 0;
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        public MetaHash HashedSituationTrigger { get; set; } = new(0);
        [MetaProperty("mHashedAnimationName", BinPropertyType.Hash)]
        public MetaHash HashedAnimationName { get; set; } = new(0);
        [MetaProperty("mPlayAsEmote", BinPropertyType.Bool)]
        public bool PlayAsEmote { get; set; } = false;
    }
    [MetaClass(3101122117)]
    public class Class0xb8d75e45 : IContextualConditionBuff
    {
        [MetaProperty(287338010, BinPropertyType.Byte)]
        public byte m287338010 { get; set; } = 0;
        [MetaProperty("mBuff", BinPropertyType.Hash)]
        public MetaHash Buff { get; set; } = new(0);
    }
    [MetaClass("TriggerOnDelay")]
    public class TriggerOnDelay : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; } = new();
        [MetaProperty("mDelay", BinPropertyType.Float)]
        public float Delay { get; set; } = 0f;
    }
    [MetaClass("IntTableGet")]
    public class IntTableGet : IIntGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<int> Default { get; set; } = new MetaOptional<int>(default(int), false);
    }
    [MetaClass("HybridMaterialDef")]
    public class HybridMaterialDef : CustomShaderDef
    {
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 1;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "normal";
        [MetaProperty("DataCollections", BinPropertyType.Embedded)]
        public MetaEmbedded<MaterialDataCollections> DataCollections { get; set; } = new (new ());
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; } = new();
        [MetaProperty("preset", BinPropertyType.ObjectLink)]
        public MetaObjectLink Preset { get; set; } = new(0);
    }
    [MetaClass("FloatingTextDamageDisplayTypeList")]
    public class FloatingTextDamageDisplayTypeList : IMetaClass
    {
        [MetaProperty("Impact", BinPropertyType.ObjectLink)]
        public MetaObjectLink Impact { get; set; } = new(0);
        [MetaProperty("Ult", BinPropertyType.ObjectLink)]
        public MetaObjectLink Ult { get; set; } = new(0);
        [MetaProperty("Zone", BinPropertyType.ObjectLink)]
        public MetaObjectLink Zone { get; set; } = new(0);
        [MetaProperty("SelfPhysicalDamageCounter", BinPropertyType.ObjectLink)]
        public MetaObjectLink SelfPhysicalDamageCounter { get; set; } = new(0);
        [MetaProperty("Multistrike", BinPropertyType.ObjectLink)]
        public MetaObjectLink Multistrike { get; set; } = new(0);
        [MetaProperty("SelfMagicalDamageCounter", BinPropertyType.ObjectLink)]
        public MetaObjectLink SelfMagicalDamageCounter { get; set; } = new(0);
        [MetaProperty("MultistrikeFast", BinPropertyType.ObjectLink)]
        public MetaObjectLink MultistrikeFast { get; set; } = new(0);
        [MetaProperty("Mini", BinPropertyType.ObjectLink)]
        public MetaObjectLink Mini { get; set; } = new(0);
        [MetaProperty("SelfTrueDamageCounter", BinPropertyType.ObjectLink)]
        public MetaObjectLink SelfTrueDamageCounter { get; set; } = new(0);
        [MetaProperty("Default", BinPropertyType.ObjectLink)]
        public MetaObjectLink Default { get; set; } = new(0);
        [MetaProperty("BarrackMinion", BinPropertyType.ObjectLink)]
        public MetaObjectLink BarrackMinion { get; set; } = new(0);
        [MetaProperty("MultistrikeSlow", BinPropertyType.ObjectLink)]
        public MetaObjectLink MultistrikeSlow { get; set; } = new(0);
        [MetaProperty("PlayerMinion", BinPropertyType.ObjectLink)]
        public MetaObjectLink PlayerMinion { get; set; } = new(0);
        [MetaProperty("Dot", BinPropertyType.ObjectLink)]
        public MetaObjectLink Dot { get; set; } = new(0);
        [MetaProperty("DotSlow", BinPropertyType.ObjectLink)]
        public MetaObjectLink DotSlow { get; set; } = new(0);
        [MetaProperty("DotNoCombine", BinPropertyType.ObjectLink)]
        public MetaObjectLink DotNoCombine { get; set; } = new(0);
    }
    [MetaClass("FontResolution")]
    public class FontResolution : IMetaClass
    {
        [MetaProperty("shadowDepthX", BinPropertyType.Int32)]
        public int ShadowDepthX { get; set; } = 0;
        [MetaProperty("shadowDepthY", BinPropertyType.Int32)]
        public int ShadowDepthY { get; set; } = 0;
        [MetaProperty("outlineSize", BinPropertyType.UInt32)]
        public uint OutlineSize { get; set; } = 0;
        [MetaProperty("fontSize", BinPropertyType.UInt32)]
        public uint FontSize { get; set; } = 10;
        [MetaProperty("screenHeight", BinPropertyType.UInt32)]
        public uint ScreenHeight { get; set; } = 1080;
    }
    [MetaClass("PerkSubStyleBonus")]
    public class PerkSubStyleBonus : IMetaClass
    {
        [MetaProperty("mPerk", BinPropertyType.ObjectLink)]
        public MetaObjectLink Perk { get; set; } = new(0);
        [MetaProperty("mStyleId", BinPropertyType.UInt32)]
        public uint StyleId { get; set; } = 0;
    }
    [MetaClass("RScript")]
    public interface RScript : IMetaClass
    {
    }
    [MetaClass("ConcatenateStringsBlock")]
    public class ConcatenateStringsBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("String1", BinPropertyType.Structure)]
        public IStringGet String1 { get; set; } = null;
        [MetaProperty("String2", BinPropertyType.Structure)]
        public IStringGet String2 { get; set; } = null;
        [MetaProperty("Result", BinPropertyType.Embedded)]
        public MetaEmbedded<StringTableSet> Result { get; set; } = new (new ());
    }
    [MetaClass("OptionItemSecondaryHotkeys1Column")]
    public class OptionItemSecondaryHotkeys1Column : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("Rows", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0x518d5fc0>> Rows { get; set; } = new();
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("Header", BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x55212361> Header { get; set; } = new (new ());
    }
    [MetaClass("TriggerOnMovementComplete")]
    public class TriggerOnMovementComplete : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; } = new();
        [MetaProperty("mDelay", BinPropertyType.Int32)]
        public int Delay { get; set; } = 0;
    }
    [MetaClass("BoolTableGet")]
    public class BoolTableGet : IBoolGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<bool> Default { get; set; } = new MetaOptional<bool>(default(bool), false);
    }
    [MetaClass("VfxFieldCollectionDefinitionData")]
    public class VfxFieldCollectionDefinitionData : IMetaClass
    {
        [MetaProperty("fieldAccelerationDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldAccelerationDefinitionData>> FieldAccelerationDefinitions { get; set; } = new();
        [MetaProperty("fieldOrbitalDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldOrbitalDefinitionData>> FieldOrbitalDefinitions { get; set; } = new();
        [MetaProperty("fieldDragDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldDragDefinitionData>> FieldDragDefinitions { get; set; } = new();
        [MetaProperty("fieldAttractionDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldAttractionDefinitionData>> FieldAttractionDefinitions { get; set; } = new();
        [MetaProperty("fieldNoiseDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldNoiseDefinitionData>> FieldNoiseDefinitions { get; set; } = new();
    }
    [MetaClass("IShaderDef")]
    public interface IShaderDef : IMetaClass
    {
        [MetaProperty("featureMask", BinPropertyType.UInt32)]
        uint FeatureMask { get; set; }
        [MetaProperty("staticSwitches", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ShaderStaticSwitch>> StaticSwitches { get; set; }
        [MetaProperty("parameters", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ShaderPhysicalParameter>> Parameters { get; set; }
        [MetaProperty("textures", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ShaderTexture>> Textures { get; set; }
        [MetaProperty(2617146753, BinPropertyType.UInt32)]
        uint m2617146753 { get; set; }
        [MetaProperty("featureDefines", BinPropertyType.Map)]
        Dictionary<string, string> FeatureDefines { get; set; }
    }
    [MetaClass("ItemSlotHasChargesCastRequirement")]
    public class ItemSlotHasChargesCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
    }
    [MetaClass("OptionsViewController")]
    public class OptionsViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("CloseButtonDefinition", BinPropertyType.Hash)]
        public MetaHash CloseButtonDefinition { get; set; } = new(0);
        [MetaProperty("TabButtonDefinition", BinPropertyType.Hash)]
        public MetaHash TabButtonDefinition { get; set; } = new(0);
        [MetaProperty(1514600716, BinPropertyType.Hash)]
        public MetaHash m1514600716 { get; set; } = new(0);
        [MetaProperty("Button2Definition", BinPropertyType.Hash)]
        public MetaHash Button2Definition { get; set; } = new(0);
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty("Button1Definition", BinPropertyType.Hash)]
        public MetaHash Button1Definition { get; set; } = new(0);
        [MetaProperty("Tabs", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Tabs { get; set; } = new();
        [MetaProperty("LastItemPadding", BinPropertyType.Hash)]
        public MetaHash LastItemPadding { get; set; } = new(0);
    }
    [MetaClass("IOptionTemplate")]
    public class IOptionTemplate : IMetaClass
    {
        [MetaProperty("boundsElement", BinPropertyType.Hash)]
        public MetaHash BoundsElement { get; set; } = new(0);
    }
    [MetaClass("StatStoneCategory")]
    public class StatStoneCategory : IMetaClass
    {
        [MetaProperty("gameIconMini", BinPropertyType.String)]
        public string GameIconMini { get; set; } = "";
        [MetaProperty("gameIconLit", BinPropertyType.String)]
        public string GameIconLit { get; set; } = "";
        [MetaProperty("gameIconFull", BinPropertyType.String)]
        public string GameIconFull { get; set; } = "";
        [MetaProperty("gameIconUnlit", BinPropertyType.String)]
        public string GameIconUnlit { get; set; } = "";
        [MetaProperty("CategoryColor", BinPropertyType.Color)]
        public Color CategoryColor { get; set; } = new Color(0f, 0f, 0f, 255f);
    }
    [MetaClass("GameCalculation")]
    public class GameCalculation : IGameCalculation
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        public IGameCalculationPart Multiplier { get; set; } = null;
        [MetaProperty(923208333, BinPropertyType.Byte)]
        public byte m923208333 { get; set; } = 8;
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        public byte m3419063832 { get; set; } = 8;
        [MetaProperty("tooltipOnly", BinPropertyType.Bool)]
        public bool TooltipOnly { get; set; } = false;
        [MetaProperty(3874405167, BinPropertyType.Byte)]
        public byte m3874405167 { get; set; } = 8;
        [MetaProperty("mFormulaParts", BinPropertyType.Container)]
        public MetaContainer<IGameCalculationPart> FormulaParts { get; set; } = new();
        [MetaProperty("mDisplayAsPercent", BinPropertyType.Bool)]
        public bool DisplayAsPercent { get; set; } = false;
        [MetaProperty("mPrecision", BinPropertyType.Int32)]
        public int Precision { get; set; } = 0;
    }
    [MetaClass("SpellRankUpRequirements")]
    public class SpellRankUpRequirements : IMetaClass
    {
        [MetaProperty("mRequirements", BinPropertyType.Container)]
        public MetaContainer<ISpellRankUpRequirement> Requirements { get; set; } = new();
    }
    [MetaClass("SetVarInTableBlock")]
    public class SetVarInTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; } = null;
        [MetaProperty("Dest", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> Dest { get; set; } = new (new ());
    }
    [MetaClass("TargeterDefinitionArc")]
    public class TargeterDefinitionArc : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; } = new (new ());
        [MetaProperty("isClockwiseArc", BinPropertyType.Bool)]
        public bool IsClockwiseArc { get; set; } = false;
        [MetaProperty("thicknessOffset", BinPropertyType.Float)]
        public float ThicknessOffset { get; set; } = 0f;
        [MetaProperty("constraintRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> ConstraintRange { get; set; } = new (new ());
        [MetaProperty("isConstrainedToRange", BinPropertyType.Bool)]
        public bool IsConstrainedToRange { get; set; } = false;
        [MetaProperty("textureArcOverrideName", BinPropertyType.String)]
        public string TextureArcOverrideName { get; set; } = "";
        [MetaProperty("overrideRadius", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideRadius { get; set; } = new (new ());
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; } = new (new ());
    }
    [MetaClass("HasSkinIDSpawnConditions")]
    public class HasSkinIDSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<IsSkinSpawnConditionData>> Conditions { get; set; } = new();
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; } = new (new ());
    }
    [MetaClass("ParticleWadFileDescriptor")]
    public class ParticleWadFileDescriptor : WadFileDescriptor
    {
    }
    [MetaClass("OptionTemplateHotkeysKey")]
    public class OptionTemplateHotkeysKey : IMetaClass
    {
        [MetaProperty("EventName", BinPropertyType.String)]
        public string EventName { get; set; } = "";
        [MetaProperty("EventNameTraKey", BinPropertyType.String)]
        public string EventNameTraKey { get; set; } = "";
        [MetaProperty("position", BinPropertyType.Hash)]
        public MetaHash Position { get; set; } = new(0);
    }
    [MetaClass("GameplayConfig")]
    public class GameplayConfig : IMetaClass
    {
        [MetaProperty("mCritBonusArmorPenPercent", BinPropertyType.Float)]
        public float CritBonusArmorPenPercent { get; set; } = 0f;
        [MetaProperty("mBasicAttackCalculation", BinPropertyType.Structure)]
        public IGameCalculation BasicAttackCalculation { get; set; } = null;
        [MetaProperty("mLegacySummonerSpells", BinPropertyType.Container)]
        public MetaContainer<MetaHash> LegacySummonerSpells { get; set; } = new();
        [MetaProperty("mLethalityRatioFromAttacker", BinPropertyType.Float)]
        public float LethalityRatioFromAttacker { get; set; } = 0f;
        [MetaProperty("mLethalityPercentGivenAtLevel0", BinPropertyType.Float)]
        public float LethalityPercentGivenAtLevel0 { get; set; } = 0f;
        [MetaProperty("mLethalityScalesCapsAtLevel", BinPropertyType.Int32)]
        public int LethalityScalesCapsAtLevel { get; set; } = 0;
        [MetaProperty("mMinionAutoLeeway", BinPropertyType.Float)]
        public float MinionAutoLeeway { get; set; } = 0f;
        [MetaProperty("ItemsRolesPerRow", BinPropertyType.Hash)]
        public MetaHash ItemsRolesPerRow { get; set; } = new(0);
        [MetaProperty("mCritTotalArmorPenPercent", BinPropertyType.Float)]
        public float CritTotalArmorPenPercent { get; set; } = 0f;
        [MetaProperty("mCCScoreMultipliers", BinPropertyType.Embedded)]
        public MetaEmbedded<CCScoreMultipliers> CCScoreMultipliers { get; set; } = new (new ());
        [MetaProperty("mCritGlobalDamageMultiplier", BinPropertyType.Float)]
        public float CritGlobalDamageMultiplier { get; set; } = 1f;
        [MetaProperty("mSpellPostponeTimeoutSec", BinPropertyType.Float)]
        public float SpellPostponeTimeoutSec { get; set; } = 2f;
        [MetaProperty("mMinionDeathDelay", BinPropertyType.Float)]
        public float MinionDeathDelay { get; set; } = 0f;
        [MetaProperty("mLethalityScalesToLevel", BinPropertyType.Int32)]
        public int LethalityScalesToLevel { get; set; } = 0;
        [MetaProperty("mAutoAttackMinPreCastLockoutDeltaTimeSec", BinPropertyType.Float)]
        public float AutoAttackMinPreCastLockoutDeltaTimeSec { get; set; } = 0f;
        [MetaProperty("mMinionAAHelperLimit", BinPropertyType.Float)]
        public float MinionAAHelperLimit { get; set; } = 0f;
        [MetaProperty("mItemSellQueueTime", BinPropertyType.Float)]
        public float ItemSellQueueTime { get; set; } = 0f;
        [MetaProperty("mAdaptiveForceAttackDamageScale", BinPropertyType.Float)]
        public float AdaptiveForceAttackDamageScale { get; set; } = 0f;
        [MetaProperty(2789737202, BinPropertyType.Hash)]
        public MetaHash m2789737202 { get; set; } = new(0);
        [MetaProperty("mSummonerSpells", BinPropertyType.Container)]
        public MetaContainer<MetaHash> SummonerSpells { get; set; } = new();
        [MetaProperty("mAdaptiveForceAbilityPowerScale", BinPropertyType.Float)]
        public float AdaptiveForceAbilityPowerScale { get; set; } = 0f;
        [MetaProperty("AbilityHasteMax", BinPropertyType.Float)]
        public float AbilityHasteMax { get; set; } = 1000f;
        [MetaProperty("mAutoAttackMinPostCastLockoutDeltaTimeSec", BinPropertyType.Float)]
        public float AutoAttackMinPostCastLockoutDeltaTimeSec { get; set; } = 0f;
        [MetaProperty("mPerSlotCDRIsAdditive", BinPropertyType.Bool)]
        public bool PerSlotCDRIsAdditive { get; set; } = false;
        [MetaProperty("mLethalityRatioFromTarget", BinPropertyType.Float)]
        public float LethalityRatioFromTarget { get; set; } = 1f;
    }
    [MetaClass("SubmeshVisibilityEventData")]
    public class SubmeshVisibilityEventData : BaseEventData
    {
        [MetaProperty("mShowSubmeshList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ShowSubmeshList { get; set; } = new();
        [MetaProperty("mHideSubmeshList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> HideSubmeshList { get; set; } = new();
    }
    [MetaClass("ScriptDataObjectList")]
    public class ScriptDataObjectList : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScriptDataObjects", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ScriptDataObjects { get; set; } = new();
    }
    [MetaClass("BaseBlendData")]
    public interface BaseBlendData : IMetaClass
    {
    }
    [MetaClass("CSSSheet")]
    public class CSSSheet : IMetaClass
    {
        [MetaProperty("iconTexture", BinPropertyType.String)]
        public string IconTexture { get; set; } = "";
        [MetaProperty("styles", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<CSSStyle>> Styles { get; set; } = new();
        [MetaProperty("icons", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<CSSIcon>> Icons { get; set; } = new();
    }
    [MetaClass("CustomTargeterDefinitions")]
    public class CustomTargeterDefinitions : IMetaClass
    {
        [MetaProperty("mTargeterDefinitions", BinPropertyType.Container)]
        public MetaContainer<TargeterDefinition> TargeterDefinitions { get; set; } = new();
    }
    [MetaClass("IsEnemyDynamicMaterialBoolDriver")]
    public class IsEnemyDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
    }
    [MetaClass("HasSpellRankSpawnConditionData")]
    public class HasSpellRankSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; } = new();
        [MetaProperty("mSpellSlot", BinPropertyType.UInt32)]
        public uint SpellSlot { get; set; } = 0;
        [MetaProperty("mSpellLevel", BinPropertyType.Int32)]
        public int SpellLevel { get; set; } = 1;
    }
    [MetaClass("FixedDistanceIgnoringTerrain")]
    public class FixedDistanceIgnoringTerrain : MissileBehaviorSpec
    {
        [MetaProperty("scanWidthOverride", BinPropertyType.Optional)]
        public MetaOptional<float> ScanWidthOverride { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mMaximumDistance", BinPropertyType.Float)]
        public float MaximumDistance { get; set; } = 0f;
        [MetaProperty("mMinimumGapBetweenTerrainWalls", BinPropertyType.Float)]
        public float MinimumGapBetweenTerrainWalls { get; set; } = 0f;
        [MetaProperty("mTargeterDefinition", BinPropertyType.Structure)]
        public TargeterDefinitionSkipTerrain TargeterDefinition { get; set; } = null;
        [MetaProperty("mMaximumTerrainWallsToSkip", BinPropertyType.Optional)]
        public MetaOptional<uint> MaximumTerrainWallsToSkip { get; set; } = new MetaOptional<uint>(default(uint), false);
    }
    [MetaClass("MapCharacterList")]
    public class MapCharacterList : IMetaClass
    {
        [MetaProperty("characters", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> Characters { get; set; } = new();
    }
    [MetaClass("LoadScreenTipConfiguration")]
    public class LoadScreenTipConfiguration : IMetaClass
    {
        [MetaProperty("mShowInCustomGames", BinPropertyType.Bool)]
        public bool ShowInCustomGames { get; set; } = false;
        [MetaProperty("mShowPBITipsOnLoadingScreen", BinPropertyType.Bool)]
        public bool ShowPBITipsOnLoadingScreen { get; set; } = false;
        [MetaProperty("mDurationInGame", BinPropertyType.Float)]
        public float DurationInGame { get; set; } = 0f;
        [MetaProperty("mPBITipDurationOnLoadingScreen", BinPropertyType.Float)]
        public float PBITipDurationOnLoadingScreen { get; set; } = 0f;
    }
    [MetaClass("EffectCooldownElementData")]
    public class EffectCooldownElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mEffectColor0", BinPropertyType.Color)]
        public Color EffectColor0 { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("mEffectColor1", BinPropertyType.Color)]
        public Color EffectColor1 { get; set; } = new Color(255f, 255f, 255f, 255f);
    }
    [MetaClass("GameModeConstantStringVector")]
    public class GameModeConstantStringVector : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Container)]
        public MetaContainer<string> Value { get; set; } = new();
    }
    [MetaClass("FadeByMouseRangeBehavior")]
    public class FadeByMouseRangeBehavior : ITargeterFadeBehavior
    {
        [MetaProperty("mStartAlpha", BinPropertyType.Float)]
        public float StartAlpha { get; set; } = 1f;
        [MetaProperty(1696085056, BinPropertyType.Float)]
        public float m1696085056 { get; set; } = 0f;
        [MetaProperty(1990666961, BinPropertyType.Float)]
        public float m1990666961 { get; set; } = 0f;
        [MetaProperty("mEndAlpha", BinPropertyType.Float)]
        public float EndAlpha { get; set; } = 1f;
    }
    [MetaClass("ScriptGlobalProperties")]
    public class ScriptGlobalProperties : IMetaClass
    {
        [MetaProperty("IsDeathRecapSource", BinPropertyType.Bool)]
        public bool IsDeathRecapSource { get; set; } = false;
        [MetaProperty("ChannelDuration", BinPropertyType.Float)]
        public float ChannelDuration { get; set; } = 0f;
        [MetaProperty("NonDispellable", BinPropertyType.Bool)]
        public bool NonDispellable { get; set; } = false;
        [MetaProperty("displayName", BinPropertyType.String)]
        public string DisplayName { get; set; } = "";
        [MetaProperty("CastTime", BinPropertyType.Float)]
        public float CastTime { get; set; } = 0f;
        [MetaProperty("AutoBuffActivateEffects", BinPropertyType.Container)]
        public MetaContainer<string> AutoBuffActivateEffects { get; set; } = new();
        [MetaProperty("SpellToggleSlot", BinPropertyType.UInt32)]
        public uint SpellToggleSlot { get; set; } = 4294967295;
        [MetaProperty("buffName", BinPropertyType.String)]
        public string BuffName { get; set; } = "";
        [MetaProperty("DeathEventType", BinPropertyType.UInt32)]
        public uint DeathEventType { get; set; } = 0;
        [MetaProperty("OnPreDamagePriority", BinPropertyType.Int32)]
        public int OnPreDamagePriority { get; set; } = -1;
        [MetaProperty("buffTextureName", BinPropertyType.String)]
        public string BuffTextureName { get; set; } = "";
        [MetaProperty("PersistsThroughDeath", BinPropertyType.Bool)]
        public bool PersistsThroughDeath { get; set; } = false;
        [MetaProperty("PopupMessages", BinPropertyType.Container)]
        public MetaContainer<string> PopupMessages { get; set; } = new();
        [MetaProperty("SpellFXOverrideSkins", BinPropertyType.Container)]
        public MetaContainer<string> SpellFXOverrideSkins { get; set; } = new();
        [MetaProperty("IsItemToggled", BinPropertyType.Bool)]
        public bool IsItemToggled { get; set; } = false;
        [MetaProperty("SpellVOOverrideSkins", BinPropertyType.Container)]
        public MetaContainer<string> SpellVOOverrideSkins { get; set; } = new();
        [MetaProperty("AutoBuffActivateAttachBoneNames", BinPropertyType.Container)]
        public MetaContainer<string> AutoBuffActivateAttachBoneNames { get; set; } = new();
    }
    [MetaClass("GlobalResourceResolver")]
    public class GlobalResourceResolver : BaseResourceResolver
    {
    }
    [MetaClass("TargetHasUnitTagFilter")]
    public class TargetHasUnitTagFilter : IStatStoneLogicDriver
    {
        [MetaProperty("UnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> UnitTags { get; set; } = new (new ());
    }
    [MetaClass("BasePerk")]
    public interface BasePerk : IMetaClass
    {
        [MetaProperty("mPingTextLocalizationKey", BinPropertyType.String)]
        string PingTextLocalizationKey { get; set; }
        [MetaProperty("mDisplayNameLocalizationKey", BinPropertyType.String)]
        string DisplayNameLocalizationKey { get; set; }
        [MetaProperty("mPerkId", BinPropertyType.UInt32)]
        uint PerkId { get; set; }
        [MetaProperty("mTooltipNameLocalizationKey", BinPropertyType.String)]
        string TooltipNameLocalizationKey { get; set; }
        [MetaProperty("mScript", BinPropertyType.Structure)]
        PerkScript Script { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        bool Enabled { get; set; }
        [MetaProperty("mPerkName", BinPropertyType.String)]
        string PerkName { get; set; }
        [MetaProperty("mIconTextureName", BinPropertyType.String)]
        string IconTextureName { get; set; }
        [MetaProperty("mShortDescLocalizationKey", BinPropertyType.String)]
        string ShortDescLocalizationKey { get; set; }
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        ResourceResolver VFXResourceResolver { get; set; }
        [MetaProperty("mLongDescLocalizationKey", BinPropertyType.String)]
        string LongDescLocalizationKey { get; set; }
        [MetaProperty("mStackable", BinPropertyType.Bool)]
        bool Stackable { get; set; }
        [MetaProperty("mDisplayStatLocalizationKey", BinPropertyType.String)]
        string DisplayStatLocalizationKey { get; set; }
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        MetaContainer<string> Characters { get; set; }
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        MetaContainer<PerkBuff> Buffs { get; set; }
        [MetaProperty("mEndOfGameStatDescriptions", BinPropertyType.Container)]
        MetaContainer<string> EndOfGameStatDescriptions { get; set; }
    }
    [MetaClass("BaseElementData")]
    public interface BaseElementData : Class0x231dd1a2
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        bool m629911194 { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        ushort RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        ushort RectSourceResolutionHeight { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        uint Draggable { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        bool Enabled { get; set; }
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        bool StickyDrag { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        uint Layer { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        bool KeepMaxScale { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        Vector4 Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        bool UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        bool Fullscreen { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        bool NoPixelSnappingY { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        bool NoPixelSnappingX { get; set; }
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        AnchorBase Anchors { get; set; }
    }
    [MetaClass("Location")]
    public class Location : TargetingTypeData
    {
    }
    [MetaClass("VfxEmissionSurfaceData")]
    public class VfxEmissionSurfaceData : IMetaClass
    {
        [MetaProperty("skeletonName", BinPropertyType.String)]
        public string SkeletonName { get; set; } = "";
        [MetaProperty("useAvatarPose", BinPropertyType.Bool)]
        public bool UseAvatarPose { get; set; } = false;
        [MetaProperty("animationName", BinPropertyType.String)]
        public string AnimationName { get; set; } = "";
        [MetaProperty("useSurfaceNormalForBirthPhysics", BinPropertyType.Bool)]
        public bool UseSurfaceNormalForBirthPhysics { get; set; } = true;
        [MetaProperty("maxJointWeights", BinPropertyType.UInt16)]
        public ushort MaxJointWeights { get; set; } = 4;
        [MetaProperty("meshScale", BinPropertyType.Float)]
        public float MeshScale { get; set; } = 1f;
        [MetaProperty("meshName", BinPropertyType.String)]
        public string MeshName { get; set; } = "";
    }
    [MetaClass("MissileSpecification")]
    public class MissileSpecification : IMetaClass
    {
        [MetaProperty("verticalFacing", BinPropertyType.Structure)]
        public VerticalFacingType VerticalFacing { get; set; } = null;
        [MetaProperty("missileGroupSpawners", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MissileGroupSpawnerSpec>> MissileGroupSpawners { get; set; } = new();
        [MetaProperty("heightSolver", BinPropertyType.Structure)]
        public HeightSolverType HeightSolver { get; set; } = null;
        [MetaProperty("visibilityComponent", BinPropertyType.Structure)]
        public MissileVisibilitySpec VisibilityComponent { get; set; } = null;
        [MetaProperty("behaviors", BinPropertyType.Container)]
        public MetaContainer<MissileBehaviorSpec> Behaviors { get; set; } = new();
        [MetaProperty("mMissileWidth", BinPropertyType.Float)]
        public float MissileWidth { get; set; } = 0f;
        [MetaProperty("movementComponent", BinPropertyType.Structure)]
        public MissileMovementSpec MovementComponent { get; set; } = null;
    }
    [MetaClass("ContextualConditionMapRegionName")]
    public class ContextualConditionMapRegionName : IContextualCondition
    {
        [MetaProperty("mRegionType", BinPropertyType.Byte)]
        public byte RegionType { get; set; } = 0;
        [MetaProperty("mRegionName", BinPropertyType.String)]
        public string RegionName { get; set; } = "";
    }
    [MetaClass("ItemDataAvailability")]
    public class ItemDataAvailability : IMetaClass
    {
        [MetaProperty("mForceLoad", BinPropertyType.Bool)]
        public bool ForceLoad { get; set; } = false;
        [MetaProperty("mInStore", BinPropertyType.Bool)]
        public bool InStore { get; set; } = false;
        [MetaProperty("mHidefromAll", BinPropertyType.Bool)]
        public bool HidefromAll { get; set; } = false;
    }
    [MetaClass("GameModeConstants")]
    public class GameModeConstants : IMetaClass
    {
        [MetaProperty("mGroups", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<GameModeConstantsGroup>> Groups { get; set; } = new();
    }
    [MetaClass(3233830641)]
    public class Class0xc0c056f1 : GenericMapPlaceable
    {
        [MetaProperty("PropName", BinPropertyType.String)]
        public string PropName { get; set; } = "";
        [MetaProperty("eyeCandy", BinPropertyType.Bool)]
        public bool EyeCandy { get; set; } = false;
        [MetaProperty("IdleAnimationName", BinPropertyType.String)]
        public string IdleAnimationName { get; set; } = "";
        [MetaProperty("SkinID", BinPropertyType.UInt32)]
        public uint SkinID { get; set; } = 0;
        [MetaProperty("PlayIdleAnimation", BinPropertyType.Bool)]
        public bool PlayIdleAnimation { get; set; } = false;
        [MetaProperty("quality", BinPropertyType.Int32)]
        public int Quality { get; set; } = 31;
        [MetaProperty("isClickable", BinPropertyType.Bool)]
        public bool IsClickable { get; set; } = false;
    }
    [MetaClass("DefaultStatModPerkSet")]
    public class DefaultStatModPerkSet : IMetaClass
    {
        [MetaProperty("mPerks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Perks { get; set; } = new();
        [MetaProperty("mStyleId", BinPropertyType.UInt32)]
        public uint StyleId { get; set; } = 0;
    }
    [MetaClass("LockRootOrientationEventData")]
    public class LockRootOrientationEventData : BaseEventData
    {
    }
    [MetaClass("TFTTraitSetData")]
    public class TFTTraitSetData : IMetaClass
    {
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string BuffName { get; set; } = "";
        [MetaProperty("mTeamToBuff", BinPropertyType.Byte)]
        public byte TeamToBuff { get; set; } = 0;
        [MetaProperty("mActivatedBuffName", BinPropertyType.String)]
        public string ActivatedBuffName { get; set; } = "";
        [MetaProperty("mMaxUnits", BinPropertyType.Optional)]
        public MetaOptional<uint> MaxUnits { get; set; } = new MetaOptional<uint>(default(uint), false);
        [MetaProperty("mMinUnits", BinPropertyType.UInt32)]
        public uint MinUnits { get; set; } = 0;
        [MetaProperty("mTargetStrategy", BinPropertyType.Byte)]
        public byte TargetStrategy { get; set; } = 0;
        [MetaProperty(2831490480, BinPropertyType.Optional)]
        public MetaOptional<uint> m2831490480 { get; set; } = new MetaOptional<uint>(default(uint), false);
        [MetaProperty("effectAmounts", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftEffectAmount>> EffectAmounts { get; set; } = new();
        [MetaProperty("mStyle", BinPropertyType.Byte)]
        public byte Style { get; set; } = 1;
    }
    [MetaClass("EvolutionDescription")]
    public class EvolutionDescription : IMetaClass
    {
        [MetaProperty("mTitle", BinPropertyType.String)]
        public string Title { get; set; } = "";
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mIconNames", BinPropertyType.Container)]
        public MetaContainer<string> IconNames { get; set; } = new();
        [MetaProperty("mTooltips", BinPropertyType.Container)]
        public MetaContainer<string> Tooltips { get; set; } = new();
    }
    [MetaClass("IIntGet")]
    public interface IIntGet : IScriptValueGet
    {
    }
    [MetaClass("KillingSpreeFilter")]
    public class KillingSpreeFilter : IStatStoneLogicDriver
    {
        [MetaProperty("KillingSpreeCount", BinPropertyType.Int32)]
        public int KillingSpreeCount { get; set; } = 0;
    }
    [MetaClass("EffectAmmoElementData")]
    public class EffectAmmoElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mEffectColor0", BinPropertyType.Color)]
        public Color EffectColor0 { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("mEffectColor1", BinPropertyType.Color)]
        public Color EffectColor1 { get; set; } = new Color(255f, 255f, 255f, 255f);
    }
    [MetaClass("VfxTrailDefinitionData")]
    public class VfxTrailDefinitionData : IMetaClass
    {
        [MetaProperty("mMaxAddedPerFrame", BinPropertyType.Int32)]
        public int MaxAddedPerFrame { get; set; } = 0;
        [MetaProperty("mBirthTilingSize", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthTilingSize { get; set; } = new (new ());
        [MetaProperty("mMode", BinPropertyType.Byte)]
        public byte Mode { get; set; } = 0;
        [MetaProperty("mSmoothingMode", BinPropertyType.Byte)]
        public byte SmoothingMode { get; set; } = 0;
        [MetaProperty("mCutoff", BinPropertyType.Float)]
        public float Cutoff { get; set; } = 0f;
    }
    [MetaClass("OptionItemColumns")]
    public class OptionItemColumns : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("ItemsEither", BinPropertyType.Container)]
        public MetaContainer<IOptionItem> ItemsEither { get; set; } = new();
        [MetaProperty("itemsRight", BinPropertyType.Container)]
        public MetaContainer<IOptionItem> ItemsRight { get; set; } = new();
        [MetaProperty("itemsLeft", BinPropertyType.Container)]
        public MetaContainer<IOptionItem> ItemsLeft { get; set; } = new();
    }
    [MetaClass("HudMenuTransitionData")]
    public class HudMenuTransitionData : IMetaClass
    {
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float TransitionTime { get; set; } = 0.10000000149011612f;
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        public byte EasingType { get; set; } = 0;
        [MetaProperty("minAlpha", BinPropertyType.Byte)]
        public byte MinAlpha { get; set; } = 0;
        [MetaProperty("maxAlpha", BinPropertyType.Byte)]
        public byte MaxAlpha { get; set; } = 255;
    }
    [MetaClass("IStatStoneLogicDriver")]
    public interface IStatStoneLogicDriver : IMetaClass
    {
    }
    [MetaClass("Destroy")]
    public class Destroy : MissileTriggeredActionSpec
    {
    }
    [MetaClass("HudTeamScoreMeterProperties")]
    public class HudTeamScoreMeterProperties : IMetaClass
    {
        [MetaProperty(1615112080, BinPropertyType.String)]
        public string m1615112080 { get; set; } = "";
        [MetaProperty(2256949180, BinPropertyType.String)]
        public string m2256949180 { get; set; } = "";
        [MetaProperty("mTeamScoreMeterType", BinPropertyType.Byte)]
        public byte TeamScoreMeterType { get; set; } = 0;
        [MetaProperty("mShowScoreText", BinPropertyType.Bool)]
        public bool ShowScoreText { get; set; } = false;
    }
    [MetaClass("HudFightRecapUIData")]
    public class HudFightRecapUIData : IMetaClass
    {
        [MetaProperty("mUnknownDamageIconTextureName", BinPropertyType.String)]
        public string UnknownDamageIconTextureName { get; set; } = "";
        [MetaProperty("mItemDamageIconTextureName", BinPropertyType.String)]
        public string ItemDamageIconTextureName { get; set; } = "";
        [MetaProperty("mBasicAttackIconTextureName", BinPropertyType.String)]
        public string BasicAttackIconTextureName { get; set; } = "";
        [MetaProperty("mRuneDamageIconTextureName", BinPropertyType.String)]
        public string RuneDamageIconTextureName { get; set; } = "";
    }
    [MetaClass(12944262)]
    public class Class0xc58386 : IOptionItemFilter
    {
        [MetaProperty(3346810982, BinPropertyType.Bool)]
        public bool m3346810982 { get; set; } = false;
    }
    [MetaClass("AddGoldCheat")]
    public class AddGoldCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
        [MetaProperty("mGoldAmount", BinPropertyType.Float)]
        public float GoldAmount { get; set; } = 0f;
    }
    [MetaClass("IScriptCondition")]
    public interface IScriptCondition : IMetaClass
    {
    }
    [MetaClass("ContextualConditionIsAlly")]
    public class ContextualConditionIsAlly : ICharacterSubcondition
    {
        [MetaProperty("mIsAlly", BinPropertyType.Bool)]
        public bool IsAlly { get; set; } = false;
    }
    [MetaClass("ChampionItemRecommendations")]
    public class ChampionItemRecommendations : IMetaClass
    {
        [MetaProperty(763353121, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m763353121 { get; set; } = new();
        [MetaProperty("mContextListLink", BinPropertyType.Hash)]
        public MetaHash ContextListLink { get; set; } = new(0);
        [MetaProperty(2909611432, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemCareyOverrideStartingItemSet>> m2909611432 { get; set; } = new();
        [MetaProperty(3426090776, BinPropertyType.Hash)]
        public MetaHash m3426090776 { get; set; } = new(0);
    }
    [MetaClass("RegaliaPrestigeCrestList")]
    public class RegaliaPrestigeCrestList : IMetaClass
    {
        [MetaProperty("PrestigeCrests", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> PrestigeCrests { get; set; } = new();
    }
    [MetaClass("LoLFeatureToggles")]
    public class LoLFeatureToggles : IMetaClass
    {
        [MetaProperty("PromoController", BinPropertyType.Bool)]
        public bool PromoController { get; set; } = false;
        [MetaProperty("useNewAttackSpeed", BinPropertyType.Bool)]
        public bool UseNewAttackSpeed { get; set; } = false;
        [MetaProperty(789484890, BinPropertyType.Bool)]
        public bool m789484890 { get; set; } = false;
        [MetaProperty("queuedOrdersTriggerPreIssueOrder", BinPropertyType.Bool)]
        public bool QueuedOrdersTriggerPreIssueOrder { get; set; } = false;
        [MetaProperty(1044127845, BinPropertyType.Bool)]
        public bool m1044127845 { get; set; } = false;
        [MetaProperty("closeOnEndGameAfterDelay", BinPropertyType.Bool)]
        public bool CloseOnEndGameAfterDelay { get; set; } = false;
        [MetaProperty(1507864935, BinPropertyType.Bool)]
        public bool m1507864935 { get; set; } = false;
        [MetaProperty(1758021693, BinPropertyType.Bool)]
        public bool m1758021693 { get; set; } = true;
        [MetaProperty(110855633, BinPropertyType.Bool)]
        public bool m110855633 { get; set; } = false;
        [MetaProperty(1838898265, BinPropertyType.Bool)]
        public bool m1838898265 { get; set; } = false;
        [MetaProperty(1841226935, BinPropertyType.Bool)]
        public bool m1841226935 { get; set; } = true;
        [MetaProperty(1863033520, BinPropertyType.Bool)]
        public bool m1863033520 { get; set; } = false;
        [MetaProperty("EnableCustomPlayerScoreColoring", BinPropertyType.Bool)]
        public bool EnableCustomPlayerScoreColoring { get; set; } = false;
        [MetaProperty(1988362409, BinPropertyType.Bool)]
        public bool m1988362409 { get; set; } = false;
        [MetaProperty(124682395, BinPropertyType.Bool)]
        public bool m124682395 { get; set; } = true;
        [MetaProperty(2189008768, BinPropertyType.Bool)]
        public bool m2189008768 { get; set; } = false;
        [MetaProperty(2231270607, BinPropertyType.Bool)]
        public bool m2231270607 { get; set; } = false;
        [MetaProperty(2242841781, BinPropertyType.Bool)]
        public bool m2242841781 { get; set; } = false;
        [MetaProperty(2343239738, BinPropertyType.Bool)]
        public bool m2343239738 { get; set; } = false;
        [MetaProperty(2362683897, BinPropertyType.Bool)]
        public bool m2362683897 { get; set; } = true;
        [MetaProperty(2430635207, BinPropertyType.Bool)]
        public bool m2430635207 { get; set; } = true;
        [MetaProperty(2451076183, BinPropertyType.Bool)]
        public bool m2451076183 { get; set; } = true;
        [MetaProperty(2607877110, BinPropertyType.Bool)]
        public bool m2607877110 { get; set; } = true;
        [MetaProperty(2633783226, BinPropertyType.Bool)]
        public bool m2633783226 { get; set; } = false;
        [MetaProperty(2727067100, BinPropertyType.Bool)]
        public bool m2727067100 { get; set; } = false;
        [MetaProperty(2743208720, BinPropertyType.Bool)]
        public bool m2743208720 { get; set; } = true;
        [MetaProperty(2785021207, BinPropertyType.Bool)]
        public bool m2785021207 { get; set; } = true;
        [MetaProperty(2798407076, BinPropertyType.Bool)]
        public bool m2798407076 { get; set; } = false;
        [MetaProperty("cooldownSpellQueueing", BinPropertyType.Bool)]
        public bool CooldownSpellQueueing { get; set; } = false;
        [MetaProperty(3260700411, BinPropertyType.Bool)]
        public bool m3260700411 { get; set; } = false;
        [MetaProperty("NewSpellScript", BinPropertyType.Bool)]
        public bool NewSpellScript { get; set; } = false;
        [MetaProperty(3346651682, BinPropertyType.Bool)]
        public bool m3346651682 { get; set; } = true;
        [MetaProperty(3428728717, BinPropertyType.Bool)]
        public bool m3428728717 { get; set; } = false;
        [MetaProperty(3442447761, BinPropertyType.Bool)]
        public bool m3442447761 { get; set; } = true;
        [MetaProperty(3543285436, BinPropertyType.Bool)]
        public bool m3543285436 { get; set; } = false;
        [MetaProperty("abilityResetUI", BinPropertyType.Bool)]
        public bool AbilityResetUI { get; set; } = false;
        [MetaProperty(3981460857, BinPropertyType.Bool)]
        public bool m3981460857 { get; set; } = true;
        [MetaProperty(4024796347, BinPropertyType.Bool)]
        public bool m4024796347 { get; set; } = false;
        [MetaProperty(4044511864, BinPropertyType.Bool)]
        public bool m4044511864 { get; set; } = true;
        [MetaProperty("ItemUndo", BinPropertyType.Bool)]
        public bool ItemUndo { get; set; } = true;
        [MetaProperty(4065351745, BinPropertyType.Bool)]
        public bool m4065351745 { get; set; } = false;
        [MetaProperty("UseNewFireBBEvents", BinPropertyType.Bool)]
        public bool UseNewFireBBEvents { get; set; } = true;
    }
    [MetaClass("MissileTriggerSpec")]
    public interface MissileTriggerSpec : MissileBehaviorSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
    }
    [MetaClass("ContextualConditionCharacterSkinID")]
    public class ContextualConditionCharacterSkinID : ICharacterSubcondition
    {
        [MetaProperty("mSkinIDs", BinPropertyType.Container)]
        public MetaContainer<int> SkinIDs { get; set; } = new();
    }
    [MetaClass("OneTrueMaterialDriver")]
    public class OneTrueMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialBoolDriver> Drivers { get; set; } = new();
    }
    [MetaClass("QuestUITunables")]
    public class QuestUITunables : IMetaClass
    {
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; } = new (new ());
    }
    [MetaClass("OptionTemplateSecondaryHotkeys1Column")]
    public class OptionTemplateSecondaryHotkeys1Column : IOptionTemplate
    {
        [MetaProperty(1415010472, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x354988a8> m1415010472 { get; set; } = new (new ());
        [MetaProperty(1515828214, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xceb70e5a> m1515828214 { get; set; } = new (new ());
        [MetaProperty(1532605833, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xceb70e5a> m1532605833 { get; set; } = new (new ());
        [MetaProperty(4247899083, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xceb70e5a> m4247899083 { get; set; } = new (new ());
    }
    [MetaClass("ParametricPairData")]
    public class ParametricPairData : IMetaClass
    {
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float Value { get; set; } = 0f;
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash ClipName { get; set; } = new(0);
    }
    [MetaClass("AssistCountFilter")]
    public class AssistCountFilter : IStatStoneLogicDriver
    {
        [MetaProperty("assistCount", BinPropertyType.Byte)]
        public byte AssistCount { get; set; } = 0;
    }
    [MetaClass("VFXSpawnConditionData")]
    public interface VFXSpawnConditionData : IMetaClass
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
    }
    [MetaClass("RatioConversion")]
    public class RatioConversion : IMetaClass
    {
        [MetaProperty("mResultingStatType", BinPropertyType.Byte)]
        public byte ResultingStatType { get; set; } = 0;
        [MetaProperty(2452082244, BinPropertyType.Float)]
        public float m2452082244 { get; set; } = 1f;
        [MetaProperty("mSourceStatType", BinPropertyType.Byte)]
        public byte SourceStatType { get; set; } = 0;
        [MetaProperty("mSourceStatOutput", BinPropertyType.Byte)]
        public byte SourceStatOutput { get; set; } = 0;
        [MetaProperty("mResultingStatOutput", BinPropertyType.Byte)]
        public byte ResultingStatOutput { get; set; } = 0;
    }
    [MetaClass("OverrideAutoAttackCastTimeData")]
    public class OverrideAutoAttackCastTimeData : IMetaClass
    {
        [MetaProperty("mOverrideAutoattackCastTimeCalculation", BinPropertyType.Structure)]
        public IGameCalculation OverrideAutoattackCastTimeCalculation { get; set; } = null;
    }
    [MetaClass("IContextualCondition")]
    public interface IContextualCondition : IMetaClass
    {
    }
    [MetaClass("FixedTimeSplineMovement")]
    public class FixedTimeSplineMovement : GenericSplineMovementSpec
    {
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        public bool UseMissilePositionAsOrigin { get; set; } = true;
        [MetaProperty("mSplineInfo", BinPropertyType.Structure)]
        public ISplineInfo SplineInfo { get; set; } = null;
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mTravelTime", BinPropertyType.Float)]
        public float TravelTime { get; set; } = 0f;
    }
    [MetaClass("ContextualConditionCharacterInRangeForSyncedAnimation")]
    public class ContextualConditionCharacterInRangeForSyncedAnimation : ICharacterSubcondition
    {
    }
    [MetaClass("OptionTemplateBorder")]
    public class OptionTemplateBorder : IOptionTemplate
    {
        [MetaProperty("border", BinPropertyType.Hash)]
        public MetaHash Border { get; set; } = new(0);
    }
    [MetaClass("VertexAnimationRigPoseModifierData")]
    public class VertexAnimationRigPoseModifierData : BaseRigPoseModifierData
    {
        [MetaProperty("mMass", BinPropertyType.Float)]
        public float Mass { get; set; } = 1f;
        [MetaProperty("mDamping", BinPropertyType.Float)]
        public float Damping { get; set; } = 5f;
        [MetaProperty("mStiffness", BinPropertyType.Float)]
        public float Stiffness { get; set; } = 45f;
        [MetaProperty("mMaxSpeed", BinPropertyType.Float)]
        public float MaxSpeed { get; set; } = 350f;
    }
    [MetaClass("IOptionItemFilter")]
    public interface IOptionItemFilter : IMetaClass
    {
    }
    [MetaClass("TftChangeDamageSkinCheat")]
    public class TftChangeDamageSkinCheat : Cheat
    {
    }
    [MetaClass("MapAction")]
    public interface MapAction : IMetaClass
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        float StartTime { get; set; }
    }
    [MetaClass("HudLoadingScreenWidgetPing")]
    public class HudLoadingScreenWidgetPing : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string SceneName { get; set; } = "";
        [MetaProperty("mPingThresholdGreen", BinPropertyType.UInt32)]
        public uint PingThresholdGreen { get; set; } = 0;
        [MetaProperty("mPingThresholdOrange", BinPropertyType.UInt32)]
        public uint PingThresholdOrange { get; set; } = 0;
        [MetaProperty("mPingThresholdYellow", BinPropertyType.UInt32)]
        public uint PingThresholdYellow { get; set; } = 0;
        [MetaProperty("mPingThresholdRed", BinPropertyType.UInt32)]
        public uint PingThresholdRed { get; set; } = 0;
        [MetaProperty("mDebugPing", BinPropertyType.UInt32)]
        public uint DebugPing { get; set; } = 0;
    }
    [MetaClass("DestroyCustomTableBlock")]
    public class DestroyCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableSet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("ContextualConditionCharacterHasCAC")]
    public class ContextualConditionCharacterHasCAC : ICharacterSubcondition
    {
        [MetaProperty("mCacs", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Cacs { get; set; } = new();
    }
    [MetaClass("InsertIntoCustomTableBlock")]
    public class InsertIntoCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; } = null;
        [MetaProperty("OutIndex", BinPropertyType.Embedded)]
        public MetaEmbedded<IntTableSet> OutIndex { get; set; } = new (new ());
        [MetaProperty("Index", BinPropertyType.Structure)]
        public IIntGet Index { get; set; } = null;
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass("DragonUITunables")]
    public class DragonUITunables : IMetaClass
    {
        [MetaProperty("mDragonBaseNames", BinPropertyType.Container)]
        public MetaContainer<string> DragonBaseNames { get; set; } = new();
        [MetaProperty("mSlots", BinPropertyType.Byte)]
        public byte Slots { get; set; } = 0;
    }
    [MetaClass("ContextualConditionItemPriceMinimum")]
    public class ContextualConditionItemPriceMinimum : IContextualCondition
    {
        [MetaProperty("mItemPriceMinimum", BinPropertyType.UInt32)]
        public uint ItemPriceMinimum { get; set; } = 0;
    }
    [MetaClass("StatBySubPartCalculationPart")]
    public class StatBySubPartCalculationPart : IGameCalculationPartWithStats
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte Stat { get; set; } = 0;
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte StatFormula { get; set; } = 0;
        [MetaProperty("mSubpart", BinPropertyType.Structure)]
        public IGameCalculationPart Subpart { get; set; } = null;
    }
    [MetaClass("MapSkin")]
    public class MapSkin : IMetaClass
    {
        [MetaProperty(351620029, BinPropertyType.ObjectLink)]
        public MetaObjectLink m351620029 { get; set; } = new(0);
        [MetaProperty("mMinimapBackgroundConfig", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapBackgroundConfig> MinimapBackgroundConfig { get; set; } = new (new ());
        [MetaProperty("mNavigationMesh", BinPropertyType.String)]
        public string NavigationMesh { get; set; } = "AIPath.aimesh_ngrid";
        [MetaProperty("mResourceResolvers", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ResourceResolvers { get; set; } = new();
        [MetaProperty("mObjectSkinFallbacks", BinPropertyType.Map)]
        public Dictionary<MetaHash, int> ObjectSkinFallbacks { get; set; } = new();
        [MetaProperty("mMapContainerLink", BinPropertyType.String)]
        public string MapContainerLink { get; set; } = "";
        [MetaProperty("mWorldParticlesINI", BinPropertyType.String)]
        public string WorldParticlesINI { get; set; } = "Particles.ini";
        [MetaProperty("mWorldGeometry", BinPropertyType.String)]
        public string WorldGeometry { get; set; } = "room";
        [MetaProperty(2968063630, BinPropertyType.String)]
        public string m2968063630 { get; set; } = "ASSETS/Maps/Skyboxes/Riots_SRU_Skybox_CubeMap.dds";
        [MetaProperty("mColorizationPostEffect", BinPropertyType.Structure)]
        public MapSkinColorizationPostEffect ColorizationPostEffect { get; set; } = null;
        [MetaProperty("mGrassTintTexture", BinPropertyType.String)]
        public string GrassTintTexture { get; set; } = "GrassTint.dds";
        [MetaProperty("mAlternateAssets", BinPropertyType.Embedded)]
        public MetaEmbedded<MapAlternateAssets> AlternateAssets { get; set; } = new (new ());
        [MetaProperty("mMapObjectsCFG", BinPropertyType.String)]
        public string MapObjectsCFG { get; set; } = "ObjectCFG.cfg";
    }
    [MetaClass("DamageSourceSettings")]
    public class DamageSourceSettings : IMetaClass
    {
        [MetaProperty("templateDefinition", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DamageSourceTemplate>> TemplateDefinition { get; set; } = new();
        [MetaProperty("damageTagDefinition", BinPropertyType.Container)]
        public MetaContainer<string> DamageTagDefinition { get; set; } = new();
    }
    [MetaClass(3443072153)]
    public class Class0xcd391c99 : IOptionItemFilter
    {
        [MetaProperty("ShowInTftReplay", BinPropertyType.Bool)]
        public bool ShowInTftReplay { get; set; } = true;
        [MetaProperty("ShowInLolGame", BinPropertyType.Bool)]
        public bool ShowInLolGame { get; set; } = true;
        [MetaProperty("ShowInTftGame", BinPropertyType.Bool)]
        public bool ShowInTftGame { get; set; } = true;
        [MetaProperty("ShowInLolReplay", BinPropertyType.Bool)]
        public bool ShowInLolReplay { get; set; } = true;
    }
    [MetaClass("ScriptPreloadParticle")]
    public class ScriptPreloadParticle : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string PreloadResourceName { get; set; } = "";
    }
    [MetaClass("ContextualConditionLastBoughtItem")]
    public class ContextualConditionLastBoughtItem : IContextualCondition
    {
        [MetaProperty("mItem", BinPropertyType.Hash)]
        public MetaHash Item { get; set; } = new(0);
    }
    [MetaClass("DrawablePositionLocator")]
    public class DrawablePositionLocator : IMetaClass
    {
        [MetaProperty("orientationType", BinPropertyType.UInt32)]
        public uint OrientationType { get; set; } = 0;
        [MetaProperty("basePosition", BinPropertyType.UInt32)]
        public uint BasePosition { get; set; } = 0;
        [MetaProperty("angleOffsetRadian", BinPropertyType.Float)]
        public float AngleOffsetRadian { get; set; } = 0f;
        [MetaProperty("distanceOffset", BinPropertyType.Float)]
        public float DistanceOffset { get; set; } = 0f;
    }
    [MetaClass("CursorDataCaptureCooldownContext")]
    public class CursorDataCaptureCooldownContext : IMetaClass
    {
        [MetaProperty("mData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorData>> Data { get; set; } = new();
    }
    [MetaClass("ElementGroupButtonState")]
    public class ElementGroupButtonState : IMetaClass
    {
        [MetaProperty("displayElements", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> DisplayElements { get; set; } = new();
        [MetaProperty("TextElement", BinPropertyType.ObjectLink)]
        public MetaObjectLink TextElement { get; set; } = new(0);
    }
    [MetaClass("ConformToPathRigPoseModifierData")]
    public class ConformToPathRigPoseModifierData : BaseRigPoseModifierData
    {
        [MetaProperty("mFrequency", BinPropertyType.Float)]
        public float Frequency { get; set; } = 10f;
        [MetaProperty("mVelMultiplier", BinPropertyType.Float)]
        public float VelMultiplier { get; set; } = -0.5f;
        [MetaProperty("mDefaultMaskName", BinPropertyType.Hash)]
        public MetaHash DefaultMaskName { get; set; } = new(0);
        [MetaProperty("mMaxBoneAngle", BinPropertyType.Float)]
        public float MaxBoneAngle { get; set; } = 65f;
        [MetaProperty("mEndingJointName", BinPropertyType.Hash)]
        public MetaHash EndingJointName { get; set; } = new(0);
        [MetaProperty("mStartingJointName", BinPropertyType.Hash)]
        public MetaHash StartingJointName { get; set; } = new(0);
        [MetaProperty("mDampingValue", BinPropertyType.Float)]
        public float DampingValue { get; set; } = 10f;
    }
    [MetaClass("AbilityResourceSlotInfo")]
    public class AbilityResourceSlotInfo : IMetaClass
    {
        [MetaProperty("arBaseFactorRegen", BinPropertyType.Float)]
        public float ArBaseFactorRegen { get; set; } = 0f;
        [MetaProperty("arType", BinPropertyType.Byte)]
        public byte ArType { get; set; } = 2;
        [MetaProperty("arOverrideSpacerName", BinPropertyType.String)]
        public string ArOverrideSpacerName { get; set; } = "";
        [MetaProperty("arBase", BinPropertyType.Float)]
        public float ArBase { get; set; } = 100f;
        [MetaProperty("arPreventRegenWhileAtZero", BinPropertyType.Bool)]
        public bool ArPreventRegenWhileAtZero { get; set; } = false;
        [MetaProperty("arBaseStaticRegen", BinPropertyType.Float)]
        public float ArBaseStaticRegen { get; set; } = 1f;
        [MetaProperty(1554462912, BinPropertyType.Bool)]
        public bool m1554462912 { get; set; } = false;
        [MetaProperty("arIsShownOnlyOnLocalPlayer", BinPropertyType.Bool)]
        public bool ArIsShownOnlyOnLocalPlayer { get; set; } = false;
        [MetaProperty("arOverrideLargePipName", BinPropertyType.String)]
        public string ArOverrideLargePipName { get; set; } = "";
        [MetaProperty("arOverrideEmptyPipName", BinPropertyType.String)]
        public string ArOverrideEmptyPipName { get; set; } = "";
        [MetaProperty("arMaxSegments", BinPropertyType.Int32)]
        public int ArMaxSegments { get; set; } = 0;
        [MetaProperty("arContributesToHealthValues", BinPropertyType.Bool)]
        public bool ArContributesToHealthValues { get; set; } = false;
        [MetaProperty("arHasRegenText", BinPropertyType.Bool)]
        public bool ArHasRegenText { get; set; } = true;
        [MetaProperty("arOverrideSmallPipName", BinPropertyType.String)]
        public string ArOverrideSmallPipName { get; set; } = "";
        [MetaProperty("arIncrements", BinPropertyType.Float)]
        public float ArIncrements { get; set; } = 0f;
        [MetaProperty(2849220732, BinPropertyType.Bool)]
        public bool m2849220732 { get; set; } = false;
        [MetaProperty("arDisplayAsPips", BinPropertyType.Bool)]
        public bool ArDisplayAsPips { get; set; } = false;
        [MetaProperty("arNegativeSpacer", BinPropertyType.Bool)]
        public bool ArNegativeSpacer { get; set; } = false;
        [MetaProperty("arRegenPerLevel", BinPropertyType.Float)]
        public float ArRegenPerLevel { get; set; } = 0f;
        [MetaProperty("arPerLevel", BinPropertyType.Float)]
        public float ArPerLevel { get; set; } = 0f;
        [MetaProperty("arOverrideMediumPipName", BinPropertyType.String)]
        public string ArOverrideMediumPipName { get; set; } = "";
        [MetaProperty("arAllowMaxValueToBeOverridden", BinPropertyType.Bool)]
        public bool ArAllowMaxValueToBeOverridden { get; set; } = false;
        [MetaProperty("arIsShown", BinPropertyType.Bool)]
        public bool ArIsShown { get; set; } = true;
    }
    [MetaClass("SkinnedMeshDataMaterialController")]
    public interface SkinnedMeshDataMaterialController : IMetaClass
    {
    }
    [MetaClass("SwitchCase")]
    public class SwitchCase : IMetaClass
    {
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; } = new (new ());
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("Condition", BinPropertyType.Structure)]
        public IScriptCondition Condition { get; set; } = null;
    }
    [MetaClass(3468103258)]
    public class Class0xceb70e5a : IMetaClass
    {
        [MetaProperty("backgroundElement", BinPropertyType.Hash)]
        public MetaHash BackgroundElement { get; set; } = new(0);
        [MetaProperty("TextElement", BinPropertyType.Hash)]
        public MetaHash TextElement { get; set; } = new(0);
    }
    [MetaClass(3470174985)]
    public class Class0xced6ab09 : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
        [MetaProperty("MapParticleName", BinPropertyType.Container)]
        public MetaContainer<string> MapParticleName { get; set; } = new();
        [MetaProperty("shown", BinPropertyType.Bool)]
        public bool Shown { get; set; } = true;
    }
    [MetaClass("ContextualActionData")]
    public class ContextualActionData : IResource
    {
        [MetaProperty("mObjectPath", BinPropertyType.String)]
        public string ObjectPath { get; set; } = "";
        [MetaProperty("mCooldown", BinPropertyType.Float)]
        public float Cooldown { get; set; } = 0f;
        [MetaProperty("mSituations", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<ContextualSituation>> Situations { get; set; } = new();
        [MetaProperty(2681747634, BinPropertyType.Float)]
        public float m2681747634 { get; set; } = 0f;
    }
    [MetaClass(3490803144)]
    public class Class0xd0116dc8 : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string KeyName { get; set; } = "";
    }
    [MetaClass("FloatGraphMaterialDriver")]
    public class FloatGraphMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("graph", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedFloatVariableData> Graph { get; set; } = new (new ());
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; } = null;
    }
    [MetaClass("CensoredImage")]
    public class CensoredImage : IMetaClass
    {
        [MetaProperty("UncensoredImages", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredImages { get; set; } = new();
        [MetaProperty("image", BinPropertyType.String)]
        public string Image { get; set; } = "";
    }
    [MetaClass(3498226065)]
    public class Class0xd082b191 : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("ZoomOutTime", BinPropertyType.Float)]
        public float ZoomOutTime { get; set; } = 0.5f;
        [MetaProperty(1514380197, BinPropertyType.Float)]
        public float m1514380197 { get; set; } = 4500f;
        [MetaProperty("ZoomOutEase", BinPropertyType.Byte)]
        public byte ZoomOutEase { get; set; } = 2;
        [MetaProperty("yaw", BinPropertyType.Float)]
        public float Yaw { get; set; } = 0f;
        [MetaProperty("ZoomInEase", BinPropertyType.Byte)]
        public byte ZoomInEase { get; set; } = 2;
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> Position { get; set; } = new (new ());
        [MetaProperty("ZoomInTime", BinPropertyType.Float)]
        public float ZoomInTime { get; set; } = 0.5f;
        [MetaProperty("fov", BinPropertyType.Float)]
        public float Fov { get; set; } = 45f;
        [MetaProperty("pitch", BinPropertyType.Float)]
        public float Pitch { get; set; } = 45f;
    }
    [MetaClass("EsportsBannerConfiguration")]
    public class EsportsBannerConfiguration : IMetaClass
    {
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string LeagueName { get; set; } = "";
        [MetaProperty("IndividualBannerOverrides", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SponsoredBanner>> IndividualBannerOverrides { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("esportsTeam", BinPropertyType.Hash)]
        public MetaHash EsportsTeam { get; set; } = new(0);
        [MetaProperty("texturePath", BinPropertyType.WadEntryLink)]
        public MetaWadEntryLink TexturePath { get; set; } = new(0);
        [MetaProperty("eventMutator", BinPropertyType.ObjectLink)]
        public MetaObjectLink EventMutator { get; set; } = new(0);
    }
    [MetaClass(3511278911)]
    public class Class0xd149dd3f : IMetaClass
    {
        [MetaProperty("ToSlotId", BinPropertyType.Int32)]
        public int ToSlotId { get; set; } = -1;
        [MetaProperty("FromSlotId", BinPropertyType.Int32)]
        public int FromSlotId { get; set; } = -1;
    }
    [MetaClass("TftItemData")]
    public class TftItemData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string BuffName { get; set; } = "";
        [MetaProperty("mVfxSystem", BinPropertyType.ObjectLink)]
        public MetaObjectLink VfxSystem { get; set; } = new(0);
        [MetaProperty("mColor", BinPropertyType.Optional)]
        public MetaOptional<Color> Color { get; set; } = new MetaOptional<Color>(default(Color), false);
        [MetaProperty(1733478293, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m1733478293 { get; set; } = new();
        [MetaProperty(1838141165, BinPropertyType.Int32)]
        public int m1838141165 { get; set; } = -1;
        [MetaProperty("mDescriptionNameTra", BinPropertyType.String)]
        public string DescriptionNameTra { get; set; } = "";
        [MetaProperty("mComposition", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Composition { get; set; } = new();
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string IconPath { get; set; } = "";
        [MetaProperty(2446810623, BinPropertyType.Vector2)]
        public Vector2 m2446810623 { get; set; } = new Vector2(0.20000000298023224f, 1f);
        [MetaProperty("mIsUnique", BinPropertyType.Bool)]
        public bool IsUnique { get; set; } = false;
        [MetaProperty(2745992408, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftItemComposition>> m2745992408 { get; set; } = new();
        [MetaProperty("ItemTags", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ItemTags { get; set; } = new();
        [MetaProperty("effectAmounts", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftEffectAmount>> EffectAmounts { get; set; } = new();
        [MetaProperty("mDisplayNameTra", BinPropertyType.String)]
        public string DisplayNameTra { get; set; } = "";
        [MetaProperty("mId", BinPropertyType.Int32)]
        public int Id { get; set; } = 0;
    }
    [MetaClass("IntegratedValueFloat")]
    public class IntegratedValueFloat : ValueFloat
    {
    }
    [MetaClass(3516211013)]
    public interface Class0xd1951f45 : IMetaClass
    {
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        float TransitionTime { get; set; }
        [MetaProperty("endAlpha", BinPropertyType.Byte)]
        byte EndAlpha { get; set; }
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        byte EasingType { get; set; }
        [MetaProperty("startAlpha", BinPropertyType.Byte)]
        byte StartAlpha { get; set; }
    }
    [MetaClass("TFTAnnouncementData")]
    public class TFTAnnouncementData : IMetaClass
    {
        [MetaProperty("mDelay", BinPropertyType.Float)]
        public float Delay { get; set; } = 0f;
        [MetaProperty("mDuration", BinPropertyType.Float)]
        public float Duration { get; set; } = 0f;
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string IconPath { get; set; } = "";
        [MetaProperty("mTitleTra", BinPropertyType.String)]
        public string TitleTra { get; set; } = "";
    }
    [MetaClass(3523215606)]
    public class Class0xd20000f6 : IMetaClass
    {
        [MetaProperty(1128878772, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xb35ad9d8> m1128878772 { get; set; } = new (new ());
        [MetaProperty("PinButton", BinPropertyType.Hash)]
        public MetaHash PinButton { get; set; } = new(0);
        [MetaProperty("SceneData", BinPropertyType.Hash)]
        public MetaHash SceneData { get; set; } = new(0);
    }
    [MetaClass("EffectFillPercentageElementData")]
    public class EffectFillPercentageElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
    }
    [MetaClass("HudReplaySliderData")]
    public class HudReplaySliderData : IMetaClass
    {
        [MetaProperty("mIconDataPriorityList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudReplaySliderIconData>> IconDataPriorityList { get; set; } = new();
        [MetaProperty("mTooltipEventWindow", BinPropertyType.Float)]
        public float TooltipEventWindow { get; set; } = 1f;
    }
    [MetaClass("Defaultvisibility")]
    public class Defaultvisibility : MissileVisibilitySpec
    {
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        public float PerceptionBubbleRadius { get; set; } = 0f;
        [MetaProperty("mTargetControlsVisibility", BinPropertyType.Bool)]
        public bool TargetControlsVisibility { get; set; } = false;
        [MetaProperty("mVisibleToOwnerTeamOnly", BinPropertyType.Bool)]
        public bool VisibleToOwnerTeamOnly { get; set; } = false;
        [MetaProperty(3143864407, BinPropertyType.Float)]
        public float m3143864407 { get; set; } = 0f;
    }
    [MetaClass("AbilityResourceDynamicMaterialFloatDriver")]
    public class AbilityResourceDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("slot", BinPropertyType.Byte)]
        public byte Slot { get; set; } = 0;
    }
    [MetaClass("HudElementalSectionUIData")]
    public class HudElementalSectionUIData : IMetaClass
    {
        [MetaProperty("stormColoration", BinPropertyType.Color)]
        public Color StormColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("fireColoration", BinPropertyType.Color)]
        public Color FireColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("firstSelectionAnimationDelay", BinPropertyType.Float)]
        public float FirstSelectionAnimationDelay { get; set; } = 0.75f;
        [MetaProperty("fairyColoration", BinPropertyType.Color)]
        public Color FairyColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("darkColoration", BinPropertyType.Color)]
        public Color DarkColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("glowingRingCycleTime", BinPropertyType.Float)]
        public float GlowingRingCycleTime { get; set; } = 0.75f;
        [MetaProperty("iceColoration", BinPropertyType.Color)]
        public Color IceColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("meterFilledButtonFadeInDelay", BinPropertyType.Float)]
        public float MeterFilledButtonFadeInDelay { get; set; } = 0.25f;
        [MetaProperty("magmaColoration", BinPropertyType.Color)]
        public Color MagmaColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("earthColoration", BinPropertyType.Color)]
        public Color EarthColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("lightColoration", BinPropertyType.Color)]
        public Color LightColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("waterColoration", BinPropertyType.Color)]
        public Color WaterColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("airColoration", BinPropertyType.Color)]
        public Color AirColoration { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("secondSelectionAnimationDelay", BinPropertyType.Float)]
        public float SecondSelectionAnimationDelay { get; set; } = 1f;
    }
    [MetaClass("MapTerrainPaint")]
    public class MapTerrainPaint : MapGraphicsFeature
    {
        [MetaProperty("TerrainPaintTexturePath", BinPropertyType.String)]
        public string TerrainPaintTexturePath { get; set; } = "";
    }
    [MetaClass("HudGameModeScoreData")]
    public class HudGameModeScoreData : IMetaClass
    {
        [MetaProperty("mEncounterUi", BinPropertyType.Structure)]
        public EncounterUITunables EncounterUi { get; set; } = null;
        [MetaProperty("mTeamFightUi", BinPropertyType.Structure)]
        public HudTeamFightData TeamFightUi { get; set; } = null;
        [MetaProperty("mIndividualScoreElementTypes", BinPropertyType.Container)]
        public MetaContainer<byte> IndividualScoreElementTypes { get; set; } = new();
        [MetaProperty("mOptionalBins", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudOptionalBinData>> OptionalBins { get; set; } = new();
        [MetaProperty("mTeamScoreElementTypes", BinPropertyType.Container)]
        public MetaContainer<byte> TeamScoreElementTypes { get; set; } = new();
        [MetaProperty("mTeamScoreMeterUi", BinPropertyType.Structure)]
        public TeamScoreMeterUITunables TeamScoreMeterUi { get; set; } = null;
        [MetaProperty("mDragonUi", BinPropertyType.Structure)]
        public DragonUITunables DragonUi { get; set; } = null;
        [MetaProperty("mQuestUi", BinPropertyType.Structure)]
        public QuestUITunables QuestUi { get; set; } = null;
        [MetaProperty("mTeamGameScorePingMessage", BinPropertyType.String)]
        public string TeamGameScorePingMessage { get; set; } = "";
        [MetaProperty("mModeKeyName", BinPropertyType.String)]
        public string ModeKeyName { get; set; } = "";
    }
    [MetaClass("IScriptPreload")]
    public interface IScriptPreload : IMetaClass
    {
    }
    [MetaClass("IconElementGradientExtension")]
    public class IconElementGradientExtension : IconElementDataExtension
    {
        [MetaProperty("mStartColor", BinPropertyType.Vector4)]
        public Vector4 StartColor { get; set; } = new Vector4(1f, 0f, 0f, 1f);
        [MetaProperty(1308578205, BinPropertyType.Vector4)]
        public Vector4 m1308578205 { get; set; } = new Vector4(1f, 0f, 0f, 0f);
        [MetaProperty("mAlphaTexture", BinPropertyType.String)]
        public string AlphaTexture { get; set; } = "";
        [MetaProperty("mEndColor", BinPropertyType.Vector4)]
        public Vector4 EndColor { get; set; } = new Vector4(0f, 0f, 1f, 1f);
        [MetaProperty("mGradientDirection", BinPropertyType.UInt32)]
        public uint GradientDirection { get; set; } = 0;
    }
    [MetaClass("SpellPassiveData")]
    public class SpellPassiveData : IMetaClass
    {
        [MetaProperty("mDisplayFlags", BinPropertyType.Byte)]
        public byte DisplayFlags { get; set; } = 3;
        [MetaProperty(1991670732, BinPropertyType.Bool)]
        public bool m1991670732 { get; set; } = true;
        [MetaProperty(2057371350, BinPropertyType.Bool)]
        public bool m2057371350 { get; set; } = false;
        [MetaProperty("mBuff", BinPropertyType.ObjectLink)]
        public MetaObjectLink Buff { get; set; } = new(0);
        [MetaProperty(2257773130, BinPropertyType.UInt32)]
        public uint m2257773130 { get; set; } = 1;
        [MetaProperty(3420404466, BinPropertyType.Bool)]
        public bool m3420404466 { get; set; } = true;
        [MetaProperty(4167197483, BinPropertyType.Bool)]
        public bool m4167197483 { get; set; } = false;
    }
    [MetaClass("ReturnToCaster")]
    public class ReturnToCaster : MissileTriggeredActionSpec
    {
        [MetaProperty("mPreserveSpeed", BinPropertyType.Bool)]
        public bool PreserveSpeed { get; set; } = false;
        [MetaProperty("mOverrideSpec", BinPropertyType.Structure)]
        public MissileMovementSpec OverrideSpec { get; set; } = null;
    }
    [MetaClass("BannerFrameData")]
    public class BannerFrameData : IMetaClass
    {
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; } = new (new ());
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnimationGraphData { get; set; } = new(0);
    }
    [MetaClass("SpellLockDeltaTimeData")]
    public class SpellLockDeltaTimeData : IMetaClass
    {
        [MetaProperty("mSpellLockDeltaTimeCalculation", BinPropertyType.Structure)]
        public IGameCalculation SpellLockDeltaTimeCalculation { get; set; } = null;
    }
    [MetaClass("FloatTableSet")]
    public class FloatTableSet : ScriptTableSet
    {
    }
    [MetaClass("IsOwnerAliveConditionData")]
    public class IsOwnerAliveConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; } = new();
    }
    [MetaClass("StringTableSet")]
    public class StringTableSet : ScriptTableSet
    {
    }
    [MetaClass("NotMaterialDriver")]
    public class NotMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Driver { get; set; } = null;
    }
    [MetaClass("FxActionSfx")]
    public class FxActionSfx : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("EventName", BinPropertyType.String)]
        public string EventName { get; set; } = "";
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> Position { get; set; } = new (new ());
    }
    [MetaClass("MaxAllSkillsCheat")]
    public class MaxAllSkillsCheat : Cheat
    {
        [MetaProperty("mOnlyOnePointEach", BinPropertyType.Bool)]
        public bool OnlyOnePointEach { get; set; } = false;
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass("ElementGroupButtonData")]
    public class ElementGroupButtonData : ElementGroupData
    {
        [MetaProperty("defaultStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupButtonState> DefaultStateElements { get; set; } = new (new ());
        [MetaProperty("selectedStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupButtonState> SelectedStateElements { get; set; } = new (new ());
        [MetaProperty("InactiveTooltipTraKey", BinPropertyType.String)]
        public string InactiveTooltipTraKey { get; set; } = "";
        [MetaProperty("SelectedClickedStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupButtonState> SelectedClickedStateElements { get; set; } = new (new ());
        [MetaProperty("hitRegionElement", BinPropertyType.ObjectLink)]
        public MetaObjectLink HitRegionElement { get; set; } = new(0);
        [MetaProperty("InactiveStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupButtonState> InactiveStateElements { get; set; } = new (new ());
        [MetaProperty("ClickReleaseParticleElement", BinPropertyType.ObjectLink)]
        public MetaObjectLink ClickReleaseParticleElement { get; set; } = new(0);
        [MetaProperty("ActiveTooltipTraKey", BinPropertyType.String)]
        public string ActiveTooltipTraKey { get; set; } = "";
        [MetaProperty("soundEvents", BinPropertyType.Structure)]
        public ElementSoundEvents SoundEvents { get; set; } = null;
        [MetaProperty(2903476354, BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupButtonState> m2903476354 { get; set; } = new (new ());
        [MetaProperty("SelectedHoverStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupButtonState> SelectedHoverStateElements { get; set; } = new (new ());
        [MetaProperty("hoverStateElements", BinPropertyType.Embedded)]
        public MetaEmbedded<ElementGroupButtonState> HoverStateElements { get; set; } = new (new ());
        [MetaProperty("IsActive", BinPropertyType.Bool)]
        public bool IsActive { get; set; } = true;
        [MetaProperty("IsEnabled", BinPropertyType.Bool)]
        public bool IsEnabled { get; set; } = false;
        [MetaProperty("IsSelected", BinPropertyType.Bool)]
        public bool IsSelected { get; set; } = false;
    }
    [MetaClass("EsportsBannerOptions")]
    public class EsportsBannerOptions : IMetaClass
    {
        [MetaProperty("subMeshName", BinPropertyType.String)]
        public string SubMeshName { get; set; } = "";
        [MetaProperty("defaultTexturePath", BinPropertyType.WadEntryLink)]
        public MetaWadEntryLink DefaultTexturePath { get; set; } = new(0);
        [MetaProperty("IsSpectatorOnly", BinPropertyType.Bool)]
        public bool IsSpectatorOnly { get; set; } = true;
        [MetaProperty("DefaultBlankMaterial", BinPropertyType.ObjectLink)]
        public MetaObjectLink DefaultBlankMaterial { get; set; } = new(0);
    }
    [MetaClass("HudPingData")]
    public class HudPingData : IMetaClass
    {
        [MetaProperty("distanceToNotTrollPingCorpses", BinPropertyType.Float)]
        public float DistanceToNotTrollPingCorpses { get; set; } = 1000f;
        [MetaProperty("timeToNotTrollPingCorpses", BinPropertyType.Float)]
        public float TimeToNotTrollPingCorpses { get; set; } = 10f;
    }
    [MetaClass("AnnouncementMap")]
    public class AnnouncementMap : IMetaClass
    {
        [MetaProperty("Announcements", BinPropertyType.Map)]
        public Dictionary<string, MetaObjectLink> Announcements { get; set; } = new();
        [MetaProperty(2031574661, BinPropertyType.Hash)]
        public MetaHash m2031574661 { get; set; } = new(0);
        [MetaProperty(2164797003, BinPropertyType.String)]
        public string m2164797003 { get; set; } = "";
        [MetaProperty(3482081563, BinPropertyType.Hash)]
        public MetaHash m3482081563 { get; set; } = new(0);
        [MetaProperty("ParentList", BinPropertyType.ObjectLink)]
        public MetaObjectLink ParentList { get; set; } = new(0);
    }
    [MetaClass("TFTBattlepassViewController")]
    public class TFTBattlepassViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(1458709778, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1458709778 { get; set; } = new(0);
        [MetaProperty(1695233717, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m1695233717 { get; set; } = new (new ());
        [MetaProperty("lootTableButton", BinPropertyType.Hash)]
        public MetaHash LootTableButton { get; set; } = new(0);
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty("upgradePassButton", BinPropertyType.Hash)]
        public MetaHash UpgradePassButton { get; set; } = new(0);
        [MetaProperty(3224678146, BinPropertyType.Float)]
        public float m3224678146 { get; set; } = 3f;
    }
    [MetaClass("TftDamageSkinDescriptor")]
    public class TftDamageSkinDescriptor : IMetaClass
    {
        [MetaProperty("EffectType", BinPropertyType.UInt32)]
        public uint EffectType { get; set; } = 0;
        [MetaProperty("effectKey", BinPropertyType.String)]
        public string EffectKey { get; set; } = "";
        [MetaProperty("AttachedToBone", BinPropertyType.UInt32)]
        public uint AttachedToBone { get; set; } = 0;
        [MetaProperty(2634861147, BinPropertyType.UInt32)]
        public uint m2634861147 { get; set; } = 0;
        [MetaProperty("EffectDelay", BinPropertyType.Float)]
        public float EffectDelay { get; set; } = 0f;
    }
    [MetaClass("OptionItemGroup")]
    public class OptionItemGroup : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<IOptionItem> Items { get; set; } = new();
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
        [MetaProperty("LabelTraKey", BinPropertyType.String)]
        public string LabelTraKey { get; set; } = "";
        [MetaProperty("ExpandedByDefault", BinPropertyType.Bool)]
        public bool ExpandedByDefault { get; set; } = false;
    }
    [MetaClass("HealthDynamicMaterialFloatDriver")]
    public class HealthDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass(3641602072)]
    public class Class0xd90e7018 : IOptionItemFilter
    {
        [MetaProperty("Map", BinPropertyType.Hash)]
        public MetaHash Map { get; set; } = new(0);
    }
    [MetaClass("ChangeMissileSpeed")]
    public class ChangeMissileSpeed : MissileTriggeredActionSpec
    {
        [MetaProperty("mSpeedValue", BinPropertyType.Float)]
        public float SpeedValue { get; set; } = 0f;
        [MetaProperty("mSpeedChangeType", BinPropertyType.UInt32)]
        public uint SpeedChangeType { get; set; } = 0;
    }
    [MetaClass("VfxMaterialOverrideDefinitionData")]
    public class VfxMaterialOverrideDefinitionData : IMetaClass
    {
        [MetaProperty("transitionTexture", BinPropertyType.String)]
        public string TransitionTexture { get; set; } = "";
        [MetaProperty("subMeshName", BinPropertyType.Optional)]
        public MetaOptional<string> SubMeshName { get; set; } = new MetaOptional<string>(default(string), false);
        [MetaProperty("overrideBlendMode", BinPropertyType.UInt32)]
        public uint OverrideBlendMode { get; set; } = 0;
        [MetaProperty("transitionSample", BinPropertyType.Float)]
        public float TransitionSample { get; set; } = 0f;
        [MetaProperty("priority", BinPropertyType.Int32)]
        public int Priority { get; set; } = 0;
        [MetaProperty("baseTexture", BinPropertyType.String)]
        public string BaseTexture { get; set; } = "";
        [MetaProperty("transitionSource", BinPropertyType.UInt32)]
        public uint TransitionSource { get; set; } = 0;
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink Material { get; set; } = new(0);
        [MetaProperty("glossTexture", BinPropertyType.String)]
        public string GlossTexture { get; set; } = "";
    }
    [MetaClass("CastOnHit")]
    public class CastOnHit : MissileBehaviorSpec
    {
    }
    [MetaClass("ResetGoldCheat")]
    public class ResetGoldCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass(3661280452)]
    public class Class0xda3ab4c4 : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("Magnitude", BinPropertyType.Float)]
        public float Magnitude { get; set; } = 20f;
        [MetaProperty(3299858169, BinPropertyType.Float)]
        public float m3299858169 { get; set; } = 6f;
        [MetaProperty("FalloffRate", BinPropertyType.Float)]
        public float FalloffRate { get; set; } = 2f;
        [MetaProperty("direction", BinPropertyType.Vector3)]
        public Vector3 Direction { get; set; } = new Vector3(1f, 1f, 0f);
    }
    [MetaClass(3661393350)]
    public class Class0xda3c6dc6 : IMetaClass
    {
        [MetaProperty("mSceneTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionIn { get; set; } = new (new ());
        [MetaProperty("mSceneTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionOut { get; set; } = new (new ());
    }
    [MetaClass("TargetTeamFilter")]
    public class TargetTeamFilter : IStatStoneLogicDriver
    {
        [MetaProperty("Self", BinPropertyType.Bool)]
        public bool Self { get; set; } = false;
        [MetaProperty("enemy", BinPropertyType.Bool)]
        public bool Enemy { get; set; } = false;
        [MetaProperty("ally", BinPropertyType.Bool)]
        public bool Ally { get; set; } = false;
    }
    [MetaClass("IDynamicMaterialBoolDriver")]
    public interface IDynamicMaterialBoolDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass("HudLoadingScreenWidgetPlayers")]
    public class HudLoadingScreenWidgetPlayers : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string SceneName { get; set; } = "";
        [MetaProperty("mCardConfig", BinPropertyType.Embedded)]
        public MetaEmbedded<PlayerCardWidgetConfig> CardConfig { get; set; } = new (new ());
    }
    [MetaClass("GdsMapObject")]
    public class GdsMapObject : GenericMapPlaceable
    {
        [MetaProperty("type", BinPropertyType.Byte)]
        public byte Type { get; set; } = 0;
        [MetaProperty("eyeCandy", BinPropertyType.Bool)]
        public bool EyeCandy { get; set; } = false;
        [MetaProperty("ignoreCollisionOnPlacement", BinPropertyType.Bool)]
        public bool IgnoreCollisionOnPlacement { get; set; } = false;
        [MetaProperty("mapObjectSkinID", BinPropertyType.UInt32)]
        public uint MapObjectSkinID { get; set; } = 0;
        [MetaProperty("boxMin", BinPropertyType.Vector3)]
        public Vector3 BoxMin { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("boxMax", BinPropertyType.Vector3)]
        public Vector3 BoxMax { get; set; } = new Vector3(0f, 0f, 0f);
        [MetaProperty("extraInfo", BinPropertyType.Container)]
        public MetaContainer<GDSMapObjectExtraInfo> ExtraInfo { get; set; } = new();
    }
    [MetaClass("TFTHudMobileDownscaleData")]
    public class TFTHudMobileDownscaleData : IMetaClass
    {
        [MetaProperty("mDownscale", BinPropertyType.Float)]
        public float Downscale { get; set; } = 0.699999988079071f;
        [MetaProperty(3570592338, BinPropertyType.Float)]
        public float m3570592338 { get; set; } = 1.600000023841858f;
    }
    [MetaClass("TftMapCharacterRecordData")]
    public class TftMapCharacterRecordData : IMetaClass
    {
        [MetaProperty("tier", BinPropertyType.Byte)]
        public byte Tier { get; set; } = 0;
    }
    [MetaClass("IconElementDataExtension")]
    public interface IconElementDataExtension : IMetaClass
    {
    }
    [MetaClass("IVectorGet")]
    public interface IVectorGet : IScriptValueGet
    {
    }
    [MetaClass("OptionItemSliderVolume")]
    public class OptionItemSliderVolume : OptionItemSliderFloat
    {
        [MetaProperty("MuteButtonTemplate", BinPropertyType.Hash)]
        public MetaHash MuteButtonTemplate { get; set; } = new(0);
        [MetaProperty("MuteOption", BinPropertyType.UInt16)]
        public ushort MuteOption { get; set; } = 65535;
    }
    [MetaClass("HasBuffComparisonData")]
    public class HasBuffComparisonData : IMetaClass
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 0;
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HasBuffData>> Buffs { get; set; } = new();
    }
    [MetaClass("StringGet")]
    public class StringGet : IStringGet
    {
        [MetaProperty("value", BinPropertyType.String)]
        public string Value { get; set; } = "";
    }
    [MetaClass("FxTiming")]
    public class FxTiming : IMetaClass
    {
        [MetaProperty("offset", BinPropertyType.Float)]
        public float Offset { get; set; } = 0f;
        [MetaProperty("anchor", BinPropertyType.UInt32)]
        public uint Anchor { get; set; } = 0;
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 1;
    }
    [MetaClass("NeutralTimerSourceIconData")]
    public class NeutralTimerSourceIconData : IMetaClass
    {
        [MetaProperty("mTooltipName", BinPropertyType.String)]
        public string TooltipName { get; set; } = "";
        [MetaProperty("mIconName", BinPropertyType.String)]
        public string IconName { get; set; } = "";
    }
    [MetaClass("ScriptCheat")]
    public class ScriptCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
        [MetaProperty("mScriptCallback", BinPropertyType.Hash)]
        public MetaHash ScriptCallback { get; set; } = new(0);
    }
    [MetaClass("AbilityObject")]
    public class AbilityObject : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mRootSpell", BinPropertyType.ObjectLink)]
        public MetaObjectLink RootSpell { get; set; } = new(0);
        [MetaProperty(2262674907, BinPropertyType.Bool)]
        public bool m2262674907 { get; set; } = false;
        [MetaProperty("mType", BinPropertyType.Byte)]
        public byte Type { get; set; } = 1;
        [MetaProperty("mChildSpells", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ChildSpells { get; set; } = new();
    }
    [MetaClass(3697672164)]
    public class Class0xdc65ffe4 : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
    }
    [MetaClass("MissileAttachedTargetingDefinition")]
    public class MissileAttachedTargetingDefinition : IMetaClass
    {
        [MetaProperty("mLineTextureName", BinPropertyType.String)]
        public string LineTextureName { get; set; } = "";
        [MetaProperty("mLineTextureWidth", BinPropertyType.Float)]
        public float LineTextureWidth { get; set; } = 10f;
        [MetaProperty("mEndPositionType", BinPropertyType.Byte)]
        public byte EndPositionType { get; set; } = 0;
        [MetaProperty("mLineEndTextureHeight", BinPropertyType.Float)]
        public float LineEndTextureHeight { get; set; } = 10f;
        [MetaProperty("mLineEndTextureName", BinPropertyType.String)]
        public string LineEndTextureName { get; set; } = "";
        [MetaProperty("mLineEndTextureWidth", BinPropertyType.Float)]
        public float LineEndTextureWidth { get; set; } = 10f;
    }
    [MetaClass("MapLightingV2")]
    public class MapLightingV2 : MapGraphicsFeature
    {
        [MetaProperty(3373705724, BinPropertyType.Float)]
        public float m3373705724 { get; set; } = 3000f;
        [MetaProperty(4002480509, BinPropertyType.Float)]
        public float m4002480509 { get; set; } = 0f;
    }
    [MetaClass("HudReplaySliderIconData")]
    public class HudReplaySliderIconData : IMetaClass
    {
        [MetaProperty("mElementSpacer", BinPropertyType.Float)]
        public float ElementSpacer { get; set; } = 1f;
        [MetaProperty("mTooltipIconNames", BinPropertyType.Container)]
        public MetaContainer<string> TooltipIconNames { get; set; } = new();
        [MetaProperty("mElementAlphaDefault", BinPropertyType.Float)]
        public float ElementAlphaDefault { get; set; } = 255f;
        [MetaProperty("mElementName", BinPropertyType.String)]
        public string ElementName { get; set; } = "";
        [MetaProperty("mElementAlphaSelected", BinPropertyType.Float)]
        public float ElementAlphaSelected { get; set; } = 255f;
        [MetaProperty("mElementAlphaUnselected", BinPropertyType.Float)]
        public float ElementAlphaUnselected { get; set; } = 80f;
        [MetaProperty("mType", BinPropertyType.Hash)]
        public MetaHash Type { get; set; } = new(0);
        [MetaProperty("mTooltipStyle", BinPropertyType.Byte)]
        public byte TooltipStyle { get; set; } = 2;
    }
    [MetaClass("FontResolutionData")]
    public class FontResolutionData : IMetaClass
    {
        [MetaProperty("autoScale", BinPropertyType.Bool)]
        public bool AutoScale { get; set; } = true;
        [MetaProperty("localeResolutions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FontLocaleResolutions>> LocaleResolutions { get; set; } = new();
    }
    [MetaClass("IsOwnerHeroConditionData")]
    public class IsOwnerHeroConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; } = new();
    }
    [MetaClass("EffectCircleMaskDesaturateElementData")]
    public class EffectCircleMaskDesaturateElementData : EffectDesaturateElementData
    {
    }
    [MetaClass("Cone")]
    public class Cone : TargetingTypeData
    {
    }
    [MetaClass("LoLSpellPreloadData")]
    public class LoLSpellPreloadData : IMetaClass
    {
        [MetaProperty("ModulePreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadModule> ModulePreloads { get; set; } = new();
        [MetaProperty("SpellPreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadSpell> SpellPreloads { get; set; } = new();
        [MetaProperty("ParticlePreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadParticle> ParticlePreloads { get; set; } = new();
        [MetaProperty("CharacterPreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadCharacter> CharacterPreloads { get; set; } = new();
    }
    [MetaClass("VfxPrimitiveBeamBase")]
    public interface VfxPrimitiveBeamBase : VfxPrimitiveBase
    {
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; }
    }
    [MetaClass("ParticleSystemElementData")]
    public class ParticleSystemElementData : BaseElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty(42091584, BinPropertyType.Bool)]
        public bool m42091584 { get; set; } = true;
        [MetaProperty("mVfxSystem", BinPropertyType.ObjectLink)]
        public MetaObjectLink VfxSystem { get; set; } = new(0);
        [MetaProperty(2398497225, BinPropertyType.Bool)]
        public bool m2398497225 { get; set; } = true;
        [MetaProperty(2494597354, BinPropertyType.Bool)]
        public bool m2494597354 { get; set; } = true;
        [MetaProperty(3296523975, BinPropertyType.UInt32)]
        public uint m3296523975 { get; set; } = 1;
    }
    [MetaClass("PerkSlot")]
    public class PerkSlot : IMetaClass
    {
        [MetaProperty("mPerks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Perks { get; set; } = new();
        [MetaProperty("mType", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 1;
        [MetaProperty("mSlotLabelKey", BinPropertyType.String)]
        public string SlotLabelKey { get; set; } = "";
    }
    [MetaClass("PostGameViewController")]
    public class PostGameViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("CurrentExpText", BinPropertyType.Hash)]
        public MetaHash CurrentExpText { get; set; } = new(0);
        [MetaProperty(448744217, BinPropertyType.Hash)]
        public MetaHash m448744217 { get; set; } = new(0);
        [MetaProperty("GainedRatingText", BinPropertyType.Hash)]
        public MetaHash GainedRatingText { get; set; } = new(0);
        [MetaProperty("RankIconData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<NamedIconData>> RankIconData { get; set; } = new();
        [MetaProperty("GainedExpText", BinPropertyType.Hash)]
        public MetaHash GainedExpText { get; set; } = new(0);
        [MetaProperty("RatedIconData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<NamedIconData>> RatedIconData { get; set; } = new();
        [MetaProperty("PlayAgainButtonDefinition", BinPropertyType.Hash)]
        public MetaHash PlayAgainButtonDefinition { get; set; } = new(0);
        [MetaProperty("PlacementTextRight", BinPropertyType.Hash)]
        public MetaHash PlacementTextRight { get; set; } = new(0);
        [MetaProperty("QuitButtonDefinition", BinPropertyType.Hash)]
        public MetaHash QuitButtonDefinition { get; set; } = new(0);
        [MetaProperty("CurrentPlayerHighlight", BinPropertyType.Hash)]
        public MetaHash CurrentPlayerHighlight { get; set; } = new(0);
        [MetaProperty("LostExpText", BinPropertyType.Hash)]
        public MetaHash LostExpText { get; set; } = new(0);
        [MetaProperty("LostRatingText", BinPropertyType.Hash)]
        public MetaHash LostRatingText { get; set; } = new(0);
        [MetaProperty(2395837000, BinPropertyType.Hash)]
        public MetaHash m2395837000 { get; set; } = new(0);
        [MetaProperty("RankText", BinPropertyType.Hash)]
        public MetaHash RankText { get; set; } = new(0);
        [MetaProperty("ProvisionalTextRight", BinPropertyType.Hash)]
        public MetaHash ProvisionalTextRight { get; set; } = new(0);
        [MetaProperty("CurrentRatingText", BinPropertyType.Hash)]
        public MetaHash CurrentRatingText { get; set; } = new(0);
        [MetaProperty("BackgroundTexture", BinPropertyType.Hash)]
        public MetaHash BackgroundTexture { get; set; } = new(0);
    }
    [MetaClass("MapContainer")]
    public class MapContainer : IMetaClass
    {
        [MetaProperty("components", BinPropertyType.Container)]
        public MetaContainer<MapComponent> Components { get; set; } = new();
        [MetaProperty("chunks", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> Chunks { get; set; } = new();
        [MetaProperty("boundsMin", BinPropertyType.Vector2)]
        public Vector2 BoundsMin { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("boundsMax", BinPropertyType.Vector2)]
        public Vector2 BoundsMax { get; set; } = new Vector2(14820f, 14881f);
        [MetaProperty("mapPath", BinPropertyType.String)]
        public string MapPath { get; set; } = "";
        [MetaProperty("lowestWalkableHeight", BinPropertyType.Float)]
        public float LowestWalkableHeight { get; set; } = 0f;
        [MetaProperty(4027637499, BinPropertyType.Float)]
        public float m4027637499 { get; set; } = 0f;
    }
    [MetaClass("ICharacterSubcondition")]
    public interface ICharacterSubcondition : IMetaClass
    {
    }
    [MetaClass("TFTAttachmentSlotStyleData")]
    public class TFTAttachmentSlotStyleData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mOverlayIconPath", BinPropertyType.String)]
        public string OverlayIconPath { get; set; } = "";
        [MetaProperty("mSubtextTra", BinPropertyType.String)]
        public string SubtextTra { get; set; } = "";
    }
    [MetaClass("IsInGrassDynamicMaterialBoolDriver")]
    public class IsInGrassDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
    }
    [MetaClass("StaticMaterialShaderParamDef")]
    public class StaticMaterialShaderParamDef : IMetaClass
    {
        [MetaProperty("value", BinPropertyType.Vector4)]
        public Vector4 Value { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
    }
    [MetaClass("ContextualConditionTeammateDeathsNearby")]
    public class ContextualConditionTeammateDeathsNearby : IContextualCondition
    {
        [MetaProperty("mTeammateDeaths", BinPropertyType.UInt32)]
        public uint TeammateDeaths { get; set; } = 0;
    }
    [MetaClass("AnnouncementStyleBasic")]
    public class AnnouncementStyleBasic : IMetaClass
    {
        [MetaProperty("TextField", BinPropertyType.Hash)]
        public MetaHash TextField { get; set; } = new(0);
        [MetaProperty(1061361454, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m1061361454 { get; set; } = new (new ());
        [MetaProperty("MinAnnouncementDuration", BinPropertyType.Float)]
        public float MinAnnouncementDuration { get; set; } = 2.5f;
        [MetaProperty(3432620763, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m3432620763 { get; set; } = new (new ());
    }
    [MetaClass("MinimapIconTextureData")]
    public class MinimapIconTextureData : IMetaClass
    {
        [MetaProperty("mBase", BinPropertyType.String)]
        public string Base { get; set; } = "";
        [MetaProperty("mColorblind", BinPropertyType.Optional)]
        public MetaOptional<string> Colorblind { get; set; } = new MetaOptional<string>(default(string), false);
    }
    [MetaClass("ContextualConditionCharacterName")]
    public class ContextualConditionCharacterName : ICharacterSubcondition
    {
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Characters { get; set; } = new();
    }
    [MetaClass("AnchorDouble")]
    public class AnchorDouble : AnchorBase
    {
        [MetaProperty("anchorLeft", BinPropertyType.Vector2)]
        public Vector2 AnchorLeft { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("anchorRight", BinPropertyType.Vector2)]
        public Vector2 AnchorRight { get; set; } = new Vector2(0f, 0f);
    }
    [MetaClass("IsSpecifiedUnitCastRequirement")]
    public class IsSpecifiedUnitCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mUnit", BinPropertyType.Hash)]
        public MetaHash Unit { get; set; } = new(0);
    }
    [MetaClass("ToggleInvulnerableCheat")]
    public class ToggleInvulnerableCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
    }
    [MetaClass("Direction")]
    public class Direction : TargetingTypeData
    {
    }
    [MetaClass("MapThemeMusic")]
    public class MapThemeMusic : MapComponent
    {
        [MetaProperty("ThemeMusicTransitionEvent", BinPropertyType.String)]
        public string ThemeMusicTransitionEvent { get; set; } = "";
        [MetaProperty("LocalThemeMusic", BinPropertyType.String)]
        public string LocalThemeMusic { get; set; } = "";
    }
    [MetaClass("Map")]
    public class Map : WadFileDescriptor
    {
        [MetaProperty(1732568803, BinPropertyType.Byte)]
        public byte m1732568803 { get; set; } = 255;
        [MetaProperty("NavigationGridOverlays", BinPropertyType.Embedded)]
        public MetaEmbedded<MapNavigationGridOverlays> NavigationGridOverlays { get; set; } = new (new ());
        [MetaProperty("characterLists", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> CharacterLists { get; set; } = new();
        [MetaProperty(2650904341, BinPropertyType.Embedded)]
        public MetaEmbedded<MapVisibilityFlagDefinitions> m2650904341 { get; set; } = new (new ());
        [MetaProperty("mapStringId", BinPropertyType.String)]
        public string MapStringId { get; set; } = "";
        [MetaProperty("BasedOnMap", BinPropertyType.ObjectLink)]
        public MetaObjectLink BasedOnMap { get; set; } = new(0);
    }
    [MetaClass("HudHealthBarBurstData")]
    public class HudHealthBarBurstData : IMetaClass
    {
        [MetaProperty("burstTriggerPercent", BinPropertyType.Float)]
        public float BurstTriggerPercent { get; set; } = 0.3499999940395355f;
        [MetaProperty("fadeAcceleration", BinPropertyType.Float)]
        public float FadeAcceleration { get; set; } = 0f;
        [MetaProperty("flashTriggerPercent", BinPropertyType.Float)]
        public float FlashTriggerPercent { get; set; } = 0.3499999940395355f;
        [MetaProperty("fadeHoldTime", BinPropertyType.Float)]
        public float FadeHoldTime { get; set; } = 0f;
        [MetaProperty("flashDuration", BinPropertyType.Float)]
        public float FlashDuration { get; set; } = 0.20000000298023224f;
        [MetaProperty("shakeTriggerPercent", BinPropertyType.Float)]
        public float ShakeTriggerPercent { get; set; } = 0.30000001192092896f;
        [MetaProperty("burstTimeWindow", BinPropertyType.Float)]
        public float BurstTimeWindow { get; set; } = 0.20000000298023224f;
        [MetaProperty("shakeDuration", BinPropertyType.Float)]
        public float ShakeDuration { get; set; } = 0.10000000149011612f;
        [MetaProperty("shakeFrequency", BinPropertyType.Float)]
        public float ShakeFrequency { get; set; } = 10f;
        [MetaProperty("shakeBox", BinPropertyType.Vector2)]
        public Vector2 ShakeBox { get; set; } = new Vector2(6f, 3f);
        [MetaProperty("shakeReferenceResolution", BinPropertyType.Vector2)]
        public Vector2 ShakeReferenceResolution { get; set; } = new Vector2(1024f, 768f);
        [MetaProperty("fadeSpeed", BinPropertyType.Float)]
        public float FadeSpeed { get; set; } = 0.5f;
    }
    [MetaClass("CameraConfig")]
    public class CameraConfig : IMetaClass
    {
        [MetaProperty("mDragMomentumDecay", BinPropertyType.Float)]
        public float DragMomentumDecay { get; set; } = 0.20000000298023224f;
        [MetaProperty(943673768, BinPropertyType.Float)]
        public float m943673768 { get; set; } = 0f;
        [MetaProperty("mDragScale", BinPropertyType.Float)]
        public float DragScale { get; set; } = 2f;
        [MetaProperty(108120199, BinPropertyType.Embedded)]
        public MetaEmbedded<CameraTrapezoid> m108120199 { get; set; } = new (new ());
        [MetaProperty(1909011002, BinPropertyType.Embedded)]
        public MetaEmbedded<CameraTrapezoid> m1909011002 { get; set; } = new (new ());
        [MetaProperty("mTopdownZoom", BinPropertyType.Float)]
        public float TopdownZoom { get; set; } = 600f;
        [MetaProperty("mDragMomentumRecencyWeight", BinPropertyType.Float)]
        public float DragMomentumRecencyWeight { get; set; } = 0.25f;
        [MetaProperty("mLockedCameraEasingDistance", BinPropertyType.Float)]
        public float LockedCameraEasingDistance { get; set; } = 1000f;
        [MetaProperty("mZoomMinSpeed", BinPropertyType.Float)]
        public float ZoomMinSpeed { get; set; } = 3f;
        [MetaProperty("mTransitionDurationIntoCinematicMode", BinPropertyType.Float)]
        public float TransitionDurationIntoCinematicMode { get; set; } = 1f;
        [MetaProperty("mZoomEaseTime", BinPropertyType.Float)]
        public float ZoomEaseTime { get; set; } = 0.20000000298023224f;
        [MetaProperty("mDecelerationTimeMouse", BinPropertyType.Float)]
        public float DecelerationTimeMouse { get; set; } = 0.10000000149011612f;
        [MetaProperty("mAccelerationTimeMouse", BinPropertyType.Float)]
        public float AccelerationTimeMouse { get; set; } = 0.10000000149011612f;
        [MetaProperty("mDecelerationTimeKeyboard", BinPropertyType.Float)]
        public float DecelerationTimeKeyboard { get; set; } = 0.10000000149011612f;
        [MetaProperty("mZoomMaxDistance", BinPropertyType.Float)]
        public float ZoomMaxDistance { get; set; } = 2250f;
        [MetaProperty(4150359381, BinPropertyType.Embedded)]
        public MetaEmbedded<CameraTrapezoid> m4150359381 { get; set; } = new (new ());
        [MetaProperty("mAccelerationTimeKeyboard", BinPropertyType.Float)]
        public float AccelerationTimeKeyboard { get; set; } = 0.10000000149011612f;
        [MetaProperty("mZoomMinDistance", BinPropertyType.Float)]
        public float ZoomMinDistance { get; set; } = 1000f;
    }
    [MetaClass("TFTTraitContributionData")]
    public class TFTTraitContributionData : IMetaClass
    {
        [MetaProperty("TraitData", BinPropertyType.ObjectLink)]
        public MetaObjectLink TraitData { get; set; } = new(0);
        [MetaProperty(2836412405, BinPropertyType.Bool)]
        public bool m2836412405 { get; set; } = true;
        [MetaProperty("Amount", BinPropertyType.Int32)]
        public int Amount { get; set; } = 1;
    }
    [MetaClass("MessageBoxDialog")]
    public class MessageBoxDialog : Class0x75259ad3
    {
        [MetaProperty("ConfirmButtonIcons", BinPropertyType.Hash)]
        public MetaHash ConfirmButtonIcons { get; set; } = new(0);
        [MetaProperty(2154483994, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2154483994 { get; set; } = new (new ());
        [MetaProperty("CancelButtonIcons", BinPropertyType.Hash)]
        public MetaHash CancelButtonIcons { get; set; } = new(0);
    }
    [MetaClass("ColorGraphMaterialDriver")]
    public class ColorGraphMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; } = null;
        [MetaProperty("colors", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedColorVariableData> Colors { get; set; } = new (new ());
    }
    [MetaClass(3773238524)]
    public class Class0xe0e70cfc : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
    }
    [MetaClass("HudTeamFightData")]
    public class HudTeamFightData : IMetaClass
    {
        [MetaProperty("mStyleFlags", BinPropertyType.UInt32)]
        public uint StyleFlags { get; set; } = 0;
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; } = new (new ());
        [MetaProperty(2808220806, BinPropertyType.Structure)]
        public HudTeamFightOffScreenDifferentiationData m2808220806 { get; set; } = null;
    }
    [MetaClass(3787165435)]
    public class Class0xe1bb8efb : MapAction
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float StartTime { get; set; } = 0f;
    }
    [MetaClass("MaterialTextureData")]
    public class MaterialTextureData : IMetaClass
    {
        [MetaProperty("addressV", BinPropertyType.Byte)]
        public byte AddressV { get; set; } = 0;
        [MetaProperty("addressU", BinPropertyType.Byte)]
        public byte AddressU { get; set; } = 0;
        [MetaProperty("filterMin", BinPropertyType.Byte)]
        public byte FilterMin { get; set; } = 2;
        [MetaProperty("filterMip", BinPropertyType.Byte)]
        public byte FilterMip { get; set; } = 0;
        [MetaProperty("defaultTexturePath", BinPropertyType.String)]
        public string DefaultTexturePath { get; set; } = "";
        [MetaProperty("filterMag", BinPropertyType.Byte)]
        public byte FilterMag { get; set; } = 2;
        [MetaProperty("addressW", BinPropertyType.Byte)]
        public byte AddressW { get; set; } = 0;
    }
    [MetaClass(3792429246)]
    public class Class0xe20be0be : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
    }
    [MetaClass("StaticMaterialSwitchDef")]
    public class StaticMaterialSwitchDef : IMetaClass
    {
        [MetaProperty("on", BinPropertyType.Bool)]
        public bool On { get; set; } = true;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
    }
    [MetaClass("TargeterDefinitionAoe")]
    public class TargeterDefinitionAoe : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("textureOrientation", BinPropertyType.UInt32)]
        public uint TextureOrientation { get; set; } = 0;
        [MetaProperty(717991947, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x29dfd7ad> m717991947 { get; set; } = new (new ());
        [MetaProperty("textureRadiusOverrideName", BinPropertyType.String)]
        public string TextureRadiusOverrideName { get; set; } = "";
        [MetaProperty("constraintRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> ConstraintRange { get; set; } = new (new ());
        [MetaProperty("isConstrainedToRange", BinPropertyType.Bool)]
        public bool IsConstrainedToRange { get; set; } = false;
        [MetaProperty("constraintPosLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> ConstraintPosLocator { get; set; } = new (new ());
        [MetaProperty("overrideRadius", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideRadius { get; set; } = new (new ());
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; } = new (new ());
    }
    [MetaClass(3798132414)]
    public class Class0xe262e6be : IMetaClass
    {
        [MetaProperty("OnSelectionEvent", BinPropertyType.String)]
        public string OnSelectionEvent { get; set; } = "";
    }
    [MetaClass("VfxPrimitiveBase")]
    public interface VfxPrimitiveBase : IMetaClass
    {
    }
    [MetaClass("CollectiblesEsportsTeamData")]
    public class CollectiblesEsportsTeamData : IMetaClass
    {
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string LeagueName { get; set; } = "";
        [MetaProperty("teamId", BinPropertyType.UInt32)]
        public uint TeamId { get; set; } = 0;
        [MetaProperty("shortName", BinPropertyType.String)]
        public string ShortName { get; set; } = "";
        [MetaProperty("fullName", BinPropertyType.String)]
        public string FullName { get; set; } = "";
    }
    [MetaClass("GameFontDescription")]
    public class GameFontDescription : IMetaClass
    {
        [MetaProperty("resolutionData", BinPropertyType.ObjectLink)]
        public MetaObjectLink ResolutionData { get; set; } = new(0);
        [MetaProperty("outlineColor", BinPropertyType.Color)]
        public Color OutlineColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("glowColor", BinPropertyType.Color)]
        public Color GlowColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("colorblindGlowColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindGlowColor { get; set; } = new MetaOptional<Color>(default(Color), false);
        [MetaProperty(1019849749, BinPropertyType.Color)]
        public Color m1019849749 { get; set; } = new Color(255f, 0f, 0f, 255f);
        [MetaProperty("color", BinPropertyType.Color)]
        public Color Color { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("shadowColor", BinPropertyType.Color)]
        public Color ShadowColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("typeData", BinPropertyType.ObjectLink)]
        public MetaObjectLink TypeData { get; set; } = new(0);
        [MetaProperty("colorblindOutlineColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindOutlineColor { get; set; } = new MetaOptional<Color>(default(Color), false);
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("fillTextureName", BinPropertyType.String)]
        public string FillTextureName { get; set; } = "";
        [MetaProperty("colorblindShadowColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindShadowColor { get; set; } = new MetaOptional<Color>(default(Color), false);
        [MetaProperty(3254815611, BinPropertyType.Optional)]
        public MetaOptional<Color> m3254815611 { get; set; } = new MetaOptional<Color>(default(Color), false);
        [MetaProperty("colorblindColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindColor { get; set; } = new MetaOptional<Color>(default(Color), false);
    }
    [MetaClass("IVFXSpawnConditions")]
    public interface IVFXSpawnConditions : IMetaClass
    {
    }
    [MetaClass(3814179094)]
    public class Class0xe357c116 : MissileBehaviorSpec
    {
    }
    [MetaClass("MinimapPingTypeContainer")]
    public class MinimapPingTypeContainer : IMetaClass
    {
        [MetaProperty("pingEffectList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MinimapPingEffectAndTextureData>> PingEffectList { get; set; } = new();
    }
    [MetaClass("RegaliaRankedCrestEntry")]
    public class RegaliaRankedCrestEntry : IMetaClass
    {
        [MetaProperty("base", BinPropertyType.ObjectLink)]
        public MetaObjectLink Base { get; set; } = new(0);
        [MetaProperty(2939033354, BinPropertyType.Map)]
        public Dictionary<int, MetaObjectLink> m2939033354 { get; set; } = new();
        [MetaProperty(4222747664, BinPropertyType.Map)]
        public Dictionary<int, MetaObjectLink> m4222747664 { get; set; } = new();
    }
    [MetaClass("SwitchScriptBlock")]
    public class SwitchScriptBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("Cases", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SwitchCase>> Cases { get; set; } = new();
    }
    [MetaClass("ContextualConditionItemVOGroup")]
    public class ContextualConditionItemVOGroup : IContextualCondition
    {
        [MetaProperty("mItemVOGroupHash", BinPropertyType.Hash)]
        public MetaHash ItemVOGroupHash { get; set; } = new(0);
    }
    [MetaClass("VfxPrimitiveProjectionBase")]
    public interface VfxPrimitiveProjectionBase : VfxPrimitiveBase
    {
        [MetaProperty("mProjection", BinPropertyType.Embedded)]
        MetaEmbedded<VfxProjectionDefinitionData> Projection { get; set; }
    }
    [MetaClass("HasBuffData")]
    public class HasBuffData : IMetaClass
    {
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string BuffName { get; set; } = "";
        [MetaProperty("mFromAnyone", BinPropertyType.Bool)]
        public bool FromAnyone { get; set; } = false;
        [MetaProperty("mFromOwner", BinPropertyType.Bool)]
        public bool FromOwner { get; set; } = false;
        [MetaProperty("mFromAttacker", BinPropertyType.Bool)]
        public bool FromAttacker { get; set; } = false;
    }
    [MetaClass("AbilityResourceStateColorOptions")]
    public class AbilityResourceStateColorOptions : IMetaClass
    {
        [MetaProperty("color", BinPropertyType.Color)]
        public Color Color { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("fadeColor", BinPropertyType.Color)]
        public Color FadeColor { get; set; } = new Color(255f, 255f, 255f, 255f);
    }
    [MetaClass("ItemShopViewController")]
    public class ItemShopViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("MinimumShopScale", BinPropertyType.Float)]
        public float MinimumShopScale { get; set; } = 0.75f;
        [MetaProperty("ResizeDragRegion", BinPropertyType.Hash)]
        public MetaHash ResizeDragRegion { get; set; } = new(0);
        [MetaProperty("MaximumShopScale", BinPropertyType.Float)]
        public float MaximumShopScale { get; set; } = 1.5f;
        [MetaProperty("DragRegion", BinPropertyType.Hash)]
        public MetaHash DragRegion { get; set; } = new(0);
    }
    [MetaClass("ClearTargetAndKeepMoving")]
    public class ClearTargetAndKeepMoving : MissileTriggeredActionSpec
    {
        [MetaProperty("mOverrideHeightAugment", BinPropertyType.Optional)]
        public MetaOptional<float> OverrideHeightAugment { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mOverrideRange", BinPropertyType.Optional)]
        public MetaOptional<float> OverrideRange { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("mOverrideMovement", BinPropertyType.Structure)]
        public MissileMovementSpec OverrideMovement { get; set; } = null;
    }
    [MetaClass("TftShopData")]
    public class TftShopData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mRarity", BinPropertyType.Byte)]
        public byte Rarity { get; set; } = 0;
        [MetaProperty("mMobileSmallIconPath", BinPropertyType.String)]
        public string MobileSmallIconPath { get; set; } = "";
        [MetaProperty("mMobileIconPath", BinPropertyType.String)]
        public string MobileIconPath { get; set; } = "";
        [MetaProperty("mPortraitIconPath", BinPropertyType.String)]
        public string PortraitIconPath { get; set; } = "";
        [MetaProperty("mAbilityNameTra", BinPropertyType.String)]
        public string AbilityNameTra { get; set; } = "";
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string IconPath { get; set; } = "";
        [MetaProperty("mDescriptionTra", BinPropertyType.String)]
        public string DescriptionTra { get; set; } = "";
        [MetaProperty("mDisplayNameTra", BinPropertyType.String)]
        public string DisplayNameTra { get; set; } = "";
    }
    [MetaClass("ItemDataBuild")]
    public class ItemDataBuild : IMetaClass
    {
        [MetaProperty("itemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemLinks { get; set; } = new();
    }
    [MetaClass("MapAlternateAsset")]
    public class MapAlternateAsset : IMetaClass
    {
        [MetaProperty(428283609, BinPropertyType.String)]
        public string m428283609 { get; set; } = "";
        [MetaProperty(1613837496, BinPropertyType.String)]
        public string m1613837496 { get; set; } = "";
        [MetaProperty("mParticleResourceResolver", BinPropertyType.ObjectLink)]
        public MetaObjectLink ParticleResourceResolver { get; set; } = new(0);
        [MetaProperty(2538024013, BinPropertyType.Hash)]
        public MetaHash m2538024013 { get; set; } = new(0);
        [MetaProperty("mGrassTintTextureName", BinPropertyType.String)]
        public string GrassTintTextureName { get; set; } = "";
    }
    [MetaClass("PerkStyle")]
    public class PerkStyle : IMetaClass
    {
        [MetaProperty("mPingTextLocalizationKey", BinPropertyType.String)]
        public string PingTextLocalizationKey { get; set; } = "";
        [MetaProperty("mDisplayNameLocalizationKey", BinPropertyType.String)]
        public string DisplayNameLocalizationKey { get; set; } = "";
        [MetaProperty("mTooltipNameLocalizationKey", BinPropertyType.String)]
        public string TooltipNameLocalizationKey { get; set; } = "";
        [MetaProperty("mScript", BinPropertyType.Structure)]
        public PerkScript Script { get; set; } = null;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("mDefaultPerksWhenSplashed", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> DefaultPerksWhenSplashed { get; set; } = new();
        [MetaProperty("mIconTextureName", BinPropertyType.String)]
        public string IconTextureName { get; set; } = "";
        [MetaProperty("mSubStyleBonus", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<PerkSubStyleBonus>> SubStyleBonus { get; set; } = new();
        [MetaProperty("mPerkStyleName", BinPropertyType.String)]
        public string PerkStyleName { get; set; } = "";
        [MetaProperty("mScriptAsSubStyle", BinPropertyType.Structure)]
        public PerkScript ScriptAsSubStyle { get; set; } = null;
        [MetaProperty("mDefaultPageLocalizationKey", BinPropertyType.String)]
        public string DefaultPageLocalizationKey { get; set; } = "";
        [MetaProperty("mAllowedSubStyles", BinPropertyType.Container)]
        public MetaContainer<uint> AllowedSubStyles { get; set; } = new();
        [MetaProperty("mSlots", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<PerkSlot>> Slots { get; set; } = new();
        [MetaProperty("mPerkStyleId", BinPropertyType.UInt32)]
        public uint PerkStyleId { get; set; } = 0;
        [MetaProperty("mSlotlinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Slotlinks { get; set; } = new();
        [MetaProperty("mIsAdvancedStyle", BinPropertyType.Bool)]
        public bool IsAdvancedStyle { get; set; } = false;
        [MetaProperty("mDefaultSplash", BinPropertyType.Structure)]
        public DefaultSplashedPerkStyle DefaultSplash { get; set; } = null;
        [MetaProperty("mStyleVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver StyleVFXResourceResolver { get; set; } = null;
        [MetaProperty("mLCUAssetFileMap", BinPropertyType.Map)]
        public Dictionary<string, string> LCUAssetFileMap { get; set; } = new();
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        public MetaContainer<PerkBuff> Buffs { get; set; } = new();
        [MetaProperty(262465954, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DefaultStatModPerkSet>> m262465954 { get; set; } = new();
    }
    [MetaClass("ContextualConditionSituationHasRecentlyRun")]
    public class ContextualConditionSituationHasRecentlyRun : IContextualCondition
    {
        [MetaProperty("mSituationNameHash", BinPropertyType.Hash)]
        public MetaHash SituationNameHash { get; set; } = new(0);
        [MetaProperty("mTime", BinPropertyType.Float)]
        public float Time { get; set; } = 0f;
    }
    [MetaClass("MapNavigationGridOverlays")]
    public class MapNavigationGridOverlays : IMetaClass
    {
        [MetaProperty("overlays", BinPropertyType.Map)]
        public Dictionary<string, MetaObjectLink> Overlays { get; set; } = new();
    }
    [MetaClass("VfxFieldDragDefinitionData")]
    public class VfxFieldDragDefinitionData : IMetaClass
    {
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Position { get; set; } = new (new ());
        [MetaProperty("radius", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Radius { get; set; } = new (new ());
        [MetaProperty("strength", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Strength { get; set; } = new (new ());
    }
    [MetaClass("MaterialInstanceDef")]
    public class MaterialInstanceDef : IResource,  IMaterialDef
    {
        [MetaProperty("params", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialInstanceParamDef>> Params { get; set; } = new();
        [MetaProperty("DynamicTextures", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialInstanceDynamicTexture>> DynamicTextures { get; set; } = new();
        [MetaProperty("DynamicSwitch", BinPropertyType.Embedded)]
        public MetaEmbedded<MaterialInstanceDynamicSwitch> DynamicSwitch { get; set; } = new (new ());
        [MetaProperty("DynamicParams", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialInstanceDynamicParam>> DynamicParams { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("BaseMaterial", BinPropertyType.ObjectLink)]
        public MetaObjectLink BaseMaterial { get; set; } = new(0);
        [MetaProperty("childTechniques", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class0xf7084b4a>> ChildTechniques { get; set; } = new();
        [MetaProperty("textures", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialInstanceTextureDef>> Textures { get; set; } = new();
        [MetaProperty("DynamicSwitchId", BinPropertyType.UInt16)]
        public ushort DynamicSwitchId { get; set; } = 0;
        [MetaProperty("switches", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialInstanceSwitchDef>> Switches { get; set; } = new();
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; } = new();
    }
    [MetaClass("UnitStatusPriorityList")]
    public class UnitStatusPriorityList : IMetaClass
    {
        [MetaProperty("mMinimumDisplayTime", BinPropertyType.Float)]
        public float MinimumDisplayTime { get; set; } = 0.125f;
        [MetaProperty("mPrioritizedUnitStatusData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<UnitStatusData>> PrioritizedUnitStatusData { get; set; } = new();
    }
    [MetaClass("LaneData")]
    public class LaneData : IMetaClass
    {
        [MetaProperty("mNavigationPoints", BinPropertyType.Container)]
        public MetaContainer<string> NavigationPoints { get; set; } = new();
        [MetaProperty("mContainedRegions", BinPropertyType.Container)]
        public MetaContainer<string> ContainedRegions { get; set; } = new();
    }
    [MetaClass("DamageSourceTemplate")]
    public class DamageSourceTemplate : IMetaClass
    {
        [MetaProperty("DamageTags", BinPropertyType.Container)]
        public MetaContainer<string> DamageTags { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("DamageProperties", BinPropertyType.UInt32)]
        public uint DamageProperties { get; set; } = 0;
    }
    [MetaClass("VerticalFacingType")]
    public interface VerticalFacingType : IMetaClass
    {
    }
    [MetaClass("HudColorData")]
    public class HudColorData : IMetaClass
    {
        [MetaProperty("mSelectedIndicatorColor", BinPropertyType.Color)]
        public Color SelectedIndicatorColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mEnemyChatColor", BinPropertyType.Color)]
        public Color EnemyChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mClubTagNeutralChatColor", BinPropertyType.Color)]
        public Color ClubTagNeutralChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mClubTagChaosChatColor", BinPropertyType.Color)]
        public Color ClubTagChaosChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mOrderColor", BinPropertyType.Color)]
        public Color OrderColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(619402659, BinPropertyType.Color)]
        public Color m619402659 { get; set; } = new Color(255f, 0f, 0f, 255f);
        [MetaProperty("mNeutralChatColor", BinPropertyType.Color)]
        public Color NeutralChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mEnemyColor", BinPropertyType.Color)]
        public Color EnemyColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(788705121, BinPropertyType.Color)]
        public Color m788705121 { get; set; } = new Color(0f, 255f, 0f, 255f);
        [MetaProperty("mPingChatColor", BinPropertyType.Color)]
        public Color PingChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(818028930, BinPropertyType.Color)]
        public Color m818028930 { get; set; } = new Color(0f, 230f, 255f, 255f);
        [MetaProperty("mSummonerNameDeadColor", BinPropertyType.Color)]
        public Color SummonerNameDeadColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mOrderChatColor", BinPropertyType.Color)]
        public Color OrderChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mTeamChatColor", BinPropertyType.Color)]
        public Color TeamChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(996466659, BinPropertyType.Color)]
        public Color m996466659 { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty(63589972, BinPropertyType.Color)]
        public Color m63589972 { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mSpellHotKeyEnabledColor", BinPropertyType.Color)]
        public Color SpellHotKeyEnabledColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(1063841720, BinPropertyType.Color)]
        public Color m1063841720 { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mJunglePlantColor", BinPropertyType.Color)]
        public Color JunglePlantColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mInputChatColor", BinPropertyType.Color)]
        public Color InputChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mSpellHotKeyDisabledColor", BinPropertyType.Color)]
        public Color SpellHotKeyDisabledColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("VoteEmptyColor", BinPropertyType.Color)]
        public Color VoteEmptyColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mSelfColor", BinPropertyType.Color)]
        public Color SelfColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mLevelUpColor", BinPropertyType.Color)]
        public Color LevelUpColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(1371618007, BinPropertyType.Color)]
        public Color m1371618007 { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mVoiceChatHaloTextureColor", BinPropertyType.Color)]
        public Color VoiceChatHaloTextureColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mEvolutionColor", BinPropertyType.Color)]
        public Color EvolutionColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mDeathFriendlyTeamColor", BinPropertyType.Color)]
        public Color DeathFriendlyTeamColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mItemCalloutBodyColor", BinPropertyType.Color)]
        public Color ItemCalloutBodyColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mPlatformChatColor", BinPropertyType.Color)]
        public Color PlatformChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(1730989398, BinPropertyType.Color)]
        public Color m1730989398 { get; set; } = new Color(86f, 90f, 91f, 255f);
        [MetaProperty("mItemHotKeyDisabledColor", BinPropertyType.Color)]
        public Color ItemHotKeyDisabledColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mVoiceChatHoverTextColor", BinPropertyType.Color)]
        public Color VoiceChatHoverTextColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mChaosChatColor", BinPropertyType.Color)]
        public Color ChaosChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mSummonerNameDefaultColor", BinPropertyType.Color)]
        public Color SummonerNameDefaultColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mItemCalloutItemColor", BinPropertyType.Color)]
        public Color ItemCalloutItemColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(2467755617, BinPropertyType.Container)]
        public MetaContainer<Color> m2467755617 { get; set; } = new();
        [MetaProperty(2491908235, BinPropertyType.Color)]
        public Color m2491908235 { get; set; } = new Color(210f, 230f, 240f, 255f);
        [MetaProperty("mStatBoostedColor", BinPropertyType.Color)]
        public Color StatBoostedColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mClubTagEnemyChatColor", BinPropertyType.Color)]
        public Color ClubTagEnemyChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mDeathChaosColor", BinPropertyType.Color)]
        public Color DeathChaosColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mAllChatColor", BinPropertyType.Color)]
        public Color AllChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mHighlightedIndicatorColor", BinPropertyType.Color)]
        public Color HighlightedIndicatorColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mMarkedIndicatorColor", BinPropertyType.Color)]
        public Color MarkedIndicatorColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mClubTagOrderChatColor", BinPropertyType.Color)]
        public Color ClubTagOrderChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mTimestampChatColor", BinPropertyType.Color)]
        public Color TimestampChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mWhisperColor", BinPropertyType.Color)]
        public Color WhisperColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("VoteNocolor", BinPropertyType.Color)]
        public Color VoteNocolor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mShadowChatColor", BinPropertyType.Color)]
        public Color ShadowChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mFeedbackChatColor", BinPropertyType.Color)]
        public Color FeedbackChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(3231501784, BinPropertyType.Color)]
        public Color m3231501784 { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty(3243078108, BinPropertyType.Color)]
        public Color m3243078108 { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mEnemyLaneMinionBarColor", BinPropertyType.Color)]
        public Color EnemyLaneMinionBarColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mNetworkChatColor", BinPropertyType.Color)]
        public Color NetworkChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(3390937202, BinPropertyType.Color)]
        public Color m3390937202 { get; set; } = new Color(0f, 0f, 255f, 255f);
        [MetaProperty(3449599685, BinPropertyType.Color)]
        public Color m3449599685 { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mFriendlyColor", BinPropertyType.Color)]
        public Color FriendlyColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(3591678551, BinPropertyType.Color)]
        public Color m3591678551 { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mFriendlyChatColor", BinPropertyType.Color)]
        public Color FriendlyChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mClubTagFriendlyChatColor", BinPropertyType.Color)]
        public Color ClubTagFriendlyChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(3755603872, BinPropertyType.Color)]
        public Color m3755603872 { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mVoiceChatDefaultTextColor", BinPropertyType.Color)]
        public Color VoiceChatDefaultTextColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mSummonerNameSelfColor", BinPropertyType.Color)]
        public Color SummonerNameSelfColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mPartyChatColor", BinPropertyType.Color)]
        public Color PartyChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(4003080326, BinPropertyType.Color)]
        public Color m4003080326 { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("mDeathOrderColor", BinPropertyType.Color)]
        public Color DeathOrderColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mNeutralColor", BinPropertyType.Color)]
        public Color NeutralColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mGoldChatColor", BinPropertyType.Color)]
        public Color GoldChatColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mItemHotKeyEnabledColor", BinPropertyType.Color)]
        public Color ItemHotKeyEnabledColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mChaosColor", BinPropertyType.Color)]
        public Color ChaosColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mDeathEnemyTeamColor", BinPropertyType.Color)]
        public Color DeathEnemyTeamColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty(264529986, BinPropertyType.Color)]
        public Color m264529986 { get; set; } = new Color(140f, 155f, 160f, 255f);
        [MetaProperty("mStatNormalColor", BinPropertyType.Color)]
        public Color StatNormalColor { get; set; } = new Color(0f, 0f, 0f, 255f);
        [MetaProperty("mFriendlyLaneMinionBarColor", BinPropertyType.Color)]
        public Color FriendlyLaneMinionBarColor { get; set; } = new Color(0f, 0f, 0f, 255f);
    }
    [MetaClass(3897992344)]
    public class Class0xe856a498 : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string KeyName { get; set; } = "";
    }
    [MetaClass("ItemRecommendationOverrideContext")]
    public class ItemRecommendationOverrideContext : IMetaClass
    {
        [MetaProperty(934764380, BinPropertyType.Hash)]
        public MetaHash m934764380 { get; set; } = new(0);
        [MetaProperty("mPosition", BinPropertyType.Hash)]
        public MetaHash Position { get; set; } = new(0);
        [MetaProperty("mMapID", BinPropertyType.UInt32)]
        public uint MapID { get; set; } = 0;
    }
    [MetaClass("IResourceResolver")]
    public interface IResourceResolver : IMetaClass
    {
    }
    [MetaClass("ClipBaseData")]
    public interface ClipBaseData : IMetaClass
    {
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        uint Flags { get; set; }
    }
    [MetaClass("VfxPrimitivePlanarProjection")]
    public class VfxPrimitivePlanarProjection : VfxPrimitiveProjectionBase
    {
        [MetaProperty("mProjection", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxProjectionDefinitionData> Projection { get; set; } = new (new ());
    }
    [MetaClass("FontLocaleResolutions")]
    public class FontLocaleResolutions : IMetaClass
    {
        [MetaProperty("localeName", BinPropertyType.String)]
        public string LocaleName { get; set; } = "en_us";
        [MetaProperty("resolutions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FontResolution>> Resolutions { get; set; } = new();
    }
    [MetaClass("CurveTheDifferenceHeightSolver")]
    public class CurveTheDifferenceHeightSolver : HeightSolverType
    {
        [MetaProperty("mInitialTargetHeightOffset", BinPropertyType.Float)]
        public float InitialTargetHeightOffset { get; set; } = 0f;
    }
    [MetaClass("HudOptionalBinData")]
    public class HudOptionalBinData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mPriority", BinPropertyType.UInt32)]
        public uint Priority { get; set; } = 0;
    }
    [MetaClass("MaterialParameterDataCollection")]
    public class MaterialParameterDataCollection : IMetaClass
    {
        [MetaProperty("Entries", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<IdMappingEntry>> Entries { get; set; } = new();
        [MetaProperty("nextID", BinPropertyType.UInt16)]
        public ushort NextID { get; set; } = 1;
        [MetaProperty("data", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialParameterData>> Data { get; set; } = new();
        [MetaProperty(3931619090, BinPropertyType.String)]
        public string m3931619090 { get; set; } = "";
    }
    [MetaClass("GameCalculationConditional")]
    public class GameCalculationConditional : IGameCalculation
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        public IGameCalculationPart Multiplier { get; set; } = null;
        [MetaProperty(923208333, BinPropertyType.Byte)]
        public byte m923208333 { get; set; } = 8;
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        public byte m3419063832 { get; set; } = 8;
        [MetaProperty("tooltipOnly", BinPropertyType.Bool)]
        public bool TooltipOnly { get; set; } = false;
        [MetaProperty(3874405167, BinPropertyType.Byte)]
        public byte m3874405167 { get; set; } = 8;
        [MetaProperty("mDefaultGameCalculation", BinPropertyType.Hash)]
        public MetaHash DefaultGameCalculation { get; set; } = new(0);
        [MetaProperty("mConditionalGameCalculation", BinPropertyType.Hash)]
        public MetaHash ConditionalGameCalculation { get; set; } = new(0);
        [MetaProperty(3225953125, BinPropertyType.Structure)]
        public ICastRequirement m3225953125 { get; set; } = null;
    }
    [MetaClass(3925560600)]
    public class Class0xe9fb4d18 : IGameCalculationPart
    {
        [MetaProperty(662912460, BinPropertyType.Byte)]
        public byte m662912460 { get; set; } = 0;
        [MetaProperty(1180173034, BinPropertyType.Byte)]
        public byte m1180173034 { get; set; } = 0;
        [MetaProperty(1634084804, BinPropertyType.Structure)]
        public IGameCalculationPart m1634084804 { get; set; } = null;
    }
    [MetaClass(3929150294)]
    public class Class0xea321356 : IMetaClass
    {
        [MetaProperty("Column2LabelTraKey", BinPropertyType.String)]
        public string Column2LabelTraKey { get; set; } = "";
        [MetaProperty("Column0LabelTraKey", BinPropertyType.String)]
        public string Column0LabelTraKey { get; set; } = "";
        [MetaProperty("Column1LabelTraKey", BinPropertyType.String)]
        public string Column1LabelTraKey { get; set; } = "";
    }
    [MetaClass("LoadoutFeatureData")]
    public class LoadoutFeatureData : IMetaClass
    {
        [MetaProperty("mLoadoutCategory", BinPropertyType.String)]
        public string LoadoutCategory { get; set; } = "";
        [MetaProperty("mBinaryFile", BinPropertyType.Optional)]
        public MetaOptional<string> BinaryFile { get; set; } = new MetaOptional<string>(default(string), false);
        [MetaProperty("mLoadoutProperties", BinPropertyType.Container)]
        public MetaContainer<string> LoadoutProperties { get; set; } = new();
        [MetaProperty("mGDSObjectPathTemplates", BinPropertyType.Map)]
        public Dictionary<string, string> GDSObjectPathTemplates { get; set; } = new();
        [MetaProperty("mMutator", BinPropertyType.Optional)]
        public MetaOptional<string> Mutator { get; set; } = new MetaOptional<string>(default(string), false);
        [MetaProperty("mFeature", BinPropertyType.UInt32)]
        public uint Feature { get; set; } = 13;
        [MetaProperty("mLoadFromContentIds", BinPropertyType.Bool)]
        public bool LoadFromContentIds { get; set; } = true;
    }
    [MetaClass("Champion")]
    public class Champion : WadFileDescriptor
    {
        [MetaProperty("mChampionItemRecommendations", BinPropertyType.Embedded)]
        public MetaEmbedded<ChampionItemRecommendations> ChampionItemRecommendations { get; set; } = new (new ());
        [MetaProperty("statStoneSets", BinPropertyType.Container)]
        public MetaContainer<MetaHash> StatStoneSets { get; set; } = new();
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(3352862803, BinPropertyType.UInt32)]
        public uint m3352862803 { get; set; } = 7;
        [MetaProperty("fixedLoadScreenPosition", BinPropertyType.SByte)]
        public sbyte FixedLoadScreenPosition { get; set; } = -1;
        [MetaProperty("additionalCharacters", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> AdditionalCharacters { get; set; } = new();
    }
    [MetaClass("SelfAoe")]
    public class SelfAoe : TargetingTypeData
    {
    }
    [MetaClass("ContextualConditionSpellIsReady")]
    public class ContextualConditionSpellIsReady : IContextualConditionSpell
    {
        [MetaProperty("mSpellIsReady", BinPropertyType.Bool)]
        public bool SpellIsReady { get; set; } = false;
    }
    [MetaClass("CreateCustomTableBlock")]
    public class CreateCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool IsDisabled { get; set; } = false;
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableSet> CustomTable { get; set; } = new (new ());
    }
    [MetaClass(3939611513)]
    public class Class0xead1b379 : IMetaClass
    {
        [MetaProperty(4215291610, BinPropertyType.Map)]
        public Dictionary<uint, MetaObjectLink> m4215291610 { get; set; } = new();
    }
    [MetaClass("HudFeedbackDamageData")]
    public class HudFeedbackDamageData : IMetaClass
    {
        [MetaProperty("mOverTimeForFlashSeconds", BinPropertyType.Float)]
        public float OverTimeForFlashSeconds { get; set; } = 0f;
        [MetaProperty("mMaxPercentageForMostReadHealth", BinPropertyType.Float)]
        public float MaxPercentageForMostReadHealth { get; set; } = 1f;
        [MetaProperty("mLowHealthFlashOpacityStrength", BinPropertyType.Float)]
        public float LowHealthFlashOpacityStrength { get; set; } = 0.800000011920929f;
        [MetaProperty("mLowHealthFlashDuration", BinPropertyType.Float)]
        public float LowHealthFlashDuration { get; set; } = 1.2000000476837158f;
        [MetaProperty("mStartFlashAlpha", BinPropertyType.Float)]
        public float StartFlashAlpha { get; set; } = 1f;
        [MetaProperty("mPercentageDamageForFlash", BinPropertyType.Float)]
        public float PercentageDamageForFlash { get; set; } = 1f;
        [MetaProperty("mLowHealthFlashThresholdPercentage", BinPropertyType.Float)]
        public float LowHealthFlashThresholdPercentage { get; set; } = 0.6000000238418579f;
        [MetaProperty("mFlashDuration", BinPropertyType.Float)]
        public float FlashDuration { get; set; } = 1f;
    }
    [MetaClass("UiComboBoxDefinition")]
    public class UiComboBoxDefinition : IMetaClass
    {
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash ObjectPath { get; set; } = new(0);
        [MetaProperty(566876281, BinPropertyType.ObjectLink)]
        public MetaObjectLink m566876281 { get; set; } = new(0);
        [MetaProperty(863728340, BinPropertyType.ObjectLink)]
        public MetaObjectLink m863728340 { get; set; } = new(0);
        [MetaProperty(55079458, BinPropertyType.ObjectLink)]
        public MetaObjectLink m55079458 { get; set; } = new(0);
        [MetaProperty(2621931938, BinPropertyType.ObjectLink)]
        public MetaObjectLink m2621931938 { get; set; } = new(0);
        [MetaProperty("soundEvents", BinPropertyType.Structure)]
        public Class0xe262e6be SoundEvents { get; set; } = null;
        [MetaProperty(2997075516, BinPropertyType.ObjectLink)]
        public MetaObjectLink m2997075516 { get; set; } = new(0);
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
        [MetaProperty(246593150, BinPropertyType.Byte)]
        public byte m246593150 { get; set; } = 0;
    }
    [MetaClass("PurchaseDialog")]
    public class PurchaseDialog : Class0x75259ad3
    {
        [MetaProperty("purchaseButton", BinPropertyType.Hash)]
        public MetaHash PurchaseButton { get; set; } = new(0);
        [MetaProperty("moreInfoButton", BinPropertyType.Hash)]
        public MetaHash MoreInfoButton { get; set; } = new(0);
        [MetaProperty(2297864248, BinPropertyType.Hash)]
        public MetaHash m2297864248 { get; set; } = new(0);
    }
    [MetaClass("SummonerNameCreateViewController")]
    public class SummonerNameCreateViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty(697499175, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m697499175 { get; set; } = new (new ());
        [MetaProperty("SubmitButtonDefinition", BinPropertyType.Hash)]
        public MetaHash SubmitButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("UpdaterData")]
    public class UpdaterData : IMetaClass
    {
        [MetaProperty("mOutputType", BinPropertyType.UInt32)]
        public uint OutputType { get; set; } = 4294967295;
        [MetaProperty("mInputType", BinPropertyType.UInt32)]
        public uint InputType { get; set; } = 4294967295;
        [MetaProperty("mValueProcessorDataList", BinPropertyType.Container)]
        public MetaContainer<ValueProcessorData> ValueProcessorDataList { get; set; } = new();
    }
    [MetaClass("CharacterToolData")]
    public class CharacterToolData : IMetaClass
    {
        [MetaProperty("searchTagsSecondary", BinPropertyType.String)]
        public string SearchTagsSecondary { get; set; } = "";
        [MetaProperty("searchTags", BinPropertyType.String)]
        public string SearchTags { get; set; } = "";
    }
    [MetaClass("TftTeamPlannerViewController")]
    public class TftTeamPlannerViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("ActiveTraitButtonTemplate", BinPropertyType.Hash)]
        public MetaHash ActiveTraitButtonTemplate { get; set; } = new(0);
        [MetaProperty("ChampionsPerRow", BinPropertyType.Byte)]
        public byte ChampionsPerRow { get; set; } = 5;
        [MetaProperty("CloseButtonTemplate", BinPropertyType.Hash)]
        public MetaHash CloseButtonTemplate { get; set; } = new(0);
        [MetaProperty("PaginateTraitButtonTemplate", BinPropertyType.Hash)]
        public MetaHash PaginateTraitButtonTemplate { get; set; } = new(0);
        [MetaProperty(1860418862, BinPropertyType.Float)]
        public float m1860418862 { get; set; } = 5f;
        [MetaProperty("TraitsPerRow", BinPropertyType.Byte)]
        public byte TraitsPerRow { get; set; } = 6;
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty(2827571664, BinPropertyType.Hash)]
        public MetaHash m2827571664 { get; set; } = new(0);
        [MetaProperty("ChampionButtonTemplate", BinPropertyType.Hash)]
        public MetaHash ChampionButtonTemplate { get; set; } = new(0);
        [MetaProperty("ClearButtonTemplate", BinPropertyType.Hash)]
        public MetaHash ClearButtonTemplate { get; set; } = new(0);
    }
    [MetaClass("DamageUnitCheat")]
    public class DamageUnitCheat : Cheat
    {
        [MetaProperty("mDamageAmount", BinPropertyType.UInt32)]
        public uint DamageAmount { get; set; } = 0;
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint Target { get; set; } = 1;
        [MetaProperty("mPercentageOfAttack", BinPropertyType.Float)]
        public float PercentageOfAttack { get; set; } = 1f;
        [MetaProperty("mDamageType", BinPropertyType.UInt32)]
        public uint DamageType { get; set; } = 0;
        [MetaProperty("mHitResult", BinPropertyType.UInt32)]
        public uint HitResult { get; set; } = 0;
    }
    [MetaClass("ContextualConditionNeutralCampId")]
    public class ContextualConditionNeutralCampId : IContextualCondition
    {
        [MetaProperty("mCampId", BinPropertyType.Byte)]
        public byte CampId { get; set; } = 6;
    }
    [MetaClass("HasNNearbyVisibleUnitsRequirement")]
    public class HasNNearbyVisibleUnitsRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mUnitsRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> UnitsRequirements { get; set; } = new();
        [MetaProperty("mDistanceType", BinPropertyType.UInt32)]
        public uint DistanceType { get; set; } = 0;
        [MetaProperty("mRange", BinPropertyType.Float)]
        public float Range { get; set; } = 0f;
        [MetaProperty("mUnitsRequired", BinPropertyType.UInt32)]
        public uint UnitsRequired { get; set; } = 0;
    }
    [MetaClass("VeritcalFacingMatchVelocity")]
    public class VeritcalFacingMatchVelocity : VerticalFacingType
    {
    }
    [MetaClass("IMaterialDef")]
    public interface IMaterialDef : IMetaClass
    {
    }
    [MetaClass("StatStoneData")]
    public class StatStoneData : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("IsRetired", BinPropertyType.Bool)]
        public bool IsRetired { get; set; } = false;
        [MetaProperty(678414787, BinPropertyType.UInt32)]
        public uint m678414787 { get; set; } = 5;
        [MetaProperty("trackingType", BinPropertyType.Byte)]
        public byte TrackingType { get; set; } = 0;
        [MetaProperty("Milestones", BinPropertyType.Container)]
        public MetaContainer<ulong> Milestones { get; set; } = new();
        [MetaProperty("EpicStatStone", BinPropertyType.Bool)]
        public bool EpicStatStone { get; set; } = false;
        [MetaProperty("stoneName", BinPropertyType.String)]
        public string StoneName { get; set; } = "";
        [MetaProperty("TriggeredFromScript", BinPropertyType.Bool)]
        public bool TriggeredFromScript { get; set; } = false;
        [MetaProperty("EventsToTrack", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StatStoneEventToTrack>> EventsToTrack { get; set; } = new();
        [MetaProperty(2461866155, BinPropertyType.Bool)]
        public bool m2461866155 { get; set; } = false;
        [MetaProperty("category", BinPropertyType.ObjectLink)]
        public MetaObjectLink Category { get; set; } = new(0);
        [MetaProperty(4213855983, BinPropertyType.UInt32)]
        public uint m4213855983 { get; set; } = 3;
    }
    [MetaClass("ViewController")]
    public interface ViewController : IMetaClass
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        MetaObjectLink m3080488622 { get; set; }
    }
    [MetaClass("FunctionDefinition")]
    public class FunctionDefinition : IMetaClass
    {
        [MetaProperty("InputParameters", BinPropertyType.Container)]
        public MetaContainer<MetaHash> InputParameters { get; set; } = new();
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; } = new (new ());
        [MetaProperty("OutputParameters", BinPropertyType.Container)]
        public MetaContainer<MetaHash> OutputParameters { get; set; } = new();
    }
    [MetaClass("MultiKillLogic")]
    public class MultiKillLogic : IStatStoneLogicDriver
    {
    }
    [MetaClass("FloatTableGet")]
    public class FloatTableGet : IFloatGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<float> Default { get; set; } = new MetaOptional<float>(default(float), false);
    }
    [MetaClass("CSSIcon")]
    public class CSSIcon : IMetaClass
    {
        [MetaProperty("xy", BinPropertyType.Vector2)]
        public Vector2 Xy { get; set; } = new Vector2(0f, 0f);
        [MetaProperty("wh", BinPropertyType.Vector2)]
        public Vector2 Wh { get; set; } = new Vector2(20f, 20f);
        [MetaProperty(2179074287, BinPropertyType.Float)]
        public float m2179074287 { get; set; } = 0f;
    }
    [MetaClass("EsportsData")]
    public class EsportsData : IMetaClass
    {
        [MetaProperty("leagues", BinPropertyType.Container)]
        public MetaContainer<string> Leagues { get; set; } = new();
    }
    [MetaClass("TargetTypeFilter")]
    public class TargetTypeFilter : IStatStoneLogicDriver
    {
        [MetaProperty(507497828, BinPropertyType.Bool)]
        public bool m507497828 { get; set; } = true;
        [MetaProperty(1203421971, BinPropertyType.Bool)]
        public bool m1203421971 { get; set; } = true;
        [MetaProperty(3588584256, BinPropertyType.Bool)]
        public bool m3588584256 { get; set; } = false;
    }
    [MetaClass("CursorConfig")]
    public class CursorConfig : IMetaClass
    {
        [MetaProperty("mHoverNotUseableCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorDataCaptureCooldownContext> HoverNotUseableCursor { get; set; } = new (new ());
        [MetaProperty("mTeamContextCursors", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorDataTeamContext>> TeamContextCursors { get; set; } = new();
        [MetaProperty("mSingleContextCursors", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorData>> SingleContextCursors { get; set; } = new();
    }
    [MetaClass("TargeterDefinitionSkipTerrain")]
    public class TargeterDefinitionSkipTerrain : TargeterDefinition
    {
        [MetaProperty("mFadeBehavior", BinPropertyType.Structure)]
        public ITargeterFadeBehavior FadeBehavior { get; set; } = null;
        [MetaProperty("mBaseTextureName", BinPropertyType.String)]
        public string BaseTextureName { get; set; } = "";
        [MetaProperty("mTargetTextureName", BinPropertyType.String)]
        public string TargetTextureName { get; set; } = "";
        [MetaProperty("mTerrainTextureName", BinPropertyType.String)]
        public string TerrainTextureName { get; set; } = "";
        [MetaProperty("mEndLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; } = new (new ());
        [MetaProperty("mFallbackDirection", BinPropertyType.UInt32)]
        public uint FallbackDirection { get; set; } = 1;
        [MetaProperty("mTargetTextureRadius", BinPropertyType.Float)]
        public float TargetTextureRadius { get; set; } = 0f;
        [MetaProperty("mStartLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; } = new (new ());
    }
    [MetaClass("UVScaleBiasFromAnimationDynamicMaterialDriver")]
    public class UVScaleBiasFromAnimationDynamicMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mSubMeshName", BinPropertyType.String)]
        public string SubMeshName { get; set; } = "";
        [MetaProperty(3685552942, BinPropertyType.UInt32)]
        public uint m3685552942 { get; set; } = 0;
    }
    [MetaClass(3998028548)]
    public class Class0xee4d1304 : IOptionItemFilter
    {
        [MetaProperty("Mutator", BinPropertyType.String)]
        public string Mutator { get; set; } = "";
    }
    [MetaClass("EffectGlowElementData")]
    public class EffectGlowElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("BaseScale", BinPropertyType.Float)]
        public float BaseScale { get; set; } = 0f;
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool PerPixelUvsX { get; set; } = false;
        [MetaProperty("MinimumAlpha", BinPropertyType.Float)]
        public float MinimumAlpha { get; set; } = 0f;
        [MetaProperty("CycleBasedScaleAddition", BinPropertyType.Float)]
        public float CycleBasedScaleAddition { get; set; } = 0f;
        [MetaProperty("CycleTime", BinPropertyType.Float)]
        public float CycleTime { get; set; } = 0f;
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; } = null;
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool FlipY { get; set; } = false;
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool FlipX { get; set; } = false;
    }
    [MetaClass("ContextualConditionItemCanBePurchased")]
    public class ContextualConditionItemCanBePurchased : IContextualCondition
    {
        [MetaProperty("mItemCanBePurchased", BinPropertyType.Bool)]
        public bool ItemCanBePurchased { get; set; } = false;
    }
    [MetaClass("TextureAndColorData")]
    public class TextureAndColorData : IMetaClass
    {
        [MetaProperty("colorblindTextureFile", BinPropertyType.String)]
        public string ColorblindTextureFile { get; set; } = "";
        [MetaProperty("color", BinPropertyType.Color)]
        public Color Color { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("textureFile", BinPropertyType.String)]
        public string TextureFile { get; set; } = "";
        [MetaProperty("colorblindColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindColor { get; set; } = new MetaOptional<Color>(default(Color), false);
    }
    [MetaClass("TFTCompanionBucket")]
    public class TFTCompanionBucket : IMetaClass
    {
        [MetaProperty("Companions", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Companions { get; set; } = new();
    }
    [MetaClass("AISpellData")]
    public class AISpellData : IMetaClass
    {
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float Speed { get; set; } = -1f;
        [MetaProperty("mSendAIEvent", BinPropertyType.Bool)]
        public bool SendAIEvent { get; set; } = false;
        [MetaProperty("mRadius", BinPropertyType.Float)]
        public float Radius { get; set; } = -1f;
        [MetaProperty("mRange", BinPropertyType.Float)]
        public float Range { get; set; } = -1f;
        [MetaProperty("mBlockLevel", BinPropertyType.Byte)]
        public byte BlockLevel { get; set; } = 0;
        [MetaProperty("mEndOnly", BinPropertyType.Bool)]
        public bool EndOnly { get; set; } = false;
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float Lifetime { get; set; } = -1f;
    }
    [MetaClass(4010129986)]
    public class Class0xef05ba42 : IMetaClass
    {
        [MetaProperty("LockedHoverIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink LockedHoverIcon { get; set; } = new(0);
        [MetaProperty("OffsetRegion", BinPropertyType.ObjectLink)]
        public MetaObjectLink OffsetRegion { get; set; } = new(0);
        [MetaProperty("CostTextSelected", BinPropertyType.ObjectLink)]
        public MetaObjectLink CostTextSelected { get; set; } = new(0);
        [MetaProperty("PopularIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink PopularIcon { get; set; } = new(0);
        [MetaProperty(1536603069, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1536603069 { get; set; } = new(0);
        [MetaProperty("RecentlyChangedIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink RecentlyChangedIcon { get; set; } = new(0);
        [MetaProperty("MythicPurchaseableVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink MythicPurchaseableVfx { get; set; } = new(0);
        [MetaProperty(2558383518, BinPropertyType.ObjectLink)]
        public MetaObjectLink m2558383518 { get; set; } = new(0);
        [MetaProperty("MythicFrameIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink MythicFrameIcon { get; set; } = new(0);
        [MetaProperty("SelectedIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink SelectedIcon { get; set; } = new(0);
        [MetaProperty("PurchasedOverlay", BinPropertyType.ObjectLink)]
        public MetaObjectLink PurchasedOverlay { get; set; } = new(0);
        [MetaProperty("FrameIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink FrameIcon { get; set; } = new(0);
        [MetaProperty("MythicPurchasedVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink MythicPurchasedVfx { get; set; } = new(0);
        [MetaProperty("HoverFrameIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink HoverFrameIcon { get; set; } = new(0);
        [MetaProperty("LockedIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink LockedIcon { get; set; } = new(0);
        [MetaProperty("CostTextUnpurchaseable", BinPropertyType.ObjectLink)]
        public MetaObjectLink CostTextUnpurchaseable { get; set; } = new(0);
        [MetaProperty("NameText", BinPropertyType.ObjectLink)]
        public MetaObjectLink NameText { get; set; } = new(0);
        [MetaProperty("HoverIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink HoverIcon { get; set; } = new(0);
        [MetaProperty("ItemIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink ItemIcon { get; set; } = new(0);
        [MetaProperty("SelectedVfx", BinPropertyType.ObjectLink)]
        public MetaObjectLink SelectedVfx { get; set; } = new(0);
        [MetaProperty("UnpurchaseableOverlay", BinPropertyType.ObjectLink)]
        public MetaObjectLink UnpurchaseableOverlay { get; set; } = new(0);
        [MetaProperty("CostText", BinPropertyType.ObjectLink)]
        public MetaObjectLink CostText { get; set; } = new(0);
        [MetaProperty("HitRegion", BinPropertyType.ObjectLink)]
        public MetaObjectLink HitRegion { get; set; } = new(0);
    }
    [MetaClass("DynamicMaterialStaticSwitch")]
    public class DynamicMaterialStaticSwitch : IMetaClass
    {
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = true;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Driver { get; set; } = null;
    }
    [MetaClass("ResourceResolver")]
    public class ResourceResolver : BaseResourceResolver
    {
    }
    [MetaClass("CursorDataTeamContext")]
    public class CursorDataTeamContext : IMetaClass
    {
        [MetaProperty("mData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorData>> Data { get; set; } = new();
    }
    [MetaClass("SinusoidalHeightSolver")]
    public class SinusoidalHeightSolver : HeightSolverType
    {
        [MetaProperty("mVerticalOffset", BinPropertyType.Float)]
        public float VerticalOffset { get; set; } = 0f;
        [MetaProperty("mAmplitude", BinPropertyType.Float)]
        public float Amplitude { get; set; } = 0f;
        [MetaProperty("mNumberOfPeriods", BinPropertyType.Float)]
        public float NumberOfPeriods { get; set; } = 0f;
    }
    [MetaClass("StringTableGet")]
    public class StringTableGet : IStringGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<string> Default { get; set; } = new MetaOptional<string>(default(string), false);
    }
    [MetaClass("ExperienceModData")]
    public class ExperienceModData : IMetaClass
    {
        [MetaProperty("mPlayerMinionSplitXp", BinPropertyType.Container)]
        public MetaContainer<float> PlayerMinionSplitXp { get; set; } = new();
    }
    [MetaClass("TrophyPedestalData")]
    public class TrophyPedestalData : BaseLoadoutData
    {
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; } = new (new ());
        [MetaProperty("mDescriptionTraKey", BinPropertyType.String)]
        public string DescriptionTraKey { get; set; } = "";
        [MetaProperty("mNameTraKey", BinPropertyType.String)]
        public string NameTraKey { get; set; } = "";
        [MetaProperty("mTierTRAKey", BinPropertyType.String)]
        public string TierTRAKey { get; set; } = "";
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; } = new (new ());
        [MetaProperty("mJointName", BinPropertyType.String)]
        public string JointName { get; set; } = "";
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink AnimationGraphData { get; set; } = new(0);
    }
    [MetaClass("VfxPrimitiveCameraSegmentBeam")]
    public class VfxPrimitiveCameraSegmentBeam : VfxPrimitiveBeamBase
    {
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; } = new (new ());
    }
    [MetaClass("InstanceVarsTable")]
    public class InstanceVarsTable : ScriptTable
    {
    }
    [MetaClass(4066427822)]
    public class Class0xf260c3ae : IMetaClass
    {
        [MetaProperty("mDropRatesByLevel", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftDropRates>> DropRatesByLevel { get; set; } = new();
    }
    [MetaClass("MouseOverEffectData")]
    public class MouseOverEffectData : IMetaClass
    {
        [MetaProperty("mInteractionSizes", BinPropertyType.Container)]
        public MetaContainer<int> InteractionSizes { get; set; } = new();
        [MetaProperty("mAvatarBlurPassCount", BinPropertyType.UInt32)]
        public uint AvatarBlurPassCount { get; set; } = 1;
        [MetaProperty("mInteractionTimes", BinPropertyType.Container)]
        public MetaContainer<float> InteractionTimes { get; set; } = new();
        [MetaProperty("mEnemyColor", BinPropertyType.Color)]
        public Color EnemyColor { get; set; } = new Color(0f, 0f, 255f, 255f);
        [MetaProperty("mAllyColor", BinPropertyType.Color)]
        public Color AllyColor { get; set; } = new Color(255f, 0f, 0f, 255f);
        [MetaProperty("mSelfColor", BinPropertyType.Color)]
        public Color SelfColor { get; set; } = new Color(0f, 255f, 0f, 255f);
        [MetaProperty("mSelectedBlurPassCount", BinPropertyType.UInt32)]
        public uint SelectedBlurPassCount { get; set; } = 1;
        [MetaProperty("mKillerColorFactor", BinPropertyType.Float)]
        public float KillerColorFactor { get; set; } = 0.5f;
        [MetaProperty("mSelectedColorFactor", BinPropertyType.Float)]
        public float SelectedColorFactor { get; set; } = 0.5f;
        [MetaProperty("mAvatarSize", BinPropertyType.Int32)]
        public int AvatarSize { get; set; } = 1;
        [MetaProperty("mSelectedSize", BinPropertyType.Int32)]
        public int SelectedSize { get; set; } = 1;
        [MetaProperty("mMouseOverColorFactor", BinPropertyType.Float)]
        public float MouseOverColorFactor { get; set; } = 0.5f;
        [MetaProperty("mAvatarColor", BinPropertyType.Color)]
        public Color AvatarColor { get; set; } = new Color(0f, 255f, 255f, 255f);
        [MetaProperty("mMouseOverBlurPassCount", BinPropertyType.UInt32)]
        public uint MouseOverBlurPassCount { get; set; } = 1;
        [MetaProperty("mMouseOverSize", BinPropertyType.Int32)]
        public int MouseOverSize { get; set; } = 1;
        [MetaProperty("mKillerBlurPassCount", BinPropertyType.UInt32)]
        public uint KillerBlurPassCount { get; set; } = 1;
        [MetaProperty("mKillerSize", BinPropertyType.Int32)]
        public int KillerSize { get; set; } = 1;
        [MetaProperty("mAvatarColorFactor", BinPropertyType.Float)]
        public float AvatarColorFactor { get; set; } = 0.5f;
        [MetaProperty("mNeutralColor", BinPropertyType.Color)]
        public Color NeutralColor { get; set; } = new Color(255f, 255f, 255f, 255f);
    }
    [MetaClass("StoreCategoryButtonDefinition")]
    public class StoreCategoryButtonDefinition : IMetaClass
    {
        [MetaProperty("category", BinPropertyType.UInt32)]
        public uint Category { get; set; } = 0;
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("TftDropRates")]
    public class TftDropRates : IMetaClass
    {
        [MetaProperty("mDropRatesByTier", BinPropertyType.Container)]
        public MetaContainer<float> DropRatesByTier { get; set; } = new();
    }
    [MetaClass("ParametricClipData")]
    public class ParametricClipData : BlendableClipData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        public MetaHash MaskDataName { get; set; } = new(0);
        [MetaProperty("mSyncGroupDataName", BinPropertyType.Hash)]
        public MetaHash SyncGroupDataName { get; set; } = new(0);
        [MetaProperty("mTrackDataName", BinPropertyType.Hash)]
        public MetaHash TrackDataName { get; set; } = new(0);
        [MetaProperty("mEventDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, BaseEventData> EventDataMap { get; set; } = new();
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; } = new();
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 0;
        [MetaProperty("mUpdaterType", BinPropertyType.UInt32)]
        public uint UpdaterType { get; set; } = 4294967295;
        [MetaProperty("mParametricPairDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ParametricPairData>> ParametricPairDataList { get; set; } = new();
    }
    [MetaClass("FeatureAudioDataProperties")]
    public class FeatureAudioDataProperties : IMetaClass
    {
        [MetaProperty("music", BinPropertyType.Embedded)]
        public MetaEmbedded<MusicAudioDataProperties> Music { get; set; } = new (new ());
        [MetaProperty("feature", BinPropertyType.Hash)]
        public MetaHash Feature { get; set; } = new(0);
        [MetaProperty("bankUnits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BankUnit>> BankUnits { get; set; } = new();
    }
    [MetaClass("MissionBuffData")]
    public class MissionBuffData : IMetaClass
    {
        [MetaProperty("fireDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> FireDrake { get; set; } = new (new ());
        [MetaProperty("airDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> AirDrake { get; set; } = new (new ());
        [MetaProperty("earthDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> EarthDrake { get; set; } = new (new ());
        [MetaProperty("elderDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> ElderDrake { get; set; } = new (new ());
        [MetaProperty("waterDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> WaterDrake { get; set; } = new (new ());
        [MetaProperty(2280272006, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2280272006 { get; set; } = new (new ());
        [MetaProperty(2297049625, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2297049625 { get; set; } = new (new ());
        [MetaProperty(2380937720, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2380937720 { get; set; } = new (new ());
        [MetaProperty("GameModeCustom2", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> GameModeCustom2 { get; set; } = new (new ());
        [MetaProperty("GameModeCustom1", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> GameModeCustom1 { get; set; } = new (new ());
        [MetaProperty(2448048196, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2448048196 { get; set; } = new (new ());
        [MetaProperty(2464825815, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2464825815 { get; set; } = new (new ());
        [MetaProperty(2481603434, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2481603434 { get; set; } = new (new ());
        [MetaProperty(2498381053, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2498381053 { get; set; } = new (new ());
        [MetaProperty(3149811562, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m3149811562 { get; set; } = new (new ());
        [MetaProperty("dragon", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> Dragon { get; set; } = new (new ());
    }
    [MetaClass(4073702540)]
    public class Class0xf2cfc48c : IMetaClass
    {
        [MetaProperty("BarBackdrop", BinPropertyType.ObjectLink)]
        public MetaObjectLink BarBackdrop { get; set; } = new(0);
        [MetaProperty("BarFill", BinPropertyType.ObjectLink)]
        public MetaObjectLink BarFill { get; set; } = new(0);
        [MetaProperty("sliderIcon", BinPropertyType.ObjectLink)]
        public MetaObjectLink SliderIcon { get; set; } = new(0);
    }
    [MetaClass("FixedShaderDef")]
    public class FixedShaderDef : IShaderDef
    {
        [MetaProperty("featureMask", BinPropertyType.UInt32)]
        public uint FeatureMask { get; set; } = 0;
        [MetaProperty("staticSwitches", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderStaticSwitch>> StaticSwitches { get; set; } = new();
        [MetaProperty("parameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderPhysicalParameter>> Parameters { get; set; } = new();
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderTexture>> Textures { get; set; } = new();
        [MetaProperty(2617146753, BinPropertyType.UInt32)]
        public uint m2617146753 { get; set; } = 0;
        [MetaProperty("featureDefines", BinPropertyType.Map)]
        public Dictionary<string, string> FeatureDefines { get; set; } = new();
        [MetaProperty("vertexShader", BinPropertyType.String)]
        public string VertexShader { get; set; } = "";
        [MetaProperty("pixelShader", BinPropertyType.String)]
        public string PixelShader { get; set; } = "";
    }
    [MetaClass("SkinSummonerEmoteLoadout")]
    public class SkinSummonerEmoteLoadout : IMetaClass
    {
        [MetaProperty("mEmotes", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Emotes { get; set; } = new();
    }
    [MetaClass("SummonerSpellPerkReplacement")]
    public class SummonerSpellPerkReplacement : IMetaClass
    {
        [MetaProperty("mSummonerSpellRequired", BinPropertyType.Hash)]
        public MetaHash SummonerSpellRequired { get; set; } = new(0);
        [MetaProperty(3565838065, BinPropertyType.Hash)]
        public MetaHash m3565838065 { get; set; } = new(0);
    }
    [MetaClass("OptionItemBorder")]
    public class OptionItemBorder : IOptionItem
    {
        [MetaProperty("LiveUpdate", BinPropertyType.Bool)]
        public bool LiveUpdate { get; set; } = false;
        [MetaProperty("ShowOnPlatform", BinPropertyType.Byte)]
        public byte ShowOnPlatform { get; set; } = 1;
        [MetaProperty("Filter", BinPropertyType.Structure)]
        public IOptionItemFilter Filter { get; set; } = null;
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<IOptionItem> Items { get; set; } = new();
        [MetaProperty("template", BinPropertyType.Hash)]
        public MetaHash Template { get; set; } = new(0);
    }
    [MetaClass("TimeMaterialDriver")]
    public class TimeMaterialDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass("EffectCircleMaskCooldownElementData")]
    public class EffectCircleMaskCooldownElementData : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mEffectColor0", BinPropertyType.Color)]
        public Color EffectColor0 { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("mEffectColor1", BinPropertyType.Color)]
        public Color EffectColor1 { get; set; } = new Color(255f, 255f, 255f, 255f);
    }
    [MetaClass("GlobalStatsUIData")]
    public class GlobalStatsUIData : IMetaClass
    {
        [MetaProperty("FormulaPartStyleBonusPercent", BinPropertyType.String)]
        public string FormulaPartStyleBonusPercent { get; set; } = "";
        [MetaProperty("FormulaPartRangeStylePercent", BinPropertyType.String)]
        public string FormulaPartRangeStylePercent { get; set; } = "";
        [MetaProperty("mNumberStyleTotalAndCoefficientPercent", BinPropertyType.String)]
        public string NumberStyleTotalAndCoefficientPercent { get; set; } = "";
        [MetaProperty("FormulaPartStyle", BinPropertyType.String)]
        public string FormulaPartStyle { get; set; } = "";
        [MetaProperty("FormulaPartRangeStyleBonus", BinPropertyType.String)]
        public string FormulaPartRangeStyleBonus { get; set; } = "";
        [MetaProperty("mStatUIData", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<StatUIData>> StatUIData { get; set; } = new();
        [MetaProperty(799458480, BinPropertyType.String)]
        public string m799458480 { get; set; } = "";
        [MetaProperty("mNumberStyleBonus", BinPropertyType.String)]
        public string NumberStyleBonus { get; set; } = "";
        [MetaProperty("FormulaPartRangeStyleBonusPercent", BinPropertyType.String)]
        public string FormulaPartRangeStyleBonusPercent { get; set; } = "";
        [MetaProperty("mNumberStylePercent", BinPropertyType.String)]
        public string NumberStylePercent { get; set; } = "";
        [MetaProperty(1254137583, BinPropertyType.String)]
        public string m1254137583 { get; set; } = "";
        [MetaProperty("mNumberStyleBonusPercent", BinPropertyType.String)]
        public string NumberStyleBonusPercent { get; set; } = "";
        [MetaProperty(2536782976, BinPropertyType.Byte)]
        public byte m2536782976 { get; set; } = 2;
        [MetaProperty("FormulaPartStyleBonus", BinPropertyType.String)]
        public string FormulaPartStyleBonus { get; set; } = "";
        [MetaProperty("BonusOutputIconModifier", BinPropertyType.String)]
        public string BonusOutputIconModifier { get; set; } = "";
        [MetaProperty(2823080650, BinPropertyType.Byte)]
        public byte m2823080650 { get; set; } = 0;
        [MetaProperty("NumberStyleTotalAndFormula", BinPropertyType.String)]
        public string NumberStyleTotalAndFormula { get; set; } = "";
        [MetaProperty("mNumberStyleTotalAndCoefficient", BinPropertyType.String)]
        public string NumberStyleTotalAndCoefficient { get; set; } = "";
        [MetaProperty("FormulaPartRangeStyle", BinPropertyType.String)]
        public string FormulaPartRangeStyle { get; set; } = "";
        [MetaProperty("BaseOutputIconModifier", BinPropertyType.String)]
        public string BaseOutputIconModifier { get; set; } = "";
        [MetaProperty("mNumberStyle", BinPropertyType.String)]
        public string NumberStyle { get; set; } = "";
        [MetaProperty("FormulaPartStylePercent", BinPropertyType.String)]
        public string FormulaPartStylePercent { get; set; } = "";
        [MetaProperty("NumberStyleTotalAndScalingIcons", BinPropertyType.String)]
        public string NumberStyleTotalAndScalingIcons { get; set; } = "";
        [MetaProperty("mManaScalingTagKey", BinPropertyType.String)]
        public string ManaScalingTagKey { get; set; } = "";
        [MetaProperty("mManaIconKey", BinPropertyType.String)]
        public string ManaIconKey { get; set; } = "";
        [MetaProperty(4031521229, BinPropertyType.Byte)]
        public byte m4031521229 { get; set; } = 0;
    }
    [MetaClass("HasTypeAndStatusFlags")]
    public class HasTypeAndStatusFlags : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mAffectsStatusFlags", BinPropertyType.UInt32)]
        public uint AffectsStatusFlags { get; set; } = 0;
        [MetaProperty("mAffectsTypeFlags", BinPropertyType.UInt32)]
        public uint AffectsTypeFlags { get; set; } = 0;
    }
    [MetaClass("ElementGroupData")]
    public class ElementGroupData : Class0x231dd1a2
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("elements", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Elements { get; set; } = new();
    }
    [MetaClass("LoadoutEmoteInfoPanel")]
    public class LoadoutEmoteInfoPanel : ILoadoutInfoPanel
    {
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty("EmoteWheelUpperButton", BinPropertyType.Hash)]
        public MetaHash EmoteWheelUpperButton { get; set; } = new(0);
        [MetaProperty("EmoteWheelRightButton", BinPropertyType.Hash)]
        public MetaHash EmoteWheelRightButton { get; set; } = new(0);
        [MetaProperty("EmoteWheelLowerButton", BinPropertyType.Hash)]
        public MetaHash EmoteWheelLowerButton { get; set; } = new(0);
        [MetaProperty("EmoteWheelLeftButton", BinPropertyType.Hash)]
        public MetaHash EmoteWheelLeftButton { get; set; } = new(0);
        [MetaProperty("emoteStartButton", BinPropertyType.Hash)]
        public MetaHash EmoteStartButton { get; set; } = new(0);
        [MetaProperty("EmoteWheelCenterButton", BinPropertyType.Hash)]
        public MetaHash EmoteWheelCenterButton { get; set; } = new(0);
        [MetaProperty("emoteVictoryButton", BinPropertyType.Hash)]
        public MetaHash EmoteVictoryButton { get; set; } = new(0);
    }
    [MetaClass("GameModeConstantTRAKey")]
    public class GameModeConstantTRAKey : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.String)]
        public string Value { get; set; } = "";
    }
    [MetaClass("UseableData")]
    public class UseableData : IMetaClass
    {
        [MetaProperty("useHeroSpellName", BinPropertyType.String)]
        public string UseHeroSpellName { get; set; } = "";
        [MetaProperty("useSpellName", BinPropertyType.String)]
        public string UseSpellName { get; set; } = "";
        [MetaProperty("useCooldownSpellSlot", BinPropertyType.Int32)]
        public int UseCooldownSpellSlot { get; set; } = 62;
        [MetaProperty("flags", BinPropertyType.UInt32)]
        public uint Flags { get; set; } = 8;
    }
    [MetaClass("ItemShopGameModeData")]
    public class ItemShopGameModeData : IMetaClass
    {
        [MetaProperty(1369541571, BinPropertyType.Hash)]
        public MetaHash m1369541571 { get; set; } = new(0);
        [MetaProperty(2306632119, BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaHash> m2306632119 { get; set; } = new();
        [MetaProperty("RecItemsSwaps", BinPropertyType.Map)]
        public Dictionary<uint, uint> RecItemsSwaps { get; set; } = new();
        [MetaProperty(3283305226, BinPropertyType.UInt32)]
        public uint m3283305226 { get; set; } = 0;
        [MetaProperty(3311532265, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m3311532265 { get; set; } = new();
    }
    [MetaClass("TFTRoundData")]
    public class TFTRoundData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty(810222384, BinPropertyType.String)]
        public string m810222384 { get; set; } = "";
        [MetaProperty("mDraft", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> Draft { get; set; } = new (new ());
        [MetaProperty("mDraftArrival", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> DraftArrival { get; set; } = new (new ());
        [MetaProperty(1286805709, BinPropertyType.String)]
        public string m1286805709 { get; set; } = "";
        [MetaProperty("mDraftDeparture", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> DraftDeparture { get; set; } = new (new ());
        [MetaProperty("mStateTooltipsTra", BinPropertyType.Map)]
        public Dictionary<uint, string> StateTooltipsTra { get; set; } = new();
        [MetaProperty("mPlanningDeparture", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> PlanningDeparture { get; set; } = new (new ());
        [MetaProperty("mScriptData", BinPropertyType.Map)]
        public Dictionary<string, GameModeConstant> ScriptData { get; set; } = new();
        [MetaProperty("mPlanningArrival", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> PlanningArrival { get; set; } = new (new ());
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string IconPath { get; set; } = "";
        [MetaProperty("mCombatArrival", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> CombatArrival { get; set; } = new (new ());
        [MetaProperty("mPlanning", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> Planning { get; set; } = new (new ());
        [MetaProperty("mDescriptionTra", BinPropertyType.String)]
        public string DescriptionTra { get; set; } = "";
        [MetaProperty("mDisplayNameTra", BinPropertyType.String)]
        public string DisplayNameTra { get; set; } = "";
        [MetaProperty("mTftDamageSidewall", BinPropertyType.String)]
        public string TftDamageSidewall { get; set; } = "";
        [MetaProperty(3414363751, BinPropertyType.String)]
        public string m3414363751 { get; set; } = "";
        [MetaProperty(3523237056, BinPropertyType.String)]
        public string m3523237056 { get; set; } = "";
        [MetaProperty("mDefaultTooltipTra", BinPropertyType.String)]
        public string DefaultTooltipTra { get; set; } = "";
        [MetaProperty("mCombatDeparture", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> CombatDeparture { get; set; } = new (new ());
        [MetaProperty(4167224325, BinPropertyType.String)]
        public string m4167224325 { get; set; } = "";
        [MetaProperty("mCombat", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> Combat { get; set; } = new (new ());
    }
    [MetaClass("StopAnimationEventData")]
    public class StopAnimationEventData : BaseEventData
    {
        [MetaProperty("mStopAnimationName", BinPropertyType.Hash)]
        public MetaHash StopAnimationName { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionItemPurchased")]
    public class ContextualConditionItemPurchased : IContextualCondition
    {
        [MetaProperty("mItemPurchased", BinPropertyType.Bool)]
        public bool ItemPurchased { get; set; } = false;
    }
    [MetaClass("GeneralSettingsGroup")]
    public class GeneralSettingsGroup : IMetaClass
    {
        [MetaProperty("restorePurchaseButton", BinPropertyType.Hash)]
        public MetaHash RestorePurchaseButton { get; set; } = new(0);
        [MetaProperty("PromoteAccountButton", BinPropertyType.Hash)]
        public MetaHash PromoteAccountButton { get; set; } = new(0);
        [MetaProperty("SignOutButton", BinPropertyType.Hash)]
        public MetaHash SignOutButton { get; set; } = new(0);
    }
    [MetaClass(4113714730)]
    public class Class0xf5324e2a : OptionItemDropdown
    {
    }
    [MetaClass("MasteryBadgeConfig")]
    public class MasteryBadgeConfig : IMetaClass
    {
        [MetaProperty("mBadges", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MasteryBadgeData>> Badges { get; set; } = new();
    }
    [MetaClass("FxActionMove")]
    public class FxActionMove : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("OvershootDistance", BinPropertyType.Float)]
        public float OvershootDistance { get; set; } = 0f;
        [MetaProperty("EasingType", BinPropertyType.Byte)]
        public byte EasingType { get; set; } = 0;
        [MetaProperty("FaceVelocity", BinPropertyType.Bool)]
        public bool FaceVelocity { get; set; } = true;
        [MetaProperty("Destination", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> Destination { get; set; } = new (new ());
        [MetaProperty(3644194370, BinPropertyType.Bool)]
        public bool m3644194370 { get; set; } = false;
        [MetaProperty("TargetObject", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTarget> TargetObject { get; set; } = new (new ());
    }
    [MetaClass("GameMutatorExpansions")]
    public class GameMutatorExpansions : IMetaClass
    {
        [MetaProperty("mExpandedMutator", BinPropertyType.String)]
        public string ExpandedMutator { get; set; } = "";
        [MetaProperty("mMutators", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<string> Mutators { get; set; } = new();
    }
    [MetaClass("HudStatStoneMilestoneData")]
    public class HudStatStoneMilestoneData : IMetaClass
    {
        [MetaProperty("MilestoneSelfIntroTime", BinPropertyType.Float)]
        public float MilestoneSelfIntroTime { get; set; } = 1f;
        [MetaProperty("MilestoneTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> MilestoneTransitionOut { get; set; } = new (new ());
        [MetaProperty(973000595, BinPropertyType.Float)]
        public float m973000595 { get; set; } = 0f;
        [MetaProperty("MilestoneDisplayTime", BinPropertyType.Float)]
        public float MilestoneDisplayTime { get; set; } = 3f;
        [MetaProperty(1319476500, BinPropertyType.Bool)]
        public bool m1319476500 { get; set; } = false;
        [MetaProperty(1333781411, BinPropertyType.String)]
        public string m1333781411 { get; set; } = "";
        [MetaProperty("MilestoneOtherIntroTime", BinPropertyType.Float)]
        public float MilestoneOtherIntroTime { get; set; } = 1f;
        [MetaProperty(2798761049, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2798761049 { get; set; } = new (new ());
        [MetaProperty("UiSound", BinPropertyType.String)]
        public string UiSound { get; set; } = "";
        [MetaProperty("MilestoneTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> MilestoneTransitionIn { get; set; } = new (new ());
        [MetaProperty(3144760040, BinPropertyType.String)]
        public string m3144760040 { get; set; } = "";
        [MetaProperty("PersonalBestIntroTime", BinPropertyType.Float)]
        public float PersonalBestIntroTime { get; set; } = 1f;
        [MetaProperty(229950671, BinPropertyType.UInt32)]
        public uint m229950671 { get; set; } = 5;
        [MetaProperty(3709612203, BinPropertyType.Float)]
        public float m3709612203 { get; set; } = 8f;
        [MetaProperty(3992539329, BinPropertyType.String)]
        public string m3992539329 { get; set; } = "";
    }
    [MetaClass("HudLoadingScreenWidgetTutorial")]
    public class HudLoadingScreenWidgetTutorial : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string SceneName { get; set; } = "";
    }
    [MetaClass("AnimationGraphData")]
    public class AnimationGraphData : IMetaClass
    {
        [MetaProperty("mCascadeBlendValue", BinPropertyType.Float)]
        public float CascadeBlendValue { get; set; } = -1f;
        [MetaProperty("mBlendDataTable", BinPropertyType.Map)]
        public Dictionary<ulong, BaseBlendData> BlendDataTable { get; set; } = new();
        [MetaProperty("mTrackDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<TrackData>> TrackDataMap { get; set; } = new();
        [MetaProperty("mClipDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, ClipBaseData> ClipDataMap { get; set; } = new();
        [MetaProperty("mSyncGroupDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<SyncGroupData>> SyncGroupDataMap { get; set; } = new();
        [MetaProperty("mUseCascadeBlend", BinPropertyType.Bool)]
        public bool UseCascadeBlend { get; set; } = false;
        [MetaProperty("mMaskDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<MaskData>> MaskDataMap { get; set; } = new();
    }
    [MetaClass("VfxAssetRemap")]
    public class VfxAssetRemap : IMetaClass
    {
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 0;
        [MetaProperty("oldAsset", BinPropertyType.Hash)]
        public MetaHash OldAsset { get; set; } = new(0);
        [MetaProperty("newAsset", BinPropertyType.String)]
        public string NewAsset { get; set; } = "";
    }
    [MetaClass("OptionTemplateSecondaryHotkeys2Column")]
    public class OptionTemplateSecondaryHotkeys2Column : IOptionTemplate
    {
        [MetaProperty(1415010472, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x354988a8> m1415010472 { get; set; } = new (new ());
        [MetaProperty(1465343329, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0x354988a8> m1465343329 { get; set; } = new (new ());
        [MetaProperty(1499050595, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xceb70e5a> m1499050595 { get; set; } = new (new ());
        [MetaProperty(1515828214, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xceb70e5a> m1515828214 { get; set; } = new (new ());
        [MetaProperty(1532605833, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xceb70e5a> m1532605833 { get; set; } = new (new ());
        [MetaProperty(4247899083, BinPropertyType.Embedded)]
        public MetaEmbedded<Class0xceb70e5a> m4247899083 { get; set; } = new (new ());
    }
    [MetaClass("RandomChanceScriptCondition")]
    public class RandomChanceScriptCondition : IScriptCondition
    {
        [MetaProperty("Chance", BinPropertyType.Structure)]
        public IFloatGet Chance { get; set; } = null;
    }
    [MetaClass("ScriptTableSet")]
    public class ScriptTableSet : IMetaClass
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; } = null;
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash Var { get; set; } = new(0);
    }
    [MetaClass("IScriptValueGet")]
    public interface IScriptValueGet : IMetaClass
    {
    }
    [MetaClass(4143783062)]
    public class Class0xf6fd1c96 : EffectElementData
    {
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool m629911194 { get; set; } = false;
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionWidth { get; set; } = 0;
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort RectSourceResolutionHeight { get; set; } = 0;
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint Draggable { get; set; } = 0;
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool Enabled { get; set; } = false;
        [MetaProperty("StickyDrag", BinPropertyType.Bool)]
        public bool StickyDrag { get; set; } = false;
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint Layer { get; set; } = 0;
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool KeepMaxScale { get; set; } = false;
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; } = new();
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4 Rect { get; set; } = new Vector4(0f, 0f, 0f, 0f);
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool UseRectSourceResolutionAsFloor { get; set; } = false;
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool Fullscreen { get; set; } = false;
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool NoPixelSnappingY { get; set; } = false;
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool NoPixelSnappingX { get; set; } = false;
        [MetaProperty("mAnchors", BinPropertyType.Structure)]
        public AnchorBase Anchors { get; set; } = null;
        [MetaProperty("mName", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink Scene { get; set; } = new(0);
        [MetaProperty("mMaterial", BinPropertyType.ObjectLink)]
        public MetaObjectLink Material { get; set; } = new(0);
    }
    [MetaClass(4144515914)]
    public class Class0xf7084b4a : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; } = new();
    }
    [MetaClass("LoadScreenTipSet")]
    public class LoadScreenTipSet : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.Hash)]
        public MetaHash Name { get; set; } = new(0);
        [MetaProperty("mTips", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Tips { get; set; } = new();
    }
    [MetaClass("StatStoneEventToTrack")]
    public class StatStoneEventToTrack : IMetaClass
    {
        [MetaProperty("EventToTrack", BinPropertyType.UInt32)]
        public uint EventToTrack { get; set; } = 227;
        [MetaProperty("StatFilters", BinPropertyType.Container)]
        public MetaContainer<IStatStoneLogicDriver> StatFilters { get; set; } = new();
    }
    [MetaClass("FixedTimeMovement")]
    public class FixedTimeMovement : MissileMovementSpec
    {
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool TracksTarget { get; set; } = true;
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float TargetHeightAugment { get; set; } = 0f;
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string TargetBoneName { get; set; } = "";
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; } = new();
        [MetaProperty("mStartDelay", BinPropertyType.Float)]
        public float StartDelay { get; set; } = 0f;
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool m2856647070 { get; set; } = false;
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string StartBoneName { get; set; } = "R_Hand";
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool UseHeightOffsetAtEnd { get; set; } = false;
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float OffsetInitialTargetHeight { get; set; } = 0f;
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool ProjectTargetToCastRange { get; set; } = false;
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool UseGroundHeightAtTarget { get; set; } = true;
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool InferDirectionFromFacingIfNeeded { get; set; } = true;
        [MetaProperty("mTravelTime", BinPropertyType.Float)]
        public float TravelTime { get; set; } = 0f;
    }
    [MetaClass("TFTStageData")]
    public class TFTStageData : IMetaClass
    {
        [MetaProperty("mRounds", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Rounds { get; set; } = new();
    }
    [MetaClass("VfxProjectionDefinitionData")]
    public class VfxProjectionDefinitionData : IMetaClass
    {
        [MetaProperty("mFading", BinPropertyType.Float)]
        public float Fading { get; set; } = 200f;
        [MetaProperty("mYRange", BinPropertyType.Float)]
        public float YRange { get; set; } = 5f;
    }
    [MetaClass("ILineIndicatorType")]
    public interface ILineIndicatorType : IMetaClass
    {
    }
    [MetaClass("HasAllSubRequirementsCastRequirement")]
    public class HasAllSubRequirementsCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool InvertResult { get; set; } = false;
        [MetaProperty("mSubRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> SubRequirements { get; set; } = new();
    }
    [MetaClass("SkinCharacterMetaDataProperties")]
    public class SkinCharacterMetaDataProperties : IMetaClass
    {
        [MetaProperty("useAudioProperties", BinPropertyType.Bool)]
        public bool UseAudioProperties { get; set; } = false;
        [MetaProperty("eSportLeagueTable", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ESportLeagueEntry>> ESportLeagueTable { get; set; } = new();
        [MetaProperty("isRelativeColorCharacter", BinPropertyType.Bool)]
        public bool IsRelativeColorCharacter { get; set; } = false;
        [MetaProperty("eSportCharacter", BinPropertyType.Bool)]
        public bool ESportCharacter { get; set; } = false;
        [MetaProperty("eSportTeamTable", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ESportTeamEntry>> ESportTeamTable { get; set; } = new();
        [MetaProperty("skinBasedRelativeColorScheme", BinPropertyType.Bool)]
        public bool SkinBasedRelativeColorScheme { get; set; } = false;
        [MetaProperty("useGDSBinaries", BinPropertyType.Bool)]
        public bool UseGDSBinaries { get; set; } = false;
        [MetaProperty("spawningSkinOffsets", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinCharacterMetaDataProperties_SpawningSkinOffset>> SpawningSkinOffsets { get; set; } = new();
        [MetaProperty("relativeColorSwapTable", BinPropertyType.Container)]
        public MetaContainer<int> RelativeColorSwapTable { get; set; } = new();
    }
    [MetaClass(4160905752)]
    public class Class0xf8026218 : IOptionItemFilter
    {
    }
    [MetaClass("FxActionVfx")]
    public class FxActionVfx : IFxAction
    {
        [MetaProperty("Start", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> Start { get; set; } = new (new ());
        [MetaProperty("End", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTiming> End { get; set; } = new (new ());
        [MetaProperty("TargetPosition", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> TargetPosition { get; set; } = new (new ());
        [MetaProperty("FollowPath", BinPropertyType.Bool)]
        public bool FollowPath { get; set; } = false;
        [MetaProperty(970384512, BinPropertyType.Float)]
        public float m970384512 { get; set; } = 0f;
        [MetaProperty("follow", BinPropertyType.Bool)]
        public bool Follow { get; set; } = false;
        [MetaProperty("scale", BinPropertyType.Float)]
        public float Scale { get; set; } = 1f;
        [MetaProperty("Particle", BinPropertyType.ObjectLink)]
        public MetaObjectLink Particle { get; set; } = new(0);
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> Position { get; set; } = new (new ());
        [MetaProperty("SplineInfo", BinPropertyType.Structure)]
        public ISplineInfo SplineInfo { get; set; } = null;
        [MetaProperty("PathTargetPosition", BinPropertyType.Embedded)]
        public MetaEmbedded<FxTransform> PathTargetPosition { get; set; } = new (new ());
    }
    [MetaClass("TeamScoreMeterUITunables")]
    public class TeamScoreMeterUITunables : IMetaClass
    {
        [MetaProperty("mTeamScoreMeterMaxRoundsPerTeam", BinPropertyType.UInt32)]
        public uint TeamScoreMeterMaxRoundsPerTeam { get; set; } = 0;
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; } = new (new ());
        [MetaProperty("mCountdownTimer", BinPropertyType.Bool)]
        public bool CountdownTimer { get; set; } = false;
        [MetaProperty("mTeamScoreMeterProperties", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudTeamScoreMeterProperties>> TeamScoreMeterProperties { get; set; } = new();
        [MetaProperty(3757209935, BinPropertyType.Byte)]
        public byte m3757209935 { get; set; } = 0;
        [MetaProperty("mAllowDynamicVisibility", BinPropertyType.Bool)]
        public bool AllowDynamicVisibility { get; set; } = false;
    }
    [MetaClass("SocialPanelViewController")]
    public class SocialPanelViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink m3080488622 { get; set; } = new(0);
        [MetaProperty("AddFriendButtonDefinition", BinPropertyType.Hash)]
        public MetaHash AddFriendButtonDefinition { get; set; } = new(0);
        [MetaProperty("InviteButtonDefinition", BinPropertyType.Hash)]
        public MetaHash InviteButtonDefinition { get; set; } = new(0);
        [MetaProperty("ViewPaneDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<ViewPaneDefinition> ViewPaneDefinition { get; set; } = new (new ());
        [MetaProperty("FriendButtonDefinition", BinPropertyType.Hash)]
        public MetaHash FriendButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("WardSkinDisabler")]
    public class WardSkinDisabler : IMetaClass
    {
        [MetaProperty("DisableAllSkins", BinPropertyType.Bool)]
        public bool DisableAllSkins { get; set; } = false;
        [MetaProperty("DisabledIds", BinPropertyType.Container)]
        public MetaContainer<uint> DisabledIds { get; set; } = new();
    }
    [MetaClass("IsDeadDynamicMaterialBoolDriver")]
    public class IsDeadDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
    }
    [MetaClass("HudLoadingScreenData")]
    public class HudLoadingScreenData : IMetaClass
    {
        [MetaProperty("mLoadingSpinnerRows", BinPropertyType.Byte)]
        public byte LoadingSpinnerRows { get; set; } = 1;
        [MetaProperty("mLoadingSpinnerSpeed", BinPropertyType.Float)]
        public float LoadingSpinnerSpeed { get; set; } = 1f;
        [MetaProperty("mLoadingSpinnerFrames", BinPropertyType.Byte)]
        public byte LoadingSpinnerFrames { get; set; } = 1;
        [MetaProperty(2635590115, BinPropertyType.Bool)]
        public bool m2635590115 { get; set; } = false;
        [MetaProperty("mProgressBarData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudLoadingScreenProgressBarData> ProgressBarData { get; set; } = new (new ());
    }
    [MetaClass("IContextualConditionBuff")]
    public interface IContextualConditionBuff : IContextualCondition
    {
    }
    [MetaClass("DestroyOnMovementComplete")]
    public class DestroyOnMovementComplete : MissileBehaviorSpec
    {
        [MetaProperty("mDelay", BinPropertyType.Int32)]
        public int Delay { get; set; } = 0;
    }
    [MetaClass("TooltipInstanceList")]
    public class TooltipInstanceList : IMetaClass
    {
        [MetaProperty("elements", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TooltipInstanceListElement>> Elements { get; set; } = new();
        [MetaProperty("levelCount", BinPropertyType.UInt32)]
        public uint LevelCount { get; set; } = 1;
    }
    [MetaClass("HasSkillPointRequirement")]
    public class HasSkillPointRequirement : ISpellRankUpRequirement
    {
    }
    [MetaClass("HudTeamFightOffScreenDifferentiationData")]
    public class HudTeamFightOffScreenDifferentiationData : IMetaClass
    {
        [MetaProperty(587753271, BinPropertyType.Byte)]
        public byte m587753271 { get; set; } = 100;
        [MetaProperty(1057185245, BinPropertyType.Float)]
        public float m1057185245 { get; set; } = 2700f;
        [MetaProperty(1088652879, BinPropertyType.Float)]
        public float m1088652879 { get; set; } = 5f;
    }
    [MetaClass(4225182998)]
    public class Class0xfbd72d16 : IMetaClass
    {
        [MetaProperty(1295117638, BinPropertyType.ObjectLink)]
        public MetaObjectLink m1295117638 { get; set; } = new(0);
    }
    [MetaClass("RenderStyleData")]
    public class RenderStyleData : IMetaClass
    {
        [MetaProperty("mUnitFilterParamsExterior", BinPropertyType.Embedded)]
        public MetaEmbedded<ToonInkingFilterParams> UnitFilterParamsExterior { get; set; } = new (new ());
        [MetaProperty("mUnitFilterParamsInterior", BinPropertyType.Embedded)]
        public MetaEmbedded<ToonInkingFilterParams> UnitFilterParamsInterior { get; set; } = new (new ());
        [MetaProperty("mUnitStyleUseInking", BinPropertyType.Bool)]
        public bool UnitStyleUseInking { get; set; } = false;
    }
    [MetaClass("ContextualSituation")]
    public class ContextualSituation : IMetaClass
    {
        [MetaProperty("mRules", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ContextualRule>> Rules { get; set; } = new();
        [MetaProperty("mChooseRandomValidRule", BinPropertyType.Bool)]
        public bool ChooseRandomValidRule { get; set; } = false;
        [MetaProperty("mCoolDownTime", BinPropertyType.Float)]
        public float CoolDownTime { get; set; } = 0f;
    }
    [MetaClass("EsportsBannerMaterialController")]
    public class EsportsBannerMaterialController : SkinnedMeshDataMaterialController
    {
    }
    [MetaClass("VfxPrimitiveMeshBase")]
    public interface VfxPrimitiveMeshBase : VfxPrimitiveBase
    {
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; }
        [MetaProperty(3934657962, BinPropertyType.Bool)]
        bool m3934657962 { get; set; }
        [MetaProperty(4227234111, BinPropertyType.Bool)]
        bool m4227234111 { get; set; }
    }
    [MetaClass("IFunctionGet")]
    public interface IFunctionGet : IScriptValueGet
    {
    }
    [MetaClass("HudHealthBarBurstHealData")]
    public class HudHealthBarBurstHealData : IMetaClass
    {
        [MetaProperty("healFadeDuration", BinPropertyType.Float)]
        public float HealFadeDuration { get; set; } = 0.20000000298023224f;
        [MetaProperty("healTimeWindow", BinPropertyType.Float)]
        public float HealTimeWindow { get; set; } = 0.10000000149011612f;
        [MetaProperty("healTriggerPercent", BinPropertyType.Float)]
        public float HealTriggerPercent { get; set; } = 0.10000000149011612f;
    }
    [MetaClass("OptionTemplateMuteButton")]
    public class OptionTemplateMuteButton : IOptionTemplate
    {
        [MetaProperty("buttonDefinition", BinPropertyType.Hash)]
        public MetaHash ButtonDefinition { get; set; } = new(0);
    }
    [MetaClass("ContextualConditionNumberOfCharactersNearTargetPos")]
    public class ContextualConditionNumberOfCharactersNearTargetPos : IContextualCondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 3;
        [MetaProperty("mNumberOfCharacters", BinPropertyType.UInt32)]
        public uint NumberOfCharacters { get; set; } = 1;
        [MetaProperty("mTeamCompareOp", BinPropertyType.Byte)]
        public byte TeamCompareOp { get; set; } = 0;
    }
    [MetaClass("VfxAnimatedFloatVariableData")]
    public class VfxAnimatedFloatVariableData : IMetaClass
    {
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; } = new();
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; } = new();
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; } = new();
    }
    [MetaClass("ContextualConditionNearbyChampionCount")]
    public class ContextualConditionNearbyChampionCount : IContextualCondition
    {
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte CompareOp { get; set; } = 0;
        [MetaProperty("mCount", BinPropertyType.UInt32)]
        public uint Count { get; set; } = 0;
        [MetaProperty("mTeamCompareOp", BinPropertyType.Byte)]
        public byte TeamCompareOp { get; set; } = 0;
    }
    [MetaClass("ItemRecommendationContext")]
    public class ItemRecommendationContext : IMetaClass
    {
        [MetaProperty(934764380, BinPropertyType.Hash)]
        public MetaHash m934764380 { get; set; } = new(0);
        [MetaProperty("mPosition", BinPropertyType.Hash)]
        public MetaHash Position { get; set; } = new(0);
        [MetaProperty("mPopularItems", BinPropertyType.Container)]
        public MetaContainer<MetaHash> PopularItems { get; set; } = new();
        [MetaProperty("mStartingItemMatrix", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemRecommendationMatrix> StartingItemMatrix { get; set; } = new (new ());
        [MetaProperty("mMapID", BinPropertyType.UInt32)]
        public uint MapID { get; set; } = 0;
        [MetaProperty("mChampionId", BinPropertyType.UInt32)]
        public uint ChampionId { get; set; } = 0;
        [MetaProperty("mCompletedItemMatrix", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemRecommendationMatrix> CompletedItemMatrix { get; set; } = new (new ());
        [MetaProperty("mIsDefaultPosition", BinPropertyType.Bool)]
        public bool IsDefaultPosition { get; set; } = false;
        [MetaProperty("mStartingItemBundles", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationItemList>> StartingItemBundles { get; set; } = new();
    }
    [MetaClass("TriggerFromScript")]
    public class TriggerFromScript : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; } = new();
        [MetaProperty("mDelay", BinPropertyType.Float)]
        public float Delay { get; set; } = 0.10000000149011612f;
        [MetaProperty("mTriggerName", BinPropertyType.Hash)]
        public MetaHash TriggerName { get; set; } = new(0);
    }
    [MetaClass("FloatTextDisplayOverrides")]
    public class FloatTextDisplayOverrides : IMetaClass
    {
        [MetaProperty("disableHorizontalReverse", BinPropertyType.Optional)]
        public MetaOptional<bool> DisableHorizontalReverse { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("randomOffsetMaxY", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMaxY { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("randomOffsetMaxX", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMaxX { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("ignoreCombineRules", BinPropertyType.Optional)]
        public MetaOptional<bool> IgnoreCombineRules { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("minYVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MinYVelocity { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("relativeOffsetMin", BinPropertyType.Optional)]
        public MetaOptional<float> RelativeOffsetMin { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("combinableCounterDisplay", BinPropertyType.Optional)]
        public MetaOptional<bool> CombinableCounterDisplay { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("combinableCounterCategory", BinPropertyType.Optional)]
        public MetaOptional<int> CombinableCounterCategory { get; set; } = new MetaOptional<int>(default(int), false);
        [MetaProperty("relativeOffsetMax", BinPropertyType.Optional)]
        public MetaOptional<float> RelativeOffsetMax { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("growthYScalar", BinPropertyType.Optional)]
        public MetaOptional<float> GrowthYScalar { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("overwritePreviousNumber", BinPropertyType.Optional)]
        public MetaOptional<bool> OverwritePreviousNumber { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("maxInstances", BinPropertyType.Optional)]
        public MetaOptional<int> MaxInstances { get; set; } = new MetaOptional<int>(default(int), false);
        [MetaProperty("extendTimeOnNewDamage", BinPropertyType.Optional)]
        public MetaOptional<float> ExtendTimeOnNewDamage { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("minXVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MinXVelocity { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("scale", BinPropertyType.Optional)]
        public MetaOptional<float> Scale { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("hangTime", BinPropertyType.Optional)]
        public MetaOptional<float> HangTime { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("priority", BinPropertyType.Optional)]
        public MetaOptional<int> Priority { get; set; } = new MetaOptional<int>(default(int), false);
        [MetaProperty("maxXVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MaxXVelocity { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("maxLifeTime", BinPropertyType.Optional)]
        public MetaOptional<float> MaxLifeTime { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("shrinkTime", BinPropertyType.Optional)]
        public MetaOptional<float> ShrinkTime { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("continualForceY", BinPropertyType.Optional)]
        public MetaOptional<float> ContinualForceY { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("shrinkScale", BinPropertyType.Optional)]
        public MetaOptional<float> ShrinkScale { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("continualForceX", BinPropertyType.Optional)]
        public MetaOptional<float> ContinualForceX { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("startOffsetX", BinPropertyType.Optional)]
        public MetaOptional<float> StartOffsetX { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("startOffsetY", BinPropertyType.Optional)]
        public MetaOptional<float> StartOffsetY { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("momentumFromHit", BinPropertyType.Optional)]
        public MetaOptional<bool> MomentumFromHit { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("isAnimated", BinPropertyType.Optional)]
        public MetaOptional<bool> IsAnimated { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("maxYVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MaxYVelocity { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("alternateRightLeft", BinPropertyType.Optional)]
        public MetaOptional<bool> AlternateRightLeft { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("decayDelay", BinPropertyType.Optional)]
        public MetaOptional<float> DecayDelay { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("randomOffsetMinY", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMinY { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("randomOffsetMinX", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMinX { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("decay", BinPropertyType.Optional)]
        public MetaOptional<float> Decay { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("disableVerticalReverse", BinPropertyType.Optional)]
        public MetaOptional<bool> DisableVerticalReverse { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("growthXScalar", BinPropertyType.Optional)]
        public MetaOptional<float> GrowthXScalar { get; set; } = new MetaOptional<float>(default(float), false);
        [MetaProperty("followSource", BinPropertyType.Optional)]
        public MetaOptional<bool> FollowSource { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("colorOffsetB", BinPropertyType.Optional)]
        public MetaOptional<int> ColorOffsetB { get; set; } = new MetaOptional<int>(default(int), false);
        [MetaProperty("colorOffsetG", BinPropertyType.Optional)]
        public MetaOptional<int> ColorOffsetG { get; set; } = new MetaOptional<int>(default(int), false);
        [MetaProperty("ignoreQueue", BinPropertyType.Optional)]
        public MetaOptional<bool> IgnoreQueue { get; set; } = new MetaOptional<bool>(default(bool), false);
        [MetaProperty("colorOffsetR", BinPropertyType.Optional)]
        public MetaOptional<int> ColorOffsetR { get; set; } = new MetaOptional<int>(default(int), false);
    }
    [MetaClass("HudInputBoxData")]
    public class HudInputBoxData : IMetaClass
    {
        [MetaProperty("selectedLineSizePx", BinPropertyType.Float)]
        public float SelectedLineSizePx { get; set; } = 2f;
        [MetaProperty("markedOffsetY", BinPropertyType.Float)]
        public float MarkedOffsetY { get; set; } = 3f;
        [MetaProperty("markedLineSizePx", BinPropertyType.Float)]
        public float MarkedLineSizePx { get; set; } = 2f;
        [MetaProperty("selectedOffsetY", BinPropertyType.Float)]
        public float SelectedOffsetY { get; set; } = 3f;
        [MetaProperty("caretBlinkTime", BinPropertyType.Float)]
        public float CaretBlinkTime { get; set; } = 0.5299999713897705f;
        [MetaProperty("inputTextLengthMax", BinPropertyType.Byte)]
        public byte InputTextLengthMax { get; set; } = 255;
        [MetaProperty(3198939864, BinPropertyType.Float)]
        public float m3198939864 { get; set; } = 5f;
        [MetaProperty("caretAlphaMax", BinPropertyType.Float)]
        public float CaretAlphaMax { get; set; } = 255f;
    }
    [MetaClass("StaticMaterialDef")]
    public class StaticMaterialDef : IResource,  IMaterialDef
    {
        [MetaProperty("defaultTechnique", BinPropertyType.String)]
        public string DefaultTechnique { get; set; } = "";
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint Type { get; set; } = 1;
        [MetaProperty("techniques", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialTechniqueDef>> Techniques { get; set; } = new();
        [MetaProperty("dynamicMaterial", BinPropertyType.Structure)]
        public DynamicMaterialDef DynamicMaterial { get; set; } = null;
        [MetaProperty("name", BinPropertyType.String)]
        public string Name { get; set; } = "";
        [MetaProperty("childTechniques", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialChildTechniqueDef>> ChildTechniques { get; set; } = new();
        [MetaProperty("samplerValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialShaderSamplerDef>> SamplerValues { get; set; } = new();
        [MetaProperty("paramValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialShaderParamDef>> ParamValues { get; set; } = new();
        [MetaProperty("switches", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialSwitchDef>> Switches { get; set; } = new();
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; } = new();
    }
    [MetaClass("HudAbilityPromptData")]
    public class HudAbilityPromptData : IMetaClass
    {
        [MetaProperty("pulseInterval", BinPropertyType.Float)]
        public float PulseInterval { get; set; } = 1f;
        [MetaProperty("pulseTime", BinPropertyType.Float)]
        public float PulseTime { get; set; } = 0.25f;
        [MetaProperty("PulseEndColor", BinPropertyType.Color)]
        public Color PulseEndColor { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("PulseStartColor", BinPropertyType.Color)]
        public Color PulseStartColor { get; set; } = new Color(255f, 255f, 255f, 255f);
        [MetaProperty("pulseOffset", BinPropertyType.Vector2)]
        public Vector2 PulseOffset { get; set; } = new Vector2(0f, 0f);
    }
}

