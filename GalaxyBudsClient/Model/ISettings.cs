using Config.Net;
using GalaxyBudsClient.Model.Constants;

namespace GalaxyBudsClient.Model
{
    public interface ICustomAction
    {
        CustomAction.Actions Action { set; get; }
        [Option(DefaultValue = "")]
        string Parameter { set; get; }
    }
    
    public interface IDevice
    {
        [Option(DefaultValue = Models.NULL)]
        Models Model { set; get; }
        [Option(DefaultValue = "")]
        string MacAddress { set; get; }
    }
    
    public interface IPopup
    {
        bool Enabled { set; get; }
        bool Compact { set; get; }
        [Option(DefaultValue = "")]
        string CustomTitle { set; get; }
        
        [Option(DefaultValue = PopupPlacement.BottomRight)]
        PopupPlacement Placement  { set; get; }
    }
    
    public interface ISettings
    {

        [Option(DefaultValue = DarkModes.Light)]
        DarkModes DarkMode { set; get; }
        [Option(DefaultValue = Locales.en)]
        
        Locales Locale { set; get; }
        [Option(DefaultValue = TemperatureUnits.Celsius)]
        
        TemperatureUnits TemperatureUnit { set; get; }
        IDevice RegisteredDevice { set; get; }
        
        ICustomAction CustomActionLeft { set; get; }
        ICustomAction CustomActionRight { set; get; }

        IPopup Popup { set; get; }
        
        [Option(DefaultValue = "")]
        string UpdateSkippedVersion { set; get; }
        bool MinimizeToTray { set; get; }
        bool ResumePlaybackOnSensor { set; get; }
    }
}