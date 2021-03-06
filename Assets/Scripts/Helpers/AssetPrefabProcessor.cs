﻿#if UNITY_EDITOR
namespace Helpers {

    using System;
    using System.IO;
    using System.Collections.Generic;

    using UnityEngine;


    using GameInfo;

    public class AssetProcessor {

        public static Type InstantiatePrefab<Type>(string objectLocation, string objectName) where Type : UnityEngine.Object {
            string prefabPath = GetPrefabPath(objectLocation, objectName);
            Type prefabObject = LoadAsset<Type>(prefabPath);

            return UnityEditor.PrefabUtility.InstantiatePrefab(prefabObject) as Type;
        }

        /// <summary>
        /// This Function Will Find And Return A Type Of An Asset In Our Project In A Specified Place
        /// </summary>
        /// <param name="objectLocation">The Location Of The Asset</param>
        /// <param name="objectName">The Name Of The Asset</param>
        /// <returns></returns>
        public static Type FindAsset<Type>(string objectLocation, string objectName) where Type : UnityEngine.Object {
            
            string prefabPath = GetPath(objectLocation, objectName);

            if(typeof(Type) == typeof(GameObject))
                prefabPath += ".prefab";
            else if(typeof(Type) == typeof(Material))
                prefabPath += ".mat";
            else
                throw new ArgumentNullException("Object Of Type(" + typeof(Type).Name + ") Not Found");

            var prefabObject = LoadAsset<Type>(prefabPath);

            if(prefabObject == null)
                throw new ArgumentNullException("Object (" + prefabPath + ") Not Found");

            return prefabObject as Type;
        }

        /// <summary>
        /// This Function Will Find And Return A Object Type To Us.
        /// </summary>
        /// <typeparam name="Type">The Type Of Object</typeparam>
        /// <param name="path">The Full Path Of Where The Object Is Located</param>
        /// <returns></returns>
        private static Type LoadAsset<Type>(string path) where Type : UnityEngine.Object {

            var temp = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Type)) as Type;
            
            if(temp == null)
                throw new ArgumentNullException("Object at " + path + " Not Found");

            return temp;
        }

        /// <summary>
        /// This Function Will Return To Us A Combined String Of The Full Path And Name Of The Object We Are Looking For.
        /// </summary>
        /// <param name="objectLocation">The Location Of The Asset</param>
        /// <param name="objectName">The Name Of The Asset</param>
        /// <returns></returns>
        private static string GetPrefabPath(string objectLocation, string objectName) {
            string name = Path.GetFileNameWithoutExtension(objectName);
            return objectLocation + name + ".prefab";
        }

        private static string GetPath(string objectLocation, string objectName) {
            string name = Path.GetFileNameWithoutExtension(objectName);
            return objectLocation + name;
        }
    }
}
#endif