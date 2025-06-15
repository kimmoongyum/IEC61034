using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.DataType;

namespace FTSolutions.IEC61034.Common.Result
{
    public class DataSetTest : BaseResult
    {
        public DataSetTest()
        {
            this.RegistrationInfo = new TypeRegistration();
            this.Info_TestProperties = new TestProperty();

            this.Info_Test = new Test();
            this.Info_TestSummary = new TestSummary();
        }



        //###################################################################
        //  Property
        //###################################################################

        private TypeRegistration _registrationInfo;
        public TypeRegistration RegistrationInfo
        {
            get { return _registrationInfo; }
            set
            {
                _registrationInfo = value;
                this.RaisePropertyChanged(nameof(RegistrationInfo));
            }
        }

        public TestProperty Info_TestProperties { get; set; }

        public Test Info_Test { get; set; }

        public TestSummary Info_TestSummary { get; set; }



        //###################################################################
        //  Override
        //###################################################################

        public override void ClearAll()
        {
            this.RegistrationInfo.Clear();
            this.Info_TestProperties.Clear();

            this.Info_Test.Clear();
            this.Info_TestSummary.Clear();
        }



        //###################################################################
        //  Public
        //###################################################################

        public void ContinueTest()
        {
            this.Info_Test.Clear();
            this.Info_TestSummary.Clear();
        }
    }
}
