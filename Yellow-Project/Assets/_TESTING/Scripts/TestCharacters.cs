using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;

namespace TESTING {
    public class TestCharacters : MonoBehaviour {

        public TMP_FontAsset tempFont;

        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        // Start is called before the first frame update
        void Start() {

            // Character Raelin = CharacterManager.instance.CreateCharacter("Raelin");
            StartCoroutine(Test());

        }

        IEnumerator Test() {

            // Character_Sprite Guard = CreateCharacter("Guard as Generic") as Character_Sprite;
            Character_Sprite Raelin = CreateCharacter("Raelin") as Character_Sprite;
            // Character_Sprite Student = CreateCharacter("Female Student 2") as Character_Sprite;
            
            yield return new WaitForSeconds(1);

            yield return Raelin.TransitionColor(Color.red, speed: 0.3f);
            yield return Raelin.TransitionColor(Color.blue);
            yield return Raelin.TransitionColor(Color.yellow);
            yield return Raelin.TransitionColor(Color.white, speed: 0.3f);

            yield return null;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}