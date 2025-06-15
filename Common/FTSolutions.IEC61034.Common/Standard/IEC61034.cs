using System;
using System.Threading;
using System.Windows.Threading;

namespace FTSolutions.IEC61034.Common.Standard
{
    public class IEC61034 : baseStandard
    {
        public IEC61034() : base()
        {
        }

        public IEC61034(string type) : base(type)
        {
        }


        //###################################################################
        //  Override
        //###################################################################

        public override void Start()
        {
            base.Start();
        }        

        public override void Stop()
        {
            base.Stop();
        }
    }
}
