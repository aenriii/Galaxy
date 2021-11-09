using System;
using System.Net;
using Galaxy.Tcp.Enums;
using Microsoft.Extensions.Logging;

namespace Galaxy.Tcp.Internal
{
    public class IGalaxyTcpOptions
    {
        #region Server Properties

        public IPAddress? Ip { get; set; } // Should default to IPAddress.Any
        public int? Port { get; set; } // No default 
        

        #endregion
        
        #region Client Properties
        
        public EndianSetting? EndianOrientation { get; set; } // Should default to BigEndian
        public int? MaxBufferSize { get; set; } // Should default to 4096
        public int? MaxMessageSize { get; set; } // Should default to 4096
        
        #endregion
        
        #region Customizability
        
        public Type? CustomContext { get; set; } // Should default to null
        public LogLevel? MinimumLogLevel { get; set; } // Should default to LogLevel.Information
        
        #endregion
        
    }
}