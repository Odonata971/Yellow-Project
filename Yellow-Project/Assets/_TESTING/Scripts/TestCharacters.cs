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

            Character_Sprite Raelin = CreateCharacter("Raelin") as Character_Sprite;
            // Character_Sprite Guard = CreateCharacter("Guard as Generic") as Character_Sprite;
            // Character_Sprite GuardRed = CreateCharacter("Guard Red as Generic") as Character_Sprite;
            Character_Sprite Student = CreateCharacter("Female Student 2") as Character_Sprite;

            Raelin.SetPosition(new Vector2(0, 0));
            Student.SetPosition(new Vector2(1, 0));

            yield return new WaitForSeconds(1);

            Student.TransitionSprite(Student.GetSprite("female student 2 - sad"));
            Student.Animate("Hop");
            yield return Student.Say("Where did this wind chill come from?");

            Raelin.FaceRight();
            Raelin.TransitionSprite(Raelin.GetSprite("A2"));
            Raelin.TransitionSprite(Raelin.GetSprite("A_Stern"), layer: 1);
            Raelin.MoveToPosition(new Vector2(0.1f, 0));
            Raelin.Animate("Shiver", true);
            yield return Raelin.Say("I don't know - but I hate it!{a} It's freezing!");

            Student.TransitionSprite(Student.GetSprite("female student 2 - happy"));
            yield return Student.Say("Oh, it's over!");

            Raelin.TransitionSprite(Raelin.GetSprite("A2"));
            Raelin.TransitionSprite(Raelin.GetSprite("A_Stern"), layer: 1);
            Raelin.Animate("Shiver", false);
            yield return Raelin.Say("Thank the Lord...{a} I'm not wearing enough clothes for that crap.");

            yield return null;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}