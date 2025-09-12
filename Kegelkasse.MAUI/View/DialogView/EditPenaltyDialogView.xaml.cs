using CommunityToolkit.Maui.Views;
using Kegelkasse.Foundation.Enumerations;

namespace Kegelkasse.MAUI.View.DialogView;

public partial class EditPenaltyDialogView : Popup
{
	public EditPenaltyDialogView()
	{
		InitializeComponent();
	}

    private void AbortButtonView_Tapped(object sender, EventArgs e)
    {
        this.Close(DialogResult.Abort);
    }

    private void SaveButtonView_Tapped(object sender, EventArgs e)
    {
        this.Close(DialogResult.Yes);
    }
}