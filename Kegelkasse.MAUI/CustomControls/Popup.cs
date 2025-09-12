using Kegelkasse.Foundation.Enumerations;

namespace Kegelkasse.MAUI.CustomControls
{
    public class Popup : CommunityToolkit.Maui.Views.Popup
    {
        protected void CloseAbort(object sender, EventArgs e)
        {
            Close(DialogResult.Abort);
        }

        protected void CloseSave(object sender, EventArgs e)
        {
            Close(DialogResult.Yes);
        }
    }
}
