using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder
{
    internal class MarkovColumn
    {
        public string[] columnStr;
        public List<MarkovColumn> transitionList;

        //Constructor
        public MarkovColumn(string[] columnStr)
        {
            this.columnStr = columnStr;
            transitionList = new List<MarkovColumn>();
        }

        //Metodos
        public void AddSuccesor(MarkovColumn newTransition)
        {
            transitionList.Add(newTransition);
        }
    }
}
