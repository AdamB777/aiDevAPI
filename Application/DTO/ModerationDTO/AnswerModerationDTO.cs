using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ModerationDTO
{
    public class AnswerModerationDTO
    {
        public string Token { get; set; }
        public List<int> Answer { get; set; }
    }
}
