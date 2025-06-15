using eccFramework.SharedLib.Core.Base;
using FTSolutions.IEC61034.Common.DataType;

namespace FTSolutions.IEC61034.Common.Result
{
    public class DataSetQualification : BaseResult
    {
        public DataSetQualification()
        {
            this.RegistrationInfo = new TypeQualificationRegistration();
            this.Info_QualificationProperties = new QualificationProperty();

            this.Info_Qualification = new Qualification();
            this.Info_QualificationSummary = new QualificationSummary();
        }



        //###################################################################
        //  Property
        //###################################################################

        private TypeQualificationRegistration _registrationInfo;
        public TypeQualificationRegistration RegistrationInfo
        {
            get { return _registrationInfo; }
            set
            {
                _registrationInfo = value;
                this.RaisePropertyChanged(nameof(RegistrationInfo));
            }
        }

        public QualificationProperty Info_QualificationProperties { get; set; }

        public Qualification Info_Qualification { get; set; }

        public QualificationSummary Info_QualificationSummary { get; set; }



        //###################################################################
        //  Override
        //###################################################################

        public override void ClearAll()
        {
            this.RegistrationInfo.Clear();
            this.Info_QualificationProperties.Clear();

            this.Info_Qualification.Clear();
            this.Info_QualificationSummary.Clear();
        }



        //###################################################################
        //  Public
        //###################################################################

        public void ContinueTest()
        {
            this.Info_Qualification.Clear();
            this.Info_QualificationSummary.Clear();
        }
    }
}
