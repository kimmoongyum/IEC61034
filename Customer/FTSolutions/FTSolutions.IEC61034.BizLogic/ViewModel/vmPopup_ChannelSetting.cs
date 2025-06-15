using eccFramework.SharedLib.Core.Base;
using eccFramework.SharedLib.Core.Command;
using eccFramework.SharedLib.Core.Helper;
using eccFramework.SharedLib.GlobalType.Protocol;
using FTSolutions.IEC61034.Common.Base;
using FTSolutions.IEC61034.Common.DataType;
using FTSolutions.IEC61034.Common.QueryService;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace FTSolutions.IEC61034.BizLogic.ViewModel
{
    public class vmPopup_ChannelSetting : BaseIEC61034ViewModel
    {
        public vmPopup_ChannelSetting()
        {
            this.CalibrationResult = new CalibrationData();

            CloseVisibility = System.Windows.Visibility.Visible;

            SaveCommand = new DelegateCommand((o) => ExecuteSaveCommand(o));
            CopyToClipboardCommand = new DelegateCommand((o) => ExecuteCopyToClipboardCommand(o));
            LoadFileCommand = new DelegateCommand((o) => ExecuteLoadFileCommand(o));
        }



        //###################################################################
        //  Command
        //###################################################################

        public ICommand SaveCommand { get; private set; }

        public ICommand CopyToClipboardCommand { get; private set; }
        public ICommand LoadFileCommand { get; private set; }



        //###################################################################
        //  Property
        //###################################################################

        private CalibrationData _calibrationResult;
        public CalibrationData CalibrationResult
        {
            get { return _calibrationResult; }
            set
            {
                if (this._calibrationResult != value)
                {
                    this._calibrationResult = value;
                    this.RaisePropertyChanged(nameof(CalibrationResult));
                }
            }
        }



        //###################################################################
        //  Override
        //###################################################################

        public override void VMLoaded()
        {
            this.BindingSystemChannel();
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

        public void ExecuteNewCommand(string channelID)
        {
            if (!SESSION_MANAGER.ChannelSetting.AddNew(channelID))
            {
                this.ShowMessageKey(MessageButtonType.OK, "msg_error", "msg_invalid_item");
            }
        }

        public void ExecuteSaveCommand(object obj)
        {
            bool result = this.ShowMessageKey(MessageButtonType.YesNo, "msg_title_save", "msg_save_data");

            if (result)
            {
                this.UpdateSystemChannel();

                this.BindingSystemChannel();
            }
        }

        public void ExecuteCopyToClipboardCommand(object obj)
        {
            StringBuilder sbResult = new StringBuilder();

            foreach (var item in SESSION_MANAGER.ChannelSetting)
            {
                sbResult.AppendLine(string.Join(",", item.Channel, item.Address, item.Description, item.MinVoltage, item.MaxVoltage, item.GradientVoltage, item.InterceptVoltage, item.MinValue, item.MaxValue, item.GradientValue, item.InterceptValue));
            }

            Clipboard.SetText(sbResult.ToString());
        }

        public void ExecuteLoadFileCommand(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TypeChannelSettingCollection result = new TypeChannelSettingCollection();

                using (StreamReader file = new StreamReader(openFileDialog.FileName))
                {
                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] infos = line.Split(new char[] { ',' });

                        if (infos.Length == 11)
                        {
                            double minVoltage = Convert.ToDouble(infos[2]);
                            double maxVoltage = Convert.ToDouble(infos[3]);
                            double gradientVoltage = Convert.ToDouble(infos[4]);
                            double interceptVoltage = Convert.ToDouble(infos[5]);
                            double minValue = Convert.ToDouble(infos[6]);
                            double maxValue = Convert.ToDouble(infos[7]);
                            double gradientValue = Convert.ToDouble(infos[8]);
                            double interceptValue = Convert.ToDouble(infos[9]);

                            result.Add(new ChannelInfo(infos[0], infos[1], infos[10], minVoltage, maxVoltage, gradientVoltage, interceptVoltage, minValue, maxValue, gradientValue, interceptValue));
                        }
                    }

                    file.Close();
                }

                SESSION_MANAGER.BindingChannelSetting(result);
            }
        }



        //###################################################################
        //  Private
        //###################################################################

        private void BindingSystemChannel()
        {
            DataTable dtResult = ManagerQueryService.Current.QueryServiceSystem.SearchChannelSetting();

            TypeChannelSettingCollection result = new TypeChannelSettingCollection();

            if (dtResult != null)
            {
                foreach (DataRow row in dtResult.Rows)
                {
                    string channel = row["CHANNEL"].ToString();
                    string address = row["ADDRESS"].ToString();
                    string desc = row["DESC"].ToString();

                    double minVoltage = Convert.ToDouble(row["MIN_VOLTAGE"]);
                    double maxVoltage = Convert.ToDouble(row["MAX_VOLTAGE"]);
                    double gradientVoltage = Convert.ToDouble(row["GRADIENT_VOLTAGE"]);
                    double interceptVoltage = Convert.ToDouble(row["INTERCEPT_VOLTAGE"]);
                    double minValue = Convert.ToDouble(row["MIN_VALUE"]);
                    double maxValue = Convert.ToDouble(row["MAX_VALUE"]);
                    double gradientValue = Convert.ToDouble(row["GRADIENT_VALUE"]);
                    double interceptValue = Convert.ToDouble(row["INTERCEPT_VALUE"]);

                    result.Add(new ChannelInfo(channel, address, desc, minVoltage, maxVoltage, gradientVoltage, interceptVoltage, minValue, maxValue, gradientValue, interceptValue));
                }
            }
            
            SESSION_MANAGER.BindingChannelSetting(result);
        }

        private void UpdateSystemChannel()
        {
            SQLiteParamInfoBatch paramBatchInfo = new SQLiteParamInfoBatch();

            foreach (var item in SESSION_MANAGER.ChannelSetting)
            {
                SQLiteParamInfoCollection paramInfoCollection = new SQLiteParamInfoCollection();
                paramInfoCollection.Add(new SQLiteParamInfo("channel", DbType.String, item.Channel));
                paramInfoCollection.Add(new SQLiteParamInfo("address", DbType.String, item.Address));
                paramInfoCollection.Add(new SQLiteParamInfo("desc", DbType.String, item.Description));
                paramInfoCollection.Add(new SQLiteParamInfo("minVoltage", DbType.Double, item.MinVoltage));
                paramInfoCollection.Add(new SQLiteParamInfo("maxVoltage", DbType.Double, item.MaxVoltage));
                paramInfoCollection.Add(new SQLiteParamInfo("gradientVoltage", DbType.Double, item.GradientVoltage));
                paramInfoCollection.Add(new SQLiteParamInfo("interceptVoltage", DbType.Double, item.InterceptVoltage));
                paramInfoCollection.Add(new SQLiteParamInfo("minValue", DbType.Double, item.MinValue));
                paramInfoCollection.Add(new SQLiteParamInfo("maxValue", DbType.Double, item.MaxValue));
                paramInfoCollection.Add(new SQLiteParamInfo("gradientValue", DbType.Double, item.GradientValue));
                paramInfoCollection.Add(new SQLiteParamInfo("interceptValue", DbType.Double, item.InterceptValue));

                paramBatchInfo.Add(paramInfoCollection);
            }

            ManagerQueryService.Current.QueryServiceSystem.UpsertChannelSetting(paramBatchInfo);
        }
    }
}
