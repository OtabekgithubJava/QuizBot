using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizbot
{
    public class QuizButton
    {
     
        public string Token { get; set; }
        public QuizButton(string token)
        {
            this.Token = token;
        }
    }
}