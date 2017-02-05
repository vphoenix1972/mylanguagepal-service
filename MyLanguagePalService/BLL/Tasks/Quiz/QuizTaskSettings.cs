﻿namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public class QuizTaskSettings
    {
        public int LanguageId { get; set; }

        public int CountOfWordsUsed { get; set; }

        public bool ReshuffleWords { get; set; } = true;
    }
}