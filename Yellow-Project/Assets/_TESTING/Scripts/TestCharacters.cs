using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;

namespace TESTING {
    public class TestCharacters : MonoBehaviour {

        public TMP_FontAsset tempFont;

        // Start is called before the first frame update
        void Start() {

            // Character Charlene = CharacterManager.instance.CreateCharacter("Charlene");
            /*Character Barrios2 = CharacterManager.instance.CreateCharacter("Barrios");
            Character Joes = CharacterManager.instance.CreateCharacter("Joes");*/
            StartCoroutine(Test());

        }

        IEnumerator Test() {

            Character John = CharacterManager.instance.CreateCharacter("John");
            Character Barrios = CharacterManager.instance.CreateCharacter("Barrios");
            Character Charlene = CharacterManager.instance.CreateCharacter("Charlene");

            List<string> lines = new List<string>() {
                "Has the dawn ever seen your eyes ?",
                "Have the days made you so unwise ?",
                "Realize, {wa 1} you are"
            };

            yield return Barrios.Say(lines);

            Barrios.SetNameColor(Color.red);
            Barrios.SetDialogueColor(Color.green);
            Barrios.SetNameFont(tempFont);
            Barrios.SetDialogueFont(tempFont);

            yield return Barrios.Say(lines);

            Barrios.ResetConfigurationData();

            yield return Barrios.Say(lines);

            lines = new List<string>() {
                "In time you'll see the sign",
                "And {wc 1} realize your sin"
            };

            yield return Charlene.Say(lines);

            yield return John.Say("The minister of hate had just arrived too late to be spared {wa 1} Who cared ?");

            Debug.Log("Finished");

        }

        // Update is called once per frame
        void Update() {

        }
    }
}