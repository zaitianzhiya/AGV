using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS_RGB
{
    public class NoteTouch
    {
        public NoteTouch()
        { }
        /// <summary>
        /// 命令功能
        /// </summary>
        private string Type;
        public string type
        {
            get { return Type; }
            set { Type = value; }
        }

        /// <summary>
        /// From
        /// </summary>
        private string From;

        public string from
        {
            get { return From; }
            set { From = value; }
        }
        /// <summary>
        /// To
        /// </summary>
        private string To;
        public string to
        {
            get { return To; }
            set { To = value; }
        }

        /// <summary>
        /// 接收命令时间
        /// </summary>
        private string SetTime;
        public string setTime
        {
            get { return SetTime; }
            set { SetTime = value; }
        }
        /// <summary>
        /// 答复时间
        /// </summary>
        private string Retime;
        public string retime
        {
            get { return Retime; }
            set { Retime = value; }
               
        }

        /// <summary>
        /// CarNumber
        /// </summary>
        private string CarNumber;
        public string carNumber
        {
            get { return CarNumber; }
            set { CarNumber = value; }

        }
       
    }
}


