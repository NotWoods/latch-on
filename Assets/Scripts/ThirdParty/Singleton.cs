﻿using UnityEngine;

/// Generic Singleton class
/// http://www.gamasutra.com/blogs/HermanTulleken/20160812/279100/
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
   protected static T instance;

   //Returns the instance of this singleton.
   public static T Instance {
      get {
         if (instance == null) {
            instance = (T) FindObjectOfType(typeof (T));

            if (instance == null) Debug.LogError(
							"An instance of " +
							typeof (T) +
							" is needed in the scene, but there is none."
						);
         }

         return instance;
      }
   }
}