using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.QueryService;
using FTSolutions.IEC61034.Common.Setting;
using System.Data;
using System.Windows.Input;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_ConfigSetting : BaseIEC61034ViewModel
    {
        public vmPopup_ConfigSetting()
        {
            CloseVisibility = System.Windows.Visibility.Visible;

            SaveCommand = new DelegateCommand((o) => ExecuteSaveCommand(o));
        }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand SaveCommand { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        


        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override bool ExecuteCloseClick(object obj)
        {
            this.CloseWindow();

            return true;
        }



        //###################################################################
        //  EventHandler
        //###################################################################

        public void ExecuteSaveCommand(object obj)
        {
            this.UpdateSystemConfig();

            SettingManager.ReadConfigDatabase();
        }


        //###################################################################
        //  Private
        //###################################################################

        private void UpdateSystemConfig()
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_save", "msg_save_data");

            if (result)
            {
                SQLiteParamInfoBatch paramBatchInfo = new SQLiteParamInfoBatch();

                foreach (var item in SESSION_MANAGER.ConfigSetting)
                {
                    SQLiteParamInfoCollection paramInfoCollection = new SQLiteParamInfoCollection();
                    paramInfoCollection.Add(new SQLiteParamInfo("key", DbType.String, item.KEY));
                    paramInfoCollection.Add(new SQLiteParamInfo("value", DbType.String, item.VALUE));
                    paramInfoCollection.Add(new SQLiteParamInfo("desc", DbType.String, item.DESC));

                    paramBatchInfo.Add(paramInfoCollection);
                }

                ManagerQueryService.Current.QueryServiceSystem.UpdateConfigSetting(paramBatchInfo);

                this.SESSION_MANAGER.BindingLogoImage();
            }
        }
    }
}
