namespace Tattoo.InkVibesTattoo.Dialogs;

public partial class PlacedBetsSummary_Dialog
{
    /// <summary>
    /// Represents a collapsible status of an element.
    /// </summary>
    internal struct CollapsingStatus
    {
        public CollapseStatus CollapseStatus { get; set; }
        public readonly string CollapsingStatusClass => CollapseStatus switch { CollapseStatus.Collapsed => "collapsed", CollapseStatus.Displayed => "", _ => "" };
        public readonly string CollapsingStatusIconClass
            => CollapseStatus switch
            {
                CollapseStatus.Collapsed => "ph ph-caret-down",
                CollapseStatus.Displayed => "ph ph-caret-up",
                _ => ""
            };
    }
}