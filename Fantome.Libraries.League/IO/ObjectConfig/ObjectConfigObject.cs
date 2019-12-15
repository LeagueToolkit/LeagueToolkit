using Fantome.Libraries.League.Helpers;
using Fantome.Libraries.League.Helpers.Attributes;
using Fantome.Libraries.League.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Fantome.Libraries.League.IO.ObjectConfig
{
    /// <summary>
    /// Represents an Object inside an <see cref="ObjectConfigFile"/>
    /// </summary>
    public class ObjectConfigObject
    {
        [IniProperty("skinID", typeof(uint))] public uint SkinID { get; set; }
        [IniProperty("Move", typeof(Vector3))] public Vector3 Translation { get; set; }
        [IniProperty("Rot", typeof(Vector3))] public Vector3 Rotation { get; set; }
        [IniProperty("mMaxHP", typeof(float))] public float MaxHP { get; set; }
        [IniProperty("mBaseStaticHPRegen", typeof(uint))] public uint BaseStaticHPRegen { get; set; }
        [IniProperty("mArmor", typeof(float))] public float Armor { get; set; }
        [IniProperty("mMaxMP", typeof(float))] public float MaxMP { get; set; }
        [IniProperty("SelectionHeight", typeof(float))] public float SelectionHeight { get; set; }
        [IniProperty("SelectionRadius", typeof(float))] public float SelectionRadius { get; set; }
        [IniProperty("Selection_T1Radius", typeof(float))] public float Selection_T1Radius { get; set; }
        [IniProperty("PerceptionBubbleRadius", typeof(float))] public float PerceptionBubbleRadius { get; set; }
        [IniProperty("ArmorMaterial", typeof(string))] public string ArmorMaterial { get; set; }
        [IniProperty("SkinName1", typeof(string))] public string SkinName1 { get; set; }
        [IniProperty("SkinName2", typeof(string))] public string SkinName2 { get; set; }
        [IniProperty("Collision Radius", typeof(float))] public float CollisionRadius { get; set; }
        [IniProperty("Collision Height", typeof(float))] public float CollisionHeight { get; set; }
        [IniProperty("PathfindingCollisionRadius", typeof(float))] public float PathfindingCollisionRadius { get; set; }
        [IniProperty("VOSkinName", typeof(string))] public string VOSkinName { get; set; }
        [IniProperty("SwapModelOnDeathTime", typeof(bool))] public bool SwapModelOnDeathTime { get; set; }
        [IniProperty("DestroyedTowerSkin", typeof(string))] public string DestroyedTowerSkin { get; set; }
        [IniProperty("DestroyedTowerSkinID", typeof(uint))] public uint DestroyedTowerSkinID { get; set; }
        [IniProperty("WarningDecalOffset", typeof(Vector3))] public Vector3 WarningDecalOffset { get; set; }
        [IniProperty("WarningDecalSizeFactor ", typeof(float))] public float WarningDecalSizeFactor { get; set; }
        private Dictionary<string, bool> _setMap = new Dictionary<string, bool>();

        /// <summary>
        /// Initializes an empty <see cref="ObjectConfigObject"/>
        /// </summary>
        public ObjectConfigObject() { }

        /// <summary>
        /// Initializes a new <see cref="ObjectConfigObject"/>
        /// </summary>
        public ObjectConfigObject(uint skinID, Vector3 translation, Vector3 rotation, float maxHP, uint baseStaticHPRegen, float armor, float maxMP,
            float selectionHeight, float selectionRadius, float selection_t1Radius, float perceptionBubbleRadius, string armorMaterial,
            string skinName1, string skinName2, float collisionRadius, float collisionHeight, float pathfindingCollisionRadius, string voSkinName,
            bool swapModelOnDeathTime, string destroyedTowerSkin, uint destroyedTowerSkinID, Vector3 warningDecalOffset, float warningDecalSizeFactor)
        {
            this.SkinID = skinID;
            this.Translation = translation;
            this.Rotation = rotation;
            this.MaxHP = maxHP;
            this.BaseStaticHPRegen = baseStaticHPRegen;
            this.Armor = armor;
            this.MaxMP = maxMP;
            this.SelectionHeight = selectionHeight;
            this.SelectionRadius = selectionRadius;
            this.Selection_T1Radius = selection_t1Radius;
            this.PerceptionBubbleRadius = perceptionBubbleRadius;
            this.ArmorMaterial = armorMaterial;
            this.SkinName1 = skinName1;
            this.SkinName2 = skinName2;
            this.CollisionRadius = collisionRadius;
            this.CollisionHeight = collisionHeight;
            this.PathfindingCollisionRadius = pathfindingCollisionRadius;
            this.VOSkinName = voSkinName;
            this.SwapModelOnDeathTime = swapModelOnDeathTime;
            this.DestroyedTowerSkin = destroyedTowerSkin;
            this.DestroyedTowerSkinID = DestroyedTowerSkinID;
            this.WarningDecalOffset = warningDecalOffset;
            this.WarningDecalSizeFactor = warningDecalSizeFactor;

            foreach (PropertyInfo property in typeof(ObjectConfigObject).GetProperties())
            {
                IniPropertyAttribute attribute = property.GetCustomAttribute<IniPropertyAttribute>();
                this._setMap.Add(attribute.Name, true);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="ObjectConfigObject"/>
        /// </summary>
        /// <param name="objectDefinition">This <see cref="ObjectConfigObject"/>'s INI definition</param>
        public ObjectConfigObject(Dictionary<string, string> objectDefinition)
        {
            Type configObject = typeof(ObjectConfigObject);

            foreach (PropertyInfo property in configObject.GetProperties())
            {
                IniPropertyAttribute attribute = property.GetCustomAttribute<IniPropertyAttribute>();
                bool isSet = objectDefinition.ContainsKey(attribute.Name);
                this._setMap.Add(attribute.Name, isSet);

                if (isSet)
                {
                    if (attribute.SerializationType == typeof(uint))
                    {
                        property.SetValue(this, uint.Parse(objectDefinition[attribute.Name]), null);
                    }
                    else if (attribute.SerializationType == typeof(float))
                    {
                        property.SetValue(this, float.Parse(objectDefinition[attribute.Name], CultureInfo.InvariantCulture));
                    }
                    else if (attribute.SerializationType == typeof(Vector2))
                    {
                        property.SetValue(this, TextStructureProcessor.ParseVector2(objectDefinition[attribute.Name]));
                    }
                    else if (attribute.SerializationType == typeof(Vector3))
                    {
                        property.SetValue(this, TextStructureProcessor.ParseVector3(objectDefinition[attribute.Name]));
                    }
                    else if (attribute.SerializationType == typeof(string))
                    {
                        property.SetValue(this, objectDefinition[attribute.Name]);
                    }
                }
            }
        }

        /// <summary>
        /// Converts this <see cref="ObjectConfigObject"/> into an INI section (property list)
        /// </summary>
        public Dictionary<string, string> ConvertToSection()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            Type configObject = typeof(ObjectConfigObject);

            foreach (PropertyInfo property in configObject.GetProperties())
            {
                IniPropertyAttribute attribute = property.GetCustomAttribute<IniPropertyAttribute>();
                if (this._setMap[attribute.Name])
                {
                    if (attribute.SerializationType == typeof(uint))
                    {
                        uint value = (uint)property.GetValue(this);
                        properties.Add(attribute.Name, value.ToString());
                    }
                    else if (attribute.SerializationType == typeof(float))
                    {
                        float value = (float)property.GetValue(this);
                        properties.Add(attribute.Name, value.ToString("0.0000", CultureInfo.InvariantCulture));
                    }
                    else if (attribute.SerializationType == typeof(Vector2))
                    {
                        Vector2 value = (Vector2)property.GetValue(this);
                        properties.Add(attribute.Name, TextStructureProcessor.ConvertVector2(value, "0.0000"));
                    }
                    else if (attribute.SerializationType == typeof(Vector3))
                    {
                        Vector3 value = (Vector3)property.GetValue(this);
                        properties.Add(attribute.Name, TextStructureProcessor.ConvertVector3(value, "0.0000"));
                    }
                    else if (attribute.SerializationType == typeof(string))
                    {
                        properties.Add(attribute.Name, (string)property.GetValue(this));
                    }
                }
            }

            return properties;
        }
    }
}
