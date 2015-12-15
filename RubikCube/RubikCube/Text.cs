using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubikCube
{
    class Text
    {
        public string MainTitle;
        public string OptionsTitle;
        public string FreePlayTitle;
        public string TutorialTitle;
        public string OptionsFreeText;
        public string TutorialFreeText;
        public string TutorialFreeText2;
        public string FreePlayScramble;
        public string FreePlayReset;

        public Text()
        {
            MainTitle = "Rubik's Cube -  Main Menu";
            FreePlayTitle = "Rubik's Cube -  Free Play";
            OptionsTitle = "Rubik's Cube -  Options";
            TutorialTitle = "Rubik's Cube -  Tutorial";
            OptionsFreeText = "Press the right key to change songs";
            TutorialFreeText = "Use the L,R,U,D,F,B keys to turn the cube";
            TutorialFreeText2 = "Hold Shift to turn the cube counter-clockwise";
            FreePlayScramble = "Scramble";
            FreePlayReset = "Reset!";
        }

        public void English()
        {
            MainTitle = "Rubik's Cube -  Main Menu";
            FreePlayTitle = "Rubik's Cube -  Free Play";
            OptionsTitle = "Rubik's Cube -  Settings";
            TutorialTitle = "Rubik's Cube -  Tutorial";
            OptionsFreeText = "Press the right key to change songs";
            TutorialFreeText = "Use the L,R,U,D,F,B keys to turn the cube";
            TutorialFreeText2 = "Hold Shift to turn the cube counter-clockwise";
            FreePlayScramble = "Scramble";
            FreePlayReset = "Reset!";
        }

        public void Hebrew()
        {
            MainTitle = "י ש א ר  ט י ר פ ת - ת י ר ג נ ו ה  ה י ב ו ק";
            FreePlayTitle = "י ש פ ו ח  ק ח ש מ - ת י ר ג נ ו ה  ה י ב ו ק";
            OptionsTitle = "ת ו ר ד ג ה - ת י ר ג נ ו ה  ה י ב ו ק";
            TutorialTitle = "ה כ ר ד ה - ת י ר ג נ ו ה  ה י ב ו ק";
            OptionsFreeText = "ר י ש  ר י ב ע ה ל  י ד כ  י נ מ י ה  ץ ח ה  ר ו ת פ כ  ל ע  ץ ח ל";
            TutorialFreeText = "ה י ב ו ק ה  ת א  ז י ז ה ל  י ד כ  L,R,U,D,F,B  ם י ש ק מ ה  ל ע  ץ ח ל";
            TutorialFreeText2 = "ן ו ע ש ה  ן ו ו כ  ד ג נ  ה י ב ו ק ה  ת א  ב ב ו ס ל  י ד כ  Shift ת א  ק ז ח ה";
            FreePlayScramble = "ב ב ר ע";
            FreePlayReset = "! ס פ א";
        }

        public void Russian() //not 100% done yet
        {
            MainTitle = "Кубик Рубика -  Главное Mеню";
            FreePlayTitle = "Кубик Рубика -  Свободная Игра";
            OptionsTitle = "Кубик Рубика -  Настройки";
            TutorialTitle = "Кубик Рубика -  Учебник";
            OptionsFreeText = "Нажимайте правую клавишу чтобы изменить песни";
            TutorialFreeText = "Использовать L,R,U,D,F,B клавиши чтобы повернуть кубика";
            TutorialFreeText2 = "Держать Shift чтобы повернуть кубика против часовой стрелки";
            FreePlayScramble = "Мишать";
            FreePlayReset = "Очистить!";
        }
    }
}
