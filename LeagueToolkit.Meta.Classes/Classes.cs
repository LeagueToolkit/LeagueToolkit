using System.Numerics;
using LeagueToolkit.Helpers.Structures;
using LeagueToolkit.Meta;
using System.Collections.Generic;
using LeagueToolkit.Meta.Attributes;
using LeagueToolkit.IO.PropertyBin;
namespace LeagueToolkit.Meta.Classes
{
    [MetaClass("WardSkinDisabler")]
    public class WardSkinDisabler : IMetaClass
    {
        [MetaProperty(1271103232, BinPropertyType.Bool)]
        public bool? m1271103232 { get; set; }
        [MetaProperty(2384885417, BinPropertyType.Container)]
        public MetaContainer<uint> m2384885417 { get; set; }
    }
    [MetaClass("IContextualAction")]
    public interface IContextualAction : IMetaClass
    {
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        MetaHash? HashedSituationTrigger { get; set; }
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        uint? MaxOccurences { get; set; }
    }
    [MetaClass("ContextualActionPlayAnimation")]
    public class ContextualActionPlayAnimation : IContextualAction
    {
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        public MetaHash? HashedSituationTrigger { get; set; }
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        public uint? MaxOccurences { get; set; }
        [MetaProperty("mHashedAnimationName", BinPropertyType.Hash)]
        public MetaHash? HashedAnimationName { get; set; }
        [MetaProperty(3031797526, BinPropertyType.Bool)]
        public bool? m3031797526 { get; set; }
    }
    [MetaClass("ContextualActionTriggerEvent")]
    public class ContextualActionTriggerEvent : IContextualAction
    {
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        public MetaHash? HashedSituationTrigger { get; set; }
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        public uint? MaxOccurences { get; set; }
    }
    [MetaClass("ContextualActionPlayAudio")]
    public interface ContextualActionPlayAudio : IContextualAction
    {
        [MetaProperty("mSelfEventName", BinPropertyType.String)]
        string? SelfEventName { get; set; }
        [MetaProperty("mAllyEventName", BinPropertyType.String)]
        string? AllyEventName { get; set; }
        [MetaProperty("mEnemyEventName", BinPropertyType.String)]
        string? EnemyEventName { get; set; }
        [MetaProperty("mSpectatorEventName", BinPropertyType.String)]
        string? SpectatorEventName { get; set; }
        [MetaProperty("mWaitForAnnouncerQueue", BinPropertyType.Bool)]
        bool? WaitForAnnouncerQueue { get; set; }
        [MetaProperty(1422745546, BinPropertyType.Bool)]
        bool? m1422745546 { get; set; }
        [MetaProperty(3199620533, BinPropertyType.Bool)]
        bool? m3199620533 { get; set; }
        [MetaProperty(1721877131, BinPropertyType.String)]
        string? m1721877131 { get; set; }
        [MetaProperty("mWaitTimeout", BinPropertyType.Float)]
        float? WaitTimeout { get; set; }
    }
    [MetaClass("ContextualActionPlayVO")]
    public class ContextualActionPlayVO : ContextualActionPlayAudio
    {
        [MetaProperty("mSelfEventName", BinPropertyType.String)]
        public string? SelfEventName { get; set; }
        [MetaProperty("mAllyEventName", BinPropertyType.String)]
        public string? AllyEventName { get; set; }
        [MetaProperty("mEnemyEventName", BinPropertyType.String)]
        public string? EnemyEventName { get; set; }
        [MetaProperty("mSpectatorEventName", BinPropertyType.String)]
        public string? SpectatorEventName { get; set; }
        [MetaProperty("mWaitForAnnouncerQueue", BinPropertyType.Bool)]
        public bool? WaitForAnnouncerQueue { get; set; }
        [MetaProperty(1422745546, BinPropertyType.Bool)]
        public bool? m1422745546 { get; set; }
        [MetaProperty(3199620533, BinPropertyType.Bool)]
        public bool? m3199620533 { get; set; }
        [MetaProperty(1721877131, BinPropertyType.String)]
        public string? m1721877131 { get; set; }
        [MetaProperty("mWaitTimeout", BinPropertyType.Float)]
        public float? WaitTimeout { get; set; }
        [MetaProperty("mHashedSituationTrigger", BinPropertyType.Hash)]
        public MetaHash? HashedSituationTrigger { get; set; }
        [MetaProperty("mMaxOccurences", BinPropertyType.UInt32)]
        public uint? MaxOccurences { get; set; }
    }
    [MetaClass("ContextualActionData")]
    public class ContextualActionData : IResource
    {
        [MetaProperty("mCooldown", BinPropertyType.Float)]
        public float? Cooldown { get; set; }
        [MetaProperty(2681747634, BinPropertyType.Float)]
        public float? m2681747634 { get; set; }
        [MetaProperty("mSituations", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<ContextualSituation>> Situations { get; set; }
        [MetaProperty("mObjectPath", BinPropertyType.String)]
        public string? ObjectPath { get; set; }
    }
    [MetaClass("IContextualCondition")]
    public interface IContextualCondition : IMetaClass
    {
    }
    [MetaClass("ContextualConditionNegation")]
    public class ContextualConditionNegation : IContextualCondition
    {
        [MetaProperty("mChildCondition", BinPropertyType.Structure)]
        public IContextualCondition ChildCondition { get; set; }
    }
    [MetaClass("ContextualConditionItemID")]
    public class ContextualConditionItemID : IContextualCondition
    {
        [MetaProperty("mItems", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Items { get; set; }
    }
    [MetaClass("ContextualConditionMultikillSize")]
    public class ContextualConditionMultikillSize : IContextualCondition
    {
        [MetaProperty("mMultikillSize", BinPropertyType.Byte)]
        public byte? MultikillSize { get; set; }
    }
    [MetaClass("ContextualConditionKillCount")]
    public class ContextualConditionKillCount : IContextualCondition
    {
        [MetaProperty("mTotalKills", BinPropertyType.UInt16)]
        public ushort? TotalKills { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("ContextualConditionItemVOGroup")]
    public class ContextualConditionItemVOGroup : IContextualCondition
    {
        [MetaProperty("mItemVOGroupHash", BinPropertyType.Hash)]
        public MetaHash? ItemVOGroupHash { get; set; }
    }
    [MetaClass("ContextualConditionHasItemFromVOGroup")]
    public class ContextualConditionHasItemFromVOGroup : IContextualCondition
    {
        [MetaProperty("mItemVOGroupHash", BinPropertyType.Hash)]
        public MetaHash? ItemVOGroupHash { get; set; }
    }
    [MetaClass("ContextualConditionLastBoughtItem")]
    public class ContextualConditionLastBoughtItem : IContextualCondition
    {
        [MetaProperty("mItem", BinPropertyType.Hash)]
        public MetaHash? Item { get; set; }
    }
    [MetaClass("ContextualConditionRuleCooldown")]
    public class ContextualConditionRuleCooldown : IContextualCondition
    {
        [MetaProperty("mRuleCooldown", BinPropertyType.Float)]
        public float? RuleCooldown { get; set; }
    }
    [MetaClass("ContextualConditionChanceToPlay")]
    public class ContextualConditionChanceToPlay : IContextualCondition
    {
        [MetaProperty("mPercentChanceToPlay", BinPropertyType.Byte)]
        public byte? PercentChanceToPlay { get; set; }
    }
    [MetaClass("ContextualConditionMoveDistance")]
    public class ContextualConditionMoveDistance : IContextualCondition
    {
        [MetaProperty("mDistance", BinPropertyType.Float)]
        public float? Distance { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("ContextualConditionCharacterLevel")]
    public class ContextualConditionCharacterLevel : IContextualCondition
    {
        [MetaProperty("mCharacterLevel", BinPropertyType.Byte)]
        public byte? CharacterLevel { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("ContextualConditionTimeSinceStealthStateChange")]
    public class ContextualConditionTimeSinceStealthStateChange : IContextualCondition
    {
        [MetaProperty(2648368516, BinPropertyType.Byte)]
        public byte? m2648368516 { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
        [MetaProperty(3998225092, BinPropertyType.Float)]
        public float? m3998225092 { get; set; }
    }
    [MetaClass("ContextualConditionMarkerName")]
    public class ContextualConditionMarkerName : IContextualCondition
    {
        [MetaProperty(253046910, BinPropertyType.Container)]
        public MetaContainer<string> m253046910 { get; set; }
    }
    [MetaClass("ContextualConditionNeutralMinionMapSide")]
    public class ContextualConditionNeutralMinionMapSide : IContextualCondition
    {
        [MetaProperty("mTeamCompareOp", BinPropertyType.Byte)]
        public byte? TeamCompareOp { get; set; }
    }
    [MetaClass("ContextualConditionNeutralMinionCampName")]
    public class ContextualConditionNeutralMinionCampName : IContextualCondition
    {
        [MetaProperty("mCampName", BinPropertyType.Hash)]
        public MetaHash? CampName { get; set; }
    }
    [MetaClass("ContextualConditionNeutralMinionCampIsAlive")]
    public class ContextualConditionNeutralMinionCampIsAlive : IContextualCondition
    {
        [MetaProperty("mCampIsAlive", BinPropertyType.Bool)]
        public bool? CampIsAlive { get; set; }
    }
    [MetaClass("ContextualConditionNeutralCampId")]
    public class ContextualConditionNeutralCampId : IContextualCondition
    {
        [MetaProperty(290168160, BinPropertyType.Byte)]
        public byte? m290168160 { get; set; }
    }
    [MetaClass("ContextualConditionSituationHasRecentlyRun")]
    public class ContextualConditionSituationHasRecentlyRun : IContextualCondition
    {
        [MetaProperty("mSituationNameHash", BinPropertyType.Hash)]
        public MetaHash? SituationNameHash { get; set; }
        [MetaProperty("mTime", BinPropertyType.Float)]
        public float? Time { get; set; }
    }
    [MetaClass("ContextualConditionMapID")]
    public class ContextualConditionMapID : IContextualCondition
    {
        [MetaProperty("mMapIDs", BinPropertyType.UInt32)]
        public uint? MapIDs { get; set; }
    }
    [MetaClass("ContextualConditionObjectiveTakeByMyTeam")]
    public class ContextualConditionObjectiveTakeByMyTeam : IContextualCondition
    {
        [MetaProperty("mTakenObjective", BinPropertyType.UInt32)]
        public uint? TakenObjective { get; set; }
    }
    [MetaClass("ContextualConditionMapRegionName")]
    public class ContextualConditionMapRegionName : IContextualCondition
    {
        [MetaProperty("mRegionType", BinPropertyType.Byte)]
        public byte? RegionType { get; set; }
        [MetaProperty("mRegionName", BinPropertyType.String)]
        public string? RegionName { get; set; }
    }
    [MetaClass("ContextualConditionTurretPosition")]
    public class ContextualConditionTurretPosition : IContextualCondition
    {
        [MetaProperty("mTurretPosition", BinPropertyType.Byte)]
        public byte? TurretPosition { get; set; }
    }
    [MetaClass("ContextualConditionHasGold")]
    public class ContextualConditionHasGold : IContextualCondition
    {
        [MetaProperty("mGold", BinPropertyType.Float)]
        public float? Gold { get; set; }
    }
    [MetaClass("ContextualConditionCustomTimer")]
    public class ContextualConditionCustomTimer : IContextualCondition
    {
        [MetaProperty("mCustomTimer", BinPropertyType.Float)]
        public float? CustomTimer { get; set; }
    }
    [MetaClass("ContextualConditionGameTimer")]
    public class ContextualConditionGameTimer : IContextualCondition
    {
        [MetaProperty("mGameTimeInMinutes", BinPropertyType.Float)]
        public float? GameTimeInMinutes { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("ContextualConditionShopOpenCount")]
    public class ContextualConditionShopOpenCount : IContextualCondition
    {
        [MetaProperty("mShopOpenCount", BinPropertyType.UInt32)]
        public uint? ShopOpenCount { get; set; }
    }
    [MetaClass("ContextualConditionShopCloseCount")]
    public class ContextualConditionShopCloseCount : IContextualCondition
    {
        [MetaProperty("mShopCloseCount", BinPropertyType.UInt32)]
        public uint? ShopCloseCount { get; set; }
    }
    [MetaClass("ContextualConditionItemPurchased")]
    public class ContextualConditionItemPurchased : IContextualCondition
    {
        [MetaProperty("mItemPurchased", BinPropertyType.Bool)]
        public bool? ItemPurchased { get; set; }
    }
    [MetaClass("ContextualConditionItemCanBePurchased")]
    public class ContextualConditionItemCanBePurchased : IContextualCondition
    {
        [MetaProperty("mItemCanBePurchased", BinPropertyType.Bool)]
        public bool? ItemCanBePurchased { get; set; }
    }
    [MetaClass("ContextualConditionItemPriceMinimum")]
    public class ContextualConditionItemPriceMinimum : IContextualCondition
    {
        [MetaProperty("mItemPriceMinimum", BinPropertyType.UInt32)]
        public uint? ItemPriceMinimum { get; set; }
    }
    [MetaClass("ContextualConditionOwnerTeamNetChampionKills")]
    public class ContextualConditionOwnerTeamNetChampionKills : IContextualCondition
    {
        [MetaProperty("mOwnerTeamNetKillAdvantage", BinPropertyType.Int32)]
        public int? OwnerTeamNetKillAdvantage { get; set; }
        [MetaProperty("mKillAdvantageCompareOp", BinPropertyType.Byte)]
        public byte? KillAdvantageCompareOp { get; set; }
        [MetaProperty("mTimeFrameSeconds", BinPropertyType.Float)]
        public float? TimeFrameSeconds { get; set; }
    }
    [MetaClass("ContextualConditionNearbyChampionCount")]
    public class ContextualConditionNearbyChampionCount : IContextualCondition
    {
        [MetaProperty("mTeamCompareOp", BinPropertyType.Byte)]
        public byte? TeamCompareOp { get; set; }
        [MetaProperty("mCount", BinPropertyType.UInt32)]
        public uint? Count { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("ContextualConditionNumberOfCharactersNearTargetPos")]
    public class ContextualConditionNumberOfCharactersNearTargetPos : IContextualCondition
    {
        [MetaProperty("mNumberOfCharacters", BinPropertyType.UInt32)]
        public uint? NumberOfCharacters { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
        [MetaProperty("mTeamCompareOp", BinPropertyType.Byte)]
        public byte? TeamCompareOp { get; set; }
    }
    [MetaClass("ContextualConditionEnemyDeathsNearby")]
    public class ContextualConditionEnemyDeathsNearby : IContextualCondition
    {
        [MetaProperty("mEnemyDeaths", BinPropertyType.UInt32)]
        public uint? EnemyDeaths { get; set; }
    }
    [MetaClass("ContextualConditionTeammateDeathsNearby")]
    public class ContextualConditionTeammateDeathsNearby : IContextualCondition
    {
        [MetaProperty("mTeammateDeaths", BinPropertyType.UInt32)]
        public uint? TeammateDeaths { get; set; }
    }
    [MetaClass("ContextualConditionCharacter")]
    public class ContextualConditionCharacter : IContextualCondition
    {
        [MetaProperty("mCharacterType", BinPropertyType.Byte)]
        public byte? CharacterType { get; set; }
        [MetaProperty("mChildConditions", BinPropertyType.Container)]
        public MetaContainer<ICharacterSubcondition> ChildConditions { get; set; }
    }
    [MetaClass("ContextualConditionAnyOtherHero")]
    public class ContextualConditionAnyOtherHero : IContextualCondition
    {
        [MetaProperty("mChildConditions", BinPropertyType.Container)]
        public MetaContainer<ICharacterSubcondition> ChildConditions { get; set; }
    }
    [MetaClass("ICharacterSubcondition")]
    public interface ICharacterSubcondition : IMetaClass
    {
    }
    [MetaClass("ContextualConditionCharacterName")]
    public class ContextualConditionCharacterName : ICharacterSubcondition
    {
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Characters { get; set; }
    }
    [MetaClass("ContextualConditionCharacterFormName")]
    public class ContextualConditionCharacterFormName : ICharacterSubcondition
    {
        [MetaProperty("mFormName", BinPropertyType.String)]
        public string? FormName { get; set; }
    }
    [MetaClass("ContextualConditionCharacterSkinID")]
    public class ContextualConditionCharacterSkinID : ICharacterSubcondition
    {
        [MetaProperty("mSkinIDs", BinPropertyType.Container)]
        public MetaContainer<int> SkinIDs { get; set; }
    }
    [MetaClass("ContextualConditionCharacterMetadata")]
    public class ContextualConditionCharacterMetadata : ICharacterSubcondition
    {
        [MetaProperty("mCategory", BinPropertyType.String)]
        public string? Category { get; set; }
        [MetaProperty("mData", BinPropertyType.String)]
        public string? Data { get; set; }
    }
    [MetaClass("ContextualConditionCharacterUnitTags")]
    public class ContextualConditionCharacterUnitTags : ICharacterSubcondition
    {
        [MetaProperty("mTagMode", BinPropertyType.Byte)]
        public byte? TagMode { get; set; }
        [MetaProperty("mUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> UnitTags { get; set; }
    }
    [MetaClass("ContextualConditionCharacterHealth")]
    public class ContextualConditionCharacterHealth : ICharacterSubcondition
    {
        [MetaProperty("mTargetHealth", BinPropertyType.Float)]
        public float? TargetHealth { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("ContextualConditionIsAlly")]
    public class ContextualConditionIsAlly : ICharacterSubcondition
    {
        [MetaProperty("mIsAlly", BinPropertyType.Bool)]
        public bool? IsAlly { get; set; }
    }
    [MetaClass("ContextualConditionCharacterPlayingEmote")]
    public class ContextualConditionCharacterPlayingEmote : ICharacterSubcondition
    {
        [MetaProperty("mEmoteID", BinPropertyType.Byte)]
        public byte? EmoteID { get; set; }
    }
    [MetaClass("ContextualConditionCharacterPlayingAnimation")]
    public class ContextualConditionCharacterPlayingAnimation : ICharacterSubcondition
    {
        [MetaProperty("mAnimationNameHash", BinPropertyType.Hash)]
        public MetaHash? AnimationNameHash { get; set; }
    }
    [MetaClass("ContextualConditionCharacterDistance")]
    public class ContextualConditionCharacterDistance : ICharacterSubcondition
    {
        [MetaProperty("mDistance", BinPropertyType.Float)]
        public float? Distance { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
        [MetaProperty("mDistanceTarget", BinPropertyType.Byte)]
        public byte? DistanceTarget { get; set; }
    }
    [MetaClass("ContextualConditionCharacterInRangeForSyncedAnimation")]
    public class ContextualConditionCharacterInRangeForSyncedAnimation : ICharacterSubcondition
    {
    }
    [MetaClass("ContextualConditionCharacterHasTimeRemainingForAnimation")]
    public class ContextualConditionCharacterHasTimeRemainingForAnimation : ICharacterSubcondition
    {
        [MetaProperty("mAnimationName", BinPropertyType.Hash)]
        public MetaHash? AnimationName { get; set; }
        [MetaProperty("mMinRemainingTime", BinPropertyType.Float)]
        public float? MinRemainingTime { get; set; }
    }
    [MetaClass("ContextualConditionCharacterIsCastingRecall")]
    public class ContextualConditionCharacterIsCastingRecall : ICharacterSubcondition
    {
    }
    [MetaClass("ContextualConditionCharacterRole")]
    public class ContextualConditionCharacterRole : ICharacterSubcondition
    {
        [MetaProperty("mRole", BinPropertyType.Byte)]
        public byte? Role { get; set; }
    }
    [MetaClass("ContextualConditionCharacterHasCAC")]
    public class ContextualConditionCharacterHasCAC : ICharacterSubcondition
    {
        [MetaProperty(201153598, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m201153598 { get; set; }
    }
    [MetaClass("IContextualConditionSpell")]
    public interface IContextualConditionSpell : IContextualCondition
    {
    }
    [MetaClass("ContextualConditionSpell")]
    public class ContextualConditionSpell : IContextualCondition
    {
        [MetaProperty("mSpellSlot", BinPropertyType.Byte)]
        public byte? SpellSlot { get; set; }
        [MetaProperty("mChildConditions", BinPropertyType.Container)]
        public MetaContainer<IContextualConditionSpell> ChildConditions { get; set; }
    }
    [MetaClass("ContextualConditionSpellName")]
    public class ContextualConditionSpellName : IContextualConditionSpell
    {
        [MetaProperty("mSpell", BinPropertyType.Hash)]
        public MetaHash? Spell { get; set; }
    }
    [MetaClass("ContextualConditionSpellSlot")]
    public class ContextualConditionSpellSlot : IContextualConditionSpell
    {
        [MetaProperty("mSpellSlot", BinPropertyType.Byte)]
        public byte? SpellSlot { get; set; }
    }
    [MetaClass("ContextualConditionSpellLevel")]
    public class ContextualConditionSpellLevel : IContextualConditionSpell
    {
        [MetaProperty("mSpellLevel", BinPropertyType.Byte)]
        public byte? SpellLevel { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("ContextualConditionSpellIsReady")]
    public class ContextualConditionSpellIsReady : IContextualConditionSpell
    {
        [MetaProperty("mSpellIsReady", BinPropertyType.Bool)]
        public bool? SpellIsReady { get; set; }
    }
    [MetaClass(4210162581)]
    public interface Class4210162581 : IContextualCondition
    {
    }
    [MetaClass(68729178)]
    public class Class68729178 : Class4210162581
    {
        [MetaProperty("mBuff", BinPropertyType.Hash)]
        public MetaHash? Buff { get; set; }
        [MetaProperty(287338010, BinPropertyType.Byte)]
        public byte? m287338010 { get; set; }
    }
    [MetaClass(3101122117)]
    public class Class3101122117 : Class4210162581
    {
        [MetaProperty("mBuff", BinPropertyType.Hash)]
        public MetaHash? Buff { get; set; }
        [MetaProperty(287338010, BinPropertyType.Byte)]
        public byte? m287338010 { get; set; }
    }
    [MetaClass("ContextualRule")]
    public class ContextualRule : IMetaClass
    {
        [MetaProperty("mConditionRelationship", BinPropertyType.UInt32)]
        public uint? ConditionRelationship { get; set; }
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<IContextualCondition> Conditions { get; set; }
        [MetaProperty("mAudioAction", BinPropertyType.Structure)]
        public ContextualActionPlayAudio AudioAction { get; set; }
        [MetaProperty("mAnimationAction", BinPropertyType.Structure)]
        public ContextualActionPlayAnimation AnimationAction { get; set; }
        [MetaProperty("mTriggerEventAction", BinPropertyType.Structure)]
        public ContextualActionTriggerEvent TriggerEventAction { get; set; }
        [MetaProperty("mPriority", BinPropertyType.Optional)]
        public MetaOptional<uint> Priority { get; set; }
        [MetaProperty(1761534916, BinPropertyType.Bool)]
        public bool? m1761534916 { get; set; }
    }
    [MetaClass("ContextualSituation")]
    public class ContextualSituation : IMetaClass
    {
        [MetaProperty("mChooseRandomValidRule", BinPropertyType.Bool)]
        public bool? ChooseRandomValidRule { get; set; }
        [MetaProperty("mCoolDownTime", BinPropertyType.Float)]
        public float? CoolDownTime { get; set; }
        [MetaProperty("mRules", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ContextualRule>> Rules { get; set; }
    }
    [MetaClass("DamageSourceTemplate")]
    public class DamageSourceTemplate : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("DamageProperties", BinPropertyType.UInt32)]
        public uint? DamageProperties { get; set; }
        [MetaProperty("DamageTags", BinPropertyType.Container)]
        public MetaContainer<string> DamageTags { get; set; }
    }
    [MetaClass("DamageSourceSettings")]
    public class DamageSourceSettings : IMetaClass
    {
        [MetaProperty("damageTagDefinition", BinPropertyType.Container)]
        public MetaContainer<string> DamageTagDefinition { get; set; }
        [MetaProperty("templateDefinition", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DamageSourceTemplate>> TemplateDefinition { get; set; }
    }
    [MetaClass("DeathTimesScalingPoint")]
    public class DeathTimesScalingPoint : IMetaClass
    {
        [MetaProperty("mStartTime", BinPropertyType.UInt32)]
        public uint? StartTime { get; set; }
        [MetaProperty("mPercentIncrease", BinPropertyType.Float)]
        public float? PercentIncrease { get; set; }
    }
    [MetaClass("DeathTimes")]
    public class DeathTimes : IMetaClass
    {
        [MetaProperty("mTimeDeadPerLevel", BinPropertyType.Container)]
        public MetaContainer<float> TimeDeadPerLevel { get; set; }
        [MetaProperty("mScalingStartTime", BinPropertyType.UInt32)]
        public uint? ScalingStartTime { get; set; }
        [MetaProperty("mScalingIncrementTime", BinPropertyType.UInt32)]
        public uint? ScalingIncrementTime { get; set; }
        [MetaProperty("mScalingPercentIncrease", BinPropertyType.Float)]
        public float? ScalingPercentIncrease { get; set; }
        [MetaProperty("mScalingPercentCap", BinPropertyType.Float)]
        public float? ScalingPercentCap { get; set; }
        [MetaProperty("mScalingPoints", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DeathTimesScalingPoint>> ScalingPoints { get; set; }
        [MetaProperty("mAllowRespawnMods", BinPropertyType.Bool)]
        public bool? AllowRespawnMods { get; set; }
        [MetaProperty("mStartDeathTimerForZombies", BinPropertyType.Bool)]
        public bool? StartDeathTimerForZombies { get; set; }
    }
    [MetaClass("AbilityResourceStateColorOptions")]
    public class AbilityResourceStateColorOptions : IMetaClass
    {
        [MetaProperty("color", BinPropertyType.Color)]
        public Color? Color { get; set; }
        [MetaProperty("fadeColor", BinPropertyType.Color)]
        public Color? FadeColor { get; set; }
    }
    [MetaClass("AbilityResourceStateData")]
    public class AbilityResourceStateData : IMetaClass
    {
        [MetaProperty("DefaultPalette", BinPropertyType.Structure)]
        public AbilityResourceStateColorOptions DefaultPalette { get; set; }
        [MetaProperty("ColorblindPalette", BinPropertyType.Structure)]
        public AbilityResourceStateColorOptions ColorblindPalette { get; set; }
        [MetaProperty("textureSuffix", BinPropertyType.String)]
        public string? TextureSuffix { get; set; }
        [MetaProperty("animationSuffix", BinPropertyType.String)]
        public string? AnimationSuffix { get; set; }
    }
    [MetaClass("AbilityResourceThresholdIndicatorRange")]
    public class AbilityResourceThresholdIndicatorRange : IMetaClass
    {
        [MetaProperty("rangeStart", BinPropertyType.Float)]
        public float? RangeStart { get; set; }
        [MetaProperty("rangeEnd", BinPropertyType.Float)]
        public float? RangeEnd { get; set; }
    }
    [MetaClass("AbilityResourceTypeData")]
    public class AbilityResourceTypeData : IMetaClass
    {
        [MetaProperty("states", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AbilityResourceStateData>> States { get; set; }
        [MetaProperty("showAbilityResource", BinPropertyType.Bool)]
        public bool? ShowAbilityResource { get; set; }
        [MetaProperty("showRegen", BinPropertyType.Bool)]
        public bool? ShowRegen { get; set; }
        [MetaProperty(61046271, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AbilityResourceThresholdIndicatorRange>> m61046271 { get; set; }
    }
    [MetaClass("AbilityResourceTypeConfig")]
    public class AbilityResourceTypeConfig : IMetaClass
    {
        [MetaProperty("mana", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Mana { get; set; }
        [MetaProperty("Energy", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Energy { get; set; }
        [MetaProperty("None", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> None { get; set; }
        [MetaProperty("Shield", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Shield { get; set; }
        [MetaProperty("BattleFury", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> BattleFury { get; set; }
        [MetaProperty("DragonFury", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> DragonFury { get; set; }
        [MetaProperty("Rage", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Rage { get; set; }
        [MetaProperty("Heat", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Heat { get; set; }
        [MetaProperty("PrimalFury", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> PrimalFury { get; set; }
        [MetaProperty("Ferocity", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Ferocity { get; set; }
        [MetaProperty("Bloodwell", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Bloodwell { get; set; }
        [MetaProperty("Wind", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Wind { get; set; }
        [MetaProperty("Ammo", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Ammo { get; set; }
        [MetaProperty("Moonlight", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Moonlight { get; set; }
        [MetaProperty("Other", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceTypeData> Other { get; set; }
    }
    [MetaClass("EvolutionDescription")]
    public class EvolutionDescription : IMetaClass
    {
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mTitle", BinPropertyType.String)]
        public string? Title { get; set; }
        [MetaProperty("mTooltips", BinPropertyType.Container)]
        public MetaContainer<string> Tooltips { get; set; }
        [MetaProperty("mIconNames", BinPropertyType.Container)]
        public MetaContainer<string> IconNames { get; set; }
    }
    [MetaClass("ExperienceCurveData")]
    public class ExperienceCurveData : IMetaClass
    {
        [MetaProperty("mExperienceRequiredPerLevel", BinPropertyType.Container)]
        public MetaContainer<float> ExperienceRequiredPerLevel { get; set; }
        [MetaProperty("mExperienceGrantedForKillPerLevel", BinPropertyType.Container)]
        public MetaContainer<float> ExperienceGrantedForKillPerLevel { get; set; }
        [MetaProperty(2575366702, BinPropertyType.Container)]
        public MetaContainer<float> m2575366702 { get; set; }
        [MetaProperty("mBaseExperienceMultiplier", BinPropertyType.Float)]
        public float? BaseExperienceMultiplier { get; set; }
        [MetaProperty("mLevelDifferenceExperienceMultiplier", BinPropertyType.Float)]
        public float? LevelDifferenceExperienceMultiplier { get; set; }
        [MetaProperty("mMinimumExperienceMultiplier", BinPropertyType.Float)]
        public float? MinimumExperienceMultiplier { get; set; }
    }
    [MetaClass("ExperienceModData")]
    public class ExperienceModData : IMetaClass
    {
        [MetaProperty("mPlayerMinionSplitXp", BinPropertyType.Container)]
        public MetaContainer<float> PlayerMinionSplitXp { get; set; }
    }
    [MetaClass("StatUIData")]
    public class StatUIData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mAbbreviation", BinPropertyType.String)]
        public string? Abbreviation { get; set; }
        [MetaProperty("mDisplayType", BinPropertyType.Byte)]
        public byte? DisplayType { get; set; }
        [MetaProperty(9297440, BinPropertyType.String)]
        public string? m9297440 { get; set; }
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        public string? ScalingTagKey { get; set; }
    }
    [MetaClass("GlobalStatsUIData")]
    public class GlobalStatsUIData : IMetaClass
    {
        [MetaProperty("mStatUIData", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<StatUIData>> StatUIData { get; set; }
        [MetaProperty("mNumberStyle", BinPropertyType.String)]
        public string? NumberStyle { get; set; }
        [MetaProperty("mNumberStylePercent", BinPropertyType.String)]
        public string? NumberStylePercent { get; set; }
        [MetaProperty("mNumberStyleBonus", BinPropertyType.String)]
        public string? NumberStyleBonus { get; set; }
        [MetaProperty("mNumberStyleBonusPercent", BinPropertyType.String)]
        public string? NumberStyleBonusPercent { get; set; }
        [MetaProperty(2611516976, BinPropertyType.String)]
        public string? m2611516976 { get; set; }
        [MetaProperty(363059393, BinPropertyType.String)]
        public string? m363059393 { get; set; }
        [MetaProperty(517944321, BinPropertyType.String)]
        public string? m517944321 { get; set; }
        [MetaProperty(3885466478, BinPropertyType.String)]
        public string? m3885466478 { get; set; }
        [MetaProperty("mNumberStyleTotalAndCoefficient", BinPropertyType.String)]
        public string? NumberStyleTotalAndCoefficient { get; set; }
        [MetaProperty("mNumberStyleTotalAndCoefficientPercent", BinPropertyType.String)]
        public string? NumberStyleTotalAndCoefficientPercent { get; set; }
        [MetaProperty(3901102167, BinPropertyType.String)]
        public string? m3901102167 { get; set; }
        [MetaProperty(178171280, BinPropertyType.String)]
        public string? m178171280 { get; set; }
        [MetaProperty(3983319277, BinPropertyType.String)]
        public string? m3983319277 { get; set; }
        [MetaProperty(3917617343, BinPropertyType.String)]
        public string? m3917617343 { get; set; }
        [MetaProperty(799458480, BinPropertyType.String)]
        public string? m799458480 { get; set; }
        [MetaProperty(1254137583, BinPropertyType.String)]
        public string? m1254137583 { get; set; }
        [MetaProperty(4031521229, BinPropertyType.Byte)]
        public byte? m4031521229 { get; set; }
        [MetaProperty(2823080650, BinPropertyType.Byte)]
        public byte? m2823080650 { get; set; }
    }
    [MetaClass("CameraTrapezoid")]
    public class CameraTrapezoid : IMetaClass
    {
        [MetaProperty(2551311184, BinPropertyType.Float)]
        public float? m2551311184 { get; set; }
        [MetaProperty(2194368105, BinPropertyType.Float)]
        public float? m2194368105 { get; set; }
        [MetaProperty("mMaxXTop", BinPropertyType.Float)]
        public float? MaxXTop { get; set; }
        [MetaProperty("mMaxXBottom", BinPropertyType.Float)]
        public float? MaxXBottom { get; set; }
    }
    [MetaClass("CameraConfig")]
    public class CameraConfig : IMetaClass
    {
        [MetaProperty(108120199, BinPropertyType.Embedded)]
        public MetaEmbedded<CameraTrapezoid> m108120199 { get; set; }
        [MetaProperty("mAccelerationTimeMouse", BinPropertyType.Float)]
        public float? AccelerationTimeMouse { get; set; }
        [MetaProperty("mDecelerationTimeMouse", BinPropertyType.Float)]
        public float? DecelerationTimeMouse { get; set; }
        [MetaProperty("mAccelerationTimeKeyboard", BinPropertyType.Float)]
        public float? AccelerationTimeKeyboard { get; set; }
        [MetaProperty("mDecelerationTimeKeyboard", BinPropertyType.Float)]
        public float? DecelerationTimeKeyboard { get; set; }
        [MetaProperty("mTopdownZoom", BinPropertyType.Float)]
        public float? TopdownZoom { get; set; }
        [MetaProperty("mZoomMinDistance", BinPropertyType.Float)]
        public float? ZoomMinDistance { get; set; }
        [MetaProperty("mZoomMaxDistance", BinPropertyType.Float)]
        public float? ZoomMaxDistance { get; set; }
        [MetaProperty("mZoomEaseTime", BinPropertyType.Float)]
        public float? ZoomEaseTime { get; set; }
        [MetaProperty("mZoomMinSpeed", BinPropertyType.Float)]
        public float? ZoomMinSpeed { get; set; }
        [MetaProperty("mDragScale", BinPropertyType.Float)]
        public float? DragScale { get; set; }
        [MetaProperty("mDragMomentumDecay", BinPropertyType.Float)]
        public float? DragMomentumDecay { get; set; }
        [MetaProperty("mDragMomentumRecencyWeight", BinPropertyType.Float)]
        public float? DragMomentumRecencyWeight { get; set; }
        [MetaProperty("mLockedCameraEasingDistance", BinPropertyType.Float)]
        public float? LockedCameraEasingDistance { get; set; }
        [MetaProperty("mTransitionDurationIntoCinematicMode", BinPropertyType.Float)]
        public float? TransitionDurationIntoCinematicMode { get; set; }
        [MetaProperty(1909011002, BinPropertyType.Embedded)]
        public MetaEmbedded<CameraTrapezoid> m1909011002 { get; set; }
        [MetaProperty(4150359381, BinPropertyType.Embedded)]
        public MetaEmbedded<CameraTrapezoid> m4150359381 { get; set; }
        [MetaProperty(943673768, BinPropertyType.Float)]
        public float? m943673768 { get; set; }
    }
    [MetaClass("MapAudioDataProperties")]
    public class MapAudioDataProperties : IMetaClass
    {
        [MetaProperty(2231333056, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2231333056 { get; set; }
        [MetaProperty("bankUnits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BankUnit>> BankUnits { get; set; }
        [MetaProperty("features", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Features { get; set; }
    }
    [MetaClass("ClientStateAudioDataProperties")]
    public class ClientStateAudioDataProperties : IMetaClass
    {
        [MetaProperty(3322153993, BinPropertyType.Container)]
        public MetaContainer<string> m3322153993 { get; set; }
        [MetaProperty("themeMusic", BinPropertyType.String)]
        public string? ThemeMusic { get; set; }
    }
    [MetaClass("FeatureAudioDataProperties")]
    public class FeatureAudioDataProperties : IMetaClass
    {
        [MetaProperty("bankUnits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BankUnit>> BankUnits { get; set; }
        [MetaProperty("music", BinPropertyType.Embedded)]
        public MetaEmbedded<MusicAudioDataProperties> Music { get; set; }
        [MetaProperty("feature", BinPropertyType.Hash)]
        public MetaHash? Feature { get; set; }
    }
    [MetaClass("BankUnit")]
    public class BankUnit : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("bankPath", BinPropertyType.Container)]
        public MetaContainer<string> BankPath { get; set; }
        [MetaProperty("events", BinPropertyType.Container)]
        public MetaContainer<string> Events { get; set; }
        [MetaProperty("asynchrone", BinPropertyType.Bool)]
        public bool? Asynchrone { get; set; }
        [MetaProperty("voiceOver", BinPropertyType.Bool)]
        public bool? VoiceOver { get; set; }
    }
    [MetaClass("AudioTagListProperties")]
    public class AudioTagListProperties : IMetaClass
    {
        [MetaProperty("tags", BinPropertyType.Container)]
        public MetaContainer<string> Tags { get; set; }
        [MetaProperty("Key", BinPropertyType.String)]
        public string? Key { get; set; }
    }
    [MetaClass("AudioSystemDataProperties")]
    public class AudioSystemDataProperties : IMetaClass
    {
        [MetaProperty("systemTagEventList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AudioTagListProperties>> SystemTagEventList { get; set; }
    }
    [MetaClass("MusicAudioDataProperties")]
    public class MusicAudioDataProperties : IMetaClass
    {
        [MetaProperty("themeMusicID", BinPropertyType.String)]
        public string? ThemeMusicID { get; set; }
        [MetaProperty("themeMusicTransitionID", BinPropertyType.String)]
        public string? ThemeMusicTransitionID { get; set; }
        [MetaProperty("legacyThemeMusicID", BinPropertyType.String)]
        public string? LegacyThemeMusicID { get; set; }
        [MetaProperty("legacyThemeMusicTransitionID", BinPropertyType.String)]
        public string? LegacyThemeMusicTransitionID { get; set; }
        [MetaProperty("victoryMusicID", BinPropertyType.String)]
        public string? VictoryMusicID { get; set; }
        [MetaProperty("defeatMusicID", BinPropertyType.String)]
        public string? DefeatMusicID { get; set; }
        [MetaProperty("victoryBannerSound", BinPropertyType.String)]
        public string? VictoryBannerSound { get; set; }
        [MetaProperty("defeatBannerSound", BinPropertyType.String)]
        public string? DefeatBannerSound { get; set; }
        [MetaProperty("ambientEvent", BinPropertyType.String)]
        public string? AmbientEvent { get; set; }
        [MetaProperty("reverbPreset", BinPropertyType.String)]
        public string? ReverbPreset { get; set; }
    }
    [MetaClass("AudioStatusEvents")]
    public class AudioStatusEvents : IMetaClass
    {
        [MetaProperty("rtpcName", BinPropertyType.String)]
        public string? RtpcName { get; set; }
        [MetaProperty("startEvent", BinPropertyType.String)]
        public string? StartEvent { get; set; }
        [MetaProperty("stopEvent", BinPropertyType.String)]
        public string? StopEvent { get; set; }
    }
    [MetaClass("GlobalAudioDataProperties")]
    public class GlobalAudioDataProperties : IMetaClass
    {
        [MetaProperty("systems", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> Systems { get; set; }
        [MetaProperty("cooldownVoiceOver", BinPropertyType.Float)]
        public float? CooldownVoiceOver { get; set; }
        [MetaProperty("localPlayerStatusEvents", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<AudioStatusEvents>> LocalPlayerStatusEvents { get; set; }
    }
    [MetaClass("EVOSettings")]
    public class EVOSettings : IMetaClass
    {
        [MetaProperty("mEnableChatVO", BinPropertyType.Bool)]
        public bool? EnableChatVO { get; set; }
        [MetaProperty("mEnableAnnouncerVOReplacement", BinPropertyType.Bool)]
        public bool? EnableAnnouncerVOReplacement { get; set; }
        [MetaProperty("mChatVOThrottleCounterThreshold", BinPropertyType.Int32)]
        public int? ChatVOThrottleCounterThreshold { get; set; }
        [MetaProperty("mChatVOThrottleCounterDecayTime", BinPropertyType.Float)]
        public float? ChatVOThrottleCounterDecayTime { get; set; }
        [MetaProperty("mPingVOThrottleThreshold", BinPropertyType.Float)]
        public float? PingVOThrottleThreshold { get; set; }
    }
    [MetaClass("ICatalogEntryOwner")]
    public interface ICatalogEntryOwner : IMetaClass
    {
    }
    [MetaClass("CatalogEntry")]
    public class CatalogEntry : IMetaClass
    {
        [MetaProperty("contentId", BinPropertyType.String)]
        public string? ContentId { get; set; }
        [MetaProperty("itemID", BinPropertyType.UInt32)]
        public uint? ItemID { get; set; }
        [MetaProperty("offerId", BinPropertyType.String)]
        public string? OfferId { get; set; }
    }
    [MetaClass("CensoredImage")]
    public class CensoredImage : IMetaClass
    {
        [MetaProperty("image", BinPropertyType.String)]
        public string? Image { get; set; }
        [MetaProperty(862364035, BinPropertyType.Map)]
        public Dictionary<MetaHash, string> m862364035 { get; set; }
    }
    [MetaClass("Champion")]
    public class Champion : WadFileDescriptor
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("statStoneSets", BinPropertyType.Container)]
        public MetaContainer<MetaHash> StatStoneSets { get; set; }
        [MetaProperty(1665628256, BinPropertyType.Embedded)]
        public MetaEmbedded<ChampionItemRecommendations> m1665628256 { get; set; }
        [MetaProperty("additionalCharacters", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> AdditionalCharacters { get; set; }
        [MetaProperty("fixedLoadScreenPosition", BinPropertyType.SByte)]
        public sbyte? FixedLoadScreenPosition { get; set; }
    }
    [MetaClass("Character")]
    public class Character : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
    }
    [MetaClass("SkinCharacterDataProperties_CharacterIdleEffect")]
    public class SkinCharacterDataProperties_CharacterIdleEffect : IMetaClass
    {
        [MetaProperty("effectKey", BinPropertyType.Hash)]
        public MetaHash? EffectKey { get; set; }
        [MetaProperty("effectName", BinPropertyType.String)]
        public string? EffectName { get; set; }
        [MetaProperty("position", BinPropertyType.Vector3)]
        public Vector3? Position { get; set; }
        [MetaProperty("boneName", BinPropertyType.String)]
        public string? BoneName { get; set; }
        [MetaProperty("targetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
    }
    [MetaClass("SkinCharacterMetaDataProperties_SpawningSkinOffset")]
    public class SkinCharacterMetaDataProperties_SpawningSkinOffset : IMetaClass
    {
        [MetaProperty("tag", BinPropertyType.String)]
        public string? Tag { get; set; }
        [MetaProperty("offset", BinPropertyType.Int32)]
        public int? Offset { get; set; }
    }
    [MetaClass("ESportTeamEntry")]
    public class ESportTeamEntry : IMetaClass
    {
        [MetaProperty("teamName", BinPropertyType.String)]
        public string? TeamName { get; set; }
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string? LeagueName { get; set; }
        [MetaProperty("textureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
    }
    [MetaClass("ESportLeagueEntry")]
    public class ESportLeagueEntry : IMetaClass
    {
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string? LeagueName { get; set; }
        [MetaProperty("textureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
    }
    [MetaClass("SkinCharacterMetaDataProperties")]
    public class SkinCharacterMetaDataProperties : IMetaClass
    {
        [MetaProperty("eSportCharacter", BinPropertyType.Bool)]
        public bool? ESportCharacter { get; set; }
        [MetaProperty("eSportTeamTable", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ESportTeamEntry>> ESportTeamTable { get; set; }
        [MetaProperty("eSportLeagueTable", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ESportLeagueEntry>> ESportLeagueTable { get; set; }
        [MetaProperty("skinBasedRelativeColorScheme", BinPropertyType.Bool)]
        public bool? SkinBasedRelativeColorScheme { get; set; }
        [MetaProperty("isRelativeColorCharacter", BinPropertyType.Bool)]
        public bool? IsRelativeColorCharacter { get; set; }
        [MetaProperty("relativeColorSwapTable", BinPropertyType.Container)]
        public MetaContainer<int> RelativeColorSwapTable { get; set; }
        [MetaProperty("useAudioProperties", BinPropertyType.Bool)]
        public bool? UseAudioProperties { get; set; }
        [MetaProperty("spawningSkinOffsets", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinCharacterMetaDataProperties_SpawningSkinOffset>> SpawningSkinOffsets { get; set; }
        [MetaProperty("useGDSBinaries", BinPropertyType.Bool)]
        public bool? UseGDSBinaries { get; set; }
    }
    [MetaClass("SkinAudioProperties")]
    public class SkinAudioProperties : IMetaClass
    {
        [MetaProperty("tagEventList", BinPropertyType.Container)]
        public MetaContainer<string> TagEventList { get; set; }
        [MetaProperty("bankUnits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BankUnit>> BankUnits { get; set; }
    }
    [MetaClass("SkinAnimationProperties")]
    public class SkinAnimationProperties : IMetaClass
    {
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? AnimationGraphData { get; set; }
    }
    [MetaClass("SkinEmblem")]
    public class SkinEmblem : IMetaClass
    {
        [MetaProperty("mEmblemData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? EmblemData { get; set; }
        [MetaProperty("mLoadingScreenAnchor", BinPropertyType.UInt32)]
        public uint? LoadingScreenAnchor { get; set; }
    }
    [MetaClass("SkinSummonerEmoteLoadout")]
    public class SkinSummonerEmoteLoadout : IMetaClass
    {
        [MetaProperty("mEmotes", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Emotes { get; set; }
    }
    [MetaClass("SkinCharacterDataProperties")]
    public class SkinCharacterDataProperties : IMetaClass
    {
        [MetaProperty("skinClassification", BinPropertyType.UInt32)]
        public uint? SkinClassification { get; set; }
        [MetaProperty("championSkinName", BinPropertyType.String)]
        public string? ChampionSkinName { get; set; }
        [MetaProperty("attributeFlags", BinPropertyType.UInt32)]
        public uint? AttributeFlags { get; set; }
        [MetaProperty("skinParent", BinPropertyType.Int32)]
        public int? SkinParent { get; set; }
        [MetaProperty("metaDataTags", BinPropertyType.String)]
        public string? MetaDataTags { get; set; }
        [MetaProperty("emoteLoadout", BinPropertyType.Hash)]
        public MetaHash? EmoteLoadout { get; set; }
        [MetaProperty("skinUpgradeData", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinUpgradeData> SkinUpgradeData { get; set; }
        [MetaProperty("endOfGameAlias", BinPropertyType.String)]
        public string? EndOfGameAlias { get; set; }
        [MetaProperty(1182316791, BinPropertyType.String)]
        public string? m1182316791 { get; set; }
        [MetaProperty("loadscreen", BinPropertyType.Embedded)]
        public MetaEmbedded<CensoredImage> Loadscreen { get; set; }
        [MetaProperty("loadscreenVintage", BinPropertyType.Embedded)]
        public MetaEmbedded<CensoredImage> LoadscreenVintage { get; set; }
        [MetaProperty("skinAudioProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinAudioProperties> SkinAudioProperties { get; set; }
        [MetaProperty("skinAnimationProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinAnimationProperties> SkinAnimationProperties { get; set; }
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; }
        [MetaProperty("armorMaterial", BinPropertyType.String)]
        public string? ArmorMaterial { get; set; }
        [MetaProperty("themeMusic", BinPropertyType.Container)]
        public MetaContainer<string> ThemeMusic { get; set; }
        [MetaProperty(93493004, BinPropertyType.Bool)]
        public bool? m93493004 { get; set; }
        [MetaProperty(577901301, BinPropertyType.String)]
        public string? m577901301 { get; set; }
        [MetaProperty(1073641932, BinPropertyType.String)]
        public string? m1073641932 { get; set; }
        [MetaProperty("defaultAnimations", BinPropertyType.Container)]
        public MetaContainer<string> DefaultAnimations { get; set; }
        [MetaProperty("idleParticlesEffects", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect>> IdleParticlesEffects { get; set; }
        [MetaProperty("particleOverride_ChampionKillDeathParticle", BinPropertyType.String)]
        public string? ParticleOverride_ChampionKillDeathParticle { get; set; }
        [MetaProperty("particleOverride_DeathParticle", BinPropertyType.String)]
        public string? ParticleOverride_DeathParticle { get; set; }
        [MetaProperty("mSpawnParticleName", BinPropertyType.String)]
        public string? SpawnParticleName { get; set; }
        [MetaProperty("extraCharacterPreloads", BinPropertyType.Container)]
        public MetaContainer<string> ExtraCharacterPreloads { get; set; }
        [MetaProperty("voiceOverOverride", BinPropertyType.String)]
        public string? VoiceOverOverride { get; set; }
        [MetaProperty("skipVOOverride", BinPropertyType.Bool)]
        public bool? SkipVOOverride { get; set; }
        [MetaProperty("iconAvatar", BinPropertyType.String)]
        public string? IconAvatar { get; set; }
        [MetaProperty("mContextualActionData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ContextualActionData { get; set; }
        [MetaProperty("skinAudioNameOverride", BinPropertyType.String)]
        public string? SkinAudioNameOverride { get; set; }
        [MetaProperty("iconCircle", BinPropertyType.Optional)]
        public MetaOptional<string> IconCircle { get; set; }
        [MetaProperty("iconCircleScale", BinPropertyType.Optional)]
        public MetaOptional<float> IconCircleScale { get; set; }
        [MetaProperty("iconSquare", BinPropertyType.Optional)]
        public MetaOptional<string> IconSquare { get; set; }
        [MetaProperty("alternateIconsCircle", BinPropertyType.Container)]
        public MetaContainer<string> AlternateIconsCircle { get; set; }
        [MetaProperty("alternateIconsSquare", BinPropertyType.Container)]
        public MetaContainer<string> AlternateIconsSquare { get; set; }
        [MetaProperty("uncensoredIconCircles", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredIconCircles { get; set; }
        [MetaProperty("uncensoredIconSquares", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredIconSquares { get; set; }
        [MetaProperty("secondaryResourceHudDisplayData", BinPropertyType.Structure)]
        public ISecondaryResourceDisplayData SecondaryResourceHudDisplayData { get; set; }
        [MetaProperty("emoteBuffbone", BinPropertyType.String)]
        public string? EmoteBuffbone { get; set; }
        [MetaProperty("emoteYOffset", BinPropertyType.Float)]
        public float? EmoteYOffset { get; set; }
        [MetaProperty("godrayFXbone", BinPropertyType.String)]
        public string? GodrayFXbone { get; set; }
        [MetaProperty("healthBarData", BinPropertyType.Embedded)]
        public MetaEmbedded<CharacterHealthBarDataRecord> HealthBarData { get; set; }
        [MetaProperty("mEmblems", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinEmblem>> Emblems { get; set; }
        [MetaProperty("rarityGemOverride", BinPropertyType.Optional)]
        public MetaOptional<int> RarityGemOverride { get; set; }
        [MetaProperty("mResourceResolver", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ResourceResolver { get; set; }
        [MetaProperty(2188533552, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m2188533552 { get; set; }
        [MetaProperty(936257914, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudOptionalBinData>> m936257914 { get; set; }
    }
    [MetaClass(2393107013)]
    public class Class2393107013 : IMetaClass
    {
        [MetaProperty(3174838756, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3174838756 { get; set; }
        [MetaProperty(390986907, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m390986907 { get; set; }
        [MetaProperty(3840396934, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m3840396934 { get; set; }
        [MetaProperty("mChildSpells", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ChildSpells { get; set; }
        [MetaProperty(933770753, BinPropertyType.Byte)]
        public byte? m933770753 { get; set; }
        [MetaProperty(1665575238, BinPropertyType.Bool)]
        public bool? m1665575238 { get; set; }
        [MetaProperty("mSkinIDs", BinPropertyType.Container)]
        public MetaContainer<uint> SkinIDs { get; set; }
    }
    [MetaClass("GlobalPerLevelStatsFactor")]
    public class GlobalPerLevelStatsFactor : IMetaClass
    {
        [MetaProperty("mPerLevelStatsFactor", BinPropertyType.Container)]
        public MetaContainer<float> PerLevelStatsFactor { get; set; }
    }
    [MetaClass("OverrideAutoAttackCastTimeData")]
    public class OverrideAutoAttackCastTimeData : IMetaClass
    {
        [MetaProperty(2305377978, BinPropertyType.Structure)]
        public IGameCalculation m2305377978 { get; set; }
    }
    [MetaClass("CharacterRecord")]
    public class CharacterRecord : IMetaClass
    {
        [MetaProperty("mCharacterName", BinPropertyType.String)]
        public string? CharacterName { get; set; }
        [MetaProperty("mFallbackCharacterName", BinPropertyType.String)]
        public string? FallbackCharacterName { get; set; }
        [MetaProperty("targetLaserEffects", BinPropertyType.Structure)]
        public TargetLaserComponentEffects TargetLaserEffects { get; set; }
        [MetaProperty(3440493367, BinPropertyType.Structure)]
        public TargetLaserComponentEffects m3440493367 { get; set; }
        [MetaProperty("evolutionData", BinPropertyType.Structure)]
        public EvolutionDescription EvolutionData { get; set; }
        [MetaProperty("useableData", BinPropertyType.Embedded)]
        public MetaEmbedded<UseableData> UseableData { get; set; }
        [MetaProperty("baseHP", BinPropertyType.Float)]
        public float? BaseHP { get; set; }
        [MetaProperty("hpPerLevel", BinPropertyType.Float)]
        public float? HpPerLevel { get; set; }
        [MetaProperty("baseStaticHPRegen", BinPropertyType.Float)]
        public float? BaseStaticHPRegen { get; set; }
        [MetaProperty("baseFactorHPRegen", BinPropertyType.Float)]
        public float? BaseFactorHPRegen { get; set; }
        [MetaProperty("hpRegenPerLevel", BinPropertyType.Float)]
        public float? HpRegenPerLevel { get; set; }
        [MetaProperty("healthBarHeight", BinPropertyType.Float)]
        public float? HealthBarHeight { get; set; }
        [MetaProperty("healthBarFullParallax", BinPropertyType.Bool)]
        public bool? HealthBarFullParallax { get; set; }
        [MetaProperty("selfChampSpecificHealthSuffix", BinPropertyType.String)]
        public string? SelfChampSpecificHealthSuffix { get; set; }
        [MetaProperty("selfCBChampSpecificHealthSuffix", BinPropertyType.String)]
        public string? SelfCBChampSpecificHealthSuffix { get; set; }
        [MetaProperty("allyChampSpecificHealthSuffix", BinPropertyType.String)]
        public string? AllyChampSpecificHealthSuffix { get; set; }
        [MetaProperty("enemyChampSpecificHealthSuffix", BinPropertyType.String)]
        public string? EnemyChampSpecificHealthSuffix { get; set; }
        [MetaProperty(2711535288, BinPropertyType.Bool)]
        public bool? m2711535288 { get; set; }
        [MetaProperty("primaryAbilityResource", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceSlotInfo> PrimaryAbilityResource { get; set; }
        [MetaProperty("secondaryAbilityResource", BinPropertyType.Embedded)]
        public MetaEmbedded<AbilityResourceSlotInfo> SecondaryAbilityResource { get; set; }
        [MetaProperty("baseDamage", BinPropertyType.Float)]
        public float? BaseDamage { get; set; }
        [MetaProperty("damagePerLevel", BinPropertyType.Float)]
        public float? DamagePerLevel { get; set; }
        [MetaProperty("baseArmor", BinPropertyType.Float)]
        public float? BaseArmor { get; set; }
        [MetaProperty("armorPerLevel", BinPropertyType.Float)]
        public float? ArmorPerLevel { get; set; }
        [MetaProperty("baseSpellBlock", BinPropertyType.Float)]
        public float? BaseSpellBlock { get; set; }
        [MetaProperty("spellBlockPerLevel", BinPropertyType.Float)]
        public float? SpellBlockPerLevel { get; set; }
        [MetaProperty("baseDodge", BinPropertyType.Float)]
        public float? BaseDodge { get; set; }
        [MetaProperty("DodgePerLevel", BinPropertyType.Float)]
        public float? DodgePerLevel { get; set; }
        [MetaProperty("baseMissChance", BinPropertyType.Float)]
        public float? BaseMissChance { get; set; }
        [MetaProperty("baseCritChance", BinPropertyType.Float)]
        public float? BaseCritChance { get; set; }
        [MetaProperty("critDamageMultiplier", BinPropertyType.Float)]
        public float? CritDamageMultiplier { get; set; }
        [MetaProperty("critPerLevel", BinPropertyType.Float)]
        public float? CritPerLevel { get; set; }
        [MetaProperty("baseMoveSpeed", BinPropertyType.Float)]
        public float? BaseMoveSpeed { get; set; }
        [MetaProperty(3410252243, BinPropertyType.Float)]
        public float? m3410252243 { get; set; }
        [MetaProperty("attackRange", BinPropertyType.Float)]
        public float? AttackRange { get; set; }
        [MetaProperty("attackSpeed", BinPropertyType.Float)]
        public float? AttackSpeed { get; set; }
        [MetaProperty("attackSpeedRatio", BinPropertyType.Float)]
        public float? AttackSpeedRatio { get; set; }
        [MetaProperty("attackSpeedPerLevel", BinPropertyType.Float)]
        public float? AttackSpeedPerLevel { get; set; }
        [MetaProperty("AbilityPowerIncPerLevel", BinPropertyType.Float)]
        public float? AbilityPowerIncPerLevel { get; set; }
        [MetaProperty("mAdaptiveForceToAbilityPowerWeight", BinPropertyType.Float)]
        public float? AdaptiveForceToAbilityPowerWeight { get; set; }
        [MetaProperty("attackAutoInterruptPercent", BinPropertyType.Float)]
        public float? AttackAutoInterruptPercent { get; set; }
        [MetaProperty("acquisitionRange", BinPropertyType.Float)]
        public float? AcquisitionRange { get; set; }
        [MetaProperty("wakeUpRange", BinPropertyType.Optional)]
        public MetaOptional<float> WakeUpRange { get; set; }
        [MetaProperty("firstAcquisitionRange", BinPropertyType.Optional)]
        public MetaOptional<float> FirstAcquisitionRange { get; set; }
        [MetaProperty("basicAttack", BinPropertyType.Embedded)]
        public MetaEmbedded<AttackSlotData> BasicAttack { get; set; }
        [MetaProperty("extraAttacks", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AttackSlotData>> ExtraAttacks { get; set; }
        [MetaProperty("critAttacks", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<AttackSlotData>> CritAttacks { get; set; }
        [MetaProperty("towerTargetingPriorityBoost", BinPropertyType.Float)]
        public float? TowerTargetingPriorityBoost { get; set; }
        [MetaProperty("expGivenOnDeath", BinPropertyType.Float)]
        public float? ExpGivenOnDeath { get; set; }
        [MetaProperty("goldGivenOnDeath", BinPropertyType.Float)]
        public float? GoldGivenOnDeath { get; set; }
        [MetaProperty("goldRadius", BinPropertyType.Float)]
        public float? GoldRadius { get; set; }
        [MetaProperty("experienceRadius", BinPropertyType.Float)]
        public float? ExperienceRadius { get; set; }
        [MetaProperty("deathEventListeningRadius", BinPropertyType.Float)]
        public float? DeathEventListeningRadius { get; set; }
        [MetaProperty("localGoldGivenOnDeath", BinPropertyType.Float)]
        public float? LocalGoldGivenOnDeath { get; set; }
        [MetaProperty("localExpGivenOnDeath", BinPropertyType.Float)]
        public float? LocalExpGivenOnDeath { get; set; }
        [MetaProperty("localGoldSplitWithLastHitter", BinPropertyType.Bool)]
        public bool? LocalGoldSplitWithLastHitter { get; set; }
        [MetaProperty("globalGoldGivenOnDeath", BinPropertyType.Float)]
        public float? GlobalGoldGivenOnDeath { get; set; }
        [MetaProperty("globalExpGivenOnDeath", BinPropertyType.Float)]
        public float? GlobalExpGivenOnDeath { get; set; }
        [MetaProperty("perceptionBubbleRadius", BinPropertyType.Optional)]
        public MetaOptional<float> PerceptionBubbleRadius { get; set; }
        [MetaProperty("perceptionBoundingBoxSize", BinPropertyType.Optional)]
        public MetaOptional<Vector3> PerceptionBoundingBoxSize { get; set; }
        [MetaProperty("significance", BinPropertyType.Float)]
        public float? Significance { get; set; }
        [MetaProperty("untargetableSpawnTime", BinPropertyType.Float)]
        public float? UntargetableSpawnTime { get; set; }
        [MetaProperty("abilityPower", BinPropertyType.Float)]
        public float? AbilityPower { get; set; }
        [MetaProperty("spellNames", BinPropertyType.Container)]
        public MetaContainer<string> SpellNames { get; set; }
        [MetaProperty("extraSpells", BinPropertyType.Container)]
        public MetaContainer<string> ExtraSpells { get; set; }
        [MetaProperty("mAbilities", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Abilities { get; set; }
        [MetaProperty("onKillEvent", BinPropertyType.UInt32)]
        public uint? OnKillEvent { get; set; }
        [MetaProperty(2663075043, BinPropertyType.UInt32)]
        public uint? m2663075043 { get; set; }
        [MetaProperty("onKillEventForSpectator", BinPropertyType.UInt32)]
        public uint? OnKillEventForSpectator { get; set; }
        [MetaProperty("criticalAttack", BinPropertyType.String)]
        public string? CriticalAttack { get; set; }
        [MetaProperty("passiveName", BinPropertyType.String)]
        public string? PassiveName { get; set; }
        [MetaProperty("passiveLuaName", BinPropertyType.String)]
        public string? PassiveLuaName { get; set; }
        [MetaProperty("passiveToolTip", BinPropertyType.String)]
        public string? PassiveToolTip { get; set; }
        [MetaProperty("passiveSpell", BinPropertyType.String)]
        public string? PassiveSpell { get; set; }
        [MetaProperty("passiveRange", BinPropertyType.Float)]
        public float? PassiveRange { get; set; }
        [MetaProperty("passive1IconName", BinPropertyType.String)]
        public string? Passive1IconName { get; set; }
        [MetaProperty("lore1", BinPropertyType.String)]
        public string? Lore1 { get; set; }
        [MetaProperty("tips1", BinPropertyType.String)]
        public string? Tips1 { get; set; }
        [MetaProperty("tips2", BinPropertyType.String)]
        public string? Tips2 { get; set; }
        [MetaProperty("friendlyTooltip", BinPropertyType.String)]
        public string? FriendlyTooltip { get; set; }
        [MetaProperty("enemyTooltip", BinPropertyType.String)]
        public string? EnemyTooltip { get; set; }
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("parName", BinPropertyType.String)]
        public string? ParName { get; set; }
        [MetaProperty("weaponMaterials", BinPropertyType.Container)]
        public MetaContainer<string> WeaponMaterials { get; set; }
        [MetaProperty("hoverIndicatorTextureName", BinPropertyType.String)]
        public string? HoverIndicatorTextureName { get; set; }
        [MetaProperty("hoverIndicatorRadius", BinPropertyType.Float)]
        public float? HoverIndicatorRadius { get; set; }
        [MetaProperty("hoverLineIndicatorBaseTextureName", BinPropertyType.String)]
        public string? HoverLineIndicatorBaseTextureName { get; set; }
        [MetaProperty("hoverLineIndicatorTargetTextureName", BinPropertyType.String)]
        public string? HoverLineIndicatorTargetTextureName { get; set; }
        [MetaProperty("hoverLineIndicatorWidth", BinPropertyType.Float)]
        public float? HoverLineIndicatorWidth { get; set; }
        [MetaProperty("hoverIndicatorRotateToPlayer", BinPropertyType.Bool)]
        public bool? HoverIndicatorRotateToPlayer { get; set; }
        [MetaProperty("hoverIndicatorMinimapOverride", BinPropertyType.String)]
        public string? HoverIndicatorMinimapOverride { get; set; }
        [MetaProperty("minimapIconOverride", BinPropertyType.String)]
        public string? MinimapIconOverride { get; set; }
        [MetaProperty("hoverIndicatorRadiusMinimap", BinPropertyType.Float)]
        public float? HoverIndicatorRadiusMinimap { get; set; }
        [MetaProperty("hoverLineIndicatorWidthMinimap", BinPropertyType.Float)]
        public float? HoverLineIndicatorWidthMinimap { get; set; }
        [MetaProperty("areaIndicatorRadius", BinPropertyType.Float)]
        public float? AreaIndicatorRadius { get; set; }
        [MetaProperty("areaIndicatorMinRadius", BinPropertyType.Float)]
        public float? AreaIndicatorMinRadius { get; set; }
        [MetaProperty("areaIndicatorMaxDistance", BinPropertyType.Float)]
        public float? AreaIndicatorMaxDistance { get; set; }
        [MetaProperty("areaIndicatorTargetDistance", BinPropertyType.Float)]
        public float? AreaIndicatorTargetDistance { get; set; }
        [MetaProperty("areaIndicatorMinDistance", BinPropertyType.Float)]
        public float? AreaIndicatorMinDistance { get; set; }
        [MetaProperty("areaIndicatorTextureName", BinPropertyType.String)]
        public string? AreaIndicatorTextureName { get; set; }
        [MetaProperty("areaIndicatorTextureSize", BinPropertyType.Float)]
        public float? AreaIndicatorTextureSize { get; set; }
        [MetaProperty("charAudioNameOverride", BinPropertyType.String)]
        public string? CharAudioNameOverride { get; set; }
        [MetaProperty("mUseCCAnimations", BinPropertyType.Bool)]
        public bool? UseCCAnimations { get; set; }
        [MetaProperty("jointForAnimAdjustedSelection", BinPropertyType.String)]
        public string? JointForAnimAdjustedSelection { get; set; }
        [MetaProperty("outlineBBoxExpansion", BinPropertyType.Float)]
        public float? OutlineBBoxExpansion { get; set; }
        [MetaProperty("silhouetteAttachmentAnim", BinPropertyType.String)]
        public string? SilhouetteAttachmentAnim { get; set; }
        [MetaProperty("hitFxScale", BinPropertyType.Float)]
        public float? HitFxScale { get; set; }
        [MetaProperty("selectionHeight", BinPropertyType.Float)]
        public float? SelectionHeight { get; set; }
        [MetaProperty("selectionRadius", BinPropertyType.Float)]
        public float? SelectionRadius { get; set; }
        [MetaProperty("pathfindingCollisionRadius", BinPropertyType.Float)]
        public float? PathfindingCollisionRadius { get; set; }
        [MetaProperty("overrideGameplayCollisionRadius", BinPropertyType.Optional)]
        public MetaOptional<float> OverrideGameplayCollisionRadius { get; set; }
        [MetaProperty("unitTagsString", BinPropertyType.String)]
        public string? UnitTagsString { get; set; }
        [MetaProperty("friendlyUxOverrideTeam", BinPropertyType.UInt32)]
        public uint? FriendlyUxOverrideTeam { get; set; }
        [MetaProperty("friendlyUxOverrideIncludeTagsString", BinPropertyType.String)]
        public string? FriendlyUxOverrideIncludeTagsString { get; set; }
        [MetaProperty("friendlyUxOverrideExcludeTagsString", BinPropertyType.String)]
        public string? FriendlyUxOverrideExcludeTagsString { get; set; }
        [MetaProperty("mEducationToolData", BinPropertyType.Embedded)]
        public MetaEmbedded<ToolEducationData> EducationToolData { get; set; }
        [MetaProperty("mAbilitySlotCC", BinPropertyType.Container)]
        public MetaContainer<int> AbilitySlotCC { get; set; }
        [MetaProperty("characterToolData", BinPropertyType.Embedded)]
        public MetaEmbedded<CharacterToolData> CharacterToolData { get; set; }
        [MetaProperty("platformEnabled", BinPropertyType.Bool)]
        public bool? PlatformEnabled { get; set; }
        [MetaProperty("spellLevelUpInfo", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellLevelUpInfo>> SpellLevelUpInfo { get; set; }
        [MetaProperty("recSpellRankUpInfo", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<RecSpellRankUpInfo>> RecSpellRankUpInfo { get; set; }
        [MetaProperty("recordAsWard", BinPropertyType.Bool)]
        public bool? RecordAsWard { get; set; }
        [MetaProperty("minionScoreValue", BinPropertyType.Float)]
        public float? MinionScoreValue { get; set; }
        [MetaProperty("useRiotRelationships", BinPropertyType.Bool)]
        public bool? UseRiotRelationships { get; set; }
        [MetaProperty("flags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("minionFlags", BinPropertyType.UInt32)]
        public uint? MinionFlags { get; set; }
        [MetaProperty("assetCategory", BinPropertyType.String)]
        public string? AssetCategory { get; set; }
        [MetaProperty("purchaseIdentities", BinPropertyType.Container)]
        public MetaContainer<MetaHash> PurchaseIdentities { get; set; }
        [MetaProperty("mClientSideItemInventory", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ClientSideItemInventory { get; set; }
        [MetaProperty("mPreferredPerkStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink? PreferredPerkStyle { get; set; }
        [MetaProperty("mPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkReplacementList> PerkReplacements { get; set; }
        [MetaProperty("deathTime", BinPropertyType.Float)]
        public float? DeathTime { get; set; }
        [MetaProperty("occludedUnitSelectableDistance", BinPropertyType.Float)]
        public float? OccludedUnitSelectableDistance { get; set; }
        [MetaProperty("MovingTowardEnemyActivationAngle", BinPropertyType.Float)]
        public float? MovingTowardEnemyActivationAngle { get; set; }
        [MetaProperty(958843525, BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> m958843525 { get; set; }
        [MetaProperty(3916366866, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3916366866 { get; set; }
        [MetaProperty(3138631806, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class2393107013>> m3138631806 { get; set; }
    }
    [MetaClass("AbilityResourceSlotInfo")]
    public class AbilityResourceSlotInfo : IMetaClass
    {
        [MetaProperty("arType", BinPropertyType.Byte)]
        public byte? ArType { get; set; }
        [MetaProperty("arBase", BinPropertyType.Float)]
        public float? ArBase { get; set; }
        [MetaProperty("arPerLevel", BinPropertyType.Float)]
        public float? ArPerLevel { get; set; }
        [MetaProperty("arBaseStaticRegen", BinPropertyType.Float)]
        public float? ArBaseStaticRegen { get; set; }
        [MetaProperty("arBaseFactorRegen", BinPropertyType.Float)]
        public float? ArBaseFactorRegen { get; set; }
        [MetaProperty("arRegenPerLevel", BinPropertyType.Float)]
        public float? ArRegenPerLevel { get; set; }
        [MetaProperty("arIncrements", BinPropertyType.Float)]
        public float? ArIncrements { get; set; }
        [MetaProperty("arMaxSegments", BinPropertyType.Int32)]
        public int? ArMaxSegments { get; set; }
        [MetaProperty("arHasRegenText", BinPropertyType.Bool)]
        public bool? ArHasRegenText { get; set; }
        [MetaProperty("arAllowMaxValueToBeOverridden", BinPropertyType.Bool)]
        public bool? ArAllowMaxValueToBeOverridden { get; set; }
        [MetaProperty("arContributesToHealthValues", BinPropertyType.Bool)]
        public bool? ArContributesToHealthValues { get; set; }
        [MetaProperty("arPreventRegenWhileAtZero", BinPropertyType.Bool)]
        public bool? ArPreventRegenWhileAtZero { get; set; }
        [MetaProperty("arDisplayAsPips", BinPropertyType.Bool)]
        public bool? ArDisplayAsPips { get; set; }
        [MetaProperty("arIsShown", BinPropertyType.Bool)]
        public bool? ArIsShown { get; set; }
        [MetaProperty("arIsShownOnlyOnLocalPlayer", BinPropertyType.Bool)]
        public bool? ArIsShownOnlyOnLocalPlayer { get; set; }
        [MetaProperty("arOverrideSmallPipName", BinPropertyType.String)]
        public string? ArOverrideSmallPipName { get; set; }
        [MetaProperty(3733597457, BinPropertyType.String)]
        public string? m3733597457 { get; set; }
        [MetaProperty("arOverrideLargePipName", BinPropertyType.String)]
        public string? ArOverrideLargePipName { get; set; }
        [MetaProperty("arOverrideEmptyPipName", BinPropertyType.String)]
        public string? ArOverrideEmptyPipName { get; set; }
        [MetaProperty("arOverrideSpacerName", BinPropertyType.String)]
        public string? ArOverrideSpacerName { get; set; }
        [MetaProperty("arNegativeSpacer", BinPropertyType.Bool)]
        public bool? ArNegativeSpacer { get; set; }
        [MetaProperty(1554462912, BinPropertyType.Bool)]
        public bool? m1554462912 { get; set; }
        [MetaProperty(2849220732, BinPropertyType.Bool)]
        public bool? m2849220732 { get; set; }
    }
    [MetaClass("AttackSlotData")]
    public class AttackSlotData : IMetaClass
    {
        [MetaProperty("mAttackTotalTime", BinPropertyType.Optional)]
        public MetaOptional<float> AttackTotalTime { get; set; }
        [MetaProperty("mAttackCastTime", BinPropertyType.Optional)]
        public MetaOptional<float> AttackCastTime { get; set; }
        [MetaProperty(369242801, BinPropertyType.Structure)]
        public OverrideAutoAttackCastTimeData m369242801 { get; set; }
        [MetaProperty("mAttackDelayCastOffsetPercent", BinPropertyType.Optional)]
        public MetaOptional<float> AttackDelayCastOffsetPercent { get; set; }
        [MetaProperty("mAttackDelayCastOffsetPercentAttackSpeedRatio", BinPropertyType.Optional)]
        public MetaOptional<float> AttackDelayCastOffsetPercentAttackSpeedRatio { get; set; }
        [MetaProperty("mAttackProbability", BinPropertyType.Optional)]
        public MetaOptional<float> AttackProbability { get; set; }
        [MetaProperty("mAttackName", BinPropertyType.Optional)]
        public MetaOptional<string> AttackName { get; set; }
    }
    [MetaClass("TargetLaserComponentEffects")]
    public class TargetLaserComponentEffects : IMetaClass
    {
        [MetaProperty("beamEffectDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect> BeamEffectDefinition { get; set; }
        [MetaProperty("towerTargetingEffectDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect> TowerTargetingEffectDefinition { get; set; }
        [MetaProperty("champTargetingEffectDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinCharacterDataProperties_CharacterIdleEffect> ChampTargetingEffectDefinition { get; set; }
    }
    [MetaClass("ToolEducationData")]
    public class ToolEducationData : IMetaClass
    {
        [MetaProperty("firstItem", BinPropertyType.Int32)]
        public int? FirstItem { get; set; }
        [MetaProperty("skillOrder", BinPropertyType.Int32)]
        public int? SkillOrder { get; set; }
    }
    [MetaClass("CharacterToolData")]
    public class CharacterToolData : IMetaClass
    {
        [MetaProperty("searchTags", BinPropertyType.String)]
        public string? SearchTags { get; set; }
        [MetaProperty("searchTagsSecondary", BinPropertyType.String)]
        public string? SearchTagsSecondary { get; set; }
    }
    [MetaClass("Companion")]
    public class Companion : Character
    {
    }
    [MetaClass("InteractionData")]
    public class InteractionData : IMetaClass
    {
        [MetaProperty("idleAnim", BinPropertyType.String)]
        public string? IdleAnim { get; set; }
        [MetaProperty("shouldRandomizeIdleAnimPhase", BinPropertyType.Bool)]
        public bool? ShouldRandomizeIdleAnimPhase { get; set; }
    }
    [MetaClass("ISpellRankUpRequirement")]
    public interface ISpellRankUpRequirement : IMetaClass
    {
    }
    [MetaClass("HasSkillPointRequirement")]
    public class HasSkillPointRequirement : ISpellRankUpRequirement
    {
    }
    [MetaClass("CharacterLevelRequirement")]
    public class CharacterLevelRequirement : ISpellRankUpRequirement
    {
        [MetaProperty("mLevel", BinPropertyType.UInt32)]
        public uint? Level { get; set; }
    }
    [MetaClass("HasBuffRequirement")]
    public class HasBuffRequirement : ISpellRankUpRequirement
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash? BuffName { get; set; }
        [MetaProperty("mFromAnyone", BinPropertyType.Bool)]
        public bool? FromAnyone { get; set; }
    }
    [MetaClass("SpellRankUpRequirements")]
    public class SpellRankUpRequirements : IMetaClass
    {
        [MetaProperty("mRequirements", BinPropertyType.Container)]
        public MetaContainer<ISpellRankUpRequirement> Requirements { get; set; }
    }
    [MetaClass("SpellLevelUpInfo")]
    public class SpellLevelUpInfo : IMetaClass
    {
        [MetaProperty("mRequirements", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellRankUpRequirements>> Requirements { get; set; }
    }
    [MetaClass("RecSpellRankUpInfo")]
    public class RecSpellRankUpInfo : IMetaClass
    {
        [MetaProperty("mDefaultPriority", BinPropertyType.Container)]
        public MetaContainer<byte> DefaultPriority { get; set; }
        [MetaProperty("mEarlyLevelOverrides", BinPropertyType.Container)]
        public MetaContainer<byte> EarlyLevelOverrides { get; set; }
    }
    [MetaClass("UseableData")]
    public class UseableData : IMetaClass
    {
        [MetaProperty("flags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("useHeroSpellName", BinPropertyType.String)]
        public string? UseHeroSpellName { get; set; }
        [MetaProperty("useSpellName", BinPropertyType.String)]
        public string? UseSpellName { get; set; }
        [MetaProperty("useCooldownSpellSlot", BinPropertyType.Int32)]
        public int? UseCooldownSpellSlot { get; set; }
    }
    [MetaClass("CharacterHealthBarDataRecord")]
    public class CharacterHealthBarDataRecord : IMetaClass
    {
        [MetaProperty("hpPerTick", BinPropertyType.Float)]
        public float? HpPerTick { get; set; }
        [MetaProperty("attachToBone", BinPropertyType.String)]
        public string? AttachToBone { get; set; }
        [MetaProperty("unitHealthBarStyle", BinPropertyType.Byte)]
        public byte? UnitHealthBarStyle { get; set; }
        [MetaProperty("showWhileUntargetable", BinPropertyType.Bool)]
        public bool? ShowWhileUntargetable { get; set; }
        [MetaProperty(2622563520, BinPropertyType.Bool)]
        public bool? m2622563520 { get; set; }
        [MetaProperty(3884244271, BinPropertyType.UInt32)]
        public uint? m3884244271 { get; set; }
        [MetaProperty(1722275594, BinPropertyType.Bool)]
        public bool? m1722275594 { get; set; }
        [MetaProperty(2346514948, BinPropertyType.Bool)]
        public bool? m2346514948 { get; set; }
        [MetaProperty(2131456110, BinPropertyType.Bool)]
        public bool? m2131456110 { get; set; }
    }
    [MetaClass("SponsoredBanner")]
    public class SponsoredBanner : IMetaClass
    {
        [MetaProperty("banner", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Banner { get; set; }
        [MetaProperty(3625922223, BinPropertyType.String)]
        public string? m3625922223 { get; set; }
    }
    [MetaClass("EsportsBannerConfiguration")]
    public class EsportsBannerConfiguration : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(3114625026, BinPropertyType.Hash)]
        public MetaHash? m3114625026 { get; set; }
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string? LeagueName { get; set; }
        [MetaProperty(4043971103, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4043971103 { get; set; }
        [MetaProperty("texturePath", BinPropertyType.String)]
        public string? TexturePath { get; set; }
        [MetaProperty(631651853, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SponsoredBanner>> m631651853 { get; set; }
    }
    [MetaClass(761042637)]
    public class Class761042637 : IMetaClass
    {
        [MetaProperty(1064597388, BinPropertyType.String)]
        public string? m1064597388 { get; set; }
        [MetaProperty("Team", BinPropertyType.UInt32)]
        public uint? Team { get; set; }
    }
    [MetaClass("EsportsData")]
    public class EsportsData : IMetaClass
    {
        [MetaProperty("leagues", BinPropertyType.Container)]
        public MetaContainer<string> Leagues { get; set; }
    }
    [MetaClass(1951208621)]
    public class Class1951208621 : IMetaClass
    {
        [MetaProperty(4078196507, BinPropertyType.UInt32)]
        public uint? m4078196507 { get; set; }
        [MetaProperty(3865085164, BinPropertyType.UInt32)]
        public uint? m3865085164 { get; set; }
        [MetaProperty(1910587384, BinPropertyType.String)]
        public string? m1910587384 { get; set; }
    }
    [MetaClass("EsportsBannerOptions")]
    public class EsportsBannerOptions : IMetaClass
    {
        [MetaProperty("subMeshName", BinPropertyType.String)]
        public string? SubMeshName { get; set; }
        [MetaProperty(878849852, BinPropertyType.Bool)]
        public bool? m878849852 { get; set; }
        [MetaProperty("defaultTexturePath", BinPropertyType.String)]
        public string? DefaultTexturePath { get; set; }
        [MetaProperty(3590584789, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3590584789 { get; set; }
    }
    [MetaClass("MasteryBadgeData")]
    public class MasteryBadgeData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.Hash)]
        public MetaHash? Name { get; set; }
        [MetaProperty("mParticleName", BinPropertyType.String)]
        public string? ParticleName { get; set; }
        [MetaProperty("mRenderScale", BinPropertyType.Float)]
        public float? RenderScale { get; set; }
        [MetaProperty("mVerticalOffset", BinPropertyType.Float)]
        public float? VerticalOffset { get; set; }
        [MetaProperty("mSummonerIconId", BinPropertyType.Int32)]
        public int? SummonerIconId { get; set; }
        [MetaProperty("mMasteryLevel", BinPropertyType.UInt32)]
        public uint? MasteryLevel { get; set; }
    }
    [MetaClass("MasteryBadgeConfig")]
    public class MasteryBadgeConfig : IMetaClass
    {
        [MetaProperty("mBadges", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MasteryBadgeData>> Badges { get; set; }
    }
    [MetaClass("MasteryData")]
    public class MasteryData : IMetaClass
    {
        [MetaProperty("texture", BinPropertyType.String)]
        public string? Texture { get; set; }
        [MetaProperty(3077895061, BinPropertyType.String)]
        public string? m3077895061 { get; set; }
        [MetaProperty(3729097533, BinPropertyType.String)]
        public string? m3729097533 { get; set; }
    }
    [MetaClass("ChampionMasteryMap")]
    public class ChampionMasteryMap : IMetaClass
    {
        [MetaProperty("masteryData", BinPropertyType.Map)]
        public Dictionary<int, MetaObjectLink> MasteryData { get; set; }
    }
    [MetaClass("BaseLoadoutData")]
    public interface BaseLoadoutData : IMetaClass,  ICatalogEntryOwner
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        string? m1905664938 { get; set; }
    }
    [MetaClass("LoadoutFeatureData")]
    public class LoadoutFeatureData : IMetaClass
    {
        [MetaProperty("mFeature", BinPropertyType.UInt32)]
        public uint? Feature { get; set; }
        [MetaProperty("mMutator", BinPropertyType.Optional)]
        public MetaOptional<string> Mutator { get; set; }
        [MetaProperty("mBinaryFile", BinPropertyType.Optional)]
        public MetaOptional<string> BinaryFile { get; set; }
        [MetaProperty("mLoadoutCategory", BinPropertyType.String)]
        public string? LoadoutCategory { get; set; }
        [MetaProperty("mLoadoutProperties", BinPropertyType.Container)]
        public MetaContainer<string> LoadoutProperties { get; set; }
        [MetaProperty("mGDSObjectPathTemplates", BinPropertyType.Map)]
        public Dictionary<string, string> GDSObjectPathTemplates { get; set; }
        [MetaProperty("mLoadFromContentIds", BinPropertyType.Bool)]
        public bool? LoadFromContentIds { get; set; }
    }
    [MetaClass("RecallDecalData")]
    public class RecallDecalData : IMetaClass
    {
        [MetaProperty("effectFile", BinPropertyType.String)]
        public string? EffectFile { get; set; }
        [MetaProperty("arrivalEffectFile", BinPropertyType.String)]
        public string? ArrivalEffectFile { get; set; }
        [MetaProperty(3221725462, BinPropertyType.String)]
        public string? m3221725462 { get; set; }
        [MetaProperty(2784775912, BinPropertyType.String)]
        public string? m2784775912 { get; set; }
        [MetaProperty("recallDecalId", BinPropertyType.UInt32)]
        public uint? RecallDecalId { get; set; }
    }
    [MetaClass("GearSkinUpgrade")]
    public class GearSkinUpgrade : IMetaClass
    {
        [MetaProperty("mGearData", BinPropertyType.Structure)]
        public GearData GearData { get; set; }
        [MetaProperty(898435083, BinPropertyType.String)]
        public string? m898435083 { get; set; }
    }
    [MetaClass("SkinUpgradeData")]
    public class SkinUpgradeData : IMetaClass
    {
        [MetaProperty(3411158819, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m3411158819 { get; set; }
    }
    [MetaClass("TFTCompanionBucket")]
    public class TFTCompanionBucket : IMetaClass
    {
        [MetaProperty("Companions", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Companions { get; set; }
    }
    [MetaClass("TFTBotLoadoutConfiguration")]
    public class TFTBotLoadoutConfiguration : IMetaClass
    {
        [MetaProperty(4026254940, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTCompanionBucket>> m4026254940 { get; set; }
        [MetaProperty("mapSkins", BinPropertyType.Container)]
        public MetaContainer<MetaHash> MapSkins { get; set; }
    }
    [MetaClass("BannerFlagData")]
    public class BannerFlagData : IMetaClass
    {
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? AnimationGraphData { get; set; }
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; }
    }
    [MetaClass("BannerFrameData")]
    public class BannerFrameData : IMetaClass
    {
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? AnimationGraphData { get; set; }
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; }
    }
    [MetaClass("CompanionData")]
    public class CompanionData : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty("rarity", BinPropertyType.UInt32)]
        public uint? Rarity { get; set; }
        [MetaProperty("level", BinPropertyType.UInt32)]
        public uint? Level { get; set; }
        [MetaProperty(1460531328, BinPropertyType.Bool)]
        public bool? m1460531328 { get; set; }
        [MetaProperty("mCharacter", BinPropertyType.Hash)]
        public MetaHash? Character { get; set; }
        [MetaProperty("mSkinId", BinPropertyType.UInt32)]
        public uint? SkinId { get; set; }
        [MetaProperty("speciesLink", BinPropertyType.String)]
        public string? SpeciesLink { get; set; }
        [MetaProperty(3290732214, BinPropertyType.String)]
        public string? m3290732214 { get; set; }
        [MetaProperty(2243605595, BinPropertyType.String)]
        public string? m2243605595 { get; set; }
        [MetaProperty(2404894612, BinPropertyType.String)]
        public string? m2404894612 { get; set; }
        [MetaProperty("mLoadScreen", BinPropertyType.String)]
        public string? LoadScreen { get; set; }
    }
    [MetaClass("CompanionSpeciesData")]
    public class CompanionSpeciesData : IMetaClass
    {
        [MetaProperty("mSpeciesName", BinPropertyType.String)]
        public string? SpeciesName { get; set; }
        [MetaProperty("mSpeciesId", BinPropertyType.UInt32)]
        public uint? SpeciesId { get; set; }
    }
    [MetaClass("SummonerEmote")]
    public class SummonerEmote : IMetaClass
    {
        [MetaProperty("summonerEmoteId", BinPropertyType.UInt32)]
        public uint? SummonerEmoteId { get; set; }
        [MetaProperty("vfxSystem", BinPropertyType.ObjectLink)]
        public MetaObjectLink? VfxSystem { get; set; }
        [MetaProperty("announcementIcon", BinPropertyType.String)]
        public string? AnnouncementIcon { get; set; }
        [MetaProperty("selectionIcon", BinPropertyType.String)]
        public string? SelectionIcon { get; set; }
        [MetaProperty("renderScale", BinPropertyType.Float)]
        public float? RenderScale { get; set; }
        [MetaProperty("verticalOffset", BinPropertyType.Float)]
        public float? VerticalOffset { get; set; }
        [MetaProperty("visibleSelectionName", BinPropertyType.String)]
        public string? VisibleSelectionName { get; set; }
    }
    [MetaClass("SummonerEmoteSettings")]
    public class SummonerEmoteSettings : IMetaClass
    {
        [MetaProperty("mFirstBlood", BinPropertyType.ObjectLink)]
        public MetaObjectLink? FirstBlood { get; set; }
        [MetaProperty("mAce", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Ace { get; set; }
    }
    [MetaClass("GearData")]
    public class GearData : IMetaClass
    {
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; }
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? AnimationGraphData { get; set; }
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; }
        [MetaProperty("mEquipAnimation", BinPropertyType.String)]
        public string? EquipAnimation { get; set; }
        [MetaProperty(1725649758, BinPropertyType.String)]
        public string? m1725649758 { get; set; }
        [MetaProperty("mPortraitIcon", BinPropertyType.String)]
        public string? PortraitIcon { get; set; }
        [MetaProperty(3066053883, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m3066053883 { get; set; }
        [MetaProperty(565581438, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m565581438 { get; set; }
    }
    [MetaClass("ModeProgressionRewardData")]
    public class ModeProgressionRewardData : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string? BuffName { get; set; }
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        public MetaContainer<string> Characters { get; set; }
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; }
    }
    [MetaClass("RegaliaData")]
    public class RegaliaData : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty("texture", BinPropertyType.String)]
        public string? Texture { get; set; }
    }
    [MetaClass("RegaliaLookup")]
    public class RegaliaLookup : IMetaClass
    {
        [MetaProperty("tier", BinPropertyType.String)]
        public string? Tier { get; set; }
        [MetaProperty("regaliaCrest", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaCrest { get; set; }
        [MetaProperty("regaliaCrown1", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaCrown1 { get; set; }
        [MetaProperty("regaliaCrown2", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaCrown2 { get; set; }
        [MetaProperty("regaliaCrown3", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaCrown3 { get; set; }
        [MetaProperty("regaliaCrown4", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaCrown4 { get; set; }
        [MetaProperty(34629342, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m34629342 { get; set; }
        [MetaProperty("regaliaSplit1", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaSplit1 { get; set; }
        [MetaProperty("regaliaSplit2", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaSplit2 { get; set; }
        [MetaProperty("regaliaSplit3", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RegaliaSplit3 { get; set; }
    }
    [MetaClass("RegaliaRankedCrestEntry")]
    public class RegaliaRankedCrestEntry : IMetaClass
    {
        [MetaProperty("base", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Base { get; set; }
        [MetaProperty(4222747664, BinPropertyType.Map)]
        public Dictionary<int, MetaObjectLink> m4222747664 { get; set; }
        [MetaProperty(2939033354, BinPropertyType.Map)]
        public Dictionary<int, MetaObjectLink> m2939033354 { get; set; }
    }
    [MetaClass("RegaliaRankedCrestMap")]
    public class RegaliaRankedCrestMap : IMetaClass
    {
        [MetaProperty(1916628881, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<RegaliaRankedCrestEntry>> m1916628881 { get; set; }
    }
    [MetaClass("RegaliaPrestigeCrestList")]
    public class RegaliaPrestigeCrestList : IMetaClass
    {
        [MetaProperty(2584072672, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m2584072672 { get; set; }
    }
    [MetaClass("RegaliaRankedBannerMap")]
    public class RegaliaRankedBannerMap : IMetaClass
    {
        [MetaProperty(3317216616, BinPropertyType.Map)]
        public Dictionary<string, MetaObjectLink> m3317216616 { get; set; }
    }
    [MetaClass("StatStoneEventToTrack")]
    public class StatStoneEventToTrack : IMetaClass
    {
        [MetaProperty(3033949705, BinPropertyType.UInt32)]
        public uint? m3033949705 { get; set; }
        [MetaProperty(4226789542, BinPropertyType.Container)]
        public MetaContainer<IStatStoneLogicDriver> m4226789542 { get; set; }
    }
    [MetaClass("StatStoneData")]
    public class StatStoneData : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty(2415261508, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StatStoneEventToTrack>> m2415261508 { get; set; }
        [MetaProperty("category", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Category { get; set; }
        [MetaProperty(4213855983, BinPropertyType.UInt32)]
        public uint? m4213855983 { get; set; }
        [MetaProperty(678414787, BinPropertyType.UInt32)]
        public uint? m678414787 { get; set; }
        [MetaProperty(1401290250, BinPropertyType.Container)]
        public MetaContainer<ulong> m1401290250 { get; set; }
        [MetaProperty(1475661895, BinPropertyType.Bool)]
        public bool? m1475661895 { get; set; }
        [MetaProperty(2094353733, BinPropertyType.Bool)]
        public bool? m2094353733 { get; set; }
        [MetaProperty(487172646, BinPropertyType.Bool)]
        public bool? m487172646 { get; set; }
        [MetaProperty(868103188, BinPropertyType.Byte)]
        public byte? m868103188 { get; set; }
        [MetaProperty(2007295361, BinPropertyType.String)]
        public string? m2007295361 { get; set; }
        [MetaProperty(3940591590, BinPropertyType.Bool)]
        public bool? m3940591590 { get; set; }
    }
    [MetaClass("IStatStoneLogicDriver")]
    public interface IStatStoneLogicDriver : IMetaClass
    {
    }
    [MetaClass("TargetHasUnitTagFilter")]
    public class TargetHasUnitTagFilter : IStatStoneLogicDriver
    {
        [MetaProperty("UnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> UnitTags { get; set; }
    }
    [MetaClass("TrueDamageGivenFilter")]
    public class TrueDamageGivenFilter : IStatStoneLogicDriver
    {
    }
    [MetaClass("CrowdControlFilter")]
    public class CrowdControlFilter : IStatStoneLogicDriver
    {
        [MetaProperty(550470828, BinPropertyType.Container)]
        public MetaContainer<byte> m550470828 { get; set; }
        [MetaProperty(2443043316, BinPropertyType.Bool)]
        public bool? m2443043316 { get; set; }
    }
    [MetaClass("TargetHasBuffFilter")]
    public class TargetHasBuffFilter : IStatStoneLogicDriver
    {
        [MetaProperty(3672164089, BinPropertyType.Container)]
        public MetaContainer<byte> m3672164089 { get; set; }
    }
    [MetaClass("SourceTypeFilter")]
    public class SourceTypeFilter : IStatStoneLogicDriver
    {
        [MetaProperty(1203421971, BinPropertyType.Bool)]
        public bool? m1203421971 { get; set; }
        [MetaProperty(507497828, BinPropertyType.Bool)]
        public bool? m507497828 { get; set; }
        [MetaProperty(3588584256, BinPropertyType.Bool)]
        public bool? m3588584256 { get; set; }
    }
    [MetaClass("TargetTypeFilter")]
    public class TargetTypeFilter : IStatStoneLogicDriver
    {
        [MetaProperty(1203421971, BinPropertyType.Bool)]
        public bool? m1203421971 { get; set; }
        [MetaProperty(507497828, BinPropertyType.Bool)]
        public bool? m507497828 { get; set; }
        [MetaProperty(3588584256, BinPropertyType.Bool)]
        public bool? m3588584256 { get; set; }
    }
    [MetaClass("TargetTeamFilter")]
    public class TargetTeamFilter : IStatStoneLogicDriver
    {
        [MetaProperty("ally", BinPropertyType.Bool)]
        public bool? Ally { get; set; }
        [MetaProperty("enemy", BinPropertyType.Bool)]
        public bool? Enemy { get; set; }
        [MetaProperty("Self", BinPropertyType.Bool)]
        public bool? Self { get; set; }
    }
    [MetaClass("SourceLessThanHealthPercentageFilter")]
    public class SourceLessThanHealthPercentageFilter : IStatStoneLogicDriver
    {
        [MetaProperty("healthPercentage", BinPropertyType.Float)]
        public float? HealthPercentage { get; set; }
    }
    [MetaClass("AssistCountFilter")]
    public class AssistCountFilter : IStatStoneLogicDriver
    {
        [MetaProperty("assistCount", BinPropertyType.Byte)]
        public byte? AssistCount { get; set; }
    }
    [MetaClass("MultiKillLogic")]
    public class MultiKillLogic : IStatStoneLogicDriver
    {
    }
    [MetaClass("DamageShieldedLogic")]
    public class DamageShieldedLogic : IStatStoneLogicDriver
    {
        [MetaProperty(2715825086, BinPropertyType.Bool)]
        public bool? m2715825086 { get; set; }
        [MetaProperty(2034521402, BinPropertyType.Bool)]
        public bool? m2034521402 { get; set; }
        [MetaProperty(718612390, BinPropertyType.Bool)]
        public bool? m718612390 { get; set; }
        [MetaProperty(1646138587, BinPropertyType.Bool)]
        public bool? m1646138587 { get; set; }
        [MetaProperty(1622655414, BinPropertyType.Bool)]
        public bool? m1622655414 { get; set; }
        [MetaProperty(3418552506, BinPropertyType.Bool)]
        public bool? m3418552506 { get; set; }
        [MetaProperty(788241703, BinPropertyType.Bool)]
        public bool? m788241703 { get; set; }
        [MetaProperty(385993226, BinPropertyType.Bool)]
        public bool? m385993226 { get; set; }
    }
    [MetaClass("GoldSourceFilter")]
    public class GoldSourceFilter : IStatStoneLogicDriver
    {
        [MetaProperty(972553818, BinPropertyType.Byte)]
        public byte? m972553818 { get; set; }
    }
    [MetaClass("KillingSpreeFilter")]
    public class KillingSpreeFilter : IStatStoneLogicDriver
    {
        [MetaProperty("KillingSpreeCount", BinPropertyType.Int32)]
        public int? KillingSpreeCount { get; set; }
    }
    [MetaClass("NeutralMinionCampClearedLogic")]
    public class NeutralMinionCampClearedLogic : IStatStoneLogicDriver
    {
    }
    [MetaClass("TurretFirstBloodLogic")]
    public class TurretFirstBloodLogic : IStatStoneLogicDriver
    {
    }
    [MetaClass("StatStoneSet")]
    public class StatStoneSet : IMetaClass,  ICatalogEntryOwner
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty("statStones", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> StatStones { get; set; }
    }
    [MetaClass("StatStoneCategory")]
    public class StatStoneCategory : IMetaClass
    {
        [MetaProperty("gameIconUnlit", BinPropertyType.String)]
        public string? GameIconUnlit { get; set; }
        [MetaProperty("gameIconLit", BinPropertyType.String)]
        public string? GameIconLit { get; set; }
        [MetaProperty("gameIconFull", BinPropertyType.String)]
        public string? GameIconFull { get; set; }
        [MetaProperty("gameIconMini", BinPropertyType.String)]
        public string? GameIconMini { get; set; }
        [MetaProperty(3915826324, BinPropertyType.Color)]
        public Color? m3915826324 { get; set; }
    }
    [MetaClass(3939611513)]
    public class Class3939611513 : IMetaClass
    {
        [MetaProperty(4215291610, BinPropertyType.Map)]
        public Dictionary<uint, MetaObjectLink> m4215291610 { get; set; }
    }
    [MetaClass(851321958)]
    public class Class851321958 : IMetaClass
    {
        [MetaProperty(438884130, BinPropertyType.Map)]
        public Dictionary<uint, MetaObjectLink> m438884130 { get; set; }
    }
    [MetaClass("TFTDamageSkin")]
    public class TFTDamageSkin : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty("rarity", BinPropertyType.UInt32)]
        public uint? Rarity { get; set; }
        [MetaProperty(2698810810, BinPropertyType.String)]
        public string? m2698810810 { get; set; }
        [MetaProperty("level", BinPropertyType.UInt32)]
        public uint? Level { get; set; }
        [MetaProperty("disabled", BinPropertyType.Bool)]
        public bool? Disabled { get; set; }
        [MetaProperty("SkinID", BinPropertyType.UInt32)]
        public uint? SkinID { get; set; }
        [MetaProperty(833576390, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class3607046696>> m833576390 { get; set; }
        [MetaProperty(2679170533, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2679170533 { get; set; }
        [MetaProperty(1125948026, BinPropertyType.String)]
        public string? m1125948026 { get; set; }
        [MetaProperty(4185673651, BinPropertyType.String)]
        public string? m4185673651 { get; set; }
        [MetaProperty(991437061, BinPropertyType.Container)]
        public MetaContainer<string> m991437061 { get; set; }
        [MetaProperty(2127934631, BinPropertyType.Float)]
        public float? m2127934631 { get; set; }
    }
    [MetaClass(3607046696)]
    public class Class3607046696 : IMetaClass
    {
        [MetaProperty("EffectType", BinPropertyType.UInt32)]
        public uint? EffectType { get; set; }
        [MetaProperty(2634861147, BinPropertyType.UInt32)]
        public uint? m2634861147 { get; set; }
        [MetaProperty("effectKey", BinPropertyType.String)]
        public string? EffectKey { get; set; }
        [MetaProperty(2630749530, BinPropertyType.UInt32)]
        public uint? m2630749530 { get; set; }
        [MetaProperty(255342883, BinPropertyType.Float)]
        public float? m255342883 { get; set; }
    }
    [MetaClass("TftMapGroupData")]
    public class TftMapGroupData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mId", BinPropertyType.UInt32)]
        public uint? Id { get; set; }
    }
    [MetaClass("TftMapSkin")]
    public class TftMapSkin : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty("rarity", BinPropertyType.UInt32)]
        public uint? Rarity { get; set; }
        [MetaProperty(1125948026, BinPropertyType.String)]
        public string? m1125948026 { get; set; }
        [MetaProperty(4185673651, BinPropertyType.String)]
        public string? m4185673651 { get; set; }
        [MetaProperty(4216969820, BinPropertyType.String)]
        public string? m4216969820 { get; set; }
        [MetaProperty(991437061, BinPropertyType.Container)]
        public MetaContainer<string> m991437061 { get; set; }
        [MetaProperty("mapContainer", BinPropertyType.String)]
        public string? MapContainer { get; set; }
        [MetaProperty(1742421228, BinPropertyType.UInt32)]
        public uint? m1742421228 { get; set; }
    }
    [MetaClass("TrophyData")]
    public class TrophyData : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        public float? PerceptionBubbleRadius { get; set; }
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; }
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? AnimationGraphData { get; set; }
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; }
        [MetaProperty("mBracketTRAKey", BinPropertyType.String)]
        public string? BracketTRAKey { get; set; }
    }
    [MetaClass("TrophyPedestalData")]
    public class TrophyPedestalData : BaseLoadoutData
    {
        [MetaProperty(4134177491, BinPropertyType.String)]
        public string? m4134177491 { get; set; }
        [MetaProperty("catalogEntry", BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> CatalogEntry { get; set; }
        [MetaProperty(1905664938, BinPropertyType.String)]
        public string? m1905664938 { get; set; }
        [MetaProperty("animationGraphData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? AnimationGraphData { get; set; }
        [MetaProperty("mJointName", BinPropertyType.String)]
        public string? JointName { get; set; }
        [MetaProperty("skinMeshProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<SkinMeshDataProperties> SkinMeshProperties { get; set; }
        [MetaProperty("mTierTRAKey", BinPropertyType.String)]
        public string? TierTRAKey { get; set; }
    }
    [MetaClass("AbilityResourceDynamicMaterialFloatDriver")]
    public class AbilityResourceDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("slot", BinPropertyType.Byte)]
        public byte? Slot { get; set; }
    }
    [MetaClass("AnimationFractionDynamicMaterialFloatDriver")]
    public class AnimationFractionDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mAnimationName", BinPropertyType.Hash)]
        public MetaHash? AnimationName { get; set; }
    }
    [MetaClass("BuffCounterDynamicMaterialFloatDriver")]
    public class BuffCounterDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mScriptName", BinPropertyType.String)]
        public string? ScriptName { get; set; }
    }
    [MetaClass("DistanceToPlayerMaterialFloatDriver")]
    public class DistanceToPlayerMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("minDistance", BinPropertyType.Float)]
        public float? MinDistance { get; set; }
        [MetaProperty("maxDistance", BinPropertyType.Float)]
        public float? MaxDistance { get; set; }
    }
    [MetaClass(2614239024)]
    public class Class2614239024 : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string? KeyName { get; set; }
    }
    [MetaClass(510412798)]
    public class Class510412798 : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string? KeyName { get; set; }
    }
    [MetaClass(3490803144)]
    public class Class3490803144 : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string? KeyName { get; set; }
    }
    [MetaClass(3897992344)]
    public class Class3897992344 : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mKeyName", BinPropertyType.String)]
        public string? KeyName { get; set; }
    }
    [MetaClass("HasBuffDynamicMaterialBoolDriver")]
    public class HasBuffDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mScriptName", BinPropertyType.String)]
        public string? ScriptName { get; set; }
        [MetaProperty(4286635898, BinPropertyType.Float)]
        public float? m4286635898 { get; set; }
    }
    [MetaClass("HasGearDynamicMaterialBoolDriver")]
    public class HasGearDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mGearIndex", BinPropertyType.Byte)]
        public byte? GearIndex { get; set; }
    }
    [MetaClass("HealthDynamicMaterialFloatDriver")]
    public class HealthDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass("IsAnimationPlayingDynamicMaterialBoolDriver")]
    public class IsAnimationPlayingDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mAnimationNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationNames { get; set; }
    }
    [MetaClass("IsDeadDynamicMaterialBoolDriver")]
    public class IsDeadDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
    }
    [MetaClass("IsEnemyDynamicMaterialBoolDriver")]
    public class IsEnemyDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
    }
    [MetaClass("IsInGrassDynamicMaterialBoolDriver")]
    public class IsInGrassDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
    }
    [MetaClass("LearnedSpellDynamicMaterialBoolDriver")]
    public class LearnedSpellDynamicMaterialBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mSlot", BinPropertyType.Byte)]
        public byte? Slot { get; set; }
    }
    [MetaClass(532586595)]
    public class Class532586595 : IDynamicMaterialDriver
    {
    }
    [MetaClass("UVScaleBiasFromAnimationDynamicMaterialDriver")]
    public class UVScaleBiasFromAnimationDynamicMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mSubMeshName", BinPropertyType.String)]
        public string? SubMeshName { get; set; }
        [MetaProperty(3685552942, BinPropertyType.UInt32)]
        public uint? m3685552942 { get; set; }
    }
    [MetaClass("VelocityDynamicMaterialFloatDriver")]
    public class VelocityDynamicMaterialFloatDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass(3162079434)]
    public class Class3162079434 : WadFileDescriptor
    {
    }
    [MetaClass("EmblemPosition")]
    public class EmblemPosition : IMetaClass
    {
        [MetaProperty("mVertical", BinPropertyType.String)]
        public string? Vertical { get; set; }
        [MetaProperty("mHorizontal", BinPropertyType.String)]
        public string? Horizontal { get; set; }
    }
    [MetaClass("EmblemData")]
    public class EmblemData : IMetaClass
    {
        [MetaProperty("mShowOnLoadingScreen", BinPropertyType.Bool)]
        public bool? ShowOnLoadingScreen { get; set; }
        [MetaProperty("mLoadingScreenScale", BinPropertyType.Float)]
        public float? LoadingScreenScale { get; set; }
        [MetaProperty("mImagePath", BinPropertyType.String)]
        public string? ImagePath { get; set; }
    }
    [MetaClass("EmblemSettings")]
    public class EmblemSettings : IMetaClass
    {
        [MetaProperty("mBottomFraction", BinPropertyType.Float)]
        public float? BottomFraction { get; set; }
        [MetaProperty("mDebugDrawEmblems", BinPropertyType.Bool)]
        public bool? DebugDrawEmblems { get; set; }
    }
    [MetaClass("GameModeAutoItemPurchasingConfig")]
    public class GameModeAutoItemPurchasingConfig : IMetaClass
    {
        [MetaProperty(435120034, BinPropertyType.Int32)]
        public int? m435120034 { get; set; }
        [MetaProperty(3813630672, BinPropertyType.Container)]
        public MetaContainer<string> m3813630672 { get; set; }
        [MetaProperty(3366845884, BinPropertyType.Container)]
        public MetaContainer<string> m3366845884 { get; set; }
        [MetaProperty(2213596365, BinPropertyType.Container)]
        public MetaContainer<string> m2213596365 { get; set; }
        [MetaProperty(341404937, BinPropertyType.Hash)]
        public MetaHash? m341404937 { get; set; }
    }
    [MetaClass("GameModeChampionList")]
    public class GameModeChampionList : IMetaClass
    {
        [MetaProperty("mChampions", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaHash> Champions { get; set; }
    }
    [MetaClass("GameModeConstant")]
    public interface GameModeConstant : IMetaClass
    {
    }
    [MetaClass("GameModeConstantFloat")]
    public class GameModeConstantFloat : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float? Value { get; set; }
    }
    [MetaClass("GameModeConstantInteger")]
    public class GameModeConstantInteger : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Int32)]
        public int? Value { get; set; }
    }
    [MetaClass("GameModeConstantBool")]
    public class GameModeConstantBool : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Bool)]
        public bool? Value { get; set; }
    }
    [MetaClass("GameModeConstantString")]
    public class GameModeConstantString : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.String)]
        public string? Value { get; set; }
    }
    [MetaClass("GameModeConstantStringVector")]
    public class GameModeConstantStringVector : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Container)]
        public MetaContainer<string> Value { get; set; }
    }
    [MetaClass("GameModeConstantTRAKey")]
    public class GameModeConstantTRAKey : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.String)]
        public string? Value { get; set; }
    }
    [MetaClass("GameModeConstantVector3f")]
    public class GameModeConstantVector3f : GameModeConstant
    {
        [MetaProperty("mValue", BinPropertyType.Vector3)]
        public Vector3? Value { get; set; }
    }
    [MetaClass("GameModeConstantFloatPerLevel")]
    public class GameModeConstantFloatPerLevel : GameModeConstant
    {
        [MetaProperty("mValues", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; }
    }
    [MetaClass("GameModeConstantsGroup")]
    public class GameModeConstantsGroup : IMetaClass
    {
        [MetaProperty("mConstants", BinPropertyType.Map)]
        public Dictionary<MetaHash, GameModeConstant> Constants { get; set; }
    }
    [MetaClass("GameModeConstants")]
    public class GameModeConstants : IMetaClass
    {
        [MetaProperty("mGroups", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<GameModeConstantsGroup>> Groups { get; set; }
    }
    [MetaClass("GameModeItemList")]
    public class GameModeItemList : IMetaClass
    {
        [MetaProperty("mItems", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaHash> Items { get; set; }
    }
    [MetaClass("GameModeMapData")]
    public class GameModeMapData : IMetaClass
    {
        [MetaProperty("mModeName", BinPropertyType.Hash)]
        public MetaHash? ModeName { get; set; }
        [MetaProperty("mChampionLists", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> ChampionLists { get; set; }
        [MetaProperty(1298046227, BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> m1298046227 { get; set; }
        [MetaProperty(4148979643, BinPropertyType.String)]
        public string? m4148979643 { get; set; }
        [MetaProperty(2519132899, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2519132899 { get; set; }
        [MetaProperty("mRelativeColorization", BinPropertyType.Bool)]
        public bool? RelativeColorization { get; set; }
        [MetaProperty("mNeutralTimersDisplay", BinPropertyType.ObjectLink)]
        public MetaObjectLink? NeutralTimersDisplay { get; set; }
        [MetaProperty("mCursorConfig", BinPropertyType.Hash)]
        public MetaHash? CursorConfig { get; set; }
        [MetaProperty("mCursorConfigUpdate", BinPropertyType.Hash)]
        public MetaHash? CursorConfigUpdate { get; set; }
        [MetaProperty("mHudScoreData", BinPropertyType.Hash)]
        public MetaHash? HudScoreData { get; set; }
        [MetaProperty("mRenderStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RenderStyle { get; set; }
        [MetaProperty("mFloatingTextOverride", BinPropertyType.ObjectLink)]
        public MetaObjectLink? FloatingTextOverride { get; set; }
        [MetaProperty(1519015945, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1519015945 { get; set; }
        [MetaProperty(2041914701, BinPropertyType.Bool)]
        public bool? m2041914701 { get; set; }
        [MetaProperty(1551909767, BinPropertyType.Bool)]
        public bool? m1551909767 { get; set; }
        [MetaProperty("mExperienceCurveData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ExperienceCurveData { get; set; }
        [MetaProperty("mExperienceModData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ExperienceModData { get; set; }
        [MetaProperty("mDeathTimes", BinPropertyType.ObjectLink)]
        public MetaObjectLink? DeathTimes { get; set; }
        [MetaProperty("mLoadScreenTipConfiguration", BinPropertyType.ObjectLink)]
        public MetaObjectLink? LoadScreenTipConfiguration { get; set; }
        [MetaProperty("mMapLocators", BinPropertyType.ObjectLink)]
        public MetaObjectLink? MapLocators { get; set; }
        [MetaProperty("mPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkReplacementList> PerkReplacements { get; set; }
        [MetaProperty("mGameModeConstants", BinPropertyType.ObjectLink)]
        public MetaObjectLink? GameModeConstants { get; set; }
        [MetaProperty("mGameplayConfig", BinPropertyType.ObjectLink)]
        public MetaObjectLink? GameplayConfig { get; set; }
        [MetaProperty(3386271259, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3386271259 { get; set; }
        [MetaProperty(3906072283, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3906072283 { get; set; }
        [MetaProperty(1765926418, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1765926418 { get; set; }
        [MetaProperty(2284479568, BinPropertyType.Bool)]
        public bool? m2284479568 { get; set; }
        [MetaProperty(1890753597, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m1890753597 { get; set; }
        [MetaProperty("mLoadingScreenBackground", BinPropertyType.String)]
        public string? LoadingScreenBackground { get; set; }
    }
    [MetaClass("GameMutatorExpansions")]
    public class GameMutatorExpansions : IMetaClass
    {
        [MetaProperty("mExpandedMutator", BinPropertyType.String)]
        public string? ExpandedMutator { get; set; }
        [MetaProperty("mMutators", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<string> Mutators { get; set; }
    }
    [MetaClass("IGameCalculationPart")]
    public interface IGameCalculationPart : IMetaClass
    {
    }
    [MetaClass("EffectValueCalculationPart")]
    public class EffectValueCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mEffectIndex", BinPropertyType.Int32)]
        public int? EffectIndex { get; set; }
    }
    [MetaClass("NamedDataValueCalculationPart")]
    public class NamedDataValueCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mDataValue", BinPropertyType.Hash)]
        public MetaHash? DataValue { get; set; }
    }
    [MetaClass("CooldownMultiplierCalculationPart")]
    public class CooldownMultiplierCalculationPart : IGameCalculationPart
    {
    }
    [MetaClass("CustomReductionMultiplierCalculationPart")]
    public class CustomReductionMultiplierCalculationPart : IGameCalculationPart
    {
        [MetaProperty(1854058873, BinPropertyType.Structure)]
        public IGameCalculationPart m1854058873 { get; set; }
        [MetaProperty(4038717832, BinPropertyType.Float)]
        public float? m4038717832 { get; set; }
    }
    [MetaClass("ProductOfSubPartsCalculationPart")]
    public class ProductOfSubPartsCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mPart1", BinPropertyType.Structure)]
        public IGameCalculationPart Part1 { get; set; }
        [MetaProperty("mPart2", BinPropertyType.Structure)]
        public IGameCalculationPart Part2 { get; set; }
    }
    [MetaClass("SumOfSubPartsCalculationPart")]
    public class SumOfSubPartsCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mSubparts", BinPropertyType.Container)]
        public MetaContainer<IGameCalculationPart> Subparts { get; set; }
    }
    [MetaClass(2151525964)]
    public class Class2151525964 : IGameCalculationPart
    {
        [MetaProperty(2289025431, BinPropertyType.Optional)]
        public MetaOptional<float> m2289025431 { get; set; }
        [MetaProperty(3426554062, BinPropertyType.Optional)]
        public MetaOptional<float> m3426554062 { get; set; }
        [MetaProperty("mSubparts", BinPropertyType.Container)]
        public MetaContainer<IGameCalculationPart> Subparts { get; set; }
    }
    [MetaClass("NumberCalculationPart")]
    public class NumberCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mNumber", BinPropertyType.Float)]
        public float? Number { get; set; }
    }
    [MetaClass("IGameCalculationPartWithStats")]
    public interface IGameCalculationPartWithStats : IGameCalculationPart
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        byte? Stat { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        byte? StatFormula { get; set; }
    }
    [MetaClass("StatByCoefficientCalculationPart")]
    public class StatByCoefficientCalculationPart : IGameCalculationPartWithStats
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte? Stat { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte? StatFormula { get; set; }
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float? Coefficient { get; set; }
    }
    [MetaClass("StatBySubPartCalculationPart")]
    public class StatBySubPartCalculationPart : IGameCalculationPartWithStats
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte? Stat { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte? StatFormula { get; set; }
        [MetaProperty("mSubpart", BinPropertyType.Structure)]
        public IGameCalculationPart Subpart { get; set; }
    }
    [MetaClass("StatByNamedDataValueCalculationPart")]
    public class StatByNamedDataValueCalculationPart : IGameCalculationPartWithStats
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte? Stat { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte? StatFormula { get; set; }
        [MetaProperty("mDataValue", BinPropertyType.Hash)]
        public MetaHash? DataValue { get; set; }
    }
    [MetaClass(95149995)]
    public class Class95149995 : IGameCalculationPart
    {
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte? Stat { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte? StatFormula { get; set; }
        [MetaProperty(3219565825, BinPropertyType.Float)]
        public float? m3219565825 { get; set; }
        [MetaProperty(182646134, BinPropertyType.Float)]
        public float? m182646134 { get; set; }
    }
    [MetaClass("SubPartScaledProportionalToStat")]
    public class SubPartScaledProportionalToStat : IGameCalculationPart
    {
        [MetaProperty("mSubpart", BinPropertyType.Structure)]
        public IGameCalculationPart Subpart { get; set; }
        [MetaProperty("mRatio", BinPropertyType.Float)]
        public float? Ratio { get; set; }
        [MetaProperty("mStat", BinPropertyType.Byte)]
        public byte? Stat { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte? StatFormula { get; set; }
        [MetaProperty(2569852907, BinPropertyType.String)]
        public string? m2569852907 { get; set; }
        [MetaProperty(2775882578, BinPropertyType.String)]
        public string? m2775882578 { get; set; }
    }
    [MetaClass("AbilityResourceByCoefficientCalculationPart")]
    public class AbilityResourceByCoefficientCalculationPart : IGameCalculationPart
    {
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float? Coefficient { get; set; }
        [MetaProperty(1306624758, BinPropertyType.Byte)]
        public byte? m1306624758 { get; set; }
        [MetaProperty("mStatFormula", BinPropertyType.Byte)]
        public byte? StatFormula { get; set; }
    }
    [MetaClass("IGameCalculationPartWithBuffCounter")]
    public interface IGameCalculationPartWithBuffCounter : IGameCalculationPart
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        MetaHash? BuffName { get; set; }
        [MetaProperty(9297440, BinPropertyType.String)]
        string? m9297440 { get; set; }
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        string? ScalingTagKey { get; set; }
    }
    [MetaClass("BuffCounterByCoefficientCalculationPart")]
    public class BuffCounterByCoefficientCalculationPart : IGameCalculationPartWithBuffCounter
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash? BuffName { get; set; }
        [MetaProperty(9297440, BinPropertyType.String)]
        public string? m9297440 { get; set; }
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        public string? ScalingTagKey { get; set; }
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float? Coefficient { get; set; }
    }
    [MetaClass("BuffCounterByNamedDataValueCalculationPart")]
    public class BuffCounterByNamedDataValueCalculationPart : IGameCalculationPartWithBuffCounter
    {
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash? BuffName { get; set; }
        [MetaProperty(9297440, BinPropertyType.String)]
        public string? m9297440 { get; set; }
        [MetaProperty("mScalingTagKey", BinPropertyType.String)]
        public string? ScalingTagKey { get; set; }
        [MetaProperty("mDataValue", BinPropertyType.Hash)]
        public MetaHash? DataValue { get; set; }
    }
    [MetaClass("IGameCalculationPartByCharLevel")]
    public interface IGameCalculationPartByCharLevel : IGameCalculationPart
    {
    }
    [MetaClass("ByCharLevelInterpolationCalculationPart")]
    public class ByCharLevelInterpolationCalculationPart : IGameCalculationPartByCharLevel
    {
        [MetaProperty("mStartValue", BinPropertyType.Float)]
        public float? StartValue { get; set; }
        [MetaProperty("mEndValue", BinPropertyType.Float)]
        public float? EndValue { get; set; }
        [MetaProperty(2145969075, BinPropertyType.Bool)]
        public bool? m2145969075 { get; set; }
        [MetaProperty(2737960639, BinPropertyType.Bool)]
        public bool? m2737960639 { get; set; }
    }
    [MetaClass("ByCharLevelBreakpointsCalculationPart")]
    public class ByCharLevelBreakpointsCalculationPart : IGameCalculationPartByCharLevel
    {
        [MetaProperty("mLevel1Value", BinPropertyType.Float)]
        public float? Level1Value { get; set; }
        [MetaProperty(48149840, BinPropertyType.Float)]
        public float? m48149840 { get; set; }
        [MetaProperty("mBreakpoints", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Breakpoint>> Breakpoints { get; set; }
    }
    [MetaClass("Breakpoint")]
    public class Breakpoint : IMetaClass
    {
        [MetaProperty("mLevel", BinPropertyType.UInt32)]
        public uint? Level { get; set; }
        [MetaProperty(3590129645, BinPropertyType.Float)]
        public float? m3590129645 { get; set; }
        [MetaProperty(1476248632, BinPropertyType.Float)]
        public float? m1476248632 { get; set; }
    }
    [MetaClass("ByCharLevelFormulaCalculationPart")]
    public class ByCharLevelFormulaCalculationPart : IGameCalculationPartByCharLevel
    {
        [MetaProperty("mValues", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; }
    }
    [MetaClass(1715297792)]
    public class Class1715297792 : IGameCalculationPart
    {
        [MetaProperty(3401692882, BinPropertyType.Float)]
        public float? m3401692882 { get; set; }
        [MetaProperty("epicness", BinPropertyType.Byte)]
        public byte? Epicness { get; set; }
    }
    [MetaClass("IGameCalculation")]
    public interface IGameCalculation : IMetaClass
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        IGameCalculationPart Multiplier { get; set; }
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        byte? m3419063832 { get; set; }
        [MetaProperty(923208333, BinPropertyType.Byte)]
        byte? m923208333 { get; set; }
        [MetaProperty(3602359842, BinPropertyType.Bool)]
        bool? m3602359842 { get; set; }
    }
    [MetaClass("GameCalculation")]
    public class GameCalculation : IGameCalculation
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        public IGameCalculationPart Multiplier { get; set; }
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        public byte? m3419063832 { get; set; }
        [MetaProperty(923208333, BinPropertyType.Byte)]
        public byte? m923208333 { get; set; }
        [MetaProperty(3602359842, BinPropertyType.Bool)]
        public bool? m3602359842 { get; set; }
        [MetaProperty(1357989312, BinPropertyType.Container)]
        public MetaContainer<IGameCalculationPart> m1357989312 { get; set; }
        [MetaProperty("mDisplayAsPercent", BinPropertyType.Bool)]
        public bool? DisplayAsPercent { get; set; }
        [MetaProperty("mPrecision", BinPropertyType.Int32)]
        public int? Precision { get; set; }
    }
    [MetaClass("GameCalculationModified")]
    public class GameCalculationModified : IGameCalculation
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        public IGameCalculationPart Multiplier { get; set; }
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        public byte? m3419063832 { get; set; }
        [MetaProperty(923208333, BinPropertyType.Byte)]
        public byte? m923208333 { get; set; }
        [MetaProperty(3602359842, BinPropertyType.Bool)]
        public bool? m3602359842 { get; set; }
        [MetaProperty("mOverrideSpellLevel", BinPropertyType.Optional)]
        public MetaOptional<int> OverrideSpellLevel { get; set; }
        [MetaProperty("mModifiedGameCalculation", BinPropertyType.Hash)]
        public MetaHash? ModifiedGameCalculation { get; set; }
    }
    [MetaClass("GameCalculationConditional")]
    public class GameCalculationConditional : IGameCalculation
    {
        [MetaProperty("mMultiplier", BinPropertyType.Structure)]
        public IGameCalculationPart Multiplier { get; set; }
        [MetaProperty(3419063832, BinPropertyType.Byte)]
        public byte? m3419063832 { get; set; }
        [MetaProperty(923208333, BinPropertyType.Byte)]
        public byte? m923208333 { get; set; }
        [MetaProperty(3602359842, BinPropertyType.Bool)]
        public bool? m3602359842 { get; set; }
        [MetaProperty(18101940, BinPropertyType.Hash)]
        public MetaHash? m18101940 { get; set; }
        [MetaProperty(7331431, BinPropertyType.Hash)]
        public MetaHash? m7331431 { get; set; }
        [MetaProperty(3225953125, BinPropertyType.Structure)]
        public ICastRequirement m3225953125 { get; set; }
    }
    [MetaClass("GameplayConfig")]
    public class GameplayConfig : IMetaClass
    {
        [MetaProperty("mSpellPostponeTimeoutSec", BinPropertyType.Float)]
        public float? SpellPostponeTimeoutSec { get; set; }
        [MetaProperty("mAutoAttackMinPreCastLockoutDeltaTimeSec", BinPropertyType.Float)]
        public float? AutoAttackMinPreCastLockoutDeltaTimeSec { get; set; }
        [MetaProperty("mAutoAttackMinPostCastLockoutDeltaTimeSec", BinPropertyType.Float)]
        public float? AutoAttackMinPostCastLockoutDeltaTimeSec { get; set; }
        [MetaProperty("mLethalityPercentGivenAtLevel0", BinPropertyType.Float)]
        public float? LethalityPercentGivenAtLevel0 { get; set; }
        [MetaProperty("mLethalityScalesToLevel", BinPropertyType.Int32)]
        public int? LethalityScalesToLevel { get; set; }
        [MetaProperty("mLethalityScalesCapsAtLevel", BinPropertyType.Int32)]
        public int? LethalityScalesCapsAtLevel { get; set; }
        [MetaProperty("mLethalityRatioFromTarget", BinPropertyType.Float)]
        public float? LethalityRatioFromTarget { get; set; }
        [MetaProperty("mLethalityRatioFromAttacker", BinPropertyType.Float)]
        public float? LethalityRatioFromAttacker { get; set; }
        [MetaProperty("mCritTotalArmorPenPercent", BinPropertyType.Float)]
        public float? CritTotalArmorPenPercent { get; set; }
        [MetaProperty("mCritBonusArmorPenPercent", BinPropertyType.Float)]
        public float? CritBonusArmorPenPercent { get; set; }
        [MetaProperty("mCritGlobalDamageMultiplier", BinPropertyType.Float)]
        public float? CritGlobalDamageMultiplier { get; set; }
        [MetaProperty("mAdaptiveForceAbilityPowerScale", BinPropertyType.Float)]
        public float? AdaptiveForceAbilityPowerScale { get; set; }
        [MetaProperty("mAdaptiveForceAttackDamageScale", BinPropertyType.Float)]
        public float? AdaptiveForceAttackDamageScale { get; set; }
        [MetaProperty("mMinionDeathDelay", BinPropertyType.Float)]
        public float? MinionDeathDelay { get; set; }
        [MetaProperty("mMinionAutoLeeway", BinPropertyType.Float)]
        public float? MinionAutoLeeway { get; set; }
        [MetaProperty("mMinionAAHelperLimit", BinPropertyType.Float)]
        public float? MinionAAHelperLimit { get; set; }
        [MetaProperty("mItemSellQueueTime", BinPropertyType.Float)]
        public float? ItemSellQueueTime { get; set; }
        [MetaProperty("mCCScoreMultipliers", BinPropertyType.Embedded)]
        public MetaEmbedded<CCScoreMultipliers> CCScoreMultipliers { get; set; }
        [MetaProperty("mPerSlotCDRIsAdditive", BinPropertyType.Bool)]
        public bool? PerSlotCDRIsAdditive { get; set; }
        [MetaProperty("mSummonerSpells", BinPropertyType.Container)]
        public MetaContainer<MetaHash> SummonerSpells { get; set; }
        [MetaProperty("mLegacySummonerSpells", BinPropertyType.Container)]
        public MetaContainer<MetaHash> LegacySummonerSpells { get; set; }
        [MetaProperty("mBasicAttackCalculation", BinPropertyType.Structure)]
        public IGameCalculation BasicAttackCalculation { get; set; }
        [MetaProperty(1627693693, BinPropertyType.Hash)]
        public MetaHash? m1627693693 { get; set; }
        [MetaProperty(2789737202, BinPropertyType.Hash)]
        public MetaHash? m2789737202 { get; set; }
    }
    [MetaClass("EnchantmentGroup")]
    public class EnchantmentGroup : IMetaClass
    {
        [MetaProperty("mEnchantments", BinPropertyType.Container)]
        public MetaContainer<int> Enchantments { get; set; }
        [MetaProperty("mBaseItems", BinPropertyType.Container)]
        public MetaContainer<int> BaseItems { get; set; }
        [MetaProperty("mCanSidegrade", BinPropertyType.Bool)]
        public bool? CanSidegrade { get; set; }
        [MetaProperty("mItemIdRangeMinimum", BinPropertyType.Int32)]
        public int? ItemIdRangeMinimum { get; set; }
        [MetaProperty("mItemIdRangeMaximum", BinPropertyType.Int32)]
        public int? ItemIdRangeMaximum { get; set; }
    }
    [MetaClass("ItemData")]
    public class ItemData : IMetaClass
    {
        [MetaProperty("spellName", BinPropertyType.String)]
        public string? SpellName { get; set; }
        [MetaProperty("mDisplayName", BinPropertyType.String)]
        public string? DisplayName { get; set; }
        [MetaProperty("mRequiredChampion", BinPropertyType.String)]
        public string? RequiredChampion { get; set; }
        [MetaProperty("mRequiredAlly", BinPropertyType.String)]
        public string? RequiredAlly { get; set; }
        [MetaProperty("mRequiredLevel", BinPropertyType.Int32)]
        public int? RequiredLevel { get; set; }
        [MetaProperty("mRequiredSpellName", BinPropertyType.String)]
        public string? RequiredSpellName { get; set; }
        [MetaProperty("mRequiredPurchaseIdentities", BinPropertyType.Container)]
        public MetaContainer<MetaHash> RequiredPurchaseIdentities { get; set; }
        [MetaProperty("mDisabledDescriptionOverride", BinPropertyType.String)]
        public string? DisabledDescriptionOverride { get; set; }
        [MetaProperty("mParentEnchantmentGroup", BinPropertyType.String)]
        public string? ParentEnchantmentGroup { get; set; }
        [MetaProperty("mDeathRecapName", BinPropertyType.String)]
        public string? DeathRecapName { get; set; }
        [MetaProperty("itemID", BinPropertyType.Int32)]
        public int? ItemID { get; set; }
        [MetaProperty("maxStack", BinPropertyType.Int32)]
        public int? MaxStack { get; set; }
        [MetaProperty("mItemGroups", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemGroups { get; set; }
        [MetaProperty(1527415230, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m1527415230 { get; set; }
        [MetaProperty("itemVOGroup", BinPropertyType.Hash)]
        public MetaHash? ItemVOGroup { get; set; }
        [MetaProperty("price", BinPropertyType.Int32)]
        public int? Price { get; set; }
        [MetaProperty("mRequiredBuffCurrencyName", BinPropertyType.String)]
        public string? RequiredBuffCurrencyName { get; set; }
        [MetaProperty("mRequiredBuffCurrencyCost", BinPropertyType.Int32)]
        public int? RequiredBuffCurrencyCost { get; set; }
        [MetaProperty("mSidegradeCredit", BinPropertyType.Float)]
        public float? SidegradeCredit { get; set; }
        [MetaProperty("consumed", BinPropertyType.Bool)]
        public bool? Consumed { get; set; }
        [MetaProperty("usableInStore", BinPropertyType.Bool)]
        public bool? UsableInStore { get; set; }
        [MetaProperty("consumeOnAcquire", BinPropertyType.Bool)]
        public bool? ConsumeOnAcquire { get; set; }
        [MetaProperty("clickable", BinPropertyType.Bool)]
        public bool? Clickable { get; set; }
        [MetaProperty("mMajorActiveItem", BinPropertyType.Bool)]
        public bool? MajorActiveItem { get; set; }
        [MetaProperty("mItemCalloutPlayer", BinPropertyType.Bool)]
        public bool? ItemCalloutPlayer { get; set; }
        [MetaProperty("mItemCalloutSpectator", BinPropertyType.Bool)]
        public bool? ItemCalloutSpectator { get; set; }
        [MetaProperty("clearUndoHistory", BinPropertyType.Byte)]
        public byte? ClearUndoHistory { get; set; }
        [MetaProperty("mCanBeSold", BinPropertyType.Bool)]
        public bool? CanBeSold { get; set; }
        [MetaProperty("mHiddenFromOpponents", BinPropertyType.Bool)]
        public bool? HiddenFromOpponents { get; set; }
        [MetaProperty("mIsEnchantment", BinPropertyType.Bool)]
        public bool? IsEnchantment { get; set; }
        [MetaProperty("specialRecipe", BinPropertyType.Int32)]
        public int? SpecialRecipe { get; set; }
        [MetaProperty("epicness", BinPropertyType.Byte)]
        public byte? Epicness { get; set; }
        [MetaProperty(2413426407, BinPropertyType.Byte)]
        public byte? m2413426407 { get; set; }
        [MetaProperty("recipeItemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> RecipeItemLinks { get; set; }
        [MetaProperty("requiredItemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> RequiredItemLinks { get; set; }
        [MetaProperty("sidegradeItemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> SidegradeItemLinks { get; set; }
        [MetaProperty("mItemModifiers", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemModifiers { get; set; }
        [MetaProperty("mMapStringIdInclusions", BinPropertyType.Container)]
        public MetaContainer<MetaHash> MapStringIdInclusions { get; set; }
        [MetaProperty("mModeNameInclusions", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ModeNameInclusions { get; set; }
        [MetaProperty("customInclusions", BinPropertyType.Container)]
        public MetaContainer<string> CustomInclusions { get; set; }
        [MetaProperty("customExclusions", BinPropertyType.Container)]
        public MetaContainer<string> CustomExclusions { get; set; }
        [MetaProperty("mScripts", BinPropertyType.Container)]
        public MetaContainer<string> Scripts { get; set; }
        [MetaProperty("parentItemLink", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ParentItemLink { get; set; }
        [MetaProperty("parentEnchantmentLink", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ParentEnchantmentLink { get; set; }
        [MetaProperty("mEffectAmount", BinPropertyType.Container)]
        public MetaContainer<float> EffectAmount { get; set; }
        [MetaProperty("mDataValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class2999331975>> DataValues { get; set; }
        [MetaProperty(180678869, BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> m180678869 { get; set; }
        [MetaProperty("mEnchantmentEffectAmount", BinPropertyType.Container)]
        public MetaContainer<float> EnchantmentEffectAmount { get; set; }
        [MetaProperty("mEffectByLevelAmount", BinPropertyType.Container)]
        public MetaContainer<float> EffectByLevelAmount { get; set; }
        [MetaProperty("mFlatCooldownMod", BinPropertyType.Float)]
        public float? FlatCooldownMod { get; set; }
        [MetaProperty("mPercentCooldownMod", BinPropertyType.Float)]
        public float? PercentCooldownMod { get; set; }
        [MetaProperty(1536713119, BinPropertyType.Float)]
        public float? m1536713119 { get; set; }
        [MetaProperty("mFlatHPPoolMod", BinPropertyType.Float)]
        public float? FlatHPPoolMod { get; set; }
        [MetaProperty("mPercentHPPoolMod", BinPropertyType.Float)]
        public float? PercentHPPoolMod { get; set; }
        [MetaProperty("mFlatHPRegenMod", BinPropertyType.Float)]
        public float? FlatHPRegenMod { get; set; }
        [MetaProperty("mPercentHPRegenMod", BinPropertyType.Float)]
        public float? PercentHPRegenMod { get; set; }
        [MetaProperty("mPercentBaseHPRegenMod", BinPropertyType.Float)]
        public float? PercentBaseHPRegenMod { get; set; }
        [MetaProperty("mPercentTenacityItemMod", BinPropertyType.Float)]
        public float? PercentTenacityItemMod { get; set; }
        [MetaProperty("mPercentSlowResistMod", BinPropertyType.Float)]
        public float? PercentSlowResistMod { get; set; }
        [MetaProperty("mFlatMovementSpeedMod", BinPropertyType.Float)]
        public float? FlatMovementSpeedMod { get; set; }
        [MetaProperty("mPercentMovementSpeedMod", BinPropertyType.Float)]
        public float? PercentMovementSpeedMod { get; set; }
        [MetaProperty("mPercentMultiplicativeMovementSpeedMod", BinPropertyType.Float)]
        public float? PercentMultiplicativeMovementSpeedMod { get; set; }
        [MetaProperty("mFlatArmorMod", BinPropertyType.Float)]
        public float? FlatArmorMod { get; set; }
        [MetaProperty("mPercentArmorMod", BinPropertyType.Float)]
        public float? PercentArmorMod { get; set; }
        [MetaProperty("mFlatArmorPenetrationMod", BinPropertyType.Float)]
        public float? FlatArmorPenetrationMod { get; set; }
        [MetaProperty("mPercentArmorPenetrationMod", BinPropertyType.Float)]
        public float? PercentArmorPenetrationMod { get; set; }
        [MetaProperty("mPercentBonusArmorPenetrationMod", BinPropertyType.Float)]
        public float? PercentBonusArmorPenetrationMod { get; set; }
        [MetaProperty("mFlatMagicPenetrationMod", BinPropertyType.Float)]
        public float? FlatMagicPenetrationMod { get; set; }
        [MetaProperty("mPercentMagicPenetrationMod", BinPropertyType.Float)]
        public float? PercentMagicPenetrationMod { get; set; }
        [MetaProperty("mPercentBonusMagicPenetrationMod", BinPropertyType.Float)]
        public float? PercentBonusMagicPenetrationMod { get; set; }
        [MetaProperty("mFlatSpellBlockMod", BinPropertyType.Float)]
        public float? FlatSpellBlockMod { get; set; }
        [MetaProperty("mPercentSpellBlockMod", BinPropertyType.Float)]
        public float? PercentSpellBlockMod { get; set; }
        [MetaProperty("mFlatDodgeMod", BinPropertyType.Float)]
        public float? FlatDodgeMod { get; set; }
        [MetaProperty("mFlatCritChanceMod", BinPropertyType.Float)]
        public float? FlatCritChanceMod { get; set; }
        [MetaProperty("mFlatMissChanceMod", BinPropertyType.Float)]
        public float? FlatMissChanceMod { get; set; }
        [MetaProperty("mFlatCritDamageMod", BinPropertyType.Float)]
        public float? FlatCritDamageMod { get; set; }
        [MetaProperty("mPercentCritDamageMod", BinPropertyType.Float)]
        public float? PercentCritDamageMod { get; set; }
        [MetaProperty("mFlatPhysicalDamageMod", BinPropertyType.Float)]
        public float? FlatPhysicalDamageMod { get; set; }
        [MetaProperty("mPercentPhysicalDamageMod", BinPropertyType.Float)]
        public float? PercentPhysicalDamageMod { get; set; }
        [MetaProperty("mFlatMagicDamageMod", BinPropertyType.Float)]
        public float? FlatMagicDamageMod { get; set; }
        [MetaProperty("mPercentMagicDamageMod", BinPropertyType.Float)]
        public float? PercentMagicDamageMod { get; set; }
        [MetaProperty("mFlatPhysicalReduction", BinPropertyType.Float)]
        public float? FlatPhysicalReduction { get; set; }
        [MetaProperty("mPercentPhysicalReduction", BinPropertyType.Float)]
        public float? PercentPhysicalReduction { get; set; }
        [MetaProperty("mFlatMagicReduction", BinPropertyType.Float)]
        public float? FlatMagicReduction { get; set; }
        [MetaProperty("mPercentMagicReduction", BinPropertyType.Float)]
        public float? PercentMagicReduction { get; set; }
        [MetaProperty("mPercentEXPBonus", BinPropertyType.Float)]
        public float? PercentEXPBonus { get; set; }
        [MetaProperty("mFlatAttackRangeMod", BinPropertyType.Float)]
        public float? FlatAttackRangeMod { get; set; }
        [MetaProperty("mPercentAttackRangeMod", BinPropertyType.Float)]
        public float? PercentAttackRangeMod { get; set; }
        [MetaProperty("mFlatCastRangeMod", BinPropertyType.Float)]
        public float? FlatCastRangeMod { get; set; }
        [MetaProperty("mPercentCastRangeMod", BinPropertyType.Float)]
        public float? PercentCastRangeMod { get; set; }
        [MetaProperty("mPercentAttackSpeedMod", BinPropertyType.Float)]
        public float? PercentAttackSpeedMod { get; set; }
        [MetaProperty("mPercentMultiplicativeAttackSpeedMod", BinPropertyType.Float)]
        public float? PercentMultiplicativeAttackSpeedMod { get; set; }
        [MetaProperty("mPercentHealingAmountMod", BinPropertyType.Float)]
        public float? PercentHealingAmountMod { get; set; }
        [MetaProperty("mPercentLifeStealMod", BinPropertyType.Float)]
        public float? PercentLifeStealMod { get; set; }
        [MetaProperty("mPercentSpellVampMod", BinPropertyType.Float)]
        public float? PercentSpellVampMod { get; set; }
        [MetaProperty("mPercentSpellEffectivenessMod", BinPropertyType.Float)]
        public float? PercentSpellEffectivenessMod { get; set; }
        [MetaProperty("mFlatBubbleRadiusMod", BinPropertyType.Float)]
        public float? FlatBubbleRadiusMod { get; set; }
        [MetaProperty("mPercentBubbleRadiusMod", BinPropertyType.Float)]
        public float? PercentBubbleRadiusMod { get; set; }
        [MetaProperty("sellBackModifier", BinPropertyType.Float)]
        public float? SellBackModifier { get; set; }
        [MetaProperty("mCooldownShowDisabledDuration", BinPropertyType.Float)]
        public float? CooldownShowDisabledDuration { get; set; }
        [MetaProperty("flatMPPoolMod", BinPropertyType.Float)]
        public float? FlatMPPoolMod { get; set; }
        [MetaProperty("PercentMPPoolMod", BinPropertyType.Float)]
        public float? PercentMPPoolMod { get; set; }
        [MetaProperty("flatMPRegenMod", BinPropertyType.Float)]
        public float? FlatMPRegenMod { get; set; }
        [MetaProperty("PercentMPRegenMod", BinPropertyType.Float)]
        public float? PercentMPRegenMod { get; set; }
        [MetaProperty("percentBaseMPRegenMod", BinPropertyType.Float)]
        public float? PercentBaseMPRegenMod { get; set; }
        [MetaProperty("mItemDataBuild", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemDataBuild> ItemDataBuild { get; set; }
        [MetaProperty("mItemDataAvailability", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemDataAvailability> ItemDataAvailability { get; set; }
        [MetaProperty(1632533069, BinPropertyType.Int32)]
        public int? m1632533069 { get; set; }
        [MetaProperty(1465224138, BinPropertyType.Container)]
        public MetaContainer<byte> m1465224138 { get; set; }
        [MetaProperty(575289365, BinPropertyType.Int32)]
        public int? m575289365 { get; set; }
        [MetaProperty(3223041757, BinPropertyType.Byte)]
        public byte? m3223041757 { get; set; }
        [MetaProperty(1361468553, BinPropertyType.Byte)]
        public byte? m1361468553 { get; set; }
        [MetaProperty("mItemDataClient", BinPropertyType.Embedded)]
        public MetaEmbedded<ItemDataClient> ItemDataClient { get; set; }
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; }
    }
    [MetaClass(2999331975)]
    public class Class2999331975 : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float? Value { get; set; }
    }
    [MetaClass("ItemDataAvailability")]
    public class ItemDataAvailability : IMetaClass
    {
        [MetaProperty("mInStore", BinPropertyType.Bool)]
        public bool? InStore { get; set; }
        [MetaProperty(781700779, BinPropertyType.Bool)]
        public bool? m781700779 { get; set; }
        [MetaProperty("mHidefromAll", BinPropertyType.Bool)]
        public bool? HidefromAll { get; set; }
    }
    [MetaClass("ItemDataBuild")]
    public class ItemDataBuild : IMetaClass
    {
        [MetaProperty("itemLinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemLinks { get; set; }
    }
    [MetaClass("ItemGroup")]
    public class ItemGroup : IMetaClass
    {
        [MetaProperty("mItemGroupID", BinPropertyType.Hash)]
        public MetaHash? ItemGroupID { get; set; }
        [MetaProperty("mMaxGroupOwnable", BinPropertyType.Int32)]
        public int? MaxGroupOwnable { get; set; }
        [MetaProperty("mInventorySlotMin", BinPropertyType.Int32)]
        public int? InventorySlotMin { get; set; }
        [MetaProperty("mInventorySlotMax", BinPropertyType.Int32)]
        public int? InventorySlotMax { get; set; }
        [MetaProperty("mPurchaseCooldown", BinPropertyType.Float)]
        public float? PurchaseCooldown { get; set; }
        [MetaProperty("mCooldownExtendedByAmbientGoldStart", BinPropertyType.Bool)]
        public bool? CooldownExtendedByAmbientGoldStart { get; set; }
        [MetaProperty("mItemModifiers", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ItemModifiers { get; set; }
    }
    [MetaClass("ItemModifier")]
    public class ItemModifier : IMetaClass
    {
        [MetaProperty("mItemModifierID", BinPropertyType.Hash)]
        public MetaHash? ItemModifierID { get; set; }
        [MetaProperty("mModifiedItem", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ModifiedItem { get; set; }
        [MetaProperty("mModifiedGroup", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ModifiedGroup { get; set; }
        [MetaProperty("mModifiedIfBuildsFromItem", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ModifiedIfBuildsFromItem { get; set; }
        [MetaProperty("mMinimumModifierInstancesToBeActive", BinPropertyType.Int32)]
        public int? MinimumModifierInstancesToBeActive { get; set; }
        [MetaProperty("mMaximumModifierInstancesToBeActive", BinPropertyType.Int32)]
        public int? MaximumModifierInstancesToBeActive { get; set; }
        [MetaProperty("mMaximumDeltasToStack", BinPropertyType.Int32)]
        public int? MaximumDeltasToStack { get; set; }
        [MetaProperty("mShowAsModifiedInUI", BinPropertyType.Bool)]
        public bool? ShowAsModifiedInUI { get; set; }
        [MetaProperty("mModifierIsInheritedByOwnedParentItems", BinPropertyType.Bool)]
        public bool? ModifierIsInheritedByOwnedParentItems { get; set; }
        [MetaProperty("mAddedBuildFrom", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> AddedBuildFrom { get; set; }
        [MetaProperty("mRemovedBuildFrom", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> RemovedBuildFrom { get; set; }
        [MetaProperty("mReplaceInsteadOfAddingBuildFrom", BinPropertyType.Bool)]
        public bool? ReplaceInsteadOfAddingBuildFrom { get; set; }
        [MetaProperty("mIgnoreMaxGroupOwnable", BinPropertyType.Bool)]
        public bool? IgnoreMaxGroupOwnable { get; set; }
        [MetaProperty("mIgnoreSpecificMaxGroupOwnable", BinPropertyType.Hash)]
        public MetaHash? IgnoreSpecificMaxGroupOwnable { get; set; }
        [MetaProperty("mDeltaGoldCost", BinPropertyType.Float)]
        public float? DeltaGoldCost { get; set; }
        [MetaProperty("mDeltaGoldCostPercent", BinPropertyType.Float)]
        public float? DeltaGoldCostPercent { get; set; }
        [MetaProperty("mDeltaBuffCurrencyCostPercent", BinPropertyType.Float)]
        public float? DeltaBuffCurrencyCostPercent { get; set; }
        [MetaProperty("mDeltaRequiredLevel", BinPropertyType.Int32)]
        public int? DeltaRequiredLevel { get; set; }
        [MetaProperty("mDeltaBuffCurrencyCost", BinPropertyType.Int32)]
        public int? DeltaBuffCurrencyCost { get; set; }
        [MetaProperty("mDeltaMaxStacks", BinPropertyType.Int32)]
        public int? DeltaMaxStacks { get; set; }
        [MetaProperty("mVisualPriority", BinPropertyType.Int32)]
        public int? VisualPriority { get; set; }
        [MetaProperty("inventoryIconToOverlay", BinPropertyType.String)]
        public string? InventoryIconToOverlay { get; set; }
        [MetaProperty("mDescriptionToAppend", BinPropertyType.String)]
        public string? DescriptionToAppend { get; set; }
        [MetaProperty("mDescriptionToPrepend", BinPropertyType.String)]
        public string? DescriptionToPrepend { get; set; }
        [MetaProperty("mDescriptionToReplace", BinPropertyType.String)]
        public string? DescriptionToReplace { get; set; }
        [MetaProperty("mDynamicTooltipToAppend", BinPropertyType.String)]
        public string? DynamicTooltipToAppend { get; set; }
        [MetaProperty("mDynamicTooltipToPrepend", BinPropertyType.String)]
        public string? DynamicTooltipToPrepend { get; set; }
        [MetaProperty(1565504299, BinPropertyType.String)]
        public string? m1565504299 { get; set; }
        [MetaProperty("mDisplayNameToAppend", BinPropertyType.String)]
        public string? DisplayNameToAppend { get; set; }
        [MetaProperty("mDisplayNameToPrepend", BinPropertyType.String)]
        public string? DisplayNameToPrepend { get; set; }
        [MetaProperty("mDisplayNameToReplace", BinPropertyType.String)]
        public string? DisplayNameToReplace { get; set; }
        [MetaProperty("mClickableToEnable", BinPropertyType.Bool)]
        public bool? ClickableToEnable { get; set; }
        [MetaProperty("mMajorActiveItemToEnable", BinPropertyType.Bool)]
        public bool? MajorActiveItemToEnable { get; set; }
        [MetaProperty("mSpellNameToReplace", BinPropertyType.String)]
        public string? SpellNameToReplace { get; set; }
    }
    [MetaClass("ItemDataClient")]
    public class ItemDataClient : IMetaClass
    {
        [MetaProperty("mTooltipData", BinPropertyType.Structure)]
        public TooltipInstanceItem TooltipData { get; set; }
        [MetaProperty("mDescription", BinPropertyType.String)]
        public string? Description { get; set; }
        [MetaProperty("mDynamicTooltip", BinPropertyType.String)]
        public string? DynamicTooltip { get; set; }
        [MetaProperty(1444652607, BinPropertyType.String)]
        public string? m1444652607 { get; set; }
        [MetaProperty("inventoryIcon", BinPropertyType.String)]
        public string? InventoryIcon { get; set; }
        [MetaProperty("epicness", BinPropertyType.Byte)]
        public byte? Epicness { get; set; }
        [MetaProperty("effectRadius", BinPropertyType.Float)]
        public float? EffectRadius { get; set; }
        [MetaProperty("mFloatVarsDecimals", BinPropertyType.Container)]
        public MetaContainer<int> FloatVarsDecimals { get; set; }
    }
    [MetaClass("ChampionItemRecommendations")]
    public class ChampionItemRecommendations : IMetaClass
    {
        [MetaProperty(3426090776, BinPropertyType.Hash)]
        public MetaHash? m3426090776 { get; set; }
        [MetaProperty(2477901527, BinPropertyType.Hash)]
        public MetaHash? m2477901527 { get; set; }
    }
    [MetaClass("ItemRecommendationContextList")]
    public class ItemRecommendationContextList : IMetaClass
    {
        [MetaProperty("mContexts", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationContext>> Contexts { get; set; }
    }
    [MetaClass("ItemRecommendationContext")]
    public class ItemRecommendationContext : IMetaClass
    {
        [MetaProperty(3155596802, BinPropertyType.UInt32)]
        public uint? m3155596802 { get; set; }
        [MetaProperty("mMapID", BinPropertyType.UInt32)]
        public uint? MapID { get; set; }
        [MetaProperty(934764380, BinPropertyType.Hash)]
        public MetaHash? m934764380 { get; set; }
        [MetaProperty("mPosition", BinPropertyType.Hash)]
        public MetaHash? Position { get; set; }
        [MetaProperty(2698561854, BinPropertyType.Embedded)]
        public MetaEmbedded<ItemRecommendationMatrix> m2698561854 { get; set; }
        [MetaProperty(43002728, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m43002728 { get; set; }
        [MetaProperty(144730223, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m144730223 { get; set; }
        [MetaProperty(3298200407, BinPropertyType.Embedded)]
        public MetaEmbedded<ItemRecommendationMatrix> m3298200407 { get; set; }
    }
    [MetaClass("ItemRecommendationChoices")]
    public class ItemRecommendationChoices : IMetaClass
    {
        [MetaProperty(766608976, BinPropertyType.Container)]
        public MetaContainer<uint> m766608976 { get; set; }
    }
    [MetaClass("ItemRecommendationMatrixRow")]
    public class ItemRecommendationMatrixRow : IMetaClass
    {
        [MetaProperty(1335420358, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<ItemRecommendationChoices>> m1335420358 { get; set; }
    }
    [MetaClass("ItemRecommendationMatrix")]
    public class ItemRecommendationMatrix : IMetaClass
    {
        [MetaProperty("mrows", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationMatrixRow>> Mrows { get; set; }
    }
    [MetaClass("ItemRecommendationCondition")]
    public class ItemRecommendationCondition : IMetaClass
    {
        [MetaProperty("mItem", BinPropertyType.Hash)]
        public MetaHash? Item { get; set; }
        [MetaProperty(1383594277, BinPropertyType.UInt32)]
        public uint? m1383594277 { get; set; }
        [MetaProperty(878428694, BinPropertyType.Byte)]
        public byte? m878428694 { get; set; }
    }
    [MetaClass("ItemRecommendationOverrideContext")]
    public class ItemRecommendationOverrideContext : IMetaClass
    {
        [MetaProperty("mMapID", BinPropertyType.UInt32)]
        public uint? MapID { get; set; }
        [MetaProperty(934764380, BinPropertyType.Hash)]
        public MetaHash? m934764380 { get; set; }
        [MetaProperty("mPosition", BinPropertyType.Hash)]
        public MetaHash? Position { get; set; }
    }
    [MetaClass("ItemRecommendationOverrideStartingItemSet")]
    public class ItemRecommendationOverrideStartingItemSet : IMetaClass
    {
        [MetaProperty(43002728, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m43002728 { get; set; }
    }
    [MetaClass("ItemRecommendationOverride")]
    public class ItemRecommendationOverride : IMetaClass
    {
        [MetaProperty(2231942803, BinPropertyType.Bool)]
        public bool? m2231942803 { get; set; }
        [MetaProperty(2318063320, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationOverrideContext>> m2318063320 { get; set; }
        [MetaProperty(971331326, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationOverrideStartingItemSet>> m971331326 { get; set; }
        [MetaProperty(1219672089, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m1219672089 { get; set; }
        [MetaProperty("mRecommendedItems", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationCondition>> RecommendedItems { get; set; }
    }
    [MetaClass("ItemRecommendationOverrideSet")]
    public class ItemRecommendationOverrideSet : IMetaClass
    {
        [MetaProperty(2168086401, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationOverride>> m2168086401 { get; set; }
    }
    [MetaClass(573358062)]
    public class Class573358062 : IMetaClass
    {
        [MetaProperty(2528880824, BinPropertyType.String)]
        public string? m2528880824 { get; set; }
    }
    [MetaClass(4211878006)]
    public class Class4211878006 : IMetaClass
    {
        [MetaProperty(2725485332, BinPropertyType.String)]
        public string? m2725485332 { get; set; }
    }
    [MetaClass("ItemShopGameModeData")]
    public class ItemShopGameModeData : IMetaClass
    {
        [MetaProperty(612716707, BinPropertyType.Bool)]
        public bool? m612716707 { get; set; }
        [MetaProperty(3185914920, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ItemRecommendationContext>> m3185914920 { get; set; }
    }
    [MetaClass("CollectiblesEsportsTeamData")]
    public class CollectiblesEsportsTeamData : IMetaClass
    {
        [MetaProperty("teamId", BinPropertyType.UInt32)]
        public uint? TeamId { get; set; }
        [MetaProperty("shortName", BinPropertyType.String)]
        public string? ShortName { get; set; }
        [MetaProperty("fullName", BinPropertyType.String)]
        public string? FullName { get; set; }
        [MetaProperty("leagueName", BinPropertyType.String)]
        public string? LeagueName { get; set; }
    }
    [MetaClass("SummonerIconData")]
    public class SummonerIconData : IMetaClass
    {
        [MetaProperty("iconId", BinPropertyType.UInt32)]
        public uint? IconId { get; set; }
        [MetaProperty(3114625026, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3114625026 { get; set; }
        [MetaProperty("gameTexture", BinPropertyType.String)]
        public string? GameTexture { get; set; }
        [MetaProperty(1357231841, BinPropertyType.Bool)]
        public bool? m1357231841 { get; set; }
        [MetaProperty("eSportsEventMutator", BinPropertyType.String)]
        public string? ESportsEventMutator { get; set; }
    }
    [MetaClass("LiveFeatureToggles")]
    public class LiveFeatureToggles : IMetaClass
    {
        [MetaProperty("mLoLToggles", BinPropertyType.Embedded)]
        public MetaEmbedded<LoLFeatureToggles> LoLToggles { get; set; }
        [MetaProperty("mGameplayToggles", BinPropertyType.Embedded)]
        public MetaEmbedded<GameplayFeatureToggles> GameplayToggles { get; set; }
        [MetaProperty("mEngineToggles", BinPropertyType.Embedded)]
        public MetaEmbedded<EngineFeatureToggles> EngineToggles { get; set; }
    }
    [MetaClass("LoLFeatureToggles")]
    public class LoLFeatureToggles : IMetaClass
    {
        [MetaProperty("NewSpellScript", BinPropertyType.Bool)]
        public bool? NewSpellScript { get; set; }
        [MetaProperty(4065351745, BinPropertyType.Bool)]
        public bool? m4065351745 { get; set; }
        [MetaProperty(1841226935, BinPropertyType.Bool)]
        public bool? m1841226935 { get; set; }
        [MetaProperty(2743208720, BinPropertyType.Bool)]
        public bool? m2743208720 { get; set; }
        [MetaProperty(4064556372, BinPropertyType.Bool)]
        public bool? m4064556372 { get; set; }
        [MetaProperty(4044511864, BinPropertyType.Bool)]
        public bool? m4044511864 { get; set; }
        [MetaProperty("DisableAutoSNR", BinPropertyType.Bool)]
        public bool? DisableAutoSNR { get; set; }
        [MetaProperty("DisableDDR", BinPropertyType.Bool)]
        public bool? DisableDDR { get; set; }
        [MetaProperty("queuedOrdersTriggerPreIssueOrder", BinPropertyType.Bool)]
        public bool? QueuedOrdersTriggerPreIssueOrder { get; set; }
        [MetaProperty("DisableRenderUIChatOSX", BinPropertyType.Bool)]
        public bool? DisableRenderUIChatOSX { get; set; }
        [MetaProperty("DisableRenderUIChatWindows", BinPropertyType.Bool)]
        public bool? DisableRenderUIChatWindows { get; set; }
        [MetaProperty("LimitedTournamentPause", BinPropertyType.Bool)]
        public bool? LimitedTournamentPause { get; set; }
        [MetaProperty("DontRefCountTargetableAndInvulnerable", BinPropertyType.Bool)]
        public bool? DontRefCountTargetableAndInvulnerable { get; set; }
        [MetaProperty("EnableCustomPlayerScoreColoring", BinPropertyType.Bool)]
        public bool? EnableCustomPlayerScoreColoring { get; set; }
        [MetaProperty("closeOnEndGameAfterDelay", BinPropertyType.Bool)]
        public bool? CloseOnEndGameAfterDelay { get; set; }
        [MetaProperty("PromoController", BinPropertyType.Bool)]
        public bool? PromoController { get; set; }
        [MetaProperty("cooldownSpellQueueing", BinPropertyType.Bool)]
        public bool? CooldownSpellQueueing { get; set; }
        [MetaProperty("useNewAttackSpeed", BinPropertyType.Bool)]
        public bool? UseNewAttackSpeed { get; set; }
        [MetaProperty("UseNewFireBBEvents", BinPropertyType.Bool)]
        public bool? UseNewFireBBEvents { get; set; }
        [MetaProperty("abilityResetUI", BinPropertyType.Bool)]
        public bool? AbilityResetUI { get; set; }
        [MetaProperty(676261369, BinPropertyType.Bool)]
        public bool? m676261369 { get; set; }
        [MetaProperty(532488372, BinPropertyType.Bool)]
        public bool? m532488372 { get; set; }
        [MetaProperty(2798407076, BinPropertyType.Bool)]
        public bool? m2798407076 { get; set; }
        [MetaProperty(1744584201, BinPropertyType.Bool)]
        public bool? m1744584201 { get; set; }
        [MetaProperty(1497565684, BinPropertyType.Bool)]
        public bool? m1497565684 { get; set; }
        [MetaProperty(538393296, BinPropertyType.Bool)]
        public bool? m538393296 { get; set; }
        [MetaProperty(2742376623, BinPropertyType.Bool)]
        public bool? m2742376623 { get; set; }
        [MetaProperty(2785021207, BinPropertyType.Bool)]
        public bool? m2785021207 { get; set; }
        [MetaProperty(3981460857, BinPropertyType.Bool)]
        public bool? m3981460857 { get; set; }
        [MetaProperty(935881602, BinPropertyType.Bool)]
        public bool? m935881602 { get; set; }
        [MetaProperty(3982538850, BinPropertyType.Bool)]
        public bool? m3982538850 { get; set; }
        [MetaProperty(3543285436, BinPropertyType.Bool)]
        public bool? m3543285436 { get; set; }
        [MetaProperty(4190048169, BinPropertyType.Bool)]
        public bool? m4190048169 { get; set; }
        [MetaProperty(2003991111, BinPropertyType.Bool)]
        public bool? m2003991111 { get; set; }
        [MetaProperty(2362683897, BinPropertyType.Bool)]
        public bool? m2362683897 { get; set; }
        [MetaProperty(2343239738, BinPropertyType.Bool)]
        public bool? m2343239738 { get; set; }
        [MetaProperty(519369962, BinPropertyType.Bool)]
        public bool? m519369962 { get; set; }
        [MetaProperty(1463616056, BinPropertyType.Bool)]
        public bool? m1463616056 { get; set; }
        [MetaProperty(2015410123, BinPropertyType.Bool)]
        public bool? m2015410123 { get; set; }
        [MetaProperty(3433695684, BinPropertyType.Bool)]
        public bool? m3433695684 { get; set; }
        [MetaProperty(1863033520, BinPropertyType.Bool)]
        public bool? m1863033520 { get; set; }
        [MetaProperty(314526828, BinPropertyType.Bool)]
        public bool? m314526828 { get; set; }
        [MetaProperty(3853756253, BinPropertyType.Bool)]
        public bool? m3853756253 { get; set; }
        [MetaProperty(1284507506, BinPropertyType.Bool)]
        public bool? m1284507506 { get; set; }
    }
    [MetaClass("GDSMapObjectExtraInfo")]
    public interface GDSMapObjectExtraInfo : IMetaClass
    {
    }
    [MetaClass("GDSMapObjectAnimationInfo")]
    public class GDSMapObjectAnimationInfo : GDSMapObjectExtraInfo
    {
        [MetaProperty("defaultAnimation", BinPropertyType.String)]
        public string? DefaultAnimation { get; set; }
        [MetaProperty("looping", BinPropertyType.Bool)]
        public bool? Looping { get; set; }
        [MetaProperty("destroyOnCompletion", BinPropertyType.Bool)]
        public bool? DestroyOnCompletion { get; set; }
        [MetaProperty("duration", BinPropertyType.Float)]
        public float? Duration { get; set; }
    }
    [MetaClass("GDSMapObjectLightingInfo")]
    public class GDSMapObjectLightingInfo : GDSMapObjectExtraInfo
    {
        [MetaProperty("colors", BinPropertyType.Container)]
        public MetaContainer<Vector4> Colors { get; set; }
    }
    [MetaClass("GDSMapObjectBannerInfo")]
    public class GDSMapObjectBannerInfo : GDSMapObjectExtraInfo
    {
        [MetaProperty("BannerData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? BannerData { get; set; }
    }
    [MetaClass("GdsMapObject")]
    public class GdsMapObject : GenericMapPlaceable
    {
        [MetaProperty("type", BinPropertyType.Byte)]
        public byte? Type { get; set; }
        [MetaProperty("ignoreCollisionOnPlacement", BinPropertyType.Bool)]
        public bool? IgnoreCollisionOnPlacement { get; set; }
        [MetaProperty("eyeCandy", BinPropertyType.Bool)]
        public bool? EyeCandy { get; set; }
        [MetaProperty("boxMin", BinPropertyType.Vector3)]
        public Vector3? BoxMin { get; set; }
        [MetaProperty("boxMax", BinPropertyType.Vector3)]
        public Vector3? BoxMax { get; set; }
        [MetaProperty("mapObjectSkinID", BinPropertyType.UInt32)]
        public uint? MapObjectSkinID { get; set; }
        [MetaProperty("extraInfo", BinPropertyType.Container)]
        public MetaContainer<GDSMapObjectExtraInfo> ExtraInfo { get; set; }
    }
    [MetaClass(2506702743)]
    public interface Class2506702743 : IMetaClass
    {
    }
    [MetaClass(1665937510)]
    public class Class1665937510 : IMetaClass
    {
        [MetaProperty(1274114748, BinPropertyType.UInt32)]
        public uint? m1274114748 { get; set; }
        [MetaProperty("mRarity", BinPropertyType.UInt32)]
        public uint? Rarity { get; set; }
        [MetaProperty("mValue", BinPropertyType.UInt32)]
        public uint? Value { get; set; }
    }
    [MetaClass(685945729)]
    public class Class685945729 : IMetaClass
    {
        [MetaProperty("mTags", BinPropertyType.Container)]
        public MetaContainer<string> Tags { get; set; }
        [MetaProperty(3437130819, BinPropertyType.String)]
        public string? m3437130819 { get; set; }
        [MetaProperty(225453014, BinPropertyType.String)]
        public string? m225453014 { get; set; }
        [MetaProperty(3474799429, BinPropertyType.String)]
        public string? m3474799429 { get; set; }
        [MetaProperty(876863779, BinPropertyType.Int32)]
        public int? m876863779 { get; set; }
        [MetaProperty(3819983323, BinPropertyType.Bool)]
        public bool? m3819983323 { get; set; }
        [MetaProperty(235007858, BinPropertyType.Bool)]
        public bool? m235007858 { get; set; }
    }
    [MetaClass("LootItem")]
    public class LootItem : Class2506702743
    {
        [MetaProperty(3912128722, BinPropertyType.String)]
        public string? m3912128722 { get; set; }
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(959175484, BinPropertyType.String)]
        public string? m959175484 { get; set; }
        [MetaProperty(3430910259, BinPropertyType.String)]
        public string? m3430910259 { get; set; }
        [MetaProperty(4283268666, BinPropertyType.Embedded)]
        public MetaEmbedded<Class685945729> m4283268666 { get; set; }
        [MetaProperty(2369037022, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1665937510> m2369037022 { get; set; }
    }
    [MetaClass("ClientStateCommonSettings")]
    public class ClientStateCommonSettings : IMetaClass
    {
        [MetaProperty(1530984701, BinPropertyType.UInt32)]
        public uint? m1530984701 { get; set; }
        [MetaProperty(1788708839, BinPropertyType.UInt32)]
        public uint? m1788708839 { get; set; }
        [MetaProperty(4025033036, BinPropertyType.UInt32)]
        public uint? m4025033036 { get; set; }
    }
    [MetaClass(1790162312)]
    public class Class1790162312 : IMetaClass
    {
        [MetaProperty(1953428173, BinPropertyType.String)]
        public string? m1953428173 { get; set; }
    }
    [MetaClass(1295117638)]
    public class Class1295117638 : IMetaClass
    {
        [MetaProperty(3844885605, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1790162312> m3844885605 { get; set; }
        [MetaProperty(1384195201, BinPropertyType.Embedded)]
        public MetaEmbedded<CatalogEntry> m1384195201 { get; set; }
    }
    [MetaClass(347010316)]
    public class Class347010316 : IMetaClass
    {
        [MetaProperty(822482004, BinPropertyType.String)]
        public string? m822482004 { get; set; }
        [MetaProperty("mOrder", BinPropertyType.Byte)]
        public byte? Order { get; set; }
        [MetaProperty("mItemIDs", BinPropertyType.Container)]
        public MetaContainer<int> ItemIDs { get; set; }
        [MetaProperty(2029937751, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class347010316>> m2029937751 { get; set; }
    }
    [MetaClass(2673469741)]
    public class Class2673469741 : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mOrder", BinPropertyType.Byte)]
        public byte? Order { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty(2029937751, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class347010316>> m2029937751 { get; set; }
    }
    [MetaClass("Map")]
    public class Map : WadFileDescriptor
    {
        [MetaProperty("mapStringId", BinPropertyType.String)]
        public string? MapStringId { get; set; }
        [MetaProperty("BasedOnMap", BinPropertyType.ObjectLink)]
        public MetaObjectLink? BasedOnMap { get; set; }
        [MetaProperty("characterLists", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> CharacterLists { get; set; }
        [MetaProperty(1732568803, BinPropertyType.Byte)]
        public byte? m1732568803 { get; set; }
        [MetaProperty(2650904341, BinPropertyType.Embedded)]
        public MetaEmbedded<MapVisibilityFlagDefinitions> m2650904341 { get; set; }
        [MetaProperty(1830007018, BinPropertyType.Embedded)]
        public MetaEmbedded<MapNavigationGridOverlays> m1830007018 { get; set; }
    }
    [MetaClass(3419333123)]
    public interface Class3419333123 : IMetaClass
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        float? StartTime { get; set; }
    }
    [MetaClass(3013740817)]
    public class Class3013740817 : Class3419333123
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float? StartTime { get; set; }
        [MetaProperty("PropName", BinPropertyType.String)]
        public string? PropName { get; set; }
        [MetaProperty("animationName", BinPropertyType.String)]
        public string? AnimationName { get; set; }
        [MetaProperty("looping", BinPropertyType.Bool)]
        public bool? Looping { get; set; }
    }
    [MetaClass(2134659206)]
    public class Class2134659206 : Class3419333123
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float? StartTime { get; set; }
        [MetaProperty(40023207, BinPropertyType.String)]
        public string? m40023207 { get; set; }
        [MetaProperty(4157284869, BinPropertyType.String)]
        public string? m4157284869 { get; set; }
    }
    [MetaClass(1239049582)]
    public class Class1239049582 : Class3419333123
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float? StartTime { get; set; }
        [MetaProperty("PropName", BinPropertyType.String)]
        public string? PropName { get; set; }
        [MetaProperty("Key", BinPropertyType.String)]
        public string? Key { get; set; }
        [MetaProperty("value", BinPropertyType.Float)]
        public float? Value { get; set; }
    }
    [MetaClass(3470174985)]
    public class Class3470174985 : Class3419333123
    {
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float? StartTime { get; set; }
        [MetaProperty(518005000, BinPropertyType.Container)]
        public MetaContainer<string> m518005000 { get; set; }
        [MetaProperty(3458811670, BinPropertyType.Bool)]
        public bool? m3458811670 { get; set; }
    }
    [MetaClass("MapAlternateAsset")]
    public class MapAlternateAsset : IMetaClass
    {
        [MetaProperty(3702238593, BinPropertyType.String)]
        public string? m3702238593 { get; set; }
        [MetaProperty(1613837496, BinPropertyType.String)]
        public string? m1613837496 { get; set; }
        [MetaProperty(428283609, BinPropertyType.String)]
        public string? m428283609 { get; set; }
        [MetaProperty(2232238088, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2232238088 { get; set; }
        [MetaProperty(2538024013, BinPropertyType.Hash)]
        public MetaHash? m2538024013 { get; set; }
    }
    [MetaClass("MapAlternateAssets")]
    public class MapAlternateAssets : IMetaClass
    {
        [MetaProperty(3351283947, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapAlternateAsset>> m3351283947 { get; set; }
    }
    [MetaClass(1013623483)]
    public class Class1013623483 : GenericMapPlaceable
    {
        [MetaProperty(3847623424, BinPropertyType.String)]
        public string? m3847623424 { get; set; }
        [MetaProperty(3048556132, BinPropertyType.Container)]
        public MetaContainer<Class3419333123> m3048556132 { get; set; }
    }
    [MetaClass("MapCharacterList")]
    public class MapCharacterList : IMetaClass
    {
        [MetaProperty("characters", BinPropertyType.UnorderedContainer)]
        public MetaUnorderedContainer<MetaObjectLink> Characters { get; set; }
    }
    [MetaClass("MapLocatorArray")]
    public class MapLocatorArray : IMetaClass
    {
        [MetaProperty("locators", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapLocator>> Locators { get; set; }
    }
    [MetaClass("MapNavigationGridOverlays")]
    public class MapNavigationGridOverlays : IMetaClass
    {
        [MetaProperty("overlays", BinPropertyType.Map)]
        public Dictionary<string, MetaObjectLink> Overlays { get; set; }
    }
    [MetaClass("MapNavigationGridOverlay")]
    public class MapNavigationGridOverlay : IMetaClass
    {
        [MetaProperty("navGridFileName", BinPropertyType.String)]
        public string? NavGridFileName { get; set; }
        [MetaProperty(3403579975, BinPropertyType.String)]
        public string? m3403579975 { get; set; }
    }
    [MetaClass("MapSkinColorizationPostEffect")]
    public class MapSkinColorizationPostEffect : IMetaClass
    {
        [MetaProperty("mMultipliersRGB", BinPropertyType.Vector3)]
        public Vector3? MultipliersRGB { get; set; }
        [MetaProperty("mMultipliersSaturation", BinPropertyType.Float)]
        public float? MultipliersSaturation { get; set; }
    }
    [MetaClass("MapSkin")]
    public class MapSkin : IMetaClass
    {
        [MetaProperty("mMapContainerLink", BinPropertyType.String)]
        public string? MapContainerLink { get; set; }
        [MetaProperty("mMinimapBackgroundConfig", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapBackgroundConfig> MinimapBackgroundConfig { get; set; }
        [MetaProperty(3351283947, BinPropertyType.Embedded)]
        public MetaEmbedded<MapAlternateAssets> m3351283947 { get; set; }
        [MetaProperty("mMapObjectsCFG", BinPropertyType.String)]
        public string? MapObjectsCFG { get; set; }
        [MetaProperty("mNavigationMesh", BinPropertyType.String)]
        public string? NavigationMesh { get; set; }
        [MetaProperty(351620029, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m351620029 { get; set; }
        [MetaProperty("mWorldGeometry", BinPropertyType.String)]
        public string? WorldGeometry { get; set; }
        [MetaProperty("mWorldParticlesINI", BinPropertyType.String)]
        public string? WorldParticlesINI { get; set; }
        [MetaProperty("mColorizationPostEffect", BinPropertyType.Structure)]
        public MapSkinColorizationPostEffect ColorizationPostEffect { get; set; }
        [MetaProperty("mGrassTintTexture", BinPropertyType.String)]
        public string? GrassTintTexture { get; set; }
        [MetaProperty(2968063630, BinPropertyType.String)]
        public string? m2968063630 { get; set; }
        [MetaProperty("mObjectSkinFallbacks", BinPropertyType.Map)]
        public Dictionary<MetaHash, int> ObjectSkinFallbacks { get; set; }
        [MetaProperty(2460302967, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m2460302967 { get; set; }
    }
    [MetaClass("MapVisibilityFlagRange")]
    public class MapVisibilityFlagRange : IMetaClass
    {
        [MetaProperty("minIndex", BinPropertyType.Byte)]
        public byte? MinIndex { get; set; }
        [MetaProperty("maxIndex", BinPropertyType.Byte)]
        public byte? MaxIndex { get; set; }
    }
    [MetaClass("MapVisibilityFlagDefinition")]
    public class MapVisibilityFlagDefinition : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.Hash)]
        public MetaHash? Name { get; set; }
        [MetaProperty(313514243, BinPropertyType.String)]
        public string? m313514243 { get; set; }
        [MetaProperty("BitIndex", BinPropertyType.Byte)]
        public byte? BitIndex { get; set; }
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float? TransitionTime { get; set; }
    }
    [MetaClass("MapVisibilityFlagDefinitions")]
    public class MapVisibilityFlagDefinitions : IMetaClass
    {
        [MetaProperty(1309176603, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapVisibilityFlagDefinition>> m1309176603 { get; set; }
        [MetaProperty(2175247852, BinPropertyType.Embedded)]
        public MetaEmbedded<MapVisibilityFlagRange> m2175247852 { get; set; }
        [MetaProperty(2183354083, BinPropertyType.Bool)]
        public bool? m2183354083 { get; set; }
        [MetaProperty(1610350815, BinPropertyType.Bool)]
        public bool? m1610350815 { get; set; }
    }
    [MetaClass("MinimapBackground")]
    public class MinimapBackground : IMetaClass
    {
        [MetaProperty("mOrigin", BinPropertyType.Vector2)]
        public Vector2? Origin { get; set; }
        [MetaProperty("mSize", BinPropertyType.Vector2)]
        public Vector2? Size { get; set; }
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
    }
    [MetaClass("MinimapBackgroundConfig")]
    public class MinimapBackgroundConfig : IMetaClass
    {
        [MetaProperty("mDefaultTextureName", BinPropertyType.String)]
        public string? DefaultTextureName { get; set; }
        [MetaProperty("mCustomMinimapBackgrounds", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<MinimapBackground>> CustomMinimapBackgrounds { get; set; }
    }
    [MetaClass(1923729607)]
    public class Class1923729607 : IMetaClass
    {
        [MetaProperty(3912128722, BinPropertyType.String)]
        public string? m3912128722 { get; set; }
        [MetaProperty(2033897871, BinPropertyType.String)]
        public string? m2033897871 { get; set; }
        [MetaProperty(4163829446, BinPropertyType.Bool)]
        public bool? m4163829446 { get; set; }
    }
    [MetaClass("CheatPage")]
    public class CheatPage : IMetaClass
    {
        [MetaProperty("mCheats", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Cheats { get; set; }
    }
    [MetaClass("CheatSet")]
    public class CheatSet : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mIsPlayerFacing", BinPropertyType.Bool)]
        public bool? IsPlayerFacing { get; set; }
        [MetaProperty("mGameMutator", BinPropertyType.String)]
        public string? GameMutator { get; set; }
        [MetaProperty("mGameModeName", BinPropertyType.Hash)]
        public MetaHash? GameModeName { get; set; }
        [MetaProperty(736167517, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m736167517 { get; set; }
        [MetaProperty("mCheatPages", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CheatPage>> CheatPages { get; set; }
        [MetaProperty("mUseIconsForButtons", BinPropertyType.Bool)]
        public bool? UseIconsForButtons { get; set; }
        [MetaProperty("mIsUIAlwaysShown", BinPropertyType.Bool)]
        public bool? IsUIAlwaysShown { get; set; }
    }
    [MetaClass("CheatMenuUIData")]
    public class CheatMenuUIData : IMetaClass
    {
        [MetaProperty("mDisplayName", BinPropertyType.String)]
        public string? DisplayName { get; set; }
        [MetaProperty("mFloatingTextDisplayName", BinPropertyType.String)]
        public string? FloatingTextDisplayName { get; set; }
        [MetaProperty("mTooltipText", BinPropertyType.String)]
        public string? TooltipText { get; set; }
        [MetaProperty("mDynamicTooltipText", BinPropertyType.String)]
        public string? DynamicTooltipText { get; set; }
        [MetaProperty("mHotkey", BinPropertyType.String)]
        public string? Hotkey { get; set; }
        [MetaProperty("mHotkeys", BinPropertyType.Container)]
        public MetaContainer<string> Hotkeys { get; set; }
        [MetaProperty("mIsToggleCheat", BinPropertyType.Bool)]
        public bool? IsToggleCheat { get; set; }
    }
    [MetaClass("Cheat")]
    public class Cheat : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mIsPlayerFacing", BinPropertyType.Bool)]
        public bool? IsPlayerFacing { get; set; }
        [MetaProperty("mCheatMenuUIData", BinPropertyType.Structure)]
        public CheatMenuUIData CheatMenuUIData { get; set; }
        [MetaProperty("mRecastFrequency", BinPropertyType.Float)]
        public float? RecastFrequency { get; set; }
    }
    [MetaClass("ScriptCheat")]
    public class ScriptCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mScriptCallback", BinPropertyType.Hash)]
        public MetaHash? ScriptCallback { get; set; }
    }
    [MetaClass("AddGoldCheat")]
    public class AddGoldCheat : Cheat
    {
        [MetaProperty("mGoldAmount", BinPropertyType.Float)]
        public float? GoldAmount { get; set; }
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
    }
    [MetaClass("MaxAllSkillsCheat")]
    public class MaxAllSkillsCheat : Cheat
    {
        [MetaProperty("mOnlyOnePointEach", BinPropertyType.Bool)]
        public bool? OnlyOnePointEach { get; set; }
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
    }
    [MetaClass("AddExperienceCheat")]
    public class AddExperienceCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mGiveMaxLevel", BinPropertyType.Bool)]
        public bool? GiveMaxLevel { get; set; }
    }
    [MetaClass("ToggleBuffCheat")]
    public class ToggleBuffCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string? BuffName { get; set; }
    }
    [MetaClass("AddHealthCheat")]
    public class AddHealthCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mAmount", BinPropertyType.Float)]
        public float? Amount { get; set; }
    }
    [MetaClass("AddPARCheat")]
    public class AddPARCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mAmount", BinPropertyType.Float)]
        public float? Amount { get; set; }
    }
    [MetaClass("ToggleRegenCheat")]
    public class ToggleRegenCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mToggleHP", BinPropertyType.Bool)]
        public bool? ToggleHP { get; set; }
        [MetaProperty("mTogglePAR", BinPropertyType.Bool)]
        public bool? TogglePAR { get; set; }
    }
    [MetaClass("ClearTargetCooldownCheat")]
    public class ClearTargetCooldownCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
    }
    [MetaClass("TimeMultiplierCheat")]
    public class TimeMultiplierCheat : Cheat
    {
        [MetaProperty("mSpeedUp", BinPropertyType.Bool)]
        public bool? SpeedUp { get; set; }
        [MetaProperty("mSpeedDown", BinPropertyType.Bool)]
        public bool? SpeedDown { get; set; }
    }
    [MetaClass("DamageUnitCheat")]
    public class DamageUnitCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mDamageAmount", BinPropertyType.UInt32)]
        public uint? DamageAmount { get; set; }
        [MetaProperty("mPercentageOfAttack", BinPropertyType.Float)]
        public float? PercentageOfAttack { get; set; }
        [MetaProperty("mDamageType", BinPropertyType.UInt32)]
        public uint? DamageType { get; set; }
        [MetaProperty("mHitResult", BinPropertyType.UInt32)]
        public uint? HitResult { get; set; }
    }
    [MetaClass("ToggleBarracksCheat")]
    public class ToggleBarracksCheat : Cheat
    {
        [MetaProperty("mKillExistingMinions", BinPropertyType.Bool)]
        public bool? KillExistingMinions { get; set; }
        [MetaProperty("mKillWards", BinPropertyType.Bool)]
        public bool? KillWards { get; set; }
    }
    [MetaClass("ToggleTeamCheat")]
    public class ToggleTeamCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
    }
    [MetaClass("SetRespawnTimerCheat")]
    public class SetRespawnTimerCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
        [MetaProperty("mTimerValue", BinPropertyType.Float)]
        public float? TimerValue { get; set; }
    }
    [MetaClass("ToggleInvulnerableCheat")]
    public class ToggleInvulnerableCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
    }
    [MetaClass("KillAllTurretsCheat")]
    public class KillAllTurretsCheat : Cheat
    {
    }
    [MetaClass("ForceSpawnNeutralCampsCheat")]
    public class ForceSpawnNeutralCampsCheat : Cheat
    {
        [MetaProperty("mSpawnBaron", BinPropertyType.Bool)]
        public bool? SpawnBaron { get; set; }
    }
    [MetaClass("ResetGoldCheat")]
    public class ResetGoldCheat : Cheat
    {
        [MetaProperty("mTarget", BinPropertyType.UInt32)]
        public uint? Target { get; set; }
    }
    [MetaClass("TogglePlantFastRespawnCheat")]
    public class TogglePlantFastRespawnCheat : Cheat
    {
    }
    [MetaClass("SwapChampionCheat")]
    public class SwapChampionCheat : Cheat
    {
    }
    [MetaClass("PerkEffectAmountPerMode")]
    public class PerkEffectAmountPerMode : IMetaClass
    {
        [MetaProperty(2562594311, BinPropertyType.Map)]
        public Dictionary<MetaHash, float> m2562594311 { get; set; }
    }
    [MetaClass("PerkScriptData")]
    public class PerkScriptData : IMetaClass
    {
        [MetaProperty("mEffectAmount", BinPropertyType.Map)]
        public Dictionary<MetaHash, float> EffectAmount { get; set; }
        [MetaProperty(2427468992, BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<PerkEffectAmountPerMode>> m2427468992 { get; set; }
        [MetaProperty(3702070358, BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> m3702070358 { get; set; }
    }
    [MetaClass("PerkScript")]
    public class PerkScript : IMetaClass
    {
        [MetaProperty("mSpellScriptName", BinPropertyType.String)]
        public string? SpellScriptName { get; set; }
        [MetaProperty("mSpellScript", BinPropertyType.Structure)]
        public LolSpellScript SpellScript { get; set; }
        [MetaProperty("mSpellScriptData", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkScriptData> SpellScriptData { get; set; }
    }
    [MetaClass("PerkBuff")]
    public class PerkBuff : IMetaClass
    {
        [MetaProperty("mBuffScriptName", BinPropertyType.String)]
        public string? BuffScriptName { get; set; }
        [MetaProperty("mBuffSpellObject", BinPropertyType.Embedded)]
        public MetaEmbedded<SpellObject> BuffSpellObject { get; set; }
    }
    [MetaClass("BasePerk")]
    public interface BasePerk : IMetaClass
    {
        [MetaProperty("mPerkId", BinPropertyType.UInt32)]
        uint? PerkId { get; set; }
        [MetaProperty("mPerkName", BinPropertyType.String)]
        string? PerkName { get; set; }
        [MetaProperty("mDisplayNameLocalizationKey", BinPropertyType.String)]
        string? DisplayNameLocalizationKey { get; set; }
        [MetaProperty("mTooltipNameLocalizationKey", BinPropertyType.String)]
        string? TooltipNameLocalizationKey { get; set; }
        [MetaProperty("mPingTextLocalizationKey", BinPropertyType.String)]
        string? PingTextLocalizationKey { get; set; }
        [MetaProperty("mShortDescLocalizationKey", BinPropertyType.String)]
        string? ShortDescLocalizationKey { get; set; }
        [MetaProperty("mLongDescLocalizationKey", BinPropertyType.String)]
        string? LongDescLocalizationKey { get; set; }
        [MetaProperty("mEndOfGameStatDescriptions", BinPropertyType.Container)]
        MetaContainer<string> EndOfGameStatDescriptions { get; set; }
        [MetaProperty("mDisplayStatLocalizationKey", BinPropertyType.String)]
        string? DisplayStatLocalizationKey { get; set; }
        [MetaProperty("mIconTextureName", BinPropertyType.String)]
        string? IconTextureName { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        bool? Enabled { get; set; }
        [MetaProperty("mStackable", BinPropertyType.Bool)]
        bool? Stackable { get; set; }
        [MetaProperty("mScript", BinPropertyType.Structure)]
        PerkScript Script { get; set; }
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        MetaContainer<PerkBuff> Buffs { get; set; }
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        ResourceResolver VFXResourceResolver { get; set; }
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        MetaContainer<string> Characters { get; set; }
    }
    [MetaClass("PerkStyle")]
    public class PerkStyle : IMetaClass
    {
        [MetaProperty("mPerkStyleId", BinPropertyType.UInt32)]
        public uint? PerkStyleId { get; set; }
        [MetaProperty("mPerkStyleName", BinPropertyType.String)]
        public string? PerkStyleName { get; set; }
        [MetaProperty("mDisplayNameLocalizationKey", BinPropertyType.String)]
        public string? DisplayNameLocalizationKey { get; set; }
        [MetaProperty("mTooltipNameLocalizationKey", BinPropertyType.String)]
        public string? TooltipNameLocalizationKey { get; set; }
        [MetaProperty("mDefaultPageLocalizationKey", BinPropertyType.String)]
        public string? DefaultPageLocalizationKey { get; set; }
        [MetaProperty("mPingTextLocalizationKey", BinPropertyType.String)]
        public string? PingTextLocalizationKey { get; set; }
        [MetaProperty("mIconTextureName", BinPropertyType.String)]
        public string? IconTextureName { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mIsAdvancedStyle", BinPropertyType.Bool)]
        public bool? IsAdvancedStyle { get; set; }
        [MetaProperty("mAllowedSubStyles", BinPropertyType.Container)]
        public MetaContainer<uint> AllowedSubStyles { get; set; }
        [MetaProperty("mSubStyleBonus", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<PerkSubStyleBonus>> SubStyleBonus { get; set; }
        [MetaProperty("mLCUAssetFileMap", BinPropertyType.Map)]
        public Dictionary<string, string> LCUAssetFileMap { get; set; }
        [MetaProperty("mDefaultSplash", BinPropertyType.Structure)]
        public DefaultSplashedPerkStyle DefaultSplash { get; set; }
        [MetaProperty("mDefaultPerksWhenSplashed", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> DefaultPerksWhenSplashed { get; set; }
        [MetaProperty(262465954, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DefaultStatModPerkSet>> m262465954 { get; set; }
        [MetaProperty("mSlots", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<PerkSlot>> Slots { get; set; }
        [MetaProperty("mSlotlinks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Slotlinks { get; set; }
        [MetaProperty("mScript", BinPropertyType.Structure)]
        public PerkScript Script { get; set; }
        [MetaProperty("mScriptAsSubStyle", BinPropertyType.Structure)]
        public PerkScript ScriptAsSubStyle { get; set; }
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        public MetaContainer<PerkBuff> Buffs { get; set; }
        [MetaProperty("mStyleVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver StyleVFXResourceResolver { get; set; }
    }
    [MetaClass("PerkSlot")]
    public class PerkSlot : IMetaClass
    {
        [MetaProperty("mSlotLabelKey", BinPropertyType.String)]
        public string? SlotLabelKey { get; set; }
        [MetaProperty("mType", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("mPerks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Perks { get; set; }
    }
    [MetaClass("DefaultSplashedPerkStyle")]
    public class DefaultSplashedPerkStyle : IMetaClass
    {
        [MetaProperty("mStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Style { get; set; }
        [MetaProperty("mPerk1", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Perk1 { get; set; }
        [MetaProperty("mPerk2", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Perk2 { get; set; }
    }
    [MetaClass("DefaultStatModPerkSet")]
    public class DefaultStatModPerkSet : IMetaClass
    {
        [MetaProperty("mStyleId", BinPropertyType.UInt32)]
        public uint? StyleId { get; set; }
        [MetaProperty("mPerks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Perks { get; set; }
    }
    [MetaClass("PerkSubStyleBonus")]
    public class PerkSubStyleBonus : IMetaClass
    {
        [MetaProperty("mStyleId", BinPropertyType.UInt32)]
        public uint? StyleId { get; set; }
        [MetaProperty("mPerk", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Perk { get; set; }
    }
    [MetaClass("SummonerSpellPerkReplacement")]
    public class SummonerSpellPerkReplacement : IMetaClass
    {
        [MetaProperty("mSummonerSpellRequired", BinPropertyType.Hash)]
        public MetaHash? SummonerSpellRequired { get; set; }
        [MetaProperty(3565838065, BinPropertyType.Hash)]
        public MetaHash? m3565838065 { get; set; }
    }
    [MetaClass("SummonerSpellPerkReplacementList")]
    public class SummonerSpellPerkReplacementList : IMetaClass
    {
        [MetaProperty("mReplacements", BinPropertyType.Container)]
        public MetaContainer<SummonerSpellPerkReplacement> Replacements { get; set; }
    }
    [MetaClass("Perk")]
    public class Perk : BasePerk
    {
        [MetaProperty("mPerkId", BinPropertyType.UInt32)]
        public uint? PerkId { get; set; }
        [MetaProperty("mPerkName", BinPropertyType.String)]
        public string? PerkName { get; set; }
        [MetaProperty("mDisplayNameLocalizationKey", BinPropertyType.String)]
        public string? DisplayNameLocalizationKey { get; set; }
        [MetaProperty("mTooltipNameLocalizationKey", BinPropertyType.String)]
        public string? TooltipNameLocalizationKey { get; set; }
        [MetaProperty("mPingTextLocalizationKey", BinPropertyType.String)]
        public string? PingTextLocalizationKey { get; set; }
        [MetaProperty("mShortDescLocalizationKey", BinPropertyType.String)]
        public string? ShortDescLocalizationKey { get; set; }
        [MetaProperty("mLongDescLocalizationKey", BinPropertyType.String)]
        public string? LongDescLocalizationKey { get; set; }
        [MetaProperty("mEndOfGameStatDescriptions", BinPropertyType.Container)]
        public MetaContainer<string> EndOfGameStatDescriptions { get; set; }
        [MetaProperty("mDisplayStatLocalizationKey", BinPropertyType.String)]
        public string? DisplayStatLocalizationKey { get; set; }
        [MetaProperty("mIconTextureName", BinPropertyType.String)]
        public string? IconTextureName { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mStackable", BinPropertyType.Bool)]
        public bool? Stackable { get; set; }
        [MetaProperty("mScript", BinPropertyType.Structure)]
        public PerkScript Script { get; set; }
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        public MetaContainer<PerkBuff> Buffs { get; set; }
        [MetaProperty("mVFXResourceResolver", BinPropertyType.Structure)]
        public ResourceResolver VFXResourceResolver { get; set; }
        [MetaProperty("mCharacters", BinPropertyType.Container)]
        public MetaContainer<string> Characters { get; set; }
        [MetaProperty("mDefault", BinPropertyType.Bool)]
        public bool? Default { get; set; }
        [MetaProperty("mMajorChangePatchVersion", BinPropertyType.String)]
        public string? MajorChangePatchVersion { get; set; }
        [MetaProperty("mSummonerPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<SummonerSpellPerkReplacementList> SummonerPerkReplacements { get; set; }
    }
    [MetaClass("PerkReplacement")]
    public class PerkReplacement : IMetaClass
    {
        [MetaProperty("mReplaceTarget", BinPropertyType.Hash)]
        public MetaHash? ReplaceTarget { get; set; }
        [MetaProperty("mReplaceWith", BinPropertyType.Hash)]
        public MetaHash? ReplaceWith { get; set; }
    }
    [MetaClass("PerkReplacementList")]
    public class PerkReplacementList : IMetaClass
    {
        [MetaProperty("mReplacements", BinPropertyType.Container)]
        public MetaContainer<PerkReplacement> Replacements { get; set; }
    }
    [MetaClass("PerkConfig")]
    public class PerkConfig : IMetaClass
    {
        [MetaProperty("mBotOverrideSet", BinPropertyType.ObjectLink)]
        public MetaObjectLink? BotOverrideSet { get; set; }
        [MetaProperty("mPerkReplacements", BinPropertyType.Embedded)]
        public MetaEmbedded<PerkReplacementList> PerkReplacements { get; set; }
        [MetaProperty(277771373, BinPropertyType.UInt32)]
        public uint? m277771373 { get; set; }
    }
    [MetaClass("OverridePerkSelectionSet")]
    public class OverridePerkSelectionSet : IMetaClass
    {
        [MetaProperty("mStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Style { get; set; }
        [MetaProperty("mSubStyle", BinPropertyType.ObjectLink)]
        public MetaObjectLink? SubStyle { get; set; }
        [MetaProperty("mPerks", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Perks { get; set; }
    }
    [MetaClass("ToonInkingFilterParams")]
    public class ToonInkingFilterParams : IMetaClass
    {
        [MetaProperty("mPixelSize", BinPropertyType.Float)]
        public float? PixelSize { get; set; }
        [MetaProperty("mMinVal", BinPropertyType.Float)]
        public float? MinVal { get; set; }
        [MetaProperty("mMaxVal", BinPropertyType.Float)]
        public float? MaxVal { get; set; }
        [MetaProperty("mResultScale", BinPropertyType.Float)]
        public float? ResultScale { get; set; }
    }
    [MetaClass("RenderStyleData")]
    public class RenderStyleData : IMetaClass
    {
        [MetaProperty("mUnitFilterParamsInterior", BinPropertyType.Embedded)]
        public MetaEmbedded<ToonInkingFilterParams> UnitFilterParamsInterior { get; set; }
        [MetaProperty("mUnitFilterParamsExterior", BinPropertyType.Embedded)]
        public MetaEmbedded<ToonInkingFilterParams> UnitFilterParamsExterior { get; set; }
        [MetaProperty("mUnitStyleUseInking", BinPropertyType.Bool)]
        public bool? UnitStyleUseInking { get; set; }
    }
    [MetaClass("MaterialOverrideCallbackDynamicMaterial")]
    public class MaterialOverrideCallbackDynamicMaterial : IMetaClass
    {
    }
    [MetaClass("MouseOverEffectData")]
    public class MouseOverEffectData : IMetaClass
    {
        [MetaProperty("mAllyColor", BinPropertyType.Color)]
        public Color? AllyColor { get; set; }
        [MetaProperty("mEnemyColor", BinPropertyType.Color)]
        public Color? EnemyColor { get; set; }
        [MetaProperty("mNeutralColor", BinPropertyType.Color)]
        public Color? NeutralColor { get; set; }
        [MetaProperty("mSelfColor", BinPropertyType.Color)]
        public Color? SelfColor { get; set; }
        [MetaProperty("mMouseOverSize", BinPropertyType.Int32)]
        public int? MouseOverSize { get; set; }
        [MetaProperty("mMouseOverColorFactor", BinPropertyType.Float)]
        public float? MouseOverColorFactor { get; set; }
        [MetaProperty("mMouseOverBlurPassCount", BinPropertyType.UInt32)]
        public uint? MouseOverBlurPassCount { get; set; }
        [MetaProperty("mSelectedSize", BinPropertyType.Int32)]
        public int? SelectedSize { get; set; }
        [MetaProperty("mSelectedColorFactor", BinPropertyType.Float)]
        public float? SelectedColorFactor { get; set; }
        [MetaProperty("mSelectedBlurPassCount", BinPropertyType.UInt32)]
        public uint? SelectedBlurPassCount { get; set; }
        [MetaProperty("mAvatarSize", BinPropertyType.Int32)]
        public int? AvatarSize { get; set; }
        [MetaProperty("mAvatarColorFactor", BinPropertyType.Float)]
        public float? AvatarColorFactor { get; set; }
        [MetaProperty("mAvatarColor", BinPropertyType.Color)]
        public Color? AvatarColor { get; set; }
        [MetaProperty("mAvatarBlurPassCount", BinPropertyType.UInt32)]
        public uint? AvatarBlurPassCount { get; set; }
        [MetaProperty("mKillerSize", BinPropertyType.Int32)]
        public int? KillerSize { get; set; }
        [MetaProperty("mKillerColorFactor", BinPropertyType.Float)]
        public float? KillerColorFactor { get; set; }
        [MetaProperty("mKillerBlurPassCount", BinPropertyType.UInt32)]
        public uint? KillerBlurPassCount { get; set; }
        [MetaProperty("mInteractionTimes", BinPropertyType.Container)]
        public MetaContainer<float> InteractionTimes { get; set; }
        [MetaProperty("mInteractionSizes", BinPropertyType.Container)]
        public MetaContainer<int> InteractionSizes { get; set; }
    }
    [MetaClass("GameplayFeatureToggles")]
    public class GameplayFeatureToggles : IMetaClass
    {
        [MetaProperty("NewActorStuckPathfinding", BinPropertyType.Bool)]
        public bool? NewActorStuckPathfinding { get; set; }
        [MetaProperty("fowCastRayAccurate", BinPropertyType.Bool)]
        public bool? FowCastRayAccurate { get; set; }
        [MetaProperty("disableSpellLevelMinimumProtections", BinPropertyType.Bool)]
        public bool? DisableSpellLevelMinimumProtections { get; set; }
        [MetaProperty("IndividualItemVisibility", BinPropertyType.Bool)]
        public bool? IndividualItemVisibility { get; set; }
        [MetaProperty("AFKDetection2", BinPropertyType.Bool)]
        public bool? AFKDetection2 { get; set; }
    }
    [MetaClass("ICastRequirement")]
    public interface ICastRequirement : IMetaClass
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        bool? InvertResult { get; set; }
    }
    [MetaClass("HasAllSubRequirementsCastRequirement")]
    public class HasAllSubRequirementsCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mSubRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> SubRequirements { get; set; }
    }
    [MetaClass("HasNNearbyUnitsRequirement")]
    public class HasNNearbyUnitsRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mUnitsRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> UnitsRequirements { get; set; }
        [MetaProperty("mUnitsRequired", BinPropertyType.UInt32)]
        public uint? UnitsRequired { get; set; }
        [MetaProperty("mRange", BinPropertyType.Float)]
        public float? Range { get; set; }
        [MetaProperty("mDistanceType", BinPropertyType.UInt32)]
        public uint? DistanceType { get; set; }
    }
    [MetaClass("HasNNearbyVisibleUnitsRequirement")]
    public class HasNNearbyVisibleUnitsRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mUnitsRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> UnitsRequirements { get; set; }
        [MetaProperty("mUnitsRequired", BinPropertyType.UInt32)]
        public uint? UnitsRequired { get; set; }
        [MetaProperty("mRange", BinPropertyType.Float)]
        public float? Range { get; set; }
        [MetaProperty("mDistanceType", BinPropertyType.UInt32)]
        public uint? DistanceType { get; set; }
    }
    [MetaClass("HasTypeAndStatusFlags")]
    public class HasTypeAndStatusFlags : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mAffectsTypeFlags", BinPropertyType.UInt32)]
        public uint? AffectsTypeFlags { get; set; }
        [MetaProperty("mAffectsStatusFlags", BinPropertyType.UInt32)]
        public uint? AffectsStatusFlags { get; set; }
    }
    [MetaClass("HasAtleastNSubRequirementsCastRequirement")]
    public class HasAtleastNSubRequirementsCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mSuccessesRequired", BinPropertyType.UInt32)]
        public uint? SuccessesRequired { get; set; }
        [MetaProperty("mSubRequirements", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> SubRequirements { get; set; }
    }
    [MetaClass("HasUnitTagsCastRequirement")]
    public class HasUnitTagsCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> UnitTags { get; set; }
    }
    [MetaClass("SameTeamCastRequirement")]
    public class SameTeamCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
    }
    [MetaClass("HasBuffCastRequirement")]
    public class HasBuffCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mBuffName", BinPropertyType.Hash)]
        public MetaHash? BuffName { get; set; }
        [MetaProperty("mFromAnyone", BinPropertyType.Bool)]
        public bool? FromAnyone { get; set; }
    }
    [MetaClass("AboveHealthPercentCastRequirement")]
    public class AboveHealthPercentCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mCurrentPercentHealth", BinPropertyType.Float)]
        public float? CurrentPercentHealth { get; set; }
    }
    [MetaClass("AbovePARPercentCastRequirement")]
    public class AbovePARPercentCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mPARType", BinPropertyType.Byte)]
        public byte? PARType { get; set; }
        [MetaProperty("mCurrentPercentPAR", BinPropertyType.Float)]
        public float? CurrentPercentPAR { get; set; }
    }
    [MetaClass("IsSpecifiedUnitCastRequirement")]
    public class IsSpecifiedUnitCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
        [MetaProperty("mUnit", BinPropertyType.Hash)]
        public MetaHash? Unit { get; set; }
    }
    [MetaClass("ItemSlotHasChargesCastRequirement")]
    public class ItemSlotHasChargesCastRequirement : ICastRequirement
    {
        [MetaProperty("mInvertResult", BinPropertyType.Bool)]
        public bool? InvertResult { get; set; }
    }
    [MetaClass("CCScoreMultipliers")]
    public class CCScoreMultipliers : IMetaClass
    {
        [MetaProperty("stun", BinPropertyType.Float)]
        public float? Stun { get; set; }
        [MetaProperty("taunt", BinPropertyType.Float)]
        public float? Taunt { get; set; }
        [MetaProperty("fear", BinPropertyType.Float)]
        public float? Fear { get; set; }
        [MetaProperty("flee", BinPropertyType.Float)]
        public float? Flee { get; set; }
        [MetaProperty("suppression", BinPropertyType.Float)]
        public float? Suppression { get; set; }
        [MetaProperty("knockup", BinPropertyType.Float)]
        public float? Knockup { get; set; }
        [MetaProperty("knockback", BinPropertyType.Float)]
        public float? Knockback { get; set; }
        [MetaProperty("polymorph", BinPropertyType.Float)]
        public float? Polymorph { get; set; }
        [MetaProperty("root", BinPropertyType.Float)]
        public float? Root { get; set; }
        [MetaProperty("silence", BinPropertyType.Float)]
        public float? Silence { get; set; }
        [MetaProperty("charm", BinPropertyType.Float)]
        public float? Charm { get; set; }
        [MetaProperty("slow", BinPropertyType.Float)]
        public float? Slow { get; set; }
        [MetaProperty("attackSpeedSlow", BinPropertyType.Float)]
        public float? AttackSpeedSlow { get; set; }
        [MetaProperty("blind", BinPropertyType.Float)]
        public float? Blind { get; set; }
        [MetaProperty("disarm", BinPropertyType.Float)]
        public float? Disarm { get; set; }
        [MetaProperty("grounded", BinPropertyType.Float)]
        public float? Grounded { get; set; }
        [MetaProperty("nearsight", BinPropertyType.Float)]
        public float? Nearsight { get; set; }
        [MetaProperty("drowsy", BinPropertyType.Float)]
        public float? Drowsy { get; set; }
        [MetaProperty("asleep", BinPropertyType.Float)]
        public float? Asleep { get; set; }
    }
    [MetaClass("BuffData")]
    public class BuffData : IMetaClass
    {
        [MetaProperty("mDescription", BinPropertyType.String)]
        public string? Description { get; set; }
        [MetaProperty("mTooltipData", BinPropertyType.Structure)]
        public TooltipInstanceBuff TooltipData { get; set; }
        [MetaProperty("mVfxSpawnConditions", BinPropertyType.Container)]
        public MetaContainer<VfxSpawnConditions> VfxSpawnConditions { get; set; }
        [MetaProperty("mShowDuration", BinPropertyType.Bool)]
        public bool? ShowDuration { get; set; }
        [MetaProperty(13638081, BinPropertyType.Bool)]
        public bool? m13638081 { get; set; }
        [MetaProperty("mFloatVarsDecimals", BinPropertyType.Container)]
        public MetaContainer<int> FloatVarsDecimals { get; set; }
        [MetaProperty(3621963549, BinPropertyType.Byte)]
        public byte? m3621963549 { get; set; }
    }
    [MetaClass("TeamBuffData")]
    public class TeamBuffData : IMetaClass
    {
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string? BuffName { get; set; }
        [MetaProperty(3736966593, BinPropertyType.String)]
        public string? m3736966593 { get; set; }
        [MetaProperty(2589193282, BinPropertyType.Bool)]
        public bool? m2589193282 { get; set; }
    }
    [MetaClass("MissionBuffData")]
    public class MissionBuffData : IMetaClass
    {
        [MetaProperty("dragon", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> Dragon { get; set; }
        [MetaProperty("fireDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> FireDrake { get; set; }
        [MetaProperty("airDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> AirDrake { get; set; }
        [MetaProperty("waterDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> WaterDrake { get; set; }
        [MetaProperty("earthDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> EarthDrake { get; set; }
        [MetaProperty("elderDrake", BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> ElderDrake { get; set; }
        [MetaProperty(2414492958, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2414492958 { get; set; }
        [MetaProperty(2397715339, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2397715339 { get; set; }
        [MetaProperty(2380937720, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2380937720 { get; set; }
        [MetaProperty(2498381053, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2498381053 { get; set; }
        [MetaProperty(2481603434, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2481603434 { get; set; }
        [MetaProperty(2464825815, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2464825815 { get; set; }
        [MetaProperty(2448048196, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2448048196 { get; set; }
        [MetaProperty(2297049625, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2297049625 { get; set; }
        [MetaProperty(2280272006, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m2280272006 { get; set; }
        [MetaProperty(3149811562, BinPropertyType.Embedded)]
        public MetaEmbedded<TeamBuffData> m3149811562 { get; set; }
    }
    [MetaClass("BuffStackingTemplate")]
    public class BuffStackingTemplate : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("maxStacks", BinPropertyType.Int32)]
        public int? MaxStacks { get; set; }
        [MetaProperty(3010375308, BinPropertyType.Int32)]
        public int? m3010375308 { get; set; }
        [MetaProperty("StacksExclusive", BinPropertyType.Bool)]
        public bool? StacksExclusive { get; set; }
        [MetaProperty("BuffAddType", BinPropertyType.UInt32)]
        public uint? BuffAddType { get; set; }
    }
    [MetaClass("BuffStackingSettings")]
    public class BuffStackingSettings : IMetaClass
    {
        [MetaProperty("templateDefinition", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<BuffStackingTemplate>> TemplateDefinition { get; set; }
    }
    [MetaClass("VFXSpawnConditionData")]
    public interface VFXSpawnConditionData : IMetaClass
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
    }
    [MetaClass("VFXDefaultSpawnConditionData")]
    public class VFXDefaultSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
    }
    [MetaClass("HasBuffData")]
    public class HasBuffData : IMetaClass
    {
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string? BuffName { get; set; }
        [MetaProperty("mFromAnyone", BinPropertyType.Bool)]
        public bool? FromAnyone { get; set; }
        [MetaProperty("mFromOwner", BinPropertyType.Bool)]
        public bool? FromOwner { get; set; }
        [MetaProperty(3118580291, BinPropertyType.Bool)]
        public bool? m3118580291 { get; set; }
    }
    [MetaClass("HasBuffComparisonData")]
    public class HasBuffComparisonData : IMetaClass
    {
        [MetaProperty("mBuffs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HasBuffData>> Buffs { get; set; }
        [MetaProperty("mCompareOp", BinPropertyType.Byte)]
        public byte? CompareOp { get; set; }
    }
    [MetaClass("HasBuffSpawnConditionData")]
    public class HasBuffSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
        [MetaProperty("mBuffComparisons", BinPropertyType.Embedded)]
        public MetaEmbedded<HasBuffComparisonData> BuffComparisons { get; set; }
    }
    [MetaClass("IsSkinSpawnConditionData")]
    public class IsSkinSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
        [MetaProperty("mSkinId", BinPropertyType.UInt32)]
        public uint? SkinId { get; set; }
    }
    [MetaClass("IsOwnerHeroConditionData")]
    public class IsOwnerHeroConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
    }
    [MetaClass("IsOwnerAliveConditionData")]
    public class IsOwnerAliveConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
    }
    [MetaClass("HasSpellRankSpawnConditionData")]
    public class HasSpellRankSpawnConditionData : VFXSpawnConditionData
    {
        [MetaProperty("mPersistentVfxs", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<EffectCreationData>> PersistentVfxs { get; set; }
        [MetaProperty("mSpellSlot", BinPropertyType.UInt32)]
        public uint? SpellSlot { get; set; }
        [MetaProperty("mSpellLevel", BinPropertyType.Int32)]
        public int? SpellLevel { get; set; }
    }
    [MetaClass("VfxSpawnConditions")]
    public class VfxSpawnConditions : IMetaClass
    {
        [MetaProperty(2275536844, BinPropertyType.Container)]
        public MetaContainer<VFXSpawnConditionData> m2275536844 { get; set; }
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; }
    }
    [MetaClass("IVFXSpawnConditions")]
    public interface IVFXSpawnConditions : IMetaClass
    {
    }
    [MetaClass("AlwaysSpawnCondition")]
    public class AlwaysSpawnCondition : IVFXSpawnConditions
    {
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; }
    }
    [MetaClass("HasBuffNameSpawnConditions")]
    public class HasBuffNameSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HasBuffSpawnConditionData>> Conditions { get; set; }
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; }
    }
    [MetaClass("HasSkinIDSpawnConditions")]
    public class HasSkinIDSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<IsSkinSpawnConditionData>> Conditions { get; set; }
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; }
    }
    [MetaClass("IsOwnerHeroSpawnConditions")]
    public class IsOwnerHeroSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<IsOwnerHeroConditionData>> Conditions { get; set; }
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; }
    }
    [MetaClass("IsOwnerAliveSpawnConditions")]
    public class IsOwnerAliveSpawnConditions : IVFXSpawnConditions
    {
        [MetaProperty("mConditions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<IsOwnerAliveConditionData>> Conditions { get; set; }
        [MetaProperty("mDefaultVfxData", BinPropertyType.Embedded)]
        public MetaEmbedded<VFXDefaultSpawnConditionData> DefaultVfxData { get; set; }
    }
    [MetaClass("EffectCreationData")]
    public class EffectCreationData : IMetaClass
    {
        [MetaProperty("mBoneName", BinPropertyType.String)]
        public string? BoneName { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty("mEffectName", BinPropertyType.String)]
        public string? EffectName { get; set; }
        [MetaProperty("mEffectKey", BinPropertyType.Hash)]
        public MetaHash? EffectKey { get; set; }
        [MetaProperty(4269114704, BinPropertyType.String)]
        public string? m4269114704 { get; set; }
        [MetaProperty(2688193858, BinPropertyType.Hash)]
        public MetaHash? m2688193858 { get; set; }
        [MetaProperty(3291281549, BinPropertyType.Bool)]
        public bool? m3291281549 { get; set; }
        [MetaProperty(3967304360, BinPropertyType.Float)]
        public float? m3967304360 { get; set; }
        [MetaProperty(2757679739, BinPropertyType.Bool)]
        public bool? m2757679739 { get; set; }
        [MetaProperty(2270784147, BinPropertyType.UInt32)]
        public uint? m2270784147 { get; set; }
        [MetaProperty(1660255353, BinPropertyType.Bool)]
        public bool? m1660255353 { get; set; }
        [MetaProperty(2347668140, BinPropertyType.Bool)]
        public bool? m2347668140 { get; set; }
        [MetaProperty(4246608820, BinPropertyType.Bool)]
        public bool? m4246608820 { get; set; }
        [MetaProperty(1161004262, BinPropertyType.Bool)]
        public bool? m1161004262 { get; set; }
    }
    [MetaClass("RatioConversion")]
    public class RatioConversion : IMetaClass
    {
        [MetaProperty("mSourceStatType", BinPropertyType.Byte)]
        public byte? SourceStatType { get; set; }
        [MetaProperty("mSourceStatOutput", BinPropertyType.Byte)]
        public byte? SourceStatOutput { get; set; }
        [MetaProperty(346472573, BinPropertyType.Byte)]
        public byte? m346472573 { get; set; }
        [MetaProperty(205246452, BinPropertyType.Byte)]
        public byte? m205246452 { get; set; }
        [MetaProperty(2452082244, BinPropertyType.Float)]
        public float? m2452082244 { get; set; }
    }
    [MetaClass("SpellModifier")]
    public class SpellModifier : IMetaClass
    {
        [MetaProperty("mModifierID", BinPropertyType.Hash)]
        public MetaHash? ModifierID { get; set; }
        [MetaProperty(2848730102, BinPropertyType.Byte)]
        public byte? m2848730102 { get; set; }
        [MetaProperty(2759808727, BinPropertyType.Byte)]
        public byte? m2759808727 { get; set; }
        [MetaProperty(1527878389, BinPropertyType.UInt32)]
        public uint? m1527878389 { get; set; }
        [MetaProperty(1142566944, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<RatioConversion>> m1142566944 { get; set; }
    }
    [MetaClass("AbilityObject")]
    public class AbilityObject : IMetaClass
    {
        [MetaProperty("mRootSpell", BinPropertyType.ObjectLink)]
        public MetaObjectLink? RootSpell { get; set; }
        [MetaProperty("mChildSpells", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> ChildSpells { get; set; }
        [MetaProperty(2262674907, BinPropertyType.Bool)]
        public bool? m2262674907 { get; set; }
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mType", BinPropertyType.Byte)]
        public byte? Type { get; set; }
    }
    [MetaClass("SpellObject")]
    public class SpellObject : IMetaClass
    {
        [MetaProperty("mScriptName", BinPropertyType.String)]
        public string? ScriptName { get; set; }
        [MetaProperty("mScript", BinPropertyType.Structure)]
        public LolSpellScript Script { get; set; }
        [MetaProperty("mSpell", BinPropertyType.Structure)]
        public SpellDataResource Spell { get; set; }
        [MetaProperty("mBuff", BinPropertyType.Structure)]
        public BuffData Buff { get; set; }
    }
    [MetaClass("MissileSpecification")]
    public class MissileSpecification : IMetaClass
    {
        [MetaProperty("mMissileWidth", BinPropertyType.Float)]
        public float? MissileWidth { get; set; }
        [MetaProperty("movementComponent", BinPropertyType.Structure)]
        public MissileMovementSpec MovementComponent { get; set; }
        [MetaProperty("visibilityComponent", BinPropertyType.Structure)]
        public MissileVisibilitySpec VisibilityComponent { get; set; }
        [MetaProperty("heightSolver", BinPropertyType.Structure)]
        public HeightSolverType HeightSolver { get; set; }
        [MetaProperty("verticalFacing", BinPropertyType.Structure)]
        public VerticalFacingType VerticalFacing { get; set; }
        [MetaProperty("missileGroupSpawners", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MissileGroupSpawnerSpec>> MissileGroupSpawners { get; set; }
        [MetaProperty("behaviors", BinPropertyType.Container)]
        public MetaContainer<MissileBehaviorSpec> Behaviors { get; set; }
    }
    [MetaClass("MissileBehaviorSpec")]
    public interface MissileBehaviorSpec : IMetaClass
    {
    }
    [MetaClass("FixedDistanceIgnoringTerrain")]
    public class FixedDistanceIgnoringTerrain : MissileBehaviorSpec
    {
        [MetaProperty("mMaximumDistance", BinPropertyType.Float)]
        public float? MaximumDistance { get; set; }
        [MetaProperty("mMinimumGapBetweenTerrainWalls", BinPropertyType.Float)]
        public float? MinimumGapBetweenTerrainWalls { get; set; }
        [MetaProperty("mMaximumTerrainWallsToSkip", BinPropertyType.Optional)]
        public MetaOptional<uint> MaximumTerrainWallsToSkip { get; set; }
        [MetaProperty("scanWidthOverride", BinPropertyType.Optional)]
        public MetaOptional<float> ScanWidthOverride { get; set; }
        [MetaProperty("mTargeterDefinition", BinPropertyType.Structure)]
        public TargeterDefinitionSkipTerrain TargeterDefinition { get; set; }
    }
    [MetaClass("ScaleByScaleSkinCoef")]
    public class ScaleByScaleSkinCoef : MissileBehaviorSpec
    {
    }
    [MetaClass("WidthPerSecond")]
    public class WidthPerSecond : MissileBehaviorSpec
    {
        [MetaProperty("mWidthPerSecond", BinPropertyType.Float)]
        public float? mWidthPerSecond { get; set; }
    }
    [MetaClass("MissileTriggerSpec")]
    public interface MissileTriggerSpec : MissileBehaviorSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
    }
    [MetaClass("TriggerOnMovementComplete")]
    public class TriggerOnMovementComplete : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
        [MetaProperty("mDelay", BinPropertyType.Int32)]
        public int? Delay { get; set; }
    }
    [MetaClass("TriggerOnDelay")]
    public class TriggerOnDelay : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
        [MetaProperty("mDelay", BinPropertyType.Float)]
        public float? Delay { get; set; }
    }
    [MetaClass("DelayStart")]
    public class DelayStart : MissileBehaviorSpec
    {
        [MetaProperty("mDelayTime", BinPropertyType.Float)]
        public float? DelayTime { get; set; }
    }
    [MetaClass("TriggerOnStart")]
    public class TriggerOnStart : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
    }
    [MetaClass("TriggerOnHit")]
    public class TriggerOnHit : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
    }
    [MetaClass("TriggerOnDistanceFromCaster")]
    public class TriggerOnDistanceFromCaster : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
        [MetaProperty("mDistance", BinPropertyType.Float)]
        public float? Distance { get; set; }
    }
    [MetaClass("TriggerFromScript")]
    public class TriggerFromScript : MissileTriggerSpec
    {
        [MetaProperty("mActions", BinPropertyType.Container)]
        public MetaContainer<MissileTriggeredActionSpec> Actions { get; set; }
        [MetaProperty("mTriggerName", BinPropertyType.Hash)]
        public MetaHash? TriggerName { get; set; }
        [MetaProperty("mDelay", BinPropertyType.Float)]
        public float? Delay { get; set; }
    }
    [MetaClass("MissileTriggeredActionSpec")]
    public interface MissileTriggeredActionSpec : IMetaClass
    {
    }
    [MetaClass("Cast")]
    public class Cast : MissileTriggeredActionSpec
    {
    }
    [MetaClass("Destroy")]
    public class Destroy : MissileTriggeredActionSpec
    {
    }
    [MetaClass("ReturnToCaster")]
    public class ReturnToCaster : MissileTriggeredActionSpec
    {
        [MetaProperty("mPreserveSpeed", BinPropertyType.Bool)]
        public bool? PreserveSpeed { get; set; }
        [MetaProperty("mOverrideSpec", BinPropertyType.Structure)]
        public MissileMovementSpec OverrideSpec { get; set; }
    }
    [MetaClass("ClearAlreadyHitTracking")]
    public class ClearAlreadyHitTracking : MissileTriggeredActionSpec
    {
    }
    [MetaClass("CallOnMissileBounce")]
    public class CallOnMissileBounce : MissileTriggeredActionSpec
    {
    }
    [MetaClass("ClearTargetAndKeepMoving")]
    public class ClearTargetAndKeepMoving : MissileTriggeredActionSpec
    {
        [MetaProperty(1145244202, BinPropertyType.Optional)]
        public MetaOptional<float> m1145244202 { get; set; }
        [MetaProperty("mOverrideRange", BinPropertyType.Optional)]
        public MetaOptional<float> OverrideRange { get; set; }
        [MetaProperty("mOverrideMovement", BinPropertyType.Structure)]
        public MissileMovementSpec OverrideMovement { get; set; }
    }
    [MetaClass("ChangeMissileSpeed")]
    public class ChangeMissileSpeed : MissileTriggeredActionSpec
    {
        [MetaProperty("mSpeedChangeType", BinPropertyType.UInt32)]
        public uint? SpeedChangeType { get; set; }
        [MetaProperty("mSpeedValue", BinPropertyType.Float)]
        public float? SpeedValue { get; set; }
    }
    [MetaClass("ChangeTurnRadius")]
    public class ChangeTurnRadius : MissileTriggeredActionSpec
    {
        [MetaProperty(2226849642, BinPropertyType.Container)]
        public MetaContainer<float> m2226849642 { get; set; }
    }
    [MetaClass(934449797)]
    public class Class934449797 : MissileTriggeredActionSpec
    {
        [MetaProperty(1549368804, BinPropertyType.Structure)]
        public HeightSolverType m1549368804 { get; set; }
    }
    [MetaClass("DestroyOnHit")]
    public class DestroyOnHit : MissileBehaviorSpec
    {
    }
    [MetaClass("DestroyOnMovementComplete")]
    public class DestroyOnMovementComplete : MissileBehaviorSpec
    {
        [MetaProperty("mDelay", BinPropertyType.Int32)]
        public int? Delay { get; set; }
    }
    [MetaClass(3814179094)]
    public class Class3814179094 : MissileBehaviorSpec
    {
    }
    [MetaClass("CastOnHit")]
    public class CastOnHit : MissileBehaviorSpec
    {
    }
    [MetaClass("CastOnMovementComplete")]
    public class CastOnMovementComplete : MissileBehaviorSpec
    {
    }
    [MetaClass("ReturnToCasterOnMovementComplete")]
    public class ReturnToCasterOnMovementComplete : MissileBehaviorSpec
    {
        [MetaProperty("mPreserveSpeed", BinPropertyType.Bool)]
        public bool? PreserveSpeed { get; set; }
        [MetaProperty("mOverrideSpec", BinPropertyType.Structure)]
        public MissileMovementSpec OverrideSpec { get; set; }
    }
    [MetaClass("MissileGroupSpawnerSpec")]
    public class MissileGroupSpawnerSpec : IMetaClass
    {
        [MetaProperty("mChildMissileSpell", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ChildMissileSpell { get; set; }
    }
    [MetaClass("HeightSolverType")]
    public interface HeightSolverType : IMetaClass
    {
    }
    [MetaClass("GravityHeightSolver")]
    public class GravityHeightSolver : HeightSolverType
    {
        [MetaProperty("mGravity", BinPropertyType.Float)]
        public float? Gravity { get; set; }
    }
    [MetaClass("FollowTerrainHeightSolver")]
    public class FollowTerrainHeightSolver : HeightSolverType
    {
        [MetaProperty("mHeightOffset", BinPropertyType.Float)]
        public float? HeightOffset { get; set; }
        [MetaProperty(3821269113, BinPropertyType.Float)]
        public float? m3821269113 { get; set; }
    }
    [MetaClass("BlendedLinearHeightSolver")]
    public class BlendedLinearHeightSolver : HeightSolverType
    {
    }
    [MetaClass("SinusoidalHeightSolver")]
    public class SinusoidalHeightSolver : HeightSolverType
    {
        [MetaProperty("mVerticalOffset", BinPropertyType.Float)]
        public float? VerticalOffset { get; set; }
        [MetaProperty("mAmplitude", BinPropertyType.Float)]
        public float? Amplitude { get; set; }
        [MetaProperty("mNumberOfPeriods", BinPropertyType.Float)]
        public float? NumberOfPeriods { get; set; }
    }
    [MetaClass("CurveTheDifferenceHeightSolver")]
    public class CurveTheDifferenceHeightSolver : HeightSolverType
    {
        [MetaProperty("mInitialTargetHeightOffset", BinPropertyType.Float)]
        public float? InitialTargetHeightOffset { get; set; }
    }
    [MetaClass("MissileMovementSpec")]
    public interface MissileMovementSpec : IMetaClass
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        float? m2799230803 { get; set; }
    }
    [MetaClass("CircleMovement")]
    public class CircleMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mRadialVelocity", BinPropertyType.Float)]
        public float? RadialVelocity { get; set; }
        [MetaProperty("mAngularVelocity", BinPropertyType.Float)]
        public float? AngularVelocity { get; set; }
        [MetaProperty("mLinearVelocity", BinPropertyType.Float)]
        public float? LinearVelocity { get; set; }
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float? Lifetime { get; set; }
    }
    [MetaClass("SyncCircleMovement")]
    public class SyncCircleMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mAngularVelocity", BinPropertyType.Float)]
        public float? AngularVelocity { get; set; }
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float? Lifetime { get; set; }
    }
    [MetaClass("NullMovement")]
    public class NullMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mWaitForChildren", BinPropertyType.Bool)]
        public bool? WaitForChildren { get; set; }
        [MetaProperty("mDelayTime", BinPropertyType.Float)]
        public float? DelayTime { get; set; }
    }
    [MetaClass("AcceleratingMovement")]
    public class AcceleratingMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool? InferDirectionFromFacingIfNeeded { get; set; }
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool? ProjectTargetToCastRange { get; set; }
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool? UseGroundHeightAtTarget { get; set; }
        [MetaProperty("mAcceleration", BinPropertyType.Float)]
        public float? Acceleration { get; set; }
        [MetaProperty("mMinSpeed", BinPropertyType.Float)]
        public float? MinSpeed { get; set; }
        [MetaProperty("mMaxSpeed", BinPropertyType.Float)]
        public float? MaxSpeed { get; set; }
        [MetaProperty("mInitialSpeed", BinPropertyType.Float)]
        public float? InitialSpeed { get; set; }
    }
    [MetaClass("DecelToLocationMovement")]
    public class DecelToLocationMovement : AcceleratingMovement
    {
    }
    [MetaClass("FixedTimeMovement")]
    public class FixedTimeMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool? InferDirectionFromFacingIfNeeded { get; set; }
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool? ProjectTargetToCastRange { get; set; }
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool? UseGroundHeightAtTarget { get; set; }
        [MetaProperty("mTravelTime", BinPropertyType.Float)]
        public float? TravelTime { get; set; }
    }
    [MetaClass("FixedSpeedMovement")]
    public class FixedSpeedMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool? InferDirectionFromFacingIfNeeded { get; set; }
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool? ProjectTargetToCastRange { get; set; }
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool? UseGroundHeightAtTarget { get; set; }
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float? Speed { get; set; }
    }
    [MetaClass("PhysicsMovement")]
    public class PhysicsMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float? Lifetime { get; set; }
        [MetaProperty("mInitialSpeed", BinPropertyType.Float)]
        public float? InitialSpeed { get; set; }
        [MetaProperty("mDrag", BinPropertyType.Float)]
        public float? Drag { get; set; }
        [MetaProperty(2468250002, BinPropertyType.Bool)]
        public bool? m2468250002 { get; set; }
        [MetaProperty(3396802375, BinPropertyType.Float)]
        public float? m3396802375 { get; set; }
    }
    [MetaClass("TrackMouseMovement")]
    public class TrackMouseMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty(2226849642, BinPropertyType.Container)]
        public MetaContainer<float> m2226849642 { get; set; }
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool? InferDirectionFromFacingIfNeeded { get; set; }
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool? ProjectTargetToCastRange { get; set; }
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool? UseGroundHeightAtTarget { get; set; }
        [MetaProperty("mAcceleration", BinPropertyType.Float)]
        public float? Acceleration { get; set; }
        [MetaProperty("mMinSpeed", BinPropertyType.Float)]
        public float? MinSpeed { get; set; }
        [MetaProperty("mMaxSpeed", BinPropertyType.Float)]
        public float? MaxSpeed { get; set; }
        [MetaProperty("mInitialSpeed", BinPropertyType.Float)]
        public float? InitialSpeed { get; set; }
        [MetaProperty(1615432143, BinPropertyType.Float)]
        public float? m1615432143 { get; set; }
    }
    [MetaClass("GenericSplineMovementSpec")]
    public interface GenericSplineMovementSpec : MissileMovementSpec
    {
        [MetaProperty("mSplineInfo", BinPropertyType.Structure)]
        ISplineInfo SplineInfo { get; set; }
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        bool? UseMissilePositionAsOrigin { get; set; }
    }
    [MetaClass("FixedTimeSplineMovement")]
    public class FixedTimeSplineMovement : GenericSplineMovementSpec
    {
        [MetaProperty("mSplineInfo", BinPropertyType.Structure)]
        public ISplineInfo SplineInfo { get; set; }
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        public bool? UseMissilePositionAsOrigin { get; set; }
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mTravelTime", BinPropertyType.Float)]
        public float? TravelTime { get; set; }
    }
    [MetaClass("FixedSpeedSplineMovement")]
    public class FixedSpeedSplineMovement : GenericSplineMovementSpec
    {
        [MetaProperty("mSplineInfo", BinPropertyType.Structure)]
        public ISplineInfo SplineInfo { get; set; }
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        public bool? UseMissilePositionAsOrigin { get; set; }
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float? Speed { get; set; }
    }
    [MetaClass("WallFollowMovement")]
    public class WallFollowMovement : MissileMovementSpec
    {
        [MetaProperty("mUseHeightOffsetAtEnd", BinPropertyType.Bool)]
        public bool? UseHeightOffsetAtEnd { get; set; }
        [MetaProperty("mTracksTarget", BinPropertyType.Bool)]
        public bool? TracksTarget { get; set; }
        [MetaProperty(2856647070, BinPropertyType.Bool)]
        public bool? m2856647070 { get; set; }
        [MetaProperty("mTargetHeightAugment", BinPropertyType.Float)]
        public float? TargetHeightAugment { get; set; }
        [MetaProperty("mOffsetInitialTargetHeight", BinPropertyType.Float)]
        public float? OffsetInitialTargetHeight { get; set; }
        [MetaProperty("mStartBoneName", BinPropertyType.String)]
        public string? StartBoneName { get; set; }
        [MetaProperty(2798329764, BinPropertyType.Map)]
        public Dictionary<uint, string> m2798329764 { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.String)]
        public string? TargetBoneName { get; set; }
        [MetaProperty(2799230803, BinPropertyType.Float)]
        public float? m2799230803 { get; set; }
        [MetaProperty("mInferDirectionFromFacingIfNeeded", BinPropertyType.Bool)]
        public bool? InferDirectionFromFacingIfNeeded { get; set; }
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool? ProjectTargetToCastRange { get; set; }
        [MetaProperty("mUseGroundHeightAtTarget", BinPropertyType.Bool)]
        public bool? UseGroundHeightAtTarget { get; set; }
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float? Speed { get; set; }
        [MetaProperty("mCounterClockwise", BinPropertyType.Bool)]
        public bool? CounterClockwise { get; set; }
        [MetaProperty("mWallOffset", BinPropertyType.Float)]
        public float? WallOffset { get; set; }
        [MetaProperty("mWallLength", BinPropertyType.Float)]
        public float? WallLength { get; set; }
        [MetaProperty("mWallSearchRadius", BinPropertyType.Float)]
        public float? WallSearchRadius { get; set; }
        [MetaProperty(3170840289, BinPropertyType.Bool)]
        public bool? m3170840289 { get; set; }
    }
    [MetaClass("VerticalFacingType")]
    public interface VerticalFacingType : IMetaClass
    {
    }
    [MetaClass("VeritcalFacingMatchVelocity")]
    public class VeritcalFacingMatchVelocity : VerticalFacingType
    {
    }
    [MetaClass("VerticalFacingFaceTarget")]
    public class VerticalFacingFaceTarget : VerticalFacingType
    {
    }
    [MetaClass("MissileVisibilitySpec")]
    public interface MissileVisibilitySpec : IMetaClass
    {
        [MetaProperty("mTargetControlsVisibility", BinPropertyType.Bool)]
        bool? TargetControlsVisibility { get; set; }
        [MetaProperty("mVisibleToOwnerTeamOnly", BinPropertyType.Bool)]
        bool? VisibleToOwnerTeamOnly { get; set; }
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        float? PerceptionBubbleRadius { get; set; }
    }
    [MetaClass("Defaultvisibility")]
    public class Defaultvisibility : MissileVisibilitySpec
    {
        [MetaProperty("mTargetControlsVisibility", BinPropertyType.Bool)]
        public bool? TargetControlsVisibility { get; set; }
        [MetaProperty("mVisibleToOwnerTeamOnly", BinPropertyType.Bool)]
        public bool? VisibleToOwnerTeamOnly { get; set; }
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        public float? PerceptionBubbleRadius { get; set; }
    }
    [MetaClass("EnterFOWVisibility")]
    public class EnterFOWVisibility : MissileVisibilitySpec
    {
        [MetaProperty("mTargetControlsVisibility", BinPropertyType.Bool)]
        public bool? TargetControlsVisibility { get; set; }
        [MetaProperty("mVisibleToOwnerTeamOnly", BinPropertyType.Bool)]
        public bool? VisibleToOwnerTeamOnly { get; set; }
        [MetaProperty("mPerceptionBubbleRadius", BinPropertyType.Float)]
        public float? PerceptionBubbleRadius { get; set; }
        [MetaProperty("mMissileClientExitFOWPrediction", BinPropertyType.Bool)]
        public bool? MissileClientExitFOWPrediction { get; set; }
        [MetaProperty("mMissileClientWaitForTargetUpdateBeforeMissileShow", BinPropertyType.Bool)]
        public bool? MissileClientWaitForTargetUpdateBeforeMissileShow { get; set; }
    }
    [MetaClass("MissileAttachedTargetingDefinition")]
    public class MissileAttachedTargetingDefinition : IMetaClass
    {
        [MetaProperty("mEndPositionType", BinPropertyType.Byte)]
        public byte? EndPositionType { get; set; }
        [MetaProperty("mLineTextureName", BinPropertyType.String)]
        public string? LineTextureName { get; set; }
        [MetaProperty("mLineTextureWidth", BinPropertyType.Float)]
        public float? LineTextureWidth { get; set; }
        [MetaProperty("mLineEndTextureName", BinPropertyType.String)]
        public string? LineEndTextureName { get; set; }
        [MetaProperty("mLineEndTextureWidth", BinPropertyType.Float)]
        public float? LineEndTextureWidth { get; set; }
        [MetaProperty("mLineEndTextureHeight", BinPropertyType.Float)]
        public float? LineEndTextureHeight { get; set; }
    }
    [MetaClass("AISpellData")]
    public class AISpellData : IMetaClass
    {
        [MetaProperty("mSendAIEvent", BinPropertyType.Bool)]
        public bool? SendAIEvent { get; set; }
        [MetaProperty("mRadius", BinPropertyType.Float)]
        public float? Radius { get; set; }
        [MetaProperty("mLifetime", BinPropertyType.Float)]
        public float? Lifetime { get; set; }
        [MetaProperty("mRange", BinPropertyType.Float)]
        public float? Range { get; set; }
        [MetaProperty("mSpeed", BinPropertyType.Float)]
        public float? Speed { get; set; }
        [MetaProperty("mEndOnly", BinPropertyType.Bool)]
        public bool? EndOnly { get; set; }
        [MetaProperty("mBlockLevel", BinPropertyType.Byte)]
        public byte? BlockLevel { get; set; }
    }
    [MetaClass("SpellEffectAmount")]
    public class SpellEffectAmount : IMetaClass
    {
        [MetaProperty("value", BinPropertyType.Container)]
        public MetaContainer<float> Value { get; set; }
    }
    [MetaClass("SpellDataValue")]
    public class SpellDataValue : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mValues", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; }
    }
    [MetaClass("PlatformSpellInfo")]
    public class PlatformSpellInfo : IMetaClass
    {
        [MetaProperty("mAvatarLevelRequired", BinPropertyType.Int32)]
        public int? AvatarLevelRequired { get; set; }
        [MetaProperty("mSpellID", BinPropertyType.Int32)]
        public int? SpellID { get; set; }
        [MetaProperty("mPlatformEnabled", BinPropertyType.Bool)]
        public bool? PlatformEnabled { get; set; }
        [MetaProperty("mGameModes", BinPropertyType.Container)]
        public MetaContainer<string> GameModes { get; set; }
    }
    [MetaClass("ISplineInfo")]
    public interface ISplineInfo : IMetaClass
    {
        [MetaProperty("mStartPositionOffset", BinPropertyType.Vector3)]
        Vector3? StartPositionOffset { get; set; }
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        bool? UseMissilePositionAsOrigin { get; set; }
    }
    [MetaClass("HermiteSplineInfo")]
    public class HermiteSplineInfo : ISplineInfo
    {
        [MetaProperty("mStartPositionOffset", BinPropertyType.Vector3)]
        public Vector3? StartPositionOffset { get; set; }
        [MetaProperty("mUseMissilePositionAsOrigin", BinPropertyType.Bool)]
        public bool? UseMissilePositionAsOrigin { get; set; }
        [MetaProperty("mControlPoint1", BinPropertyType.Vector3)]
        public Vector3? ControlPoint1 { get; set; }
        [MetaProperty("mControlPoint2", BinPropertyType.Vector3)]
        public Vector3? ControlPoint2 { get; set; }
    }
    [MetaClass("OverrideAttackTimeData")]
    public class OverrideAttackTimeData : IMetaClass
    {
        [MetaProperty(546903361, BinPropertyType.Structure)]
        public IGameCalculation m546903361 { get; set; }
        [MetaProperty(2238351055, BinPropertyType.Float)]
        public float? m2238351055 { get; set; }
    }
    [MetaClass("UseAutoattackCastTimeData")]
    public class UseAutoattackCastTimeData : IMetaClass
    {
        [MetaProperty(1559208202, BinPropertyType.Structure)]
        public IGameCalculation m1559208202 { get; set; }
        [MetaProperty(2251275924, BinPropertyType.Bool)]
        public bool? m2251275924 { get; set; }
    }
    [MetaClass("SpellLockDeltaTimeData")]
    public class SpellLockDeltaTimeData : IMetaClass
    {
        [MetaProperty(3257162073, BinPropertyType.Structure)]
        public IGameCalculation m3257162073 { get; set; }
    }
    [MetaClass(609301268)]
    public class Class609301268 : IMetaClass
    {
        [MetaProperty("mAnimationName", BinPropertyType.String)]
        public string? AnimationName { get; set; }
        [MetaProperty("mAnimationLoopName", BinPropertyType.String)]
        public string? AnimationLoopName { get; set; }
        [MetaProperty("mAnimationWinddownName", BinPropertyType.String)]
        public string? AnimationWinddownName { get; set; }
        [MetaProperty("mAnimationLeadOutName", BinPropertyType.String)]
        public string? AnimationLeadOutName { get; set; }
        [MetaProperty(2883554725, BinPropertyType.Bool)]
        public bool? m2883554725 { get; set; }
        [MetaProperty("mHitEffectOrientType", BinPropertyType.UInt32)]
        public uint? HitEffectOrientType { get; set; }
        [MetaProperty(3054978970, BinPropertyType.Bool)]
        public bool? m3054978970 { get; set; }
        [MetaProperty(1645205029, BinPropertyType.Bool)]
        public bool? m1645205029 { get; set; }
        [MetaProperty("mHitBoneName", BinPropertyType.String)]
        public string? HitBoneName { get; set; }
        [MetaProperty("mHitEffectKey", BinPropertyType.Hash)]
        public MetaHash? HitEffectKey { get; set; }
        [MetaProperty("mHitEffectName", BinPropertyType.String)]
        public string? HitEffectName { get; set; }
        [MetaProperty("mHitEffectPlayerKey", BinPropertyType.Hash)]
        public MetaHash? HitEffectPlayerKey { get; set; }
        [MetaProperty("mHitEffectPlayerName", BinPropertyType.String)]
        public string? HitEffectPlayerName { get; set; }
        [MetaProperty("mAfterEffectKey", BinPropertyType.Hash)]
        public MetaHash? AfterEffectKey { get; set; }
        [MetaProperty("mAfterEffectName", BinPropertyType.String)]
        public string? AfterEffectName { get; set; }
    }
    [MetaClass("SpellDataResource")]
    public class SpellDataResource : IMetaClass
    {
        [MetaProperty("flags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAffectsTypeFlags", BinPropertyType.UInt32)]
        public uint? AffectsTypeFlags { get; set; }
        [MetaProperty("mAffectsStatusFlags", BinPropertyType.UInt32)]
        public uint? AffectsStatusFlags { get; set; }
        [MetaProperty("mRequiredUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> RequiredUnitTags { get; set; }
        [MetaProperty("mExcludedUnitTags", BinPropertyType.Embedded)]
        public MetaEmbedded<ObjectTags> ExcludedUnitTags { get; set; }
        [MetaProperty("mCastRequirementsCaster", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> CastRequirementsCaster { get; set; }
        [MetaProperty("mCastRequirementsTarget", BinPropertyType.Container)]
        public MetaContainer<ICastRequirement> CastRequirementsTarget { get; set; }
        [MetaProperty("mPlatformSpellInfo", BinPropertyType.Embedded)]
        public MetaEmbedded<PlatformSpellInfo> PlatformSpellInfo { get; set; }
        [MetaProperty("mAlternateName", BinPropertyType.String)]
        public string? AlternateName { get; set; }
        [MetaProperty("mSpellTags", BinPropertyType.Container)]
        public MetaContainer<string> SpellTags { get; set; }
        [MetaProperty("mEffectAmount", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellEffectAmount>> EffectAmount { get; set; }
        [MetaProperty("mDataValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SpellDataValue>> DataValues { get; set; }
        [MetaProperty(2488738436, BinPropertyType.Map)]
        public Dictionary<MetaHash, IGameCalculation> m2488738436 { get; set; }
        [MetaProperty("mCoefficient", BinPropertyType.Float)]
        public float? Coefficient { get; set; }
        [MetaProperty("mCoefficient2", BinPropertyType.Float)]
        public float? Coefficient2 { get; set; }
        [MetaProperty(3464567601, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class609301268>> m3464567601 { get; set; }
        [MetaProperty("mAnimationName", BinPropertyType.String)]
        public string? AnimationName { get; set; }
        [MetaProperty("mAnimationLoopName", BinPropertyType.String)]
        public string? AnimationLoopName { get; set; }
        [MetaProperty("mAnimationWinddownName", BinPropertyType.String)]
        public string? AnimationWinddownName { get; set; }
        [MetaProperty("mAnimationLeadOutName", BinPropertyType.String)]
        public string? AnimationLeadOutName { get; set; }
        [MetaProperty("mImgIconName", BinPropertyType.Container)]
        public MetaContainer<string> ImgIconName { get; set; }
        [MetaProperty("mMinimapIconName", BinPropertyType.String)]
        public string? MinimapIconName { get; set; }
        [MetaProperty("mKeywordWhenAcquired", BinPropertyType.String)]
        public string? KeywordWhenAcquired { get; set; }
        [MetaProperty("mCastTime", BinPropertyType.Float)]
        public float? CastTime { get; set; }
        [MetaProperty("mChannelDuration", BinPropertyType.Container)]
        public MetaContainer<float> ChannelDuration { get; set; }
        [MetaProperty("cooldownTime", BinPropertyType.Container)]
        public MetaContainer<float> CooldownTime { get; set; }
        [MetaProperty("delayCastOffsetPercent", BinPropertyType.Float)]
        public float? DelayCastOffsetPercent { get; set; }
        [MetaProperty("delayTotalTimePercent", BinPropertyType.Float)]
        public float? DelayTotalTimePercent { get; set; }
        [MetaProperty("mPreCastLockoutDeltaTime", BinPropertyType.Float)]
        public float? PreCastLockoutDeltaTime { get; set; }
        [MetaProperty(1870394526, BinPropertyType.Structure)]
        public SpellLockDeltaTimeData m1870394526 { get; set; }
        [MetaProperty("mPostCastLockoutDeltaTime", BinPropertyType.Float)]
        public float? PostCastLockoutDeltaTime { get; set; }
        [MetaProperty(1498177893, BinPropertyType.Structure)]
        public SpellLockDeltaTimeData m1498177893 { get; set; }
        [MetaProperty("mIsDelayedByCastLocked", BinPropertyType.Bool)]
        public bool? IsDelayedByCastLocked { get; set; }
        [MetaProperty("mStartCooldown", BinPropertyType.Float)]
        public float? StartCooldown { get; set; }
        [MetaProperty("mCastRangeGrowthMax", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeGrowthMax { get; set; }
        [MetaProperty("mCastRangeGrowthStartTime", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeGrowthStartTime { get; set; }
        [MetaProperty("mCastRangeGrowthDuration", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeGrowthDuration { get; set; }
        [MetaProperty("mChargeUpdateInterval", BinPropertyType.Float)]
        public float? ChargeUpdateInterval { get; set; }
        [MetaProperty("mCancelChargeOnRecastTime", BinPropertyType.Float)]
        public float? CancelChargeOnRecastTime { get; set; }
        [MetaProperty(1031040799, BinPropertyType.Byte)]
        public byte? m1031040799 { get; set; }
        [MetaProperty(372438780, BinPropertyType.Container)]
        public MetaContainer<SpellPassiveData> m372438780 { get; set; }
        [MetaProperty("mMaxAmmo", BinPropertyType.Container)]
        public MetaContainer<int> MaxAmmo { get; set; }
        [MetaProperty("mAmmoUsed", BinPropertyType.Container)]
        public MetaContainer<int> AmmoUsed { get; set; }
        [MetaProperty("mAmmoRechargeTime", BinPropertyType.Container)]
        public MetaContainer<float> AmmoRechargeTime { get; set; }
        [MetaProperty("mAmmoNotAffectedByCDR", BinPropertyType.Bool)]
        public bool? AmmoNotAffectedByCDR { get; set; }
        [MetaProperty("mCooldownNotAffectedByCDR", BinPropertyType.Bool)]
        public bool? CooldownNotAffectedByCDR { get; set; }
        [MetaProperty("mAmmoCountHiddenInUI", BinPropertyType.Bool)]
        public bool? AmmoCountHiddenInUI { get; set; }
        [MetaProperty("mCostAlwaysShownInUI", BinPropertyType.Bool)]
        public bool? CostAlwaysShownInUI { get; set; }
        [MetaProperty("cannotBeSuppressed", BinPropertyType.Bool)]
        public bool? CannotBeSuppressed { get; set; }
        [MetaProperty("canCastWhileDisabled", BinPropertyType.Bool)]
        public bool? CanCastWhileDisabled { get; set; }
        [MetaProperty("mCanTriggerChargeSpellWhileDisabled", BinPropertyType.Bool)]
        public bool? CanTriggerChargeSpellWhileDisabled { get; set; }
        [MetaProperty("canCastOrQueueWhileCasting", BinPropertyType.Bool)]
        public bool? CanCastOrQueueWhileCasting { get; set; }
        [MetaProperty("canOnlyCastWhileDisabled", BinPropertyType.Bool)]
        public bool? CanOnlyCastWhileDisabled { get; set; }
        [MetaProperty("mCantCancelWhileWindingUp", BinPropertyType.Bool)]
        public bool? CantCancelWhileWindingUp { get; set; }
        [MetaProperty(2117350048, BinPropertyType.Bool)]
        public bool? m2117350048 { get; set; }
        [MetaProperty("mCantCancelWhileChanneling", BinPropertyType.Bool)]
        public bool? CantCancelWhileChanneling { get; set; }
        [MetaProperty("cantCastWhileRooted", BinPropertyType.Bool)]
        public bool? CantCastWhileRooted { get; set; }
        [MetaProperty("mChannelIsInterruptedByDisables", BinPropertyType.Bool)]
        public bool? ChannelIsInterruptedByDisables { get; set; }
        [MetaProperty("mChannelIsInterruptedByAttacking", BinPropertyType.Bool)]
        public bool? ChannelIsInterruptedByAttacking { get; set; }
        [MetaProperty("mApplyAttackDamage", BinPropertyType.Bool)]
        public bool? ApplyAttackDamage { get; set; }
        [MetaProperty("mApplyAttackEffect", BinPropertyType.Bool)]
        public bool? ApplyAttackEffect { get; set; }
        [MetaProperty("mApplyMaterialOnHitSound", BinPropertyType.Bool)]
        public bool? ApplyMaterialOnHitSound { get; set; }
        [MetaProperty("mDoesntBreakChannels", BinPropertyType.Bool)]
        public bool? DoesntBreakChannels { get; set; }
        [MetaProperty("mBelongsToAvatar", BinPropertyType.Bool)]
        public bool? BelongsToAvatar { get; set; }
        [MetaProperty("mIsDisabledWhileDead", BinPropertyType.Bool)]
        public bool? IsDisabledWhileDead { get; set; }
        [MetaProperty("canOnlyCastWhileDead", BinPropertyType.Bool)]
        public bool? CanOnlyCastWhileDead { get; set; }
        [MetaProperty("mCursorChangesInGrass", BinPropertyType.Bool)]
        public bool? CursorChangesInGrass { get; set; }
        [MetaProperty("mCursorChangesInTerrain", BinPropertyType.Bool)]
        public bool? CursorChangesInTerrain { get; set; }
        [MetaProperty("mProjectTargetToCastRange", BinPropertyType.Bool)]
        public bool? ProjectTargetToCastRange { get; set; }
        [MetaProperty("mSpellRevealsChampion", BinPropertyType.Bool)]
        public bool? SpellRevealsChampion { get; set; }
        [MetaProperty("mUseMinimapTargeting", BinPropertyType.Bool)]
        public bool? UseMinimapTargeting { get; set; }
        [MetaProperty("castRangeUseBoundingBoxes", BinPropertyType.Bool)]
        public bool? CastRangeUseBoundingBoxes { get; set; }
        [MetaProperty("mMinimapIconRotation", BinPropertyType.Bool)]
        public bool? MinimapIconRotation { get; set; }
        [MetaProperty("mUseChargeChanneling", BinPropertyType.Bool)]
        public bool? UseChargeChanneling { get; set; }
        [MetaProperty("mCanMoveWhileChanneling", BinPropertyType.Bool)]
        public bool? CanMoveWhileChanneling { get; set; }
        [MetaProperty("mDisableCastBar", BinPropertyType.Bool)]
        public bool? DisableCastBar { get; set; }
        [MetaProperty("mShowChannelBar", BinPropertyType.Bool)]
        public bool? ShowChannelBar { get; set; }
        [MetaProperty("alwaysSnapFacing", BinPropertyType.Bool)]
        public bool? AlwaysSnapFacing { get; set; }
        [MetaProperty("useAnimatorFramerate", BinPropertyType.Bool)]
        public bool? UseAnimatorFramerate { get; set; }
        [MetaProperty("bHaveHitEffect", BinPropertyType.Bool)]
        public bool? BHaveHitEffect { get; set; }
        [MetaProperty("bIsToggleSpell", BinPropertyType.Bool)]
        public bool? BIsToggleSpell { get; set; }
        [MetaProperty("mDoNotNeedToFaceTarget", BinPropertyType.Bool)]
        public bool? DoNotNeedToFaceTarget { get; set; }
        [MetaProperty("mTurnSpeedScalar", BinPropertyType.Float)]
        public float? TurnSpeedScalar { get; set; }
        [MetaProperty("mNoWinddownIfCancelled", BinPropertyType.Bool)]
        public bool? NoWinddownIfCancelled { get; set; }
        [MetaProperty("mIgnoreRangeCheck", BinPropertyType.Bool)]
        public bool? IgnoreRangeCheck { get; set; }
        [MetaProperty("mOrientRadiusTextureFromPlayer", BinPropertyType.Bool)]
        public bool? OrientRadiusTextureFromPlayer { get; set; }
        [MetaProperty(3579459509, BinPropertyType.Structure)]
        public OverrideAttackTimeData m3579459509 { get; set; }
        [MetaProperty("mUseAutoattackCastTimeData", BinPropertyType.Structure)]
        public UseAutoattackCastTimeData UseAutoattackCastTimeData { get; set; }
        [MetaProperty("mIgnoreAnimContinueUntilCastFrame", BinPropertyType.Bool)]
        public bool? IgnoreAnimContinueUntilCastFrame { get; set; }
        [MetaProperty("mHideRangeIndicatorWhenCasting", BinPropertyType.Bool)]
        public bool? HideRangeIndicatorWhenCasting { get; set; }
        [MetaProperty("mUpdateRotationWhenCasting", BinPropertyType.Bool)]
        public bool? UpdateRotationWhenCasting { get; set; }
        [MetaProperty("mPingableWhileDisabled", BinPropertyType.Bool)]
        public bool? PingableWhileDisabled { get; set; }
        [MetaProperty("mConsideredAsAutoAttack", BinPropertyType.Bool)]
        public bool? ConsideredAsAutoAttack { get; set; }
        [MetaProperty("mDoesNotConsumeMana", BinPropertyType.Bool)]
        public bool? DoesNotConsumeMana { get; set; }
        [MetaProperty("mDoesNotConsumeCooldown", BinPropertyType.Bool)]
        public bool? DoesNotConsumeCooldown { get; set; }
        [MetaProperty("mLockedSpellOriginationCastID", BinPropertyType.Bool)]
        public bool? LockedSpellOriginationCastID { get; set; }
        [MetaProperty(2307898068, BinPropertyType.Bool)]
        public bool? m2307898068 { get; set; }
        [MetaProperty("mMinimapIconDisplayFlag", BinPropertyType.UInt16)]
        public ushort? MinimapIconDisplayFlag { get; set; }
        [MetaProperty("castRange", BinPropertyType.Container)]
        public MetaContainer<float> CastRange { get; set; }
        [MetaProperty("castRangeDisplayOverride", BinPropertyType.Container)]
        public MetaContainer<float> CastRangeDisplayOverride { get; set; }
        [MetaProperty("castRadius", BinPropertyType.Container)]
        public MetaContainer<float> CastRadius { get; set; }
        [MetaProperty("castRadiusSecondary", BinPropertyType.Container)]
        public MetaContainer<float> CastRadiusSecondary { get; set; }
        [MetaProperty("castConeAngle", BinPropertyType.Float)]
        public float? CastConeAngle { get; set; }
        [MetaProperty("castConeDistance", BinPropertyType.Float)]
        public float? CastConeDistance { get; set; }
        [MetaProperty("castTargetAdditionalUnitsRadius", BinPropertyType.Float)]
        public float? CastTargetAdditionalUnitsRadius { get; set; }
        [MetaProperty("luaOnMissileUpdateDistanceInterval", BinPropertyType.Float)]
        public float? LuaOnMissileUpdateDistanceInterval { get; set; }
        [MetaProperty("mMissileSpec", BinPropertyType.Structure)]
        public MissileSpecification MissileSpec { get; set; }
        [MetaProperty("mCastType", BinPropertyType.UInt32)]
        public uint? CastType { get; set; }
        [MetaProperty("castFrame", BinPropertyType.Float)]
        public float? CastFrame { get; set; }
        [MetaProperty("missileSpeed", BinPropertyType.Float)]
        public float? MissileSpeed { get; set; }
        [MetaProperty("mMissileEffectKey", BinPropertyType.Hash)]
        public MetaHash? MissileEffectKey { get; set; }
        [MetaProperty("mMissileEffectName", BinPropertyType.String)]
        public string? MissileEffectName { get; set; }
        [MetaProperty("mMissileEffectPlayerKey", BinPropertyType.Hash)]
        public MetaHash? MissileEffectPlayerKey { get; set; }
        [MetaProperty("mMissileEffectPlayerName", BinPropertyType.String)]
        public string? MissileEffectPlayerName { get; set; }
        [MetaProperty("mMissileEffectEnemyKey", BinPropertyType.Hash)]
        public MetaHash? MissileEffectEnemyKey { get; set; }
        [MetaProperty("mMissileEffectEnemyName", BinPropertyType.String)]
        public string? MissileEffectEnemyName { get; set; }
        [MetaProperty("mLineWidth", BinPropertyType.Float)]
        public float? LineWidth { get; set; }
        [MetaProperty("mLineDragLength", BinPropertyType.Float)]
        public float? LineDragLength { get; set; }
        [MetaProperty("mLookAtPolicy", BinPropertyType.UInt32)]
        public uint? LookAtPolicy { get; set; }
        [MetaProperty("mHitEffectOrientType", BinPropertyType.UInt32)]
        public uint? HitEffectOrientType { get; set; }
        [MetaProperty(2460302967, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m2460302967 { get; set; }
        [MetaProperty("mHitEffectKey", BinPropertyType.Hash)]
        public MetaHash? HitEffectKey { get; set; }
        [MetaProperty("mHitEffectName", BinPropertyType.String)]
        public string? HitEffectName { get; set; }
        [MetaProperty("mHitEffectPlayerKey", BinPropertyType.Hash)]
        public MetaHash? HitEffectPlayerKey { get; set; }
        [MetaProperty("mHitEffectPlayerName", BinPropertyType.String)]
        public string? HitEffectPlayerName { get; set; }
        [MetaProperty("mAfterEffectKey", BinPropertyType.Hash)]
        public MetaHash? AfterEffectKey { get; set; }
        [MetaProperty("mAfterEffectName", BinPropertyType.String)]
        public string? AfterEffectName { get; set; }
        [MetaProperty("bHaveHitBone", BinPropertyType.Bool)]
        public bool? BHaveHitBone { get; set; }
        [MetaProperty("mHitBoneName", BinPropertyType.String)]
        public string? HitBoneName { get; set; }
        [MetaProperty("mParticleStartOffset", BinPropertyType.Vector3)]
        public Vector3? ParticleStartOffset { get; set; }
        [MetaProperty("mFloatVarsDecimals", BinPropertyType.Container)]
        public MetaContainer<int> FloatVarsDecimals { get; set; }
        [MetaProperty("mana", BinPropertyType.Container)]
        public MetaContainer<float> Mana { get; set; }
        [MetaProperty("manaUiOverride", BinPropertyType.Container)]
        public MetaContainer<float> ManaUiOverride { get; set; }
        [MetaProperty("selectionPriority", BinPropertyType.UInt32)]
        public uint? SelectionPriority { get; set; }
        [MetaProperty("mTargetingTypeData", BinPropertyType.Structure)]
        public TargetingTypeData TargetingTypeData { get; set; }
        [MetaProperty("mVOEventCategory", BinPropertyType.String)]
        public string? VOEventCategory { get; set; }
        [MetaProperty("mAIData", BinPropertyType.Embedded)]
        public MetaEmbedded<AISpellData> AIData { get; set; }
        [MetaProperty("mSpellCooldownOrSealedQueueThreshold", BinPropertyType.Optional)]
        public MetaOptional<float> SpellCooldownOrSealedQueueThreshold { get; set; }
        [MetaProperty(66335398, BinPropertyType.Byte)]
        public byte? m66335398 { get; set; }
        [MetaProperty(16246204, BinPropertyType.Bool)]
        public bool? m16246204 { get; set; }
        [MetaProperty(959977248, BinPropertyType.Bool)]
        public bool? m959977248 { get; set; }
        [MetaProperty(2833975761, BinPropertyType.Bool)]
        public bool? m2833975761 { get; set; }
        [MetaProperty(615998402, BinPropertyType.Bool)]
        public bool? m615998402 { get; set; }
        [MetaProperty("mClientData", BinPropertyType.Embedded)]
        public MetaEmbedded<SpellDataResourceClient> ClientData { get; set; }
    }
    [MetaClass("SpellPassiveData")]
    public class SpellPassiveData : IMetaClass
    {
        [MetaProperty("mBuff", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Buff { get; set; }
        [MetaProperty(2257773130, BinPropertyType.UInt32)]
        public uint? m2257773130 { get; set; }
        [MetaProperty(1991670732, BinPropertyType.Bool)]
        public bool? m1991670732 { get; set; }
        [MetaProperty(3420404466, BinPropertyType.Bool)]
        public bool? m3420404466 { get; set; }
        [MetaProperty(933770753, BinPropertyType.Byte)]
        public byte? m933770753 { get; set; }
    }
    [MetaClass("CustomTargeterDefinitions")]
    public class CustomTargeterDefinitions : IMetaClass
    {
        [MetaProperty("mTargeterDefinitions", BinPropertyType.Container)]
        public MetaContainer<TargeterDefinition> TargeterDefinitions { get; set; }
    }
    [MetaClass("SpellDataResourceClient")]
    public class SpellDataResourceClient : IMetaClass
    {
        [MetaProperty(928405213, BinPropertyType.Hash)]
        public MetaHash? m928405213 { get; set; }
        [MetaProperty(2102005358, BinPropertyType.Hash)]
        public MetaHash? m2102005358 { get; set; }
        [MetaProperty("mTooltipData", BinPropertyType.Structure)]
        public TooltipInstanceSpell TooltipData { get; set; }
        [MetaProperty("mSpawningUIDefinition", BinPropertyType.Structure)]
        public SpawningUIDefinition SpawningUIDefinition { get; set; }
        [MetaProperty("mTargeterDefinitions", BinPropertyType.Container)]
        public MetaContainer<TargeterDefinition> TargeterDefinitions { get; set; }
        [MetaProperty("mCustomTargeterDefinitions", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<CustomTargeterDefinitions>> CustomTargeterDefinitions { get; set; }
        [MetaProperty("mMissileTargeterDefinitions", BinPropertyType.Container)]
        public MetaContainer<MissileAttachedTargetingDefinition> MissileTargeterDefinitions { get; set; }
        [MetaProperty("mLeftClickSpellAction", BinPropertyType.UInt32)]
        public uint? LeftClickSpellAction { get; set; }
        [MetaProperty("mRightClickSpellAction", BinPropertyType.UInt32)]
        public uint? RightClickSpellAction { get; set; }
    }
    [MetaClass("SpawningUIDefinition")]
    public class SpawningUIDefinition : IMetaClass
    {
        [MetaProperty("buffNameFilter", BinPropertyType.String)]
        public string? BuffNameFilter { get; set; }
        [MetaProperty("maxNumberOfUnits", BinPropertyType.Int32)]
        public int? MaxNumberOfUnits { get; set; }
    }
    [MetaClass("LolSpellScript")]
    public class LolSpellScript : RScript
    {
        [MetaProperty("globalProperties", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptGlobalProperties> GlobalProperties { get; set; }
        [MetaProperty("sequences", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<ScriptSequence>> Sequences { get; set; }
        [MetaProperty("CustomSequences", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<ScriptSequence>> CustomSequences { get; set; }
        [MetaProperty("PreloadData", BinPropertyType.Embedded)]
        public MetaEmbedded<LoLSpellPreloadData> PreloadData { get; set; }
    }
    [MetaClass("IScriptPreload")]
    public interface IScriptPreload : IMetaClass
    {
    }
    [MetaClass("ScriptPreloadCharacter")]
    public class ScriptPreloadCharacter : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string? PreloadResourceName { get; set; }
    }
    [MetaClass("ScriptPreloadSpell")]
    public class ScriptPreloadSpell : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string? PreloadResourceName { get; set; }
    }
    [MetaClass("ScriptPreloadModule")]
    public class ScriptPreloadModule : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string? PreloadResourceName { get; set; }
    }
    [MetaClass("ScriptPreloadParticle")]
    public class ScriptPreloadParticle : IMetaClass
    {
        [MetaProperty("PreloadResourceName", BinPropertyType.String)]
        public string? PreloadResourceName { get; set; }
    }
    [MetaClass("LoLSpellPreloadData")]
    public class LoLSpellPreloadData : IMetaClass
    {
        [MetaProperty("CharacterPreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadCharacter> CharacterPreloads { get; set; }
        [MetaProperty("SpellPreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadSpell> SpellPreloads { get; set; }
        [MetaProperty("ModulePreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadModule> ModulePreloads { get; set; }
        [MetaProperty("ParticlePreloads", BinPropertyType.Container)]
        public MetaContainer<ScriptPreloadParticle> ParticlePreloads { get; set; }
    }
    [MetaClass("ScriptGlobalProperties")]
    public class ScriptGlobalProperties : IMetaClass
    {
        [MetaProperty("PersistsThroughDeath", BinPropertyType.Bool)]
        public bool? PersistsThroughDeath { get; set; }
        [MetaProperty("NonDispellable", BinPropertyType.Bool)]
        public bool? NonDispellable { get; set; }
        [MetaProperty("OnPreDamagePriority", BinPropertyType.Int32)]
        public int? OnPreDamagePriority { get; set; }
        [MetaProperty("DeathEventType", BinPropertyType.UInt32)]
        public uint? DeathEventType { get; set; }
        [MetaProperty("CastTime", BinPropertyType.Float)]
        public float? CastTime { get; set; }
        [MetaProperty("ChannelDuration", BinPropertyType.Float)]
        public float? ChannelDuration { get; set; }
        [MetaProperty("buffName", BinPropertyType.String)]
        public string? BuffName { get; set; }
        [MetaProperty("buffTextureName", BinPropertyType.String)]
        public string? BuffTextureName { get; set; }
        [MetaProperty("displayName", BinPropertyType.String)]
        public string? DisplayName { get; set; }
        [MetaProperty("AutoBuffActivateEffects", BinPropertyType.Container)]
        public MetaContainer<string> AutoBuffActivateEffects { get; set; }
        [MetaProperty("AutoBuffActivateAttachBoneNames", BinPropertyType.Container)]
        public MetaContainer<string> AutoBuffActivateAttachBoneNames { get; set; }
        [MetaProperty("IsDeathRecapSource", BinPropertyType.Bool)]
        public bool? IsDeathRecapSource { get; set; }
        [MetaProperty("SpellToggleSlot", BinPropertyType.UInt32)]
        public uint? SpellToggleSlot { get; set; }
        [MetaProperty("IsItemToggled", BinPropertyType.Bool)]
        public bool? IsItemToggled { get; set; }
        [MetaProperty("SpellFXOverrideSkins", BinPropertyType.Container)]
        public MetaContainer<string> SpellFXOverrideSkins { get; set; }
        [MetaProperty("SpellVOOverrideSkins", BinPropertyType.Container)]
        public MetaContainer<string> SpellVOOverrideSkins { get; set; }
        [MetaProperty("PopupMessages", BinPropertyType.Container)]
        public MetaContainer<string> PopupMessages { get; set; }
    }
    [MetaClass("InstanceVarsTable")]
    public class InstanceVarsTable : ScriptTable
    {
    }
    [MetaClass("CharacterVarsTable")]
    public class CharacterVarsTable : ScriptTable
    {
    }
    [MetaClass("NextBuffVarsTable")]
    public class NextBuffVarsTable : ScriptTable
    {
    }
    [MetaClass("WorldVarsTable")]
    public class WorldVarsTable : ScriptTable
    {
    }
    [MetaClass("AvatarVarsTable")]
    public class AvatarVarsTable : ScriptTable
    {
    }
    [MetaClass("NextSpellVarsTable")]
    public class NextSpellVarsTable : ScriptTable
    {
    }
    [MetaClass("TempTable1Table")]
    public class TempTable1Table : ScriptTable
    {
    }
    [MetaClass("TempTable2Table")]
    public class TempTable2Table : ScriptTable
    {
    }
    [MetaClass("TempTable3Table")]
    public class TempTable3Table : ScriptTable
    {
    }
    [MetaClass("ILineIndicatorType")]
    public interface ILineIndicatorType : IMetaClass
    {
    }
    [MetaClass("IndicatorTypeLocal")]
    public class IndicatorTypeLocal : ILineIndicatorType
    {
    }
    [MetaClass("IndicatorTypeGlobal")]
    public class IndicatorTypeGlobal : ILineIndicatorType
    {
        [MetaProperty(3804467094, BinPropertyType.Bool)]
        public bool? m3804467094 { get; set; }
    }
    [MetaClass("ITargeterFadeBehavior")]
    public interface ITargeterFadeBehavior : IMetaClass
    {
    }
    [MetaClass("FadeOverTimeBehavior")]
    public class FadeOverTimeBehavior : ITargeterFadeBehavior
    {
        [MetaProperty("mTimeStart", BinPropertyType.Float)]
        public float? TimeStart { get; set; }
        [MetaProperty("mTimeEnd", BinPropertyType.Float)]
        public float? TimeEnd { get; set; }
        [MetaProperty("mStartAlpha", BinPropertyType.Float)]
        public float? StartAlpha { get; set; }
        [MetaProperty("mEndAlpha", BinPropertyType.Float)]
        public float? EndAlpha { get; set; }
    }
    [MetaClass("FadeByMouseRangeBehavior")]
    public class FadeByMouseRangeBehavior : ITargeterFadeBehavior
    {
        [MetaProperty(1990666961, BinPropertyType.Float)]
        public float? m1990666961 { get; set; }
        [MetaProperty(1696085056, BinPropertyType.Float)]
        public float? m1696085056 { get; set; }
        [MetaProperty("mStartAlpha", BinPropertyType.Float)]
        public float? StartAlpha { get; set; }
        [MetaProperty("mEndAlpha", BinPropertyType.Float)]
        public float? EndAlpha { get; set; }
    }
    [MetaClass("FadeToExplicitValueBehavior")]
    public class FadeToExplicitValueBehavior : ITargeterFadeBehavior
    {
        [MetaProperty("mAlpha", BinPropertyType.Float)]
        public float? Alpha { get; set; }
    }
    [MetaClass("FloatPerSpellLevel")]
    public class FloatPerSpellLevel : IMetaClass
    {
        [MetaProperty("mPerLevelValues", BinPropertyType.Container)]
        public MetaContainer<float> PerLevelValues { get; set; }
        [MetaProperty("mValueType", BinPropertyType.UInt32)]
        public uint? ValueType { get; set; }
    }
    [MetaClass("DrawablePositionLocator")]
    public class DrawablePositionLocator : IMetaClass
    {
        [MetaProperty("basePosition", BinPropertyType.UInt32)]
        public uint? BasePosition { get; set; }
        [MetaProperty("distanceOffset", BinPropertyType.Float)]
        public float? DistanceOffset { get; set; }
        [MetaProperty("angleOffsetRadian", BinPropertyType.Float)]
        public float? AngleOffsetRadian { get; set; }
        [MetaProperty("orientationType", BinPropertyType.UInt32)]
        public uint? OrientationType { get; set; }
    }
    [MetaClass("TargeterDefinition")]
    public interface TargeterDefinition : IMetaClass
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        ITargeterFadeBehavior m3384398744 { get; set; }
    }
    [MetaClass("TargeterDefinitionAoe")]
    public class TargeterDefinitionAoe : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; }
        [MetaProperty("textureOrientation", BinPropertyType.UInt32)]
        public uint? TextureOrientation { get; set; }
        [MetaProperty("isConstrainedToRange", BinPropertyType.Bool)]
        public bool? IsConstrainedToRange { get; set; }
        [MetaProperty("constraintPosLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> ConstraintPosLocator { get; set; }
        [MetaProperty("constraintRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> ConstraintRange { get; set; }
        [MetaProperty("overrideRadius", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideRadius { get; set; }
        [MetaProperty("textureRadiusOverrideName", BinPropertyType.String)]
        public string? TextureRadiusOverrideName { get; set; }
    }
    [MetaClass("TargeterDefinitionArc")]
    public class TargeterDefinitionArc : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; }
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; }
        [MetaProperty("isClockwiseArc", BinPropertyType.Bool)]
        public bool? IsClockwiseArc { get; set; }
        [MetaProperty("isConstrainedToRange", BinPropertyType.Bool)]
        public bool? IsConstrainedToRange { get; set; }
        [MetaProperty("constraintRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> ConstraintRange { get; set; }
        [MetaProperty("overrideRadius", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideRadius { get; set; }
        [MetaProperty("textureArcOverrideName", BinPropertyType.String)]
        public string? TextureArcOverrideName { get; set; }
        [MetaProperty("thicknessOffset", BinPropertyType.Float)]
        public float? ThicknessOffset { get; set; }
    }
    [MetaClass("TargeterDefinitionCone")]
    public class TargeterDefinitionCone : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; }
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; }
        [MetaProperty("fallbackDirection", BinPropertyType.UInt32)]
        public uint? FallbackDirection { get; set; }
        [MetaProperty("hasMaxGrowRange", BinPropertyType.Bool)]
        public bool? HasMaxGrowRange { get; set; }
        [MetaProperty("coneFollowsEnd", BinPropertyType.Bool)]
        public bool? ConeFollowsEnd { get; set; }
        [MetaProperty("coneAngleDegrees", BinPropertyType.Optional)]
        public MetaOptional<float> ConeAngleDegrees { get; set; }
        [MetaProperty("coneRange", BinPropertyType.Optional)]
        public MetaOptional<float> ConeRange { get; set; }
        [MetaProperty("rangeGrowthStartTime", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthStartTime { get; set; }
        [MetaProperty("rangeGrowthDuration", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthDuration { get; set; }
        [MetaProperty("rangeGrowthMax", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthMax { get; set; }
        [MetaProperty("textureConeOverrideName", BinPropertyType.String)]
        public string? TextureConeOverrideName { get; set; }
        [MetaProperty("textureConeMaxGrowName", BinPropertyType.String)]
        public string? TextureConeMaxGrowName { get; set; }
    }
    [MetaClass("TargeterDefinitionLine")]
    public class TargeterDefinitionLine : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty(1086768030, BinPropertyType.Structure)]
        public ILineIndicatorType m1086768030 { get; set; }
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; }
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; }
        [MetaProperty("fallbackDirection", BinPropertyType.UInt32)]
        public uint? FallbackDirection { get; set; }
        [MetaProperty("alwaysDraw", BinPropertyType.Bool)]
        public bool? AlwaysDraw { get; set; }
        [MetaProperty("hasMaxGrowRange", BinPropertyType.Bool)]
        public bool? HasMaxGrowRange { get; set; }
        [MetaProperty("useGlobalLineIndicator", BinPropertyType.Bool)]
        public bool? UseGlobalLineIndicator { get; set; }
        [MetaProperty("lineStopsAtEndPosition", BinPropertyType.Optional)]
        public MetaOptional<bool> LineStopsAtEndPosition { get; set; }
        [MetaProperty("minimumDisplayedRange", BinPropertyType.Float)]
        public float? MinimumDisplayedRange { get; set; }
        [MetaProperty("arrowSize", BinPropertyType.Float)]
        public float? ArrowSize { get; set; }
        [MetaProperty("lineWidth", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> LineWidth { get; set; }
        [MetaProperty("overrideBaseRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideBaseRange { get; set; }
        [MetaProperty("rangeGrowthStartTime", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthStartTime { get; set; }
        [MetaProperty("rangeGrowthDuration", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthDuration { get; set; }
        [MetaProperty("rangeGrowthMax", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthMax { get; set; }
        [MetaProperty("textureBaseOverrideName", BinPropertyType.String)]
        public string? TextureBaseOverrideName { get; set; }
        [MetaProperty("textureTargetOverrideName", BinPropertyType.String)]
        public string? TextureTargetOverrideName { get; set; }
        [MetaProperty("textureBaseMaxGrowName", BinPropertyType.String)]
        public string? TextureBaseMaxGrowName { get; set; }
        [MetaProperty("textureTargetMaxGrowName", BinPropertyType.String)]
        public string? TextureTargetMaxGrowName { get; set; }
        [MetaProperty("mAngleLineToEndpointHeight", BinPropertyType.Bool)]
        public bool? AngleLineToEndpointHeight { get; set; }
        [MetaProperty("mCenterArrowToEndPoint", BinPropertyType.Bool)]
        public bool? CenterArrowToEndPoint { get; set; }
        [MetaProperty("facingLine", BinPropertyType.Bool)]
        public bool? FacingLine { get; set; }
        [MetaProperty("minAngle", BinPropertyType.Float)]
        public float? MinAngle { get; set; }
        [MetaProperty("maxAngle", BinPropertyType.Float)]
        public float? MaxAngle { get; set; }
        [MetaProperty("minAngleRangeFactor", BinPropertyType.Float)]
        public float? MinAngleRangeFactor { get; set; }
        [MetaProperty("maxAngleRangeFactor", BinPropertyType.Float)]
        public float? MaxAngleRangeFactor { get; set; }
        [MetaProperty("fade", BinPropertyType.Bool)]
        public bool? Fade { get; set; }
        [MetaProperty("fadeAngle", BinPropertyType.Float)]
        public float? FadeAngle { get; set; }
    }
    [MetaClass("TargeterDefinitionMinimap")]
    public class TargeterDefinitionMinimap : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; }
        [MetaProperty("useCasterBoundingBox", BinPropertyType.Optional)]
        public MetaOptional<bool> UseCasterBoundingBox { get; set; }
        [MetaProperty("overrideBaseRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideBaseRange { get; set; }
    }
    [MetaClass("TargeterDefinitionRange")]
    public class TargeterDefinitionRange : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; }
        [MetaProperty("textureOrientation", BinPropertyType.UInt32)]
        public uint? TextureOrientation { get; set; }
        [MetaProperty("hideWithLineIndicator", BinPropertyType.Bool)]
        public bool? HideWithLineIndicator { get; set; }
        [MetaProperty("hasMaxGrowRange", BinPropertyType.Bool)]
        public bool? HasMaxGrowRange { get; set; }
        [MetaProperty("useCasterBoundingBox", BinPropertyType.Optional)]
        public MetaOptional<bool> UseCasterBoundingBox { get; set; }
        [MetaProperty("overrideBaseRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideBaseRange { get; set; }
        [MetaProperty("rangeGrowthStartTime", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthStartTime { get; set; }
        [MetaProperty("rangeGrowthDuration", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthDuration { get; set; }
        [MetaProperty("rangeGrowthMax", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> RangeGrowthMax { get; set; }
        [MetaProperty("textureOverrideName", BinPropertyType.String)]
        public string? TextureOverrideName { get; set; }
        [MetaProperty("textureMaxGrowName", BinPropertyType.String)]
        public string? TextureMaxGrowName { get; set; }
    }
    [MetaClass("TargeterDefinitionWall")]
    public class TargeterDefinitionWall : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; }
        [MetaProperty("wallOrientation", BinPropertyType.UInt32)]
        public uint? WallOrientation { get; set; }
        [MetaProperty("wallRotation", BinPropertyType.Float)]
        public float? WallRotation { get; set; }
        [MetaProperty("thickness", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> Thickness { get; set; }
        [MetaProperty("length", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> Length { get; set; }
        [MetaProperty("textureWallOverrideName", BinPropertyType.String)]
        public string? TextureWallOverrideName { get; set; }
    }
    [MetaClass("TargeterDefinitionMultiAOE")]
    public class TargeterDefinitionMultiAOE : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("centerLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> CenterLocator { get; set; }
        [MetaProperty("overrideAOERadius", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideAOERadius { get; set; }
        [MetaProperty("overrideMinCastRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideMinCastRange { get; set; }
        [MetaProperty("overrideMaxCastRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> OverrideMaxCastRange { get; set; }
        [MetaProperty("angelOffsetRadian", BinPropertyType.Float)]
        public float? AngelOffsetRadian { get; set; }
        [MetaProperty("numOfInnerAOE", BinPropertyType.UInt32)]
        public uint? NumOfInnerAOE { get; set; }
        [MetaProperty("leftTextureName", BinPropertyType.String)]
        public string? LeftTextureName { get; set; }
        [MetaProperty("rightTextureName", BinPropertyType.String)]
        public string? RightTextureName { get; set; }
        [MetaProperty("innerTextureName", BinPropertyType.String)]
        public string? InnerTextureName { get; set; }
    }
    [MetaClass("TargeterDefinitionSpline")]
    public class TargeterDefinitionSpline : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("startLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; }
        [MetaProperty("endLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; }
        [MetaProperty("baseTextureName", BinPropertyType.String)]
        public string? BaseTextureName { get; set; }
        [MetaProperty("frontTextureName", BinPropertyType.String)]
        public string? FrontTextureName { get; set; }
        [MetaProperty("splineWidth", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> SplineWidth { get; set; }
        [MetaProperty("overrideSpline", BinPropertyType.Structure)]
        public ISplineInfo OverrideSpline { get; set; }
        [MetaProperty("minSegmentCount", BinPropertyType.UInt32)]
        public uint? MinSegmentCount { get; set; }
        [MetaProperty("maxSegmentLength", BinPropertyType.Float)]
        public float? MaxSegmentLength { get; set; }
        [MetaProperty("isConstrainedToRange", BinPropertyType.Bool)]
        public bool? IsConstrainedToRange { get; set; }
        [MetaProperty("constraintRange", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatPerSpellLevel> ConstraintRange { get; set; }
    }
    [MetaClass("TargeterDefinitionSkipTerrain")]
    public class TargeterDefinitionSkipTerrain : TargeterDefinition
    {
        [MetaProperty(3384398744, BinPropertyType.Structure)]
        public ITargeterFadeBehavior m3384398744 { get; set; }
        [MetaProperty("mStartLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> StartLocator { get; set; }
        [MetaProperty("mEndLocator", BinPropertyType.Embedded)]
        public MetaEmbedded<DrawablePositionLocator> EndLocator { get; set; }
        [MetaProperty("mBaseTextureName", BinPropertyType.String)]
        public string? BaseTextureName { get; set; }
        [MetaProperty("mTerrainTextureName", BinPropertyType.String)]
        public string? TerrainTextureName { get; set; }
        [MetaProperty("mTargetTextureName", BinPropertyType.String)]
        public string? TargetTextureName { get; set; }
        [MetaProperty("mTargetTextureRadius", BinPropertyType.Float)]
        public float? TargetTextureRadius { get; set; }
        [MetaProperty("mFallbackDirection", BinPropertyType.UInt32)]
        public uint? FallbackDirection { get; set; }
    }
    [MetaClass("TargetingTypeData")]
    public interface TargetingTypeData : IMetaClass
    {
    }
    [MetaClass("Self")]
    public class Self : TargetingTypeData
    {
    }
    [MetaClass("Target")]
    public class Target : TargetingTypeData
    {
        [MetaProperty(1871894195, BinPropertyType.Bool)]
        public bool? m1871894195 { get; set; }
    }
    [MetaClass("Area")]
    public class Area : TargetingTypeData
    {
    }
    [MetaClass("Cone")]
    public class Cone : TargetingTypeData
    {
    }
    [MetaClass("SelfAoe")]
    public class SelfAoe : TargetingTypeData
    {
    }
    [MetaClass("TargetOrLocation")]
    public class TargetOrLocation : TargetingTypeData
    {
    }
    [MetaClass("Location")]
    public class Location : TargetingTypeData
    {
    }
    [MetaClass("Direction")]
    public class Direction : TargetingTypeData
    {
    }
    [MetaClass("DragDirection")]
    public class DragDirection : TargetingTypeData
    {
    }
    [MetaClass("LineTargetToCaster")]
    public class LineTargetToCaster : TargetingTypeData
    {
    }
    [MetaClass("AreaClamped")]
    public class AreaClamped : TargetingTypeData
    {
    }
    [MetaClass("LocationClamped")]
    public class LocationClamped : TargetingTypeData
    {
    }
    [MetaClass("TerrainLocation")]
    public class TerrainLocation : TargetingTypeData
    {
    }
    [MetaClass("TerrainType")]
    public class TerrainType : TargetingTypeData
    {
        [MetaProperty("mBrushCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorData> BrushCursor { get; set; }
        [MetaProperty("mRiverCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorData> RiverCursor { get; set; }
        [MetaProperty("mWallCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorData> WallCursor { get; set; }
    }
    [MetaClass("TftSurrenderCheat")]
    public class TftSurrenderCheat : Cheat
    {
    }
    [MetaClass(3415378836)]
    public class Class3415378836 : Cheat
    {
    }
    [MetaClass("TFTItemMaterialController")]
    public class TFTItemMaterialController : SkinnedMeshDataMaterialController
    {
    }
    [MetaClass("TFTAnnouncementData")]
    public class TFTAnnouncementData : IMetaClass
    {
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string? IconPath { get; set; }
        [MetaProperty(3280235327, BinPropertyType.String)]
        public string? m3280235327 { get; set; }
        [MetaProperty("mDuration", BinPropertyType.Float)]
        public float? Duration { get; set; }
        [MetaProperty("mDelay", BinPropertyType.Float)]
        public float? Delay { get; set; }
    }
    [MetaClass("TFTAttachmentSlotStyleData")]
    public class TFTAttachmentSlotStyleData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(1689854402, BinPropertyType.String)]
        public string? m1689854402 { get; set; }
        [MetaProperty(2049301906, BinPropertyType.String)]
        public string? m2049301906 { get; set; }
    }
    [MetaClass("TFTCharacterRecord")]
    public class TFTCharacterRecord : CharacterRecord
    {
        [MetaProperty("PortraitIcon", BinPropertyType.String)]
        public string? PortraitIcon { get; set; }
        [MetaProperty("tier", BinPropertyType.Byte)]
        public byte? Tier { get; set; }
        [MetaProperty("mLinkedTraits", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTTraitContributionData>> LinkedTraits { get; set; }
        [MetaProperty("mMoveInterval", BinPropertyType.Float)]
        public float? MoveInterval { get; set; }
        [MetaProperty("mMoveProximity", BinPropertyType.Float)]
        public float? MoveProximity { get; set; }
        [MetaProperty("mMoveRange", BinPropertyType.Float)]
        public float? MoveRange { get; set; }
        [MetaProperty("mMoveHeight", BinPropertyType.Float)]
        public float? MoveHeight { get; set; }
        [MetaProperty("mInitialMana", BinPropertyType.Float)]
        public float? InitialMana { get; set; }
        [MetaProperty("mShopData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ShopData { get; set; }
        [MetaProperty(1311286886, BinPropertyType.String)]
        public string? m1311286886 { get; set; }
        [MetaProperty(3645083651, BinPropertyType.Vector3)]
        public Vector3? m3645083651 { get; set; }
        [MetaProperty(1246904587, BinPropertyType.Bool)]
        public bool? m1246904587 { get; set; }
        [MetaProperty(4015458703, BinPropertyType.Bool)]
        public bool? m4015458703 { get; set; }
        [MetaProperty(3144797065, BinPropertyType.Bool)]
        public bool? m3144797065 { get; set; }
    }
    [MetaClass("TFTDragData")]
    public class TFTDragData : IMetaClass
    {
        [MetaProperty(3857069222, BinPropertyType.Bool)]
        public bool? m3857069222 { get; set; }
        [MetaProperty(3420603526, BinPropertyType.Bool)]
        public bool? m3420603526 { get; set; }
        [MetaProperty(1152070302, BinPropertyType.Bool)]
        public bool? m1152070302 { get; set; }
        [MetaProperty(3509153429, BinPropertyType.Float)]
        public float? m3509153429 { get; set; }
        [MetaProperty(1838159659, BinPropertyType.Float)]
        public float? m1838159659 { get; set; }
        [MetaProperty(1494391998, BinPropertyType.Float)]
        public float? m1494391998 { get; set; }
        [MetaProperty(3797316178, BinPropertyType.Float)]
        public float? m3797316178 { get; set; }
        [MetaProperty(3110014664, BinPropertyType.Float)]
        public float? m3110014664 { get; set; }
        [MetaProperty(1275098597, BinPropertyType.Float)]
        public float? m1275098597 { get; set; }
        [MetaProperty(141215298, BinPropertyType.Float)]
        public float? m141215298 { get; set; }
        [MetaProperty(1243173199, BinPropertyType.Float)]
        public float? m1243173199 { get; set; }
        [MetaProperty(1869864963, BinPropertyType.Float)]
        public float? m1869864963 { get; set; }
        [MetaProperty(1708904904, BinPropertyType.Float)]
        public float? m1708904904 { get; set; }
        [MetaProperty(52406491, BinPropertyType.String)]
        public string? m52406491 { get; set; }
        [MetaProperty(1682447548, BinPropertyType.String)]
        public string? m1682447548 { get; set; }
    }
    [MetaClass("TftEffectAmount")]
    public class TftEffectAmount : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.Hash)]
        public MetaHash? Name { get; set; }
        [MetaProperty("value", BinPropertyType.Float)]
        public float? Value { get; set; }
        [MetaProperty("formatString", BinPropertyType.String)]
        public string? FormatString { get; set; }
    }
    [MetaClass("TftItemData")]
    public class TftItemData : IMetaClass
    {
        [MetaProperty("mId", BinPropertyType.Int32)]
        public int? Id { get; set; }
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string? BuffName { get; set; }
        [MetaProperty(2509677447, BinPropertyType.Bool)]
        public bool? m2509677447 { get; set; }
        [MetaProperty("mComposition", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Composition { get; set; }
        [MetaProperty(1733478293, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m1733478293 { get; set; }
        [MetaProperty("effectAmounts", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftEffectAmount>> EffectAmounts { get; set; }
        [MetaProperty(3272883558, BinPropertyType.String)]
        public string? m3272883558 { get; set; }
        [MetaProperty(1985943770, BinPropertyType.String)]
        public string? m1985943770 { get; set; }
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string? IconPath { get; set; }
        [MetaProperty("mColor", BinPropertyType.Optional)]
        public MetaOptional<Color> Color { get; set; }
        [MetaProperty(2446810623, BinPropertyType.Vector2)]
        public Vector2? m2446810623 { get; set; }
        [MetaProperty("mVfxSystem", BinPropertyType.ObjectLink)]
        public MetaObjectLink? VfxSystem { get; set; }
        [MetaProperty(1838141165, BinPropertyType.Int32)]
        public int? m1838141165 { get; set; }
    }
    [MetaClass("TFTItemList")]
    public class TFTItemList : IMetaClass
    {
        [MetaProperty("mItems", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Items { get; set; }
        [MetaProperty(2679170533, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2679170533 { get; set; }
    }
    [MetaClass("TFTModeData")]
    public class TFTModeData : IMetaClass
    {
        [MetaProperty(749352059, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m749352059 { get; set; }
        [MetaProperty(3020870234, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTDragData> m3020870234 { get; set; }
        [MetaProperty(3126367348, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTDragData> m3126367348 { get; set; }
        [MetaProperty(1018083252, BinPropertyType.Float)]
        public float? m1018083252 { get; set; }
        [MetaProperty(1243157057, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1243157057 { get; set; }
        [MetaProperty(753102698, BinPropertyType.Hash)]
        public MetaHash? m753102698 { get; set; }
        [MetaProperty(1092711647, BinPropertyType.Hash)]
        public MetaHash? m1092711647 { get; set; }
        [MetaProperty("mDefaultTftCompanion", BinPropertyType.Hash)]
        public MetaHash? DefaultTftCompanion { get; set; }
        [MetaProperty(2781118562, BinPropertyType.Hash)]
        public MetaHash? m2781118562 { get; set; }
        [MetaProperty(3076159825, BinPropertyType.Float)]
        public float? m3076159825 { get; set; }
    }
    [MetaClass("TFTGameVariationData")]
    public class TFTGameVariationData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(4287681780, BinPropertyType.String)]
        public string? m4287681780 { get; set; }
        [MetaProperty(1454904977, BinPropertyType.String)]
        public string? m1454904977 { get; set; }
        [MetaProperty(2648100384, BinPropertyType.String)]
        public string? m2648100384 { get; set; }
        [MetaProperty(1539123372, BinPropertyType.String)]
        public string? m1539123372 { get; set; }
        [MetaProperty(1220847671, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1220847671 { get; set; }
    }
    [MetaClass("TFTNotificationData")]
    public class TFTNotificationData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(103528693, BinPropertyType.Float)]
        public float? m103528693 { get; set; }
        [MetaProperty(3973906906, BinPropertyType.String)]
        public string? m3973906906 { get; set; }
        [MetaProperty(4079192626, BinPropertyType.String)]
        public string? m4079192626 { get; set; }
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string? IconPath { get; set; }
        [MetaProperty(511705008, BinPropertyType.String)]
        public string? m511705008 { get; set; }
        [MetaProperty(3730563465, BinPropertyType.String)]
        public string? m3730563465 { get; set; }
        [MetaProperty(2793884270, BinPropertyType.String)]
        public string? m2793884270 { get; set; }
    }
    [MetaClass("TFTPhaseData")]
    public class TFTPhaseData : IMetaClass
    {
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDuration", BinPropertyType.Float)]
        public float? Duration { get; set; }
        [MetaProperty("mDisplay", BinPropertyType.UInt32)]
        public uint? Display { get; set; }
        [MetaProperty("mLabel", BinPropertyType.String)]
        public string? Label { get; set; }
        [MetaProperty(1220847671, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1220847671 { get; set; }
    }
    [MetaClass("TFTRoundData")]
    public class TFTRoundData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string? IconPath { get; set; }
        [MetaProperty(3272883558, BinPropertyType.String)]
        public string? m3272883558 { get; set; }
        [MetaProperty(3159300275, BinPropertyType.String)]
        public string? m3159300275 { get; set; }
        [MetaProperty(4074052049, BinPropertyType.String)]
        public string? m4074052049 { get; set; }
        [MetaProperty(1630940802, BinPropertyType.Map)]
        public Dictionary<uint, string> m1630940802 { get; set; }
        [MetaProperty(3523237056, BinPropertyType.String)]
        public string? m3523237056 { get; set; }
        [MetaProperty(810222384, BinPropertyType.String)]
        public string? m810222384 { get; set; }
        [MetaProperty(4167224325, BinPropertyType.String)]
        public string? m4167224325 { get; set; }
        [MetaProperty(3414363751, BinPropertyType.String)]
        public string? m3414363751 { get; set; }
        [MetaProperty(3373210076, BinPropertyType.String)]
        public string? m3373210076 { get; set; }
        [MetaProperty(1286805709, BinPropertyType.String)]
        public string? m1286805709 { get; set; }
        [MetaProperty("mDraftArrival", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> DraftArrival { get; set; }
        [MetaProperty("mDraft", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> Draft { get; set; }
        [MetaProperty("mDraftDeparture", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> DraftDeparture { get; set; }
        [MetaProperty("mPlanningArrival", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> PlanningArrival { get; set; }
        [MetaProperty("mPlanning", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> Planning { get; set; }
        [MetaProperty("mPlanningDeparture", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> PlanningDeparture { get; set; }
        [MetaProperty("mCombatArrival", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> CombatArrival { get; set; }
        [MetaProperty("mCombat", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> Combat { get; set; }
        [MetaProperty("mCombatDeparture", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTPhaseData> CombatDeparture { get; set; }
        [MetaProperty(2029389651, BinPropertyType.Map)]
        public Dictionary<string, GameModeConstant> m2029389651 { get; set; }
    }
    [MetaClass("TFTStageData")]
    public class TFTStageData : IMetaClass
    {
        [MetaProperty(548140649, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m548140649 { get; set; }
    }
    [MetaClass("TFTSetData")]
    public class TFTSetData : IMetaClass
    {
        [MetaProperty("number", BinPropertyType.UInt32)]
        public uint? Number { get; set; }
        [MetaProperty("Mutator", BinPropertyType.String)]
        public string? Mutator { get; set; }
        [MetaProperty("characterLists", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> CharacterLists { get; set; }
        [MetaProperty(1298046227, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m1298046227 { get; set; }
        [MetaProperty(2970411059, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2970411059 { get; set; }
        [MetaProperty("traits", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Traits { get; set; }
        [MetaProperty(3528691290, BinPropertyType.Map)]
        public Dictionary<string, GameModeConstant> m3528691290 { get; set; }
        [MetaProperty("stages", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTStageData>> Stages { get; set; }
        [MetaProperty(1739760738, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m1739760738 { get; set; }
        [MetaProperty(2679170533, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2679170533 { get; set; }
    }
    [MetaClass("TftShopData")]
    public class TftShopData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mRarity", BinPropertyType.Byte)]
        public byte? Rarity { get; set; }
        [MetaProperty("mPortraitIconPath", BinPropertyType.String)]
        public string? PortraitIconPath { get; set; }
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string? IconPath { get; set; }
        [MetaProperty(1673645048, BinPropertyType.String)]
        public string? m1673645048 { get; set; }
        [MetaProperty(584007147, BinPropertyType.String)]
        public string? m584007147 { get; set; }
        [MetaProperty(3272883558, BinPropertyType.String)]
        public string? m3272883558 { get; set; }
        [MetaProperty(2275842654, BinPropertyType.String)]
        public string? m2275842654 { get; set; }
        [MetaProperty(3159300275, BinPropertyType.String)]
        public string? m3159300275 { get; set; }
    }
    [MetaClass("TFTStatData")]
    public class TFTStatData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mType", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("mContext", BinPropertyType.UInt32)]
        public uint? Context { get; set; }
        [MetaProperty("mLifetime", BinPropertyType.UInt32)]
        public uint? Lifetime { get; set; }
        [MetaProperty(255472540, BinPropertyType.Int32)]
        public int? m255472540 { get; set; }
    }
    [MetaClass("TFTStreak")]
    public class TFTStreak : IMetaClass
    {
        [MetaProperty(2940314578, BinPropertyType.Optional)]
        public MetaOptional<uint> m2940314578 { get; set; }
        [MetaProperty(3076655600, BinPropertyType.Optional)]
        public MetaOptional<uint> m3076655600 { get; set; }
        [MetaProperty(3278832061, BinPropertyType.UInt32)]
        public uint? m3278832061 { get; set; }
        [MetaProperty(3234469753, BinPropertyType.String)]
        public string? m3234469753 { get; set; }
    }
    [MetaClass("TFTStreakData")]
    public class TFTStreakData : IMetaClass
    {
        [MetaProperty(3287630061, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTStreak>> m3287630061 { get; set; }
        [MetaProperty(3004055014, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTStreak>> m3004055014 { get; set; }
    }
    [MetaClass("TFTTraitContributionData")]
    public class TFTTraitContributionData : IMetaClass
    {
        [MetaProperty(87695155, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m87695155 { get; set; }
        [MetaProperty("Amount", BinPropertyType.Int32)]
        public int? Amount { get; set; }
        [MetaProperty(2836412405, BinPropertyType.Bool)]
        public bool? m2836412405 { get; set; }
    }
    [MetaClass("TFTTraitSetData")]
    public class TFTTraitSetData : IMetaClass
    {
        [MetaProperty("mMinUnits", BinPropertyType.UInt32)]
        public uint? MinUnits { get; set; }
        [MetaProperty("mMaxUnits", BinPropertyType.Optional)]
        public MetaOptional<uint> MaxUnits { get; set; }
        [MetaProperty("mTeamToBuff", BinPropertyType.Byte)]
        public byte? TeamToBuff { get; set; }
        [MetaProperty(2516677504, BinPropertyType.Byte)]
        public byte? m2516677504 { get; set; }
        [MetaProperty(2831490480, BinPropertyType.Optional)]
        public MetaOptional<uint> m2831490480 { get; set; }
        [MetaProperty("mStyle", BinPropertyType.Byte)]
        public byte? Style { get; set; }
        [MetaProperty("mBuffName", BinPropertyType.String)]
        public string? BuffName { get; set; }
        [MetaProperty(1435427873, BinPropertyType.String)]
        public string? m1435427873 { get; set; }
        [MetaProperty("effectAmounts", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TftEffectAmount>> EffectAmounts { get; set; }
    }
    [MetaClass("TftTraitData")]
    public class TftTraitData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(3272883558, BinPropertyType.String)]
        public string? m3272883558 { get; set; }
        [MetaProperty(3805595466, BinPropertyType.String)]
        public string? m3805595466 { get; set; }
        [MetaProperty(1985943770, BinPropertyType.String)]
        public string? m1985943770 { get; set; }
        [MetaProperty("mDisplayNameIcon", BinPropertyType.String)]
        public string? DisplayNameIcon { get; set; }
        [MetaProperty("mIconPath", BinPropertyType.String)]
        public string? IconPath { get; set; }
        [MetaProperty("mTraitSets", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TFTTraitSetData>> TraitSets { get; set; }
    }
    [MetaClass(1539106983)]
    public class Class1539106983 : IMetaClass
    {
        [MetaProperty(1367954925, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m1367954925 { get; set; }
        [MetaProperty(2679170533, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2679170533 { get; set; }
    }
    [MetaClass(1963259073)]
    public class Class1963259073 : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(3604921790, BinPropertyType.Byte)]
        public byte? m3604921790 { get; set; }
    }
    [MetaClass("TFTHudAnnouncementData")]
    public class TFTHudAnnouncementData : IMetaClass
    {
        [MetaProperty("mSceneTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionIn { get; set; }
        [MetaProperty("mSceneTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionOut { get; set; }
    }
    [MetaClass("TFTHudCombatRecapData")]
    public class TFTHudCombatRecapData : IMetaClass
    {
        [MetaProperty(3747431117, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m3747431117 { get; set; }
        [MetaProperty(1162113435, BinPropertyType.Float)]
        public float? m1162113435 { get; set; }
    }
    [MetaClass("TFTHudNotificationsData")]
    public class TFTHudNotificationsData : IMetaClass
    {
        [MetaProperty("mSceneTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionIn { get; set; }
        [MetaProperty("mSceneTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionOut { get; set; }
    }
    [MetaClass(3661393350)]
    public class Class3661393350 : IMetaClass
    {
        [MetaProperty("mSceneTransitionIn", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionIn { get; set; }
        [MetaProperty("mSceneTransitionOut", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransitionOut { get; set; }
    }
    [MetaClass("TFTHudScoreboardData")]
    public class TFTHudScoreboardData : IMetaClass
    {
        [MetaProperty(2215596130, BinPropertyType.Float)]
        public float? m2215596130 { get; set; }
        [MetaProperty(625770807, BinPropertyType.Float)]
        public float? m625770807 { get; set; }
        [MetaProperty(4110713210, BinPropertyType.Float)]
        public float? m4110713210 { get; set; }
    }
    [MetaClass("TFTHudStageData")]
    public class TFTHudStageData : IMetaClass
    {
        [MetaProperty(2702329528, BinPropertyType.Float)]
        public float? m2702329528 { get; set; }
    }
    [MetaClass("TFTHudUnitShopData")]
    public class TFTHudUnitShopData : IMetaClass
    {
        [MetaProperty(2288019587, BinPropertyType.Float)]
        public float? m2288019587 { get; set; }
        [MetaProperty(3280759721, BinPropertyType.Float)]
        public float? m3280759721 { get; set; }
    }
    [MetaClass("TFTHudMobileDownscaleData")]
    public class TFTHudMobileDownscaleData : IMetaClass
    {
        [MetaProperty(3570592338, BinPropertyType.Float)]
        public float? m3570592338 { get; set; }
        [MetaProperty(2798263326, BinPropertyType.Float)]
        public float? m2798263326 { get; set; }
    }
    [MetaClass("TFTHudTunables")]
    public class TFTHudTunables : IMetaClass
    {
        [MetaProperty(1220847671, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudAnnouncementData> m1220847671 { get; set; }
        [MetaProperty("mCombatRecapData", BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudCombatRecapData> CombatRecapData { get; set; }
        [MetaProperty(4040593892, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudNotificationsData> m4040593892 { get; set; }
        [MetaProperty(1719092004, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3661393350> m1719092004 { get; set; }
        [MetaProperty(53570106, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudScoreboardData> m53570106 { get; set; }
        [MetaProperty(272066278, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudStageData> m272066278 { get; set; }
        [MetaProperty(614458760, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m614458760 { get; set; }
        [MetaProperty(731802664, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudMobileDownscaleData> m731802664 { get; set; }
        [MetaProperty(1666522042, BinPropertyType.Embedded)]
        public MetaEmbedded<TFTHudUnitShopData> m1666522042 { get; set; }
    }
    [MetaClass("ObjectTags")]
    public class ObjectTags : IMetaClass
    {
        [MetaProperty("mTagList", BinPropertyType.String)]
        public string? TagList { get; set; }
    }
    [MetaClass("NumberFormattingBehavior")]
    public class NumberFormattingBehavior : IMetaClass
    {
        [MetaProperty(1778472996, BinPropertyType.UInt32)]
        public uint? m1778472996 { get; set; }
        [MetaProperty(2559746888, BinPropertyType.Bool)]
        public bool? m2559746888 { get; set; }
        [MetaProperty(905883269, BinPropertyType.Bool)]
        public bool? m905883269 { get; set; }
    }
    [MetaClass("NumberFormattingData")]
    public class NumberFormattingData : IMetaClass
    {
        [MetaProperty(1535520071, BinPropertyType.String)]
        public string? m1535520071 { get; set; }
        [MetaProperty(3515031867, BinPropertyType.String)]
        public string? m3515031867 { get; set; }
        [MetaProperty(3113614111, BinPropertyType.String)]
        public string? m3113614111 { get; set; }
        [MetaProperty(1089846550, BinPropertyType.String)]
        public string? m1089846550 { get; set; }
        [MetaProperty(3990416003, BinPropertyType.String)]
        public string? m3990416003 { get; set; }
        [MetaProperty(823565823, BinPropertyType.String)]
        public string? m823565823 { get; set; }
        [MetaProperty(3841310158, BinPropertyType.String)]
        public string? m3841310158 { get; set; }
        [MetaProperty(19785452, BinPropertyType.String)]
        public string? m19785452 { get; set; }
        [MetaProperty(1880587249, BinPropertyType.String)]
        public string? m1880587249 { get; set; }
        [MetaProperty(2965779045, BinPropertyType.String)]
        public string? m2965779045 { get; set; }
        [MetaProperty(2309425659, BinPropertyType.String)]
        public string? m2309425659 { get; set; }
        [MetaProperty(4092495889, BinPropertyType.String)]
        public string? m4092495889 { get; set; }
        [MetaProperty(4018485617, BinPropertyType.String)]
        public string? m4018485617 { get; set; }
        [MetaProperty(4252791735, BinPropertyType.String)]
        public string? m4252791735 { get; set; }
        [MetaProperty(3710897474, BinPropertyType.String)]
        public string? m3710897474 { get; set; }
        [MetaProperty(3274771674, BinPropertyType.String)]
        public string? m3274771674 { get; set; }
        [MetaProperty(2051901883, BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> m2051901883 { get; set; }
    }
    [MetaClass("InvalidDeviceViewController")]
    public class InvalidDeviceViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
    }
    [MetaClass("LoginViewController")]
    public class LoginViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
    }
    [MetaClass("PatchingViewController")]
    public class PatchingViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(1000346113, BinPropertyType.Hash)]
        public MetaHash? m1000346113 { get; set; }
        [MetaProperty(4140331697, BinPropertyType.Hash)]
        public MetaHash? m4140331697 { get; set; }
    }
    [MetaClass("SummonerNameCreateViewController")]
    public class SummonerNameCreateViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(697499175, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m697499175 { get; set; }
        [MetaProperty(2809917428, BinPropertyType.Hash)]
        public MetaHash? m2809917428 { get; set; }
    }
    [MetaClass(3963535729)]
    public class Class3963535729 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(1860418862, BinPropertyType.Float)]
        public float? m1860418862 { get; set; }
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(3019062520, BinPropertyType.Hash)]
        public MetaHash? m3019062520 { get; set; }
        [MetaProperty(2827571664, BinPropertyType.Hash)]
        public MetaHash? m2827571664 { get; set; }
        [MetaProperty(660238919, BinPropertyType.Hash)]
        public MetaHash? m660238919 { get; set; }
        [MetaProperty(1515685582, BinPropertyType.Hash)]
        public MetaHash? m1515685582 { get; set; }
        [MetaProperty(1513907673, BinPropertyType.Hash)]
        public MetaHash? m1513907673 { get; set; }
        [MetaProperty(3343676972, BinPropertyType.Hash)]
        public MetaHash? m3343676972 { get; set; }
        [MetaProperty(1246660210, BinPropertyType.Byte)]
        public byte? m1246660210 { get; set; }
        [MetaProperty(1994263565, BinPropertyType.Byte)]
        public byte? m1994263565 { get; set; }
    }
    [MetaClass("ChatThrottlerData")]
    public class ChatThrottlerData : IMetaClass
    {
        [MetaProperty("throttleLimit", BinPropertyType.UInt32)]
        public uint? ThrottleLimit { get; set; }
        [MetaProperty("checkDurationSeconds", BinPropertyType.Float)]
        public float? CheckDurationSeconds { get; set; }
        [MetaProperty("cooldownTimeSeconds", BinPropertyType.Float)]
        public float? CooldownTimeSeconds { get; set; }
    }
    [MetaClass(2228508446)]
    public class Class2228508446 : IMetaClass
    {
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(1863558141, BinPropertyType.Embedded)]
        public MetaEmbedded<ChatThrottlerData> m1863558141 { get; set; }
    }
    [MetaClass(3766132529)]
    public class Class3766132529 : Class1965398739
    {
        [MetaProperty(2154483994, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2154483994 { get; set; }
        [MetaProperty(1117474311, BinPropertyType.Hash)]
        public MetaHash? m1117474311 { get; set; }
        [MetaProperty(4040210147, BinPropertyType.Hash)]
        public MetaHash? m4040210147 { get; set; }
    }
    [MetaClass(1965398739)]
    public class Class1965398739 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(893211270, BinPropertyType.Hash)]
        public MetaHash? m893211270 { get; set; }
        [MetaProperty(3228738802, BinPropertyType.Hash)]
        public MetaHash? m3228738802 { get; set; }
        [MetaProperty(2278507006, BinPropertyType.Hash)]
        public MetaHash? m2278507006 { get; set; }
        [MetaProperty("ContentScene", BinPropertyType.Hash)]
        public MetaHash? ContentScene { get; set; }
        [MetaProperty(2330109623, BinPropertyType.Structure)]
        public Class2330109623 m2330109623 { get; set; }
    }
    [MetaClass("CursorData")]
    public class CursorData : IMetaClass
    {
        [MetaProperty("mHotSpot", BinPropertyType.Vector2)]
        public Vector2? HotSpot { get; set; }
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
        [MetaProperty(1703356166, BinPropertyType.String)]
        public string? m1703356166 { get; set; }
        [MetaProperty(3527462479, BinPropertyType.String)]
        public string? m3527462479 { get; set; }
        [MetaProperty(4132662353, BinPropertyType.String)]
        public string? m4132662353 { get; set; }
    }
    [MetaClass("CursorDataTeamContext")]
    public class CursorDataTeamContext : IMetaClass
    {
        [MetaProperty("mData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorData>> Data { get; set; }
    }
    [MetaClass("CursorDataCaptureCooldownContext")]
    public class CursorDataCaptureCooldownContext : IMetaClass
    {
        [MetaProperty("mData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorData>> Data { get; set; }
    }
    [MetaClass("CursorConfig")]
    public class CursorConfig : IMetaClass
    {
        [MetaProperty("mSingleContextCursors", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorData>> SingleContextCursors { get; set; }
        [MetaProperty("mTeamContextCursors", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<CursorDataTeamContext>> TeamContextCursors { get; set; }
        [MetaProperty("mHoverNotUseableCursor", BinPropertyType.Embedded)]
        public MetaEmbedded<CursorDataCaptureCooldownContext> HoverNotUseableCursor { get; set; }
    }
    [MetaClass("HealthbarImageInfo")]
    public class HealthbarImageInfo : IMetaClass
    {
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
        [MetaProperty("mOffset", BinPropertyType.Vector2)]
        public Vector2? Offset { get; set; }
        [MetaProperty("mTextureUvs", BinPropertyType.Vector4)]
        public Vector4? TextureUvs { get; set; }
    }
    [MetaClass("UnitStatusData")]
    public class UnitStatusData : IMetaClass
    {
        [MetaProperty("locType", BinPropertyType.UInt32)]
        public uint? LocType { get; set; }
        [MetaProperty("attackableUnitStatusType", BinPropertyType.UInt32)]
        public uint? AttackableUnitStatusType { get; set; }
        [MetaProperty("statusName", BinPropertyType.String)]
        public string? StatusName { get; set; }
        [MetaProperty("textColor", BinPropertyType.Optional)]
        public MetaOptional<Color> TextColor { get; set; }
        [MetaProperty("imageInfo", BinPropertyType.Embedded)]
        public MetaEmbedded<HealthbarImageInfo> ImageInfo { get; set; }
    }
    [MetaClass("UnitStatusPriorityList")]
    public class UnitStatusPriorityList : IMetaClass
    {
        [MetaProperty("mMinimumDisplayTime", BinPropertyType.Float)]
        public float? MinimumDisplayTime { get; set; }
        [MetaProperty("mPrioritizedUnitStatusData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<UnitStatusData>> PrioritizedUnitStatusData { get; set; }
    }
    [MetaClass("FloatTextIconData")]
    public class FloatTextIconData : IMetaClass
    {
        [MetaProperty("mIconFileName", BinPropertyType.String)]
        public string? IconFileName { get; set; }
        [MetaProperty("mColor", BinPropertyType.Color)]
        public Color? Color { get; set; }
        [MetaProperty("mDisplaySize", BinPropertyType.Vector2)]
        public Vector2? DisplaySize { get; set; }
        [MetaProperty("mOffset", BinPropertyType.Vector2)]
        public Vector2? Offset { get; set; }
        [MetaProperty("mAlignment", BinPropertyType.UInt32)]
        public uint? Alignment { get; set; }
    }
    [MetaClass("FloatTextDisplayOverrides")]
    public class FloatTextDisplayOverrides : IMetaClass
    {
        [MetaProperty("priority", BinPropertyType.Optional)]
        public MetaOptional<int> Priority { get; set; }
        [MetaProperty("maxInstances", BinPropertyType.Optional)]
        public MetaOptional<int> MaxInstances { get; set; }
        [MetaProperty("isAnimated", BinPropertyType.Optional)]
        public MetaOptional<bool> IsAnimated { get; set; }
        [MetaProperty("disableHorizontalReverse", BinPropertyType.Optional)]
        public MetaOptional<bool> DisableHorizontalReverse { get; set; }
        [MetaProperty("disableVerticalReverse", BinPropertyType.Optional)]
        public MetaOptional<bool> DisableVerticalReverse { get; set; }
        [MetaProperty("momentumFromHit", BinPropertyType.Optional)]
        public MetaOptional<bool> MomentumFromHit { get; set; }
        [MetaProperty("followSource", BinPropertyType.Optional)]
        public MetaOptional<bool> FollowSource { get; set; }
        [MetaProperty("ignoreCombineRules", BinPropertyType.Optional)]
        public MetaOptional<bool> IgnoreCombineRules { get; set; }
        [MetaProperty("ignoreQueue", BinPropertyType.Optional)]
        public MetaOptional<bool> IgnoreQueue { get; set; }
        [MetaProperty("alternateRightLeft", BinPropertyType.Optional)]
        public MetaOptional<bool> AlternateRightLeft { get; set; }
        [MetaProperty("combinableCounterDisplay", BinPropertyType.Optional)]
        public MetaOptional<bool> CombinableCounterDisplay { get; set; }
        [MetaProperty("combinableCounterCategory", BinPropertyType.Optional)]
        public MetaOptional<int> CombinableCounterCategory { get; set; }
        [MetaProperty("overwritePreviousNumber", BinPropertyType.Optional)]
        public MetaOptional<bool> OverwritePreviousNumber { get; set; }
        [MetaProperty("extendTimeOnNewDamage", BinPropertyType.Optional)]
        public MetaOptional<float> ExtendTimeOnNewDamage { get; set; }
        [MetaProperty("maxLifeTime", BinPropertyType.Optional)]
        public MetaOptional<float> MaxLifeTime { get; set; }
        [MetaProperty("colorOffsetR", BinPropertyType.Optional)]
        public MetaOptional<int> ColorOffsetR { get; set; }
        [MetaProperty("colorOffsetG", BinPropertyType.Optional)]
        public MetaOptional<int> ColorOffsetG { get; set; }
        [MetaProperty("colorOffsetB", BinPropertyType.Optional)]
        public MetaOptional<int> ColorOffsetB { get; set; }
        [MetaProperty("scale", BinPropertyType.Optional)]
        public MetaOptional<float> Scale { get; set; }
        [MetaProperty("decay", BinPropertyType.Optional)]
        public MetaOptional<float> Decay { get; set; }
        [MetaProperty("decayDelay", BinPropertyType.Optional)]
        public MetaOptional<float> DecayDelay { get; set; }
        [MetaProperty("shrinkTime", BinPropertyType.Optional)]
        public MetaOptional<float> ShrinkTime { get; set; }
        [MetaProperty("shrinkScale", BinPropertyType.Optional)]
        public MetaOptional<float> ShrinkScale { get; set; }
        [MetaProperty("hangTime", BinPropertyType.Optional)]
        public MetaOptional<float> HangTime { get; set; }
        [MetaProperty("randomOffsetMinX", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMinX { get; set; }
        [MetaProperty("randomOffsetMaxX", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMaxX { get; set; }
        [MetaProperty("randomOffsetMinY", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMinY { get; set; }
        [MetaProperty("randomOffsetMaxY", BinPropertyType.Optional)]
        public MetaOptional<float> RandomOffsetMaxY { get; set; }
        [MetaProperty("startOffsetX", BinPropertyType.Optional)]
        public MetaOptional<float> StartOffsetX { get; set; }
        [MetaProperty("startOffsetY", BinPropertyType.Optional)]
        public MetaOptional<float> StartOffsetY { get; set; }
        [MetaProperty("relativeOffsetMin", BinPropertyType.Optional)]
        public MetaOptional<float> RelativeOffsetMin { get; set; }
        [MetaProperty("relativeOffsetMax", BinPropertyType.Optional)]
        public MetaOptional<float> RelativeOffsetMax { get; set; }
        [MetaProperty("minXVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MinXVelocity { get; set; }
        [MetaProperty("maxXVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MaxXVelocity { get; set; }
        [MetaProperty("minYVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MinYVelocity { get; set; }
        [MetaProperty("maxYVelocity", BinPropertyType.Optional)]
        public MetaOptional<float> MaxYVelocity { get; set; }
        [MetaProperty("continualForceX", BinPropertyType.Optional)]
        public MetaOptional<float> ContinualForceX { get; set; }
        [MetaProperty("continualForceY", BinPropertyType.Optional)]
        public MetaOptional<float> ContinualForceY { get; set; }
        [MetaProperty("growthXScalar", BinPropertyType.Optional)]
        public MetaOptional<float> GrowthXScalar { get; set; }
        [MetaProperty("growthYScalar", BinPropertyType.Optional)]
        public MetaOptional<float> GrowthYScalar { get; set; }
    }
    [MetaClass("FloatingTextTunables")]
    public class FloatingTextTunables : IMetaClass
    {
        [MetaProperty("mMaxFloatingTextItems", BinPropertyType.UInt32)]
        public uint? MaxFloatingTextItems { get; set; }
        [MetaProperty("mIntervalInPix", BinPropertyType.Float)]
        public float? IntervalInPix { get; set; }
        [MetaProperty("mScrollSpeed", BinPropertyType.Float)]
        public float? ScrollSpeed { get; set; }
        [MetaProperty("mAnimatedTextQueueDelay", BinPropertyType.Float)]
        public float? AnimatedTextQueueDelay { get; set; }
        [MetaProperty("mYResolutionBaseline", BinPropertyType.Float)]
        public float? YResolutionBaseline { get; set; }
        [MetaProperty("mMinimumDynamicScale", BinPropertyType.Float)]
        public float? MinimumDynamicScale { get; set; }
        [MetaProperty("mMaximumDynamicScale", BinPropertyType.Float)]
        public float? MaximumDynamicScale { get; set; }
        [MetaProperty(3438744487, BinPropertyType.Float)]
        public float? m3438744487 { get; set; }
        [MetaProperty(4117694812, BinPropertyType.Float)]
        public float? m4117694812 { get; set; }
        [MetaProperty("mMinionComparisonMultiplier", BinPropertyType.Float)]
        public float? MinionComparisonMultiplier { get; set; }
        [MetaProperty("mLocalPlayerHealthComparison", BinPropertyType.Float)]
        public float? LocalPlayerHealthComparison { get; set; }
        [MetaProperty("mComparisonByLevel", BinPropertyType.Container)]
        public MetaContainer<float> ComparisonByLevel { get; set; }
    }
    [MetaClass("FloatingTextDamageDisplayTypeList")]
    public class FloatingTextDamageDisplayTypeList : IMetaClass
    {
        [MetaProperty("Default", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Default { get; set; }
        [MetaProperty("Impact", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Impact { get; set; }
        [MetaProperty("Zone", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Zone { get; set; }
        [MetaProperty("Ult", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Ult { get; set; }
        [MetaProperty("DotNoCombine", BinPropertyType.ObjectLink)]
        public MetaObjectLink? DotNoCombine { get; set; }
        [MetaProperty("Dot", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Dot { get; set; }
        [MetaProperty("DotSlow", BinPropertyType.ObjectLink)]
        public MetaObjectLink? DotSlow { get; set; }
        [MetaProperty("Multistrike", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Multistrike { get; set; }
        [MetaProperty("MultistrikeFast", BinPropertyType.ObjectLink)]
        public MetaObjectLink? MultistrikeFast { get; set; }
        [MetaProperty("MultistrikeSlow", BinPropertyType.ObjectLink)]
        public MetaObjectLink? MultistrikeSlow { get; set; }
        [MetaProperty("PlayerMinion", BinPropertyType.ObjectLink)]
        public MetaObjectLink? PlayerMinion { get; set; }
        [MetaProperty("BarrackMinion", BinPropertyType.ObjectLink)]
        public MetaObjectLink? BarrackMinion { get; set; }
        [MetaProperty("Mini", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Mini { get; set; }
        [MetaProperty("SelfTrueDamageCounter", BinPropertyType.ObjectLink)]
        public MetaObjectLink? SelfTrueDamageCounter { get; set; }
        [MetaProperty("SelfPhysicalDamageCounter", BinPropertyType.ObjectLink)]
        public MetaObjectLink? SelfPhysicalDamageCounter { get; set; }
        [MetaProperty("SelfMagicalDamageCounter", BinPropertyType.ObjectLink)]
        public MetaObjectLink? SelfMagicalDamageCounter { get; set; }
    }
    [MetaClass("FloatTextFormattingData")]
    public class FloatTextFormattingData : IMetaClass
    {
        [MetaProperty("mTypeName", BinPropertyType.Hash)]
        public MetaHash? TypeName { get; set; }
        [MetaProperty("colorOffsetR", BinPropertyType.Int32)]
        public int? ColorOffsetR { get; set; }
        [MetaProperty("colorOffsetG", BinPropertyType.Int32)]
        public int? ColorOffsetG { get; set; }
        [MetaProperty("colorOffsetB", BinPropertyType.Int32)]
        public int? ColorOffsetB { get; set; }
        [MetaProperty("mFontDescription", BinPropertyType.ObjectLink)]
        public MetaObjectLink? FontDescription { get; set; }
        [MetaProperty("ignoreCombineRules", BinPropertyType.Bool)]
        public bool? IgnoreCombineRules { get; set; }
        [MetaProperty("combinableNumberFormat", BinPropertyType.String)]
        public string? CombinableNumberFormat { get; set; }
        [MetaProperty("combinableNegativeNumberFormat", BinPropertyType.String)]
        public string? CombinableNegativeNumberFormat { get; set; }
        [MetaProperty("priority", BinPropertyType.Int32)]
        public int? Priority { get; set; }
        [MetaProperty("height", BinPropertyType.Float)]
        public float? Height { get; set; }
        [MetaProperty("decay", BinPropertyType.Float)]
        public float? Decay { get; set; }
        [MetaProperty("decayDelay", BinPropertyType.Float)]
        public float? DecayDelay { get; set; }
        [MetaProperty("disabled", BinPropertyType.Bool)]
        public bool? Disabled { get; set; }
        [MetaProperty("isAnimated", BinPropertyType.Bool)]
        public bool? IsAnimated { get; set; }
        [MetaProperty("momentumFromHit", BinPropertyType.Bool)]
        public bool? MomentumFromHit { get; set; }
        [MetaProperty("followSource", BinPropertyType.Bool)]
        public bool? FollowSource { get; set; }
        [MetaProperty("disableHorizontalReverse", BinPropertyType.Bool)]
        public bool? DisableHorizontalReverse { get; set; }
        [MetaProperty("disableVerticalReverse", BinPropertyType.Bool)]
        public bool? DisableVerticalReverse { get; set; }
        [MetaProperty("combinableCounterDisplay", BinPropertyType.Bool)]
        public bool? CombinableCounterDisplay { get; set; }
        [MetaProperty("combinableCounterCategory", BinPropertyType.Int32)]
        public int? CombinableCounterCategory { get; set; }
        [MetaProperty("overwritePreviousNumber", BinPropertyType.Bool)]
        public bool? OverwritePreviousNumber { get; set; }
        [MetaProperty("minXVelocity", BinPropertyType.Float)]
        public float? MinXVelocity { get; set; }
        [MetaProperty("maxXVelocity", BinPropertyType.Float)]
        public float? MaxXVelocity { get; set; }
        [MetaProperty("minYVelocity", BinPropertyType.Float)]
        public float? MinYVelocity { get; set; }
        [MetaProperty("maxYVelocity", BinPropertyType.Float)]
        public float? MaxYVelocity { get; set; }
        [MetaProperty("startOffsetX", BinPropertyType.Float)]
        public float? StartOffsetX { get; set; }
        [MetaProperty("startOffsetY", BinPropertyType.Float)]
        public float? StartOffsetY { get; set; }
        [MetaProperty("hangTime", BinPropertyType.Float)]
        public float? HangTime { get; set; }
        [MetaProperty("randomStartOffsetMinX", BinPropertyType.Float)]
        public float? RandomStartOffsetMinX { get; set; }
        [MetaProperty("randomStartOffsetMaxX", BinPropertyType.Float)]
        public float? RandomStartOffsetMaxX { get; set; }
        [MetaProperty("randomStartOffsetMinY", BinPropertyType.Float)]
        public float? RandomStartOffsetMinY { get; set; }
        [MetaProperty("randomStartOffsetMaxY", BinPropertyType.Float)]
        public float? RandomStartOffsetMaxY { get; set; }
        [MetaProperty("growthYScalar", BinPropertyType.Float)]
        public float? GrowthYScalar { get; set; }
        [MetaProperty("growthXScalar", BinPropertyType.Float)]
        public float? GrowthXScalar { get; set; }
        [MetaProperty("relativeOffsetMin", BinPropertyType.Float)]
        public float? RelativeOffsetMin { get; set; }
        [MetaProperty("relativeOffsetMax", BinPropertyType.Float)]
        public float? RelativeOffsetMax { get; set; }
        [MetaProperty("continualForceX", BinPropertyType.Float)]
        public float? ContinualForceX { get; set; }
        [MetaProperty("continualForceY", BinPropertyType.Float)]
        public float? ContinualForceY { get; set; }
        [MetaProperty("continualForceXBase", BinPropertyType.Float)]
        public float? ContinualForceXBase { get; set; }
        [MetaProperty("continualForceYBase", BinPropertyType.Float)]
        public float? ContinualForceYBase { get; set; }
        [MetaProperty("shrinkTime", BinPropertyType.Float)]
        public float? ShrinkTime { get; set; }
        [MetaProperty("scale", BinPropertyType.Float)]
        public float? Scale { get; set; }
        [MetaProperty("extendTimeOnNewDamage", BinPropertyType.Float)]
        public float? ExtendTimeOnNewDamage { get; set; }
        [MetaProperty("maxLifeTime", BinPropertyType.Float)]
        public float? MaxLifeTime { get; set; }
        [MetaProperty("ignoreQueue", BinPropertyType.Bool)]
        public bool? IgnoreQueue { get; set; }
        [MetaProperty("alternateRightLeft", BinPropertyType.Bool)]
        public bool? AlternateRightLeft { get; set; }
        [MetaProperty("maxInstances", BinPropertyType.Int32)]
        public int? MaxInstances { get; set; }
        [MetaProperty("shrinkScale", BinPropertyType.Float)]
        public float? ShrinkScale { get; set; }
        [MetaProperty("refreshExisting", BinPropertyType.Bool)]
        public bool? RefreshExisting { get; set; }
        [MetaProperty("attachToHealthBar", BinPropertyType.Bool)]
        public bool? AttachToHealthBar { get; set; }
        [MetaProperty("offsetByBoundingBox", BinPropertyType.Bool)]
        public bool? OffsetByBoundingBox { get; set; }
        [MetaProperty("horizontalAlignment", BinPropertyType.Byte)]
        public byte? HorizontalAlignment { get; set; }
        [MetaProperty("verticalAlignment", BinPropertyType.Byte)]
        public byte? VerticalAlignment { get; set; }
        [MetaProperty("icons", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FloatTextIconData>> Icons { get; set; }
    }
    [MetaClass("FloatingTextTypeList")]
    public class FloatingTextTypeList : IMetaClass
    {
        [MetaProperty("Invulnerable", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Invulnerable { get; set; }
        [MetaProperty("Special", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Special { get; set; }
        [MetaProperty("Heal", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Heal { get; set; }
        [MetaProperty("ManaHeal", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ManaHeal { get; set; }
        [MetaProperty("ManaDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ManaDamage { get; set; }
        [MetaProperty("Dodge", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Dodge { get; set; }
        [MetaProperty("PhysicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink? PhysicalDamageCritical { get; set; }
        [MetaProperty("MagicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink? MagicalDamageCritical { get; set; }
        [MetaProperty("TrueDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink? TrueDamageCritical { get; set; }
        [MetaProperty("Experience", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Experience { get; set; }
        [MetaProperty("Gold", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Gold { get; set; }
        [MetaProperty("level", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Level { get; set; }
        [MetaProperty("Disable", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Disable { get; set; }
        [MetaProperty("QuestReceived", BinPropertyType.ObjectLink)]
        public MetaObjectLink? QuestReceived { get; set; }
        [MetaProperty("QuestComplete", BinPropertyType.ObjectLink)]
        public MetaObjectLink? QuestComplete { get; set; }
        [MetaProperty("Score", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Score { get; set; }
        [MetaProperty("PhysicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? PhysicalDamage { get; set; }
        [MetaProperty("MagicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? MagicalDamage { get; set; }
        [MetaProperty("TrueDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? TrueDamage { get; set; }
        [MetaProperty("EnemyPhysicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? EnemyPhysicalDamage { get; set; }
        [MetaProperty("EnemyMagicalDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? EnemyMagicalDamage { get; set; }
        [MetaProperty("EnemyTrueDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? EnemyTrueDamage { get; set; }
        [MetaProperty("EnemyPhysicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink? EnemyPhysicalDamageCritical { get; set; }
        [MetaProperty("EnemyMagicalDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink? EnemyMagicalDamageCritical { get; set; }
        [MetaProperty("EnemyTrueDamageCritical", BinPropertyType.ObjectLink)]
        public MetaObjectLink? EnemyTrueDamageCritical { get; set; }
        [MetaProperty("Countdown", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Countdown { get; set; }
        [MetaProperty("OMW", BinPropertyType.ObjectLink)]
        public MetaObjectLink? OMW { get; set; }
        [MetaProperty("Absorbed", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Absorbed { get; set; }
        [MetaProperty("Debug", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Debug { get; set; }
        [MetaProperty("PracticeToolTotal", BinPropertyType.ObjectLink)]
        public MetaObjectLink? PracticeToolTotal { get; set; }
        [MetaProperty("PracticeToolLastHit", BinPropertyType.ObjectLink)]
        public MetaObjectLink? PracticeToolLastHit { get; set; }
        [MetaProperty("PracticeToolDPS", BinPropertyType.ObjectLink)]
        public MetaObjectLink? PracticeToolDPS { get; set; }
        [MetaProperty("ScoreDarkStar", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ScoreDarkStar { get; set; }
        [MetaProperty("ScoreProject0", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ScoreProject0 { get; set; }
        [MetaProperty("ScoreProject1", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ScoreProject1 { get; set; }
        [MetaProperty("ShieldBonusDamage", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ShieldBonusDamage { get; set; }
        [MetaProperty(897531011, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m897531011 { get; set; }
    }
    [MetaClass("FloatingTextGlobalConfig")]
    public class FloatingTextGlobalConfig : IMetaClass
    {
        [MetaProperty("mTunables", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatingTextTunables> Tunables { get; set; }
        [MetaProperty("mDamageDisplayTypes", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatingTextDamageDisplayTypeList> DamageDisplayTypes { get; set; }
        [MetaProperty("mFloatingTextTypes", BinPropertyType.Embedded)]
        public MetaEmbedded<FloatingTextTypeList> FloatingTextTypes { get; set; }
    }
    [MetaClass("FloatingTextOverride")]
    public class FloatingTextOverride : IMetaClass
    {
        [MetaProperty(1397360953, BinPropertyType.Map)]
        public Dictionary<uint, bool> m1397360953 { get; set; }
    }
    [MetaClass("HudColorData")]
    public class HudColorData : IMetaClass
    {
        [MetaProperty("mSelfColor", BinPropertyType.Color)]
        public Color? SelfColor { get; set; }
        [MetaProperty("mFriendlyColor", BinPropertyType.Color)]
        public Color? FriendlyColor { get; set; }
        [MetaProperty("mEnemyColor", BinPropertyType.Color)]
        public Color? EnemyColor { get; set; }
        [MetaProperty("mNeutralColor", BinPropertyType.Color)]
        public Color? NeutralColor { get; set; }
        [MetaProperty("mOrderColor", BinPropertyType.Color)]
        public Color? OrderColor { get; set; }
        [MetaProperty("mChaosColor", BinPropertyType.Color)]
        public Color? ChaosColor { get; set; }
        [MetaProperty("mJunglePlantColor", BinPropertyType.Color)]
        public Color? JunglePlantColor { get; set; }
        [MetaProperty("mFriendlyLaneMinionBarColor", BinPropertyType.Color)]
        public Color? FriendlyLaneMinionBarColor { get; set; }
        [MetaProperty("mEnemyLaneMinionBarColor", BinPropertyType.Color)]
        public Color? EnemyLaneMinionBarColor { get; set; }
        [MetaProperty("mDeathFriendlyTeamColor", BinPropertyType.Color)]
        public Color? DeathFriendlyTeamColor { get; set; }
        [MetaProperty("mDeathEnemyTeamColor", BinPropertyType.Color)]
        public Color? DeathEnemyTeamColor { get; set; }
        [MetaProperty("mDeathOrderColor", BinPropertyType.Color)]
        public Color? DeathOrderColor { get; set; }
        [MetaProperty("mDeathChaosColor", BinPropertyType.Color)]
        public Color? DeathChaosColor { get; set; }
        [MetaProperty("mInputChatColor", BinPropertyType.Color)]
        public Color? InputChatColor { get; set; }
        [MetaProperty("mShadowChatColor", BinPropertyType.Color)]
        public Color? ShadowChatColor { get; set; }
        [MetaProperty("mFriendlyChatColor", BinPropertyType.Color)]
        public Color? FriendlyChatColor { get; set; }
        [MetaProperty("mAllChatColor", BinPropertyType.Color)]
        public Color? AllChatColor { get; set; }
        [MetaProperty("mEnemyChatColor", BinPropertyType.Color)]
        public Color? EnemyChatColor { get; set; }
        [MetaProperty("mNeutralChatColor", BinPropertyType.Color)]
        public Color? NeutralChatColor { get; set; }
        [MetaProperty("mPingChatColor", BinPropertyType.Color)]
        public Color? PingChatColor { get; set; }
        [MetaProperty("mTeamChatColor", BinPropertyType.Color)]
        public Color? TeamChatColor { get; set; }
        [MetaProperty("mNetworkChatColor", BinPropertyType.Color)]
        public Color? NetworkChatColor { get; set; }
        [MetaProperty("mOrderChatColor", BinPropertyType.Color)]
        public Color? OrderChatColor { get; set; }
        [MetaProperty("mChaosChatColor", BinPropertyType.Color)]
        public Color? ChaosChatColor { get; set; }
        [MetaProperty("mGoldChatColor", BinPropertyType.Color)]
        public Color? GoldChatColor { get; set; }
        [MetaProperty("mTimestampChatColor", BinPropertyType.Color)]
        public Color? TimestampChatColor { get; set; }
        [MetaProperty("mWhisperColor", BinPropertyType.Color)]
        public Color? WhisperColor { get; set; }
        [MetaProperty("mPartyChatColor", BinPropertyType.Color)]
        public Color? PartyChatColor { get; set; }
        [MetaProperty("mPlatformChatColor", BinPropertyType.Color)]
        public Color? PlatformChatColor { get; set; }
        [MetaProperty("mFeedbackChatColor", BinPropertyType.Color)]
        public Color? FeedbackChatColor { get; set; }
        [MetaProperty("mItemCalloutBodyColor", BinPropertyType.Color)]
        public Color? ItemCalloutBodyColor { get; set; }
        [MetaProperty("mItemCalloutItemColor", BinPropertyType.Color)]
        public Color? ItemCalloutItemColor { get; set; }
        [MetaProperty("mMarkedIndicatorColor", BinPropertyType.Color)]
        public Color? MarkedIndicatorColor { get; set; }
        [MetaProperty("mSelectedIndicatorColor", BinPropertyType.Color)]
        public Color? SelectedIndicatorColor { get; set; }
        [MetaProperty(2673467485, BinPropertyType.Color)]
        public Color? m2673467485 { get; set; }
        [MetaProperty("mItemHotKeyEnabledColor", BinPropertyType.Color)]
        public Color? ItemHotKeyEnabledColor { get; set; }
        [MetaProperty("mItemHotKeyDisabledColor", BinPropertyType.Color)]
        public Color? ItemHotKeyDisabledColor { get; set; }
        [MetaProperty("mSpellHotKeyEnabledColor", BinPropertyType.Color)]
        public Color? SpellHotKeyEnabledColor { get; set; }
        [MetaProperty("mSpellHotKeyDisabledColor", BinPropertyType.Color)]
        public Color? SpellHotKeyDisabledColor { get; set; }
        [MetaProperty("mStatNormalColor", BinPropertyType.Color)]
        public Color? StatNormalColor { get; set; }
        [MetaProperty("mStatBoostedColor", BinPropertyType.Color)]
        public Color? StatBoostedColor { get; set; }
        [MetaProperty("mLevelUpColor", BinPropertyType.Color)]
        public Color? LevelUpColor { get; set; }
        [MetaProperty("mEvolutionColor", BinPropertyType.Color)]
        public Color? EvolutionColor { get; set; }
        [MetaProperty("mSummonerNameDeadColor", BinPropertyType.Color)]
        public Color? SummonerNameDeadColor { get; set; }
        [MetaProperty("mSummonerNameDefaultColor", BinPropertyType.Color)]
        public Color? SummonerNameDefaultColor { get; set; }
        [MetaProperty("mSummonerNameSelfColor", BinPropertyType.Color)]
        public Color? SummonerNameSelfColor { get; set; }
        [MetaProperty("mClubTagFriendlyChatColor", BinPropertyType.Color)]
        public Color? ClubTagFriendlyChatColor { get; set; }
        [MetaProperty("mClubTagEnemyChatColor", BinPropertyType.Color)]
        public Color? ClubTagEnemyChatColor { get; set; }
        [MetaProperty("mClubTagNeutralChatColor", BinPropertyType.Color)]
        public Color? ClubTagNeutralChatColor { get; set; }
        [MetaProperty("mClubTagOrderChatColor", BinPropertyType.Color)]
        public Color? ClubTagOrderChatColor { get; set; }
        [MetaProperty("mClubTagChaosChatColor", BinPropertyType.Color)]
        public Color? ClubTagChaosChatColor { get; set; }
        [MetaProperty("mVoiceChatDefaultTextColor", BinPropertyType.Color)]
        public Color? VoiceChatDefaultTextColor { get; set; }
        [MetaProperty("mVoiceChatHoverTextColor", BinPropertyType.Color)]
        public Color? VoiceChatHoverTextColor { get; set; }
        [MetaProperty("mVoiceChatHaloTextureColor", BinPropertyType.Color)]
        public Color? VoiceChatHaloTextureColor { get; set; }
        [MetaProperty(63589972, BinPropertyType.Color)]
        public Color? m63589972 { get; set; }
        [MetaProperty(3755603872, BinPropertyType.Color)]
        public Color? m3755603872 { get; set; }
        [MetaProperty(3591678551, BinPropertyType.Color)]
        public Color? m3591678551 { get; set; }
        [MetaProperty(1139757985, BinPropertyType.Color)]
        public Color? m1139757985 { get; set; }
        [MetaProperty(1371618007, BinPropertyType.Color)]
        public Color? m1371618007 { get; set; }
        [MetaProperty(3154015837, BinPropertyType.Color)]
        public Color? m3154015837 { get; set; }
        [MetaProperty(3243078108, BinPropertyType.Color)]
        public Color? m3243078108 { get; set; }
        [MetaProperty(3449599685, BinPropertyType.Color)]
        public Color? m3449599685 { get; set; }
        [MetaProperty(1063841720, BinPropertyType.Color)]
        public Color? m1063841720 { get; set; }
        [MetaProperty(2491908235, BinPropertyType.Color)]
        public Color? m2491908235 { get; set; }
        [MetaProperty(264529986, BinPropertyType.Color)]
        public Color? m264529986 { get; set; }
        [MetaProperty(996466659, BinPropertyType.Color)]
        public Color? m996466659 { get; set; }
        [MetaProperty(1730989398, BinPropertyType.Color)]
        public Color? m1730989398 { get; set; }
        [MetaProperty(2467755617, BinPropertyType.Container)]
        public MetaContainer<Color> m2467755617 { get; set; }
        [MetaProperty(4003080326, BinPropertyType.Color)]
        public Color? m4003080326 { get; set; }
        [MetaProperty(619402659, BinPropertyType.Color)]
        public Color? m619402659 { get; set; }
        [MetaProperty(788705121, BinPropertyType.Color)]
        public Color? m788705121 { get; set; }
        [MetaProperty(3390937202, BinPropertyType.Color)]
        public Color? m3390937202 { get; set; }
    }
    [MetaClass("HudFeedbackDamageData")]
    public class HudFeedbackDamageData : IMetaClass
    {
        [MetaProperty("mPercentageDamageForFlash", BinPropertyType.Float)]
        public float? PercentageDamageForFlash { get; set; }
        [MetaProperty("mOverTimeForFlashSeconds", BinPropertyType.Float)]
        public float? OverTimeForFlashSeconds { get; set; }
        [MetaProperty("mMaxPercentageForMostReadHealth", BinPropertyType.Float)]
        public float? MaxPercentageForMostReadHealth { get; set; }
        [MetaProperty("mFlashDuration", BinPropertyType.Float)]
        public float? FlashDuration { get; set; }
        [MetaProperty("mStartFlashAlpha", BinPropertyType.Float)]
        public float? StartFlashAlpha { get; set; }
        [MetaProperty("mLowHealthFlashThresholdPercentage", BinPropertyType.Float)]
        public float? LowHealthFlashThresholdPercentage { get; set; }
        [MetaProperty("mLowHealthFlashDuration", BinPropertyType.Float)]
        public float? LowHealthFlashDuration { get; set; }
        [MetaProperty("mLowHealthFlashOpacityStrength", BinPropertyType.Float)]
        public float? LowHealthFlashOpacityStrength { get; set; }
    }
    [MetaClass("TeamScoreMeterUITunables")]
    public class TeamScoreMeterUITunables : IMetaClass
    {
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; }
        [MetaProperty(2975799563, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudTeamScoreMeterProperties>> m2975799563 { get; set; }
        [MetaProperty("mTeamScoreMeterMaxRoundsPerTeam", BinPropertyType.UInt32)]
        public uint? TeamScoreMeterMaxRoundsPerTeam { get; set; }
        [MetaProperty("mAllowDynamicVisibility", BinPropertyType.Bool)]
        public bool? AllowDynamicVisibility { get; set; }
        [MetaProperty("mCountdownTimer", BinPropertyType.Bool)]
        public bool? CountdownTimer { get; set; }
        [MetaProperty(3757209935, BinPropertyType.Byte)]
        public byte? m3757209935 { get; set; }
    }
    [MetaClass("HudTeamScoreMeterProperties")]
    public class HudTeamScoreMeterProperties : IMetaClass
    {
        [MetaProperty("mTeamScoreMeterType", BinPropertyType.Byte)]
        public byte? TeamScoreMeterType { get; set; }
        [MetaProperty(3397650234, BinPropertyType.Bool)]
        public bool? m3397650234 { get; set; }
        [MetaProperty(2256949180, BinPropertyType.String)]
        public string? m2256949180 { get; set; }
        [MetaProperty(1615112080, BinPropertyType.String)]
        public string? m1615112080 { get; set; }
    }
    [MetaClass("EncounterUITunables")]
    public class EncounterUITunables : IMetaClass
    {
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; }
        [MetaProperty("mProgressBarEaseRate", BinPropertyType.Float)]
        public float? ProgressBarEaseRate { get; set; }
        [MetaProperty("mProgressMeterSuffix", BinPropertyType.String)]
        public string? ProgressMeterSuffix { get; set; }
        [MetaProperty(3286494379, BinPropertyType.String)]
        public string? m3286494379 { get; set; }
        [MetaProperty("mUnitBarFadeSpeed", BinPropertyType.Float)]
        public float? UnitBarFadeSpeed { get; set; }
        [MetaProperty(1070132460, BinPropertyType.Bool)]
        public bool? m1070132460 { get; set; }
        [MetaProperty(3428022377, BinPropertyType.String)]
        public string? m3428022377 { get; set; }
        [MetaProperty(1663411567, BinPropertyType.String)]
        public string? m1663411567 { get; set; }
        [MetaProperty("mPipsHoverText", BinPropertyType.String)]
        public string? PipsHoverText { get; set; }
    }
    [MetaClass("QuestUITunables")]
    public class QuestUITunables : IMetaClass
    {
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; }
    }
    [MetaClass("DragonUITunables")]
    public class DragonUITunables : IMetaClass
    {
        [MetaProperty("mSlots", BinPropertyType.Byte)]
        public byte? Slots { get; set; }
        [MetaProperty(314664954, BinPropertyType.Container)]
        public MetaContainer<string> m314664954 { get; set; }
    }
    [MetaClass("HudGameModeScoreData")]
    public class HudGameModeScoreData : IMetaClass
    {
        [MetaProperty("mTeamScoreElementTypes", BinPropertyType.Container)]
        public MetaContainer<byte> TeamScoreElementTypes { get; set; }
        [MetaProperty("mIndividualScoreElementTypes", BinPropertyType.Container)]
        public MetaContainer<byte> IndividualScoreElementTypes { get; set; }
        [MetaProperty("mModeKeyName", BinPropertyType.String)]
        public string? ModeKeyName { get; set; }
        [MetaProperty("mTeamGameScorePingMessage", BinPropertyType.String)]
        public string? TeamGameScorePingMessage { get; set; }
        [MetaProperty(2321304744, BinPropertyType.Structure)]
        public TeamScoreMeterUITunables m2321304744 { get; set; }
        [MetaProperty(527990547, BinPropertyType.Structure)]
        public EncounterUITunables m527990547 { get; set; }
        [MetaProperty(618622443, BinPropertyType.Structure)]
        public HudTeamFightData m618622443 { get; set; }
        [MetaProperty(2770289004, BinPropertyType.Structure)]
        public QuestUITunables m2770289004 { get; set; }
        [MetaProperty(157616145, BinPropertyType.Structure)]
        public DragonUITunables m157616145 { get; set; }
        [MetaProperty(936257914, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudOptionalBinData>> m936257914 { get; set; }
    }
    [MetaClass("MinimapIconTextureData")]
    public class MinimapIconTextureData : IMetaClass
    {
        [MetaProperty("mBase", BinPropertyType.String)]
        public string? Base { get; set; }
        [MetaProperty("mColorblind", BinPropertyType.Optional)]
        public MetaOptional<string> Colorblind { get; set; }
    }
    [MetaClass("MinimapIconColorData")]
    public class MinimapIconColorData : IMetaClass
    {
        [MetaProperty("mBase", BinPropertyType.Color)]
        public Color? Base { get; set; }
        [MetaProperty("mColorblind", BinPropertyType.Optional)]
        public MetaOptional<Color> Colorblind { get; set; }
    }
    [MetaClass("MinimapIcon")]
    public class MinimapIcon : IMetaClass
    {
        [MetaProperty("mRelativeTeams", BinPropertyType.Bool)]
        public bool? RelativeTeams { get; set; }
        [MetaProperty("mSize", BinPropertyType.Vector2)]
        public Vector2? Size { get; set; }
        [MetaProperty("mMinScale", BinPropertyType.Float)]
        public float? MinScale { get; set; }
        [MetaProperty("mMaxScale", BinPropertyType.Float)]
        public float? MaxScale { get; set; }
        [MetaProperty("mBaseTexture", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapIconTextureData> BaseTexture { get; set; }
        [MetaProperty("mTeamTextures", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<MinimapIconTextureData>> TeamTextures { get; set; }
        [MetaProperty("mBaseColor", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapIconColorData> BaseColor { get; set; }
        [MetaProperty("mTeamColors", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<MinimapIconColorData>> TeamColors { get; set; }
    }
    [MetaClass("MinimapData")]
    public class MinimapData : IMetaClass
    {
        [MetaProperty("mIcons", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<MinimapIcon>> Icons { get; set; }
        [MetaProperty("mCustomIcons", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<MinimapIcon>> CustomIcons { get; set; }
    }
    [MetaClass("HudOptionalBinData")]
    public class HudOptionalBinData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mPriority", BinPropertyType.UInt32)]
        public uint? Priority { get; set; }
    }
    [MetaClass("HudTunables")]
    public class HudTunables : IMetaClass
    {
        [MetaProperty("mScaleSettings", BinPropertyType.Embedded)]
        public MetaEmbedded<HudScaleSettingsData> ScaleSettings { get; set; }
        [MetaProperty("mLevelUpTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudLevelUpData> LevelUpTransitionData { get; set; }
        [MetaProperty("mGameScoreboardTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> GameScoreboardTransitionData { get; set; }
        [MetaProperty("mReplayScoreboardTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> ReplayScoreboardTransitionData { get; set; }
        [MetaProperty("mReplayGameStatsTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> ReplayGameStatsTransitionData { get; set; }
        [MetaProperty("mReplayTeamFramesTransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> ReplayTeamFramesTransitionData { get; set; }
        [MetaProperty(2632753136, BinPropertyType.Embedded)]
        public MetaEmbedded<HudBannerData> m2632753136 { get; set; }
        [MetaProperty(1316827209, BinPropertyType.Embedded)]
        public MetaEmbedded<HudAbilityPromptData> m1316827209 { get; set; }
        [MetaProperty("mElementalSelectionAnimationData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudElementalSectionUIData> ElementalSelectionAnimationData { get; set; }
        [MetaProperty("mEmotePopupData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudEmotePopupData> EmotePopupData { get; set; }
        [MetaProperty("mGearSelectionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudGearSelectionUIData> GearSelectionData { get; set; }
        [MetaProperty("mRadailWheelData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudRadialWheelData> RadailWheelData { get; set; }
        [MetaProperty("mReplayData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudReplayData> ReplayData { get; set; }
        [MetaProperty("mCheatMenuData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudCheatMenuData> CheatMenuData { get; set; }
        [MetaProperty("mPingData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudPingData> PingData { get; set; }
        [MetaProperty("mChatData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudChatData> ChatData { get; set; }
        [MetaProperty("mVoiceChatData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudVoiceChatData> VoiceChatData { get; set; }
        [MetaProperty("mInputBoxData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudInputBoxData> InputBoxData { get; set; }
        [MetaProperty("mHealthBarData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarData> HealthBarData { get; set; }
        [MetaProperty("mHudSpellSlotResetFeedbackData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudSpellSlotResetFeedbackData> HudSpellSlotResetFeedbackData { get; set; }
        [MetaProperty("mLoadingScreenData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudLoadingScreenData> LoadingScreenData { get; set; }
        [MetaProperty(2076079679, BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatPanelStatStoneData> m2076079679 { get; set; }
        [MetaProperty("mStatStoneData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatStoneData> StatStoneData { get; set; }
        [MetaProperty(3128315546, BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatStoneDeathRecapData> m3128315546 { get; set; }
        [MetaProperty(106443622, BinPropertyType.Embedded)]
        public MetaEmbedded<HudStatStoneMilestoneData> m106443622 { get; set; }
        [MetaProperty(3266966018, BinPropertyType.Embedded)]
        public MetaEmbedded<HudEndOfGameData> m3266966018 { get; set; }
        [MetaProperty(2252352223, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1269294773> m2252352223 { get; set; }
        [MetaProperty(1221021762, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1269294773> m1221021762 { get; set; }
        [MetaProperty(1116989102, BinPropertyType.Embedded)]
        public MetaEmbedded<HudDamageDisplayData> m1116989102 { get; set; }
        [MetaProperty(3882694614, BinPropertyType.Embedded)]
        public MetaEmbedded<HudFightRecapUIData> m3882694614 { get; set; }
        [MetaProperty(4223985568, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudWindowPlacementData>> m4223985568 { get; set; }
    }
    [MetaClass("HudScaleSettingsData")]
    public class HudScaleSettingsData : IMetaClass
    {
        [MetaProperty("maximumGlobalScale", BinPropertyType.Float)]
        public float? MaximumGlobalScale { get; set; }
        [MetaProperty("minimumGlobalScale", BinPropertyType.Float)]
        public float? MinimumGlobalScale { get; set; }
        [MetaProperty("maximumMinimapScale", BinPropertyType.Float)]
        public float? MaximumMinimapScale { get; set; }
        [MetaProperty("minimumMinimapScale", BinPropertyType.Float)]
        public float? MinimumMinimapScale { get; set; }
        [MetaProperty(1804113590, BinPropertyType.Float)]
        public float? m1804113590 { get; set; }
        [MetaProperty(3043348288, BinPropertyType.Float)]
        public float? m3043348288 { get; set; }
        [MetaProperty("maximumPracticeToolScale", BinPropertyType.Float)]
        public float? MaximumPracticeToolScale { get; set; }
        [MetaProperty("minimumPracticeToolScale", BinPropertyType.Float)]
        public float? MinimumPracticeToolScale { get; set; }
        [MetaProperty("maximumChatScale", BinPropertyType.Float)]
        public float? MaximumChatScale { get; set; }
        [MetaProperty("minimumChatScale", BinPropertyType.Float)]
        public float? MinimumChatScale { get; set; }
    }
    [MetaClass("HudLevelUpData")]
    public class HudLevelUpData : IMetaClass
    {
        [MetaProperty("minAlpha", BinPropertyType.Byte)]
        public byte? MinAlpha { get; set; }
        [MetaProperty("maxAlpha", BinPropertyType.Byte)]
        public byte? MaxAlpha { get; set; }
        [MetaProperty("maxOffset", BinPropertyType.Float)]
        public float? MaxOffset { get; set; }
        [MetaProperty("animTime", BinPropertyType.Float)]
        public float? AnimTime { get; set; }
        [MetaProperty("delay", BinPropertyType.Float)]
        public float? Delay { get; set; }
        [MetaProperty("overshoot", BinPropertyType.Float)]
        public float? Overshoot { get; set; }
        [MetaProperty("inertia", BinPropertyType.Float)]
        public float? Inertia { get; set; }
        [MetaProperty("playerBuffsOffset", BinPropertyType.Float)]
        public float? PlayerBuffsOffset { get; set; }
        [MetaProperty("idleSheenInterval", BinPropertyType.Float)]
        public float? IdleSheenInterval { get; set; }
    }
    [MetaClass("HudBannerData")]
    public class HudBannerData : IMetaClass
    {
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float? TransitionTime { get; set; }
        [MetaProperty(2449524942, BinPropertyType.Byte)]
        public byte? m2449524942 { get; set; }
        [MetaProperty(2960635160, BinPropertyType.Byte)]
        public byte? m2960635160 { get; set; }
        [MetaProperty("transitionOffset", BinPropertyType.Vector2)]
        public Vector2? TransitionOffset { get; set; }
        [MetaProperty(4045513993, BinPropertyType.Vector2)]
        public Vector2? m4045513993 { get; set; }
        [MetaProperty(1284438647, BinPropertyType.Float)]
        public float? m1284438647 { get; set; }
        [MetaProperty(229098546, BinPropertyType.Float)]
        public float? m229098546 { get; set; }
        [MetaProperty(427443163, BinPropertyType.Float)]
        public float? m427443163 { get; set; }
    }
    [MetaClass("HudAbilityPromptData")]
    public class HudAbilityPromptData : IMetaClass
    {
        [MetaProperty(3136505745, BinPropertyType.Color)]
        public Color? m3136505745 { get; set; }
        [MetaProperty(1649820186, BinPropertyType.Color)]
        public Color? m1649820186 { get; set; }
        [MetaProperty(4045513993, BinPropertyType.Vector2)]
        public Vector2? m4045513993 { get; set; }
        [MetaProperty(1284438647, BinPropertyType.Float)]
        public float? m1284438647 { get; set; }
        [MetaProperty(427443163, BinPropertyType.Float)]
        public float? m427443163 { get; set; }
    }
    [MetaClass("HudMenuTransitionData")]
    public class HudMenuTransitionData : IMetaClass
    {
        [MetaProperty("transitionTime", BinPropertyType.Float)]
        public float? TransitionTime { get; set; }
        [MetaProperty("minAlpha", BinPropertyType.Byte)]
        public byte? MinAlpha { get; set; }
        [MetaProperty("maxAlpha", BinPropertyType.Byte)]
        public byte? MaxAlpha { get; set; }
        [MetaProperty("EasingType", BinPropertyType.UInt32)]
        public uint? EasingType { get; set; }
    }
    [MetaClass("HudElementalSectionUIData")]
    public class HudElementalSectionUIData : IMetaClass
    {
        [MetaProperty("airColoration", BinPropertyType.Color)]
        public Color? AirColoration { get; set; }
        [MetaProperty("fireColoration", BinPropertyType.Color)]
        public Color? FireColoration { get; set; }
        [MetaProperty("earthColoration", BinPropertyType.Color)]
        public Color? EarthColoration { get; set; }
        [MetaProperty("waterColoration", BinPropertyType.Color)]
        public Color? WaterColoration { get; set; }
        [MetaProperty("darkColoration", BinPropertyType.Color)]
        public Color? DarkColoration { get; set; }
        [MetaProperty("fairyColoration", BinPropertyType.Color)]
        public Color? FairyColoration { get; set; }
        [MetaProperty("iceColoration", BinPropertyType.Color)]
        public Color? IceColoration { get; set; }
        [MetaProperty("lightColoration", BinPropertyType.Color)]
        public Color? LightColoration { get; set; }
        [MetaProperty("magmaColoration", BinPropertyType.Color)]
        public Color? MagmaColoration { get; set; }
        [MetaProperty("stormColoration", BinPropertyType.Color)]
        public Color? StormColoration { get; set; }
        [MetaProperty("meterFilledButtonFadeInDelay", BinPropertyType.Float)]
        public float? MeterFilledButtonFadeInDelay { get; set; }
        [MetaProperty("firstSelectionAnimationDelay", BinPropertyType.Float)]
        public float? FirstSelectionAnimationDelay { get; set; }
        [MetaProperty("secondSelectionAnimationDelay", BinPropertyType.Float)]
        public float? SecondSelectionAnimationDelay { get; set; }
        [MetaProperty("glowingRingCycleTime", BinPropertyType.Float)]
        public float? GlowingRingCycleTime { get; set; }
    }
    [MetaClass("HudEmotePopupData")]
    public class HudEmotePopupData : IMetaClass
    {
        [MetaProperty("mEmotePopupTransitionIntro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> EmotePopupTransitionIntro { get; set; }
        [MetaProperty("mEmotePopupTransitionOutro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> EmotePopupTransitionOutro { get; set; }
        [MetaProperty(1567183921, BinPropertyType.String)]
        public string? m1567183921 { get; set; }
        [MetaProperty("mEmotePopupDisplayTime", BinPropertyType.Float)]
        public float? EmotePopupDisplayTime { get; set; }
        [MetaProperty(1921880927, BinPropertyType.Float)]
        public float? m1921880927 { get; set; }
        [MetaProperty(130878851, BinPropertyType.Float)]
        public float? m130878851 { get; set; }
        [MetaProperty(1976912936, BinPropertyType.Float)]
        public float? m1976912936 { get; set; }
        [MetaProperty(4135630809, BinPropertyType.Float)]
        public float? m4135630809 { get; set; }
        [MetaProperty(1809279107, BinPropertyType.Float)]
        public float? m1809279107 { get; set; }
        [MetaProperty("mEmoteFloatEnabled", BinPropertyType.Bool)]
        public bool? EmoteFloatEnabled { get; set; }
    }
    [MetaClass("HudGearSelectionUIData")]
    public class HudGearSelectionUIData : IMetaClass
    {
        [MetaProperty("mGearSelectionTransitionIntro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> GearSelectionTransitionIntro { get; set; }
        [MetaProperty("mGearSelectionTransitionOutro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> GearSelectionTransitionOutro { get; set; }
        [MetaProperty("selectionButtonDelayTime", BinPropertyType.Float)]
        public float? SelectionButtonDelayTime { get; set; }
        [MetaProperty("timerEnabled", BinPropertyType.Bool)]
        public bool? TimerEnabled { get; set; }
        [MetaProperty("timerCountdownDuration", BinPropertyType.Float)]
        public float? TimerCountdownDuration { get; set; }
        [MetaProperty("timerCountdownWarningStart", BinPropertyType.Float)]
        public float? TimerCountdownWarningStart { get; set; }
    }
    [MetaClass("HudRadialWheelData")]
    public class HudRadialWheelData : IMetaClass
    {
        [MetaProperty("mRadialWheelUITransitionData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> RadialWheelUITransitionData { get; set; }
        [MetaProperty("mRadialWheelButtonTransitionIntro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> RadialWheelButtonTransitionIntro { get; set; }
        [MetaProperty("mRadialWheelButtonTransitionOutro", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> RadialWheelButtonTransitionOutro { get; set; }
        [MetaProperty("activateWheelDelayTime", BinPropertyType.Float)]
        public float? ActivateWheelDelayTime { get; set; }
    }
    [MetaClass("HudTeamFightOffScreenDifferentiationData")]
    public class HudTeamFightOffScreenDifferentiationData : IMetaClass
    {
        [MetaProperty(1088652879, BinPropertyType.Float)]
        public float? m1088652879 { get; set; }
        [MetaProperty(1057185245, BinPropertyType.Float)]
        public float? m1057185245 { get; set; }
        [MetaProperty(587753271, BinPropertyType.Float)]
        public float? m587753271 { get; set; }
    }
    [MetaClass("HudTeamFightData")]
    public class HudTeamFightData : IMetaClass
    {
        [MetaProperty("mSceneTransition", BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> SceneTransition { get; set; }
        [MetaProperty(529870772, BinPropertyType.UInt32)]
        public uint? m529870772 { get; set; }
        [MetaProperty(2808220806, BinPropertyType.Structure)]
        public HudTeamFightOffScreenDifferentiationData m2808220806 { get; set; }
    }
    [MetaClass("HudReplayData")]
    public class HudReplayData : IMetaClass
    {
        [MetaProperty("messageVisibleTime", BinPropertyType.Float)]
        public float? MessageVisibleTime { get; set; }
        [MetaProperty(3478055546, BinPropertyType.Embedded)]
        public MetaEmbedded<HudTeamFightData> m3478055546 { get; set; }
    }
    [MetaClass("HudCheatMenuData")]
    public class HudCheatMenuData : IMetaClass
    {
        [MetaProperty("tooltipDelay", BinPropertyType.Float)]
        public float? TooltipDelay { get; set; }
        [MetaProperty("groupDividerGapSize", BinPropertyType.Float)]
        public float? GroupDividerGapSize { get; set; }
        [MetaProperty("groupDividerIndex", BinPropertyType.Byte)]
        public byte? GroupDividerIndex { get; set; }
    }
    [MetaClass("HudPingData")]
    public class HudPingData : IMetaClass
    {
        [MetaProperty("distanceToNotTrollPingCorpses", BinPropertyType.Float)]
        public float? DistanceToNotTrollPingCorpses { get; set; }
        [MetaProperty("timeToNotTrollPingCorpses", BinPropertyType.Float)]
        public float? TimeToNotTrollPingCorpses { get; set; }
    }
    [MetaClass("HudChatData")]
    public class HudChatData : IMetaClass
    {
        [MetaProperty("defaultWordWrapMargin", BinPropertyType.Byte)]
        public byte? DefaultWordWrapMargin { get; set; }
        [MetaProperty("hideAfterSeconds", BinPropertyType.Float)]
        public float? HideAfterSeconds { get; set; }
    }
    [MetaClass("HudVoiceChatData")]
    public class HudVoiceChatData : IMetaClass
    {
        [MetaProperty("highlightTimeoutSeconds", BinPropertyType.Float)]
        public float? HighlightTimeoutSeconds { get; set; }
    }
    [MetaClass("HudInputBoxData")]
    public class HudInputBoxData : IMetaClass
    {
        [MetaProperty("inputTextLengthMax", BinPropertyType.Byte)]
        public byte? InputTextLengthMax { get; set; }
        [MetaProperty("caretAlphaMax", BinPropertyType.Float)]
        public float? CaretAlphaMax { get; set; }
        [MetaProperty("caretBlinkTime", BinPropertyType.Float)]
        public float? CaretBlinkTime { get; set; }
        [MetaProperty("markedOffsetY", BinPropertyType.Float)]
        public float? MarkedOffsetY { get; set; }
        [MetaProperty("markedLineSizePx", BinPropertyType.Float)]
        public float? MarkedLineSizePx { get; set; }
        [MetaProperty("selectedOffsetY", BinPropertyType.Float)]
        public float? SelectedOffsetY { get; set; }
        [MetaProperty("selectedLineSizePx", BinPropertyType.Float)]
        public float? SelectedLineSizePx { get; set; }
        [MetaProperty(3198939864, BinPropertyType.Float)]
        public float? m3198939864 { get; set; }
    }
    [MetaClass("HudHealthBarBurstData")]
    public class HudHealthBarBurstData : IMetaClass
    {
        [MetaProperty("burstTimeWindow", BinPropertyType.Float)]
        public float? BurstTimeWindow { get; set; }
        [MetaProperty("burstTriggerPercent", BinPropertyType.Float)]
        public float? BurstTriggerPercent { get; set; }
        [MetaProperty("flashTriggerPercent", BinPropertyType.Float)]
        public float? FlashTriggerPercent { get; set; }
        [MetaProperty("flashDuration", BinPropertyType.Float)]
        public float? FlashDuration { get; set; }
        [MetaProperty("shakeDuration", BinPropertyType.Float)]
        public float? ShakeDuration { get; set; }
        [MetaProperty("shakeBox", BinPropertyType.Vector2)]
        public Vector2? ShakeBox { get; set; }
        [MetaProperty("shakeReferenceResolution", BinPropertyType.Vector2)]
        public Vector2? ShakeReferenceResolution { get; set; }
        [MetaProperty("shakeTriggerPercent", BinPropertyType.Float)]
        public float? ShakeTriggerPercent { get; set; }
        [MetaProperty("shakeFrequency", BinPropertyType.Float)]
        public float? ShakeFrequency { get; set; }
        [MetaProperty("fadeSpeed", BinPropertyType.Float)]
        public float? FadeSpeed { get; set; }
        [MetaProperty("fadeAcceleration", BinPropertyType.Float)]
        public float? FadeAcceleration { get; set; }
        [MetaProperty("fadeHoldTime", BinPropertyType.Float)]
        public float? FadeHoldTime { get; set; }
    }
    [MetaClass("HudHealthBarFadeData")]
    public class HudHealthBarFadeData : IMetaClass
    {
        [MetaProperty("fadeSpeed", BinPropertyType.Float)]
        public float? FadeSpeed { get; set; }
        [MetaProperty("fadeAcceleration", BinPropertyType.Float)]
        public float? FadeAcceleration { get; set; }
        [MetaProperty("fadeHoldTime", BinPropertyType.Float)]
        public float? FadeHoldTime { get; set; }
    }
    [MetaClass("HudHealthBarDefenseModifierData")]
    public class HudHealthBarDefenseModifierData : IMetaClass
    {
        [MetaProperty("defenseUpPercent", BinPropertyType.Float)]
        public float? DefenseUpPercent { get; set; }
        [MetaProperty("defenseDownL1Percent", BinPropertyType.Float)]
        public float? DefenseDownL1Percent { get; set; }
        [MetaProperty("defenseDownL2Percent", BinPropertyType.Float)]
        public float? DefenseDownL2Percent { get; set; }
        [MetaProperty("defenseDownL3Percent", BinPropertyType.Float)]
        public float? DefenseDownL3Percent { get; set; }
    }
    [MetaClass("HudHealthBarBurstHealData")]
    public class HudHealthBarBurstHealData : IMetaClass
    {
        [MetaProperty("healTimeWindow", BinPropertyType.Float)]
        public float? HealTimeWindow { get; set; }
        [MetaProperty("healTriggerPercent", BinPropertyType.Float)]
        public float? HealTriggerPercent { get; set; }
        [MetaProperty("healFadeDuration", BinPropertyType.Float)]
        public float? HealFadeDuration { get; set; }
    }
    [MetaClass("HudHealthBarDefenseIconData")]
    public class HudHealthBarDefenseIconData : IMetaClass
    {
        [MetaProperty("enlargeTime", BinPropertyType.Float)]
        public float? EnlargeTime { get; set; }
        [MetaProperty("enlargeSize", BinPropertyType.Float)]
        public float? EnlargeSize { get; set; }
        [MetaProperty("settleTime", BinPropertyType.Float)]
        public float? SettleTime { get; set; }
    }
    [MetaClass("MicroTicksPerTickData")]
    public class MicroTicksPerTickData : IMetaClass
    {
        [MetaProperty("minHealth", BinPropertyType.Float)]
        public float? MinHealth { get; set; }
        [MetaProperty("microTicksBetween", BinPropertyType.UInt32)]
        public uint? MicroTicksBetween { get; set; }
    }
    [MetaClass("HudHealthBarProgressiveTickData")]
    public class HudHealthBarProgressiveTickData : IMetaClass
    {
        [MetaProperty("microTickPerTickData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MicroTicksPerTickData>> MicroTickPerTickData { get; set; }
        [MetaProperty("healthPerTick", BinPropertyType.Float)]
        public float? HealthPerTick { get; set; }
    }
    [MetaClass("HudHealthBarData")]
    public class HudHealthBarData : IMetaClass
    {
        [MetaProperty("burstData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarBurstData> BurstData { get; set; }
        [MetaProperty("towerBurstData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarBurstData> TowerBurstData { get; set; }
        [MetaProperty("burstHealData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarBurstHealData> BurstHealData { get; set; }
        [MetaProperty("championProgressiveTickData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarProgressiveTickData> ChampionProgressiveTickData { get; set; }
        [MetaProperty("fadeData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarFadeData> FadeData { get; set; }
        [MetaProperty("defenseModifierData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarDefenseModifierData> DefenseModifierData { get; set; }
        [MetaProperty("defenseIconData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudHealthBarDefenseIconData> DefenseIconData { get; set; }
        [MetaProperty("untargetableAlpha", BinPropertyType.Float)]
        public float? UntargetableAlpha { get; set; }
        [MetaProperty("resurrectingAlpha", BinPropertyType.Float)]
        public float? ResurrectingAlpha { get; set; }
    }
    [MetaClass("HudSpellSlotResetFeedbackData")]
    public class HudSpellSlotResetFeedbackData : IMetaClass
    {
        [MetaProperty("spellResetFlashFadeDuration", BinPropertyType.Float)]
        public float? SpellResetFlashFadeDuration { get; set; }
        [MetaProperty("spellResetFlashScaleDownDuration", BinPropertyType.Float)]
        public float? SpellResetFlashScaleDownDuration { get; set; }
        [MetaProperty("spellResetScaleMultiplier", BinPropertyType.Float)]
        public float? SpellResetScaleMultiplier { get; set; }
    }
    [MetaClass("HudLoadingScreenProgressBarData")]
    public class HudLoadingScreenProgressBarData : IMetaClass
    {
        [MetaProperty(3836273065, BinPropertyType.Float)]
        public float? m3836273065 { get; set; }
        [MetaProperty(3600106471, BinPropertyType.Float)]
        public float? m3600106471 { get; set; }
        [MetaProperty(2671588403, BinPropertyType.Float)]
        public float? m2671588403 { get; set; }
        [MetaProperty(3041975949, BinPropertyType.Float)]
        public float? m3041975949 { get; set; }
    }
    [MetaClass("HudLoadingScreenData")]
    public class HudLoadingScreenData : IMetaClass
    {
        [MetaProperty("mProgressBarData", BinPropertyType.Embedded)]
        public MetaEmbedded<HudLoadingScreenProgressBarData> ProgressBarData { get; set; }
        [MetaProperty(2635590115, BinPropertyType.Bool)]
        public bool? m2635590115 { get; set; }
        [MetaProperty(1649527380, BinPropertyType.Float)]
        public float? m1649527380 { get; set; }
        [MetaProperty(2189949099, BinPropertyType.Byte)]
        public byte? m2189949099 { get; set; }
        [MetaProperty(387190268, BinPropertyType.Byte)]
        public byte? m387190268 { get; set; }
    }
    [MetaClass("HudStatPanelStatStoneData")]
    public class HudStatPanelStatStoneData : IMetaClass
    {
        [MetaProperty(3397678954, BinPropertyType.Float)]
        public float? m3397678954 { get; set; }
        [MetaProperty(4134905527, BinPropertyType.Float)]
        public float? m4134905527 { get; set; }
        [MetaProperty(1407617488, BinPropertyType.Float)]
        public float? m1407617488 { get; set; }
        [MetaProperty(2010657113, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2010657113 { get; set; }
        [MetaProperty(1256611322, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m1256611322 { get; set; }
        [MetaProperty(494112949, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m494112949 { get; set; }
        [MetaProperty(191273630, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m191273630 { get; set; }
    }
    [MetaClass("HudStatStoneData")]
    public class HudStatStoneData : IMetaClass
    {
        [MetaProperty(769045550, BinPropertyType.String)]
        public string? m769045550 { get; set; }
        [MetaProperty(1016048105, BinPropertyType.String)]
        public string? m1016048105 { get; set; }
    }
    [MetaClass("HudStatStoneDeathRecapData")]
    public class HudStatStoneDeathRecapData : IMetaClass
    {
        [MetaProperty(656979086, BinPropertyType.Float)]
        public float? m656979086 { get; set; }
        [MetaProperty(1115385192, BinPropertyType.Float)]
        public float? m1115385192 { get; set; }
        [MetaProperty(2707103634, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2707103634 { get; set; }
        [MetaProperty(225474848, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m225474848 { get; set; }
        [MetaProperty(1529925733, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m1529925733 { get; set; }
    }
    [MetaClass("HudStatStoneMilestoneData")]
    public class HudStatStoneMilestoneData : IMetaClass
    {
        [MetaProperty(2938438826, BinPropertyType.String)]
        public string? m2938438826 { get; set; }
        [MetaProperty(3992539329, BinPropertyType.String)]
        public string? m3992539329 { get; set; }
        [MetaProperty(1333781411, BinPropertyType.String)]
        public string? m1333781411 { get; set; }
        [MetaProperty(3144760040, BinPropertyType.String)]
        public string? m3144760040 { get; set; }
        [MetaProperty(1043625708, BinPropertyType.Float)]
        public float? m1043625708 { get; set; }
        [MetaProperty(3709612203, BinPropertyType.Float)]
        public float? m3709612203 { get; set; }
        [MetaProperty(776943490, BinPropertyType.Float)]
        public float? m776943490 { get; set; }
        [MetaProperty(2107277216, BinPropertyType.Float)]
        public float? m2107277216 { get; set; }
        [MetaProperty(3541856466, BinPropertyType.Float)]
        public float? m3541856466 { get; set; }
        [MetaProperty(973000595, BinPropertyType.Float)]
        public float? m973000595 { get; set; }
        [MetaProperty(229950671, BinPropertyType.UInt32)]
        public uint? m229950671 { get; set; }
        [MetaProperty(2798761049, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2798761049 { get; set; }
        [MetaProperty(3053429941, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m3053429941 { get; set; }
        [MetaProperty(803870366, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m803870366 { get; set; }
        [MetaProperty(1319476500, BinPropertyType.Bool)]
        public bool? m1319476500 { get; set; }
    }
    [MetaClass("HudEndOfGameData")]
    public class HudEndOfGameData : IMetaClass
    {
        [MetaProperty(730945406, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m730945406 { get; set; }
        [MetaProperty(3856555387, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m3856555387 { get; set; }
    }
    [MetaClass(1269294773)]
    public class Class1269294773 : IMetaClass
    {
        [MetaProperty(2407574701, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m2407574701 { get; set; }
        [MetaProperty(3756733196, BinPropertyType.Float)]
        public float? m3756733196 { get; set; }
        [MetaProperty(415023739, BinPropertyType.UInt32)]
        public uint? m415023739 { get; set; }
    }
    [MetaClass("HudDamageDisplayData")]
    public class HudDamageDisplayData : IMetaClass
    {
        [MetaProperty(3754862555, BinPropertyType.Float)]
        public float? m3754862555 { get; set; }
    }
    [MetaClass("HudFightRecapUIData")]
    public class HudFightRecapUIData : IMetaClass
    {
        [MetaProperty(2847772997, BinPropertyType.String)]
        public string? m2847772997 { get; set; }
        [MetaProperty(404749450, BinPropertyType.String)]
        public string? m404749450 { get; set; }
        [MetaProperty(2846050793, BinPropertyType.String)]
        public string? m2846050793 { get; set; }
        [MetaProperty(3439137196, BinPropertyType.String)]
        public string? m3439137196 { get; set; }
    }
    [MetaClass("HudWindowPlacementData")]
    public class HudWindowPlacementData : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.Hash)]
        public MetaHash? Name { get; set; }
        [MetaProperty(2888080348, BinPropertyType.Float)]
        public float? m2888080348 { get; set; }
        [MetaProperty(1182848221, BinPropertyType.Float)]
        public float? m1182848221 { get; set; }
        [MetaProperty("MinScale", BinPropertyType.Float)]
        public float? MinScale { get; set; }
        [MetaProperty("UseSimpleScaling", BinPropertyType.Bool)]
        public bool? UseSimpleScaling { get; set; }
        [MetaProperty("UseHeightScale", BinPropertyType.Bool)]
        public bool? UseHeightScale { get; set; }
        [MetaProperty("anchor", BinPropertyType.String)]
        public string? Anchor { get; set; }
    }
    [MetaClass("MinimapPingEffectDefinition")]
    public class MinimapPingEffectDefinition : IMetaClass
    {
        [MetaProperty("alphaStart", BinPropertyType.UInt32)]
        public uint? AlphaStart { get; set; }
        [MetaProperty("alphaFadeSpeed", BinPropertyType.Float)]
        public float? AlphaFadeSpeed { get; set; }
        [MetaProperty("scaleStart", BinPropertyType.Float)]
        public float? ScaleStart { get; set; }
        [MetaProperty("scaleSpeed", BinPropertyType.Float)]
        public float? ScaleSpeed { get; set; }
        [MetaProperty("startDelay", BinPropertyType.Float)]
        public float? StartDelay { get; set; }
        [MetaProperty("life", BinPropertyType.Float)]
        public float? Life { get; set; }
        [MetaProperty("onDeathFadeSpeed", BinPropertyType.Float)]
        public float? OnDeathFadeSpeed { get; set; }
        [MetaProperty("repeatCount", BinPropertyType.UInt32)]
        public uint? RepeatCount { get; set; }
        [MetaProperty("blendWithAlpha", BinPropertyType.Bool)]
        public bool? BlendWithAlpha { get; set; }
    }
    [MetaClass("TextureAndColorData")]
    public class TextureAndColorData : IMetaClass
    {
        [MetaProperty("textureFile", BinPropertyType.String)]
        public string? TextureFile { get; set; }
        [MetaProperty("color", BinPropertyType.Color)]
        public Color? Color { get; set; }
        [MetaProperty("colorblindTextureFile", BinPropertyType.String)]
        public string? ColorblindTextureFile { get; set; }
        [MetaProperty("colorblindColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindColor { get; set; }
    }
    [MetaClass("MinimapPingEffectAndTextureData")]
    public class MinimapPingEffectAndTextureData : IMetaClass
    {
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mEffect", BinPropertyType.Embedded)]
        public MetaEmbedded<MinimapPingEffectDefinition> Effect { get; set; }
        [MetaProperty("mDefault", BinPropertyType.Embedded)]
        public MetaEmbedded<TextureAndColorData> Default { get; set; }
        [MetaProperty("mOrder", BinPropertyType.Structure)]
        public TextureAndColorData Order { get; set; }
        [MetaProperty("mChaos", BinPropertyType.Structure)]
        public TextureAndColorData Chaos { get; set; }
    }
    [MetaClass("MinimapPingTypeContainer")]
    public class MinimapPingTypeContainer : IMetaClass
    {
        [MetaProperty("pingEffectList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MinimapPingEffectAndTextureData>> PingEffectList { get; set; }
    }
    [MetaClass("MinimapPingData")]
    public class MinimapPingData : IMetaClass
    {
        [MetaProperty("mOMWPingRangeCutoffs", BinPropertyType.Map)]
        public Dictionary<byte, float> OMWPingRangeCutoffs { get; set; }
        [MetaProperty("mPings", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MinimapPingTypeContainer>> Pings { get; set; }
    }
    [MetaClass("ISecondaryResourceDisplayData")]
    public interface ISecondaryResourceDisplayData : IMetaClass
    {
    }
    [MetaClass("SecondaryResourceDisplayFractional")]
    public class SecondaryResourceDisplayFractional : ISecondaryResourceDisplayData
    {
    }
    [MetaClass(489596909)]
    public class Class489596909 : IMetaClass
    {
        [MetaProperty(974390421, BinPropertyType.UInt32)]
        public uint? m974390421 { get; set; }
        [MetaProperty("scale", BinPropertyType.Float)]
        public float? Scale { get; set; }
    }
    [MetaClass(141273677)]
    public class Class141273677 : IMetaClass
    {
        [MetaProperty(1441054572, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class489596909>> m1441054572 { get; set; }
    }
    [MetaClass("LoadingScreenRankedProperties")]
    public class LoadingScreenRankedProperties : IMetaClass
    {
        [MetaProperty("mDescriptor", BinPropertyType.String)]
        public string? Descriptor { get; set; }
        [MetaProperty("mDivision", BinPropertyType.Byte)]
        public byte? Division { get; set; }
    }
    [MetaClass("LoadingScreenRankedData")]
    public class LoadingScreenRankedData : IMetaClass
    {
        [MetaProperty(1751568959, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<LoadingScreenRankedProperties>> m1751568959 { get; set; }
    }
    [MetaClass(3842081615)]
    public class Class3842081615 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
    }
    [MetaClass("NavHeaderViewController")]
    public class NavHeaderViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(3498282858, BinPropertyType.Hash)]
        public MetaHash? m3498282858 { get; set; }
        [MetaProperty(2582753338, BinPropertyType.Hash)]
        public MetaHash? m2582753338 { get; set; }
        [MetaProperty(157177727, BinPropertyType.Hash)]
        public MetaHash? m157177727 { get; set; }
        [MetaProperty(1701094588, BinPropertyType.Hash)]
        public MetaHash? m1701094588 { get; set; }
        [MetaProperty(3962715134, BinPropertyType.Hash)]
        public MetaHash? m3962715134 { get; set; }
        [MetaProperty(811139480, BinPropertyType.Hash)]
        public MetaHash? m811139480 { get; set; }
        [MetaProperty(2476808443, BinPropertyType.Hash)]
        public MetaHash? m2476808443 { get; set; }
        [MetaProperty(2072938133, BinPropertyType.Hash)]
        public MetaHash? m2072938133 { get; set; }
    }
    [MetaClass(3154693744)]
    public class Class3154693744 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(893211270, BinPropertyType.Hash)]
        public MetaHash? m893211270 { get; set; }
        [MetaProperty(1001273705, BinPropertyType.Hash)]
        public MetaHash? m1001273705 { get; set; }
        [MetaProperty(2767052001, BinPropertyType.Hash)]
        public MetaHash? m2767052001 { get; set; }
        [MetaProperty(2129943644, BinPropertyType.Hash)]
        public MetaHash? m2129943644 { get; set; }
        [MetaProperty(203279661, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m203279661 { get; set; }
        [MetaProperty(1514600716, BinPropertyType.Hash)]
        public MetaHash? m1514600716 { get; set; }
        [MetaProperty(3998205451, BinPropertyType.Hash)]
        public MetaHash? m3998205451 { get; set; }
    }
    [MetaClass("QualitySetting")]
    public class QualitySetting : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(54440845, BinPropertyType.Float)]
        public float? m54440845 { get; set; }
        [MetaProperty(502492027, BinPropertyType.UInt32)]
        public uint? m502492027 { get; set; }
        [MetaProperty("mEnvironmentQuality", BinPropertyType.UInt32)]
        public uint? EnvironmentQuality { get; set; }
        [MetaProperty(2832832311, BinPropertyType.UInt32)]
        public uint? m2832832311 { get; set; }
        [MetaProperty("mEffectsQuality", BinPropertyType.UInt32)]
        public uint? EffectsQuality { get; set; }
        [MetaProperty("mShadowQuality", BinPropertyType.UInt32)]
        public uint? ShadowQuality { get; set; }
        [MetaProperty(484066796, BinPropertyType.Bool)]
        public bool? m484066796 { get; set; }
    }
    [MetaClass(2037513198)]
    public interface Class2037513198 : IMetaClass
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        Class3415079880 m3353438327 { get; set; }
    }
    [MetaClass(3154887987)]
    public class Class3154887987 : IMetaClass
    {
        [MetaProperty(1564656176, BinPropertyType.Hash)]
        public MetaHash? m1564656176 { get; set; }
    }
    [MetaClass(4083351021)]
    public class Class4083351021 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<Class2037513198> Items { get; set; }
    }
    [MetaClass(2624893466)]
    public class Class2624893466 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(3890322324, BinPropertyType.UInt16)]
        public ushort? m3890322324 { get; set; }
        [MetaProperty(1971108699, BinPropertyType.Bool)]
        public bool? m1971108699 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty(1609595816, BinPropertyType.String)]
        public string? m1609595816 { get; set; }
    }
    [MetaClass(3265939366)]
    public class Class3265939366 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(655852410, BinPropertyType.Container)]
        public MetaContainer<Class2037513198> m655852410 { get; set; }
        [MetaProperty(2948583814, BinPropertyType.Container)]
        public MetaContainer<Class2037513198> m2948583814 { get; set; }
        [MetaProperty(1825392187, BinPropertyType.Container)]
        public MetaContainer<Class2037513198> m1825392187 { get; set; }
    }
    [MetaClass(1128087393)]
    public class Class1128087393 : IMetaClass
    {
        [MetaProperty(2975295581, BinPropertyType.String)]
        public string? m2975295581 { get; set; }
        [MetaProperty("value", BinPropertyType.Int32)]
        public int? Value { get; set; }
    }
    [MetaClass(2993708970)]
    public class Class2993708970 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(3890322324, BinPropertyType.UInt16)]
        public ushort? m3890322324 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty(1609595816, BinPropertyType.String)]
        public string? m1609595816 { get; set; }
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class1128087393>> Items { get; set; }
    }
    [MetaClass(3415079880)]
    public interface Class3415079880 : IMetaClass
    {
    }
    [MetaClass(174539687)]
    public class Class174539687 : Class3415079880
    {
        [MetaProperty(2882977356, BinPropertyType.Container)]
        public MetaContainer<Class3415079880> m2882977356 { get; set; }
    }
    [MetaClass(3443072153)]
    public class Class3443072153 : Class3415079880
    {
        [MetaProperty(1107783316, BinPropertyType.Bool)]
        public bool? m1107783316 { get; set; }
        [MetaProperty(1143239913, BinPropertyType.Bool)]
        public bool? m1143239913 { get; set; }
        [MetaProperty(1108101821, BinPropertyType.Bool)]
        public bool? m1108101821 { get; set; }
        [MetaProperty(501119360, BinPropertyType.Bool)]
        public bool? m501119360 { get; set; }
    }
    [MetaClass(1046078154)]
    public class Class1046078154 : Class3415079880
    {
        [MetaProperty(2073624541, BinPropertyType.Bool)]
        public bool? m2073624541 { get; set; }
        [MetaProperty(2166835362, BinPropertyType.Bool)]
        public bool? m2166835362 { get; set; }
        [MetaProperty(932731737, BinPropertyType.Bool)]
        public bool? m932731737 { get; set; }
    }
    [MetaClass(3641602072)]
    public class Class3641602072 : Class3415079880
    {
        [MetaProperty("Map", BinPropertyType.Hash)]
        public MetaHash? Map { get; set; }
    }
    [MetaClass(3998028548)]
    public class Class3998028548 : Class3415079880
    {
        [MetaProperty("Mutator", BinPropertyType.String)]
        public string? Mutator { get; set; }
    }
    [MetaClass(4160905752)]
    public class Class4160905752 : Class3415079880
    {
    }
    [MetaClass(2761167747)]
    public class Class2761167747 : Class3415079880
    {
    }
    [MetaClass(12944262)]
    public class Class12944262 : Class3415079880
    {
        [MetaProperty(3346810982, BinPropertyType.Bool)]
        public bool? m3346810982 { get; set; }
    }
    [MetaClass(2795848259)]
    public class Class2795848259 : Class3415079880
    {
    }
    [MetaClass(3617299650)]
    public class Class3617299650 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<Class2037513198> Items { get; set; }
    }
    [MetaClass(2450342190)]
    public class Class2450342190 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
    }
    [MetaClass(1981427363)]
    public class Class1981427363 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3748421612, BinPropertyType.String)]
        public string? m3748421612 { get; set; }
        [MetaProperty(1578692245, BinPropertyType.String)]
        public string? m1578692245 { get; set; }
    }
    [MetaClass(1259449812)]
    public class Class1259449812 : Class2993708970
    {
    }
    [MetaClass(1428235105)]
    public class Class1428235105 : IMetaClass
    {
        [MetaProperty(1803962597, BinPropertyType.String)]
        public string? m1803962597 { get; set; }
        [MetaProperty(1971332848, BinPropertyType.String)]
        public string? m1971332848 { get; set; }
    }
    [MetaClass(1368219584)]
    public class Class1368219584 : IMetaClass
    {
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty(705137820, BinPropertyType.String)]
        public string? m705137820 { get; set; }
    }
    [MetaClass(3144650323)]
    public class Class3144650323 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3834172512, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1428235105> m3834172512 { get; set; }
        [MetaProperty(708293528, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class1368219584>> m708293528 { get; set; }
    }
    [MetaClass(3929150294)]
    public class Class3929150294 : IMetaClass
    {
        [MetaProperty(1803962597, BinPropertyType.String)]
        public string? m1803962597 { get; set; }
        [MetaProperty(1971332848, BinPropertyType.String)]
        public string? m1971332848 { get; set; }
        [MetaProperty(885511311, BinPropertyType.String)]
        public string? m885511311 { get; set; }
    }
    [MetaClass(1124978957)]
    public class Class1124978957 : IMetaClass
    {
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty(705137820, BinPropertyType.String)]
        public string? m705137820 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
    }
    [MetaClass(1788849882)]
    public class Class1788849882 : Class2037513198
    {
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3834172512, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3929150294> m3834172512 { get; set; }
        [MetaProperty(708293528, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class1124978957>> m708293528 { get; set; }
    }
    [MetaClass(1991153274)]
    public interface Class1991153274 : Class2037513198
    {
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        MetaHash? m1766500875 { get; set; }
        [MetaProperty(3389946293, BinPropertyType.String)]
        string? m3389946293 { get; set; }
        [MetaProperty(1609595816, BinPropertyType.String)]
        string? m1609595816 { get; set; }
    }
    [MetaClass(2846603080)]
    public class Class2846603080 : Class1991153274
    {
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty(1609595816, BinPropertyType.String)]
        public string? m1609595816 { get; set; }
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(3890322324, BinPropertyType.UInt16)]
        public ushort? m3890322324 { get; set; }
        [MetaProperty(2142584699, BinPropertyType.Bool)]
        public bool? m2142584699 { get; set; }
    }
    [MetaClass(1524323892)]
    public class Class1524323892 : Class1991153274
    {
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty(1609595816, BinPropertyType.String)]
        public string? m1609595816 { get; set; }
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
    }
    [MetaClass(641011299)]
    public class Class641011299 : Class1991153274
    {
        [MetaProperty(1766500875, BinPropertyType.Hash)]
        public MetaHash? m1766500875 { get; set; }
        [MetaProperty(3389946293, BinPropertyType.String)]
        public string? m3389946293 { get; set; }
        [MetaProperty(1609595816, BinPropertyType.String)]
        public string? m1609595816 { get; set; }
        [MetaProperty(1090655294, BinPropertyType.Bool)]
        public bool? m1090655294 { get; set; }
        [MetaProperty(1210455602, BinPropertyType.Byte)]
        public byte? m1210455602 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty(3890322324, BinPropertyType.UInt16)]
        public ushort? m3890322324 { get; set; }
        [MetaProperty(92209806, BinPropertyType.UInt32)]
        public uint? m92209806 { get; set; }
    }
    [MetaClass(3682643564)]
    public class Class3682643564 : Class2846603080
    {
        [MetaProperty(2758761786, BinPropertyType.Hash)]
        public MetaHash? m2758761786 { get; set; }
        [MetaProperty(3011713149, BinPropertyType.UInt16)]
        public ushort? m3011713149 { get; set; }
    }
    [MetaClass(4113714730)]
    public class Class4113714730 : Class2993708970
    {
    }
    [MetaClass(2946448300)]
    public class Class2946448300 : IMetaClass
    {
        [MetaProperty(3806037407, BinPropertyType.String)]
        public string? m3806037407 { get; set; }
        [MetaProperty(3841823045, BinPropertyType.Byte)]
        public byte? m3841823045 { get; set; }
        [MetaProperty(3353438327, BinPropertyType.Structure)]
        public Class3415079880 m3353438327 { get; set; }
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<Class2037513198> Items { get; set; }
    }
    [MetaClass(3405043372)]
    public class Class3405043372 : Class3154887987
    {
        [MetaProperty(525480503, BinPropertyType.Hash)]
        public MetaHash? m525480503 { get; set; }
    }
    [MetaClass(2855504619)]
    public class Class2855504619 : Class3154887987
    {
        [MetaProperty(3751321492, BinPropertyType.Hash)]
        public MetaHash? m3751321492 { get; set; }
        [MetaProperty(3889856367, BinPropertyType.Hash)]
        public MetaHash? m3889856367 { get; set; }
    }
    [MetaClass(2120591967)]
    public class Class2120591967 : Class3154887987
    {
        [MetaProperty(3889856367, BinPropertyType.Hash)]
        public MetaHash? m3889856367 { get; set; }
        [MetaProperty(4204097927, BinPropertyType.Hash)]
        public MetaHash? m4204097927 { get; set; }
    }
    [MetaClass(1734681201)]
    public class Class1734681201 : Class3154887987
    {
        [MetaProperty(2302996240, BinPropertyType.Hash)]
        public MetaHash? m2302996240 { get; set; }
        [MetaProperty(3889856367, BinPropertyType.Hash)]
        public MetaHash? m3889856367 { get; set; }
        [MetaProperty(3812151080, BinPropertyType.Color)]
        public Color? m3812151080 { get; set; }
        [MetaProperty(3882223319, BinPropertyType.Hash)]
        public MetaHash? m3882223319 { get; set; }
        [MetaProperty(2576771507, BinPropertyType.Float)]
        public float? m2576771507 { get; set; }
    }
    [MetaClass(1432209297)]
    public class Class1432209297 : IMetaClass
    {
        [MetaProperty(4137097213, BinPropertyType.Hash)]
        public MetaHash? m4137097213 { get; set; }
    }
    [MetaClass(3163647920)]
    public class Class3163647920 : IMetaClass
    {
        [MetaProperty(705137820, BinPropertyType.String)]
        public string? m705137820 { get; set; }
        [MetaProperty(1245960580, BinPropertyType.String)]
        public string? m1245960580 { get; set; }
        [MetaProperty("position", BinPropertyType.Hash)]
        public MetaHash? Position { get; set; }
    }
    [MetaClass(704287001)]
    public class Class704287001 : Class3154887987
    {
        [MetaProperty(165670742, BinPropertyType.Hash)]
        public MetaHash? m165670742 { get; set; }
        [MetaProperty(3331404723, BinPropertyType.Hash)]
        public MetaHash? m3331404723 { get; set; }
        [MetaProperty(1236606745, BinPropertyType.Hash)]
        public MetaHash? m1236606745 { get; set; }
        [MetaProperty(3367289996, BinPropertyType.Hash)]
        public MetaHash? m3367289996 { get; set; }
        [MetaProperty(2284695756, BinPropertyType.Hash)]
        public MetaHash? m2284695756 { get; set; }
        [MetaProperty(789705163, BinPropertyType.Hash)]
        public MetaHash? m789705163 { get; set; }
        [MetaProperty(2833578361, BinPropertyType.Hash)]
        public MetaHash? m2833578361 { get; set; }
        [MetaProperty(3185185930, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class1432209297>> m3185185930 { get; set; }
        [MetaProperty(4182378701, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class3163647920>> m4182378701 { get; set; }
    }
    [MetaClass(313035124)]
    public class Class313035124 : Class3154887987
    {
        [MetaProperty(4225398308, BinPropertyType.Hash)]
        public MetaHash? m4225398308 { get; set; }
        [MetaProperty(4275731165, BinPropertyType.Hash)]
        public MetaHash? m4275731165 { get; set; }
    }
    [MetaClass(4250471695)]
    public class Class4250471695 : Class3154887987
    {
        [MetaProperty(3751321492, BinPropertyType.Hash)]
        public MetaHash? m3751321492 { get; set; }
    }
    [MetaClass(3468103258)]
    public class Class3468103258 : IMetaClass
    {
        [MetaProperty(1272541071, BinPropertyType.Hash)]
        public MetaHash? m1272541071 { get; set; }
        [MetaProperty("TextElement", BinPropertyType.Hash)]
        public MetaHash? TextElement { get; set; }
    }
    [MetaClass(894011560)]
    public class Class894011560 : IMetaClass
    {
        [MetaProperty(3751321492, BinPropertyType.Hash)]
        public MetaHash? m3751321492 { get; set; }
    }
    [MetaClass(3369934580)]
    public class Class3369934580 : Class3154887987
    {
        [MetaProperty(1532605833, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3468103258> m1532605833 { get; set; }
        [MetaProperty(1515828214, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3468103258> m1515828214 { get; set; }
        [MetaProperty(4247899083, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3468103258> m4247899083 { get; set; }
        [MetaProperty(1415010472, BinPropertyType.Embedded)]
        public MetaEmbedded<Class894011560> m1415010472 { get; set; }
    }
    [MetaClass(258305809)]
    public class Class258305809 : Class3154887987
    {
        [MetaProperty(1532605833, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3468103258> m1532605833 { get; set; }
        [MetaProperty(1515828214, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3468103258> m1515828214 { get; set; }
        [MetaProperty(1499050595, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3468103258> m1499050595 { get; set; }
        [MetaProperty(4247899083, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3468103258> m4247899083 { get; set; }
        [MetaProperty(1415010472, BinPropertyType.Embedded)]
        public MetaEmbedded<Class894011560> m1415010472 { get; set; }
        [MetaProperty(1465343329, BinPropertyType.Embedded)]
        public MetaEmbedded<Class894011560> m1465343329 { get; set; }
    }
    [MetaClass(517015619)]
    public class Class517015619 : Class3154887987
    {
        [MetaProperty(3889856367, BinPropertyType.Hash)]
        public MetaHash? m3889856367 { get; set; }
        [MetaProperty(2645057986, BinPropertyType.Hash)]
        public MetaHash? m2645057986 { get; set; }
        [MetaProperty(1778722188, BinPropertyType.Hash)]
        public MetaHash? m1778722188 { get; set; }
    }
    [MetaClass("PostGameViewController")]
    public class PostGameViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(1293073416, BinPropertyType.Hash)]
        public MetaHash? m1293073416 { get; set; }
        [MetaProperty(1370950415, BinPropertyType.Hash)]
        public MetaHash? m1370950415 { get; set; }
    }
    [MetaClass("TFTOutOfGameCharData")]
    public class TFTOutOfGameCharData : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(3735203329, BinPropertyType.String)]
        public string? m3735203329 { get; set; }
    }
    [MetaClass("CelebrationViewController")]
    public class CelebrationViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(1633434665, BinPropertyType.Float)]
        public float? m1633434665 { get; set; }
    }
    [MetaClass("ModeSelectButtonData")]
    public class ModeSelectButtonData : IMetaClass
    {
        [MetaProperty("queueId", BinPropertyType.Int64)]
        public long? QueueId { get; set; }
        [MetaProperty(3751321492, BinPropertyType.Hash)]
        public MetaHash? m3751321492 { get; set; }
    }
    [MetaClass("HomeViewController")]
    public class HomeViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(3395835653, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ModeSelectButtonData>> m3395835653 { get; set; }
        [MetaProperty(3821028943, BinPropertyType.Hash)]
        public MetaHash? m3821028943 { get; set; }
        [MetaProperty(2193212256, BinPropertyType.Hash)]
        public MetaHash? m2193212256 { get; set; }
        [MetaProperty(3822862547, BinPropertyType.Hash)]
        public MetaHash? m3822862547 { get; set; }
        [MetaProperty(3575910535, BinPropertyType.Hash)]
        public MetaHash? m3575910535 { get; set; }
        [MetaProperty(89778525, BinPropertyType.Hash)]
        public MetaHash? m89778525 { get; set; }
        [MetaProperty(2947402559, BinPropertyType.Hash)]
        public MetaHash? m2947402559 { get; set; }
        [MetaProperty(4225182998, BinPropertyType.Embedded)]
        public MetaEmbedded<Class4225182998> m4225182998 { get; set; }
        [MetaProperty(3518751045, BinPropertyType.String)]
        public string? m3518751045 { get; set; }
        [MetaProperty(1435883698, BinPropertyType.String)]
        public string? m1435883698 { get; set; }
        [MetaProperty(3692913182, BinPropertyType.Float)]
        public float? m3692913182 { get; set; }
        [MetaProperty(2259066423, BinPropertyType.UInt32)]
        public uint? m2259066423 { get; set; }
    }
    [MetaClass(2293774348)]
    public class Class2293774348 : IMetaClass
    {
        [MetaProperty(3441777566, BinPropertyType.String)]
        public string? m3441777566 { get; set; }
        [MetaProperty(2358317979, BinPropertyType.UInt32)]
        public uint? m2358317979 { get; set; }
        [MetaProperty(1311529430, BinPropertyType.Hash)]
        public MetaHash? m1311529430 { get; set; }
        [MetaProperty(1736778658, BinPropertyType.Hash)]
        public MetaHash? m1736778658 { get; set; }
        [MetaProperty("Region", BinPropertyType.Hash)]
        public MetaHash? Region { get; set; }
        [MetaProperty(3751321492, BinPropertyType.Hash)]
        public MetaHash? m3751321492 { get; set; }
    }
    [MetaClass(2838012998)]
    public class Class2838012998 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(2293774348, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class2293774348>> m2293774348 { get; set; }
    }
    [MetaClass("LoadoutViewController")]
    public class LoadoutViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(222799957, BinPropertyType.Embedded)]
        public MetaEmbedded<LoadoutCompanionInfoPanel> m222799957 { get; set; }
        [MetaProperty(3889368386, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2807723955> m3889368386 { get; set; }
        [MetaProperty(1746567471, BinPropertyType.Embedded)]
        public MetaEmbedded<Class4100634241> m1746567471 { get; set; }
        [MetaProperty(1713107521, BinPropertyType.Embedded)]
        public MetaEmbedded<Class845574667> m1713107521 { get; set; }
        [MetaProperty(967670299, BinPropertyType.Hash)]
        public MetaHash? m967670299 { get; set; }
        [MetaProperty(3349513554, BinPropertyType.Hash)]
        public MetaHash? m3349513554 { get; set; }
        [MetaProperty(3029377995, BinPropertyType.Hash)]
        public MetaHash? m3029377995 { get; set; }
        [MetaProperty(3577606887, BinPropertyType.Hash)]
        public MetaHash? m3577606887 { get; set; }
        [MetaProperty(2439455768, BinPropertyType.Hash)]
        public MetaHash? m2439455768 { get; set; }
        [MetaProperty(3517705117, BinPropertyType.Hash)]
        public MetaHash? m3517705117 { get; set; }
        [MetaProperty(2690247672, BinPropertyType.String)]
        public string? m2690247672 { get; set; }
        [MetaProperty(54554555, BinPropertyType.String)]
        public string? m54554555 { get; set; }
    }
    [MetaClass("LobbyViewController")]
    public class LobbyViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(3508832112, BinPropertyType.Hash)]
        public MetaHash? m3508832112 { get; set; }
        [MetaProperty(2196467966, BinPropertyType.Hash)]
        public MetaHash? m2196467966 { get; set; }
        [MetaProperty(3583548078, BinPropertyType.Hash)]
        public MetaHash? m3583548078 { get; set; }
        [MetaProperty("LobbyCloseButton", BinPropertyType.Hash)]
        public MetaHash? LobbyCloseButton { get; set; }
        [MetaProperty(4167390460, BinPropertyType.Hash)]
        public MetaHash? m4167390460 { get; set; }
        [MetaProperty(2689792797, BinPropertyType.Hash)]
        public MetaHash? m2689792797 { get; set; }
        [MetaProperty(1177766852, BinPropertyType.Hash)]
        public MetaHash? m1177766852 { get; set; }
        [MetaProperty(3518751045, BinPropertyType.String)]
        public string? m3518751045 { get; set; }
        [MetaProperty(3570395155, BinPropertyType.String)]
        public string? m3570395155 { get; set; }
        [MetaProperty(706897365, BinPropertyType.String)]
        public string? m706897365 { get; set; }
    }
    [MetaClass(1013446472)]
    public class Class1013446472 : Class1965398739
    {
    }
    [MetaClass(162987137)]
    public class Class162987137 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(4202464323, BinPropertyType.Float)]
        public float? m4202464323 { get; set; }
        [MetaProperty(3967028638, BinPropertyType.UInt32)]
        public uint? m3967028638 { get; set; }
    }
    [MetaClass("QueueDisplayData")]
    public class QueueDisplayData : IMetaClass
    {
        [MetaProperty("queueId", BinPropertyType.Int64)]
        public long? QueueId { get; set; }
        [MetaProperty(3465986044, BinPropertyType.Hash)]
        public MetaHash? m3465986044 { get; set; }
        [MetaProperty(4062300114, BinPropertyType.Hash)]
        public MetaHash? m4062300114 { get; set; }
        [MetaProperty(35565451, BinPropertyType.Hash)]
        public MetaHash? m35565451 { get; set; }
        [MetaProperty(2745365850, BinPropertyType.Hash)]
        public MetaHash? m2745365850 { get; set; }
        [MetaProperty(3441777566, BinPropertyType.String)]
        public string? m3441777566 { get; set; }
    }
    [MetaClass("ModeSelectViewController")]
    public class ModeSelectViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty("queueDisplayData", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<QueueDisplayData>> QueueDisplayData { get; set; }
        [MetaProperty(2655774189, BinPropertyType.Hash)]
        public MetaHash? m2655774189 { get; set; }
        [MetaProperty(2239425430, BinPropertyType.Hash)]
        public MetaHash? m2239425430 { get; set; }
        [MetaProperty(409816793, BinPropertyType.Hash)]
        public MetaHash? m409816793 { get; set; }
        [MetaProperty(2690247672, BinPropertyType.String)]
        public string? m2690247672 { get; set; }
        [MetaProperty(54554555, BinPropertyType.String)]
        public string? m54554555 { get; set; }
    }
    [MetaClass(379849898)]
    public class Class379849898 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(2978897416, BinPropertyType.Hash)]
        public MetaHash? m2978897416 { get; set; }
    }
    [MetaClass("PurchaseDialog")]
    public class PurchaseDialog : Class1965398739
    {
        [MetaProperty(1457318954, BinPropertyType.Hash)]
        public MetaHash? m1457318954 { get; set; }
        [MetaProperty(91533656, BinPropertyType.Hash)]
        public MetaHash? m91533656 { get; set; }
    }
    [MetaClass(968608392)]
    public class Class968608392 : Class1965398739
    {
        [MetaProperty(688187426, BinPropertyType.Hash)]
        public MetaHash? m688187426 { get; set; }
    }
    [MetaClass("SocialPanelViewController")]
    public class SocialPanelViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(502506537, BinPropertyType.Hash)]
        public MetaHash? m502506537 { get; set; }
        [MetaProperty(1200704461, BinPropertyType.Hash)]
        public MetaHash? m1200704461 { get; set; }
        [MetaProperty(3699678108, BinPropertyType.Hash)]
        public MetaHash? m3699678108 { get; set; }
    }
    [MetaClass(4225182998)]
    public class Class4225182998 : IMetaClass
    {
        [MetaProperty(1295117638, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1295117638 { get; set; }
    }
    [MetaClass(4069990911)]
    public class Class4069990911 : IMetaClass
    {
        [MetaProperty("category", BinPropertyType.UInt32)]
        public uint? Category { get; set; }
        [MetaProperty(3751321492, BinPropertyType.Hash)]
        public MetaHash? m3751321492 { get; set; }
    }
    [MetaClass(1146372659)]
    public class Class1146372659 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(3402876242, BinPropertyType.Structure)]
        public Class2330109623 m3402876242 { get; set; }
        [MetaProperty(2662239426, BinPropertyType.Hash)]
        public MetaHash? m2662239426 { get; set; }
        [MetaProperty(3536587999, BinPropertyType.Hash)]
        public MetaHash? m3536587999 { get; set; }
        [MetaProperty(1877593956, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class4069990911>> m1877593956 { get; set; }
    }
    [MetaClass("TFTBattlepassViewController")]
    public class TFTBattlepassViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(2938854594, BinPropertyType.Hash)]
        public MetaHash? m2938854594 { get; set; }
        [MetaProperty(1855297149, BinPropertyType.Hash)]
        public MetaHash? m1855297149 { get; set; }
        [MetaProperty(3224678146, BinPropertyType.Float)]
        public float? m3224678146 { get; set; }
        [MetaProperty(1695233717, BinPropertyType.Embedded)]
        public MetaEmbedded<HudMenuTransitionData> m1695233717 { get; set; }
        [MetaProperty(1458709778, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1458709778 { get; set; }
    }
    [MetaClass(2807723955)]
    public class Class2807723955 : Class2136477118
    {
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
    }
    [MetaClass("LoadoutCompanionInfoPanel")]
    public class LoadoutCompanionInfoPanel : Class2136477118
    {
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(2549755037, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m2549755037 { get; set; }
        [MetaProperty(2085656919, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m2085656919 { get; set; }
        [MetaProperty(257989652, BinPropertyType.Hash)]
        public MetaHash? m257989652 { get; set; }
        [MetaProperty(3055944576, BinPropertyType.Hash)]
        public MetaHash? m3055944576 { get; set; }
        [MetaProperty(1784034802, BinPropertyType.Hash)]
        public MetaHash? m1784034802 { get; set; }
        [MetaProperty(2430432239, BinPropertyType.Hash)]
        public MetaHash? m2430432239 { get; set; }
    }
    [MetaClass(845574667)]
    public class Class845574667 : Class2136477118
    {
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(2549755037, BinPropertyType.Container)]
        public MetaContainer<MetaHash> m2549755037 { get; set; }
    }
    [MetaClass(4100634241)]
    public class Class4100634241 : Class2136477118
    {
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m2330109623 { get; set; }
        [MetaProperty(523298734, BinPropertyType.Hash)]
        public MetaHash? m523298734 { get; set; }
        [MetaProperty("EmoteWheelLeftButton", BinPropertyType.Hash)]
        public MetaHash? EmoteWheelLeftButton { get; set; }
        [MetaProperty(44244159, BinPropertyType.Hash)]
        public MetaHash? m44244159 { get; set; }
        [MetaProperty(566058836, BinPropertyType.Hash)]
        public MetaHash? m566058836 { get; set; }
        [MetaProperty("EmoteWheelCenterButton", BinPropertyType.Hash)]
        public MetaHash? EmoteWheelCenterButton { get; set; }
        [MetaProperty(2434524805, BinPropertyType.Hash)]
        public MetaHash? m2434524805 { get; set; }
        [MetaProperty(3414208295, BinPropertyType.Hash)]
        public MetaHash? m3414208295 { get; set; }
    }
    [MetaClass(2136477118)]
    public interface Class2136477118 : IMetaClass
    {
        [MetaProperty(2330109623, BinPropertyType.Embedded)]
        MetaEmbedded<Class2330109623> m2330109623 { get; set; }
    }
    [MetaClass(1973062744)]
    public class Class1973062744 : Class1965398739
    {
        [MetaProperty(2532609168, BinPropertyType.Hash)]
        public MetaHash? m2532609168 { get; set; }
        [MetaProperty(469739042, BinPropertyType.Hash)]
        public MetaHash? m469739042 { get; set; }
        [MetaProperty(3708628627, BinPropertyType.Hash)]
        public MetaHash? m3708628627 { get; set; }
        [MetaProperty(1778819735, BinPropertyType.Hash)]
        public MetaHash? m1778819735 { get; set; }
        [MetaProperty(856050794, BinPropertyType.Hash)]
        public MetaHash? m856050794 { get; set; }
        [MetaProperty(1159147490, BinPropertyType.Hash)]
        public MetaHash? m1159147490 { get; set; }
        [MetaProperty(3311057616, BinPropertyType.Hash)]
        public MetaHash? m3311057616 { get; set; }
        [MetaProperty(3666595123, BinPropertyType.Hash)]
        public MetaHash? m3666595123 { get; set; }
        [MetaProperty(1106065574, BinPropertyType.Hash)]
        public MetaHash? m1106065574 { get; set; }
    }
    [MetaClass(2610640435)]
    public class Class2610640435 : IMetaClass
    {
        [MetaProperty(939364701, BinPropertyType.String)]
        public string? m939364701 { get; set; }
        [MetaProperty(2305194088, BinPropertyType.Map)]
        public Dictionary<byte, string> m2305194088 { get; set; }
    }
    [MetaClass(2873675736)]
    public class Class2873675736 : IMetaClass
    {
        [MetaProperty(2815783430, BinPropertyType.String)]
        public string? m2815783430 { get; set; }
        [MetaProperty(3606586263, BinPropertyType.UInt32)]
        public uint? m3606586263 { get; set; }
        [MetaProperty(892697274, BinPropertyType.String)]
        public string? m892697274 { get; set; }
        [MetaProperty(912104930, BinPropertyType.String)]
        public string? m912104930 { get; set; }
        [MetaProperty(3646698844, BinPropertyType.String)]
        public string? m3646698844 { get; set; }
    }
    [MetaClass(1179857030)]
    public class Class1179857030 : IMetaClass
    {
        [MetaProperty(1450574097, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2873675736> m1450574097 { get; set; }
        [MetaProperty(1149844633, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2873675736> m1149844633 { get; set; }
        [MetaProperty(3590991722, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2873675736> m3590991722 { get; set; }
        [MetaProperty(1449424944, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2873675736> m1449424944 { get; set; }
        [MetaProperty(1827542789, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2873675736> m1827542789 { get; set; }
        [MetaProperty(3379408477, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2873675736> m3379408477 { get; set; }
    }
    [MetaClass(1257187638)]
    public class Class1257187638 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(103721739, BinPropertyType.UInt32)]
        public uint? m103721739 { get; set; }
        [MetaProperty(1848909498, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1269294773> m1848909498 { get; set; }
        [MetaProperty(3685154491, BinPropertyType.Float)]
        public float? m3685154491 { get; set; }
        [MetaProperty(414900293, BinPropertyType.Float)]
        public float? m414900293 { get; set; }
        [MetaProperty(2678288798, BinPropertyType.Float)]
        public float? m2678288798 { get; set; }
        [MetaProperty(687202616, BinPropertyType.Float)]
        public float? m687202616 { get; set; }
        [MetaProperty(2695771816, BinPropertyType.Float)]
        public float? m2695771816 { get; set; }
        [MetaProperty(3329922468, BinPropertyType.Float)]
        public float? m3329922468 { get; set; }
    }
    [MetaClass(3792429246)]
    public class Class3792429246 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
    }
    [MetaClass(4110481513)]
    public class Class4110481513 : IMetaClass
    {
        [MetaProperty(1060839150, BinPropertyType.Hash)]
        public MetaHash? m1060839150 { get; set; }
        [MetaProperty(1939712764, BinPropertyType.Hash)]
        public MetaHash? m1939712764 { get; set; }
        [MetaProperty(392875102, BinPropertyType.Hash)]
        public MetaHash? m392875102 { get; set; }
    }
    [MetaClass("SettingsViewController")]
    public class SettingsViewController : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(3487959510, BinPropertyType.Embedded)]
        public MetaEmbedded<Class4110481513> m3487959510 { get; set; }
        [MetaProperty(3029377995, BinPropertyType.Hash)]
        public MetaHash? m3029377995 { get; set; }
        [MetaProperty(2523201800, BinPropertyType.Hash)]
        public MetaHash? m2523201800 { get; set; }
        [MetaProperty(2690247672, BinPropertyType.String)]
        public string? m2690247672 { get; set; }
        [MetaProperty(54554555, BinPropertyType.String)]
        public string? m54554555 { get; set; }
    }
    [MetaClass("SurrenderTypeData")]
    public class SurrenderTypeData : IMetaClass
    {
        [MetaProperty(1387564760, BinPropertyType.Float)]
        public float? m1387564760 { get; set; }
        [MetaProperty("windowLength", BinPropertyType.Float)]
        public float? WindowLength { get; set; }
        [MetaProperty("startTime", BinPropertyType.Float)]
        public float? StartTime { get; set; }
        [MetaProperty("percentageRequired", BinPropertyType.Float)]
        public float? PercentageRequired { get; set; }
    }
    [MetaClass("SurrenderData")]
    public class SurrenderData : IMetaClass
    {
        [MetaProperty(1140283803, BinPropertyType.Float)]
        public float? m1140283803 { get; set; }
        [MetaProperty(244881724, BinPropertyType.Float)]
        public float? m244881724 { get; set; }
        [MetaProperty(3430961411, BinPropertyType.Float)]
        public float? m3430961411 { get; set; }
        [MetaProperty(989768947, BinPropertyType.Float)]
        public float? m989768947 { get; set; }
        [MetaProperty("mTypeData", BinPropertyType.Map)]
        public Dictionary<byte, MetaEmbedded<SurrenderTypeData>> TypeData { get; set; }
    }
    [MetaClass("LoadScreenTip")]
    public class LoadScreenTip : IMetaClass
    {
        [MetaProperty("mId", BinPropertyType.UInt16)]
        public ushort? Id { get; set; }
        [MetaProperty("mHeaderLocalizationKey", BinPropertyType.String)]
        public string? HeaderLocalizationKey { get; set; }
        [MetaProperty("mMinimumSummonerLevel", BinPropertyType.Optional)]
        public MetaOptional<uint> MinimumSummonerLevel { get; set; }
        [MetaProperty("mMaximumSummonerLevel", BinPropertyType.Optional)]
        public MetaOptional<uint> MaximumSummonerLevel { get; set; }
        [MetaProperty("mLocalizationKey", BinPropertyType.String)]
        public string? LocalizationKey { get; set; }
    }
    [MetaClass("LoadScreenTipSet")]
    public class LoadScreenTipSet : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.Hash)]
        public MetaHash? Name { get; set; }
        [MetaProperty("mTips", BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> Tips { get; set; }
    }
    [MetaClass("LoadScreenTipConfiguration")]
    public class LoadScreenTipConfiguration : IMetaClass
    {
        [MetaProperty("mShowInCustomGames", BinPropertyType.Bool)]
        public bool? ShowInCustomGames { get; set; }
        [MetaProperty("mShowPBITipsOnLoadingScreen", BinPropertyType.Bool)]
        public bool? ShowPBITipsOnLoadingScreen { get; set; }
        [MetaProperty("mPBITipDurationOnLoadingScreen", BinPropertyType.Float)]
        public float? PBITipDurationOnLoadingScreen { get; set; }
        [MetaProperty("mDurationInGame", BinPropertyType.Float)]
        public float? DurationInGame { get; set; }
    }
    [MetaClass("TooltipInstanceSpell")]
    public class TooltipInstanceSpell : TooltipInstance
    {
        [MetaProperty(1669781464, BinPropertyType.Bool)]
        public bool? m1669781464 { get; set; }
        [MetaProperty(4272119378, BinPropertyType.Bool)]
        public bool? m4272119378 { get; set; }
    }
    [MetaClass("TooltipInstanceBuff")]
    public class TooltipInstanceBuff : TooltipInstance
    {
        [MetaProperty(2929024189, BinPropertyType.Bool)]
        public bool? m2929024189 { get; set; }
    }
    [MetaClass("TooltipInstanceItem")]
    public class TooltipInstanceItem : TooltipInstance
    {
    }
    [MetaClass(2656759159)]
    public class Class2656759159 : IMetaClass
    {
        [MetaProperty(1461360547, BinPropertyType.Int32)]
        public int? m1461360547 { get; set; }
        [MetaProperty(2292470017, BinPropertyType.Int32)]
        public int? m2292470017 { get; set; }
        [MetaProperty(445332680, BinPropertyType.Int32)]
        public int? m445332680 { get; set; }
        [MetaProperty(3146852779, BinPropertyType.Int32)]
        public int? m3146852779 { get; set; }
        [MetaProperty(3806656962, BinPropertyType.Int32)]
        public int? m3806656962 { get; set; }
        [MetaProperty(1203616745, BinPropertyType.Int32)]
        public int? m1203616745 { get; set; }
    }
    [MetaClass(3013700229)]
    public class Class3013700229 : ViewController
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3080488622 { get; set; }
        [MetaProperty(1944635290, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2656759159> m1944635290 { get; set; }
        [MetaProperty(4126940474, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<Class2656759159>> m4126940474 { get; set; }
    }
    [MetaClass("ViewController")]
    public interface ViewController : IMetaClass
    {
        [MetaProperty(3080488622, BinPropertyType.ObjectLink)]
        MetaObjectLink? m3080488622 { get; set; }
    }
    [MetaClass("NeutralTimerSourceIconData")]
    public class NeutralTimerSourceIconData : IMetaClass
    {
        [MetaProperty("mTooltipName", BinPropertyType.String)]
        public string? TooltipName { get; set; }
        [MetaProperty("mIconName", BinPropertyType.String)]
        public string? IconName { get; set; }
    }
    [MetaClass("NeutralTimerData")]
    public class NeutralTimerData : IMetaClass
    {
        [MetaProperty("mTimerKeyName", BinPropertyType.String)]
        public string? TimerKeyName { get; set; }
        [MetaProperty("mTooltip", BinPropertyType.String)]
        public string? Tooltip { get; set; }
        [MetaProperty("mTooltipCampName", BinPropertyType.String)]
        public string? TooltipCampName { get; set; }
        [MetaProperty("mTooltipRespawn", BinPropertyType.String)]
        public string? TooltipRespawn { get; set; }
        [MetaProperty("mTooltipChatNameOrder", BinPropertyType.String)]
        public string? TooltipChatNameOrder { get; set; }
        [MetaProperty("mTooltipChatNameChaos", BinPropertyType.String)]
        public string? TooltipChatNameChaos { get; set; }
        [MetaProperty("mSourceIcons", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<NeutralTimerSourceIconData>> SourceIcons { get; set; }
    }
    [MetaClass("NeutralTimers")]
    public class NeutralTimers : IMetaClass
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string? SceneName { get; set; }
        [MetaProperty(4002892974, BinPropertyType.String)]
        public string? m4002892974 { get; set; }
        [MetaProperty("mTimers", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<NeutralTimerData>> Timers { get; set; }
    }
    [MetaClass(2733481098)]
    public class Class2733481098 : IMetaClass
    {
        [MetaProperty(1194034797, BinPropertyType.Float)]
        public float? m1194034797 { get; set; }
        [MetaProperty(3019154977, BinPropertyType.UInt32)]
        public uint? m3019154977 { get; set; }
    }
    [MetaClass(2330109623)]
    public class Class2330109623 : IMetaClass
    {
        [MetaProperty(1778722188, BinPropertyType.Hash)]
        public MetaHash? m1778722188 { get; set; }
        [MetaProperty(1179841693, BinPropertyType.Hash)]
        public MetaHash? m1179841693 { get; set; }
        [MetaProperty(3707048936, BinPropertyType.Hash)]
        public MetaHash? m3707048936 { get; set; }
        [MetaProperty(1719874907, BinPropertyType.Hash)]
        public MetaHash? m1719874907 { get; set; }
        [MetaProperty(2427304548, BinPropertyType.Hash)]
        public MetaHash? m2427304548 { get; set; }
        [MetaProperty(3394259485, BinPropertyType.Byte)]
        public byte? m3394259485 { get; set; }
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash? ObjectPath { get; set; }
    }
    [MetaClass("UIButtonState")]
    public class UIButtonState : IMetaClass
    {
        [MetaProperty(120427356, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m120427356 { get; set; }
        [MetaProperty("TextElement", BinPropertyType.ObjectLink)]
        public MetaObjectLink? TextElement { get; set; }
    }
    [MetaClass("UIButtonDefinition")]
    public class UIButtonDefinition : IMetaClass
    {
        [MetaProperty(469001906, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m469001906 { get; set; }
        [MetaProperty(2025490612, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m2025490612 { get; set; }
        [MetaProperty(3167879375, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m3167879375 { get; set; }
        [MetaProperty(2903476354, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m2903476354 { get; set; }
        [MetaProperty(1180059016, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m1180059016 { get; set; }
        [MetaProperty(3146987930, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m3146987930 { get; set; }
        [MetaProperty(1645717439, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonState> m1645717439 { get; set; }
        [MetaProperty(1811093838, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1811093838 { get; set; }
        [MetaProperty(2074868558, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2074868558 { get; set; }
        [MetaProperty(2735039949, BinPropertyType.Structure)]
        public Class547029623 m2735039949 { get; set; }
        [MetaProperty(2351487734, BinPropertyType.String)]
        public string? m2351487734 { get; set; }
        [MetaProperty(1345521803, BinPropertyType.String)]
        public string? m1345521803 { get; set; }
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash? ObjectPath { get; set; }
    }
    [MetaClass("UIButtonAdditionalState")]
    public class UIButtonAdditionalState : IMetaClass
    {
        [MetaProperty(120427356, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m120427356 { get; set; }
    }
    [MetaClass("UIButtonAdditionalElements")]
    public class UIButtonAdditionalElements : IMetaClass
    {
        [MetaProperty(469001906, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m469001906 { get; set; }
        [MetaProperty(2025490612, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m2025490612 { get; set; }
        [MetaProperty(3167879375, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m3167879375 { get; set; }
        [MetaProperty(2903476354, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m2903476354 { get; set; }
        [MetaProperty(1180059016, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m1180059016 { get; set; }
        [MetaProperty(3146987930, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m3146987930 { get; set; }
        [MetaProperty(1645717439, BinPropertyType.Embedded)]
        public MetaEmbedded<UIButtonAdditionalState> m1645717439 { get; set; }
    }
    [MetaClass(547029623)]
    public class Class547029623 : IMetaClass
    {
        [MetaProperty(2774166396, BinPropertyType.String)]
        public string? m2774166396 { get; set; }
        [MetaProperty(554607262, BinPropertyType.String)]
        public string? m554607262 { get; set; }
        [MetaProperty(3629002872, BinPropertyType.String)]
        public string? m3629002872 { get; set; }
        [MetaProperty(72483660, BinPropertyType.String)]
        public string? m72483660 { get; set; }
        [MetaProperty(2226573059, BinPropertyType.String)]
        public string? m2226573059 { get; set; }
        [MetaProperty(3030163781, BinPropertyType.String)]
        public string? m3030163781 { get; set; }
    }
    [MetaClass(3798132414)]
    public class Class3798132414 : IMetaClass
    {
        [MetaProperty(2229790538, BinPropertyType.String)]
        public string? m2229790538 { get; set; }
    }
    [MetaClass(3941835837)]
    public class Class3941835837 : IMetaClass
    {
        [MetaProperty(3751321492, BinPropertyType.Hash)]
        public MetaHash? m3751321492 { get; set; }
        [MetaProperty(2997075516, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2997075516 { get; set; }
        [MetaProperty(55079458, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m55079458 { get; set; }
        [MetaProperty(2621931938, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2621931938 { get; set; }
        [MetaProperty(566876281, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m566876281 { get; set; }
        [MetaProperty(863728340, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m863728340 { get; set; }
        [MetaProperty(246593150, BinPropertyType.Byte)]
        public byte? m246593150 { get; set; }
        [MetaProperty(2735039949, BinPropertyType.Structure)]
        public Class3798132414 m2735039949 { get; set; }
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash? ObjectPath { get; set; }
    }
    [MetaClass(765791391)]
    public class Class765791391 : IMetaClass
    {
        [MetaProperty(3624350102, BinPropertyType.String)]
        public string? m3624350102 { get; set; }
        [MetaProperty(2400858989, BinPropertyType.String)]
        public string? m2400858989 { get; set; }
        [MetaProperty(1943793530, BinPropertyType.String)]
        public string? m1943793530 { get; set; }
    }
    [MetaClass(4073702540)]
    public class Class4073702540 : IMetaClass
    {
        [MetaProperty(2211737984, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2211737984 { get; set; }
        [MetaProperty(4149898573, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4149898573 { get; set; }
    }
    [MetaClass(2642491558)]
    public class Class2642491558 : IMetaClass
    {
        [MetaProperty(3680683107, BinPropertyType.Embedded)]
        public MetaEmbedded<Class4073702540> m3680683107 { get; set; }
        [MetaProperty(39763638, BinPropertyType.Embedded)]
        public MetaEmbedded<Class4073702540> m39763638 { get; set; }
        [MetaProperty(96062416, BinPropertyType.Embedded)]
        public MetaEmbedded<Class4073702540> m96062416 { get; set; }
        [MetaProperty(3035679710, BinPropertyType.Embedded)]
        public MetaEmbedded<Class4073702540> m3035679710 { get; set; }
        [MetaProperty(4212105619, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4212105619 { get; set; }
        [MetaProperty(1034263897, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1034263897 { get; set; }
        [MetaProperty("direction", BinPropertyType.Byte)]
        public byte? Direction { get; set; }
        [MetaProperty(2735039949, BinPropertyType.Structure)]
        public Class765791391 m2735039949 { get; set; }
        [MetaProperty("objectPath", BinPropertyType.Hash)]
        public MetaHash? ObjectPath { get; set; }
    }
    [MetaClass(3009075672)]
    public class Class3009075672 : IMetaClass
    {
        [MetaProperty("items", BinPropertyType.Container)]
        public MetaContainer<MetaHash> Items { get; set; }
        [MetaProperty(4237928370, BinPropertyType.UInt32)]
        public uint? m4237928370 { get; set; }
    }
    [MetaClass(4010129986)]
    public class Class4010129986 : IMetaClass
    {
        [MetaProperty(3708628627, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3708628627 { get; set; }
        [MetaProperty(3141101907, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3141101907 { get; set; }
        [MetaProperty(162672449, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m162672449 { get; set; }
        [MetaProperty(3383292715, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3383292715 { get; set; }
        [MetaProperty(1536603069, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1536603069 { get; set; }
        [MetaProperty(3707442792, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3707442792 { get; set; }
        [MetaProperty(2622578581, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2622578581 { get; set; }
        [MetaProperty(3907959463, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3907959463 { get; set; }
        [MetaProperty(2999482642, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2999482642 { get; set; }
        [MetaProperty(3506185970, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3506185970 { get; set; }
        [MetaProperty(394911362, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m394911362 { get; set; }
        [MetaProperty(1998759362, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1998759362 { get; set; }
        [MetaProperty(807661563, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m807661563 { get; set; }
        [MetaProperty(4086196895, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4086196895 { get; set; }
        [MetaProperty(3538530743, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3538530743 { get; set; }
        [MetaProperty(502750510, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m502750510 { get; set; }
        [MetaProperty(3587103093, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3587103093 { get; set; }
        [MetaProperty(4092291606, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4092291606 { get; set; }
    }
    [MetaClass(2449846901)]
    public class Class2449846901 : Class4010129986
    {
        [MetaProperty(2208006024, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2208006024 { get; set; }
        [MetaProperty(558396809, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m558396809 { get; set; }
        [MetaProperty(1260157044, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1260157044 { get; set; }
        [MetaProperty(2862131044, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2862131044 { get; set; }
        [MetaProperty(1210894143, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1210894143 { get; set; }
        [MetaProperty(1482017448, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m1482017448 { get; set; }
        [MetaProperty(236594122, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m236594122 { get; set; }
        [MetaProperty(3315268305, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3315268305 { get; set; }
        [MetaProperty(163752404, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m163752404 { get; set; }
        [MetaProperty(3331072719, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3331072719 { get; set; }
        [MetaProperty(334720249, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m334720249 { get; set; }
        [MetaProperty(4107456060, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4107456060 { get; set; }
        [MetaProperty(4124233679, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4124233679 { get; set; }
        [MetaProperty(2061681547, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2061681547 { get; set; }
        [MetaProperty(189591720, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m189591720 { get; set; }
        [MetaProperty(4072014996, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m4072014996 { get; set; }
        [MetaProperty(805495209, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m805495209 { get; set; }
        [MetaProperty(3780699960, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3780699960 { get; set; }
        [MetaProperty(3508721437, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3508721437 { get; set; }
    }
    [MetaClass(946411408)]
    public class Class946411408 : IMetaClass
    {
        [MetaProperty(2206191507, BinPropertyType.Container)]
        public MetaContainer<byte> m2206191507 { get; set; }
        [MetaProperty("groupName", BinPropertyType.String)]
        public string? GroupName { get; set; }
    }
    [MetaClass(2696268697)]
    public class Class2696268697 : IMetaClass
    {
        [MetaProperty(1001273705, BinPropertyType.Hash)]
        public MetaHash? m1001273705 { get; set; }
        [MetaProperty(893211270, BinPropertyType.Hash)]
        public MetaHash? m893211270 { get; set; }
        [MetaProperty(514438747, BinPropertyType.Hash)]
        public MetaHash? m514438747 { get; set; }
        [MetaProperty(1498444244, BinPropertyType.Hash)]
        public MetaHash? m1498444244 { get; set; }
        [MetaProperty(1026575442, BinPropertyType.Hash)]
        public MetaHash? m1026575442 { get; set; }
        [MetaProperty(1634417284, BinPropertyType.Hash)]
        public MetaHash? m1634417284 { get; set; }
        [MetaProperty(518100384, BinPropertyType.Hash)]
        public MetaHash? m518100384 { get; set; }
        [MetaProperty(3706895331, BinPropertyType.Hash)]
        public MetaHash? m3706895331 { get; set; }
        [MetaProperty(2852516434, BinPropertyType.Hash)]
        public MetaHash? m2852516434 { get; set; }
        [MetaProperty(1834167418, BinPropertyType.Hash)]
        public MetaHash? m1834167418 { get; set; }
        [MetaProperty(1700556964, BinPropertyType.Hash)]
        public MetaHash? m1700556964 { get; set; }
        [MetaProperty(4055334374, BinPropertyType.Hash)]
        public MetaHash? m4055334374 { get; set; }
        [MetaProperty(3525322849, BinPropertyType.Hash)]
        public MetaHash? m3525322849 { get; set; }
        [MetaProperty(1878129342, BinPropertyType.Hash)]
        public MetaHash? m1878129342 { get; set; }
        [MetaProperty(2717405335, BinPropertyType.Hash)]
        public MetaHash? m2717405335 { get; set; }
        [MetaProperty(1909552450, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m1909552450 { get; set; }
        [MetaProperty(3413797483, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m3413797483 { get; set; }
        [MetaProperty(3018445638, BinPropertyType.Embedded)]
        public MetaEmbedded<Class2330109623> m3018445638 { get; set; }
        [MetaProperty(2040756048, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Class946411408>> m2040756048 { get; set; }
        [MetaProperty(3240068493, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3009075672> m3240068493 { get; set; }
        [MetaProperty(3168417964, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3009075672> m3168417964 { get; set; }
        [MetaProperty(2687402434, BinPropertyType.Embedded)]
        public MetaEmbedded<Class141273677> m2687402434 { get; set; }
        [MetaProperty(1179496250, BinPropertyType.Hash)]
        public MetaHash? m1179496250 { get; set; }
        [MetaProperty(717593223, BinPropertyType.Hash)]
        public MetaHash? m717593223 { get; set; }
        [MetaProperty(1642715352, BinPropertyType.Hash)]
        public MetaHash? m1642715352 { get; set; }
        [MetaProperty(1677099276, BinPropertyType.Hash)]
        public MetaHash? m1677099276 { get; set; }
        [MetaProperty(1016719341, BinPropertyType.Hash)]
        public MetaHash? m1016719341 { get; set; }
        [MetaProperty(1229605698, BinPropertyType.Hash)]
        public MetaHash? m1229605698 { get; set; }
        [MetaProperty(930069533, BinPropertyType.Hash)]
        public MetaHash? m930069533 { get; set; }
        [MetaProperty(1522120281, BinPropertyType.Hash)]
        public MetaHash? m1522120281 { get; set; }
        [MetaProperty(4175508524, BinPropertyType.Hash)]
        public MetaHash? m4175508524 { get; set; }
        [MetaProperty(1771605430, BinPropertyType.Hash)]
        public MetaHash? m1771605430 { get; set; }
        [MetaProperty(1865188340, BinPropertyType.Hash)]
        public MetaHash? m1865188340 { get; set; }
        [MetaProperty(1190420351, BinPropertyType.Bool)]
        public bool? m1190420351 { get; set; }
        [MetaProperty(3435879953, BinPropertyType.Bool)]
        public bool? m3435879953 { get; set; }
        [MetaProperty(2171337895, BinPropertyType.Bool)]
        public bool? m2171337895 { get; set; }
    }
    [MetaClass("IHudLoadingScreenWidget")]
    public interface IHudLoadingScreenWidget : IMetaClass
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        string? SceneName { get; set; }
    }
    [MetaClass("HudLoadingScreenCarouselData")]
    public class HudLoadingScreenCarouselData : IMetaClass
    {
        [MetaProperty("image", BinPropertyType.String)]
        public string? Image { get; set; }
        [MetaProperty(1888172557, BinPropertyType.Byte)]
        public byte? m1888172557 { get; set; }
    }
    [MetaClass("HudLoadingScreenWidgetCarousel")]
    public class HudLoadingScreenWidgetCarousel : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string? SceneName { get; set; }
        [MetaProperty(2065649608, BinPropertyType.Byte)]
        public byte? m2065649608 { get; set; }
        [MetaProperty(878940594, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m878940594 { get; set; }
    }
    [MetaClass(1107815263)]
    public class Class1107815263 : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string? SceneName { get; set; }
    }
    [MetaClass("HudLoadingScreenWidgetPing")]
    public class HudLoadingScreenWidgetPing : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string? SceneName { get; set; }
        [MetaProperty("mDebugPing", BinPropertyType.UInt32)]
        public uint? DebugPing { get; set; }
        [MetaProperty("mPingThresholdGreen", BinPropertyType.UInt32)]
        public uint? PingThresholdGreen { get; set; }
        [MetaProperty("mPingThresholdYellow", BinPropertyType.UInt32)]
        public uint? PingThresholdYellow { get; set; }
        [MetaProperty("mPingThresholdOrange", BinPropertyType.UInt32)]
        public uint? PingThresholdOrange { get; set; }
        [MetaProperty("mPingThresholdRed", BinPropertyType.UInt32)]
        public uint? PingThresholdRed { get; set; }
    }
    [MetaClass("HudLoadingScreenWidgetPlayers")]
    public class HudLoadingScreenWidgetPlayers : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string? SceneName { get; set; }
        [MetaProperty(4101352720, BinPropertyType.Embedded)]
        public MetaEmbedded<PlayerCardWidgetConfig> m4101352720 { get; set; }
    }
    [MetaClass("HudLoadingScreenWidgetProgressBar")]
    public class HudLoadingScreenWidgetProgressBar : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string? SceneName { get; set; }
    }
    [MetaClass(4123015996)]
    public class Class4123015996 : IHudLoadingScreenWidget
    {
        [MetaProperty("mSceneName", BinPropertyType.String)]
        public string? SceneName { get; set; }
    }
    [MetaClass("PlayerCardWidgetConfig")]
    public class PlayerCardWidgetConfig : IMetaClass
    {
        [MetaProperty(3602094040, BinPropertyType.Bool)]
        public bool? m3602094040 { get; set; }
        [MetaProperty(1497660678, BinPropertyType.Byte)]
        public byte? m1497660678 { get; set; }
        [MetaProperty(2073776835, BinPropertyType.UInt32)]
        public uint? m2073776835 { get; set; }
    }
    [MetaClass(2539231955)]
    public class Class2539231955 : IMetaClass
    {
        [MetaProperty(1665946782, BinPropertyType.Hash)]
        public MetaHash? m1665946782 { get; set; }
        [MetaProperty(697394402, BinPropertyType.Hash)]
        public MetaHash? m697394402 { get; set; }
    }
    [MetaClass("HudReplaySliderIconData")]
    public class HudReplaySliderIconData : IMetaClass
    {
        [MetaProperty("mType", BinPropertyType.Hash)]
        public MetaHash? Type { get; set; }
        [MetaProperty("mTooltipStyle", BinPropertyType.Byte)]
        public byte? TooltipStyle { get; set; }
        [MetaProperty("mElementName", BinPropertyType.String)]
        public string? ElementName { get; set; }
        [MetaProperty("mElementSpacer", BinPropertyType.Float)]
        public float? ElementSpacer { get; set; }
        [MetaProperty("mElementAlphaDefault", BinPropertyType.Float)]
        public float? ElementAlphaDefault { get; set; }
        [MetaProperty("mElementAlphaSelected", BinPropertyType.Float)]
        public float? ElementAlphaSelected { get; set; }
        [MetaProperty("mElementAlphaUnselected", BinPropertyType.Float)]
        public float? ElementAlphaUnselected { get; set; }
        [MetaProperty("mTooltipIconNames", BinPropertyType.Container)]
        public MetaContainer<string> TooltipIconNames { get; set; }
    }
    [MetaClass("HudReplaySliderData")]
    public class HudReplaySliderData : IMetaClass
    {
        [MetaProperty("mIconDataPriorityList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<HudReplaySliderIconData>> IconDataPriorityList { get; set; }
        [MetaProperty("mTooltipEventWindow", BinPropertyType.Float)]
        public float? TooltipEventWindow { get; set; }
    }
    [MetaClass("WadFileDescriptor")]
    public interface WadFileDescriptor : IMetaClass
    {
    }
    [MetaClass("EsportsBannerMaterialController")]
    public class EsportsBannerMaterialController : SkinnedMeshDataMaterialController
    {
    }
    [MetaClass("BaseRigPoseModifierData")]
    public interface BaseRigPoseModifierData : IMetaClass
    {
    }
    [MetaClass("ConformToPathRigPoseModifierData")]
    public class ConformToPathRigPoseModifierData : BaseRigPoseModifierData
    {
        [MetaProperty("mStartingJointName", BinPropertyType.Hash)]
        public MetaHash? StartingJointName { get; set; }
        [MetaProperty("mEndingJointName", BinPropertyType.Hash)]
        public MetaHash? EndingJointName { get; set; }
        [MetaProperty("mDefaultMaskName", BinPropertyType.Hash)]
        public MetaHash? DefaultMaskName { get; set; }
        [MetaProperty("mMaxBoneAngle", BinPropertyType.Float)]
        public float? MaxBoneAngle { get; set; }
        [MetaProperty("mDampingValue", BinPropertyType.Float)]
        public float? DampingValue { get; set; }
        [MetaProperty("mVelMultiplier", BinPropertyType.Float)]
        public float? VelMultiplier { get; set; }
        [MetaProperty("mFrequency", BinPropertyType.Float)]
        public float? Frequency { get; set; }
    }
    [MetaClass("JointSnapRigPoseModifilerData")]
    public class JointSnapRigPoseModifilerData : BaseRigPoseModifierData
    {
    }
    [MetaClass("LockRootOrientationRigPoseModifierData")]
    public class LockRootOrientationRigPoseModifierData : BaseRigPoseModifierData
    {
    }
    [MetaClass("SyncedAnimationRigPoseModifierData")]
    public class SyncedAnimationRigPoseModifierData : BaseRigPoseModifierData
    {
    }
    [MetaClass("VertexAnimationRigPoseModifierData")]
    public class VertexAnimationRigPoseModifierData : BaseRigPoseModifierData
    {
        [MetaProperty("mMaxSpeed", BinPropertyType.Float)]
        public float? MaxSpeed { get; set; }
        [MetaProperty("mStiffness", BinPropertyType.Float)]
        public float? Stiffness { get; set; }
        [MetaProperty("mMass", BinPropertyType.Float)]
        public float? Mass { get; set; }
        [MetaProperty("mDamping", BinPropertyType.Float)]
        public float? Damping { get; set; }
    }
    [MetaClass("AnimationGraphData")]
    public class AnimationGraphData : IMetaClass
    {
        [MetaProperty("mUseCascadeBlend", BinPropertyType.Bool)]
        public bool? UseCascadeBlend { get; set; }
        [MetaProperty("mCascadeBlendValue", BinPropertyType.Float)]
        public float? CascadeBlendValue { get; set; }
        [MetaProperty("mClipDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, ClipBaseData> ClipDataMap { get; set; }
        [MetaProperty("mMaskDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<MaskData>> MaskDataMap { get; set; }
        [MetaProperty("mTrackDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<TrackData>> TrackDataMap { get; set; }
        [MetaProperty("mSyncGroupDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<SyncGroupData>> SyncGroupDataMap { get; set; }
        [MetaProperty("mBlendDataTable", BinPropertyType.Map)]
        public Dictionary<ulong, BaseBlendData> BlendDataTable { get; set; }
    }
    [MetaClass("AnimationResourceData")]
    public class AnimationResourceData : IMetaClass
    {
        [MetaProperty("mAnimationFilePath", BinPropertyType.String)]
        public string? AnimationFilePath { get; set; }
    }
    [MetaClass("MaskData")]
    public class MaskData : IMetaClass
    {
        [MetaProperty("mId", BinPropertyType.UInt32)]
        public uint? Id { get; set; }
        [MetaProperty("mWeightList", BinPropertyType.Container)]
        public MetaContainer<float> WeightList { get; set; }
    }
    [MetaClass("Joint")]
    public class Joint : IMetaClass
    {
        [MetaProperty("mIndex", BinPropertyType.UInt16)]
        public ushort? Index { get; set; }
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mNameHash", BinPropertyType.UInt32)]
        public uint? NameHash { get; set; }
        [MetaProperty("mFlags", BinPropertyType.UInt16)]
        public ushort? Flags { get; set; }
        [MetaProperty("mRadius", BinPropertyType.Float)]
        public float? Radius { get; set; }
        [MetaProperty("mParentIndex", BinPropertyType.Int16)]
        public short? ParentIndex { get; set; }
    }
    [MetaClass("RigResource")]
    public class RigResource : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mAssetName", BinPropertyType.String)]
        public string? AssetName { get; set; }
        [MetaProperty("mFlags", BinPropertyType.UInt16)]
        public ushort? Flags { get; set; }
        [MetaProperty("mJointList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<Joint>> JointList { get; set; }
        [MetaProperty("mShaderJointList", BinPropertyType.Container)]
        public MetaContainer<ushort> ShaderJointList { get; set; }
    }
    [MetaClass("SyncGroupData")]
    public class SyncGroupData : IMetaClass
    {
        [MetaProperty("mType", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
    }
    [MetaClass("TrackData")]
    public class TrackData : IMetaClass
    {
        [MetaProperty("mPriority", BinPropertyType.UInt32)]
        public uint? Priority { get; set; }
        [MetaProperty("mBlendWeight", BinPropertyType.Float)]
        public float? BlendWeight { get; set; }
        [MetaProperty("mBlendMode", BinPropertyType.UInt32)]
        public uint? BlendMode { get; set; }
    }
    [MetaClass("UpdaterData")]
    public class UpdaterData : IMetaClass
    {
        [MetaProperty("mInputType", BinPropertyType.UInt32)]
        public uint? InputType { get; set; }
        [MetaProperty("mOutputType", BinPropertyType.UInt32)]
        public uint? OutputType { get; set; }
        [MetaProperty("mValueProcessorDataList", BinPropertyType.Container)]
        public MetaContainer<ValueProcessorData> ValueProcessorDataList { get; set; }
    }
    [MetaClass("UpdaterResourceData")]
    public class UpdaterResourceData : IMetaClass
    {
        [MetaProperty("mUpdaterDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<UpdaterData>> UpdaterDataList { get; set; }
    }
    [MetaClass("AtomicClipData")]
    public class AtomicClipData : BlendableClipData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        public MetaHash? MaskDataName { get; set; }
        [MetaProperty("mTrackDataName", BinPropertyType.Hash)]
        public MetaHash? TrackDataName { get; set; }
        [MetaProperty("mSyncGroupDataName", BinPropertyType.Hash)]
        public MetaHash? SyncGroupDataName { get; set; }
        [MetaProperty("mEventDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, BaseEventData> EventDataMap { get; set; }
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mTickDuration", BinPropertyType.Float)]
        public float? TickDuration { get; set; }
        [MetaProperty("mAnimationResourceData", BinPropertyType.Embedded)]
        public MetaEmbedded<AnimationResourceData> AnimationResourceData { get; set; }
        [MetaProperty("mUpdaterResourceData", BinPropertyType.Structure)]
        public UpdaterResourceData UpdaterResourceData { get; set; }
    }
    [MetaClass("BlendableClipData")]
    public interface BlendableClipData : ClipBaseData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        MetaHash? MaskDataName { get; set; }
        [MetaProperty("mTrackDataName", BinPropertyType.Hash)]
        MetaHash? TrackDataName { get; set; }
        [MetaProperty("mSyncGroupDataName", BinPropertyType.Hash)]
        MetaHash? SyncGroupDataName { get; set; }
        [MetaProperty("mEventDataMap", BinPropertyType.Map)]
        Dictionary<MetaHash, BaseEventData> EventDataMap { get; set; }
    }
    [MetaClass("ConditionBoolClipData")]
    public class ConditionBoolClipData : ClipBaseData
    {
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mUpdaterType", BinPropertyType.UInt32)]
        public uint? UpdaterType { get; set; }
        [MetaProperty("mChangeAnimationMidPlay", BinPropertyType.Bool)]
        public bool? ChangeAnimationMidPlay { get; set; }
        [MetaProperty(836456042, BinPropertyType.Bool)]
        public bool? m836456042 { get; set; }
        [MetaProperty("mTrueConditionClipName", BinPropertyType.Hash)]
        public MetaHash? TrueConditionClipName { get; set; }
        [MetaProperty("mFalseConditionClipName", BinPropertyType.Hash)]
        public MetaHash? FalseConditionClipName { get; set; }
        [MetaProperty("mChildAnimDelaySwitchTime", BinPropertyType.Float)]
        public float? ChildAnimDelaySwitchTime { get; set; }
    }
    [MetaClass("ConditionFloatPairData")]
    public class ConditionFloatPairData : IMetaClass
    {
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash? ClipName { get; set; }
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float? Value { get; set; }
        [MetaProperty("mHoldAnimationToHigher", BinPropertyType.Float)]
        public float? HoldAnimationToHigher { get; set; }
        [MetaProperty("mHoldAnimationToLower", BinPropertyType.Float)]
        public float? HoldAnimationToLower { get; set; }
    }
    [MetaClass("ConditionFloatClipData")]
    public class ConditionFloatClipData : ClipBaseData
    {
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mConditionFloatPairDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ConditionFloatPairData>> ConditionFloatPairDataList { get; set; }
        [MetaProperty("mUpdaterType", BinPropertyType.UInt32)]
        public uint? UpdaterType { get; set; }
        [MetaProperty("mChangeAnimationMidPlay", BinPropertyType.Bool)]
        public bool? ChangeAnimationMidPlay { get; set; }
        [MetaProperty(836456042, BinPropertyType.Bool)]
        public bool? m836456042 { get; set; }
        [MetaProperty("mChildAnimDelaySwitchTime", BinPropertyType.Float)]
        public float? ChildAnimDelaySwitchTime { get; set; }
    }
    [MetaClass("ParallelClipData")]
    public class ParallelClipData : ClipBaseData
    {
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mClipNameList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ClipNameList { get; set; }
    }
    [MetaClass("ParametricPairData")]
    public class ParametricPairData : IMetaClass
    {
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash? ClipName { get; set; }
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float? Value { get; set; }
    }
    [MetaClass("ParametricClipData")]
    public class ParametricClipData : BlendableClipData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        public MetaHash? MaskDataName { get; set; }
        [MetaProperty("mTrackDataName", BinPropertyType.Hash)]
        public MetaHash? TrackDataName { get; set; }
        [MetaProperty("mSyncGroupDataName", BinPropertyType.Hash)]
        public MetaHash? SyncGroupDataName { get; set; }
        [MetaProperty("mEventDataMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, BaseEventData> EventDataMap { get; set; }
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mUpdaterType", BinPropertyType.UInt32)]
        public uint? UpdaterType { get; set; }
        [MetaProperty("mParametricPairDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ParametricPairData>> ParametricPairDataList { get; set; }
    }
    [MetaClass("SelectorPairData")]
    public class SelectorPairData : IMetaClass
    {
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash? ClipName { get; set; }
        [MetaProperty("mProbability", BinPropertyType.Float)]
        public float? Probability { get; set; }
    }
    [MetaClass("SelectorClipData")]
    public class SelectorClipData : ClipBaseData
    {
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mSelectorPairDataList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SelectorPairData>> SelectorPairDataList { get; set; }
    }
    [MetaClass("SequencerClipData")]
    public class SequencerClipData : ClipBaseData
    {
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        public uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        public MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
        [MetaProperty("mClipNameList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ClipNameList { get; set; }
    }
    [MetaClass("BaseBlendData")]
    public interface BaseBlendData : IMetaClass
    {
    }
    [MetaClass("BaseEventData")]
    public class BaseEventData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.Hash)]
        public MetaHash? Name { get; set; }
        [MetaProperty("mStartFrame", BinPropertyType.Float)]
        public float? StartFrame { get; set; }
        [MetaProperty("mEndFrame", BinPropertyType.Float)]
        public float? EndFrame { get; set; }
        [MetaProperty("mIsSelfOnly", BinPropertyType.Bool)]
        public bool? IsSelfOnly { get; set; }
        [MetaProperty("mFireIfAnimationEndsEarly", BinPropertyType.Bool)]
        public bool? FireIfAnimationEndsEarly { get; set; }
    }
    [MetaClass("ClipBaseData")]
    public interface ClipBaseData : IMetaClass
    {
        [MetaProperty("mFlags", BinPropertyType.UInt32)]
        uint? Flags { get; set; }
        [MetaProperty("mAnimationInterruptionGroupNames", BinPropertyType.Container)]
        MetaContainer<MetaHash> AnimationInterruptionGroupNames { get; set; }
    }
    [MetaClass("ConformToPathEventData")]
    public class ConformToPathEventData : BaseEventData
    {
        [MetaProperty("mMaskDataName", BinPropertyType.Hash)]
        public MetaHash? MaskDataName { get; set; }
        [MetaProperty("mBlendInTime", BinPropertyType.Float)]
        public float? BlendInTime { get; set; }
        [MetaProperty("mBlendOutTime", BinPropertyType.Float)]
        public float? BlendOutTime { get; set; }
    }
    [MetaClass("EnableLookAtEventData")]
    public class EnableLookAtEventData : BaseEventData
    {
        [MetaProperty("mEnableLookAt", BinPropertyType.Bool)]
        public bool? EnableLookAt { get; set; }
        [MetaProperty("mLockCurrentValues", BinPropertyType.Bool)]
        public bool? LockCurrentValues { get; set; }
    }
    [MetaClass("FaceCameraEventData")]
    public class FaceCameraEventData : BaseEventData
    {
        [MetaProperty(3117400491, BinPropertyType.Float)]
        public float? m3117400491 { get; set; }
        [MetaProperty(2577320358, BinPropertyType.Float)]
        public float? m2577320358 { get; set; }
        [MetaProperty(2367322749, BinPropertyType.Float)]
        public float? m2367322749 { get; set; }
    }
    [MetaClass("FadeEventData")]
    public class FadeEventData : BaseEventData
    {
        [MetaProperty("mTimeToFade", BinPropertyType.Float)]
        public float? TimeToFade { get; set; }
        [MetaProperty("mTargetAlpha", BinPropertyType.Float)]
        public float? TargetAlpha { get; set; }
    }
    [MetaClass("IdleParticlesVisibilityEventData")]
    public class IdleParticlesVisibilityEventData : BaseEventData
    {
        [MetaProperty("mShow", BinPropertyType.Bool)]
        public bool? Show { get; set; }
    }
    [MetaClass("JointSnapEventData")]
    public class JointSnapEventData : BaseEventData
    {
        [MetaProperty("mJointNameToOverride", BinPropertyType.Hash)]
        public MetaHash? JointNameToOverride { get; set; }
        [MetaProperty("mJointNameToSnapTo", BinPropertyType.Hash)]
        public MetaHash? JointNameToSnapTo { get; set; }
    }
    [MetaClass("LinearTransformProcessorData")]
    public class LinearTransformProcessorData : ValueProcessorData
    {
        [MetaProperty("mMultiplier", BinPropertyType.Float)]
        public float? Multiplier { get; set; }
        [MetaProperty("mIncrement", BinPropertyType.Float)]
        public float? Increment { get; set; }
    }
    [MetaClass("LockRootOrientationEventData")]
    public class LockRootOrientationEventData : BaseEventData
    {
    }
    [MetaClass("ParticleEventDataPair")]
    public class ParticleEventDataPair : IMetaClass
    {
        [MetaProperty("mBoneName", BinPropertyType.Hash)]
        public MetaHash? BoneName { get; set; }
        [MetaProperty("mTargetBoneName", BinPropertyType.Hash)]
        public MetaHash? TargetBoneName { get; set; }
    }
    [MetaClass("ParticleEventData")]
    public class ParticleEventData : BaseEventData
    {
        [MetaProperty("mEffectKey", BinPropertyType.Hash)]
        public MetaHash? EffectKey { get; set; }
        [MetaProperty("mEnemyEffectKey", BinPropertyType.Hash)]
        public MetaHash? EnemyEffectKey { get; set; }
        [MetaProperty("mEffectName", BinPropertyType.String)]
        public string? EffectName { get; set; }
        [MetaProperty("mParticleEventDataPairList", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ParticleEventDataPair>> ParticleEventDataPairList { get; set; }
        [MetaProperty("mIsLoop", BinPropertyType.Bool)]
        public bool? IsLoop { get; set; }
        [MetaProperty("mIsKillEvent", BinPropertyType.Bool)]
        public bool? IsKillEvent { get; set; }
        [MetaProperty("mIsDetachable", BinPropertyType.Bool)]
        public bool? IsDetachable { get; set; }
        [MetaProperty("mScalePlaySpeedWithAnimation", BinPropertyType.Bool)]
        public bool? ScalePlaySpeedWithAnimation { get; set; }
        [MetaProperty(2743230979, BinPropertyType.Bool)]
        public bool? m2743230979 { get; set; }
    }
    [MetaClass("SoundEventData")]
    public class SoundEventData : BaseEventData
    {
        [MetaProperty("mSoundName", BinPropertyType.String)]
        public string? SoundName { get; set; }
        [MetaProperty("mIsLoop", BinPropertyType.Bool)]
        public bool? IsLoop { get; set; }
        [MetaProperty("mIsKillEvent", BinPropertyType.Bool)]
        public bool? IsKillEvent { get; set; }
        [MetaProperty(108144598, BinPropertyType.Bool)]
        public bool? m108144598 { get; set; }
    }
    [MetaClass("StopAnimationEventData")]
    public class StopAnimationEventData : BaseEventData
    {
        [MetaProperty("mStopAnimationName", BinPropertyType.Hash)]
        public MetaHash? StopAnimationName { get; set; }
    }
    [MetaClass("SubmeshVisibilityEventData")]
    public class SubmeshVisibilityEventData : BaseEventData
    {
        [MetaProperty("mShowSubmeshList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> ShowSubmeshList { get; set; }
        [MetaProperty("mHideSubmeshList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> HideSubmeshList { get; set; }
    }
    [MetaClass("SyncedAnimationEventData")]
    public class SyncedAnimationEventData : BaseEventData
    {
        [MetaProperty("mLerpTime", BinPropertyType.Float)]
        public float? LerpTime { get; set; }
    }
    [MetaClass("TimeBlendData")]
    public class TimeBlendData : BaseBlendData
    {
        [MetaProperty("mTime", BinPropertyType.Float)]
        public float? Time { get; set; }
    }
    [MetaClass("TransitionClipBlendData")]
    public class TransitionClipBlendData : BaseBlendData
    {
        [MetaProperty("mClipName", BinPropertyType.Hash)]
        public MetaHash? ClipName { get; set; }
    }
    [MetaClass("ValueProcessorData")]
    public class ValueProcessorData : IMetaClass
    {
    }
    [MetaClass("EngineFeatureToggles")]
    public class EngineFeatureToggles : IMetaClass
    {
        [MetaProperty(2699198108, BinPropertyType.Bool)]
        public bool? m2699198108 { get; set; }
        [MetaProperty(771509602, BinPropertyType.Bool)]
        public bool? m771509602 { get; set; }
        [MetaProperty(650098706, BinPropertyType.Bool)]
        public bool? m650098706 { get; set; }
        [MetaProperty(100560457, BinPropertyType.Bool)]
        public bool? m100560457 { get; set; }
    }
    [MetaClass(3233830641)]
    public class Class3233830641 : GenericMapPlaceable
    {
        [MetaProperty("PropName", BinPropertyType.String)]
        public string? PropName { get; set; }
        [MetaProperty(2382726993, BinPropertyType.Bool)]
        public bool? m2382726993 { get; set; }
        [MetaProperty(1947732752, BinPropertyType.String)]
        public string? m1947732752 { get; set; }
        [MetaProperty("eyeCandy", BinPropertyType.Bool)]
        public bool? EyeCandy { get; set; }
        [MetaProperty("SkinID", BinPropertyType.UInt32)]
        public uint? SkinID { get; set; }
        [MetaProperty("quality", BinPropertyType.Int32)]
        public int? Quality { get; set; }
        [MetaProperty(2880359499, BinPropertyType.Bool)]
        public bool? m2880359499 { get; set; }
    }
    [MetaClass("MapCamera")]
    public class MapCamera : MapPlaceable
    {
        [MetaProperty(1866351399, BinPropertyType.Float)]
        public float? m1866351399 { get; set; }
        [MetaProperty(1446648129, BinPropertyType.Float)]
        public float? m1446648129 { get; set; }
        [MetaProperty("pitch", BinPropertyType.Float)]
        public float? Pitch { get; set; }
        [MetaProperty("yaw", BinPropertyType.Float)]
        public float? Yaw { get; set; }
        [MetaProperty("FieldOfView", BinPropertyType.Float)]
        public float? FieldOfView { get; set; }
    }
    [MetaClass("MapComponent")]
    public interface MapComponent : IMetaClass
    {
    }
    [MetaClass("MapContainer")]
    public class MapContainer : IMetaClass
    {
        [MetaProperty("mapPath", BinPropertyType.String)]
        public string? MapPath { get; set; }
        [MetaProperty("components", BinPropertyType.Container)]
        public MetaContainer<MapComponent> Components { get; set; }
        [MetaProperty("boundsMin", BinPropertyType.Vector2)]
        public Vector2? BoundsMin { get; set; }
        [MetaProperty("boundsMax", BinPropertyType.Vector2)]
        public Vector2? BoundsMax { get; set; }
        [MetaProperty("lowestWalkableHeight", BinPropertyType.Float)]
        public float? LowestWalkableHeight { get; set; }
        [MetaProperty(4027637499, BinPropertyType.Float)]
        public float? m4027637499 { get; set; }
        [MetaProperty("chunks", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> Chunks { get; set; }
    }
    [MetaClass("MapContainsOtherMaps")]
    public class MapContainsOtherMaps : MapComponent
    {
        [MetaProperty(3704860912, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m3704860912 { get; set; }
    }
    [MetaClass("LaneData")]
    public class LaneData : IMetaClass
    {
        [MetaProperty(14980125, BinPropertyType.Container)]
        public MetaContainer<string> m14980125 { get; set; }
        [MetaProperty(3967407862, BinPropertyType.Container)]
        public MetaContainer<string> m3967407862 { get; set; }
    }
    [MetaClass("MapLaneComponent")]
    public class MapLaneComponent : MapComponent
    {
        [MetaProperty(2327119739, BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<LaneData>> m2327119739 { get; set; }
    }
    [MetaClass("MapLocator")]
    public class MapLocator : GenericMapPlaceable
    {
    }
    [MetaClass("MapNavGrid")]
    public class MapNavGrid : MapComponent
    {
        [MetaProperty(4145642937, BinPropertyType.String)]
        public string? m4145642937 { get; set; }
    }
    [MetaClass("MapPlaceable")]
    public class MapPlaceable : IMetaClass
    {
        [MetaProperty("transform", BinPropertyType.Matrix44)]
        public R3DMatrix44? Transform { get; set; }
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(3438777127, BinPropertyType.Byte)]
        public byte? m3438777127 { get; set; }
    }
    [MetaClass("GenericMapPlaceable")]
    public class GenericMapPlaceable : MapPlaceable
    {
    }
    [MetaClass("MapPlaceableContainer")]
    public class MapPlaceableContainer : IMetaClass
    {
        [MetaProperty("items", BinPropertyType.Map)]
        public Dictionary<MetaHash, MapPlaceable> Items { get; set; }
    }
    [MetaClass("MapPrefabInstance")]
    public class MapPrefabInstance : MapPlaceable
    {
        [MetaProperty(2120627422, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2120627422 { get; set; }
    }
    [MetaClass(3750744125)]
    public class Class3750744125 : MapComponent
    {
        [MetaProperty(3546830876, BinPropertyType.String)]
        public string? m3546830876 { get; set; }
        [MetaProperty(1036822818, BinPropertyType.String)]
        public string? m1036822818 { get; set; }
    }
    [MetaClass("RegionsThatAllowContent")]
    public class RegionsThatAllowContent : IMetaClass
    {
        [MetaProperty("mRegionList", BinPropertyType.Container)]
        public MetaContainer<MetaHash> RegionList { get; set; }
    }
    [MetaClass("RegionSettings")]
    public class RegionSettings : IMetaClass
    {
        [MetaProperty("mContentTypeAllowedSettings", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaEmbedded<RegionsThatAllowContent>> ContentTypeAllowedSettings { get; set; }
    }
    [MetaClass("FontLocaleType")]
    public class FontLocaleType : IMetaClass
    {
        [MetaProperty("localeName", BinPropertyType.String)]
        public string? LocaleName { get; set; }
        [MetaProperty(2271524388, BinPropertyType.String)]
        public string? m2271524388 { get; set; }
        [MetaProperty(281301214, BinPropertyType.String)]
        public string? m281301214 { get; set; }
        [MetaProperty(1802990466, BinPropertyType.String)]
        public string? m1802990466 { get; set; }
    }
    [MetaClass("FontResolution")]
    public class FontResolution : IMetaClass
    {
        [MetaProperty("screenHeight", BinPropertyType.UInt32)]
        public uint? ScreenHeight { get; set; }
        [MetaProperty("fontSize", BinPropertyType.UInt32)]
        public uint? FontSize { get; set; }
        [MetaProperty("outlineSize", BinPropertyType.UInt32)]
        public uint? OutlineSize { get; set; }
        [MetaProperty("shadowDepthX", BinPropertyType.Int32)]
        public int? ShadowDepthX { get; set; }
        [MetaProperty("shadowDepthY", BinPropertyType.Int32)]
        public int? ShadowDepthY { get; set; }
    }
    [MetaClass("FontLocaleResolutions")]
    public class FontLocaleResolutions : IMetaClass
    {
        [MetaProperty("localeName", BinPropertyType.String)]
        public string? LocaleName { get; set; }
        [MetaProperty("resolutions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FontResolution>> Resolutions { get; set; }
    }
    [MetaClass("FontType")]
    public class FontType : IMetaClass
    {
        [MetaProperty("localeTypes", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FontLocaleType>> LocaleTypes { get; set; }
    }
    [MetaClass("FontResolutionData")]
    public class FontResolutionData : IMetaClass
    {
        [MetaProperty("autoScale", BinPropertyType.Bool)]
        public bool? AutoScale { get; set; }
        [MetaProperty("localeResolutions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<FontLocaleResolutions>> LocaleResolutions { get; set; }
    }
    [MetaClass("GameFontDescription")]
    public class GameFontDescription : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("typeData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? TypeData { get; set; }
        [MetaProperty("resolutionData", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ResolutionData { get; set; }
        [MetaProperty("color", BinPropertyType.Color)]
        public Color? Color { get; set; }
        [MetaProperty("outlineColor", BinPropertyType.Color)]
        public Color? OutlineColor { get; set; }
        [MetaProperty("shadowColor", BinPropertyType.Color)]
        public Color? ShadowColor { get; set; }
        [MetaProperty("glowColor", BinPropertyType.Color)]
        public Color? GlowColor { get; set; }
        [MetaProperty("colorblindColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindColor { get; set; }
        [MetaProperty("colorblindOutlineColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindOutlineColor { get; set; }
        [MetaProperty("colorblindShadowColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindShadowColor { get; set; }
        [MetaProperty("colorblindGlowColor", BinPropertyType.Optional)]
        public MetaOptional<Color> ColorblindGlowColor { get; set; }
        [MetaProperty("fillTextureName", BinPropertyType.String)]
        public string? FillTextureName { get; set; }
    }
    [MetaClass("TooltipFormat")]
    public class TooltipFormat : IMetaClass
    {
        [MetaProperty("mObjectName", BinPropertyType.String)]
        public string? ObjectName { get; set; }
        [MetaProperty("mInputLocKeysWithDefaults", BinPropertyType.Map)]
        public Dictionary<string, string> InputLocKeysWithDefaults { get; set; }
        [MetaProperty("mListNames", BinPropertyType.Container)]
        public MetaContainer<string> ListNames { get; set; }
        [MetaProperty("mListTypeChoices", BinPropertyType.Map)]
        public Dictionary<string, string> ListTypeChoices { get; set; }
        [MetaProperty(991429346, BinPropertyType.Map)]
        public Dictionary<uint, string> m991429346 { get; set; }
        [MetaProperty("mUsesListValues", BinPropertyType.Bool)]
        public bool? UsesListValues { get; set; }
        [MetaProperty("mListValueSeparator", BinPropertyType.String)]
        public string? ListValueSeparator { get; set; }
        [MetaProperty("mListGridPrefix", BinPropertyType.String)]
        public string? ListGridPrefix { get; set; }
        [MetaProperty("mListGridPostfix", BinPropertyType.String)]
        public string? ListGridPostfix { get; set; }
        [MetaProperty("mListGridSeparator", BinPropertyType.String)]
        public string? ListGridSeparator { get; set; }
        [MetaProperty("mOutputStrings", BinPropertyType.Map)]
        public Dictionary<string, string> OutputStrings { get; set; }
    }
    [MetaClass("TooltipInstanceListElement")]
    public class TooltipInstanceListElement : IMetaClass
    {
        [MetaProperty("type", BinPropertyType.String)]
        public string? Type { get; set; }
        [MetaProperty("typeIndex", BinPropertyType.Int32)]
        public int? TypeIndex { get; set; }
        [MetaProperty("multiplier", BinPropertyType.Float)]
        public float? Multiplier { get; set; }
        [MetaProperty("nameOverride", BinPropertyType.String)]
        public string? NameOverride { get; set; }
        [MetaProperty("Style", BinPropertyType.UInt32)]
        public uint? Style { get; set; }
    }
    [MetaClass("TooltipInstanceList")]
    public class TooltipInstanceList : IMetaClass
    {
        [MetaProperty("levelCount", BinPropertyType.UInt32)]
        public uint? LevelCount { get; set; }
        [MetaProperty("elements", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<TooltipInstanceListElement>> Elements { get; set; }
    }
    [MetaClass("TooltipInstance")]
    public class TooltipInstance : IMetaClass
    {
        [MetaProperty("mObjectName", BinPropertyType.String)]
        public string? ObjectName { get; set; }
        [MetaProperty("mFormat", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Format { get; set; }
        [MetaProperty("mLocKeys", BinPropertyType.Map)]
        public Dictionary<string, string> LocKeys { get; set; }
        [MetaProperty("mLists", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<TooltipInstanceList>> Lists { get; set; }
    }
    [MetaClass("CSSStyle")]
    public class CSSStyle : IMetaClass
    {
        [MetaProperty("color", BinPropertyType.Optional)]
        public MetaOptional<Color> Color { get; set; }
        [MetaProperty("bold", BinPropertyType.Optional)]
        public MetaOptional<bool> Bold { get; set; }
        [MetaProperty("italics", BinPropertyType.Optional)]
        public MetaOptional<bool> Italics { get; set; }
        [MetaProperty("underline", BinPropertyType.Optional)]
        public MetaOptional<bool> Underline { get; set; }
    }
    [MetaClass("CSSIcon")]
    public class CSSIcon : IMetaClass
    {
        [MetaProperty("xy", BinPropertyType.Vector2)]
        public Vector2? Xy { get; set; }
        [MetaProperty("wh", BinPropertyType.Vector2)]
        public Vector2? Wh { get; set; }
        [MetaProperty(2179074287, BinPropertyType.Float)]
        public float? m2179074287 { get; set; }
    }
    [MetaClass("CSSSheet")]
    public class CSSSheet : IMetaClass
    {
        [MetaProperty("styles", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<CSSStyle>> Styles { get; set; }
        [MetaProperty("iconTexture", BinPropertyType.String)]
        public string? IconTexture { get; set; }
        [MetaProperty("icons", BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<CSSIcon>> Icons { get; set; }
    }
    [MetaClass("ValueFloat")]
    public class ValueFloat : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Float)]
        public float? ConstantValue { get; set; }
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedFloatVariableData Dynamics { get; set; }
    }
    [MetaClass("IntegratedValueFloat")]
    public class IntegratedValueFloat : ValueFloat
    {
    }
    [MetaClass("ValueVector2")]
    public class ValueVector2 : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Vector2)]
        public Vector2? ConstantValue { get; set; }
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedVector2fVariableData Dynamics { get; set; }
    }
    [MetaClass("IntegratedValueVector2")]
    public class IntegratedValueVector2 : ValueVector2
    {
    }
    [MetaClass("ValueVector3")]
    public class ValueVector3 : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Vector3)]
        public Vector3? ConstantValue { get; set; }
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedVector3fVariableData Dynamics { get; set; }
    }
    [MetaClass("IntegratedValueVector3")]
    public class IntegratedValueVector3 : ValueVector3
    {
    }
    [MetaClass("ValueColor")]
    public class ValueColor : IMetaClass
    {
        [MetaProperty("constantValue", BinPropertyType.Vector4)]
        public Vector4? ConstantValue { get; set; }
        [MetaProperty("dynamics", BinPropertyType.Structure)]
        public VfxAnimatedColorVariableData Dynamics { get; set; }
    }
    [MetaClass("VfxAnimatedFloatVariableData")]
    public class VfxAnimatedFloatVariableData : IMetaClass
    {
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; }
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; }
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<float> Values { get; set; }
    }
    [MetaClass("VfxAnimatedVector2fVariableData")]
    public class VfxAnimatedVector2fVariableData : IMetaClass
    {
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; }
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; }
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<Vector2> Values { get; set; }
    }
    [MetaClass("VfxAnimatedVector3fVariableData")]
    public class VfxAnimatedVector3fVariableData : IMetaClass
    {
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; }
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; }
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<Vector3> Values { get; set; }
    }
    [MetaClass("VfxAnimatedColorVariableData")]
    public class VfxAnimatedColorVariableData : IMetaClass
    {
        [MetaProperty("probabilityTables", BinPropertyType.Container)]
        public MetaContainer<VfxProbabilityTableData> ProbabilityTables { get; set; }
        [MetaProperty("times", BinPropertyType.Container)]
        public MetaContainer<float> Times { get; set; }
        [MetaProperty("values", BinPropertyType.Container)]
        public MetaContainer<Vector4> Values { get; set; }
    }
    [MetaClass("VfxSoftParticleDefinitionData")]
    public class VfxSoftParticleDefinitionData : IMetaClass
    {
        [MetaProperty("beginIn", BinPropertyType.Float)]
        public float? BeginIn { get; set; }
        [MetaProperty("beginOut", BinPropertyType.Float)]
        public float? BeginOut { get; set; }
        [MetaProperty("deltaIn", BinPropertyType.Float)]
        public float? DeltaIn { get; set; }
        [MetaProperty("deltaOut", BinPropertyType.Float)]
        public float? DeltaOut { get; set; }
    }
    [MetaClass("FlexValueVector3")]
    public class FlexValueVector3 : IMetaClass
    {
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint? FlexID { get; set; }
        [MetaProperty("mValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Value { get; set; }
    }
    [MetaClass("FlexValueVector2")]
    public class FlexValueVector2 : IMetaClass
    {
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint? FlexID { get; set; }
        [MetaProperty("mValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> Value { get; set; }
    }
    [MetaClass("FlexValueFloat")]
    public class FlexValueFloat : IMetaClass
    {
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint? FlexID { get; set; }
        [MetaProperty("mValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Value { get; set; }
    }
    [MetaClass("FlexTypeFloat")]
    public class FlexTypeFloat : IMetaClass
    {
        [MetaProperty("mFlexID", BinPropertyType.UInt32)]
        public uint? FlexID { get; set; }
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float? Value { get; set; }
    }
    [MetaClass("ColorGraphMaterialDriver")]
    public class ColorGraphMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; }
        [MetaProperty("colors", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedColorVariableData> Colors { get; set; }
    }
    [MetaClass("VfxFieldAccelerationDefinitionData")]
    public class VfxFieldAccelerationDefinitionData : IMetaClass
    {
        [MetaProperty("isLocalSpace", BinPropertyType.Bool)]
        public bool? IsLocalSpace { get; set; }
        [MetaProperty("acceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Acceleration { get; set; }
    }
    [MetaClass("VfxFieldAttractionDefinitionData")]
    public class VfxFieldAttractionDefinitionData : IMetaClass
    {
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Position { get; set; }
        [MetaProperty("radius", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Radius { get; set; }
        [MetaProperty("acceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Acceleration { get; set; }
    }
    [MetaClass("VfxFieldCollectionDefinitionData")]
    public class VfxFieldCollectionDefinitionData : IMetaClass
    {
        [MetaProperty("fieldAccelerationDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldAccelerationDefinitionData>> FieldAccelerationDefinitions { get; set; }
        [MetaProperty("fieldAttractionDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldAttractionDefinitionData>> FieldAttractionDefinitions { get; set; }
        [MetaProperty("fieldDragDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldDragDefinitionData>> FieldDragDefinitions { get; set; }
        [MetaProperty("fieldNoiseDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldNoiseDefinitionData>> FieldNoiseDefinitions { get; set; }
        [MetaProperty("fieldOrbitalDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxFieldOrbitalDefinitionData>> FieldOrbitalDefinitions { get; set; }
    }
    [MetaClass("VfxFieldDragDefinitionData")]
    public class VfxFieldDragDefinitionData : IMetaClass
    {
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Position { get; set; }
        [MetaProperty("radius", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Radius { get; set; }
        [MetaProperty("strength", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Strength { get; set; }
    }
    [MetaClass("VfxFieldNoiseDefinitionData")]
    public class VfxFieldNoiseDefinitionData : IMetaClass
    {
        [MetaProperty("position", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Position { get; set; }
        [MetaProperty("radius", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Radius { get; set; }
        [MetaProperty("frequency", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Frequency { get; set; }
        [MetaProperty("velocityDelta", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> VelocityDelta { get; set; }
        [MetaProperty("axisFraction", BinPropertyType.Vector3)]
        public Vector3? AxisFraction { get; set; }
    }
    [MetaClass("VfxFieldOrbitalDefinitionData")]
    public class VfxFieldOrbitalDefinitionData : IMetaClass
    {
        [MetaProperty("isLocalSpace", BinPropertyType.Bool)]
        public bool? IsLocalSpace { get; set; }
        [MetaProperty("direction", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Direction { get; set; }
    }
    [MetaClass("FloatGraphMaterialDriver")]
    public class FloatGraphMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; }
        [MetaProperty("graph", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedFloatVariableData> Graph { get; set; }
    }
    [MetaClass("IVfxMaterialDriver")]
    public interface IVfxMaterialDriver : IMetaClass
    {
    }
    [MetaClass("VfxMaterialOverrideDefinitionData")]
    public class VfxMaterialOverrideDefinitionData : IMetaClass
    {
        [MetaProperty("priority", BinPropertyType.Int32)]
        public int? Priority { get; set; }
        [MetaProperty("subMeshName", BinPropertyType.Optional)]
        public MetaOptional<string> SubMeshName { get; set; }
        [MetaProperty("overrideBlendMode", BinPropertyType.UInt32)]
        public uint? OverrideBlendMode { get; set; }
        [MetaProperty("baseTexture", BinPropertyType.String)]
        public string? BaseTexture { get; set; }
        [MetaProperty("glossTexture", BinPropertyType.String)]
        public string? GlossTexture { get; set; }
        [MetaProperty("transitionTexture", BinPropertyType.String)]
        public string? TransitionTexture { get; set; }
        [MetaProperty("transitionSample", BinPropertyType.Float)]
        public float? TransitionSample { get; set; }
        [MetaProperty("transitionSource", BinPropertyType.UInt32)]
        public uint? TransitionSource { get; set; }
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Material { get; set; }
    }
    [MetaClass("VfxProbabilityTableData")]
    public class VfxProbabilityTableData : IMetaClass
    {
        [MetaProperty("keyTimes", BinPropertyType.Container)]
        public MetaContainer<float> KeyTimes { get; set; }
        [MetaProperty("keyValues", BinPropertyType.Container)]
        public MetaContainer<float> KeyValues { get; set; }
        [MetaProperty("singleValue", BinPropertyType.Float)]
        public float? SingleValue { get; set; }
    }
    [MetaClass("VfxSystemDefinitionData")]
    public class VfxSystemDefinitionData : IResource
    {
        [MetaProperty("materialOverrideDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxMaterialOverrideDefinitionData>> MaterialOverrideDefinitions { get; set; }
        [MetaProperty("complexEmitterDefinitionData", BinPropertyType.Container)]
        public MetaContainer<VfxEmitterDefinitionData> ComplexEmitterDefinitionData { get; set; }
        [MetaProperty("simpleEmitterDefinitionData", BinPropertyType.Container)]
        public MetaContainer<VfxEmitterDefinitionData> SimpleEmitterDefinitionData { get; set; }
        [MetaProperty("visibilityRadius", BinPropertyType.Float)]
        public float? VisibilityRadius { get; set; }
        [MetaProperty("particleName", BinPropertyType.String)]
        public string? ParticleName { get; set; }
        [MetaProperty("particlePath", BinPropertyType.String)]
        public string? ParticlePath { get; set; }
        [MetaProperty("overrideScaleCap", BinPropertyType.Optional)]
        public MetaOptional<float> OverrideScaleCap { get; set; }
        [MetaProperty("soundOnCreateDefault", BinPropertyType.String)]
        public string? SoundOnCreateDefault { get; set; }
        [MetaProperty("voiceOverOnCreateDefault", BinPropertyType.String)]
        public string? VoiceOverOnCreateDefault { get; set; }
        [MetaProperty("soundPersistentDefault", BinPropertyType.String)]
        public string? SoundPersistentDefault { get; set; }
        [MetaProperty("voiceOverPersistentDefault", BinPropertyType.String)]
        public string? VoiceOverPersistentDefault { get; set; }
        [MetaProperty("assetCategory", BinPropertyType.String)]
        public string? AssetCategory { get; set; }
        [MetaProperty("audioParameterFlexID", BinPropertyType.Int32)]
        public int? AudioParameterFlexID { get; set; }
        [MetaProperty("audioParameterTimeScaledDuration", BinPropertyType.Float)]
        public float? AudioParameterTimeScaledDuration { get; set; }
        [MetaProperty("flags", BinPropertyType.Byte)]
        public byte? Flags { get; set; }
        [MetaProperty("buildUpTime", BinPropertyType.Float)]
        public float? BuildUpTime { get; set; }
        [MetaProperty("selfIllumination", BinPropertyType.Float)]
        public float? SelfIllumination { get; set; }
        [MetaProperty("assetRemappingTable", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxAssetRemap>> AssetRemappingTable { get; set; }
        [MetaProperty("systemTranslation", BinPropertyType.Vector3)]
        public Vector3? SystemTranslation { get; set; }
        [MetaProperty("systemScale", BinPropertyType.Vector3)]
        public Vector3? SystemScale { get; set; }
        [MetaProperty("drawingLayer", BinPropertyType.Byte)]
        public byte? DrawingLayer { get; set; }
        [MetaProperty("hudLayerDimension", BinPropertyType.Float)]
        public float? HudLayerDimension { get; set; }
        [MetaProperty(3180870615, BinPropertyType.Float)]
        public float? m3180870615 { get; set; }
        [MetaProperty("hudAnchorPositionFromWorldProjection", BinPropertyType.Bool)]
        public bool? HudAnchorPositionFromWorldProjection { get; set; }
        [MetaProperty("scaleDynamicallyWithAttachedBone", BinPropertyType.Bool)]
        public bool? ScaleDynamicallyWithAttachedBone { get; set; }
        [MetaProperty("mEyeCandy", BinPropertyType.Bool)]
        public bool? EyeCandy { get; set; }
        [MetaProperty(3473471718, BinPropertyType.Bool)]
        public bool? m3473471718 { get; set; }
    }
    [MetaClass("VfxMigrationResources")]
    public class VfxMigrationResources : IMetaClass
    {
        [MetaProperty("resourceMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> ResourceMap { get; set; }
    }
    [MetaClass("VfxAssetRemap")]
    public class VfxAssetRemap : IMetaClass
    {
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("oldAsset", BinPropertyType.Hash)]
        public MetaHash? OldAsset { get; set; }
        [MetaProperty("newAsset", BinPropertyType.String)]
        public string? NewAsset { get; set; }
    }
    [MetaClass("VfxColorOverLifeMaterialDriver")]
    public class VfxColorOverLifeMaterialDriver : IVfxMaterialDriver
    {
        [MetaProperty("frequency", BinPropertyType.Byte)]
        public byte? Frequency { get; set; }
        [MetaProperty("colors", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedColorVariableData> Colors { get; set; }
    }
    [MetaClass("VfxShape")]
    public class VfxShape : IMetaClass
    {
        [MetaProperty("birthTranslation", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthTranslation { get; set; }
        [MetaProperty("emitOffset", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> EmitOffset { get; set; }
        [MetaProperty("emitRotationAngles", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ValueFloat>> EmitRotationAngles { get; set; }
        [MetaProperty("emitRotationAxes", BinPropertyType.Container)]
        public MetaContainer<Vector3> EmitRotationAxes { get; set; }
    }
    [MetaClass("VfxEmitterDefinitionData")]
    public class VfxEmitterDefinitionData : IMetaClass
    {
        [MetaProperty("soundOnCreateName", BinPropertyType.String)]
        public string? SoundOnCreateName { get; set; }
        [MetaProperty("soundPersistentName", BinPropertyType.String)]
        public string? SoundPersistentName { get; set; }
        [MetaProperty("voiceOverOnCreateName", BinPropertyType.String)]
        public string? VoiceOverOnCreateName { get; set; }
        [MetaProperty("voiceOverPersistentName", BinPropertyType.String)]
        public string? VoiceOverPersistentName { get; set; }
        [MetaProperty("timeBeforeFirstEmission", BinPropertyType.Float)]
        public float? TimeBeforeFirstEmission { get; set; }
        [MetaProperty(3472013936, BinPropertyType.Float)]
        public float? m3472013936 { get; set; }
        [MetaProperty("rate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Rate { get; set; }
        [MetaProperty(1401880484, BinPropertyType.BitBool)]
        public MetaBitBool? m1401880484 { get; set; }
        [MetaProperty("particleLifetime", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> ParticleLifetime { get; set; }
        [MetaProperty("particleLingerType", BinPropertyType.Byte)]
        public byte? ParticleLingerType { get; set; }
        [MetaProperty("particleLinger", BinPropertyType.Optional)]
        public MetaOptional<float> ParticleLinger { get; set; }
        [MetaProperty("lifetime", BinPropertyType.Optional)]
        public MetaOptional<float> Lifetime { get; set; }
        [MetaProperty("isSingleParticle", BinPropertyType.BitBool)]
        public MetaBitBool? IsSingleParticle { get; set; }
        [MetaProperty("emitterLinger", BinPropertyType.Optional)]
        public MetaOptional<float> EmitterLinger { get; set; }
        [MetaProperty("excludedAttachmentTypes", BinPropertyType.Container)]
        public MetaContainer<string> ExcludedAttachmentTypes { get; set; }
        [MetaProperty("period", BinPropertyType.Optional)]
        public MetaOptional<float> Period { get; set; }
        [MetaProperty("timeActiveDuringPeriod", BinPropertyType.Optional)]
        public MetaOptional<float> TimeActiveDuringPeriod { get; set; }
        [MetaProperty("rateByVelocityFunction", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> RateByVelocityFunction { get; set; }
        [MetaProperty(3338279773, BinPropertyType.Optional)]
        public MetaOptional<float> m3338279773 { get; set; }
        [MetaProperty("offsetLifetimeScaling", BinPropertyType.Vector3)]
        public Vector3? OffsetLifetimeScaling { get; set; }
        [MetaProperty("offsetLifeScalingSymmetryMode", BinPropertyType.Byte)]
        public byte? OffsetLifeScalingSymmetryMode { get; set; }
        [MetaProperty("doesLifetimeScale", BinPropertyType.BitBool)]
        public MetaBitBool? DoesLifetimeScale { get; set; }
        [MetaProperty("flexRate", BinPropertyType.Structure)]
        public FlexValueFloat FlexRate { get; set; }
        [MetaProperty("flexParticleLifetime", BinPropertyType.Structure)]
        public FlexValueFloat FlexParticleLifetime { get; set; }
        [MetaProperty("doesParticleLifetimeScale", BinPropertyType.BitBool)]
        public MetaBitBool? DoesParticleLifetimeScale { get; set; }
        [MetaProperty("emissionSurfaceDefinition", BinPropertyType.Structure)]
        public VfxEmissionSurfaceData EmissionSurfaceDefinition { get; set; }
        [MetaProperty("childParticleSetDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxChildParticleSetDefinitionData> ChildParticleSetDefinition { get; set; }
        [MetaProperty("fieldCollectionDefinition", BinPropertyType.Structure)]
        public VfxFieldCollectionDefinitionData FieldCollectionDefinition { get; set; }
        [MetaProperty("emitterName", BinPropertyType.String)]
        public string? EmitterName { get; set; }
        [MetaProperty("disabled", BinPropertyType.Bool)]
        public bool? Disabled { get; set; }
        [MetaProperty("importance", BinPropertyType.Byte)]
        public byte? Importance { get; set; }
        [MetaProperty(1183687955, BinPropertyType.Byte)]
        public byte? m1183687955 { get; set; }
        [MetaProperty("keywordsRequired", BinPropertyType.Container)]
        public MetaContainer<string> KeywordsRequired { get; set; }
        [MetaProperty("keywordsExcluded", BinPropertyType.Container)]
        public MetaContainer<string> KeywordsExcluded { get; set; }
        [MetaProperty("keywordsIncluded", BinPropertyType.Container)]
        public MetaContainer<string> KeywordsIncluded { get; set; }
        [MetaProperty("spectatorPolicy", BinPropertyType.Byte)]
        public byte? SpectatorPolicy { get; set; }
        [MetaProperty("censorPolicy", BinPropertyType.Byte)]
        public byte? CensorPolicy { get; set; }
        [MetaProperty("translationOverride", BinPropertyType.Vector3)]
        public Vector3? TranslationOverride { get; set; }
        [MetaProperty("rotationOverride", BinPropertyType.Vector3)]
        public Vector3? RotationOverride { get; set; }
        [MetaProperty("birthOrbitalVelocity", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthOrbitalVelocity { get; set; }
        [MetaProperty("birthVelocity", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthVelocity { get; set; }
        [MetaProperty("birthDrag", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthDrag { get; set; }
        [MetaProperty("birthAcceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthAcceleration { get; set; }
        [MetaProperty("velocity", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Velocity { get; set; }
        [MetaProperty("drag", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Drag { get; set; }
        [MetaProperty("acceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Acceleration { get; set; }
        [MetaProperty(848541616, BinPropertyType.BitBool)]
        public MetaBitBool? m848541616 { get; set; }
        [MetaProperty(160052109, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> m160052109 { get; set; }
        [MetaProperty(3007716357, BinPropertyType.BitBool)]
        public MetaBitBool? m3007716357 { get; set; }
        [MetaProperty(4207272600, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> m4207272600 { get; set; }
        [MetaProperty(4071846627, BinPropertyType.BitBool)]
        public MetaBitBool? m4071846627 { get; set; }
        [MetaProperty(3751201134, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> m3751201134 { get; set; }
        [MetaProperty("flexBirthVelocity", BinPropertyType.Structure)]
        public FlexValueVector3 FlexBirthVelocity { get; set; }
        [MetaProperty("worldAcceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector3> WorldAcceleration { get; set; }
        [MetaProperty("shape", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxShape> Shape { get; set; }
        [MetaProperty("bindWeight", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BindWeight { get; set; }
        [MetaProperty("scaleEmitOffsetByBoundObjectSize", BinPropertyType.Float)]
        public float? ScaleEmitOffsetByBoundObjectSize { get; set; }
        [MetaProperty("emissionMeshScale", BinPropertyType.Float)]
        public float? EmissionMeshScale { get; set; }
        [MetaProperty("emissionMeshName", BinPropertyType.String)]
        public string? EmissionMeshName { get; set; }
        [MetaProperty("scaleEmitOffsetByBoundObjectHeight", BinPropertyType.Float)]
        public float? ScaleEmitOffsetByBoundObjectHeight { get; set; }
        [MetaProperty("scaleEmitOffsetByBoundObjectRadius", BinPropertyType.Float)]
        public float? ScaleEmitOffsetByBoundObjectRadius { get; set; }
        [MetaProperty("flexOffset", BinPropertyType.Structure)]
        public FlexValueVector3 FlexOffset { get; set; }
        [MetaProperty("flexBirthTranslation", BinPropertyType.Structure)]
        public FlexValueVector3 FlexBirthTranslation { get; set; }
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Material { get; set; }
        [MetaProperty("materialDrivers", BinPropertyType.Map)]
        public Dictionary<string, IVfxMaterialDriver> MaterialDrivers { get; set; }
        [MetaProperty("renderPhaseOverride", BinPropertyType.Byte)]
        public byte? RenderPhaseOverride { get; set; }
        [MetaProperty("primitive", BinPropertyType.Structure)]
        public VfxPrimitiveBase Primitive { get; set; }
        [MetaProperty("falloffTexture", BinPropertyType.String)]
        public string? FalloffTexture { get; set; }
        [MetaProperty("particleColorTexture", BinPropertyType.String)]
        public string? ParticleColorTexture { get; set; }
        [MetaProperty("blendMode", BinPropertyType.Byte)]
        public byte? BlendMode { get; set; }
        [MetaProperty("birthColor", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> BirthColor { get; set; }
        [MetaProperty("color", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> Color { get; set; }
        [MetaProperty(452491014, BinPropertyType.BitBool)]
        public MetaBitBool? m452491014 { get; set; }
        [MetaProperty("lingerColor", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> LingerColor { get; set; }
        [MetaProperty("pass", BinPropertyType.Int16)]
        public short? Pass { get; set; }
        [MetaProperty("meshRenderFlags", BinPropertyType.Byte)]
        public byte? MeshRenderFlags { get; set; }
        [MetaProperty("colorLookUpTypeX", BinPropertyType.Byte)]
        public byte? ColorLookUpTypeX { get; set; }
        [MetaProperty("colorLookUpTypeY", BinPropertyType.Byte)]
        public byte? ColorLookUpTypeY { get; set; }
        [MetaProperty("colorLookUpScales", BinPropertyType.Vector2)]
        public Vector2? ColorLookUpScales { get; set; }
        [MetaProperty("alphaRef", BinPropertyType.Byte)]
        public byte? AlphaRef { get; set; }
        [MetaProperty("colorLookUpOffsets", BinPropertyType.Vector2)]
        public Vector2? ColorLookUpOffsets { get; set; }
        [MetaProperty("softParticleParams", BinPropertyType.Structure)]
        public VfxSoftParticleDefinitionData SoftParticleParams { get; set; }
        [MetaProperty("colorRenderFlags", BinPropertyType.Byte)]
        public byte? ColorRenderFlags { get; set; }
        [MetaProperty("censorModulateValue", BinPropertyType.Vector4)]
        public Vector4? CensorModulateValue { get; set; }
        [MetaProperty("sliceTechniqueRange", BinPropertyType.Float)]
        public float? SliceTechniqueRange { get; set; }
        [MetaProperty("modulationFactor", BinPropertyType.Vector4)]
        public Vector4? ModulationFactor { get; set; }
        [MetaProperty("alphaErosionDefinition", BinPropertyType.Structure)]
        public VfxAlphaErosionDefinitionData AlphaErosionDefinition { get; set; }
        [MetaProperty("reflectionDefinition", BinPropertyType.Structure)]
        public VfxReflectionDefinitionData ReflectionDefinition { get; set; }
        [MetaProperty("distortionDefinition", BinPropertyType.Structure)]
        public VfxDistortionDefinitionData DistortionDefinition { get; set; }
        [MetaProperty("isTexturePixelated", BinPropertyType.Bool)]
        public bool? IsTexturePixelated { get; set; }
        [MetaProperty("uvParallaxScale", BinPropertyType.Float)]
        public float? UvParallaxScale { get; set; }
        [MetaProperty("depthBiasFactors", BinPropertyType.Vector2)]
        public Vector2? DepthBiasFactors { get; set; }
        [MetaProperty("disableBackfaceCull", BinPropertyType.Bool)]
        public bool? DisableBackfaceCull { get; set; }
        [MetaProperty("miscRenderFlags", BinPropertyType.Byte)]
        public byte? MiscRenderFlags { get; set; }
        [MetaProperty("stencilMode", BinPropertyType.Byte)]
        public byte? StencilMode { get; set; }
        [MetaProperty("stencilRef", BinPropertyType.Byte)]
        public byte? StencilRef { get; set; }
        [MetaProperty("uvScrollAlphaMult", BinPropertyType.BitBool)]
        public MetaBitBool? UvScrollAlphaMult { get; set; }
        [MetaProperty("particleIsLocalOrientation", BinPropertyType.BitBool)]
        public MetaBitBool? ParticleIsLocalOrientation { get; set; }
        [MetaProperty("isDirectionOriented", BinPropertyType.BitBool)]
        public MetaBitBool? IsDirectionOriented { get; set; }
        [MetaProperty("isUniformScale", BinPropertyType.BitBool)]
        public MetaBitBool? IsUniformScale { get; set; }
        [MetaProperty("hasPostRotateOrientation", BinPropertyType.BitBool)]
        public MetaBitBool? HasPostRotateOrientation { get; set; }
        [MetaProperty("isRandomStartFrame", BinPropertyType.BitBool)]
        public MetaBitBool? IsRandomStartFrame { get; set; }
        [MetaProperty("isRandomStartFrameMult", BinPropertyType.BitBool)]
        public MetaBitBool? IsRandomStartFrameMult { get; set; }
        [MetaProperty("doesCastShadow", BinPropertyType.BitBool)]
        public MetaBitBool? DoesCastShadow { get; set; }
        [MetaProperty("isRotationEnabled", BinPropertyType.BitBool)]
        public MetaBitBool? IsRotationEnabled { get; set; }
        [MetaProperty("uvScrollClamp", BinPropertyType.BitBool)]
        public MetaBitBool? UvScrollClamp { get; set; }
        [MetaProperty("uvScrollClampMult", BinPropertyType.BitBool)]
        public MetaBitBool? UvScrollClampMult { get; set; }
        [MetaProperty("isFollowingTerrain", BinPropertyType.BitBool)]
        public MetaBitBool? IsFollowingTerrain { get; set; }
        [MetaProperty("isGroundLayer", BinPropertyType.BitBool)]
        public MetaBitBool? IsGroundLayer { get; set; }
        [MetaProperty("useEmissionMeshNormalForBirth", BinPropertyType.BitBool)]
        public MetaBitBool? UseEmissionMeshNormalForBirth { get; set; }
        [MetaProperty("useNavmeshMask", BinPropertyType.BitBool)]
        public MetaBitBool? UseNavmeshMask { get; set; }
        [MetaProperty(1734953377, BinPropertyType.BitBool)]
        public MetaBitBool? m1734953377 { get; set; }
        [MetaProperty(3181085639, BinPropertyType.BitBool)]
        public MetaBitBool? m3181085639 { get; set; }
        [MetaProperty("birthRotation0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthRotation0 { get; set; }
        [MetaProperty("birthRotationalVelocity0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthRotationalVelocity0 { get; set; }
        [MetaProperty(3730513113, BinPropertyType.BitBool)]
        public MetaBitBool? m3730513113 { get; set; }
        [MetaProperty(1751193500, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> m1751193500 { get; set; }
        [MetaProperty("isLocalOrientation", BinPropertyType.BitBool)]
        public MetaBitBool? IsLocalOrientation { get; set; }
        [MetaProperty("directionVelocityScale", BinPropertyType.Float)]
        public float? DirectionVelocityScale { get; set; }
        [MetaProperty("directionVelocityMinScale", BinPropertyType.Float)]
        public float? DirectionVelocityMinScale { get; set; }
        [MetaProperty("postRotateOrientationAxis", BinPropertyType.Vector3)]
        public Vector3? PostRotateOrientationAxis { get; set; }
        [MetaProperty("birthRotationalAcceleration", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthRotationalAcceleration { get; set; }
        [MetaProperty("flexBirthRotationalVelocity0", BinPropertyType.Structure)]
        public FlexValueVector3 FlexBirthRotationalVelocity0 { get; set; }
        [MetaProperty("rotation0", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector3> Rotation0 { get; set; }
        [MetaProperty("birthScale0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthScale0 { get; set; }
        [MetaProperty("scale0", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> Scale0 { get; set; }
        [MetaProperty(1582117303, BinPropertyType.BitBool)]
        public MetaBitBool? m1582117303 { get; set; }
        [MetaProperty(2106505348, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> m2106505348 { get; set; }
        [MetaProperty("scaleBirthScaleByBoundObjectSize", BinPropertyType.Float)]
        public float? ScaleBirthScaleByBoundObjectSize { get; set; }
        [MetaProperty("scaleBirthScaleByBoundObjectRadius", BinPropertyType.Float)]
        public float? ScaleBirthScaleByBoundObjectRadius { get; set; }
        [MetaProperty("flexScaleBirthScale", BinPropertyType.Structure)]
        public FlexTypeFloat FlexScaleBirthScale { get; set; }
        [MetaProperty("flexScaleEmitOffset", BinPropertyType.Structure)]
        public FlexTypeFloat FlexScaleEmitOffset { get; set; }
        [MetaProperty(2172495129, BinPropertyType.Structure)]
        public FlexTypeFloat m2172495129 { get; set; }
        [MetaProperty("scaleBirthScaleByBoundObjectHeight", BinPropertyType.Float)]
        public float? ScaleBirthScaleByBoundObjectHeight { get; set; }
        [MetaProperty("texture", BinPropertyType.String)]
        public string? Texture { get; set; }
        [MetaProperty("meshTextureName", BinPropertyType.String)]
        public string? MeshTextureName { get; set; }
        [MetaProperty("frameRate", BinPropertyType.Float)]
        public float? FrameRate { get; set; }
        [MetaProperty("birthFrameRate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthFrameRate { get; set; }
        [MetaProperty("numFrames", BinPropertyType.UInt16)]
        public ushort? NumFrames { get; set; }
        [MetaProperty("startFrame", BinPropertyType.UInt16)]
        public ushort? StartFrame { get; set; }
        [MetaProperty("uvMode", BinPropertyType.Byte)]
        public byte? UvMode { get; set; }
        [MetaProperty("paletteDefinition", BinPropertyType.Structure)]
        public VfxPaletteDefinitionData PaletteDefinition { get; set; }
        [MetaProperty("materialOverrideDefinitions", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxMaterialOverrideDefinitionData>> MaterialOverrideDefinitions { get; set; }
        [MetaProperty("birthUvScrollRate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUvScrollRate { get; set; }
        [MetaProperty("flexBirthUVScrollRate", BinPropertyType.Structure)]
        public FlexValueVector2 FlexBirthUVScrollRate { get; set; }
        [MetaProperty("birthUVOffset", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUVOffset { get; set; }
        [MetaProperty("texAddressModeBase", BinPropertyType.Byte)]
        public byte? TexAddressModeBase { get; set; }
        [MetaProperty("texDiv", BinPropertyType.Vector2)]
        public Vector2? TexDiv { get; set; }
        [MetaProperty("particleUVScrollRate", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector2> ParticleUVScrollRate { get; set; }
        [MetaProperty("emitterUvScrollRate", BinPropertyType.Vector2)]
        public Vector2? EmitterUvScrollRate { get; set; }
        [MetaProperty("flexBirthUVOffset", BinPropertyType.Structure)]
        public FlexValueVector2 FlexBirthUVOffset { get; set; }
        [MetaProperty("uvScale", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> UvScale { get; set; }
        [MetaProperty("uvRotation", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> UvRotation { get; set; }
        [MetaProperty("birthUvRotateRate", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthUvRotateRate { get; set; }
        [MetaProperty("particleUVRotateRate", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueFloat> ParticleUVRotateRate { get; set; }
        [MetaProperty("uvTransformCenter", BinPropertyType.Vector2)]
        public Vector2? UvTransformCenter { get; set; }
        [MetaProperty("textureMult", BinPropertyType.String)]
        public string? TextureMult { get; set; }
        [MetaProperty("birthUvScrollRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUvScrollRateMult { get; set; }
        [MetaProperty("birthUVOffsetMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> BirthUVOffsetMult { get; set; }
        [MetaProperty("texAddressModeMult", BinPropertyType.Byte)]
        public byte? TexAddressModeMult { get; set; }
        [MetaProperty("texDivMult", BinPropertyType.Vector2)]
        public Vector2? TexDivMult { get; set; }
        [MetaProperty("uvScaleMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector2> UvScaleMult { get; set; }
        [MetaProperty("birthUvRotateRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthUvRotateRateMult { get; set; }
        [MetaProperty("particleUVRotateRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueFloat> ParticleUVRotateRateMult { get; set; }
        [MetaProperty("uvTransformCenterMult", BinPropertyType.Vector2)]
        public Vector2? UvTransformCenterMult { get; set; }
        [MetaProperty("particleUVScrollRateMult", BinPropertyType.Embedded)]
        public MetaEmbedded<IntegratedValueVector2> ParticleUVScrollRateMult { get; set; }
        [MetaProperty("emitterUvScrollRateMult", BinPropertyType.Vector2)]
        public Vector2? EmitterUvScrollRateMult { get; set; }
        [MetaProperty("flexBirthUVScrollRateMult", BinPropertyType.Structure)]
        public FlexValueVector2 FlexBirthUVScrollRateMult { get; set; }
        [MetaProperty("scaleOverride", BinPropertyType.Vector3)]
        public Vector3? ScaleOverride { get; set; }
        [MetaProperty("birthScale1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthScale1 { get; set; }
        [MetaProperty("scale1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Scale1 { get; set; }
        [MetaProperty("birthRotation1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthRotation1 { get; set; }
        [MetaProperty("particleBind", BinPropertyType.Vector2)]
        public Vector2? ParticleBind { get; set; }
        [MetaProperty("birthRotationalVelocity1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> BirthRotationalVelocity1 { get; set; }
        [MetaProperty("orientation", BinPropertyType.Byte)]
        public byte? Orientation { get; set; }
        [MetaProperty("rotation1", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> Rotation1 { get; set; }
        [MetaProperty("scaleBias", BinPropertyType.Vector2)]
        public Vector2? ScaleBias { get; set; }
        [MetaProperty("uvScrollRate1", BinPropertyType.Vector2)]
        public Vector2? UvScrollRate1 { get; set; }
        [MetaProperty("hasFixedOrbit", BinPropertyType.BitBool)]
        public MetaBitBool? HasFixedOrbit { get; set; }
        [MetaProperty("fixedOrbitType", BinPropertyType.Byte)]
        public byte? FixedOrbitType { get; set; }
        [MetaProperty("scaleUpFromOrigin", BinPropertyType.BitBool)]
        public MetaBitBool? ScaleUpFromOrigin { get; set; }
        [MetaProperty("lockedToEmitter", BinPropertyType.BitBool)]
        public MetaBitBool? LockedToEmitter { get; set; }
        [MetaProperty("flexBirthRotationalVelocity1", BinPropertyType.Structure)]
        public FlexValueFloat FlexBirthRotationalVelocity1 { get; set; }
    }
    [MetaClass("VfxChildIdentifier")]
    public class VfxChildIdentifier : IMetaClass
    {
        [MetaProperty("effectKey", BinPropertyType.Hash)]
        public MetaHash? EffectKey { get; set; }
        [MetaProperty("effect", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Effect { get; set; }
        [MetaProperty("effectName", BinPropertyType.String)]
        public string? EffectName { get; set; }
    }
    [MetaClass("VfxChildParticleSetDefinitionData")]
    public class VfxChildParticleSetDefinitionData : IMetaClass
    {
        [MetaProperty("childrenIdentifiers", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<VfxChildIdentifier>> ChildrenIdentifiers { get; set; }
        [MetaProperty("boneToSpawnAt", BinPropertyType.Container)]
        public MetaContainer<string> BoneToSpawnAt { get; set; }
        [MetaProperty("childrenProbability", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> ChildrenProbability { get; set; }
        [MetaProperty("childEmitOnDeath", BinPropertyType.Bool)]
        public bool? ChildEmitOnDeath { get; set; }
    }
    [MetaClass("VfxAlphaErosionDefinitionData")]
    public class VfxAlphaErosionDefinitionData : IMetaClass
    {
        [MetaProperty("erosionDriveCurve", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> ErosionDriveCurve { get; set; }
        [MetaProperty("erosionDriveSource", BinPropertyType.UInt32)]
        public uint? ErosionDriveSource { get; set; }
        [MetaProperty(649389603, BinPropertyType.Bool)]
        public bool? m649389603 { get; set; }
        [MetaProperty(2102344908, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> m2102344908 { get; set; }
        [MetaProperty("erosionFeatherIn", BinPropertyType.Float)]
        public float? ErosionFeatherIn { get; set; }
        [MetaProperty("erosionFeatherOut", BinPropertyType.Float)]
        public float? ErosionFeatherOut { get; set; }
        [MetaProperty("erosionSliceWidth", BinPropertyType.Float)]
        public float? ErosionSliceWidth { get; set; }
        [MetaProperty("erosionMapName", BinPropertyType.String)]
        public string? ErosionMapName { get; set; }
        [MetaProperty("erosionMapChannelMixer", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> ErosionMapChannelMixer { get; set; }
        [MetaProperty("erosionMapAddressMode", BinPropertyType.Byte)]
        public byte? ErosionMapAddressMode { get; set; }
    }
    [MetaClass("VfxPaletteDefinitionData")]
    public class VfxPaletteDefinitionData : IMetaClass
    {
        [MetaProperty("paletteTexture", BinPropertyType.String)]
        public string? PaletteTexture { get; set; }
        [MetaProperty(91092758, BinPropertyType.Byte)]
        public byte? m91092758 { get; set; }
        [MetaProperty("palleteSrcMixColor", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> PalleteSrcMixColor { get; set; }
        [MetaProperty("paletteSelector", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> PaletteSelector { get; set; }
        [MetaProperty(886635206, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> m886635206 { get; set; }
        [MetaProperty(1157448907, BinPropertyType.Embedded)]
        public MetaEmbedded<ValueFloat> m1157448907 { get; set; }
        [MetaProperty("paletteCount", BinPropertyType.Int32)]
        public int? PaletteCount { get; set; }
    }
    [MetaClass("VfxReflectionDefinitionData")]
    public class VfxReflectionDefinitionData : IMetaClass
    {
        [MetaProperty("reflectionMapTexture", BinPropertyType.String)]
        public string? ReflectionMapTexture { get; set; }
        [MetaProperty("reflectionOpacityDirect", BinPropertyType.Float)]
        public float? ReflectionOpacityDirect { get; set; }
        [MetaProperty("reflectionOpacityGlancing", BinPropertyType.Float)]
        public float? ReflectionOpacityGlancing { get; set; }
        [MetaProperty("reflectionFresnel", BinPropertyType.Float)]
        public float? ReflectionFresnel { get; set; }
        [MetaProperty("reflectionFresnelColor", BinPropertyType.Vector4)]
        public Vector4? ReflectionFresnelColor { get; set; }
        [MetaProperty("fresnel", BinPropertyType.Float)]
        public float? Fresnel { get; set; }
        [MetaProperty("fresnelColor", BinPropertyType.Vector4)]
        public Vector4? FresnelColor { get; set; }
    }
    [MetaClass("VfxDistortionDefinitionData")]
    public class VfxDistortionDefinitionData : IMetaClass
    {
        [MetaProperty("distortion", BinPropertyType.Float)]
        public float? Distortion { get; set; }
        [MetaProperty("distortionMode", BinPropertyType.Byte)]
        public byte? DistortionMode { get; set; }
        [MetaProperty("normalMapTexture", BinPropertyType.String)]
        public string? NormalMapTexture { get; set; }
    }
    [MetaClass("VfxProjectionDefinitionData")]
    public class VfxProjectionDefinitionData : IMetaClass
    {
        [MetaProperty("mYRange", BinPropertyType.Float)]
        public float? YRange { get; set; }
        [MetaProperty("mFading", BinPropertyType.Float)]
        public float? Fading { get; set; }
    }
    [MetaClass("VfxTrailDefinitionData")]
    public class VfxTrailDefinitionData : IMetaClass
    {
        [MetaProperty("mMode", BinPropertyType.Byte)]
        public byte? Mode { get; set; }
        [MetaProperty("mCutoff", BinPropertyType.Float)]
        public float? Cutoff { get; set; }
        [MetaProperty("mBirthTilingSize", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthTilingSize { get; set; }
        [MetaProperty("mSmoothingMode", BinPropertyType.Byte)]
        public byte? SmoothingMode { get; set; }
        [MetaProperty("mMaxAddedPerFrame", BinPropertyType.Int32)]
        public int? MaxAddedPerFrame { get; set; }
    }
    [MetaClass("VfxBeamDefinitionData")]
    public class VfxBeamDefinitionData : IMetaClass
    {
        [MetaProperty("mSegments", BinPropertyType.Int32)]
        public int? Segments { get; set; }
        [MetaProperty("mBirthTilingSize", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueVector3> BirthTilingSize { get; set; }
        [MetaProperty("mIsColorBindedWithDistance", BinPropertyType.Bool)]
        public bool? IsColorBindedWithDistance { get; set; }
        [MetaProperty("mAnimatedColorWithDistance", BinPropertyType.Embedded)]
        public MetaEmbedded<ValueColor> AnimatedColorWithDistance { get; set; }
        [MetaProperty("mLocalSpaceSourceOffset", BinPropertyType.Vector3)]
        public Vector3? LocalSpaceSourceOffset { get; set; }
        [MetaProperty("mLocalSpaceTargetOffset", BinPropertyType.Vector3)]
        public Vector3? LocalSpaceTargetOffset { get; set; }
        [MetaProperty("mTrailMode", BinPropertyType.Byte)]
        public byte? TrailMode { get; set; }
        [MetaProperty("mMode", BinPropertyType.Byte)]
        public byte? Mode { get; set; }
    }
    [MetaClass("VfxEmissionSurfaceData")]
    public class VfxEmissionSurfaceData : IMetaClass
    {
        [MetaProperty("meshName", BinPropertyType.String)]
        public string? MeshName { get; set; }
        [MetaProperty("skeletonName", BinPropertyType.String)]
        public string? SkeletonName { get; set; }
        [MetaProperty("animationName", BinPropertyType.String)]
        public string? AnimationName { get; set; }
        [MetaProperty("meshScale", BinPropertyType.Float)]
        public float? MeshScale { get; set; }
        [MetaProperty("maxJointWeights", BinPropertyType.UInt16)]
        public ushort? MaxJointWeights { get; set; }
        [MetaProperty("useAvatarPose", BinPropertyType.Bool)]
        public bool? UseAvatarPose { get; set; }
        [MetaProperty("useSurfaceNormalForBirthPhysics", BinPropertyType.Bool)]
        public bool? UseSurfaceNormalForBirthPhysics { get; set; }
    }
    [MetaClass("VfxMeshDefinitionData")]
    public class VfxMeshDefinitionData : IMetaClass
    {
        [MetaProperty("mSimpleMeshName", BinPropertyType.String)]
        public string? SimpleMeshName { get; set; }
        [MetaProperty("mMeshName", BinPropertyType.String)]
        public string? MeshName { get; set; }
        [MetaProperty("mMeshSkeletonName", BinPropertyType.String)]
        public string? MeshSkeletonName { get; set; }
        [MetaProperty("mAnimationName", BinPropertyType.String)]
        public string? AnimationName { get; set; }
        [MetaProperty("mAnimationVariants", BinPropertyType.Container)]
        public MetaContainer<string> AnimationVariants { get; set; }
        [MetaProperty("mSubmeshesToDraw", BinPropertyType.Container)]
        public MetaContainer<MetaHash> SubmeshesToDraw { get; set; }
        [MetaProperty("mSubmeshesToDrawAlways", BinPropertyType.Container)]
        public MetaContainer<MetaHash> SubmeshesToDrawAlways { get; set; }
        [MetaProperty("mLockMeshToAttachment", BinPropertyType.Bool)]
        public bool? LockMeshToAttachment { get; set; }
    }
    [MetaClass("VfxPrimitiveBase")]
    public interface VfxPrimitiveBase : IMetaClass
    {
    }
    [MetaClass("VfxPrimitiveCameraQuad")]
    public class VfxPrimitiveCameraQuad : VfxPrimitiveBase
    {
    }
    [MetaClass("VfxPrimitiveArbitraryQuad")]
    public class VfxPrimitiveArbitraryQuad : VfxPrimitiveBase
    {
    }
    [MetaClass("VfxPrimitiveRay")]
    public class VfxPrimitiveRay : VfxPrimitiveBase
    {
    }
    [MetaClass("VfxPrimitiveProjectionBase")]
    public interface VfxPrimitiveProjectionBase : VfxPrimitiveBase
    {
        [MetaProperty("mProjection", BinPropertyType.Embedded)]
        MetaEmbedded<VfxProjectionDefinitionData> Projection { get; set; }
    }
    [MetaClass("VfxPrimitivePlanarProjection")]
    public class VfxPrimitivePlanarProjection : VfxPrimitiveProjectionBase
    {
        [MetaProperty("mProjection", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxProjectionDefinitionData> Projection { get; set; }
    }
    [MetaClass("VfxPrimitiveTrailBase")]
    public interface VfxPrimitiveTrailBase : VfxPrimitiveBase
    {
        [MetaProperty("mTrail", BinPropertyType.Embedded)]
        MetaEmbedded<VfxTrailDefinitionData> Trail { get; set; }
    }
    [MetaClass("VfxPrimitiveCameraTrail")]
    public class VfxPrimitiveCameraTrail : VfxPrimitiveTrailBase
    {
        [MetaProperty("mTrail", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxTrailDefinitionData> Trail { get; set; }
    }
    [MetaClass("VfxPrimitiveArbitraryTrail")]
    public class VfxPrimitiveArbitraryTrail : VfxPrimitiveTrailBase
    {
        [MetaProperty("mTrail", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxTrailDefinitionData> Trail { get; set; }
    }
    [MetaClass("VfxPrimitiveBeamBase")]
    public interface VfxPrimitiveBeamBase : VfxPrimitiveBase
    {
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; }
    }
    [MetaClass("VfxPrimitiveCameraSegmentBeam")]
    public class VfxPrimitiveCameraSegmentBeam : VfxPrimitiveBeamBase
    {
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; }
    }
    [MetaClass("VfxPrimitiveArbitrarySegmentBeam")]
    public class VfxPrimitiveArbitrarySegmentBeam : VfxPrimitiveBeamBase
    {
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; }
    }
    [MetaClass("VfxPrimitiveMeshBase")]
    public interface VfxPrimitiveMeshBase : VfxPrimitiveBase
    {
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; }
        [MetaProperty(4227234111, BinPropertyType.Bool)]
        bool? m4227234111 { get; set; }
        [MetaProperty(3934657962, BinPropertyType.Bool)]
        bool? m3934657962 { get; set; }
    }
    [MetaClass("VfxPrimitiveMesh")]
    public class VfxPrimitiveMesh : VfxPrimitiveMeshBase
    {
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; }
        [MetaProperty(4227234111, BinPropertyType.Bool)]
        public bool? m4227234111 { get; set; }
        [MetaProperty(3934657962, BinPropertyType.Bool)]
        public bool? m3934657962 { get; set; }
    }
    [MetaClass("VfxPrimitiveAttachedMesh")]
    public class VfxPrimitiveAttachedMesh : VfxPrimitiveMeshBase
    {
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; }
        [MetaProperty(4227234111, BinPropertyType.Bool)]
        public bool? m4227234111 { get; set; }
        [MetaProperty(3934657962, BinPropertyType.Bool)]
        public bool? m3934657962 { get; set; }
    }
    [MetaClass("VfxPrimitiveBeam")]
    public class VfxPrimitiveBeam : VfxPrimitiveBase
    {
        [MetaProperty("mMesh", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxMeshDefinitionData> Mesh { get; set; }
        [MetaProperty("mBeam", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxBeamDefinitionData> Beam { get; set; }
    }
    [MetaClass("VfxFloatOverLifeMaterialDriver")]
    public class VfxFloatOverLifeMaterialDriver : IVfxMaterialDriver
    {
        [MetaProperty("frequency", BinPropertyType.Byte)]
        public byte? Frequency { get; set; }
        [MetaProperty("graph", BinPropertyType.Embedded)]
        public MetaEmbedded<VfxAnimatedFloatVariableData> Graph { get; set; }
    }
    [MetaClass("VfxSineMaterialDriver")]
    public class VfxSineMaterialDriver : IVfxMaterialDriver
    {
        [MetaProperty("mFrequency", BinPropertyType.Float)]
        public float? Frequency { get; set; }
        [MetaProperty("mScale", BinPropertyType.Float)]
        public float? Scale { get; set; }
        [MetaProperty("mBias", BinPropertyType.Float)]
        public float? Bias { get; set; }
    }
    [MetaClass("MapParticle")]
    public class MapParticle : MapPlaceable
    {
        [MetaProperty("system", BinPropertyType.ObjectLink)]
        public MetaObjectLink? System { get; set; }
        [MetaProperty("eyeCandy", BinPropertyType.Bool)]
        public bool? EyeCandy { get; set; }
        [MetaProperty(2372739535, BinPropertyType.Bool)]
        public bool? m2372739535 { get; set; }
        [MetaProperty("quality", BinPropertyType.Int32)]
        public int? Quality { get; set; }
        [MetaProperty("visibilityMode", BinPropertyType.UInt32)]
        public uint? VisibilityMode { get; set; }
        [MetaProperty("colorModulate", BinPropertyType.Vector4)]
        public Vector4? ColorModulate { get; set; }
        [MetaProperty("groupName", BinPropertyType.String)]
        public string? GroupName { get; set; }
        [MetaProperty(1054618511, BinPropertyType.Bool)]
        public bool? m1054618511 { get; set; }
    }
    [MetaClass("MapParticleGroups")]
    public class MapParticleGroups : IMetaClass
    {
        [MetaProperty("groups", BinPropertyType.Container)]
        public MetaContainer<string> Groups { get; set; }
    }
    [MetaClass("ColorChooserMaterialDriver")]
    public class ColorChooserMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; }
        [MetaProperty("mColorOn", BinPropertyType.Vector4)]
        public Vector4? ColorOn { get; set; }
        [MetaProperty("mColorOff", BinPropertyType.Vector4)]
        public Vector4? ColorOff { get; set; }
    }
    [MetaClass("DelayedBoolMaterialDriver")]
    public class DelayedBoolMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; }
        [MetaProperty(3737718546, BinPropertyType.Float)]
        public float? m3737718546 { get; set; }
        [MetaProperty(446087716, BinPropertyType.Float)]
        public float? m446087716 { get; set; }
    }
    [MetaClass("DynamicMaterialParameterDef")]
    public class DynamicMaterialParameterDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialDriver Driver { get; set; }
    }
    [MetaClass("DynamicMaterialTextureSwapOption")]
    public class DynamicMaterialTextureSwapOption : IMetaClass
    {
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Driver { get; set; }
        [MetaProperty("textureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
    }
    [MetaClass("DynamicMaterialTextureSwapDef")]
    public class DynamicMaterialTextureSwapDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("options", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialTextureSwapOption>> Options { get; set; }
    }
    [MetaClass("DynamicMaterialStaticSwitch")]
    public class DynamicMaterialStaticSwitch : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("enabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("driver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Driver { get; set; }
    }
    [MetaClass("DynamicMaterialDef")]
    public class DynamicMaterialDef : IMetaClass
    {
        [MetaProperty(1590877069, BinPropertyType.Bool)]
        public bool? m1590877069 { get; set; }
        [MetaProperty("parameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialParameterDef>> Parameters { get; set; }
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialTextureSwapDef>> Textures { get; set; }
        [MetaProperty("staticSwitch", BinPropertyType.Structure)]
        public DynamicMaterialStaticSwitch StaticSwitch { get; set; }
    }
    [MetaClass("FixedDurationTriggeredBoolDriver")]
    public class FixedDurationTriggeredBoolDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty(2046475623, BinPropertyType.Float)]
        public float? m2046475623 { get; set; }
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; }
    }
    [MetaClass("FloatComparisonMaterialDriver")]
    public class FloatComparisonMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mOperator", BinPropertyType.UInt32)]
        public uint? Operator { get; set; }
        [MetaProperty("mValueA", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver ValueA { get; set; }
        [MetaProperty("mValueB", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver ValueB { get; set; }
    }
    [MetaClass("FloatLiteralMaterialDriver")]
    public class FloatLiteralMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mValue", BinPropertyType.Float)]
        public float? Value { get; set; }
    }
    [MetaClass("FloorFloatMaterialDriver")]
    public class FloorFloatMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; }
    }
    [MetaClass("IDynamicMaterialBoolDriver")]
    public interface IDynamicMaterialBoolDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass("IDynamicMaterialDriver")]
    public interface IDynamicMaterialDriver : IMetaClass
    {
    }
    [MetaClass("IDynamicMaterialFloatDriver")]
    public interface IDynamicMaterialFloatDriver : IDynamicMaterialDriver
    {
    }
    [MetaClass("LerpMaterialDriver")]
    public class LerpMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mBoolDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver BoolDriver { get; set; }
        [MetaProperty("mOnValue", BinPropertyType.Float)]
        public float? OnValue { get; set; }
        [MetaProperty("mOffValue", BinPropertyType.Float)]
        public float? OffValue { get; set; }
        [MetaProperty("mTurnOnTimeSec", BinPropertyType.Float)]
        public float? TurnOnTimeSec { get; set; }
        [MetaProperty("mTurnOffTimeSec", BinPropertyType.Float)]
        public float? TurnOffTimeSec { get; set; }
        [MetaProperty(2756886175, BinPropertyType.Bool)]
        public bool? m2756886175 { get; set; }
    }
    [MetaClass("AllTrueMaterialDriver")]
    public class AllTrueMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialBoolDriver> Drivers { get; set; }
    }
    [MetaClass("OneTrueMaterialDriver")]
    public class OneTrueMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialBoolDriver> Drivers { get; set; }
    }
    [MetaClass("NotMaterialDriver")]
    public class NotMaterialDriver : IDynamicMaterialBoolDriver
    {
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Driver { get; set; }
    }
    [MetaClass("MaxMaterialDriver")]
    public class MaxMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialDriver> Drivers { get; set; }
    }
    [MetaClass("MinMaterialDriver")]
    public class MinMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mDrivers", BinPropertyType.Container)]
        public MetaContainer<IDynamicMaterialDriver> Drivers { get; set; }
    }
    [MetaClass("RemapFloatMaterialDriver")]
    public class RemapFloatMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; }
        [MetaProperty("mMinValue", BinPropertyType.Float)]
        public float? MinValue { get; set; }
        [MetaProperty("mMaxValue", BinPropertyType.Float)]
        public float? MaxValue { get; set; }
        [MetaProperty("mOutputMinValue", BinPropertyType.Float)]
        public float? OutputMinValue { get; set; }
        [MetaProperty("mOutputMaxValue", BinPropertyType.Float)]
        public float? OutputMaxValue { get; set; }
    }
    [MetaClass("SineMaterialDriver")]
    public class SineMaterialDriver : IDynamicMaterialFloatDriver
    {
        [MetaProperty("mDriver", BinPropertyType.Structure)]
        public IDynamicMaterialFloatDriver Driver { get; set; }
        [MetaProperty("mFrequency", BinPropertyType.Float)]
        public float? Frequency { get; set; }
        [MetaProperty("mScale", BinPropertyType.Float)]
        public float? Scale { get; set; }
        [MetaProperty("mBias", BinPropertyType.Float)]
        public float? Bias { get; set; }
    }
    [MetaClass("SpecificColorMaterialDriver")]
    public class SpecificColorMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mColor", BinPropertyType.Vector4)]
        public Vector4? Color { get; set; }
    }
    [MetaClass("SwitchMaterialDriverElement")]
    public class SwitchMaterialDriverElement : IMetaClass
    {
        [MetaProperty("mCondition", BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver Condition { get; set; }
        [MetaProperty("mValue", BinPropertyType.Structure)]
        public IDynamicMaterialDriver Value { get; set; }
    }
    [MetaClass("SwitchMaterialDriver")]
    public class SwitchMaterialDriver : IDynamicMaterialDriver
    {
        [MetaProperty("mElements", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SwitchMaterialDriverElement>> Elements { get; set; }
        [MetaProperty("mDefaultValue", BinPropertyType.Structure)]
        public IDynamicMaterialDriver DefaultValue { get; set; }
    }
    [MetaClass("BlendingSwitchMaterialDriver")]
    public class BlendingSwitchMaterialDriver : SwitchMaterialDriver
    {
        [MetaProperty("mBlendTime", BinPropertyType.Float)]
        public float? BlendTime { get; set; }
        [MetaProperty(642664763, BinPropertyType.Container)]
        public MetaContainer<float> m642664763 { get; set; }
    }
    [MetaClass("TimeMaterialDriver")]
    public class TimeMaterialDriver : IDynamicMaterialFloatDriver
    {
    }
    [MetaClass("CustomShaderDef")]
    public class CustomShaderDef : IShaderDef
    {
        [MetaProperty("parameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderPhysicalParameter>> Parameters { get; set; }
        [MetaProperty("staticSwitches", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderStaticSwitch>> StaticSwitches { get; set; }
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderTexture>> Textures { get; set; }
        [MetaProperty("featureDefines", BinPropertyType.Map)]
        public Dictionary<string, string> FeatureDefines { get; set; }
        [MetaProperty("featureMask", BinPropertyType.UInt32)]
        public uint? FeatureMask { get; set; }
        [MetaProperty(2617146753, BinPropertyType.UInt32)]
        public uint? m2617146753 { get; set; }
        [MetaProperty("objectPath", BinPropertyType.String)]
        public string? ObjectPath { get; set; }
    }
    [MetaClass(176045846)]
    public class Class176045846 : IMetaClass
    {
        [MetaProperty("ID", BinPropertyType.UInt16)]
        public ushort? ID { get; set; }
        [MetaProperty("Count", BinPropertyType.UInt16)]
        public ushort? Count { get; set; }
    }
    [MetaClass("FixedShaderDef")]
    public class FixedShaderDef : IShaderDef
    {
        [MetaProperty("parameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderPhysicalParameter>> Parameters { get; set; }
        [MetaProperty("staticSwitches", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderStaticSwitch>> StaticSwitches { get; set; }
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderTexture>> Textures { get; set; }
        [MetaProperty("featureDefines", BinPropertyType.Map)]
        public Dictionary<string, string> FeatureDefines { get; set; }
        [MetaProperty("featureMask", BinPropertyType.UInt32)]
        public uint? FeatureMask { get; set; }
        [MetaProperty(2617146753, BinPropertyType.UInt32)]
        public uint? m2617146753 { get; set; }
        [MetaProperty("vertexShader", BinPropertyType.String)]
        public string? VertexShader { get; set; }
        [MetaProperty("pixelShader", BinPropertyType.String)]
        public string? PixelShader { get; set; }
    }
    [MetaClass(1659255250)]
    public class Class1659255250 : IMetaClass
    {
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; }
        [MetaProperty("depthEnable", BinPropertyType.Bool)]
        public bool? DepthEnable { get; set; }
        [MetaProperty("stencilEnable", BinPropertyType.Bool)]
        public bool? StencilEnable { get; set; }
        [MetaProperty("blendEnable", BinPropertyType.Bool)]
        public bool? BlendEnable { get; set; }
        [MetaProperty("cullEnable", BinPropertyType.Bool)]
        public bool? CullEnable { get; set; }
        [MetaProperty(2863927372, BinPropertyType.Bool)]
        public bool? m2863927372 { get; set; }
        [MetaProperty("depthCompareFunc", BinPropertyType.Byte)]
        public byte? DepthCompareFunc { get; set; }
        [MetaProperty("stencilCompareFunc", BinPropertyType.Byte)]
        public byte? StencilCompareFunc { get; set; }
        [MetaProperty("stencilReferenceVal", BinPropertyType.UInt32)]
        public uint? StencilReferenceVal { get; set; }
        [MetaProperty("stencilMask", BinPropertyType.UInt32)]
        public uint? StencilMask { get; set; }
        [MetaProperty("stencilFailOp", BinPropertyType.Byte)]
        public byte? StencilFailOp { get; set; }
        [MetaProperty("stencilPassDepthFailOp", BinPropertyType.Byte)]
        public byte? StencilPassDepthFailOp { get; set; }
        [MetaProperty("stencilPassDepthPassOp", BinPropertyType.Byte)]
        public byte? StencilPassDepthPassOp { get; set; }
        [MetaProperty("srcColorBlendFactor", BinPropertyType.Byte)]
        public byte? SrcColorBlendFactor { get; set; }
        [MetaProperty("srcAlphaBlendFactor", BinPropertyType.Byte)]
        public byte? SrcAlphaBlendFactor { get; set; }
        [MetaProperty("dstColorBlendFactor", BinPropertyType.Byte)]
        public byte? DstColorBlendFactor { get; set; }
        [MetaProperty("dstAlphaBlendFactor", BinPropertyType.Byte)]
        public byte? DstAlphaBlendFactor { get; set; }
        [MetaProperty("blendEquation", BinPropertyType.Byte)]
        public byte? BlendEquation { get; set; }
        [MetaProperty("windingToCull", BinPropertyType.Byte)]
        public byte? WindingToCull { get; set; }
        [MetaProperty("writeMask", BinPropertyType.Byte)]
        public byte? WriteMask { get; set; }
        [MetaProperty("depthOffsetSlope", BinPropertyType.Float)]
        public float? DepthOffsetSlope { get; set; }
        [MetaProperty("depthOffsetBias", BinPropertyType.Float)]
        public float? DepthOffsetBias { get; set; }
    }
    [MetaClass("HybridMaterialDef")]
    public class HybridMaterialDef : CustomShaderDef
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("preset", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Preset { get; set; }
        [MetaProperty(2866812454, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1874373301> m2866812454 { get; set; }
    }
    [MetaClass("IMaterialDef")]
    public interface IMaterialDef : IMetaClass
    {
    }
    [MetaClass("ShaderLogicalParameter")]
    public class ShaderLogicalParameter : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("fields", BinPropertyType.UInt32)]
        public uint? Fields { get; set; }
    }
    [MetaClass("ShaderPhysicalParameter")]
    public class ShaderPhysicalParameter : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("data", BinPropertyType.Vector4)]
        public Vector4? Data { get; set; }
        [MetaProperty("logicalParameters", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<ShaderLogicalParameter>> LogicalParameters { get; set; }
    }
    [MetaClass("ShaderStaticSwitch")]
    public class ShaderStaticSwitch : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("onByDefault", BinPropertyType.Bool)]
        public bool? OnByDefault { get; set; }
    }
    [MetaClass("ShaderTexture")]
    public class ShaderTexture : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("defaultTexturePath", BinPropertyType.String)]
        public string? DefaultTexturePath { get; set; }
    }
    [MetaClass("IShaderDef")]
    public interface IShaderDef : IMetaClass
    {
        [MetaProperty("parameters", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ShaderPhysicalParameter>> Parameters { get; set; }
        [MetaProperty("staticSwitches", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ShaderStaticSwitch>> StaticSwitches { get; set; }
        [MetaProperty("textures", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ShaderTexture>> Textures { get; set; }
        [MetaProperty("featureDefines", BinPropertyType.Map)]
        Dictionary<string, string> FeatureDefines { get; set; }
        [MetaProperty("featureMask", BinPropertyType.UInt32)]
        uint? FeatureMask { get; set; }
        [MetaProperty(2617146753, BinPropertyType.UInt32)]
        uint? m2617146753 { get; set; }
    }
    [MetaClass(1407148951)]
    public class Class1407148951 : IMetaClass
    {
        [MetaProperty("type", BinPropertyType.Byte)]
        public byte? Type { get; set; }
        [MetaProperty("DefaultValue", BinPropertyType.Vector4)]
        public Vector4? DefaultValue { get; set; }
    }
    [MetaClass(3791453475)]
    public class Class3791453475 : IMetaClass
    {
        [MetaProperty("defaultTexturePath", BinPropertyType.String)]
        public string? DefaultTexturePath { get; set; }
        [MetaProperty("addressU", BinPropertyType.Byte)]
        public byte? AddressU { get; set; }
        [MetaProperty("addressV", BinPropertyType.Byte)]
        public byte? AddressV { get; set; }
        [MetaProperty("addressW", BinPropertyType.Byte)]
        public byte? AddressW { get; set; }
        [MetaProperty("filterMin", BinPropertyType.Byte)]
        public byte? FilterMin { get; set; }
        [MetaProperty("filterMip", BinPropertyType.Byte)]
        public byte? FilterMip { get; set; }
        [MetaProperty("filterMag", BinPropertyType.Byte)]
        public byte? FilterMag { get; set; }
    }
    [MetaClass(1327860340)]
    public class Class1327860340 : IMetaClass
    {
        [MetaProperty("on", BinPropertyType.Bool)]
        public bool? On { get; set; }
    }
    [MetaClass(3921803671)]
    public class Class3921803671 : IMetaClass
    {
        [MetaProperty("nextID", BinPropertyType.UInt16)]
        public ushort? NextID { get; set; }
        [MetaProperty(1868071667, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<Class176045846>> m1868071667 { get; set; }
        [MetaProperty(3931619090, BinPropertyType.String)]
        public string? m3931619090 { get; set; }
        [MetaProperty("data", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<Class1407148951>> Data { get; set; }
    }
    [MetaClass(1749974331)]
    public class Class1749974331 : IMetaClass
    {
        [MetaProperty("nextID", BinPropertyType.UInt16)]
        public ushort? NextID { get; set; }
        [MetaProperty(1868071667, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<Class176045846>> m1868071667 { get; set; }
        [MetaProperty(3931619090, BinPropertyType.String)]
        public string? m3931619090 { get; set; }
        [MetaProperty("data", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<Class3791453475>> Data { get; set; }
    }
    [MetaClass(3078356408)]
    public class Class3078356408 : IMetaClass
    {
        [MetaProperty("nextID", BinPropertyType.UInt16)]
        public ushort? NextID { get; set; }
        [MetaProperty(1868071667, BinPropertyType.Map)]
        public Dictionary<string, MetaEmbedded<Class176045846>> m1868071667 { get; set; }
        [MetaProperty(3931619090, BinPropertyType.String)]
        public string? m3931619090 { get; set; }
        [MetaProperty("data", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<Class1327860340>> Data { get; set; }
    }
    [MetaClass(1874373301)]
    public class Class1874373301 : IMetaClass
    {
        [MetaProperty(2084884472, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3921803671> m2084884472 { get; set; }
        [MetaProperty(3842163164, BinPropertyType.Embedded)]
        public MetaEmbedded<Class1749974331> m3842163164 { get; set; }
        [MetaProperty(2718256129, BinPropertyType.Embedded)]
        public MetaEmbedded<Class3078356408> m2718256129 { get; set; }
    }
    [MetaClass("MaterialInstanceParamDef")]
    public class MaterialInstanceParamDef : IMetaClass
    {
        [MetaProperty("value", BinPropertyType.Vector4)]
        public Vector4? Value { get; set; }
    }
    [MetaClass(2719807978)]
    public class Class2719807978 : IMetaClass
    {
        [MetaProperty("on", BinPropertyType.Bool)]
        public bool? On { get; set; }
    }
    [MetaClass(1305331905)]
    public class Class1305331905 : IMetaClass
    {
        [MetaProperty("texturePath", BinPropertyType.String)]
        public string? TexturePath { get; set; }
        [MetaProperty("uncensoredTextures", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredTextures { get; set; }
    }
    [MetaClass(1083476907)]
    public class Class1083476907 : IMetaClass
    {
        [MetaProperty("options", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<DynamicMaterialTextureSwapOption>> Options { get; set; }
    }
    [MetaClass("MaterialInstanceDef")]
    public class MaterialInstanceDef : IResource,  IMaterialDef
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty(2404001921, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2404001921 { get; set; }
        [MetaProperty("params", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<MaterialInstanceParamDef>> Params { get; set; }
        [MetaProperty("textures", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<Class1305331905>> Textures { get; set; }
        [MetaProperty("switches", BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<Class2719807978>> Switches { get; set; }
        [MetaProperty(2233445620, BinPropertyType.Map)]
        public Dictionary<ushort, IDynamicMaterialDriver> m2233445620 { get; set; }
        [MetaProperty(1873541542, BinPropertyType.Map)]
        public Dictionary<ushort, MetaEmbedded<Class1083476907>> m1873541542 { get; set; }
        [MetaProperty(1887807190, BinPropertyType.Structure)]
        public IDynamicMaterialBoolDriver m1887807190 { get; set; }
        [MetaProperty(2613483115, BinPropertyType.UInt16)]
        public ushort? m2613483115 { get; set; }
    }
    [MetaClass("StaticMaterialShaderParamDef")]
    public class StaticMaterialShaderParamDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("value", BinPropertyType.Vector4)]
        public Vector4? Value { get; set; }
    }
    [MetaClass("StaticMaterialSwitchDef")]
    public class StaticMaterialSwitchDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("on", BinPropertyType.Bool)]
        public bool? On { get; set; }
    }
    [MetaClass("StaticMaterialShaderSamplerDef")]
    public class StaticMaterialShaderSamplerDef : IMetaClass
    {
        [MetaProperty("samplerName", BinPropertyType.String)]
        public string? SamplerName { get; set; }
        [MetaProperty("textureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
        [MetaProperty("uncensoredTextures", BinPropertyType.Map)]
        public Dictionary<MetaHash, string> UncensoredTextures { get; set; }
        [MetaProperty("addressU", BinPropertyType.UInt32)]
        public uint? AddressU { get; set; }
        [MetaProperty("addressV", BinPropertyType.UInt32)]
        public uint? AddressV { get; set; }
        [MetaProperty("addressW", BinPropertyType.UInt32)]
        public uint? AddressW { get; set; }
        [MetaProperty("filterMin", BinPropertyType.UInt32)]
        public uint? FilterMin { get; set; }
        [MetaProperty("filterMip", BinPropertyType.UInt32)]
        public uint? FilterMip { get; set; }
        [MetaProperty("filterMag", BinPropertyType.UInt32)]
        public uint? FilterMag { get; set; }
    }
    [MetaClass("StaticMaterialPassDef")]
    public class StaticMaterialPassDef : IMetaClass
    {
        [MetaProperty("shader", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Shader { get; set; }
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; }
        [MetaProperty("paramValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialShaderParamDef>> ParamValues { get; set; }
        [MetaProperty("depthEnable", BinPropertyType.Bool)]
        public bool? DepthEnable { get; set; }
        [MetaProperty("stencilEnable", BinPropertyType.Bool)]
        public bool? StencilEnable { get; set; }
        [MetaProperty("blendEnable", BinPropertyType.Bool)]
        public bool? BlendEnable { get; set; }
        [MetaProperty("cullEnable", BinPropertyType.Bool)]
        public bool? CullEnable { get; set; }
        [MetaProperty(2863927372, BinPropertyType.Bool)]
        public bool? m2863927372 { get; set; }
        [MetaProperty("depthCompareFunc", BinPropertyType.UInt32)]
        public uint? DepthCompareFunc { get; set; }
        [MetaProperty("stencilCompareFunc", BinPropertyType.UInt32)]
        public uint? StencilCompareFunc { get; set; }
        [MetaProperty("stencilReferenceVal", BinPropertyType.UInt32)]
        public uint? StencilReferenceVal { get; set; }
        [MetaProperty("stencilMask", BinPropertyType.UInt32)]
        public uint? StencilMask { get; set; }
        [MetaProperty("stencilFailOp", BinPropertyType.UInt32)]
        public uint? StencilFailOp { get; set; }
        [MetaProperty("stencilPassDepthFailOp", BinPropertyType.UInt32)]
        public uint? StencilPassDepthFailOp { get; set; }
        [MetaProperty("stencilPassDepthPassOp", BinPropertyType.UInt32)]
        public uint? StencilPassDepthPassOp { get; set; }
        [MetaProperty("srcColorBlendFactor", BinPropertyType.UInt32)]
        public uint? SrcColorBlendFactor { get; set; }
        [MetaProperty("srcAlphaBlendFactor", BinPropertyType.UInt32)]
        public uint? SrcAlphaBlendFactor { get; set; }
        [MetaProperty("dstColorBlendFactor", BinPropertyType.UInt32)]
        public uint? DstColorBlendFactor { get; set; }
        [MetaProperty("dstAlphaBlendFactor", BinPropertyType.UInt32)]
        public uint? DstAlphaBlendFactor { get; set; }
        [MetaProperty("blendEquation", BinPropertyType.UInt32)]
        public uint? BlendEquation { get; set; }
        [MetaProperty("windingToCull", BinPropertyType.UInt32)]
        public uint? WindingToCull { get; set; }
        [MetaProperty("writeMask", BinPropertyType.UInt32)]
        public uint? WriteMask { get; set; }
        [MetaProperty("depthOffsetSlope", BinPropertyType.Float)]
        public float? DepthOffsetSlope { get; set; }
        [MetaProperty("depthOffsetBias", BinPropertyType.Float)]
        public float? DepthOffsetBias { get; set; }
    }
    [MetaClass("StaticMaterialTechniqueDef")]
    public class StaticMaterialTechniqueDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("passes", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialPassDef>> Passes { get; set; }
    }
    [MetaClass("StaticMaterialChildTechniqueDef")]
    public class StaticMaterialChildTechniqueDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("parentName", BinPropertyType.String)]
        public string? ParentName { get; set; }
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; }
    }
    [MetaClass("StaticMaterialDef")]
    public class StaticMaterialDef : IResource,  IMaterialDef
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("defaultTechnique", BinPropertyType.String)]
        public string? DefaultTechnique { get; set; }
        [MetaProperty("samplerValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialShaderSamplerDef>> SamplerValues { get; set; }
        [MetaProperty("paramValues", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialShaderParamDef>> ParamValues { get; set; }
        [MetaProperty("switches", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialSwitchDef>> Switches { get; set; }
        [MetaProperty("shaderMacros", BinPropertyType.Map)]
        public Dictionary<string, string> ShaderMacros { get; set; }
        [MetaProperty("techniques", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialTechniqueDef>> Techniques { get; set; }
        [MetaProperty("childTechniques", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<StaticMaterialChildTechniqueDef>> ChildTechniques { get; set; }
        [MetaProperty("dynamicMaterial", BinPropertyType.Structure)]
        public DynamicMaterialDef DynamicMaterial { get; set; }
    }
    [MetaClass("MapPerInstanceInfo")]
    public class MapPerInstanceInfo : IMetaClass
    {
        [MetaProperty("shadowMapPath", BinPropertyType.String)]
        public string? ShadowMapPath { get; set; }
        [MetaProperty("shadowMapUVScaleAndBias", BinPropertyType.Vector4)]
        public Vector4? ShadowMapUVScaleAndBias { get; set; }
    }
    [MetaClass("MapBakeProperties")]
    public class MapBakeProperties : MapComponent
    {
        [MetaProperty("lightGridSize", BinPropertyType.UInt32)]
        public uint? LightGridSize { get; set; }
        [MetaProperty(584207002, BinPropertyType.Float)]
        public float? m584207002 { get; set; }
        [MetaProperty("lightGridCharacterFullBrightIntensity", BinPropertyType.Float)]
        public float? LightGridCharacterFullBrightIntensity { get; set; }
        [MetaProperty(3931004104, BinPropertyType.Float)]
        public float? m3931004104 { get; set; }
        [MetaProperty(792417393, BinPropertyType.Float)]
        public float? m792417393 { get; set; }
        [MetaProperty("lightGridFileName", BinPropertyType.String)]
        public string? LightGridFileName { get; set; }
    }
    [MetaClass("MapCloudsLayer")]
    public class MapCloudsLayer : IMetaClass
    {
        [MetaProperty("scale", BinPropertyType.Float)]
        public float? Scale { get; set; }
        [MetaProperty("speed", BinPropertyType.Float)]
        public float? Speed { get; set; }
        [MetaProperty("direction", BinPropertyType.Vector2)]
        public Vector2? Direction { get; set; }
    }
    [MetaClass("MapClouds")]
    public class MapClouds : MapGraphicsFeature
    {
        [MetaProperty(1664582205, BinPropertyType.String)]
        public string? m1664582205 { get; set; }
        [MetaProperty(1686403411, BinPropertyType.Vector4)]
        public Vector4? m1686403411 { get; set; }
        [MetaProperty("Layers", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<MapCloudsLayer>> Layers { get; set; }
        [MetaProperty(839153119, BinPropertyType.Bool)]
        public bool? m839153119 { get; set; }
    }
    [MetaClass("MapGraphicsFeature")]
    public interface MapGraphicsFeature : MapComponent
    {
    }
    [MetaClass("MapLightingV2")]
    public class MapLightingV2 : MapGraphicsFeature
    {
        [MetaProperty(4002480509, BinPropertyType.Float)]
        public float? m4002480509 { get; set; }
        [MetaProperty(3373705724, BinPropertyType.Float)]
        public float? m3373705724 { get; set; }
    }
    [MetaClass("MapLightingVolume")]
    public class MapLightingVolume : MapPlaceable
    {
        [MetaProperty("sunColor", BinPropertyType.Vector4)]
        public Vector4? SunColor { get; set; }
        [MetaProperty("sunDirection", BinPropertyType.Vector3)]
        public Vector3? SunDirection { get; set; }
        [MetaProperty(2689325503, BinPropertyType.Optional)]
        public MetaOptional<Vector3> m2689325503 { get; set; }
        [MetaProperty(3632599555, BinPropertyType.Float)]
        public float? m3632599555 { get; set; }
        [MetaProperty(3120754966, BinPropertyType.Float)]
        public float? m3120754966 { get; set; }
        [MetaProperty("skyLightColor", BinPropertyType.Vector4)]
        public Vector4? SkyLightColor { get; set; }
        [MetaProperty("horizonColor", BinPropertyType.Vector4)]
        public Vector4? HorizonColor { get; set; }
        [MetaProperty("groundColor", BinPropertyType.Vector4)]
        public Vector4? GroundColor { get; set; }
        [MetaProperty("skyLightScale", BinPropertyType.Float)]
        public float? SkyLightScale { get; set; }
        [MetaProperty("lightMapColorScale", BinPropertyType.Float)]
        public float? LightMapColorScale { get; set; }
        [MetaProperty("fogEnabled", BinPropertyType.Bool)]
        public bool? FogEnabled { get; set; }
        [MetaProperty("fogColor", BinPropertyType.Vector4)]
        public Vector4? FogColor { get; set; }
        [MetaProperty("fogAlternateColor", BinPropertyType.Vector4)]
        public Vector4? FogAlternateColor { get; set; }
        [MetaProperty("fogStartAndEnd", BinPropertyType.Vector2)]
        public Vector2? FogStartAndEnd { get; set; }
        [MetaProperty("fogEmissiveRemap", BinPropertyType.Float)]
        public float? FogEmissiveRemap { get; set; }
        [MetaProperty("fogLowQualityModeEmissiveRemap", BinPropertyType.Float)]
        public float? FogLowQualityModeEmissiveRemap { get; set; }
    }
    [MetaClass("MapPointLight")]
    public class MapPointLight : MapPlaceable
    {
        [MetaProperty("type", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Type { get; set; }
        [MetaProperty("radiusScale", BinPropertyType.Float)]
        public float? RadiusScale { get; set; }
        [MetaProperty("intensityScale", BinPropertyType.Float)]
        public float? IntensityScale { get; set; }
        [MetaProperty("overrideCastStaticShadows", BinPropertyType.Optional)]
        public MetaOptional<bool> OverrideCastStaticShadows { get; set; }
        [MetaProperty("overrideUseSpecular", BinPropertyType.Optional)]
        public MetaOptional<bool> OverrideUseSpecular { get; set; }
    }
    [MetaClass("MapPointLightType")]
    public class MapPointLightType : IMetaClass
    {
        [MetaProperty("lightColor", BinPropertyType.Vector4)]
        public Vector4? LightColor { get; set; }
        [MetaProperty("falloffColor", BinPropertyType.Vector4)]
        public Vector4? FalloffColor { get; set; }
        [MetaProperty("radius", BinPropertyType.Float)]
        public float? Radius { get; set; }
        [MetaProperty("castStaticShadows", BinPropertyType.Bool)]
        public bool? CastStaticShadows { get; set; }
        [MetaProperty("specular", BinPropertyType.Bool)]
        public bool? Specular { get; set; }
        [MetaProperty("Impact", BinPropertyType.Int32)]
        public int? Impact { get; set; }
    }
    [MetaClass("MapSunProperties")]
    public class MapSunProperties : MapComponent
    {
        [MetaProperty("sunColor", BinPropertyType.Vector4)]
        public Vector4? SunColor { get; set; }
        [MetaProperty("sunDirection", BinPropertyType.Vector3)]
        public Vector3? SunDirection { get; set; }
        [MetaProperty(2689325503, BinPropertyType.Optional)]
        public MetaOptional<Vector3> m2689325503 { get; set; }
        [MetaProperty(3632599555, BinPropertyType.Float)]
        public float? m3632599555 { get; set; }
        [MetaProperty(3120754966, BinPropertyType.Float)]
        public float? m3120754966 { get; set; }
        [MetaProperty("skyLightColor", BinPropertyType.Vector4)]
        public Vector4? SkyLightColor { get; set; }
        [MetaProperty("horizonColor", BinPropertyType.Vector4)]
        public Vector4? HorizonColor { get; set; }
        [MetaProperty("groundColor", BinPropertyType.Vector4)]
        public Vector4? GroundColor { get; set; }
        [MetaProperty("skyLightScale", BinPropertyType.Float)]
        public float? SkyLightScale { get; set; }
        [MetaProperty("lightMapColorScale", BinPropertyType.Float)]
        public float? LightMapColorScale { get; set; }
        [MetaProperty("fogEnabled", BinPropertyType.Bool)]
        public bool? FogEnabled { get; set; }
        [MetaProperty("fogColor", BinPropertyType.Vector4)]
        public Vector4? FogColor { get; set; }
        [MetaProperty("fogAlternateColor", BinPropertyType.Vector4)]
        public Vector4? FogAlternateColor { get; set; }
        [MetaProperty("fogStartAndEnd", BinPropertyType.Vector2)]
        public Vector2? FogStartAndEnd { get; set; }
        [MetaProperty("fogEmissiveRemap", BinPropertyType.Float)]
        public float? FogEmissiveRemap { get; set; }
        [MetaProperty("fogLowQualityModeEmissiveRemap", BinPropertyType.Float)]
        public float? FogLowQualityModeEmissiveRemap { get; set; }
        [MetaProperty("useBloom", BinPropertyType.Bool)]
        public bool? UseBloom { get; set; }
        [MetaProperty("surfaceAreaToShadowMapScale", BinPropertyType.Float)]
        public float? SurfaceAreaToShadowMapScale { get; set; }
    }
    [MetaClass("MapTerrainPaint")]
    public class MapTerrainPaint : MapGraphicsFeature
    {
        [MetaProperty(3270086406, BinPropertyType.String)]
        public string? m3270086406 { get; set; }
    }
    [MetaClass("SHData")]
    public class SHData : IMetaClass
    {
        [MetaProperty("bandData", BinPropertyType.Container)]
        public MetaContainer<Vector3> BandData { get; set; }
    }
    [MetaClass("SkinMeshDataProperties_MaterialOverride")]
    public class SkinMeshDataProperties_MaterialOverride : IMetaClass
    {
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Material { get; set; }
        [MetaProperty("texture", BinPropertyType.String)]
        public string? Texture { get; set; }
        [MetaProperty("glossTexture", BinPropertyType.String)]
        public string? GlossTexture { get; set; }
        [MetaProperty("submesh", BinPropertyType.String)]
        public string? Submesh { get; set; }
    }
    [MetaClass("SkinnedMeshDataMaterialController")]
    public interface SkinnedMeshDataMaterialController : IMetaClass
    {
    }
    [MetaClass("SkinMeshDataProperties")]
    public class SkinMeshDataProperties : IMetaClass
    {
        [MetaProperty("skeleton", BinPropertyType.String)]
        public string? Skeleton { get; set; }
        [MetaProperty("simpleSkin", BinPropertyType.String)]
        public string? SimpleSkin { get; set; }
        [MetaProperty(3593334908, BinPropertyType.Bool)]
        public bool? m3593334908 { get; set; }
        [MetaProperty("texture", BinPropertyType.String)]
        public string? Texture { get; set; }
        [MetaProperty("emissiveTexture", BinPropertyType.String)]
        public string? EmissiveTexture { get; set; }
        [MetaProperty("glossTexture", BinPropertyType.String)]
        public string? GlossTexture { get; set; }
        [MetaProperty("skinScale", BinPropertyType.Float)]
        public float? SkinScale { get; set; }
        [MetaProperty("selfIllumination", BinPropertyType.Float)]
        public float? SelfIllumination { get; set; }
        [MetaProperty("brushAlphaOverride", BinPropertyType.Float)]
        public float? BrushAlphaOverride { get; set; }
        [MetaProperty("overrideBoundingBox", BinPropertyType.Optional)]
        public MetaOptional<Vector3> OverrideBoundingBox { get; set; }
        [MetaProperty("material", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Material { get; set; }
        [MetaProperty("boundingCylinderRadius", BinPropertyType.Float)]
        public float? BoundingCylinderRadius { get; set; }
        [MetaProperty("boundingCylinderHeight", BinPropertyType.Float)]
        public float? BoundingCylinderHeight { get; set; }
        [MetaProperty("boundingSphereRadius", BinPropertyType.Optional)]
        public MetaOptional<float> BoundingSphereRadius { get; set; }
        [MetaProperty("fresnelColor", BinPropertyType.Color)]
        public Color? FresnelColor { get; set; }
        [MetaProperty("fresnel", BinPropertyType.Float)]
        public float? Fresnel { get; set; }
        [MetaProperty("usesSkinVO", BinPropertyType.Bool)]
        public bool? UsesSkinVO { get; set; }
        [MetaProperty("castShadows", BinPropertyType.Bool)]
        public bool? CastShadows { get; set; }
        [MetaProperty("allowCharacterInking", BinPropertyType.Bool)]
        public bool? AllowCharacterInking { get; set; }
        [MetaProperty("reflectionMap", BinPropertyType.String)]
        public string? ReflectionMap { get; set; }
        [MetaProperty("reflectionOpacityDirect", BinPropertyType.Float)]
        public float? ReflectionOpacityDirect { get; set; }
        [MetaProperty("reflectionOpacityGlancing", BinPropertyType.Float)]
        public float? ReflectionOpacityGlancing { get; set; }
        [MetaProperty("reflectionFresnel", BinPropertyType.Float)]
        public float? ReflectionFresnel { get; set; }
        [MetaProperty("reflectionFresnelColor", BinPropertyType.Color)]
        public Color? ReflectionFresnelColor { get; set; }
        [MetaProperty("initialSubmeshToHide", BinPropertyType.String)]
        public string? InitialSubmeshToHide { get; set; }
        [MetaProperty("initialSubmeshShadowsToHide", BinPropertyType.String)]
        public string? InitialSubmeshShadowsToHide { get; set; }
        [MetaProperty("initialSubmeshMouseOversToHide", BinPropertyType.String)]
        public string? InitialSubmeshMouseOversToHide { get; set; }
        [MetaProperty("submeshRenderOrder", BinPropertyType.String)]
        public string? SubmeshRenderOrder { get; set; }
        [MetaProperty("materialOverride", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SkinMeshDataProperties_MaterialOverride>> MaterialOverride { get; set; }
        [MetaProperty(2059875848, BinPropertyType.Structure)]
        public SkinnedMeshDataMaterialController m2059875848 { get; set; }
        [MetaProperty("rigPoseModifierData", BinPropertyType.Container)]
        public MetaContainer<BaseRigPoseModifierData> RigPoseModifierData { get; set; }
    }
    [MetaClass("BaseResourceResolver")]
    public class BaseResourceResolver : IResourceResolver
    {
        [MetaProperty("resourceMap", BinPropertyType.Map)]
        public Dictionary<MetaHash, MetaObjectLink> ResourceMap { get; set; }
    }
    [MetaClass("GlobalResourceResolver")]
    public class GlobalResourceResolver : BaseResourceResolver
    {
    }
    [MetaClass("IResource")]
    public interface IResource : IMetaClass
    {
    }
    [MetaClass("IResourceResolver")]
    public interface IResourceResolver : IMetaClass
    {
    }
    [MetaClass("ResourceResolver")]
    public class ResourceResolver : BaseResourceResolver
    {
    }
    [MetaClass("SetVarInTableBlock")]
    public class SetVarInTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; }
        [MetaProperty("Dest", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> Dest { get; set; }
    }
    [MetaClass("ScriptCommentBlock")]
    public class ScriptCommentBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; }
    }
    [MetaClass("ConcatenateStringsBlock")]
    public class ConcatenateStringsBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("String1", BinPropertyType.Structure)]
        public IStringGet String1 { get; set; }
        [MetaProperty("String2", BinPropertyType.Structure)]
        public IStringGet String2 { get; set; }
        [MetaProperty("Result", BinPropertyType.Embedded)]
        public MetaEmbedded<StringTableSet> Result { get; set; }
    }
    [MetaClass("CreateCustomTableBlock")]
    public class CreateCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableSet> CustomTable { get; set; }
    }
    [MetaClass("DestroyCustomTableBlock")]
    public class DestroyCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableSet> CustomTable { get; set; }
    }
    [MetaClass("SetKeyValueInCustomTableBlock")]
    public class SetKeyValueInCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; }
        [MetaProperty("Key", BinPropertyType.Structure)]
        public IScriptValueGet Key { get; set; }
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; }
    }
    [MetaClass("GetKeyValueInCustomTableBlock")]
    public class GetKeyValueInCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; }
        [MetaProperty("Key", BinPropertyType.Structure)]
        public IScriptValueGet Key { get; set; }
        [MetaProperty("OutValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutValue { get; set; }
    }
    [MetaClass("InsertIntoCustomTableBlock")]
    public class InsertIntoCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; }
        [MetaProperty("Index", BinPropertyType.Structure)]
        public IIntGet Index { get; set; }
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; }
        [MetaProperty("OutIndex", BinPropertyType.Embedded)]
        public MetaEmbedded<IntTableSet> OutIndex { get; set; }
    }
    [MetaClass("RemoveFromCustomTableBlock")]
    public class RemoveFromCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; }
        [MetaProperty("Key", BinPropertyType.Structure)]
        public IScriptValueGet Key { get; set; }
        [MetaProperty("Index", BinPropertyType.Structure)]
        public IIntGet Index { get; set; }
    }
    [MetaClass("GetSizeOfCustomTableBlock")]
    public class GetSizeOfCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; }
        [MetaProperty("OutSize", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutSize { get; set; }
    }
    [MetaClass("ForEachInCustomTableBlock")]
    public class ForEachInCustomTableBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; }
        [MetaProperty("SortedByKeys", BinPropertyType.Bool)]
        public bool? SortedByKeys { get; set; }
        [MetaProperty("OutKey", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutKey { get; set; }
        [MetaProperty("OutValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutValue { get; set; }
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; }
    }
    [MetaClass("CustomTableContainsValueBlock")]
    public class CustomTableContainsValueBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("CustomTable", BinPropertyType.Embedded)]
        public MetaEmbedded<CustomTableGet> CustomTable { get; set; }
        [MetaProperty("value", BinPropertyType.Structure)]
        public IScriptValueGet Value { get; set; }
        [MetaProperty("OutKey", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableSet> OutKey { get; set; }
        [MetaProperty("OutWasFound", BinPropertyType.Embedded)]
        public MetaEmbedded<BoolTableSet> OutWasFound { get; set; }
    }
    [MetaClass("FunctionDefinition")]
    public class FunctionDefinition : IMetaClass
    {
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; }
        [MetaProperty("InputParameters", BinPropertyType.Container)]
        public MetaContainer<MetaHash> InputParameters { get; set; }
        [MetaProperty("OutputParameters", BinPropertyType.Container)]
        public MetaContainer<MetaHash> OutputParameters { get; set; }
    }
    [MetaClass("CreateFunctionBlock")]
    public class CreateFunctionBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("Function", BinPropertyType.Embedded)]
        public MetaEmbedded<FunctionTableSet> Function { get; set; }
        [MetaProperty("FunctionDefinition", BinPropertyType.Embedded)]
        public MetaEmbedded<FunctionDefinition> FunctionDefinition { get; set; }
    }
    [MetaClass("IRunFunctionBlock")]
    public interface IRunFunctionBlock : IScriptBlock
    {
        [MetaProperty("Function", BinPropertyType.Embedded)]
        MetaEmbedded<FunctionTableGet> Function { get; set; }
        [MetaProperty("InputParameters", BinPropertyType.Container)]
        MetaContainer<IScriptValueGet> InputParameters { get; set; }
        [MetaProperty("OutputParameters", BinPropertyType.Container)]
        MetaContainer<MetaEmbedded<ScriptTableSet>> OutputParameters { get; set; }
    }
    [MetaClass("IScriptBlock")]
    public interface IScriptBlock : IMetaClass
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        bool? IsDisabled { get; set; }
    }
    [MetaClass("SwitchCase")]
    public class SwitchCase : IMetaClass
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("Condition", BinPropertyType.Structure)]
        public IScriptCondition Condition { get; set; }
        [MetaProperty("Sequence", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptSequence> Sequence { get; set; }
    }
    [MetaClass("SwitchScriptBlock")]
    public class SwitchScriptBlock : IScriptBlock
    {
        [MetaProperty("IsDisabled", BinPropertyType.Bool)]
        public bool? IsDisabled { get; set; }
        [MetaProperty("Cases", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<SwitchCase>> Cases { get; set; }
    }
    [MetaClass("IScriptCondition")]
    public interface IScriptCondition : IMetaClass
    {
    }
    [MetaClass("TableValueExistsScriptCondition")]
    public class TableValueExistsScriptCondition : IScriptCondition
    {
        [MetaProperty("TableValue", BinPropertyType.Embedded)]
        public MetaEmbedded<ScriptTableGet> TableValue { get; set; }
    }
    [MetaClass("RandomChanceScriptCondition")]
    public class RandomChanceScriptCondition : IScriptCondition
    {
        [MetaProperty("Chance", BinPropertyType.Structure)]
        public IFloatGet Chance { get; set; }
    }
    [MetaClass("AndScriptCondition")]
    public class AndScriptCondition : IScriptCondition
    {
        [MetaProperty("Conditions", BinPropertyType.Container)]
        public MetaContainer<IScriptCondition> Conditions { get; set; }
    }
    [MetaClass("OrScriptCondition")]
    public class OrScriptCondition : IScriptCondition
    {
        [MetaProperty("Conditions", BinPropertyType.Container)]
        public MetaContainer<IScriptCondition> Conditions { get; set; }
    }
    [MetaClass("NotScriptCondition")]
    public class NotScriptCondition : IScriptCondition
    {
        [MetaProperty("Condition", BinPropertyType.Structure)]
        public IScriptCondition Condition { get; set; }
    }
    [MetaClass("ComparisonScriptCondition")]
    public class ComparisonScriptCondition : IScriptCondition
    {
        [MetaProperty("Value1", BinPropertyType.Structure)]
        public IScriptValueGet Value1 { get; set; }
        [MetaProperty("Operation", BinPropertyType.UInt32)]
        public uint? Operation { get; set; }
        [MetaProperty("Value2", BinPropertyType.Structure)]
        public IScriptValueGet Value2 { get; set; }
    }
    [MetaClass("IScriptSequence")]
    public interface IScriptSequence : RScript
    {
    }
    [MetaClass("RScript")]
    public interface RScript : IMetaClass
    {
    }
    [MetaClass("ScriptSequence")]
    public class ScriptSequence : IScriptSequence
    {
        [MetaProperty("blocks", BinPropertyType.Container)]
        public MetaContainer<IScriptBlock> Blocks { get; set; }
    }
    [MetaClass("ScriptTable")]
    public interface ScriptTable : IMetaClass
    {
    }
    [MetaClass("PassThroughParamsTable")]
    public class PassThroughParamsTable : ScriptTable
    {
    }
    [MetaClass("IBoolGet")]
    public interface IBoolGet : IScriptValueGet
    {
    }
    [MetaClass("BoolGet")]
    public class BoolGet : IBoolGet
    {
        [MetaProperty("value", BinPropertyType.Bool)]
        public bool? Value { get; set; }
    }
    [MetaClass("BoolTableGet")]
    public class BoolTableGet : IBoolGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<bool> Default { get; set; }
    }
    [MetaClass("CustomTableGet")]
    public class CustomTableGet : IScriptValueGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
    }
    [MetaClass("IFloatGet")]
    public interface IFloatGet : IScriptValueGet
    {
    }
    [MetaClass("FloatGet")]
    public class FloatGet : IFloatGet
    {
        [MetaProperty("value", BinPropertyType.Float)]
        public float? Value { get; set; }
    }
    [MetaClass("FloatTableGet")]
    public class FloatTableGet : IFloatGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<float> Default { get; set; }
    }
    [MetaClass("FloatOffsetTableGet")]
    public class FloatOffsetTableGet : IFloatGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
        [MetaProperty("offset", BinPropertyType.Float)]
        public float? Offset { get; set; }
    }
    [MetaClass("IFunctionGet")]
    public interface IFunctionGet : IScriptValueGet
    {
    }
    [MetaClass("FunctionTableGet")]
    public class FunctionTableGet : IFunctionGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
    }
    [MetaClass("IIntGet")]
    public interface IIntGet : IScriptValueGet
    {
    }
    [MetaClass("IntGet")]
    public class IntGet : IIntGet
    {
        [MetaProperty("value", BinPropertyType.Int32)]
        public int? Value { get; set; }
    }
    [MetaClass("IntTableGet")]
    public class IntTableGet : IIntGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<int> Default { get; set; }
    }
    [MetaClass("IntOffsetTableGet")]
    public class IntOffsetTableGet : IIntGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
        [MetaProperty("offset", BinPropertyType.Int32)]
        public int? Offset { get; set; }
    }
    [MetaClass("ScriptTableGet")]
    public class ScriptTableGet : IScriptValueGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
    }
    [MetaClass("ScriptTableSet")]
    public class ScriptTableSet : IMetaClass
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
    }
    [MetaClass("IntTableSet")]
    public class IntTableSet : ScriptTableSet
    {
    }
    [MetaClass("BoolTableSet")]
    public class BoolTableSet : ScriptTableSet
    {
    }
    [MetaClass("FloatTableSet")]
    public class FloatTableSet : ScriptTableSet
    {
    }
    [MetaClass("StringTableSet")]
    public class StringTableSet : ScriptTableSet
    {
    }
    [MetaClass("VectorTableSet")]
    public class VectorTableSet : ScriptTableSet
    {
    }
    [MetaClass("FunctionTableSet")]
    public class FunctionTableSet : ScriptTableSet
    {
    }
    [MetaClass("CustomTableSet")]
    public class CustomTableSet : ScriptTableSet
    {
    }
    [MetaClass("IScriptValueGet")]
    public interface IScriptValueGet : IMetaClass
    {
    }
    [MetaClass("IStringGet")]
    public interface IStringGet : IScriptValueGet
    {
    }
    [MetaClass("StringGet")]
    public class StringGet : IStringGet
    {
        [MetaProperty("value", BinPropertyType.String)]
        public string? Value { get; set; }
    }
    [MetaClass("StringTableGet")]
    public class StringTableGet : IStringGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<string> Default { get; set; }
    }
    [MetaClass("IVectorGet")]
    public interface IVectorGet : IScriptValueGet
    {
    }
    [MetaClass("VectorGet")]
    public class VectorGet : IVectorGet
    {
        [MetaProperty("value", BinPropertyType.Vector3)]
        public Vector3? Value { get; set; }
    }
    [MetaClass("VectorTableGet")]
    public class VectorTableGet : IVectorGet
    {
        [MetaProperty("Table", BinPropertyType.Structure)]
        public ScriptTable Table { get; set; }
        [MetaProperty("Var", BinPropertyType.Hash)]
        public MetaHash? Var { get; set; }
        [MetaProperty("Default", BinPropertyType.Optional)]
        public MetaOptional<Vector3> Default { get; set; }
    }
    [MetaClass("AnchorBase")]
    public interface AnchorBase : IMetaClass
    {
    }
    [MetaClass(1396238320)]
    public class Class1396238320 : AnchorBase
    {
        [MetaProperty("anchor", BinPropertyType.Vector2)]
        public Vector2? Anchor { get; set; }
    }
    [MetaClass(3742500809)]
    public class Class3742500809 : AnchorBase
    {
        [MetaProperty("anchorLeft", BinPropertyType.Vector2)]
        public Vector2? AnchorLeft { get; set; }
        [MetaProperty("anchorRight", BinPropertyType.Vector2)]
        public Vector2? AnchorRight { get; set; }
    }
    [MetaClass(2069111393)]
    public interface Class2069111393 : IMetaClass
    {
        [MetaProperty("mTextureName", BinPropertyType.String)]
        string? TextureName { get; set; }
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        uint? TextureSourceResolutionWidth { get; set; }
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        uint? TextureSourceResolutionHeight { get; set; }
    }
    [MetaClass("AtlasData")]
    public class AtlasData : Class2069111393
    {
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionWidth { get; set; }
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionHeight { get; set; }
        [MetaProperty("mTextureUV", BinPropertyType.Vector4)]
        public Vector4? TextureUV { get; set; }
    }
    [MetaClass(3050387163)]
    public class Class3050387163 : Class2069111393
    {
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionWidth { get; set; }
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionHeight { get; set; }
        [MetaProperty(3808350240, BinPropertyType.Vector4)]
        public Vector4? m3808350240 { get; set; }
        [MetaProperty(3942718287, BinPropertyType.Vector2)]
        public Vector2? m3942718287 { get; set; }
        [MetaProperty(458738727, BinPropertyType.Vector2)]
        public Vector2? m458738727 { get; set; }
    }
    [MetaClass(2748390021)]
    public class Class2748390021 : Class2069111393
    {
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionWidth { get; set; }
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionHeight { get; set; }
        [MetaProperty(3808350240, BinPropertyType.Vector2)]
        public Vector2? m3808350240 { get; set; }
        [MetaProperty(3942718287, BinPropertyType.Vector4)]
        public Vector4? m3942718287 { get; set; }
        [MetaProperty(367825281, BinPropertyType.Vector2)]
        public Vector2? m367825281 { get; set; }
    }
    [MetaClass(2872907111)]
    public class Class2872907111 : Class2069111393
    {
        [MetaProperty("mTextureName", BinPropertyType.String)]
        public string? TextureName { get; set; }
        [MetaProperty("mTextureSourceResolutionWidth", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionWidth { get; set; }
        [MetaProperty("mTextureSourceResolutionHeight", BinPropertyType.UInt32)]
        public uint? TextureSourceResolutionHeight { get; set; }
        [MetaProperty(3808350240, BinPropertyType.Vector4)]
        public Vector4? m3808350240 { get; set; }
        [MetaProperty(3942718287, BinPropertyType.Vector4)]
        public Vector4? m3942718287 { get; set; }
        [MetaProperty(367825281, BinPropertyType.Vector2)]
        public Vector2? m367825281 { get; set; }
        [MetaProperty(458738727, BinPropertyType.Vector2)]
        public Vector2? m458738727 { get; set; }
    }
    [MetaClass("BaseElementData")]
    public interface BaseElementData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        bool? m629911194 { get; set; }
    }
    [MetaClass("RegionElementData")]
    public class RegionElementData : BaseElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
    }
    [MetaClass("TextElementData")]
    public class TextElementData : BaseElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty(2975295581, BinPropertyType.String)]
        public string? m2975295581 { get; set; }
        [MetaProperty("mFontDescription", BinPropertyType.ObjectLink)]
        public MetaObjectLink? FontDescription { get; set; }
        [MetaProperty("mTextAlignmentHorizontal", BinPropertyType.Byte)]
        public byte? TextAlignmentHorizontal { get; set; }
        [MetaProperty("mTextAlignmentVertical", BinPropertyType.Byte)]
        public byte? TextAlignmentVertical { get; set; }
        [MetaProperty(2081063769, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2081063769 { get; set; }
        [MetaProperty(1778988038, BinPropertyType.Byte)]
        public byte? m1778988038 { get; set; }
        [MetaProperty(255926962, BinPropertyType.Float)]
        public float? m255926962 { get; set; }
    }
    [MetaClass("IconElementData")]
    public class IconElementData : BaseElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mColor", BinPropertyType.Color)]
        public Color? Color { get; set; }
        [MetaProperty("mUseAlpha", BinPropertyType.Bool)]
        public bool? UseAlpha { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public Class2069111393 Atlas { get; set; }
    }
    [MetaClass("ParticleSystemElementData")]
    public class ParticleSystemElementData : BaseElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mVfxSystem", BinPropertyType.ObjectLink)]
        public MetaObjectLink? VfxSystem { get; set; }
        [MetaProperty(3296523975, BinPropertyType.UInt32)]
        public uint? m3296523975 { get; set; }
        [MetaProperty(42091584, BinPropertyType.Bool)]
        public bool? m42091584 { get; set; }
        [MetaProperty(2494597354, BinPropertyType.Bool)]
        public bool? m2494597354 { get; set; }
    }
    [MetaClass("ScissorRegionElementData")]
    public class ScissorRegionElementData : BaseElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty(2899083445, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2899083445 { get; set; }
    }
    [MetaClass("EffectElementData")]
    public interface EffectElementData : BaseElementData
    {
    }
    [MetaClass("EffectCooldownElementData")]
    public class EffectCooldownElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mEffectColor0", BinPropertyType.Color)]
        public Color? EffectColor0 { get; set; }
        [MetaProperty("mEffectColor1", BinPropertyType.Color)]
        public Color? EffectColor1 { get; set; }
    }
    [MetaClass("EffectCircleMaskCooldownElementData")]
    public class EffectCircleMaskCooldownElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mEffectColor0", BinPropertyType.Color)]
        public Color? EffectColor0 { get; set; }
        [MetaProperty("mEffectColor1", BinPropertyType.Color)]
        public Color? EffectColor1 { get; set; }
    }
    [MetaClass("EffectCooldownRadialElementData")]
    public class EffectCooldownRadialElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mIsFill", BinPropertyType.Bool)]
        public bool? IsFill { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
    }
    [MetaClass("EffectArcFillElementData")]
    public class EffectArcFillElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
    }
    [MetaClass("EffectAmmoElementData")]
    public class EffectAmmoElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mEffectColor0", BinPropertyType.Color)]
        public Color? EffectColor0 { get; set; }
        [MetaProperty("mEffectColor1", BinPropertyType.Color)]
        public Color? EffectColor1 { get; set; }
    }
    [MetaClass("EffectGlowElementData")]
    public class EffectGlowElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
        [MetaProperty("CycleTime", BinPropertyType.Float)]
        public float? CycleTime { get; set; }
        [MetaProperty("BaseScale", BinPropertyType.Float)]
        public float? BaseScale { get; set; }
        [MetaProperty("CycleBasedScaleAddition", BinPropertyType.Float)]
        public float? CycleBasedScaleAddition { get; set; }
        [MetaProperty("MinimumAlpha", BinPropertyType.Float)]
        public float? MinimumAlpha { get; set; }
    }
    [MetaClass("EffectAnimationElementData")]
    public class EffectAnimationElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
        [MetaProperty("FramesPerSecond", BinPropertyType.Float)]
        public float? FramesPerSecond { get; set; }
        [MetaProperty("TotalNumberOfFrames", BinPropertyType.Float)]
        public float? TotalNumberOfFrames { get; set; }
        [MetaProperty("NumberOfFramesPerRowInAtlas", BinPropertyType.Float)]
        public float? NumberOfFramesPerRowInAtlas { get; set; }
        [MetaProperty("mLooping", BinPropertyType.Bool)]
        public bool? Looping { get; set; }
        [MetaProperty(3996911377, BinPropertyType.Byte)]
        public byte? m3996911377 { get; set; }
    }
    [MetaClass("EffectFillPercentageElementData")]
    public class EffectFillPercentageElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
    }
    [MetaClass("EffectDesaturateElementData")]
    public class EffectDesaturateElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
        [MetaProperty("MinimumSaturation", BinPropertyType.Float)]
        public float? MinimumSaturation { get; set; }
        [MetaProperty("MaximumSaturation", BinPropertyType.Float)]
        public float? MaximumSaturation { get; set; }
    }
    [MetaClass("EffectRotatingIconElementData")]
    public class EffectRotatingIconElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
    }
    [MetaClass("EffectGlowingRotatingIconElementData")]
    public class EffectGlowingRotatingIconElementData : EffectRotatingIconElementData
    {
        [MetaProperty("CycleTime", BinPropertyType.Float)]
        public float? CycleTime { get; set; }
        [MetaProperty(88872846, BinPropertyType.Float)]
        public float? m88872846 { get; set; }
    }
    [MetaClass("EffectAnimatedRotatingIconElementData")]
    public class EffectAnimatedRotatingIconElementData : EffectAnimationElementData
    {
    }
    [MetaClass("EffectLineElementData")]
    public class EffectLineElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
        [MetaProperty("mThickness", BinPropertyType.Float)]
        public float? Thickness { get; set; }
        [MetaProperty("mRightSlicePercentage", BinPropertyType.Float)]
        public float? RightSlicePercentage { get; set; }
    }
    [MetaClass("EffectInstancedElementData")]
    public class EffectInstancedElementData : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty("mFlipX", BinPropertyType.Bool)]
        public bool? FlipX { get; set; }
        [MetaProperty("mFlipY", BinPropertyType.Bool)]
        public bool? FlipY { get; set; }
        [MetaProperty("mPerPixelUvsX", BinPropertyType.Bool)]
        public bool? PerPixelUvsX { get; set; }
        [MetaProperty("mAtlas", BinPropertyType.Structure)]
        public AtlasData Atlas { get; set; }
        [MetaProperty("mAdditionalOffsets", BinPropertyType.Container)]
        public MetaContainer<Vector2> AdditionalOffsets { get; set; }
    }
    [MetaClass("EffectCircleMaskDesaturateElementData")]
    public class EffectCircleMaskDesaturateElementData : EffectDesaturateElementData
    {
    }
    [MetaClass(4143783062)]
    public class Class4143783062 : EffectElementData
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? Scene { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mDraggable", BinPropertyType.UInt32)]
        public uint? Draggable { get; set; }
        [MetaProperty(1502845044, BinPropertyType.Bool)]
        public bool? m1502845044 { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty(3822917598, BinPropertyType.Structure)]
        public AnchorBase m3822917598 { get; set; }
        [MetaProperty("mNoPixelSnappingX", BinPropertyType.Bool)]
        public bool? NoPixelSnappingX { get; set; }
        [MetaProperty("mNoPixelSnappingY", BinPropertyType.Bool)]
        public bool? NoPixelSnappingY { get; set; }
        [MetaProperty("mRect", BinPropertyType.Vector4)]
        public Vector4? Rect { get; set; }
        [MetaProperty("mUseRectSourceResolutionAsFloor", BinPropertyType.Bool)]
        public bool? UseRectSourceResolutionAsFloor { get; set; }
        [MetaProperty("mRectSourceResolutionWidth", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionWidth { get; set; }
        [MetaProperty("mRectSourceResolutionHeight", BinPropertyType.UInt16)]
        public ushort? RectSourceResolutionHeight { get; set; }
        [MetaProperty("mKeepMaxScale", BinPropertyType.Bool)]
        public bool? KeepMaxScale { get; set; }
        [MetaProperty("mFullscreen", BinPropertyType.Bool)]
        public bool? Fullscreen { get; set; }
        [MetaProperty("mHitTestPolygon", BinPropertyType.Container)]
        public MetaContainer<Vector2> HitTestPolygon { get; set; }
        [MetaProperty(629911194, BinPropertyType.Bool)]
        public bool? m629911194 { get; set; }
        [MetaProperty(2246894113, BinPropertyType.ObjectLink)]
        public MetaObjectLink? m2246894113 { get; set; }
    }
    [MetaClass("SceneData")]
    public class SceneData : IMetaClass
    {
        [MetaProperty("mName", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("mEnabled", BinPropertyType.Bool)]
        public bool? Enabled { get; set; }
        [MetaProperty("mLayer", BinPropertyType.UInt32)]
        public uint? Layer { get; set; }
        [MetaProperty("mHealthBar", BinPropertyType.Bool)]
        public bool? HealthBar { get; set; }
        [MetaProperty(574697843, BinPropertyType.Bool)]
        public bool? m574697843 { get; set; }
        [MetaProperty("mParentScene", BinPropertyType.ObjectLink)]
        public MetaObjectLink? ParentScene { get; set; }
        [MetaProperty(4160985070, BinPropertyType.Bool)]
        public bool? m4160985070 { get; set; }
    }
    [MetaClass("X3DSharedConstantDef")]
    public class X3DSharedConstantDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("Count", BinPropertyType.UInt32)]
        public uint? Count { get; set; }
        [MetaProperty("register", BinPropertyType.Int32)]
        public int? Register { get; set; }
        [MetaProperty(1757672278, BinPropertyType.Bool)]
        public bool? m1757672278 { get; set; }
        [MetaProperty(3548357530, BinPropertyType.UInt32)]
        public uint? m3548357530 { get; set; }
    }
    [MetaClass("X3DSharedConstantBufferDef")]
    public class X3DSharedConstantBufferDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("frequency", BinPropertyType.UInt32)]
        public uint? Frequency { get; set; }
        [MetaProperty("constants", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<X3DSharedConstantDef>> Constants { get; set; }
        [MetaProperty(2825931196, BinPropertyType.Bool)]
        public bool? m2825931196 { get; set; }
        [MetaProperty("register", BinPropertyType.Int32)]
        public int? Register { get; set; }
        [MetaProperty(3548357530, BinPropertyType.UInt32)]
        public uint? m3548357530 { get; set; }
    }
    [MetaClass("X3DSharedData")]
    public class X3DSharedData : IMetaClass
    {
        [MetaProperty(3851804024, BinPropertyType.Container)]
        public MetaContainer<MetaObjectLink> m3851804024 { get; set; }
        [MetaProperty("textures", BinPropertyType.Container)]
        public MetaContainer<MetaEmbedded<X3DSharedTextureDef>> Textures { get; set; }
    }
    [MetaClass("X3DSharedTextureDef")]
    public class X3DSharedTextureDef : IMetaClass
    {
        [MetaProperty("name", BinPropertyType.String)]
        public string? Name { get; set; }
        [MetaProperty("type", BinPropertyType.UInt32)]
        public uint? Type { get; set; }
        [MetaProperty("register", BinPropertyType.Int32)]
        public int? Register { get; set; }
        [MetaProperty(3548357530, BinPropertyType.UInt32)]
        public uint? m3548357530 { get; set; }
    }
}

