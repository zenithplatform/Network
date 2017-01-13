using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenith.Core.Shared.Helpers;

namespace Zenith.Network.Api.Statistics
{
    //public delegate void OnFileTransferProgressChanged();

    public class FileTransferProgress
    {
        private long _totalFileSize = 0;
        private int _tranferStartedTimeStamp = 0;
        //public event OnFileTransferProgressChanged ProgressChanged;

        public FileTransferProgress(long totalSize)
            :this(totalSize, Environment.TickCount)
        {

        }

        public FileTransferProgress(long totalSize, int transferStartedTimeStamp)
        {
            _totalFileSize = totalSize;
            _tranferStartedTimeStamp = transferStartedTimeStamp;
        }

        public double GetPercentTransferred(long current)
        {
            int ini = Environment.TickCount;
            return (current / _totalFileSize) * 100;
        }

        public long GetTransferRatePerSecond(long current)
        {
            int seconds = (Environment.TickCount - _tranferStartedTimeStamp) / 1000;
            return (current / seconds);
        }

        public string GetPercentTransferredFormatted(long current)
        {
            double percent = GetPercentTransferred(current);
            return string.Format("{0} %", percent);
        }

        public string GetTransferRatePerSecondFormatted(long current)
        {
            long rate = GetTransferRatePerSecond(current);
            return SizeConverter.ConvertToSizeString(rate);
        }
    }
}
