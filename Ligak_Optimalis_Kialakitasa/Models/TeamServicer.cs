using System.Collections.Generic;

namespace Ligak_Optimalis_Kialakitasa.Models
{
    public class TeamServicer
    {
        public TeamServicer()
        {
            this.RemovedTeams = new Stack<string>();
            this.Teams = new List<string>();
        }

        public Stack<string> RemovedTeams  { get; set; }
        public List<string> Teams { get; set; }
    }
}
